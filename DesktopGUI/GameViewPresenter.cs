using AppCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Operations;

namespace DesktopGUI
{
    public class GameViewPresenter
    {
        private IMainView mainView;
        private IOperationsManager operationsManager;
        private GameBoard playerGameBoard;
        private GameBoard computerGameBoard;

        public GameViewPresenter(IMainView mainView, IOperationsManager operationsManager)
        {
            this.mainView = mainView;
            this.operationsManager = operationsManager;
            this.mainView.CellAttacked += HandleCellAttack;
            this.mainView.ShipPlaced += PlacePlayerShip;
            this.mainView.GameReset += InitializeGameBoards;
            InitializeGameBoards();
        }

        private void PlacePlayerShip(IEnumerable<Coordinates> shipCellCoordinates)
        {
            IEnumerable<Cell> shipCells = operationsManager.PlaceShipByPlayer(shipCellCoordinates, playerGameBoard);
            foreach (var cell in shipCells)
                mainView.SetCellState(cell.Coordinates, OperationResult.shipPlaced);

        }

        private void InitializeGameBoards()
        {
            playerGameBoard = CreateCleanGameBoard();
            computerGameBoard = CreateCleanGameBoard();
            operationsManager.PlaceShipsForComputer(computerGameBoard);
            mainView.GameInitialized = false; //player has to place his ships
        }

        private static GameBoard CreateCleanGameBoard()
        {
            return new GameBoard
            {
                BattleshipPlaced = false,
                DestroyersCount = 0
            };
        }

        private void HandleCellAttack(Coordinates cellCoordinates)
        {
            var cell = new Cell
            {
                Coordinates = cellCoordinates
            };
            OperationResult operationResult = operationsManager.PlaceShotByPlayer(cellCoordinates, computerGameBoard);
            mainView.SetCellState(cellCoordinates,operationResult);
            if (operationResult == OperationResult.sink)
            {
                bool gameWonByPlayer = !computerGameBoard.Cells.Any(x => x.IsOccupied && !x.IsHit);
                if (gameWonByPlayer)
                {
                    mainView.Inform("You won");
                    mainView.PrepareNewGame();
                }
                else
                {
                    mainView.Inform("Ship sinked!");
                }
            }
            else if (operationResult == OperationResult.hit)
                mainView.Inform("It's a hit!");
        }
    }
}
