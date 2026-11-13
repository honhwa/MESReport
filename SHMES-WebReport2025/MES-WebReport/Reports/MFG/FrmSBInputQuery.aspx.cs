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
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using Report.Library;

namespace WebReport.Reports
{
    /// <summary>
    /// FrmSBInputQuery 的摘要描述。
    /// </summary>
    public partial class FrmSBInputQuery : System.Web.UI.Page
    {

        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.IsPostBack)
            {
                          if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                         {
                              Response.Redirect("../../Login.aspx");
                         }
                this.TextBox1.Text = System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
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

        protected void CheckBox2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.CheckBox2.Checked == true)
            {
                this.DropDownList2.Enabled = true;
                query();
            }
            else
            {
                this.DropDownList2.Enabled = false;
                this.DropDownList2.Items.Clear();
            }
        }
        protected void Button1_Click(object sender, System.EventArgs e)
        {
            if (Convert.ToDateTime(this.TextBox1.Text.ToString()) > Convert.ToDateTime(this.TextBox2.Text.ToString()))
            {
                this.Response.Write("<script language=javascript>window.alert('開始時間應該小與結束時間');</script>");
                return;
            }
            if (Convert.ToDateTime(this.TextBox1.Text.ToString()).AddMonths(1) < Convert.ToDateTime(this.TextBox2.Text.ToString()))
            {
                this.Response.Write("<script language=javascript>window.alert('請將查詢時間設在一個月內');</script>");
                return;
            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            Server.Transfer("FrmSBInput.aspx");
        }
        protected void Button2_Click(object sender, System.EventArgs e)
        {
            this.RadioButtonList1.Items[0].Selected=true;
            this.RadioButtonList1.Items[1].Selected=false;
            this.DropDownList2.Enabled = false;
            this.DropDownList3.Enabled = false;
            this.DropDownList4.Enabled = false;
            this.CheckBox2.Checked = false;
            this.CheckBox3.Checked = false;
            this.CheckBox4.Checked = false;
            this.DropDownList2.Items.Clear();
            this.DropDownList3.Items.Clear();
            this.DropDownList4.Items.Clear();
            this.TextBox1.Text = System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected void CheckBox3_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.CheckBox3.Checked == true)
            {
                this.DropDownList3.Enabled = true;
                query();
            }
            else
            {
                this.DropDownList3.Enabled = false;
                this.DropDownList3.Items.Clear();
            }
        }

        protected void CheckBox4_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.CheckBox4.Checked == true)
            {
                this.DropDownList4.Enabled = true;
                query();
            }
            else
            {
                this.DropDownList4.Enabled = false;
                this.DropDownList4.Items.Clear();
            }
        }

        private void query()
        {

            if (this.DropDownList2.SelectedValue == "" && this.CheckBox2.Checked == true)
            {
                string strSql = @"select distinct prodid from mpim where fabid='"+((UserInfo)Session["UserInfo"]).FabId+"'order by prodid";
                DataTable dt1 = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                this.DropDownList2.DataSource = dt1;
                this.DropDownList2.DataTextField = "prodid";
                this.DropDownList2.DataValueField = "prodid";
                this.DropDownList2.DataBind();
            }

            if (this.DropDownList3.SelectedValue == "" && this.CheckBox3.Checked == true)
            {
                string strSql2 = "";
                if (this.CheckBox2.Checked == true)
                {
                    strSql2 = @"select distinct wonbr from New_swom where fabid='"+((UserInfo)Session["UserInfo"]).FabId+"' and prodid='"+this.DropDownList2.SelectedItem.Text+"'order by wonbr";
                }
                else
                {
                    strSql2 = @"select distinct wonbr from New_swom where fabid='" + ((UserInfo)Session["UserInfo"]).FabId+"'order by wonbr";

                }
                DataTable dt2 = this.dbConn.GetDataSetBySql(strSql2).Tables[0];
                this.DropDownList3.DataSource = dt2;
                this.DropDownList3.DataTextField = "wonbr";
                this.DropDownList3.DataValueField = "wonbr";
                this.DropDownList3.DataBind();
            }

            if(this.DropDownList4.SelectedValue=="" && this.CheckBox4.Checked==true)
            {


                 string strSql3 = @"select distinct Customerid from MCUM where fabid='"+((UserInfo)Session["UserInfo"]).FabId+"'order by Customerid";
                 DataTable dt3 = this.dbConn.GetDataSetBySql(strSql3).Tables[0];
                 this.DropDownList4.DataSource = dt3;
                 this.DropDownList4.DataTextField = "Customerid";
                 this.DropDownList4.DataValueField = "Customerid";
                 this.DropDownList4.DataBind();

            }
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
                public string Prodid
                {
                    get
                    {
                        return this.DropDownList2.SelectedValue.Trim();
                    }
                }
                public string Wonbr
                {
                    get
                    {
                        return this.DropDownList3.SelectedValue.Trim();
                    }
                }
                public string Customer
                {
                    get
                    {
                        return this.DropDownList4.SelectedValue.Trim();
                    }
                }
                  public string Type
                 {
                     get
                     {
                         return this.RadioButtonList1.SelectedValue.Trim();
                     }
                 }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}