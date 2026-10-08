//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterCommandLineParseResult.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;

    internal sealed class DatabaseUpdaterCommandLineParseResult : IDatabaseUpdaterCommandLine
    {
        public ParseResult? Result { get; set; }

        public T? GetValue<T>(Argument<T> argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            return this.Result!.GetValue(argument);
        }

        public T? GetValue<T>(Option<T> option)
        {
            ArgumentNullException.ThrowIfNull(option);

            return this.Result!.GetValue(option);
        }
    }
}
