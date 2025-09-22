//-----------------------------------------------------------------------
// <copyright file="IDatabaseMigrationContext.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the context of the current database migration performed by the <see cref="IDatabaseMigrationEngine"/>.
    /// </summary>
    internal interface IDatabaseMigrationContext
    {
        /// <summary>
        /// Gets the access token (if need) used to authenticate on database server.
        /// </summary>
        string? AccessToken { get; }

        /// <summary>
        /// Gets the assemblies which contains the Entity Framework Core migrations to execute.
        /// </summary>
        ReadOnlyCollection<string> Assemblies { get; }

        /// <summary>
        /// Gets the timeout allowed to a SQL command to be executed.
        /// </summary>
        int CommandTimeout { get; }

        /// <summary>
        /// Gets the connection string to the database to upgrade.
        /// </summary>
        string ConnectionString { get; }
    }
}
