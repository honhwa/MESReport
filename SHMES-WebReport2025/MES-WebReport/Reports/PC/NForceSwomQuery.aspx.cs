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
    public partial class NForceSwomQuery : System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    stb.Append("select TRANSDATE,PRODID,N9NCNO,WONBR,CWORKCTR as WORKCENTER,CWORKCTR,ENTITYTQTY FROM New_WLTM WHERE FABID ='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND CWORKCTR ='8800' AND WONBR IN ( ");
                    stb.Append(" select WONBR From New_swom where fabid ='" + ((UserInfo)Session["UserInfo"]).FabId + "' and wostatus ='FORC'  ");
                    stb.Append(" and (substring(prodid,1,9) in (select soprodid From mspm where class in ('PANEL')) ");
                    stb.Append("  or substring(prodid,1,13) in (select soprodid From mspm where class in ('PANEL')))  ");
                    stb.Append("  and 'A' + substring(wonbr,2,len(wonbr)-1) not in (select wonbr from New_swom) ");
                    stb.Append(" and CONVERT(varchar(10),CLOSEDATE,120) BETWEEN '" + this.txtFromDate.Text + "' AND '" + this.txtToDate.Text + "' ) ");

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
                        localReportRoqcs.ReportPath = @"RDLC\NForceSwomQuery.rdlc";
                        ReportDataSource rsd = new ReportDataSource("NForceSwom", dt);
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
