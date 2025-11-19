//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterBuilderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    public class DatabaseUpdaterBuilderTest
    {
        [Fact]
        public void Constructor_NoApplicationName()
        {
            var action = () => new DatabaseUpdaterBuilder(null);

            action.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("applicationName")
                .WithMessage("Value cannot be null. (Parameter 'applicationName')");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        public void Constructor_ApplicationName_EmptyOrWhitespace(string applicationName)
        {
            var action = () => new DatabaseUpdaterBuilder(applicationName);

            action.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("applicationName")
                .WithMessage("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'applicationName')");
        }

        [Fact]
        public void Build_NoDatabaseProvider()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.Build())
                .Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("No database provider has been configured.");
        }
    }
}