using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Innolux.Biz.Role
{
    public class UpdataRole:InnSqlAgent.IApiBase
    {
        public UpdataRole()
        {
            parameters["EMP_ID"] = "EMP_ID";
            parameters["EMP_NAME"] = "EMP_NAME";
            parameters["EMP_PERMS"] = "EMP_PERMS";
            parameters["BACKUP"] = "BACKUP";
            description = "账号角色管理（更改账号权限）";
        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string EMP_NAME = GetParameterValueInString("EMP_NAME");
            string EMP_PERMS = GetParameterValueInString("EMP_PERMS");
            string BACKUP = GetParameterValueInString("BACKUP");
            string sql = $"update REPORT_LOGIN_LOG set EMP_PERMS = '{EMP_PERMS}', UPDATE_TIME = sysdate, EMP_NAME = '{EMP_NAME}', BACKUP = '{BACKUP}' where EMP_ID = '{EMP_ID}' ";
            //"update REPORT_SYS_MANAGE_PERMISSION set TYPE = '{type}',NAME = '{name}',PARENTID = '{parentId}',ROUTER = '{router}',ICON = '{icon}',ISSHOW = '{isShow}',UPDATETIME = trunc(sysdate)
            //where ID = '{id}' and SYSID = '{SYSID}' and ORDERNUM = '{orderNum}'
            if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql) == 1)
            {
                returnValue = "Success!";
            }
            else
            {
                returnValue = "False!";
            }

        }
    }
}
