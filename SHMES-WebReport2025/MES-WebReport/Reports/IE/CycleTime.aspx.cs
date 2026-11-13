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
    public partial class CycleTime : System.Web.UI.Page
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
                
                this.SetProductIDDataSource();
                this.SetStationDataSource();
                this.SetModuleStationDataSource();
                this.SetWOTypeSource();
                this.SetLotTypeSource();
                //this.SetLotSizeSource();
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                this.lblErrMsg.Text = "No FabId!";
                Response.Redirect("../../Login.aspx");
            }
        }

        /// <summary>
        /// 設置DataSource: ProdID
        /// </summary>
        private void SetProductIDDataSource()
        {
            string strSql = @"SELECT ProdID FROM MPIM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId+ "'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch Product ID!";
            }
            else
            {
                this.cmbProductID.DataSource = dt;
                this.cmbProductID.DataTextField = "ProdID";
                this.cmbProductID.DataValueField = "ProdID";
                this.cmbProductID.DataBind();
                this.cmbProductID.Items.Insert(0, "");
            }
        }

        /// <summary>
        /// 設置DataSource: Cell2 Station
        /// </summary>
        private void SetStationDataSource()
        {
            this.chkAll.Checked = false;
            this.lstStation.Items.Clear();
            string strSql = @"SELECT WORKCTR,WORKCTR+' '+LEFT(WORKSHORTNAME,10)  AS WORKNAME FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND WORKTYPE IN ('BC','AS','SS','CN','CW','S1','FC','PB','M1','MN','M2','M3','P2') AND WORKCTR<>'8084' AND  SHOPCD='CELL2' ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstStation.DataSource = dt;
            this.lstStation.DataTextField = "WORKNAME";
            this.lstStation.DataValueField = "Workctr";
            this.lstStation.DataBind();
            this.lstStation.ClearSelection();
        }

        /// <summary>
        /// 設置DataSource:Module Station
        /// </summary>
        private void SetModuleStationDataSource()
        {
            this.CheckModuleALL.Checked = false;
            this.lstModuleStation.Items.Clear();
            string strSql = @"SELECT WORKCTR,WORKCTR+' '+LEFT(WORKSHORTNAME,10)  AS WORKNAME FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND WORKTYPE IN ('SS','CN','CW','S1','FC','PB','M1','MN','M2','M3','P2') AND SHOPCD='MODULE' ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstModuleStation.DataSource = dt;
            this.lstModuleStation.DataTextField = "WORKNAME";
            this.lstModuleStation.DataValueField = "Workctr";
            this.lstModuleStation.DataBind();
            this.lstModuleStation.ClearSelection();
        }

        /// <summary>
        /// 設置DataSource: WoType
        /// </summary>
        private void SetWOTypeSource()
        {
            string strSql = @"SELECT WOTYPE FROM MWOTM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId+ "'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch WOType!";
            }
            else
            {
                this.cmbWOType.DataSource = dt;
                this.cmbWOType.DataTextField = "WOTYPE";
                this.cmbWOType.DataValueField = "WOTYPE";
                this.cmbWOType.DataBind();
                this.cmbWOType.Items.Insert(0, "");
            }
        }

        private void SetLotTypeSource()
        {
            string strSql = @"SELECT LOTTYPE FROM MLTM  WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY LOTTYPE";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch Lot Type!";
            }
            else
            {
                this.dlLOTTYPE.DataSource = dt;
                this.dlLOTTYPE.DataTextField = "LOTTYPE";
                this.dlLOTTYPE.DataValueField = "LOTTYPE";
                this.dlLOTTYPE.DataBind();
                this.dlLOTTYPE.Items.Insert(0, "");
            }
        }

        /// <summary>
        /// 設置DataSource: LotSize
        /// </summary>
        private void SetLotSizeSource()
        {
            string strSql = @"SELECT Distinct SIZEID FROM MPIM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId+ "'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch SIZE!";
            }
            else
            {
                this.cmbLotSize.DataSource = dt;
                this.cmbLotSize.DataTextField = "SIZEID";
                this.cmbLotSize.DataValueField = "SIZEID";
                this.cmbLotSize.DataBind();
                this.cmbLotSize.Items.Insert(0, "");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;

            if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtToTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtFromTime.Text.Trim().Replace("'", "") != "" && this.txtToTime.Text.Trim().Replace("'", "") != "")
            {
                DateTime fr = new DateTime();
                DateTime to = new DateTime();
                bool bTimeFormatError = false;

                if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "開始時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!DateTime.TryParse(this.txtToTime.Text.Trim().Replace("'", ""), out to))
                {
                    strErrorMsg += "結束時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!bTimeFormatError)
                {
                    TimeSpan s = to - fr;
                    if (fr > to)
                    {
                        strErrorMsg += "結束時間在開始時間之前！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }

                }

                bool b = false;
                bool c = false;
                foreach (ListItem i in this.lstStation.Items)
                {
                    if (i.Selected)
                    {
                        b = true;
                        break;
                    }
                }

                foreach (ListItem i in this.lstModuleStation.Items)
                {
                    if (i.Selected)
                    {
                        c = true;
                        break;
                    }
                }
                if ((!b)&&(!c))
                {
                    strErrorMsg += "Station未選擇！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                }
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                #region LocalReport DataSource Setting
                try
                {
                    this.reportViewer.ProcessingMode = ProcessingMode.Local;
                    LocalReport localReport = reportViewer.LocalReport;
                    localReport.ReportPath = @"RDLC\CycleTime2.rdlc";
                    Report.Sources.CycleTime rs = new Report.Sources.CycleTime();

                    rs.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                    rs.FromTime = DateTime.Parse(this.txtFromTime.Text.ToString().Trim().Replace("'", ""));
                    rs.ToTime = DateTime.Parse(this.txtToTime.Text.ToString().Trim().Replace("'", ""));
                    
                    List<string> stationList = new List<string>();
                    List<string> stationModuleList = new List<string>();
                    foreach (ListItem li in this.lstStation.Items)
                    {
                        if (li.Selected)
                        {
                            stationList.Add(li.Value);
                        }
                    }
                    rs.StationList = stationList;

                    foreach (ListItem li in this.lstModuleStation.Items)
                    {
                        if (li.Selected)
                        {
                            stationModuleList.Add(li.Value);
                        }
                    }
                    rs.StationModuleList = stationModuleList;


                    if (this.cmbWOType.SelectedIndex > 0)
                    {
                        rs.WOType = this.cmbWOType.SelectedValue;
                    }

                    if (this.cmbProductID.SelectedIndex > 0)
                    {
                        rs.ProdID = this.cmbProductID.SelectedValue;
                    }

                    if (this.cmbLotSize.SelectedIndex >= 0)
                    {
                        rs.LotSize = this.cmbLotSize.SelectedValue;
                    }

                    if (dlLOTTYPE.SelectedIndex > 0)
                    {
                        rs.LotType = this.dlLOTTYPE.SelectedValue;
                    }

                    if (this.rboDateScope.SelectedIndex==0)
                    {
                        rs.DateScope = "0";
                    }
                    else
                    {
                        rs.DateScope = "1";
                    }

                    DataSet dataset = rs.GetDataSet();
                    ReportDataSource dsCycleTime = new ReportDataSource();
                    dsCycleTime.Name = "CycleTime";
                    dsCycleTime.Value = dataset.Tables[0];
                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(dsCycleTime);
                }
                catch (Exception ex)
                {
                    this.lblErrMsg.Text = ex.Message;
                }
                #endregion
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked)
            {
                foreach (ListItem i in this.lstStation.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstStation.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void lstStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstStation.Items)
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
        }

        protected void CheckModuleALL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckModuleALL.Checked)
            {
                foreach (ListItem i in this.lstModuleStation.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstModuleStation.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void lstModuleStation_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool bResult = true;
            foreach (ListItem i in this.lstModuleStation.Items)
            {
                if (!i.Selected)
                {
                    this.CheckModuleALL.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.CheckModuleALL.Checked = true;
            }

        }
    }
}
