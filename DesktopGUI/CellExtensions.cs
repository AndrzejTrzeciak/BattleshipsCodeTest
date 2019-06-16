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

    public static class CellExtensions
    {
        public static IGameCellView ToGameCellView(this Cell cell)
        {
            var result = new GameCell(cell.Coordinates);
            if (cell.IsCloaked)
            {
                result.SetState(GameCellState.Cloaked);
                return result;
            }

            if (!cell.IsHit && !cell.IsOccupied)
            {
                result.SetState(GameCellState.Uncloaked);
                return result;
            }

            if (!cell.IsHit && cell.IsOccupied)
            {
                result.SetState(GameCellState.Occupied);
                return result;
            }

            if (cell.IsHit && !cell.IsSinked)
            {
                result.SetState(GameCellState.Hit);
                return result;
            }

            if (cell.IsSinked)
            {
                result.SetState(GameCellState.Sinked);
                return result;
            }
            return result;
        } 
    }
}
