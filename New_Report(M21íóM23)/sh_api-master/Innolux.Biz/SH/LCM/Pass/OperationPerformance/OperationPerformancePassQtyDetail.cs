using Google.Protobuf.WellKnownTypes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Crypto.Macs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class OperationPerformancePassQtyDetail : InnSqlAgent.IApiBase
    {
        public OperationPerformancePassQtyDetail()
        {
            parameters["row1"] = "记录用户选择的站点";//dataset界面 點選行 對應的站點
            parameters["col1"] = "用户PassQty选择的列[4/ 5/ 6] ";//pass_qty界面，用戶選擇的列--投產數、良品數、rework
            parameters["prod_nbr"] = "记录用户选择的機種";
            parameters["ver"] = "记录用户选择的机种版本";
            parameters["equip_nbr"] = "记录用户选择的机台";
            parameters["shift_id"] = "记录用户选择的班别";
            parameters["in_qty"] = "记录用户选择的投产数";
            parameters["pass_qty"] = "记录用户选择的良品数";
            parameters["rwk_qty"] = "记录用户选择的重工品数";
            //parameters["sql_TransType"] = "前一阶页面传递的 trans_type条件";

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

            description = "对应OperPerfDetail 10文件";
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

        private void Get_Data_SH(string fab)
        {
            #region 获取前一阶页面传递条件
            //前一阶用户选择条件
            string row1 = GetParameterValueInString("row1");
            string col1 = GetParameterValueInString("col1");
            string S_Prod_Nbr = GetParameterValueInString("Prod_Nbr");
            string S_Ver = GetParameterValueInString("Ver");
            string S_Equip_nbr = GetParameterValueInString("Eqp_Nbr");
            string S_Shift_Id = GetParameterValueInString("Shift");
            string S_In_Qty = GetParameterValueInString("In_Qty");
            string S_Good_Qty = GetParameterValueInString("Good_Qty");
            string S_Rwk_Qty = GetParameterValueInString("rwk_qty");
            //string sql_TransType = GetParameterValueInString("sql_TransType");

            string lstr_G_FAC = GetParameterValueInString("G_FAC");

            string lstr_FromDate = GetParameterValueInString("FromDate") + " 07:00:00";
            string lstr_ToDate = Convert.ToDateTime(GetParameterValueInString("ToDate").ToString()).AddDays(1).ToString("d") + " 07:00:00";

            string lstr_Shift = GetParameterValueInString("lstr_Shift");
            string lstr_ShopCD = GetParameterValueInString("lstr_ShopCD");
            string lstr_Floor = GetParameterValueInString("lstr_Floor");
            string lstr_WorkOrder = GetParameterValueInString("lstr_WorkOrder");
            string lstr_Size = GetParameterValueInString("lstr_Size");
            string lstr_BigProdProduct = GetParameterValueInString("lstr_BigProdProduct");
            string lstr_Product = GetParameterValueInString("lstr_Product");
            string lstr_Wotype = GetParameterValueInString("lstr_Wotype");

            string sql_Shift = string.Empty;
            string sql_ShopCD = string.Empty;
            string sql_Floor = string.Empty;
            string sql_WorkOrder = string.Empty;
            string sql_Size = string.Empty;
            string sql_BigProdProduct = string.Empty;
            string sql_Product = string.Empty;
            string sql_Wotype = string.Empty;

            if (lstr_Shift != "ALL" && lstr_Shift.Trim() != "")
                sql_Shift = " and shiftid = '" + lstr_Shift + "'  ";
            if (lstr_ShopCD != "ALL" && lstr_ShopCD.Trim() != "")
                sql_ShopCD = " and nshopcd = '" + lstr_ShopCD + "'  ";
            if (lstr_Floor != "ALL" && lstr_Floor.Trim() != "")
                sql_Floor = " and SUBSTR(nsublineid,1,3) = '" + lstr_Floor + "'  ";
            if (lstr_WorkOrder != "ALL" && lstr_WorkOrder.Trim() != "")
                sql_WorkOrder = " and TOWONBR in ('" + lstr_WorkOrder.TrimEnd(',').Replace(",", "','") + "')";
            if (lstr_Size != "ALL" && lstr_Size.Trim() != "")
                sql_Size = " and sizeid in ('" + lstr_Size.TrimEnd(',').Replace(",", "','") + "')";
            //if (lstr_BigProdProduct != "ALL" && lstr_BigProdProduct.Trim() != "")
            //    sql_BigProdProduct = " and substring(toprodid,1,8) in ('" + lstr_BigProdProduct.TrimEnd(',').Replace(",", "','") + "')  ";
            var products = lstr_BigProdProduct.TrimEnd(',').Split(',');
            var likeConditions = new List<string>();

            foreach (var prod in products.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                // 先去掉数据本身可能带的前后单引号和空格，再转义
                var clean = prod.Trim().Trim('\'', ' ', '\t');
                var escaped = clean.Replace("'", "''");
                likeConditions.Add($"toprodid like '{escaped}%'");
            }

            sql_BigProdProduct = likeConditions.Count > 0
                ? $" and ({string.Join(" or ", likeConditions)}) "
                : "";

            if (lstr_Product != "ALL" && lstr_Product.Trim() != "")
                sql_Product = " and toprodid in ('" + lstr_Product.TrimEnd(',').Replace(",", "','") + "')  ";
            if (lstr_Wotype != "ALL" && lstr_Wotype.Trim() != "")
                sql_Wotype = " and towotype in ('" + lstr_Wotype.TrimEnd(',').Replace(",", "','") + "')  ";

            string sql = string.Empty;
            string connectDB = string.Empty;
            string? pass_cuttime = string.Empty;
            string? wip_cuttime = string.Empty;
            string lstr_index = string.Empty;
            string sql_subline_Id = string.Empty;
            //传递给下一阶页面条件
            string col0 = string.Empty;
            string row0 = string.Empty;
            string sql01 = string.Empty;
            #endregion

            if (!string.IsNullOrWhiteSpace(lstr_WorkOrder))
                lstr_WorkOrder = " and towonbr = '" + lstr_WorkOrder + "'";

            #region
            if (S_Prod_Nbr != "合計")
            {
                //覆盖 dataset页面条件
                //sql_Product = " AND V.TOPRODID = '"+ S_Prod_Nbr  + "' ";
                //sql_subline_Id = "  AND V.NSUBLINEID = '" + S_subLine_Id + "'  ";
                //sql_Shift = "  and shift_id  ='" + S_Shift + "'  ";
            }
            #endregion

            #region TABLE + DB连接
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion

            #region 不同指标呈现
            // 4:投产数列  5:良品数列  6：rework数列
            if (S_In_Qty.Trim() == "")
                S_In_Qty = "0";
            if (S_Good_Qty.Trim() == "")
                S_Good_Qty = "0";

            if (col1 == "4")
            {
                #region SQL
                sql01 = @$"SELECT V.FABID AS FAC_ID
                            ,V.TOENTITYID AS PNL_ID
                            ,V.TRANSDATE AS TRANS_DATE
                            ,0 AS TRANS_NBR
                            ,0 AS OPERATORS
                            ,V.TOPRODID AS PROD_NBR
                            ,V.NSUBLINEID AS EQUIP_NBR
                            ,'' AS VER
                            ,V.CWORKCTR AS OPER
                            ,'MES' AS COMMENTS
                            ,'' AS FLOOR
                            ,V.TOWONBR AS WO_NBR
                            ,CASE WHEN V.SHIFTID = 'D' THEN 'A' ELSE 'B' END AS SHIFT_ID
                            ,M.RW_COUNT AS RW_COUNT
                            ,V.TOWOTYPE AS WO_TYPE
                            ,'' AS TRANS_TYPE
                            ,V.NWORKCTR AS TO_OPER
                            ,'' AS MAIN_OPER
                            ,'' AS OUTPUT_FG
                            ,V.TOLOTGRADE AS GRADE
                            ,'' AS LCM_OWNER
                            ,V.CARTID AS CART_ID
                            ,V.CUSTOMERID AS SHIPPING_NBR
                             FROM VWLTXA V 
                            LEFT JOIN (SELECT MAX(M1.RWK_CUM_NUM) RW_COUNT,M1.PANELID FROM RPT_MA21_PANEL_RWK_CNT M1 GROUP BY PANELID) M ON M.PANELID = V.TOENTITYID 
                            LEFT JOIN (SELECT PRODID,SIZEID FROM MPIM) N ON N.PRODID = V.TOPRODID                             
                            WHERE FABID ='B' 
                            AND VERB = 'MVIN' 
                            AND TRANSDATE BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
                            AND CWORKCTR = '{row1}'
                            AND TOPRODID = '{S_Prod_Nbr}'
                            AND NSUBLINEID = '{S_Equip_nbr}'
                            AND SHIFTID = '{S_Shift_Id}'
			                /*{sql_Shift}*/
                            {sql_ShopCD}
                            /*{sql_Floor}*/
                            {sql_WorkOrder}
                            {sql_Size}
                            /*{sql_BigProdProduct}*/
                            /*{sql_Product}*/
                            {sql_Wotype}";
                #endregion

            }
            else if (col1 == "5")
            {

                #region SQL
                sql01 = @$"SELECT V.FABID AS FAC_ID
                            ,V.TOENTITYID AS PNL_ID
                            ,V.TRANSDATE AS TRANS_DATE
                            ,0 AS TRANS_NBR
                            ,0 AS OPERATORS
                            ,V.TOPRODID AS PROD_NBR
                            ,V.CSUBLINEID AS EQUIP_NBR
                            ,'' AS VER
                            ,V.CWORKCTR AS OPER
                            ,'MES' AS COMMENTS
                            ,'' AS FLOOR
                            ,V.TOWONBR AS WO_NBR
                            ,CASE WHEN V.SHIFTID = 'D' THEN 'A' ELSE 'B' END AS SHIFT_ID
                            ,M.RW_COUNT AS RW_COUNT
                            ,V.TOWOTYPE AS WO_TYPE
                            ,'' AS TRANS_TYPE
                            ,V.NWORKCTR AS TO_OPER
                            ,'' AS MAIN_OPER
                            ,'' AS OUTPUT_FG
                            ,V.TOLOTGRADE AS GRADE
                            ,'' AS LCM_OWNER
                            ,V.CARTID AS CART_ID
                            ,V.CUSTOMERID AS SHIPPING_NBR
                             FROM VWLTXA V 
                            LEFT JOIN (SELECT MAX(M1.RWK_CUM_NUM) RW_COUNT,M1.PANELID FROM RPT_MA21_PANEL_RWK_CNT M1 GROUP BY PANELID) M ON M.PANELID = V.TOENTITYID 
                            LEFT JOIN (SELECT PRODID,SIZEID FROM MPIM) N ON N.PRODID = V.TOPRODID                            
                            WHERE FABID ='B' 
                            AND VERB = 'MVOU'
                            AND V.TOLOTGRADE='A' 
                            AND V.DEFECTCD IS NULL --Oracle空串即NULL,原為 = ''
                            AND TRANSDATE BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
                            AND CWORKCTR = '{row1}'
                            AND TOPRODID = '{S_Prod_Nbr}'
                            AND CSUBLINEID = '{S_Equip_nbr}'
                            AND SHIFTID = '{S_Shift_Id}'
			                /*{sql_Shift}*/
                            {sql_ShopCD}
                            /*{sql_Floor}*/
                            {sql_WorkOrder}
                            {sql_Size}
                            /*{sql_BigProdProduct}*/
                            /*{sql_Product}*/
                            {sql_Wotype}";
                #endregion

            }
            else if (col1 == "6")
            {
                #region SQL
                sql01 = @$"SELECT V.FABID AS FAC_ID
                            ,V.TOENTITYID AS PNL_ID
                            ,V.TRANSDATE AS TRANS_DATE
                            ,0 AS TRANS_NBR
                            ,0 AS OPERATORS
                            ,V.TOPRODID AS PROD_NBR
                            ,V.CSUBLINEID AS EQUIP_NBR
                            ,'' AS VER
                            ,V.CWORKCTR AS OPER
                            ,'MES' AS COMMENTS
                            ,'' AS FLOOR
                            ,V.TOWONBR AS WO_NBR
                            ,CASE WHEN V.SHIFTID = 'D' THEN 'A' ELSE 'B' END AS SHIFT_ID
                            ,M.RW_COUNT AS RW_COUNT
                            ,V.TOWOTYPE AS WO_TYPE
                            ,'' AS TRANS_TYPE
                            ,V.NWORKCTR AS TO_OPER
                            ,'' AS MAIN_OPER
                            ,'' AS OUTPUT_FG
                            ,V.TOLOTGRADE AS GRADE
                            ,'' AS LCM_OWNER
                            ,V.CARTID AS CART_ID
                            ,V.CUSTOMERID AS SHIPPING_NBR
                            ,V.NENTITYTQTY QTY
                             FROM VWLTXA V
                            LEFT JOIN (SELECT MAX(M1.RWK_CUM_NUM) RW_COUNT,M1.PANELID FROM RPT_MA21_PANEL_RWK_CNT M1 GROUP BY PANELID) M ON M.PANELID = V.TOENTITYID 
                            LEFT JOIN (SELECT PRODID,SIZEID FROM MPIM) N ON N.PRODID = V.TOPRODID                            
                            WHERE FABID ='B' 
                            AND VERB = 'MVOU'
                            AND V.NWORKCTR LIKE 'MR%'
                            AND TRANSDATE BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
                            AND CWORKCTR = '{row1}'
                            AND TOPRODID = '{S_Prod_Nbr}'
                            AND CSUBLINEID = '{S_Equip_nbr}'
                            AND SHIFTID =  '{S_Shift_Id}'
			                /*{sql_Shift}*/
                            {sql_ShopCD}
                            /*{sql_Floor}*/
                            {sql_WorkOrder}
                            {sql_Size}
                            /*{sql_BigProdProduct}*/
                            /*{sql_Product}*/
                            {sql_Wotype}";
                #endregion

            }
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
            };
            returnValue = ob;
            #endregion
        }
    }
}
