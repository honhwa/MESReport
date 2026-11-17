using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class ToBeMoveIn:InnSqlAgent.IApiBase
    {
        public ToBeMoveIn()
        {
            parameters["row1"] = "记录用户选择的站点";

            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "C/E";

            parameters["FromDate"] = "开始时间(B)";
            parameters["ToDate"] = "结束时间(B)";

            parameters["lstr_Shift"] = "";
            parameters["lstr_ShopCD"] = "";
            parameters["lstr_Floor"] = "";
            parameters["lstr_WorkOrder"] = "";
            parameters["lstr_Size"] = "";
            parameters["lstr_BigProdProduct"] = "";
            parameters["lstr_Product"] = "";
            parameters["lstr_Wotype"] = "";

            parameters["sql_Shift"] = "";
            parameters["sql_ShopCD"] = "";
            parameters["sql_Floor"] = "";
            parameters["sql_WorkOrder"] = "";
            parameters["sql_Size"] = "";
            parameters["sql_BigProduct"] = "";
            parameters["sql_Product"] = "";
            parameters["sql_Wotype"] = "";

            description = "对应OperPerfDetail 15文件";
        }

        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Data_SH(fab);
                    break;
                default:
                    returnValue = "请填写厂别(FactoryType)！";
                    return;
            }
        }

        public void Get_Data_SH(string fab)
        {
            #region 获取前一阶页面传递条件
            string row1 = GetParameterValueInString("row1");
            string col1 = GetParameterValueInString("col1");

            string lstr_G_FAC = GetParameterValueInString("G_FAC");

            string lstr_FromDate = GetParameterValueInString("FromDate");
            string lstr_ToDate = GetParameterValueInString("ToDate");

            string lstr_Shift = GetParameterValueInString("lstr_Shift");
            string lstr_ShopCD = GetParameterValueInString("lstr_ShopCD");
            string lstr_Floor = GetParameterValueInString("lstr_Floor");
            string lstr_WorkOrder = GetParameterValueInString("lstr_WorkOrder");
            string lstr_Size = GetParameterValueInString("lstr_Size");
            string lstr_BigProdProduct = GetParameterValueInString("lstr_BigProdProduct");
            string lstr_Product = GetParameterValueInString("lstr_Product");
            string lstr_Wotype = GetParameterValueInString("lstr_Wotype");

            string sql_Shift = GetParameterValueInString("sql_Shift");
            string sql_ShopCD = GetParameterValueInString("sql_ShopCD");
            string sql_Floor = GetParameterValueInString("sql_Floor");
            string sql_WorkOrder = GetParameterValueInString("sql_WorkOrder");
            string sql_Size = GetParameterValueInString("sql_Size");
            string sql_BigProduct = GetParameterValueInString("sql_BigProduct");
            string sql_Product = GetParameterValueInString("sql_Product");
            string sql_Wotype = GetParameterValueInString("sql_Wotype");

            string sql_OperRange = string.Empty;
            #region SQL条件
            //2021-07-17  wyx 新增1910拆箱，分別在1800與1901的pass量中減去拆箱數，1901拆箱,則1800站點的pass量中減去拆箱數
            sql_OperRange = " and  WORKCTR = '" + row1 + "'  ";
            #endregion

            string connectDB = string.Empty;
            string sql01 = string.Empty;
            #endregion

            #region TABLE + DB连接
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion


            #region SQL
            sql01 = $@"SELECT SUM(TOTALRWKQTY) AS COUNT 
                        ,PRODID AS PROD_NBR
                        ,'' AS VER
                        ,'' AS CART_FLAG
                        ,0 AS HOLDQTY 
                        FROM RPT_MA21_PASS_RWK_SCR_QTY
                        WHERE FABID = 'B' 
                        AND WORKCTR LIKE 'MR%'
			            AND REPORTDATE BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
			            {sql_Shift}
                        {sql_ShopCD}
			            {sql_Floor}
			            {sql_WorkOrder}
                        {sql_Size}
			            {sql_BigProduct}
			            {sql_Product}
                        {sql_Wotype}
                        {sql_OperRange}
                        GROUP BY PRODID" ;
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql01);
            #region 去除SQL语句中的特殊符号
            sql01 = sql01.Replace("\r\n", " ");
            sql01 = sql01.Replace("\n", " ");
            sql01 = sql01.Replace("\t", " ");
            sql01 = sql01.Replace("\t\t", " ");
            sql01 = sql01.Replace("\t\t\t", " ");
            sql01 = sql01.Replace("\t\t\t\t", " ");
            #endregion

            #region 回传前端data
            object ob = new
            {
                Data = ds,
                SQL01 = sql01,

                FactoryType = fab,
                G_FAC = lstr_G_FAC,

                FromDate = lstr_FromDate,
                ToDate = lstr_ToDate,

                row1 = row1,

                lstr_Shift = lstr_Shift,
                lstr_ShopCD = lstr_ShopCD,
                lstr_Floor = lstr_Floor,
                lstr_WorkOrder = lstr_WorkOrder,
                lstr_Size = lstr_Size,
                lstr_BigProdProduct = lstr_BigProdProduct,
                lstr_Product = lstr_Product,
                lstr_Wotype = lstr_Wotype
            };
            returnValue = ob;
            #endregion

        }

    }
}
