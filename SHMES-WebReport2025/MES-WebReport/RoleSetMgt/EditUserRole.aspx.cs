using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Report.Library;

namespace Report.RoleSetMgt
{
    public partial class EditUserRole : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                this.lbMsg.Text = "連線逾時，請重新登入.";
            }
            else
            {
                if (!this.IsPostBack)
                {
                    this.LoadGV();
                }
            }
        }

        private string buildQuerySql()
        {
            string sql = " select ROW_NUMBER() over (order by id asc) as 'RowNum' , * from MESReport_RoleSet where 1=1 ";
            if (this.tbGroupCode.Text != "")
            {
                sql += " and UDEPART='" + this.tbGroupCode.Text + "'";
            }
            if (this.tbGroupName.Text != "")
            {
                sql += " and UDEPART_NAME='" + this.tbGroupName.Text + "'";
            }
            return sql;
        }

        private void LoadGV()
        {            
            DataSet ds = dbConn.GetDataSetBySql(buildQuerySql());
            this.gvGroup.DataSource = ds;
            this.gvGroup.DataBind();
            this.lbMsg.Text = ds.Tables[0].Rows.Count.ToString();
            this.gvGroup.FooterRow.Visible = true;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.LoadGV();
        }

        protected void gvGroup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.gvGroup.EditIndex = e.NewEditIndex;
            this.LoadGV();
        }

        //private int updateMemmTitle()
        //{

        //}

        protected void gvGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            TextBox tbGroupCode_EIT = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("tbGroupCode_EIT");
            TextBox tbGroupName_EIT = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("tbGroupName_EIT");
            HiddenField hfID_EIT = (HiddenField)gvGroup.Rows[e.RowIndex].FindControl("hfID_EIT");
            HiddenField hfGroupCode_EIT = (HiddenField)gvGroup.Rows[e.RowIndex].FindControl("hfGroupCode_EIT");
            if (tbGroupCode_EIT.Text.Length >= 21)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('群組代碼字數不可超過20字');'';", true);
                //this.lbMsg.Text = "群組代碼字數不可超過20字";
                return;
            }
            if (tbGroupName_EIT.Text.Length >= 51)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('群組名稱字數不可超過20字');'';", true);
                //this.lbMsg.Text = "群組名稱字數不可超過50字";
                return;
            }
            //update UserRole Table            
            string strSql = " UPDATE MESReport_RoleSet set UDEPART='" + tbGroupCode_EIT.Text + "' , UDEPART_NAME = '" + tbGroupName_EIT.Text + "' WHERE id='" + hfID_EIT.Value + "'";
            int iRtn = dbConn.ExecuteNonQuery(strSql);
            if (iRtn == 1)
            {
                DBConn dbConnWIP = new DBConn();
                //20240322 penny : change to use dblink
                strSql = @"update [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] set title = '" + tbGroupCode_EIT.Text + "' where title = '" + hfGroupCode_EIT.Value + "'";
                int iRtnMemm = dbConnWIP.ExecuteNonQuery(strSql);
                //this.lbMsg.Text = "修改群組成功, MEMM表同時更新" + iRtnMemm.ToString() + "筆帳號";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('修改群組成功, MEMM表同時更新" + iRtnMemm.ToString() + "筆帳號');'';", true);
            }
            else if (iRtn==0)
            {
                //this.lbMsg.Text = "修改失敗, 未找到";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('修改失敗, 未找到');'';", true);
            }
            else
            {
                //this.lbMsg.Text = "異常，已修改" + iRtn.ToString() + "筆記錄";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('異常，已修改" + iRtn.ToString() + "筆記錄');'';", true);
            }
            this.gvGroup.EditIndex = -1;
            this.LoadGV();
        }

        protected void gvGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gvGroup.EditIndex = -1;
            this.LoadGV();
        }

        protected void btnInsert_FT_Click(object sender, EventArgs e)
        {
            TextBox tbGroupCode_FT = (TextBox)gvGroup.FooterRow.FindControl("tbGroupCode_FT");
            TextBox tbGroupName_FT = (TextBox)gvGroup.FooterRow.FindControl("tbGroupName_FT");
            //HiddenField hfID_EIT = (HiddenField)gvGroup.Rows[e.RowIndex].FindControl("hfID_EIT");
            if (tbGroupCode_FT.Text.Length >= 21)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('群組代碼字數不可超過20字');'';", true);
                //this.lbMsg.Text = "群組代碼字數不可超過20字";
                return;
            }
            if (tbGroupName_FT.Text.Length >= 51)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('群組名稱字數不可超過20字');'';", true);
                //this.lbMsg.Text = "群組名稱字數不可超過50字";
                return;
            }
            //insert UserRole Table
            string strSql = " insert into MESReport_RoleSet ([UDEPART],[UDEPART_NAME]) values ('" + tbGroupCode_FT.Text + "' ,  '" + tbGroupName_FT.Text + "') ";
            int iRtn = dbConn.ExecuteNonQuery(strSql);
            if (iRtn == 1)
            {
                //this.lbMsg.Text = "新增成功";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('新增成功');'';", true);                
            }
            else if (iRtn == 0)
            {
                //this.lbMsg.Text = "新增失敗, 未找到";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('新增失敗, 未找到');'';", true);
            }
            else
            {
                //this.lbMsg.Text = "異常，已新增" + iRtn.ToString() + "筆記錄";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('異常，已新增" + iRtn.ToString() + "筆記錄');'';", true);

            }

            this.LoadGV();
        }

        protected void gvGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TextBox tbGroupCode_EIT = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("tbGroupCode_EIT");
            TextBox tbGroupName_EIT = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("tbGroupName_EIT");
            HiddenField hfID_EIT = (HiddenField)gvGroup.Rows[e.RowIndex].FindControl("hfID_EIT");

            string strSql = " DELETE FROM [dbo].[MESReport_RoleSet] WHERE id='" + hfID_EIT.Value + "' and UDEPART='" + tbGroupCode_EIT.Text + "' and UDEPART_NAME = '" + tbGroupName_EIT.Text + "' ";
            int iRtn = dbConn.ExecuteNonQuery(strSql);
            if (iRtn == 1)
            {
                //this.lbMsg.Text = "刪除成功";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('刪除成功');'';", true);
            }
            else if (iRtn == 0)
            {
                //this.lbMsg.Text = "刪除失敗, 未找到";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('刪除失敗, 未找到');'';", true);
            }
            else
            {
                //this.lbMsg.Text = "已刪除" + iRtn.ToString() + "筆記錄";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('已刪除" + iRtn.ToString() + "筆記錄');'';", true);
            }
            this.gvGroup.EditIndex = -1;
            this.LoadGV();
        }
    }
}