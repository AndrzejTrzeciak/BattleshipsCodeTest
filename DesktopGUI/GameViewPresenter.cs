using AppCore.Model;
using System.Collections.Generic;
using System.Linq;
using AppCore.Operations;

namespace DesktopGUI
{
    public class GameViewPresenter
    {
        internal IMainView mainView;
        internal IOperationsManager operationsManager;
        internal GameBoard computerGameBoard;

        public GameViewPresenter(IMainView mainView, IOperationsManager operationsManager)
        {
            this.mainView = mainView;
            this.operationsManager = operationsManager;
            this.mainView.CellAttacked += HandleCellAttack;
            this.mainView.GameReset += InitializeGameBoards;
        }

        private void InitializeGameBoards()
        {
            computerGameBoard = CreateCleanGameBoard();
            foreach (var cell in computerGameBoard.Cells)
            {
                cell.IsCloaked = true;
            }
            operationsManager.PlaceShipsForComputer(computerGameBoard);
            mainView.RenderComputerGameBoard(computerGameBoard.Cells.Select(cell => cell.ToGameCellView()));
        }

        private static GameBoard CreateCleanGameBoard()
        {
            return new GameBoard
            {
                BattleshipPlaced = false,
                DestroyersCount = 0,
                Ships = new List<Ship>()
            };
        }

        private void HandleCellAttack(Coordinates cellCoordinates)
        {
            OperationResult operationResult = operationsManager.PlaceShotByPlayer(cellCoordinates, computerGameBoard);
            mainView.RenderComputerGameBoard(computerGameBoard.Cells.Select(c => c.ToGameCellView()));
            if (operationResult == OperationResult.sink)
            {
                bool gameWonByPlayer = !computerGameBoard.Ships.Any(ship => ship.Cells.Any(c => !c.IsSinked));
                if (gameWonByPlayer)
                {
                    var clickCount = computerGameBoard.Cells.Count -
                                     computerGameBoard.Cells.Where(c => c.IsCloaked).Count();
                    mainView.Inform($"You won. You used\n{clickCount}\nshots.");
                    mainView.PrepareNewGame();
                }
            }
        }
    }
}
