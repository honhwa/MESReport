using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Report.Library;
using System.Drawing;
using System.Threading;
namespace WebReport
{
    public partial class FabNavigator : System.Web.UI.Page
    {
        public double[] TargetPercent = { 0.5, 1, 1, 0.33, 1, 1, 1, 1, 0.67, 1, 1, 1 };
        protected void Page_Load(object sender, EventArgs e)
        {
            
            btnQuery_Click(sender, e);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            #region Time
            string strBDate = "";
            string strEDate = "";
            DateTime dtDate = DateTime.Now;
            int intHours;
            if (Convert.ToInt32(dtDate.Hour.ToString()) < 7)
            {
                strBDate = dtDate.AddDays(-1).ToString("yyyy-MM-dd 19:00:00");
                intHours = 12 - Convert.ToInt32(dtDate.Hour.ToString());
            }
            else if (Convert.ToInt32(dtDate.Hour.ToString()) >= 7 && Convert.ToInt32(dtDate.Hour.ToString()) < 19)
            {
                strBDate = dtDate.ToString("yyyy-MM-dd 07:00:00");
                intHours = Convert.ToInt32(dtDate.Hour.ToString());
            }
            else
            {
                strBDate = dtDate.ToString("yyyy-MM-dd 19:00:00");
                intHours = Convert.ToInt32(dtDate.Hour.ToString()) - 12;
            }

            strEDate = dtDate.ToString("yyyy-MM-dd  HH:mm:ss");

            //DateTime aa = Convert.ToDateTime(strBDate);
            DateTime dtBDate = Convert.ToDateTime(strBDate);
            TimeSpan span = dtDate.Subtract(dtBDate);
            Int32 intSecondDiff = Convert.ToInt32(span.TotalSeconds);
            //string strHours = dtDate.Hour.ToString();
            string strMinutes = dtDate.Minute.ToString();
            #endregion

            //gvYieldSummary.Columns.Clear();
            DataSet dsYieldSummary = new DataSet();
            if (Request.QueryString["Area"] != null)
            {
                if ( intSecondDiff < 600)
                {
                    strEDate = Convert.ToDateTime(strBDate).ToString("yyyy-MM-dd  HH:mm:ss");
                    strBDate = Convert.ToDateTime(strBDate).AddHours(-12).ToString("yyyy-MM-dd  HH:mm:ss");
                }

                ddlArea.SelectedItem.Text = Request.QueryString["Area"];ddlArea.SelectedItem.Value = Request.QueryString["Area"];
                ddlFloor.SelectedItem.Text = "ALL"; ddlFloor.SelectedItem.Value = "ALL";
                dsYieldSummary = GetYieldSummary("MAPP", Request.QueryString["Area"].ToString(), "ALL", strBDate, strEDate, intHours, strMinutes);
            }
            else
            {
                dsYieldSummary = GetYieldSummary("", ddlArea.SelectedItem.Value, ddlFloor.SelectedItem.Value, strBDate, strEDate, intHours, strMinutes);
            }

            lbldatetime.Text = strBDate + "  ~  " + strEDate;    

            gvYieldSummary.DataSource = dsYieldSummary;
            gvYieldSummary.DataBind();

            if (gvYieldSummary.Rows.Count > 0)
            {
                for (int i = 0; i < gvYieldSummary.Rows.Count; i++)
                {
                    string line = gvYieldSummary.Rows[i].Cells[0].Text.ToString();
                    gvYieldSummary.Rows[i].Cells[0].Attributes.Add("style", "cursor: hand;");
                    if (line.IndexOf("COG", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        gvYieldSummary.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('OnLineReportFinishingVer1.aspx?COG=" + line + "&FOG=&LOT2=')");
                    }
                    else if (line.IndexOf("BOD", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        gvYieldSummary.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('OnLineReportFinishingVer1.aspx?COG=&FOG=" + line + "&LOT2=" + line + "')");
                    }
                    else if (line.IndexOf("Assy", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        gvYieldSummary.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('OnLineReportAssyVer3.aspx?Line=" + line + "')");
                    }
                    else
                    {
                        gvYieldSummary.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('')");
                    }
 
                }
            }
        }

        private static DataSet GetYieldSummary(string MAPP, string FabArea, string FabFloor, string strBDate, string strEDate, int intHours, string strMinutes)
        {

            // Get YieldSummary
            //DBConn conn2 = new DBConn("Data Source=10.119.8.173;Initial Catalog=IMSRoom;User ID=IMSUser;Password=IMS12345;Max Pool Size=30;Min Pool Size=5;Pooling=True;");
            DBConn conn2 = new DBConn("");

            string strsqlFabArea = "";
            string strFabFloor = "";

            switch (FabArea)
            {
                case "Assy":
                case "LAM":
                case "Finishing":
                    strsqlFabArea = " and Area='" + FabArea + "'";
                    break;
                default:
                    break;
            }
            switch (FabFloor)
            {
                case "2F":
                case "3F":
                case "4F":
                    strFabFloor = " and SUBSTRING(Line, 1, 2)='" + FabFloor + "'";
                    break;
                default:
                    break;
            }

            string strsql = string.Format(@" SELECT TransDate,
		                                        ReportDate,
		                                        ReportMonth,
		                                        ReportWeek,
		                                        REPORTYEAR,
		                                        FAB_ID,
		                                        DEFECT_DATE,
		                                        CREATE_TIME,
		                                        OPERATION_ID,
		                                        ENTITY_ID,
		                                        DEF_CODE,
		                                        LOT_GRADE,
		                                        LOT_LEVEL,
		                                        REWORK_CNT,
		                                        R_LAST_FLG,
		                                        DEF_CNT,
		                                        PRODUCT_ID,
		                                        PRODUCT_GROUP,
		                                        WORK_ORDER,
		                                        WORK_TYPE,
		                                        LOT_TYPE,
		                                        LOT_T_CNT,
		                                        LOT_G_CNT,
		                                        TRANSID,
		                                        SHIFT_ID,
		                                        OPERATOR_ID,
		                                        LAST_MODIFY_TIME,
		                                        REF2,
		                                        CUSTOMERID,
		                                        RACK,
		                                        SUBLINEID,
		                                        SHOPCD,
		                                        PALLETID,
		                                        CPRODUCT_ID
                                                ,CT9NCNO
                                                ,T9NCNO
                                         INTO #New_WLTX
                                         FROM (
                                                  SELECT GETDATE() AS TransDate,
			                                         CONVERT(varchar(14),D.TRANSDATE,20) + '00:00:000' ReportDate,
			                                         MONTH(D.MFGDATE) AS ReportMonth,
			                                         datediff(ww,cast(year(D.MFGDATE) as varchar)  + '-01-01',D.MFGDATE) AS ReportWeek,
			                                         YEAR(D.MFGDATE) AS REPORTYEAR,
		                                             D.FABID AS FAB_ID,
		                                             D.MFGDATE AS DEFECT_DATE,
		                                             D.MFGDATE AS CREATE_TIME,
			                                         CASE WHEN D.CWORKCTR='8270' THEN '8280'  ELSE D.CWORKCTR END AS OPERATION_ID,--1.16ADD
		                                             D.TOENTITYID AS  ENTITY_ID,
		                                             ISNULL(D.DEFECTCD,'') AS DEF_CODE,
		                                             D.TOLOTGRADE AS  LOT_GRADE,
		                                             D.TOLOTLEVEL AS  LOT_LEVEL,
		                                             0 AS  REWORK_CNT,
		                                             1 AS R_LAST_FLG,
		                                            CASE WHEN D.TOLOTGRADE='R' OR D.TOLOTGRADE='S' THEN D.NENTITYTQTY ELSE 0 END AS DEF_CNT,
		                                            F.PROJECT_NAME AS PRODUCT_ID,
		                                            ISNULL(F.PRODGROUP,'') AS  PRODUCT_GROUP,
		                                            D.TOWONBR AS  WORK_ORDER,
		                                            D.TOWOTYPE AS WORK_TYPE,
		                                            D.TOLOTTYPE AS LOT_TYPE,
		                                            D.NENTITYTQTY AS  LOT_T_CNT,
		                                            CASE WHEN D.TOLOTGRADE='A' THEN  D.NENTITYTQTY ELSE 0 END AS LOT_G_CNT,
		                                           D.TRANSID,
		                                           D.SHIFTID AS SHIFT_ID,
		                                           D.USERID AS OPERATOR_ID,
		                                           GETDATE() AS LAST_MODIFY_TIME,
		                                           '' AS REF2,
		                                           D.CUSTOMERID AS CUSTOMERID,
		                                           ''  AS RACK,
		                                           ISNULL(CSUBLINEID,'') AS SUBLINEID,
		                                           D.CSHOPCD AS SHOPCD,
		                                           ISNULL(D.PALLETID,'') AS PALLETID,
    	                                           D.TOCPRODID CPRODUCT_ID
                                                   ,D.TOC9NCNO CT9NCNO
                                                   ,D.TON9NCNO T9NCNO
                                        FROM New_WLTX  D WITH(NOLOCK) LEFT JOIN MPIM F ON( D.TOPRODID=F.PRODID AND D.FABID=F.FABID)
                                         WHERE D.FABID='B' AND VERB='MVOU' AND D.TRANSDATE>='{0}' AND D.TRANSDATE<'{1}' AND D.CWORKCTR IN('8180','8210','8230','8240','8250','8270','8285','8290','8292','8294','8296','8350','8353','8360')
                                         ) Z;
 
                                         SELECT 
		                                        ReportDate,
		                                        ReportMonth,
		                                        ReportWeek,
		                                        REPORTYEAR,
		                                        FAB_ID,
		                                        OPERATION_ID,
		                                        SHIFT_ID,
		                                        CDATE,
		                                        LOT_ID,
		                                        PRODUCT_ID, 
		                                        PRODUCT_GROUP, 
		                                        T_CNT,
		                                        T_CNT-B-R-S AS A_CNT,
		                                        B AS B_CNT,
		                                        R AS R_CNT,
		                                        S AS S_CNT,
		                                        LOT_TYPE, 
		                                        A3 AS A3_CNT,
		                                        ((t_cnt-b-r-s)-a1-a2-a3-A4-AD-A5-A6-A7-A8-A9-AW-ap-ak-a0-ab-af-ah-al-ax-am-undefine_cnt-av-L2-LR-BP-PC-PT-P5) AS AA_CNT,
		                                        A1 AS A1_CNT,
		                                        A2 AS A2_CNT,
		                                        AP AS AP_CNT,
		                                        AK AS AK_CNT,
		                                        A0 AS A0_CNT,
		                                        AB AS AB_CNT,
		                                        AF AS AF_CNT,
		                                        AH AS AH_CNT,
		                                        AL AS AL_CNT,
		                                        UNDEFINE_CNT,
		                                        WORK_ORDER,
		                                        AX AS AX_CNT,
		                                        REF2,
		                                        CUSTOMERID,
		                                        RACK,
		                                        AM AS AM_CNT,
		                                        AV AS AV_CNT,
		                                        S1 AS S1_CNT,
		                                        S2 AS S2_CNT,
		                                        SUBLINEID, 
		                                        S3 AS S3_CNT,
		                                        S4 AS S4_CNT,
		                                        S5 AS S5_CNT,
		                                        SHOPCD,
		                                        A4 AS A4_CNT,
		                                        AD AS AD_CNT,
		                                        A5 AS A5_CNT,
		                                        A6 AS A6_CNT,
		                                        A7 AS A7_CNT,
		                                        A8 AS A8_CNT,
		                                        A9 AS A9_CNT,
		                                        AW AS AW_CNT,
		                                        CPRODUCT_ID
		                                        ,ASs AS AS_CNT
		                                        ,PALLETID
		                                        ,L2 AS L2_CNT,
                                                 LR AS LR_CNT,
                                                 BP AS BP_CNT,
                                                 PC AS PC_CNT,
                                                 PT AS PT_CNT,
                                                 P5 AS P5_CNT
		                                        ,CT9NCNO
                                                ,T9NCNO
                                                INTO #New_WLTX_SUM
		                                        FROM 
		                                        (
		                                        SELECT 
		                                        ReportDate,
		                                        MONTH(ReportDate) AS ReportMonth,
		                                        datediff(ww,cast(year(ReportDate) as varchar)  + '-01-01',ReportDate) AS ReportWeek,
		                                        REPORTYEAR,
		                                        FAB_ID,
		                                        OPERATION_ID,
		                                        SHIFT_ID,
		                                        DEFECT_DATE AS CDATE,
		                                        '_' AS LOT_ID,
		                                        PRODUCT_ID AS PRODUCT_ID,
		                                        PRODUCT_GROUP AS PRODUCT_GROUP,
		                                        SUM(LOT_T_CNT) AS T_CNT,		
		                                        LOT_TYPE,SUBLINEID,SHOPCD,WORK_ORDER,
		                                        REF2,
		                                        CUSTOMERID,
		                                        RACK,
                                                sum(case when lot_grade='B' then LOT_T_CNT else 0 end) AS B, 
                                                sum(case when lot_grade='R' then LOT_T_CNT else 0 end) AS R ,
                                                sum(case when lot_grade='S' then LOT_T_CNT else 0 end) AS S,
                                                sum(case when lot_level='A0' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A0,
                                                sum(case when lot_level='A1' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A1, 
                                                sum(case when lot_level='A2' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A2,
                                                sum(case when lot_level='A3' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A3,
		                                        sum(case when lot_level='A4' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A4,
		                                        sum(case when lot_level='A5' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A5,
		                                        sum(case when lot_level='A6' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A6,
		                                        sum(case when lot_level='A7' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A7,
		                                        sum(case when lot_level='A8' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A8,
		                                        sum(case when lot_level='A9' and lot_grade='A' then LOT_T_CNT else 0 end ) AS A9,
		                                        sum(case when lot_level='AW' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AW,
		                                        sum(case when lot_level='AS' and lot_grade='A' then LOT_T_CNT else 0 end ) AS ASs,
		                                        sum(case when lot_level='L2' and lot_grade='A' then LOT_T_CNT else 0 end ) AS L2,
		                                        sum(case when lot_level='LR' and lot_grade='A' then LOT_T_CNT else 0 end ) AS LR,
		                                        sum(case when lot_level='BP' and lot_grade='A' then LOT_T_CNT else 0 end ) AS BP,
		                                        sum(case when lot_level='PC' and lot_grade='A' then LOT_T_CNT else 0 end ) AS PC,
		                                        sum(case when lot_level='PT' and lot_grade='A' then LOT_T_CNT else 0 end ) AS PT,
		                                        sum(case when lot_level='P5' and lot_grade='A' then LOT_T_CNT else 0 end ) AS P5,
		                                        sum(case when lot_level='AD' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AD,
                                                sum(case when lot_level='AP' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AP,
                                                sum(case when lot_level='AK' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AK,
                                                sum(case when lot_level='AB' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AB,
                                                sum(case when lot_level='AF' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AF,
                                                sum(case when lot_level='AH' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AH,
                                                sum(case when lot_level='AL' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AL,
                                                sum(case when lot_level='AX' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AX,
                                                sum(case when lot_level='AM' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AM,
                                                sum(case when lot_level='AV' and lot_grade='A' then LOT_T_CNT else 0 end ) AS AV,
                                                sum(case when lot_level='S1' and lot_grade='S' then LOT_T_CNT else 0 end ) AS S1,
                                                sum(case when lot_level='S2' and lot_grade='S' then LOT_T_CNT else 0 end ) AS S2,
                                                sum(case when lot_level='S3' and lot_grade='S' then LOT_T_CNT else 0 end ) AS S3, 
                                                sum(case when lot_level='S4' and lot_grade='S' then LOT_T_CNT else 0 end ) AS S4,
                                                sum(case when lot_level='S5' and lot_grade='S' then LOT_T_CNT else 0 end ) AS S5,
		                                        sum(case when lot_grade not in('B','R','S') THEN
                                                CASE when lot_level not in ('A1','A2','A3','A4','A5','A6','A7','A8','A9','AW','AD','AP','AK','A0','AB','AF','AH','AL','AX','AM','AV','-','AS','L2','LR','BP','PC','PT','P5') -- 1.06 ADD	
                                                THEN def_cnt else 0 end ELSE 0 END) AS UNDEFINE_CNT,
		                                        CPRODUCT_ID	
		                                        ,PALLETID
		                                        ,CT9NCNO
                                                ,T9NCNO
		                                        FROM #New_WLTX H
		                                        WHERE 
                                                DEF_CNT>=0
		                                        GROUP BY 
		                                        ReportDate,
		                                        MONTH(ReportDate),
		                                        REPORTYEAR,
		                                        FAB_ID,
		                                        OPERATION_ID,
		                                        SHIFT_ID,
		                                        DEFECT_DATE ,
		                                        PRODUCT_ID ,
		                                        PRODUCT_GROUP,LOT_TYPE,SUBLINEID,SHOPCD,WORK_ORDER,
		                                        REF2,
		                                        CUSTOMERID,
		                                        RACK,
		                                        CPRODUCT_ID
		                                        ,PALLETID
		                                        ,CT9NCNO
                                                ,T9NCNO
		                                        ) X
		
		
 
                                         SELECT 
		                                        GETDATE() TRANSDATE,
		                                        ReportDate,
		                                        ReportMonth,
		                                        ReportWeek,
		                                        REPORTYEAR,
		                                        FAB_ID,
		                                        OPERATION_ID,
		                                        SHIFT_ID,
		                                        CDATE,
		                                        PRODUCT_ID,
		                                        PRODUCT_GROUP,
		                                        SUM(T_CNT) T_CNT,
		                                        SUM(A_CNT) A_CNT,
		                                        SUM(B_CNT) B_CNT,
		                                        SUM(R_CNT) R_CNT,
		                                        SUM(S_CNT) S_CNT,
		                                        LOT_TYPE,
		                                        SUM(A3_CNT) A3_CNT,
		                                        SUM(AA_CNT) AA_CNT,
		                                        SUM(A1_CNT) A1_CNT,
		                                        SUM(A2_CNT) A2_CNT,
		                                        SUM(AP_CNT) AP_CNT,
		                                        SUM(AK_CNT) AK_CNT,
		                                        SUM(A0_CNT) A0_CNT,
		                                        SUM(AB_CNT) AB_CNT,
		                                        SUM(AF_CNT) AF_CNT,
		                                        SUM(AH_CNT) AH_CNT,
		                                        SUM(AL_CNT) AL_CNT,
		                                        SUM(UNDEFINE_CNT) UNDEFINE_CNT,
		                                        WORK_ORDER,
		                                        SUM(AX_CNT) AX_CNT,
		                                        SUM(AM_CNT) AM_CNT,
		                                        SUM(AV_CNT) AV_CNT,
		                                        SUM(S1_CNT) S1_CNT,
		                                        SUM(S2_CNT) S2_CNT,
		                                        SUBLINEID,
		                                        SUM(S3_CNT) S3_CNT,
		                                        SUM(S4_CNT) S4_CNT,
		                                        SUM(S5_CNT) S5_CNT,
		                                        SHOPCD,
		                                        SUM(A4_CNT) A4_CNT,
		                                        SUM(AD_CNT) AD_CNT,
		                                        SUM(A5_CNT) A5_CNT,
		                                        SUM(A6_CNT) A6_CNT,
		                                        SUM(A7_CNT) A7_CNT,
		                                        SUM(A8_CNT) A8_CNT,
		                                        SUM(A9_CNT) A9_CNT,
		                                        SUM(AW_CNT) AW_CNT,
		                                        CPRODUCT_ID
		                                        ,SUM(AS_CNT) AS_CNT
		                                        ,PALLETID
		                                        ,SUM(L2_CNT) L2_CNT	
		                                        ,SUM(LR_CNT) LR_CNT	
		                                        ,SUM(BP_CNT) BP_CNT	
		                                        ,SUM(PC_CNT) PC_CNT	
		                                        ,SUM(PT_CNT) PT_CNT	
		                                        ,SUM(P5_CNT) P5_CNT	
		                                        ,CT9NCNO
                                                ,T9NCNO
		                                        INTO #New_WLTX_TotalCost
		                                        FROM #New_WLTX_SUM 
		                                        GROUP BY 
		                                        ReportDate,
		                                        ReportMonth,
		                                        ReportWeek,
		                                        REPORTYEAR,
		                                        FAB_ID,
		                                        OPERATION_ID,
		                                        SHIFT_ID,
		                                        CDATE,
		                                        PRODUCT_ID,
		                                        PRODUCT_GROUP,
		                                        LOT_TYPE,
		                                        WORK_ORDER,SUBLINEID,SHOPCD,
		                                        CPRODUCT_ID	
                                                ,PALLETID	
		                                        ,CT9NCNO
                                                ,T9NCNO
 
                                        SELECT SUBLINEID,T9NCNO,PRODUCT_ID Product INTO #PROMAX FROM #New_WLTX WHERE transid IN(
                                        SELECT MAX(transid) FROM #New_WLTX AAS
                                        GROUP BY SUBLINEID)

                                        SELECT SUBLINEID Line,OPERATION_ID,
                                        CASE WHEN OPERATION_ID IN('8180','8210','8230','8240','8250','8280') THEN 'Finishing' WHEN OPERATION_ID IN('8285','8290','8292','8294','8296') THEN 'LAM' WHEN OPERATION_ID IN('8350','8353','8360') THEN 'Assy' END Area,
                                        CASE OPERATION_ID 
                                        WHEN '8180' THEN 'POL'
                                        WHEN '8210' THEN 'COG' 
                                        WHEN '8230' THEN 'FOG' 
                                        WHEN '8240' THEN 'TOD'
                                        WHEN '8250' THEN 'FOB'
                                        WHEN '8280' THEN 'LOT2'
                                        WHEN '8285' THEN 'LAM pre-baking'
                                        WHEN '8290' THEN 'LAM'
                                        WHEN '8292' THEN 'LAM UV'
                                        WHEN '8294' THEN 'ASF 貼附'
                                        WHEN '8296' THEN 'LAM 功能測試外觀'
                                        WHEN '8350' THEN 'Assy' 
                                        WHEN '8353' THEN 'Module Test' 
                                        WHEN '8360' THEN 'OQC' END WORKNAME,
                                        SUM(T_CNT) T_CNT,SUM(A_CNT) A_CNT,
                                        CAST(SUM(A_CNT) AS numeric(5, 1))/CAST(SUM(T_CNT) AS numeric(5, 1)) Yield
                                        INTO #New_WLTX_SUM_YieldOutPut
                                        FROM #New_WLTX_TotalCost
                                         GROUP BY SUBLINEID,OPERATION_ID


                                        SELECT *, CASE 
                                        WHEN ({4} >=7 AND {4} <8) THEN cast(STD AS NUMERIC(18,3))/10.83 * .83 * {5} /60
                                        WHEN ({4} >=8 AND {4} <10) THEN cast(STD AS NUMERIC(18,3))/10.83 * .83 + ({4}-7-1) * cast(STD AS NUMERIC(18,3))/10.83 +  cast(STD AS NUMERIC(18,3))/10.83 * {5}/60
                                        WHEN ({4} >=10 AND {4} <11) THEN  cast(STD AS NUMERIC(18,3))/10.83 * .83 + ({4}-7-1) * cast(STD AS NUMERIC(18,3))/10.83 +  cast(STD AS NUMERIC(18,3))/10.83 * .33 * {5}/60
                                        WHEN ({4} >=11 AND {4} <15) THEN  cast(STD AS NUMERIC(18,3))/10.83 * .83 + cast(STD AS NUMERIC(18,3))/10.83 * .33 + ({4}-7-2) * cast(STD AS NUMERIC(18,3))/10.83 +  cast(STD AS NUMERIC(18,3))/10.83 * {5}/60
                                        WHEN ({4} >=15 AND {4} <16) THEN  cast(STD AS NUMERIC(18,3))/10.83 * .83 + + cast(STD AS NUMERIC(18,3))/10.83 * .33 + ({4}-7-1) * cast(STD AS NUMERIC(18,3))/10.83 +  cast(STD AS NUMERIC(18,3))/10.83 * .66 * {5}/60
                                        WHEN ({4} >=16) THEN  cast(STD AS NUMERIC(18,3))/10.83 * .83 + cast(STD AS NUMERIC(18,3))/10.83 * .33 + cast(STD AS NUMERIC(18,3))/10.83 * .66 + ({4}-7-3) * cast(STD AS NUMERIC(18,3))/10.83 +  cast(STD AS NUMERIC(18,3))/10.83 * {5}/60
                                        END Target_Output 
                                        INTO #MPIEM 
                                        FROM MPIEM

                                        SELECT KA.*,QW.*,CAST(ISNULL(MP.TARGET_OUTPUT,0) AS numeric(18, 0)) TARGET_OUTPUT   
                                        INTO #New_WLTX_SUM_YieldOutPutTargetOutput FROM #New_WLTX_SUM_YieldOutPut KA
                                        LEFT JOIN #PROMAX QW ON KA.LINE=QW.SUBLINEID
                                        LEFT JOIN #MPIEM MP ON QW.T9NCNO=PRODUCTID AND KA.OPERATION_ID=MP.STATION AND KA.LINE=MP.LINE


                                        SELECT AA.*,
                                        CAST(isnull(A.Yield,0) AS numeric(18, 4)) * 100 Yield1,isnull(A.A_CNT,0) OutPut1,ISNULL(A.TARGET_OUTPUT,0) StdOutput1,
                                        CAST(isnull(B.Yield,0) AS numeric(18, 4)) * 100 Yield2,isnull(B.A_CNT,0) OutPut2,ISNULL(B.TARGET_OUTPUT,0) StdOutput2,
                                        CAST(isnull(C.Yield,0) AS numeric(18, 4)) * 100 Yield3,isnull(C.A_CNT,0) OutPut3,ISNULL(C.TARGET_OUTPUT,0) StdOutput3,
                                        0 Yield4,0 OutPut4,0 StdOutput4,
                                        0 Yield5,0 OutPut5,0 StdOutput5,
                                        0 Yield6,0 OutPut6,0 StdOutput6
                                         FROM (SELECT DISTINCT line,Area,Product FROM #New_WLTX_SUM_YieldOutPutTargetOutput WHERE workname IN('Assy','Module Test','OQC') {2}{3}) AA
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='Assy') A ON AA.LINE =A.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='Module Test') B ON AA.LINE =B.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='OQC') C ON AA.LINE =C.LINE

                                        UNION ALL

                                        SELECT AA.*,
                                        CAST(isnull(A.Yield,0) AS numeric(18, 4)) * 100 Yield1,isnull(A.A_CNT,0) OutPut1,ISNULL(A.TARGET_OUTPUT,0) StdOutput1,
                                        CAST(isnull(B.Yield,0) AS numeric(18, 4)) * 100 Yield2,isnull(B.A_CNT,0) OutPut2,ISNULL(B.TARGET_OUTPUT,0) StdOutput2,
                                        CAST(isnull(C.Yield,0) AS numeric(18, 4)) * 100 Yield3,isnull(C.A_CNT,0) OutPut3,ISNULL(C.TARGET_OUTPUT,0) StdOutput3,
                                        CAST(isnull(D.Yield,0) AS numeric(18, 4)) * 100 Yield4,isnull(D.A_CNT,0) OutPut4,ISNULL(D.TARGET_OUTPUT,0) StdOutput4,
                                        CAST(isnull(E.Yield,0) AS numeric(18, 4)) * 100 Yield5,isnull(E.A_CNT,0) OutPut5,ISNULL(E.TARGET_OUTPUT,0) StdOutput5,
                                        0 Yield6,0 OutPut6,0 StdOutput6
                                         FROM (SELECT DISTINCT line,Area,Product FROM #New_WLTX_SUM_YieldOutPutTargetOutput WHERE workname IN('LAM pre-baking','LAM','LAM UV','ASF 貼附','LAM 功能測試外觀') {2}{3}) AA
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='LAM pre-baking') A ON AA.LINE =A.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='LAM') B ON AA.LINE =B.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='LAM UV') C ON AA.LINE =C.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='ASF 貼附') D ON AA.LINE =D.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='LAM 功能測試外觀') E ON AA.LINE =E.LINE

                                        UNION ALL

                                        SELECT AA.*,
                                        CAST(isnull(A.Yield,0) AS numeric(18, 4)) * 100 Yield1,isnull(A.A_CNT,0) OutPut1,ISNULL(A.TARGET_OUTPUT,0) StdOutput1,
                                        CAST(isnull(B.Yield,0) AS numeric(18, 4)) * 100 Yield2,isnull(B.A_CNT,0) OutPut2,ISNULL(B.TARGET_OUTPUT,0) StdOutput2,
                                        CAST(isnull(C.Yield,0) AS numeric(18, 4)) * 100 Yield3,isnull(C.A_CNT,0) OutPut3,ISNULL(C.TARGET_OUTPUT,0) StdOutput3,
                                        CAST(isnull(D.Yield,0) AS numeric(18, 4)) * 100 Yield4,isnull(D.A_CNT,0) OutPut4,ISNULL(D.TARGET_OUTPUT,0) StdOutput4,
                                        CAST(isnull(E.Yield,0) AS numeric(18, 4)) * 100 Yield5,isnull(E.A_CNT,0) OutPut5,ISNULL(E.TARGET_OUTPUT,0) StdOutput5,
                                        CAST(isnull(F.Yield,0) AS numeric(18, 4)) * 100 Yield6,isnull(F.A_CNT,0) OutPut6,ISNULL(F.TARGET_OUTPUT,0) StdOutput6
                                         FROM (SELECT DISTINCT line,Area,Product FROM #New_WLTX_SUM_YieldOutPutTargetOutput WHERE workname IN('POL','COG','FOG','TOD','FOB','LOT2') {2}{3}) AA
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='POL') A ON AA.LINE =A.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='COG') B ON AA.LINE =B.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='FOG') C ON AA.LINE =C.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='TOD') D ON AA.LINE =D.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='FOB') E ON AA.LINE =E.LINE
                                        LEFT JOIN (select * from #New_WLTX_SUM_YieldOutPutTargetOutput where WORKNAME='LOT2') F ON AA.LINE =F.LINE", strBDate, strEDate, strsqlFabArea, strFabFloor, intHours, strMinutes);
            DataSet ds = new DataSet();
            ds = conn2.GetDataSetBySql(strsql); ;
            return ds;
        }

        private static void GetTime(out string BeginTime, out string EndTime)
        {

            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 07:00";
                EndTime = DateTime.Now.ToShortDateString() + " 19:00";
            }
            else if (DateTime.Now.Hour >= 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 19:00";
                EndTime = DateTime.Now.AddDays(1).ToShortDateString() + " 07:00";
            }
            else
            {
                BeginTime = DateTime.Now.AddDays(-1).ToShortDateString() + " 19:00";
                EndTime = DateTime.Now.ToShortDateString() + " 07:00";
            }

        }

        private double GetTimeRate()
        {
            double dresult = 0;

            string BeginTime;
            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 07:00";
            }
            else if (DateTime.Now.Hour >= 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 19:00";
            }
            else
            {
                BeginTime = DateTime.Now.AddDays(-1).ToShortDateString() + " 19:00";
            }
            DateTime BaseTime = DateTime.Parse(BeginTime);

            for (int i = 0; i < TargetPercent.Length; i++)
            {
                if (DateTime.Compare(DateTime.Now.AddHours(0 - i - 1), BaseTime) > 0)
                {
                    dresult = dresult + TargetPercent[i];
                }
                else
                {
                    int minute = DateTime.Now.AddHours(0 - i - 1).Minute;
                    dresult = dresult + TargetPercent[i] * minute / 60;
                    break;
                }
            }
            dresult = dresult / 10.5; //換算為班別佔比
            return dresult;
        }



        protected void gvYieldSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ddlArea.SelectedItem.Value == "Assy")
                {
                    if (e.Row.Cells[1] != null)
                    {
                        e.Row.Cells[3].Text = "Assy Yield";
                        e.Row.Cells[4].Text = "Assy Output";
                        e.Row.Cells[5].Text = "Assy Target Output";
                        e.Row.Cells[6].Text = "Module Test Yield";
                        e.Row.Cells[7].Text = "Module Test Output";
                        e.Row.Cells[8].Text = "Module Test Target Output";
                        e.Row.Cells[9].Text = "OQC Yield";
                        e.Row.Cells[10].Text = "OQC Output";
                        e.Row.Cells[11].Text = "OQC Target Output";


                        gvYieldSummary.Columns[12].Visible = false;
                        gvYieldSummary.Columns[13].Visible = false;
                        gvYieldSummary.Columns[14].Visible = false;
                        gvYieldSummary.Columns[15].Visible = false;
                        gvYieldSummary.Columns[16].Visible = false;
                        gvYieldSummary.Columns[17].Visible = false;
                        gvYieldSummary.Columns[18].Visible = false;
                        gvYieldSummary.Columns[19].Visible = false;
                        gvYieldSummary.Columns[20].Visible = false;
                        //e.Row.Cells[12].Text = " ";
                        //e.Row.Cells[13].Text = " ";
                        //e.Row.Cells[14].Text = " ";

                        //e.Row.Cells[15].Text = " ";
                        //e.Row.Cells[16].Text = " ";
                        //e.Row.Cells[17].Text = " ";


                        //e.Row.Cells[0].Width = Unit.Pixel(75);
                    }
                }
                else if (ddlArea.SelectedItem.Value == "LAM")
                {
                    if (e.Row.Cells[1] != null)
                    {
                        e.Row.Cells[3].Text = "LAM pre-baking Yield";
                        e.Row.Cells[4].Text = "LAM pre-baking Output";
                        e.Row.Cells[5].Text = "LAM pre-baking Target Output";
                        e.Row.Cells[6].Text = "LAM Yield";
                        e.Row.Cells[7].Text = "LAM Output";
                        e.Row.Cells[8].Text = "LAM Target Output";
                        e.Row.Cells[9].Text = "LAM UV Yield";
                        e.Row.Cells[10].Text = "LAM UV Output";
                        e.Row.Cells[11].Text = "LAM UV Target Output";

                        e.Row.Cells[12].Text = "ASF 貼附 Yield";
                        e.Row.Cells[13].Text = "ASF 貼附 Output";
                        e.Row.Cells[14].Text = "ASF 貼附 Target Output";

                        e.Row.Cells[15].Text = "LAM 功能測試外觀 Yield";
                        e.Row.Cells[16].Text = "LAM 功能測試外觀 Output";
                        e.Row.Cells[17].Text = "LAM 功能測試外觀 Target Output";

                        gvYieldSummary.Columns[12].Visible = true;
                        gvYieldSummary.Columns[13].Visible = true;
                        gvYieldSummary.Columns[14].Visible = true;
                        gvYieldSummary.Columns[15].Visible = true;
                        gvYieldSummary.Columns[16].Visible = true;
                        gvYieldSummary.Columns[17].Visible = true;
                        gvYieldSummary.Columns[18].Visible = false;
                        gvYieldSummary.Columns[19].Visible = false;
                        gvYieldSummary.Columns[20].Visible = false;
                    }
                }
                else if (ddlArea.SelectedItem.Value == "Finishing")
                {
                    if (e.Row.Cells[1] != null)
                    {
                        e.Row.Cells[3].Text = "POL Yield";
                        e.Row.Cells[4].Text = "POL Output";
                        e.Row.Cells[5].Text = "POL Target Output";
                        e.Row.Cells[6].Text = "COG Yield";
                        e.Row.Cells[7].Text = "COG Output";
                        e.Row.Cells[8].Text = "COG Target Output";
                        e.Row.Cells[9].Text = "FOG Yield";
                        e.Row.Cells[10].Text = "FOG Output";
                        e.Row.Cells[11].Text = "FOG Target Output";

                        e.Row.Cells[12].Text = "TOD Yield";
                        e.Row.Cells[13].Text = "TOD Output";
                        e.Row.Cells[14].Text = "TOD Target Output";

                        e.Row.Cells[15].Text = "FOB Yield";
                        e.Row.Cells[16].Text = "FOB Output";
                        e.Row.Cells[17].Text = "FOB Target Output";

                        e.Row.Cells[18].Text = "LOT2 Yield";
                        e.Row.Cells[19].Text = "LOT2 Output";
                        e.Row.Cells[20].Text = "LOT2 Target Output";

                        gvYieldSummary.Columns[12].Visible = true;
                        gvYieldSummary.Columns[13].Visible = true;
                        gvYieldSummary.Columns[14].Visible = true;
                        gvYieldSummary.Columns[15].Visible = true;
                        gvYieldSummary.Columns[16].Visible = true;
                        gvYieldSummary.Columns[17].Visible = true;
                        gvYieldSummary.Columns[18].Visible = true;
                        gvYieldSummary.Columns[19].Visible = true;
                        gvYieldSummary.Columns[20].Visible = true;
                    }
                }



            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                double TimeRate = GetTimeRate();
                // Yield red-Light
                if (double.Parse(e.Row.Cells[3].Text.Trim()) == 0)
                {
                    e.Row.Cells[3].BackColor = Color.White;
                }
                else if (double.Parse(e.Row.Cells[3].Text.Trim()) < 98.5)
                {
                    e.Row.Cells[3].BackColor = Color.Red;
                }
                else
                {
                    e.Row.Cells[3].BackColor = Color.Green;
                }

                if (double.Parse(e.Row.Cells[6].Text.Trim()) == 0)
                {
                    e.Row.Cells[6].BackColor = Color.White;
                }
                else if (double.Parse(e.Row.Cells[6].Text.Trim()) < 98.5)
                {
                    e.Row.Cells[6].BackColor = Color.Red;
                }
                else
                {
                    e.Row.Cells[6].BackColor = Color.Green;
                }

                if (double.Parse(e.Row.Cells[9].Text.Trim()) == 0)
                {
                    e.Row.Cells[9].BackColor = Color.White;
                }
                else if (double.Parse(e.Row.Cells[9].Text.Trim()) < 98.5)
                {
                    e.Row.Cells[9].BackColor = Color.Red;
                }
                else
                {
                    e.Row.Cells[9].BackColor = Color.Green;
                }

                if (gvYieldSummary.Columns[12].Visible == true)
                {
                    if (double.Parse(e.Row.Cells[12].Text.Trim()) == 0)
                    {
                        e.Row.Cells[12].BackColor = Color.White;
                    }
                    else if (double.Parse(e.Row.Cells[12].Text.Trim()) < 98.5)
                    {
                        e.Row.Cells[12].BackColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[12].BackColor = Color.Green;
                    }
                }

                if (gvYieldSummary.Columns[15].Visible == true)
                {
                    if (double.Parse(e.Row.Cells[15].Text.Trim()) == 0)
                    {
                        e.Row.Cells[15].BackColor = Color.White;
                    }
                    else if (double.Parse(e.Row.Cells[15].Text.Trim()) < 98.5)
                    {
                        e.Row.Cells[15].BackColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[15].BackColor = Color.Green;
                    }
                }

                if (gvYieldSummary.Columns[18].Visible == true)
                {
                    if (double.Parse(e.Row.Cells[18].Text.Trim()) == 0)
                    {
                        e.Row.Cells[18].BackColor = Color.White;
                    }
                    else if (double.Parse(e.Row.Cells[18].Text.Trim()) < 98.5)
                    {
                        e.Row.Cells[18].BackColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[18].BackColor = Color.Green;
                    }
                }

                // Yield Format
                if (e.Row.Cells[3].Text == "100.0")
                {
                    e.Row.Cells[3].Text = "100";
                }
                if (e.Row.Cells[6].Text == "100.0")
                {
                    e.Row.Cells[6].Text = "100";
                }
                if (e.Row.Cells[9].Text == "100.0")
                {
                    e.Row.Cells[9].Text = "100";
                }
                if (gvYieldSummary.Columns[12].Visible == true)
                {
                    if (e.Row.Cells[12].Text == "100.0")
                    {
                        e.Row.Cells[12].Text = "100";
                    }
                }
                if (gvYieldSummary.Columns[15].Visible == true)
                {
                    if (e.Row.Cells[15].Text == "100.0")
                    {
                        e.Row.Cells[15].Text = "100";
                    }
                }
                if (gvYieldSummary.Columns[18].Visible == true)
                {
                    if (e.Row.Cells[18].Text == "100.0")
                    {
                        e.Row.Cells[18].Text = "100";
                    }
                }
                if (e.Row.Cells[3].Text == "0.0")
                {
                    e.Row.Cells[3].Text = "";
                }
                if (e.Row.Cells[6].Text == "0.0")
                {
                    e.Row.Cells[6].Text = "";
                }
                if (e.Row.Cells[9].Text == "0.0")
                {
                    e.Row.Cells[9].Text = "";
                }
                if (gvYieldSummary.Columns[12].Visible == true)
                {
                    if (e.Row.Cells[12].Text == "0.0")
                    {
                        e.Row.Cells[12].Text = "";
                    }
                }
                if (gvYieldSummary.Columns[15].Visible == true)
                {
                    if (e.Row.Cells[15].Text == "0.0")
                    {
                        e.Row.Cells[15].Text = "";
                    }
                }
                if (gvYieldSummary.Columns[18].Visible == true)
                {
                    if (e.Row.Cells[18].Text == "0.0")
                    {
                        e.Row.Cells[18].Text = "";
                    }
                }
                // 


                // Output Red-Green Light
                if (int.Parse(e.Row.Cells[4].Text) < int.Parse(e.Row.Cells[5].Text) * TimeRate * .9)
                {
                    e.Row.Cells[4].BackColor = Color.Red;
                }
                else if (int.Parse(e.Row.Cells[4].Text) < int.Parse(e.Row.Cells[5].Text) * TimeRate * 1)
                {
                    e.Row.Cells[4].BackColor = Color.Yellow;
                }
                else
                {
                    e.Row.Cells[4].BackColor = Color.Green;
                }

                if (int.Parse(e.Row.Cells[7].Text) < int.Parse(e.Row.Cells[8].Text) * TimeRate * .9)
                {
                    e.Row.Cells[7].BackColor = Color.Red;
                }
                else if (int.Parse(e.Row.Cells[7].Text) < int.Parse(e.Row.Cells[8].Text) * TimeRate * 1)
                {
                    e.Row.Cells[7].BackColor = Color.Yellow;
                }
                else
                {
                    e.Row.Cells[7].BackColor = Color.Green;
                }

                if (int.Parse(e.Row.Cells[10].Text) < int.Parse(e.Row.Cells[11].Text) * TimeRate * .9)
                {
                    e.Row.Cells[10].BackColor = Color.Red;
                }
                else if (int.Parse(e.Row.Cells[10].Text) < int.Parse(e.Row.Cells[11].Text) * TimeRate * 1)
                {
                    e.Row.Cells[10].BackColor = Color.Yellow;
                }
                else
                {
                    e.Row.Cells[10].BackColor = Color.Green;
                }
                if (gvYieldSummary.Columns[13].Visible == true)
                {
                    if (int.Parse(e.Row.Cells[13].Text) < int.Parse(e.Row.Cells[14].Text) * TimeRate * .9)
                    {
                        e.Row.Cells[13].BackColor = Color.Red;
                    }
                    else if (int.Parse(e.Row.Cells[13].Text) < int.Parse(e.Row.Cells[14].Text) * TimeRate * 1)
                    {
                        e.Row.Cells[13].BackColor = Color.Yellow;
                    }
                    else
                    {
                        e.Row.Cells[13].BackColor = Color.Green;
                    }
                }

                if (gvYieldSummary.Columns[16].Visible == true)
                {
                    if (int.Parse(e.Row.Cells[16].Text) < int.Parse(e.Row.Cells[17].Text) * TimeRate * .9)
                    {
                        e.Row.Cells[16].BackColor = Color.Red;
                    }
                    else if (int.Parse(e.Row.Cells[16].Text) < int.Parse(e.Row.Cells[17].Text) * TimeRate * 1)
                    {
                        e.Row.Cells[16].BackColor = Color.Yellow;
                    }
                    else
                    {
                        e.Row.Cells[16].BackColor = Color.Green;
                    }
                }

                if (gvYieldSummary.Columns[19].Visible == true)
                {
                    if (int.Parse(e.Row.Cells[19].Text) < int.Parse(e.Row.Cells[20].Text) * TimeRate * .9)
                    {
                        e.Row.Cells[19].BackColor = Color.Red;
                    }
                    else if (int.Parse(e.Row.Cells[19].Text) < int.Parse(e.Row.Cells[20].Text) * TimeRate * 1)
                    {
                        e.Row.Cells[19].BackColor = Color.Yellow;
                    }
                    else
                    {
                        e.Row.Cells[19].BackColor = Color.Green;
                    }
                }
                //
            }
        }

        protected void TimerPage_Tick(object sender, EventArgs e)
        {
            btnQuery_Click(sender, e);
        }
    }
}