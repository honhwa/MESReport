using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;

public partial class Material_ScrapRateChart : System.Web.UI.Page
{
    const string strReportName = "單產品月趨勢圖";
    const string CstrPEPlan = "PEPlan";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strPlant = "", strProjectName = "", strSIZEID = "", strPRODGROUP = "", strMFGDT = "", strWoType = "", strProdID = "";
            string strFABID = "";
            if (Request.QueryString["FABID"] != null) { strFABID = this.Request.QueryString["FABID"].ToString(); }
            if (Request.QueryString["Plant"] != null) { strPlant = this.Request.QueryString["Plant"].ToString(); }
            if (Request.QueryString["ProjectName"] != null) { strProjectName = this.Request.QueryString["ProjectName"].ToString(); }
            if (Request.QueryString["MFGDT"] != null) { strMFGDT = this.Request.QueryString["MFGDT"].ToString(); }
            if (Request.QueryString["WoType"] != null) { strWoType = this.Request.QueryString["WoType"].ToString(); }
            if (Request.QueryString["ProdID"] != null) { strProdID = this.Request.QueryString["ProdID"].ToString(); }

            QueryData(strFABID, strPlant, strPRODGROUP, strSIZEID, strProjectName, strMFGDT, strWoType, strProdID);
        }
    }
    protected void QueryData(string strFABID, string strPlant, string strPRODGROUP, string strSIZEID, string strProjectName, string strMFGDT, string strWoType, string strProdID)
    {
        string strTimeDiff = "";
        ModuleClass mc = new ModuleClass();
        DateTime dtBegin = DateTime.Now;

        string strFabID = mc.GetFABID(strPlant);
        DBConn dbConn = new DBConn();
        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);

        string strBDate_M5 = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 5).ToString("yyyy-MM-dd");
        string strBDate_M4 = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 4).ToString("yyyy-MM-dd");
        string strBDate_M3 = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 3).ToString("yyyy-MM-dd");
        string strBDate_M2 = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 2).ToString("yyyy-MM-dd");
        string strBDate_M1 = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 1).ToString("yyyy-MM-dd");
        string strBDate_M0 = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 0).ToString("yyyy-MM-dd");

        string strEDate_M5 = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 4).ToString("yyyy-MM-dd");
        string strEDate_M4 = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 3).ToString("yyyy-MM-dd");
        string strEDate_M3 = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 2).ToString("yyyy-MM-dd");
        string strEDate_M2 = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 1).ToString("yyyy-MM-dd");
        string strEDate_M1 = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 0).ToString("yyyy-MM-dd");
        string strEDate_M0 = dtDate.ToString("yyyy-MM-dd");

        string strBDate_W4 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 4 * 7 + 1).ToString("yyyy-MM-dd");
        string strBDate_W3 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 3 * 7 + 1).ToString("yyyy-MM-dd");
        string strBDate_W2 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 2 * 7 + 1).ToString("yyyy-MM-dd");
        string strBDate_W1 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 1 * 7 + 1).ToString("yyyy-MM-dd");

        string strEDate_W4 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 3 * 7).ToString("yyyy-MM-dd");
        string strEDate_W3 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 2 * 7).ToString("yyyy-MM-dd");
        string strEDate_W2 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 1 * 7).ToString("yyyy-MM-dd");
        string strEDate_W1 = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 0 * 7).ToString("yyyy-MM-dd");

        string strDate_D7 = dtDate.AddDays(-6).ToString("yyyy-MM-dd");
        string strDate_D6 = dtDate.AddDays(-5).ToString("yyyy-MM-dd");
        string strDate_D5 = dtDate.AddDays(-4).ToString("yyyy-MM-dd");
        string strDate_D4 = dtDate.AddDays(-3).ToString("yyyy-MM-dd");
        string strDate_D3 = dtDate.AddDays(-2).ToString("yyyy-MM-dd");
        string strDate_D2 = dtDate.AddDays(-1).ToString("yyyy-MM-dd");
        string strDate_D1 = dtDate.AddDays(-0).ToString("yyyy-MM-dd");

        string strSql = "";

        strSql = strSql + " SELECT T1.REPORTDATE,T1.REPORTYEAR,T1.REPORTMONTH,CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID,T1.SHOPCD AS PLANTID,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,T1.COMP_PART,T1.COMP_M_GROUP";
        strSql = strSql + " ,SUM(T1.OutPutQTY) AS OUT_QTY";
        strSql = strSql + " ,SUM(T1.ScrapQTY) AS SCRAP_QTY";
        strSql = strSql + " INTO #EIS_SUM FROM dbo.RPT_EIS AS T1 WITH(NOLOCK)";

        strSql = strSql + " WHERE T1.REPORTDATE BETWEEN '" + strBDate_M5 + "' AND '" + strEDate_M0 + "'";
        if (strFABID == "WE01") { strFABID = "B"; } else { strFABID = ""; };
        strSql = strSql + " AND T1.FABID = '" + strFABID + "'";
        strSql = strSql + " AND T1.SHOPCD = '" + strPlant + "'";
        strSql = strSql + " AND T1.PROJECT_NAME = '" + strProjectName + "'";
        #region parameters [選擇條件過濾]
        if (strWoType != "")
        {
            string[] str2WorkType = strWoType.Split(',');
            string strSqlWortType = "";
            for (int i = 0; i < str2WorkType.Length; i++)
            {
                if (strSqlWortType == "")
                {
                    strSqlWortType = strSqlWortType + " AND T1.WOTYPE IN('" + str2WorkType[i].ToString() + "'";
                }
                else
                {
                    strSqlWortType = strSqlWortType + " , '" + str2WorkType[i].ToString() + "'";
                }
            }
            if (strSqlWortType != "")
            {
                strSql = strSql + strSqlWortType + " )";
            }
        }
        if (strProdID != "")
        {
            string[] str2Product = strProdID.Split(',');
            string strSqlProdID = "";
            for (int i = 0; i < str2Product.Length; i++)
            {
                if (strSqlProdID == "")
                {
                    strSqlProdID = strSqlProdID + " AND T1.PRODID IN('" + str2Product[i].ToString() + "'";
                }
                else
                {
                    strSqlProdID = strSqlProdID + " ,'" + str2Product[i].ToString() + "'";
                }
            }
            if (strSqlProdID != "")
            {
                strSql = strSql + strSqlProdID + " )";
            }
        }
        #endregion
        strSql = strSql + " GROUP BY T1.REPORTDATE";
        strSql = strSql + " ,T1.REPORTYEAR";
        strSql = strSql + " ,T1.REPORTMONTH";
        strSql = strSql + " ,CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END";
        strSql = strSql + " ,T1.SHOPCD";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.PRODID";
        strSql = strSql + " ,T1.TP_PART_NUM";
        strSql = strSql + " ,T1.COMP_M_GROUP";
        strSql = strSql + " ,T1.COMP_PART;";

        #region parameters
        strSql = strSql + " SELECT FABID,PLANTID,PROJECT_NAME,COMP_M_GROUP";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M5 + "' AND '" + strEDate_M5 + "' THEN OUT_QTY ELSE 0 END) AS M5OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M5 + "' AND '" + strEDate_M5 + "' THEN SCRAP_QTY ELSE 0 END) AS M5ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M4 + "' AND '" + strEDate_M4 + "' THEN OUT_QTY ELSE 0 END) AS M4OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M4 + "' AND '" + strEDate_M4 + "' THEN SCRAP_QTY ELSE 0 END) AS M4ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M3 + "' AND '" + strEDate_M3 + "' THEN OUT_QTY ELSE 0 END) AS M3OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M3 + "' AND '" + strEDate_M3 + "' THEN SCRAP_QTY ELSE 0 END) AS M3ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M2 + "' AND '" + strEDate_M2 + "' THEN OUT_QTY ELSE 0 END) AS M2OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M2 + "' AND '" + strEDate_M2 + "' THEN SCRAP_QTY ELSE 0 END) AS M2ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M1 + "' AND '" + strEDate_M1 + "' THEN OUT_QTY ELSE 0 END) AS M1OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M1 + "' AND '" + strEDate_M1 + "' THEN SCRAP_QTY ELSE 0 END) AS M1ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M0 + "' AND '" + strEDate_M0 + "' THEN OUT_QTY ELSE 0 END) AS M0OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M0 + "' AND '" + strEDate_M0 + "' THEN SCRAP_QTY ELSE 0 END) AS M0ScrapQty";

        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W4 + "' AND '" + strEDate_W4 + "' THEN OUT_QTY ELSE 0 END) AS W4OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W4 + "' AND '" + strEDate_W4 + "' THEN SCRAP_QTY ELSE 0 END) AS W4ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W3 + "' AND '" + strEDate_W3 + "' THEN OUT_QTY ELSE 0 END) AS W3OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W3 + "' AND '" + strEDate_W3 + "' THEN SCRAP_QTY ELSE 0 END) AS W3ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W2 + "' AND '" + strEDate_W2 + "' THEN OUT_QTY ELSE 0 END) AS W2OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W2 + "' AND '" + strEDate_W2 + "' THEN SCRAP_QTY ELSE 0 END) AS W2ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W1 + "' AND '" + strEDate_W1 + "' THEN OUT_QTY ELSE 0 END) AS W1OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W1 + "' AND '" + strEDate_W1 + "' THEN SCRAP_QTY ELSE 0 END) AS W1ScrapQty";

        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D7 + "' THEN OUT_QTY ELSE 0 END) AS D7OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D7 + "' THEN SCRAP_QTY ELSE 0 END) AS D7ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D6 + "' THEN OUT_QTY ELSE 0 END) AS D6OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D6 + "' THEN SCRAP_QTY ELSE 0 END) AS D6ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D5 + "' THEN OUT_QTY ELSE 0 END) AS D5OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D5 + "' THEN SCRAP_QTY ELSE 0 END) AS D5ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D4 + "' THEN OUT_QTY ELSE 0 END) AS D4OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D4 + "' THEN SCRAP_QTY ELSE 0 END) AS D4ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D3 + "' THEN OUT_QTY ELSE 0 END) AS D3OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D3 + "' THEN SCRAP_QTY ELSE 0 END) AS D3ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D2 + "' THEN OUT_QTY ELSE 0 END) AS D2OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D2 + "' THEN SCRAP_QTY ELSE 0 END) AS D2ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D1 + "' THEN OUT_QTY ELSE 0 END) AS D1OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D1 + "' THEN SCRAP_QTY ELSE 0 END) AS D1ScrapQty";

        strSql = strSql + " INTO #QtyProjectName";
        strSql = strSql + " FROM #EIS_SUM";
        strSql = strSql + " GROUP BY FABID,PLANTID,PROJECT_NAME,COMP_M_GROUP;";

        strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PROJECT_NAME,A.COMP_M_GROUP";
        strSql = strSql + " ,CASE WHEN SUM(A.M5OutQty) = 0 THEN CASE WHEN SUM(A.M5ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M5ScrapQty + A.M5OutQty) / SUM(A.M5OutQty)  END AS M5";
        strSql = strSql + " ,CASE WHEN SUM(A.M4OutQty) = 0 THEN CASE WHEN SUM(A.M4ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M4ScrapQty + A.M4OutQty) / SUM(A.M4OutQty)  END AS M4";
        strSql = strSql + " ,CASE WHEN SUM(A.M3OutQty) = 0 THEN CASE WHEN SUM(A.M3ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M3ScrapQty + A.M3OutQty) / SUM(A.M3OutQty)  END AS M3";
        strSql = strSql + " ,CASE WHEN SUM(A.M2OutQty) = 0 THEN CASE WHEN SUM(A.M2ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M2ScrapQty + A.M2OutQty) / SUM(A.M2OutQty)  END AS M2";
        strSql = strSql + " ,CASE WHEN SUM(A.M1OutQty) = 0 THEN CASE WHEN SUM(A.M1ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M1ScrapQty + A.M1OutQty) / SUM(A.M1OutQty)  END AS M1";
        strSql = strSql + " ,CASE WHEN SUM(A.M0OutQty) = 0 THEN CASE WHEN SUM(A.M0ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M0ScrapQty + A.M0OutQty) / SUM(A.M0OutQty)  END AS M0";
        strSql = strSql + " ,CASE WHEN SUM(A.W4OutQty) = 0 THEN CASE WHEN SUM(A.W4ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W4ScrapQty + A.W4OutQty) / SUM(A.W4OutQty)  END AS W4";
        strSql = strSql + " ,CASE WHEN SUM(A.W3OutQty) = 0 THEN CASE WHEN SUM(A.W3ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W3ScrapQty + A.W3OutQty) / SUM(A.W3OutQty)  END AS W3";
        strSql = strSql + " ,CASE WHEN SUM(A.W2OutQty) = 0 THEN CASE WHEN SUM(A.W2ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W2ScrapQty + A.W2OutQty) / SUM(A.W2OutQty)  END AS W2";
        strSql = strSql + " ,CASE WHEN SUM(A.W1OutQty) = 0 THEN CASE WHEN SUM(A.W1ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W1ScrapQty + A.W1OutQty) / SUM(A.W1OutQty)  END AS W1";
        strSql = strSql + " ,CASE WHEN SUM(A.D7OutQty) = 0 THEN CASE WHEN SUM(A.D7ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D7ScrapQty + A.D7OutQty) / SUM(A.D7OutQty)  END AS D7";
        strSql = strSql + " ,CASE WHEN SUM(A.D6OutQty) = 0 THEN CASE WHEN SUM(A.D6ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D6ScrapQty + A.D6OutQty) / SUM(A.D6OutQty)  END AS D6";
        strSql = strSql + " ,CASE WHEN SUM(A.D5OutQty) = 0 THEN CASE WHEN SUM(A.D5ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D5ScrapQty + A.D5OutQty) / SUM(A.D5OutQty)  END AS D5";
        strSql = strSql + " ,CASE WHEN SUM(A.D4OutQty) = 0 THEN CASE WHEN SUM(A.D4ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D4ScrapQty + A.D4OutQty) / SUM(A.D4OutQty)  END AS D4";
        strSql = strSql + " ,CASE WHEN SUM(A.D3OutQty) = 0 THEN CASE WHEN SUM(A.D3ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D3ScrapQty + A.D3OutQty) / SUM(A.D3OutQty)  END AS D3";
        strSql = strSql + " ,CASE WHEN SUM(A.D2OutQty) = 0 THEN CASE WHEN SUM(A.D2ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D2ScrapQty + A.D2OutQty) / SUM(A.D2OutQty)  END AS D2";
        strSql = strSql + " ,CASE WHEN SUM(A.D1OutQty) = 0 THEN CASE WHEN SUM(A.D1ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D1ScrapQty + A.D1OutQty) / SUM(A.D1OutQty)  END AS D1";
        strSql = strSql + " INTO #DATEProjectName";
        strSql = strSql + " FROM #QtyProjectName A";
        strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PROJECT_NAME,A.COMP_M_GROUP;";


        strSql = strSql + " SELECT FABID,PLANTID,PRODID+ ' ' +TP_PART_NUM as ProdID,COMP_M_GROUP+ ' ' +COMP_PART as PartID";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M5 + "' AND '" + strEDate_M5 + "' THEN OUT_QTY ELSE 0 END) AS M5OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M5 + "' AND '" + strEDate_M5 + "' THEN SCRAP_QTY ELSE 0 END) AS M5ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M4 + "' AND '" + strEDate_M4 + "' THEN OUT_QTY ELSE 0 END) AS M4OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M4 + "' AND '" + strEDate_M4 + "' THEN SCRAP_QTY ELSE 0 END) AS M4ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M3 + "' AND '" + strEDate_M3 + "' THEN OUT_QTY ELSE 0 END) AS M3OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M3 + "' AND '" + strEDate_M3 + "' THEN SCRAP_QTY ELSE 0 END) AS M3ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M2 + "' AND '" + strEDate_M2 + "' THEN OUT_QTY ELSE 0 END) AS M2OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M2 + "' AND '" + strEDate_M2 + "' THEN SCRAP_QTY ELSE 0 END) AS M2ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M1 + "' AND '" + strEDate_M1 + "' THEN OUT_QTY ELSE 0 END) AS M1OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M1 + "' AND '" + strEDate_M1 + "' THEN SCRAP_QTY ELSE 0 END) AS M1ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M0 + "' AND '" + strEDate_M0 + "' THEN OUT_QTY ELSE 0 END) AS M0OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_M0 + "' AND '" + strEDate_M0 + "' THEN SCRAP_QTY ELSE 0 END) AS M0ScrapQty";

        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W4 + "' AND '" + strEDate_W4 + "' THEN OUT_QTY ELSE 0 END) AS W4OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W4 + "' AND '" + strEDate_W4 + "' THEN SCRAP_QTY ELSE 0 END) AS W4ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W3 + "' AND '" + strEDate_W3 + "' THEN OUT_QTY ELSE 0 END) AS W3OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W3 + "' AND '" + strEDate_W3 + "' THEN SCRAP_QTY ELSE 0 END) AS W3ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W2 + "' AND '" + strEDate_W2 + "' THEN OUT_QTY ELSE 0 END) AS W2OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W2 + "' AND '" + strEDate_W2 + "' THEN SCRAP_QTY ELSE 0 END) AS W2ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W1 + "' AND '" + strEDate_W1 + "' THEN OUT_QTY ELSE 0 END) AS W1OutQty,SUM(CASE WHEN REPORTDATE BETWEEN '" + strBDate_W1 + "' AND '" + strEDate_W1 + "' THEN SCRAP_QTY ELSE 0 END) AS W1ScrapQty";

        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D7 + "' THEN OUT_QTY ELSE 0 END) AS D7OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D7 + "' THEN SCRAP_QTY ELSE 0 END) AS D7ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D6 + "' THEN OUT_QTY ELSE 0 END) AS D6OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D6 + "' THEN SCRAP_QTY ELSE 0 END) AS D6ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D5 + "' THEN OUT_QTY ELSE 0 END) AS D5OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D5 + "' THEN SCRAP_QTY ELSE 0 END) AS D5ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D4 + "' THEN OUT_QTY ELSE 0 END) AS D4OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D4 + "' THEN SCRAP_QTY ELSE 0 END) AS D4ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D3 + "' THEN OUT_QTY ELSE 0 END) AS D3OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D3 + "' THEN SCRAP_QTY ELSE 0 END) AS D3ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D2 + "' THEN OUT_QTY ELSE 0 END) AS D2OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D2 + "' THEN SCRAP_QTY ELSE 0 END) AS D2ScrapQty";
        strSql = strSql + " ,SUM(CASE WHEN REPORTDATE = '" + strDate_D1 + "' THEN OUT_QTY ELSE 0 END) AS D1OutQty,SUM(CASE WHEN REPORTDATE = '" + strDate_D1 + "' THEN SCRAP_QTY ELSE 0 END) AS D1ScrapQty";

        strSql = strSql + " INTO #QtyPartID";
        strSql = strSql + " FROM #EIS_SUM";
        strSql = strSql + " GROUP BY FABID,PLANTID,PRODID+ ' ' +TP_PART_NUM,COMP_M_GROUP+ ' ' +COMP_PART;";

        strSql = strSql + " SELECT A.FABID,A.PLANTID,A.ProdID,A.PartID";
        strSql = strSql + " ,CASE WHEN SUM(A.M5OutQty) = 0 THEN CASE WHEN SUM(A.M5ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M5ScrapQty + A.M5OutQty) / SUM(A.M5OutQty)  END AS M5";
        strSql = strSql + " ,CASE WHEN SUM(A.M4OutQty) = 0 THEN CASE WHEN SUM(A.M4ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M4ScrapQty + A.M4OutQty) / SUM(A.M4OutQty)  END AS M4";
        strSql = strSql + " ,CASE WHEN SUM(A.M3OutQty) = 0 THEN CASE WHEN SUM(A.M3ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M3ScrapQty + A.M3OutQty) / SUM(A.M3OutQty)  END AS M3";
        strSql = strSql + " ,CASE WHEN SUM(A.M2OutQty) = 0 THEN CASE WHEN SUM(A.M2ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M2ScrapQty + A.M2OutQty) / SUM(A.M2OutQty)  END AS M2";
        strSql = strSql + " ,CASE WHEN SUM(A.M1OutQty) = 0 THEN CASE WHEN SUM(A.M1ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M1ScrapQty + A.M1OutQty) / SUM(A.M1OutQty)  END AS M1";
        strSql = strSql + " ,CASE WHEN SUM(A.M0OutQty) = 0 THEN CASE WHEN SUM(A.M0ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M0ScrapQty + A.M0OutQty) / SUM(A.M0OutQty)  END AS M0";
        strSql = strSql + " ,CASE WHEN SUM(A.W4OutQty) = 0 THEN CASE WHEN SUM(A.W4ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W4ScrapQty + A.W4OutQty) / SUM(A.W4OutQty)  END AS W4";
        strSql = strSql + " ,CASE WHEN SUM(A.W3OutQty) = 0 THEN CASE WHEN SUM(A.W3ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W3ScrapQty + A.W3OutQty) / SUM(A.W3OutQty)  END AS W3";
        strSql = strSql + " ,CASE WHEN SUM(A.W2OutQty) = 0 THEN CASE WHEN SUM(A.W2ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W2ScrapQty + A.W2OutQty) / SUM(A.W2OutQty)  END AS W2";
        strSql = strSql + " ,CASE WHEN SUM(A.W1OutQty) = 0 THEN CASE WHEN SUM(A.W1ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W1ScrapQty + A.W1OutQty) / SUM(A.W1OutQty)  END AS W1";
        strSql = strSql + " ,CASE WHEN SUM(A.D7OutQty) = 0 THEN CASE WHEN SUM(A.D7ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D7ScrapQty + A.D7OutQty) / SUM(A.D7OutQty)  END AS D7";
        strSql = strSql + " ,CASE WHEN SUM(A.D6OutQty) = 0 THEN CASE WHEN SUM(A.D6ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D6ScrapQty + A.D6OutQty) / SUM(A.D6OutQty)  END AS D6";
        strSql = strSql + " ,CASE WHEN SUM(A.D5OutQty) = 0 THEN CASE WHEN SUM(A.D5ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D5ScrapQty + A.D5OutQty) / SUM(A.D5OutQty)  END AS D5";
        strSql = strSql + " ,CASE WHEN SUM(A.D4OutQty) = 0 THEN CASE WHEN SUM(A.D4ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D4ScrapQty + A.D4OutQty) / SUM(A.D4OutQty)  END AS D4";
        strSql = strSql + " ,CASE WHEN SUM(A.D3OutQty) = 0 THEN CASE WHEN SUM(A.D3ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D3ScrapQty + A.D3OutQty) / SUM(A.D3OutQty)  END AS D3";
        strSql = strSql + " ,CASE WHEN SUM(A.D2OutQty) = 0 THEN CASE WHEN SUM(A.D2ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D2ScrapQty + A.D2OutQty) / SUM(A.D2OutQty)  END AS D2";
        strSql = strSql + " ,CASE WHEN SUM(A.D1OutQty) = 0 THEN CASE WHEN SUM(A.D1ScrapQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D1ScrapQty + A.D1OutQty) / SUM(A.D1OutQty)  END AS D1";
        strSql = strSql + " INTO #DATEPartID";
        strSql = strSql + " FROM #QtyPartID A";
        strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.ProdID,A.PartID;";

        /*Table_0 計算單一ProjectName的數據*/
        strSql = strSql + " SELECT FABID,PLANTID,PROJECT_NAME,COMP_M_GROUP";
        strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
        strSql = strSql + " FROM #DATEProjectName";
        strSql = strSql + " ORDER BY";
        strSql = strSql + " FABID";
        strSql = strSql + " ,CASE PLANTID WHEN 'CELL2' THEN 1 WHEN 'MODULE' THEN 2 ELSE 3 END";
        strSql = strSql + " ,PROJECT_NAME";
        strSql = strSql + " ,CASE COMP_M_GROUP WHEN 'PANEL' THEN 1 WHEN '26' THEN 2 WHEN 'BC' THEN 3 WHEN 'BF' THEN 4 WHEN 'BL' THEN 5 WHEN 'DF' THEN 6 WHEN 'DT' THEN 7 WHEN 'FP' THEN 8 ELSE 99 END";
        strSql = strSql + " ,COMP_M_GROUP;";
        
        /*Table_1 計算各TP對應的物料料號的數據*/
        strSql = strSql + " SELECT FABID,PLANTID,ProdID,PartID";
        strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
        strSql = strSql + " FROM #DATEPartID";
        strSql = strSql + " ORDER BY";
        strSql = strSql + " FABID";
        strSql = strSql + " ,CASE PLANTID WHEN 'CELL2' THEN 1 WHEN 'MODULE' THEN 2 ELSE 3 END";
        strSql = strSql + " ,ProdID,PartID;";
        #endregion

        /*Table_2 抓取PlanRate的數據*/
        strSql = strSql + " SELECT CAST([Year] AS VARCHAR(4)) + '-' + RIGHT('0'+ CAST([Month] AS VARCHAR(2)),2) AS PlanMonth,PlanType,PLANTID,PROJECT_NAME,PartType,PlanRate ";
        strSql = strSql + " FROM MPlanRateM ";
        strSql = strSql + " WHERE [Year] >= DATEPART(YEAR, '" + strBDate_M5 + "') AND [Year] <= DATEPART(YEAR, '" + strEDate_M0 + "')";
        strSql = strSql + " AND PlanType = '" + CstrPEPlan + "' ";
        strSql = strSql + " AND PLANTID = CASE WHEN '" + strPlant + "' = 'ALL' THEN PLANTID ELSE '" + strPlant + "' END; ";

        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        gvData.DataSource = dsTmp.Tables[0];
        gvData.DataBind();

        gvDataPartID.DataSource = dsTmp.Tables[1];
        gvDataPartID.DataBind();

        if (dsTmp.Tables[0].Rows.Count > 0)
        {
            GridViewFormat(gvData, dsTmp, strPlant, strWoType, strProdID, strProjectName, dtDate.ToString("yyyy-MM-dd"));
            gvData.HeaderRow.Visible = false;
        }
        if (dsTmp.Tables[1].Rows.Count > 0)
        {
            GridViewFormat(gvDataPartID, dsTmp, strPlant, strWoType, strProdID, strProjectName, dtDate.ToString("yyyy-MM-dd"));
            gvDataPartID.HeaderRow.Visible = false;
        }
        DataBoundChart(dsTmp.Tables[0].DefaultView.ToTable(), this.Title, strProjectName, dtDate.ToString("MM"));

        DateTime dtEnd = DateTime.Now;
        TimeSpan tsDiff = dtEnd - dtBegin;
        strTimeDiff = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";
        mc.RecordLogInfo(strFabID, "UnLogin", Session.SessionID.ToString(), "Query_Child","", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");
    }
    protected void GridViewFormat(GridView gvTmp, DataSet dsTmp, string strPlant, string strWoType, string strProdID, string strProjectName, string strMFGDate)
    {
        /*處理GridView的格式*/
        ModuleClass mc = new ModuleClass();
        if (gvTmp.Rows.Count > 0)
        {
            /*GridView循環處理 無生產 和 有報廢數量的邏輯*/
            for (int i = 0; i < gvTmp.Rows.Count; i++)
            {
                string strGVPartType = Server.UrlEncode(gvTmp.Rows[i].Cells[1].Text.Replace("&nbsp;", "").ToString());
                for (int j = 2; j < gvTmp.Columns.Count; j++)    /*6個月+4周+7天*/
                {
                    if (gvTmp.Rows[i].Cells[j].Text.ToString() != "-2.000" && gvTmp.Rows[i].Cells[j].Text.ToString() != "-1.000")
                    {
                        double dblPlanRate = mc.GetPlanRate(dsTmp.Tables[2], QueryMonth(j), strPlant, strProjectName, strGVPartType, "PEPlan");
                        //double dblPlanRate = 10000;
                        /*判斷是否有超PLAN的RATE，超了顯示紅色字體*/
                        if (Convert.ToDouble(gvTmp.Rows[i].Cells[j].Text.Trim().ToString()) >= dblPlanRate) { gvTmp.Rows[i].Cells[j].ForeColor = Color.Red; } else { gvTmp.Rows[i].Cells[j].ForeColor = Color.Black; }
                    }
                    if (gvTmp.Rows[i].Cells[j].Text.ToString() == "-2.000") { gvTmp.Rows[i].Cells[j].Text = ""; gvTmp.Rows[i].Cells[j].ToolTip = "提示：當天無數據"; }
                    if (gvTmp.Rows[i].Cells[j].Text.ToString() == "-1.000") { gvTmp.Rows[i].Cells[j].Text = "*0.000"; gvTmp.Rows[i].Cells[j].ToolTip = "提示：當天產出數為0，報廢數不為0"; }
                }
            }
            /*合併欄位,要放在處理欄位雙擊時間之後處理*/
            for (int i = 0; i < 1; i++)
            {
                mc.GroupRows(gvTmp, i);
            }
        }
    }
    protected string QueryMonth(int intCol)
    {
        string strMonth = "";
        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);
        if (intCol >= 2 && intCol <= 18)
        {
            strMonth = dtDate.AddMonths(1).ToString("yyyy-MM");
        }

        if (intCol >= 12 && intCol <= 18)
        {
            strMonth = dtDate.AddDays(intCol - 20).ToString("yyyy-MM");
        }
        if (intCol >= 8 && intCol <= 11)
        {
            strMonth = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - (11 - intCol) * 7).ToString("yyyy-MM");
        }
        if (intCol >= 2 && intCol <= 7)
        {
            strMonth = dtDate.AddMonths(intCol - 7).ToString("yyyy-MM");
        }
        return strMonth;
    }
    protected void DataBoundChart(DataTable dtTmp, string strTitle,string strProjectName, string strMonth)
    {
        ModuleClass mc=new ModuleClass();
        DataTable dtChart = new DataTable();
        dtChart.Columns.Add(new DataColumn("Month", typeof(string)));
        for (int i = 0; i < dtTmp.Rows.Count; i++)
        {
            dtChart.Columns.Add(new DataColumn(dtTmp.Rows[i]["COMP_M_GROUP"].ToString(), typeof(double)));
        }
        dtChart.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] keys = new DataColumn[1];
        keys[0] = dtChart.Columns["Month"];
        dtChart.PrimaryKey = keys;

        double dblYMin = 1;
        double dblYMax = 0;
        for (int m = 0; m < 6; m++)
        {
            DataRow drNew = dtChart.NewRow();
            int intMonth = ((12 + Convert.ToInt32(strMonth) + 6 + 1 + m) % 12 == 0) ? 12 : (12 + Convert.ToInt32(strMonth) + 6 + 1 + m) % 12;
            drNew["Month"] = mc.GetEngMonth(intMonth.ToString().PadLeft(2, '0'));
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                double dblValue=Convert.ToDouble(dtTmp.Rows[i][4 + m].ToString());
                if (dblValue == -2.00) { dblValue = 1.00; }
                if (dblValue == -1.00) { dblValue = 0.00; }
                drNew[dtTmp.Rows[i]["COMP_M_GROUP"].ToString()] = dblValue;
                if (dblValue > dblYMax) { dblYMax = dblValue; }
                if (dblValue < dblYMin) { dblYMin = dblValue; }
            }
            dtChart.Rows.Add(drNew);
        }
        ChartData.Visible = false;
        if (dtChart.Rows.Count > 0)
        {
            ChartData.Visible = true;
            ChartData.DataSource = "";
            ChartData.Series.Clear();
            ChartData.ChartAreas.Clear();
            ChartData.Titles.Clear();
            ChartData.Legends.Clear();
            ChartData.DataSource = dtChart;

            ChartData.Titles.Add("Title1");
            ChartData.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartData.Titles[0].Font = new Font("Arial", 14, FontStyle.Italic);
            ChartData.Titles[0].ForeColor = Color.Black;
            ChartData.Titles[0].Text = strProjectName + strTitle;

            for (int i = 0; i < dtChart.Columns.Count - 1; i++)
            {
                ChartData.Series.Add("Series" + (i + 1).ToString());
                switch (i)
                {
                    case 0: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Blue; break;
                    case 1: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Red; break;
                    case 2: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Yellow; break;
                    case 3: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Black; break;
                    case 4: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Magenta; break;
                    case 5: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Pink; break;
                    case 6: ChartData.Series["Series" + (i + 1).ToString()].Color = Color.Green; break;
                    default:
                        break;
                }
                ChartData.Series["Series" + (i + 1).ToString()].Name = dtChart.Columns[i + 1].ColumnName.ToString();


                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].XValueMember = "Month";
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].YValueMembers = dtChart.Columns[i + 1].ColumnName.ToString();
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].YAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].ToolTip = " #VALY{##,##0.###}";
                ChartData.Series[dtChart.Columns[i + 1].ColumnName.ToString()].BorderWidth = 2;
            }
            ChartData.BackColor = Color.PaleTurquoise;
            //ChartData.BackGradientEndColor = Color.White;
            //ChartData.BackGradientType = GradientType.DiagonalLeft;
            ChartData.BorderlineColor = Color.Black;
            ChartData.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.SeaGreen;

            ChartData.Legends.Add("Default");
            ChartData.Legends["Default"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Bottom;
            //ChartData.Legends["Default"].BackColor = System.Drawing.Color.FromArgb(234, 234, 234);
            ChartData.Legends["Default"].BackColor = Color.Transparent;
            ChartData.Legends["Default"].BorderColor = Color.Transparent;

            ChartData.ChartAreas.Add("Default");
            //ChartData.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartData.ChartAreas["Default"].BackColor = Color.Transparent;
            ChartData.ChartAreas["Default"].BorderColor = Color.Peru;

            ChartData.ChartAreas["Default"].AxisX.LabelStyle.Angle = 0;
            ChartData.ChartAreas["Default"].AxisX.Interval = 1;
            //ChartData.ChartAreas["Default"].AxisX.Title = "REPORTMONTH";
            //ChartData.ChartAreas["Default"].AxisY.Title = "Rate";
            //ChartData.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
            //ChartData.ChartAreas["Default"].AxisY.Interval = 0.080;
            ChartData.ChartAreas["Default"].AxisY.Minimum = Math.Round(dblYMin, 2);
            ChartData.ChartAreas["Default"].AxisY.Maximum = Math.Round(dblYMax, 2);

            ChartData.ChartAreas["Default"].AxisY.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
            //ChartData.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.White;
            ChartData.ChartAreas["Default"].AxisX.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Dot;
            //ChartData.ChartAreas["Default"].AxisX.MajorGrid.LineColor = Color.White;

            //ChartData.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);

            //ChartData.UI.Toolbar.Enabled = true;
            //ChartData.UI.Toolbar.Position.Width = 28;
            //ChartData.UI.Toolbar.BorderStyle = 0;

            ChartData.BorderSkin.BackColor = Color.Transparent;
            //ChartData.BorderSkin.FrameBackGradientEndColor = Color.Teal;
            ChartData.BorderSkin.PageColor = Color.AliceBlue;


            ChartData.DataBind();
        }
    }
    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ModuleClass mc = new ModuleClass();
            int intCnt = gvData.Columns.Count;
            for (int h = 0; h < 2; h++)
            {
                GridViewRow RowHeader = new GridViewRow(h, h, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell THeaderCell;
                for (int i = 0; i < intCnt; i++)
                {
                    THeaderCell = new TableHeaderCell();
                    THeaderCell.Text = gvData.Columns[i].HeaderText.ToString();
                    THeaderCell.BackColor = ColorTranslator.FromHtml("#507CD1");
                    THeaderCell.ForeColor = Color.White;
                    THeaderCell.BackColor = Color.YellowGreen;
                    THeaderCell.ForeColor = Color.Black;
                    THeaderCell.CssClass = "Freezing";
                    THeaderCell.Wrap = false;
                    if (h == 0)
                    {
                        if ((i >= 0 && i <= 1))
                        {
                            THeaderCell.RowSpan = 2;
                        }
                        if (i == 2)
                        {
                            THeaderCell.ColumnSpan = 17;
                            THeaderCell.Text = "損耗率";
                        }
                        if (i >= 3 && i <= 18)
                        {
                            THeaderCell.Visible = false;
                        }
                    }
                    else
                    {
                        if ((i >= 0 && i <= 1))
                        {
                            THeaderCell.Visible = false;
                        }
                        if ((i >= 2 && i <= 6))
                        {
                            THeaderCell.Text = mc.GetEngMonth(QueryHeadCellText(i));
                        }
                        if ((i >= 8 && i <= 11))
                        {
                            THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryHeadCellText(i)));
                        }
                        if ((i >= 12 && i <= 18))
                        {
                            THeaderCell.Text = QueryHeadCellText(i);
                        }
                        if ((i >= 7 && i <= 18))
                        {
                            THeaderCell.BackColor = Color.Yellow;
                        }
                    }
                    THeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    RowHeader.Cells.Add(THeaderCell);
                }
                gvData.Controls[0].Controls.AddAt(h, RowHeader);
            }
        }
    }
    protected string QueryHeadCellText(int intCol)
    {
        string strCellText = "";
        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);
        if (intCol >= 12 && intCol <= 18)
        {
            strCellText = dtDate.AddDays(intCol - 18).ToString("MM/dd");
        }
        if (intCol >= 8 && intCol <= 11)
        {
            strCellText = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - (11 - intCol) * 7).ToString("yyyy-MM-dd");
        }
        if (intCol >= 2 && intCol <= 7)
        {
            strCellText = dtDate.AddMonths(intCol - 7).ToString("MM");
        }
        return strCellText;
    }
    protected void gvDataPartID_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ModuleClass mc = new ModuleClass();
            int intCnt = gvDataPartID.Columns.Count;
            for (int h = 0; h < 2; h++)
            {
                GridViewRow RowHeader = new GridViewRow(h, h, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell THeaderCell;
                for (int i = 0; i < intCnt; i++)
                {
                    THeaderCell = new TableHeaderCell();
                    THeaderCell.Text = gvDataPartID.Columns[i].HeaderText.ToString();
                    THeaderCell.BackColor = ColorTranslator.FromHtml("#507CD1");
                    THeaderCell.ForeColor = Color.White;
                    THeaderCell.BackColor = Color.YellowGreen;
                    THeaderCell.ForeColor = Color.Black;
                    THeaderCell.CssClass = "Freezing";
                    THeaderCell.Wrap = false;
                    if (h == 0)
                    {
                        if ((i >= 0 && i <= 1))
                        {
                            THeaderCell.RowSpan = 2;
                        }
                        if (i == 2)
                        {
                            THeaderCell.ColumnSpan = 17;
                            THeaderCell.Text = "損耗率";
                        }
                        if (i >= 3 && i <= 18)
                        {
                            THeaderCell.Visible = false;
                        }
                    }
                    else
                    {
                        if ((i >= 0 && i <= 1))
                        {
                            THeaderCell.Visible = false;
                        }
                        if ((i >= 2 && i <= 6))
                        {
                            THeaderCell.Text = mc.GetEngMonth(QueryHeadCellText(i));
                        }
                        if ((i >= 8 && i <= 11))
                        {
                            THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryHeadCellText(i)));
                        }
                        if ((i >= 12 && i <= 18))
                        {
                            THeaderCell.Text = QueryHeadCellText(i);
                        }
                        if ((i >= 7 && i <= 18))
                        {
                            THeaderCell.BackColor = Color.Yellow;
                        }
                    }
                    THeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    RowHeader.Cells.Add(THeaderCell);
                }
                gvDataPartID.Controls[0].Controls.AddAt(h, RowHeader);
            }
        }
    }
}