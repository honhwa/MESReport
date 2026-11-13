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
    public partial class MaterialLotSNPanel : System.Web.UI.Page
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
            stb.Append("SELECT FABID,TransDate , PRODID,ENTITYID,PARTID,PARTTYPE");
            stb.Append(",SUBSTRING(PARTLOTID,0,CASE WHEN PATINDEX('%/%',PARTLOTID)<=0 THEN LEN(PARTLOTID)+1 ELSE  PATINDEX('%/%',PARTLOTID) END) as PARTLOTID");
            stb.Append(",SUBSTRING(PARTLOTID,CASE WHEN PATINDEX('%/%',PARTLOTID)<=0 THEN 0 ELSE  PATINDEX('%/%',PARTLOTID)+1 END,CASE WHEN PATINDEX('%/%',PARTLOTID)<=0 THEN 0 ELSE  LEN(PARTLOTID) END) as PARTSN");
            stb.Append(",SUBLINEID,WONBR,WORKCTR,USERID,SHIFTID ,CMT  FROM  New_WPKX WHERE PRODID IN (SELECT PRODID FROM MPIM WHERE PFLAG<>'0')");

            if ((Chkdate.Checked) && (txtFromTime.Text == string.Empty) && (txtToTime.Text == string.Empty))
            {
                lblErrMsg.Text = "開始時間或結束時間為空，請重輸!\n";
                IsErr = true;
                return;
            }
            else if ((Chkdate.Checked) && (txtFromTime.Text != string.Empty) && (txtToTime.Text != string.Empty))
            {
                stb.Append("  AND TRANSDATE>='" + txtFromTime.Text.Trim().Replace("'", "") + "' AND TRANSDATE<='" + txtToTime.Text.Trim().Replace("'","")+"'");
            }

            if ((ChkEntityid.Checked) && (TxtEntityid.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入批號或SN!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkEntityid.Checked) && (TxtEntityid.Text !=string.Empty))
            {
                //if (TxtEntityid.Text.Length == 14)
                //{
                //    stb.Append("  AND ENTITYID IN(SELECT ENTITYID FROM WSSD WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'" + " AND PREFIX=LEFT('" + TxtEntityid.Text.Trim().Replace("'", "") + "'" + ",7) ");
                //    stb.Append("  AND RIGHT('" + TxtEntityid.Text.Trim().Replace("'", "") + "'" + ",LEN('" + TxtEntityid.Text.Trim().Replace("'", "") + "')-7)");
                //    stb.Append("  BETWEEN FRSN AND TOSN");
                //}
                //else
                //{
                    //stb.Append(" AND ENTITYID ='"+TxtEntityid.Text.Trim().Replace("'", "")+"'");
                //}
                stb.Append(" AND (ENTITYID ='" + TxtEntityid.Text.Trim().Replace("'", "") + "'");
                stb.Append(" OR ENTITYID IN (SELECT entityid from wssd where CMT='" + TxtEntityid.Text.Trim().Replace("'", "") + "'))");              
            }
            if ((ChkMaterial.Checked) && (this.TxtMaterial.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入正確的料號!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkMaterial.Checked) && (this.TxtMaterial.Text != string.Empty))
            {
                stb.Append(" AND PARTID='" + this.TxtMaterial.Text.Trim().Replace("'","")+"'");
            }
            if ((ChkBatchId.Checked) && (this.TxtBatchId.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入正確BatchId!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkBatchId.Checked) && (this.TxtBatchId.Text != string.Empty))
            {
                stb.Append(" AND SUBSTRING(PARTLOTID,0,CASE WHEN PATINDEX('%/%',PARTLOTID)<=0 THEN LEN(PARTLOTID)+1 ELSE  PATINDEX('%/%',PARTLOTID) END) ='" + this.TxtBatchId.Text.Trim().Replace("'", "") + "'");
            }



            if ((ChkSN.Checked) && (this.TxtSN.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入正確的MATERIAL SN!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkSN.Checked) && (this.TxtSN.Text != string.Empty))
            {
                stb.Append(" AND SUBSTRING(PARTLOTID,CASE WHEN PATINDEX('%/%',PARTLOTID)<=0 THEN 0 ELSE  PATINDEX('%/%',PARTLOTID)+1 END,CASE WHEN PATINDEX('%/%',PARTLOTID)<=0 THEN 0 ELSE  LEN(PARTLOTID) END) ='" + this.TxtSN.Text.Trim().Replace("'", "") + "'");
            }


            if ((ChkEntityid.Checked == false) && (ChkMaterial.Checked == false) && (ChkBatchId.Checked == false) && (ChkSN.Checked == false) && (Chkdate.Checked == false))
            {
                lblErrMsg.Text = "查詢條件不能全為空!!!\n";
                IsErr = true;
                return;
            }
            stb.Append(" ORDER BY TRANSDATE,PRODID");
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
            localReportRoqcs.ReportPath = @"RDLC\MaterialLotSNPanel.rdlc";


            ReportDataSource rsd = new ReportDataSource("MaterialLotSNPanel_WPKX", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
        }

        protected void ChkEntityid_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkEntityid.Checked)
            {
                this.TxtEntityid.Enabled = true;
            }
            else
            {
                this.TxtEntityid.Enabled = false;
                this.TxtEntityid.Text = "";
            }

        }

        protected void ChkMaterial_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkMaterial.Checked)
            {
                this.TxtMaterial.Enabled = true;
            }
            else
            {
                this.TxtMaterial.Enabled = false;
                this.TxtMaterial.Text = "";
            }
        }

        protected void ChkBatchId_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBatchId.Checked)
            {
                this.TxtBatchId.Enabled = true;
            }
            else
            {
                this.TxtBatchId.Enabled = false;
                this.TxtBatchId.Text = "";
            }
        }

        protected void ChkSN_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSN.Checked)
            {
                this.TxtSN.Enabled = true;
            }
            else
            {
                this.TxtSN.Enabled = false;
                this.TxtSN.Text = "";
            }
        }

        protected void Chkdate_CheckedChanged(object sender, EventArgs e)
        {
            if (Chkdate.Checked)
            {
                this.txtFromTime.Enabled = true;
                this.txtToTime.Enabled = true;
            }
            else
            {
                this.txtFromTime.Enabled = false;
                this.txtToTime.Enabled = false;
            }
        }
    }
}
