using Innolux.Core.Extensions.ApiModel;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Innolux.Core.Logger
{
    /// <summary>
    /// 對自定義的信息進行
    /// </summary>
    public class InnoluxLayout : log4net.Layout.PatternLayout
    {
        /// <summary>
        /// 註冊
        /// </summary>
        public InnoluxLayout()
        {
            this.AddConverter("serverName", typeof(ServerNameConverter));
        }

    }
    /// <summary>
    /// 構造自定義的格式
    /// </summary>
    public class ServerNameConverter : PatternLayoutConverter
    {
        private readonly string serverName = AppConfig.ServerName;
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (!string.IsNullOrEmpty(serverName))
            {
                writer.Write($"{serverName}");
            }
        }
    }
}
