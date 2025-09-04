//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.SqlServer
{
    using System.Data.Common;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    internal sealed class SqlServerDatabaseProvider : IDatabaseProvider
    {
        public SqlServerDatabaseProvider()
        {
        }

        public DbConnection CreateConnection(string connectionString, int commandTimeout, string? accessToken)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            connectionStringBuilder.CommandTimeout = commandTimeout;

            return new SqlConnection(connectionStringBuilder.ToString())
            {
                AccessToken = accessToken,
            };
        }

        public DbContextOptionsBuilder CreateDbContextOptionsBuilder(DbConnection connection, IReadOnlyList<string> migrationsAssemblies, int commandTimeout)
        {
            return new DbContextOptionsBuilder().UseSqlServer(
                connection,
                opt =>
                {
                    foreach (var assembly in migrationsAssemblies)
                    {
                        opt.MigrationsAssembly(assembly);
                    }

                    opt.CommandTimeout(commandTimeout);
                });
        }
    }
}
