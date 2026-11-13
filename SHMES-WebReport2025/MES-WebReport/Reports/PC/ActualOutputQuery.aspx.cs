using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Report.Library;
using System.Text;
using System.Data.SqlClient;

namespace WebReport.Reports
{
    public partial class ActualOutputQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        string strYear;
        string strMonth;
        string strWeekNum;
        string strDay1;
        string strDay2;
        string strDay3;

        DataTable dtReMark = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.Attributes.Add("style", "word-break:keep-all;word-wrap:normal");
            //下面这行是自动换行




            GridView1.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            if (!Page.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    txtMUGMM.Text = "F";
                }
                else
                {
                    SetUserInfo();
                }
                SetMFGDate();
                SetProdLineLine();
                SetApplication();
                SetCustomer();
            }
            BoundData();
        }
        private void BoundData()
        {
            string strMFGDate = "";
            string strApplication = "";
            string strProdLine = "";
            string strPRODUCTID = "";
            string strCustomer = "";
            Label2.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();

            if (DDLMFGDate.SelectedValue.ToString() == "MFGDATE")
            {
                strMFGDate = DDLMFGDate.Items[1].Value.ToString();
            }
            else
            {
                strMFGDate = DDLMFGDate.SelectedValue.ToString();
            }

            if (DDLProdLine.SelectedValue.ToString() != "PRODLINE")
            {
                strProdLine = DDLProdLine.SelectedValue.ToString();
            }

            if (DDLApplication.SelectedValue.ToString() != "APPLICATION")
            {
                strApplication = DDLApplication.SelectedValue.ToString();
            }

            if (DDLCustomer.SelectedValue.ToString() != "CUSTOMER")
            {
                strCustomer = DDLCustomer.SelectedValue.ToString();
            }

            strMonth = "";
            strWeekNum = "";
            strDay1 = "";
            strDay2 = "";
            strDay3 = "";

            StringBuilder strsql = new StringBuilder();
            strsql.Append(" SELECT DATEPART(YEAR,'" + (strMFGDate) + "') AS YEAR,DATEPART(MONTH,'" + (strMFGDate) + "') AS MONTH,DATEPART(WEEK,'" + (strMFGDate) + "') AS  WEEKNUM, ");
            strsql.Append("  SUBSTRING(Convert(Varchar(10),CAST('" + (strMFGDate) + "' AS DATETIME) ,111),6,5) AS TDAY, ");
            strsql.Append("  SUBSTRING(Convert(Varchar(10),CAST('" + (strMFGDate) + "' AS DATETIME) - 1 ,111),6,5) AS DAY1, ");
            strsql.Append("  SUBSTRING(Convert(Varchar(10),CAST('" + (strMFGDate) + "' AS DATETIME) - 2 ,111),6,5) AS DAY2 ");
            DataTable dt = this.dbConn.GetDataSetBySql(strsql.ToString()).Tables[0];
            strYear = dt.Rows[0].ItemArray[0].ToString();
            strMonth = dt.Rows[0].ItemArray[1].ToString();
            strWeekNum = dt.Rows[0].ItemArray[2].ToString();

            strDay3 = dt.Rows[0].ItemArray[3].ToString();
            strDay2 = dt.Rows[0].ItemArray[4].ToString();
            strDay1 = dt.Rows[0].ItemArray[5].ToString();

            DataTable dtSum = new DataTable();
            dtSum.Columns.Add("FABID", typeof(string));
            dtSum.Columns.Add("PRODINFO", typeof(string));
            dtSum.Columns.Add("Category", typeof(string));
            dtSum.Columns.Add("TTL", typeof(string));
            dtSum.Columns.Add("MTD", typeof(string));
            dtSum.Columns.Add("WTD", typeof(string));
            dtSum.Columns.Add("T1", typeof(string));
            dtSum.Columns.Add("T2", typeof(string));
            dtSum.Columns.Add("T3", typeof(string));
            dtSum.Columns.Add("H8", typeof(string));
            dtSum.Columns.Add("H9", typeof(string));
            dtSum.Columns.Add("H10", typeof(string));
            dtSum.Columns.Add("H11", typeof(string));
            dtSum.Columns.Add("H12", typeof(string));
            dtSum.Columns.Add("H13", typeof(string));
            dtSum.Columns.Add("H14", typeof(string));
            dtSum.Columns.Add("H15", typeof(string));
            dtSum.Columns.Add("H16", typeof(string));
            dtSum.Columns.Add("H17", typeof(string));
            dtSum.Columns.Add("H18", typeof(string));
            dtSum.Columns.Add("H19", typeof(string));
            dtSum.Columns.Add("H20", typeof(string));
            dtSum.Columns.Add("H21", typeof(string));
            dtSum.Columns.Add("H22", typeof(string));
            dtSum.Columns.Add("H23", typeof(string));
            dtSum.Columns.Add("H0", typeof(string));
            dtSum.Columns.Add("H1", typeof(string));
            dtSum.Columns.Add("H2", typeof(string));
            dtSum.Columns.Add("H3", typeof(string));
            dtSum.Columns.Add("H4", typeof(string));
            dtSum.Columns.Add("H5", typeof(string));
            dtSum.Columns.Add("H6", typeof(string));
            dtSum.Columns.Add("H7", typeof(string));
            dtSum.Columns.Add("Sort", typeof(string));
            dtSum.Columns.Add("ProductID", typeof(string));

            StringBuilder sqlWhere = new StringBuilder();
            if (strApplication != "")
            {
                sqlWhere.Append(" AND Application ='" + strApplication + "' ");
            }
            if (strProdLine != "")
            {
                sqlWhere.Append(" AND PRODLINE = '" + strProdLine + "' ");
            }
            if (strCustomer != "")
            {
                sqlWhere.Append(" AND PRODUCTID IN(SELECT PRODID FROM MPDCSTM WHERE CUSTOMER ='" + strCustomer + "') ");
            }

            /*查詢Plan Summary Data (Original) (每月2號保留的一版資料，一個月就一天的資料)*/
            StringBuilder sqlFirstMTDPlan = new StringBuilder();
            sqlFirstMTDPlan.Append(" SELECT FABID,SUM(ALL_PLAN_QTY) AS First_MTDPlan FROM ROPPSDO WHERE YEAR = DATEPART(YEAR,'" + (strMFGDate) + "') AND MONTH = DATEPART(MONTH,'" + (strMFGDate) + "') ");
            sqlFirstMTDPlan.Append(" GROUP BY FABID");
            DataTable dtFirstMTDPlan = this.dbConn.GetDataSetBySql(sqlFirstMTDPlan.ToString()).Tables[0];

            /*查詢Plan Summary Data（每小時將Plan Raw Data Summary的資料)*/
            StringBuilder sqlSumPlan = new StringBuilder();
            sqlSumPlan.Append(" SELECT FABID,'','','',PRODLINE,PRODUCTID, ");
            sqlSumPlan.Append(" SUM(DTH_PLAN_QTY) AS ALL_PLAN_QTY, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE <= '" + (strMFGDate) + "'  THEN DTH_PLAN_QTY ELSE 0 END) AS MTD_PLAN_QTY, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE <= '" + (strMFGDate) + "'  AND WEEKNUM = DATEPART(WEEK,'" + (strMFGDate) + "') THEN DTH_PLAN_QTY ELSE 0 END) AS WTD_PLAN_QTY, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = CAST('" + (strMFGDate) + "' AS DATETIME)- 2 THEN DTH_PLAN_QTY ELSE 0 END) AS P_2D_PLAN_QTY, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = CAST('" + (strMFGDate) + "' AS DATETIME)- 1 THEN DTH_PLAN_QTY ELSE 0 END) AS P_1D_PLAN_QTY, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN DTH_PLAN_QTY ELSE 0 END) AS DTH_PLAN_QTY,  ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H8_PLAN ELSE 0 END) AS H8_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H9_PLAN ELSE 0 END) AS H9_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H10_PLAN ELSE 0 END) AS H10_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H11_PLAN ELSE 0 END) AS H11_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H12_PLAN ELSE 0 END) AS H12_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H13_PLAN ELSE 0 END) AS H13_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H14_PLAN ELSE 0 END) AS H14_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H15_PLAN ELSE 0 END) AS H15_PLAN,   ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H16_PLAN ELSE 0 END) AS H16_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H17_PLAN ELSE 0 END) AS H17_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H18_PLAN ELSE 0 END) AS H18_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H19_PLAN ELSE 0 END) AS H19_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H20_PLAN ELSE 0 END) AS H20_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H21_PLAN ELSE 0 END) AS H21_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H22_PLAN ELSE 0 END) AS H22_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H23_PLAN ELSE 0 END) AS H23_PLAN,   ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H24_PLAN ELSE 0 END) AS H24_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H1_PLAN ELSE 0 END) AS H1_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H2_PLAN ELSE 0 END) AS H2_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H3_PLAN ELSE 0 END) AS H3_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H4_PLAN ELSE 0 END) AS H4_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H5_PLAN ELSE 0 END) AS H5_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H6_PLAN ELSE 0 END) AS H6_PLAN, ");
            sqlSumPlan.Append(" SUM(CASE WHEN MFGDATE = '" + (strMFGDate) + "' THEN H7_PLAN ELSE 0 END) AS H7_PLAN  ");
            sqlSumPlan.Append(" FROM ROPPSD ");
            sqlSumPlan.Append(" WHERE MONTH = DATEPART(MONTH,'" + (strMFGDate) + "') ");
            sqlSumPlan.Append(" GROUP BY FABID,PRODLINE,PRODUCTID ");
            DataTable dtSumPlan = this.dbConn.GetDataSetBySql(sqlSumPlan.ToString()).Tables[0];

            /*查詢Actual 生產的線，以便後續循環處理*/
            StringBuilder sqlLine = new StringBuilder();
            sqlLine.Append(" SELECT  DISTINCT FABID,PRODLINE,PRODUCTID FROM ROPASD ");
            sqlLine.Append(" WHERE MFGDATE = '" + (strMFGDate) + "' ");
            sqlLine.Append(" " + sqlWhere.ToString() + "");
            sqlLine.Append("ORDER BY FABID,PRODLINE,PRODUCTID");
            DataTable dtLine = this.dbConn.GetDataSetBySql(sqlLine.ToString()).Tables[0];

            /*By Date查詢Actual Summary Data*/
            StringBuilder sqlTTL = new StringBuilder();
            sqlTTL.Append(" SELECT FABID,MONTH,WEEKNUM,MFGDATE,PRODLINE,PRODUCTID,0,0,0, ");
            sqlTTL.Append(" MAX(P_2D_ACT_QTY) AS P_2D_ACT_QTY,MAX(P_1D_ACT_QTY) AS P_1D_ACT_QTY,SUM(HOUR_ACT_QTY) AS DTH_ACT_QTY, ");
            sqlTTL.Append(" SUM(H8) AS H8,SUM(H9) AS H9,SUM(H10) AS H10,SUM(H11) AS H11,SUM(H12) AS H12,SUM(H13) AS H13,SUM(H14) AS H14, ");
            sqlTTL.Append(" SUM(H15) AS H15,SUM(H16) AS H16,SUM(H17) AS H17,SUM(H18) AS H18,SUM(H19) AS H19,SUM(H20) AS H20,SUM(H21) AS H21, ");
            sqlTTL.Append(" SUM(H22) AS H22,SUM(H23) AS H23,SUM(H0) AS H0,SUM(H1) AS H1,SUM(H2) AS H2,SUM(H3) AS H3,SUM(H4) AS H4,SUM(H5) AS H5, ");
            sqlTTL.Append(" SUM(H6) AS H6,SUM(H7) AS H7 ");
            sqlTTL.Append(" FROM( ");
            sqlTTL.Append(" SELECT FABID,MONTH,WEEKNUM,MFGDATE,PRODLINE,PRODUCTID,DTH_ACT_QTY,P_1D_ACT_QTY,P_2D_ACT_QTY,HOUR_ACT_QTY, ");
            sqlTTL.Append(" CASE WHEN HOUR = 8	THEN HOUR_ACT_QTY ELSE 0 END AS H8, ");
            sqlTTL.Append(" CASE WHEN HOUR = 9	THEN HOUR_ACT_QTY ELSE 0 END AS H9, ");
            sqlTTL.Append(" CASE WHEN HOUR = 10	THEN HOUR_ACT_QTY ELSE 0 END AS H10, ");
            sqlTTL.Append(" CASE WHEN HOUR = 11	THEN HOUR_ACT_QTY ELSE 0 END AS H11, ");
            sqlTTL.Append(" CASE WHEN HOUR = 12	THEN HOUR_ACT_QTY ELSE 0 END AS H12, ");
            sqlTTL.Append(" CASE WHEN HOUR = 13	THEN HOUR_ACT_QTY ELSE 0 END AS H13, ");
            sqlTTL.Append(" CASE WHEN HOUR = 14	THEN HOUR_ACT_QTY ELSE 0 END AS H14, ");
            sqlTTL.Append(" CASE WHEN HOUR = 15	THEN HOUR_ACT_QTY ELSE 0 END AS H15, ");
            sqlTTL.Append(" CASE WHEN HOUR = 16	THEN HOUR_ACT_QTY ELSE 0 END AS H16, ");
            sqlTTL.Append(" CASE WHEN HOUR = 17	THEN HOUR_ACT_QTY ELSE 0 END AS H17, ");
            sqlTTL.Append(" CASE WHEN HOUR = 18	THEN HOUR_ACT_QTY ELSE 0 END AS H18, ");
            sqlTTL.Append(" CASE WHEN HOUR = 19	THEN HOUR_ACT_QTY ELSE 0 END AS H19, ");
            sqlTTL.Append(" CASE WHEN HOUR = 20	THEN HOUR_ACT_QTY ELSE 0 END AS H20, ");
            sqlTTL.Append(" CASE WHEN HOUR = 21	THEN HOUR_ACT_QTY ELSE 0 END AS H21, ");
            sqlTTL.Append(" CASE WHEN HOUR = 22	THEN HOUR_ACT_QTY ELSE 0 END AS H22, ");
            sqlTTL.Append(" CASE WHEN HOUR = 23	THEN HOUR_ACT_QTY ELSE 0 END AS H23, ");
            sqlTTL.Append(" CASE WHEN HOUR = 0	THEN HOUR_ACT_QTY ELSE 0 END AS H0, ");
            sqlTTL.Append(" CASE WHEN HOUR = 1	THEN HOUR_ACT_QTY ELSE 0 END AS H1, ");
            sqlTTL.Append(" CASE WHEN HOUR = 2	THEN HOUR_ACT_QTY ELSE 0 END AS H2, ");
            sqlTTL.Append(" CASE WHEN HOUR = 3	THEN HOUR_ACT_QTY ELSE 0 END AS H3, ");
            sqlTTL.Append(" CASE WHEN HOUR = 4	THEN HOUR_ACT_QTY ELSE 0 END AS H4, ");
            sqlTTL.Append(" CASE WHEN HOUR = 5	THEN HOUR_ACT_QTY ELSE 0 END AS H5, ");
            sqlTTL.Append(" CASE WHEN HOUR = 6	THEN HOUR_ACT_QTY ELSE 0 END AS H6, ");
            sqlTTL.Append(" CASE WHEN HOUR = 7	THEN HOUR_ACT_QTY ELSE 0 END AS H7 ");
            sqlTTL.Append(" FROM ROPASD  ");
            sqlTTL.Append(" WHERE MFGDATE = '" + (strMFGDate) + "'  ");
            sqlTTL.Append(" )A GROUP BY FABID,MONTH,WEEKNUM,MFGDATE,PRODLINE,PRODUCTID ");
            DataTable dtTTL = this.dbConn.GetDataSetBySql(sqlTTL.ToString()).Tables[0];


            /*By Month&Week查詢Actual Summary Data*/
            StringBuilder sqlMonthTTL = new StringBuilder();
            sqlMonthTTL.Append(" SELECT FABID,PRODLINE,SUM(MONTHQTY) AS MONTHQTY,SUM(MTDQTY) AS MTDQTY,SUM(WTDQTY) AS WTDQTY,PRODUCTID,");
            sqlMonthTTL.Append(" SUM(QTYDAY1) AS DAY1,");
            sqlMonthTTL.Append(" SUM(QTYDAY2) AS DAY2,");
            sqlMonthTTL.Append(" SUM(QTYDAY3) AS DAY3 FROM(  ");
            sqlMonthTTL.Append(" SELECT FABID,MFGDATE,PRODLINE,PRODUCTID,HOUR_ACT_QTY AS MONTHQTY, ");
            sqlMonthTTL.Append(" CASE WHEN MFGDATE <= '" + (strMFGDate) + "'  " + sqlWhere.ToString() + " ");
            sqlMonthTTL.Append(" THEN HOUR_ACT_QTY ELSE 0 END AS MTDQTY, ");
            sqlMonthTTL.Append(" CASE WHEN MFGDATE <= '" + (strMFGDate) + "' AND WEEKNUM = DATEPART(WEEK, '" + (strMFGDate) + "' ) " + sqlWhere.ToString() + " ");
            sqlMonthTTL.Append(" THEN HOUR_ACT_QTY ELSE 0 END AS WTDQTY, ");

            sqlMonthTTL.Append(" CASE WHEN MFGDATE + 2 = '" + (strMFGDate) + "' " + sqlWhere.ToString() + " THEN HOUR_ACT_QTY ELSE 0 END AS QTYDAY1,");
            sqlMonthTTL.Append(" CASE WHEN MFGDATE + 1 = '" + (strMFGDate) + "' " + sqlWhere.ToString() + " THEN HOUR_ACT_QTY ELSE 0 END AS QTYDAY2,");
            sqlMonthTTL.Append(" CASE WHEN MFGDATE = '" + (strMFGDate) + "' " + sqlWhere.ToString() + " THEN HOUR_ACT_QTY ELSE 0 END AS QTYDAY3 ");

            sqlMonthTTL.Append(" FROM ROPASD  ");
            sqlMonthTTL.Append(" WHERE MONTH = DATEPART(MONTH,'" + (strMFGDate) + "') )AA  ");
            sqlMonthTTL.Append(" GROUP BY FABID,PRODLINE,PRODUCTID ");
            DataTable dtMonthTTL = this.dbConn.GetDataSetBySql(sqlMonthTTL.ToString()).Tables[0];

            /*查詢Remark Data,寫到頁面中*/
            StringBuilder sqlMark = new StringBuilder();
            sqlMark.Append(" SELECT FABID,MONTH,WEEKNUM,MFGDATE,PRODLINE,'' AS PRODUCTID,'' AS ALL_RMK,MTD_RMK,WTD_RMK,First_2D_RMK,Prev_1D_RMK,DTH_RMK, ");
            sqlMark.Append(" H8_RMK,H9_RMK,H10_RMK,H11_RMK,H12_RMK,H13_RMK,H14_RMK,H15_RMK,H16_RMK,H17_RMK,H18_RMK,H19_RMK,H20_RMK,H21_RMK, ");
            sqlMark.Append(" H22_RMK,H23_RMK,H24_RMK,H1_RMK,H2_RMK,H3_RMK,H4_RMK,H5_RMK,H6_RMK,H7_RMK ");
            sqlMark.Append(" FROM ROPRD ");
            sqlMark.Append(" WHERE MFGDATE = '" + (strMFGDate) + "' ");
            if (strProdLine != "")
            {
                sqlMark.Append(" AND PRODLINE = '" + strProdLine + "' ");
            }
            DataTable dtMark = this.dbConn.GetDataSetBySql(sqlMark.ToString()).Tables[0];
            /*複製 備註說明 的資料集，供後續RowDataBound事件 去填寫介面的"備註說明"的內容*/
            dtReMark = dtMark.Copy();

            ///*查詢Work Hour Data,做合併儲存格*/
            //StringBuilder sqlWorkSys = new StringBuilder();
            //sqlWorkSys.Append(" SELECT FABID,WORKSYSTEM,HOUR,BEGINTIME,ENDTIME FROM MWKSYSM ");
            //DataTable dtWorkSys = this.dbConn.GetDataSetBySql(sqlWorkSys.ToString()).Tables[0];
            ///*複製 Work Hour 的資料集，供後續RowDataBound事件 去做合併儲存格*/
            //dtWorkHour = dtWorkSys.Copy();

            if (dtLine.Rows.Count > 0)
            {
                string strFabID = "";   /*廠別代碼*/
                string strSort = "";    /*排序用 總廠別為0 LCM1:1  LCM2:2  VAP:3  LINE:9*/
                int intFABFST = 0;      /*各廠初次PLAN的值*/
                int intALLFST = 0;      /*所有廠初次PLAN值總和*/
                long intALLTTL = 0, intALLMTD = 0, intALLWTD = 0;   /*存放 TTL的SUMMARY的值*/

                /*計算所有廠別的PLAN&ACT和用*/
                int intALLPlanTTL = 0, intALLPlanMTD = 0, intALLPlanWTD = 0, intALLPlanT1 = 0, intALLPlanT2 = 0, intALLPlanT3 = 0;
                int intALLPlanH8 = 0, intALLPlanH9 = 0, intALLPlanH10 = 0, intALLPlanH11 = 0, intALLPlanH12 = 0, intALLPlanH13 = 0, intALLPlanH14 = 0, intALLPlanH15 = 0;
                int intALLPlanH16 = 0, intALLPlanH17 = 0, intALLPlanH18 = 0, intALLPlanH19 = 0, intALLPlanH20 = 0, intALLPlanH21 = 0, intALLPlanH22 = 0, intALLPlanH23 = 0;
                int intALLPlanH0 = 0, intALLPlanH1 = 0, intALLPlanH2 = 0, intALLPlanH3 = 0, intALLPlanH4 = 0, intALLPlanH5 = 0, intALLPlanH6 = 0, intALLPlanH7 = 0;

                int intALLActTTL = 0, intALLActMTD = 0, intALLActWTD = 0, intALLActT1 = 0, intALLActT2 = 0, intALLActT3 = 0;
                int intALLActH8 = 0, intALLActH9 = 0, intALLActH10 = 0, intALLActH11 = 0, intALLActH12 = 0, intALLActH13 = 0, intALLActH14 = 0, intALLActH15 = 0;
                int intALLActH16 = 0, intALLActH17 = 0, intALLActH18 = 0, intALLActH19 = 0, intALLActH20 = 0, intALLActH21 = 0, intALLActH22 = 0, intALLActH23 = 0;
                int intALLActH0 = 0, intALLActH1 = 0, intALLActH2 = 0, intALLActH3 = 0, intALLActH4 = 0, intALLActH5 = 0, intALLActH6 = 0, intALLActH7 = 0;

                DataRow drSum = dtSum.NewRow();
                /*目前就只有LCM1&LCM2&VAP 3個廠要看，故循環只要處理3次即可*/
                //for (int intF = 0; intF < 3; intF++)
                  for (int intF = 0; intF < 1; intF++)
                {
                    strFabID = "";
                    intFABFST = 0;

                    /*計算各廠別的PLAN&ACT用*/
                    int intPlanTTL = 0, intPlanMTD = 0, intPlanWTD = 0, intPlanT1 = 0, intPlanT2 = 0, intPlanT3 = 0;
                    int intPlanH8 = 0, intPlanH9 = 0, intPlanH10 = 0, intPlanH11 = 0, intPlanH12 = 0, intPlanH13 = 0, intPlanH14 = 0, intPlanH15 = 0;
                    int intPlanH16 = 0, intPlanH17 = 0, intPlanH18 = 0, intPlanH19 = 0, intPlanH20 = 0, intPlanH21 = 0, intPlanH22 = 0, intPlanH23 = 0;
                    int intPlanH0 = 0, intPlanH1 = 0, intPlanH2 = 0, intPlanH3 = 0, intPlanH4 = 0, intPlanH5 = 0, intPlanH6 = 0, intPlanH7 = 0;

                    int intActTTL = 0, intActMTD = 0, intActWTD = 0, intActT1 = 0, intActT2 = 0, intActT3 = 0;
                    int intActH8 = 0, intActH9 = 0, intActH10 = 0, intActH11 = 0, intActH12 = 0, intActH13 = 0, intActH14 = 0, intActH15 = 0;
                    int intActH16 = 0, intActH17 = 0, intActH18 = 0, intActH19 = 0, intActH20 = 0, intActH21 = 0, intActH22 = 0, intActH23 = 0;
                    int intActH0 = 0, intActH1 = 0, intActH2 = 0, intActH3 = 0, intActH4 = 0, intActH5 = 0, intActH6 = 0, intActH7 = 0;

                    /*存放 TTL的SUMMARY的值*/
                    int intFABTTL = 0, intFABMTD = 0, intFABWTD = 0, intFABDAY1 = 0, intFABDAY2 = 0, intFABDAY3 = 0;

                    /*定義每次循環處理的廠別*/
                    switch (intF)
                    {
                        //case 0:
                        //    strFabID = "LCM1";
                        //    strSort = "1";
                        //    break;
                        case 0:
                            strFabID = "SH";
                            strSort = "2";
                            break;
                        //case 2:
                        //    strFabID = "VAP";
                        //    strSort = "3";
                        //    break;
                    }
                    /*根據當天有資料的線別進行循環處理*/
                    DataRow[] drLine = dtLine.Select("(FABID = '" + strFabID + "')", "PRODLINE,PRODUCTID");
                    if (drLine.Length > 0)
                    {
                        for (int k = 0; k < Convert.ToInt32(drLine.Length); k++)
                        {
                            /*目前處理的線別*/
                            string strLineID = "";
                            strLineID = drLine[k].ItemArray[1].ToString();
                            strPRODUCTID = drLine[k].ItemArray[2].ToString();
                            for (int j = 0; j < 4; j++)
                            {
                                string strProdID = "", strCategory = "";
                                bool bolTrue = false;

                                strProdID = strPRODUCTID;

                                int intTTL = 0, intMTD = 0, intWTD = 0, intT1 = 0, intT2 = 0, intT3 = 0;
                                int intH8 = 0, intH9 = 0, intH10 = 0, intH11 = 0, intH12 = 0, intH13 = 0, intH14 = 0, intH15 = 0;
                                int intH16 = 0, intH17 = 0, intH18 = 0, intH19 = 0, intH20 = 0, intH21 = 0, intH22 = 0, intH23 = 0;
                                int intH0 = 0, intH1 = 0, intH2 = 0, intH3 = 0, intH4 = 0, intH5 = 0, intH6 = 0, intH7 = 0;

                                DataRow[] drQuery = new DataRow[35];
                                drQuery = dtSumPlan.Select("FABID = '' ");
                                switch (j)
                                {
                                    case 0:
                                        strCategory = "計畫產出";
                                        drQuery = dtSumPlan.Select("(FABID = '" + strFabID + "' AND PRODLINE='" + strLineID + "' AND PRODUCTID='" + strPRODUCTID + "')", "FABID,PRODLINE");
                                        break;
                                    case 1:
                                        strCategory = "實際產出";
                                        drQuery = dtTTL.Select("(FABID = '" + strFabID + "' AND PRODLINE='" + strLineID + "' AND PRODUCTID='" + strPRODUCTID + "')", "FABID,PRODLINE");
                                        break;
                                    case 2:
                                        strCategory = "達成率";
                                        drQuery = dtSumPlan.Select("FABID = '' ");
                                        break;
                                    case 3:
                                        strCategory = "備註說明";
                                        drQuery = dtSumPlan.Select("FABID = '' ");
                                        break;
                                }
                                if (drQuery.Length > 0)
                                {
                                    if (j == 0 || j == 1)
                                    {
                                        bolTrue = true;
                                    }
                                    else
                                    {
                                        bolTrue = false;
                                    }
                                    for (int t = 0; t < Convert.ToInt32(drQuery.Length); t++)
                                    {
                                        if (j == 1)
                                        {
                                            DataRow[] drMWTTL = dtMonthTTL.Select("(FABID = '" + strFabID + "' AND PRODLINE = '" + strLineID + "' AND PRODUCTID='" + strPRODUCTID + "')");
                                            if (drMWTTL.Length > 0)
                                            {
                                                intTTL = Convert.ToInt32(drMWTTL[0].ItemArray[2]);
                                                intMTD = Convert.ToInt32(drMWTTL[0].ItemArray[3]);
                                                intWTD = Convert.ToInt32(drMWTTL[0].ItemArray[4]);
                                                intT1 = Convert.ToInt32(drMWTTL[0].ItemArray[6]);
                                                intT2 = Convert.ToInt32(drMWTTL[0].ItemArray[7]);
                                                intT3 = Convert.ToInt32(drMWTTL[0].ItemArray[8]);
                                            }
                                            //strProdID = drQuery[0].ItemArray[5].ToString();
                                        }
                                        else
                                        {
                                            intTTL = intTTL + Convert.ToInt32(drQuery[0].ItemArray[6]);
                                            intMTD = intMTD + Convert.ToInt32(drQuery[0].ItemArray[7]);
                                            intWTD = intWTD + Convert.ToInt32(drQuery[0].ItemArray[8]);
                                            intT1 = intT1 + Convert.ToInt32(drQuery[0].ItemArray[9]);
                                            intT2 = intT2 + Convert.ToInt32(drQuery[0].ItemArray[10]);
                                            intT3 = intT3 + Convert.ToInt32(drQuery[0].ItemArray[11]);
                                        }
                                        intH8 = intH8 + Convert.ToInt32(drQuery[0].ItemArray[12]);
                                        intH9 = intH9 + Convert.ToInt32(drQuery[0].ItemArray[13]);
                                        intH10 = intH10 + Convert.ToInt32(drQuery[0].ItemArray[14]);
                                        intH11 = intH11 + Convert.ToInt32(drQuery[0].ItemArray[15]);
                                        intH12 = intH12 + Convert.ToInt32(drQuery[0].ItemArray[16]);
                                        intH13 = intH13 + Convert.ToInt32(drQuery[0].ItemArray[17]);
                                        intH14 = intH14 + Convert.ToInt32(drQuery[0].ItemArray[18]);
                                        intH15 = intH15 + Convert.ToInt32(drQuery[0].ItemArray[19]);
                                        intH16 = intH16 + Convert.ToInt32(drQuery[0].ItemArray[20]);
                                        intH17 = intH17 + Convert.ToInt32(drQuery[0].ItemArray[21]);
                                        intH18 = intH18 + Convert.ToInt32(drQuery[0].ItemArray[22]);
                                        intH19 = intH19 + Convert.ToInt32(drQuery[0].ItemArray[23]);
                                        intH20 = intH20 + Convert.ToInt32(drQuery[0].ItemArray[24]);
                                        intH21 = intH21 + Convert.ToInt32(drQuery[0].ItemArray[25]);
                                        intH22 = intH22 + Convert.ToInt32(drQuery[0].ItemArray[26]);
                                        intH23 = intH23 + Convert.ToInt32(drQuery[0].ItemArray[27]);
                                        intH0 = intH0 + Convert.ToInt32(drQuery[0].ItemArray[28]);
                                        intH1 = intH1 + Convert.ToInt32(drQuery[0].ItemArray[29]);
                                        intH2 = intH2 + Convert.ToInt32(drQuery[0].ItemArray[30]);
                                        intH3 = intH3 + Convert.ToInt32(drQuery[0].ItemArray[31]);
                                        intH4 = intH4 + Convert.ToInt32(drQuery[0].ItemArray[32]);
                                        intH5 = intH5 + Convert.ToInt32(drQuery[0].ItemArray[33]);
                                        intH6 = intH6 + Convert.ToInt32(drQuery[0].ItemArray[34]);
                                        intH7 = intH7 + Convert.ToInt32(drQuery[0].ItemArray[35]);
                                    }
                                }
                                drSum = dtSum.NewRow();
                                drSum["FABID"] = strFabID;
                                drSum["PRODINFO"] = strLineID.ToString();
                                drSum["Category"] = strCategory;
                                drSum["TTL"] = (intTTL == 0 && bolTrue == false) ? "" : ((intTTL == 0) ? "0" : intTTL.ToString("###,###.##"));
                                drSum["MTD"] = (intTTL == 0 && bolTrue == false) ? "" : ((intTTL == 0) ? "0" : intMTD.ToString("###,###.##"));
                                drSum["WTD"] = (intWTD == 0 && bolTrue == false) ? "" : ((intWTD == 0) ? "0" : intWTD.ToString("###,###.##"));
                                drSum["T1"] = (intT1 == 0 && bolTrue == false) ? "" : ((intT1 == 0) ? "0" : intT1.ToString("###,###.##"));
                                drSum["T2"] = (intT2 == 0 && bolTrue == false) ? "" : ((intT2 == 0) ? "0" : intT2.ToString("###,###.##"));
                                drSum["T3"] = (intT3 == 0 && bolTrue == false) ? "" : ((intT3 == 0) ? "0" : intT3.ToString("###,###.##"));
                                drSum["H8"] = (intH8 == 0 && bolTrue == false) ? "" : ((intH8 == 0) ? "0" : intH8.ToString("###,###.##"));
                                drSum["H9"] = (intH9 == 0 && bolTrue == false) ? "" : ((intH9 == 0) ? "0" : intH9.ToString("###,###.##"));
                                drSum["H10"] = (intH10 == 0 && bolTrue == false) ? "" : ((intH10 == 0) ? "0" : intH10.ToString("###,###.##"));
                                drSum["H11"] = (intH11 == 0 && bolTrue == false) ? "" : ((intH11 == 0) ? "0" : intH11.ToString("###,###.##"));
                                drSum["H12"] = (intH12 == 0 && bolTrue == false) ? "" : ((intH12 == 0) ? "0" : intH12.ToString("###,###.##"));
                                drSum["H13"] = (intH13 == 0 && bolTrue == false) ? "" : ((intH13 == 0) ? "0" : intH13.ToString("###,###.##"));
                                drSum["H14"] = (intH14 == 0 && bolTrue == false) ? "" : ((intH14 == 0) ? "0" : intH14.ToString("###,###.##"));
                                drSum["H15"] = (intH15 == 0 && bolTrue == false) ? "" : ((intH15 == 0) ? "0" : intH15.ToString("###,###.##"));
                                drSum["H16"] = (intH16 == 0 && bolTrue == false) ? "" : ((intH16 == 0) ? "0" : intH16.ToString("###,###.##"));
                                drSum["H17"] = (intH17 == 0 && bolTrue == false) ? "" : ((intH17 == 0) ? "0" : intH17.ToString("###,###.##"));
                                drSum["H18"] = (intH18 == 0 && bolTrue == false) ? "" : ((intH18 == 0) ? "0" : intH18.ToString("###,###.##"));
                                drSum["H19"] = (intH19 == 0 && bolTrue == false) ? "" : ((intH19 == 0) ? "0" : intH19.ToString("###,###.##"));
                                drSum["H20"] = (intH20 == 0 && bolTrue == false) ? "" : ((intH20 == 0) ? "0" : intH20.ToString("###,###.##"));
                                drSum["H21"] = (intH21 == 0 && bolTrue == false) ? "" : ((intH21 == 0) ? "0" : intH21.ToString("###,###.##"));
                                drSum["H22"] = (intH22 == 0 && bolTrue == false) ? "" : ((intH22 == 0) ? "0" : intH22.ToString("###,###.##"));
                                drSum["H23"] = (intH23 == 0 && bolTrue == false) ? "" : ((intH23 == 0) ? "0" : intH23.ToString("###,###.##"));
                                drSum["H0"] = (intH0 == 0 && bolTrue == false) ? "" : ((intH0 == 0) ? "0" : intH0.ToString("###,###.##"));
                                drSum["H1"] = (intH1 == 0 && bolTrue == false) ? "" : ((intH1 == 0) ? "0" : intH1.ToString("###,###.##"));
                                drSum["H2"] = (intH2 == 0 && bolTrue == false) ? "" : ((intH2 == 0) ? "0" : intH2.ToString("###,###.##"));
                                drSum["H3"] = (intH3 == 0 && bolTrue == false) ? "" : ((intH3 == 0) ? "0" : intH3.ToString("###,###.##"));
                                drSum["H4"] = (intH4 == 0 && bolTrue == false) ? "" : ((intH4 == 0) ? "0" : intH4.ToString("###,###.##"));
                                drSum["H5"] = (intH5 == 0 && bolTrue == false) ? "" : ((intH5 == 0) ? "0" : intH5.ToString("###,###.##"));
                                drSum["H6"] = (intH6 == 0 && bolTrue == false) ? "" : ((intH6 == 0) ? "0" : intH6.ToString("###,###.##"));
                                drSum["H7"] = (intH7 == 0 && bolTrue == false) ? "" : ((intH7 == 0) ? "0" : intH7.ToString("###,###.##"));
                                drSum["Sort"] = "9";
                                drSum["ProductID"] = strProdID;

                                dtSum.Rows.Add(drSum);

                                if (j == 0)
                                {
                                    intPlanTTL = intPlanTTL + intTTL;
                                    intPlanMTD = intPlanMTD + intMTD;
                                    intPlanWTD = intPlanWTD + intWTD;
                                    intPlanT1 = intPlanT1 + intT1;
                                    intPlanT2 = intPlanT2 + intT2;
                                    intPlanT3 = intPlanT3 + intT3;
                                    intPlanH8 = intPlanH8 + intH8;
                                    intPlanH9 = intPlanH9 + intH9;
                                    intPlanH10 = intPlanH10 + intH10;
                                    intPlanH11 = intPlanH11 + intH11;
                                    intPlanH12 = intPlanH12 + intH12;
                                    intPlanH13 = intPlanH13 + intH13;
                                    intPlanH14 = intPlanH14 + intH14;
                                    intPlanH15 = intPlanH15 + intH15;
                                    intPlanH16 = intPlanH16 + intH16;
                                    intPlanH17 = intPlanH17 + intH17;
                                    intPlanH18 = intPlanH18 + intH18;
                                    intPlanH19 = intPlanH19 + intH19;
                                    intPlanH20 = intPlanH20 + intH20;
                                    intPlanH21 = intPlanH21 + intH21;
                                    intPlanH22 = intPlanH22 + intH22;
                                    intPlanH23 = intPlanH23 + intH23;
                                    intPlanH0 = intPlanH0 + intH0;
                                    intPlanH1 = intPlanH1 + intH1;
                                    intPlanH2 = intPlanH2 + intH2;
                                    intPlanH3 = intPlanH3 + intH3;
                                    intPlanH4 = intPlanH4 + intH4;
                                    intPlanH5 = intPlanH5 + intH5;
                                    intPlanH6 = intPlanH6 + intH6;
                                    intPlanH7 = intPlanH7 + intH7;
                                }
                                if (j == 1)
                                {
                                    intActTTL = intActTTL + intTTL;
                                    intActMTD = intActMTD + intMTD;
                                    intActWTD = intActWTD + intWTD;
                                    intActT1 = intActT1 + intT1;
                                    intActT2 = intActT2 + intT2;
                                    intActT3 = intActT3 + intT3;
                                    intActH8 = intActH8 + intH8;
                                    intActH9 = intActH9 + intH9;
                                    intActH10 = intActH10 + intH10;
                                    intActH11 = intActH11 + intH11;
                                    intActH12 = intActH12 + intH12;
                                    intActH13 = intActH13 + intH13;
                                    intActH14 = intActH14 + intH14;
                                    intActH15 = intActH15 + intH15;
                                    intActH16 = intActH16 + intH16;
                                    intActH17 = intActH17 + intH17;
                                    intActH18 = intActH18 + intH18;
                                    intActH19 = intActH19 + intH19;
                                    intActH20 = intActH20 + intH20;
                                    intActH21 = intActH21 + intH21;
                                    intActH22 = intActH22 + intH22;
                                    intActH23 = intActH23 + intH23;
                                    intActH0 = intActH0 + intH0;
                                    intActH1 = intActH1 + intH1;
                                    intActH2 = intActH2 + intH2;
                                    intActH3 = intActH3 + intH3;
                                    intActH4 = intActH4 + intH4;
                                    intActH5 = intActH5 + intH5;
                                    intActH6 = intActH6 + intH6;
                                    intActH7 = intActH7 + intH7;
                                }
                            }
                        }
                    }

                    /*處理廠別Summary資料*/
                    DataRow[] drFirstMTDPlan = dtFirstMTDPlan.Select("(FABID = '" + strFabID + "')");
                    if (drFirstMTDPlan.Length > 0)
                    {
                        intFABFST = intFABFST + Convert.ToInt32(drFirstMTDPlan[0].ItemArray[1]);
                    }

                    /*處理Month To Day&Week To Day的Sum資料*/
                    DataRow[] drFABTTL = dtMonthTTL.Select("(FABID = '" + strFabID + "' )");
                    if (drFABTTL.Length > 0)
                    {
                        for (int h = 0; h < Convert.ToInt32(drFABTTL.Length); h++)
                        {
                            /*算各個廠別的TTL資料 以及TOTAL廠的SUM資料*/
                            intFABTTL = intFABTTL + Convert.ToInt32(drFABTTL[h].ItemArray[2]);
                            intALLTTL = intALLTTL + Convert.ToInt32(drFABTTL[h].ItemArray[2]);

                            /*算各個廠別的MTD資料 以及TOTAL廠的SUM資料*/
                            intFABMTD = intFABMTD + Convert.ToInt32(drFABTTL[h].ItemArray[3]);
                            intALLMTD = intALLMTD + Convert.ToInt32(drFABTTL[h].ItemArray[3]);

                            /*算各個廠別的WTD資料 以及TOTAL廠的SUM資料*/
                            intFABWTD = intFABWTD + Convert.ToInt32(drFABTTL[h].ItemArray[4]);
                            intALLWTD = intALLWTD + Convert.ToInt32(drFABTTL[h].ItemArray[4]);

                            /*算各個廠別的DAY1資料 以及TOTAL廠的SUM資料*/
                            intFABDAY1 = intFABDAY1 + Convert.ToInt32(drFABTTL[h].ItemArray[6]);

                            /*算各個廠別的DAY2資料 以及TOTAL廠的SUM資料*/
                            intFABDAY2 = intFABDAY2 + Convert.ToInt32(drFABTTL[h].ItemArray[7]);

                            /*算各個廠別的DAY3資料 以及TOTAL廠的SUM資料*/
                            intFABDAY3 = intFABDAY3 + Convert.ToInt32(drFABTTL[h].ItemArray[8]);
                        }
                    }

                    for (int kk = 1; kk < 4; kk++)      /*暫時先將 原始計畫 mark掉不顯示*/
                    {
                        drSum = dtSum.NewRow();
                        drSum["FABID"] = strFabID;
                        drSum["PRODINFO"] = "Total";
                        if (kk == 0)
                        {
                            drSum["Category"] = "原始計畫";
                        }
                        if (kk == 1)
                        {
                            drSum["Category"] = "計畫產出";
                        }
                        if (kk == 2)
                        {
                            drSum["Category"] = "實際產出";
                        }
                        if (kk == 3)
                        {
                            drSum["Category"] = "達成率";
                        }
                        drSum["TTL"] = (kk == 0) ? intFABFST.ToString("###,###.##") : ((kk == 1) ? ((intPlanTTL == 0) ? "0" : intPlanTTL.ToString("###,###.##")) : ((kk == 2) ? ((intFABTTL == 0) ? "" : intFABTTL.ToString("###,###.##")) : ""));
                        drSum["MTD"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanMTD == 0) ? "0" : intPlanMTD.ToString("###,###.##")) : ((kk == 2) ? ((intFABMTD == 0) ? "0" : intFABMTD.ToString("###,###.##")) : ""));
                        drSum["WTD"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanWTD == 0) ? "0" : intPlanWTD.ToString("###,###.##")) : ((kk == 2) ? ((intFABWTD == 0) ? "0" : intFABWTD.ToString("###,###.##")) : ""));
                        drSum["T1"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanT1 == 0) ? "0" : intPlanT1.ToString("###,###.##")) : ((kk == 2) ? ((intFABDAY1 == 0) ? "0" : intFABDAY1.ToString("###,###.##")) : ""));
                        drSum["T2"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanT2 == 0) ? "0" : intPlanT2.ToString("###,###.##")) : ((kk == 2) ? ((intFABDAY2 == 0) ? "0" : intFABDAY2.ToString("###,###.##")) : ""));
                        drSum["T3"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanT3 == 0) ? "0" : intPlanT3.ToString("###,###.##")) : ((kk == 2) ? ((intFABDAY3 == 0) ? "0" : intFABDAY3.ToString("###,###.##")) : ""));
                        drSum["H8"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH8 == 0) ? "0" : intPlanH8.ToString("###,###.##")) : ((kk == 2) ? ((intActH8 == 0) ? "0" : intActH8.ToString("###,###.##")) : ""));
                        drSum["H9"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH9 == 0) ? "0" : intPlanH9.ToString("###,###.##")) : ((kk == 2) ? ((intActH9 == 0) ? "0" : intActH9.ToString("###,###.##")) : ""));
                        drSum["H10"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH10 == 0) ? "0" : intPlanH10.ToString("###,###.##")) : ((kk == 2) ? ((intActH10 == 0) ? "0" : intActH10.ToString("###,###.##")) : ""));
                        drSum["H11"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH11 == 0) ? "0" : intPlanH11.ToString("###,###.##")) : ((kk == 2) ? ((intActH11 == 0) ? "0" : intActH11.ToString("###,###.##")) : ""));
                        drSum["H12"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH12 == 0) ? "0" : intPlanH12.ToString("###,###.##")) : ((kk == 2) ? ((intActH12 == 0) ? "0" : intActH12.ToString("###,###.##")) : ""));
                        drSum["H13"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH13 == 0) ? "0" : intPlanH13.ToString("###,###.##")) : ((kk == 2) ? ((intActH13 == 0) ? "0" : intActH13.ToString("###,###.##")) : ""));
                        drSum["H14"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH14 == 0) ? "0" : intPlanH14.ToString("###,###.##")) : ((kk == 2) ? ((intActH14 == 0) ? "0" : intActH14.ToString("###,###.##")) : ""));
                        drSum["H15"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH15 == 0) ? "0" : intPlanH15.ToString("###,###.##")) : ((kk == 2) ? ((intActH15 == 0) ? "0" : intActH15.ToString("###,###.##")) : ""));
                        drSum["H16"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH16 == 0) ? "0" : intPlanH16.ToString("###,###.##")) : ((kk == 2) ? ((intActH16 == 0) ? "0" : intActH16.ToString("###,###.##")) : ""));
                        drSum["H17"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH17 == 0) ? "0" : intPlanH17.ToString("###,###.##")) : ((kk == 2) ? ((intActH17 == 0) ? "0" : intActH17.ToString("###,###.##")) : ""));
                        drSum["H18"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH18 == 0) ? "0" : intPlanH18.ToString("###,###.##")) : ((kk == 2) ? ((intActH18 == 0) ? "0" : intActH18.ToString("###,###.##")) : ""));
                        drSum["H19"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH19 == 0) ? "0" : intPlanH19.ToString("###,###.##")) : ((kk == 2) ? ((intActH19 == 0) ? "0" : intActH19.ToString("###,###.##")) : ""));
                        drSum["H20"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH20 == 0) ? "0" : intPlanH20.ToString("###,###.##")) : ((kk == 2) ? ((intActH20 == 0) ? "0" : intActH20.ToString("###,###.##")) : ""));
                        drSum["H21"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH21 == 0) ? "0" : intPlanH21.ToString("###,###.##")) : ((kk == 2) ? ((intActH21 == 0) ? "0" : intActH21.ToString("###,###.##")) : ""));
                        drSum["H22"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH22 == 0) ? "0" : intPlanH22.ToString("###,###.##")) : ((kk == 2) ? ((intActH22 == 0) ? "0" : intActH22.ToString("###,###.##")) : ""));
                        drSum["H23"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH23 == 0) ? "0" : intPlanH23.ToString("###,###.##")) : ((kk == 2) ? ((intActH23 == 0) ? "0" : intActH23.ToString("###,###.##")) : ""));
                        drSum["H0"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH0 == 0) ? "0" : intPlanH0.ToString("###,###.##")) : ((kk == 2) ? ((intActH0 == 0) ? "0" : intActH0.ToString("###,###.##")) : ""));
                        drSum["H1"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH1 == 0) ? "0" : intPlanH1.ToString("###,###.##")) : ((kk == 2) ? ((intActH1 == 0) ? "0" : intActH1.ToString("###,###.##")) : ""));
                        drSum["H2"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH2 == 0) ? "0" : intPlanH2.ToString("###,###.##")) : ((kk == 2) ? ((intActH2 == 0) ? "0" : intActH2.ToString("###,###.##")) : ""));
                        drSum["H3"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH3 == 0) ? "0" : intPlanH3.ToString("###,###.##")) : ((kk == 2) ? ((intActH3 == 0) ? "0" : intActH3.ToString("###,###.##")) : ""));
                        drSum["H4"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH4 == 0) ? "0" : intPlanH4.ToString("###,###.##")) : ((kk == 2) ? ((intActH4 == 0) ? "0" : intActH4.ToString("###,###.##")) : ""));
                        drSum["H5"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH5 == 0) ? "0" : intPlanH5.ToString("###,###.##")) : ((kk == 2) ? ((intActH5 == 0) ? "0" : intActH5.ToString("###,###.##")) : ""));
                        drSum["H6"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH6 == 0) ? "0" : intPlanH6.ToString("###,###.##")) : ((kk == 2) ? ((intActH6 == 0) ? "0" : intActH6.ToString("###,###.##")) : ""));
                        drSum["H7"] = (kk == 0) ? "" : ((kk == 1) ? ((intPlanH7 == 0) ? "0" : intPlanH7.ToString("###,###.##")) : ((kk == 2) ? ((intActH7 == 0) ? "0" : intActH7.ToString("###,###.##")) : ""));
                        drSum["Sort"] = strSort;
                        drSum["ProductID"] = "";

                        dtSum.Rows.Add(drSum);
                    }

                    intALLFST = intALLFST + intFABFST;

                    intALLPlanTTL = intALLPlanTTL + intPlanTTL;
                    intALLPlanMTD = intALLPlanMTD + intPlanMTD;
                    intALLPlanWTD = intALLPlanWTD + intPlanWTD;
                    intALLPlanT1 = intALLPlanT1 + intPlanT1;
                    intALLPlanT2 = intALLPlanT2 + intPlanT2;
                    intALLPlanT3 = intALLPlanT3 + intPlanT3;
                    intALLPlanH8 = intALLPlanH8 + intPlanH8;
                    intALLPlanH9 = intALLPlanH9 + intPlanH9;
                    intALLPlanH10 = intALLPlanH10 + intPlanH10;
                    intALLPlanH11 = intALLPlanH11 + intPlanH11;
                    intALLPlanH12 = intALLPlanH12 + intPlanH12;
                    intALLPlanH13 = intALLPlanH13 + intPlanH13;
                    intALLPlanH14 = intALLPlanH14 + intPlanH14;
                    intALLPlanH15 = intALLPlanH15 + intPlanH15;
                    intALLPlanH16 = intALLPlanH16 + intPlanH16;
                    intALLPlanH17 = intALLPlanH17 + intPlanH17;
                    intALLPlanH18 = intALLPlanH18 + intPlanH18;
                    intALLPlanH19 = intALLPlanH19 + intPlanH19;
                    intALLPlanH20 = intALLPlanH20 + intPlanH20;
                    intALLPlanH21 = intALLPlanH21 + intPlanH21;
                    intALLPlanH22 = intALLPlanH22 + intPlanH22;
                    intALLPlanH23 = intALLPlanH23 + intPlanH23;
                    intALLPlanH0 = intALLPlanH0 + intPlanH0;
                    intALLPlanH1 = intALLPlanH1 + intPlanH1;
                    intALLPlanH2 = intALLPlanH2 + intPlanH2;
                    intALLPlanH3 = intALLPlanH3 + intPlanH3;
                    intALLPlanH4 = intALLPlanH4 + intPlanH4;
                    intALLPlanH5 = intALLPlanH5 + intPlanH5;
                    intALLPlanH6 = intALLPlanH6 + intPlanH6;
                    intALLPlanH7 = intALLPlanH7 + intPlanH7;

                    intALLActTTL = intALLActTTL + intActTTL;
                    intALLActMTD = intALLActMTD + intActMTD;
                    intALLActWTD = intALLActWTD + intActWTD;
                    intALLActT1 = intALLActT1 + intFABDAY1;
                    intALLActT2 = intALLActT2 + intFABDAY2;
                    intALLActT3 = intALLActT3 + intFABDAY3;
                    intALLActH8 = intALLActH8 + intActH8;
                    intALLActH9 = intALLActH9 + intActH9;
                    intALLActH10 = intALLActH10 + intActH10;
                    intALLActH11 = intALLActH11 + intActH11;
                    intALLActH12 = intALLActH12 + intActH12;
                    intALLActH13 = intALLActH13 + intActH13;
                    intALLActH14 = intALLActH14 + intActH14;
                    intALLActH15 = intALLActH15 + intActH15;
                    intALLActH16 = intALLActH16 + intActH16;
                    intALLActH17 = intALLActH17 + intActH17;
                    intALLActH18 = intALLActH18 + intActH18;
                    intALLActH19 = intALLActH19 + intActH19;
                    intALLActH20 = intALLActH20 + intActH20;
                    intALLActH21 = intALLActH21 + intActH21;
                    intALLActH22 = intALLActH22 + intActH22;
                    intALLActH23 = intALLActH23 + intActH23;
                    intALLActH0 = intALLActH0 + intActH0;
                    intALLActH1 = intALLActH1 + intActH1;
                    intALLActH2 = intALLActH2 + intActH2;
                    intALLActH3 = intALLActH3 + intActH3;
                    intALLActH4 = intALLActH4 + intActH4;
                    intALLActH5 = intALLActH5 + intActH5;
                    intALLActH6 = intALLActH6 + intActH6;
                    intALLActH7 = intALLActH7 + intActH7;
                }
                /*處理Total廠Summary資料*/
                for (int kk = 1; kk < 4; kk++)      /*暫時先將 原始計畫 mark掉不顯示*/
                {
                    drSum = dtSum.NewRow();
                    //drSum["FABID"] = "LCM1+LCM2+VAP";
                    drSum["FABID"] = "SH";
                    drSum["PRODINFO"] = "Total";
                    if (kk == 0)
                    {
                        drSum["Category"] = "原始計畫";
                    }
                    if (kk == 1)
                    {
                        drSum["Category"] = "計畫產出";
                    }
                    if (kk == 2)
                    {
                        drSum["Category"] = "實際產出";
                    }
                    if (kk == 3)
                    {
                        drSum["Category"] = "達成率";
                    }
                    drSum["TTL"] = (kk == 0) ? intALLFST.ToString("###,###.##") : ((kk == 1) ? ((intALLPlanTTL == 0) ? "0" : intALLPlanTTL.ToString("###,###.##")) : ((kk == 2) ? ((intALLTTL == 0) ? "" : intALLTTL.ToString("###,###.##")) : ""));
                    drSum["MTD"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanMTD == 0) ? "0" : intALLPlanMTD.ToString("###,###.##")) : ((kk == 2) ? ((intALLMTD == 0) ? "0" : intALLMTD.ToString("###,###.##")) : ""));
                    drSum["WTD"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanWTD == 0) ? "0" : intALLPlanWTD.ToString("###,###.##")) : ((kk == 2) ? ((intALLWTD == 0) ? "0" : intALLWTD.ToString("###,###.##")) : ""));
                    drSum["T1"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanT1 == 0) ? "0" : intALLPlanT1.ToString("###,###.##")) : ((kk == 2) ? ((intALLActT1 == 0) ? "0" : intALLActT1.ToString("###,###.##")) : ""));
                    drSum["T2"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanT2 == 0) ? "0" : intALLPlanT2.ToString("###,###.##")) : ((kk == 2) ? ((intALLActT2 == 0) ? "0" : intALLActT2.ToString("###,###.##")) : ""));
                    drSum["T3"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanT3 == 0) ? "0" : intALLPlanT3.ToString("###,###.##")) : ((kk == 2) ? ((intALLActT3 == 0) ? "0" : intALLActT3.ToString("###,###.##")) : ""));
                    drSum["H8"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH8 == 0) ? "0" : intALLPlanH8.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH8 == 0) ? "0" : intALLActH8.ToString("###,###.##")) : ""));
                    drSum["H9"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH9 == 0) ? "0" : intALLPlanH9.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH9 == 0) ? "0" : intALLActH9.ToString("###,###.##")) : ""));
                    drSum["H10"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH10 == 0) ? "0" : intALLPlanH10.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH10 == 0) ? "0" : intALLActH10.ToString("###,###.##")) : ""));
                    drSum["H11"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH11 == 0) ? "0" : intALLPlanH11.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH11 == 0) ? "0" : intALLActH11.ToString("###,###.##")) : ""));
                    drSum["H12"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH12 == 0) ? "0" : intALLPlanH12.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH12 == 0) ? "0" : intALLActH12.ToString("###,###.##")) : ""));
                    drSum["H13"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH13 == 0) ? "0" : intALLPlanH13.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH13 == 0) ? "0" : intALLActH13.ToString("###,###.##")) : ""));
                    drSum["H14"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH14 == 0) ? "0" : intALLPlanH14.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH14 == 0) ? "0" : intALLActH14.ToString("###,###.##")) : ""));
                    drSum["H15"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH15 == 0) ? "0" : intALLPlanH15.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH15 == 0) ? "0" : intALLActH15.ToString("###,###.##")) : ""));
                    drSum["H16"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH16 == 0) ? "0" : intALLPlanH16.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH16 == 0) ? "0" : intALLActH16.ToString("###,###.##")) : ""));
                    drSum["H17"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH17 == 0) ? "0" : intALLPlanH17.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH17 == 0) ? "0" : intALLActH17.ToString("###,###.##")) : ""));
                    drSum["H18"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH18 == 0) ? "0" : intALLPlanH18.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH18 == 0) ? "0" : intALLActH18.ToString("###,###.##")) : ""));
                    drSum["H19"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH19 == 0) ? "0" : intALLPlanH19.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH19 == 0) ? "0" : intALLActH19.ToString("###,###.##")) : ""));
                    drSum["H20"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH20 == 0) ? "0" : intALLPlanH20.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH20 == 0) ? "0" : intALLActH20.ToString("###,###.##")) : ""));
                    drSum["H21"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH21 == 0) ? "0" : intALLPlanH21.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH21 == 0) ? "0" : intALLActH21.ToString("###,###.##")) : ""));
                    drSum["H22"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH22 == 0) ? "0" : intALLPlanH22.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH22 == 0) ? "0" : intALLActH22.ToString("###,###.##")) : ""));
                    drSum["H23"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH23 == 0) ? "0" : intALLPlanH23.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH23 == 0) ? "0" : intALLActH23.ToString("###,###.##")) : ""));
                    drSum["H0"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH0 == 0) ? "0" : intALLPlanH0.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH0 == 0) ? "0" : intALLActH0.ToString("###,###.##")) : ""));
                    drSum["H1"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH1 == 0) ? "0" : intALLPlanH1.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH1 == 0) ? "0" : intALLActH1.ToString("###,###.##")) : ""));
                    drSum["H2"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH2 == 0) ? "0" : intALLPlanH2.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH2 == 0) ? "0" : intALLActH2.ToString("###,###.##")) : ""));
                    drSum["H3"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH3 == 0) ? "0" : intALLPlanH3.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH3 == 0) ? "0" : intALLActH3.ToString("###,###.##")) : ""));
                    drSum["H4"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH4 == 0) ? "0" : intALLPlanH4.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH4 == 0) ? "0" : intALLActH4.ToString("###,###.##")) : ""));
                    drSum["H5"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH5 == 0) ? "0" : intALLPlanH5.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH5 == 0) ? "0" : intALLActH5.ToString("###,###.##")) : ""));
                    drSum["H6"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH6 == 0) ? "0" : intALLPlanH6.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH6 == 0) ? "0" : intALLActH6.ToString("###,###.##")) : ""));
                    drSum["H7"] = (kk == 0) ? "" : ((kk == 1) ? ((intALLPlanH7 == 0) ? "0" : intALLPlanH7.ToString("###,###.##")) : ((kk == 2) ? ((intALLActH7 == 0) ? "0" : intALLActH7.ToString("###,###.##")) : ""));
                    drSum["Sort"] = "0";
                    drSum["ProductID"] = "";

                    dtSum.Rows.Add(drSum);
                }

                DataView dvSum = new DataView();
                dvSum = dtSum.DefaultView;
                dvSum.Sort = "Sort,FABID,ProdInfo";

                GridView1.HeaderStyle.BackColor = System.Drawing.Color.LemonChiffon;
                GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                GridView1.DataSource = dvSum;
                GridView1.DataBind();

                GridView1.HeaderRow.Cells[6].Text = strDay1;
                GridView1.HeaderRow.Cells[7].Text = strDay2;
                GridView1.HeaderRow.Cells[8].Text = strDay3;
                for (int i = 0; i < GridView1.Rows.Count - 1; i++)
                {
                    if (GridView1.Rows[i].Cells[2].Text.Trim() == "實際產出" && GridView1.Rows[i].Cells[1].Text.Trim() != "Total")
                    {
                        string strTmp = "";
                        strTmp = GridView1.Rows[i - 1].Cells[1].Text.ToString();
                        GridView1.Rows[i - 1].Cells[1].Text = strTmp + "<br>" + GridView1.Rows[i - 1].Cells[33].Text.ToString();
                    }
                }

                Label2.Text = "資料筆數:" + GridView1.Rows.Count.ToString();
            }
            else
            {
                Label2.Text = "查無資料";
            }
        }
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;

                GridViewRow rowHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

                rowHeader.BackColor = System.Drawing.Color.LemonChiffon;
                rowHeader.Font.Bold = true;
                rowHeader.ForeColor = System.Drawing.Color.Black;

                TableCellCollection cells = e.Row.Cells;
                TableCell headerCell = new TableCell();
                headerCell.Text = "廠別";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.RowSpan = 2;
                headerCell.Font.Name = "Airl";
                //headerCell.ColumnSpan = 2;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "線別" + "<br>" + "產品型號";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.RowSpan = 2;
                headerCell.Wrap = false;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "Category";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.RowSpan = 2;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                headerCell.Wrap = false;
                rowHeader.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = strYear + "M" + strMonth + "<br>" + "(Month)";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.Wrap = false;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells.Add(headerCell);
                headerCell.ColumnSpan = 2;

                headerCell = new TableCell();
                headerCell.Text = strYear + "W" + strWeekNum + "<br>" + "(Week)";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.Wrap = false;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "3 Days";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.ColumnSpan = 3;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "Hourly";
                headerCell.BackColor = System.Drawing.Color.LemonChiffon;
                headerCell.BorderStyle = BorderStyle.Solid;
                headerCell.BorderWidth = 1;
                headerCell.ColumnSpan = 24;
                headerCell.HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells.Add(headerCell);

                rowHeader.RowType = DataControlRowType.Header;
                rowHeader.Visible = true;
                GridView1.Controls[0].Controls.AddAt(0, rowHeader);
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string strDate= "";
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            e.Row.Cells[33].Visible = false;
            for (int intmg = 0; intmg < 2; intmg++)
            {
                for (int i = 0; i < GridView1.Rows.Count - 1; i++)		//DataGridㄖ虫じ篶

                {
                    int colnum = 1;
                    int j;
                    for (j = i + 1; j < GridView1.Rows.Count; j++)
                    {
                        if (GridView1.Rows[i].Cells[1].Text != "Total" && GridView1.Rows[j].Cells[1].Text != "Total")
                        {
                            if (intmg == 1)
                            {
                                if (GridView1.Rows[i].Cells[intmg].Text == GridView1.Rows[j].Cells[intmg].Text && GridView1.Rows[i].Cells[33].Text == GridView1.Rows[j].Cells[33].Text)
                                {
                                    colnum++;
                                    GridView1.Rows[i].Cells[intmg].RowSpan = colnum;
                                    GridView1.Rows[j].Cells[intmg].Visible = false;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (GridView1.Rows[i].Cells[intmg].Text == GridView1.Rows[j].Cells[intmg].Text)
                                {
                                    colnum++;
                                    GridView1.Rows[i].Cells[intmg].RowSpan = colnum;
                                    GridView1.Rows[j].Cells[intmg].Visible = false;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (GridView1.Rows[i].Cells[0].Text == GridView1.Rows[j].Cells[0].Text && GridView1.Rows[i].Cells[1].Text == GridView1.Rows[j].Cells[1].Text)
                            {
                                colnum++;
                                GridView1.Rows[i].Cells[intmg].RowSpan = colnum;
                                GridView1.Rows[j].Cells[intmg].Visible = false;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    i = j - 1;
                }
            }
            for (int i = 0; i < GridView1.Rows.Count; i++)		//DataGridㄖ虫じ篶

            {
                if (GridView1.Rows[i].Cells[1].Text != "Total")
                {
                    GridView1.Rows[i].Cells[0].BackColor = System.Drawing.Color.LightBlue;
                    GridView1.Rows[i].Cells[1].BackColor = System.Drawing.Color.LightBlue;
                    GridView1.Rows[i].Cells[2].BackColor = System.Drawing.Color.LightBlue;
                }
                else
                {
                    GridView1.Rows[i].Cells[0].BackColor = System.Drawing.Color.BurlyWood;
                    GridView1.Rows[i].Cells[1].BackColor = System.Drawing.Color.BurlyWood;
                    GridView1.Rows[i].Cells[2].BackColor = System.Drawing.Color.BurlyWood;
                }
                if (GridView1.Rows[i].Cells[2].Text.Trim() == "備註說明")
                {
                    int Column = 1;
                    for (int j = 9; j < 33; j++)
                    {
                        if (GridView1.Rows[i - 3].Cells[j].Text != "0")
                        {
                            for (int s = 1; s < j - 9; s++)
                            {
                                if (GridView1.Rows[i - 3].Cells[j - s].Text == "0")
                                {
                                    Column++;
                                    GridView1.Rows[i - 3].Cells[j - s].ColumnSpan = Column;
                                    GridView1.Rows[i - 3].Cells[j - s + 1].Visible = false;
                                    GridView1.Rows[i - 3].Cells[j - s].Text = Convert.ToString(Convert.ToInt32(GridView1.Rows[i - 3].Cells[j - s].Text.Replace(",", "")) + Convert.ToInt32(GridView1.Rows[i - 3].Cells[j - s + 1].Text.Replace(",", "")));

                                    GridView1.Rows[i - 2].Cells[j - s].ColumnSpan = Column;
                                    GridView1.Rows[i - 2].Cells[j - s + 1].Visible = false;
                                    GridView1.Rows[i - 2].Cells[j - s].Text = Convert.ToString(Convert.ToInt32(GridView1.Rows[i - 2].Cells[j - s].Text.Replace(",", "")) + Convert.ToInt32(GridView1.Rows[i - 2].Cells[j - s + 1].Text.Replace(",", "")));

                                    GridView1.Rows[i - 1].Cells[j - s].ColumnSpan = Column;
                                    GridView1.Rows[i - 1].Cells[j - s + 1].Visible = false;

                                    GridView1.Rows[i].Cells[j - s].ColumnSpan = Column;
                                    GridView1.Rows[i].Cells[j - s + 1].Visible = false;

                                }
                                else
                                {
                                    break;
                                }
                            }
                            Column = 1;
                        }
                    }

                    if (dtReMark.Rows.Count > 0)
                    {
                        DataRow[] drQuery = dtReMark.Select("(FABID = '" + GridView1.Rows[i].Cells[0].Text + "' AND PRODLINE= '" + GridView1.Rows[i].Cells[1].Text + "' )", "FABID");
                        if (drQuery.Length > 0)
                        {
                            for (int mk = 4; mk < 33; mk++)
                            {
                                GridView1.Rows[i].Cells[3].Text = "";
                                GridView1.Rows[i].Cells[mk].ToolTip = drQuery[0].ItemArray[mk + 3].ToString();
                                GridView1.Rows[i].Cells[mk].Text = (drQuery[0].ItemArray[mk + 3].ToString().Trim() == "") ? "" : "*";
                            }
                        }
                    }
                    if (txtMUGMM.Text == "T")
                    {
                        for (int row = 4; row <= 32; row++)
                        {
                            strDate = (DDLMFGDate.SelectedValue.ToString() == "MFGDATE") ? DDLMFGDate.Items[1].Value.ToString() : DDLMFGDate.SelectedValue.ToString();
                            GridView1.Rows[i].Cells[row].Attributes.Add("ondblclick", "window.open('../PC/ActualOutputMark.aspx?FABID=" + GridView1.Rows[i].Cells[0].Text + "&MFGDate=" + strDate + "&PRODLINE=" + GridView1.Rows[i].Cells[1].Text + "&PRODUCTID=" + GridView1.Rows[i].Cells[33].Text + "&ColID=" + row.ToString() + " ','備註說明','toolbar=no,directories=no,scrollbars=no,status=no,menubar=no,resizable=no,width=160,height=180')");
                        }
                    }
                }
                if (GridView1.Rows[i].Cells[2].Text == "達成率")
                {
                    double dblPlanQty = 0.00, dblActQty = 0.00;
                    for (int j = 3; j <= 32; j++)
                    {
                        dblPlanQty = 0.00;
                        dblActQty = 0.00;

                        if (GridView1.Rows[i - 2].Cells[j].Text.Trim().Replace("&nbsp;","").ToString() != "")
                        {
                            dblPlanQty = Convert.ToDouble(GridView1.Rows[i - 2].Cells[j].Text);
                        }
                        if (GridView1.Rows[i - 1].Cells[j].Text.Trim().Replace("&nbsp;", "").ToString() != "")
                        {
                            dblActQty = Convert.ToDouble(GridView1.Rows[i - 1].Cells[j].Text);
                        }
                        if (dblPlanQty == 0 && dblActQty == 0)
                        {
                            GridView1.Rows[i].Cells[j].Text = "";
                        }
                        else
                        {
                            if (dblPlanQty == 0 && dblActQty != 0)
                            {
                                GridView1.Rows[i].Cells[j].Text = "100%";
                            }
                            else if (dblPlanQty != 0 && dblActQty == 0)
                            {
                                GridView1.Rows[i].Cells[j].Text = "0%";
                                GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Red;
                                GridView1.Rows[i].Cells[j].Font.Bold = true;
                            }
                            else
                            {
                                GridView1.Rows[i].Cells[j].Text = Convert.ToString(Math.Round(dblActQty / dblPlanQty, 4) * 100) + "%";
                                if (dblActQty / dblPlanQty < 1)
                                {
                                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Red;
                                    GridView1.Rows[i].Cells[j].Font.Bold = true;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void SetMFGDate()
        {
            this.DDLMFGDate.Items.Clear();
            string strSql = @" SELECT 'MFGDATE' AS MFGDATE ";
            for (int i = 0; i < 120; i++)
            {
                strSql = strSql + @" UNION ALL SELECT convert(Varchar(10),Getdate() - '08:00:00' - " + i + ",120) AS MFGDATE ";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.DDLMFGDate.DataSource = dt;
            this.DDLMFGDate.DataTextField = "MFGDATE";
            this.DDLMFGDate.DataValueField = "MFGDATE";
            this.DDLMFGDate.DataBind();
            this.DDLMFGDate.ClearSelection();
        }
        private void SetProdLineLine()
        {
            this.DDLProdLine.Items.Clear();
            string strSql = @" SELECT 'PRODLINE' AS SUBLINEID,'0' AS ROWID UNION ALL ";
                    strSql = strSql + @" SELECT SUBLINEID, '1' AS ROWID FROM [REP].[MSLM] WHERE WORKCTR = '8380' ";
                    strSql = strSql + @" ORDER BY ROWID,SUBLINEID ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.DDLProdLine.DataSource = dt;
            this.DDLProdLine.DataTextField = "SUBLINEID";
            this.DDLProdLine.DataValueField = "SUBLINEID";
            this.DDLProdLine.DataBind();
            this.DDLProdLine.ClearSelection();
        }
        private void SetApplication()
        {
            this.DDLApplication.Items.Clear();
            string strSql = @" SELECT 'APPLICATION' AS APPLICATION,'0' AS ROWID UNION ALL SELECT APPLICATION, '1' AS ROWID FROM(";
            strSql = strSql + @" SELECT DISTINCT PRODGROUP AS APPLICATION FROM [REP].[MPIM] ";
            strSql = strSql + @" )A ORDER BY ROWID,APPLICATION ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.DDLApplication.DataSource = dt;
            this.DDLApplication.DataTextField = "APPLICATION";
            this.DDLApplication.DataValueField = "APPLICATION";
            this.DDLApplication.DataBind();
            this.DDLApplication.ClearSelection();
        }
        private void SetCustomer()
        {
            this.DDLCustomer.Items.Clear();
            string strSql = @" SELECT'CUSTOMER' AS CUSTOMER, '0' AS ROWID UNION ALL SELECT DISTINCT CUSTOMER, '1' AS ROWID FROM MPDCSTM ";
            strSql = strSql + @" ORDER BY ROWID,CUSTOMER ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.DDLCustomer.DataSource = dt;
            this.DDLCustomer.DataTextField = "CUSTOMER";
            this.DDLCustomer.DataValueField = "CUSTOMER";
            this.DDLCustomer.DataBind();
            this.DDLCustomer.ClearSelection();
            DDLCustomer.SelectedValue = "N90";
        }
        private void SetUserInfo()
        {
            string strSql = @" select Userid from MUGMM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' and userid='" + ((UserInfo)Session["UserInfo"]).UserID + "' and usergroupcd='RPMF'   ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtMUGMM.Text = "T";
            }
            else
            {
                txtMUGMM.Text = "F";
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            BoundData();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.GridView1.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
