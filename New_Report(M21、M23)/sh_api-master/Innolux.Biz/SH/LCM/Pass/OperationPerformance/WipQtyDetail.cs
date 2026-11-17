using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Org.BouncyCastle.Crypto.Macs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class WipQtyDetail : InnSqlAgent.IApiBase
    {
        public WipQtyDetail()
        {
            parameters["row1"] = "记录用户选择的站点";
            parameters["col0"] = "记录用户点击DataSet 列名";
            parameters["Ver"] = "记录用户选择的机种版本";
            parameters["Prod_Nbr"] = "记录用户选择的機種";
            parameters["Cart_Flag"] = "记录用户选择的 Cart_Flag  /N";

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

            description = "对应OperPerfDetail 2文件";
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
            string col0 = GetParameterValueInString("col0");
            string S_Ver = GetParameterValueInString("Ver");
            string S_Prod_Nbr = GetParameterValueInString("Prod_Nbr");
            string S_Cart_Flag = GetParameterValueInString("Cart_Flag");

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
            string lstr_Product = S_Prod_Nbr;
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
                sql_ShopCD = " and CSHOPCD = '" + lstr_ShopCD + "'  ";
            if (lstr_Floor != "ALL" && lstr_Floor.Trim() != "")
                sql_Floor = " AND '" + lstr_Floor + "'= CASE WHEN W.SUBLINEID IS NULL THEN '' \r\n\t\t\t\t\t\tELSE CASE WHEN SUBSTR(W.SUBLINEID,1,3) = 'SH2' \r\n\t\t\t\t\t\t\t\t\tTHEN 'SH2-2F' \r\n\t\t\t\t\t\t\t\t\t\tELSE 'SH1-' || SUBSTR(W.SUBLINEID,1,2) END END"; //Oracle空串即NULL,ISNULL(x,'')=''等價於x IS NULL
            if (lstr_WorkOrder != "ALL" && lstr_WorkOrder.Trim() != "")
                sql_WorkOrder = " and WONBR in ('" + lstr_WorkOrder.TrimEnd(',').Replace(",", "','") + "')";
            if (lstr_Size != "ALL" && lstr_Size.Trim() != "")
                sql_Size = " and sizeid in ('" + lstr_Size.TrimEnd(',').Replace(",", "','") + "')";
            //if (lstr_BigProdProduct != "ALL" && lstr_BigProdProduct.Trim() != "")
            //    sql_BigProdProduct = " and substring(W.prodid,1,8) in ('" + lstr_BigProdProduct.TrimEnd(',').Replace(",", "','") + "')  ";
            var products = lstr_BigProdProduct.TrimEnd(',').Split(',');
            var likeConditions = new List<string>();

            foreach (var prod in products.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                // 先去掉数据本身可能带的前后单引号和空格，再转义
                var clean = prod.Trim().Trim('\'', ' ', '\t');
                var escaped = clean.Replace("'", "''");
                likeConditions.Add($"W.prodid like '{escaped}%'");
            }

            sql_BigProdProduct = likeConditions.Count > 0
                ? $" and ({string.Join(" or ", likeConditions)}) "
                : "";

            if (lstr_Product != "ALL" && lstr_Product.Trim() != "")
                sql_Product = " and W.prodid in ('" + lstr_Product.TrimEnd(',').Replace(",", "','") + "')  ";
            if (lstr_Wotype != "ALL" && lstr_Wotype.Trim() != "")
                sql_Wotype = " and wotype in ('" + lstr_Wotype.TrimEnd(',').Replace(",", "','") + "')  ";


            string connectDB = string.Empty;
            string table_name1 = string.Empty;
            string sql01 = string.Empty;
            string sql_Ver = string.Empty;
            string sql_OperRange = string.Empty;
            #endregion


            #region TABLE + DB连接
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion

            #region SQL串取

            if (col0.IndexOf("HOLD") >= 0 || col0.IndexOf("hold") >= 0)
            {
                #region 条件串取
                if(S_Prod_Nbr != "合計" && (string.Compare(row1, "2000") < 0) )
                {
                    sql_Product = "  and  prod_nbr =  '" + S_Prod_Nbr + "'  ";
                    sql_Ver = "  and ver  =  '" + S_Ver + "'  ";
                }
                #endregion

                #region SQL
                sql01 = $@"SELECT W.PRODID AS PROD_NBR
                                    ,W.TRANSDATE AS LAST_TRANS_DT
                                    ,W.ENTITYID AS PNL_ID
                                    ,NVL(W.CARTID, '') AS CART_ID
                                    ,'' AS CART_ID_CUST
                                                    , W.SubLineID AS LAST_EQP_NBR
                                    ,W.CWORKCTR AS OPER
                                    ,NVL(R.RWK_COUNT, 0) AS RWK_COUNT
                                    ,CASE WHEN CSHOPCD = 'CELL2' THEN '1' ELSE '0' END AS RESERVED_21
                                                    ,'' AS FAST_RWK_FLG
                                                    ,'' AS CUR_HOLD_CODE
                                    , W.ENTITYTQTY AS QTY
                                    FROM WLTM W
                                    LEFT JOIN(SELECT PANELID, MAX(RWK_CUM_NUM) AS RWK_COUNT FROM RPT_MA21_PANEL_RWK_CNT GROUP BY PANELID) r ON r.PANELID = W.ENTITYID
                                    LEFT JOIN (SELECT PRODID,SIZEID FROM MPIM) N ON N.PRODID = W.PRODID                                      
                                    WHERE FABID ='B'
                                    AND W.HIDEFLG ='0'			                        
                                    AND CWORKCTR = '{row1}'                        
                                    {sql_ShopCD}
                                    {sql_Floor}
                                    {sql_WorkOrder}
                                    {sql_Size}
                                    {sql_BigProdProduct}
                                    {sql_Product}
                                    {sql_Wotype}";
                #endregion
            }
            else if (col0.IndexOf("WIP") >= 0 || col0.IndexOf("wip") >= 0)
            {
                #region sql
                sql01 = $@"SELECT W.PRODID AS PROD_NBR
                                    ,W.TRANSDATE AS LAST_TRANS_DT
                                    ,W.ENTITYID AS PNL_ID
                                    ,NVL(W.CARTID, '') AS CART_ID
                                    ,'' AS CART_ID_CUST
                                                    , W.SubLineID AS LAST_EQP_NBR
                                    ,W.CWORKCTR AS OPER
                                    ,NVL(R.RWK_COUNT, 0) AS RWK_COUNT
                                    ,CASE WHEN CSHOPCD = 'CELL2' THEN '1' ELSE '0' END AS RESERVED_21
                                                    ,'' AS FAST_RWK_FLG
                                                    ,'' AS CUR_HOLD_CODE
                                    , W.ENTITYTQTY AS QTY
                                    FROM WLTM W
                                    LEFT JOIN(SELECT PANELID, MAX(RWK_CUM_NUM) AS RWK_COUNT FROM RPT_MA21_PANEL_RWK_CNT GROUP BY PANELID) r ON r.PANELID = W.ENTITYID
                                    LEFT JOIN (SELECT PRODID,SIZEID FROM MPIM) N ON N.PRODID = W.PRODID                                     
                                    WHERE FABID ='B'
                                    AND W.HIDEFLG ='0'			                        
                                    AND CWORKCTR = '{row1}'                        
                                    {sql_ShopCD}
                                    {sql_Floor}
                                    {sql_WorkOrder}
                                    {sql_Size}
                                    {sql_BigProdProduct}
                                    {sql_Product}
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

            #region datatable格式准备
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (string.Compare(ds.Tables[0].Rows[i]["RESERVED_21"].ToString(), "0") == 1)
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
