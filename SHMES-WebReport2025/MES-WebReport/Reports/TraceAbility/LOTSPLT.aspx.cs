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
using System.Data.SqlClient;
using System.Text;
using Microsoft.Reporting.WebForms;
using Report.Library;
using Report.Sources;

namespace WebReport.Reports
{
    public partial class LOTSPLT : System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.BindProdID();
                this.BindWonbrID();
                //this.txtFromDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                //this.txtToDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            }
        }
        
        private void BindProdID()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {

                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY PRODID";

                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    //this.DDLProdid.DataSource = null;
                    return;
                }
                //this.DDLProdid.DataSource = dt;
                //this.DDLProdid.DataTextField = "PRODID";
                //this.DDLProdid.DataValueField = "PRODID";
                //this.DDLProdid.DataBind();
                //this.DDLProdid.Items.Insert(0, "ALL");
                //this.DDLProdid.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindWonbrID()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {

                strSql = " select wonbr from New_swom WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY PRODID";

                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    //this.DDLProdid.DataSource = null;
                    return;
                }
                //this.DDLWonbr.DataSource = dt;
                //this.DDLWonbr.DataTextField = "wonbr";
                //this.DDLWonbr.DataValueField = "wonbr";
                //this.DDLWonbr.DataBind();
                //this.DDLWonbr.Items.Insert(0, "ALL");
                //this.DDLWonbr.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void ButQ_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            ReportViewer3.Visible = true;

            if (txtLot.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的LOT ID');</script>");
                ReportViewer3.Visible = false;
            }
            else
            {
                DataTable dt = new DataTable();
                StringBuilder stb = new StringBuilder();
                string lot;
                lot = txtLot.Text.Trim().Replace("'", "");
                lot = "'" + lot + "'";
                stb.Append(" select * from New_vwltxa");
                stb.Append(" where FABID = 'B' AND frentityid='"+txtLot.Text.Trim()  + "'" );
                stb.Append(" and VERB='SPLT'");
                stb.Append("AND len(TOENTITYID) in ('10','12','13')");
                //stb.Append("AND HIDEFLG IN('0','1') AND CARTID ='" + TxtCartId.Text.Trim() + "'");
                //if (this.DDLProdid.Text.ToString() != "ALL")
                //{
                //    stb.Append(" and toprodid='" + this.DDLProdid.Text.Trim() + "' ");
                //}

                //if (this.DDLProdid.Text.ToString() == "ALL")
                //{
                //    stb.Append("");
                //}

                //if (this.DDLWonbr.Text.ToString() != "ALL")
                //{
                //    stb.Append(" and towonbr='" + this.DDLWonbr.Text.Trim() + "' ");
                //}

                //if (this.DDLWonbr.Text.ToString() == "ALL")
                //{
                //    stb.Append("");
                //}


                //stb.Append("AND HIDEFLG IN('0','1') AND CARTID ='" + TxtCartId.Text.Trim() + "'");
                //if (txtFromDate.Text.Trim().Replace("'", "") == "")
                //{
                //    this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的起始時間！');</script>");

                //}
                //else
                //{
                //    stb.Append(" and transdate >= '" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
                //}
                //if (txtToDate.Text.Trim().Replace("'", "") == "")
                //{
                //    this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的結束時間！');</script>");
                //}
                //else
                //{
                //    stb.Append(" and transdate<= '" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
                //}
                stb.Append(" order by transdate");
 
                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
                }
                ReportViewer3.Visible = true;
                this.ReportViewer3.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer3.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\LOTSPLT.rdlc";


                ReportDataSource rsd = new ReportDataSource("LOTSPLT_VWLTXA", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);

            }

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //this.Calendar1.Visible = true;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            //txtFromDate.Text = this.Calendar1.SelectedDate.ToShortDateString();
            //txtFromDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            //this.Calendar1.Visible = false;
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            //this.Calendar2.Visible = true;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            //txtToDate.Text = this.Calendar2.SelectedDate.ToShortDateString();
            //txtToDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            //this.Calendar2.Visible = false;
        }

        protected void DDLProdid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
