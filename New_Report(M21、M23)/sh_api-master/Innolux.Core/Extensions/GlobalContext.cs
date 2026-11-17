using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Innolux.Core.Extensions
{
    public static class GlobalContext
    {
        /// <summary>
        /// 注入对象服务提供类
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 手动获取注入的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetGlobalService<T>() where T : class
        {
            return ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext.RequestServices.GetService<T>();
        }

        public static T GetRequiredService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
