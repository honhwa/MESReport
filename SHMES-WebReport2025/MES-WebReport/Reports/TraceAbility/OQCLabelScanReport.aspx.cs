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
/*Leo   20140805 Add*/
namespace WebReport.Reports
{
    public partial class OQCLabelScanReport : System.Web.UI.Page
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

        protected void ChkLOTID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkLOTID.Checked)
            {
                this.txtLOTID.Enabled = true;
            }
            else
            {
                this.txtLOTID.Enabled = false;
                this.txtLOTID.Text = "";
            }
        }

        protected void ChkPANELID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPANELID.Checked)
            {
                this.txtPANELID.Enabled = true;
            }
            else
            {
                this.txtPANELID.Enabled = false;
                this.txtPANELID.Text = "";
            }
        }
        protected void ChkSN_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSN.Checked)
            {
                this.txtSN.Enabled = true;
            }
            else
            {
                this.txtSN.Enabled = false;
                this.txtSN.Text = "";
            }
        }
        protected void ChkBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBarcode.Checked)
            {
                this.txtBarcode.Enabled = true;
            }
            else
            {
                this.txtBarcode.Enabled = false;
                this.txtBarcode.Text = "";
            }
        }
        protected void ButQ_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strErrorMsg = "";
            lblErr.Text = "";
            bool Err = false;
            DataTable dt = new DataTable();
            StringBuilder stb = new StringBuilder();
            ReportViewer1.Visible = true;
            stb.Append("SELECT LotID,PANELID,Barcode,SN FROM New_TBL_OQCSN Where Barcode<>''");
            
            if (this.ChkLOTID.Checked)
            {
                if (txtLOTID.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入LotID再查詢，謝謝!\n";

                    Err = true;
                }
                else
                {
                    string strLOTID = txtLOTID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND LOTID IN ('{0}')", strLOTID);
                }
            }

            if (this.ChkPANELID.Checked)
            {
                if (txtPANELID.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入PANELID再查詢，謝謝!\n";

                    Err = true; 
                }
                else
                {
                    string strPANELID = txtPANELID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND PANELID  IN ('{0}')", strPANELID);
                }
            }

            if (this.ChkBarcode.Checked)
            {
                if (txtBarcode.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入Barcode再查詢，謝謝！\n";

                    Err = true;
                }
                else
                {
                    string strBarcode = txtBarcode.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND Barcode IN ('{0}')", strBarcode);
                }
            }

            if (this.ChkSN.Checked)
            {
                if (txtSN.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入SN再查詢，謝謝！\n";

                    Err = true;
                }
                else
                {
                    string strSN = txtSN.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    //stb.AppendFormat(" AND SN IN ('{0}')", strSN);
                    stb.Append(" AND PANELID IN (SELECT LOTID AS PANELID FROM New_FILEPARM WHERE SN='" + strSN + "' UNION SELECT PANELID FROM New_FILEPARM_AUDI WHERE SERIALNUMBER='" + strSN + "'  ");
                    stb.Append("  UNION SELECT PANELID FROM New_TBL_PANSONICSN WHERE CUSTOMERSN='" + strSN + "')  ");
                }
            }

            if (ChkLOTID.Checked == false && ChkPANELID.Checked == false && ChkBarcode.Checked == false && ChkSN.Checked == false)
            {
                strErrorMsg += "請選擇查詢條件！\n";
                Err = true;
            }

            if (Err)
            {
                this.lblErr.Text = strErrorMsg;
            }
            else
            {

                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    this.lblErr.Text = strErrorMsg;
                }
            }

            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\OQCLabelScan.rdlc";


            ReportDataSource rsd = new ReportDataSource("OQCLabelScan", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
  }
}
