using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
public partial class Material_ScrapRate : System.Web.UI.Page
{
    const string strReportName = "部材耗損率";
    const string strReportNameCost = "部材報廢率";
    const string strCost = "Cost";
    const string CstrPEPlan = "PEPlan";
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
            cbShowPartID.Checked = false;
            cbShowPartID.Visible = false;
            ModuleClass mc = new ModuleClass();
            string strNTName = "", strPCIP = "", strPCName = "";
            mc.GetLoginInfo(out strNTName, out strPCIP, out strPCName);
            tbNTName.Text = strNTName;

            //if (mc.GetMPCIPM(HttpContext.Current.Request.Url.AbsolutePath.ToString(), strPCIP) == false)
            //{
            //    Response.Redirect("../ShowMessage.aspx");
            //    return;
            //}

            tbReportName.Text = "";
            if (Request.QueryString["Name"] != null) { tbReportName.Text = this.Request.QueryString["Name"].ToString(); }
            
            ChooseTransMode();
            SetTitle();
            ConfigRole();
            Response.Expires = -1;
        }
    }
    protected void SetTitle()
    {
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            this.Title = " 部材報廢率";
            cbShowPartID.Visible = false;
        }
        else
        {
            this.Title = " 部材耗損率";
            cbShowPartID.Visible = true;
        }
    }
    protected void InitialBoundData(string strPlant, string strTransMode)
    {
        ModuleClass mc = new ModuleClass();
        string strFabID = mc.GetFABID(strPlant);

        //if (rblMergeModule.Items[0].Selected == true)
        //{
            DataTable dtPlant = mc.GetPlant("");
            Plant.DataTableValue = dtPlant;
        //}
        //if (rblMergeModule.Items[1].Selected == true)
        //{
        //    DataTable dtPlant = mc.GetPlant("BEOL&LCM+LAM");
        //    Plant.DataTableValue = dtPlant;
        //}
        DataTable dtWorkType = mc.GetMWOTM(strFabID);
        WorkOrderType.DataTableValue = dtWorkType;

        /*因會依據多個Plant看數據，故產品不做PLANT的過濾*/
        //Product.GetPlant = strPlant;
        //DataTable dtSize = mc.GetSize(strFabID, strPlant, strTransMode);
        Product.GetPlant = "";
        DataTable dtSIZE = mc.GetSIZE(strFabID);
        Product.DataTableSIZEValue = dtSIZE;
    }
    protected void ConfigRole()
    {
        ModuleClass mc = new ModuleClass();
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            if (mc.GetRole("Cost", tbNTName.Text) == false) { hlCost.Visible = false; } else { hlCost.Visible = true; }
            if (mc.GetUserGroupCode("Cost", tbNTName.Text) == false) { hlLimitCost.Visible = false; } else { hlLimitCost.Visible = true; }
        }
        else
        {
            if (mc.GetRole("PEPlan", tbNTName.Text) == false) { hlPEPlan.Visible = false; } else { hlPEPlan.Visible = true; }
            if (mc.GetUserGroupCode("PEPlan", tbNTName.Text) == false) { hlLimitPEPlan.Visible = false; } else { hlLimitPEPlan.Visible = true; }
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strTransMode = "";
        QueryData("", strTransMode);
    }
    protected void QueryData(string strPlant, string strTransMode)
    {
        ModuleClass mc = new ModuleClass();
        DBConn dbConn = new DBConn();
        lblRunTime.Text = "";
        DateTime dtBegin = DateTime.Now;
        string strFabID = mc.GetFABID(strPlant);
        gvData.Columns[5].HeaderText = "TP";
        gvData.Columns[7].HeaderText = "物料料號";
        if (cbShowPartID.Checked == false)
        {
            gvData.Columns[5].HeaderText = "False";
            gvData.Columns[7].HeaderText = "False";
        }
        gvData.Columns[5].Visible = true;
        gvData.Columns[7].Visible = true;

        #region parameters
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

        strSql = strSql + " SELECT r.FABID,r.REPORTDATE,r.REPORTYEAR,r.REPORTMONTH,R.PRODGROUP,r.SIZEID,r.PROJECT_NAME,r.PRODID,r.TP_PART_NUM,r.SHOPCD,r.COMP_PART,r.COMP_M_GROUP";
        strSql = strSql + " ,SUM(r.OutPutQTY) AS OUT_QTY";
        strSql = strSql + " ,SUM(r.ScrapQTY) AS SCRAP_QTY";
        strSql = strSql + " INTO #RPT_EIS_SUM FROM dbo.RPT_EIS AS r WITH(NOLOCK)";
        strSql = strSql + " WHERE r.ReportDate BETWEEN '" + strBDate_M5 + "' AND '" + strEDate_M0 + "'";
        #region parameters [選擇條件過濾]
        string strWoType = "", strProdID = "",strSIZE = "",strProjectName = "";
        string strSqlPlant = mc.ListBoxToValue(Plant.ListBoxToValue);
        string strSqlWortType = mc.ListBoxToValue(WorkOrderType.ListBoxToValue);
        string strSqlSIZE = Product.DropDownListSIZE.SelectedValue.Trim().ToString();
        string strSqlProjectName = Product.DropDownListProjectName.SelectedValue.Trim().ToString();
        string strSqlProduct = mc.ListBoxToValue(Product.ListBoxToValue);
        strWoType=strSqlWortType.Replace("'", "");;
        strSIZE = strSqlSIZE;
        strProjectName=strSqlProjectName;
        strProdID = strSqlProduct.Replace("'", "");
        if (strSqlPlant != "")//SHOPCD
        {
            strSql = strSql + " AND r.SHOPCD IN(" + strSqlPlant + ") ";
        }
        if (strSqlWortType != "")//工單類型
        {
            strSql = strSql + " AND r.WOTYPE IN(" + strSqlWortType + ")";
        }
        if (strSqlProduct != "")//TP料號
        {
            strSql = strSql + " AND r.PRODID IN(" + strSqlProduct + ")";
        }
        else
        {
            if (strSqlProjectName != "")//型號
            {
                strSql = strSql + " AND r.PROJECT_NAME ='" + strSqlProjectName + "'";
            }
            else
            {
                if (strSqlSIZE != "")//尺寸
                {
                    strSql = strSql + " AND r.SIZEID ='" + strSqlSIZE + "'";
                }
            }
        }
        #endregion
        strSql = strSql + " GROUP BY r.FABID,r.REPORTDATE,r.REPORTYEAR,r.REPORTMONTH,R.PRODGROUP,r.SIZEID,r.PROJECT_NAME,r.PRODID,r.TP_PART_NUM,r.SHOPCD,r.COMP_PART,r.COMP_M_GROUP;";

        /*查詢指定時間段的數據*/
        strSql = strSql + " SELECT CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID";
        strSql = strSql + " ,T1.REPORTDATE";
        strSql = strSql + " ,T1.REPORTYEAR";
        strSql = strSql + " ,T1.REPORTMONTH";
        strSql = strSql + " ,T1.SHOPCD AS PLANTID";
        strSql = strSql + " ,T1.PRODGROUP";
        strSql = strSql + " ,T1.SIZEID";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.COMP_M_GROUP";
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            strSql = strSql + " ,SUM(T1.OUT_QTY * ISNULL(TM.Unit_Cost, 1)) AS OUT_QTY";
            strSql = strSql + " ,SUM(T1.SCRAP_QTY * ISNULL(TM.Unit_Cost, 1)) AS SCRAP_QTY";
            strSql = strSql + " ,'' AS TP_PART_NUM";
            strSql = strSql + " ,'' AS COMP_PART";
        }
        else
        {
            strSql = strSql + " ,SUM(T1.OUT_QTY) AS OUT_QTY";
            strSql = strSql + " ,SUM(T1.SCRAP_QTY) AS SCRAP_QTY";
            if (cbShowPartID.Checked == true)
            {
                strSql = strSql + " ,T1.TP_PART_NUM";
                strSql = strSql + " ,T1.COMP_PART";
            }
            else
            {
                strSql = strSql + " ,'' AS TP_PART_NUM";
                strSql = strSql + " ,'' AS COMP_PART";
            }
        }
        strSql = strSql + " INTO #EIS_SUM";
        strSql = strSql + " FROM #RPT_EIS_SUM T1 WITH(NOLOCK)";
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            strSql = strSql + " LEFT JOIN MUCM TM WITH(NOLOCK) ON(T1.REPORTYEAR = TM.Year AND T1.REPORTMONTH = TM.Month AND T1.COMP_PART = TM.Material)";
        }
        strSql = strSql + " WHERE 1=1";

        strSql = strSql + " GROUP BY CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END";
        strSql = strSql + " ,T1.REPORTDATE";
        strSql = strSql + " ,T1.REPORTYEAR";
        strSql = strSql + " ,T1.REPORTMONTH";
        strSql = strSql + " ,T1.SHOPCD";
        strSql = strSql + " ,T1.PRODGROUP";
        strSql = strSql + " ,T1.SIZEID";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.COMP_M_GROUP";
        if (tbReportName.Text.ToUpper() == strCost.ToUpper()) { } else { if (cbShowPartID.Checked == true) { strSql = strSql + " ,T1.TP_PART_NUM,T1.COMP_PART"; } }
        strSql = strSql + ";";

        /*定義排序--按照近30天的PANEL的OUTQTY從大到小排序*/
        strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,ROW_NUMBER()OVER(ORDER BY SUM(OUT_QTY) DESC) AS SORTID";
        strSql = strSql + " INTO #SORT";
        strSql = strSql + " FROM #EIS_SUM WHERE REPORTDATE >= GETDATE() - 30 AND COMP_M_GROUP = 'PANEL' GROUP BY FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME;";
        #region parameters
        strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP";
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
        strSql = strSql + " ,TP_PART_NUM,COMP_PART";
        strSql = strSql + " INTO #Cost";
        strSql = strSql + " FROM #EIS_SUM";
        strSql = strSql + " GROUP BY FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,TP_PART_NUM,COMP_PART;";
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            /*計算各型號的所有材料的加總Out金額 & Scrap金額*/
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP";
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
            strSql = strSql + " ,TP_PART_NUM,COMP_PART";
            strSql = strSql + " INTO #TotalCost";
            strSql = strSql + " FROM #EIS_SUM";
            strSql = strSql + " GROUP BY FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,TP_PART_NUM,COMP_PART;";
        }
        #endregion
        /*如下默認值為 -1代表是產出為0，報廢不為0； -2代表產出為0，報廢為0，*/
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            #region parameters [Cost計算SQL]
            int intCnt = 5;
            for (int i = 0; i < intCnt; i++)
            {
                if (i == 0) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME,A.COMP_M_GROUP"; }/*計算天匯總數據*/
                if (i == 1) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME,'合計' AS COMP_M_GROUP"; }/*計算月By ProjectName匯總數據*/
                if (i == 2) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,'合計' AS PROJECT_NAME,'' AS COMP_M_GROUP"; }/*計算天By SIZEID匯總數據*/
                if (i == 3) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,'合計' AS SIZEID,'' AS PROJECT_NAME,'' AS COMP_M_GROUP"; }/*計算天By PRODGOUP匯總數據*/
                if (i == 4) { strSql = strSql + " SELECT A.FABID,A.PLANTID,'合計' AS PRODGROUP,'' AS SIZEID,'' AS PROJECT_NAME,'' AS COMP_M_GROUP"; }/*計算天By Shop匯總數據*/
                if (i != 0)
                {
                    strSql = strSql + " ," + i * 1000 + " AS SORTID";   /*放大到1000以後排序*/
                }
                else
                {
                    strSql = strSql + " ,ISNULL(S.SORTID,999) AS SORTID";   /*非最近30天生產的產品 默認排序為999*/
                }
                if (i == 0)
                {
                    strSql = strSql + " ,CASE WHEN SUM(B.M5OutQty) = 0 THEN CASE WHEN SUM(A.M5ScrapQty + B.M5OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M5ScrapQty + B.M5OutQty) / SUM(B.M5OutQty) END AS M5";
                    strSql = strSql + " ,CASE WHEN SUM(B.M4OutQty) = 0 THEN CASE WHEN SUM(A.M4ScrapQty + B.M4OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M4ScrapQty + B.M4OutQty) / SUM(B.M4OutQty) END AS M4";
                    strSql = strSql + " ,CASE WHEN SUM(B.M3OutQty) = 0 THEN CASE WHEN SUM(A.M3ScrapQty + B.M3OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M3ScrapQty + B.M3OutQty) / SUM(B.M3OutQty) END AS M3";
                    strSql = strSql + " ,CASE WHEN SUM(B.M2OutQty) = 0 THEN CASE WHEN SUM(A.M2ScrapQty + B.M2OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M2ScrapQty + B.M2OutQty) / SUM(B.M2OutQty) END AS M2";
                    strSql = strSql + " ,CASE WHEN SUM(B.M1OutQty) = 0 THEN CASE WHEN SUM(A.M1ScrapQty + B.M1OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M1ScrapQty + B.M1OutQty) / SUM(B.M1OutQty) END AS M1";
                    strSql = strSql + " ,CASE WHEN SUM(B.M0OutQty) = 0 THEN CASE WHEN SUM(A.M0ScrapQty + B.M0OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M0ScrapQty + B.M0OutQty) / SUM(B.M0OutQty) END AS M0";
                    strSql = strSql + " ,CASE WHEN SUM(B.W4OutQty) = 0 THEN CASE WHEN SUM(A.W4ScrapQty + B.W4OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W4ScrapQty + B.W4OutQty) / SUM(B.W4OutQty) END AS W4";
                    strSql = strSql + " ,CASE WHEN SUM(B.W3OutQty) = 0 THEN CASE WHEN SUM(A.W3ScrapQty + B.W3OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W3ScrapQty + B.W3OutQty) / SUM(B.W3OutQty) END AS W3";
                    strSql = strSql + " ,CASE WHEN SUM(B.W2OutQty) = 0 THEN CASE WHEN SUM(A.W2ScrapQty + B.W2OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W2ScrapQty + B.W2OutQty) / SUM(B.W2OutQty) END AS W2";
                    strSql = strSql + " ,CASE WHEN SUM(B.W1OutQty) = 0 THEN CASE WHEN SUM(A.W1ScrapQty + B.W1OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W1ScrapQty + B.W1OutQty) / SUM(B.W1OutQty) END AS W1";
                    strSql = strSql + " ,CASE WHEN SUM(B.D7OutQty) = 0 THEN CASE WHEN SUM(A.D7ScrapQty + B.D7OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D7ScrapQty + B.D7OutQty) / SUM(B.D7OutQty) END AS D7";
                    strSql = strSql + " ,CASE WHEN SUM(B.D6OutQty) = 0 THEN CASE WHEN SUM(A.D6ScrapQty + B.D6OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D6ScrapQty + B.D6OutQty) / SUM(B.D6OutQty) END AS D6";
                    strSql = strSql + " ,CASE WHEN SUM(B.D5OutQty) = 0 THEN CASE WHEN SUM(A.D5ScrapQty + B.D5OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D5ScrapQty + B.D5OutQty) / SUM(B.D5OutQty) END AS D5";
                    strSql = strSql + " ,CASE WHEN SUM(B.D4OutQty) = 0 THEN CASE WHEN SUM(A.D4ScrapQty + B.D4OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D4ScrapQty + B.D4OutQty) / SUM(B.D4OutQty) END AS D4";
                    strSql = strSql + " ,CASE WHEN SUM(B.D3OutQty) = 0 THEN CASE WHEN SUM(A.D3ScrapQty + B.D3OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D3ScrapQty + B.D3OutQty) / SUM(B.D3OutQty) END AS D3";
                    strSql = strSql + " ,CASE WHEN SUM(B.D2OutQty) = 0 THEN CASE WHEN SUM(A.D2ScrapQty + B.D2OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D2ScrapQty + B.D2OutQty) / SUM(B.D2OutQty) END AS D2";
                    strSql = strSql + " ,CASE WHEN SUM(B.D1OutQty) = 0 THEN CASE WHEN SUM(A.D1ScrapQty + B.D1OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D1ScrapQty + B.D1OutQty) / SUM(B.D1OutQty) END AS D1";
                    strSql = strSql + " ,CASE WHEN SUM(B.M5ScrapQty) = 0 THEN 0 ELSE SUM(A.M5ScrapQty) / SUM(B.M5ScrapQty) END AS M5Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.M4ScrapQty) = 0 THEN 0 ELSE SUM(A.M4ScrapQty) / SUM(B.M4ScrapQty) END AS M4Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.M3ScrapQty) = 0 THEN 0 ELSE SUM(A.M3ScrapQty) / SUM(B.M3ScrapQty) END AS M3Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.M2ScrapQty) = 0 THEN 0 ELSE SUM(A.M2ScrapQty) / SUM(B.M2ScrapQty) END AS M2Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.M1ScrapQty) = 0 THEN 0 ELSE SUM(A.M1ScrapQty) / SUM(B.M1ScrapQty) END AS M1Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.M0ScrapQty) = 0 THEN 0 ELSE SUM(A.M0ScrapQty) / SUM(B.M0ScrapQty) END AS M0Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.W4ScrapQty) = 0 THEN 0 ELSE SUM(A.W4ScrapQty) / SUM(B.W4ScrapQty) END AS W4Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.W3ScrapQty) = 0 THEN 0 ELSE SUM(A.W3ScrapQty) / SUM(B.W3ScrapQty) END AS W3Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.W2ScrapQty) = 0 THEN 0 ELSE SUM(A.W2ScrapQty) / SUM(B.W2ScrapQty) END AS W2Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.W1ScrapQty) = 0 THEN 0 ELSE SUM(A.W1ScrapQty) / SUM(B.W1ScrapQty) END AS W1Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D7ScrapQty) = 0 THEN 0 ELSE SUM(A.D7ScrapQty) / SUM(B.D7ScrapQty) END AS D7Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D6ScrapQty) = 0 THEN 0 ELSE SUM(A.D6ScrapQty) / SUM(B.D6ScrapQty) END AS D6Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D5ScrapQty) = 0 THEN 0 ELSE SUM(A.D5ScrapQty) / SUM(B.D5ScrapQty) END AS D5Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D4ScrapQty) = 0 THEN 0 ELSE SUM(A.D4ScrapQty) / SUM(B.D4ScrapQty) END AS D4Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D3ScrapQty) = 0 THEN 0 ELSE SUM(A.D3ScrapQty) / SUM(B.D3ScrapQty) END AS D3Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D2ScrapQty) = 0 THEN 0 ELSE SUM(A.D2ScrapQty) / SUM(B.D2ScrapQty) END AS D2Rate";
                    strSql = strSql + " ,CASE WHEN SUM(B.D1ScrapQty) = 0 THEN 0 ELSE SUM(A.D1ScrapQty) / SUM(B.D1ScrapQty) END AS D1Rate";
                    if (i == 0) { strSql = strSql + " ,A.TP_PART_NUM,A.COMP_PART"; } else { strSql = strSql + " ,'' AS TP_PART_NUM,'' AS COMP_PART"; }
                    if (i == 0) { strSql = strSql + " INTO #DATE"; }
                    if (i == 1) { strSql = strSql + " INTO #DATE_ProjectName"; }
                    if (i == 2) { strSql = strSql + " INTO #DATE_SIZEID"; }
                    if (i == 3) { strSql = strSql + " INTO #DATE_PRODGROUP"; }
                    if (i == 4) { strSql = strSql + " INTO #DATE_Shop"; }
                    strSql = strSql + " FROM #Cost A";
                    strSql = strSql + " LEFT JOIN #TotalCost B ON(A.FABID=B.FABID AND A.PLANTID=B.PLANTID AND A.PRODGROUP=B.PRODGROUP AND A.SIZEID=B.SIZEID  AND A.PROJECT_NAME=B.PROJECT_NAME)";
                    strSql = strSql + " LEFT JOIN #SORT S ON (A.FABID=S.FABID AND A.PLANTID=S.PLANTID AND A.PRODGROUP=S.PRODGROUP AND A.SIZEID=S.SIZEID AND A.PROJECT_NAME=S.PROJECT_NAME)";
                }
                else
                {
                    strSql = strSql + " ,CASE WHEN SUM(A.M5OutQty) = 0 THEN CASE WHEN SUM(A.M5ScrapQty + A.M5OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M5ScrapQty + A.M5OutQty) / SUM(A.M5OutQty) END AS M5";
                    strSql = strSql + " ,CASE WHEN SUM(A.M4OutQty) = 0 THEN CASE WHEN SUM(A.M4ScrapQty + A.M4OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M4ScrapQty + A.M4OutQty) / SUM(A.M4OutQty) END AS M4";
                    strSql = strSql + " ,CASE WHEN SUM(A.M3OutQty) = 0 THEN CASE WHEN SUM(A.M3ScrapQty + A.M3OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M3ScrapQty + A.M3OutQty) / SUM(A.M3OutQty) END AS M3";
                    strSql = strSql + " ,CASE WHEN SUM(A.M2OutQty) = 0 THEN CASE WHEN SUM(A.M2ScrapQty + A.M2OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M2ScrapQty + A.M2OutQty) / SUM(A.M2OutQty) END AS M2";
                    strSql = strSql + " ,CASE WHEN SUM(A.M1OutQty) = 0 THEN CASE WHEN SUM(A.M1ScrapQty + A.M1OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M1ScrapQty + A.M1OutQty) / SUM(A.M1OutQty) END AS M1";
                    strSql = strSql + " ,CASE WHEN SUM(A.M0OutQty) = 0 THEN CASE WHEN SUM(A.M0ScrapQty + A.M0OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.M0ScrapQty + A.M0OutQty) / SUM(A.M0OutQty) END AS M0";
                    strSql = strSql + " ,CASE WHEN SUM(A.W4OutQty) = 0 THEN CASE WHEN SUM(A.W4ScrapQty + A.W4OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W4ScrapQty + A.W4OutQty) / SUM(A.W4OutQty) END AS W4";
                    strSql = strSql + " ,CASE WHEN SUM(A.W3OutQty) = 0 THEN CASE WHEN SUM(A.W3ScrapQty + A.W3OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W3ScrapQty + A.W3OutQty) / SUM(A.W3OutQty) END AS W3";
                    strSql = strSql + " ,CASE WHEN SUM(A.W2OutQty) = 0 THEN CASE WHEN SUM(A.W2ScrapQty + A.W2OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W2ScrapQty + A.W2OutQty) / SUM(A.W2OutQty) END AS W2";
                    strSql = strSql + " ,CASE WHEN SUM(A.W1OutQty) = 0 THEN CASE WHEN SUM(A.W1ScrapQty + A.W1OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.W1ScrapQty + A.W1OutQty) / SUM(A.W1OutQty) END AS W1";
                    strSql = strSql + " ,CASE WHEN SUM(A.D7OutQty) = 0 THEN CASE WHEN SUM(A.D7ScrapQty + A.D7OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D7ScrapQty + A.D7OutQty) / SUM(A.D7OutQty) END AS D7";
                    strSql = strSql + " ,CASE WHEN SUM(A.D6OutQty) = 0 THEN CASE WHEN SUM(A.D6ScrapQty + A.D6OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D6ScrapQty + A.D6OutQty) / SUM(A.D6OutQty) END AS D6";
                    strSql = strSql + " ,CASE WHEN SUM(A.D5OutQty) = 0 THEN CASE WHEN SUM(A.D5ScrapQty + A.D5OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D5ScrapQty + A.D5OutQty) / SUM(A.D5OutQty) END AS D5";
                    strSql = strSql + " ,CASE WHEN SUM(A.D4OutQty) = 0 THEN CASE WHEN SUM(A.D4ScrapQty + A.D4OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D4ScrapQty + A.D4OutQty) / SUM(A.D4OutQty) END AS D4";
                    strSql = strSql + " ,CASE WHEN SUM(A.D3OutQty) = 0 THEN CASE WHEN SUM(A.D3ScrapQty + A.D3OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D3ScrapQty + A.D3OutQty) / SUM(A.D3OutQty) END AS D3";
                    strSql = strSql + " ,CASE WHEN SUM(A.D2OutQty) = 0 THEN CASE WHEN SUM(A.D2ScrapQty + A.D2OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D2ScrapQty + A.D2OutQty) / SUM(A.D2OutQty) END AS D2";
                    strSql = strSql + " ,CASE WHEN SUM(A.D1OutQty) = 0 THEN CASE WHEN SUM(A.D1ScrapQty + A.D1OutQty) = 0 THEN -2 ELSE -1 END ELSE SUM(A.D1ScrapQty + A.D1OutQty) / SUM(A.D1OutQty) END AS D1";
                    strSql = strSql + " ,1 AS M5Rate";
                    strSql = strSql + " ,1 AS M4Rate";
                    strSql = strSql + " ,1 AS M3Rate";
                    strSql = strSql + " ,1 AS M2Rate";
                    strSql = strSql + " ,1 AS M1Rate";
                    strSql = strSql + " ,1 AS M0Rate";
                    strSql = strSql + " ,1 AS W4Rate";
                    strSql = strSql + " ,1 AS W3Rate";
                    strSql = strSql + " ,1 AS W2Rate";
                    strSql = strSql + " ,1 AS W1Rate";
                    strSql = strSql + " ,1 AS D7Rate";
                    strSql = strSql + " ,1 AS D6Rate";
                    strSql = strSql + " ,1 AS D5Rate";
                    strSql = strSql + " ,1 AS D4Rate";
                    strSql = strSql + " ,1 AS D3Rate";
                    strSql = strSql + " ,1 AS D2Rate";
                    strSql = strSql + " ,1 AS D1Rate";
                    if (i == 0) { strSql = strSql + " INTO #DATE"; }
                    if (i == 1) { strSql = strSql + " INTO #DATE_ProjectName"; }
                    if (i == 3) { strSql = strSql + " INTO #DATE_SIZEID"; }
                    if (i == 2) { strSql = strSql + " INTO #DATE_PRODGROUP"; }
                    if (i == 4) { strSql = strSql + " INTO #DATE_Shop"; }
                    strSql = strSql + " FROM #TotalCost A";
                    strSql = strSql + " LEFT JOIN #SORT S ON (A.FABID=S.FABID AND A.PLANTID=S.PLANTID AND A.PRODGROUP=S.PRODGROUP AND A.SIZEID=S.SIZEID AND A.PROJECT_NAME=S.PROJECT_NAME)";
                }
                if (i == 0) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME,A.COMP_M_GROUP,ISNULL(S.SORTID,999),A.TP_PART_NUM,A.COMP_PART;"; }
                if (i == 1) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME;"; }
                if (i == 2) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID;"; }
                if (i == 3) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP;"; }
                if (i == 4) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID;"; }
            }
            /*Table_0 計算數據*/
            strSql = strSql + " SELECT * FROM(";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,M5Rate,M4Rate,M3Rate,M2Rate,M1Rate,M0Rate,W4Rate,W3Rate,W2Rate,W1Rate,D7Rate,D6Rate,D5Rate,D4Rate,D3Rate,D2Rate,D1Rate";
            strSql = strSql + " FROM #DATE";
            //strSql = strSql + " UNION ALL";
            //strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            //strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            //strSql = strSql + " ,M5Rate,M4Rate,M3Rate,M2Rate,M1Rate,M0Rate,W4Rate,W3Rate,W2Rate,W1Rate,D7Rate,D6Rate,D5Rate,D4Rate,D3Rate,D2Rate,D1Rate";
            //strSql = strSql + " FROM #DATE_ProjectName";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,M5Rate,M4Rate,M3Rate,M2Rate,M1Rate,M0Rate,W4Rate,W3Rate,W2Rate,W1Rate,D7Rate,D6Rate,D5Rate,D4Rate,D3Rate,D2Rate,D1Rate";
            strSql = strSql + " FROM #DATE_SIZEID";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,M5Rate,M4Rate,M3Rate,M2Rate,M1Rate,M0Rate,W4Rate,W3Rate,W2Rate,W1Rate,D7Rate,D6Rate,D5Rate,D4Rate,D3Rate,D2Rate,D1Rate";
            strSql = strSql + " FROM #DATE_PRODGROUP";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,M5Rate,M4Rate,M3Rate,M2Rate,M1Rate,M0Rate,W4Rate,W3Rate,W2Rate,W1Rate,D7Rate,D6Rate,D5Rate,D4Rate,D3Rate,D2Rate,D1Rate";
            strSql = strSql + " FROM #DATE_Shop";
            strSql = strSql + " )T";
            strSql = strSql + " ORDER BY FABID";
            strSql = strSql + " ,CASE PLANTID WHEN '合計' THEN 1 ELSE 0 END";
            strSql = strSql + " ,CASE PLANTID WHEN 'CELL2' THEN 1 WHEN 'MODULE' THEN 2 ELSE 3 END";
            strSql = strSql + " ,CASE PRODGROUP WHEN '合計' THEN 1 ELSE 0 END";
            //strSql = strSql + " ,CASE PRODGROUP WHEN 'Alpine' THEN 1 WHEN 'Audi' THEN 2 WHEN 'BMW' THEN 3 WHEN 'Bosch' THEN 4 WHEN 'Continental' THEN 5 ELSE 30 END";
            strSql = strSql + " ,PRODGROUP";
            strSql = strSql + " ,CASE SIZEID WHEN '合計' THEN 1 ELSE 0 END";
            strSql = strSql + " ,SIZEID";
            strSql = strSql + " ,CASE PROJECT_NAME WHEN '合計' THEN 1 ELSE 0 END";
            strSql = strSql + " ,PROJECT_NAME";
            strSql = strSql + " ,SORTID";
            strSql = strSql + " ,CASE COMP_M_GROUP WHEN 'PANEL' THEN 1 WHEN '26' THEN 2 WHEN 'BC' THEN 3 WHEN 'BF' THEN 4 WHEN 'BL' THEN 5 WHEN 'DF' THEN 6 WHEN 'DT' THEN 7 WHEN 'FP' THEN 8 ELSE 99 END";
            strSql = strSql + " ,COMP_M_GROUP;";

            #endregion
        }
        else
        {
            #region parameters [Qty計算SQL]
            int intCnt = 5;
            for (int i = 0; i < intCnt; i++)
            {
                if (i == 0) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME,A.COMP_M_GROUP"; }/*計算天匯總數據*/
                if (i == 1) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME,'合計' AS COMP_M_GROUP"; }/*計算月By ProjectName匯總數據*/
                if (i == 2) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,'合計' AS PROJECT_NAME,'' AS COMP_M_GROUP"; }/*計算天By SIZEID匯總數據*/
                if (i == 3) { strSql = strSql + " SELECT A.FABID,A.PLANTID,A.PRODGROUP,'合計' AS SIZEID,'' AS PROJECT_NAME,'' AS COMP_M_GROUP"; }/*計算天By PRODGOUP匯總數據*/
                if (i == 4) { strSql = strSql + " SELECT A.FABID,A.PLANTID,'合計' AS PRODGROUP,'' AS SIZEID,'' AS PROJECT_NAME,'' AS COMP_M_GROUP"; }/*計算天By Shop匯總數據*/
                if (i != 0)
                {
                    strSql = strSql + " ," + i * 1000 + " AS SORTID";   /*放大到1000以後排序*/
                }
                else
                {
                    strSql = strSql + " ,ISNULL(S.SORTID,999) AS SORTID";   /*非最近30天生產的產品 默認排序為999*/
                }
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
                if (i == 0) { strSql = strSql + " ,A.TP_PART_NUM,A.COMP_PART"; } else { strSql = strSql + " ,'' AS TP_PART_NUM,'' AS COMP_PART"; }
                if (i == 0) { strSql = strSql + " INTO #DATE"; }
                if (i == 1) { strSql = strSql + " INTO #DATE_ProjectName"; }
                if (i == 2) { strSql = strSql + " INTO #DATE_SIZEID"; }
                if (i == 3) { strSql = strSql + " INTO #DATE_PRODGROUP"; }
                if (i == 4) { strSql = strSql + " INTO #DATE_Shop"; }
                strSql = strSql + " FROM #Cost A";
                strSql = strSql + " LEFT JOIN #SORT S ON (A.FABID=S.FABID AND A.PLANTID=S.PLANTID AND A.PRODGROUP=S.PRODGROUP AND A.SIZEID=S.SIZEID AND A.PROJECT_NAME=S.PROJECT_NAME)";
                if (i == 0) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME,A.COMP_M_GROUP,ISNULL(S.SORTID,999),TP_PART_NUM,COMP_PART;"; }
                if (i == 1) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID,A.PROJECT_NAME;"; }
                if (i == 2) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP,A.SIZEID;"; }
                if (i == 3) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID,A.PRODGROUP;"; }
                if (i == 4) { strSql = strSql + " GROUP BY A.FABID,A.PLANTID;"; }

            }
            /*Table_0 計算數據*/
            strSql = strSql + " SELECT * FROM(";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,TP_PART_NUM,COMP_PART";
            strSql = strSql + " FROM #DATE";
            //strSql = strSql + " UNION ALL";
            //strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            //strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            //strSql = strSql + " ,TP_PART_NUM,COMP_PART";
            //strSql = strSql + " FROM #DATE_ProjectName";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,TP_PART_NUM,COMP_PART";
            strSql = strSql + " FROM #DATE_SIZEID";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,TP_PART_NUM,COMP_PART";
            strSql = strSql + " FROM #DATE_PRODGROUP";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP,SORTID";
            strSql = strSql + " ,M5,M4,M3,M2,M1,M0,W4,W3,W2,W1,D7,D6,D5,D4,D3,D2,D1";
            strSql = strSql + " ,TP_PART_NUM,COMP_PART";
            strSql = strSql + " FROM #DATE_Shop";
            strSql = strSql + " )T";
            strSql = strSql + " ORDER BY FABID";
            strSql = strSql + " ,CASE PLANTID WHEN '合計' THEN 1 ELSE 0 END";
            strSql = strSql + " ,CASE PLANTID WHEN 'CELL2' THEN 1 WHEN 'MODULE' THEN 2 ELSE 3 END";
            strSql = strSql + " ,CASE PRODGROUP WHEN '合計' THEN 1 ELSE 0 END";
            //strSql = strSql + " ,CASE PRODGROUP WHEN 'Alpine' THEN 1 WHEN 'Audi' THEN 2 WHEN 'BMW' THEN 3 WHEN 'Bosch' THEN 4 WHEN 'Continental' THEN 5 ELSE 30 END";
            strSql = strSql + " ,PRODGROUP";
            strSql = strSql + " ,CASE SIZEID WHEN '合計' THEN 1 ELSE 0 END";
            strSql = strSql + " ,SIZEID";
            strSql = strSql + " ,CASE WHEN PROJECT_NAME = '合計' THEN 1 ELSE 0 END";
            strSql = strSql + " ,PROJECT_NAME";
            strSql = strSql + " ,SORTID"; 
            strSql = strSql + " ,TP_PART_NUM";
            strSql = strSql + " ,CASE COMP_M_GROUP WHEN 'PANEL' THEN 1 WHEN '26' THEN 2 WHEN 'BC' THEN 3 WHEN 'BF' THEN 4 WHEN 'BL' THEN 5 WHEN 'DF' THEN 6 WHEN 'DT' THEN 7 WHEN 'FP' THEN 8 ELSE 99 END";
            strSql = strSql + " ,COMP_M_GROUP,COMP_PART;";
            #endregion
        }
        /*Table_1 抓取PlanRate的數據*/
        strSql = strSql + " SELECT CAST([Year] AS VARCHAR(4)) + '-' + RIGHT('0'+ CAST([Month] AS VARCHAR(2)),2) AS PlanMonth,PlanType,PLANTID,PROJECT_NAME,PartType,PlanRate ";
        strSql = strSql + " FROM MPlanRateM ";
        strSql = strSql + " WHERE [Year] >= DATEPART(YEAR, '" + strBDate_M5 + "') AND [Year] <= DATEPART(YEAR, '" + strEDate_M0 + "')";
        strSql = strSql + " AND PlanType = '" + CstrPEPlan + "' ";
        if (strSqlPlant != "")
        {
            strSql = strSql + " AND PLANTID IN(" + strSqlPlant + ") ";
        }
        strSql = strSql + " ; ";

        /*Table_2 檢查是否有未設定ProjectName的料號*/
        strSql = strSql + " SELECT DISTINCT A.PROJECT_NAME FROM #EIS_SUM A";
        strSql = strSql + " WHERE ISNULL(A.PROJECT_NAME,'') = '';";

        /*Table_3 檢查是否有未設定單價的料號 For Cost*/
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            strSql = strSql + " SELECT DISTINCT CONVERT(VARCHAR(7), REPORTDATE,120) AS ReportMonth,COMP_PART,ISNULL(t2.Unit_Cost,0) AS UnitCost FROM #EIS_SUM T1";
            strSql = strSql + " LEFT JOIN dbo.MUCM T2 ON (DATEPART(YEAR,T1.REPORTDATE)=T2.[Year] AND DATEPART(MONTH,T1.REPORTDATE)=T2.[Month] AND T1.COMP_PART=T2.Material)";
            strSql = strSql + " WHERE T2.Unit_Cost IS NULL;";
        }
        
        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        #endregion parameters

        if (dsTmp.Tables[2].Rows.Count != 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('有" + dsTmp.Tables[2].Rows.Count.ToString() + "顆料號沒有維護ProjectName，系統默認不會呈現相關數據，為不影響計算，請與主檔維護人員確認！');</script>");
        }

        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            if (dsTmp.Tables[3].Rows.Count != 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('有" + dsTmp.Tables[3].Rows.Count.ToString() + "顆料號沒有維護價格，系統默認價格為0，為不影響計算，請經管盡快維護！');</script>");
            }
            gvDataCost.DataSource = dsTmp.Tables[0];
            gvDataCost.DataBind();
            if (dsTmp.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
            }
            else
            {
                GridViewFormatCost();
                gvDataCost.HeaderRow.Visible = false;
            }
            mc.GridViewOnMouse(gvDataCost);
        }
        else
        {
            gvData.DataSource = dsTmp.Tables[0];
            gvData.DataBind();
            if (dsTmp.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
            }
            else
            {
                GridViewFormat(dsTmp, strWoType, strProdID, dtDate.ToString("yyyy-MM-dd"), strProjectName);
                gvData.HeaderRow.Visible = false;
            }
            mc.GridViewOnMouse(gvData);
            if (cbShowPartID.Checked == false)
            {
                gvData.Columns[5].Visible = false;
                gvData.Columns[7].Visible = false;
            }
            else
            {
                gvData.Columns[5].Visible = true;
                gvData.Columns[7].Visible = true;
            }
        }
        DateTime dtEnd = DateTime.Now;
        TimeSpan tsDiff = dtEnd - dtBegin;
        lblRunTime.Text = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";

        /*使用記錄，每隻報表必須要增加*/
        if (tbReportName.Text.ToUpper() == strCost.ToUpper())
        {
            mc.RecordLogInfo(strFabID, "UnLogin", Session.SessionID.ToString(), "Query", "", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportNameCost, "SH", "");
        }
        else
        {
            mc.RecordLogInfo(strFabID, "UnLogin", Session.SessionID.ToString(), "Query", "", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");
        }
        SetTitle();
    }
    protected void GridViewFormat(DataSet dsTmp, string strWoType, string strProdID, string strMFGDate, string strProjectName)
    {
        /*處理GridView的格式*/
        ModuleClass mc = new ModuleClass();
        if (gvData.Rows.Count > 0)
        {

            /*GridView循環處理 無生產 和 有報廢數量的邏輯*/
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                string strGVFABID = Server.UrlEncode(gvData.Rows[i].Cells[0].Text.Replace("&nbsp;", "").ToString());
                string strGVPlant = Server.UrlEncode(gvData.Rows[i].Cells[1].Text.Replace("&nbsp;", "").ToString());
                string strGVPRODGROUP = Server.UrlEncode(gvData.Rows[i].Cells[2].Text.Replace("&nbsp;", "").ToString());
                string strGVSIZEID = Server.UrlEncode(gvData.Rows[i].Cells[3].Text.Replace("&nbsp;", "").ToString());
                string strGVProjectName = Server.UrlEncode(gvData.Rows[i].Cells[4].Text.Replace("&nbsp;", "").ToString());
                string strGVPartType = Server.UrlEncode(gvData.Rows[i].Cells[6].Text.Replace("&nbsp;", "").ToString());

                string strShowPart = "F";
                if (cbShowPartID.Checked == true) { strShowPart = "T"; } else { strShowPart = "F"; }
                if (gvData.Rows[i].Cells[2].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.FromArgb(153, 204, 255); }
                if (gvData.Rows[i].Cells[3].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.FromArgb(204, 153, 255); }
                if (gvData.Rows[i].Cells[4].Text.ToUpper() == "合計") { gvData.Rows[i].BackColor = Color.FromArgb(51, 204, 204); }

                gvData.Rows[i].Cells[4].Attributes.Clear();
                if (gvData.Rows[i].Cells[4].Text.Replace("&nbsp;", "") != "" && gvData.Rows[i].Cells[4].Text != "合計")/*ProjectName為空不給看明細*/
                {
                    gvData.Rows[i].Cells[4].Attributes.Add("style", "cursor: hand;");
                    gvData.Rows[i].Cells[4].Attributes.Add("ondblclick", "window.open('../Material/ScrapRateChart.aspx?FABID=" + strGVFABID + "&Plant=" + strGVPlant + "&ProjectName=" + strGVProjectName + "&MFGDT=" + strMFGDate + "&WoType=" + strWoType + "&ProdID=" + strProdID + "')");
                }
                for (int j = 8; j < gvData.Columns.Count; j++)    /*6個月+4周+7天*/
                {
                    if (gvData.Rows[i].Cells[2].Text.ToUpper() != "合計" && gvData.Rows[i].Cells[3].Text.ToUpper() != "合計" && gvData.Rows[i].Cells[4].Text.ToUpper() != "合計")
                    {
                        if (gvData.Rows[i].Cells[j].Text.ToString() != "-2.000" && gvData.Rows[i].Cells[j].Text.ToString() != "-1.000")
                        {
                            double dblPlanRate = mc.GetPlanRate(dsTmp.Tables[1], QueryMonth(j), strGVPlant, strGVProjectName, strGVPartType, "PEPlan");
                            //double dblPlanRate = 10000;
                            /*判斷是否有超PLAN的RATE，超了顯示紅色字體*/
                            if (Convert.ToDouble(gvData.Rows[i].Cells[j].Text.Trim().ToString()) >= dblPlanRate) { gvData.Rows[i].Cells[j].ForeColor = Color.Red; } else { gvData.Rows[i].Cells[j].ForeColor = Color.Black; }
                            if (gvData.Rows[i].Cells[4].Text.Replace("&nbsp;", "") != "" && gvData.Rows[i].Cells[4].Text != "合計")/*ProjectName為空不給看明細*/
                            {
                                string strDT = gvData.HeaderRow.Cells[j].Text.ToString();
                                gvData.Rows[i].Cells[j].Attributes.Add("style", "cursor: hand;");
                                gvData.Rows[i].Cells[j].Attributes.Add("ondblclick", "window.open('../Material/ScrapRateStation.aspx?FABID=" + strGVFABID + "&Plant=" + strGVPlant + "&ProjectName=" + strGVProjectName + "&PartType=" + strGVPartType + "&MFGDT=" + strMFGDate + "&DTValue=" + strDT + "&WoType=" + strWoType + "&ProdID=" + strProdID + "&ShowPart=" + strShowPart + "')");
                            }
                        }
                    }
                    if (gvData.Rows[i].Cells[j].Text.ToString() == "-2.000") { gvData.Rows[i].Cells[j].Text = ""; gvData.Rows[i].Cells[j].ToolTip = "提示：當天無數據"; }
                    if (gvData.Rows[i].Cells[j].Text.ToString() == "-1.000") { gvData.Rows[i].Cells[j].Text = "*0.000"; gvData.Rows[i].Cells[j].ToolTip = "提示：當天產出數為0，報廢數不為0"; }
                }
            }
            /*合併欄位,要放在處理欄位雙擊時間之後處理*/
            for (int i = 0; i < 5; i++) 
            {
                mc.GroupRows(gvData, i);
            }
        }
    }
    protected string QueryMonth(int intCol)
    {
        string strMonth = "";
        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);
        if (intCol >= 8 && intCol <= 24)
        {
            strMonth = dtDate.AddMonths(1).ToString("yyyy-MM");
        }

        if (intCol >= 18 && intCol <= 24)
        {
            strMonth = dtDate.AddDays(intCol - 24).ToString("yyyy-MM");
        }
        if (intCol >= 14 && intCol <= 17)
        {
            strMonth = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - (17 - intCol) * 7).ToString("yyyy-MM");
        }
        if (intCol >= 8 && intCol <= 13)
        {
            strMonth = dtDate.AddMonths(intCol - 13).ToString("yyyy-MM");
        }
        return strMonth;
    }
    protected void GridViewFormatCost()
    {
        /*處理GridView的格式*/
        ModuleClass mc = new ModuleClass();
        if (gvDataCost.Rows.Count > 0)
        {
            /*GridView循環處理 無生產 和 有報廢數量的邏輯*/
            for (int i = 0; i < gvDataCost.Rows.Count; i++)
            {
                string strGVFABID = Server.UrlEncode(gvDataCost.Rows[i].Cells[0].Text.Replace("&nbsp;", "").ToString());
                string strGVPlant = Server.UrlEncode(gvDataCost.Rows[i].Cells[1].Text.Replace("&nbsp;", "").ToString());
                string strGVPRODGROUP = Server.UrlEncode(gvDataCost.Rows[i].Cells[2].Text.Replace("&nbsp;", "").ToString());
                string strGVSIZEID = Server.UrlEncode(gvDataCost.Rows[i].Cells[3].Text.Replace("&nbsp;", "").ToString());
                string strGVProjectName = Server.UrlEncode(gvDataCost.Rows[i].Cells[4].Text.Replace("&nbsp;", "").ToString());

                if (gvDataCost.Rows[i].Cells[2].Text.ToUpper() == "合計") { gvDataCost.Rows[i].BackColor = Color.FromArgb(153, 204, 255); }
                if (gvDataCost.Rows[i].Cells[3].Text.ToUpper() == "合計") { gvDataCost.Rows[i].BackColor = Color.FromArgb(204, 153, 255); }
                if (gvDataCost.Rows[i].Cells[4].Text.ToUpper() == "合計") { gvDataCost.Rows[i].BackColor = Color.FromArgb(51, 204, 204); }
                for (int j = 4; j < gvDataCost.Columns.Count; j++)    /*6個月+4周+7天*/
                {
                    if (gvDataCost.Rows[i].Cells[j].Text.ToString() == "-2.000")
                    {
                        gvDataCost.Rows[i].Cells[j].Text = ""; 
                        gvDataCost.Rows[i].Cells[j].ToolTip = "提示：當天無數據";
                        gvDataCost.Rows[i].Cells[j + 1].Text = ""; 
                    }
                    if (gvDataCost.Rows[i].Cells[j].Text.ToString() == "-1.000")
                    {
                        gvDataCost.Rows[i].Cells[j].Text = "*0.000"; 
                        gvDataCost.Rows[i].Cells[j].ToolTip = "提示：當天產出數為0，報廢數不為0";
                    }
                }
            }
            /*合併欄位,要放在處理欄位雙擊時間之後處理*/
            for (int i = 0; i < 5; i++)
            {
                mc.GroupRows(gvDataCost, i);
            }
        }
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
    protected void ChooseTransMode()
    {
        string strTransMode = "";
        InitialBoundData("", strTransMode);
    }
    protected void Plant_SelectedIndexChanged()
    {
        InitialBoundData("", "");
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

                    if (i == 5 || i == 7)
                    {
                        if (gvData.Columns[i].HeaderText.ToString() == "False")
                        {
                            THeaderCell.Visible = false;
                        }
                        else
                        {
                            THeaderCell.Visible = true;
                        }
                    }

                    THeaderCell.BackColor = ColorTranslator.FromHtml("#507CD1");
                    THeaderCell.ForeColor = Color.White;
                    THeaderCell.BackColor = Color.YellowGreen;
                    THeaderCell.ForeColor = Color.Black;
                    THeaderCell.Wrap = false;
                    if (h == 0)
                    {
                        if ((i >= 0 && i <= 7))
                        {
                            THeaderCell.RowSpan = 2;
                        }
                        if (i == 8)
                        {
                            THeaderCell.ColumnSpan = 17;
                            THeaderCell.Text = "損耗率";
                        }
                        if (i >= 9 && i <= 24)
                        {
                            THeaderCell.Visible = false;
                        }
                    }
                    else
                    {
                        if ((i >= 0 && i <= 7))
                        {
                            THeaderCell.Visible = false;
                        }
                        if ((i >= 8 && i <= 13))
                        {
                            THeaderCell.Text = mc.GetEngMonth(QueryHeadCellText(i));
                        }
                        if ((i >= 14 && i <= 17))
                        {
                            THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryHeadCellText(i)));
                        }
                        if ((i >= 18 && i <= 24))
                        {
                            THeaderCell.Text = QueryHeadCellText(i);
                        }
                        if ((i >= 13 && i <= 24))
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
        if (intCol >= 18 && intCol <= 24)
        {
            strCellText = dtDate.AddDays(intCol - 22).ToString("MM/dd");
        }
        if (intCol >= 14 && intCol <= 17)
        {
            strCellText = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - (15 - intCol) * 7).ToString("yyyy-MM-dd");
        }
        if (intCol >= 8 && intCol <= 13)
        {
            strCellText = dtDate.AddMonths(intCol - 11).ToString("MM");
        }
        return strCellText;
    }
    protected string QueryCostHeadCellText(int intCol)
    {
        string strCellText = "";
        DateTime dtDate = DateTime.Now.AddDays(-1).AddHours(-9);
        if (intCol >= 26 && intCol <= 39)
        {
            strCellText = dtDate.AddDays(intCol / 2 - 19).ToString("MM/dd");
        }
        if (intCol >= 18 && intCol <= 25)
        {
            strCellText = dtDate.AddDays(-1 * Convert.ToInt32(dtDate.DayOfWeek) - (11 - intCol / 2) * 7).ToString("yyyy-MM-dd");
        }
        if (intCol >= 6 && intCol <= 17)
        {
            strCellText = dtDate.AddMonths(intCol / 2 - 9).ToString("MM");
        }
        return strCellText;
    }
    protected void gvDataCost_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ModuleClass mc = new ModuleClass();
            int intCnt = gvDataCost.Columns.Count;
            for (int h = 0; h < 3; h++)
            {
                GridViewRow RowHeader = new GridViewRow(h, h, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell THeaderCell;
                for (int i = 0; i < intCnt; i++)
                {
                    THeaderCell = new TableHeaderCell();
                    THeaderCell.Text = gvDataCost.Columns[i].HeaderText.ToString();
                    THeaderCell.BackColor = Color.YellowGreen;
                    THeaderCell.ForeColor = Color.Black;
                    THeaderCell.Wrap = false;
                    if (h == 0)
                    {
                        if ((i >= 0 && i <= 5))
                        {
                            THeaderCell.RowSpan = 3;
                        }
                        if (i == 6)
                        {
                            THeaderCell.ColumnSpan = 34;
                            THeaderCell.Text = "損耗率";
                        }
                        if (i >= 7 && i <= 41)
                        {
                            THeaderCell.Visible = false;
                        }
                    }
                    else
                    {
                        if ((i >= 0 && i <= 5))
                        {
                            THeaderCell.Visible = false;
                        }
                        if (h == 1)
                        {
                            if (i == 6) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetEngMonth(QueryCostHeadCellText(i)); }
                            if (i == 8) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetEngMonth(QueryCostHeadCellText(i)); }
                            if (i == 10) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetEngMonth(QueryCostHeadCellText(i)); }
                            if (i == 12) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetEngMonth(QueryCostHeadCellText(i)); }
                            if (i == 14) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetEngMonth(QueryCostHeadCellText(i)); }
                            if (i == 16) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = "MTD"; }

                            if (i == 18) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryCostHeadCellText(i))); }
                            if (i == 20) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryCostHeadCellText(i))); }
                            if (i == 22) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryCostHeadCellText(i))); }
                            if (i == 24) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = mc.GetWeek(Convert.ToDateTime(QueryCostHeadCellText(i))); }

                            if (i == 26) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 28) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 30) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 32) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 34) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 36) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 38) { THeaderCell.ColumnSpan = 2; THeaderCell.Text = QueryCostHeadCellText(i); }
                            if (i == 7 || i == 9 || i == 11 || i == 13 || i == 15 || i == 17 || i == 19 || i == 21 || i == 23 || i == 25 || i == 27 || i == 29 || i == 31 || i == 33 || i == 35 || i == 37 || i == 39)
                            {
                                THeaderCell.Visible = false;
                            }
                        }
                        if (i >= 16)
                        {
                            THeaderCell.BackColor = Color.Yellow;
                        }
                    }
                    THeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    RowHeader.Cells.Add(THeaderCell);
                }
                gvDataCost.Controls[0].Controls.AddAt(h, RowHeader);
            }
        }
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        ChooseTransMode();
        SetTitle();
        ConfigRole();
        lblRunTime.Text = "";
        gvData.DataSource = "";
        gvData.DataBind();
        gvDataCost.DataSource = "";
        gvDataCost.DataBind();
        cbShowPartID.Checked = false;
        gvData.Columns[5].HeaderText = "TP";
        gvData.Columns[7].HeaderText = "物料料號";

    }
    protected void rblMergeModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChooseTransMode();
        SetTitle();
        ConfigRole();
        lblRunTime.Text = "";
        gvData.DataSource = "";
        gvData.DataBind();
        gvDataCost.DataSource = "";
        gvDataCost.DataBind();
        cbShowPartID.Checked = false;
        gvData.Columns[5].HeaderText = "TP";
        gvData.Columns[7].HeaderText = "物料料號";
    }
}