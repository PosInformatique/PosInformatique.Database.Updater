//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterBuilder.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;
    using System.CommandLine.Hosting;
    using System.CommandLine.NamingConventionBinder;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
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

        private readonly IList<string> migrationsAssemblies;

        private readonly IList<Action<ILoggingBuilder>> loggerBuilders;

        private IDatabaseProvider? databaseProvider;

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
            this.loggerBuilders = new List<Action<ILoggingBuilder>>();
        }

        /// <summary>
        /// Configures the logging for the upgrade database operation.
        /// Use the <see cref="InMemoryLoggingProvider"/> to capture the logs in memory.
        /// </summary>
        /// <param name="builder"><see cref="ILoggingBuilder"/> which allows to configure the logging.</param>
        /// <returns>The current <see cref="DatabaseUpdaterBuilder"/> instance to continue the configuration.</returns>
        public DatabaseUpdaterBuilder ConfigureLogging(Action<ILoggingBuilder> builder)
        {
            this.loggerBuilders.Add(builder);

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
            if (this.databaseProvider is null)
            {
                throw new InvalidOperationException("No database provider has been configured.");
            }

            var rootCommand = new RootCommand($"Upgrade the {this.applicationName} database.")
            {
                new SqlServerConnectionStringArgument(this.databaseProvider, "connection-string")
                {
                    Description = "The connection string to the database to upgrade",
                },
            };

            rootCommand.Options.Add(new Option<string>("--access-token")
            {
                Description = "Access token to connect to the SQL database.",
                Required = false,
            });

            rootCommand.Options.Add(new Option<int>("--command-timeout")
            {
                DefaultValueFactory = _ => DefaultCommandTimeout,
                Description = "Maximum time in seconds to execute each SQL statements.",
                Required = false,
            });

            // Gets the migration assembly and add the current assembly if not specified.
            var migrationsAssemblies = this.migrationsAssemblies.ToList();

            if (migrationsAssemblies.Count == 0)
            {
                migrationsAssemblies.Add(this.callingAssembly.GetName().Name!);
            }

            var databaseUpdater = new EntityFrameworkDatabaseUpdater(this.databaseProvider, migrationsAssemblies);

            rootCommand.Action = CommandHandler.Create<string, int, string, IHost, CancellationToken>(databaseUpdater.UpgradeAsync);

            var commandLine = new CommandLineConfiguration(rootCommand)
                .UseHost(
                    _ => Host.CreateDefaultBuilder(),
                    hostBuilder =>
                    {
                        foreach (var loggerBuilder in this.loggerBuilders)
                        {
                            hostBuilder.ConfigureLogging(loggerBuilder);
                        }

                        hostBuilder.ConfigureServices(services =>
                        {
                        });
                    });

            return new CommandLineDatabaseUpdater(commandLine);
        }

        internal DatabaseUpdaterBuilder UseDatabaseProvider(IDatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider;

            return this;
        }

        private sealed class CommandLineDatabaseUpdater : IDatabaseUpdater
        {
            private readonly CommandLineConfiguration commandLine;

            public CommandLineDatabaseUpdater(CommandLineConfiguration commandLine)
            {
                this.commandLine = commandLine;
            }

            public async Task<int> UpgradeAsync(IReadOnlyList<string> args, CancellationToken cancellationToken = default)
            {
                var parseResult = this.commandLine.Parse(args);

                return await parseResult.InvokeAsync(cancellationToken);
            }
        }
    }
}
