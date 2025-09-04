//-----------------------------------------------------------------------
// <copyright file="IDatabaseUpdater.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    /// <summary>
    /// Allows to perform the migration of the database.
    /// </summary>
    public interface IDatabaseUpdater
    {
        /// <summary>
        /// Performs the migration of the database.
        /// </summary>
        /// <param name="args">Command line argument of the migration tool.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> which allows to cancel the asynchronous operation.</param>
        /// <returns>An instance of <see cref="Task{TResult}"/> which represents the asynchronous operation.</returns>
        Task<int> UpgradeAsync(IReadOnlyList<string> args, CancellationToken cancellationToken = default);
    }
}
