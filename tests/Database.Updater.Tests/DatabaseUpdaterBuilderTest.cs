//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterBuilderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    using System.Reflection;

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

        [Fact]
        public void ConfigureCommandLine_WithNullArgument()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.ConfigureCommandLine(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("commandLine")
                .WithMessage("Value cannot be null. (Parameter 'commandLine')");
        }

        [Fact]
        public void ConfigureServices_WithNullArgument()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.ConfigureServices(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("configureServices")
                .WithMessage("Value cannot be null. (Parameter 'configureServices')");
        }

        [Fact]
        public void Configure_WithNullArgument()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.Configure(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options")
                .WithMessage("Value cannot be null. (Parameter 'options')");
        }

        [Fact]
        public void ConfigureLogging_WithNullArgument()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.ConfigureLogging(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("builder")
                .WithMessage("Value cannot be null. (Parameter 'builder')");
        }

        [Fact]
        public void UseMigrationsAssembly_Assembly_WithNullArgument()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.UseMigrationsAssembly((Assembly)null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("assembly")
                .WithMessage("Value cannot be null. (Parameter 'assembly')");
        }

        [Fact]
        public void UseMigrationsAssembly_String_WithNullArgument()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.UseMigrationsAssembly((string)null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("assembly")
                .WithMessage("Value cannot be null. (Parameter 'assembly')");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        public void UseMigrationsAssembly_String_EmptyOrWhitespace(string assembly)
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.Invoking(b => b.UseMigrationsAssembly(assembly))
                .Should().ThrowExactly<ArgumentException>()
                .WithParameterName("assembly")
                .WithMessage("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'assembly')");
        }
    }
}