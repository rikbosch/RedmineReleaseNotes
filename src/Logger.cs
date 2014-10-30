using System;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseNotesGenerator
{
    internal static class Logger
    {

        public enum LogLevel
        {
            Verbose,

            Info,

            Warn,

            Error
        };

        private static Dictionary<LogLevel, Tuple<ConsoleColor, ConsoleColor>> ColorMapping =
            new Dictionary<LogLevel, Tuple<ConsoleColor, ConsoleColor>>();

        static Logger()
        {
            ColorMapping.Add(LogLevel.Verbose, Tuple.Create(ConsoleColor.Black, ConsoleColor.Gray));
            ColorMapping.Add(LogLevel.Info, Tuple.Create(ConsoleColor.Black, ConsoleColor.White));
            ColorMapping.Add(LogLevel.Warn, Tuple.Create(ConsoleColor.Black, ConsoleColor.Red));
            ColorMapping.Add(LogLevel.Error, Tuple.Create(ConsoleColor.Red, ConsoleColor.White));
        }

        public static void Log(LogLevel level, string format, params object[] parameters)
        {
            string message = format;
            if (parameters.Any())
            {
                message = String.Format(format, parameters);
            }

            var color = ColorMapping[level];
            Console.BackgroundColor = color.Item1;
            Console.ForegroundColor = color.Item2;
            Console.WriteLine("[{0:g}] - {1}", level, message);
            Console.ResetColor();
        }
        public static bool VerboseEnabled { get; set; }

        public static void LogVerbose(string format, params object[] parameters)
        {
            if (VerboseEnabled)
            {
                Log(LogLevel.Verbose, format, parameters);
            }
        }

        public static void LogError(string format, params object[] parameters)
        {
            Log(LogLevel.Error, format, parameters);
        }

        public static void LogException(Exception ex, string format = null, params object[] parameters)
        {
            string message = null;
            if (format != null)
            {
                message = String.Format(format, parameters);
            }
            LogError("{0}\r\n{1}", message, ex.ToString());
        }

        public static void LogInfo(string message, params object[] parameters)
        {
            Log(LogLevel.Info, message, parameters);
        }

        public static void LogWarning(string message, params object[] parameters)
        {
            Log(LogLevel.Warn, message, parameters);
        }
    }
}