using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Innolux.Application.Dto;
using Innolux.Application.Domain;
using Innolux.EFCore;
using Innolux.Core.Extensions;
using Innolux.Core.Extensions.EntityFrameworkCore;
using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Newtonsoft.Json;


namespace Innolux.Application.AppService
{
  public class CommonAppService : ICommonAppService
  {
    private readonly IConfiguration _configuration;
    private readonly MSSqlDbContext _mssql; //SmoothHr

    public CommonAppService(IConfiguration configuration, MSSqlDbContext mssql)
    {
      _mssql = mssql;
      _configuration = configuration;
    }

    #region  SAMV4發郵件
    public async Task<SamV4MailResult> SendSamV4Mail(string mailto, string subject, string body, string mailcc = "", List<Attachements> files = null)
    {
      if (string.IsNullOrWhiteSpace(mailto))
        throw new Exception("mailto 不能為空！");

      SamV4MailResult reult = new SamV4MailResult();
      SamV4MailSend svtSend = new SamV4MailSend()
      {
        MailFrom = _configuration.GetSection("Mail:MailFrom").Value,
        MailToList = mailto,
        MailCcList = mailcc,
        Subject = subject,
        Body = body,
        IsBodyhtml = true,
      };
      if (files == null)
      {
        svtSend.Attachements = new List<Attachements>();
      }
      try
      {
        string url = _configuration.GetSection("SamV4ApiUrl:SendMail").Value;
        var reval = await new HttpHelpper().Post(url, svtSend);
        if (reval.StatusCode.ToString() == "OK")
        {
          //reult = JsonConvert.DeserializeObject<SamV4MailResult>(reval.Content);
          reult.Code = "0";
        }
        else if (reval.StatusCode.ToString() == "BadRequest")
        {
          reult = JsonConvert.DeserializeObject<SamV4MailResult>(reval.Content);
          reult.Code = "2";
        }
      }
      catch (Exception e)
      {
        throw new InnoluxException(new ErrResult(ApiCodeEnum.Samv4MailSendError, e.Message));
      }
      return reult;
    }

    #endregion
  }
}
