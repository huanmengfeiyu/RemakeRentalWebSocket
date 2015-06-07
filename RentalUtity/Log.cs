using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class Log
    {
        private static readonly log4net.ILog InfoLogger = log4net.LogManager.GetLogger("infoAppender");
        private static readonly log4net.ILog ErrorLogger = log4net.LogManager.GetLogger("errorAppender");
        private static readonly log4net.ILog WarnLogger = log4net.LogManager.GetLogger("warnAppender");
        private static readonly log4net.ILog DebugLogger = log4net.LogManager.GetLogger("debugAppender");
        private static readonly bool isErrorEnabled = ErrorLogger.IsErrorEnabled;
        private static readonly bool isInfoEnabled = InfoLogger.IsInfoEnabled;
        private static readonly bool isWarnEnabled = WarnLogger.IsWarnEnabled;
        private static readonly bool isDebugEnabled = DebugLogger.IsDebugEnabled;
        public static void Warn(string msg)
        {
            if(isWarnEnabled)
            {
                WarnLogger.Warn(msg);
            }
        }
        public static void Info(string msg)
        {
            if(isInfoEnabled)
            {
                InfoLogger.Info(msg);
            }
        }
        public static void Error(string msg)
        {
            if(isErrorEnabled)
            {
                ErrorLogger.Error(msg);
            }
        }
        public static void Debug(string msg)
        {
            if(isDebugEnabled)
            {
                DebugLogger.Debug(msg);
            }
        }
    }
}
