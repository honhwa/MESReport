using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Common
{
    public class Size : InnSqlAgent.IApiBase
    {
        public Size()
        {
            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "G_FAC";
            //parameters["PRODGROUP"] = "用户选择產品群組";
            //parameters["G_FAC"] = "标识（A）";
            description = "左侧共用接口(Size)";
        }
        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Size(fab);
                    return;
                default:
                    returnValue = "请填写厂别！";
                    return;
            }
        }

        public void Get_Size(string fab)
        {
            #region 变量获取
            //string fab = GetParameterValueInString("FactoryType");
            string lstr_G_FAC = GetParameterValueInString("G_FAC");
            //string lstr_Prodgroup = GetParameterValueInString("PRODGROUP");
            //string sql_Prodgroup = string.Empty;
            string connectDB = string.Empty;
            #endregion

            #region DB连接
            if (fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion

            //#region sql条件获取 
            //if (lstr_Prodgroup != "ALL" && lstr_Prodgroup.Trim() != "")
            //    sql_Prodgroup = "  AND PRODGROUP in ('" + lstr_Prodgroup + "') ";
            //#endregion

            

            #region SQL
            string sql = $@"Select Distinct SIZEID
                    From MPIM Where FABID='B'
                    Order by SIZEID";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql);
            object ob = new { Data = ds, sql = sql };
            returnValue = ob;
        }
    }
}
