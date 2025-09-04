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
