//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    using System.CommandLine;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PosInformatique.Testing.Databases.SqlServer;

    [Collection(nameof(DatabaseUpdaterTest))]
    public class DatabaseUpdaterTest
    {
        [Fact]
        public async Task UpgradeAsync_WithExplicitMigrationsAssembly()
        {
            var server = new SqlServer(ConnectionStrings.Default);

            var database = await server.CreateEmptyDatabaseAsync("DatabaseUpdaterTest_UpgradeAsync_WithExplicitMigrationsAssembly", cancellationToken: TestContext.Current.CancellationToken);

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication")
                .Configure(c =>
                {
                    c.ThrowExceptionOnError = true;
                })
                .ConfigureCommandLine(commandLine =>
                {
                    commandLine.AddArgument(UpdaterTestArguments.Argument1);
                    commandLine.AddOption(UpdaterTestArguments.Options1);
                })
                .ConfigureCommandLine(commandLine =>
                {
                    commandLine.AddOption(UpdaterTestArguments.Options2);
                    commandLine.AddOption(UpdaterTestArguments.Options3);
                })
                .ConfigureServices(s =>
                {
                    s.AddSingleton<ISingletonService, SingletonService>();
                })
                .ConfigureServices(s =>
                {
                    s.AddTransient<ITransientService, TransientService>();
                })
                .UseSqlServer()
                .UseMigrationsAssembly(typeof(MigrationsAssembly.Version1).Assembly);

            using var databaseUpdater = databaseUpdaterBuilder
                .Build();

            var args = new[]
            {
                database.ConnectionString,
                "Argument1Value",
                "--option1=Option1Value",
                "--option2=123",
            };

            var result = await databaseUpdater.UpgradeAsync(args, TestContext.Current.CancellationToken);

            result.Should().Be(0);

            var tables = await database.GetTablesAsync(TestContext.Current.CancellationToken);

            tables.Should().HaveCount(2);

            tables[0].Name.Should().Be("__EFMigrationsHistory");

            tables[1].Columns.Should().HaveCount(3);
            tables[1].Columns[0].Name.Should().Be("Id");
            tables[1].Columns[1].Name.Should().Be("Name");
            tables[1].Columns[2].Name.Should().Be("IsActive");
            tables[1].Name.Should().Be("Person");
        }

        [Fact]
        public async Task UpgradeAsync_WithErrorMigrationsAssembly()
        {
            using var output = new StringWriter();
            Console.SetOut(output);

            var server = new SqlServer(ConnectionStrings.Default);

            var database = await server.CreateEmptyDatabaseAsync("DatabaseUpdaterTest_UpgradeAsync_WithErrorMigrationsAssembly", cancellationToken: TestContext.Current.CancellationToken);

            var loggingProvider = new InMemoryLoggingProvider();

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication")
                .ConfigureLogging(l =>
                {
                    l.AddProvider(loggingProvider)
                        .SetMinimumLevel(LogLevel.Error);
                })
                .UseSqlServer()
                .UseMigrationsAssembly(typeof(MigrationsErrorAssembly.Version1).Assembly);

            using var databaseUpdater = databaseUpdaterBuilder
                .Build();

            var result = await databaseUpdater.UpgradeAsync([database.ConnectionString], TestContext.Current.CancellationToken);

            result.Should().Be(99);

            loggingProvider.Output.Should().Be($"[PosInformatique.Database.Updater.IDatabaseUpdater] (Error) : Some errors occured during the migration...{Environment.NewLine}");

            output.ToString().Should().StartWith("fail: PosInformatique.Database.Updater.IDatabaseUpdater[0]");
        }

        [Fact]
        public async Task UpgradeAsync_WithThrowException()
        {
            using var output = new StringWriter();
            Console.SetOut(output);

            var server = new SqlServer(ConnectionStrings.Default);

            var database = await server.CreateEmptyDatabaseAsync("DatabaseUpdaterTest_UpgradeAsync_WithErrorMigrationsAssembly", cancellationToken: TestContext.Current.CancellationToken);

            var loggingProvider = new InMemoryLoggingProvider();

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication")
                .Configure(opt =>
                {
                    opt.ThrowExceptionOnError = false;
                })
                .Configure(opt =>
                {
                    opt.ThrowExceptionOnError = true;
                })
                .ConfigureLogging(l =>
                {
                    l.AddProvider(loggingProvider)
                        .SetMinimumLevel(LogLevel.Error);
                })
                .UseSqlServer()
                .UseMigrationsAssembly(typeof(MigrationsErrorAssembly.Version1).Assembly);

            using var databaseUpdater = databaseUpdaterBuilder
                .Build();

            await databaseUpdater.Invoking(du => du.UpgradeAsync([database.ConnectionString]))
                .Should().ThrowExactlyAsync<DivideByZeroException>()
                .WithMessage("Some errors occured during the migration...");

            loggingProvider.Output.Should().Be($"[PosInformatique.Database.Updater.IDatabaseUpdater] (Error) : Some errors occured during the migration...{Environment.NewLine}");

            output.ToString().Should().StartWith("fail: PosInformatique.Database.Updater.IDatabaseUpdater[0]");
        }

        [Fact]
        public async Task UpgradeAsync_NoArguments()
        {
            using var output = new StringWriter();
            Console.SetOut(output);

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication");

            using var databaseUpdater = databaseUpdaterBuilder
                .ConfigureCommandLine(commandLine =>
                {
                    commandLine.AddOption(new Option<string>("option1")
                    {
                        Description = "The option 1",
                        Required = false,
                    });
                })
                .ConfigureCommandLine(commandLine =>
                {
                    commandLine.AddOption(new Option<string>("option2")
                    {
                        Description = "The option 2",
                    });
                })
                .UseSqlServer()
                .Build();

            var result = await databaseUpdater.UpgradeAsync([], TestContext.Current.CancellationToken);

            result.Should().Be(1);

            output.ToString().Should().Be(
                """
                Description:
                  Upgrade the MyApplication database.

                Usage:
                  PosInformatique.Database.Updater.Tests <connection-string> [options]

                Arguments:
                  <connection-string>  The connection string to the database to upgrade

                Options:
                  --access-token <access-token>        Access token to connect to the SQL database.
                  --command-timeout <command-timeout>  Maximum time in seconds to execute each SQL statements. [default: 30]
                  option1 <option1>                    The option 1
                  option2 <option2>                    The option 2
                  -?, -h, --help                       Show help and usage information
                  --version                            Show version information

                
                """);
        }

        [Theory]
        [InlineData("NotConnectionString")]
        [InlineData("Data Source=some_server;", "--command-timeout=abcd")]
        public async Task UpgradeAsync_WrongArguments(params string[] args)
        {
            using var output = new StringWriter();
            Console.SetOut(output);

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication");

            using var databaseUpdater = databaseUpdaterBuilder
                .UseSqlServer()
                .Build();

            var result = await databaseUpdater.UpgradeAsync(args, TestContext.Current.CancellationToken);

            result.Should().Be(1);

            output.ToString().Should().Be(
                """
                Description:
                  Upgrade the MyApplication database.

                Usage:
                  PosInformatique.Database.Updater.Tests <connection-string> [options]

                Arguments:
                  <connection-string>  The connection string to the database to upgrade

                Options:
                  --access-token <access-token>        Access token to connect to the SQL database.
                  --command-timeout <command-timeout>  Maximum time in seconds to execute each SQL statements. [default: 30]
                  -?, -h, --help                       Show help and usage information
                  --version                            Show version information

                
                """);
        }
    }
}