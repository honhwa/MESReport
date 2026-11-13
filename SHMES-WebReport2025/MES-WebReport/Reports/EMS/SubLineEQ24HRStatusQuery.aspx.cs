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
    public partial class SubLineEQ24HRStatusQuery : System.Web.UI.Page
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
                //this.lblIp.Text = "IP: " + Request.UserHostAddress.ToString();
                //this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
                this.SetStationsListDatSource();
                this.SetSubLineDataSource();
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        private void SetStationsListDatSource()
        {
            string strSql = @"SELECT WORKCTR FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch Stations!");
            }
            else
            {
                this.lstStations.Items.Clear();
                this.lstStations.DataSource = dt;
                this.lstStations.DataTextField = "WORKCTR";
                this.lstStations.DataValueField = "WORKCTR";
                this.lstStations.DataBind();
                this.lstStations.ClearSelection();
            }
        }

        private void SetSubLineDataSource()
        {
            string strSql = @"SELECT MSLM.SUBLINEID FROM MSLM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND STOCKID<>''";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch SubLineId!");
            }
            else
            {
                this.cmbSubLine.DataSource = dt;
                this.cmbSubLine.DataTextField = "SUBLINEID";
                this.cmbSubLine.DataValueField = "SUBLINEID";
                this.cmbSubLine.DataBind();
                this.cmbSubLine.Items.Insert(0, "");
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            //this.reportViewer.Reset();

            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;
            bool bHasError = false;

            bool b1 = false;
            bool b2 = false;
            try
            {
                foreach (ListItem i in this.lstStations.Items)
                {
                    if (i.Selected)
                    {
                        b1 = true;
                        break;
                    }
                }
                if (this.cmbSubLine.SelectedIndex >= 0)
                {
                    b2 = true;
                }

                if (b1 && b2)
                {
                    strErrorMsg += "請選則篩選條件！";
                    bHasError = true;
                }

                if (!b1 && !b2)
                {
                    strErrorMsg += "篩選條件出錯！";
                    bHasError = true;
                }
                else
                {
                    bHasError = false;
                }


                if (bHasError)
                {
                    this.lblErrMsg.Text = strErrorMsg;
                }
                else
                {
                    RecordLog Log = new RecordLog();
                    if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                    {
                        this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                    }
                    #region LocalReport DataSource Setting
                    if (b1)
                    {

                        this.reportViewer.ProcessingMode = ProcessingMode.Local;
                        LocalReport localReport = reportViewer.LocalReport;
                        localReport.ReportPath = @"RDLC\SubLine24HRStatus.rdlc";


                        Report.Sources.SubLine24HRStatus s = new Report.Sources.SubLine24HRStatus();
                        s.FabID = ((UserInfo)Session["UserInfo"]).FabId;

                        List<string> workCtrList = new List<string>();
                        foreach (ListItem i in this.lstStations.Items)
                        {
                            if (i.Selected)
                            {
                                workCtrList.Add(i.Value);
                            }
                        }
                        s.WorkCTRList = workCtrList;
                        DataSet dataset = s.GetDataSet();

                        ReportDataSource dsSubLine24HRStatus = new ReportDataSource();
                        dsSubLine24HRStatus.Name = "SubLine24HRStatus";
                        dsSubLine24HRStatus.Value = dataset.Tables[0];

                        localReport.DataSources.Add(dsSubLine24HRStatus);
                    }
                    else if (b2)
                    {
                        this.reportViewer.ProcessingMode = ProcessingMode.Local;
                        LocalReport localReport = reportViewer.LocalReport;
                        localReport.ReportPath = @"RDLC\EQ24HRStatus.rdlc";


                        Report.Sources.EQ24HRStatus s = new Report.Sources.EQ24HRStatus();
                        s.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                        s.SubLine = this.cmbSubLine.SelectedValue;
                        DataSet dataset = s.GetDataSet();

                        ReportDataSource dsEQ24HRStatus = new ReportDataSource();
                        dsEQ24HRStatus.Name = "EQ24HRStatus";
                        dsEQ24HRStatus.Value = dataset.Tables[0];

                        localReport.DataSources.Add(dsEQ24HRStatus);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }


        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        protected void cmbSubLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkAll.Checked = false;
            this.lstStations.ClearSelection();
        }

        protected void lstStations_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool bResult = true;
            foreach (ListItem i in this.lstStations.Items)
            {
                
                if (!i.Selected)
                {
                    this.chkAll.Checked = false;
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                this.chkAll.Checked = true;
            }
            this.cmbSubLine.SelectedIndex = 0;
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked)
            {
                foreach (ListItem i in this.lstStations.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstStations.Items)
                {
                    i.Selected = false;
                }
            }
            this.cmbSubLine.SelectedIndex = 0;
        }
            
    }
}
