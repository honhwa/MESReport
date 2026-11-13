declare namespace API {
  /** 登录参数 */
  type LoginParams = {
    UserID: string;
    Password: string;
    UserType:string;
    SysID:string;
  };
  /**验证token的参数 */
  type VerifyParams = {
    Token: string;
  };
  /**获取菜单参数 */
  type GetMenuParams = {
    // Token?: string;
    SYSID: string;
    EMP_ID?:string
  };
  type GetRoleListParams = {
    apiName:string;
  }
  /** 登录成功结果 */
  type LoginResult = {
    token: string;
  };

  /** 获取人员信息的结果 */
  type VerifyTokenResult = {
    MemID: string;
    MemName: string;
    AD: string;
    MemType: string;
    SSOToken: string;
    RemoteIP: string;
    RealUser: string;
    SYSID: string;
  };

  /** 获取验证码参数 */
  type CaptchaParams = {
    width?: number;
    height?: number;
  };

  /** 获取验证码结果 */
  type CaptchaResult = {
    img: string;
    id: string;
  };
}
