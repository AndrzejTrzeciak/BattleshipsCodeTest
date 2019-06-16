namespace AppCore.Model
{
    public class Cell
    {
        public bool IsCloaked { get; set; }
        public bool IsHit { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsSinked { get; set; }
        public Coordinates Coordinates { get; set; }
    }
}
