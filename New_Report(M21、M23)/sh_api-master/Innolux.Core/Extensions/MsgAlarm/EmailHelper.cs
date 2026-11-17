using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Newtonsoft.Json;

namespace Innolux.Core.Extensions.MsgAlarm
{
    public partial class Utils
    {
        /// <summary>
        /// 郵件發送
        /// </summary>
        /// <param name="mailto">收件人</param>
        /// <param name="subject">主旨</param>
        /// <param name="body">內容</param>
        /// <param name="mailcc">cc收件人,可以為空</param>
        /// <param name="files">文件,可以為空</param>
        /// <param name="isHtml">是否為html格式,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(string mailto, string subject, string body, string mailcc = "", List<Attachements> files = null, bool isHtml = true)
        {
            MsgResult result = new MsgResult();
            MailContent mailcontent = new MailContent
            {
                MailFrom = MailFrom,
                MailToList = mailto,
                MailCcList = mailcc,
                Subject = subject,
                Body = body,
                IsBodyhtml = isHtml,
                Attachements = files
            };
            try
            {
                string url = "http://pSAMv4SMTPfs.cminl.oa/api/Main/SendMail";
                var reval = new HttpHelpper().Post(url, mailcontent).Result;
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
                    throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4MailSendError, "錯誤代碼:" + reval.StatusCode.ToString()));
                }
            }
            catch (System.Exception e)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4MailSendError, e.Message));
            }
            return result;
        }
        public MsgResult Send(MailContent mailcontent)
        {
            MsgResult result = new MsgResult();
            mailcontent.MailFrom ??= MailFrom;
            try
            {
                string url = "http://pSAMv4SMTPfs.cminl.oa/api/Main/SendMail";
                var reval = new HttpHelpper().Post(url, mailcontent).Result;
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
                    throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4MailSendError, "錯誤代碼:" + reval.StatusCode.ToString()));
                }
            }
            catch (System.Exception e)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4MailSendError, e.Message));
            }
            return result;
        }

    }
    /// <summary>
    /// SAMv4發郵件的類
    /// </summary>
    public class MailContent
    {
        /// <summary>
        /// 寄件者
        /// </summary>
        public string MailFrom { get; set; }
        /// <summary>
        /// 寄件者
        /// </summary>
        public string MailToList { get; set; }
        /// <summary>
        /// 副本
        /// </summary>
        public string MailCcList { get; set; }
        /// <summary>
        /// 密件副本
        /// </summary>
        public string MailBccList { get; set; } = "";
        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 是否為HTML
        /// </summary>
        public bool IsBodyhtml { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Attachements> Attachements { get; set; }
    }
    /// <summary>
    /// 附件
    /// </summary>
    public class Attachements
    {
        /// <summary>
        /// 文件名稱
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件BASE64編碼字符串
        /// </summary>
        public string FileBodyB64 { get; set; }
    }

}
