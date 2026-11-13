using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
public partial class Material_ScrapRateQuery : System.Web.UI.Page
{
    const string strReportName = "部材超耗查詢";
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

            InitialBoundData("");
            SetTitle();
            Response.Expires = -1;
        }
    }
    protected void SetTitle()
    {
        this.Title = " 部材超耗查詢";
    }
    protected void InitialBoundData(string strPlant)
    {
        ModuleClass mc = new ModuleClass();
        string strFabID = mc.GetFABID(strPlant);

        DataTable dtPlant = mc.GetPlant("");
        Plant.DataTableValue = dtPlant;

        DataTable dtWorkType = mc.GetMWOTM(strFabID);
        WorkOrderType.DataTableValue = dtWorkType;

        /*因會依據多個Plant看數據，故產品不做PLANT的過濾*/
        //Product.GetTransMode = "";
        Product.GetPlant = "";
        DataTable dtSIZE = mc.GetSIZE(strFabID);
        Product.DataTableSIZEValue = dtSIZE;
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
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        QueryData("");
    }
    protected void QueryData(string strPlant)
    {
        ModuleClass mc = new ModuleClass();
        lblRunTime.Text = "";
        DateTime dtBegin = DateTime.Now;
        string strFabID = mc.GetFABID(strPlant);
        #region parameters
        DBConn dbConn = new DBConn();
        
        
        string strSql = "";
        string strFromDate = Date.FromDate.ToString();
        string strToDate = Date.ToDate.ToString();
        string strSqlPlant = mc.ListBoxToValue(Plant.ListBoxToValue);
        string strSqlWortType = mc.ListBoxToValue(WorkOrderType.ListBoxToValue);
        string strSqlSIZE = Product.DropDownListSIZE.SelectedValue.Trim().ToString();
        string strSqlProjectName = Product.DropDownListProjectName.SelectedValue.Trim().ToString();
        string strSqlProduct = mc.ListBoxToValue(Product.ListBoxToValue);

        strSql = strSql + " SELECT REPORTDATE,FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,FP_PART_NUM,SHOPCD,COMP_PART,COMP_M_GROUP";
        strSql = strSql + " ,OUTPUTQTY";
        strSql = strSql + " ,SCRAPQTY";
        strSql = strSql + " INTO #RPT_EIS FROM RPT_EIS";
        strSql = strSql + " WHERE 1=1 AND REPORTDATE >= '" + strFromDate + "' AND REPORTDATE <= '" + strToDate + "'";
        #region parameters [選擇條件過濾]
        if (strSqlPlant != "")//SHOP
        {
            strSql = strSql + " AND SHOPCD IN(" + strSqlPlant + ")";
        }
        if (WorkOrder.strValue != "")//工單
        {
            strSql = strSql + " AND WONBR = '" + WorkOrder.strValue + "'";
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
                    strSql = strSql + " AND SIZEID ='" + strSqlSIZE + "';";
                }
            }
        }
        #endregion

        strSql = strSql + " SELECT CASE WHEN T1.FABID='B' THEN 'WE01' ELSE '' END AS FABID,T1.SHOPCD,T1.PRODGROUP,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,T1.FP_PART_NUM,T1.COMP_M_GROUP,T1.COMP_PART";
        strSql = strSql + " ,SUM(T1.SCRAPQTY) AS SCRAP_QTY";
        strSql = strSql + " ,SUM(T1.OUTPUTQTY) AS OUT_QTY";
        strSql = strSql + " ,CASE WHEN SUM(T1.OUTPUTQTY) = 0 THEN 0 ELSE  SUM(T1.OUTPUTQTY + T1.SCRAPQTY) / SUM(T1.OUTPUTQTY) END AS ScrapRate";
        strSql = strSql + " FROM #RPT_EIS T1";
        strSql = strSql + " GROUP BY T1.FABID,T1.SHOPCD,T1.PRODGROUP,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,T1.FP_PART_NUM,T1.COMP_M_GROUP,T1.COMP_PART";
        strSql = strSql + " ORDER BY T1.FABID,CASE T1.SHOPCD WHEN 'CELL2' THEN 1 WHEN 'MODULE' THEN 2 ELSE 3 END";
        strSql = strSql + " ,CASE PRODGROUP WHEN 'Alpine' THEN 1 WHEN 'Audi' THEN 2 WHEN 'BMW' THEN 3 WHEN 'Bosch' THEN 4 WHEN 'Continental' THEN 5 ELSE 30 END";
        strSql = strSql + " ,CASE WHEN ISNULL(T1.PROJECT_NAME,'') = 'Total' THEN 1 ELSE 0 END";
        strSql = strSql + " ,ISNULL(T1.PROJECT_NAME,'')";
        strSql = strSql + " ,T1.PRODID";
        strSql = strSql + " ,CASE COMP_M_GROUP WHEN 'PANEL' THEN 1 WHEN '26' THEN 2 WHEN 'BC' THEN 3 WHEN 'BF' THEN 4 WHEN 'BL' THEN 5 WHEN 'DF' THEN 6 WHEN 'DT' THEN 7 WHEN 'FP' THEN 8 ELSE 99 END";
        strSql = strSql + " ,T1.COMP_M_GROUP;";
        DataTable dtQuery = dbConn.GetDataSetBySql(strSql).Tables[0];   
        #endregion
        
        if (dtQuery.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
        gvData.DataSource = dtQuery;
        gvData.DataBind();
        
        mc.GridViewOnMouse(gvData);

        /*合併欄位,要放在處理欄位雙擊時間之後處理*/
        for (int i = 0; i < 2; i++)
        {
            mc.GroupRows(gvData, i);
        }

        DateTime dtEnd = DateTime.Now;
        TimeSpan tsDiff = dtEnd - dtBegin;
        lblRunTime.Text = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";

        /*使用記錄，每隻報表必須要增加*/
        mc.RecordLogInfo(strFabID, "UnLogin", Session.SessionID.ToString(), "Query","", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");
        SetTitle();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void Plant_SelectedIndexChanged()
    {
        InitialBoundData("");
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        InitialBoundData("");
        SetTitle();
        lblRunTime.Text = "";
        WorkOrder.strValue = "";
        gvData.DataSource = "";
        gvData.DataBind();
    }
}