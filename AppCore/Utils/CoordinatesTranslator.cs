using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Model
{

    public static class CoordinatesTranslator
    {
        private static Dictionary<int, string> mapTostring = new Dictionary<int, string>()
        {
            {1, "A"},
            {2, "B"},
            {3, "C"},
            {4, "D"},
            {5, "E"},
            {6, "F"},
            {7, "G"},
            {8, "H"},
            {9, "I"},
            {10, "J"}
        };

        private static Dictionary<string, int> mapToDigit = new Dictionary<string, int>
        {
            {"A", 1},
            {"B", 2},
            {"C", 3},
            {"D", 4},
            {"E", 5},
            {"F", 6},
            {"G", 7},
            {"H", 8},
            {"I", 9},
            {"J", 10}
        };

        /// <summary>
        /// maps coordinates according to the given examples
        /// {X = 0; Y = 0} -> A1
        /// {X = 0; Y = 1} -> B1
        /// {X = 0; Y = 9} -> J1
        /// {X = 1; Y = 0} -> A2
        /// {X = 9; Y = 0} -> A10
        /// {X = 3; Y = 5} -> E4
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static string ToFriendlyForm(this Coordinates coordinates)
        {
            if (coordinates.Y < 0 || coordinates.Y > 9)
                throw new ApplicationException("Invalid vertical coordinate. Recieved coordinate: " + coordinates.Y);
            if (coordinates.X < 0 || coordinates.Y > 9)
                throw new ApplicationException("Invalid horizontal coordinate. Recieved coordinate: " + coordinates.X);

            return mapTostring[coordinates.Y] + (coordinates.X + 1).ToString();
        }

        /// <summary>
        /// /// maps coordinates according to the given examples
        /// a1 || A1 -> {X = 0; Y = 0}
        /// b1 || B1 -> {X = 0; Y = 1}
        /// j1 || J1 -> {X = 0; Y = 9}
        /// a2 || A2 -> {X = 1; Y = 0}
        /// a10 || A10 -> {X = 9; Y = 0}
        /// e4 || E4 -> {X = 3; Y = 5}
        /// <param name="stringCoordinates"></param>
        /// <returns></returns>
        public static Coordinates FromStringCoordinates(this string stringCoordinates)
        {
            //TODO: ograć regexem walidacje
            if (stringCoordinates.Length > 3)
                throw new ApplicationException("Invalid coordinates length");
            var y = mapToDigit[stringCoordinates.Substring(0, 1).ToUpper()];
            short x = (short) (short.Parse(stringCoordinates.Substring(1)) - 1);
            return new Coordinates
            {
                X = x, Y = y
            };
        }
    }
}
