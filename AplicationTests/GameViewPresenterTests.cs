using AppCore.Model;
using AppCore.Operations;
using DesktopGUI;
using DesktopGUI.Views;
using Moq;
using NUnit.Framework;

namespace AplicationTests
{

    [TestFixture]
    public class GameViewPresenterTests : BattleShipTestBase
    {
        private Mock<IOperationsManager> managerMock = new Mock<IOperationsManager>();
        private Mock<IMainView> mainViewMock = new Mock<IMainView>();
        private GameViewPresenter presenter;

        [OneTimeSetUp]
        public void SetUp()
        {
            presenter = new GameViewPresenter(mainViewMock.Object,managerMock.Object);
            presenter.computerGameBoard = CreateBoardWithShips();

        }

        [Test]
        public void GameResetTest()
        {
            mainViewMock.Raise(view => view.GameReset += null);
            managerMock.Verify(m => m.PlaceShipsForComputer(It.IsAny<GameBoard>()),Times.Once);
        }

        [Test]
        public void AttackTest()
        {
            var attackCoords = new Coordinates {X = 3, Y = 5};
            mainViewMock.Raise(view => view.CellAttacked += null, attackCoords);
            managerMock.Verify(m => m.PlaceShotByPlayer(attackCoords,It.IsAny<GameBoard>()),Times.Once);
        }
    }
}
