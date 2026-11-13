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


namespace WebReport.Reports
{
    public partial class OQCRecordReport : System.Web.UI.Page
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
                this.BindProd();
                this.BindGroup();
                this.BindSubLine();
            }
        }

        #region Event

        /// <summary>
        /// [查詢]作業
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSerch_Click(object sender, EventArgs e)
        {

            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            try
            {
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\OQCRecordROQCS.rdlc";

                this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqc = this.ReportViewer2.LocalReport;
                localReportRoqc.ReportPath = @"RDLC\OQCRecordROQC.rdlc";
                if (this.CheckFormControl())
                {
                    DataSet ds = new DataSet();
                    DataSet dsRoqc = new DataSet();
                    OQCRecord oqcRoqcs = new OQCRecord();
                    oqcRoqcs.StartTime = this.txtFromTime.Text.Trim().Substring(0, 10) + " 00:00:00.000";
                    oqcRoqcs.EndTime = this.txtToTime.Text.Trim().Substring(0, 10) + " 00:00:00.000";
                    oqcRoqcs.ProdID = this.dlProduct.Text;
                    oqcRoqcs.Group = this.dlGroup.Text;
                    oqcRoqcs.Week = this.dlWeek.Text;
                    oqcRoqcs.SubLine = this.DLSubLine.Text;
                    oqcRoqcs.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                    ds = oqcRoqcs.ReadROQCS();
                    dsRoqc = oqcRoqcs.ReadROQC();

                    //this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    //LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                    //localReportRoqcs.ReportPath = @"RDLC\OQCRecordROQCS.rdlc";
                    ReportDataSource rsd = new ReportDataSource("localReport", ds.Tables[0]);


                    //this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    //LocalReport localReportRoqc = this.ReportViewer2.LocalReport;
                    //localReportRoqc.ReportPath = @"RDLC\OQCRecordROQC.rdlc";
                    ReportDataSource rsd2 = new ReportDataSource("localReport", dsRoqc.Tables[0]);

                    #region Binding ReportViewer1

                    localReportRoqcs.DataSources.Clear();
                    localReportRoqcs.DataSources.Add(rsd);
                    #endregion

                    #region Binding ReportViewer2

                    localReportRoqc.DataSources.Clear();
                    localReportRoqc.DataSources.Add(rsd2);

                    #endregion
                }
                else
                {
                    DataTable dttmp = new DataTable();
                    DataTable dtRoqctmp = new DataTable();

                    ReportDataSource rsdTmp = new ReportDataSource("localReport", dttmp);
                    ReportDataSource rsd2Tmp = new ReportDataSource("localReport", dtRoqctmp);

                    localReportRoqc.DataSources.Clear();
                    localReportRoqc.DataSources.Add(rsdTmp);

                    localReportRoqcs.DataSources.Clear();
                    localReportRoqcs.DataSources.Add(rsdTmp);
                    //localReportRoqcs.DataSources.Clear();
                    //localReportRoqc.DataSources.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Session["IsLogin"] = "N";
                Session["UserInfo"] = null;
                Response.Redirect("~/Login.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 綁定Prod
        /// </summary>
        private void BindProd()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODID FROM MPIM WHERE FABID = 'B' AND PRODGROUP = '" + this.dlGroup.Text + "' ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.dlProduct.DataSource = null;
                    return;
                }
                this.dlProduct.DataSource = dt;
                this.dlProduct.DataTextField = "PRODID";
                this.dlProduct.DataValueField = "PRODID";
                this.dlProduct.DataBind();
                this.dlProduct.Items.Insert(0, "");
                //this.DLSubLine.Items.Insert(0, "");
                //this.dlProduct.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 綁定Group
        /// </summary>
        private void BindGroup()
        {
             string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID ='B' ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.dlGroup.DataSource = null;
                    return;
                }
                this.dlGroup.DataSource = dt;
                this.dlGroup.DataTextField = "PRODGROUP";
                this.dlGroup.DataValueField = "PRODGROUP";
                this.dlGroup.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 綁定SubLine資料
        /// </summary>
        private void BindSubLine()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = "  SELECT SUBLINEID FROM MSLM WHERE FABID='B' AND WORKCTR IN( SELECT WORKCTR FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND WORKTYPE='M3')  GROUP BY SUBLINEID ORDER BY SUBLINEID DESC  ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.DLSubLine.DataSource = null;
                    return;
                }
                this.DLSubLine.DataSource = dt;
                this.DLSubLine.DataTextField = "SUBLINEID";
                this.DLSubLine.DataValueField = "SUBLINEID";
                this.DLSubLine.DataBind();
                this.DLSubLine.Items.Insert(0, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 檢查控件合法性

        /// </summary>
        /// <returns></returns>
        private bool CheckFormControl()
        {
            if (this.txtFromTime.Text == "")
            {
                this.lblErrMsg.Text = "請選擇開始時間!";
                return false;
            }
            if (this.txtToTime.Text == "")
            {
                this.lblErrMsg.Text = "請選擇結束時間!";
                return false;
            }

            if (DateTime.Parse(this.txtToTime.Text) < DateTime.Parse(this.txtFromTime.Text))
            {
                this.lblErrMsg.Text = "結束時間不能小於開始時間!";
                return false;
            }

            if (!this.CheckTime())
            {
                this.lblErrMsg.Text = "開始時間,結束時間不能大於7天!";
                return false;
            }
            this.lblErrMsg.Text = "";
            return true;
        }

        /// <summary>
        /// 檢查時間是否大於7天

        /// </summary>
        /// <returns></returns>
        private bool CheckTime()
        {
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Parse(this.txtToTime.Text) - DateTime.Parse(this.txtFromTime.Text);
            if (ts.Days > 7)
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
