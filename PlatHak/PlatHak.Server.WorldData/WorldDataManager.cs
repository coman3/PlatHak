using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Geo;
using Geo.Geometries;
using ICSharpCode.SharpZipLib.Zip;

namespace PlatHak.Server.WorldData
{
    public partial class WorldDataManager
    {
        private const string ConsolePrefix = "[WorldDataManager] ";
        private const string WorldDataTileUrl = @"https://earthexplorer.usgs.gov/download/4220/{0}/STANDARD/EE";
        private const string Cookie = @"_gat_ee=1; _gat_lta=1; ee-system-notices=%5B%226021%22%5D; EROS_SSO_production_secure=eyJjcmVhdGVkIjoxNTA1ODgzOTczLCJ1cGRhdGVkIjoiMjAxNy0wOS0yMCAwMDowNjoxMiIsImlkIjoiTmdoNWk%2BK1ptOlFMYVwvV14iLCJzZWNyZXQiOiJXZC5xUEd1cmVIa2s%2FJVs6ODt%2BPC4tIiwiYXV0aFR5cGUiOiIiLCJhdXRoU2VydmljZSI6IkVST1MiLCJ2ZXJzaW9uIjoxLjEsInN0YXRlIjoiYjVjMGZhYjkxOTEwMmZjMDQ1MmI0MGJkMGRmY2QxYWMyYjIzM2NhNzY0NTVhNjFkODIyYWQ2NDZjYTBhY2U3NyJ9; EROS_SSO_production=eyJjcmVhdGVkIjoxNTA1ODgzOTczLCJ1cGRhdGVkIjoiMjAxNy0wOS0yMCAwMDowNjoxMiIsImlkIjoiTmdoNWk%2BK1ptOlFMYVwvV14iLCJzZWNyZXQiOiJXZC5xUEd1cmVIa2s%2FJVs6ODt%2BPC4tIiwiYXV0aFR5cGUiOiIiLCJhdXRoU2VydmljZSI6IkVST1MiLCJ2ZXJzaW9uIjoxLjEsInN0YXRlIjoiYjVjMGZhYjkxOTEwMmZjMDQ1MmI0MGJkMGRmY2QxYWMyYjIzM2NhNzY0NTVhNjFkODIyYWQ2NDZjYTBhY2U3NyJ9; PHPSESSID=2epjtuv9bq395k72ma1je43dt7; _ga=GA1.2.1595815959.1505690179; _gid=GA1.2.854775227.1505883956";
        
        public List<WorldDataTile> Tiles { get; set; }

        public string DataPath => Path.Combine(Environment.CurrentDirectory, "Data", "ASTER_GLOBAL_DEM_DE_206008.csv");
        public string WorldDataTilePath { get; set; }
        private HttpClient HttpClient { get; }

        public WorldDataManager(string worldDataTilePath)
        {
            if (!Directory.Exists(worldDataTilePath))
                Directory.CreateDirectory(worldDataTilePath);
             WorldDataTilePath = worldDataTilePath;

            var cookieContainer = new CookieContainer();
            cookieContainer.SetCookies(new Uri("https://earthexplorer.usgs.gov/"), Cookie);
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            HttpClient = new HttpClient(handler);
        }

        public async Task LoadFile()
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                Tiles = new List<WorldDataTile>();
                if (!File.Exists(DataPath))
                    throw new FileNotFoundException("'" + DataPath + "' Not Found!");
                using (var file = File.OpenRead(DataPath))
                using (var reader = new StreamReader(file))
                {
                    await reader.ReadLineAsync(); //skip first line
                    while (!reader.EndOfStream)
                    {
                        var currentLine = await reader.ReadLineAsync();
                        if (currentLine == null)
                            continue;
                        var currentLineData = currentLine.Split(',');
                        if (currentLineData.Length <= 0)
                            continue;
                        var entity = new WorldDataTile(currentLineData);
                        Tiles.Add(entity);
                    }
                }
                stopWatch.Stop();
                Console.WriteLine(ConsolePrefix + $"Loaded World Data. (Took: {stopWatch.Elapsed})");
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                Console.WriteLine(ConsolePrefix + $"Failed to load World Data: {e.Message}");
            }
            
        }

        public async Task<bool> DownloadDataTile(WorldDataTile tile)
        {

            var localTempFilePath = Path.Combine(WorldDataTilePath, tile.EntityId + ".zip");
            var localOutFilePath = Path.Combine(WorldDataTilePath, tile.EntityId + "_dem.tif");
            var externalFilePath = string.Format(WorldDataTileUrl, tile.EntityId);

            if(File.Exists(localOutFilePath))
                //file already downloaded
                return true;

            try
            {
                var streamTask = await HttpClient.PostAsync(externalFilePath,
                    new FormUrlEncodedContent(new Dictionary<string, string> {["licenseCode"] = "ASTER_DEM_CO"}));

                //using(var data = await httpClient.GetStreamAsync(externalFilePath))
                Console.WriteLine(ConsolePrefix + $"Downloading Tile Data: {tile.EntityId}");

                if (!streamTask.IsSuccessStatusCode)
                {
                    return false;
                }
                using (var zipData = await streamTask.Content.ReadAsStreamAsync())
                using (var file = File.OpenWrite(localTempFilePath))
                {
                    await zipData.CopyToAsync(file);
                }
                using (var file = File.OpenRead(localTempFilePath))
                using (var zip = new ZipInputStream(file))
                {
                    ZipEntry theEntry;
                    while ((theEntry = zip.GetNextEntry()) != null)
                    {
                        if (!theEntry.Name.EndsWith("_dem.tif")) continue;

                        using (var streamWriter = File.Create(localOutFilePath))
                        {
                            var data = new byte[2048];
                            while (true)
                            {
                                var size = zip.Read(data, 0, data.Length);
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else break;
                            }
                        }
                    }
                }
                File.Delete(localTempFilePath);

                Console.WriteLine(ConsolePrefix + $"Extracted Tile Data: {tile.EntityId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ConsolePrefix + $"Failed to download: {tile.EntityId}");
            }
            return false;
        }

        public async Task<Dictionary<Coordinate, Bitmap>> Load(Polygon polygon)
        {
            var images = new Dictionary<Coordinate, Bitmap>();
            foreach (var outline in polygon.Shell.Coordinates)
            {
                var image = await Load(outline);
                if (image != null) images.Add(outline, image);
            }
            return images;
        }

        public async Task<Bitmap> Load(Coordinate cord)
        {
            var tile = Tiles.FirstOrDefault(x => !x.Loaded && x.Polygon.GetBounds().Contains(cord));
            if (tile == null)
                return null;
            var exportedFilePath = Path.Combine(WorldDataTilePath, tile.EntityId + "_dem.tif");
            if (!File.Exists(exportedFilePath) && !await DownloadDataTile(tile))
                return null;
                tile.Loaded = true;
            return new Bitmap(Image.FromFile(exportedFilePath));
        }
        public async Task Preload(Polygon polygon)
        {
            var bounds = polygon.GetBounds();
            var requiredTiles = Tiles.Where(x => x.Polygon.GetBounds().Intersects(bounds)).ToList();
            foreach (var worldDataTile in requiredTiles)
            {
                await DownloadDataTile(worldDataTile);
            }
        }
       
    }
}