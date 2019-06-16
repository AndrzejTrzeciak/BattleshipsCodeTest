using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AppCore.Operations;
using DesktopGUI;
using Moq;
using AppCore.Model;

namespace AplicationTests
{
    [TestFixture]
    public class OperationsManagerTests : BattleShipTestBase
    {
        private Mock<IMainView> mainViewMock = new Mock<IMainView>();
        private GameBoard board;
        public OperationsManagerTests()
        {
            board = CreateBoardWithShips();
        }

        [Test]
        public void TestShipCount()
        {
            ///arrange
            var manager = new OperationsManager();
            GameBoard board = new GameBoard();
            board.Ships = new List<Ship>();
            //act
            manager.PlaceShipsForComputer(board);
            //assert
            Assert.True(board.BattleshipPlaced);
            Assert.True(board.DestroyersPlaced);
            Assert.AreEqual(3,board.Ships.Count);
            Assert.AreEqual(2,board.Ships.Where(ship => ship.Cells.Count() == 4).Count());
            Assert.AreEqual(1, board.Ships.Where(ship => ship.Cells.Count() == 5).Count());
        }

        [Test]
        [TestCase(0,0,OperationResult.mishit)]
        [TestCase(5,1,OperationResult.hit)]
        [TestCase(4, 1, OperationResult.hit)]
        [TestCase(3, 1, OperationResult.hit)]
        [TestCase(2, 1, OperationResult.sink)]
        public void CellAttackTest(int Xcoord, int Ycoord, OperationResult expecctedResult)
        {
            //arrange
            var coord = new Coordinates
            {
                X = Xcoord,
                Y = Ycoord
            };
            var manager = new OperationsManager();
            //act
            var result = manager.PlaceShotByPlayer(coord, board);
            //assert

            Assert.AreEqual(expecctedResult,result);
        }

        [Test]
        //this test runs for about 50s
        public void PlacedShipsShouldNeverOverlap()
        {
            for (int i = 0; i < 1000; i++)
            {
                //arrange
                var board = base.CreateEmptyCloakedBoard();
                var manager = new OperationsManager();

                //act
                manager.PlaceShipsForComputer(board);

                //assert
                bool overlappingShipsPresent = CheckOverlapping(board);
                Assert.IsFalse(overlappingShipsPresent);
            }
        }

        /// <summary>
        /// returns true if overlapping ships are found
        /// otherwise returns false
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private bool CheckOverlapping(GameBoard board)
        {
            List<Coordinates> allCellsInShips = board.Ships.SelectMany(ship => ship.Cells.Select(cell => cell.Coordinates).ToList()).ToList();
            List<Coordinates> distinctElementsInAllShips = allCellsInShips.Distinct().ToList();
            return allCellsInShips.Count != distinctElementsInAllShips.Count;
        }
    }
}
