using chapter_10.Engine;
using chapter_10.States;
using System;

namespace chapter_10
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private const int WIDTH = 1280;
        private const int HEIGHT = 720;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame(WIDTH, HEIGHT, new SplashState()))
                game.Run();
        }
    }
#endif
}
