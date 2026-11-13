using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Material_ScrapRatePlan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ModuleClass mc = new ModuleClass();
            string strNTName = "", strPCIP = "", strPCName = "";
            mc.GetLoginInfo(out strNTName, out strPCIP, out strPCName);
            tbNTName.Text = strNTName;
            tbPCName.Text = strPCIP + "(" + strPCName + ")";

            string strPlanType = "";
            if (Request.QueryString["PlanType"] != null) { strPlanType = this.Request.QueryString["PlanType"].ToString(); }

            tbPlanType.Text = strPlanType;

            if (mc.GetRole(strPlanType, tbNTName.Text) == false)
            {
                Main.Visible = false;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('無權限操作！');</script>");
                return;
            }

            DataTable dtYear = mc.GetYear(3);
            this.ddlYear.DataSource = dtYear;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            DataTable dtMonth = mc.GetMonth();
            this.ddlMonth.DataSource = dtMonth;
            ddlMonth.DataTextField = "Month";
            ddlMonth.DataValueField = "Month";
            ddlMonth.DataBind();

            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

            ExportExcel.HRef = "../ExcelFormat/PlanRate.xls";
            SetTitle();
        }
    }
    protected void SetTitle()
    {
        if (tbPlanType.Text == "PEPlan")
        {
            this.Title = "PE Plan目標值設定";
        }
    }
    protected void QueryDataPlan(int intYear, int intMonth, string strPlanType, string strPlantID, string strProjectName, string strPartType)
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtTmp = mc.GetMPlanRateM(intYear, intMonth, strPlanType, strPlantID, strProjectName, strPartType);
        gvData.DataSource = dtTmp;
        gvData.DataBind();
        if (dtTmp.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void QueryDataProjectName(int intYear, int intMonth, string strPlanType)
    {
        DBConn dbConn = new DBConn();
        string strSql = "";
        if (tbPlanType.Text == "PEPlan")
        {
            strSql = strSql + " SELECT DISTINCT CONVERT(VARCHAR(7), r.REPORTDATE,120) AS REPORTYEARMonth,r.REPORTYEAR,r.ReportMonth,r.SHOPCD AS PLANTID,r.PROJECT_NAME,r.COMP_M_GROUP";
            strSql = strSql + " INTO #RPT_EIS_SUM FROM dbo.RPT_EIS AS r WITH(NOLOCK) ";
            strSql = strSql + " WHERE r.REPORTYEAR = " + intYear + " AND r.ReportMonth = " + intMonth + " ";


            strSql = strSql + " SELECT DISTINCT T1.REPORTYEARMonth AS ReportMonth,T1.PLANTID,T1.PROJECT_NAME,T1.COMP_M_GROUP AS PartType,ISNULL(T2.PlanRate,0) AS PlanRate ";
            strSql = strSql + " FROM #RPT_EIS_SUM T1";
            strSql = strSql + " LEFT JOIN dbo.MPlanRateM T2 ON (T1.REPORTYEAR=T2.[Year] AND T1.ReportMonth=T2.[Month] AND T2.PlanType = '" + strPlanType + "' AND T1.PLANTID = T2.PLANTID AND T1.PROJECT_NAME=T2.PROJECT_NAME)";
            //strSql = strSql + " WHERE DATEPART(YEAR,REPORTDATE) = " + intYear + " AND DATEPART(MONTH,REPORTDATE) = " + intMonth + " ";
            strSql = strSql + " WHERE T2.PlanRate IS NULL";
        }
        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        ProjectName.DataSource = dsTmp.Tables[0];
        ProjectName.DataBind();
        if (dsTmp.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        if (rblPlan.Items[0].Selected == true)
        {
            ProjectName.Visible = false;
            gvData.Visible = true;
            QueryDataPlan(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), tbPlanType.Text, "", tbProjectName.Text, "");
        }
        if (rblPlan.Items[1].Selected == true)
        {
            ProjectName.Visible = true;
            gvData.Visible = false;
            QueryDataProjectName(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), tbPlanType.Text);
        }
        SetTitle();
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        }
    }
    protected void btExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = false;
        Response.Charset = "UTF-8";
        //Response.ContentType = "Content-Language;content=UTF-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
        this.EnableViewState = false;
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        this.gvData.RenderControl(oHtmlTextWriter);
        this.ProjectName.RenderControl(oHtmlTextWriter);
        Response.Output.Write(oStringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btUPLoad_Click(object sender, EventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        try
        {
            if (FileUpload1.HasFile == false)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇上傳檔案！');</script>");
                return;
            }
            string strServerPath = "PlanRate" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            if (!System.IO.Directory.Exists(Server.MapPath(@"~/ExcelUpload/")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(@"~/ExcelUpload/"));
            }
            FileUpload1.SaveAs(Server.MapPath(@"~/ExcelUpload/" + strServerPath));
            string strPath = Server.MapPath(@"~/ExcelUpload/" + strServerPath);
            int intErrCount = 0;

            DBConn dbConn = new DBConn();
            DataSet dsReadExcel = mc.ExcelRead(strPath, "Sheet1$");     /*讀取EXCEL表并返回結果集*/
            if (dsReadExcel != null)
            {
                DataTable dtReadExcel = dsReadExcel.Tables[0];     /*讀取EXCEL表并返回結果集*/
                int intRowCnt = dtReadExcel.Rows.Count;
                int intColCnt = dtReadExcel.Columns.Count;
                if (intRowCnt > 0)
                {
                    if (intColCnt >= 5)
                    {
                        string strSql = "";
                        for (int i = 0; i < intRowCnt; i++)
                        {
                            int intYear = Convert.ToInt32(dtReadExcel.Rows[i][0].ToString().Trim());
                            int intMonth = Convert.ToInt32(dtReadExcel.Rows[i][1].ToString().Trim());
                            string strPlantID = dtReadExcel.Rows[i][2].ToString().Trim();
                            string strProjectName = dtReadExcel.Rows[i][3].ToString().Trim();
                            string strPartType = dtReadExcel.Rows[i][4].ToString().Trim();
                            double dblPlanRate = Convert.ToDouble(dtReadExcel.Rows[i][5].ToString().Trim());
                            strSql = "";
                            if (dblPlanRate == 0)
                            {
                                strSql = strSql + " DELETE FROM dbo.MPlanRateM";
                                strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND PlanType = '" + tbPlanType.Text + "' AND PLANTID = '" + strPlantID + "' AND PROJECT_NAME = '" + strProjectName + "' AND PartType = '" + strPartType + "' ";
                            }
                            else
                            {
                                strSql = strSql + " UPDATE dbo.MPlanRateM";
                                strSql = strSql + " SET PlanRate = " + dblPlanRate + ", Time_Stamp = GETDATE(), Userid = '" + tbNTName.Text + "', PCName = '" + tbPCName.Text + "' ";
                                strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND PlanType = '" + tbPlanType.Text + "' AND PLANTID = '" + strPlantID + "' AND PROJECT_NAME = '" + strProjectName + "' AND PartType = '" + strPartType + "' ";
                                strSql = strSql + " IF @@ROWCOUNT = 0";
                                strSql = strSql + " BEGIN";
                                strSql = strSql + " INSERT INTO dbo.MPlanRateM";
                                strSql = strSql + " ( [Year] ,";
                                strSql = strSql + " [Month] ,";
                                strSql = strSql + " PlanType ,";
                                strSql = strSql + " PLANTID ,";
                                strSql = strSql + " PROJECT_NAME ,";
                                strSql = strSql + " PartType ,";
                                strSql = strSql + " PlanRate ,";
                                strSql = strSql + " Userid ,";
                                strSql = strSql + " PCName ,";
                                strSql = strSql + " Time_Stamp";
                                strSql = strSql + " )";
                                strSql = strSql + " VALUES  ( " + intYear + "";
                                strSql = strSql + " ," + intMonth + "";
                                strSql = strSql + " ,'" + tbPlanType.Text + "' ";
                                strSql = strSql + " ,'" + strPlantID + "' ";
                                strSql = strSql + " ,'" + strProjectName + "' ";
                                strSql = strSql + " ,'" + strPartType + "' ";
                                strSql = strSql + " ," + dblPlanRate + " ";
                                strSql = strSql + " ,'" + tbNTName.Text + "' ";
                                strSql = strSql + " ,'" + tbPCName.Text + "' ";
                                strSql = strSql + " ,GETDATE() ";
                                strSql = strSql + " )";
                                strSql = strSql + " END";
                            }
                            string strErrDesc = "";
                            int intErrCode = 0;
                            if (dbConn.ExecuteSql(strSql, out strErrDesc, out intErrCode) == false)
                            {
                                intErrCount++;
                                mc.SaveLog(ModuleClass.CstrLogType, intErrCode.ToString() + strErrDesc);
                            }
                        }
                        mc.SaveLog(ModuleClass.CstrLogType, "上傳檔案名稱：" + strPath + "; 總筆數" + intRowCnt.ToString() + "; 失敗筆數:" + intErrCount.ToString());
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('已上傳完畢！共" + intRowCnt.ToString() + "筆，失敗" + intErrCount.ToString() + "筆');</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請確認上傳數據的格式是否正確！');</script>");
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('無可上傳的數據！');</script>");
                    return;
                }
            }
            SetTitle();
        }
        catch(Exception ex)
        {
            mc.SaveLog(ModuleClass.CstrErrorLogType, ex.ToString());
        }
    }
}