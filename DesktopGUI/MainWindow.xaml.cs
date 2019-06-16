using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AppCore.Model;
using AppCore.Operations;
using DesktopGUI.Views;

namespace DesktopGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private List<IGameCellView> computerGameCells = new List<IGameCellView>();
        public event Action<Coordinates> CellAttacked;
        public event Action GameReset;

        public MainWindow()
        {
            InitializeComponent();
            IOperationsManager manager = new OperationsManager();
            var presenter = new GameViewPresenter(this, manager);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    var gameCellComputer = new GameCell(new Coordinates() {X = x, Y = y});
                    Grid.SetColumn(gameCellComputer, x);
                    Grid.SetRow(gameCellComputer, y);
                    ComputerBoard.Children.Add(gameCellComputer);
                    computerGameCells.Add(gameCellComputer);
                }
            }
            PrepareNewGame();
        }

        public void Inform(string message)
        {
            MessageBox.Show(message);
        }

        public void PrepareNewGame()
        {
            GameReset?.Invoke();
            foreach (IGameCellView computerGameCell in computerGameCells)
            {
                computerGameCell.ClearEvents();
                computerGameCell.cellClicked += (coordinates) => CellAttacked?.Invoke(coordinates);
            }
        }

        public void RenderComputerGameBoard(IEnumerable<IGameCellView> updatedGameCells)
        {
            foreach (var cellView in updatedGameCells)
            {
                var matchingCellInGUI = computerGameCells
                    .FirstOrDefault(cell => cell.Coordinates.Equals(cellView.Coordinates));
                matchingCellInGUI?.SetState(cellView.State);
            }
        }
    }
}
