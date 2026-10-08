//-----------------------------------------------------------------------
// <copyright file="ITransientService.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    public interface ITransientService
    {
        int InstanceCount { get; }

        ISingletonService SingletonService { get; }
    }
}
