//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkDatabaseUpdater.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.Runtime.ExceptionServices;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal sealed class EntityFrameworkDatabaseUpdater
    {
        private readonly IDatabaseProvider databaseProvider;

        private readonly IReadOnlyList<string> migrationsAssemblies;

        public EntityFrameworkDatabaseUpdater(IDatabaseProvider databaseProvider, IReadOnlyList<string> migrationsAssemblies)
        {
            this.databaseProvider = databaseProvider;
            this.migrationsAssemblies = migrationsAssemblies;
        }

        public ExceptionDispatchInfo? CapturedException { get; private set; }

        public async Task<int> UpgradeAsync(string connectionString, int commandTimeout, string? accessToken, IHost host, CancellationToken cancellationToken)
        {
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<EntityFrameworkDatabaseUpdater>();

            using (var connection = this.databaseProvider.CreateConnection(connectionString, commandTimeout, accessToken))
            {
                var builder = this.databaseProvider.CreateDbContextOptionsBuilder(
                    connection,
                    this.migrationsAssemblies,
                    commandTimeout);

                builder.UseLoggerFactory(loggerFactory);

                using (var context = new DbContext(builder.Options))
                {
                    try
                    {
                        await context.Database.MigrateAsync(cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        this.CapturedException = ExceptionDispatchInfo.Capture(exception);

                        logger.LogError(exception, exception.Message);

                        return 99;
                    }
                }
            }

            return 0;
        }
    }
}
