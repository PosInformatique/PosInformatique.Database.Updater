//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterCommandLineParseResultTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    using System.CommandLine;

    public class DatabaseUpdaterCommandLineParseResultTest
    {
        [Fact]
        public void GetValue_WithNullArgument_ThrowsArgumentNullException()
        {
            var configurator = new DatabaseUpdaterCommandLineParseResult();

            configurator.Invoking(c => c.GetValue((Argument<int>)null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("argument")
                .WithMessage("Value cannot be null. (Parameter 'argument')");
        }

        [Fact]
        public void GetValue_WithNullOption_ThrowsArgumentNullException()
        {
            var configurator = new DatabaseUpdaterCommandLineParseResult();

            configurator.Invoking(c => c.GetValue((Option<int>)null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("option")
                .WithMessage("Value cannot be null. (Parameter 'option')");
        }
    }
}
