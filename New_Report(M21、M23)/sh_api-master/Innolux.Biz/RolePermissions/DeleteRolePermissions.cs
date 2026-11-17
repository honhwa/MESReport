using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RolePermissions
{
    public class DeleteRolePermissions : InnSqlAgent.IApiBase
    {
        public DeleteRolePermissions()
        {
            parameters["ROLE"] = "角色";
            parameters["ROLELEVEL"] = "角色等级";
            description = "角色管理（删除角色）";
        }
        public override void doWork()
        {
            string ROLE = GetParameterValueInString("ROLE");
            string ROLELEVEL = GetParameterValueInString("ROLELEVEL");
            string sql = $" update REPORT_PERMISSION_MAPPING  set ISDELETE = 'N', UPDATE_TIME = sysdate  where ROLE = '{ROLE}' and ROLELEVEL='{ROLELEVEL}' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'])
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
