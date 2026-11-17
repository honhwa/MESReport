using Microsoft.Extensions.Configuration;

namespace Innolux.Core.Extensions.MsgAlarm
{
    public partial class Utils
    {
        private readonly bool IsPrd;
        private readonly string MailFrom;
        private readonly IConfiguration _config;
        public Utils(IConfiguration configuration)
        {
            _config = configuration;
            MailFrom = configuration.GetSection("SAMv4:From").Value;
        }
        public class MsgResult
        {
            public string Message { get; set; }
            public string Code { get; set; }

        }
    }
}
