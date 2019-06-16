using NUnit.Framework;
using System;
using System.Linq;
using AppCore.Operations;
using DesktopGUI;
using Moq;


namespace AplicationTests
{
    [TestFixture]
    public class UnitTest1
    {
        private Mock<IMainView> mainViewMock = new Mock<IMainView>();

        [Test]
        public void TestShipCount()
        {
            var manager = new OperationsManager();
            var presenter = new GameViewPresenter(mainViewMock.Object, manager);
            ///tbc
        }
    }
}
