using System;
using System.IO;

namespace TestsWpf
{
    public static class Log
    {
        public static string FilePath { get; } = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
        public static string Name { get; set; }
        public static void Write(string Content)
        {
            File.AppendAllText(FilePath, $"[{DateTime.Now:dd:MM:yyyy HH:mm:ss}] <{Name}> {Content}\n");
        }
    }
}
