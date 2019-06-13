using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppCore.Model;

namespace DesktopGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool GameInitialized { get; set; }

        public event Action<Coordinates> CellAttacked;
        public event Action<IEnumerable<Coordinates>> ShipPlaced;
        public event Action GameReset;

        public void Inform(string message)
        {
            throw new NotImplementedException();
        }

        public void PrepareNewGame()
        {
            throw new NotImplementedException();
        }

        public void SetCellState(Coordinates coordinates, OperationResult operationResult)
        {
            throw new NotImplementedException();
        }
    }
}
