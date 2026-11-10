//-----------------------------------------------------------------------
// <copyright file="ICommandLineConfiguration.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;

    /// <summary>
    /// Defines the interface for configuring command line arguments and options.
    /// </summary>
    public interface ICommandLineConfiguration
    {
        /// <summary>
        /// Adds an argument to the command line configuration.
        /// </summary>
        /// <param name="argument">The argument to add to the command line configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> is <see langword="null"/>.</exception>
        void AddArgument(Argument argument);

        /// <summary>
        /// Adds an option to the command line configuration.
        /// </summary>
        /// <param name="option">The option to add to the command line configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="option"/> is <see langword="null"/>.</exception>
        void AddOption(Option option);
    }
}
