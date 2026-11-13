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
    public partial class MaterialK : System.Web.UI.Page
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
            stb.Append("SELECT DISTINCT A.FABID, A.TransDate ,A.PRODID, A.ENTITYID, A.PARTID, A.PARTTYPE, case when (B.orderBarCode IS NULL ) THEN A.PARTLOTID ELSE B.orderBarCode END AS PARTLOTID,A.SUBLINEID, A.WONBR, A.WORKCTR, A.USERID, A.SHIFTID,A.CMT, case when c.Rework_times = '3' THEN '2' ELSE c.Rework_times END AS Rework_times FROM New_VwpkxA A left JOIN New_TBL_IrrBarCode_Record B ON A.ENTITYID = B.PANELID AND A.PARTID = B.partid left JOIN (SELECT distinct orderBarCode,COUNT (*) AS Rework_times FROM(SELECT orderBarCode FROM New_TBL_IrrBarCode_Record WHERE FABID= 'B' AND UPFLG ='Y'UNION ALL SELECT orderBarCode FROM TBL_tIrrBarCode_Record_H WHERE FABID= 'B' )a GROUP BY a.orderBarCode) AS C ON b.orderBarCode=C.orderBarCode");
            if ((txtFromTime.Text == string.Empty) || (txtToTime.Text == string.Empty))
            {
                lblErrMsg.Text = "開始時間或結束時間為空，請重輸!\n";
                return;
            }
            else
            {
                stb.Append(" WHERE a.TRANSDATE>='" + txtFromTime.Text.Trim().Replace("'", "") + "' AND a.TRANSDATE<='" + txtToTime.Text.Trim().Replace("'","")+"'");
            }

            if ((ChkEntityid.Checked) && (TxtEntityid.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入批號或SN!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkEntityid.Checked) && (TxtEntityid.Text !=string.Empty))
            {
            
                string strEntityid = TxtEntityid.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                int intEntityCount = strEntityid.Split(',').Length - 1;
                if (intEntityCount >300)
                {
                    lblErrMsg.Text = $@"查詢數量{intEntityCount}超出查詢資料最大數量300!!!\n";
                    IsErr = true;
                    return;
                }
                stb.AppendFormat(" AND a.ENTITYID IN ('{0}')", strEntityid);
                
            }
            if ((ChkMaterial.Checked) && (this.TxtMaterial.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入正確的料號!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkMaterial.Checked) && (this.TxtMaterial.Text != string.Empty))
            {
                string strMaterial = TxtMaterial.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                stb.AppendFormat(" AND a.PARTID IN ('{0}')", strMaterial);
            }
            if ((ChkBatchId.Checked) && (this.TxtBatchId.Text == string.Empty))
            {
                lblErrMsg.Text = "請輸入正確BatchId!!!\n";
                IsErr = true;
                return;
            }
            else if ((ChkBatchId.Checked) && (this.TxtBatchId.Text != string.Empty))
            {
                string strBatchId = TxtBatchId.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                stb.AppendFormat(" AND a.PARTLOTID  IN ('{0}')", strBatchId);
            }
            //if ((ChkEntityid.Checked == false) && (ChkMaterial.Checked == false) && (ChkBatchId.Checked == false))
            //{
            //    lblErrMsg.Text = "請輸入正確查詢條件!!!\n";
            //    IsErr = true;
            //}
            stb.Append(" ORDER BY a.TRANSDATE,a.PRODID");
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
            localReportRoqcs.ReportPath = @"RDLC\MaterialK.rdlc";


            ReportDataSource rsd = new ReportDataSource("MaterialK", dt);
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
    }
}
