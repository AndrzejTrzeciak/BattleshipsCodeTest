using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Model
{
    public class Cell
    {
        public bool IsCloaked { get; set; }
        public bool IsHit { get; set; }
        public bool IsOccupied { get; set; }
        public Coordinates Coordinates { get; set; }
    }
}
