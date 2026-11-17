using System.ComponentModel.DataAnnotations;

namespace Innolux.Core.Extensions.ApiModel
{
    public enum ApiCodeEnum
    {
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        [Display(Name = "服务器内部错误")]
        Error = 500,

        /// <summary>
        /// 文件不存在
        /// </summary>
        [Display(Name = "文件不存在")]
        NotFound = 404,


        /// <summary>
        /// 操作成功
        ///</summary>
        [Display(Name = "操作成功")]
        Ok = 200,

        /// <summary>
        /// 操作失败
        ///</summary>
        [Display(Name = "操作失败")]
        Fail = 1000,

        /// <summary>
        /// Token缺失
        /// </summary>
        [Display(Name = "Token缺失")]
        NoToken = 1001,

        /// <summary>
        /// 无效Token
        /// </summary>
        [Display(Name = "无效Token")]
        InvalidToken = 1002,


        /// <summary>
        /// 登录过期
        ///</summary>
        [Display(Name = "登录过期")]
        LoginExpire = 1003,

        /// <summary>
        /// 用户不存在
        ///</summary>
        [Display(Name = "用户不存在")]
        NoSuchUser = 1004,

        /// <summary>
        /// 未登录
        ///</summary>
        [Display(Name = "未登录")]
        Unauthorized = 1005,

        /// <summary>
        /// 未授权
        /// </summary>
        [Display(Name = "未授权")]
        Forbidden = 1006,

        /// <summary>
        /// 参数验证失败
        /// </summary>
        [Display(Name = "参数验证失败")]
        InvalidData = 1007,

        /// <summary>
        /// 登录异常
        ///</summary>
        [Display(Name = "登录异常")]
        LoginError = 1008,

        /// <summary>
        /// 登录失败
        ///</summary>
        [Display(Name = "登录失败")]
        LoginFail = 1009,

        /// <summary>
        /// 没有数据
        /// </summary>
        [Display(Name = "没有数据")]
        NoRecord = 1010,

        /// <summary>
        /// Session不存在
        /// </summary>
        [Display(Name = "Session不存在")]
        NoSession = 1011,

        /// <summary>
        /// Session超時
        /// </summary>
        [Display(Name = "Session超時")]
        SessionTimeOut = 1012,

        /// <summary>
        /// SAMV4請求登錄異常
        /// </summary>
        [Display(Name = "SAMV4請求登錄異常")]
        Samv4LoginError = 2001,

        /// <summary>
        /// SAMV4請求TOKEN認證異常
        /// </summary>
        [Display(Name = "SAMV4請求TOKEN認證異常")]
        Samv4VerifyTokenError = 2002,

        /// <summary>
        /// SAMV4請求Property異常
        /// </summary>
        [Display(Name = "SAMV4請求Property異常")]
        Samv4GetPropertyError = 2003,

        /// <summary>
        /// SAMV4請求菜單異常
        /// </summary>
        [Display(Name = "SAMV4請求菜單異常")]
        Samv4GetMenuError = 2004,

        /// <summary>
        /// SAMV4菜單為NULL
        /// </summary>
        [Display(Name = "SAMV4菜單為NULL")]
        Samv4MenuIsNull = 2005,

        /// <summary>
        /// SAMV4菜單轉換ROUTES失敗
        /// </summary>
        [Display(Name = "SAMV4轉換ROUTES失敗")]
        Samv4RoutesError = 2006,

        /// <summary>
        /// SAMV4菜單Mail發送失敗
        /// </summary>
        [Display(Name = "SAMV4菜單Mail發送失敗")]
        Samv4MailSendError = 2007,

        /// <summary>
        /// MAPP消息發送失敗
        /// </summary>
        [Display(Name = "MAPP 發送失敗")]
        MAPPSendErrot = 2008,
    }
}
