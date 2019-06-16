using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AppCore.Model;

namespace DesktopGUI.Views
{
    /// <summary>
    /// Interaction logic for GameCell.xaml
    /// </summary>
    public partial class GameCell : UserControl, IGameCellView
    {
        public Coordinates Coordinates { get; set; }
        public GameCellState State { get; private set; }
        public GameCell(Coordinates coordinates)
        {
            InitializeComponent();
            this.Coordinates = coordinates;
        }

        public event Action<Coordinates> cellClicked;

        public void SetState(GameCellState state)
        {
            switch (state)
            {
                case GameCellState.Cloaked:
                {
                    Btn.IsEnabled = true;
                    Btn.Background = Brushes.Black;
                    Btn.Content = "";
                    State = state;
                }break;
                case GameCellState.Uncloaked:
                {
                    //Btn.IsEnabled = false;
                    Btn.Background = Brushes.AntiqueWhite;
                    Btn.Content = "";
                    State = state;
                    cellClicked = null;
                }
                    break;
                case GameCellState.Occupied:
                {
                    //Btn.IsEnabled = false;
                    Btn.Background = Brushes.Blue;
                    State = state;
                    cellClicked = null;
                    }
                    break;
                case GameCellState.Hit:
                {
                    //Btn.IsEnabled = false;
                    Btn.Background = Brushes.DarkBlue;
                    Btn.Content = "X";
                    State = state;
                    cellClicked = null;
                    }
                    break;
                case GameCellState.Sinked:
                {
                    //Btn.IsEnabled = false;
                    Btn.Background = Brushes.IndianRed;
                    Btn.Content = "X";
                    State = state;
                    cellClicked = null;
                    }
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cellClicked?.Invoke(Coordinates);
        }

        public void ClearEvents()
        {
            cellClicked = null;
        }
    }
}
