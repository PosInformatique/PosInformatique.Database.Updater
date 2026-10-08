//-----------------------------------------------------------------------
// <copyright file="DatabaseUpdaterExtensionTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    public class DatabaseUpdaterExtensionTest
    {
        [Fact]
        public void Info()
        {
            var extension = new DatabaseUpdaterExtension(default);

            extension.Info.IsDatabaseProvider.Should().BeFalse();
            extension.Info.LogFragment.Should().BeEmpty();
            extension.Info.GetServiceProviderHashCode().Should().Be(0);
            extension.Info.ShouldUseSameServiceProvider(default).Should().BeTrue();
        }
    }
}
