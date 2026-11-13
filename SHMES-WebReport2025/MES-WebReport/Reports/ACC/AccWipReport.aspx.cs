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
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Text;


namespace WebReport.Reports
{
    public partial class AccWipReport : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        protected void Seac_but_Click(object sender, EventArgs e)
        {
                if (this.txtDate.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的日期！');</script>");
            }
            else
            {
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                DataTable dt = new DataTable();

                StringBuilder sb = new StringBuilder();
                sb.Append("select cworkctr,cshopcd,'W' as bankwip ,prodid,wonbr,entityid,lotgrade,lotlevel,lottype,entitytqty");
                sb.Append(",datediff(hour,cast(left(transid,23) as datetime),getdate()) as staytime ");
                sb.Append("from NJPLCM2HIS.eif.wltm (NOLOCK) where mfgdate='" + this.txtDate.Text + "'  and hideflg = '0' ");
                sb.Append("and lotgrade in ('A','R')");

                dt = this.dbConn.GetDataSetBySql(sb.ToString()).Tables[0];

               
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\ACCWIPReport.rdlc";

                
                ReportDataSource rsd = new ReportDataSource("AccReport", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }
        }


    }
}
