using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Newtonsoft.Json;

namespace Innolux.Core.Extensions.MsgAlarm
{
    public partial class Utils
    {
        /// <summary>
        /// 向企業號發送文本消息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="appName">企業號對應的某一個應用的名稱</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(WXText model, string appName)
        {
            MsgResult result = new();
            string url = $"http://10.189.217.130:8464/api/WeChat/Text/{appName}";
            try
            {
                var reval = new HttpHelpper().Post(url, model).Result;
                if (reval.StatusCode.ToString() == "OK")
                {
                    result.Code = "0";
                }
                else if (reval.StatusCode.ToString() == "BadRequest")
                {
                    result = JsonConvert.DeserializeObject<MsgResult>(reval.Content);
                    result.Code = "2";
                }
                else
                {
                    throw new InnoluxException(new ErrResult(ApiCodeEnum.MAPPSendErrot, "錯誤代碼:" + reval.StatusCode.ToString()));
                }
            }
            catch (System.Exception e)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.MAPPSendErrot, e.Message));
            }
            return result;
        }
        /// <summary>
        /// 向企業號發送文字卡片
        /// </summary>
        /// <param name="model"></param>
        /// <param name="appName">企業號對應的某一個應用的名稱</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(WXTextCard model, string appName)
        {
            MsgResult result = new();
            string url = $"http://10.189.217.130:8464/api/WeChat/TextCard/{appName}";
            try
            {
                var reval = new HttpHelpper().Post(url, model).Result;
                if (reval.StatusCode.ToString() == "OK")
                {
                    result.Code = "0";
                }
                else if (reval.StatusCode.ToString() == "BadRequest")
                {
                    result = JsonConvert.DeserializeObject<MsgResult>(reval.Content);
                    result.Code = "2";
                }
                else
                {
                    throw new InnoluxException(new ErrResult(ApiCodeEnum.MAPPSendErrot, "錯誤代碼:" + reval.StatusCode.ToString()));
                }
            }
            catch (System.Exception e)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.MAPPSendErrot, e.Message));
            }
            return result;
        }
    }
    /// <summary>
    /// <para>微信文本消息發送</para>
    /// <para>發送需要聯繫微信相關的負責人開通跨域權限</para>
    /// </summary>
    public class WXText
    {
        /// <summary>
        /// 指定接收消息的員工，員工工號列表（多个接收者用‘|’分隔，最多支持1000个）。 特殊情况：指定为”@all”，则向该企业应用的全部成员发送
        /// </summary>
        public string? touser { get; set; }
        /// <summary>
        /// 指定接收消息的部门，部门ID列表，多个接收者用‘|’分隔，最多支持100个。 当touser为”@all”时忽略本参数
        /// </summary>
        public string? toparty { get; set; }
        /// <summary>
        /// 指定接收消息的标签，标签ID列表，多个接收者用‘|’分隔，最多支持100个。 当touser为”@all”时忽略本参数
        /// </summary>
        public string? totag { get; set; }
        /// <summary>
        /// 消息的文本內容
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        /// <example>0</example>
        public string safe { get; set; }
    }
    /// <summary>
    /// <para>微信文字卡片消息發送</para>
    /// <para>發送需要聯繫微信相關的負責人開通跨域權限</para>
    /// </summary>
    public class WXTextCard
    {
        /// <summary>
        /// 指定接收消息的員工，員工工號列表（多个接收者用‘|’分隔，最多支持1000个）。 特殊情况：指定为”@all”，则向该企业应用的全部成员发送
        /// </summary>
        public string? touser { get; set; }
        /// <summary>
        /// 指定接收消息的部门，部门ID列表，多个接收者用‘|’分隔，最多支持100个。 当touser为”@all”时忽略本参数
        /// </summary>
        public string? toparty { get; set; }
        /// <summary>
        /// 指定接收消息的标签，标签ID列表，多个接收者用‘|’分隔，最多支持100个。 当touser为”@all”时忽略本参数
        /// </summary>
        public string? totag { get; set; }
        /// <summary>
        /// 文字卡片的內容
        /// </summary>
        public TextCard textcard { get; set; }
    }

    public class TextCard
    {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        public string desctiption { get; set; }
        /// <summary>
        /// 点击后跳转的链接
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 按钮文字。 默认为“详情”， 不超过4个文字，超过自动截断。
        /// </summary>
        public string btntxt { get; set; }
    }
}
