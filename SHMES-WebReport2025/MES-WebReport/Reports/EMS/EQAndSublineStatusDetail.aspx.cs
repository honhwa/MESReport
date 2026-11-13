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
    public partial class EQAndSublineStatusDetail : System.Web.UI.Page
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
                this.txtFrDate.Text = "";
                this.txtToDate.Text = "";
                this.ddModulCell.Items.Add("ALL");
                this.ddModulCell.Items.Add("MODULE");
                this.ddModulCell.Items.Add("CELL2");
                this.ddModulCell.SelectedIndex =1;
                this.ddLineEq.Items.Add("ALL");
                this.ddLineEq.Items.Add("LINE");
                this.ddLineEq.Items.Add("EQ");
                this.ddLineEq.SelectedIndex = 1;
                this.SetStation();
                this.SetEQType();
                this.SetEQID();
                this.SetLineID();
                this.SetSublineID();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }
        private void SetStation()
        {
            this.lstStations.Items.Clear();
            string strSql = @"SELECT WORKCTR,WORKCTR+'   '+WORKNAME AS WORKNAME FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
            if (!this.ddModulCell.Text.Equals("ALL"))
            {
                strSql += " AND SHOPCD='" + this.ddModulCell.Text + "'";
            }
            strSql +=" ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstStations.DataSource = dt;
            this.lstStations.DataTextField = "WORKNAME";
            this.lstStations.DataValueField = "WORKCTR";
            this.lstStations.DataBind();
            this.lstStations.ClearSelection();
        }
        private void SetEQType()
        {
            string strSql = "SELECT EQTID FROM  METM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' " + " AND EQTID <>''  GROUP BY EQTID ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lstEQType.Items.Clear();
                this.chkAllEQType.Enabled = false;
            }
            else
            {
                this.chkAllEQType.Enabled = true;
                this.lstEQType.Items.Clear();
                this.lstEQType.DataSource = dt;
                this.lstEQType.DataTextField = "EQTID";
                this.lstEQType.DataValueField = "EQTID";
                this.lstEQType.DataBind();
                this.lstEQType.ClearSelection();
            }
        }
        private void SetEQID()
        {
            string strFilter = string.Empty;
            string strSql = string.Empty;
            strSql = "SELECT EQID FROM MEQM  WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
            if (this.lstEQType.SelectedIndex >= 0)
            {
                StringBuilder sbEqtypeds = new StringBuilder();
                foreach (ListItem li in this.lstEQType.Items)
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
                strSql += strFilter;
            }
            if (!this.ddModulCell.Text.Equals("ALL"))
            {
                strSql += "AND SHOPCD='" + this.ddModulCell.Text.ToString() + "'";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lstEQID.Items.Clear();
                this.chkAllEQID.Enabled = false;
            }
            else
            {
                this.chkAllEQID.Enabled = true;
                this.lstEQID.Items.Clear();
                this.lstEQID.DataSource = dt;
                this.lstEQID.DataTextField = "EQID";
                this.lstEQID.DataValueField = "EQID";
                this.lstEQID.DataBind();
                this.lstEQID.ClearSelection();
            }
        }
        private void SetLineID()
        {
            string strFilter = string.Empty;
            string strSql = "SELECT LINEID FROM MLINEM  WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' " ;
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                //Response.Write("Cannot Fetch LineID!");
                this.lstLine.Items.Clear();
                this.chkAllLine.Enabled = false;
            }
            else
            {
                this.chkAllLine.Enabled = true;
                this.lstLine.Items.Clear();
                this.lstLine.DataSource = dt;
                this.lstLine.DataTextField = "LINEID";
                this.lstLine.DataValueField = "LINEID";
                this.lstLine.DataBind();
                this.lstLine.ClearSelection();
            }
        }
        private void SetSublineID()
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
            if (this.lstLine.SelectedIndex >= 0)
            {
                StringBuilder sbLineIds = new StringBuilder();
                foreach (ListItem li in this.lstLine.Items)
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
            string strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "'" + strFilter;

            DataTable dt = this.dbConn.GetDataSetBySql(strSql + strFilter).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lstSubline.Items.Clear();
                this.chkAllSubline.Enabled = false;
            }
            else
            {
                this.chkAllSubline.Enabled = true;
                this.lstSubline.Items.Clear();
                this.lstSubline.DataSource = dt;
                this.lstSubline.DataTextField = "SubLineID";
                this.lstSubline.DataValueField = "SubLineID";
                this.lstSubline.DataBind();
                this.lstSubline.ClearSelection();
            }
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            DateTime dtFromTime = new DateTime();
            DateTime dtToTime = new DateTime();
            this.lblErrMsg.Text = "";
            if (this.txtFrDate.Text.Trim().Replace("'", "") == "" || this.txtFrDate.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }
            string strFromTime = this.txtFrDate.Text.ToString();
            string strToTime = this.txtToDate.Text.ToString();
            if (!DateTime.TryParse(strFromTime, out dtFromTime))
            {
                strErrorMsg += "結束時間錯誤！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }
            if (!DateTime.TryParse(strToTime, out dtToTime))
            {
                strErrorMsg += "結束時間錯誤！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }
            if (dtFromTime > dtToTime)
            {
                strErrorMsg += "結束時間在開始時間之前！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strSqlEQ = string.Empty;
            string strSqlSubline = string.Empty;
            DataTable dtResult = new DataTable();
            DataSet dsEQ =new DataSet ();
            DataSet dsSubline=new DataSet();
            if (this.ddLineEq.Text.Equals("EQ") || this.ddLineEq.Text.Equals("ALL"))
            {
                strSqlEQ = @"SELECT PRODID,SHIFTID,CMT,EQID,SUBLINEID,WORKCTR,EQCD,EQCCD,DOWNEQID,STARTDT,ENDDT,DATEDIFF(second,STARTDT,ENDDT)/60 AS SUMDATE,USERID FROM (";
                strSqlEQ += "  SELECT SH.PRODID,SH.SHIFTID,SH.CMT,SH.EQID,SH.SUBLINEID,SH.WORKCTR,SH.PREVEQCCD AS EQCCD,SH.PREVSTATUS AS EQCD,'' AS DOWNEQID,";
                strSqlEQ += "  CASE WHEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.STARTDATE,120))<CONVERT(VARCHAR(19),'" + strFromTime + "', 120) ";
                strSqlEQ += "  THEN   CONVERT(VARCHAR(19),'" + strFromTime + "', 120) ELSE  CONVERT(VARCHAR(19),SH.STARTDATE ,120) END AS STARTDT ,";
                strSqlEQ += "  CASE WHEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.ENDDATE,120))<CONVERT(VARCHAR(19),'" + strToTime + "', 120) THEN";
                strSqlEQ += "  CONVERT(VARCHAR(19),SH.ENDDATE,120) ELSE CONVERT(VARCHAR(19),'" + strToTime + "', 120) END AS ENDDT,SH.USERID+'  / '+ISNULL(EM.USERNAME,'') AS USERID FROM New_EQSHM SH";
                strSqlEQ += "  LEFT JOIN MEMM EM ON SH.FABID=EM.FABID AND SH.USERID=EM.USERID WHERE ";
                strSqlEQ += " ((CONVERT(VARCHAR(19),SH.STARTDATE,120) BETWEEN CONVERT(VARCHAR(19),'" + strFromTime + "', 120)";
                strSqlEQ += "  AND CONVERT(VARCHAR(19),'" + strToTime + "', 120)AND SH.SUBLINEID NOT IN ('BMXXX-01','BCXXX-01'))";
                strSqlEQ += "  OR";
                strSqlEQ += "  (CONVERT(VARCHAR(19),SH.ENDDATE,120) BETWEEN CONVERT(VARCHAR(19),'" + strFromTime + "', 120)";
                strSqlEQ += "  AND CONVERT(VARCHAR(19),'" + strToTime + "', 120) AND SH.SUBLINEID NOT IN ('BMXXX-01','BCXXX-01'))";
                strSqlEQ += "  OR";
                strSqlEQ += " (CONVERT(VARCHAR(19),SH.ENDDATE,120) > CONVERT(VARCHAR(19),'" + strToTime + "', 120)";
                strSqlEQ += "  AND CONVERT(VARCHAR(19),SH.STARTDATE,120) < CONVERT(VARCHAR(19),'" + strFromTime + "', 120)))";
                strSqlEQ += "  AND SH.FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "'";
                strSqlEQ += "  AND SH.SUBLINEID NOT LIKE 'BMCOG002' AND SH.SUBLINEID NOT LIKE 'BMXXX-%' AND SH.SUBLINEID NOT LIKE 'BMACS%' AND SH.SUBLINEID NOT LIKE 'BMSLC-%'";
                if (!this.ddModulCell.Text.Equals("ALL"))
                {
                    strSqlEQ += "  AND SH.SHOPCD= '" + this.ddModulCell.Text + "'";
                }
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
                    strSqlEQ += " AND SH.WORKCTR IN (" + sbStations.Replace("''", "','") + ") ";
                }                
                StringBuilder sbEqID = new StringBuilder();
                foreach (ListItem li in this.lstEQID.Items)
                {
                    if (li.Selected)
                    {
                        sbEqID.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbEqID.ToString().Length > 0)
                {
                    strSqlEQ += " AND SH.EQID IN (" + sbEqID.Replace("''", "','") + ") ";
                }
                strSqlEQ += "  UNION SELECT SH.PRODID,SH.SHIFTID,SH.CMT,SH.EQID,SH.SUBLINEID,SH.WORKCTR,SH.EQCCD,SH.EQCD,'' AS DOWNEQID,";
                strSqlEQ += "   CASE WHEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.TRANSDATE,120))>CONVERT(VARCHAR(19),'" + strFromTime + "', 120)";
                strSqlEQ += "   THEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.TRANSDATE,120)) ELSE '" + strFromTime + "' END AS STARTDT,";
                strSqlEQ += "'" + strToTime + "' AS ENDDT,";
                strSqlEQ += "  SH.USERID+'  / '+ISNULL(EM.USERNAME,'') AS USERID  FROM New_EQSM SH";
                strSqlEQ += "  LEFT JOIN MEMM EM ON SH.FABID=EM.FABID AND SH.USERID=EM.USERID WHERE ";
                strSqlEQ += " (CONVERT(VARCHAR(19),SH.TRANSDATE,120)) < CONVERT(VARCHAR(19),'" + strToTime + "', 120)";
                strSqlEQ += " AND SH.FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "'";
                strSqlEQ += "  AND SH.SUBLINEID NOT LIKE 'BMCOG002' AND SH.SUBLINEID NOT LIKE 'BMXXX-%' AND SH.SUBLINEID NOT LIKE 'BMACS%' AND SH.SUBLINEID NOT LIKE 'BMSLC-%'";
                if (!this.ddModulCell.Text.Equals("ALL"))
                {
                    strSqlEQ += " AND SH.SHOPCD= '" + this.ddModulCell.Text + "'";
                }
                if (sbStations.ToString().Length > 0)
                {
                    strSqlEQ += " AND SH.WORKCTR IN (" + sbStations.Replace("''", "','") + ") ";
                }
                if (sbEqID.ToString().Length > 0)
                {
                    strSqlEQ += " AND SH.EQID IN (" + sbEqID.Replace("''", "','") + ") ";
                }
                strSqlEQ += "  )C ORDER BY EQID,STARTDT,ENDDT";
                dsEQ = this.dbConn.GetDataSetBySql(strSqlEQ);
                dtResult = dsEQ.Tables[0].Copy();
            }
            if (this.ddLineEq.Text.Equals("LINE") || this.ddLineEq.Text.Equals("ALL"))
            {
                strSqlSubline = @"SELECT PRODID,SHIFTID,CMT,EQID,SUBLINEID,WORKCTR,EQCD,EQCCD,DOWNEQID,STARTDT,ENDDT,DATEDIFF(second,STARTDT,ENDDT)/60 AS SUMDATE,USERID FROM (";
                strSqlSubline += "  SELECT SH.PRODID,SH.SHIFTID,SH.CMT,SL.LINEID AS EQID,SH.SUBLINEID,SH.WORKCTR,SH.PREVEQCCD AS EQCCD,SH.PREVSTATUS AS EQCD,";
                strSqlSubline += "  CASE WHEN SH.PREVEQCCD='D' THEN ESH.EQID ELSE '' END AS DOWNEQID, ";
                strSqlSubline += "  CASE WHEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.STARTDATE,120))<CONVERT(VARCHAR(19),'" + strFromTime + "', 120) ";
                strSqlSubline += "  THEN   CONVERT(VARCHAR(19),'" + strFromTime + "', 120) ELSE   CONVERT(VARCHAR(19),SH.STARTDATE,120) END AS STARTDT ,";
                strSqlSubline += "  CASE WHEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.ENDDATE,120))<CONVERT(VARCHAR(19),'" + strToTime + "', 120) THEN";
                strSqlSubline += "  CONVERT(VARCHAR(19),SH.ENDDATE,120) ELSE CONVERT(VARCHAR(19),'" + strToTime + "', 120) END AS ENDDT,SH.USERID+'  / '+ISNULL(EM.USERNAME,'') AS USERID FROM New_ESLSH SH";
                strSqlSubline += "  INNER JOIN MSLM SL ON SH.FABID=SL.FABID AND SH.SUBLINEID=SL.SUBLINEID ";
                strSqlSubline += "  LEFT JOIN MEMM EM ON SH.FABID=EM.FABID AND SH.USERID=EM.USERID ";
                strSqlSubline += "  LEFT OUTER JOIN New_EQSHM ESH ON SH.FABID=ESH.FABID AND SH.SUBLINEID=ESH.SUBLINEID AND SH.STARTDATE=ESH.STARTDATE AND SH.ENDDATE=ESH.ENDDATE  AND ESH.PREVEQCCD='D'";
                strSqlSubline += "  WHERE ((CONVERT(VARCHAR(19),SH.STARTDATE,120) BETWEEN CONVERT(VARCHAR(19),'" + strFromTime + "', 120)";
                strSqlSubline += "  AND CONVERT(VARCHAR(19),'" + strToTime + "', 120))";
                strSqlSubline += "  AND SH.SUBLINEID NOT LIKE 'BMCOG002' AND SH.SUBLINEID NOT LIKE 'BMXXX-%' AND SH.SUBLINEID NOT LIKE 'BMACS%' AND SH.SUBLINEID NOT LIKE 'BMSLC-%'";
                strSqlSubline += "  OR";
                strSqlSubline += "  (CONVERT(VARCHAR(19),SH.ENDDATE,120) BETWEEN CONVERT(VARCHAR(19),'" + strFromTime + "', 120)";
                strSqlSubline += "  AND CONVERT(VARCHAR(19),'" + strToTime + "', 120))";
                strSqlSubline += "  OR";
                strSqlSubline += "  (CONVERT(VARCHAR(19),SH.ENDDATE,120) > CONVERT(VARCHAR(19),'" + strToTime + "', 120)";
                strSqlSubline += "  AND CONVERT(VARCHAR(19),SH.STARTDATE,120) < CONVERT(VARCHAR(19),'" + strFromTime + "', 120)))";
                strSqlSubline += "  AND SH.FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "'";
                if (!this.ddModulCell.Text.Equals("ALL"))
                {
                    strSqlSubline += "  AND SH.SHOPCD= '" + this.ddModulCell.Text + "'";
                }
                StringBuilder sbSubline = new StringBuilder();
                StringBuilder sbStation = new StringBuilder();
                foreach (ListItem li in this.lstStations.Items)
                {
                    if (li.Selected)
                    {
                        sbStation.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbStation.ToString().Length > 0)
                {
                    strSqlSubline += " AND SH.WORKCTR IN (" + sbStation.Replace("''", "','") + ") ";
                }
                foreach (ListItem li in this.lstSubline.Items)
                {
                    if (li.Selected)
                    {
                        sbSubline.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbSubline.ToString().Length > 0)
                {
                    strSqlSubline += " AND SH.SUBLINEID IN (" + sbSubline.Replace("''", "','") + ") ";
                }
                strSqlSubline += " UNION SELECT SH.PRODID,SH.SHIFTID,SH.CMT,SL.LINEID AS EQID,SH.SUBLINEID,SH.WORKCTR,SH.EQCCD AS EQCCD,SH.SUBLINESTATUS AS EQCD,'' AS DOWNEQID,";
                strSqlSubline += "   CASE WHEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.TRANSDATE,120))>CONVERT(VARCHAR(19),'" + strFromTime + "', 120)";
                //strSqlSubline += "   THEN CONVERT(DATETIME,CONVERT(VARCHAR(19),SH.TRANSDATE,120)) ELSE '" + strFromTime + "' END AS STARTDT,";
                strSqlSubline += "   THEN CONVERT(VARCHAR(19),SH.TRANSDATE,120) ELSE '" + strFromTime + "' END AS STARTDT,";
                strSqlSubline +=  "'"  + strToTime + "' AS ENDDT,";
                strSqlSubline += " SH.USERID+' /   '+ISNULL(EM.USERNAME,'') AS USERID  FROM New_ESLS SH";
                strSqlSubline +="  INNER JOIN MSLM SL ON SH.FABID=SL.FABID AND SH.SUBLINEID=SL.SUBLINEID";
                strSqlSubline +="  LEFT JOIN MEMM EM ON SH.FABID=EM.FABID AND SH.USERID=EM.USERID WHERE ";
                strSqlSubline += " (CONVERT(VARCHAR(19),SH.TRANSDATE,120))< CONVERT(VARCHAR(19),'" + strToTime + "', 120)";
                strSqlSubline += " AND SH.FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "'";
                strSqlSubline += "  AND SH.SUBLINEID NOT LIKE 'BMCOG002' AND SH.SUBLINEID NOT LIKE 'BMXXX-%' AND SH.SUBLINEID NOT LIKE 'BMACS%' AND SH.SUBLINEID NOT LIKE 'BMSLC-%'";
                if (!this.ddModulCell.Text.Equals("ALL"))
                {
                    strSqlSubline += " AND SH.SHOPCD= '" + this.ddModulCell.Text + "'";
                }
                if (sbStation.ToString().Length > 0)
                {
                    strSqlSubline += " AND SH.WORKCTR IN (" + sbStation.Replace("''", "','") + ") ";
                }
                if (sbSubline.ToString().Length > 0)
                {
                    strSqlSubline += " AND SH.SUBLINEID IN (" + sbSubline.Replace("''", "','") + ") ";
                }
                strSqlSubline += "  )C ORDER BY SUBLINEID,STARTDT,ENDDT"; 
                dsSubline = this.dbConn.GetDataSetBySql(strSqlSubline);
                dtResult = dsSubline.Tables[0].Copy();
            }
            if (this.ddLineEq.Text.Equals("ALL"))
            {
                dtResult.Merge(dsEQ.Tables[0]);
            }
            
            this.RVStatusDetail.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport1 = RVStatusDetail.LocalReport;
            localReport1.ReportPath = @"RDLC\EQAndSublineStatusDetail.rdlc";
            ReportDataSource rds1 = new ReportDataSource();
            rds1.Name = "EQAndSublineStatusDetail";
            rds1.Value = dtResult;
            localReport1.DataSources.Clear();
            localReport1.DataSources.Add(rds1);
        }
        protected void chkAllEQID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllEQID.Checked)
            {
                foreach (ListItem i in this.lstEQID.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstEQID.Items)
                {
                    i.Selected = false;
                }
            }
        }
        protected void chkAllEQType_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllEQType.Checked)
            {
                foreach (ListItem i in this.lstEQType.Items)
                {
                    i.Selected = true;
                }
                this.SetEQID();
            }
            else
            {
                foreach (ListItem i in this.lstEQType.Items)
                {
                    i.Selected = false;
                }
            }
        }
        protected void chkAllLine_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllLine.Checked)
            {
                foreach (ListItem i in this.lstLine.Items)
                {
                    i.Selected = true;
                }
                this.SetSublineID();
            }
            else
            {
                foreach (ListItem i in this.lstLine.Items)
                {
                    i.Selected = false;
                }
            }
        }
        protected void chkAllSubline_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllSubline.Checked)
            {
                foreach (ListItem i in this.lstSubline.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstSubline.Items)
                {
                    i.Selected = false;
                }
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
        }

        protected void ddModulCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetStation();
        }

        protected void lstEQType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetEQID();
        }

        protected void lstLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetSublineID();
        }
    }
}
