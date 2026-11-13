using Newtonsoft.Json;
using RestSharp;

namespace Innolux.Core.Extensions
{
    public class HttpHelpper<T> where T : new()
    {

        public async Task<T> Get(string url, object pars)
        {
            var type = Method.Get;
            RestResponse<T> reval = await GetApiInfo(url, pars, type);
            return reval.Data;
        }
        /// <summary>
        /// 当flag为 false:调用以前框架的API的模式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pars"></param>
        /// <param name="flag">false:调用以前框架的API的模式</param>
        /// <returns></returns>
        public async Task<T> Post(string url, object pars, bool flag = true)
        {
            var type = Method.Post;
            if (flag)
            {
                var reval = await GetApiInfo(url, pars, type);
                return reval.Data;
            }
            else
            {
                T reval = await GetApiInfoBase64(url, pars, type);
                return reval;
            }


        }

        private static async Task<RestResponse<T>> GetApiInfo(string url, object pars, Method type)
        {
            var request = new RestRequest("", type);
            if (pars != null)
                request.AddJsonBody(pars);
            var client = new RestClient(url);
            // client.CookieContainer = new System.Net.CookieContainer();
            var reval = await client.ExecuteAsync<T>(request);

            if (reval.ErrorException != null)
            {
                throw new System.Exception("请求出错");
            }
            return reval;
        }

        private static async Task<T> GetApiInfoBase64(string url, object pars, Method type)
        {
            var request = new RestRequest("", type);
            if (pars != null)
            {
                pars = CommonUtils.EncodeBase64("utf-8", JsonConvert.SerializeObject(pars));
                request.AddParameter("", pars, ParameterType.RequestBody);
            }
            var client = new RestClient(url);
            // client.CookieContainer = new System.Net.CookieContainer();
            var reval = await client.ExecuteAsync(request);
            if (reval.ErrorException != null)
            {
                throw new System.Exception("请求出错");
            }
            T t = new T();
            t = JsonConvert.DeserializeObject<T>(reval.Content);
            return t;
        }


    }

    public class HttpHelpper
    {
        public async Task<RestResponse> Post(string url, object pars)
        {
            var type = Method.Post;
            RestResponse reval = await GetApiInfo(url, pars, type);
            return reval;
        }

        private static async Task<RestResponse> GetApiInfo(string url, object pars, Method type)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", type);
            request.AddHeader("Content-Type", "application/json");
            if (pars != null)
            {
                var body = JsonConvert.SerializeObject(pars);
                request.AddStringBody(body, DataFormat.Json);
            }
            // var request = new RestRequest("", type);
            // if (pars != null)
            // {
            //   // request.AddBody(pars);
            //   string t = JsonConvert.SerializeObject(pars);
            //   request.AddParameter("application/json", t, ParameterType.RequestBody);
            // }
            // var client = new RestClient(url);
            // client.CookieContainer = new System.Net.CookieContainer();
            RestResponse reval = await client.ExecuteAsync(request);
            if (reval.ErrorException != null)
            {
                throw new System.Exception("请求出错:" + url);
            }
            return reval;
        }
    }
}
