using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using StackExchange.Redis;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class OperationPerformance : InnSqlAgent.IApiBase
    {
        public OperationPerformance()
        {
            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "C/E"; 
            
            parameters["FromDate"] = "开始时间(B)";
            parameters["ToDate"] = "结束时间(B)";
            
            parameters["Shift"] = "ALL/A/B(B)";
            parameters["ShopCD"] = "ALL/Cell2/Module";
            parameters["Floor"] = "ALL/SH1-1F/SH1-2F/SH1-3F/SH1-4F/SH2-2F";
            parameters["Wo_nbr"] = "工單";
            parameters["Size"] = "尺寸";
            parameters["Big_Prod_nbr"] = "機種前八位";
            parameters["Prod_nbr"] = "點選機種(B)";
            parameters["WoType"] = "工單類型";

            description = "OperationPerformance首页";
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
            string lstr_G_FAC = GetParameterValueInString("G_FAC");

            string lstr_FromDate = GetParameterValueInString("FromDate");
            string lstr_ToDate = GetParameterValueInString("ToDate");

            string lstr_Shift = GetParameterValueInString("Shift");
            string lstr_ShopCD = GetParameterValueInString("ShopCD");
            string lstr_Floor = GetParameterValueInString("Floor");
            string lstr_WorkOrder = GetParameterValueInString("Wo_nbr");
            string lstr_Size = GetParameterValueInString("Size");
            string lstr_BigProdProduct = GetParameterValueInString("Big_Prod_nbr");
            string lstr_Product = GetParameterValueInString("Prod_nbr");
            string lstr_Wotype = GetParameterValueInString("WoType");

            //暂存条件sql条件
            string sql_Shift = string.Empty;
            string sql_ShopCD = string.Empty;
            string sql_Floor = string.Empty;
            string sql_WorkOrder = string.Empty;
            string sql_Size = string.Empty;
            string sql_BigProduct = string.Empty;
            string sql_Product = string.Empty;
            string sql_Wotype = string.Empty;

            string sql = string.Empty;
            string connectDB = string.Empty;
            string? pass_cuttime = string.Empty;
            string? wip_cuttime = string.Empty;
            string lstr_index = string.Empty;
            //传递给下一阶页面条件
            string col0 = string.Empty;
            string col1 = string.Empty;
            string row0 = string.Empty;
            string row1 = string.Empty;
            #endregion

            #region 条件串取
            //班别
            if (lstr_Shift != "ALL" && lstr_Shift.Trim() != "")
                sql_Shift = " and ShiftId = '" + lstr_Shift + "'  ";
            //大段別
            if (lstr_ShopCD != "ALL" && lstr_ShopCD.Trim() != ""){
                sql_ShopCD = " and SHOPCD = '" + lstr_ShopCD + "'  ";
            }
            //楼层
            if (lstr_Floor != "ALL" && lstr_Floor.Trim() != ""){
                sql_Floor = " and Floor = '" + lstr_Floor + "'  ";
            }
            //工單號
            if (lstr_WorkOrder != "ALL" && lstr_WorkOrder.Trim() != ""){
                sql_WorkOrder = " and WONBR in ('" + lstr_WorkOrder.TrimEnd(',') + "')";
            }
            //尺寸
            if (lstr_Size != "ALL" && lstr_Size.Trim() != "")
                sql_Size = " and SizeId in ('" + lstr_Size.TrimEnd(',') + "')  ";
            //sql_Size = " and SizeId in ('" + lstr_Size.TrimEnd(',').Replace(",", "','") + "')  ";
            //大機種
            if (lstr_BigProdProduct != "ALL" && lstr_BigProdProduct.Trim() != "") {
                sql_BigProduct = " and BIGPRODID in ('" + lstr_BigProdProduct.TrimEnd(',') + "')  ";
            }

            // PRODUCT
            if (lstr_Product != "ALL" && lstr_Product.Trim() != "") {
                sql_Product = "  and PRODID in ( '" + lstr_Product.TrimEnd(',') + "') ";
            }

            // 工單類型
            if (lstr_Wotype != "ALL" && lstr_Wotype.Trim() != "") {
                sql_Wotype = "  and WOTYPE in ( '" + lstr_Wotype.TrimEnd(',') + "') ";
            }
                
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
            sql = @$"SELECT AA.WORKCTR WORK_CTR
	                ,SUM(AA.PASS_QTY) AS PASS_QTY
	                ,SUM(AA.TOTALRWK_QTY) AS TOTALRWK_QTY
	                ,SUM(AA.FIRSTRWK_QTY) AS FIRSTRWK_QTY
	                ,SUM(AA.TOBESCRAP_QTY) AS TOBESCRAP_QTY
	                ,SUM(AA.PASS1_QTY) AS PASS1_QTY
	                ,SUM(AA.PASS2_QTY) AS PASS2_QTY
	                ,SUM(AA.IN_QTY) AS IN_QTY
	                ,SUM(AA.WIP_QTY) AS WIP_QTY
	                ,SUM(AA.HODE_QTY) AS HODE_QTY
	                ,SUM(AA.NO_HODE_QTY) AS NO_HODE_QTY
	                ,SUM(AA.TOBEMVIN_QTY) AS TOBEMVIN_QTY	
	                 FROM (
			                SELECT MW.WORKCTR
					                ,0 AS PASS_QTY --1
					                ,0 AS TOTALRWK_QTY --1
					                ,0 AS FIRSTRWK_QTY --1
					                ,0 AS TOBESCRAP_QTY --1
					                ,0 AS PASS1_QTY --3
					                ,0 AS PASS2_QTY --4
					                ,0 AS IN_QTY --1
					                ,0 AS WIP_QTY --2
					                ,0 AS HODE_QTY
					                ,0 AS NO_HODE_QTY
					                ,0 AS TOBEMVIN_QTY
			                 FROM MWCM MW
where 1= 1 {sql_ShopCD}
			                UNION
			                /*1. PASS_QTY, TOTALRWK_QTY, FIRSTRWK_QTY, TOBESCRAP_QTY, PASS1_QTY, PASS2_QTY, IN_QTY*/
			                SELECT WORKCTR
					                ,SUM(PASSQTY) AS PASS_QTY
					                ,SUM(TOTALRWKQTY) AS TOTALRWK_QTY
					                ,SUM(FIRSTRWKQTY) AS FIRSTRWK_QTY
					                ,SUM(TOBESCRAPQTY) AS TOBESCRAP_QTY
					                ,SUM(PASS1QTY) AS PASS1_QTY
					                ,SUM(PASS2QTY) AS PASS2_QTY
					                ,SUM(INPUTQTY) AS IN_QTY
					                ,0 AS WIP_QTY
					                ,0 AS HODE_QTY
					                ,0 AS NO_HODE_QTY
					                ,0 AS TOBEMVIN_QTY
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
			                GROUP BY WORKCTR
			                UNION 
			                /*2. WIP_QTY*/
			                SELECT WORKCTR
					                ,0 AS PASS_QTY
					                ,0 AS TOTALRWK_QTY
					                ,0 AS FIRSTRWK_QTY
					                ,0 AS TOBESCRAP_QTY
					                ,0 AS PASS1_QTY
					                ,0 AS PASS2_QTY
					                ,0 AS IN_QTY
					                ,SUM(WIPQTY) WIP_QTY 
					                ,SUM(HODE_QTY) AS HODE_QTY
					                ,SUM(WIPQTY) - SUM(HODE_QTY) AS NO_HODE_QTY
					                ,0 AS TOBEMVIN_QTY
			                 FROM RPT_MA21_WIP 
                            WHERE FABID = 'B' 
			                  /*AND ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
			                  {sql_Shift}*/
			                  {sql_ShopCD}
			                  {sql_Floor}
			                  {sql_WorkOrder}
                              {sql_Size}
			                  {sql_BigProduct}
			                  {sql_Product}
                              {sql_Wotype}
			                GROUP BY WORKCTR
	                   ) AA GROUP BY AA.WORKCTR 
                       ORDER BY WORKCTR";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql);
            #region cuttime获取
            //SP_MCSPAS_SOUTPUT1
            string sql_cuttime = $@" SELECT 'pass' appname, MAX(TransDate) cuttime FROM RPT_MA21_PASS_RWK_SCR_QTY UNION SELECT 'wip' appname,MAX(TransDate) cuttime FROM RPT_MA21_WIP ";
            System.Data.DataSet ds_cuttime = sqlExtension.SqlTxn.GetDataSet(connectDB, sql_cuttime);
            //cuttime = ds_cuttime.Tables[0].Rows[0]["CUTTIME"].ToString();
            pass_cuttime = ds_cuttime.Tables[0].Select(" appname = 'pass'")[0][1].ToString();
            wip_cuttime = ds_cuttime.Tables[0].Select(" appname = 'wip'")[0][1].ToString();
            #endregion

            #region 去除SQL语句中的特殊符号
            sql = sql.Replace("\r\n", " ");
            sql = sql.Replace("\n", " ");
            sql = sql.Replace("\t", " ");
            sql = sql.Replace("\t\t", " ");
            sql = sql.Replace("\t\t\t", " ");
            sql = sql.Replace("\t\t\t\t", " ");
            #endregion

            #region 回传前端 data
            object ob = new{
                Data = ds,
                SQL01 = sql,
                
                pass_cuttime = pass_cuttime,
                wip_cuttime = wip_cuttime,
                
                FactoryType = fab,
                G_FAC = lstr_G_FAC,
                
                FromDate = lstr_FromDate,
                ToDate = lstr_ToDate,

                lstr_Shift = lstr_Shift,
                lstr_ShopCD = lstr_ShopCD,
                lstr_Floor = lstr_Floor,
                lstr_WorkOrder = lstr_WorkOrder,
                lstr_Size = lstr_Size,
                lstr_BigProdProduct = lstr_BigProdProduct,
                lstr_Product = lstr_Product,
                lstr_Wotype = lstr_Wotype,

                sql_Shift = sql_Shift,
                sql_ShopCD = sql_ShopCD,
                sql_Floor = sql_Floor,
                sql_WorkOrder = sql_WorkOrder,
                sql_Size = sql_Size,
                sql_BigProduct = sql_BigProduct,
                sql_Product = sql_Product,
                sql_Wotype = sql_Wotype,

                //传递行数  用于判断开启哪个页面,前端传入，非后端程式提供
                //col0 = col0,
                //col1 = col1,
                //row0 = row0,
                //row1 = row1
            };
            returnValue = ob;
            #endregion
        }
    }
}
