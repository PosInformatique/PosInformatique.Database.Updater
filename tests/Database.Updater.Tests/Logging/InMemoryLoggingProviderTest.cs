//-----------------------------------------------------------------------
// <copyright file="InMemoryLoggingProviderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Database.Updater.Tests
{
    using Microsoft.Extensions.Logging;

    public class InMemoryLoggingProviderTest
    {
        [Fact]
        public void CreateLogger_Log()
        {
            var provider = new InMemoryLoggingProvider();

            var logger1 = provider.CreateLogger("The category 1");
            var logger2 = provider.CreateLogger("The category 2");

            logger1.LogInformation("The information");

            provider.Output.Should().Be("[The category 1] (Information) : The information\r\n");

            logger2.LogError("The error");

            provider.Output.Should().Be("[The category 1] (Information) : The information\r\n[The category 2] (Error) : The error\r\n");
        }

        [Fact]
        public void CreateLogger_BeginScope()
        {
            var provider = new InMemoryLoggingProvider();

            var logger1 = provider.CreateLogger("The category 1");

            logger1.BeginScope(null).Should().BeNull();
        }

        [Theory]
        [InlineData(LogLevel.Critical)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.None)]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Warning)]
        public void CreateLogger_IsEnabled(LogLevel logLevel)
        {
            var provider = new InMemoryLoggingProvider();

            var logger = provider.CreateLogger("The category 1");

            logger.IsEnabled(logLevel).Should().BeTrue();
        }

        [Fact]
        public void Dispose()
        {
            var provider = new InMemoryLoggingProvider();

            var logger = provider.CreateLogger("The category 1");

            logger.LogInformation("The information");

            provider.Dispose();

            provider.Invoking(p => p.CreateLogger(default))
                .Should().ThrowExactly<ObjectDisposedException>()
                .WithMessage("Cannot access a disposed object.\r\nObject name: 'PosInformatique.Database.Updater.InMemoryLoggingProvider'.")
                .Which.ObjectName.Should().Be("PosInformatique.Database.Updater.InMemoryLoggingProvider");

            provider.Invoking(p => p.Output)
                .Should().ThrowExactly<ObjectDisposedException>()
                .WithMessage("Cannot access a disposed object.\r\nObject name: 'PosInformatique.Database.Updater.InMemoryLoggingProvider'.")
                .Which.ObjectName.Should().Be("PosInformatique.Database.Updater.InMemoryLoggingProvider");

            logger.Invoking(l => l.BeginScope(default))
                .Should().ThrowExactly<ObjectDisposedException>()
                .WithMessage("Cannot access a disposed object.\r\nObject name: 'PosInformatique.Database.Updater.InMemoryLoggingProvider'.")
                .Which.ObjectName.Should().Be("PosInformatique.Database.Updater.InMemoryLoggingProvider");

            logger.Invoking(l => l.IsEnabled(default))
                .Should().ThrowExactly<ObjectDisposedException>()
                .WithMessage("Cannot access a disposed object.\r\nObject name: 'PosInformatique.Database.Updater.InMemoryLoggingProvider'.")
                .Which.ObjectName.Should().Be("PosInformatique.Database.Updater.InMemoryLoggingProvider");

            logger.Invoking(l => l.LogInformation("The log"))
                .Should().ThrowExactly<ObjectDisposedException>()
                .WithMessage("Cannot access a disposed object.\r\nObject name: 'PosInformatique.Database.Updater.InMemoryLoggingProvider'.")
                .Which.ObjectName.Should().Be("PosInformatique.Database.Updater.InMemoryLoggingProvider");
        }
    }
}