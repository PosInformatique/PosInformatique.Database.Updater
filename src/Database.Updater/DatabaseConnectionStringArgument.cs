//-----------------------------------------------------------------------
// <copyright file="DatabaseConnectionStringArgument.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater
{
    using System.CommandLine;
    using System.CommandLine.Parsing;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class DatabaseConnectionStringArgument : Argument<string>
    {
        private readonly IDatabaseProvider databaseProvider;

        public DatabaseConnectionStringArgument(IDatabaseProvider databaseProvider, string name)
            : base(name)
        {
            this.databaseProvider = databaseProvider;

            this.Validators.Add(this.Validate);
        }

        private void Validate(ArgumentResult result)
        {
            var connectionString = result.GetValue(this);
            var validationResult = this.databaseProvider.ValidateConnectionString(connectionString!, this.Name);

            if (validationResult is not null)
            {
                result.AddError(validationResult);
            }
        }
    }
}
