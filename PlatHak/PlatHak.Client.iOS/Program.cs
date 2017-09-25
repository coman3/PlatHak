using System;
using Foundation;
using PlatHak.Client.Game;
using UIKit;

namespace PlatHak.Client.iOS
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static PlatHakGame game;

        internal static void RunGame()
        {
            game = new PlatHakGame();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
