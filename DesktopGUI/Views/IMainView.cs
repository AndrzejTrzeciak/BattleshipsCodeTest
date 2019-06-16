using System;
using System.Collections.Generic;
using AppCore.Model;

namespace DesktopGUI.Views
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
