using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RoleFunction
{
    public class DeleteRoleFunction : InnSqlAgent.IApiBase
    {
        public DeleteRoleFunction()
        {
            parameters["ROLE_ID"] = "角色ID";
            parameters["ROLE_NAME"] = "角色名称";
            parameters["ROUTER_ID"] = "路由ID";
            description = "角色权限管理（删除角色权限）";
        }

        public override void doWork()
        {
            string ROLE_ID = GetParameterValueInString("ROLE_ID");
            string ROLE_NAME = GetParameterValueInString("ROLE_NAME");
            string ROUTER_ID = GetParameterValueInString("ROUTER_ID");
            string sql = $" update REPORT_PERMISSION_MAPPING  set ISDELETE = 'N', UPDATE_TIME = sysdate  where ROLE_ID = '{ROLE_ID}' and ROLE_NAME='{ROLE_NAME}' amd ROUTER_ID = '{ROUTER_ID}' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'])
            //System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
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
