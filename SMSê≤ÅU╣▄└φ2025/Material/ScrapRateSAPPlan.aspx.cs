using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Material_ScrapRateSAPPlan : System.Web.UI.Page
{
    const string strPlanType = "SAPPlan";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ModuleClass mc = new ModuleClass();
            string strNTName = "", strPCIP = "", strPCName = "";
            mc.GetLoginInfo(out strNTName, out strPCIP, out strPCName);

            //string strUserID = "";
            //if (Request.QueryString["UserID"] != null) { strUserID = this.Request.QueryString["UserID"].ToString().ToUpper(); }

            tbNTName.Text = strNTName;
            tbPCName.Text = strPCIP + "(" + strPCName + ")";

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

            ExportExcel.HRef = "../ExcelFormat/SAPPlanRate.xls";
            SetTitle();
        }
    }
    protected void SetTitle()
    {
        this.Title = "SAP Plan目標值設定";
    }
    protected void QueryDataPlan(int intYear, int intMonth, string strTP, string strShop)
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtTmp = mc.GetMSAPPlanRateM(intYear, intMonth, strTP, strShop);
        gvData.DataSource = dtTmp;
        gvData.DataBind();
        if (dtTmp.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void QueryDataTP(int intYear, int intMonth)
    {
        DBConn dbConn = new DBConn();
        string strSql = "";

        strSql = strSql + " SELECT DISTINCT CONVERT(VARCHAR(7), r.REPORTDATE,120) AS REPORTYEARMonth,r.REPORTYEAR,r.ReportMonth,r.SHOPCD,r.TP_PART_NUM";
        strSql = strSql + " INTO #RPT_EIS_SUM FROM dbo.RPT_EIS AS r WITH(NOLOCK) ";
        strSql = strSql + " WHERE r.REPORTYEAR = " + intYear + " AND r.ReportMonth = " + intMonth + " ";

        strSql = strSql + " SELECT DISTINCT REPORTYEARMonth AS ReportMonth,T1.SHOPCD as PLANTID,T1.TP_PART_NUM,SHOPCD AS SHOP ";
        strSql = strSql + " ,ISNULL(T2.PlanRate,1) AS PlanRate";
        strSql = strSql + " FROM #RPT_EIS_SUM T1 LEFT JOIN dbo.MSAPPlanRateM T2 ON (T1.REPORTYEAR=T2.[Year] AND T1.ReportMonth=T2.[Month] AND T1.TP_PART_NUM=T2.TP_PART_NUM) AND SHOPCD=T2.SHOP";
        //strSql = strSql + " WHERE DATEPART(YEAR,REPORTDATE) = " + intYear + " AND DATEPART(MONTH,REPORTDATE) = " + intMonth + " ";
        strSql = strSql + " WHERE T2.TP_PART_NUM IS NULL";
        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        TP_PART_NUM.DataSource = dsTmp.Tables[0];
        TP_PART_NUM.DataBind();
        if (dsTmp.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        if (rblPlan.Items[0].Selected == true)
        {
            TP_PART_NUM.Visible = false;
            gvData.Visible = true;
            QueryDataPlan(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), "", "");
        }
        if (rblPlan.Items[1].Selected == true)
        {
            TP_PART_NUM.Visible = true;
            gvData.Visible = false;
            QueryDataTP(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
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
        this.TP_PART_NUM.RenderControl(oHtmlTextWriter);
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
                            string strTP = dtReadExcel.Rows[i][2].ToString().Trim();
                            string strShop = dtReadExcel.Rows[i][3].ToString().Trim();
                            double dblPlanRate = Convert.ToDouble(dtReadExcel.Rows[i][4].ToString().Trim());
                            strSql = "";
                            if (strShop != "BEOL" && strShop != "LCM" && strShop != "LAM" && strShop != "CELL2" && strShop != "MODULE")
                            {
                                intErrCount++;
                                mc.SaveLog(ModuleClass.CstrLogType, intYear.ToString() + " " + intMonth.ToString() + " " + strTP + " " + strShop + " SHOP的值上傳錯誤");
                            }
                            else
                            {
                                if (dblPlanRate == 0)
                                {
                                    strSql = strSql + " DELETE FROM dbo.MSAPPlanRateM";
                                    strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND TP_PART_NUM = '" + strTP + "' AND SHOP = '" + strShop + "' ";
                                }
                                else
                                {
                                    strSql = strSql + " UPDATE dbo.MSAPPlanRateM";
                                    strSql = strSql + " SET PlanRate = " + dblPlanRate + " ";
                                    strSql = strSql + " , Time_Stamp = GETDATE(), Userid = '" + tbNTName.Text + "', PCName = '" + tbPCName.Text + "' ";
                                    strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND TP_PART_NUM = '" + strTP + "' AND SHOP = '" + strShop + "' ";
                                    strSql = strSql + " IF @@ROWCOUNT = 0";
                                    strSql = strSql + " BEGIN";
                                    strSql = strSql + " INSERT INTO dbo.MSAPPlanRateM";
                                    strSql = strSql + " ( [Year] ,";
                                    strSql = strSql + " [Month] ,";
                                    strSql = strSql + " TP_PART_NUM ,";
                                    strSql = strSql + " SHOP ,";
                                    strSql = strSql + " PlanRate ,";
                                    strSql = strSql + " Userid ,";
                                    strSql = strSql + " PCName ,";
                                    strSql = strSql + " Time_Stamp";
                                    strSql = strSql + " )";
                                    strSql = strSql + " VALUES  ( " + intYear + "";
                                    strSql = strSql + " ," + intMonth + "";
                                    strSql = strSql + " ,'" + strTP + "' ";
                                    strSql = strSql + " ,'" + strShop + "' ";
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