using System;

namespace Tetris
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Tetris game = new Tetris())
            {
                game.Run();
            }
        }
    }
#endif
}

