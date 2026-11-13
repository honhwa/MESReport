using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Material_ScrapRateCost : System.Web.UI.Page
{
    const string strGroup = "Cost";
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

            if (mc.GetRole(strGroup, tbNTName.Text) == false)
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

            ExportExcel.HRef = "../ExcelFormat/MUCM.xls";
        }
    }
    protected void QueryDataCost(int intYear, int intMonth, string strMaterial)
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtTmp = mc.GetMUCM(intYear, intMonth, strMaterial);
        gvData.DataSource = dtTmp;
        gvData.DataBind();
        if (dtTmp.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void QueryDataMaterial(int intYear, int intMonth)
    {
        DBConn dbConn = new DBConn();
        string strSql = "";

        strSql = strSql + " SELECT DISTINCT CONVERT(VARCHAR(7), REPORTDATE,120) AS REPORTYEARMonth,r.REPORTYEAR,r.ReportMonth, r.COMP_PART";
        strSql = strSql + " INTO #RPT_EIS_SUM FROM dbo.RPT_EIS AS r WITH(NOLOCK) ";
        strSql = strSql + " WHERE r.REPORTYEAR = " + intYear + " AND r.ReportMonth = " + intMonth + " ";

        strSql = strSql + " SELECT DISTINCT REPORTYEARMonth AS ReportMonth,COMP_PART,ISNULL(t2.Unit_Cost,0) AS UnitCost FROM #RPT_EIS_SUM T1";
        strSql = strSql + " LEFT JOIN dbo.MUCM T2 ON (T1.REPORTYEAR=T2.[Year] AND T1.ReportMonth=T2.[Month] AND T1.COMP_PART=T2.Material)";
        strSql = strSql + " WHERE T2.Unit_Cost IS NULL";
        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        gvMaterial.DataSource = dsTmp.Tables[0];
        gvMaterial.DataBind();
        if (dsTmp.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        if (rblCost.Items[0].Selected == true)
        {
            gvMaterial.Visible = false;
            gvData.Visible = true;
            QueryDataCost(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), tbMaterial.Text);
        }
        if (rblCost.Items[1].Selected == true)
        {
            gvMaterial.Visible = true;
            gvData.Visible = false;
            QueryDataMaterial(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
        }
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
        this.gvMaterial.RenderControl(oHtmlTextWriter);
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
            string strServerPath = "MUCM" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
                    if (intColCnt >= 4)
                    {
                        string strSql = "";
                        for (int i = 0; i < intRowCnt; i++)
                        {
                            int intYear = Convert.ToInt32(dtReadExcel.Rows[i][0].ToString().Trim());
                            int intMonth = Convert.ToInt32(dtReadExcel.Rows[i][1].ToString().Trim());
                            string strMaterial = dtReadExcel.Rows[i][2].ToString().Trim();
                            double dblCost = Convert.ToDouble(dtReadExcel.Rows[i][3].ToString().Trim());
                            string strCurrency = dtReadExcel.Rows[i][4].ToString().Trim();
                            strSql = "";
                            if (dblCost == 0)
                            {
                                strSql = strSql + " DELETE FROM dbo.MUCM";
                                strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND Material = '" + strMaterial + "' ";
                            }
                            else
                            {
                                strSql = strSql + " UPDATE dbo.MUCM";
                                strSql = strSql + " SET Unit_Cost = " + dblCost + ", Currency = '" + strCurrency + "', UpdateTime = GETDATE(), UpdateUserID = '" + tbNTName.Text + "', PCName = '" + tbPCName.Text + "' ";
                                strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND Material = '" + strMaterial + "' ";
                                strSql = strSql + " IF @@ROWCOUNT = 0";
                                strSql = strSql + " BEGIN";
                                strSql = strSql + " INSERT INTO dbo.MUCM";
                                strSql = strSql + " ([Year] ,";
                                strSql = strSql + " [Month] ,";
                                strSql = strSql + " Material ,";
                                strSql = strSql + " Unit_Cost ,";
                                strSql = strSql + " Currency ,";
                                strSql = strSql + " UpdateTime ,";
                                strSql = strSql + " PCName ,";
                                strSql = strSql + " UpdateUserID";
                                strSql = strSql + " )";
                                strSql = strSql + " VALUES  ( " + intYear + "";
                                strSql = strSql + " ," + intMonth + "";
                                strSql = strSql + " ,'" + strMaterial + "' ";
                                strSql = strSql + " ," + dblCost + " ";
                                strSql = strSql + " ,'" + strCurrency + "' ";
                                strSql = strSql + " ,GETDATE() ";
                                strSql = strSql + " ,'" + tbPCName.Text + "' ";
                                strSql = strSql + " ,'" + tbNTName.Text + "' ";
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
        }
        catch(Exception ex)
        {
            mc.SaveLog(ModuleClass.CstrErrorLogType, ex.ToString());
        }
    }
    protected void gvMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        }
    }
}