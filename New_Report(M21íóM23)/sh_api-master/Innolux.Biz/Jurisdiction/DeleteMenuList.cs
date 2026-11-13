using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Jurisdiction
{
    public class DeleteMenuList : InnSqlAgent.IApiBase
    {
        public DeleteMenuList()
        {
            parameters["id"] = "id";
            parameters["SYSID"] = "SYSID";
            parameters["orderNum"] = "orderNum";
            description = "权限管理接口(删除菜单)";

        }
        public override void doWork()
        {
            string id = GetParameterValueInString("id");
            string SYSID = GetParameterValueInString("SYSID");
            string orderNum = GetParameterValueInString("orderNum");
            string sql = $" update REPORT_SYS_MANAGE_PERMISSION  set ISDELETE = 'N' where ID = '{id}' and SYSID='{SYSID}' and ORDERNUM = '{orderNum}' and ISDELETE <> 'N' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'])
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
