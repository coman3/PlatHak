using System;
using Geo;
using Geo.Geometries;

namespace PlatHak.Server.WorldData
{
    public class WorldDataTile
    {
        public string EntityId;
        public string Agency;
        public string AcquisitionDate;
        public string Vendor;
        public string MapProjection;
        public string Sensor;
        public string Resolution;
        public string FileSize;
        public string SensorType;
        public string Ellipsoid;
        public string Units;
        public string Version;
        public string ProductFormat;
        public string CheckSumValue;
        public string LicenseId;
        public string LicenseUpliftUpdate;
        public string DateEntered;
        public string DateUpdated;
        public string CenterLatitude;
        public string CenterLongitude;
        public string NwCornerLat;
        public string NwCornerLong;
        public string NeCornerLat;
        public string NeCornerLong;
        public string SeCornerLat;
        public string SeCornerLong;
        public string SwCornerLat;
        public string SwCornerLong;
        public double CenterLatitudedec;
        public double CenterLongitudedec;
        public double NwCornerLatdec;
        public double NwCornerLongdec;
        public double NeCornerLatdec;
        public double NeCornerLongdec;
        public double SeCornerLatdec;
        public double SeCornerLongdec;
        public double SwCornerLatdec;
        public double SwCornerLongdec;

        public bool Loaded { get; set; }

        public Polygon Polygon =>
            new Polygon(new Coordinate(NwCornerLatdec, NwCornerLongdec), new Coordinate(NeCornerLatdec, NeCornerLongdec),
                    new Coordinate(SeCornerLatdec, SeCornerLongdec), new Coordinate(SwCornerLatdec, SwCornerLongdec), new Coordinate(NwCornerLatdec, NwCornerLongdec));

    public string DisplayId;
        public string OrderingId;
        public string BrowseLink;

        public WorldDataTile(string[] values)
        {
            EntityId = values[0];
            Agency = values[1];
            AcquisitionDate = values[2];
            Vendor = values[3];
            MapProjection = values[4];
            Sensor = values[5];
            Resolution = values[6];
            FileSize = values[7];
            SensorType = values[8];
            Ellipsoid = values[9];
            Units = values[10];
            Version = values[11];
            ProductFormat = values[12];
            CheckSumValue = values[13];
            LicenseId = values[14];
            LicenseUpliftUpdate = values[15];
            DateEntered = values[16];
            DateUpdated = values[17];
            CenterLatitude = values[18];
            CenterLongitude = values[19];
            NwCornerLat = values[20];
            NwCornerLong = values[21];
            NeCornerLat = values[22];
            NeCornerLong = values[23];
            SeCornerLat = values[24];
            SeCornerLong = values[25];
            SwCornerLat = values[26];
            SwCornerLong = values[27];
            CenterLatitudedec = Convert.ToDouble(values[28]);
            CenterLongitudedec = Math.Max(-179.999999, Math.Min(179.999999, Convert.ToDouble(values[29])));
            NwCornerLatdec = Convert.ToDouble(values[30]);
            NwCornerLongdec = Math.Max(-179.999999, Math.Min(179.999999, Convert.ToDouble(values[31])));
            NeCornerLatdec = Convert.ToDouble(values[32]);
            NeCornerLongdec = Math.Max(-179.999999, Math.Min(179.999999, Convert.ToDouble(values[33])));
            SeCornerLatdec = Convert.ToDouble(values[34]);
            SeCornerLongdec = Math.Max(-179.999999, Math.Min(179.999999, Convert.ToDouble(values[35])));
            SwCornerLatdec = Convert.ToDouble(values[36]);
            SwCornerLongdec = Math.Max(-179.999999, Math.Min(179.999999, Convert.ToDouble(values[37])));
            DisplayId = values[38];
            OrderingId = values[39];
            BrowseLink = values[40];
        }
    }
}