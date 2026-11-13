using Innolux.Core.Extensions;

namespace Innolux.Core.Logger
{
    public class InnoluxLog
    {

        public InnoluxLog()
        {
        }

        public static void Info(string message)
        {
            LogHelper.InfoLog.Info(message);
            LogHelper.GrayLog.Info(message);
        }

        public static void Error(string messager)
        {
            LogHelper.ErrorLog.Error(messager);
            LogHelper.GrayLog.Error(messager);
        }

        public static void Debug(string message)
        {
            LogHelper.InfoLog.Debug(message);
            LogHelper.GrayLog.Debug(message);
        }
    }
}
