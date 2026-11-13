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
    public partial class QueryGroupUserMapping : System.Web.UI.Page
    {
        //DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LoadDdlGroup();
            }
        }

        private void LoadDdlGroup()
        {
            string sql = "select udepart , udepart_name from MESReport_RoleSet";
            DBConn dbConn = new DBConn();
            DataSet ds = dbConn.GetDataSetBySql(sql);
            this.ddlGroup.DataSource = ds;
            this.ddlGroup.DataBind();
        }

        private void Query()
        {
            //20240322 penny : change to use dblink
            string sql = @"select userid, username,title from [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] where title = '" + this.ddlGroup.SelectedValue + "'";
            DBConn dbConn = new DBConn();
            DataSet ds = dbConn.GetDataSetBySql(sql);
            this.gvResult.DataSource = ds;
            this.gvResult.DataBind();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query();
        }
    }
}