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
    public partial class PanelReworkInfo : System.Web.UI.Page
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
                this.txtFromDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            this.Calendar1.Visible = true;
        }

        protected void Calendar1_SelectionChanged1(object sender, EventArgs e)
        {
            txtFromDate.Text = this.Calendar1.SelectedDate.ToShortDateString();
            txtFromDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            this.Calendar1.Visible = false;
        }
        protected void ImageButton2_Click1(object sender, ImageClickEventArgs e)
        {
            this.Calendar2.Visible = true;
        }

       protected void Calendar2_SelectionChanged1(object sender, EventArgs e)
        {
            txtToDate.Text = this.Calendar2.SelectedDate.ToShortDateString();
            txtToDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            this.Calendar2.Visible = false;
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
                    this.DDLProdid.DataSource = null;
                    return;
                }
                this.DDLProdid.DataSource = dt;
                this.DDLProdid.DataTextField = "PRODID";
                this.DDLProdid.DataValueField = "PRODID";
                this.DDLProdid.DataBind();
                this.DDLProdid.Items.Insert(0, "ALL");
                this.DDLProdid.ClearSelection();
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
            ReportViewer10.Visible = true;
            if (txtPanel.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的Panel ID！');</script>");
                ReportViewer10.Visible = false;
            }
            else
            {
                DataTable dt = new DataTable();
                StringBuilder stb = new StringBuilder();
                string lot;
                lot = txtPanel.Text.Trim().Replace("'", "");
                lot = "'" + lot + "'";
                stb.Append(" select a.*,b.cmt from (select transdate,toentityid,verb,cworkctr,toprodid,ton9ncno,towonbr,solotid,soprodid,so9ncno,mlotid from New_vwltxa) a left join wssd b on a.toentityid=b.entityid");
                stb.Append(" where cworkctr in (select workctr from mwcm where worktype in ('MR','CR')) and toentityid='" + txtPanel.Text.Trim() + "'");
                if (txtFromDate.Text.Trim().Replace("'", "") == "")
                {
                    this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的起始時間！');</script>");

                }
                else
                {
                    stb.Append(" and a.transdate >= '" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
                }
                if (txtToDate.Text.Trim().Replace("'", "") == "")
                {
                    this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的結束時間！');</script>");
                }
                else
                {
                    stb.Append(" and a.transdate<= '" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
                }

                if (this.DDLProdid.Text.ToString() != "ALL")
                {
                    stb.Append(" and toprodid='" + this.DDLProdid.Text.Trim() + "' ");
                }

                if (this.DDLProdid.Text.ToString() == "ALL")
                {
                    stb.Append("");
                }

                if (this.txtSN.Text.Trim().Replace("'", "") == "")
                {
                    stb.Append("");
                }
                else
                {
                    stb.Append(" and cmt='" + txtSN.Text.Trim() + "'");
                }


                //stb.Append("AND HIDEFLG IN('0','1') AND CARTID ='" + TxtCartId.Text.Trim() + "'");
                stb.Append(" order by a.transdate");
                    
                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
                    if (dt.Rows.Count < 1)
                    {
                        this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
                    }
                    ReportViewer10.Visible = true;
                    this.ReportViewer10.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    LocalReport localReportRoqcs = this.ReportViewer10.LocalReport;
                    localReportRoqcs.ReportPath = @"RDLC\PanelInfo.rdlc";


                    ReportDataSource rsd = new ReportDataSource("DataSet1_DataTable1", dt);
                    localReportRoqcs.DataSources.Clear();
                    localReportRoqcs.DataSources.Add(rsd);

                }
            }

        protected void DDLProdid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        }


    }
