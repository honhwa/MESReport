using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
public partial class Material_ScrapRateSRTrendQuery : System.Web.UI.Page
{
    const string strReportName = "SR報表-趨勢查詢";
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

            //string strUserID = "";
            //if (Request.QueryString["UserID"] != null) { strUserID = this.Request.QueryString["UserID"].ToString().ToUpper(); }

            tbNTName.Text = strNTName;

            //if (mc.GetMPCIPM(HttpContext.Current.Request.Url.AbsolutePath.ToString(), strPCIP) == false)
            //{
            //    Response.Redirect("../ShowMessage.aspx");
            //    return;
            //}

            InitialBoundData();
            SetTitle();
            ConfigRole();
            //QueryData();
            Response.Expires = -1;
        }
    }
    protected void SetTitle()
    {
        this.Title = " SR報表-趨勢查詢";
    }
    protected void InitialBoundData()
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtWorkType = mc.GetMWOTM("");
        WorkOrderType.DataTableValue = dtWorkType;

        /*因會依據多個Plant看數據，故產品不做PLANT的過濾*/
        Product.GetPlant = "";
        DataTable dtSIZE = mc.GetSIZE("");
        Product.DataTableSIZEValue = dtSIZE;
    }
    protected void ConfigRole()
    {
        ModuleClass mc = new ModuleClass();
        if (mc.GetRole("Cost", tbNTName.Text) == false) { hlCost.Visible = false; } else { hlCost.Visible = true; }
        if (mc.GetRole("SAPPlan", tbNTName.Text) == false) { hlSAPPlan.Visible = false; } else { hlSAPPlan.Visible = true; }

        if (mc.GetUserGroupCode("Cost", tbNTName.Text) == false) { hlLimitCost.Visible = false; } else { hlLimitCost.Visible = true; }
        if (mc.GetUserGroupCode("SAPPlan", tbNTName.Text) == false) { hlLimitSAPPlan.Visible = false; } else { hlLimitSAPPlan.Visible = true; }
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
        strSql = strSql + " SET DATEFIRST  1;";
        strSql = strSql + " DECLARE @MFGDate AS DATETIME;";
        strSql = strSql + " DECLARE @STBDate AS DATETIME;";
        strSql = strSql + " DECLARE @ENDDate AS DATETIME;";
        strSql = strSql + " SET @MFGDate = CAST(CONVERT(VARCHAR(10), DATEADD(HOUR, -9,GETDATE() - 1), 120) AS DATETIME);";

        strSql = strSql + " SET @STBDate = CAST(CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) + '-01' AS DATETIME);";
        strSql = strSql + " SET @ENDDate = @MFGDate;";

        string strSqlWortType = mc.ListBoxToValue(WorkOrderType.ListBoxToValue);
        string strSqlSIZE = Product.DropDownListSIZE.SelectedValue.Trim().ToString();
        string strSqlProjectName = Product.DropDownListProjectName.SelectedValue.Trim().ToString();
        string strSqlProduct = mc.ListBoxToValue(Product.ListBoxToValue);

        strSql = strSql + " SELECT r.FABID,r.REPORTDATE,r.REPORTYEAR,r.REPORTMONTH,r.PROJECT_NAME,r.PRODGROUP,r.PRODID,r.TP_PART_NUM,r.COMP_PART,r.COMP_M_GROUP,r.SHOPCD,SUM(r.OutPutQTY) AS OutPutQTY";
        strSql = strSql + " ,SUM(r.ScrapQTY) AS ScrapQTY";
        strSql = strSql + " INTO #EIS_SUM FROM dbo.RPT_EIS AS r WITH(NOLOCK)";
        strSql = strSql + " WHERE r.ReportDate >= @STBDate AND r.ReportDate <= @ENDDate";
        #region parameters [選擇條件過濾]
        if (WorkOrder.strValue != "")//工單
        {
            strSql = strSql + " AND r.WONBR = '" + WorkOrder.strValue + "' ";
        }
        if (strSqlWortType != "")//工單類型
        {
            strSql = strSql + " AND r.WOTYPE IN(" + strSqlWortType + ")";
        }
        if (strSqlProduct != "")//TP料號
        {
            strSql = strSql + " AND r.PRODID IN(" + strSqlProduct + ");";
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
                    strSql = strSql + " AND r.SIZEID ='" + strSqlSIZE + "' ";
                }
            }
        }
        #endregion
        strSql = strSql + " GROUP BY r.FABID,r.REPORTDATE,r.REPORTYEAR,r.REPORTMONTH,r.PROJECT_NAME,r.PRODGROUP,r.PRODID,r.TP_PART_NUM,r.COMP_PART,r.COMP_M_GROUP,r.SHOPCD;";

        strSql = strSql + " SELECT CONVERT(VARCHAR(7), T1.REPORTDATE, 120) AS REPORTMONTH";
        strSql = strSql + " ,T1.REPORTYEAR";
        strSql = strSql + " ,T1.REPORTMONTH AS REPORTMON";
        strSql = strSql + " ,CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID";
        strSql = strSql + " ,SHOPCD";
        strSql = strSql + " ,T1.PRODGROUP";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.PRODID";
        strSql = strSql + " ,T1.TP_PART_NUM";
        strSql = strSql + " ,SUM(CASE WHEN COMP_M_GROUP = 'PANEL' THEN OUTPUTQTY ELSE 0 END) AS OutQty";
        strSql = strSql + " ,SUM(T1.OUTPUTQTY * ISNULL(TC.Unit_Cost,0)) AS OutCost";
        strSql = strSql + " ,SUM(T1.SCRAPQTY * ISNULL(TC.Unit_Cost,0)) AS ScrapCost";
        strSql = strSql + " ,SUM(T1.OUTPUTQTY +T1.SCRAPQTY) AS TotalQty";
        strSql = strSql + " ,SUM((T1.OUTPUTQTY +T1.SCRAPQTY)* ISNULL(TC.Unit_Cost,0)) AS TotalCost";
        strSql = strSql + " INTO #QtyCost";
        strSql = strSql + " FROM #EIS_SUM T1";
        strSql = strSql + " LEFT JOIN dbo.MUCM TC WITH(NOLOCK) ON(DATEPART(YEAR,T1.REPORTDATE)=TC.[Year] AND DATEPART(MONTH,T1.REPORTDATE)=TC.[Month] AND T1.COMP_PART = TC.Material) ";
        strSql = strSql + " GROUP BY CONVERT(VARCHAR(7), T1.REPORTDATE, 120)";
        strSql = strSql + " ,T1.REPORTYEAR";
        strSql = strSql + " ,T1.REPORTMONTH";
        strSql = strSql + " ,CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END";
        strSql = strSql + " ,SHOPCD";
        strSql = strSql + " ,T1.PRODGROUP";
        strSql = strSql + " ,T1.PROJECT_NAME";
        strSql = strSql + " ,T1.PRODID";
        strSql = strSql + " ,T1.TP_PART_NUM;";

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


        strSql = strSql + " SELECT DISTINCT TM.FABID,TM.PRODGROUP,TM.PROJECT_NAME,TM.PRODID,TM.TP_PART_NUM,TM.SHOPCD,TM.SOPRODID,TM.SO_TP_PART_NUM,TM.ReportMonth,TM.OutCost,TM.TotalCost,TM.OutQty,ISNULL(TP.PlanRate,0) AS PlanRate";
        strSql = strSql + " INTO #ShopCost";
        strSql = strSql + " FROM(";
        strSql = strSql + " SELECT T1.FABID,T1.PRODGROUP,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,T2.SHOPCD";
        strSql = strSql + " ,T2.PRODID AS SOPRODID,T2.TP_PART_NUM AS SO_TP_PART_NUM,T2.ReportMonth,T2.REPORTYEAR,T2.ReportMon,OutCost,TotalCost,OutQty";
        strSql = strSql + " FROM(SELECT FABID,PRODID,TP_PART_NUM,PRODGROUP,PROJECT_NAME,SOPRODID FROM #MPMPM UNION ALL SELECT DISTINCT FABID,PRODID,TP_PART_NUM,PRODGROUP,PROJECT_NAME,PRODID AS SOPRODID FROM #MPMPM)T1";
        strSql = strSql + " INNER JOIN #QtyCost T2 ON(T1.FABID = T2.FABID AND T1.SOPRODID = T2.PRODID)";
        strSql = strSql + " )TM";
        strSql = strSql + " LEFT JOIN dbo.MSAPPlanRateM TP ON(TM.REPORTYEAR = TP.[Year] AND TM.ReportMon = TP.[Month] AND TM.SHOPCD=TP.SHOP AND TM.TP_PART_NUM=TP.TP_PART_NUM);";

        //strSql = strSql + " SELECT DISTINCT TM.FABID,TM.PRODGROUP,TM.PROJECT_NAME,TM.PRODID,TM.TP_PART_NUM,TM.PLANTID,TM.SOPRODID,TM.SO_TP_PART_NUM,TM.ReportMonth,TM.OutCost,TM.TotalCost,TM.OutQty,ISNULL(TP.PlanRate,0) AS PlanRate";
        //strSql = strSql + " INTO #ShopCost";
        //strSql = strSql + " FROM(";
        //strSql = strSql + " SELECT T1.FABID,T1.PRODGROUP,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,REPLACE(REPLACE(T2.PLANTID,'2',''),'1','') AS PLANTID";
        //strSql = strSql + " ,T2.PRODID AS SOPRODID,T2.TP_PART_NUM AS SO_TP_PART_NUM,ReportMonth,OutCost,TotalCost,OutQty";
        //strSql = strSql + " FROM(SELECT FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,SOPRODID FROM #MPMPM UNION ALL SELECT DISTINCT FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,PRODID AS SOPRODID FROM #MPMPM)T1";
        //strSql = strSql + " INNER JOIN #QtyCost T2 ON(T1.FABID = T2.FABID AND T1.PRODGROUP = T2.PRODGROUP AND T1.PROJECT_NAME = T2.PROJECT_NAME AND T1.SOPRODID = T2.PRODID)";
        //strSql = strSql + " )TM LEFT JOIN dbo.MSAPPlanRateM TP ON(CAST(SUBSTRING(TM.ReportMonth,1,4) AS INT) = TP.[Year] AND CAST(SUBSTRING(TM.ReportMonth,6,2) AS INT) = TP.[Month] AND TM.PLANTID=TP.SHOP AND TM.SO_TP_PART_NUM=TP.TP_PART_NUM);";

        /*定義排序--按照近30天的PANEL的OUTQTY從大到小排序*/
        strSql = strSql + " SELECT FABID,PRODGROUP,PROJECT_NAME,SUM(OutQty) AS OutQty,ROW_NUMBER()OVER(ORDER BY FABID,PRODGROUP,SUM(OutQty) DESC) AS SORTID";
        strSql = strSql + " INTO #SORT";
        strSql = strSql + " FROM #ShopCost GROUP BY FABID,PRODGROUP,PROJECT_NAME;";

        strSql = strSql + " SELECT A.FABID,A.PRODGROUP,A.PROJECT_NAME,A.PRODID,A.TP_PART_NUM,A.SHOP,A.SOPRODID,A.SO_TP_PART_NUM";
        strSql = strSql + " ,A.OutQty";
        strSql = strSql + " ,ISNULL(S.SORTID,999) AS PJ_SORTID";
        strSql = strSql + " ,CASE WHEN Rate5 = 0 THEN 1 ELSE Rate5 END AS Rate5";
        strSql = strSql + " ,CASE WHEN Rate4 = 0 THEN 1 ELSE Rate4 END AS Rate4";
        strSql = strSql + " ,CASE WHEN Rate3 = 0 THEN 1 ELSE Rate3 END AS Rate3";
        strSql = strSql + " ,CASE WHEN Rate2 = 0 THEN 1 ELSE Rate2 END AS Rate2";
        strSql = strSql + " ,CASE WHEN Rate1 = 0 THEN 1 ELSE Rate1 END AS Rate1";
        strSql = strSql + " ,CASE WHEN Rate0 = 0 THEN 1 ELSE Rate0 END AS Rate0";
        strSql = strSql + " ,CASE WHEN PlanRate5 = 0 THEN 1 ELSE PlanRate5 END AS PlanRate5";
        strSql = strSql + " ,CASE WHEN PlanRate4 = 0 THEN 1 ELSE PlanRate4 END AS PlanRate4";
        strSql = strSql + " ,CASE WHEN PlanRate3 = 0 THEN 1 ELSE PlanRate3 END AS PlanRate3";
        strSql = strSql + " ,CASE WHEN PlanRate2 = 0 THEN 1 ELSE PlanRate2 END AS PlanRate2";
        strSql = strSql + " ,CASE WHEN PlanRate1 = 0 THEN 1 ELSE PlanRate1 END AS PlanRate1";
        strSql = strSql + " ,CASE WHEN PlanRate0 = 0 THEN 1 ELSE PlanRate0 END AS PlanRate0";
        strSql = strSql + " FROM(";

        strSql = strSql + " SELECT T1.FABID,T1.PRODGROUP,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,T1.SHOPCD AS SHOP,T1.SOPRODID,T1.SO_TP_PART_NUM";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN OutQty ELSE 0 END) AS OutQty";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) THEN OutCost ELSE 0 END) = 0 THEN 1.00 ELSE SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) THEN TotalCost ELSE 0 END) / SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) THEN OutCost ELSE 0 END) END AS Rate5";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-4,@MFGDate), 120) THEN OutCost ELSE 0 END) = 0 THEN 1.00 ELSE SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-4,@MFGDate), 120) THEN TotalCost ELSE 0 END) / SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-4,@MFGDate), 120) THEN OutCost ELSE 0 END) END AS Rate4";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-3,@MFGDate), 120) THEN OutCost ELSE 0 END) = 0 THEN 1.00 ELSE SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-3,@MFGDate), 120) THEN TotalCost ELSE 0 END) / SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-3,@MFGDate), 120) THEN OutCost ELSE 0 END) END AS Rate3";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-2,@MFGDate), 120) THEN OutCost ELSE 0 END) = 0 THEN 1.00 ELSE SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-2,@MFGDate), 120) THEN TotalCost ELSE 0 END) / SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-2,@MFGDate), 120) THEN OutCost ELSE 0 END) END AS Rate2";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-1,@MFGDate), 120) THEN OutCost ELSE 0 END) = 0 THEN 1.00 ELSE SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-1,@MFGDate), 120) THEN TotalCost ELSE 0 END) / SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-1,@MFGDate), 120) THEN OutCost ELSE 0 END) END AS Rate1";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN OutCost ELSE 0 END) = 0 THEN 1.00 ELSE SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN TotalCost ELSE 0 END) / SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN OutCost ELSE 0 END) END AS Rate0";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) THEN ISNULL(T1.PlanRate,0) ELSE 0 END) AS PlanRate5";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-4,@MFGDate), 120) THEN ISNULL(T1.PlanRate,0) ELSE 0 END) AS PlanRate4";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-3,@MFGDate), 120) THEN ISNULL(T1.PlanRate,0) ELSE 0 END) AS PlanRate3";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-2,@MFGDate), 120) THEN ISNULL(T1.PlanRate,0) ELSE 0 END) AS PlanRate2";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-1,@MFGDate), 120) THEN ISNULL(T1.PlanRate,0) ELSE 0 END) AS PlanRate1";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN ISNULL(T1.PlanRate,0) ELSE 0 END) AS PlanRate0";
        strSql = strSql + " FROM #ShopCost T1";
        strSql = strSql + " GROUP BY T1.FABID,T1.PRODGROUP,T1.PROJECT_NAME,T1.PRODID,T1.TP_PART_NUM,T1.SHOPCD,T1.SOPRODID,T1.SO_TP_PART_NUM";
        strSql = strSql + " UNION ALL";
        strSql = strSql + " SELECT FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,'合計' AS SHOP,'' AS SOPRODID,'' AS SO_TP_PART_NUM";
        strSql = strSql + " ,0 AS OutQty";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) THEN Rate ELSE 0 END) Rate5";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-4,@MFGDate), 120) THEN Rate ELSE 0 END) Rate4";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-3,@MFGDate), 120) THEN Rate ELSE 0 END) Rate3";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-2,@MFGDate), 120) THEN Rate ELSE 0 END) Rate2";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-1,@MFGDate), 120) THEN Rate ELSE 0 END) Rate1";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN Rate ELSE 0 END) Rate0";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-5,@MFGDate), 120) THEN ISNULL(T.PlanRate,0) ELSE 0 END) AS PlanRate5";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-4,@MFGDate), 120) THEN ISNULL(T.PlanRate,0) ELSE 0 END) AS PlanRate4";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-3,@MFGDate), 120) THEN ISNULL(T.PlanRate,0) ELSE 0 END) AS PlanRate3";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-2,@MFGDate), 120) THEN ISNULL(T.PlanRate,0) ELSE 0 END) AS PlanRate2";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-1,@MFGDate), 120) THEN ISNULL(T.PlanRate,0) ELSE 0 END) AS PlanRate1";
        strSql = strSql + " ,SUM(CASE WHEN ReportMonth = CONVERT(VARCHAR(7), DATEADD(MONTH,-0,@MFGDate), 120) THEN ISNULL(T.PlanRate,0) ELSE 0 END) AS PlanRate0";
        strSql = strSql + " FROM(";

        strSql = strSql + " SELECT FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,ReportMonth";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN SHOPCD = 'CELL2' THEN OutCost ELSE 0 END) = 0 THEN 1 ELSE SUM(CASE WHEN SHOPCD = 'CELL2' THEN TotalCost ELSE 0 END) / SUM(CASE WHEN SHOPCD = 'CELL2' THEN OutCost ELSE 0 END) END *";
        strSql = strSql + "  CASE WHEN SUM(CASE WHEN SHOPCD = 'MODULE' THEN OutCost ELSE 0 END) = 0 THEN 1 ELSE SUM(CASE WHEN SHOPCD = 'MODULE' THEN TotalCost ELSE 0 END) / SUM(CASE WHEN SHOPCD = 'MODULE' THEN OutCost ELSE 0 END) END AS Rate";
        strSql = strSql + " ,CASE WHEN SUM(CASE WHEN SHOPCD = 'CELL2' THEN PlanRate ELSE 0 END) = 0 THEN 1 ELSE SUM(CASE WHEN SHOPCD = 'CELL2' THEN PlanRate ELSE 0 END) END *";
        strSql = strSql + "  CASE WHEN SUM(CASE WHEN SHOPCD = 'MODULE' THEN PlanRate ELSE 0 END) = 0 THEN 1 ELSE SUM(CASE WHEN SHOPCD = 'MODULE' THEN PlanRate ELSE 0 END) END AS PlanRate";
        strSql = strSql + " FROM(";
        strSql = strSql + " SELECT FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,ReportMonth,SHOPCD,SUM(OutCost) AS OutCost,SUM(TotalCost) AS TotalCost,AVG(PlanRate) AS PlanRate";
        strSql = strSql + " FROM #ShopCost";
        strSql = strSql + " GROUP BY FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,ReportMonth,SHOPCD";
        strSql = strSql + " )TT GROUP BY FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM,ReportMonth";
        strSql = strSql + " )T GROUP BY FABID,PRODGROUP,PROJECT_NAME,PRODID,TP_PART_NUM";
        strSql = strSql + " )A";
        strSql = strSql + " LEFT JOIN #SORT S ON ( A.FABID=S.FABID AND A.PRODGROUP=S.PRODGROUP AND A.PROJECT_NAME=S.PROJECT_NAME)";
        strSql = strSql + " ORDER BY A.FABID";
        strSql = strSql + " ,CASE WHEN A.PRODGROUP= 'SmartPhone' THEN 1 WHEN A.PRODGROUP = 'FeaturePhone' THEN 2 WHEN A.PRODGROUP = 'CE' THEN 3 ELSE 4 END,A.PRODGROUP";
        strSql = strSql + " ,ISNULL(S.SORTID,999)";
        strSql = strSql + " ,A.PROJECT_NAME,A.PRODID,A.TP_PART_NUM";
        strSql = strSql + " ,CASE A.SHOP WHEN '合計' THEN 9 WHEN 'CELL2' THEN 1 WHEN 'MODULE' THEN 2 ELSE 8 END;";

        /*Table_1 查詢未維護單價的料號數量*/
        strSql = strSql + " SELECT DISTINCT CONVERT(VARCHAR(7), REPORTDATE,120) AS ReportMonth,COMP_PART,ISNULL(t2.Unit_Cost,0) AS UnitCost FROM #EIS_SUM T1";
        strSql = strSql + " LEFT JOIN dbo.MUCM T2 ON (DATEPART(YEAR,T1.REPORTDATE)=T2.[Year] AND DATEPART(MONTH,T1.REPORTDATE)=T2.[Month] AND T1.COMP_PART=T2.Material)";
        strSql = strSql + " WHERE T2.Unit_Cost IS NULL;";

        /*Table_2 檢查是否有未設定ProjectName的料號*/
        strSql = strSql + " SELECT DISTINCT A.PROJECT_NAME FROM #QtyCost A";
        strSql = strSql + " WHERE ISNULL(A.PROJECT_NAME,'') = '';";


        #endregion
        DataSet dsData = dbConn.GetDataSetBySql(strSql);
        DataTable dtQuery = dsData.Tables[0];
        gvData.DataSource = dtQuery;
        gvData.DataBind();

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

        if (dtQuery.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
        else
        {
            GridViewFormat();
            gvData.HeaderRow.Visible = false;
        }
        DateTime dtEnd = DateTime.Now;
        TimeSpan tsDiff = dtEnd - dtBegin;
        lblRunTime.Text = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";

        /*使用記錄，每隻報表必須要增加*/
        mc.RecordLogInfo("B", "UnLogin", Session.SessionID.ToString(), "Query", "", "/SMS" + HttpContext.Current.Request.Url.AbsolutePath.ToString(), strReportName, "SH", "");
        SetTitle();
    }
    protected void GridViewFormat()
    {
        /*處理GridView的格式*/
        ModuleClass mc = new ModuleClass();
        if (gvData.Rows.Count > 0)
        {
            /*GridView循環處理 無生產 和 有報廢數量的邏輯*/
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                if (gvData.Rows[i].Cells[5].Text.ToUpper() == "合計")
                {
                    gvData.Rows[i].BackColor = Color.FromArgb(255, 255, 204);
                    gvData.Rows[i].Cells[7].Text = "";
                }
                else
                {
                    /*判斷是否有超PLAN的RATE，超了顯示紅色字體*/
                    for (int intCnt = 8; intCnt <= 13; intCnt++)
                    {
                        if (Convert.ToDouble(gvData.Rows[i].Cells[intCnt].Text.Trim()) > Convert.ToDouble(gvData.Rows[i].Cells[intCnt + 6].Text.Trim()))
                        {
                            gvData.Rows[i].Cells[intCnt].ForeColor = Color.Red;
                        }
                        else
                        {
                            gvData.Rows[i].Cells[intCnt].ForeColor = Color.Black;
                        }
                    }
                }
            }
            /*合併欄位,要放在處理欄位雙擊時間之後處理*/
            for (int i = 0; i < 5; i++)
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
            DateTime dtMFGDate = DateTime.Now.AddDays(-1).AddHours(-9);
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
                    if (i == 7)
                    {
                        THeaderCell.Text = gvData.Columns[i].HeaderText.ToString() + "<br>(" + dtMFGDate.ToString("yyyy-MM") + ")";
                    }
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
                            THeaderCell.ColumnSpan = 6;
                            THeaderCell.Text = "Month 損耗率";
                        }
                        if (i >= 9 && i <= 13)
                        {
                            THeaderCell.Visible = false;
                        }
                        if (i == 14)
                        {
                            THeaderCell.ColumnSpan = 6;
                            THeaderCell.Text = "SAP Plan損耗率";
                        }
                        if (i >= 15 && i <= 19)
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
                        if (i >= 8 && i <= 13)
                        {
                            string strYear = dtMFGDate.AddMonths(i - 13).ToString("yy");
                            string strMonth = dtMFGDate.AddMonths(i - 13).ToString("MM");
                            THeaderCell.Text = mc.GetEngMonth(strMonth)+ strYear;
                        }

                        if (i >= 14 && i <= 19)
                        {
                            string strYear = dtMFGDate.AddMonths(i - 19).ToString("yy");
                            string strMonth = dtMFGDate.AddMonths(i - 19).ToString("MM");
                            THeaderCell.Text = mc.GetEngMonth(strMonth) + strYear;
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