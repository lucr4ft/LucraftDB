using System;
using System.Threading;

namespace Lucraft.Database
{
    public class SimpleLogger
    {

        public static bool Debug = false;

        public static void Log(Level lvl, string s)
        {
            if (lvl == Level.INFO) Console.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/INFO]: {s}");
            else if (lvl == Level.WARN) Console.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/WARN]: {s}");
            else if (lvl == Level.DEBUG && Debug) Console.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/DEBUG]: {s}");
            else if (lvl == Level.ERROR) Console.Error.WriteLine($"[{DateTime.Now}.{DateTime.Now:fff}] [{Thread.CurrentThread.Name}/ERROR]: {s}");
        }

    }

    public enum Level
    {
        INFO = 0,
        WARN = 1,
        DEBUG = 2,
        ERROR = 3
    }
}
