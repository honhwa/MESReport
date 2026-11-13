using Innolux.Core.Extensions.redis;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Innolux.Core.Extensions.ApiModel
{
    public class ApiRedisToken
    {
        private readonly IConfiguration _configuration;

        private IDatabase _db;
        private InnoluxEncrypt _es;

        public string empno { get; private set; }
        public string loginTime { get; private set; }
        public string freshTime { get; private set; }


        public string token { get; private set; }

        public string uerInfo { get; set; }

        public ApiRedisToken(IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _configuration = configuration;

            _db = redis.GetDatabase();
            InnoluxEncrypt es = InnoluxEncrypt.GetInstance();
            _es = es;

        }

        public ApiRedisToken()
        {
        }
        /// <summary>
        /// 创建userToken 并存储到redis hash表
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        public ApiRedisToken CreateUserToken(string empno, string samToken)
        {
            ApiRedisToken q = new ApiRedisToken();
            q.empno = empno;
            q.loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
            q.freshTime = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm:dd");
            q.token = _es.Encrypto(q.empno + "@" + samToken);
            //q.token = token;
            //--存入redis hash
            _db.HashSet(empno, q.ToHashEntries());
            int hours = Convert.ToInt32(_configuration.GetSection("LoginExpire:KeySpan").Value);
            _db.KeyExpire(empno, TimeSpan.FromHours(hours));
            return q;
        }
        /// <summary>
        /// 更新UserToekn
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        public void UpdateUserToken(string empno, ApiRedisToken token)
        {


            _db.KeyDelete(empno);

            token.loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
            token.freshTime = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm:dd");
            //--存入redis hash
            _db.HashSet(empno, token.ToHashEntries());
            int hours = Convert.ToInt32(_configuration.GetSection("LoginExpire:KeySpan").Value);
            _db.KeyExpire(empno, TimeSpan.FromHours(hours));

        }

        /// <summary>
        /// 获取userToken
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        public ApiRedisToken GetUserToken(string empno)
        {
            //--获取key
            HashEntry[] entries = _db.HashGetAll(empno);

            var token = entries.ConvertFromRedis<ApiRedisToken>();
            return token;
        }

        /// <summary>
        /// 检查登陆过期 false:过期 true:未过期
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        public bool CheckLoginExpire(string empno)
        {
            var token = this.GetUserToken(empno);

            if (token.empno == null)
            {
                return false;
            }
            //先检查loginTime是否过期
            int hours = Convert.ToInt32(_configuration.GetSection("LoginExpire:LoginSpan").Value);
            DateTime now = DateTime.Now;
            DateTime logintime = Convert.ToDateTime(token.loginTime);

            TimeSpan loginspan = now - logintime;


            double loginHours = loginspan.TotalHours;



            if (loginHours > hours)
            {
                //登陆过期直接删除
                _db.KeyDelete(empno);
                return false;
            }
            else
            {
                this.UpdateUserToken(empno, token);
                return true;
            }

        }


        public void DeleteUserToken(string empno)
        {
            _db.KeyDelete(empno);
        }


        public string GetSamV4SSOToken(string token)
        {
            var Xtoken = _es.Decrypto(token);
            var samV4Token = Xtoken.Split('@')[1];
            return samV4Token;
        }

        public string GetUserInfo(string empno)
        {
            ApiRedisToken userToken = GetUserToken(empno);
            return userToken.uerInfo;

        }

        public bool IsExist(string key, string value)
        {
            if (!_db.KeyExists(key)) return false;
            string _token = _db.StringGet(key);
            if (_token != value) return false;
            return true;
        }
    }
}
