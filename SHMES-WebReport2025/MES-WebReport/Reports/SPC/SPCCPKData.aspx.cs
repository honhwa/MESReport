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
using System.Text;
using Report.Library;
using Report.Sources;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;

namespace WebReport.Reports.SPC
{
    public partial class SPCCPKData : System.Web.UI.Page
    {
        SPCDBConn  dbConn = new SPCDBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            //GridView1.AllowPaging = true;
            //GridView1.PageSize = 20;
        }

        private void SetTime()
        {
            string strsql = @"select distinct time from dbo.VSPCCPKDATATEMP";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('資料表里沒有相應的時間!');</script>");
            }
            else
            {
                this.ddlTime.DataSource = dt;
                this.ddlTime.DataTextField = "time";
                this.ddlTime.DataValueField = "time";
                this.ddlTime.DataBind();
            }
        }

        protected void ChkDateType_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkDateType.Checked)
            {
                ddlType.Enabled = true;
                ddlType.Items.Add("Month");
                ddlType.Items.Add("Week");
            }
            else
            {
                ddlType.Enabled = false;
                ddlType.Items.Clear();
            }
        }

        protected void ChkTime_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkTime.Checked)
            {
                ddlTime.Enabled = true;
                this.SetTime();
            }
            else
            {
                ddlTime.Enabled = false;
                ddlTime.Items.Clear();
            }
        }

        protected void ChkFabID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkFabID.Checked)
            {
                ddlFabID.Enabled = true;
                ddlFabID.Items.Add("NJ1");
                ddlFabID.Items.Add("NJ2");
            }
            else
            {
                ddlFabID.Enabled = false;
                ddlFabID.Items.Clear();
            }
        }

        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataBound();
            //ImageButton1.Visible = true;
        }

        private void DataBound()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("select * from dbo.VSPCCPKDATATEMP where 1=1 and (ctrlitem in (select ctrlitem from SPC_SENDCN_CTRLITEM) or station ='8380')");
            if (ChkFabID.Checked == false && ChkDateType.Checked == false && ChkTime.Checked == false)
            {
                stb.Append("");
            }
            else
            {
                if (ChkFabID.Checked)
                {
                    stb.Append(" and BU='" + ddlFabID.Text.Trim() + "'");
                }
                if (ChkDateType.Checked)
                {
                    stb.Append(" and datetype=substring('"+ddlType.Text.Trim()+"',1,1)");
                }
                if (ChkTime.Checked)
                {
                    stb.Append(" and time='"+ddlTime.Text.Trim()+"'");
                }
            }
            stb.Append(" order by datetime");
            DataTable  dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];

            this.reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.reportViewer.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\SPCCPKDataInfo.rdlc";


            ReportDataSource rsd = new ReportDataSource("SPCCPKDataInfo", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);

            if (dt.Rows.Count == 0)
            {
                Label1.Visible = true;
                Label1.Text = "查無資料!";
            }
            else
            {
                Label1.Visible = false;
            }
        }

        //protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridView1.PageIndex = e.NewPageIndex;
        //    DataBound();
        //}

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.reportViewer.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
