using Geo;
using Geo.Geometries;

namespace PlatHak.Server.WorldData
{
    public partial class WorldDataManager
    {
        public static partial class Citys
        {
            public static readonly Polygon MelbourneAustralia = new Polygon(
                new Coordinate(-37.4465, 144.4592),
                new Coordinate(-37.5533, 145.5936),
                new Coordinate(-38.3890, 145.3821),
                new Coordinate(-38.2339, 144.2450),
                new Coordinate(-37.4465, 144.4592)
            );
        }
    }
}
