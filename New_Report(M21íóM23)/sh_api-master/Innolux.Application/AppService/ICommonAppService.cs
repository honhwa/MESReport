using System;
using System.Data;
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

namespace Innolux.Application.AppService
{
  public interface ICommonAppService
  {
    Task<SamV4MailResult> SendSamV4Mail(string mailto, string subject, string body, string mailcc = "", List<Attachements> files = null);
  }
}
