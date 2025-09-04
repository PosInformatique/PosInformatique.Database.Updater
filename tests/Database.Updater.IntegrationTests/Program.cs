//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.IntegrationTests
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var databaseUpdaterBuilder = new DatabaseUpdaterBuilder("MyApplication");

            var updater = databaseUpdaterBuilder
                .UseSqlServer()
                .UseMigrationsAssembly(typeof(MigrationsAssembly.PersonDbContext).Assembly)
                .Build();

            await updater.UpgradeAsync(args);
        }
    }
}
