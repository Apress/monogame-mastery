using System;

namespace Chapter3
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }
    }
#endif
}