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
using System.Data.OracleClient;
using Report.Library;

namespace Report.Reports.TPT
{
    public partial class tptmpmgm1 : System.Web.UI.Page
    {

        OracleConnection Oracon;
        DBConn dbcom = new DBConn();
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
            Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            string strConnectionString = cfg.AppSettings.Settings["Commhis"].Value;
            Oracon = new OracleConnection(strConnectionString);

            string sqlstr = " INSERT INTO MPMGM(FABID,PRODID,GROUPNAME) SELECT FABID,PRODID,'' FROM MPIM WHERE PRODID NOT IN (SELECT PRODID FROM MPMGM) AND FABID='5' ";
            //try
            //{
                DataSet dsInsert = new DataSet();
                OracleDataAdapter Orada = new OracleDataAdapter(sqlstr, Oracon);
                Orada.Fill(dsInsert);
            //}
            //catch
            //{
            //    this.Response.Write("<script language=javascript>window.alert('數據操縱失敗！');</script>");
            //    return;
            //}

            ShowGvData();
        }
        private void ShowGvData()
        {

            string sqlgv = "SELECT fabid,prodid,groupname FROM MPMGM ORDER BY prodid";
            DataSet dsgv = new DataSet();
            OracleDataAdapter Orada = new OracleDataAdapter(sqlgv, Oracon);
            Orada.Fill(dsgv, "MPMGMall");
            this.gvAll.DataSource = dsgv.Tables[0];
            this.gvAll.DataBind();
        }

        protected void ddlgroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvAll_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            string sgroupname = this.ddlgroup.Text.Trim();
            int index = Convert.ToInt32(e.CommandArgument);
            string sprodid = this.gvAll.Rows[index].Cells[1].Text.ToString();
            string sqlstr = " UPDATE MPMGM SET GroupName='" + sgroupname + "' WHERE Prodid = '" + sprodid + "'";

            try
            {
                DataSet dsUpdate = new DataSet();
                OracleDataAdapter Orada = new OracleDataAdapter(sqlstr, Oracon);
                Orada.Fill(dsUpdate);
            }
            catch
            {
                this.Response.Write("<script language=javascript>window.alert('數據操縱失敗！');</script>");
                return;
            }
            ShowGvData();
            this.gvAll.SelectedIndex = index;
        }

        protected void ddlgroup_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }
    }
}
