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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace WebReport.Reports
{
    public partial class PerformanceAccessReport : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.SetProdGroupDataSource();
                this.SetTeamIDDataSource();
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                this.lblErrMsg.Text = "No FabId!";
                Response.Redirect("../../Login.aspx");
            }
        }

        /// <summary>
        /// 設置DataSource: TeamID
        /// </summary>
        private void SetTeamIDDataSource()
        {
            string strSql = @"SELECT TEAMID FROM MTEAM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch TEAMID!";
            }
            else
            {
                this.cmbTeamID.DataSource = dt;
                this.cmbTeamID.DataTextField = "TEAMID";
                this.cmbTeamID.DataValueField = "TEAMID";
                this.cmbTeamID.DataBind();
                this.cmbTeamID.Items.Insert(0, "");
            }
        }

        /// <summary>
        /// 設置DataSource: ProdGroup
        /// </summary>
        private void SetProdGroupDataSource()
        {
            string strSql = @"SELECT PRODGROUP FROM MPGTM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch PRODGROUP!";
            }
            else
            {
                this.cmbProdGroup.DataSource = dt;
                this.cmbProdGroup.DataTextField = "PRODGROUP";
                this.cmbProdGroup.DataValueField = "PRODGROUP";
                this.cmbProdGroup.DataBind();
                this.cmbProdGroup.Items.Insert(0, "");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;

            #region LocalReport DataSource Setting
                try
                {
                    if (this.cmbYear.SelectedIndex <= 0)
                    {
                        strErrorMsg += "年份未設定！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }

                    if (this.cmbMonth.SelectedIndex <= 0)
                    {
                        strErrorMsg += "月份未設定！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }
                     
                    this.reportViewer.ProcessingMode = ProcessingMode.Local;
                    LocalReport localReport = reportViewer.LocalReport;
                    localReport.ReportPath = @"RDLC\PerformanceAccess.rdlc";
                    Report.Sources.PerformanceAccess rs = new Report.Sources.PerformanceAccess();

                    rs.FabID = ((UserInfo)Session["UserInfo"]).FabId;

                    if (this.cmbYear.SelectedIndex > 0)
                    {
                        rs.Year = this.cmbYear.SelectedValue;
                    }

                    if (this.cmbMonth.SelectedIndex > 0)
                    {
                        rs.Month=this.cmbMonth.SelectedValue;
                    }

                    if (this.cmbProdGroup.SelectedIndex > 0)
                    {
                        rs.ProdGroup=this.cmbProdGroup.SelectedValue;
                    }

                    if (this.cmbTeamID.SelectedIndex > 0)
                    {
                        rs.TeamID=this.cmbTeamID.SelectedValue;
                    }
                    RecordLog Log = new RecordLog();
                    if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                    {
                        this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                    }
                    DataSet dataset = rs.GetDataSet();

                    ReportDataSource dsPerformanceAccess = new ReportDataSource();
                    dsPerformanceAccess.Name = "PerformanceAccess";
                    dsPerformanceAccess.Value = dataset.Tables[0];
                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(dsPerformanceAccess);
                }
                catch (Exception ex)
                {
                    this.lblErrMsg.Text = ex.Message;
                }
            #endregion
        }
    }
}
