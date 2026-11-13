using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Role
{
    public class DeleteRole:InnSqlAgent.IApiBase
    {
        public DeleteRole()
        {
            parameters["EMP_ID"] = "EMP_ID";
            parameters["EMP_NAME"] = "EMP_NAME";
            description = "账号角色管理（删除账号及其权限）";
        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string EMP_NAME = GetParameterValueInString("EMP_NAME");
            string sql = $" delete from REPORT_LOGIN_LOG  where EMP_ID = '{EMP_ID}' and EMP_NAME ='{EMP_NAME}' ";
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
