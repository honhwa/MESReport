namespace Innolux.Core.Extensions
{
    public class LogHelper
    {
        public static readonly log4net.ILog GrayLog = log4net.LogManager.GetLogger("GrayLoger");
        public static readonly log4net.ILog ErrorLog = log4net.LogManager.GetLogger("ErrorLoger");
        public static readonly log4net.ILog InfoLog = log4net.LogManager.GetLogger("InfoLoger");
        public static readonly log4net.ILog DbLoger = log4net.LogManager.GetLogger("DbLoger");

    }
}
