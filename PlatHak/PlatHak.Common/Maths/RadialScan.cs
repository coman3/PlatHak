using System;
using System.Threading.Tasks;

namespace PlatHak.Common.Maths
{
    public static class RadialScan
    {
        public static void AllPoints(VectorInt2 centerPos, int radius, Action<int, int> action)
        {
            for (int x = Math.Max(0,  centerPos.X - radius); x < centerPos.X + radius; x++)
            {
                for (int y = Math.Max(0, centerPos.Y- radius); y < centerPos.Y + radius; y++)
                {
                    if(!IsPointWithinCircle(centerPos, radius, new VectorInt2(x, y))) continue;
                    action.Invoke(x, y);
                }
            }
        }

        public static void AllPointsParrell(VectorInt2 centerPos, int radius, Action<int, int> action)
        {
            Parallel.For(Math.Max(0, centerPos.X - radius), centerPos.X + radius, x =>
            {
                Parallel.For(Math.Max(0, centerPos.Y - radius), centerPos.Y + radius, y =>
                {
                    if (!IsPointWithinCircle(centerPos, radius, new VectorInt2(x, y))) return;
                    action.Invoke(x, y);
                });
            });
        }

        public static bool IsPointWithinCircle(VectorInt2 centerPos, int radius, VectorInt2 testPoint)
        {
            return Math.Pow(testPoint.X - centerPos.X, 2) + Math.Pow(testPoint.Y - centerPos.Y, 2) <=
                   Math.Pow(radius, 2);
        }
    }
}