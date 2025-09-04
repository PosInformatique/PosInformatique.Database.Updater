//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseUpdaterBuilderExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using PosInformatique.Database.Updater.SqlServer;

    /// <summary>
    /// Contains extensions methods for the <see cref="DatabaseUpdaterBuilder"/> class to use SQL Server database provider.
    /// </summary>
    public static class SqlServerDatabaseUpdaterBuilderExtensions
    {
        /// <summary>
        /// Configures the <see cref="DatabaseUpdaterBuilder"/> to use SQL Server database provider.
        /// </summary>
        /// <param name="builder"><see cref="DatabaseUpdaterBuilder"/> to configure.</param>
        /// <returns>The <paramref name="builder"/> instance to continue the configuration.</returns>
        public static DatabaseUpdaterBuilder UseSqlServer(this DatabaseUpdaterBuilder builder)
        {
            return builder.UseDatabaseProvider(new SqlServerDatabaseProvider());
        }
    }
}
