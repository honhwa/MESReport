using System.Linq;
using System.Threading.Tasks;
using Innolux.Core.Exception;
using Innolux.Core.Extensions;
using Innolux.Core.Extensions.ApiModel;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innolux.Api.Policy
{
    public class TokenAuthorizationHandler : AuthorizationHandler<TokenRequirement>
    {
        private readonly ApiRedisToken _apiRedisToken;

        private readonly InnoluxEncrypt _es;

        public TokenAuthorizationHandler(ApiRedisToken apiRedisToken)
        {
            _apiRedisToken = apiRedisToken;
            InnoluxEncrypt es = InnoluxEncrypt.GetInstance();
            _es = es;

        }

        public TokenAuthorizationHandler()
        {

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenRequirement requirement)
        {

            //获取当前http请求的context对象
            var mvcContext = context.Resource as DefaultHttpContext;
            //检查Token是否存在，是否匹配
            string token = mvcContext.HttpContext.Request.Headers["token"];
            string emp_id = mvcContext.HttpContext.Request.Headers["emp_id"];

            if (string.IsNullOrEmpty(token))
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.NoToken));
            }
            if (string.IsNullOrEmpty(emp_id))
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.NoToken));
            }
            ////解密
            //var userToken = _es.Decrypto(token);
            //var userId = userToken.Split('@')[0];

            ////验证并且刷新token login time
            //bool flag = _apiRedisToken.CheckLoginExpire(userId);
            //if (flag)
            //{
            //  //設置HttpContext上下文的"User-Id"
            //  mvcContext.HttpContext.Request.Headers["User-Id"] = userId;
            //  //通过验证，这句代码必须要有
            //  context.Succeed(requirement);
            //}
            //else
            //{

            //  throw new InnoluxException(new ErrResult(ApiCodeEnum.LoginExpire));

            //}
            bool check = _apiRedisToken.IsExist(emp_id, token);
            if (!check) throw new InnoluxException(new ErrResult(ApiCodeEnum.InvalidToken, "Token不存在或匹對失敗!"));



            return Task.CompletedTask;
        }
    }
}
