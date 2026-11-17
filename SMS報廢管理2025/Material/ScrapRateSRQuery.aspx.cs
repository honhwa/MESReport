using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
public partial class Material_ScrapRateSRQuery : System.Web.UI.Page
{
    const string strReportName = "SR報表-區間明細查詢";
    protected void Page_Load(object sender, EventArgs e)
    {
        /*定位Left PANEL的滾動條 Start*/
        //定义两个HiddenField，分别纪录Panel的ScrollBar的X与Y位置
        HiddenField HF_ScrollPosX = new HiddenField();
        HiddenField HF_ScrollPosY = new HiddenField();

        HF_ScrollPosX.ID = "ScrollPosX";
        HF_ScrollPosY.ID = "ScrollPosY";
        this.Form.Controls.Add(HF_ScrollPosX);
        this.Form.Controls.Add(HF_ScrollPosY);
        //生成JS：将Panel的ScrollBar的X,Y位置设置给两个HiddenField
        string script;
        script = "window.document.getElementById('" + HF_ScrollPosX.ClientID + "').value = "
                  + "window.document.getElementById('" + PLeft.ClientID + "').scrollLeft;"
                  + "window.document.getElementById('" + HF_ScrollPosY.ClientID + "').value = "
                  + "window.document.getElementById('" + PLeft.ClientID + "').scrollTop;";

        //ScriptManager.RegisterOnSubmitStatement(UpdatePanel1, this.GetType(), "SavePanelScroll", script);
        ClientScript.RegisterOnSubmitStatement(this.GetType(), "SavePanelScroll", script);
        if (IsPostBack) //如果是PostBack，将保存在HiddenField的ScrollBar的X,Y值重设回给Panel的ScrollBar
        {
            script = "window.document.getElementById('" + PLeft.ClientID + "').scrollLeft = "
                    + "window.document.getElementById('" + HF_ScrollPosX.ClientID + "').value;"
                    + "window.document.getElementById('" + PLeft.ClientID + "').scrollTop = "
                    + "window.document.getElementById('" + HF_ScrollPosY.ClientID + "').value;";

            //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "SetPanelScroll", script, true);
            ClientScript.RegisterStartupScript(this.GetType(), "SetPanelScroll", script, true);
        }
        /*定位PANEL的滾動條 End*/

        if (!IsPostBack)
        {
            ModuleClass mc = new ModuleClass();
            string strNTName = "", strPCIP = "", strPCName = "";
            mc.GetLoginInfo(out strNTName, out strPCIP, out strPCName);
            tbNTName.Text = strNTName;

            //if (mc.GetMPCIPM(HttpContext.Current.Request.Url.AbsolutePath.ToString(), strPCIP) == false)
            //{
            //    Response.Redirect("../ShowMessage.aspx");
            //    return;
            //}

            InitialBoundData();
            SetTitle();
            ConfigRole();
            Response.Expires = -1;
        }
    }
    protected void SetTitle()
    {
        this.Title = " SR報表-區間明細查詢";
    }
    protected void InitialBoundData()
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtWorkType = mc.GetMWOTM("");
        WorkOrderType.DataTableValue = dtWorkType;

        DataTable dtSourceFab = mc.GetSourceFab("", "");
        RespShop.DataTableValue = dtSourceFab;

        /*因會依據多個Plant看數據，故產品不做PLANT的過濾*/
        //Product.GetTransMode = "";
        Product.GetPlant = "";
        DataTable dtSIZE = mc.GetSIZE("");
        Product.DataTableSIZEValue = dtSIZE;
    }
    protected void ConfigRole()
    {
        ModuleClass mc = new ModuleClass();
        if (mc.GetRole("Cost", tbNTName.Text) == false) { hlCost.Visible = false; } else { hlCost.Visible = true; }
        if (mc.GetUserGroupCode("Cost", tbNTName.Text) == false) { hlLimitCost.Visible = false; } else { hlLimitCost.Visible = true; }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = false;
        Response.Charset = "UTF-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
        this.EnableViewState = false;
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        PRight.RenderControl(oHtmlTextWriter);
        Response.Output.Write(oStringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        QueryData();
    }
    protected void QueryData()
    {
        ModuleClass mc = new ModuleClass();
        lblRunTime.Text = "";
        DateTime dtBegin = DateTime.Now;
        #region parameters
        DBConn dbConn = new DBConn();
        string strSql = "";
        //strSql = strSql + " SELECT FABID,PRODID,PROJECT_NAME,PRODGROUP,SHOP_CODE,SIZEID INTO #MPIM FROM dbo.MPIM WITH(NOLOCK);";
        //strSql = strSql + " CREATE NONCLUSTERED INDEX [#MPIM_I1] ON [#MPIM] ([FABID],[PRODID]);";
        //strSql = strSql + " SELECT FABID,REPORTYEAR,REPORTMONTH,DEFECTCODE,RATE,SHOPCD INTO #MDCRATEM FROM dbo.MDCRATEM WITH(NOLOCK) WHERE REPORTYEAR >= LEFT('" + DateSelect.FromDate + "',4);";
        //strSql = strSql + " CREATE NONCLUSTERED INDEX [#MDCRATEM_I1] ON [#MDCRATEM] ([FABID],[REPORTYEAR],[REPORTMONTH],[DEFECTCODE],[SHOPCD]);";

        string strFromDate = Date.FromDate.ToString();
        string strToDate = Date.ToDate.ToString();
        string strSqlWortType = mc.ListBoxToValue(WorkOrderType.ListBoxToValue);
        string strSqlProduct = mc.ListBoxToValue(Product.ListBoxToValue);
        string strSqlSIZE = Product.DropDownListSIZE.SelectedValue.Trim().ToString();
        string strSqlProjectName = Product.DropDownListProjectName.SelectedValue.Trim().ToString();

        strSql = strSql + " SELECT T1.REPORTDATE,T1.REPORTYEAR,T1.REPORTMONTH,T1.FABID,T1.PRODID,T1.PROJECT_NAME,T1.PRODGROUP,T1.SIZEID,T1.COMP_PART,T1.COMP_M_GROUP,T1.SHOPCD";
        strSql = strSql + " ,SUM(T1.OUTPUTQTY) AS OUTPUTQTY";//產出
        strSql = strSql + " ,SUM(T1.SCRAPQTY) AS SCRAPQTY";//總共報廢數量
        strSql = strSql + " ,SUM(PRE_SCRAPQTY) AS PRE_SCRAPQTY";//前責報廢數量
        strSql = strSql + " ,CASE WHEN T1.SOURCE_FAB='SH' THEN '' ELSE T1.SOURCE_FAB END SOURCE_FAB";
        strSql = strSql + " INTO #EIS_SUM";
        strSql = strSql + " FROM dbo.RPT_EIS T1 WITH(NOLOCK)";
        strSql = strSql + " WHERE REPORTDATE >= '" + strFromDate + "' AND REPORTDATE <= '" + strToDate + "' ";
        #region parameters [選擇條件過濾]
        if (RespShop.strValue.ToUpper() != "TOTAL" && RespShop.strValue.ToUpper() != "SH")
        {
            strSql = strSql + " AND T1.SOURCE_FAB = '" + RespShop.strValue.ToString() + "' ";
        }
        if (WorkOrder.strValue != "")//工單
        {
            strSql = strSql + " AND WONBR = '" + WorkOrder.strValue + "' ";
        }
        if (strSqlWortType != "")//工單類型
        {
            strSql = strSql + " AND WOTYPE IN(" + strSqlWortType + ")";
        }
        if (strSqlProduct != "")//TP料號
        {
            strSql = strSql + " AND PRODID IN(" + strSqlProduct + ");";
        }
        else
        {
            if (strSqlProjectName != "")//型號
            {
                strSql = strSql + " AND PROJECT_NAME ='" + strSqlProjectName + "'";
            }
            else
            {
                if (strSqlSIZE != "")//尺寸
                {
                    strSql = strSql + " AND SIZEID ='" + strSqlSIZE + "' ";
                }
            }
        }
        #endregion
        strSql = strSql + " GROUP BY T1.REPORTDATE,T1.REPORTYEAR,T1.REPORTMONTH,T1.FABID,T1.PRODID,T1.PROJECT_NAME,T1.PRODGROUP,T1.SIZEID,T1.COMP_PART,T1.COMP_M_GROUP,T1.SHOPCD,T1.SOURCE_FAB;";


        strSql = strSql + " SELECT CASE WHEN T1.FABID = 'B' THEN 'WE01' ELSE '' END AS FABID";
        strSql = strSql + " ,SHOPCD";
        strSql = strSql + " ,T1.PRODGROUP";
        strSql = strSql + " ,T1.SIZEID";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.PRODID";
        strSql = strSql + " ,SUM(CASE WHEN T1.COMP_M_GROUP = 'PANEL' THEN OUTPUTQTY ELSE 0 END) AS OutQty";
        strSql = strSql + " ,SUM(T1.OUTPUTQTY * ISNULL(TC.Unit_Cost,0)) AS OutCost";
        if (RespShop.strValue.ToUpper() == "TOTAL")
        {
            strSql = strSql + " ,SUM(T1.SCRAPQTY * ISNULL(TC.Unit_Cost,0)) AS ScrapCost";
            strSql = strSql + " ,SUM(T1.OUTPUTQTY +T1.SCRAPQTY) AS TotalQty";
            strSql = strSql + " ,SUM((T1.OUTPUTQTY +T1.SCRAPQTY)* ISNULL(TC.Unit_Cost,0)) AS TotalCost";
        }
        else if (RespShop.strValue.ToUpper() == "SH")
        {   /*SOURCE_FAB 為空的 定義為 SH，所有PRE_SCRAPQTY報廢不需要減去*/
            strSql = strSql + " ,SUM((T1.SCRAPQTY - CASE WHEN SOURCE_FAB != '' THEN T1.PRE_SCRAPQTY ELSE 0 END) * ISNULL(TC.Unit_Cost,0)) AS ScrapCost";
            strSql = strSql + " ,SUM(T1.OUTPUTQTY + T1.SCRAPQTY - CASE WHEN SOURCE_FAB != '' THEN T1.PRE_SCRAPQTY ELSE 0 END) AS TotalQty";
            strSql = strSql + " ,SUM((T1.OUTPUTQTY + T1.SCRAPQTY - CASE WHEN SOURCE_FAB != '' THEN T1.PRE_SCRAPQTY ELSE 0 END)* ISNULL(TC.Unit_Cost,0)) AS TotalCost";
        }
        else
        {
            strSql = strSql + " ,SUM(T1.PRE_SCRAPQTY * ISNULL(TC.Unit_Cost,0)) AS ScrapCost";
            strSql = strSql + " ,SUM(T1.OUTPUTQTY +T1.PRE_SCRAPQTY) AS TotalQty";
            strSql = strSql + " ,SUM((T1.OUTPUTQTY +T1.PRE_SCRAPQTY)* ISNULL(TC.Unit_Cost,0)) AS TotalCost";
        }
        strSql = strSql + " INTO #QtyCost";
        strSql = strSql + " FROM #EIS_SUM T1";
        strSql = strSql + " LEFT JOIN dbo.MUCM TC ON(T1.REPORTYEAR=TC.[Year] AND T1.REPORTMONTH=TC.[Month] AND T1.COMP_PART = TC.Material) ";
        strSql = strSql + " GROUP BY CASE WHEN T1.FABID = 'B' THEN 'WE01' ELSE '' END";
        strSql = strSql + " ,SHOPCD";
        strSql = strSql + " ,T1.PRODGROUP";
        strSql = strSql + " ,T1.SIZEID";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.PRODID;";

        strSql = strSql + " SELECT CASE WHEN T1.FABID = 'B' THEN 'WE01' ELSE '' END AS FABID";
        strSql = strSql + " ,T1.PRODID";
        strSql = strSql + " ,T1.PRODID AS TP_PART_NUM";
        strSql = strSql + " ,TP.PRODGROUP";
        strSql = strSql + " ,TP.PROJECT_NAME";
        strSql = strSql + " ,T1.SOPRODID";
        strSql = strSql + " INTO #MPMPM";
        strSql = strSql + " FROM dbo.MPMPM T1";
        strSql = strSql + " INNER JOIN dbo.MPIM TP ON T1.FABID = TP.FABID AND T1.PRODID = TP.PRODID";
        strSql = strSql + " WHERE T1.PRODID!=T1.SOPRODID";
        strSql = strSql + " AND T1.PRODID IN(SELECT PRODID FROM #QtyCost);";

        strSql = strSql + " SELECT T2.FABID,T2.PRODGROUP,T2.SIZEID,T2.PROJECT_NAME,T2.PRODID";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'CELL2' THEN ISNULL(T2.OutQty,0) ELSE 0 END) + SUM(CASE WHEN T2.SHOPCD LIKE 'MODULE' THEN ISNULL(T2.OutQty,0) ELSE 0 END) AS OutQty";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'CELL2' THEN ISNULL(T2.OutQty,0) ELSE 0 END) AS CELL2OutQty ";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'MODULE' THEN ISNULL(T2.OutQty,0) ELSE 0 END) AS MODULEOutQty";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'CELL2' THEN ISNULL(T2.ScrapCost,0) ELSE 0 END) AS CELL2ScrapCost";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'CELL2' THEN ISNULL(T2.OutCost,0) ELSE 0 END) AS CELL2OutCost";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'MODULE' THEN ISNULL(T2.ScrapCost,0) ELSE 0 END) AS MODULEScrapCost";
        strSql = strSql + " ,SUM(CASE WHEN T2.SHOPCD LIKE 'MODULE' THEN ISNULL(T2.OutCost,0) ELSE 0 END) AS MODULEOutCost";
        strSql = strSql + " INTO #Cost0";
        strSql = strSql + " FROM #MPMPM T1";
        strSql = strSql + " LEFT JOIN #QtyCost T2 ON(T1.FABID=T2.FABID AND T1.PRODID = T2.PRODID)";
        strSql = strSql + " LEFT JOIN #QtyCost T3 ON(T1.FABID=T3.FABID AND T1.SOPRODID = T3.PRODID)";
        strSql = strSql + " GROUP BY T2.FABID,T2.PRODGROUP,T2.SIZEID,T2.PROJECT_NAME,T2.PRODID,ISNULL(T2.OutQty,0);";

        for (int i = 0; i < 5; i++)
        {
            strSql = strSql + " SELECT";
            if (i == 0) { strSql = strSql + " T1.FABID,T1.PRODGROUP,T1.SIZEID,T1.PROJECT_NAME,'合計' AS PRODID,SUM(T1.OutQty) AS OutQty"; }
            if (i == 1) { strSql = strSql + " T1.FABID,T1.PRODGROUP,T1.SIZEID,'合計' AS PROJECT_NAME,'' AS PRODID,SUM(T1.OutQty) AS OutQty"; }
            if (i == 2) { strSql = strSql + " T1.FABID,T1.PRODGROUP,'合計' AS SIZEID,'' AS PROJECT_NAME,'' AS PRODID,SUM(T1.OutQty) AS OutQty"; }
            if (i == 3) { strSql = strSql + " T1.FABID,'合計' AS PRODGROUP,'' AS SIZEID,'' AS PROJECT_NAME,'' AS PRODID,SUM(T1.OutQty) AS OutQty"; }
            if (i == 4) { strSql = strSql + " '合計' AS FABID,'' AS PRODGROUP,'' AS SIZEID,'' AS PROJECT_NAME,'' AS PRODID,SUM(T1.OutQty) AS OutQty"; }
            strSql = strSql + " ,SUM(CELL2OutQty) AS CELL2OutQty";
            strSql = strSql + " ,SUM(MODULEOutQty) AS MODULEOutQty";
            strSql = strSql + " ,SUM(CELL2ScrapCost) AS CELL2ScrapCost";
            strSql = strSql + " ,SUM(CELL2OutCost) AS CELL2OutCost";
            strSql = strSql + " ,SUM(MODULEScrapCost) AS MODULEScrapCost";
            strSql = strSql + " ,SUM(MODULEOutCost) AS MODULEOutCost";
            if (i == 0) { strSql = strSql + " INTO #Cost1"; }
            if (i == 1) { strSql = strSql + " INTO #Cost2"; }
            if (i == 2) { strSql = strSql + " INTO #Cost3"; }
            if (i == 3) { strSql = strSql + " INTO #Cost4"; }
            if (i == 4) { strSql = strSql + " INTO #Cost5"; }
            strSql = strSql + " FROM #Cost0 T1";
            if (i == 0) { strSql = strSql + " GROUP BY T1.FABID,T1.PRODGROUP,T1.SIZEID,T1.PROJECT_NAME;"; }
            if (i == 1) { strSql = strSql + " GROUP BY T1.FABID,T1.PRODGROUP,T1.SIZEID;"; }
            if (i == 2) { strSql = strSql + " GROUP BY T1.FABID,T1.PRODGROUP;"; }
            if (i == 3) { strSql = strSql + " GROUP BY T1.FABID;"; }
            if (i == 4) { strSql = strSql + " ;"; }
        }

        /*定義排序--按照近30天的PANEL的OUTQTY從大到小排序*/
        strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,SUM(OutQty) AS OutQty,ROW_NUMBER()OVER(ORDER BY FABID,PRODGROUP,SUM(OutQty) DESC) AS SORTID";
        strSql = strSql + " INTO #SORT";
        strSql = strSql + " FROM #Cost0 GROUP BY FABID,PRODGROUP,SIZEID,PROJECT_NAME;";

        /*Table_0 Cost Data*/
        strSql = strSql + " SELECT T.FABID,T.PRODGROUP,T.SIZEID,T.PROJECT_NAME,T.PRODID,T.OutQty,T.CELL2OutQty,T.MODULEOutQty,T.Cell2Rate,T.MODULERate,T.TotalRate,T.SORTID";
        strSql = strSql + " ,ISNULL(S.SORTID,999) AS PJ_SORTID";
        strSql = strSql + " FROM(";
        strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,PRODID,OutQty,CELL2OutQty,MODULEOutQty";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END AS Cell2Rate";
        strSql = strSql + " ,CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS MODULERate";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END * CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS TotalRate";
        strSql = strSql + " ,100 AS SORTID";
        strSql = strSql + " FROM #Cost0";
        strSql = strSql + " WHERE CELL2OutCost != 0 OR MODULEOutCost != 0";
        //strSql = strSql + " UNION ALL";
        //strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,PRODID,OutQty,CELL2OutQty,MODULEOutQty";
        //strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END AS Cell2Rate";
        //strSql = strSql + " ,CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS MODULERate";
        //strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END * CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS TotalRate";
        //strSql = strSql + " ,1000 AS SORTID";
        //strSql = strSql + " FROM #Cost1";
        //strSql = strSql + " WHERE CELL2OutCost != 0 OR MODULEOutCost != 0";
        strSql = strSql + " UNION ALL";
        strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,PRODID,OutQty,CELL2OutQty,MODULEOutQty";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END AS Cell2Rate";
        strSql = strSql + " ,CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS MODULERate";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END * CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS TotalRate";
        strSql = strSql + " ,2000 AS SORTID";
        strSql = strSql + " FROM #Cost2";
        strSql = strSql + " WHERE CELL2OutCost != 0 OR MODULEOutCost != 0";
        strSql = strSql + " UNION ALL";
        strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,PRODID,OutQty,CELL2OutQty,MODULEOutQty";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END AS Cell2Rate";
        strSql = strSql + " ,CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS MODULERate";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END * CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS TotalRate";
        strSql = strSql + " ,3000 AS SORTID";
        strSql = strSql + " FROM #Cost3";
        strSql = strSql + " WHERE CELL2OutCost != 0 OR MODULEOutCost != 0";
        strSql = strSql + " UNION ALL";
        strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,PRODID,OutQty,CELL2OutQty,MODULEOutQty";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END AS Cell2Rate";
        strSql = strSql + " ,CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS MODULERate";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END * CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS TotalRate";
        strSql = strSql + " ,4000 AS SORTID";
        strSql = strSql + " FROM #Cost4";
        strSql = strSql + " WHERE CELL2OutCost != 0 OR MODULEOutCost != 0";
        strSql = strSql + " UNION ALL";
        strSql = strSql + " SELECT FABID,PRODGROUP,SIZEID,PROJECT_NAME,PRODID,OutQty,CELL2OutQty,MODULEOutQty";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END AS Cell2Rate";
        strSql = strSql + " ,CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS MODULERate";
        strSql = strSql + " ,CASE WHEN CELL2OutCost = 0 THEN 1 ELSE (CELL2ScrapCost + CELL2OutCost) / CELL2OutCost END * CASE WHEN MODULEOutCost = 0 THEN 1 ELSE (MODULEScrapCost + MODULEOutCost) / MODULEOutCost END AS TotalRate";
        strSql = strSql + " ,4000 AS SORTID";
        strSql = strSql + " FROM #Cost5";
        strSql = strSql + " WHERE CELL2OutCost != 0 OR MODULEOutCost != 0";
        strSql = strSql + " )T";
        strSql = strSql + " LEFT JOIN #SORT S ON ( T.FABID=S.FABID AND T.PRODGROUP=S.PRODGROUP AND T.PROJECT_NAME=S.PROJECT_NAME)";
        strSql = strSql + " ORDER BY CASE WHEN T.FABID = '合計' then 1 ELSE 0 END,FABID";
        strSql = strSql + " ,CASE WHEN T.PRODGROUP = '合計' then 1 ELSE 0 END";
        strSql = strSql + " ,CASE T.PRODGROUP WHEN 'Alpine' THEN 1 WHEN 'Audi' THEN 2 WHEN 'BMW' THEN 3 WHEN 'Bosch' THEN 4 WHEN 'Continental' THEN 5 ELSE 30 END";
        strSql = strSql + " ,CASE WHEN T.SIZEID = '合計' then 1 ELSE 0 END";
        strSql = strSql + " ,T.SIZEID";
        strSql = strSql + " ,CASE WHEN T.PROJECT_NAME = '合計' then 1 ELSE 0 END";
        strSql = strSql + " ,T.PROJECT_NAME";
        strSql = strSql + " ,T.PRODID";
        strSql = strSql + " ,ISNULL(S.SORTID,999) ";
        strSql = strSql + " ,T.SORTID;";

        /*Table_1 查詢未維護單價的料號數量*/
        strSql = strSql + " SELECT DISTINCT CONVERT(VARCHAR(7), REPORTDATE,120) AS ReportMonth,COMP_PART,ISNULL(t2.Unit_Cost,0) AS UnitCost FROM #EIS_SUM T1";
        strSql = strSql + " LEFT JOIN dbo.MUCM T2 ON (T1.REPORTYEAR=T2.[Year] AND T1.REPORTMONTH=T2.[Month] AND T1.COMP_PART=T2.Material)";
        strSql = strSql + " WHERE T2.Unit_Cost IS NULL;";

        /*Table_2 檢查是否有未設定ProjectName的料號*/
        strSql = strSql + " SELECT DISTINCT A.PROJECT_NAME FROM #QtyCost A";
        strSql = strSql + " WHERE ISNULL(A.PROJECT_NAME,'') = '';";
        #endregion
        DataSet dsData = dbConn.GetDataSetBySql(strSql);
        DataTable dtCost = dsData.Tables[1];
        if (dtCost.Rows.Count != 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('有" + dtCost.Rows.Count.ToString() + "顆料號沒有維護價格，系統默認價格為0，為不影響計算，請經管盡快維護！');</script>");
        }
        DataTable dtProjectName = dsData.Tables[2];
        if (dtProjectName.Rows.Count != 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('有" + dtProjectName.Rows.Count.ToString() + "顆料號沒有維護ProjectName，系統默認不會呈現相關數據，為不影響計算，請與主檔維護人員確認！');</script>");
        }
        DataTable dtQuery = dsData.Tables[0];

        gvData.DataSource = dtQuery;
        gvData.DataBind();
        if (dtQuery.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
        else
        {
            GridViewFormatCost();
            gvData.HeaderRow.Visible = false;
        } 
        mc.GridViewOnMouse(gvData);
        DateTime dtEnd = DateTime.Now;
        TimeSpan tsDiff = dtEnd - dtBegin;
        lblRunTime.Text = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";

        /*使用記錄，每隻報表必須要增加*/
        mc.RecordLogInfo("B", "UnLogin", Session.SessionID.ToString(), "Query", "", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");
        SetTitle();
    }
    protected void GridViewFormatCost()
    {
        /*處理GridView的格式*/
        ModuleClass mc = new ModuleClass();
        if (gvData.Rows.Count > 0)
        {
            /*GridView循環處理 無生產 和 有報廢數量的邏輯*/
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                if (gvData.Rows[i].Cells[0].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.YellowGreen; }
                if (gvData.Rows[i].Cells[1].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.FromArgb(153, 204, 255); }
                if (gvData.Rows[i].Cells[2].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.FromArgb(204, 153, 255); }
                if (gvData.Rows[i].Cells[3].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.FromArgb(51, 204, 204); }
                ///*合併欄位,要放在處理欄位雙擊時間之後處理*/
            }
            for (int i = 0; i < 4; i++)
            {
                mc.GroupRows(gvData, i);
            }
            
        }
    }
    protected void Plant_SelectedIndexChanged()
    {
        InitialBoundData();
    }
    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int intCnt = gvData.Columns.Count;
            for (int h = 0; h < 2; h++)
            {
                GridViewRow RowHeader = new GridViewRow(h, h, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell THeaderCell;
                for (int i = 0; i < intCnt; i++)
                {
                    THeaderCell = new TableHeaderCell();
                    THeaderCell.Text = gvData.Columns[i].HeaderText.ToString();
                    //THeaderCell.BackColor = ColorTranslator.FromHtml("#C8E1FB");
                    THeaderCell.BackColor = Color.YellowGreen;
                    THeaderCell.ForeColor = ColorTranslator.FromHtml("#014F8A");
                    THeaderCell.Wrap = false;
                    if (h == 0)
                    {
                        if (i >= 0 && i <= 7)
                        {
                            THeaderCell.RowSpan = 2;
                        }
                        if (i == 8)
                        {
                            THeaderCell.ColumnSpan = 3;
                            THeaderCell.Text = "損耗率";
                        }
                        if (i >= 9 && i <= 11)
                        {
                            THeaderCell.Visible = false;
                        }
                    }
                    else
                    {
                        if (i >= 0 && i <= 7)
                        {
                            THeaderCell.Visible = false;
                        }
                    }
                    THeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    RowHeader.Cells.Add(THeaderCell);
                }
                gvData.Controls[0].Controls.AddAt(h, RowHeader);
            }
        }
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        InitialBoundData();
        SetTitle();
        ConfigRole();
        lblRunTime.Text = "";
        WorkOrder.strValue = "";
        gvData.DataSource = "";
        gvData.DataBind();
    }
}