//-----------------------------------------------------------------------
// <copyright file="Version1.cs" company="P.O.S Informatique">
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
    [Migration("Version1")]
    public class Version1 : Migration
    {
        public Version1(ISingletonService singletonService, ITransientService transientService, IDatabaseUpdaterCommandLine commandLine)
        {
            transientService.SingletonService.Should().BeSameAs(singletonService);
            transientService.InstanceCount.Should().Be(1);

            commandLine.GetValue(UpdaterTestArguments.Argument1).Should().Be("Argument1Value");
            commandLine.GetValue(UpdaterTestArguments.Options1).Should().Be("Option1Value");
            commandLine.GetValue(UpdaterTestArguments.Options2).Should().Be(123);
            commandLine.GetValue(UpdaterTestArguments.Options3).Should().Be(9999);
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                });
        }
    }
}
