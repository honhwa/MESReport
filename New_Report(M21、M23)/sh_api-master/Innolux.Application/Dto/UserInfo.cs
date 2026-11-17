using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innolux.Application.Dto
{
  public class UserInfo
  {
    public string Eid { get; set; }
    public string Uid { get; set; }
    public string WorkOrgId { get; set; }
    public string EmpNo { get; set; }

    public string Name { get; set; }

    public string DeptCode { get; set; }

    public string DeptName { get; set; }
    public string OrgLevel { get; set; }
    public string Indirect { get; set; }
    public string DomainAccount { get; set; }
    public string EMail { get; set; }
    public List<string> Roles { get; set; }

    public string RolesStr
    {
      get
      {
        string rolesStr = "";
        if (Roles != null)
        {
          Roles.ForEach(item =>
          {
            rolesStr += rolesStr.Length == 0 ? "" : "," + item;
          });
        }
        return rolesStr;
      }
    }
  }
}
