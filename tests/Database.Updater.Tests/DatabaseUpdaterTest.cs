//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    using PosInformatique.Testing.Databases.SqlServer;

    [Collection(nameof(DatabaseUpdaterTest))]
    public class DatabaseUpdaterTest
    {
        private const string ConnectionString = "Data Source=(localDB)\\posinfo-tests; Integrated Security=True";

        [Fact]
        public async Task UpgradeAsync_WithExplicitMigrationsAssembly()
        {
            var server = new SqlServer(ConnectionString);

            var database = await server.CreateEmptyDatabaseAsync("DatabaseUpdaterTest_UpgradeAsync_WithExplicitMigrationsAssembly");

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication")
                .UseMigrationsAssembly(typeof(MigrationsAssembly.Version1).Assembly);
            var databaseUpdater = databaseUpdaterBuilder
                .Build();

            var result = await databaseUpdater.UpgradeAsync([database.ConnectionString]);

            result.Should().Be(0);

            var tables = await database.GetTablesAsync();

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
            var server = new SqlServer(ConnectionString);

            var database = await server.CreateEmptyDatabaseAsync("DatabaseUpdaterTest_UpgradeAsync_WithErrorMigrationsAssembly");

            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication")
                .UseMigrationsAssembly(typeof(MigrationsErrorAssembly.Version1).Assembly);
            var databaseUpdater = databaseUpdaterBuilder
                .Build();

            var result = await databaseUpdater.UpgradeAsync([database.ConnectionString]);

            result.Should().Be(99);
        }

        [Fact]
        public async Task UpgradeAsync_NoArguments()
        {
            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication");
            var databaseUpdater = databaseUpdaterBuilder
                .Build();

            var result = await databaseUpdater.UpgradeAsync([]);

            result.Should().Be(1);
        }

        [Theory]
        [InlineData("NotConnectionString")]
        [InlineData(ConnectionString, "--command-timeout=abcd")]
        public async Task UpgradeAsync_WrongArguments(params string[] args)
        {
            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication");
            var databaseUpdater = databaseUpdaterBuilder
                .Build();

            var result = await databaseUpdater.UpgradeAsync(args);

            result.Should().Be(1);
        }
    }
}