//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    /// <summary>
    /// Options for the <see cref="IDatabaseUpdater"/>.
    /// </summary>
    public class DatabaseUpdaterOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether an exception should be thrown if an error occurs during the migration
        /// instead to return an error code.
        /// </summary>
        public bool ThrowExceptionOnError { get; set; }
    }
}
