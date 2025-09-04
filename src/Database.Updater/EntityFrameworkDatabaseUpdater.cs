//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkDatabaseUpdater.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal sealed class EntityFrameworkDatabaseUpdater
    {
        private readonly IReadOnlyList<string> migrationsAssemblies;

        public EntityFrameworkDatabaseUpdater(IReadOnlyList<string> migrationsAssemblies)
        {
            this.migrationsAssemblies = migrationsAssemblies;
        }

        public async Task<int> UpgradeAsync(string connectionString, int commandTimeout, string accessToken, IHost host, CancellationToken cancellationToken)
        {
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<EntityFrameworkDatabaseUpdater>();

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            connectionStringBuilder.CommandTimeout = commandTimeout;

            using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                connection.AccessToken = accessToken;

                var builder = new DbContextOptionsBuilder<DbContext>();
                builder.UseSqlServer(
                    connection,
                    opt =>
                    {
                        foreach (var assembly in this.migrationsAssemblies)
                        {
                            opt.MigrationsAssembly(assembly);
                        }

                        opt.CommandTimeout(commandTimeout);
                    });

                builder.UseLoggerFactory(loggerFactory);

                using (var context = new DbContext(builder.Options))
                {
                    try
                    {
                        await context.Database.MigrateAsync(cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        logger.LogError(exception, exception.Message);

                        return 99;
                    }
                }
            }

            return 0;
        }
    }
}
