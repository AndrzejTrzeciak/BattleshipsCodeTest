using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Model;

namespace AppCore.Utils
{

    public static class CoordinatresGenerator
    {
        public static List<Coordinates> GenerateShipCoordinates(int shipCellCount)
        {
            bool succeded = false;
            List<Coordinates> result = null;
            while (!succeded)
            {
                Coordinates startingCoordinates = GenerateRandomCoordinate();
                Shape shape = PickRandomShape();
                result = new List<Coordinates> { startingCoordinates };
                switch (shape)
                {
                    case Shape.StraightHorizontal:
                        {
                            List<Coordinates> coordinates =
                                GenerateStraightHorizontalCoordinated(startingCoordinates, shipCellCount - 1);
                            result.AddRange(coordinates);
                            succeded = true;

                        }
                        break;
                    case Shape.Lshape:
                        {
                            List<Coordinates> coordinates =
                                GenerateL_coordinates(startingCoordinates, shipCellCount - 1);
                            result.AddRange(coordinates);
                            succeded = true;

                        }
                        break;
                    case Shape.Square:
                        {
                            List<Coordinates> coordinates =
                                TryGenerateSquareCoordinates(startingCoordinates, shipCellCount - 1);
                            if (coordinates != null)
                            {
                                result.AddRange(coordinates);
                                succeded = true;
                            }
                        }
                        break;
                    default:
                        {
                            List<Coordinates> coordinates =
                                GenerateStraightVerticalCoordinates(startingCoordinates, shipCellCount - 1);
                            result.AddRange(coordinates);
                            succeded = true;
                        }
                        break;
                }
            }

            return result;
        }

        private static List<Coordinates> TryGenerateSquareCoordinates(Coordinates startingCoordinates, int cellToPlaceCount)
        {
            if (cellToPlaceCount != 3)
                return null;
            var result = new List<Coordinates>();
            if (startingCoordinates.X == 9)
            {//we have to go to the left
                result.Add(new Coordinates
                {
                    X = startingCoordinates.X - 1,
                    Y = startingCoordinates.Y
                });
                if (startingCoordinates.Y == 9)
                {//we have to go upwards
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y - 1
                    });
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X - 1,
                        Y = startingCoordinates.Y - 1
                    });
                }
                else
                {//we go downwards
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y + 1
                    });
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X - 1,
                        Y = startingCoordinates.Y + 1
                    });
                }
            }
            else
            {//we go to the right
                result.Add(new Coordinates
                {
                    X = startingCoordinates.X + 1,
                    Y = startingCoordinates.Y
                });
                if (startingCoordinates.Y == 9)
                {//we have to go upwards
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y - 1
                    });
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X + 1,
                        Y = startingCoordinates.Y - 1
                    });
                }
                else
                {//we go downwards
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y + 1
                    });
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X + 1,
                        Y = startingCoordinates.Y + 1
                    });
                }
            }

            return result;
        }

        private static List<Coordinates> GenerateL_coordinates(Coordinates startingCoordinates, int cellToPlaceCount)
        {
            //to make it simple we'll allways go on cell upwards or downwards
            //and 2 or 3 cells - depending on whether it a destroyer or battleship
            //to the left or to the right
            var result = new List<Coordinates>();
            if (startingCoordinates.Y + 1 > 9)
                result.Add(new Coordinates
                {
                    X = startingCoordinates.X,
                    Y = startingCoordinates.Y - 1
                });
            else
                result.Add(new Coordinates
                {
                    X = startingCoordinates.X,
                    Y = startingCoordinates.Y + 1
                });
            var placedCoordinate = result.First();
            if (startingCoordinates.X + (cellToPlaceCount - 1) > 9)
            {//we have to go to the left
                for (int i = 1; i < cellToPlaceCount; i++)
                    result.Add(new Coordinates
                    {
                        X = placedCoordinate.X - i,
                        Y = placedCoordinate.Y
                    });
            }
            else
            {//we go to the right
                for (int i = 1; i < cellToPlaceCount; i++)
                    result.Add(new Coordinates
                    {
                        X = placedCoordinate.X + i,
                        Y = placedCoordinate.Y
                    });
            }
            return result;

        }

        private static List<Coordinates> GenerateStraightVerticalCoordinates(Coordinates startingCoordinates, int cellToPlaceCount)
        {
            var result = new List<Coordinates>();
            if (startingCoordinates.Y + cellToPlaceCount > 9)
            {
                //we have to place the ship upwards from the starting point
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    //if (startingCoordinates.Y - i < 0)
                    //    return null;
                    //this check would be needed if we used ships with length > 5 (cellToPlaceCount > 4)
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y - i
                    });
                }
            }
            else
            {
                //we have to place the ship downwards
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X,
                        Y = startingCoordinates.Y + i
                    });
                }
            }

            return result;
        }

        private static List<Coordinates> GenerateStraightHorizontalCoordinated(Coordinates startingCoordinates, int cellToPlaceCount)
        {
            var result = new List<Coordinates>();
            if (startingCoordinates.X + cellToPlaceCount > 9)
            {
                //we have to place the ship to the left from the starting point
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    //if (startingCoordinates.X - i < 0)
                    //    return null;
                    //this check would be needed if we used ships with length > 5 (cellToPlaceCount > 4)
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X - i,
                        Y = startingCoordinates.Y
                    });
                }
            }
            else
            {
                //we have to place the ship to the right
                for (int i = 1; i <= cellToPlaceCount; i++)
                {
                    result.Add(new Coordinates
                    {
                        X = startingCoordinates.X + i,
                        Y = startingCoordinates.Y
                    });
                }
            }

            return result;
        }

        private static Shape PickRandomShape()
        {
            var random = new Random();
            var x = random.Next(400);
            if (x <= 100)
                return Shape.StraightVertical;
            if (x <= 200)
                return Shape.StraightHorizontal;
            if (x <= 300)
                return Shape.Lshape;
            return Shape.Square;
        }

        private static Coordinates GenerateRandomCoordinate()
        {
            var random = new Random();
            var startingX = random.Next(9);
            var startingY = random.Next(9);
            return new Coordinates()
            {
                X = startingX,
                Y = startingY
            };
        }

        //explicite enumeration just to make the idea more readable - not needed from a technical point of view
        private enum Shape
        {
            StraightVertical = 100,
            StraightHorizontal = 200,
            Lshape = 300,
            Square = 400
        }
    }
}
