using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class ReworkInOutDetail:InnSqlAgent.IApiBase
    {
        public ReworkInOutDetail()
        {
            parameters["row1"] = "记录用户dataset页面选择的站点";
            parameters["col1"] = "记录用户dataset页面选择的列 index";
            parameters["Operation"] = "记录用户前一阶(reworkInOut)页面选择的站点";

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

            description = "对应OperPerfDetail 7文件";
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
            //前一阶用户选择条件
            string row1 = GetParameterValueInString("row1");
            string col1 = GetParameterValueInString("col1");
            string Operation = GetParameterValueInString("Operation");


            //dataset传递条件
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
            if (lstr_BigProdProduct != "ALL" && lstr_BigProdProduct.Trim() != "")
                sql_BigProdProduct = " and SUBSTR(toprodid,1,3) in ('" + lstr_BigProdProduct.TrimEnd(',').Replace(",", "','") + "')  ";
            if (lstr_Product != "ALL" && lstr_Product.Trim() != "")
                sql_Product = " and toprodid in ('" + lstr_Product.TrimEnd(',').Replace(",", "','") + "')  ";
            if (lstr_Wotype != "ALL" && lstr_Wotype.Trim() != "")
                sql_Wotype = " and towotype in ('" + lstr_Wotype.TrimEnd(',').Replace(",", "','") + "')  ";

            string connectDB = string.Empty;
            string table_name1 = string.Empty;
            string sql01 = string.Empty;
            string sql_OperRange = string.Empty;
            #endregion


            #region TABLE + DB连接
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion

            #region sql条件
            if (col1 == "1")
            {
                sql_OperRange = " AND to_oper = " + row1 + " ";
                if (Operation != "合計")
                {
                    sql_OperRange = sql_OperRange + " AND oper =  " + Operation + "   ";
                }
            }
            else if (col1 == "2")
            {
                sql_OperRange = "  AND to_oper <> 4000  AND ( main_oper = " + row1 + "   OR oper = " + row1 + " ) ";
                if (Operation != "合計")
                {
                    sql_OperRange = sql_OperRange + " AND to_oper =  " + Operation + "   ";
                }
            }
            else if (col1 == "3")
            {
                sql_OperRange = "  AND to_oper <> 4000    AND rw_count > 1  AND ( main_oper = " + row1 + "   OR oper = " + row1 + " ) ";
                if (Operation != "合計")
                {
                    sql_OperRange = sql_OperRange + " AND to_oper =  " + Operation + "   ";
                }
            }
            #endregion

            #region SQL
            sql01 = $@"SELECT V.TOPRODID AS PROD_NBR
                        ,V.MFGDATE AS ACCT_DATE
                        ,V.TOENTITYID AS PNL_ID
                        ,'' AS TRANS_TYPE
                        ,'' AS TRANS_NBR
                        ,V.NSUBLINEID AS EQUIP_NBR
                        ,'' AS OPERATORS
                        ,CASE WHEN V.VERB =  'MVOU' THEN 'Y' ELSE 'N' END AS OUTPUT_FG
                        ,V.CWORKCTR AS OPER
                        ,V.NWORKCTR AS TO_OPER
                        ,R.RW_COUNT AS RW_COUNT
                        ,'' AS STIMES_TTL
                        ,'' AS RESERVED_21
                        ,'' AS SERIAL_NO
                        ,'' AS RMA_NBR
                        ,COUNT(0) AS QTY
                         FROM VWLTXA V 
                        LEFT JOIN (SELECT PANELID,MAX(RWK_CUM_NUM) AS RW_COUNT FROM RPT_MA21_PANEL_RWK_CNT GROUP BY PANELID) R ON R.PANELID = V.TOENTITYID
                        WHERE FABID = 'B'
                        AND TRANSDATE BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
                        AND CWORKCTR = '{row1}'                        
                        {sql_Shift}
                        {sql_ShopCD}
                        {sql_Floor}
                        {sql_WorkOrder}
                        {sql_Size}
                        {sql_BigProdProduct}
                        {sql_Product}
                        {sql_Wotype}
                        GROUP BY V.TOPRODID
                        ,V.MFGDATE
                        ,V.TOENTITYID
                        ,V.NSUBLINEID
                        ,V.NWORKCTR
                        ,V.VERB
                        ,V.CWORKCTR
                        ,R.RW_COUNT";
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

            #region datatable格式准备
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (string.Compare(ds.Tables[0].Rows[i]["RESERVED_21"].ToString(), "0") > 1)
                {
                    ds.Tables[0].Rows[i]["RESERVED_21"] = "模組";
                }
                else
                {
                    ds.Tables[0].Rows[i]["RESERVED_21"] = "單玻";
                }
            }
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
