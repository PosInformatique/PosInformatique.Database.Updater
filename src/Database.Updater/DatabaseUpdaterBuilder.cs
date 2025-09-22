//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterBuilder.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PosInformatique.Database.Updater.SqlServer;

    /// <summary>
    /// Allows to setup builder which will create <see cref="IDatabaseUpdater"/> instance
    /// to perform the migration of the database.
    /// <para>
    /// By default the <see cref="DatabaseUpdaterBuilder"/> will configure and create <see cref="IDatabaseUpdater"/> instances
    /// with the following behavior:
    ///     <list type="bullet">
    ///         <item>It will use the calling assembly as the assembly which contains the <see cref="Migration"/> to execute if the <see cref="UseMigrationsAssembly(string)"/> methods is not used.</item>
    ///         <item>It will use SQL Server provider for the migration.</item>
    ///         <item></item>
    ///     </list>
    /// </para>
    /// </summary>
    public sealed class DatabaseUpdaterBuilder
    {
        private static readonly int DefaultCommandTimeout = new SqlConnectionStringBuilder().CommandTimeout;

        private readonly string applicationName;

        private readonly Assembly callingAssembly;

        private readonly List<string> migrationsAssemblies;

        private readonly IHostBuilder hostBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUpdaterBuilder"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application which the database will be updated for.</param>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="applicationName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the specified <paramref name="applicationName"/> argument is empty or contains only white spaces.</exception>
        public DatabaseUpdaterBuilder(string applicationName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);

            this.callingAssembly = Assembly.GetCallingAssembly();

            this.applicationName = applicationName;
            this.migrationsAssemblies = new List<string>();

            this.hostBuilder = Host.CreateDefaultBuilder();
            this.hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IDatabaseMigrationEngine, EntityFrameworkDatabaseMigrationEngine>();
            });
        }

        /// <summary>
        /// Configures the options of the database upgrade process.
        /// </summary>
        /// <param name="options">Callback which allows to configure the options of the database upgrade process.</param>
        /// <returns>The current <see cref="DatabaseUpdaterBuilder"/> instance to continue the configuration.</returns>
        public DatabaseUpdaterBuilder Configure(Action<DatabaseUpdaterOptions> options)
        {
            this.hostBuilder.ConfigureServices(services =>
            {
                services.Configure(options);
            });

            return this;
        }

        /// <summary>
        /// Configures the logging for the upgrade database operation.
        /// Use the <see cref="InMemoryLoggingProvider"/> to capture the logs in memory.
        /// </summary>
        /// <param name="builder"><see cref="ILoggingBuilder"/> which allows to configure the logging.</param>
        /// <returns>The current <see cref="DatabaseUpdaterBuilder"/> instance to continue the configuration.</returns>
        public DatabaseUpdaterBuilder ConfigureLogging(Action<ILoggingBuilder> builder)
        {
            this.hostBuilder.ConfigureLogging(builder);

            return this;
        }

        /// <summary>
        /// Use a specific Entity Framework Core assembly which contains the <see cref="Migration"/> to execute.
        /// </summary>
        /// <param name="assembly">Name of the assembly which contains the <see cref="Migration"/> to execute.</param>
        /// <returns>The current <see cref="DatabaseUpdaterBuilder"/> instance to continue the configuration.</returns>
        public DatabaseUpdaterBuilder UseMigrationsAssembly(string assembly)
        {
            this.migrationsAssemblies.Add(assembly);

            return this;
        }

        /// <summary>
        /// Use a specific Entity Framework Core assembly which contains the <see cref="Migration"/> to execute.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> which contains the <see cref="Migration"/> to execute.</param>
        /// <returns>The current <see cref="DatabaseUpdaterBuilder"/> instance to continue the configuration.</returns>
        public DatabaseUpdaterBuilder UseMigrationsAssembly(Assembly assembly)
        {
            return this.UseMigrationsAssembly(assembly.GetName().Name!);
        }

        /// <summary>
        /// Builds an instance of the <see cref="IDatabaseUpdater"/> to perform the migration of the database.
        /// </summary>
        /// <returns>An instance of the <see cref="IDatabaseUpdater"/> to perform the migration of the database.</returns>
        /// <exception cref="InvalidOperationException">No database provider has been configured.</exception>
        public IDatabaseUpdater Build()
        {
            return new CommandLineDatabaseUpdater(this);
        }

        internal DatabaseUpdaterBuilder UseDatabaseProvider<TDatabaseProvider>()
            where TDatabaseProvider : class, IDatabaseProvider
        {
            this.hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IDatabaseProvider, TDatabaseProvider>();
            });

            return this;
        }

        private sealed class CommandLineDatabaseUpdater : IDatabaseUpdater
        {
            private readonly DatabaseUpdaterBuilder builder;

            private readonly RootCommand commandLine;

            private readonly DatabaseConnectionStringArgument connectionStringArgument;

            private readonly Option<string> accessTokenOption;

            private readonly Option<int> commandTimeoutOption;

            private IHost? host;

            public CommandLineDatabaseUpdater(DatabaseUpdaterBuilder builder)
            {
                this.builder = builder;
                this.host = builder.hostBuilder.Build();

                var databaseProvider = this.host.Services.GetService<IDatabaseProvider>();

                if (databaseProvider == null)
                {
                    throw new InvalidOperationException("No database provider has been configured.");
                }

                this.commandLine = new RootCommand($"Upgrade the {this.builder.applicationName} database.");

                // Connection string argument
                this.connectionStringArgument = new DatabaseConnectionStringArgument(databaseProvider, "connection-string")
                {
                    Description = "The connection string to the database to upgrade",
                };

                this.commandLine.Add(this.connectionStringArgument);

                // Access token option "--access-token"
                this.accessTokenOption = new Option<string>("--access-token")
                {
                    Description = "Access token to connect to the SQL database.",
                    Required = false,
                };

                this.commandLine.Options.Add(this.accessTokenOption);

                // Command timeout option "--command-timeout"
                this.commandTimeoutOption = new Option<int>("--command-timeout")
                {
                    DefaultValueFactory = _ => DefaultCommandTimeout,
                    Description = "Maximum time in seconds to execute each SQL statements.",
                    Required = false,
                };

                this.commandLine.Options.Add(this.commandTimeoutOption);

                this.commandLine.SetAction(this.ExecuteMigrationAsync);
            }

            public void Dispose()
            {
                if (this.host is not null)
                {
                    this.host.Dispose();
                    this.host = null;
                }
            }

            public async Task<int> UpgradeAsync(IReadOnlyList<string> args, CancellationToken cancellationToken = default)
            {
                var parseResult = this.commandLine.Parse(args);

                var invocationConfiguration = new InvocationConfiguration() { EnableDefaultExceptionHandler = false };

                return await parseResult.InvokeAsync(invocationConfiguration, cancellationToken: cancellationToken);
            }

            private async Task<int> ExecuteMigrationAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
            {
                ObjectDisposedException.ThrowIf(this.host is null, this);

                var logger = this.host.Services.GetRequiredService<ILogger<IDatabaseUpdater>>();

                try
                {
                    await this.host.StartAsync(cancellationToken);

                    // Gets the migration assembly and add the current assembly if not specified.
                    var migrationsAssemblies = this.builder.migrationsAssemblies.ToList();

                    if (migrationsAssemblies.Count == 0)
                    {
                        migrationsAssemblies.Add(this.builder.callingAssembly.GetName().Name!);
                    }

                    var context = new DatabaseMigrationContext(
                        parseResult.GetRequiredValue(this.connectionStringArgument),
                        migrationsAssemblies)
                    {
                        AccessToken = parseResult.GetValue(this.accessTokenOption),
                        CommandTimeout = parseResult.GetValue(this.commandTimeoutOption),
                    };

                    var migrationEngine = this.host.Services.GetRequiredService<IDatabaseMigrationEngine>();

                    return await migrationEngine.UpgradeAsync(context, cancellationToken);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, exception.Message);

                    var options = this.host.Services.GetRequiredService<IOptions<DatabaseUpdaterOptions>>();

                    if (options.Value.ThrowExceptionOnError)
                    {
                        throw;
                    }

                    return 99;
                }
                finally
                {
                    await this.host.StopAsync(cancellationToken);
                }
            }
        }
    }
}
