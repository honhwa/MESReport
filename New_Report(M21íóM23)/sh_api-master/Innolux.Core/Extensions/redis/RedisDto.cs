using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Innolux.Core.Extensions.redis
{
    public class RedisDto
    {
        /// <summary>
        /// redis的key
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// redis的value
        /// </summary>
        public string? Value { get; set; }
        private readonly IConfiguration _configuration;
        private readonly IDatabase _db;
        public RedisDto(IConfiguration configuration, IConnectionMultiplexer db)
        {
            _configuration = configuration;
            _db = db.GetDatabase(); ;
        }
        /// <summary>
        /// 在redis創建對應的key-value
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            if (_db.KeyExists(Key)) _db.KeyDelete(Key);
            _db.StringSet(Key, Value);
            int hours = Convert.ToInt32(_configuration.GetSection("LoginExpire:KeySpan").Value);
            _db.KeyExpire(Key, TimeSpan.FromHours(hours));
            return true;
        }
        /// <summary>
        /// 更新key的過期時間
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InnoluxException"></exception>
        public bool UpdateToken()
        {
            int hours = Convert.ToInt32(_configuration.GetSection("LoginExpire:KeySpan").Value);
            if (_db.KeyExists(Key)) _db.KeyExpire(Key, TimeSpan.FromHours(hours));
            else
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.NoToken, "該工號查不到相關的Token!"));
            }
            return true;
        }

    }

    /// <summary>
    /// CUP平臺存儲Redis所用
    /// </summary>
    public class CUP_RedisStorge
    {
        /// <summary>
        /// 登入員工工號,同時也是redis的key
        /// </summary>
        public string? EmpNo { get; set; }
        /// <summary>
        /// SAMv4的token,同時也是redis的value
        /// </summary>
        public string? Token { get; set; }
    }

}
