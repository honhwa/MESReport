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
    public partial class LcRecipeCheckQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
                this.DdlType.SelectedIndex = 0;
                SetDdl(this.DdlType.Items[0].Value.Trim());
            }
        }

        private void SetDdl(string sTransType)
        {

            string sqlcmd = " select SOPRODID AS PRODID from MPMPM where  SOPRODID LIKE 'AP%' UNION select SOPRODID AS PRODID from MPMPM where SOPRODID LIKE 'VAP%'UNION select SOPRODID AS PRODID from MPMPM where  SOPRODID LIKE 'TA%' UNION select SOPRODID AS PRODID from MPMPM where  SOPRODID LIKE 'VTA%' ORDER BY SOPRODID";
            DataTable dt = new DataTable();
            dt = this.dbConn.GetDataSetBySql(sqlcmd).Tables[0];
            
            string sqlcmd1 ="";
            if (sTransType == "SEAL")
            {
                sqlcmd1 = " SELECT EQID FROM MEQM WHERE EQNAME='End Seal-Small'";
            }
            else 
            {
                sqlcmd1 = " SELECT EQID FROM MEQM WHERE EQNAME='LC Injection-Small'";
            }
            DataTable dt1 = new DataTable();
            dt1 = this.dbConn.GetDataSetBySql(sqlcmd1).Tables[0];

            this.ddlProdid.DataSource = dt;
            this.ddlProdid.DataTextField = "PRODID";
            this.ddlProdid.DataValueField = "PRODID";
            this.ddlProdid.DataBind();
            this.ddlEqid.DataSource = dt1;
            this.ddlEqid.DataTextField = "EQID";
            this.ddlEqid.DataValueField = "EQID";
            this.ddlEqid.DataBind();
            this.txtlotid.Text = "";
            this.txtuserid.Text = "";
            this.chkLotId.Checked = false;
            this.ChkUserid.Checked = false;

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
            string sTransType = this.DdlType.Items[this.DdlType.SelectedIndex].Value.Trim();
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT A.TRANSID,A.userid,a.eqid,b.lotid,a.prodid,a.lctypeid,a.recipenoin,a.recipenost,A.LCOMMENT,A.PROCESSID,A.CHECKUSERID,B.RECYCLECOUNT ,B.CLEANMCOUNT");
            sb.Append(" from bs_wlcxm a,bs_wlcxdm b where a.transid=b.transid and a.transtype='"+sTransType+"'");
            sb.Append("AND CONVERT(CHAR(10),a.transid,120)  BETWEEN '" + this.txtDate.Text.Trim() + "' AND '"+this.txtToDate.Text.Trim()+"'");
            if (this.chkProdid.Checked == true)
            {
                sb.Append(" AND a.prodid='" + this.ddlProdid.Items[this.ddlProdid.SelectedIndex].Value.Trim() + "'");
            }
            if ((this.chkLotId.Checked == true) && (this.txtlotid.Text != string.Empty))
            {
                sb.Append(" AND b.lotid='" + this.txtlotid.Text.Trim() + "'");
            }
            if ((this.ChkUserid.Checked == true ) && (this.txtuserid.Text != string.Empty))
            {
                sb.Append(" AND a.userid='"+this.txtuserid.Text.Trim()+"'");
            }            
            if (this.ChkEQID.Checked == true)
            {
                sb.Append(" AND a.eqid='" + this.ddlEqid.Items[this.ddlEqid.SelectedIndex].Value.Trim() + "'");
            }

            
            


            dt = this.dbConn.GetDataSetBySql(sb.ToString()).Tables[0];
           
             
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\LcRecipeCheckQuary.rdlc";

                
            ReportDataSource rsd = new ReportDataSource("LcRecipeCheckQuary", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
            
        }

        protected void DdlType_TextChanged(object sender, EventArgs e)
        {
            string sTransType = this.DdlType.Items[this.DdlType.SelectedIndex].Value.Trim();
            SetDdl(sTransType);
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtlotid_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
