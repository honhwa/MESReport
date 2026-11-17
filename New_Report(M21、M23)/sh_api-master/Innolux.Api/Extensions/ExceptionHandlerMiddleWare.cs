using System.Threading.Tasks;
using Innolux.Core.Exception;
using Innolux.Core.Extensions;
using log4net;
using Newtonsoft.Json;

namespace Innolux.Api
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            bool isCatched = false;
            try
            {
                ThreadContext.Properties["Route"] = context.Request.Path;
                await _next(context);
            }
            catch (System.Exception ex) //发生异常
            {
                //自定义业务异常
                if (ex is InnoluxException)
                {
                    await HandleInnoluxExceptionAsync(context, ex.Message);

                    LogHelper.ErrorLog.Error("InnoluxException  " + ex.Message, ex);
                }
                //未知异常
                else
                {
                    context.Response.StatusCode = 500;

                    await HandleExceptionAsync(context, context.Response.StatusCode, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message);
                    //记录异常

                    LogHelper.ErrorLog.Error("UnknownException  " + ex.Message, ex);

                }

                isCatched = true;
            }
            finally
            {
                if (!isCatched && context.Response.StatusCode != 200)//未捕捉过并且状态码不为200
                {
                    string msg = "";
                    switch (context.Response.StatusCode)
                    {
                        case 401:
                            msg = "未授权";
                            break;
                        case 404:
                            msg = "未找到服务";
                            break;
                        case 502:
                            msg = "请求错误";
                            break;
                        case 400:
                            msg = "坏请求";
                            break;
                        default:
                            msg = "未知错误";
                            break;
                    }
                    await HandleExceptionAsync(context, context.Response.StatusCode, msg);

                    LogHelper.ErrorLog.Error("NoCatchException  " + msg);

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static async Task HandleExceptionAsync(HttpContext context, int code, string msg, string innerMsg = "")
        {
            var data = new { code, success = false, message = msg + "\r\n" + innerMsg };
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(data));

        }

        private static async Task HandleInnoluxExceptionAsync(HttpContext context, string msg)
        {
            //var err = JsonConvert.DeserializeObject<ErrResult>(msg);
            context.Response.ContentType = "application/json;charset=utf-8";
            // context.Response.StatusCode = err.code;
            await context.Response.WriteAsync(msg);
        }
    }
}

