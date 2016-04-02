namespace PlatHak.Common.Maths
{
    public struct BoundingBox
    {
        public static readonly BoundingBox Zero = new BoundingBox(new Vector2(0, 0), new Vector2(0, 0));

        public Vector2 Min { get; set; }
        public Vector2 Max { get; set; }

        public BoundingBox(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        } 
    }
}