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
using ExcelIO;
using Report.Library;

namespace Report.RoleSetMgt
{
    public partial class EditUserGroupMapping : System.Web.UI.Page
    {        
        //DBConn dbConn = new DBConn();

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

                }
            }

            //else
            //{
            //    //驗證登陸用戶是否有權限訪問頁面
            //    StringBuilder sbSql = new StringBuilder();
            //    sbSql.Append(" SELECT * ");
            //    sbSql.Append(" FROM MESReport_UserRole ");
            //    sbSql.Append(" WHERE URL LIKE '%Set_permit.ASPX' ");
            //    sbSql.Append(" AND ROLE IN('" + ((UserInfo)Session["UserInfo"]).Role + "') ");
            //    DataSet ds = null;
            //    ds = dbConn.GetDataSetBySql(sbSql.ToString());
            //    if (ds.Tables[0].Rows.Count == 0)
            //    {
            //        this.lbMsg.Text = "無權限";
            //    }
            //}



        }

        //private void Show()
        //{            
        //    string sql = " SELECT ROW_NUMBER() OVER(ORDER BY a.USERID ASC) as 'RowNum' , username, udepart_name";
        //    sql += " FROM memm a ";
        //    sql += " LEFT JOIN MESReport_RoleSet b ON a.Title = b.udepart ";
        //    DataSet ds = dbConn.GetDataSetBySql(sql);
        //    this.GridView1.DataSource = ds;
        //    this.GridView1.DataBind();
        //    ds.Dispose();
        //}

        protected void BindGV()
        {
            DataSet ds;
            //20240322 penny : change to use dblink
            string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY a.USERID ASC) AS 'RowNumber' , '' as 'DEPT_NO' , a.userid , a.username , b.udepart_name , a.TITLE as 'ROLE' " ;
            sql += @" FROM [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] a ";
            sql += @" LEFT JOIN [SHMESHIS\MESHIS].[SHMESHIS].[dbo].[MESReport_RoleSet] b ON a.TITLE = b.udepart ";
            sql += @" WHERE (a.userid LIKE '%" + this.tbQuery.Text.Trim() + "' OR a.USERNAME LIKE '%" + this.tbQuery.Text.Trim() + "')";
            DBConn dbConnWIP = new DBConn();
            ds = dbConnWIP.GetDataSetBySql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('查詢無記錄,請確認查詢條件！');'';", true);
                return;
            }
        }

        protected void ibtnQuery_Click(object sender, ImageClickEventArgs e)
        {
            if (this.tbQuery.Text.Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('請輸入查詢條件！');'';", true);
                this.GridView1.DataSource = null;
                this.GridView1.DataBind();
            }
            else
            {
                this.BindGV();
            }            
        }

        protected void ibtnExport_Click(object sender, ImageClickEventArgs e)
        {            
            DataSet ds;
            //20240322 penny : change to use dblink
            string sql = @" SELECT a.USERID as '工號' ,a.USERNAME as '姓名', b.udepart_name as '群組名稱', a.TITLE as '群組代碼' ";
            sql += @" FROM [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] a ";
            sql += @" LEFT JOIN [SHMESHIS\\MESHIS].[SHMESHIS].[dbo].[MESReport_RoleSet] b ON a.TITLE = b.udepart ";
            if (this.tbQuery.Text.Trim() != "")
            {
                sql += " where (a.userid LIKE '%" + this.tbQuery.Text.Trim() + "%' OR a.USERNAME LIKE '%" + this.tbQuery.Text.Trim() + "%')";
            }
            DBConn dbConnWIP = new DBConn();
            ds = dbConnWIP.GetDataSetBySql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string filename = "人員權限表.xls";
                ExcelHelper.ExportByWeb(ds.Tables[0], "", filename);
            }
        }
      
        private void LoadGVRoleSetDDL(ref DropDownList pddlRole)
        {
            pddlRole.Items.Clear();
            pddlRole.Items.Insert(0, new ListItem("", string.Empty));
            DataSet ds;
            //string sql = " SELECT * FROM MESReport_RoleSet where UDEPART <> 'ADMIN' ORDER BY ID";
            string sql = " SELECT * FROM MESReport_RoleSet ORDER BY ID";
            DBConn dbConn = new DBConn();
            ds = dbConn.GetDataSetBySql(sql);
            pddlRole.DataSource = ds;
            pddlRole.DataBind();            
            pddlRole.Items.Insert(0, new ListItem("無群組", ""));
            pddlRole.SelectedIndex = 0;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
            //{
            //    //HiddenField hfRowIndex = new HiddenField();
            //    //Button btnClear = new Button();
            //    //lblTagLink = (LinkButton)e.Row.FindControl("btnAddTags");
            //    //lblTagLink.CommandArgument = e.Row.RowIndex.ToString();
            //    e.Row.RowIndex.ToString();
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddlRole");
                TextBox tbRole = (TextBox)e.Row.FindControl("tbRole");

                //Disable UI
                //tbRole.Attributes.Add("style", "display: none;");
                tbRole.Visible = false;
                tbRole.Enabled = false;                
                e.Row.Cells[1].Enabled = false;
                e.Row.Cells[2].Enabled = false;
                e.Row.Cells[3].Enabled = false;
                //e.Row.Cells[4].Enabled = false;

                //Select Current Group
                DataSet dsRole;
                //20240322 penny : change to use dblink
                string sql = @" SELECT a.UserID , b.udepart_name, a.TITLE as 'ROLE' FROM [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] a ";
                sql += @" LEFT JOIN [SHMESHIS\MESHIS].[SHMESHIS].[dbo].[MESReport_RoleSet] b ON a.TITLE = b.udepart ";
                sql += @" WHERE a.UserID='" + e.Row.Cells[2].Text.ToString() + "' ";
                if (tbRole.Text != "")
                {
                    sql += " AND b.udepart_name='" + tbRole.Text + "'";
                }
                DBConn dbConnWIP = new DBConn();
                dsRole = dbConnWIP.GetDataSetBySql(sql);
                if (dsRole.Tables[0].Rows.Count > 0)
                {
                    ddlRole.Items.Insert(0, new ListItem(dsRole.Tables[0].Rows[0]["udepart_name"].ToString(), dsRole.Tables[0].Rows[0]["ROLE"].ToString()));
                    ddlRole.SelectedIndex = 0;
                    ddlRole.Enabled = false;
                }
                else
                {
                    this.LoadGVRoleSetDDL(ref ddlRole);

                    //penny mark
                    //ddlRole.Items.Clear();
                    //ddlRole.Items.Insert(0, new ListItem("", string.Empty));
                    //DataSet dsRoleSet;
                    //string sqlRoleSet = "";
                    //sqlRoleSet += " SELECT * FROM MESReport_RoleSet where UDEPART <> 'ADMIN' ORDER BY ID";
                    //dsRoleSet = dbConn.GetDataSetBySql(sqlRoleSet);
                    //for (int i = 0; i < dsRoleSet.Tables[0].Rows.Count; i++)
                    //{

                    //    ddlRole.Items.Insert(i + 1, new ListItem(dsRoleSet.Tables[0].Rows[i]["UDEPART_NAME"].ToString(), dsRoleSet.Tables[0].Rows[i]["UDEPART"].ToString()));
                    //}
                    //ddlRole.SelectedIndex = 0;
                }

            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
            GridView1.EditIndex = e.NewEditIndex;
            this.BindGV();

             //this.GridView1.Rows[e.NewEditIndex].Cells[1].Enabled = false;
             //this.GridView1.Rows[e.NewEditIndex].Cells[2].Enabled = false;
             //this.GridView1.Rows[e.NewEditIndex].Cells[3].Enabled = false;
             //this.GridView1.Rows[e.NewEditIndex].Cells[4].Enabled = false;

             //DropDownList ddlRole = (DropDownList)GridView1.Rows[e.NewEditIndex].FindControl("ddlRole");

            //string ROLESET = "";
            //ROLESET = ROLESET + " SELECT * FROM MESReport_RoleSet ORDER BY ID";
            //DataSet ROLESETDS = DBTOOL.ExecuteQuery(ROLESET);
            //ddlrole.Items.Clear();
            //for (int i = 0; i < ROLESETDS.Tables[0].Rows.Count; i++)
            //{
            //    ddlrole.Items.Insert(i, new ListItem(ROLESETDS.Tables[0].Rows[i]["UDEPART_NAME"].ToString(), ROLESETDS.Tables[0].Rows[i]["UDEPART"].ToString()));

            //}

            //ddlRole.SelectedIndex = 0;
            //ddlRole.Enabled = true;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {  
            DropDownList ddlRole = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlRole");
            TextBox tbUserID = (TextBox)GridView1.Rows[e.RowIndex].Cells[2].Controls[0];
            DataSet ds;
            //string strSql = " SELECT * FROM [MEMM] WHERE userid='" + tbUserID.Text + "' AND title='"+ ddlRole.Text.ToString() + "' ";
            //20240322 penny : change to use dblink
            string strSql = @" SELECT * FROM [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] WHERE userid='" + tbUserID.Text + "' ";
            DBConn dbConn = new DBConn();
            ds = dbConn.GetDataSetBySql(strSql);
            //if (Convert.ToString(ddlRole.SelectedValue) == "" )
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('未選擇群組，不能設置.');'';", true);
            //    return;
            //}
            //if (((UserInfo)Session["UserInfo"]).Role.ToString() == "" )
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('連線逾時，請重新登入.');'';", true);
            //    return;
            //}
            //string strSelectRole = ddlRole.SelectedValue.ToString().Trim();
            //string strCurrentLoginRole = ((UserInfo)Session["UserInfo"]).Role.ToString();
            if (ds.Tables[0].Rows.Count == 1)
            {                
                strSql = @" UPDATE [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] SET TITLE='" + ddlRole.Text + "' WHERE userid='" + tbUserID.Text + "'";
                //int iRtn = dbConnWIP.ExecuteNonQuery(strSql);
                //if (iRtn != 0)
                //{
                //    GridView1.EditIndex = -1;
                //    BindGV();
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('設置成功');'';", true);
                //}

                //20240628 penny : DAO丟出下面error
                //      無法為連結伺服器 "SHMESWIP\MESWIP" 的 OLE DB 提供者 "SQLNCLI10" 啟動巢狀交易。因為 XACT_ABORT 選項已設定為 OFF，所以必須要有巢狀交易。
                //      連結伺服器 "SHMESWIP\MESWIP" 的 OLE DB 提供者 "SQLNCLI10" 傳回訊息 "交易中已經存在有徵募。"。
                //原因: SQL2008後有自動XACT_ABORT選項, 是否開啟自動rollback , 舊DAO需修改
                //解決方法: 1.懶得改DAO,打開自動rollback  或  2.不設SqlCommand Transaction
                //1.
                bool bRtn = dbConn.ExecuteSql("SET XACT_ABORT ON;" + strSql);
                //2.
                //bool bRtn = dbConnWIP.ExecuteSqlWithoutTranscation(strSql);
                if (bRtn)
                {
                    GridView1.EditIndex = -1;
                    BindGV();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('設置成功');'';", true);
                }

                //20220624 penny : mark
                //if (strRole.IndexOf("ADMIN")>-1)
                //{
                //    if (strSelectRole.IndexOf("ADMIN") > -1)
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('管理員不能提升其他人為管理員,不能賦權！');'';", true);
                //        return;
                //    }
                //    else
                //    {
                //        strInsertSql = strInsertSql + " INSERT INTO memm(userid,ROLE) VALUES ('" + tbEmpNo.Text + "','" + ddlRole.Text.ToString() + "')";
                //        //updatesql = updatesql + " UPDATE memm SET ROLE='" + ddlrole.Text.ToString() + "' WHERE userid='" + empno.Text + "'";
                //        int updatere = dbConn.ExecuteNonQuery(strInsertSql);
                //        if (updatere != 0)
                //        {
                //            GridView1.EditIndex = -1;
                //            Bind();
                //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('賦予權限Success');'';", true);
                //        }
                //    }
                //}
                //else
                //{
                //    if (strSelectRole.IndexOf("ADMIN") > -1)
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('非管理員不能提升其他人為管理員,不能賦權！');'';", true);
                //        return;
                //    }
                //    else
                //    {
                //        strInsertSql = strInsertSql + " INSERT INTO memm(userid,ROLE) VALUES ('" + tbEmpNo.Text + "','" + ddlRole.Text.ToString() + "')";
                //        //updatesql = updatesql + " UPDATE memm SET ROLE='" + ddlrole.Text.ToString() + "' WHERE userid='" + empno.Text + "'";
                //        int updatere = dbConn.ExecuteNonQuery(strUpdateSql);
                //        if (updatere != 0)
                //        {
                //            GridView1.EditIndex = -1;
                //            Bind();
                //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('賦予權限Success');'';", true);
                //        }
                //    }
                //}


                //if (Convert.ToString(ddlrole.SelectedValue) == "ADMIN")
                //{
                //    if (Convert.ToString(Session["ROLE"]) == "ADMIN")
                //    {
                //        updatesql = updatesql + " UPDATE memm SET ROLE='" + ddlrole.Text.ToString() + "' WHERE userid='" + empno.Text + "'";
                //        int updatere = DBTOOL.ExecuteNonQuery(updatesql);
                //        if (updatere != 0)
                //        {
                //            GridView1.EditIndex = -1;
                //            Bind();
                //        }
                //    }
                //    else
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('權限不足,不能賦予系統管理員權限');'';", true);
                //        return;
                //    }
                //}
                //else
                //{
                //    if (Convert.ToString(Session["ROLE"]) == "ADMIN")
                //    {
                //        updatesql = updatesql + " UPDATE memm SET ROLE='" + ddlrole.Text.ToString() + "' WHERE userid='" + empno.Text + "'";
                //        int updatere = DBTOOL.ExecuteNonQuery(updatesql);
                //        if (updatere != 0)
                //        {
                //            GridView1.EditIndex = -1;
                //            Bind();
                //        }
                //    }
                //    else
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('權限不足,不能賦予系統管理員權限');'';", true);
                //        return;
                //    }
                //}

            }
            else
            {

                GridView1.EditIndex = -1;
                BindGV();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "alert('查無資料，設置失敗!');'';", true);
               
            }
            
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string strEmpNo = this.GridView1.Rows[e.RowIndex].Cells[2].Text;
            //DropDownList ddlRole = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlRole");
            //string role = ddlRole.SelectedValue.ToString();
            DBConn dbConnWIP = new DBConn();
            //20240322 penny : change to use dblink
            string strSql = @" UPDATE [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] SET TITLE='' WHERE userid='" + strEmpNo + "'";
            int iRtn = dbConnWIP.ExecuteNonQuery(strSql);
            if (iRtn != 0)
            {
                GridView1.EditIndex = -1;
                BindGV();
            }
        }
        
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            this.BindGV();
        }

        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName =="RemoveGroup")
        //    {
        //        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent;
        //        row
        //        int index = gvRow.RowIndex;
        //        string strEmpNo = this.GridView1.Rows[index].Cells[2].Text;
        //        DropDownList ddlRole = (DropDownList)GridView1.Rows[index].FindControl("ddlRole");
        //        string role = ddlRole.SelectedValue.ToString();
        //        DBConn dbConnWIP = new DBConn(true);
        //        string strSql = " UPDATE [MEMM] SET TITLE='' WHERE userid='" + ((UserInfo)Session["UserInfo"]).UserID + "'";
        //        int iRtn = dbConnWIP.ExecuteNonQuery(strSql);
        //        if (iRtn != 0)
        //        {
        //            GridView1.EditIndex = -1;
        //            BindGV();
        //        }
        //    }
        //}
    }
}
