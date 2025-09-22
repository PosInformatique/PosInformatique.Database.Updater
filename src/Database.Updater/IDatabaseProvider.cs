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
        /// <param name="migrationContext">Migration database context.</param>
        /// <returns>The <see cref="DbConnection"/> which allows to connect to the database.</returns>
        DbConnection CreateConnection(IDatabaseMigrationContext migrationContext);

        /// <summary>
        /// Creates an instance of the <see cref="DbContextOptionsBuilder"/> to create a <see cref="DbContext"/>
        /// which will be used for the Entity Framework migrations.
        /// </summary>
        /// <param name="connection"><see cref="DbConnection"/> to the database.</param>
        /// <param name="migrationContext">Migration database context.</param>
        /// <returns>An instance of the <see cref="DbContextOptionsBuilder"/> to create a <see cref="DbContext"/>
        /// which will be used for the Entity Framework migrations.</returns>
        DbContextOptionsBuilder CreateDbContextOptionsBuilder(DbConnection connection, IDatabaseMigrationContext migrationContext);

        /// <summary>
        /// Validates the specified connection string.
        /// </summary>
        /// <param name="connectionString">Connection string to validate.</param>
        /// <param name="argumentName">Command line argument which contains the connection string to validate.</param>
        /// <returns>An error message if the <paramref name="connectionString"/> is invalid. <see langword="null"/> in otherwise.</returns>
        string? ValidateConnectionString(string connectionString, string argumentName);
    }
}
