using System.Globalization;
using System;
using Domain.Core.Logging;

namespace Infrastructure
{
    public class ConsoleWindowLogger : ILog
    {
        private const string MessageFormat = "{0} - {1}";
        private static readonly object Sync = new object();
        private readonly ConsoleColor originalColor = Console.ForegroundColor;
        private readonly Type typeToLog;

        public ConsoleWindowLogger(Type typeToLog)
        {
            this.typeToLog = typeToLog;
        }

        public virtual void Verbose(string message, params object[] values)
        {
            this.Log(ConsoleColor.DarkGreen, message, values);
        }

        public virtual void Debug(string message, params object[] values)
        {
            this.Log(ConsoleColor.Green, message, values);
        }

        public virtual void Info(string message, params object[] values)
        {
            this.Log(ConsoleColor.White, message, values);
        }

        public virtual void Warn(string message, params object[] values)
        {
            this.Log(ConsoleColor.Yellow, message, values);
        }

        public virtual void Error(string message, params object[] values)
        {
            this.Log(ConsoleColor.DarkRed, message, values);
        }

        public virtual void Fatal(string message, params object[] values)
        {
            this.Log(ConsoleColor.Red, message, values);
        }

        private void Log(ConsoleColor color, string message, params object[] values)
        {
            lock (Sync)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(FormatMessage(message, typeToLog, values));
                Console.ForegroundColor = originalColor;
            }
        }

        public static string FormatMessage(string message, Type typeToLog, params object[] values)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                MessageFormat,
                typeToLog.FullName,
                string.Format(CultureInfo.InvariantCulture, message, values));
        }
    }
}
