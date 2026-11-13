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
    public partial class IssueinfoQuary : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                
                this.txtDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.txtToDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
                if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
                {
                    Response.Write("No FabId!");
                    Response.Redirect("../../Login.aspx");
                }
            }
        }

        protected void Seac_but_Click(object sender, EventArgs e)
        {
            if (this.txtDate.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的開始日期！');</script>");
                return;
            }
            if (this.txtToDate.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的結束日期！');</script>");
                return;
            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT TRANSID,USERID,DESCRIBE,PRODID,PROCESSID,EQID,LOTID,LCTYPE,RECIPENO,LCCOUNT,RYCOUNT");
            sb.Append(" from bs_wlogm where FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND   CONVERT(CHAR(10),transid,120)  BETWEEN '" + this.txtDate.Text.Trim() + "' AND '" + this.txtToDate.Text.Trim() + "'");
            /*if (this.chkProdid.Checked == true)    FABID='"+((UserInfo )Session["UserInfo "]).FabId  +"' AND
            {
                sb.Append(" AND a.prodid='" + this.ddlProdid.Items[this.ddlProdid.SelectedIndex].Value.Trim() + "'");
            }
            if ((this.chkLotId.Checked == true) && (this.txtlotid.Text != string.Empty))
            {
                sb.Append(" AND b.lotid='" + this.txtlotid.Text.Trim() + "'");
            }
            if ((this.ChkUserid.Checked == true) && (this.txtuserid.Text != string.Empty))
            {
                sb.Append(" AND a.userid='" + this.txtuserid.Text.Trim() + "'");
            }
            if (this.ChkEQID.Checked == true)
            {
                sb.Append(" AND a.eqid='" + this.ddlEqid.Items[this.ddlEqid.SelectedIndex].Value.Trim() + "'");
            }*/





            dt = this.dbConn.GetDataSetBySql(sb.ToString()).Tables[0];


            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\IssueinfoQuary.rdlc";


            ReportDataSource rsd = new ReportDataSource("DataSet5_BS_WLOGM", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
            
        }
    }
}
