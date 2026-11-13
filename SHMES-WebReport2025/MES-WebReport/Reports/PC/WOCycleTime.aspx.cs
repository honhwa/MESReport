using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Report.Library;
using Microsoft.Reporting.WebForms;
using System.Text;
using System.Data.SqlClient;

namespace WebReport.Reports.PC
{
    public partial class WOCycleTime : System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.txtFromDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            //this.txtToDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
            {
                Response.Redirect(@"~\Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        protected void btnSerch_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;

            if (this.txtFromDate.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtToDate.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtFromDate.Text.Trim().Replace("'", "") != "" && this.txtToDate.Text.Trim().Replace("'", "") != "")
            {
                DateTime fr = new DateTime();
                DateTime to = new DateTime();
                bool bTimeFormatError = false;

                if (!DateTime.TryParse(this.txtFromDate.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "開始時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!DateTime.TryParse(this.txtToDate.Text.Trim().Replace("'", ""), out to))
                {
                    strErrorMsg += "結束時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!bTimeFormatError)
                {
                    TimeSpan s = to - fr;
                    if (fr > to)
                    {
                        strErrorMsg += "結束時間在開始時間之前！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }

                }

                try
                {
                    StringBuilder stb = new StringBuilder();
                    ReportViewer.Visible = true;

                    RecordLog Log = new RecordLog();
                    if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                    {
                        this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                    }
                    stb.Append("SELECT A.PRODID,A.T9NCNO,A.LOTID,A.LOTTYPE,A.Total_SF,A.Total_FIN,A.Total_ASY,A.Total_TPT,A.Total_SFSTORE,A.Total_ASYSTORE,A.Total");
                    //if (prodid.Length != 0)
                    //{
                    //    prodid = prodid.Substring(0, prodid.Length - 1);
                    //    stb.Append(" and A.prodid in (" + prodid + ")");
                    //}
                    stb.Append(" FROM WOCYCT (NOLOCK) A,New_SWOM B");
                    stb.Append(" WHERE A.LOTID=B.WONBR");
                    stb.Append(" AND A.PRODID=B.PRODID");
                    stb.Append(" AND A.T9NCNO=B.T9NCNO");
                    stb.Append(" AND A.LOTTYPE=B.LOTTYPE");
                    stb.Append(" AND A.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
                    stb.Append(" AND B.WOSTATUS='FORC'");
                    stb.Append(" AND CONVERT(varchar(10),CLOSEDATE,120) BETWEEN '" + this.txtFromDate.Text + "' AND '" + this.txtToDate.Text + "'");
                    stb.Append(" ORDER BY B.CLOSEDATE");

                    DataTable dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];

                    if (dt.Rows.Count == 0)
                    {
                        lblErrMsg.Text = "查無資料!";
                        ReportViewer.Visible = false;
                    }
                    else
                    {
                        lblErrMsg.Visible = false;
                        ReportViewer.Visible = true;
                        this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
                        localReportRoqcs.ReportPath = @"RDLC\WOCycleTime.rdlc";
                        ReportDataSource rsd = new ReportDataSource("WOCycleTime", dt);
                        localReportRoqcs.DataSources.Clear();
                        localReportRoqcs.DataSources.Add(rsd);
                    }
                }
                catch (Exception ex)
                {
                    this.lblErrMsg.Text = ex.Message;
                }
            }
        }
    }
}
