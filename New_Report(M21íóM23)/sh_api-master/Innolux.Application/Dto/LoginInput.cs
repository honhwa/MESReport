using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Innolux.Application.Dto
{
  public class LoginInput
  {
    /// <summary>
    /// 賬號
    /// </summary>
    [Required]
    public string username { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    [Required]
    public string password { get; set; }

    public string ip { get; set; } = "";
  }

}
