using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innolux.Application.Dto;
using Innolux.Application.Domain;
using Innolux.EFCore;
using Innolux.Core.Extensions;
using Innolux.Core.Extensions.EntityFrameworkCore;
using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Text.RegularExpressions;

namespace Innolux.Application.AppService
{
  public class UserAppService : IUserAppService
  {
    private readonly BaseUserInfo _userInfoDomain;
    private readonly IConfiguration _configuration;
    private readonly ApiRedisToken _apiRedisToken;
    private readonly SessionHelper _sessionHelper;

    public UserAppService(IConfiguration configuration, BaseUserInfo userInfoDomain,
                        ApiRedisToken apiRedisToken, SessionHelper sessionHelper)
    {
      _userInfoDomain = userInfoDomain;
      _configuration = configuration;
      _apiRedisToken = apiRedisToken;
      _sessionHelper = sessionHelper;
    }

    #region 查詢登錄用戶信息
    public async Task<UserInfo> GetUserInfo(string token, string empno)
    {
#if DEBUG
      var user = _userInfoDomain.GetUserInfo(_configuration.GetSection("Debug:empno").Value);
      user.Roles = new List<string>() { _configuration.GetSection("Debug:roles").Value };
#else
      var user = _userInfoDomain.GetUserInfo(empno);
      //獲取SAMV4 Property
      SamV4PropertyResult property = await GetSamV4Property(token, empno);
      user.Roles = property.Roles;
#endif
      return user;
    }
    #endregion

    #region SAMV4  登錄
    public async Task<SamV4LoginResult> LoginSamV4(LoginInput input)
    {
      SamV4LoginResult reult = new SamV4LoginResult();
      SamV4LoginSend samLoginSend = new SamV4LoginSend()
      {
        UserID = input.username,
        Password = input.password
      };
      try
      {
        string url = _configuration.GetSection("SamV4ApiUrl:Login").Value;
        var reval = await new HttpHelpper().Post(url, samLoginSend);
        if (reval.StatusCode.ToString() == "OK")
        {
          reult.Code = "0";
          //reult.Token = reval.Content;
          reult.Token = JsonConvert.DeserializeObject<String>(reval.Content);
        }
        else if (reval.StatusCode.ToString() == "Unauthorized")
        {
          reult = JsonConvert.DeserializeObject<SamV4LoginResult>(reval.Content);
          reult.Code = "2";
        }
      }
      catch (Exception e)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4LoginError, e.Message));
      }
      return reult;
    }
    #endregion

    #region SAMV4  TOKEN認證
    public async Task<SamV4VerifyTokenResult> GetSamV4UserInfo(string token)
    {
      SamV4VerifyTokenResult reult = new SamV4VerifyTokenResult();
      SamV4VerifyTokenSend svtSend = new SamV4VerifyTokenSend()
      {
        Token = token
        //Token= "324c849f-9a31-416d-8cde-c87b5d50bf08"
      };
      try
      {
        string url = _configuration.GetSection("SamV4ApiUrl:VerifyToken").Value;
        var reval = await new HttpHelpper().Post(url, svtSend);
        if (reval.StatusCode.ToString() == "OK")
        {
          reult = JsonConvert.DeserializeObject<SamV4VerifyTokenResult>(reval.Content);
          reult.Code = "0";
        }
        else if (reval.StatusCode.ToString() == "Unauthorized")
        {
          reult = JsonConvert.DeserializeObject<SamV4VerifyTokenResult>(reval.Content);
          reult.Code = "2";
        }
      }
      catch (Exception e)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4VerifyTokenError, e.Message));
      }
      return reult;
    }
    #endregion

    #region SAMV4  獲取角色屬性
    public async Task<SamV4PropertyResult> GetSamV4Property(string token, string empno, string propertyid = "")
    {
      SamV4PropertyResult reult = new SamV4PropertyResult();
      try
      {
        string url = _configuration.GetSection("SamV4ApiUrl:GetProperty").Value;
        string sysid = _configuration.GetSection("SysID").Value;

        SamV4PropertySend svtSend = new SamV4PropertySend()
        {
          Token = token,
          EmpNo = empno,
          SysID = sysid,
          PropertyID = "Role"
        };
        var reval = await new HttpHelpper().Post(url, svtSend);
        if (reval.StatusCode.ToString() == "OK")
        {
          reult.Roles = JsonConvert.DeserializeObject<List<String>>(reval.Content);
          reult.Code = "0";
        }
        else if (reval.StatusCode.ToString() == "BadRequest")
        {
          reult = JsonConvert.DeserializeObject<SamV4PropertyResult>(reval.Content);
          reult.Code = "2";
        }
      }
      catch (Exception e)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4GetPropertyError, e.Message));
      }
      return reult;
    }
    #endregion

    #region SAMV4  獲取菜單
    public async Task<List<SamV4MenuNode>> GetSamV4MenuList(string token)
    {
      SamV4MenuResult reult = new SamV4MenuResult();
      try
      {
        string url = _configuration.GetSection("SamV4ApiUrl:GetMenu").Value;
        string sysid = _configuration.GetSection("SysID").Value;
        SamV4MenuSend svmSend = new SamV4MenuSend()
        {
          Token = token,
          SysID = sysid
          //Token= "324c849f-9a31-416d-8cde-c87b5d50bf08"
        };
        var reval = await new HttpHelpper().Post(url, svmSend);
        if (reval.StatusCode.ToString() == "OK")
        {
          reult = JsonConvert.DeserializeObject<SamV4MenuResult>(reval.Content);
          reult.Code = "0";
        }
        else if (reval.StatusCode.ToString() == "BadRequest")
        {
          reult = JsonConvert.DeserializeObject<SamV4MenuResult>(reval.Content);
          reult.Code = "2";
        }
      }
      catch (Exception e)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4GetMenuError, e.Message));
      }
      return reult.lstNode;

    }

    /// <summary>
    /// 把SamV4返回的菜單，轉換成Vue前台要使用的Routes路由表
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<List<GetRoutesOutPut>> GetRoutes(string token)
    {
      List<GetRoutesOutPut> Routelist = new List<GetRoutesOutPut>();
      List<SamV4MenuNode> MenuList = await GetSamV4MenuList(token);
      if (MenuList == null)
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4MenuIsNull));
      try
      {
        string sysid = _configuration.GetSection("SysID").Value;
        if (MenuList != null)
        {
          #region SAMV4菜單轉換成Vue Routes
          //刪除SAMV4的根節點
          var newMenuList = MenuList.Where(p => p.NodeID != sysid && p.Url != "" && p.FunDesc != "" && p.FunDesc_E != "").ToList();

          //把SAMV4節點中的非數字部分去除并增加位數 ,例如："1.基礎資料" => "100" 
          newMenuList.ForEach(item =>
             {
               item.RouteId = Regex.Replace(item.FunName, @"[^\d]", "");
               item.RouteId = item.RouteId.PadRight(3, '0');
             }
          );
          // 生成RouteParentID
          newMenuList.ForEach(father =>
          {
            newMenuList.ForEach(child =>
                      {
                        if (child.ParentID == father.NodeID)
                        {
                          child.RouteParentID = father.RouteId;

                        };
                      });
          }
          );
          //把SAMV4的實體類轉換成 Vue使用的Route實體類
          newMenuList.ForEach(item =>
              {
                GetRoutesOutPut route = new GetRoutesOutPut();
                route.Meta = JsonConvert.DeserializeObject<RoutMetaOutPut>(item.FunDesc_E);
                route.ComponentMap = item.Url;
                route.Name = item.FunDesc;
                route.Id = item.RouteId;
                route.ParentId = item.RouteParentID == null ? "0" : item.RouteParentID;
                Routelist.Add(route);
              }
          );
          #endregion
          //重新按照route Id進行排序
          Routelist = Routelist.OrderBy(x => x.Id).ToList();

        }
      }
      catch (Exception e)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4RoutesError, e.Message));

      }
      return Routelist;

    }
    #endregion

    #region UserInfo  Redis操作
    public UserInfo GetUserSession(string empno)
    {
      ApiRedisToken userToken = _apiRedisToken.GetUserToken(empno);
      UserInfo uif = JsonConvert.DeserializeObject<UserInfo>(userToken.uerInfo);
      if (uif == null)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSession));
      }
      return uif;
    }

    public void SetUserSession(string empno, UserInfo uif)
    {
      ApiRedisToken userToken = _apiRedisToken.GetUserToken(empno);
      userToken.uerInfo = JsonConvert.SerializeObject(uif);
      _apiRedisToken.UpdateUserToken(empno, userToken);
    }

    #endregion

    #region Userinfo Session 操作
    public UserInfo GetUserSession()
    {
      UserInfo uif = _sessionHelper.GetUserSession<UserInfo>();
      return uif;
    }

    public void SetUserSession(UserInfo uif)
    {
      _sessionHelper.CreateUserSession(uif);
    }

    public void ClearUserSession()
    {
      _sessionHelper.ClearSession();
    }

    #endregion
  }
}
