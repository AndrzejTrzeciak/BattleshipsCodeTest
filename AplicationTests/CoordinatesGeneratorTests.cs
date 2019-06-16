using NUnit.Framework;
using System.Linq;
using AppCore.Operations;

namespace AplicationTests
{

    [TestFixture]
    public class CoordinatesGeneratorTests
    {
        [Test]
        public void CheckIfCoordinatesAreInBounds()
        {
            for (int i = 0; i < 10000; i++)
            {
                var coordinates = CoordinatresGenerator.GenerateShipCoordinates(4);
                var allCoordinatesInBounds = coordinates.All(c => c.X < 10 && c.X >= 0 && c.Y >= 0 && c.Y < 10);
                Assert.True(allCoordinatesInBounds);
            }

            for (int i = 0; i < 10000; i++)
            {
                var coordinates = CoordinatresGenerator.GenerateShipCoordinates(5);
                var allCoordinatesInBounds = coordinates.All(c => c.X < 10 && c.X >= 0 && c.Y >= 0 && c.Y < 10);
                Assert.True(allCoordinatesInBounds);
            }
        }
    }
}
