using System;
using System.Threading;
using static Lucraft.Database.Level;

namespace Lucraft.Database
{
    public static class SimpleLogger
    {
        public static bool Debug { get; set; }

        public static void Log(Level lvl, string s)
        {
            switch (lvl)
            {
                case Info:
                    Console.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/INFO]: {s}");
                    break;
                case Warn:
                    Console.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/WARN]: {s}");
                    break;
                case Level.Debug when Debug:
                    Console.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/DEBUG]: {s}");
                    break;
                case Error:
                    Console.Error.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/ERROR]: {s}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lvl), lvl, null);
            }
        }
    }

    public enum Level
    {
        Info = 0,
        Warn = 1,
        Debug = 2,
        Error = 3
    }
}
