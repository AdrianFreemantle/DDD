using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ConsoleWindowLogger : ILog
    {
        private static readonly object Sync = new object();
        private readonly ConsoleColor originalColor = Console.ForegroundColor;

        public ConsoleWindowLogger()
        {
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

                if (values.Length > 0)
                {
                    Console.WriteLine(message, values);
                }
                else
                {
                    Console.WriteLine(message);
                }

                Console.ForegroundColor = this.originalColor;
            }
        }
    }
}
