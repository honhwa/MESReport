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
using Microsoft.Reporting.WebForms;
using Report.Library;

namespace WebReport.Reports
{
    public partial class LineProductStatus : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                //this.lblIp.Text = "IP: " + Request.UserHostAddress.ToString();
                //this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }

            #region LocalReport DataSource Setting

            this.reportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = reportViewer.LocalReport;
            localReport.ReportPath = @"RDLC\LineProductStatus.rdlc";


            Report.Sources.LineProductStatus s = new Report.Sources.LineProductStatus();
            s.FabId = ((UserInfo)Session["UserInfo"]).FabId;
            DataSet dataset = s.GetDataSet2();

            ReportDataSource dsLineProductStatus = new ReportDataSource();
            dsLineProductStatus.Name = "LineProductStatus";
            dsLineProductStatus.Value = dataset.Tables[0];
            localReport.DataSources.Clear();
            localReport.DataSources.Add(dsLineProductStatus);
            this.reportViewer.LocalReport.Refresh();
            #endregion
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            this.reportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = reportViewer.LocalReport;
            localReport.ReportPath = @"RDLC\LineProductStatus.rdlc";


            Report.Sources.LineProductStatus s = new Report.Sources.LineProductStatus();
            s.FabId = ((UserInfo)Session["UserInfo"]).FabId;
            DataSet dataset = s.GetDataSet2();

            ReportDataSource dsLineProductStatus = new ReportDataSource();
            dsLineProductStatus.Name = "LineProductStatus";
            dsLineProductStatus.Value = dataset.Tables[0];

            localReport.DataSources.Add(dsLineProductStatus);
        }
            
    }
}
