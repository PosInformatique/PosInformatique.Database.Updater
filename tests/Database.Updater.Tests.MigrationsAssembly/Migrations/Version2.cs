//-----------------------------------------------------------------------
// <copyright file="Version2.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.MigrationsAssembly
{
    using AwesomeAssertions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;

    [DbContext(typeof(DbContext))]
    [Migration("Version2")]
    public class Version2 : Migration
    {
        public Version2(ISingletonService singletonService, ITransientService transientService)
        {
            transientService.SingletonService.Should().BeSameAs(singletonService);
            transientService.InstanceCount.Should().Be(1);
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Person",
                type: "bit",
                nullable: true);
        }
    }
}
