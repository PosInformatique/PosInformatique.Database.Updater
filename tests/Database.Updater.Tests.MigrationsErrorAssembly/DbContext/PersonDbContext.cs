//-----------------------------------------------------------------------
// <copyright file="PersonDbContext.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.MigrationsErrorAssembly
{
    using Microsoft.EntityFrameworkCore;

    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable("Person")
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
