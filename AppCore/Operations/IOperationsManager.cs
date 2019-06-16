using AppCore.Model;

namespace AppCore.Operations
{
    public interface IOperationsManager
    {
        void PlaceShipsForComputer(GameBoard computerGameBoard);
        OperationResult PlaceShotByPlayer(Coordinates targetCoordinates, GameBoard computerGameBoard);
    }
}