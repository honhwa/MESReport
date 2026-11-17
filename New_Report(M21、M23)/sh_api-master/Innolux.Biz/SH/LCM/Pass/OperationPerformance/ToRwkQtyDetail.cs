using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class ToRwkQtyDetail : InnSqlAgent.IApiBase
    {
        public ToRwkQtyDetail()
        {
            parameters["row1"] = "记录用户选择的站点";
            parameters["code"] = "记录用户选择的 code";

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

            description = "对应OperPerfDetail 4文件";
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

            string code = GetParameterValueInString("code");

            string lstr_Shift = GetParameterValueInString("lstr_Shift");
            string lstr_ShopCD = GetParameterValueInString("lstr_ShopCD");
            string lstr_Floor = GetParameterValueInString("lstr_Floor");
            string lstr_WorkOrder = GetParameterValueInString("lstr_WorkOrder");
            string lstr_Size = GetParameterValueInString("lstr_Size");
            string lstr_BigProdProduct = GetParameterValueInString("lstr_BigProdProduct");
            string lstr_Product = GetParameterValueInString("lstr_Product");
            string lstr_Wotype = GetParameterValueInString("lstr_Wotype");

            string sql_code = string.Empty;

            string sql_Shift = string.Empty;
            string sql_ShopCD = string.Empty;
            string sql_Floor = string.Empty;
            string sql_WorkOrder = string.Empty;
            string sql_Size = string.Empty;
            string sql_BigProdProduct = string.Empty;
            string sql_Product = string.Empty;
            string sql_Wotype = string.Empty;

            if (lstr_Shift != "ALL" && lstr_Shift.Trim() != "")
                sql_Shift = " AND shiftid = '" + (lstr_Shift == "A" ? "D" : "N") + "'";
            if (lstr_ShopCD != "ALL" && lstr_ShopCD.Trim() != "")
                sql_ShopCD = " AND nshopcd = '" + lstr_ShopCD + "'";
            if (lstr_Floor != "ALL" && lstr_Floor.Trim() != "")
                sql_Floor = " AND '" + lstr_Floor + "'= CASE WHEN SUBSTR((CASE WHEN W.VERB='MVIN' THEN NVL(W.NSUBLINEID,'') ELSE NVL(W.CSUBLINEID,'') END),1,3) IS NULL THEN '' \r\n\t\t\t\t\t\t\tELSE CASE WHEN SUBSTR((CASE WHEN W.VERB='MVIN' THEN NVL(W.NSUBLINEID,'') ELSE NVL(W.CSUBLINEID,'') END),1,3) = 'SH2' \r\n\t\t\t\t\t\t\t\t\t\tTHEN 'SH2-2F' \r\n\t\t\t\t\t\t\t\t\t\t\tELSE 'SH1-' || SUBSTR((CASE WHEN W.VERB='MVIN' THEN NVL(W.NSUBLINEID,'')ELSE NVL(W.CSUBLINEID,'') END),1,2) END END"; //Oracle空串即NULL,原外層判斷為= ''
            if (lstr_WorkOrder != "ALL" && lstr_WorkOrder.Trim() != "")
                sql_WorkOrder = " AND TOWONBR in ('" + lstr_WorkOrder.TrimEnd(',').Replace(",", "','") + "')";
            if (lstr_Size != "ALL" && lstr_Size.Trim() != "")
                sql_Size = " AND sizeid in ('" + lstr_Size.TrimEnd(',').Replace(",", "','") + "')";
            //if (lstr_BigProdProduct != "ALL" && lstr_BigProdProduct.Trim() != "")
            //    sql_BigProdProduct = " AND substring(toprodid,1,8) in ('" + lstr_BigProdProduct.TrimEnd(',').Replace(",", "','") + "')";
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
                sql_Product = " AND toprodid in ('" + lstr_Product.TrimEnd(',').Replace(",", "','") + "')";
            if (lstr_Wotype != "ALL" && lstr_Wotype.Trim() != "")
                sql_Wotype = " AND towotype in ('" + lstr_Wotype.TrimEnd(',').Replace(",", "','") + "')";

            string connectDB = string.Empty;
            string table_name1 = string.Empty;
            string sql01 = string.Empty;
            string sql_OperRange = string.Empty;
            #endregion

            #region TABLE + DB连接
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion


            #region sql
            //defect code detail
            #region 条件
            if (code != "合計")
            {
                sql_code = "  and DEFECTCD='" + code + "'  ";
            }
            #endregion

            sql01 = $@"SELECT W.TOPRODID AS PROD_NBR
                        ,W.TOENTITYID AS ENTITY_NO
                        ,W.TRANSDATE AS TRANS_DATE
                        ,'' AS TRANS_TYPE
                        ,W.NSUBLINEID AS EQUIP_NBR_NBR
                        ,USERID AS OPERATOR
                        ,'' AS OUTPUT_FG
                        ,W.CWORKCTR AS WORK_CTR
                        ,W.NWORKCTR AS TO_WC
                        ,W.DEFECTCD AS DFCT_CODE
                        ,'' AS STIMES_TTL
                        ,'' AS ITEM_NBR
                        ,NENTITYTQTY AS PASS_QTY
                        FROM VWLTXA W 
                        LEFT JOIN (SELECT PANELID,MAX(RWK_CUM_NUM) AS RWK_COUNT FROM RPT_MA21_PANEL_RWK_CNT GROUP BY PANELID) r ON r.PANELID = W.TOENTITYID
                        LEFT JOIN (SELECT PRODID,SIZEID FROM MPIM) N ON N.PRODID = W.TOPRODID                         
                        WHERE FABID ='B'
                        AND VERB = 'MVOU'                        
                        AND NWORKCTR LIKE 'MR%'
			            AND TRANSDATE BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
			            AND CWORKCTR = '{row1}'
                        AND W.DEFECTCD = '{code}'
                        {sql_Shift}
                        {sql_ShopCD}
                        {sql_WorkOrder}
                        {sql_Size}
                        {sql_BigProdProduct}
                        {sql_Product}
                        {sql_Wotype}
                        {sql_Floor}";
            
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
