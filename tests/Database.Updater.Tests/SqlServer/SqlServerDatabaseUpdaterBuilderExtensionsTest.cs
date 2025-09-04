//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseUpdaterBuilderExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    using PosInformatique.Database.Updater.SqlServer;

    public class SqlServerDatabaseUpdaterBuilderExtensionsTest
    {
        [Fact]
        public void Constructor()
        {
            var builder = new DatabaseUpdaterBuilder("MyApplication");

            builder.UseSqlServer().Should().BeSameAs(builder);

            builder.GetFieldValue<IDatabaseProvider>("databaseProvider").Should().BeOfType<SqlServerDatabaseProvider>();
        }
    }
}