using PlatHak.Client.Common;
using PlatHak.Client.Network;
using PlatHak.Common;
using SharpDX.Mathematics.Interop;

namespace PlatHack.Game
{
    public static class Game
    {
        public static GameConfiguration Configuration { get; set; }
        private static string Username { get; set; }
        private static string Password { get; set; }
        public static MyGame MyGame { get; set; }

        
        
        public static void Start(string server, string username, string password, GameConfiguration config)
        {
            Username = username;
            Password = password;
            Configuration = config;
            MyGame = new MyGame(new WebSocketClientConfig
            {
                ServerAddress =  server,
                Username = username,
                PasswordHash = password
            });
            MyGame.Run(Configuration);
            

        }
    }
}
