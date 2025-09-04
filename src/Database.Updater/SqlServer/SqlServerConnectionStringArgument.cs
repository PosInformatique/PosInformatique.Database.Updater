//-----------------------------------------------------------------------
// <copyright file="SqlServerConnectionStringArgument.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.SqlServer
{
    using System.CommandLine;
    using System.CommandLine.Parsing;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Data.SqlClient;

    [ExcludeFromCodeCoverage]
    internal class SqlServerConnectionStringArgument : Argument<string>
    {
        public SqlServerConnectionStringArgument(string name)
            : base(name)
        {
            this.Validators.Add(this.Validate);
        }

        private void Validate(ArgumentResult result)
        {
            try
            {
#pragma warning disable S1848 // Objects should not be created to be dropped immediately without being used
#pragma warning disable CA1806 // Do not ignore method results
                new SqlConnectionStringBuilder(result.GetValue(this));
#pragma warning restore CA1806 // Do not ignore method results
#pragma warning restore S1848 // Objects should not be created to be dropped immediately without being used
            }
            catch (ArgumentException)
            {
                result.AddError($"The SQL Server connection string specified in the '{this.Name}' argument is invalid.");
            }
        }
    }
}
