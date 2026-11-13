using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Click
{
    public class SHAP_Number_Of_Hits_Api_Report:InnSqlAgent.IApiBase
    {
        public SHAP_Number_Of_Hits_Api_Report()
        {
            parameters["FactoryType"] = "厂别";
            parameters["FromDate"] = "开始时间";
            parameters["ToDate"] = "结束时间";
            description = "点击率(报表統計功能)";
        }
        public override void doWork()
        {
            string FactoryType = GetParameterValueInString("FactoryType");
            string FromDate = GetParameterValueInString("FromDate");
            string ToDate = GetParameterValueInString("ToDate");
            string ClicksNum = GetParameterValueInString("ClicksNum");
            string sql = $"SELECT * FROM (SELECT REPORT_ID, SYSFUN, SYSMODULE, MODULE, SYSNAME, COUNT(SYSFUN) AS REPORT from REPORT_NUMBER_OF_HIT " +
                $"WHERE SYSMODULE = '{FactoryType}' AND WRITE_DATE >= TO_DATE('{FromDate}','YYYY-MM-DD') AND WRITE_DATE <= TO_DATE('{ToDate}','YYYY-MM-DD')+1 " +
                $"GROUP BY SYSFUN, REPORT_ID, SYSMODULE, MODULE, SYSNAME) order by REPORT desc ";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            object ob = new { Data = ds, Messqge = sql };
            returnValue = ob;
        }

    }
}
