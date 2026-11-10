//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterServiceProvider.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    internal sealed class DatabaseUpdaterServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        public DatabaseUpdaterServiceProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}
