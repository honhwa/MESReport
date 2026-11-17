using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Material_RPT_EIS : System.Web.UI.Page
{
    const string strReportName = "報廢率數據查詢";
    protected void Page_Init(object sender, EventArgs e)
    {
        PlantSelect.GetDropDownList.AutoPostBack = true;
        PlantSelect.GetDropDownList.SelectedIndexChanged += new EventHandler(ddlPlantSelect_SelectedIndexChanged);
    }
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
            SetTitle();
            ConfigRole();

            DataTable dtPlant = mc.GetPlant("FABID");
            PlantSelect.DataTableValue = dtPlant;

            DataTable dtWorkOrder = mc.GetMWOTM(PlantSelect.strValue);
            WorkOrderType.DataTableValue = dtWorkOrder;

            DataTable dtSIZE = mc.GetSIZE(PlantSelect.strValue);
            Product.DataTableSIZEValue = dtSIZE;

            Response.Expires = -1;
        }
    }
    protected void ddlPlantSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtWorkOrder = mc.GetMWOTM(PlantSelect.strValue);
        WorkOrderType.DataTableValue = dtWorkOrder;

        Product.GetPlant = PlantSelect.GetDropDownList.SelectedItem.Text;

        DataTable dtSIZE = mc.GetSIZE(PlantSelect.strValue);
        Product.DataTableSIZEValue = dtSIZE;
    }
    protected void SetTitle()
    {
        this.Title = strReportName;
    }
    protected void ConfigRole()
    {
        //ModuleClass mc = new ModuleClass();
        //if (mc.GetRole("EIS_SP", tbNTName.Text) == false) { hlRunSP.Visible = false; } else { hlRunSP.Visible = true; }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            ModuleClass mc = new ModuleClass();
            lblRunTime.Text = "";
            DateTime dtBegin = DateTime.Now;
            for (int i = 0; i < gvData.Columns.Count; i++)
            {
                gvData.Columns[i].Visible = true;
            }
            if (REPORTDATE.Checked == false) { gvData.Columns[0].Visible = false; }
            if (SHOPCD.Checked == false) { gvData.Columns[2].Visible = false; }
            if (PROJECT_NAME.Checked == false) { gvData.Columns[6].Visible = false; }
            if (PRODGROUP.Checked == false) { gvData.Columns[7].Visible = false; }
            if (SIZEID.Checked == false) { gvData.Columns[8].Visible = false; }
            if (SHOP_CODE.Checked == false) { gvData.Columns[9].Visible = false; }
            if (WONBR.Checked == false) { gvData.Columns[10].Visible = false; }
            if (WOTYPE.Checked == false) { gvData.Columns[11].Visible = false; }
            if (WORKCTR.Checked == false) { gvData.Columns[12].Visible = false; }
            if (DEFECTCODE.Checked == false) { gvData.Columns[13].Visible = false; }
            if (DEFECTCODE.Checked == false) { gvData.Columns[14].Visible = false; }
            if (SOURCE_FAB.Checked == false) { gvData.Columns[20].Visible = false; }
            if (ScatteredMaterial.Checked == false) { gvData.Columns[21].Visible = false; } 
            
            string strSql="";
            string strFabID = PlantSelect.strValue;
            string strFromDate = Date.FromDate.ToString();
            string strToDate = Date.ToDate.ToString();
            DBConn dbConn = new DBConn();

            string strSqlWortType = mc.ListBoxToValue(WorkOrderType.ListBoxToValue);
            string strSqlSIZE = Product.DropDownListSIZE.SelectedValue.Trim().ToString();
            string strSqlProjectName = Product.DropDownListProjectName.SelectedValue.Trim().ToString();
            string strSqlProduct = mc.ListBoxToValue(Product.ListBoxToValue);

            strSql = strSql + " SELECT * INTO #RPT_EIS FROM dbo.RPT_EIS WITH(NOLOCK)";
            #region parameters [選擇條件過濾]
            strSql = strSql + " WHERE FABID = '" + strFabID + "' AND REPORTDATE >= '" + strFromDate + "' AND REPORTDATE <= '" + strToDate + "'";
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
                strSql = strSql + " AND PRODID IN(" + strSqlProduct + ")";
            }
            else 
            {
                if (strSqlProjectName != "")//型號
                {
                    strSql = strSql + " AND PROJECT_NAME ='" + strSqlProjectName + "'";
                }
                else 
                {
                    if (strSqlSIZE!="")//尺寸
                    {
                        strSql = strSql + " AND SIZEID ='" + strSqlSIZE + "' ";
                    }
                }
            }
            #endregion

            strSql = strSql + " Select FABID,REPORTDATE,PRODID,TP_PART_NUM,FP_PART_NUM,PROJECT_NAME,PRODGROUP,SIZEID,SHOP_CODE";
            strSql = strSql + " ,WONBR,WOTYPE,WORKCTR,DEFECTCODE,DEFECTDESC";
            strSql = strSql + " ,SHOPCD,ScatteredMaterial,COMP_PART,COMP_M_GROUP,OUTPUTQTY,SCRAPQTY,PRE_SCRAPQTY,SOURCE_FAB";
            strSql = strSql + " From(";
            strSql = strSql + " SELECT CASE WHEN T1.FABID = 'B' THEN 'WE01' END AS FABID";
            if (REPORTDATE.Checked == true) { strSql = strSql + " ,CONVERT(VARCHAR(10),T1.REPORTDATE,120) as REPORTDATE"; } else { strSql = strSql + " ,'' as REPORTDATE"; }
            strSql = strSql + " ,T1.PRODID";
            strSql = strSql + " ,T1.TP_PART_NUM";
            strSql = strSql + " ,T1.FP_PART_NUM";
            if (PROJECT_NAME.Checked == true) { strSql = strSql + " ,T1.PROJECT_NAME"; } else { strSql = strSql + " ,'' as PROJECT_NAME"; }
            if (PRODGROUP.Checked == true) { strSql = strSql + " ,T1.PRODGROUP"; } else { strSql = strSql + " ,'' as PRODGROUP"; }
            if (SIZEID.Checked == true) { strSql = strSql + " ,T1.SIZEID"; } else { strSql = strSql + " ,'' as SIZEID"; }
            if (SHOP_CODE.Checked == true) { strSql = strSql + " ,T1.SHOP_CODE"; } else { strSql = strSql + " ,'' as SHOP_CODE"; }
            if (WONBR.Checked == true) { strSql = strSql + " ,T1.WONBR"; } else { strSql = strSql + " ,'' as WONBR"; }
            if (WOTYPE.Checked == true) { strSql = strSql + " ,T1.WOTYPE"; } else { strSql = strSql + " ,'' as WOTYPE"; }
            if (WORKCTR.Checked == true) { strSql = strSql + " ,T1.WORKCTR"; } else { strSql = strSql + " ,'' as WORKCTR"; }
            if (DEFECTCODE.Checked == true) { strSql = strSql + " ,T1.DEFECTCODE"; } else { strSql = strSql + " ,'' as DEFECTCODE"; }
            if (DEFECTCODE.Checked == true) { strSql = strSql + " ,REPLACE(REPLACE(ISNULL(T1.DEFECTDESC,''),CHAR(10),''),CHAR(13),'') as DEFECTDESC"; } else { strSql = strSql + " ,'' as DEFECTDESC"; }
            if (SHOPCD.Checked == true) { strSql = strSql + " ,T1.SHOPCD"; } else { strSql = strSql + " ,'' as SHOPCD"; }
            if (ScatteredMaterial.Checked == true) { strSql = strSql + " ,CASE WHEN T1.DEFECTCODE LIKE 'R%' THEN 'Y' ELSE '' END AS ScatteredMaterial"; } else { strSql = strSql + " ,'' as ScatteredMaterial"; }
            strSql = strSql + " ,T1.COMP_PART";
            strSql = strSql + " ,T1.COMP_M_GROUP";
            strSql = strSql + " ,SUM(T1.OUTPUTQTY) AS OUTPUTQTY";
            strSql = strSql + " ,SUM(T1.SCRAPQTY) AS SCRAPQTY";
            strSql = strSql + " ,SUM(0.01 * ISNULL(T1.RATE,0) * T1.SCRAPQTY) AS PRE_SCRAPQTY";
            if (SOURCE_FAB.Checked == true) { strSql = strSql + " ,T1.SOURCE_FAB"; } else { strSql = strSql + " ,'' as SOURCE_FAB"; }
            strSql = strSql + " FROM #RPT_EIS T1 WITH(NOLOCK)";
            strSql = strSql + " WHERE T1.FABID='" + strFabID + "' ";
            strSql = strSql + " GROUP BY CASE WHEN T1.FABID = 'B' THEN 'WE01' END";
            if (REPORTDATE.Checked == true) { strSql = strSql + " ,CONVERT(VARCHAR(10),T1.REPORTDATE,120)"; }
            strSql = strSql + " ,T1.PRODID";
            strSql = strSql + " ,T1.TP_PART_NUM";
            strSql = strSql + " ,T1.FP_PART_NUM";
            if (PROJECT_NAME.Checked == true) { strSql = strSql + " ,T1.PROJECT_NAME"; }
            if (PRODGROUP.Checked == true) { strSql = strSql + " ,T1.PRODGROUP"; }
            if (SIZEID.Checked == true) { strSql = strSql + " ,T1.SIZEID"; }
            if (SHOP_CODE.Checked == true) { strSql = strSql + " ,T1.SHOP_CODE"; }
            if (WONBR.Checked == true) { strSql = strSql + " ,T1.WONBR"; }
            if (WOTYPE.Checked == true) { strSql = strSql + " ,T1.WOTYPE"; }
            if (WORKCTR.Checked == true) { strSql = strSql + " ,T1.WORKCTR"; }
            if (DEFECTCODE.Checked == true) { strSql = strSql + " ,T1.DEFECTCODE"; }
            if (DEFECTCODE.Checked == true) { strSql = strSql + " ,ISNULL(T1.DEFECTDESC,'')"; }
            if (SHOPCD.Checked == true) { strSql = strSql + " ,T1.SHOPCD"; }
            if (ScatteredMaterial.Checked == true) { strSql = strSql + " ,CASE WHEN T1.DEFECTCODE LIKE 'R%' THEN 'Y' ELSE '' END"; }
            strSql = strSql + " ,T1.COMP_PART";
            strSql = strSql + " ,T1.COMP_M_GROUP";
            if (SOURCE_FAB.Checked == true) { strSql = strSql + " ,T1.SOURCE_FAB"; }
            strSql = strSql + " )t";
            strSql = strSql + " Order By FABID,REPORTDATE";
            strSql = strSql + " ,CASE WHEN SHOPCD = 'CELL2' THEN 1 WHEN SHOPCD = 'MODULE' THEN 2 ELSE 9 END ";
            strSql = strSql + " ,CASE PRODGROUP WHEN 'Alpine' THEN 1 WHEN 'Audi' THEN 2 WHEN 'BMW' THEN 3 WHEN 'Bosch' THEN 4 WHEN 'Continental' THEN 5 ELSE 30 END ";
            strSql = strSql + " ,PROJECT_NAME,PRODID";
            strSql = strSql + " ,WOTYPE,WONBR";
            strSql = strSql + " ,WORKCTR,DEFECTCODE";
            strSql = strSql + " ,CASE COMP_M_GROUP WHEN 'PANEL' THEN 1 WHEN '26' THEN 2 WHEN 'BC' THEN 3 WHEN 'BF' THEN 4 WHEN 'BL' THEN 5 WHEN 'DF' THEN 6 WHEN 'DT' THEN 7 WHEN 'FP' THEN 8 ELSE 99 END";
            strSql = strSql + " ";
         
            DataTable dtQuery = dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dtQuery.Rows.Count == 0)
            {
                gvData.DataSource = "";
                gvData.DataBind();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查無資料！');</script>");
                return;
            }

            DateTime dtEnd = DateTime.Now;
            TimeSpan tsDiff = dtEnd - dtBegin;
            lblRunTime.Text = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";
            /*放在匯出之前，否則不會記錄時間*/
            mc.RecordLogInfo(strFabID, "UnLogin", Session.SessionID.ToString(), "Query", "", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");

            if (dtQuery.Rows.Count > 10000)
            {
                gvData.DataSource = "";
                gvData.DataBind();
                QueryToExcel(dtQuery);
                return;
            }
            else
            {
                gvData.DataSource = dtQuery;
                gvData.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryToExcel(DataTable dtData)
    {
        try
        {
            this.EnableViewState = false;
            ModuleClass mc = new ModuleClass();
            if (REPORTDATE.Checked == false) { dtData.Columns.Remove("REPORTDATE"); } else { dtData.Columns["REPORTDATE"].ColumnName = "日期"; }
            dtData.Columns["FABID"].ColumnName = "廠別";
            dtData.Columns["SHOPCD"].ColumnName = "SHOPCD";
            dtData.Columns["PRODID"].ColumnName = "產品號碼";
            dtData.Columns["TP_PART_NUM"].ColumnName = "TP產品13NC";
            dtData.Columns["FP_PART_NUM"].ColumnName = "FP產品13NC";
            if (PROJECT_NAME.Checked == false) { dtData.Columns.Remove("PROJECT_NAME"); } else { dtData.Columns["PROJECT_NAME"].ColumnName = "PROJECT_NAME"; }
            if (PRODGROUP.Checked == false) { dtData.Columns.Remove("PRODGROUP"); } else { dtData.Columns["PRODGROUP"].ColumnName = "客戶名稱"; }
            if (SIZEID.Checked == false) { dtData.Columns.Remove("SIZEID"); } else { dtData.Columns["SIZEID"].ColumnName = "尺寸"; }
            if (SHOP_CODE.Checked == false) { dtData.Columns.Remove("SHOP_CODE"); } else { dtData.Columns["SHOP_CODE"].ColumnName = "料號段別"; }
            if (WONBR.Checked == false) { dtData.Columns.Remove("WONBR"); } else { dtData.Columns["WONBR"].ColumnName = "工單"; }
            if (WOTYPE.Checked == false) { dtData.Columns.Remove("WOTYPE"); } else { dtData.Columns["WOTYPE"].ColumnName = "工單類型"; }
            if (WORKCTR.Checked == false) { dtData.Columns.Remove("WORKCTR"); } else { dtData.Columns["WORKCTR"].ColumnName = "站點"; }
            if (DEFECTCODE.Checked == false) { dtData.Columns.Remove("DEFECTCODE"); } else { dtData.Columns["DEFECTCODE"].ColumnName = "不良代碼"; }
            if (DEFECTCODE.Checked == false) { dtData.Columns.Remove("DEFECTDESC"); } else { dtData.Columns["DEFECTDESC"].ColumnName = "不良代碼描述"; }
            if (ScatteredMaterial.Checked == false) { dtData.Columns.Remove("ScatteredMaterial"); } else { dtData.Columns["ScatteredMaterial"].ColumnName = "零散物料標識"; }
            dtData.Columns["COMP_PART"].ColumnName = "COMP_PART";
            dtData.Columns["COMP_M_GROUP"].ColumnName = "COMP_M_GROUP";
            dtData.Columns["OUTPUTQTY"].ColumnName = "產出數量";
            dtData.Columns["SCRAPQTY"].ColumnName = "報廢數量";
            dtData.Columns["PRE_SCRAPQTY"].ColumnName = "前廠責報廢數量";
            if (SOURCE_FAB.Checked == false) { dtData.Columns.Remove("SOURCE_FAB"); } else { dtData.Columns["SOURCE_FAB"].ColumnName = "來源廠別"; }

            mc.ExportExcel(dtData);
        }
        catch
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        this.EnableViewState = false;
        ModuleClass mc = new ModuleClass();
        mc.ExportExcel(gvData);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        WorkOrder.strValue = "";
        lblRunTime.Text = "";
        gvData.DataSource = "";
        gvData.DataBind();
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        }
    }
}