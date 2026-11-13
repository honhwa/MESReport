//2014.10.08                 Leo               V1.1          修復BY piece過賬根據LOT查詢箱號數據LOSS 

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
using System.Text;

namespace WebReport.Reports
{
    public partial class OQCBlock : System.Web.UI.Page
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

            }
        }

        protected void Refresh_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }

        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strErrorMsg = "";
            lblErrMsg.Text = "";
            DateTime dtFromTime = new DateTime();
            DateTime dtToTime = new DateTime();
            StringBuilder stb = new StringBuilder();
            bool Err = false;
            DataTable dt = new DataTable();

           // if (ChkByPiece.Checked)
            if (ByLot.Checked==false)
            {
                txtSN.Enabled = true;
                txtLOTID.Enabled = false;    //ADD BY V1.1
                ReportViewer2.Visible = true;
                ReportViewer1.Visible = false;
                string strTable = "";
                string strTable2 = "";
                string strTable3 = "";
                if (BMW.Checked)
                { 
                    strTable = "New_FILEPARM";
                    strTable2 = "LOTID";
                    strTable3="SN";
                }
                if (Panasonic.Checked)
                {
                    strTable = "New_TBL_PANSONICSN";
                    strTable2 = "PANELID";
                    strTable3 = "CUSTOMERSN";
                }
                if (Audi.Checked)
                {
                    strTable = "New_TBL_AudiSN";
                    strTable2 = "PANELID";
                    strTable3 = "CUSTOMERSN";
                }
                if (Audi.Checked)
                {
                    stb.Append("SELECT A.ENTITYID,A.SOLOTID,A.WONBR,A.CARTID,D.SERIALNUMBER AS SerialNo,ISNULL(C." + strTable2 + ",A.ENTITYID)AS PANELID,A.ENTITYTQTY,A.BANKID,A. TRANSDATE,A.PRODID ");
                }
                else
                {
                    stb.Append("SELECT A.ENTITYID,A.SOLOTID,A.WONBR,A.CARTID,b.SerialNo,ISNULL(C." + strTable2 + ",A.ENTITYID)AS PANELID,A.ENTITYTQTY,A.BANKID,A. TRANSDATE,A.PRODID ");
                }
                stb.Append("FROM New_WLTM A ");
                stb.Append("left JOIN New_WCMSD B ON A.CARTID=b.CARTID AND A.ENTITYID=B.ENTITYID  ");               
                stb.Append("LEFT JOIN "+strTable+" c ON b.SerialNo=c."+strTable3  );
                if (Audi.Checked)
                {
                    stb.Append(" LEFT JOIN New_FILEPARM_AUDI D ON D.PANELID=ISNULL(C.PANELID,A.ENTITYID)");
                }
                stb.Append(" WHERE HIDEFLG='1' AND BANKID<>'NULL'");
                if (txtSN.Text != string.Empty)
                {

                    string strSN= txtSN.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND C."+strTable3+" IN ('{0}')", strSN);
                }
            }
            else
            {
                txtSN.Enabled = false;
                txtLOTID.Enabled = true;    //ADD BY V1.1
                txtSN.Text = "";
                ReportViewer1.Visible = true;
                ReportViewer2.Visible = false;
                stb.Append("SELECT ENTITYID,SOLOTID,WONBR,CARTID,BANKID,TRANSDATE,ENTITYTQTY,PRODID FROM New_WLTM A ");
                stb.Append("WHERE A.HIDEFLG='1' AND A.BANKID<>'NULL'");
            }
            if ((Chkdate.Checked) &&((txtFromTime.Text == string.Empty) || (txtToTime.Text == string.Empty)))
            {
                strErrorMsg += "開始時間或結束時間為空，請重輸!\n";
                Err = true;
            }
            else if ((Chkdate.Checked) && ((txtFromTime.Text != string.Empty) && (txtToTime.Text != string.Empty)))
            {

                string strFromTime = this.txtFromTime.Text.ToString();
                string strToTime = this.txtToTime.Text.ToString();
                if (!DateTime.TryParse(strFromTime, out dtFromTime))
                {
                    strErrorMsg += "開始時間錯誤！\n";
                    Err = true;
                }
                if (!DateTime.TryParse(strToTime, out dtToTime))
                {
                    strErrorMsg += "結束時間錯誤！\n";
                    Err = true;               
                }
                if (dtFromTime > dtToTime)
                {
                    strErrorMsg += "結束時間在開始時間之前！\n";
                    Err = true;
                }
                else
                    stb.Append("  AND A.TRANSDATE>='" + strFromTime + "' AND TRANSDATE<='" + strToTime + "'");
            }

            if (this.ChkLOTID.Checked)
            {
                if (txtLOTID.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入LOTID再查詢，謝謝！\n";

                    Err = true;
                }
                else
                {
                    string strLOTID= txtLOTID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND A.SOLOTID IN ('{0}')", strLOTID);
                }
            }
            if (this.ChkWONBR.Checked)
            {
                if (txtWONBR.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入WONBR再查詢，謝謝！\n";

                    Err = true;
                }
                else
                {
                    string strWONBR= txtWONBR.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND A.WONBR IN ('{0}')", strWONBR);
                }
            }
            if (this.ChkPRODID.Checked)
            {
                if (txtPRODID.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入PRODID再查詢，謝謝！\n";

                    Err = true;
                }
                else
                {
                    string strPRODID = txtPRODID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND A.PRODID IN ('{0}')", strPRODID);
                }
            }
            if (ChkLOTID.Checked == false && ChkPRODID.Checked == false && Chkdate.Checked == false && ChkWONBR.Checked == false && txtSN.Enabled == false)
            {
                strErrorMsg += "請選擇查詢條件！\n";
                Err = true;
            }

            if (Err)
            {
                lblErrMsg.Text = strErrorMsg;
            }
            else
            {
                stb.Append("ORDER BY A.TRANSDATE");
                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    lblErrMsg.Text = strErrorMsg;
                }
            }


            // if (ChkByPiece.Checked)
            if (ByLot.Checked==false)
            {
                this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer2.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\OQCPieceBlock.rdlc";
                ReportDataSource rsd = new ReportDataSource("OQCPieceBlock", dt); 
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);

            }
            else
            {
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\OQCLotBlock.rdlc";
                ReportDataSource rsd = new ReportDataSource("LotWIP_WLTM", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }

        }

        protected void ChkLOTID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkLOTID.Checked)
            {
                if (ByLot.Checked)                //ADD BY V1.1
                {
                    this.txtLOTID.Enabled = true;
                }
            }
            else
            {
                this.txtLOTID.Enabled = false;
                this.txtLOTID.Text = "";
            }
        }

        protected void ChkWONBR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWONBR.Checked)
            {
                this.txtWONBR.Enabled = true;
            }
            else
            {
                this.txtWONBR.Enabled = false;
                this.txtWONBR.Text = "";
            }
        }

        protected void ChkPRODID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPRODID.Checked)
            {
                this.txtPRODID.Enabled = true;
            }
            else
            {
                this.txtPRODID.Enabled = false;
                this.txtPRODID.Text = "";
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
                this.txtFromTime.Text = "";
                this.txtToTime.Text = "";
            }
        }

        /*     add  by V1.1   */
        protected void ByWhat_CheckedChanged(object sender, EventArgs e)
        {
            if (ByLot.Checked && ChkLOTID.Checked)
            {
                this.txtLOTID.Enabled = true;
            }
            else
            {
                this.txtLOTID.Enabled = false;
            }
        }
        /*     END  by V1.1   */
     
}
}
