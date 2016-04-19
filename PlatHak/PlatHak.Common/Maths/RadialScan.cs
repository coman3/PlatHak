using System;
using System.Threading.Tasks;

namespace PlatHak.Common.Maths
{
    public static class RadialScan
    {
        public static void AllPoints(VectorInt2 centerPos, int radius, Action<int, int> action)
        {
            for (int x = centerPos.X - radius; x < centerPos.X + radius; x++)
            {
                for (int y = centerPos.Y - radius; y < centerPos.Y + radius; y++)
                {
                    if(!IsPointWithinCircle(centerPos, radius, new VectorInt2(x, y))) continue;
                    action.Invoke(x, y);
                }
            }
        }

        public static void AllPointsParrell(VectorInt2 centerPos, int radius, Action<int, int> action)
        {
            Parallel.For(centerPos.X - radius, centerPos.X + radius, x =>
            {
                Parallel.For(centerPos.Y - radius, centerPos.Y + radius, y =>
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