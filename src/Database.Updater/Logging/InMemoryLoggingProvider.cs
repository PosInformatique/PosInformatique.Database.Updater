//-----------------------------------------------------------------------
// <copyright file="InMemoryLoggingProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementation of the <see cref="ILoggerProvider"/> which stores the log messages in memory.
    /// The logs are accessible through the <see cref="Output"/> property.
    /// </summary>
    public class InMemoryLoggingProvider : ILoggerProvider
    {
        private readonly TextWriter output;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryLoggingProvider"/> class.
        /// </summary>
        public InMemoryLoggingProvider()
        {
            this.output = new StringWriter();
        }

        /// <summary>
        /// Gets the logs output.
        /// </summary>
        public string Output => this.output.ToString()!;

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new StringDumpLogger(this, categoryName);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.output.Dispose();
        }

        private sealed class StringDumpLogger : ILogger
        {
            private readonly InMemoryLoggingProvider provider;

            private readonly string categoryName;

            public StringDumpLogger(InMemoryLoggingProvider provider, string categoryName)
            {
                this.provider = provider;
                this.categoryName = categoryName;
            }

            public IDisposable? BeginScope<TState>(TState state)
                where TState : notnull
                    => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                this.provider.output.WriteLine($"[{this.categoryName}] ({logLevel}) : {formatter(state, exception)}");
            }
        }
    }
}
