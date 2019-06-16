using AppCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopGUI.Views;

namespace DesktopGUI
{

    public interface IMainView
    {
        event Action<Coordinates> CellAttacked;
        event Action GameReset;
        void Inform(string message);
        void PrepareNewGame();
        void RenderComputerGameBoard(IEnumerable<IGameCellView> updatedGameCells);
    }


}
