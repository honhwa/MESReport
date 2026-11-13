using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Report.Library;

namespace Report.RoleSetMgt
{
    public partial class EditGroupMenuMapping : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null;
            //UserName = Convert.ToString(Session["USER_NAME"]);
            //UserName = ((UserInfo)Session["UserInfo"]).UserID;
            //UIName = "群組權限設置";
            //驗證登陸用戶是否有權限訪問頁面
            //StringBuilder sbSql = new StringBuilder();
            //sbSql.Append(" SELECT * ");            
            //sbSql.Append(" FROM MESReport_UserRole ");            
            //sbSql.Append(" WHERE ");
            //sbSql.Append(" URL LIKE '%EditGroupMenuMapping.aspx' ");
            //sbSql.Append(" AND ROLE IN('" + ((UserInfo)Session["UserInfo"]).Role + "')");            
            //try
            //{
            //    ds = dbConn.GetDataSetBySql(sbSql.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    ds.Dispose();
            //    //Response.Redirect("../../FORM/ERROR.ASPX");
            //    this.lbMsg.Text = "";

            //}

            if (Session["UserInfo"] == null)
            {
                this.lbMsg.Text = "連線逾時，請重新登入.";
            }
            else
            {
                if (!IsPostBack)
                {
                    string sql = "select udepart_name from MESReport_RoleSet";
                    ds = dbConn.GetDataSetBySql(sql);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        this.DropDownList1.Items.Add(dr[0].ToString());
                    }
                    Show();
                }
            }


        }
        private void Show()
        {
            try
            {
                string sql = "select id,fmenu,cmenu,url,fmenu_id from MESReport_Menu order by fmenu";
                DataSet ds = dbConn.GetDataSetBySql(sql);
                GridView1.DataSource = ds;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Selected_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox allSelected = (CheckBox)sender;

            if (allSelected.Checked)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    CheckBox selected = (CheckBox)GridView1.Rows[i].FindControl("Selected");

                    selected.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    CheckBox selected = (CheckBox)GridView1.Rows[i].FindControl("Selected");

                    selected.Checked = false;
                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox selected = (CheckBox)GridView1.Rows[i].FindControl("Selected");
                selected.Checked = false;
                string cname = ((Label)GridView1.Rows[i].FindControl("lbl_id1")).Text.Trim();
                string jname = ((Label)GridView1.Rows[i].FindControl("lbl_id2")).Text.Trim();
                string sql = "select * from MESReport_UserRole where fmenu='" + cname + "' and cmenu='" + jname + "' and ";
                sql = sql + "role=(select udepart from MESReport_RoleSet where udepart_name='" + this.DropDownList1.SelectedItem.Text + "')";
                DataSet ds = dbConn.GetDataSetBySql(sql);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    selected.Checked = true;
                }
            }
        }

        protected void IBtn_send_Click(object sender, ImageClickEventArgs e)
        {
            if (this.DropDownList1.SelectedItem.Text == "---請選擇---")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇群組名稱！');</script>");
                return;
            }

            string sql = "delete from MESReport_UserRole where role=(select udepart from MESReport_RoleSet where udepart_name='" + this.DropDownList1.SelectedItem.Text + "') ";
            dbConn.ExecuteSql(sql);
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox cbox = (CheckBox)GridView1.Rows[i].FindControl("Selected");
                if (cbox.Checked == true)
                {
                    try
                    {
                        string cname = ((Label)GridView1.Rows[i].FindControl("lbl_id1")).Text.Trim();
                        string jname = ((Label)GridView1.Rows[i].FindControl("lbl_id2")).Text.Trim();
                        string jurl = ((Label)GridView1.Rows[i].FindControl("lbl_id3")).Text.Trim();
                        string jfmenu = ((Label)GridView1.Rows[i].FindControl("lbl_id4")).Text.Trim();
                        string sql1 = "insert into MESReport_UserRole(role,fmenu,cmenu,url,fmenu_id) values ";
                        sql1 = sql1 + "((select udepart from MESReport_RoleSet where udepart_name='" + this.DropDownList1.SelectedItem.Text + "'),";
                        sql1 = sql1 + "'" + cname + "','" + jname + "','" + jurl + "','" + jfmenu + "')";
                        dbConn.ExecuteSql(sql1);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('設置成功！');</script>");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            Show();
        }


    }
}
