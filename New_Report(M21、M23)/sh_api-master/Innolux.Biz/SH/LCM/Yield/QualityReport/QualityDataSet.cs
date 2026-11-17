using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using StackExchange.Redis;
namespace Innolux.Biz.SH.LCM.Yield.QualityReport
{
    public class QualityDataSet : InnSqlAgent.IApiBase
    {
        public QualityDataSet()
        {
            //傳入參教及API說明
            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "C/E";

            parameters["FromDate"] = "开始时间(B)";
            parameters["ToDate"] = "结束时间(B)";

            parameters["Shift"] = "ALL/D/N(B)";
            parameters["ShopCD"] = "ALL/Cell2/Module";
            parameters["Floor"] = "ALL/SH1-1F/SH1-2F/SH1-3F/SH1-4F/SH2-2F";
            parameters["Wo_nbr"] = "工單";
            parameters["Size"] = "尺寸";
            parameters["PROJECT_NAME"] = "型號";
            parameters["Product"] = "機種前八位";
            parameters["Prod_nbr"] = "點選機種(B)";
            parameters["WoType"] = "工單類型";

            parameters["ShowMode"] = "显示方式  work_ctr/ equip_nbr/ big_prod/ product_id";
            parameters["SourceFab"] = "来源厂别";
            parameters["Reworked"] = "reworked"; /*Rework次數*/
            parameters["Operator"] = "人员工号";
            //parameters["WoType"] = "工单性质选项分类";
            parameters["Operation"] = "Operation";
            parameters["Equipment"] = "Equipment";
            parameters["DefectCode"] = "defect code";
            parameters["SpecialCode"] = "MA23 需要排除的特定的defect code";

            description = "STQualityDataSet 报表";
        }

        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Data_SH(fab);
                    return;
                default:
                    returnValue = "请填写厂别";
                    return;
            }
        }

        private void Get_Data_SH(string fab)
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
            string lstr_BigProdProduct = GetParameterValueInString("Product");
            string lstr_Product = GetParameterValueInString("Prod_nbr");
            string lstr_Wotype = GetParameterValueInString("WoType");

            string lstr_ShowMode = GetParameterValueInString("ShowMode");
            string lstr_SourceFab = GetParameterValueInString("SourceFab");
            string lstr_ReworkCnt = GetParameterValueInString("Reworked");
            string lstr_Operator = GetParameterValueInString("Operator");
            string lstr_Operation = GetParameterValueInString("Operation");
            string lstr_Equipment = GetParameterValueInString("Equipment");
            string lstr_DefectCode = GetParameterValueInString("DefectCode");
            string lstr_SpecialCode = GetParameterValueInString("SpecialCode");

            //暂存条件sql条件
            string sql_Shift = string.Empty;
            string sql_ShopCD = string.Empty;
            string sql_Floor = string.Empty;
            string sql_WorkOrder = string.Empty;
            string sql_Size = string.Empty;
            string sql_BigProduct = string.Empty;
            string sql_Product = string.Empty;
            string sql_Wotype = string.Empty;
            string sql_ReworkCnt = string.Empty;
            string sql_Operator = string.Empty;
            string sql_Operation = string.Empty;
            string sql_Equipment = string.Empty;
            string sql_DefectCode1 = string.Empty;
            string sql_DefectCode2 = string.Empty;
            string sql_SourceFab = string.Empty;

            //where条件暂存
            string sql_pass_where = string.Empty;
            string sql_dfct_where = string.Empty;
            string STR_INDEX_PAS = string.Empty;
            string STR_INDEX_DFCD = string.Empty;

            string sql = string.Empty;
            string connectDB = string.Empty;
            string? pass_cuttime = string.Empty;
            string? wip_cuttime = string.Empty;
            string lstr_index = string.Empty;
            string SQL01 = string.Empty;
            string SQL03 = string.Empty;
            //传递给下一阶页面条件
            string col0 = string.Empty;
            string col1 = string.Empty;
            string row0 = string.Empty;
            string row1 = string.Empty;
            #endregion

            #region 条件串取
            //班别
            if (lstr_Shift != "ALL" && lstr_Shift.Trim() != "")
                sql_Shift = " and Shift_Id = '" + lstr_Shift + "'  ";
            //大段別
            if (lstr_ShopCD != "ALL" && lstr_ShopCD.Trim() != "")
            {
                sql_ShopCD = " and n.SHOPCD = '" + lstr_ShopCD + "'  ";
            }
            //楼层
            if (lstr_Floor != "ALL" && lstr_Floor.Trim() != "")
            {
                sql_Floor = @$" AND SUBLINEID IN (SELECT SUBLINEID 
                                                    FROM MSLM 
                                                   WHERE FABID = 'B' 
                                                     AND '{lstr_Floor}' = (CASE WHEN MCSTOCKID IN ('5103', '51A3') THEN 'SH2-2F'
                                                                                WHEN MCSTOCKID IN ('5101', '51A1') THEN 'SH1-1F'
                                                                                WHEN MCSTOCKID IN ('5105', '51A5') THEN 'SH1-2F'
                                                                                WHEN MCSTOCKID IN ('5107', '51A7') THEN 'SH1-3F'
                                                                                WHEN MCSTOCKID IN ('5109', '51A9') THEN 'SH1-4F' END))  ";
            }
            //工單號
            if (lstr_WorkOrder != "ALL" && lstr_WorkOrder.Trim() != "")
            {
                sql_WorkOrder = " and WORK_ORDER in ('" + lstr_WorkOrder.TrimEnd(',') + "')";
            }
            //大機種
            //if (lstr_BigProdProduct != "ALL" && lstr_BigProdProduct.Trim() != "")
            //{
            //    sql_BigProduct = " and SUBSTRING(n.T9NCNO,1,8) in ('" + lstr_BigProdProduct.TrimEnd(',') + "')  ";
            //}
            var products = lstr_BigProdProduct.TrimEnd(',').Split(',');
            var likeConditions = new List<string>();

            foreach (var prod in products.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                // 先去掉数据本身可能带的前后单引号和空格，再转义
                var clean = prod.Trim().Trim('\'', ' ', '\t');
                var escaped = clean.Replace("'", "''");
                likeConditions.Add($"n.T9NCNO like '{escaped}%'");
            }

            sql_BigProduct = likeConditions.Count > 0
                ? $" and ({string.Join(" or ", likeConditions)}) "
                : "";
            // PRODUCT
            if (lstr_Product != "ALL" && lstr_Product.Trim() != "")
            {
                sql_Product = " and n.T9NCNO in ( '" + lstr_Product.TrimEnd(',') + "' ) ";
            }
            //尺寸
            if (lstr_Size != "ALL" && lstr_Size.Trim() != "")
                sql_Size = " and p.SIZEID in ('" + lstr_Size.TrimEnd(',') + "')  ";
            //sql_Size = " and SizeId in ('" + lstr_Size.TrimEnd(',').Replace(",", "','") + "')  ";
            // 工單類型
            if (lstr_Wotype != "ALL" && lstr_Wotype.Trim() != "")
            {
                sql_Wotype = " AND sw.WOTYPE in ( '" + lstr_Wotype.TrimEnd(',') + "' ) ";
            }
            // 来源厂别
            if (lstr_SourceFab != "ALL" && lstr_SourceFab.Trim() != "")
            {
                //sql_SourceFab = @$" and (CASE WHEN PALLETID = '' THEN '' 
                //                                ELSE SUBSTRING(PALLETID,1,CASE WHEN CHARINDEX(' ',PALLETID) = 0 THEN 
                //                                                LEN(PALLETID) ELSE CHARINDEX(' ',PALLETID) - 1 END) 
                //                            END) in ( '" + lstr_SourceFab.TrimEnd(',') + "' ) ";
                sql_SourceFab = " AND m.SourceFab in ( '" + lstr_SourceFab.TrimEnd(',') + "')";
            }
            //// Rework次數
            //if (lstr_ReworkCnt != "ALL" && lstr_ReworkCnt.Trim() != "")
            //{
            //    sql_ReworkCnt = "  and REWORK_CNT = '" + lstr_ReworkCnt + "' ";
            //}
            string sql_DefectCode = ",SUM(CASE WHEN (t.LOT_GRADE = 'S'";
            string sql_Rework_Where = " AND (LOT_GRADE = 'S'";
            if (lstr_ReworkCnt == "No" || lstr_ReworkCnt == "N")
            {
                sql_DefectCode += " OR (t.LOT_GRADE = 'A' AND t.DEF_CODE IS NOT NULL))"; //Oracle空串即NULL,原為 <> ''
                sql_Rework_Where += " OR (LOT_GRADE = 'A' AND DEF_CODE IS NOT NULL))"; //Oracle空串即NULL,原為 <> ''
            }
            else
            {
                sql_DefectCode += ")";
                sql_Rework_Where += ")";
            }
            // 站點
            if (lstr_Operation != "ALL" && lstr_Operation.Trim() != "")
            {
                sql_Operation = " and OPERATION_ID in ( '" + lstr_Operation.TrimEnd(',') + "' ) ";
            }
            // 機台(SH為SubLineId)
            if (lstr_Equipment != "ALL" && lstr_Equipment.Trim() != "")
            {
                sql_Equipment = " and SUBLINEID in ( '" + lstr_Equipment.TrimEnd(',') + "' ) ";
            }
            // 查詢DefectCode
            //string sql_DefectCode = ",SUM(CASE WHEN t.LOT_GRADE = 'S'";
            if (lstr_DefectCode != "ALL" && lstr_DefectCode.Trim() != "")
            {
                sql_DefectCode1 = " and DEF_CODE in ( '" + lstr_DefectCode.TrimEnd(',') + "' ) ";
                sql_DefectCode += sql_DefectCode1;
            }
            // 排除DefectCode
            if (lstr_SpecialCode != "ALL" && lstr_SpecialCode.Trim() != "")
            {
                sql_DefectCode2 = " and DEF_CODE not in ( '" + lstr_SpecialCode.TrimEnd(',') + "' ) ";
                sql_DefectCode += sql_DefectCode2;
            }
            // 作業人員
            if (lstr_Operator != "ALL" && lstr_Operator.Trim() != "")
            {
                sql_Operator = " and OPERATOR_ID in ( '" + lstr_Operator.TrimEnd(',') + "' ) ";
                sql_DefectCode += sql_Operator;
            }
            //string sql_SpecialCode = string.Empty;
            string sql_reject_qty = sql_DefectCode + " THEN T.DEF_CNT ELSE 0 END)";
            string sql_reject_qty1 = sql_reject_qty;

            sql_dfct_where = sql_Shift + sql_ShopCD + sql_Floor + sql_Wotype + sql_WorkOrder + sql_BigProduct
                + sql_Product + sql_SourceFab + sql_ReworkCnt + sql_Operator + sql_Operation + sql_Equipment;
            #endregion

            #region DB + table
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
            {
                connectDB = "MESHIS";
                //TABLE_NAME4 = "mcrda1.bs_defcode_new";
                //STR_INDEX_PAS = "/*+index (a MCSPAS_SOUTPUT1_IDX2)+*/";
                //STR_INDEX_DFCD = "/*+index(a MCSHIS_DEFECTCODE_IDX1)+*/";
            }
            #endregion

            #region SQL
            //3种模式   work_ctr / equip_nbr(SH為SubLineId) / big_prod
            if (lstr_ShowMode == "work_ctr")
            {
                SQL01 = $@"SELECT WORK_NAME
                            ,t.OPERATION_ID AS WORK_CTR
                            ,SUM(t.DEF_CNT) AS QTY
                            {sql_reject_qty} AS REJECT_QTY
                            {sql_reject_qty1} AS REJECT_QTY1 
                           FROM (SELECT n.OPERATION_ID || '  ' || mw.WORKNAME AS work_name, n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MWCM mw,MPIM mp, SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
                                        AND n.FAB_ID = 'B'
                                        {sql_dfct_where}
                            ) t GROUP BY t.work_name,t.OPERATION_ID
                                ORDER BY t.OPERATION_ID ASC";

                SQL03 = $@"select T.DEF_CODE AS DFCT_CODE
                                    ,t.OPERATION_ID AS WORK_CTR
                                    ,t.DEFECTDESC AS ERRC_DESCR
                                    ,SUM(t.DEF_CNT) AS MAIN_QTY
                                    ,0 AS SECOND_QTY
                            from (select md.DEFECTDESC,n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MDCM md,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = md.FABID AND n.DEF_CODE = md.DEFECTCD
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and n.FAB_ID = 'B'
				                            {sql_dfct_where}
                                            /*AND LOT_GRADE = 'S'*/
                                            {sql_Rework_Where}
                                            {sql_DefectCode1}
                                            {sql_DefectCode2}
				                            ) T
                            GROUP BY OPERATION_ID,T.DEF_CODE,t.DEFECTDESC 
                            ORDER BY SUBSTR(T.DEF_CODE,3,1) ASC, OPERATION_ID ASC ";
            }
            else if (lstr_ShowMode == "equip_nbr")
            {
                SQL01 = $@"SELECT t.SUBLINEID AS work_name
                            ,t.SUBLINEID AS work_ctr
                            ,SUM(t.DEF_CNT) AS qty
                            {sql_reject_qty} AS reject_qty
                            {sql_reject_qty1} AS reject_qty1 
                           FROM (SELECT n.OPERATION_ID || '  ' || mw.WORKNAME AS work_name, n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
                                        AND n.FAB_ID = 'B'
                                        {sql_dfct_where}
                            ) t GROUP BY t.SUBLINEID,t.OPERATION_ID
                                ORDER BY t.SUBLINEID ASC";

                SQL03 = $@"select T.DEF_CODE AS DFCT_CODE
                                    ,t.SUBLINEID AS WORK_CTR
                                    ,t.DEFECTDESC AS ERRC_DESCR
                                    ,SUM(t.DEF_CNT) AS MAIN_QTY
                                    ,0 AS SECOND_QTY
                            from (select md.DEFECTDESC,n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MDCM md,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = md.FABID AND n.DEF_CODE = md.DEFECTCD
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and n.FAB_ID = 'B'
				                            {sql_dfct_where}
                                            /*AND LOT_GRADE = 'S'*/
                                            {sql_Rework_Where}
                                            {sql_DefectCode1}
                                            {sql_DefectCode2}
				                            ) T
                            GROUP BY t.SUBLINEID,T.DEF_CODE,t.DEFECTDESC 
                            ORDER BY SUBSTR(T.DEF_CODE,3,1) ASC, t.SUBLINEID ASC ";

            }
            else if (lstr_ShowMode == "big_prod")
            {
                SQL01 = $@"SELECT SUBSTR(t.T9NCNO,1,8) AS work_name
                            ,SUBSTR(t.T9NCNO,1,8) AS work_ctr
                            ,SUM(t.DEF_CNT) AS qty
                            {sql_reject_qty} AS reject_qty
                            {sql_reject_qty1} AS reject_qty1 
                           FROM (SELECT n.OPERATION_ID || '  ' || mw.WORKNAME AS work_name, n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
                                        AND n.FAB_ID = 'B'
                                        {sql_dfct_where}
                            ) t GROUP BY SUBSTR(t.T9NCNO,1,8),t.OPERATION_ID
                                ORDER BY SUBSTR(t.T9NCNO,1,8) ASC";

                SQL03 = $@"select T.DEF_CODE AS DFCT_CODE
                                    ,SUBSTR(t.T9NCNO,1,8) AS WORK_CTR
                                    ,t.DEFECTDESC AS ERRC_DESCR
                                    ,SUM(t.DEF_CNT) AS MAIN_QTY
                                    ,0 AS SECOND_QTY
                            from (select md.DEFECTDESC,n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MDCM md,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = md.FABID AND n.DEF_CODE = md.DEFECTCD
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and n.FAB_ID = 'B'
				                            {sql_dfct_where}
                                            /*AND LOT_GRADE = 'S'*/
                                            {sql_Rework_Where}
                                            {sql_DefectCode1}
                                            {sql_DefectCode2}
				                            ) T
                            GROUP BY SUBSTR(t.T9NCNO,1,8),T.DEF_CODE,T.DEFECTDESC --原M.DEFECTDESC為筆誤(外層無別名M),修正為T.DEFECTDESC
                            ORDER BY SUBSTR(T.DEF_CODE,3,1) ASC, SUBSTR(t.T9NCNO,1,8) ASC "; //改用GROUP BY運算式排序(原t.T9NCNO)
            }
            else if (lstr_ShowMode == "product_id")
            {
                SQL01 = $@"SELECT t.product_id AS work_name 
                            ,t.product_id AS work_ctr 
                            ,SUM(t.DEF_CNT) AS qty 
                            {sql_reject_qty} AS reject_qty 
                            {sql_reject_qty1} AS reject_qty1 
                           FROM (SELECT n.OPERATION_ID || '  ' || mw.WORKNAME AS work_name, n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS')
                                        AND n.FAB_ID = 'B'
                                        {sql_dfct_where}
                            ) t GROUP BY t.product_id,t.OPERATION_ID
                                ORDER BY t.product_id ASC";

                SQL03 = $@"select T.DEF_CODE AS dfct_code
                                    ,t.product_id AS work_ctr
                                    ,t.DEFECTDESC AS errc_descr
                                    ,SUM(t.DEF_CNT) AS main_qty
                                    ,0 AS second_qty
                            from (select md.DEFECTDESC,n.* FROM NJ_TBL_LIGHTON_DEFECT n,MSFM ms,MDCM md,MPIM mp,SWOM sw
                                    WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = md.FABID AND n.DEF_CODE = md.DEFECTCD
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate BETWEEN TO_DATE('{lstr_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{lstr_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and n.FAB_ID = 'B'
				                            {sql_dfct_where}
                                            /*AND LOT_GRADE = 'S'*/
                                            {sql_Rework_Where}
                                            {sql_DefectCode1}
                                            {sql_DefectCode2}
				                            ) T
                            GROUP BY t.product_id,T.DEF_CODE,t.DEFECTDESC 
                            ORDER BY SUBSTR(T.DEF_CODE,3,1) ASC, t.product_id ASC ";
            }
            ////单独串写 内部快修类型资料
            #region
            //if (lstr_FastRework == "8")
            //{
            //    if (lstr_ShowMode == "work_ctr")
            //    {
            //        SQL01 = "SELECT work_name, p.work_ctr, '' prod_nbr,p.qty, nvl(d.reject_qty, 0) reject_qty, nvl(d.reject_qty1, 0) reject_qty1  from ";
            //        SQL01 = SQL01 + " ( SELECT 'ASSEMBLY' work_name,a.main_wc work_ctr,count(*)as qty  From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')  group by a.main_wc) p   left join (   select sum(reject_qty)reject_qty,0 reject_qty1, work_ctr from ";
            //        SQL01 = SQL01 + " ( SELECT entity_NO,COUNT(*)-1 reject_qty,main_wc work_ctr From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400    AND FAST_RWK_FLG='C'  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73') ";
            //        SQL01 = SQL01 + " GROUP BY entity_NO,main_wc HAVING COUNT(*)>1) group by work_ctr ) d on p.work_ctr = trim(d.work_ctr) ";


            //        SQL03 = "SELECT dfct_code, work_ctr,SF_A4R_GETERRC_DESC(substr(dfct_code, 1, 5)) errc_descr,  count(*)main_qty, 0 second_qty  From  (";
            //        SQL03 = SQL03 + " select entity_NO,dfct_code,work_ctr,trans_date, ROW_NUMBER() OVER(PARTITION BY entity_NO ORDER BY trans_date ASC) rn From " + table_name3 + " a ";
            //        SQL03 = SQL03 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL03 = SQL03 + " AND main_wc=1400  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')   AND FAST_RWK_FLG='C'  ";
            //        SQL03 = SQL03 + "  and entity_NO in ( SELECT entity_NO  From " + table_name3 + " a ";
            //        SQL03 = SQL03 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL03 = SQL03 + " AND main_wc=1400  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')   AND FAST_RWK_FLG='C'  ";
            //        SQL03 = SQL03 + " GROUP BY entity_NO, main_wc   HAVING COUNT(*) > 1)    ) where rn>1   ";
            //        SQL03 = SQL03 + "  group by dfct_code, work_ctr,SF_A4R_GETERRC_DESC(substr(dfct_code, 1, 5)) ";

            //    }
            //    else if (lstr_ShowMode == "equip_nbr")
            //    {
            //        SQL01 = "SELECT work_name, p.work_ctr,p.prod_nbr, p.qty, nvl(d.reject_qty, 0) reject_qty, nvl(d.reject_qty1, 0) reject_qty1  from ";
            //        SQL01 = SQL01 + " ( SELECT trim(equip_nbr) as work_name, trim(equip_nbr) work_ctr, prod_nbr,count(*) as qty  From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400   and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73') group by trim(equip_nbr) , prod_nbr) p   left join ( select count(*) reject_qty, 0 reject_qty1, work_ctr,prod_nbr from ";
            //        SQL01 = SQL01 + " ( SELECT entity_NO,trans_date, equip_nbr as work_ctr,prod_nbr,ROW_NUMBER() OVER(PARTITION BY entity_NO ORDER BY trans_date ASC) rn From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400    AND FAST_RWK_FLG='C'  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73') ";
            //        SQL01 = SQL01 + " and entity_NO in ( SELECT entity_NO From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400    AND FAST_RWK_FLG='C'  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')  group by entity_NO HAVING COUNT(*)>1 ) )";
            //        SQL01 = SQL01 + "  where rn>1   GROUP BY work_ctr ,prod_nbr ) d  on p.work_ctr = trim(d.work_ctr)  and p.prod_nbr=d.prod_nbr order by p.work_ctr ";


            //        SQL03 = "SELECT dfct_code, work_ctr,SF_A4R_GETERRC_DESC(substr(dfct_code, 1, 5)) errc_descr,  count(*)main_qty, 0 second_qty  From  (";
            //        SQL03 = SQL03 + " select entity_NO,dfct_code,equip_nbr as work_ctr,trans_date, ROW_NUMBER() OVER(PARTITION BY entity_NO ORDER BY trans_date ASC) rn From " + table_name3 + " a ";
            //        SQL03 = SQL03 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL03 = SQL03 + " AND main_wc=1400   and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')   AND FAST_RWK_FLG='C'  ";
            //        SQL03 = SQL03 + "  and entity_NO in ( SELECT entity_NO  From " + table_name3 + " a ";
            //        SQL03 = SQL03 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL03 = SQL03 + " AND main_wc=1400  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')   AND FAST_RWK_FLG='C'  ";
            //        SQL03 = SQL03 + " GROUP BY entity_NO, main_wc   HAVING COUNT(*) > 1)    ) where rn>1   ";
            //        SQL03 = SQL03 + "  group by dfct_code, work_ctr,SF_A4R_GETERRC_DESC(substr(dfct_code, 1, 5))  order by dfct_code, work_ctr ";
            //    }
            //    else if (lstr_ShowMode == "big_prod")
            //    {
            //        SQL01 = "SELECT work_name, p.work_ctr,p.qty, nvl(d.reject_qty, 0) reject_qty, nvl(d.reject_qty1, 0) reject_qty1  from ";
            //        SQL01 = SQL01 + " ( SELECT case  when length(a.prod_nbr) = 10 then substr(a.prod_nbr, 1, 5)  when length(a.prod_nbr) = 13 then  substr(a.prod_nbr, 1, 8)  end work_name,  ";
            //        SQL01 = SQL01 + "case when length(a.prod_nbr) = 10 then  substr(a.prod_nbr, 1, 5)  when length(a.prod_nbr) = 13 then   substr(a.prod_nbr, 1, 8)  end work_ctr,count(*) as qty  From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400   and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73') group by case when length(a.prod_nbr) = 10 then substr(a.prod_nbr, 1, 5)  when length(a.prod_nbr) = 13 then substr(a.prod_nbr, 1, 8) end ) p  ";
            //        SQL01 = SQL01 + " left join ( select count(*) reject_qty, 0 reject_qty1, work_ctr from ";
            //        SQL01 = SQL01 + " ( SELECT entity_NO,trans_date, case when length(a.prod_nbr) = 10 then substr(a.prod_nbr, 1, 5) when length(a.prod_nbr) = 13 then substr(a.prod_nbr, 1, 8) end  as work_ctr,prod_nbr, ";
            //        SQL01 = SQL01 + " ROW_NUMBER() OVER(PARTITION BY entity_NO ORDER BY trans_date ASC) rn From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //           + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400    AND FAST_RWK_FLG='C'  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73') ";
            //        SQL01 = SQL01 + " and entity_NO in ( SELECT entity_NO From " + table_name3 + " a ";
            //        SQL01 = SQL01 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL01 = SQL01 + " AND main_wc=1400    AND FAST_RWK_FLG='C'  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')  group by entity_NO HAVING COUNT(*)>1 ) )";
            //        SQL01 = SQL01 + "  where rn>1   GROUP BY work_ctr  ) d  on p.work_ctr = trim(d.work_ctr)   order by p.work_ctr ";


            //        SQL03 = "SELECT dfct_code, work_ctr,SF_A4R_GETERRC_DESC(substr(dfct_code, 1, 5)) errc_descr,  count(*)main_qty, 0 second_qty  From  (";
            //        SQL03 = SQL03 + " select entity_NO,dfct_code,case when length(a.prod_nbr) = 10 then substr(a.prod_nbr, 1, 5) when length(a.prod_nbr) = 13 then substr(a.prod_nbr, 1, 8) END as work_ctr,trans_date, ";
            //        SQL03 = SQL03 + " ROW_NUMBER() OVER(PARTITION BY entity_NO ORDER BY trans_date ASC) rn From " + table_name3 + " a ";
            //        SQL03 = SQL03 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL03 = SQL03 + " AND main_wc=1400  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')   AND FAST_RWK_FLG='C'  ";
            //        SQL03 = SQL03 + "  and entity_NO in ( SELECT entity_NO  From " + table_name3 + " a ";
            //        SQL03 = SQL03 + "  where a.fac_id='" + lstr_G_FAC + "' "
            //            + sql_dfct_where;//defect 限制条件
            //        SQL03 = SQL03 + " AND main_wc=1400  and dfct_code in ('PCU10','PCU11','PCU12','PCU16','PCV73')   AND FAST_RWK_FLG='C'  ";
            //        SQL03 = SQL03 + " GROUP BY entity_NO, main_wc   HAVING COUNT(*) > 1)    ) where rn>1   ";
            //        SQL03 = SQL03 + "  group by dfct_code, work_ctr,SF_A4R_GETERRC_DESC(substr(dfct_code, 1, 5))  order by dfct_code, work_ctr ";
            //    }
            //}
            #endregion
            #endregion

            System.Data.DataSet ds_insp = sqlExtension.SqlTxn.GetDataSet(connectDB, SQL01);
            SQL01 = SQL01.Replace("\r\n", " ");
            SQL01 = SQL01.Replace("\n", " ");
            SQL01 = SQL01.Replace("\t", " ");

            System.Data.DataSet ds_dfct = sqlExtension.SqlTxn.GetDataSet(connectDB, SQL03);
            SQL03 = SQL03.Replace("\r\n", " ");
            SQL03 = SQL03.Replace("\n", " ");
            SQL03 = SQL03.Replace("\t", " ");



            //#region cuttime获取 
            ////string sql_cuttime = $@" SELECT max(CUTTIME) as cuttime FROM {TIME_TABLE} WHERE APPNAME LIKE 'SP_M{lstr_G_FAC}SPAS_SOUTPUT1' or appname like 'SP_M{lstr_G_FAC}SHIS_DEFECTCODE%' ";
            //string sql_cuttime = $@"SELECT LASTTIME as cuttime FROM T_COMM_PJOBM WHERE JTYPE = 'ENGSUM01'";
            //System.Data.DataSet ds_cuttime = sqlExtension.SqlTxn.GetDataSet(connectDB, sql_cuttime);
            //string? cuttime = ds_cuttime.Tables[0].Rows[0]["CUTTIME"].ToString();
            //#endregion


            object ob = new
            {
                Data = ds_insp,
                Data_2 = ds_dfct,
                SQL01 = SQL01,
                SQL03 = SQL03,
                //cuttime = cuttime,
                FactoryType = fab,
                G_FAC = lstr_G_FAC,

                FromDate = lstr_FromDate,
                ToDate = lstr_ToDate,
                ShowMode = lstr_ShowMode,

                StrSql_w = sql_dfct_where,
                StrSql_w1 = string.Empty,
                StrSql_wf = string.Empty,
                rework = string.Empty,
                guolv = string.Empty,
                stroper = string.Empty,
                FAST_RWK_FLG_type = string.Empty,
                //传递至detail 1.asp页面  
                //前端点击表格 携带信息 col0 row0
            };
            returnValue = ob;
        }
    }
}
