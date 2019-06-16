﻿using System;
using AppCore.Model;

namespace DesktopGUI.Views
{

    public interface IGameCellView
    {
        void SetState(GameCellState state);
        event Action<Coordinates> cellClicked;
        Coordinates Coordinates { get; set; }
        GameCellState State { get; }

        void ClearEvents();
    }
}
