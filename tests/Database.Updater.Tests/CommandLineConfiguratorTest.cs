//-----------------------------------------------------------------------
// <copyright file="CommandLineConfiguratorTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    public class CommandLineConfiguratorTest
    {
        [Fact]
        public void AddArgument_WithNullArgument_ThrowsArgumentNullException()
        {
            var configurator = new CommandLineConfigurator(default);

            configurator.Invoking(c => c.AddArgument(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("argument")
                .WithMessage("Value cannot be null. (Parameter 'argument')");
        }

        [Fact]
        public void AddOption_WithNullOption_ThrowsArgumentNullException()
        {
            var configurator = new CommandLineConfigurator(default);

            configurator.Invoking(c => c.AddOption(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("option")
                .WithMessage("Value cannot be null. (Parameter 'option')");
        }
    }
}
