using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Innolux.Biz.Click
{
    public class SHAP_Number_Of_Hits_Api_Select : InnSqlAgent.IApiBase
    {
        public SHAP_Number_Of_Hits_Api_Select()
        {
            description = "点击率";
        }
        public override void doWork()
        {
            string sql = "select * From REPORT_NUMBER_OF_HIT";
            //连接测试DB
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            returnValue = ds;
        }
    }

}
