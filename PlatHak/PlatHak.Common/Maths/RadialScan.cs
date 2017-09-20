using System;
using System.Threading.Tasks;

namespace PlatHak.Common.Maths
{
    public static class RadialScan
    {
        public static void AllPoints(VectorLong2 centerPos, int radius, Action<long, long> action)
        {
            for (long x = Math.Max(0,  centerPos.X - radius); x < centerPos.X + radius; x++)
            {
                for (long y = Math.Max(0, centerPos.Y- radius); y < centerPos.Y + radius; y++)
                {
                    if(!IsPointWithinCircle(centerPos, radius, new VectorLong2(x, y))) continue;
                    action.Invoke(x, y);
                }
            }
        }

        public static bool IsPointWithinCircle(VectorLong2 centerPos, int radius, VectorLong2 testPoint)
        {
            return Math.Pow(testPoint.X - centerPos.X, 2) + Math.Pow(testPoint.Y - centerPos.Y, 2) <=
                   Math.Pow(radius, 2);
        }
    }
}