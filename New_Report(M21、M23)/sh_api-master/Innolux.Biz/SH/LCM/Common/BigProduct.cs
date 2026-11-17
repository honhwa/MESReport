using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Common
{
    public class BigProduct : InnSqlAgent.IApiBase
    {
        public BigProduct()
        {
            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "G_FAC";
            parameters["Size"] = "用户选择尺寸";
            //parameters["G_FAC"] = "标识（A）";
            description = "左侧共用接口(BigProduct)";
        }
        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_BigProd(fab);
                    return;
                default:
                    returnValue = "请填写厂别！";
                    return;
            }
        }

        public void Get_BigProd(string fab)
        {
            #region 变量获取
            //string fab = GetParameterValueInString("FactoryType");
            string lstr_G_FAC = GetParameterValueInString("G_FAC");
            string lstr_Size = GetParameterValueInString("Size");
            string sql_Size = string.Empty;
            string connectDB = string.Empty;
            #endregion

            #region DB连接
            if (fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion

            #region sql条件获取 
            if (lstr_Size != "ALL" && lstr_Size.Trim() != "")
                sql_Size = "  AND SIZEID in ('" + lstr_Size + "') ";
            #endregion

            #region SQL
            string sql = $@"Select Distinct SIZEID part_nbr, SUBSTR(PRODID,1,8) As PART
                    From MPIM Where FABID='B'
                    {sql_Size} 
                    Order by SIZEID, SUBSTR(PRODID,1,8)";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql);
            object ob = new { Data = ds, sql = sql };
            returnValue = ob;
        }
    }
}
