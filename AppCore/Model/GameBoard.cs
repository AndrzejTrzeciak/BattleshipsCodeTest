using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Model
{

    public class GameBoard
    {
        public IList<Cell> Cells;
        public IList<Ship> Ships;
        public bool BattleshipPlaced;
        public bool DestroyersPlaced => DestroyersCount == 2;
        public short DestroyersCount;

        public GameBoard()
        {
            Cells = new List<Cell>();
            for (short x = 0; x < 10; x++)
            {
                for (short y = 0; y < 10; y++)
                {
                    Cells.Add(new Cell
                    {
                        Coordinates = new Coordinates
                        {
                            X = x,Y = y
                        }
                    });
                }
            }
        }
    }
    // 1x Battleship(5 squares)
    // 2x Destroyers(4 squares)
}
