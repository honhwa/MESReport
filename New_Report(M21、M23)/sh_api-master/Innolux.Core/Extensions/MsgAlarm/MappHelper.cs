using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Newtonsoft.Json;

namespace Innolux.Core.Extensions.MsgAlarm
{
    public partial class Utils
    {
        /// <summary>
        /// 交談室文字發送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrd">是否發正式區,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(ChatText model, bool isPrd = true)
        {
            MsgResult result = new();
            string url = isPrd ? "http://10.189.127.62:46012/api/MappTest/ChatText" : "http://10.189.127.62:46012/api/Common/ChatText";
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
        /// 交談室圖片發送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrd">是否發正式區,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(ChatImage model, bool isPrd = true)
        {
            MsgResult result = new();
            string url = isPrd ? "http://10.189.127.62:46012/api/MappTest/ChatImage" : "http://10.189.127.62:46012/api/Common/ChatImage";
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
        /// 交談室Excel發送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrd">是否發正式區,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(ChatExcel model, bool isPrd = true)
        {
            MsgResult result = new();
            string url = isPrd ? "http://10.189.127.62:46012/api/MappTest/ChatExcel" : "http://10.189.127.62:46012/api/Common/ChatExcel";
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
        /// 團隊文字發送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrd">是否發正式區,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(TeamText model, bool isPrd = true)
        {
            MsgResult result = new();
            string url = isPrd ? "http://10.189.127.62:46012/api/MappTest/TeamText" : "http://10.189.127.62:46012/api/Common/TeamText";
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
        /// 團隊圖片發送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrd">是否發正式區,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(TeamImage model, bool isPrd = true)
        {
            MsgResult result = new();
            string url = isPrd ? "http://10.189.127.62:46012/api/MappTest/TeamImage" : "http://10.189.127.62:46012/api/Common/TeamImage";
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
        /// 團隊Excel發送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrd">是否發正式區,默認是</param>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public MsgResult Send(TeamExcel model, bool isPrd = true)
        {
            MsgResult result = new();
            string url = isPrd ? "http://10.189.127.62:46012/api/MappTest/TeamExcel" : "http://10.189.127.62:46012/api/Common/TeamExcel";
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
    /// 交談室文字推送
    /// </summary>
    public class ChatText
    {
        /// <summary>
        /// 交談室號碼
        /// </summary>
        /// <example>117029</example>
        public string ChatSn { get; set; }
        /// <summary>
        /// 發送的內容
        /// </summary>
        /// <example>這是一則測試的消息</example>
        public string Message { get; set; }
    }
    /// <summary>
    /// 交談室圖片推送
    /// </summary>
    public class ChatImage
    {
        /// <summary>
        /// 交談室號碼
        /// </summary>
        /// <example>117029</example>
        public string ChatSn { get; set; }
        /// <summary>
        /// <para>文件的類型</para>
        /// </summary>
        /// <example>png</example>
        public string FileType { get; set; }
        /// <summary>
        /// <para>文件名稱(帶上後綴)</para>
        /// </summary>
        /// <example>TEST.png</example>
        public string FileName { get; set; }
        /// <summary>
        /// 文件的base64轉碼,不需要帶上','及前面的字串
        /// </summary>
        public string FileBase64 { get; set; }
    }
    /// <summary>
    /// 交談室Excel文件推送
    /// </summary>
    public class ChatExcel
    {
        /// <summary>
        /// 交談室號碼
        /// </summary>
        public string ChatSn { get; set; }
        /// <summary>
        /// <para>文件的類型</para>
        /// <para>e.g. xlsx</para>
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// <para>文件名稱(帶上後綴)</para>
        /// <para>e.g. TEST.xlsx</para>
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件的base64轉碼,不需要帶上','及前面的字串
        /// </summary>
        public string FileBase64 { get; set; }
    }
    /// <summary>
    /// 團隊文字推送
    /// </summary>
    public class TeamText
    {
        /// <summary>
        /// 團隊號碼
        /// </summary>
        /// <example>117029</example>
        public string TeamSn { get; set; }
        /// <summary>
        /// 主旨
        /// </summary>
        /// <example>標題</example>
        public string Subject { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        /// <example>這是一則測試的消息</example>
        public string Message { get; set; }
    }
    /// <summary>
    /// 團隊圖片推送
    /// </summary>
    public class TeamImage
    {
        /// <summary>
        /// 團隊號碼
        /// </summary>
        public string TeamSn { get; set; }
        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// <para>文件的類型</para>
        /// <para>e.g. png</para>
        /// </summary>
        /// <example>jpg</example>
        public string FileType { get; set; }
        /// <summary>
        /// <para>文件名稱(帶上後綴)</para>
        /// <para>e.g. TEST.png</para>
        /// </summary>
        /// <example>TEST.jpg</example>
        public string FileName { get; set; }
        /// <summary>
        /// 文件的base64轉碼,不需要帶上','及前面的字串
        /// </summary>
        public string FileBase64 { get; set; }
    }
    /// <summary>
    /// 團隊Excel文件推送
    /// </summary>
    public class TeamExcel
    {
        /// <summary>
        /// 團隊號碼
        /// </summary>
        public string TeamSn { get; set; }
        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// <para>文件的類型</para>
        /// <para>e.g. xlsx</para>
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// <para>文件名稱(帶上後綴)</para>
        /// <para>e.g. TEST.xlsx</para>
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件的base64轉碼,不需要帶上','及前面的字串
        /// </summary>
        public string FileBase64 { get; set; }
    }

}
