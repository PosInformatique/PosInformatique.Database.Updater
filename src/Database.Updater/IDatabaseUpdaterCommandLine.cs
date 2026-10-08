//-----------------------------------------------------------------------
// <copyright file="IDatabaseUpdaterCommandLine.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;

    /// <summary>
    /// Interface for a command line that can retrieve values for arguments and options.
    /// </summary>
    public interface IDatabaseUpdaterCommandLine
    {
        /// <summary>
        /// Gets the value of the specified <paramref name="argument"/>.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument to get the value for.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> is <see langword="null"/>.</exception>
        T? GetValue<T>(Argument<T> argument);

        /// <summary>
        /// Gets the value of the specified <paramref name="option"/>.
        /// </summary>
        /// <typeparam name="T">The type of the option.</typeparam>
        /// <param name="option">The option to get the value for.</param>
        /// <returns>The value of the option.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="option"/> is <see langword="null"/>.</exception>
        T? GetValue<T>(Option<T> option);
    }
}
