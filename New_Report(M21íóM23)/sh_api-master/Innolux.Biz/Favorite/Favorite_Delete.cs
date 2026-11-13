using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Favorite
{
    public class Favorite_Delete : InnSqlAgent.IApiBase
    {
        public Favorite_Delete()
        {
            parameters["EMP_ID"] = "用户ID";
            parameters["EMP_NAME"] = "用户名称";
            parameters["ROUTE"] = "路由";
            parameters["ID"] = "当前节点ID";
            parameters["REP_NAME"] = "报表名称";
            parameters["SYSMODULE"] = "Module";
            description = "我的最爱（删除）";
        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string EMP_NAME = GetParameterValueInString("EMP_NAME");
            string ROUTE = GetParameterValueInString("ROUTE");
            string ID = GetParameterValueInString("ID");
            string REP_NAME = GetParameterValueInString("REP_NAME");
            string SYSMODULE = GetParameterValueInString("SYSMODULE");
            string sql = $"select EMP_ID from REPORT_FAVORITE where EMP_ID = '{EMP_ID}' and ID = '{ID}' and DELETE_TAG <> 'N' ";
            string ins_Sql = "";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                ins_Sql = $"update REPORT_FAVORITE set DELETE_TAG = 'N' where EMP_ID = '{EMP_ID}' and EMP_NAME = '{EMP_NAME}' and ID = '{ID}' and REP_NAME = '{REP_NAME}' and SYSMODULE = '{SYSMODULE}'";
            }
            if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", ins_Sql) == 1)
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
