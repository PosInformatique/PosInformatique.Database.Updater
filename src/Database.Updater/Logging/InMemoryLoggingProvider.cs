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
        private TextWriter? output;

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
        public string Output
        {
            get
            {
                ObjectDisposedException.ThrowIf(this.output == null, this);

                return this.output.ToString()!;
            }
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            ObjectDisposedException.ThrowIf(this.output == null, this);

            return new StringDumpLogger(this, categoryName);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.output is not null)
            {
                this.output.Dispose();
                this.output = null;
            }
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
            {
                ObjectDisposedException.ThrowIf(this.provider.output == null, this.provider);

                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                ObjectDisposedException.ThrowIf(this.provider.output == null, this.provider);

                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                ObjectDisposedException.ThrowIf(this.provider.output == null, this.provider);

                this.provider.output.WriteLine($"[{this.categoryName}] ({logLevel}) : {formatter(state, exception)}");
            }
        }
    }
}
