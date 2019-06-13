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

        public IEnumerable<Cell> PlaceShipByPlayer(IEnumerable<Coordinates> shipCoordinates, GameBoard userGameBoard)
        {
            var shipCells = new List<Cell>();
            foreach (var coordinate in shipCoordinates)
            {
                var matchingCellInUserGameBoard =
                    userGameBoard.Cells.FirstOrDefault(c => c.Coordinates.Equals(coordinate));
                if (matchingCellInUserGameBoard != null)
                    matchingCellInUserGameBoard.IsOccupied = true;
                shipCells.Add(matchingCellInUserGameBoard);
            }
            userGameBoard.Ships.Add(new Ship(shipCells));
            if (shipCoordinates.Count() == 4) //destroyer was placed
                userGameBoard.DestroyersCount++;
            else 
                userGameBoard.BattleshipPlaced = true;
            return shipCells;
        }

        public void PlaceShipsForComputer(GameBoard computerGameBoard)
        {
            while (!computerGameBoard.DestroyersPlaced)
            {
                Shape shape = PickRandomShape();
                List<Coordinates> coordinates= GenerateShipCoordinates(4,shape);
                bool shipPlaced = TryPlaceShip(coordinates, computerGameBoard);
                if (shipPlaced)
                    computerGameBoard.DestroyersCount++;
            }

            while (!computerGameBoard.BattleshipPlaced)
            {
                Shape shape = PickRandomShape();
                List<Coordinates> coordinates = GenerateShipCoordinates(5,shape);
                bool shipPlaced = TryPlaceShip(coordinates, computerGameBoard);
                if (shipPlaced)
                    computerGameBoard.BattleshipPlaced = true;
            }

        }

        private static bool TryPlaceShip(List<Coordinates> coordinatesToPlace, GameBoard computerGameBoard)
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

        private List<Coordinates> GenerateShipCoordinates(int shipCellCount, Shape shape)
        {
            bool succeded = false;
            List<Coordinates> result = null;
            while (!succeded)
            {
                Coordinates startingCoordinates = GenerateRandomCoordinate();
                result = new List<Coordinates> { startingCoordinates };
                switch (shape)
                {
                    case Shape.StraightHorizontal:
                    {
                        List<Coordinates> coordinateses =
                            TryGenerateStraightHorizontal(startingCoordinates, shipCellCount - 1);
                        if (coordinateses != null)
                        {
                            result.AddRange(coordinateses);
                            succeded = true;
                        }
                    }break;
                    default://other shapes to be implemented in the same fashion
                    {
                        List<Coordinates> coordinateses =
                            TryGenerateStraightVertical(startingCoordinates, shipCellCount - 1);
                        if (coordinateses != null)
                        {
                            result.AddRange(coordinateses);
                            succeded = true;
                        }
                    }break;
                }
            }

            return result;
        }

        private static List<Coordinates> TryGenerateStraightVertical(Coordinates startingCoordinates, int cellToPlaceCount)
        {
            var result = new List<Coordinates>();
            if (startingCoordinates.Y + cellToPlaceCount > 9)
            {
                //we have to place the ship upwards from the starting point
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    //if (startingCoordinates.Y - i < 0)
                    //    return null;
                    //this check would be needed if we used ships with length > 5 (cellToPlaceCount > 4)
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y - i
                    });
                }
            }
            else
            {
                //we have to place the ship downwards
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y + i
                    });
                }
            }

            return result;
        }

        private static List<Coordinates> TryGenerateStraightHorizontal(Coordinates startingCoordinates, int cellToPlaceCount)
        {
            var result = new List<Coordinates>();
            if (startingCoordinates.X + cellToPlaceCount > 9)
            {
                //we have to place the ship to the left from the starting point
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    //if (startingCoordinates.X - i < 0)
                    //    return null;
                    //this check would be needed if we used ships with length > 5 (cellToPlaceCount > 4)
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X - i,
                        Y = startingCoordinates.Y
                    });
                }
            }
            else
            {
                //we have to place the ship to the right
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X + i,
                        Y = startingCoordinates.Y
                    });
                }
            }

            return result;
        }

        private static Coordinates GenerateRandomCoordinate()
        {
            var random = new Random();
            var startingX = random.Next(9);
            var startingY = random.Next(9);
            return new Coordinates()
            {
                X = startingX,
                Y = startingY
            };
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
                        shipCell.IsSink = true;
                    }

                    return OperationResult.sink;
                }

                return OperationResult.hit;
            }
            else
                return OperationResult.mishit;

        }

        public OperationResult PlaceShotByComputer(GameBoard userGameBoard)
        {
            var shipUnderAttack =
                userGameBoard.Ships.FirstOrDefault(ship => ship.Cells.Any(cell => cell.IsHit && !cell.IsSink));
            if (shipUnderAttack != null)
            {
                //shoot the adjacent cells to finnish the attack
                Cell cellToAttack = FindAdjacentCloakedCell(shipUnderAttack, userGameBoard);
                cellToAttack.IsCloaked = false;
                if (cellToAttack.IsOccupied)
                {
                    //we migth have hit an adjacent ship
                    var ship = userGameBoard.Ships.FirstOrDefault(s => s.Cells.Contains(cellToAttack));
                    cellToAttack.IsHit = true;
                    if (ship.Cells.All(c => c.IsHit))
                    {
                        foreach (var cell in ship.Cells)
                        {
                            cell.IsSink = true;
                        }

                        return OperationResult.sink;
                    }

                    return OperationResult.hit;
                }
                return OperationResult.hit;
            }
            else
            {
                //no ship under attack - place a random shot in a cloaked cell
                bool cloakedCellFound = false;
                while (!cloakedCellFound)
                {
                    Coordinates randomCoordinate = GenerateRandomCoordinate();
                    Cell matchingGameBoardCell =
                        userGameBoard.Cells.FirstOrDefault(c => c.Coordinates.Equals(randomCoordinate));
                    if (matchingGameBoardCell.IsCloaked)
                    {
                        cloakedCellFound = true;
                        matchingGameBoardCell.IsCloaked = false;
                        if (matchingGameBoardCell.IsOccupied)
                        {
                            matchingGameBoardCell.IsHit = true;
                            Ship ship = userGameBoard.Ships.FirstOrDefault(s =>
                                s.Cells.Contains(matchingGameBoardCell));
                            if (ship.Cells.All(c => c.IsHit)) //that situation shouldn't occur, but just in case...
                            {
                                foreach (Cell cell in ship.Cells)
                                    cell.IsSink = true;
                                return OperationResult.sink;
                            }
                            else
                            {
                                return OperationResult.hit;
                            }
                        }
                    }
                }
                
            }
            return OperationResult.mishit;
        }

        private Cell FindAdjacentCloakedCell(Ship shipUnderAttack, GameBoard userGameBoard)
        {
            foreach (var shipCell in shipUnderAttack.Cells)
            {
                Cell cellToTheRight = FindNeighbourCell(shipCell,userGameBoard,1,0);
                if (cellToTheRight != null && cellToTheRight.IsCloaked)
                    return cellToTheRight;
                Cell cellToTheLeft = FindNeighbourCell(shipCell, userGameBoard, -1, 0); ;
                if (cellToTheLeft != null && cellToTheLeft.IsCloaked)
                    return cellToTheLeft;
                Cell cellAbove = FindNeighbourCell(shipCell, userGameBoard, 0, -1); ;
                if (cellAbove != null && cellAbove.IsCloaked)
                    return cellAbove;
                Cell cellBelow = FindNeighbourCell(shipCell, userGameBoard, 0, 1); ;
                if (cellBelow != null && cellBelow.IsCloaked)
                    return cellBelow;
            }
            //no cloaked cell found - that shouldn't occur - the ship would have been sinked
            return null;
        }

        private static Cell FindNeighbourCell(Cell cell,GameBoard board, int xOffset, int yOffset)
        {
            var coordinatesWithOffset = new Coordinates
            {
                X = cell.Coordinates.X + xOffset,
                Y = cell.Coordinates.Y + yOffset
            };
            return board.Cells.FirstOrDefault(c => c.Coordinates.Equals(coordinatesWithOffset));
        }
        // 1x Battleship(5 squares)
        // 2x Destroyers(4 squares)

        private static Shape PickRandomShape()
        {
            var random = new Random();
            var x = random.Next(600);
            if (x <= 100)
                return Shape.StraightVertical;
            if (x <= 200)
                return Shape.StraightHorizontal;
            if (x <= 300)
                return Shape.Symmertical_L_lookingUp;
            if (x <= 400)
                return Shape.Symmetrical_L_lookingDown;
            if (x <= 500)
                return Shape.Asymmetrical_L_lookingUp;
            return Shape.Asymmetrical_L_lookingDown;
        }

        //explicite enumeration just to make the idea more readable/understandable - not needed from a technical point of view
        private enum Shape
        {
            StraightVertical = 100,
            StraightHorizontal = 200,
            Symmertical_L_lookingUp = 300,
            Symmetrical_L_lookingDown = 400,
            Asymmetrical_L_lookingUp = 500,
            Asymmetrical_L_lookingDown = 600
        }
    }
}
