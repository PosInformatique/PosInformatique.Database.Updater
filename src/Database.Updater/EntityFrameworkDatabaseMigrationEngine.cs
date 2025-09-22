//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkDatabaseMigrationEngine.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    internal sealed class EntityFrameworkDatabaseMigrationEngine : IDatabaseMigrationEngine
    {
        private readonly IDatabaseProvider databaseProvider;

        private readonly ILoggerFactory loggerFactory;

        public EntityFrameworkDatabaseMigrationEngine(IDatabaseProvider databaseProvider, ILoggerFactory loggerFactory)
        {
            this.databaseProvider = databaseProvider;
            this.loggerFactory = loggerFactory;
        }

        public async Task<int> UpgradeAsync(IDatabaseMigrationContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = this.databaseProvider.CreateConnection(context))
            {
                var builder = this.databaseProvider.CreateDbContextOptionsBuilder(connection, context);

                builder.UseLoggerFactory(this.loggerFactory);

                using (var dbContext = new DbContext(builder.Options))
                {
                    await dbContext.Database.MigrateAsync(cancellationToken);
                }
            }

            return 0;
        }
    }
}
