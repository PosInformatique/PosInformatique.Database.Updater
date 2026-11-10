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
            var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            serviceProvider.Setup(sp => sp.GetHashCode())
                .Returns(1234);

            var extension = new DatabaseUpdaterExtension(serviceProvider.Object);

            extension.Info.IsDatabaseProvider.Should().BeFalse();
            extension.Info.LogFragment.Should().BeEmpty();
            extension.Info.GetServiceProviderHashCode().Should().Be(1234);
            extension.Info.ShouldUseSameServiceProvider(default).Should().BeFalse();
        }
    }
}
