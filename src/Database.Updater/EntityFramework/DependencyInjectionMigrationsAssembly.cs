//-----------------------------------------------------------------------
// <copyright file="DependencyInjectionMigrationsAssembly.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.Migrations.Internal;
    using Microsoft.Extensions.DependencyInjection;

#pragma warning disable EF1001 // Internal EF Core API usage.
    internal sealed class DependencyInjectionMigrationsAssembly : MigrationsAssembly
    {
        private readonly IServiceProvider serviceProvider;

        public DependencyInjectionMigrationsAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
            DatabaseUpdaterServiceProvider serviceProvider)
            : base(currentContext, options, idGenerator, logger)
        {
            this.serviceProvider = serviceProvider;
        }

        public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            var migration = (Migration)ActivatorUtilities.CreateInstance(this.serviceProvider, migrationClass);

            return migration;
        }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}
