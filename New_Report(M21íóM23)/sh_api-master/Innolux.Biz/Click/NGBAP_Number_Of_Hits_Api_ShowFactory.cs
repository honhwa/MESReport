using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Click
{
    public class SHAP_Number_Of_Hits_Api_ShowFactory:InnSqlAgent.IApiBase
    {
        public SHAP_Number_Of_Hits_Api_ShowFactory()
        {
            parameters["SysName"] = "SysName";
            description = "点击率()";
        }
        public override void doWork()
        {
            string SysName = GetParameterValueInString("SysName");
            string sql = $"select DISTINCT SYSMODULE from REPORT_NUMBER_OF_HIT where SYSNAME = '{SysName}'";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            object ob = new { Data = ds, Messqge = sql };
            returnValue = ob;
        }

    }
}
