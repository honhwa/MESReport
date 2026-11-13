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
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Text;


namespace WebReport.Reports
{
    public partial class LcRecyclequary : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        DBConn dbprodconn=new DBConn ();
        DBConn dblctconn=new DBConn ();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            string sqlprod = " select SOPRODID AS PRODID from MPMPM where  SOPRODID LIKE 'AP%' UNION select SOPRODID AS PRODID from MPMPM where SOPRODID LIKE 'VAP%'UNION select SOPRODID AS PRODID from MPMPM where  SOPRODID LIKE 'TA%' UNION select SOPRODID AS PRODID from MPMPM where  SOPRODID LIKE 'VTA%' ORDER BY SOPRODID";
            DataTable dtprod = new DataTable();
            dtprod = this.dbprodconn.GetDataSetBySql(sqlprod).Tables[0];
            this.ddlProdid.DataSource = dtprod;
            this.ddlProdid.DataTextField = "PRODID";
            this.ddlProdid.DataValueField = "PRODID";
            this.ddlProdid.DataBind();
            string sqllctype = "select lcpartid from mlcm WHERE fabid= '" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            DataTable dtlct=new DataTable();
            dtlct =this.dblctconn.GetDataSetBySql(sqllctype).Tables[0];
            this.ddlLctype.DataSource = dtlct;
            this.ddlLctype.DataTextField = "lcpartid";
            this.ddlLctype.DataValueField = "lcpartid";
            this.ddlLctype.DataBind();
        }

        private void SetDdl()
        {

            this.txtlotid.Text = "";
            this.txtuserid.Text = "";
            this.txtbottle.Text = "";
            this.CheckLotID.Checked = false;
            this.ChkUserid.Checked = false;
            this.chkBottle.Checked = false;

        }
        protected void Seac_but_Click(object sender, EventArgs e)
        {
            if (this.txtDate.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的開始日期！');</script>");
                return;
            }
            if (this.txtToDate.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的結束日期！');</script>");
                return;
            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string sholdflg = this.ddlHoldFlag.SelectedIndex.ToString();
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append(" select A.TRANSID,A.bottlesn,a.userid,a.lotid,a.prodid,a.lctypeid,a.vresist,a.vtni,  ");
            sb.Append(" A.resist_min,a.resist_max,a.tni_min,a.tni_max,a.commt,a.holdflg ");
            sb.Append(" from bs_wrcxm a where  a.fabid= '" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sb.Append("and CONVERT(CHAR(10),a.transid,120)  BETWEEN '" + this.txtDate.Text.Trim() + "' AND '"+this.txtToDate.Text.Trim()+"'");
            sb.Append("and a.holdflg='" + sholdflg + "'");
            if (this.chkProdid.Checked == true)
            {
                sb.Append(" AND a.prodid='" + this.ddlProdid.Items[this.ddlProdid.SelectedIndex].Value.Trim() + "'");
            }
            if (this.ChkLCTYPE .Checked == true)
            {
                sb.Append(" AND a.lctypeid='" + this.ddlLctype .Items[this.ddlLctype .SelectedIndex].Value.Trim() + "'");
            }
            if ((this.CheckLotID.Checked == true) && (this.txtlotid.Text != string.Empty))
            {
                sb.Append(" AND a.lotid='" + this.txtlotid.Text.Trim() + "'");
            }
            if ((this.ChkUserid.Checked == true ) && (this.txtuserid.Text != string.Empty))
            {
                sb.Append(" AND a.userid='"+this.txtuserid.Text.Trim()+"'");
            }
            if ((this.chkBottle.Checked == true) && (this.txtbottle.Text != string.Empty))
            {
                sb.Append(" AND a.bottlesn='" + this.txtbottle.Text.Trim() + "'");

            }

            dt = this.dbConn.GetDataSetBySql(sb.ToString()).Tables[0];

             
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\LcRecycle.rdlc";
            

            ReportDataSource rsd = new ReportDataSource("LCRecycle_LcRecycle", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
            
        }

        protected void ddlHoldFlag_TextChanged(object sender, EventArgs e)
        {
            SetDdl();
        }

    }
}
