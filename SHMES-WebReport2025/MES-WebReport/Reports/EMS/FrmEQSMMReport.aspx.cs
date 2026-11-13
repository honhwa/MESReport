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
using Report.Library;

namespace WebReport.Reports
{
	/// <summary>
	/// FrmPScrapSummaryQuery 的摘要描述。
	/// </summary>
	public partial class FrmEQSMMReport : System.Web.UI.Page
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
                this.TextBox1.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
		}
		#region Web Form 設計工具產生的程式碼
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 此為 ASP.NET Web Form 設計工具所需的呼叫。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 此為設計工具支援所必須的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			if(Convert.ToDateTime(this.TextBox1.Text.ToString())>Convert.ToDateTime(this.TextBox2.Text.ToString()))
			{
				this.Response.Write("<script language=javascript>window.alert('開始時間應該小與結束時間');</script>");
				return;
			}
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
			this.Session["Name"]="Line Status Real-time Monitor";
		//	Log TLog=new Log(this.Session["pbfab"].ToString(),this.Session["Name"].ToString(),this.Session["userid"].ToString());
            string url;
			if(this.RadioButtonList2.Items[0].Selected==true)
			{
                
                url="../EMS/FrmEQSMMReportQuery.aspx?FrTime=" + this.TextBox1.Text.Trim() + "&ToTime=" + this.TextBox2.Text.Trim()+"&process="+this.DropDownList2.SelectedValue;
                Response.Redirect(url);
			}
			else
			{
                url = "../EMS/FrmEQSMMSummary.aspx?FrTime=" + this.TextBox1.Text.Trim() + "&ToTime=" + this.TextBox2.Text.Trim() + "&process=" + this.DropDownList2.SelectedValue;
                Response.Redirect(url);
 			}
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			this.TextBox1.Text=System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
			this.TextBox2.Text=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			this.DropDownList2.SelectedIndex=0;
		}

		
		public string FrTime
		{
			get
			{
				return this.TextBox1.Text.Trim();
			}
		}
		public string ToTime
		{
			get
			{
				return this.TextBox2.Text.Trim();
			}
		}
		public string Process
		{
			get
			{
				return this.DropDownList2.SelectedValue.Trim();
			}
		}
	}
}
