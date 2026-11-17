namespace Innolux.Core.Extensions.ApiModel
{
    public class SamV4LoginSend
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } = "Auto";

    }

    public class SamV4LoginResult
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }

    }

    public class SamV4VerifyTokenSend
    {
        public string Token { get; set; }
        public string SysID { get; set; }

    }
    public class SamV4VerifyTokenResult
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public string MemID { get; set; }
        public string MemName { get; set; }
        public string AD { get; set; }
        public string MemType { get; set; }
        public string SSOToken { get; set; }

    }

    public class SamV4PropertySend
    {
        public string Token { get; set; }
        public string EmpNo { get; set; }

        public string SysID { get; set; }
        public string PropertyID { get; set; }

    }

    public class SamV4PropertyResult
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public List<string> Roles { get; set; }

    }

    public class SamV4MenuSend
    {
        public string SysID { get; set; }
        public string Token { get; set; }

    }

    public class SamV4MenuResult
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public List<SamV4MenuNode> lstNode { get; set; }

    }

    public class SamV4MenuNode
    {
        public string RouteId { get; set; }
        public string RouteParentID { get; set; }
        public string NodeID { get; set; }
        public string FunName { get; set; }
        public string FunName_E { get; set; }
        public string ParentID { get; set; }
        public string FunDesc { get; set; }
        public string FunDesc_E { get; set; }
        public string Version { get; set; }
        public string SortIndex { get; set; }
        public string Url { get; set; }

    }

    public class SamV4MailResult
    {
        public string Message { get; set; }
        public string Code { get; set; }

    }

    public class SamV4MailSend
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
        public string MailBccList { get; set; }
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
