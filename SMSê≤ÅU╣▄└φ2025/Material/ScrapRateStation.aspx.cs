using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class Material_ScrapRateStation : System.Web.UI.Page
{
    const string strReportName = "站點損耗率明細";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strFABID = "",strPlant = "", strProjectName = "", strPartType = "", strMFGDT = "", strDT = "", strWoType = "", strProdID = "", strShowPart = "";
            if (Request.QueryString["FABID"] != null) { strFABID = this.Request.QueryString["FABID"].ToString(); }
            if (Request.QueryString["Plant"] != null) { strPlant = this.Request.QueryString["Plant"].ToString(); }
            if (Request.QueryString["ProjectName"] != null) { strProjectName = this.Request.QueryString["ProjectName"].ToString(); }
            if (Request.QueryString["PartType"] != null) { strPartType = this.Request.QueryString["PartType"].ToString(); }
            if (Request.QueryString["MFGDT"] != null) { strMFGDT = this.Request.QueryString["MFGDT"].ToString(); }
            if (Request.QueryString["DTValue"] != null) { strDT = this.Request.QueryString["DTValue"].ToString(); }
            if (Request.QueryString["WoType"] != null) { strWoType = this.Request.QueryString["WoType"].ToString(); }
            if (Request.QueryString["ProdID"] != null) { strProdID = this.Request.QueryString["ProdID"].ToString(); }
            if (Request.QueryString["ShowPart"] != null) { strShowPart = this.Request.QueryString["ShowPart"].ToString(); }

            QueryData(strFABID, strPlant, strProjectName, strPartType, strMFGDT, strDT, strWoType, strProdID, strShowPart);
        }
    }
    protected void QueryData(string strFABID, string strPlant, string strProjectName, string strPartType, string strMFGDT, string strDT, string strWoType, string strProdID, string strShowPart)
    {
        string strTimeDiff = "";
        ModuleClass mc = new ModuleClass();
        DateTime dtBegin = DateTime.Now;

        gvData.Columns[0].HeaderText = "產品號碼";
        gvData.Columns[1].HeaderText = "13NC";
        gvData.Columns[2].HeaderText = "COMP_PART";
        if (strShowPart == "F")
        {
            gvData.Columns[0].HeaderText = "";
            gvData.Columns[1].HeaderText = "";
            gvData.Columns[2].HeaderText = "";
        }

        string strFabID = mc.GetFABID(strPlant);
        DBConn dbConn = new DBConn();

        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);

        string strFromDate = "", strToDate = "";

        string strSql = "";
        if (strDT.Substring(0, 1) == "M")
        {
            switch (strDT)
            {
                case "M5": strFromDate = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 5).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 4).ToString("yyyy-MM-dd"); break;
                case "M4": strFromDate = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 4).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 3).ToString("yyyy-MM-dd"); break;
                case "M3": strFromDate = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 3).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 2).ToString("yyyy-MM-dd"); break;
                case "M2": strFromDate = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 2).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 1).ToString("yyyy-MM-dd"); break;
                case "M1": strFromDate = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 1).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * dtDate.Day).AddMonths(-1 * 0).ToString("yyyy-MM-dd"); break;
                case "MTD": strFromDate = dtDate.AddDays(-1 * dtDate.Day + 1).AddMonths(-1 * 0).ToString("yyyy-MM-dd"); strToDate = dtDate.ToString("yyyy-MM-dd"); break;
                default:
                    break;
            }
        }
        if (strDT.Substring(0, 1) == "W")
        {
            switch (strDT)
            {
                case "W4": strFromDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 4 * 7 + 1).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 3 * 7).ToString("yyyy-MM-dd"); break;
                case "W3": strFromDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 3 * 7 + 1).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 2 * 7).ToString("yyyy-MM-dd"); break;
                case "W2": strFromDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 2 * 7 + 1).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 1 * 7).ToString("yyyy-MM-dd"); break;
                case "W1": strFromDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 1 * 7 + 1).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 0 * 7).ToString("yyyy-MM-dd"); break;
                default:
                    break;
            }
        }
        if (strDT.Substring(0, 1) == "D")
        {
            switch (strDT)
            {
                case "D7": strFromDate = dtDate.AddDays(-6).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-6).ToString("yyyy-MM-dd"); break;
                case "D6": strFromDate = dtDate.AddDays(-5).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-5).ToString("yyyy-MM-dd"); break;
                case "D5": strFromDate = dtDate.AddDays(-4).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-4).ToString("yyyy-MM-dd"); break;
                case "D4": strFromDate = dtDate.AddDays(-3).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-3).ToString("yyyy-MM-dd"); break;
                case "D3": strFromDate = dtDate.AddDays(-2).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-2).ToString("yyyy-MM-dd"); break;
                case "D2": strFromDate = dtDate.AddDays(-1).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-1).ToString("yyyy-MM-dd"); break;
                case "D1": strFromDate = dtDate.AddDays(-0).ToString("yyyy-MM-dd"); strToDate = dtDate.AddDays(-0).ToString("yyyy-MM-dd"); break;
                default:
                    break;
            }
        }

        if (strShowPart == "F")
        {
            strSql = strSql + " SELECT '' as PRODID,'' as TP_PART_NUM,T1.WORKCTR,T1.COMP_M_GROUP,'' as COMP_PART";
        }
        else
        {
            strSql = strSql + " SELECT T1.PRODID,T1.TP_PART_NUM,T1.WORKCTR,T1.COMP_M_GROUP,T1.COMP_PART";
        }
        strSql = strSql + " ,SUM(CASE WHEN T1.DEFECTCODE NOT LIKE 'R%' THEN T1.SCRAPQTY ELSE 0 END) AS Scrap1Qty";
        strSql = strSql + " ,SUM(CASE WHEN T1.DEFECTCODE LIKE 'R%' THEN T1.SCRAPQTY ELSE 0 END) AS Scrap2Qty";
        strSql = strSql + " ,SUM(T1.SCRAPQTY) AS ScrapQty";
        strSql = strSql + " ,SUM(T1.OUTPUTQTY) AS OutputQty";
        strSql = strSql + " INTO #Sum";
        strSql = strSql + " FROM dbo.RPT_EIS T1";

        strSql = strSql + " WHERE T1.REPORTDATE >= '" + strFromDate + "' AND T1.REPORTDATE <= '" + strToDate + "' ";
        if (strFABID == "WE01") { strFABID = "B"; } else { strFABID = ""; };
        strSql = strSql + " AND T1.FABID  = '" + strFABID + "' ";
        strSql = strSql + " AND T1.SHOPCD  = '" + strPlant + "' ";
        strSql = strSql + " AND T1.PROJECT_NAME = '" + strProjectName + "'";
        strSql = strSql + " AND T1.COMP_M_GROUP = '" + strPartType + "'";
        #region parameters [選擇條件過濾]
        if (strWoType != "")
        {
            string[] str2WorkType = strWoType.Split(',');
            string strSqlWortType = "";
            for (int i = 0; i < str2WorkType.Length; i++)
            {
                if (strSqlWortType == "")
                {
                    strSqlWortType = strSqlWortType + " AND T1.WOTYPE IN( '" + str2WorkType[i].ToString() + "' ";
                }
                else
                {
                    strSqlWortType = strSqlWortType + " , '" + str2WorkType[i].ToString() + "' ";
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
                    strSqlProdID = strSqlProdID + " AND T1.PRODID IN( '" + str2Product[i].ToString() + "' ";
                }
                else
                {
                    strSqlProdID = strSqlProdID + " , '" + str2Product[i].ToString() + "' ";
                }
            }
            if (strSqlProdID != "")
            {
                strSql = strSql + strSqlProdID + " )";
            }
        }
        #endregion
        if (strShowPart == "F")
        {
            strSql = strSql + " GROUP BY T1.WORKCTR,T1.COMP_M_GROUP";
            strSql = strSql + " ORDER BY T1.WORKCTR,T1.COMP_M_GROUP;";
        }
        else
        {
            strSql = strSql + " GROUP BY T1.PRODID,T1.TP_PART_NUM,T1.WORKCTR,T1.COMP_M_GROUP,T1.COMP_PART";
            strSql = strSql + " ORDER BY T1.PRODID,T1.TP_PART_NUM,T1.WORKCTR,T1.COMP_M_GROUP,T1.COMP_PART;";
        }

        strSql = strSql + " SELECT T1.PRODID,T1.TP_PART_NUM,T1.WORKCTR";
        strSql = strSql + " ,SUM(T1.Scrap1Qty) AS Scrap1Qty ";
        strSql = strSql + " ,SUM(T1.Scrap2Qty) AS Scrap2Qty";
        strSql = strSql + " ,SUM(T1.ScrapQty) AS ScrapQty";
        strSql = strSql + " ,SUM(T1.OutputQty) AS OutputQty";
        strSql = strSql + " ,CASE WHEN SUM(T2.OutputQty) = 0 THEN 0 ELSE SUM(T2.OutputQty + T1.ScrapQty) / SUM(T2.OutputQty) END AS ScrapRate";
        strSql = strSql + " ,T1.COMP_PART";
        strSql = strSql + " FROM #Sum T1";
        strSql = strSql + " LEFT JOIN (";
        strSql = strSql + "  SELECT COMP_M_GROUP,SUM(Scrap1Qty) AS Scrap1Qty,SUM(Scrap2Qty) AS Scrap2Qty,SUM(ScrapQty) AS ScrapQty,SUM(OutputQty) AS OutputQty,COMP_PART ";
        strSql = strSql + "  FROM #Sum ";
        strSql = strSql + "  GROUP BY COMP_M_GROUP,COMP_PART";
        strSql = strSql + " )T2 ON (T1.COMP_M_GROUP = T2.COMP_M_GROUP AND T1.COMP_PART = T2.COMP_PART)";
        strSql = strSql + " GROUP BY T1.PRODID,T1.TP_PART_NUM,T1.WORKCTR,T1.COMP_PART";
        strSql = strSql + " UNION ALL";
        strSql = strSql + " SELECT PRODID,TP_PART_NUM,'Total' AS WORKCTR";
        strSql = strSql + " ,SUM(Scrap1Qty) AS Scrap1Qty";
        strSql = strSql + " ,SUM(Scrap2Qty) AS Scrap2Qty";
        strSql = strSql + " ,SUM(ScrapQty) AS ScrapQty";
        strSql = strSql + " ,SUM(OutputQty) AS OutputQty";
        strSql = strSql + " ,CASE WHEN SUM(OutputQty) = 0 THEN 0 ELSE SUM(OutputQty + ScrapQty) / SUM(OutputQty) END AS ScrapRate";
        strSql = strSql + " ,COMP_PART";
        strSql = strSql + " FROM #Sum";
        strSql = strSql + " GROUP BY PRODID,TP_PART_NUM,COMP_PART";
        strSql = strSql + " ORDER BY PRODID,COMP_PART,WORKCTR";
        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        gvData.DataSource = dsTmp.Tables[0];
        gvData.DataBind();

        lblProjectName.Text = "機種：" + strProjectName;
        lblPartType.Text = "部材：" + strPartType;
        ;
        if (strDT.Substring(0, 1) == "M") { lblDate.Text = "查詢時間：" + QueryHeadCellText(strDT); }
        if (strDT.Substring(0, 1) == "W") { lblDate.Text = "查詢時間：" + QueryHeadCellText(strDT); }
        if (strDT.Substring(0, 1) == "D") { lblDate.Text = "查詢時間：" + QueryHeadCellText(strDT); }

        /*合併欄位,要放在處理欄位雙擊時間之後處理*/
        for (int i = 0; i < 3; i++)
        {
            mc.GroupRows(gvData, i);
        }

        DateTime dtEnd = DateTime.Now;
        TimeSpan tsDiff = dtEnd - dtBegin;
        strTimeDiff = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";
        mc.RecordLogInfo(strFabID, "UnLogin", Session.SessionID.ToString(), "Query_Child", "", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");
    }
    protected string QueryHeadCellText(string strCol)
    {
        ModuleClass mc = new ModuleClass();
        string strCellText = "";
        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);
        if (strCol == "D7") { strCellText = dtDate.AddDays(-6).ToString("MM/dd"); }
        if (strCol == "D6") { strCellText = dtDate.AddDays(-5).ToString("MM/dd"); }
        if (strCol == "D5") { strCellText = dtDate.AddDays(-4).ToString("MM/dd"); }
        if (strCol == "D4") { strCellText = dtDate.AddDays(-3).ToString("MM/dd"); }
        if (strCol == "D3") { strCellText = dtDate.AddDays(-2).ToString("MM/dd"); }
        if (strCol == "D2") { strCellText = dtDate.AddDays(-1).ToString("MM/dd"); }
        if (strCol == "D1") { strCellText = dtDate.AddDays(-0).ToString("MM/dd"); }

        if (strCol == "W4") { strCellText = mc.GetWeek(dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 3 * 7)); }
        if (strCol == "W3") { strCellText = mc.GetWeek(dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 2 * 7)); }
        if (strCol == "W2") { strCellText = mc.GetWeek(dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 1 * 7)); }
        if (strCol == "W1") { strCellText = mc.GetWeek(dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - 0 * 7)); }

        if (strCol == "M5") { strCellText =mc.GetEngMonth(dtDate.AddMonths(-5).ToString("MM")); }
        if (strCol == "M4") { strCellText = mc.GetEngMonth(dtDate.AddMonths(-4).ToString("MM")); }
        if (strCol == "M3") { strCellText = mc.GetEngMonth(dtDate.AddMonths(-3).ToString("MM")); }
        if (strCol == "M2") { strCellText = mc.GetEngMonth(dtDate.AddMonths(-2).ToString("MM")); }
        if (strCol == "M1") { strCellText = mc.GetEngMonth(dtDate.AddMonths(-1).ToString("MM")); }
        if (strCol == "MTD") { strCellText = mc.GetEngMonth(dtDate.AddMonths(-0).ToString("MM")); }

        return strCellText;
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*處理GridView的格式*/
        if (gvData.Rows.Count > 0)
        {
            /*GridView循環處理 無生產 和 有報廢數量的邏輯*/
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                if (gvData.Rows[i].Cells[3].Text.ToUpper() == "Total".ToUpper()) { gvData.Rows[i].BackColor = Color.FromArgb(153, 204, 255); }
            }
        }
    }
}