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
using Report.Library;
using Report.Sources;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports
{
    public partial class QueryScrapData : System.Web.UI.Page
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
                SetProdID();
                SetWorkCenter();
            }
        }

        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            lblErrMsg.Text = "";
            StringBuilder stb = new StringBuilder();
            bool IsErr = false;
            DataTable dt = new DataTable();
            ReportViewer1.Visible = true;
            stb.Append("SELECT FABID,TRANSDATE,TOLOTGRADE,TON9NCNO,CWORKCTR,SUM(NENTITYTQTY) as QTY  FROM  New_VWLTXA WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
            stb.Append("AND VERB ='MVOU' AND TOLOTGRADE ='S' ");
            if ((txtFromTime.Text == string.Empty) || (txtToTime.Text == string.Empty))
            {
                lblErrMsg.Text = "開始時間或結束時間為空，請重輸!\n";
                return;
            }
            else
            {
                stb.Append(" AND TRANSDATE>='" + txtFromTime.Text.Trim().Replace("'", "") + "' AND TRANSDATE<='" + txtToTime.Text.Trim().Replace("'","")+"'");
            }

            if ((ChkProDID.Checked) && (DDProdID.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入批號或SN!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkProDID.Checked) && (DDProdID.Text != string.Empty))
            {
                stb.Append("   AND TON9NCNO = '" + DDProdID.Text.Trim() + "'");
            }
            if ((ChkWorkCenter.Checked) && (this.DDWorkCenter.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入正確的料號!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkWorkCenter.Checked) && (this.DDWorkCenter.Text != string.Empty))
            {
                stb.Append(" AND CWORKCTR ='" + this.DDWorkCenter.Text.Trim().Replace("'", "") + "'");
            }
            //if ((ChkBatchId.Checked) && (this.TxtBatchId.Text == string.Empty))
            //{
            //    lblErrMsg.Text = "請輸入正確BatchId!!!\n";
            //    IsErr = true;
            //    return;
            //}
            //else if ((ChkBatchId.Checked) && (this.TxtBatchId.Text != string.Empty))
            //{
            //    stb.Append(" AND PARTLOTID ='" + this.TxtBatchId.Text.Trim().Replace("'", "")+"'");
            //}
            //if ((ChkProdID.Checked == false) && (ChkWorkCenter.Checked == false) && (ChkBatchId.Checked == false))
            //{
            //    lblErrMsg.Text = "請輸入正確查詢條件!!!\n";
            //    IsErr = true;
            //}
            stb.Append(" GROUP BY FABID,TRANSDATE,TOLOTGRADE,TON9NCNO,CWORKCTR ORDER BY TRANSDATE");
            if (IsErr == false)
            {
                ReportViewer1.Visible = true;
                dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    lblErrMsg.Text = "查無資料！\n";
                }
            }
            else
            {
                ReportViewer1.Visible = false;
            }
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\QueryScrapData.rdlc";


            ReportDataSource rsd = new ReportDataSource("QueryScrapData", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
        }



        //protected void ChkBatchId_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ChkBatchId.Checked)
        //    {
        //        this.TxtBatchId.Enabled = true;
        //    }
        //    else
        //    {
        //        this.TxtBatchId.Enabled = false;
        //        this.TxtBatchId.Text = "";
        //    }
        //}

        private void SetProdID()
        {
            string strsql;
            if (ChkProDID.Checked == false)
            {
                strsql = @"select T9NCNO From mpm9m where prodid in (select PRODID from mpim )";
            }
            else
            {
                strsql = @"select T9NCNO From mpm9m where prodid in (select PRODID from mpim )";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點所對應的線別！');</script>");
                ChkProDID.Checked = false;
            }
            else
            {
                this.DDProdID.DataSource = dt;
                this.DDProdID.DataTextField = "T9NCNO";
                this.DDProdID.DataValueField = "T9NCNO";
                this.DDProdID.DataBind();
            }
        }

        private void SetWorkCenter()
        {
            string strsql;
            if (ChkWorkCenter.Checked == false)
            {
                strsql = @"select WORKCTR From mwcm order by WORKCTR";
            }
            else
            {
                strsql = @"select WORKCTR From mwcm order by WORKCTR";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點所對應的線別！');</script>");
                ChkWorkCenter.Checked = false;
            }
            else
            {
                this.DDWorkCenter.DataSource = dt;
                this.DDWorkCenter.DataTextField = "WORKCTR";
                this.DDWorkCenter.DataValueField = "WORKCTR";
                this.DDWorkCenter.DataBind();
            }
        }

        protected void ChkWorkCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWorkCenter.Checked)
            {
                this.DDWorkCenter.Enabled = true;
            }
            else
            {
                this.DDWorkCenter.Enabled = false;
                this.DDWorkCenter.Text = "";
            }
        }

        protected void ChkProDID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkProDID.Checked)
            {
                this.DDProdID.Enabled = true;
            }
            else
            {
                this.DDProdID.Enabled = false;
                this.DDProdID.Text = "";
            }
        }

    }
}
