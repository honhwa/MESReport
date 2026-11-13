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
using System.Data.SqlClient;


namespace WebReport.Reports
{
    public partial class VAPWIPReport : System.Web.UI.Page
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

                this.txtStartDT.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.TxtEndDT.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }
        
        protected void Seac_but_Click(object sender, EventArgs e)
        {
            if (DateIsOk() == false)
            {
                return;
            }

            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            SqlConnection myConn = new SqlConnection(dbConn.ConnectionString);
            SqlDataAdapter myda = new SqlDataAdapter("dbo.USP_RPT_VAPWIP", myConn);
            myda.SelectCommand.Parameters.AddWithValue("@Z_StartDateTime", this.txtStartDT.Text.Trim());
            myda.SelectCommand.Parameters.AddWithValue("@Z_EndDateTime", this.TxtEndDT.Text.Trim());
            myda.SelectCommand.Parameters.AddWithValue("@Z_CSHOPCD", this.DDLCSHOPCD.Text.Trim());
            myda.SelectCommand.Parameters.AddWithValue("@Z_NonV", this.DDLNonV.Text.Trim());
            myda.SelectCommand.Parameters.AddWithValue("@Z_FabId", ((UserInfo)Session["UserInfo"]).FabId);
            myda.SelectCommand.CommandType = CommandType.StoredProcedure;
            myda.SelectCommand.CommandTimeout = 0;
            DataSet myds = new DataSet();
            myConn.Open();
            myda.Fill(myds);
            myConn.Close();



            ReportViewer1.Visible = true;
            this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = this.ReportViewer1.LocalReport;
            localReport.ReportPath = @"RDLC\VAPWIP.rdlc";

            ReportDataSource rds = new ReportDataSource("VAPWIP_USP_RPT_VAPWIP", myds.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            if (myds.Tables[0].Rows.Count == 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Sorry, no data under this category！');</script>");
            }
            ReportViewer1.LocalReport.Refresh();
     

        }
        private Boolean DateIsOk()
        {

            if (this.txtStartDT.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇開始日期！');</script>");
                return false;
            }

            if (this.TxtEndDT.Text == String.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇結束日期！');</script>");
                return false;
            }
            DateTime fr ;
            DateTime to ;

            bool isdatefr=DateTime.TryParse(this.txtStartDT.Text.Trim().Replace("'", "") , out fr);
            bool isdateto=DateTime.TryParse(this.TxtEndDT.Text.Trim().Replace("'", "") , out to);
             if ( fr > to)
            {
                this.Response.Write("<script language=javascript>window.alert('結束日期不能大于開始日期!');</script>");
                return false;
            }
            return true;
      
        }
         
    }
}
