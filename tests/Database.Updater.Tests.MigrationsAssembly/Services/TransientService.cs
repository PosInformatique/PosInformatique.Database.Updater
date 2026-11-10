//-----------------------------------------------------------------------
// <copyright file="TransientService.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    public class TransientService : ITransientService
    {
        public TransientService(ISingletonService singletonService)
        {
            this.InstanceCount++;
            this.SingletonService = singletonService;
        }

        public ISingletonService SingletonService { get; }

        public int InstanceCount { get; private set; }
    }
}
