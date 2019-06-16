using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AppCore.Model;

namespace AppCore.Operations
{
    /// <summary>
    /// Updates the game state.
    /// Most validations are negleced - they will be provided by the frontend.
    /// In case of a new client with less validations (e.g. a consol application) the validations in this service need to be enhanced.
    /// </summary>
    public class OperationsManager : IOperationsManager
    {

        public void PlaceShipsForComputer(GameBoard computerGameBoard)
        {
            while (!computerGameBoard.DestroyersPlaced)
            {
                List<Coordinates> coordinates= CoordinatresGenerator.GenerateShipCoordinates(4);
                bool shipPlaced = TryPlaceShipForComputer(coordinates, computerGameBoard);
                if (shipPlaced)
                    computerGameBoard.DestroyersCount++;
            }

            while (!computerGameBoard.BattleshipPlaced)
            {
                List<Coordinates> coordinates = CoordinatresGenerator.GenerateShipCoordinates(5);
                bool shipPlaced = TryPlaceShipForComputer(coordinates, computerGameBoard);
                if (shipPlaced)
                    computerGameBoard.BattleshipPlaced = true;
            }

        }

        private static bool TryPlaceShipForComputer(List<Coordinates> coordinatesToPlace, GameBoard computerGameBoard)
        {
            foreach (var coordinate in coordinatesToPlace)
            {
                if (computerGameBoard.Cells.Any(c => c.Coordinates.Equals(coordinate) && c.IsOccupied))
                    return false;
            }

            var newShipCells = new List<Cell>();
            foreach (var coordinate in coordinatesToPlace)
            {
                var matchingCell = computerGameBoard.Cells.FirstOrDefault(c => c.Coordinates.Equals(coordinate));
                if (matchingCell != null)
                    matchingCell.IsOccupied = true;
                newShipCells.Add(matchingCell);
            }
            var ship = new Ship(newShipCells);
            computerGameBoard.Ships.Add(ship);
            return true;
        }

        public OperationResult PlaceShotByPlayer(Coordinates targetCoordinates, GameBoard computerGameBoard)
        {

            var matchingCell = computerGameBoard.Cells.FirstOrDefault(cell => cell.Coordinates.Equals(targetCoordinates));
            matchingCell.IsCloaked = false;
            var ship = computerGameBoard.Ships.FirstOrDefault(s => s.Cells.Contains(matchingCell));
            if (ship != null)
            {
                matchingCell.IsHit = true;
                if (ship.Cells.All(cell => cell.IsHit))
                {
                    foreach (var shipCell in ship.Cells)
                    {
                        shipCell.IsSinked = true;
                    }

                    return OperationResult.sink;
                }

                return OperationResult.hit;
            }
            else
                return OperationResult.mishit;

        }

        
    }
}
