using System;

namespace Domain.Core.Logging
{
    public static class LogFactory
    {
        /// <summary>
        /// Initializes static members of the LogFactory class.
        /// </summary>
        static LogFactory()
        {
            var logger = new NullLogger();
            BuildLogger = type => logger;
        }

        /// <summary>
        /// Gets or sets the log builder of the configured logger.  This should be invoked to return a new logging instance.
        /// </summary>
        public static Func<Type, ILog> BuildLogger { get; set; }

        private class NullLogger : ILog
        {
            public void Verbose(string message, params object[] values)
            {
            }
            public void Debug(string message, params object[] values)
            {
            }
            public void Info(string message, params object[] values)
            {
            }
            public void Warn(string message, params object[] values)
            {
            }
            public void Error(string message, params object[] values)
            {
            }
            public void Fatal(string message, params object[] values)
            {
            }
        }
    }
}