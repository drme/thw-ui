using System;

namespace ThW.UI.Demo
{
#if WINDOWS || XBOX
    public static class StartUp
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            using (XNADemo game = new XNADemo())
            {
                game.Run();
           }
        }
    }
#endif
}
