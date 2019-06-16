using System.Collections.Generic;

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
}
