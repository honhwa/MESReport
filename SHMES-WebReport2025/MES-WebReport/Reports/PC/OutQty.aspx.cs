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

namespace Report.Reports.Material
{
    public partial class OutQty : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.GridView1.Visible = false;
            this.FormView1.Visible = false;
            Label1.Visible = false;
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
            }
            ddlDate.Enabled = false;
            ddlfab.Enabled = false;
            ddlSub.Enabled = false;
            ddlProdid.Enabled = false;
            string userinfo = ((UserInfo)Session["userinfo"]).UserID;
            StringBuilder stb = new StringBuilder();
            stb.Append("select * from MUGMM where fabid = 'B' and usergroupcd = 'RPMF' and userid='"+userinfo+"'");
            DataTable dt = new DataTable();
            dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                btnAdd.Visible = false;
                GridView1.Columns[0].Visible = false;
            }
            else
            {
                btnAdd.Visible = true;
                GridView1.Columns[0].Visible = true;
            }

        }

        private void loadqty()
        {
            SqlDataSource1.SelectCommand = "select * from ROPPRD where 1=1";
            if (this.ChkFab.Checked)
            {
                SqlDataSource1.SelectCommand += " and Fabid='" + ddlfab.Text.Trim() + "'";
            }
            if (this.ChkDate.Checked)
            {
                if (ddlDate.Text.Trim().Replace("'", "") == "")
                {

                }
                else
                {

                    SqlDataSource1.SelectCommand += " and mfgdate='" + Convert.ToDateTime(ddlDate.Text.Trim ()).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
            }
            if (ChkSub.Checked)
            {
                if (ddlSub.Text.Trim().Replace("'", "") == "")
                {

                }
                else
                {

                    SqlDataSource1.SelectCommand += " and prodline='" + ddlSub.Text.Trim() + "'";
                }
            }
            if (this.ChkPro.Checked)
            {
                if (ddlProdid.Text.Trim().Replace("'", "") == "")
                {

                }
                else
                {

                    SqlDataSource1.SelectCommand += " and productid='" + ddlProdid.Text.Trim() + "'";
                }
            }

            if (ChkFab.Checked == false && ChkDate.Checked == false && ChkSub.Checked == false && ChkPro.Checked == false)
            {
                //SqlDataSource1.SelectCommand += "";
            }
            SqlDataSource1.SelectCommand += " order by dataend_time";
            SqlDataSource1.DataBind();
            GridView1.DataBind();
            if (GridView1.Rows.Count == 0)
            {
                Label1.Text = "查無資料！";
                Label1.Visible = true;
            }
            else
            {
                Label1.Visible = false;
            }
            this.GridView1.Visible = true;
        }


        //------賦值-------
        private void SetMFGDateDatasource()
        {
            string strsql = @"select mfgdate from ROPPRD";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find MFGDate！');</script>");
            }
            else
            {
                this.ddlDate.DataSource = dt;
                this.ddlDate.DataTextField = "mfgdate";
                this.ddlDate.DataValueField = "mfgdate";
                this.ddlDate.DataBind ();
            }
        }
        private void SetSubDatasource()
        {
            string strsql;
            strsql = @"select SUBLINEID from MSLM";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點所對應的線別！');</script>");
                ChkSub.Checked = false;
            }
            else
            {
                this.ddlSub.DataSource = dt;
                this.ddlSub.DataTextField = "SUBLINEID";
                this.ddlSub.DataValueField = "SUBLINEID";
                this.ddlSub.DataBind ();
            }
        }

        private void SetProductDatasource()
        {
            string strsql = @"select prodid from mpim";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Status！');</script>");
            }
            else
            {
                this.ddlProdid.DataSource = dt;
                this.ddlProdid.DataTextField = "prodid";
                this.ddlProdid.DataValueField = "prodid";
                this.ddlProdid.DataBind ();
            }
        }

        protected void ChkFab_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkFab.Checked)
            {
                ddlfab.Enabled = true;
            }
            else
            {
                ddlfab.Enabled = false;
            }

        }
        protected void ChkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkDate.Checked)
            {
                ddlDate.Enabled = true;
                this.SetMFGDateDatasource();
            }
            else
            {
                this.ddlDate.Items.Clear();
                ddlDate.Enabled = false;
            }
        }

        protected void ChkPro_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPro.Checked)
            {
                ddlProdid.Enabled = true;
                this.SetProductDatasource();
            }
            else
            {
                this.ddlProdid.Items.Clear();
                ddlProdid.Enabled = false;
            }
        }
        protected void ChkSub_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSub.Checked)
            {
                ddlSub.Enabled = true;
                this.SetSubDatasource();
            }
            else
            {
                this.ddlSub.Items.Clear();
                ddlSub.Enabled = false;
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //編輯之前
            this.GridView1.EditIndex = -1;
            this.GridView1.EditIndex = e.NewEditIndex;
            loadqty();
            this.GridView1.Visible = true;

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //編輯取消
            
            this.GridView1.EditIndex = -1;
            loadqty();
            this.GridView1.Visible = true;

        }
        //protected virtual void GridView1_OnRowDeleting(GridViewDeletedEventArgs e)
        //{
        //    this.GridView1.EditIndex = -1;
        //    loadqty();
        //    this.GridView1.Visible = true;
        //}

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            loadqty();
            GridView1.EditIndex = -1; 
            this.GridView1.Visible = true;
        }

        protected void Grid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            loadqty();

            this.GridView1.Visible = true;

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.FormView1.Visible = true;
            this.FormView1.ChangeMode(FormViewMode.Insert );
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {

        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {

        }

        protected void NewButton_Click(object sender, EventArgs e)
        {

        }

    }
}
