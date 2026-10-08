//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkDatabaseMigrationEngine.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Logging;

    internal sealed class EntityFrameworkDatabaseMigrationEngine : IDatabaseMigrationEngine
    {
        private readonly IDatabaseProvider databaseProvider;

        private readonly IServiceProvider serviceProvider;

        private readonly ILoggerFactory loggerFactory;

        public EntityFrameworkDatabaseMigrationEngine(IDatabaseProvider databaseProvider, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            this.databaseProvider = databaseProvider;
            this.serviceProvider = serviceProvider;
            this.loggerFactory = loggerFactory;
        }

        public async Task<int> UpgradeAsync(IDatabaseMigrationContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = this.databaseProvider.CreateConnection(context))
            {
                var builder = this.databaseProvider.CreateDbContextOptionsBuilder(connection, context);

                builder.UseLoggerFactory(this.loggerFactory);
                builder.ReplaceService<IMigrationsAssembly, DependencyInjectionMigrationsAssembly>();

                var extension = new DatabaseUpdaterExtension(this.serviceProvider);
                ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);

                using (var dbContext = new DbContext(builder.Options))
                {
                    await dbContext.Database.MigrateAsync(cancellationToken);
                }
            }

            return 0;
        }
    }
}
