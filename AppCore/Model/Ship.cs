using System.Collections.Generic;

namespace AppCore.Model
{

    public class Ship
    {
        public IEnumerable<Cell> Cells;

        public Ship(IEnumerable<Cell> shipCells)
        {
            this.Cells = shipCells;
        }
    }
    // 1x Battleship(5 squares)
    // 2x Destroyers(4 squares)
}
