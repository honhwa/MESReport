using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;
using System.Web.Hosting;
using System.Data.SqlClient;
using Report.Library;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports
{
	/// <summary>
	/// FrmWipSummary 的摘要描述。
	/// </summary>
	public partial class FrmEQSMMReportQuery : System.Web.UI.Page
	{

        DBConn dbConn = new DBConn();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在這裡放置使用者程式碼以初始化網頁
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    this.Response.Write("<script language=javascript>window.alert('請先登陸,再使用,謝謝');</script>");
                    Response.Redirect("../../Login.aspx");
                }
                this.TextBox1.Text = this.Request.QueryString["FrTime"].ToString();
                this.TextBox2.Text = this.Request.QueryString["ToTime"].ToString();
                this.TextBox3.Text = this.Request.QueryString["Process"].ToString(); 

                BindDataToDataGrid();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
		}

		#region Web Form 設計工具產生的程式碼
        //override protected void OnInit(EventArgs e)
        //{
        //    //
        //    // CODEGEN: 此為 ASP.NET Web Form 設計工具所需的呼叫。
        //    //
        //    InitializeComponent();
        //    base.OnInit(e);
        //}
		
		/// <summary>
		/// 此為設計工具支援所必須的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
        //private void InitializeComponent()
        //{    
        //    this.ImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.ImageButton2_Click);
        //    this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

        //}
		#endregion
		private void BindDataToDataGrid()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
			StringBuilder sql=new StringBuilder();

            sql.Append(" SELECT SUBSTRING(CONVERT(VARCHAR ,TRANSDATE,20),1,10) AS DDATE,D.PRODGROUP AS GROUPNAME,case when B.PROCID='CELL2' then A.workctr else b.procid end AS PROCESS,A.SHIFTID,A.SUBLINEID as EQID,A.Prodid,A.EQCCD AS EQCD,C.EQDES AS EBADESC,C.EQCDDESC,CONVERT(VARCHAR,A.STARTDATE,20) AS STARTDATE,CONVERT(VARCHAR,A.ENDDATE,20) AS ENDDATE");
            sql.Append(" ,round(convert(numeric(30,2),datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end)))/60,0) as DTime");
            sql.Append(" ,E.USERNAME AS USERID");
            sql.Append(" ,+A.PRODID+'  '+SUBSTRING(CONVERT(VARCHAR ,TRANSDATE,108),1,5)+'  '+C.EQDES+'  '+C.EQCDDESC+'  '+a.cmt as cmt");
            sql.Append(" FROM ");
            sql.Append(" (");
            sql.Append(" SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,Prodid ");
            sql.Append(" FROM REP.ESLSH WHERE  FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sql.Append(" UNION ");
            sql.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,GETDATE()  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,Prodid ");
            sql.Append(" FROM REP.ESLS WHERE   FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sql.Append(" ) A ");
            sql.Append(" inner JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            sql.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD)");
            sql.Append(" LEFT join dbo.memm E on (A.FABID=E.FABID And A.userid=E.userid) ");
            sql.Append(" LEFT join MPIM D on (A.FABID=D.FABID And A.PRODID=D.PRODID)");
            sql.Append(" WHERE A.FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sql.Append(" AND (case when a.enddate is null then getdate() else a.enddate end)>convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) AND A.STARTDATE<=convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) "); ;

            sql.Append(" AND CASE WHEN A.SHOPCD = 'CELL2' THEN B.WORKCTR ELSE  b.procid  END ='" + this.TextBox3.Text.Trim() + "'");
            
            sql.Append(" order by D.PRODGROUP,B.PROCID,A.SUBLINEID,A.Prodid,a.startdate,a.enddate ");

            SqlConnection myConn4 = new SqlConnection(dbConn.ConnectionString);
            SqlDataAdapter strdf = new SqlDataAdapter(sql.ToString(), myConn4);
            DataSet strdt = new DataSet();
            myConn4.Open();
            strdf.Fill(strdt, "A");
            myConn4.Close();


            //this.DataGrid1.DataSource = strdt;
            //this.DataGrid1.DataBind();
            DataTable dt = this.dbConn.GetDataSetBySql(sql.ToString()).Tables[0];

            this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\EQSMMQuery.rdlc";


            ReportDataSource rsd = new ReportDataSource("EQSMMQuery", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);

		}
		private void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Clear();
			Response.Buffer= false;
			Response.Charset="UTF-8";
			Response.ContentType="Content-Language;content=UTF-8";
			Response.AppendHeader("Content-Disposition","attachment;filename=addNewWorker.xls");
			Response.ContentEncoding=System.Text.Encoding.UTF7;  
			Response.ContentType = "application/ms-excel";   
			this.EnableViewState = false;
			System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.ReportViewer.RenderControl(oHtmlTextWriter);
			Response.Output.Write(oStringWriter.ToString());
			Response.End();
		}
		public override void VerifyRenderingInServerForm(Control control)   
		{
			return;   //強迫執行匯出excel
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			e.Item.Attributes.Add("onmouseover","currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
			e.Item.Attributes.Add("onmouseout","this.style.backgroundColor=currencol");
		}
		public void Button1_Click(object sender, System.EventArgs e)
		{
			BindDataToDataGrid();
		}
	}
}
