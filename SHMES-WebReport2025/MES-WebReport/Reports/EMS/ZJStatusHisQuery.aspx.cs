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
using System.Data.SqlClient;
using Report.Library;
using System.Text;

//-------------------------------------------------------------------------------------------------------------------------------
//Last Modify Date    Author          Version     Description
//--------------------    -------------   -----------  ----------------------------------------------------------------------------
//  2012/08/01         TPO/Li.Miao    1.00         由之前的GridView變更為ReportViewer呈現數據
//-------------------------------------------------------------------------------------------------------------------------------


//namespace Report.Reports.EMS        1.00 
namespace WebReport.Reports.EMS
{
    public partial class ZJStatusHisQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UserInfo"] == null) || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Redirect("../../Login.aspx");
            }
            if (!this.IsPostBack)
            {
                this.txtFromTime.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd hh:mm:ss");
                this.txtToTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                EQIDQuery();
            }
        }
        private void EQIDQuery()
        {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" select EQID from WZJM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY EQID  ");
            DataTable dt = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.DropDownList1.DataSource = dt;
                this.DropDownList1.DataTextField = "EQID";
                this.DropDownList1.DataBind();
            }
            this.DropDownList1.Items.Add("ALL");
            this.DropDownList1.SelectedValue = "ALL";
        }
        private void EQStatusHisQuery()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder sql2 = new StringBuilder();
            sql2.Append(" SELECT  CONVERT(VARCHAR(10),STARTDATE,120) AS date,SHIFTID,EQID,PRODID,STATUS,EQCDDESC as StatusDesc,CONVERT(VARCHAR(20),STARTDATE,120) AS STARTDATE,ISNULL(CONVERT(VARCHAR(20),ENDDATE,120),'') AS ENDDATE, ");
            sql2.Append(" DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + txtFromTime.Text + "' THEN  '" + txtFromTime.Text + "' ELSE STARTDATE END,ISNULL(ENDDATE,  '" + txtToTime.Text + "' )) AS DMINUTE,USERID,CMT  ");
            sql2.Append(" FROM WZJX A  LEFT JOIN MECCU B ON(A.STATUS = B.EQCCDESC)    ");
            sql2.Append(" LEFT JOIN MECM  C ON(A.FABID=C.FABID AND A.SUBEQCCD=C.EQCD)    ");
            sql2.Append(" WHERE A.FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "'   ");
            if (this.DropDownList1.SelectedValue.Trim() != "ALL")
            {
                sql2.Append(" AND A.EQID =  '" + DropDownList1.SelectedValue.Trim() + "'  ");
            }
            sql2.Append(" AND  CASE WHEN ENDDATE IS NULL THEN GETDATE() ELSE ENDDATE END >=   '" + txtFromTime.Text + "'   ");
            sql2.Append(" AND STARTDATE <=  '" + txtToTime.Text + "'   ");
            sql2.Append(" ORDER BY EQID,STARTDATE  ");
            DataTable DT2 = this.dbConn.GetDataSetBySql(sql2.ToString()).Tables[0];
            //1.00 Begin By Li.Miao
            //this.GridView1.DataSource = DT2;
            //this.GridView1.DataBind();

            this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\ZJStatusHisQuery.rdlc";

            ReportDataSource rsd = new ReportDataSource("ZJStatusHisQuery", DT2);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
            //1.00 End
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (this.txtFromTime.Text.Trim() == "")
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇開始時間,謝謝');</script>");
                return;
            }
            if (this.txtToTime.Text.Trim() == "")
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇結束時間,謝謝');</script>");
                return;
            }
            EQStatusHisQuery();
        }

        //1.00 Begin By Li.Miao Mark
        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Clear();
        //    Response.Buffer = false;
        //    Response.Charset = "UTF-8";
        //    Response.ContentType = "Content-Language;content=UTF-8";
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        //    Response.ContentEncoding = System.Text.Encoding.UTF7;
        //    Response.ContentType = "application/ms-excel";
        //    this.EnableViewState = false;
        //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        //    this.GridView1.RenderControl(oHtmlTextWriter);
        //    Response.Output.Write(oStringWriter.ToString());
        //    Response.End();
        //}
        //1.00 End
    }
}
