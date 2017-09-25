using System;
using System.Runtime.CompilerServices;
using PlatHak.Client.Desktop.Managers;
using PlatHak.Client.Game;

namespace PlatHak.Client.Desktop
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new PlatHakGame(new WindowsInputManager()))
                game.Run();
        }
    }
}
