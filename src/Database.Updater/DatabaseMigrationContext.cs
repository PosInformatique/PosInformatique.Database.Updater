//-----------------------------------------------------------------------
// <copyright file="DatabaseMigrationContext.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.Collections.ObjectModel;

    internal sealed class DatabaseMigrationContext : IDatabaseMigrationContext
    {
        public DatabaseMigrationContext(string connectionString, IList<string> assemblies)
        {
            this.Assemblies = new ReadOnlyCollection<string>(assemblies);
            this.ConnectionString = connectionString;
        }

        public string? AccessToken { get; set; }

        public ReadOnlyCollection<string> Assemblies { get; }

        public int CommandTimeout { get; set; }

        public string ConnectionString { get; }
    }
}
