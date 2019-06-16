using AppCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace AplicationTests
{

    public class BattleShipTestBase
    {
        protected GameBoard CreateBoardWithShips()
        {
            GameBoard board = new GameBoard();
            board.Ships = new List<Ship>();
            var destroyer1CellCoordinates = new List<Coordinates>
            {
                new Coordinates
                {
                    X = 5, Y = 1
                },
                new Coordinates
                {
                    X = 4, Y = 1
                },
                new Coordinates
                {
                    X = 3, Y = 1
                },
                new Coordinates
                {
                    X = 2, Y = 1
                }
            };
            var destroyer2CellCoordinates = new List<Coordinates>()
            {
                new Coordinates
                {
                    X = 7, Y = 1
                },
                new Coordinates
                {
                    X = 7, Y = 2
                },
                new Coordinates
                {
                    X = 7, Y = 3
                },
                new Coordinates
                {
                    X = 7, Y = 4
                }
            };
            var battleShipCellCoordiantes = new List<Coordinates>()
            {
               new Coordinates
                    {
                        X = 8, Y = 1
                    },
                new Coordinates
                    {
                        X = 8, Y = 2
                    },
                new Coordinates
                    {
                        X = 8, Y = 3
                    },
                new Coordinates
                    {
                        X = 8, Y = 4
                    },
                new Coordinates
                    {
                        X = 8, Y = 5
                    }
            };
            var destroyer1 = new Ship(new List<Cell>());
            foreach (var coord in destroyer1CellCoordinates)
            {
                var cell = board.Cells.FirstOrDefault(c => c.Coordinates.Equals(coord));
                cell.IsOccupied = true;
                ((List<Cell>)destroyer1.Cells).Add(cell);
            }
            board.Ships.Add(destroyer1);

            var destroyer2 = new Ship(new List<Cell>());
            foreach (var coord in destroyer2CellCoordinates)
            {
                var cell = board.Cells.FirstOrDefault(c => c.Coordinates.Equals(coord));
                cell.IsOccupied = true;
                ((List<Cell>)destroyer2.Cells).Add(cell);
            }
            board.Ships.Add(destroyer2);

            var battleShip = new Ship(new List<Cell>());
            foreach (var coord in battleShipCellCoordiantes)
            {
                var cell = board.Cells.FirstOrDefault(c => c.Coordinates.Equals(coord));
                cell.IsOccupied = true;
                ((List<Cell>)battleShip.Cells).Add(cell);
            }
            board.Ships.Add(battleShip);
            return board;

        }

        protected GameBoard CreateEmptyCloakedBoard()
        {
            var board = new GameBoard();
            foreach (var cell in board.Cells)
            {
                cell.IsCloaked = true;
            }

            board.Ships = new List<Ship>();
            return board;
        }
    }
}
