//-----------------------------------------------------------------------
// <copyright file="UpdaterTestArguments.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;

    public class UpdaterTestArguments
    {
        public static Argument<string> Argument1 { get; } = new Argument<string>("argument1")
        {
            Description = "The argument 1",
        };

        public static Option<string> Options1 { get; } = new Option<string>("--option1")
        {
            Description = "The option 1",
            Required = true,
        };

        public static Option<int> Options2 { get; } = new Option<int>("--option2")
        {
            Description = "The option 2",
            Required = false,
        };

        public static Option<int> Options3 { get; } = new Option<int>("--option3")
        {
            Description = "The option 3",
            DefaultValueFactory = _ => 9999,
            Required = false,
        };
    }
}
