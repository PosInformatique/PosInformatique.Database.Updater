//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterExtension.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class DatabaseUpdaterExtension : IDbContextOptionsExtension
    {
        private readonly IServiceProvider serviceProvider;

        public DatabaseUpdaterExtension(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public DbContextOptionsExtensionInfo Info => new CustomExtensionInfo(this);

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new DatabaseUpdaterServiceProvider(this.serviceProvider));
        }

        public void Validate(IDbContextOptions options)
        {
        }

        private sealed class CustomExtensionInfo : DbContextOptionsExtensionInfo
        {
            public CustomExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override bool IsDatabaseProvider => false;

            public override string LogFragment => string.Empty;

            public override int GetServiceProviderHashCode() => 0;

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
            }
        }
    }
}
