using AppCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Operations;
using DesktopGUI.Views;

namespace DesktopGUI
{
    public class GameViewPresenter
    {
        private IMainView mainView;
        private IOperationsManager operationsManager;
        private GameBoard computerGameBoard;

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
            var cell = new Cell
            {
                Coordinates = cellCoordinates
            };
            OperationResult operationResult = operationsManager.PlaceShotByPlayer(cellCoordinates, computerGameBoard);
            mainView.RenderComputerGameBoard(computerGameBoard.Cells.Select(c => c.ToGameCellView()));
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
                    //mainView.Inform("Ship sinked!");
                }
            }
            //else if (operationResult == OperationResult.hit)
                //mainView.Inform("It's a hit!");
        }
    }
}
