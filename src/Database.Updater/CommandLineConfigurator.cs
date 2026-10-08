//-----------------------------------------------------------------------
// <copyright file="CommandLineConfigurator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;

    internal sealed class CommandLineConfigurator : ICommandLineConfiguration
    {
        private readonly RootCommand commandLine;

        public CommandLineConfigurator(RootCommand commandLine)
        {
            this.commandLine = commandLine;
        }

        public void AddArgument(Argument argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            this.commandLine.Arguments.Add(argument);
        }

        public void AddOption(Option option)
        {
            ArgumentNullException.ThrowIfNull(option);

            this.commandLine.Options.Add(option);
        }
    }
}
