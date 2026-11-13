using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;



namespace Innolux.Core.Extensions
{

    public class SessionHelper
    {
        private static string SESSION_USER = "SESSION_USER";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;//在属性中用Lambda语法

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 登记User Session
        /// </summary>
        /// <param name="userinfo"></param>
        public void CreateUserSession<T>(T value)
        {
            _session.SetString(SESSION_USER, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 登记User Session
        /// </summary>
        /// <param name="value"></param>
        public void CreateUserSession<T>(T value, string key)
        {
            _session.SetString(key, JsonConvert.SerializeObject(value));
        }
        /// <summary>
        /// 登记User Session
        /// </summary>
        /// <param name="userinfo"></param>
        public T GetUserSession<T>()
        {
            var value = _session.GetString(SESSION_USER);
            if (value == null) throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSession));
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value); ;
        }
        /// <summary>
        /// 登记User Session
        /// </summary>
        /// <param name="userinfo"></param>
        public T GetUserSession<T>(string key)
        {
            var value = _session.GetString(key);
            if (value == null) throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSession));
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value); ;
        }
        public void RemoveSession()
        {
            _session.Remove(SESSION_USER);

        }

        public void RemoveSession(string key)
        {
            _session.Remove(key);

        }

        public void ClearSession()
        {
            _session.Clear();
        }
    }
    public static class SessionExtensions
    {
        private static string SESSION_USER = "SMOOTH_SESSION_USER";
        public static void CreateUserSession(this ISession session, object value)
        {
            session.SetString(SESSION_USER, JsonConvert.SerializeObject(value));
        }
        public static void CreateUserSession(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetUserSession<T>(this ISession session)
        {
            var value = session.GetString(SESSION_USER);
            if (value == null) throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSession));
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static T GetUserSession<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null) throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSession));
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void RemoveSession(this ISession session)
        {
            session.Remove(SESSION_USER);

        }
        public static void RemoveSession(this ISession session, string key)
        {
            session.Remove(key);

        }

        public static void ClearSession(this ISession session)
        {
            session.Clear();
        }
    }


}
