//-----------------------------------------------------------------------
// <copyright file="PersonDbContextFactory.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.MigrationsAssembly
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    internal sealed class PersonDbContextFactory : IDesignTimeDbContextFactory<PersonDbContext>
    {
        public PersonDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<PersonDbContext>()
                .UseSqlServer(b => b.MigrationsAssembly(this.GetType().Assembly.GetName().Name));

            return new PersonDbContext(options.Options);
        }
    }
}