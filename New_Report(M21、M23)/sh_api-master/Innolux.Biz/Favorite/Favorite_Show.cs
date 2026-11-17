using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Favorite
{
    public class Favorite_Show : InnSqlAgent.IApiBase
    {
        public Favorite_Show()
        {
            parameters["EMP_ID"] = "用户ID";
            description = "我的最爱（查询）";
        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string sql = $"select ROUTE, ID, REP_NAME, SYSMODULE from REPORT_FAVORITE where EMP_ID = '{EMP_ID}' and DELETE_TAG <> 'N'";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            object ob = new { Data = ds, Messqge = sql };
            returnValue = ob;
        }
    }

}
