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

        public DbConnection CreateConnection(IDatabaseMigrationContext migrationContext)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(migrationContext.ConnectionString);
            connectionStringBuilder.CommandTimeout = migrationContext.CommandTimeout;

            return new SqlConnection(connectionStringBuilder.ToString())
            {
                AccessToken = migrationContext.AccessToken,
            };
        }

        public DbContextOptionsBuilder CreateDbContextOptionsBuilder(DbConnection connection, IDatabaseMigrationContext migrationContext)
        {
            return new DbContextOptionsBuilder().UseSqlServer(
                connection,
                opt =>
                {
                    foreach (var assembly in migrationContext.Assemblies)
                    {
                        opt.MigrationsAssembly(assembly);
                    }

                    opt.CommandTimeout(migrationContext.CommandTimeout);
                });
        }

        public string? ValidateConnectionString(string connectionString, string argumentName)
        {
            try
            {
#pragma warning disable S1848 // Objects should not be created to be dropped immediately without being used
#pragma warning disable CA1806 // Do not ignore method results
                new SqlConnectionStringBuilder(connectionString);
#pragma warning restore CA1806 // Do not ignore method results
#pragma warning restore S1848 // Objects should not be created to be dropped immediately without being used
            }
            catch (ArgumentException)
            {
                return $"The SQL Server connection string specified in the '{argumentName}' argument is invalid.";
            }

            return null;
        }
    }
}
