using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class OperationPerformancePassQty : InnSqlAgent.IApiBase
    {
        public OperationPerformancePassQty()
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

            description = "对应OperPerfDetail3文件/選中的pass_qty列";
        }
        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Date_SH(fab);
                    break;
                default:
                    returnValue = "请填写厂别(FactoryType)！";
                    return;
            }
        }

        private void Get_Date_SH(string fab)
        {
            #region 变量获取
            string row1 = GetParameterValueInString("row1");

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

            string sql = string.Empty;
            string connectDB = string.Empty;
            string sql_OperRange = string.Empty;
            string? pass_cuttime = string.Empty;
            string? wip_cuttime = string.Empty;
            string lstr_index = string.Empty;
            //传递给下一阶页面条件
            string col0 = string.Empty;
            string col1 = string.Empty;
            string row0 = string.Empty;
            //传递二阶 trans_type条件
            string sql02 = string.Empty;
            string sql_TransType = string.Empty;
            #endregion

            #region SQL条件
            sql_OperRange = " and  WORKCTR = '" + row1 + "'  ";
            #endregion

            #region table
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
            {
                connectDB = "MESHIS";
                //LCM1 强制索引
                //lstr_index = "   /*+index(mcspas_soutput1 MCSPAS_SOUTPUT1_IDX2)+*/   ";
            }
            #endregion

            #region  SQL
            sql = @$"SELECT PRODID AS PROD_NBR,
                            '' AS VER,
                            SUBLINEID AS EQUIP_NBR,
                            SHIFTID AS SHIFT_ID,
                            SUM(INPUTQTY) AS IN_QTY,                            
                            SUM(PASSQTY) AS PASS_QTY,
                            SUM(TOTALRWKQTY) AS RWK_QTY
			           FROM RPT_MA21_PASS_RWK_SCR_QTY
                      WHERE FABID = 'B' 
			            AND ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
			                {sql_Shift}
                            {sql_ShopCD}
			                {sql_Floor}
			                {sql_WorkOrder}
                            {sql_Size}
			                {sql_BigProduct}
			                {sql_Product}
                            {sql_Wotype}
                            {sql_OperRange}
			GROUP BY PRODID,SUBLINEID,SHIFTID";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql);
            #region 去除SQL语句中的特殊符号
            sql = sql.Replace("\r\n", " ");
            sql = sql.Replace("\n", " ");
            sql = sql.Replace("\t", " ");
            sql = sql.Replace("\t\t", " ");
            sql = sql.Replace("\t\t\t", " ");
            sql = sql.Replace("\t\t\t\t", " ");
            #endregion

            #region 传递下一阶页面条件串取
            ////不同站点卡取 trans_type
            #endregion

            #region 回传前端data
            object ob = new
            {
                Data = ds,
                //SQL01 = sql,
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
                //特殊条件 各站点的 trans_type
                //传递至下一阶页面 还需要加入 点选 row / col及资料

            };
            returnValue = ob;
            #endregion
        }
    }

}
