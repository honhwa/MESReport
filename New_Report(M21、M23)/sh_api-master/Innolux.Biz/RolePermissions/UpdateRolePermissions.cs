using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RolePermissions
{
    public class UpdateRolePermissions : InnSqlAgent.IApiBase
    {
        public UpdateRolePermissions()
        {
            parameters["ROLE"] = "角色";
            parameters["ROLELEVEL"] = "角色等级";
            parameters["BACKUP"] = "备注";
            parameters["PERSON"] = "最后更新者";
            description = "角色管理（修改角色）";
        }
        public override void doWork()
        {
            string ROLE = GetParameterValueInString("ROLE");
            string ROLELEVEL = GetParameterValueInString("ROLELEVEL");
            string BACKUP = GetParameterValueInString("BACKUP");
            string PERSON = GetParameterValueInString("PERSON");
            string sql = $" update REPORT_PERMISSION_MAPPING  set UPDATE_TIME = sysdate, ROLE = '{ROLE}', ROLELEVEL = '{ROLELEVEL}', BACKUP = '{BACKUP}' , PERSON = '{PERSON}' where ROLE = '{ROLE}' and ROLELEVEL = '{ROLELEVEL}' and ISDELETE <> 'N' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'])
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
