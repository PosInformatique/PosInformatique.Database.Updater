//-----------------------------------------------------------------------
// <copyright file="IDatabaseMigrationEngine.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    /// <summary>
    /// Represents the engine which will perform the migration of the database.
    /// </summary>
    internal interface IDatabaseMigrationEngine
    {
        /// <summary>
        /// Performs the migration to the database.
        /// </summary>
        /// <param name="context">Migration database context.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> which allows to cancel the migration.</param>
        /// <returns>A <see cref="Task{Int32}"/> instance which represents the asynchronous operation and contains the exit code of the application.</returns>
        Task<int> UpgradeAsync(IDatabaseMigrationContext context, CancellationToken cancellationToken = default);
    }
}
