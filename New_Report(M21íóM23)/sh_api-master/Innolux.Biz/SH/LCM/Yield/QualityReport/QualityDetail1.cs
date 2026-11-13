using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Yield.QualityReport
{
    public class QualityDetail1 : InnSqlAgent.IApiBase
    {
        public QualityDetail1()
        {
            //傳入參教及API說明
            parameters["FactoryType"] = "厂别 SH";
            parameters["G_FAC"] = "G_FAC";
            parameters["FromDate"] = "开始日期";
            parameters["ToDate"] = "结束日期";
            parameters["ShowMode"] = "展示方式";
            parameters["col0"] = "列 名称";
            parameters["row0"] = "行  名称";
            parameters["StrSql_w"] = "sql_dfct_where defect条件";
            parameters["StrSql_w1"] = "sql_pass_where  pass 条件";
            parameters["StrSql_wf"] = "sql_Dfct_FastRework 快修选项 条件";
            parameters["rework"] = "rework判断";
            parameters["guolv"] = "defect 条件添加 默认时加入条件，选择defect其他选项时，使用sql串中defect条件";
            parameters["stroper"] = "站点条件";
            parameters["FAST_RWK_FLG_type"] = "FAST_RWK_FLG_type";

            description = "STQualityDetail1 报表";
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
            #region 变量声明
            string str_G_FAC;
            string str_FromDate;
            string str_ToDate;
            string str_ShowMode;
            string col0;
            string row0;
            string StrSql_w;
            string sql_opercond = string.Empty;
            #endregion

            //暂存条件sql条件
            #region SQL条件
            str_G_FAC = GetParameterValueInString("G_FAC");

            str_FromDate = GetParameterValueInString("FromDate");
            str_ToDate = GetParameterValueInString("ToDate");
            str_ShowMode = GetParameterValueInString("ShowMode");
            col0 = GetParameterValueInString("col0");
            row0 = GetParameterValueInString("row0");
            StrSql_w = GetParameterValueInString("StrSql_w");

            #region SQL
            //4种模式   work_ctr / equip_nbr(SH為SubLineId) / big_prod / product_id
            if (str_ShowMode == "work_ctr")
            {
                if (col0 == "TOTAL YIELD")
                {
                    sql_opercond = " and OPERATION_ID in ('8210','8230','8234','823T','8240','8250','8280','8285','8290','8292','8294','8296','8297','8298','8299','8305','8350','8351','8353','8360','8366','8380')";
                }
                else
                {
                    sql_opercond = " and OPERATION_ID in (SUBSTR('" + col0.TrimEnd(',') + "',1,4))";
                }
            }
            else if (str_ShowMode == "equip_nbr")
            {
                if (col0 != "TOTAL YIELD")
                {
                    sql_opercond = " and SUBLINEID in ('" + col0.TrimEnd(',') + "')";
                }
            }
            else if (str_ShowMode == "big_prod")
            {
                if (col0 != "TOTAL YIELD")
                {
                    //sql_opercond = " and SUBSTRING(T9NCNO,1,8) in ('" + col0.TrimEnd(',') + "')";
                    var products = col0.TrimEnd(',').Split(',');
                    var likeConditions = new List<string>();

                    foreach (var prod in products.Where(p => !string.IsNullOrWhiteSpace(p)))
                    {
                        // 先去掉数据本身可能带的前后单引号和空格，再转义
                        var clean = prod.Trim().Trim('\'', ' ', '\t');
                        var escaped = clean.Replace("'", "''");
                        likeConditions.Add($"T9NCNO like '{escaped}%'");
                    }

                    sql_opercond = likeConditions.Count > 0
                        ? $" and ({string.Join(" or ", likeConditions)}) "
                        : "";
                }
            }
            else if (str_ShowMode == "product_id")
            {
                if (col0 != "TOTAL YIELD")
                {
                    sql_opercond = " and T9NCNO in ('" + col0.TrimEnd(',') + "')";
                }
            }

            #region DB + table
            string connectDB = string.Empty;
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
            {
                connectDB = "MESHIS";
            }
            #endregion

            string? pass_cuttime = string.Empty;
            string? wip_cuttime = string.Empty;
            string lstr_index = string.Empty;
            string SQL01;
            string SQL02;
            string SQL03 = string.Empty;
            //传递给下一阶页面条件
            //string col0 = string.Empty;
            //string col1 = string.Empty;
            string row1 = string.Empty;
            #endregion

            if (row0.IndexOf("品") >= 0)
            {
                SQL01 = $@"SELECT /*PRODUCT_ID
                                ,*/T.T9NCNO AS PROD_NBR
                                ,ENTITY_ID AS ENTITY_NO
                                ,SUBSTR(TRANSID,1,10) AS TRANS_DATE1
                                ,SUBSTR(TRANSID,12,26) AS TRANS_DATE2
                                ,SUBLINEID AS EQUIP_NBR
                                ,'' AS OPERATOR
                                ,OPERATOR_ID AS OPERATORS
                                ,'' AS OUTPUT_FG
                                ,OPERATION_ID AS WORK_CTR
                                ,M.NWORKCTR AS TO_WC
                                ,DEF_CODE AS DFCT_CODE
                                ,'' AS PATTERN_CODE
                                ,'' AS PREV_DFCT_CODE
                                ,1 AS STIMES_TTL
                                ,1 AS ITEM_NBR
                                ,0 AS OPERATOR_1600
                                ,LOT_T_CNT AS PASS_QTY
                        FROM (SELECT n.* FROM NJ_TBL_LIGHTON n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate between TO_DATE('{str_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{str_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and FAB_ID = 'B'
                                            and LOT_GRADE = 'S'
                                            and SUBSTR(DEF_CODE,3,1) = SUBSTR('{row0}',1,1)
                                            {sql_opercond}
                                            {StrSql_w}) T
                        LEFT JOIN MSKM M ON m.fabid = T.FAB_ID and M.CWORKCTR = T.OPERATION_ID AND M.GradeID = 'S'";
            }
            else if (row0.IndexOf("QTY") >= 0)
            {
                SQL01 = $@"SELECT /*PRODUCT_ID
                                ,*/T.T9NCNO AS PROD_NBR
                                ,ENTITY_ID AS ENTITY_NO
                                ,SUBSTR(TRANSID,1,10) AS TRANS_DATE1
                                ,SUBSTR(TRANSID,12,26) AS TRANS_DATE2
                                ,SUBLINEID AS EQUIP_NBR
                                ,'' AS OPERATOR
                                ,OPERATOR_ID AS OPERATORS
                                ,'' AS OUTPUT_FG
                                ,OPERATION_ID AS WORK_CTR
                                ,M.NWORKCTR AS TO_WC
                                ,DEF_CODE AS DFCT_CODE
                                ,'' AS PATTERN_CODE
                                ,'' AS PREV_DFCT_CODE
                                ,1 AS STIMES_TTL
                                ,1 AS ITEM_NBR
                                ,0 AS OPERATOR_1600
                                ,LOT_T_CNT AS PASS_QTY
                        FROM (SELECT n.* FROM NJ_TBL_LIGHTON n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate between TO_DATE('{str_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{str_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and FAB_ID = 'B'
                                            /*and LOT_GRADE = 'S'
                                            and SUBSTR(DEF_CODE,3,1) = SUBSTR('{row0}',1,1)*/
                                            {sql_opercond}
                                            {StrSql_w}) T
                        LEFT JOIN MSKM M ON m.fabid = T.FAB_ID and M.CWORKCTR = T.OPERATION_ID AND M.GradeID = 'S'";
            }
            else if (row0.IndexOf("GOOD") >= 0 || row0.IndexOf("YIELD Rate") >= 0)
            {
                SQL01 = $@"SELECT /*PRODUCT_ID
                                ,*/T.T9NCNO AS PROD_NBR
                                ,ENTITY_ID AS ENTITY_NO
                                ,SUBSTR(TRANSID,1,10) AS TRANS_DATE1
                                ,SUBSTR(TRANSID,12,26) AS TRANS_DATE2
                                ,SUBLINEID AS EQUIP_NBR
                                ,'' AS OPERATOR
                                ,OPERATOR_ID AS OPERATORS
                                ,'' AS OUTPUT_FG
                                ,OPERATION_ID AS WORK_CTR
                                ,M.NWORKCTR AS TO_WC
                                ,DEF_CODE AS DFCT_CODE
                                ,'' AS PATTERN_CODE
                                ,'' AS PREV_DFCT_CODE
                                ,1 AS STIMES_TTL
                                ,1 AS ITEM_NBR
                                ,0 AS OPERATOR_1600
                                ,LOT_T_CNT AS PASS_QTY
                        FROM (SELECT n.* FROM NJ_TBL_LIGHTON n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate between TO_DATE('{str_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{str_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and FAB_ID = 'B'
                                            and LOT_GRADE = 'A'
                                            /*and SUBSTR(DEF_CODE,3,1) = SUBSTR('{row0}',1,1)*/
                                            {sql_opercond}
                                            {StrSql_w}) T
                        LEFT JOIN MSKM M ON m.fabid = T.FAB_ID and M.CWORKCTR = T.OPERATION_ID AND M.GradeID = 'S'";
            }
            else if (row0.IndexOf("NG") >= 0 || row0.IndexOf("YIELD LOSS") >= 0)
            {
                SQL01 = $@"SELECT /*PRODUCT_ID
                                ,*/T.T9NCNO AS PROD_NBR
                                ,ENTITY_ID AS ENTITY_NO
                                ,SUBSTR(TRANSID,1,10) AS TRANS_DATE1
                                ,SUBSTR(TRANSID,12,26) AS TRANS_DATE2
                                ,SUBLINEID AS EQUIP_NBR
                                ,'' AS OPERATOR
                                ,OPERATOR_ID AS OPERATORS
                                ,'' AS OUTPUT_FG
                                ,OPERATION_ID AS WORK_CTR
                                ,M.NWORKCTR AS TO_WC
                                ,DEF_CODE AS DFCT_CODE
                                ,'' AS PATTERN_CODE
                                ,'' AS PREV_DFCT_CODE
                                ,1 AS STIMES_TTL
                                ,1 AS ITEM_NBR
                                ,0 AS OPERATOR_1600
                                ,LOT_T_CNT AS PASS_QTY
                        FROM (SELECT n.* FROM NJ_TBL_LIGHTON n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate between TO_DATE('{str_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{str_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and FAB_ID = 'B'
                                            and LOT_GRADE = 'S'
                                            /*and SUBSTR(DEF_CODE,3,1) = SUBSTR('{row0}',1,1)*/
                                            {sql_opercond}
                                            {StrSql_w}) T
                        LEFT JOIN MSKM M ON m.fabid = T.FAB_ID and M.CWORKCTR = T.OPERATION_ID AND M.GradeID = 'S'";
            }
            else
            {
                SQL01 = $@"SELECT /*PRODUCT_ID
                                ,*/T.T9NCNO AS PROD_NBR
                                ,ENTITY_ID AS ENTITY_NO
                                ,SUBSTR(TRANSID,1,10) AS TRANS_DATE1
                                ,SUBSTR(TRANSID,12,26) AS TRANS_DATE2
                                ,SUBLINEID AS EQUIP_NBR
                                ,'' AS OPERATOR
                                ,OPERATOR_ID AS OPERATORS
                                ,'' AS OUTPUT_FG
                                ,OPERATION_ID AS WORK_CTR
                                ,M.NWORKCTR AS TO_WC
                                ,DEF_CODE AS DFCT_CODE
                                ,'' AS PATTERN_CODE
                                ,'' AS PREV_DFCT_CODE
                                ,1 AS STIMES_TTL
                                ,1 AS ITEM_NBR
                                ,0 AS OPERATOR_1600
                                ,LOT_T_CNT AS PASS_QTY
                        FROM (SELECT n.* FROM NJ_TBL_LIGHTON n,MSFM ms,MWCM mw,MPIM mp,SWOM sw
                                WHERE n.FAB_ID = ms.FABID AND n.T9NCNO = ms.PRODID
                                        AND n.FAB_ID = mw.FABID AND n.OPERATION_ID = mw.WORKCTR
                                        AND n.FAB_ID = mp.FABID AND n.T9NCNO = mp.PRODID
                                        AND n.FAB_ID = sw.FABID AND n.WORK_ORDER = sw.WONBR
                                        AND n.ReportDate between TO_DATE('{str_FromDate}', 'YYYY/MM/DD HH24:MI:SS') and TO_DATE('{str_ToDate}', 'YYYY/MM/DD HH24:MI:SS') 
				                            and FAB_ID = 'B'
                                            and LOT_GRADE = 'S'
                                            and DEF_CODE = '{row0}'
                                            {sql_opercond}
                                            {StrSql_w}) T
                        LEFT JOIN MSKM M ON m.fabid = T.FAB_ID and M.CWORKCTR = T.OPERATION_ID AND M.GradeID = 'S'";
            }

            //Oracle無TOP語法,以 from dual where 1=0 取代 TOP 0;移除結尾分號
            SQL02 = "select '' as DFCT_CODE,'' as ERRC_DESCR,'' as DFCT_REASON,'' as ERRM_DESCR, 0 as QTY,'' as RATE from dual where 1 = 0";
            #endregion

            SQL01 = SQL01.Replace("\\r\\n", "\r\n");
            System.Data.DataSet ds_insp = sqlExtension.SqlTxn.GetDataSet(connectDB, SQL01);
            SQL01 = SQL01.Replace("\r\n", " ");
            SQL01 = SQL01.Replace("\n", " ");
            SQL01 = SQL01.Replace("\t", " ");

            System.Data.DataSet ds_dfct = sqlExtension.SqlTxn.GetDataSet(connectDB, SQL02);
            SQL02 = SQL02.Replace("\r\n", " ");
            SQL02 = SQL02.Replace("\n", " ");
            SQL02 = SQL02.Replace("\t", " ");

            //#region cuttime获取 
            ////string sql_cuttime = $@" SELECT max(CUTTIME) as cuttime FROM {TIME_TABLE} WHERE APPNAME LIKE 'SP_M{str_G_FAC}SPAS_SOUTPUT1' or appname like 'SP_M{str_G_FAC}SHIS_DEFECTCODE%' ";
            //string sql_cuttime = $@"SELECT LASTTIME as cuttime FROM T_COMM_PJOBM WHERE JTYPE = 'ENGSUM01'";
            //System.Data.DataSet ds_cuttime = sqlExtension.SqlTxn.GetDataSet(connectDB, sql_cuttime);
            //string? cuttime = ds_cuttime.Tables[0].Rows[0]["CUTTIME"].ToString();
            //#endregion

            object ob = new
            {
                Data = ds_insp,
                //Data_2 = ds_dfct,
                SQL01 = SQL01,
                SQL02 = SQL02,
                SQL03 = SQL03,
                //cuttime = cuttime,
                FactoryType = fab,
                G_FAC = str_G_FAC,
                FromDate = str_FromDate,
                ToDate = str_ToDate,
                ShowMode = str_ShowMode,
                //StrSql_w = sql_dfct_where,
                //StrSql_w1 = sql_pass_where,
                //StrSql_wf = sql_Dfct_FastRework,
                //rework = lstr_Reworked,
                //guolv = guolv,
                //stroper = lstr_Operation,
                //FAST_RWK_FLG_type = lstr_FastRework,
                //传递至detail 1.asp页面  
                //前端点击表格 携带信息 col0 row0
            };
            returnValue = ob;
        }
    }
}
