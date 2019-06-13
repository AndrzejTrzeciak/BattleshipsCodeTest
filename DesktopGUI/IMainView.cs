using AppCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopGUI
{

    public interface IMainView
    {
        event Action<Coordinates> CellAttacked;
        event Action<IEnumerable<Coordinates>> ShipPlaced;
        event Action GameReset;
        void SetCellState(Coordinates coordinates, OperationResult operationResult);
        bool GameInitialized { get; set; }
        void Inform(string message);
        void PrepareNewGame();
    }


}
