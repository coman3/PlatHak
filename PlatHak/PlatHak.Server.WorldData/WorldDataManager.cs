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
using PlatHak.Common.World;

namespace PlatHak.Server.WorldData
{
    public partial class WorldDataManager
    {
        private const string ConsolePrefix = "[WorldDataManager] ";
        private const string WorldDataTileUrl = @"https://earthexplorer.usgs.gov/download/4220/{0}/STANDARD/EE";
        private const string Cookie = @"EROS_SSO_production_secure=eyJjcmVhdGVkIjoxNTA1ODgzOTczLCJ1cGRhdGVkIjoiMjAxNy0wOS0yMCAwMDowNjoxMiIsImlkIjoiTmdoNWk%2BK1ptOlFMYVwvV14iLCJzZWNyZXQiOiJXZC5xUEd1cmVIa2s%2FJVs6ODt%2BPC4tIiwiYXV0aFR5cGUiOiIiLCJhdXRoU2VydmljZSI6IkVST1MiLCJ2ZXJzaW9uIjoxLjEsInN0YXRlIjoiYjVjMGZhYjkxOTEwMmZjMDQ1MmI0MGJkMGRmY2QxYWMyYjIzM2NhNzY0NTVhNjFkODIyYWQ2NDZjYTBhY2U3NyJ9; EROS_SSO_production=eyJjcmVhdGVkIjoxNTA1ODgzOTczLCJ1cGRhdGVkIjoiMjAxNy0wOS0yMCAwMDowNjoxMiIsImlkIjoiTmdoNWk%2BK1ptOlFMYVwvV14iLCJzZWNyZXQiOiJXZC5xUEd1cmVIa2s%2FJVs6ODt%2BPC4tIiwiYXV0aFR5cGUiOiIiLCJhdXRoU2VydmljZSI6IkVST1MiLCJ2ZXJzaW9uIjoxLjEsInN0YXRlIjoiYjVjMGZhYjkxOTEwMmZjMDQ1MmI0MGJkMGRmY2QxYWMyYjIzM2NhNzY0NTVhNjFkODIyYWQ2NDZjYTBhY2U3NyJ9; PHPSESSID=5fmd6prf538gi4282dttndact3; _ga=GA1.2.1595815959.1505690179; _gid=GA1.2.1765924644.1506325231; _gat_ee=1; _gat_lta=1";
        
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

        public async Task<Dictionary<WorldDataTile, Bitmap>> Generate(WorldDataTile[] worldDataTiles)
        {
            var images = new Dictionary<WorldDataTile, Bitmap>();
            foreach (var tile in worldDataTiles)
            {
                try
                {
                    var image = await Generate(tile);
                    if (image != null) images.Add(tile, image);
                }
                catch (FileLoadException)
                {
                    images.Add(tile, null);
                }
            }
            return images;
        }

        public async Task<Bitmap> Generate(WorldDataTile tile)
        {
            var exportedFilePath = Path.Combine(WorldDataTilePath, tile.EntityId + "_dem.tif");
            if (!File.Exists(exportedFilePath) && !await DownloadDataTile(tile))
                throw new FileLoadException("Failed to download and or find the world tile.");

            return new Bitmap(Image.FromFile(exportedFilePath));
        }
        public async Task DownloadWorldData(WorldDataTile[] tiles)
        {
            foreach (var worldDataTile in tiles)
            {
                await DownloadDataTile(worldDataTile);
            }
        }

        public WorldDataTile[] GetRequiredDataTiles(Polygon polygon)
        {
            return Tiles.Where(x => x.Polygon.GetBounds().Intersects(polygon.GetBounds())).ToArray();
        }
    }
}