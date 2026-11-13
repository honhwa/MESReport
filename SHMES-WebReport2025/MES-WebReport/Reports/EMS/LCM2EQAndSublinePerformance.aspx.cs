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
using Microsoft.Reporting.WebForms;
using Report.Library;
using System.Text;

namespace WebReport.Reports
{
    public partial class LCM2EQAndSublinePerformance : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                //this.SetFromYearList();
                //this.SetToYearList();
                this.SetStationsList();
                this.SetLineIdList();
                this.SetSublineIdList();
                this.SetEqIdList("EQTYPE");
                this.setEqTypeList();
                //this.SetTimeList("Month");
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }
        protected void chkYear_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkYear.Checked)
            {
                this.chkDate.Checked = false;
                this.txtFromTime.Enabled = false;
                this.SetFromYearList();
                this.SetToYearList();
                this.SetTimeList("Month");
            }
        }
        protected void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkDate.Checked)
            {
                this.chkYear.Checked = false;
                this.txtFromTime.Enabled = true;
                this.cmbFromYear.Items.Clear();
                this.cmbToYear.Items.Clear();
            }
        }

       // protected void cbSubLine_CheckedChanged(object sender, EventArgs e)
       // {
       //     if (this.cbSubLine.Checked)
       //     {
       //         cbEQ.Checked = false;
       //     }
       //}

       // protected void cbEQ_CheckedChanged(object sender, EventArgs e)
       // {
            
       //     if (this.cbEQ.Checked)
       //     {
       //         cbSubLine.Checked = false;
       //     }           
       // }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblErrMsg.Text = string.Empty;
                string strErrorMsg = string.Empty;
                bool bHasError = false;
                DateTime dtFromTime = new DateTime();
                DateTime dtToTime = new DateTime();
                DateTime dtTmp = new DateTime();
                DataSet dsSL = new DataSet();
                DataSet dsEQ = new DataSet();
                DataSet dsDptSL = new DataSet();
                DataSet dsDptEQ = new DataSet();
                DataTable dtTmp1 = new DataTable();
                DataTable dtTmp2 = new DataTable();
                if (!this.cbSubLine.Checked && !this.cbEQ.Checked)
                {
                    this.cbSubLine.Checked = true;
                    this.cbEQ.Checked = true;
                }
                if (!this.chkYear.Checked && !this.chkDate.Checked)
                {
                    strErrorMsg += "請選擇By天查詢或By年月查詢！\n";
                    bHasError = true;
                }
                if (bHasError==false)
                {
                    if (this.chkYear.Checked)
                    {
                        if (this.cmbFromTime.SelectedValue == "")
                        {
                            strErrorMsg += "開始時間為空！\n";
                            bHasError = true;
                        }
                        if (this.cmbFromYear.SelectedValue == "")
                        {
                            strErrorMsg += "開始年份為空！\n";
                            bHasError = true;
                        }
                        if (this.cmbToTime.SelectedValue == "")
                        {
                            strErrorMsg += "結束時間為空！\n";
                            bHasError = true;
                        }
                        if (this.cmbToYear.SelectedValue == "")
                        {
                            strErrorMsg += "結束年份為空！\n";
                            bHasError = true;
                        }
                        if (!bHasError && this.cmbBy.SelectedValue == "Month")
                        {
                            string strFromTime = this.cmbFromYear.SelectedValue + "/" + this.cmbFromTime.SelectedValue + "/1";
                            if (!DateTime.TryParse(strFromTime, out dtFromTime))
                            {
                                strErrorMsg += "開始時間轉換錯誤！\n";
                                bHasError = true;
                            }
                            string strToTime = this.cmbToYear.SelectedValue + "/" + this.cmbToTime.SelectedValue + "/" + Report.Library.DateHelper.NumberOfDays(Int16.Parse(this.cmbToYear.SelectedValue), Int16.Parse(this.cmbToTime.SelectedValue)).ToString();
                            if (!DateTime.TryParse(strToTime, out dtToTime))
                            {
                                strErrorMsg += "結束時間轉換錯誤！\n";
                                bHasError = true;
                            }
                            else
                            {
                                dtToTime = dtToTime.AddDays(1);
                                dtToTime = dtToTime.AddMilliseconds(-3);
                            }
                        }
                        else if (!bHasError && this.cmbBy.SelectedValue == "Week")
                        {
                            if (!Report.Library.DateHelper.GetDaysOfWeeks(Int16.Parse(this.cmbFromYear.SelectedValue), Int16.Parse(this.cmbFromTime.SelectedValue), System.Globalization.CalendarWeekRule.FirstFourDayWeek, out dtFromTime, out dtTmp))
                            {
                                strErrorMsg += "結束時間轉換錯誤！\n";
                                bHasError = true;
                            }
                            if (!Report.Library.DateHelper.GetDaysOfWeeks(Int16.Parse(this.cmbToYear.SelectedValue), Int16.Parse(this.cmbToTime.SelectedValue), System.Globalization.CalendarWeekRule.FirstFourDayWeek, out dtTmp, out dtToTime))
                            {
                                strErrorMsg += "結束時間轉換錯誤！\n";
                                bHasError = true;
                            }
                            else
                            {
                                dtToTime = dtToTime.AddMilliseconds(-3);
                            }
                        }
                    }
                    if (this.chkDate.Checked)
                    {
                        if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
                        {
                            strErrorMsg += "時間為空！\n";
                            this.lblErrMsg.Text = strErrorMsg;
                            bHasError = true;
                            return;
                        }
                         string strFromTime = this.txtFromTime.Text.ToString();
                         if (!DateTime.TryParse(strFromTime, out dtFromTime))
                         {
                            strErrorMsg += "結束時間轉換錯誤！\n";
                            bHasError = true;
                         }
                    }
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
                    if (this.cbSubLine.Checked)
                    {
                        Report.Sources.LCM2SubLinePerformance sSL = new Report.Sources.LCM2SubLinePerformance();
                        sSL.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                        if (chkYear.Checked)
                        {
                            sSL.TimeBy = this.cmbBy.SelectedValue;
                            sSL.ToTime = dtToTime;
                            sSL.FromTime = dtFromTime;
                        }
                        if (chkDate.Checked)
                        {
                            sSL.TimeBy = "Date";
                            sSL.FromTime = dtFromTime;
                            sSL.ToTime = dtFromTime;
                        }

                        sSL.Process = this.cmdProcess.SelectedValue;
                        foreach (ListItem i in this.lstStations.Items)
                        {
                            if (i.Selected)
                            {
                                sSL.WorkCTRList.Add(i.Value);
                            }
                        }
                        foreach (ListItem i in this.lstSubLineId.Items)
                        {
                            if (i.Selected)
                            {
                                sSL.SubLineIDList.Add(i.Value);
                            }
                        }
                        dsSL = sSL.GetDataSet();
                        dtTmp1 = dsSL.Tables[0].Copy();
                        
                        Report.Sources.LCM2DptSubLinePerformance sDptSL = new Report.Sources.LCM2DptSubLinePerformance();
                        sDptSL.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                        if (chkYear.Checked)
                        {
                            sDptSL.TimeBy = this.cmbBy.SelectedValue;
                            sDptSL.ToTime = dtToTime;
                            sDptSL.FromTime = dtFromTime;
                        }
                        if (chkDate.Checked)
                        {
                            sDptSL.TimeBy = "Date";
                            sDptSL.FromTime = dtFromTime;
                            sDptSL.ToTime = dtFromTime;
                        }
                        sDptSL.Process = this.cmdProcess.SelectedValue;
                        foreach (ListItem i in this.lstStations.Items)
                        {
                            if (i.Selected)
                            {
                                sDptSL.WorkCTRList.Add(i.Value);
                            }
                        }
                        foreach (ListItem i in this.lstSubLineId.Items)
                        {
                            if (i.Selected)
                            {
                                sDptSL.SubLineIDList.Add(i.Value);
                            }
                        }
                        dsDptSL = sDptSL.GetDataSet();
                        dtTmp2 = dsDptSL.Tables[0].Copy();

                    }
                    if (this.cbEQ.Checked)
                    {
                        Report.Sources.LCM2EQPerformance sEQ = new Report.Sources.LCM2EQPerformance();
                        sEQ.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                        if (this.chkYear.Checked)
                        {
                            sEQ.FromTime = dtFromTime;
                            sEQ.TimeBy = this.cmbBy.SelectedValue;
                            sEQ.ToTime = dtToTime;
                        }
                        if (this.chkDate.Checked)
                        {
                            sEQ.FromTime = dtFromTime;
                            sEQ.TimeBy = "Date";
                            sEQ.ToTime = dtFromTime;
                        }
                        sEQ.Process = this.cmdProcess.SelectedValue;
                        foreach (ListItem i in this.lstStations.Items)
                        {
                            if (i.Selected)
                            {
                                sEQ.WorkCTRList.Add(i.Text);
                            }
                        }
                        foreach (ListItem i in this.lstSubLineId.Items)
                        {
                            if (i.Selected)
                            {
                                sEQ.SubLineIDList.Add(i.Text);
                            }
                        }
                        foreach (ListItem i in this.lstEqId.Items)
                        {
                            if (i.Selected)
                            {
                                sEQ.EQIDList.Add(i.Text);
                            }
                        }
                        foreach (ListItem i in this.lstEqType.Items)
                        {
                            if (i.Selected)
                            {
                                sEQ.EQTypeList.Add(i.Text);
                            }
                        }
                        dsEQ = sEQ.GetDataSet();

                        Report.Sources.LCM2DptEQPerformance sDptEQ = new Report.Sources.LCM2DptEQPerformance();
                        sDptEQ.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                        if (chkYear.Checked)
                        {
                            sDptEQ.TimeBy = this.cmbBy.SelectedValue;
                            sDptEQ.ToTime = dtToTime;
                            sDptEQ.FromTime = dtFromTime;
                        }
                        if (chkDate.Checked)
                        {
                            sDptEQ.TimeBy = "Date";
                            sDptEQ.FromTime = dtFromTime;
                            sDptEQ.ToTime = dtFromTime;
                        }
                        sDptEQ.Process = this.cmdProcess.SelectedValue;
                        foreach (ListItem i in this.lstStations.Items)
                        {
                            if (i.Selected)
                            {
                                sDptEQ.WorkCTRList.Add(i.Text);
                            }
                        }
                        foreach (ListItem i in this.lstSubLineId.Items)
                        {
                            if (i.Selected)
                            {
                                sDptEQ.SubLineIDList.Add(i.Text);
                            }
                        }
                        foreach (ListItem i in this.lstEqId.Items)
                        {
                            if (i.Selected)
                            {
                                sDptEQ.EQIDList.Add(i.Text);
                            }
                        }
                        foreach (ListItem i in this.lstEqType.Items)
                        {
                            if (i.Selected)
                            {
                                sDptEQ.EQTypeList.Add(i.Text);
                            }

                        }
                        dsDptEQ = sDptEQ.GetDataSet();
                        dtTmp1 = dsEQ.Tables[0].Copy();
                        dtTmp2 = dsDptEQ.Tables[0].Copy();
                    }
                    
                    if (this.cbEQ.Checked && this.cbSubLine.Checked)
                    {
                        dtTmp1 = dsEQ.Tables[0].Copy();
                        dtTmp1.Merge(dsSL.Tables[0]);
                        dtTmp2 = dsDptEQ.Tables[0].Copy();
                        dtTmp2.Merge(dsDptSL.Tables[0]);
                    }
                    this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                    LocalReport localReport1 = reportViewer1.LocalReport;
                    localReport1.ReportPath = @"RDLC\LCM2SubLineEQPerformance1.rdlc";

                    this.reportViewer2.ProcessingMode = ProcessingMode.Local;
                    LocalReport localReport2 = reportViewer2.LocalReport;
                    localReport2.ReportPath = @"RDLC\LCM2SubLineEQPerformance2.rdlc";

                    this.reportViewer3.ProcessingMode = ProcessingMode.Local;
                    LocalReport localReport3 = reportViewer3.LocalReport;
                    localReport3.ReportPath = @"RDLC\LCM2SubLineEQPerformance3.rdlc";

                    #region 20080328
                    ReportParameter p = new ReportParameter("pUrl", Request.Url.AbsoluteUri.Replace("LCM2EQAndSublinePerformance.aspx", "Graph1.aspx"));
                    ReportParameter[] ps ={ p };
                    localReport2.SetParameters(ps);

                    ReportParameter Pm = new ReportParameter("pURL", Request.Url.AbsoluteUri.Replace("LCM2EQAndSublinePerformance.aspx", "Graph2.aspx"));
                    ReportParameter[] pms ={ Pm };
                    localReport3.SetParameters(pms);
                    
                    #endregion



                    ReportDataSource rds1 = new ReportDataSource();
                    rds1.Name = "LCM2SubLineEQPerformance";
                    rds1.Value = dtTmp1;
                    localReport1.DataSources.Clear();
                    localReport2.DataSources.Clear();
                    localReport1.DataSources.Add(rds1);
                    localReport2.DataSources.Add(rds1);
                    Session["G1"] = dtTmp1;

                    ReportDataSource rds3 = new ReportDataSource();
                    rds3.Name = "LCM2DptSubLineEQPerformance";
                    rds3.Value = dtTmp2;
                    localReport3.DataSources.Clear();
                    localReport3.DataSources.Add(rds3);
                    Session["G2"] = dtTmp2;

                    this.reportViewer1.LocalReport.Refresh();
                    this.reportViewer2.LocalReport.Refresh();
                    this.reportViewer3.LocalReport.Refresh();



                }
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        protected void cmbBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbBy.SelectedItem.Value == "Month")
            {
                this.SetTimeList("Month");
            }
            else if (this.cmbBy.SelectedItem.Value == "Week")
            {
                this.SetTimeList("Week");
            }
        }

        protected void chkAllStations_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllStations.Checked)
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
            this.SetLineIdList();
            this.SetSublineIdList();

        }

        protected void chkAllLineIDs_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllLineIDs.Checked)
            {
                foreach (ListItem i in this.lstLineId.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstLineId.Items)
                {
                    i.Selected = false;
                }
            }
            this.SetSublineIdList();

        }

        protected void chkAllSubLineIDs_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllSubLineIDs.Checked)
            {
                foreach (ListItem i in this.lstSubLineId.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstSubLineId.Items)
                {
                    i.Selected = false;
                }
            }
            this.SetEqIdList("SUBLINE");
        }

        protected void chkAllEQIDs_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllEQIDs.Checked)
            {
                foreach (ListItem i in this.lstEqId.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstEqId.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkAllEQTypes_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllEQTypes.Checked)
            {
                foreach (ListItem i in this.lstEqType.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstEqType.Items)
                {
                    i.Selected = false;
                }
            }
            this.SetEqIdList("EQTYPE");
        }

        protected void lstStations_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool bResult = true;
            foreach (ListItem i in this.lstStations.Items)
            {

                if (!i.Selected)
                {
                    this.chkAllStations.Checked = false;
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                this.chkAllStations.Checked = true;
            }
            this.SetLineIdList();
            this.SetSublineIdList();
        }

        protected void lstLineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstLineId.Items)
            {

                if (!i.Selected)
                {
                    this.chkAllLineIDs.Checked = false;
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                this.chkAllLineIDs.Checked = true;
            }
            this.SetSublineIdList();
        }

        protected void lstSubLineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstSubLineId.Items)
            {

                if (!i.Selected)
                {
                    this.chkAllSubLineIDs.Checked = false;
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                this.chkAllSubLineIDs.Checked = true;
            }
            this.SetEqIdList("SUBLINE");
        }

        protected void lstEqId_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstEqId.Items)
            {

                if (!i.Selected)
                {
                    this.chkAllEQIDs.Checked = false;
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                this.chkAllEQIDs.Checked = true;
            }
        }

        protected void lstEqType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstEqType.Items)
            {

                if (!i.Selected)
                {
                    this.chkAllEQTypes.Checked = false;
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                this.chkAllEQTypes.Checked = true;
            }
            this.SetEqIdList("EQTYPE");
        }
        
        #endregion

        #region Filter DataSource
        private void SetFromYearList()
        {
            string strSql = "SELECT YEAR(TRANSDATE) AS TRANSYEAR FROM RSUMESLSH GROUP BY YEAR(TRANSDATE) ORDER BY YEAR(TRANSDATE)";
            DataTable dtFromYear = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.cmbFromYear.DataSource = dtFromYear;
            this.cmbFromYear.DataTextField = "TRANSYEAR";
            this.cmbFromYear.DataValueField = "TRANSYEAR";
            this.cmbFromYear.DataBind();
            this.cmbFromYear.Items.Insert(0, "");
        }

        private void SetToYearList()
        {
            string strSql = "SELECT YEAR(TRANSDATE) AS TRANSYEAR FROM RSUMESLSH GROUP BY YEAR(TRANSDATE) ORDER BY YEAR(TRANSDATE)";
            DataTable dtToYear = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.cmbToYear.DataSource = dtToYear;
            this.cmbToYear.DataTextField = "TRANSYEAR";
            this.cmbToYear.DataValueField = "TRANSYEAR";
            this.cmbToYear.DataBind();
            this.cmbToYear.Items.Insert(0, "");
        }

        private void SetTimeList(string p)
        {
            if (p == "Week")
            {
                this.lblFromTime.Text = "周";
                this.lblToTime.Text = "周";
                this.cmbFromTime.Items.Clear();
                this.cmbToTime.Items.Clear();
                this.cmbFromTime.Items.Add(new ListItem("1", "1"));
                this.cmbFromTime.Items.Add(new ListItem("2", "2"));
                this.cmbFromTime.Items.Add(new ListItem("3", "3"));
                this.cmbFromTime.Items.Add(new ListItem("4", "4"));
                this.cmbFromTime.Items.Add(new ListItem("5", "5"));
                this.cmbFromTime.Items.Add(new ListItem("6", "6"));
                this.cmbFromTime.Items.Add(new ListItem("7", "7"));
                this.cmbFromTime.Items.Add(new ListItem("8", "8"));
                this.cmbFromTime.Items.Add(new ListItem("9", "9"));
                this.cmbFromTime.Items.Add(new ListItem("10", "10"));
                this.cmbFromTime.Items.Add(new ListItem("11", "11"));
                this.cmbFromTime.Items.Add(new ListItem("12", "12"));
                this.cmbFromTime.Items.Add(new ListItem("13", "13"));
                this.cmbFromTime.Items.Add(new ListItem("14", "14"));
                this.cmbFromTime.Items.Add(new ListItem("15", "15"));
                this.cmbFromTime.Items.Add(new ListItem("16", "16"));
                this.cmbFromTime.Items.Add(new ListItem("17", "17"));
                this.cmbFromTime.Items.Add(new ListItem("18", "18"));
                this.cmbFromTime.Items.Add(new ListItem("19", "19"));
                this.cmbFromTime.Items.Add(new ListItem("20", "20"));
                this.cmbFromTime.Items.Add(new ListItem("21", "21"));
                this.cmbFromTime.Items.Add(new ListItem("22", "22"));
                this.cmbFromTime.Items.Add(new ListItem("23", "23"));
                this.cmbFromTime.Items.Add(new ListItem("24", "24"));
                this.cmbFromTime.Items.Add(new ListItem("25", "25"));
                this.cmbFromTime.Items.Add(new ListItem("26", "26"));
                this.cmbFromTime.Items.Add(new ListItem("27", "27"));
                this.cmbFromTime.Items.Add(new ListItem("28", "28"));
                this.cmbFromTime.Items.Add(new ListItem("29", "29"));
                this.cmbFromTime.Items.Add(new ListItem("30", "30"));
                this.cmbFromTime.Items.Add(new ListItem("31", "31"));
                this.cmbFromTime.Items.Add(new ListItem("32", "32"));
                this.cmbFromTime.Items.Add(new ListItem("33", "33"));
                this.cmbFromTime.Items.Add(new ListItem("34", "34"));
                this.cmbFromTime.Items.Add(new ListItem("35", "35"));
                this.cmbFromTime.Items.Add(new ListItem("36", "36"));
                this.cmbFromTime.Items.Add(new ListItem("37", "37"));
                this.cmbFromTime.Items.Add(new ListItem("38", "38"));
                this.cmbFromTime.Items.Add(new ListItem("39", "39"));
                this.cmbFromTime.Items.Add(new ListItem("40", "40"));
                this.cmbFromTime.Items.Add(new ListItem("41", "41"));
                this.cmbFromTime.Items.Add(new ListItem("42", "42"));
                this.cmbFromTime.Items.Add(new ListItem("43", "43"));
                this.cmbFromTime.Items.Add(new ListItem("44", "44"));
                this.cmbFromTime.Items.Add(new ListItem("45", "45"));
                this.cmbFromTime.Items.Add(new ListItem("46", "46"));
                this.cmbFromTime.Items.Add(new ListItem("47", "47"));
                this.cmbFromTime.Items.Add(new ListItem("48", "48"));
                this.cmbFromTime.Items.Add(new ListItem("49", "49"));
                this.cmbFromTime.Items.Add(new ListItem("50", "50"));
                this.cmbFromTime.Items.Add(new ListItem("51", "51"));
                this.cmbFromTime.Items.Add(new ListItem("52", "52"));
                this.cmbFromTime.Items.Add(new ListItem("53", "53"));
                this.cmbToTime.Items.Add(new ListItem("1", "1"));
                this.cmbToTime.Items.Add(new ListItem("2", "2"));
                this.cmbToTime.Items.Add(new ListItem("3", "3"));
                this.cmbToTime.Items.Add(new ListItem("4", "4"));
                this.cmbToTime.Items.Add(new ListItem("5", "5"));
                this.cmbToTime.Items.Add(new ListItem("6", "6"));
                this.cmbToTime.Items.Add(new ListItem("7", "7"));
                this.cmbToTime.Items.Add(new ListItem("8", "8"));
                this.cmbToTime.Items.Add(new ListItem("9", "9"));
                this.cmbToTime.Items.Add(new ListItem("10", "10"));
                this.cmbToTime.Items.Add(new ListItem("11", "11"));
                this.cmbToTime.Items.Add(new ListItem("12", "12"));
                this.cmbToTime.Items.Add(new ListItem("13", "13"));
                this.cmbToTime.Items.Add(new ListItem("14", "14"));
                this.cmbToTime.Items.Add(new ListItem("15", "15"));
                this.cmbToTime.Items.Add(new ListItem("16", "16"));
                this.cmbToTime.Items.Add(new ListItem("17", "17"));
                this.cmbToTime.Items.Add(new ListItem("18", "18"));
                this.cmbToTime.Items.Add(new ListItem("19", "19"));
                this.cmbToTime.Items.Add(new ListItem("20", "20"));
                this.cmbToTime.Items.Add(new ListItem("21", "21"));
                this.cmbToTime.Items.Add(new ListItem("22", "22"));
                this.cmbToTime.Items.Add(new ListItem("23", "23"));
                this.cmbToTime.Items.Add(new ListItem("24", "24"));
                this.cmbToTime.Items.Add(new ListItem("25", "25"));
                this.cmbToTime.Items.Add(new ListItem("26", "26"));
                this.cmbToTime.Items.Add(new ListItem("27", "27"));
                this.cmbToTime.Items.Add(new ListItem("28", "28"));
                this.cmbToTime.Items.Add(new ListItem("29", "29"));
                this.cmbToTime.Items.Add(new ListItem("30", "30"));
                this.cmbToTime.Items.Add(new ListItem("31", "31"));
                this.cmbToTime.Items.Add(new ListItem("32", "32"));
                this.cmbToTime.Items.Add(new ListItem("33", "33"));
                this.cmbToTime.Items.Add(new ListItem("34", "34"));
                this.cmbToTime.Items.Add(new ListItem("35", "35"));
                this.cmbToTime.Items.Add(new ListItem("36", "36"));
                this.cmbToTime.Items.Add(new ListItem("37", "37"));
                this.cmbToTime.Items.Add(new ListItem("38", "38"));
                this.cmbToTime.Items.Add(new ListItem("39", "39"));
                this.cmbToTime.Items.Add(new ListItem("40", "40"));
                this.cmbToTime.Items.Add(new ListItem("41", "41"));
                this.cmbToTime.Items.Add(new ListItem("42", "42"));
                this.cmbToTime.Items.Add(new ListItem("43", "43"));
                this.cmbToTime.Items.Add(new ListItem("44", "44"));
                this.cmbToTime.Items.Add(new ListItem("45", "45"));
                this.cmbToTime.Items.Add(new ListItem("46", "46"));
                this.cmbToTime.Items.Add(new ListItem("47", "47"));
                this.cmbToTime.Items.Add(new ListItem("48", "48"));
                this.cmbToTime.Items.Add(new ListItem("49", "49"));
                this.cmbToTime.Items.Add(new ListItem("50", "50"));
                this.cmbToTime.Items.Add(new ListItem("51", "51"));
                this.cmbToTime.Items.Add(new ListItem("52", "52"));
                this.cmbToTime.Items.Add(new ListItem("53", "53"));
            }
            else if (p == "Month")
            {
                this.lblFromTime.Text = "月";
                this.lblToTime.Text = "月";
                this.cmbFromTime.Items.Clear();
                this.cmbToTime.Items.Clear();
                this.cmbFromTime.Items.Add(new ListItem("1", "1"));
                this.cmbFromTime.Items.Add(new ListItem("2", "2"));
                this.cmbFromTime.Items.Add(new ListItem("3", "3"));
                this.cmbFromTime.Items.Add(new ListItem("4", "4"));
                this.cmbFromTime.Items.Add(new ListItem("5", "5"));
                this.cmbFromTime.Items.Add(new ListItem("6", "6"));
                this.cmbFromTime.Items.Add(new ListItem("7", "7"));
                this.cmbFromTime.Items.Add(new ListItem("8", "8"));
                this.cmbFromTime.Items.Add(new ListItem("9", "9"));
                this.cmbFromTime.Items.Add(new ListItem("10", "10"));
                this.cmbFromTime.Items.Add(new ListItem("11", "11"));
                this.cmbFromTime.Items.Add(new ListItem("12", "12"));

                this.cmbToTime.Items.Add(new ListItem("1", "1"));
                this.cmbToTime.Items.Add(new ListItem("2", "2"));
                this.cmbToTime.Items.Add(new ListItem("3", "3"));
                this.cmbToTime.Items.Add(new ListItem("4", "4"));
                this.cmbToTime.Items.Add(new ListItem("5", "5"));
                this.cmbToTime.Items.Add(new ListItem("6", "6"));
                this.cmbToTime.Items.Add(new ListItem("7", "7"));
                this.cmbToTime.Items.Add(new ListItem("8", "8"));
                this.cmbToTime.Items.Add(new ListItem("9", "9"));
                this.cmbToTime.Items.Add(new ListItem("10", "10"));
                this.cmbToTime.Items.Add(new ListItem("11", "11"));
                this.cmbToTime.Items.Add(new ListItem("12", "12"));

            }

        }

        private void SetStationsList()
        {
            string strSql = @"SELECT WORKCTR FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY WORKCTR";
            if (this.cmdProcess.Text == "Module")
            {
                strSql += "AND ProcID<>'CELL2'";
            }
            if (this.cmdProcess.Text == "Cell2")
            {
                strSql += "AND ProcID='CELL2'";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lstStations.Items.Clear();
                this.chkAllStations.Enabled = false;
            }
            else
            {
                this.chkAllStations.Enabled = true;
                this.lstStations.Items.Clear();
                this.lstStations.DataSource = dt;
                this.lstStations.DataTextField = "WORKCTR";
                this.lstStations.DataValueField = "WORKCTR";
                this.lstStations.DataBind();
                this.lstStations.ClearSelection();
            }
        }

        private void SetLineIdList()
        {
            string strFilter = string.Empty;

            if (this.lstStations.SelectedIndex >= 0)
            {
                StringBuilder sbStations = new StringBuilder();
                foreach (ListItem li in this.lstStations.Items)
                {
                    if (li.Selected)
                    {
                        sbStations.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbStations.ToString().Length > 0)
                {
                    strFilter += " AND ESLS.WORKCTR IN (" + sbStations.Replace("''", "','") + ") ";
                }
            }

            string strSql = "SELECT MSLM.LINEID FROM MSLM INNER JOIN New_ESLS ON MSLM.SUBLINEID = ESLS.SUBLINEID WHERE MSLM.FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' AND ESLS.FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' " + strFilter;
            if (this.cmdProcess.Text != "All")
            {
                strSql += "AND ESLS.SHOPCD='" + this.cmdProcess.Text.ToString() + "'";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                //Response.Write("Cannot Fetch LineID!");
                this.lstLineId.Items.Clear();
                this.chkAllLineIDs.Enabled = false;
            }
            else
            {
                this.chkAllLineIDs.Enabled = true;
                this.lstLineId.Items.Clear();
                this.lstLineId.DataSource = dt;
                this.lstLineId.DataTextField = "LINEID";
                this.lstLineId.DataValueField = "LINEID";
                this.lstLineId.DataBind();
                this.lstLineId.ClearSelection();
            }
        }

        private void SetSublineIdList()
        {
            string strFilter = string.Empty;

            if (this.lstStations.SelectedIndex >= 0)
            {
                StringBuilder sbStations = new StringBuilder();
                foreach (ListItem li in this.lstStations.Items)
                {
                    if (li.Selected)
                    {
                        sbStations.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbStations.ToString().Length > 0)
                {
                    strFilter += " AND WORKCTR IN (" + sbStations.Replace("''", "','") + ") ";
                }
            }

            if (this.lstLineId.SelectedIndex >= 0)
            {
                StringBuilder sbLineIds = new StringBuilder();
                foreach (ListItem li in this.lstLineId.Items)
                {
                    if (li.Selected)
                    {
                        sbLineIds.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbLineIds.ToString().Length > 0)
                {
                    strFilter += " AND LINEID IN (" + sbLineIds.Replace("''", "','") + ") ";
                }
            }
            if (this.lstStations.SelectedIndex >= 0)
            {
                StringBuilder sbStationds = new StringBuilder();
                foreach (ListItem li in this.lstStations.Items)
                {
                    if (li.Selected)
                    {
                        sbStationds.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbStationds.ToString().Length > 0)
                {
                    strFilter += " AND WORKCTR IN (" + sbStationds.Replace("''", "','") + ") ";
                }
            }
            string strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "'" + strFilter;

            DataTable dt = this.dbConn.GetDataSetBySql(strSql + strFilter).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lstSubLineId.Items.Clear();
                this.chkAllSubLineIDs.Enabled = false;
            }
            else
            {
                this.chkAllSubLineIDs.Enabled = true;
                this.lstSubLineId.Items.Clear();
                this.lstSubLineId.DataSource = dt;
                this.lstSubLineId.DataTextField = "SubLineID";
                this.lstSubLineId.DataValueField = "SubLineID";
                this.lstSubLineId.DataBind();
                this.lstSubLineId.ClearSelection();
            }
        }

        private void SetEqIdList(string strtype)
        {
            string strFilter = string.Empty;
            String strSql = string.Empty;
            if (strtype == "SUBLINE")
            {
                if (this.lstSubLineId.SelectedIndex >= 0)
                {
                    StringBuilder sbSubLineIds = new StringBuilder();
                    foreach (ListItem li in this.lstSubLineId.Items)
                    {
                        if (li.Selected)
                        {
                            sbSubLineIds.Append("'").Append(li.Value).Append("'");
                        }
                    }
                    if (sbSubLineIds.ToString().Length > 0)
                    {
                        strFilter += " AND SUBLINEID IN (" + sbSubLineIds.Replace("''", "','") + ") ";
                    }
                }
                strSql = "SELECT EQID FROM EQSM  WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                strSql += strFilter;
                if (this.cmdProcess.Text != "All")
                {
                    strSql += "AND SHOPCD='" + this.cmdProcess.Text.ToString() + "'";
                }
            }
            if (strtype == "EQTYPE")
            {
                if (this.lstEqType.SelectedIndex >= 0)
                {
                    StringBuilder sbEqtypeds = new StringBuilder();
                    foreach (ListItem li in this.lstEqType.Items)
                    {
                        if (li.Selected)
                        {
                            sbEqtypeds.Append("'").Append(li.Value).Append("'");
                        }
                    }
                    if (sbEqtypeds.ToString().Length > 0)
                    {
                        strFilter += " AND EQTID IN (" + sbEqtypeds.Replace("''", "','") + ") ";
                    }
                }
                strSql = "SELECT EQID FROM MEQM  WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                strSql += strFilter;
                if (this.cmdProcess.Text != "All")
                {
                    strSql += "AND SHOPCD='" + this.cmdProcess.Text.ToString() + "'";
                }
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                //Response.Write("Cannot Fetch EQID!");
                this.lstEqId.Items.Clear();
                this.chkAllEQIDs.Enabled = false;
            }
            else
            {
                this.chkAllEQIDs.Enabled = true;
                this.lstEqId.Items.Clear();
                this.lstEqId.DataSource = dt;
                this.lstEqId.DataTextField = "EQID";
                this.lstEqId.DataValueField = "EQID";
                this.lstEqId.DataBind();
                this.lstEqId.ClearSelection();
            }
        }

        private void setEqTypeList()
        {
            string strSql = "SELECT EQTID FROM  METM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' " + " AND EQTID <>''  GROUP BY EQTID ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lstEqType.Items.Clear();
                this.chkAllEQTypes.Enabled = false;
            }
            else
            {
                this.chkAllEQTypes.Enabled = true;
                this.lstEqType.Items.Clear();
                this.lstEqType.DataSource = dt;
                this.lstEqType.DataTextField = "EQTID";
                this.lstEqType.DataValueField = "EQTID";
                this.lstEqType.DataBind();
                this.lstEqType.ClearSelection();
            }
        }
        
        #endregion

        protected void txtFromTime_TextChanged(object sender, EventArgs e)
        {
            foreach (ListItem i in this.lstStations.Items)
            {
                i.Selected = false;
            }
            this.chkAllStations.Checked = false;

        }
    }
}
