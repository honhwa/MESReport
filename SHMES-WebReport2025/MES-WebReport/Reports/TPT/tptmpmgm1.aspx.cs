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
using Report.Library;

namespace Report.Reports.TPT
{
    public partial class tptmpmgm11 : System.Web.UI.Page
    {
        DBConn dbcom = new DBConn();
        private DataSet mydsall = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
                string sqlrtptu = "SELECT GroupName FROM RTPTU ";
                DataSet myds = new DataSet();
                myds = dbcom.GetDataSetBySql(sqlrtptu, "rtptu");

                ddlgroup.Items.Clear();

                for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
                {
                    ddlgroup.Items.Add(myds.Tables[0].Rows[i]["GroupName"].ToString());
                }

            }
            string sqlstr = " INSERT INTO MPMGM(FABID,PRODID,GROUPNAME) SELECT FABID,PRODID,'' FROM MPIM WHERE PRODID NOT IN (SELECT PRODID FROM MPMGM)";
            if (dbcom.ExecuteSql(sqlstr) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('數據操縱失敗！');</script>");
                return;
            }

            ShowGvData();


        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ShowGvData()
        {

            string sqlall = "SELECT fabid,prodid,groupname,0 as IsSelect FROM MPMGM ORDER BY prodid";
            mydsall = dbcom.GetDataSetBySql(sqlall, "MPMGMall");
            this.gvAll.DataSource = mydsall.Tables[0];
            this.gvAll.DataBind();
        }

        protected void DLall_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DLall_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void LstData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvAll_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtmpmgm_TextChanged(object sender, EventArgs e)
        {

        }

        protected void gvAll_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            string sGroupName = this.ddlgroup.Text.ToString();
            int index = Convert.ToInt32(e.CommandArgument);
            string sprodid = this.gvAll.Rows[index].Cells[1].Text.ToString();
            string sqlstr = " UPDATE MPMGM SET GroupName='" + sGroupName + "' WHERE Prodid = '" + sprodid + "'";

            if (dbcom.ExecuteSql(sqlstr) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('數據操縱失敗！');</script>");
                return;
            }
            ShowGvData();
            this.gvAll.SelectedIndex = index;
        }
    }
}
