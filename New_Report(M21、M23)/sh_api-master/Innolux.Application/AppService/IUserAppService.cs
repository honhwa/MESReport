using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innolux.Application.Domain;
using Innolux.Application.Dto;
using Innolux.Core.Extensions.ApiModel;


namespace Innolux.Application.AppService
{
  public interface IUserAppService
  {
    Task<UserInfo> GetUserInfo(string xtoken, string empno);
    Task<SamV4LoginResult> LoginSamV4(LoginInput input);
    Task<SamV4VerifyTokenResult> GetSamV4UserInfo(string token);

    Task<SamV4PropertyResult> GetSamV4Property(string token, string empno, string propertyid);
    Task<List<SamV4MenuNode>> GetSamV4MenuList(string token);

    Task<List<GetRoutesOutPut>> GetRoutes(string token);
    UserInfo GetUserSession(string empno);
    UserInfo GetUserSession();
    void SetUserSession(UserInfo uif);
    void SetUserSession(string empno, UserInfo uif);
    void ClearUserSession();
  }
}
