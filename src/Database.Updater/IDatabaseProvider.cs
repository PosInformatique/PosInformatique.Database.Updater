//-----------------------------------------------------------------------
// <copyright file="IDatabaseProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.Data.Common;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Represents a database provider.
    /// </summary>
    internal interface IDatabaseProvider
    {
        /// <summary>
        /// Creates a <see cref="DbConnection"/> to the database.
        /// </summary>
        /// <param name="connectionString">Connection string to the database.</param>
        /// <param name="commandTimeout">Timeout for the command execution.</param>
        /// <param name="accessToken">Access token for authentication if need.</param>
        /// <returns>The <see cref="DbConnection"/> which allows to connect to the database.</returns>
        DbConnection CreateConnection(string connectionString, int commandTimeout, string? accessToken);

        /// <summary>
        /// Creates an instance of the <see cref="DbContextOptionsBuilder"/> to create a <see cref="DbContext"/>
        /// which will be used for the Entity Framework migrations.
        /// </summary>
        /// <param name="connection"><see cref="DbConnection"/> to the database.</param>
        /// <param name="migrationsAssemblies">List of the assemblies that contains the migrations to execute.</param>
        /// <param name="commandTimeout">Timeout for the command execution.</param>
        /// <returns>An instance of the <see cref="DbContextOptionsBuilder"/> to create a <see cref="DbContext"/>
        /// which will be used for the Entity Framework migrations.</returns>
        DbContextOptionsBuilder CreateDbContextOptionsBuilder(DbConnection connection, IReadOnlyList<string> migrationsAssemblies, int commandTimeout);
    }
}
