using System.Collections.Generic;
using AppCore.Model;

namespace AppCore.Operations
{
    public interface IOperationsManager
    {
        IEnumerable<Cell> PlaceShipByPlayer(IEnumerable<Coordinates> shipCoordinates, GameBoard userGameBoard);
        void PlaceShipsForComputer(GameBoard computerGameBoard);
        OperationResult PlaceShotByComputer(GameBoard userGameBoard);
        OperationResult PlaceShotByPlayer(Coordinates targetCoordinates, GameBoard computerGameBoard);
    }
}