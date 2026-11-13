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
    public partial class QueryCarton : System.Web.UI.Page
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

                if (ByLot.Checked)
                {
                    txtSN.Enabled = false;
                    txtSN.Text = "";
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
            string strErrorMsg = "";
            lblErrMsg.Text = "";
            DateTime dtFromTime = new DateTime();
            DateTime dtToTime = new DateTime();
            StringBuilder stb1 = new StringBuilder();
            StringBuilder stb = new StringBuilder();
            StringBuilder stbU = new StringBuilder();
            StringBuilder stbW = new StringBuilder();
            bool Err = false;
            DataTable dt = new DataTable();

            //stb.Append("select *  INTO #TMP_1 FROM (  ");
            //stb.Append("SELECT CARTID,SOLOTID,BANKID,ENTITYID,ENTITYTQTY,PRODID,CWORKCTR,TRANSDATE FROM dbo.WLTM WHERE  FABID='B' AND CARTID<>'NULL' ");
            //stb.Append("UNION SELECT CARTID,SOLOTID,BANKID,ENTITYID,ENTITYTQTY,PRODID,CWORKCTR,TRANSDATE FROM dbo.WLTMH ");
            //stb.AppendLine("WHERE FABID='B' AND CARTID<>'NULL') AA");

            //if (ChkByPiece.Checked)
            if (ByLot.Checked == false)
            {
                txtSN.Enabled = true;
                txtPANELID.Enabled = true;
                txtLOTID.Enabled = false;    //ADD BY V1.1
                ReportViewer2.Visible = true;
                ReportViewer1.Visible = false;
                string strTable = "";
                string strTable2 = "";
                string strTable3 = "";
                string strGroup = "";
                if (BMW.Checked)
                {
                    strTable = "New_FILEPARM";
                    strTable2 = "LOTID";
                    strTable3 = "SN";
                    strGroup = "BMW";
                }
                if (Panasonic.Checked)
                {
                    strTable = "New_TBL_PANSONICSN";
                    strTable2 = "PANELID";
                    strTable3 = "CUSTOMERSN";
                    strGroup = "Panasonic";
                }
                if (Audi.Checked)
                {
                    strTable = "New_TBL_AudiSN";
                    strTable2 = "PANELID";
                    strTable3 = "CUSTOMERSN";
                    strGroup = "AUDI";
                }
                if (Automotive.Checked)
                {
                    strTable = "New_TBL_AUTOMOTIVE";
                    strTable2 = "PANELID";
                    strTable3 = "SN";
                }
                //stb.Append("SELECT CARTID,ENTITYID,SERIALNO INTO #TMP_2 FROM (  ");
                //stb.Append("SELECT CARTID,ENTITYID,SERIALNO FROM DBO.WCMSD WHERE  FABID='B' UNION ");
                //stb.AppendLine("SELECT CARTID,ENTITYID,SERIALNO FROM DBO.WCMSDH WHERE  FABID='B' ) BB");

                stb1.Append("SELECT * FROM ( ");
                
                stb.Append("SELECT DISTINCT A.CARTID,A.SOLOTID,A.BANKID,A.TOENTITYID AS ENTITYID,A.NENTITYTQTY AS ENTITYTQTY,A.TOPRODID AS PRODID,A.CWORKCTR,A.TRANSDATE,");

                if (Audi.Checked)
                    {
                        stb.Append("row_number() OVER (partition by ISNULL(D." + strTable2 + ",A.TOENTITYID) ORDER BY A.TRANSDATE DESC) rn, ");//1.2 ADD
                        stb.Append("D.SERIALNUMBER AS SERIALNO,ISNULL(D." + strTable2 + ",A.TOENTITYID) AS PANELID FROM New_VWLTXA A ");
                    }
                    else if (Automotive.Checked)
                    {
                        stb.Append("row_number() OVER (partition by C." + strTable2 + " ORDER BY A.TRANSDATE DESC) rn,  ");//1.2 ADD
                        stb.Append("C." + strTable3 + " AS SERIALNO,C." + strTable2 + " AS PANELID FROM New_VWLTXA A ");
                    }
                    else
                    {
                        stb.Append("row_number() OVER (partition by C." + strTable2 + " ORDER BY A.TRANSDATE DESC) rn,  ");//1.2 ADD
                        stb.Append("B.SERIALNO,C." + strTable2 + " AS PANELID FROM New_VWLTXA A ");
                    }
                stb.Append("LEFT JOIN New_VWCMSDA B ON b.fabid=a.fabid and A.CARTID = B.CARTID  AND A.TOENTITYID=B.ENTITYID AND B.PRODID=A.TOPRODID ");
                if (Audi.Checked)
                { }
                else if (Automotive.Checked)
                {
                    stb.Append("LEFT JOIN " + strTable + " C ON B.SERIALNO=C." + strTable2);
                }
                else 
                {
                    stb.Append("LEFT JOIN " + strTable + " C ON B.SERIALNO=C." + strTable3);
                }
                if (Audi.Checked)
                {
                    stb.Append(" LEFT JOIN (SELECT PANELID,SERIALNUMBER FROM New_FILEPARM_AUDI UNION SELECT PANELID,CUSTOMERSN FROM New_TBL_AUDISN) D ON  SUBSTRING(SerialNumber,15,2)+SUBSTRING(SerialNumber,7,6)+SUBSTRING(SerialNumber,17,20) =B.SERIALNO OR SERIALNUMBER=B.SERIALNO");
                }
                if (Automotive.Checked)
                {
                    stb.AppendLine(" WHERE A.FABID='B' AND  A.VERB in('FILT','sflt','PKCI')  AND A.TOPRODID IN(SELECT PRODID FROM MPIM WHERE FABID='B' AND PRODGROUP NOT IN('Audi','Panasonic','bmw'))  ");
                }
                else
                {
                    stb.AppendLine(" WHERE A.FABID='B' AND  A.VERB in('FILT','sflt','PKCI')  AND A.TOPRODID IN(SELECT PRODID FROM MPIM WHERE FABID='B' AND PRODGROUP='"+strGroup +"')  ");
                }
                if (txtSN.Text != string.Empty)
                {

                    string strSN=txtSN.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    if (Audi.Checked)
                    {
                        stb.AppendFormat(" AND  D.SERIALNUMBER IN ('{0}')", strSN);

                    }
                    else
                    {
                        stb.AppendFormat(" AND C." + strTable3 + " IN ('{0}')", strSN);
                    }
                }
                if (txtPANELID.Text != string.Empty )
                {

                    string strPANELID = txtPANELID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    if (Audi.Checked)
                    {
                        stbU.Append(stb.ToString());

                        stb.AppendFormat(" AND D." + strTable2 + " IN ('{0}')", strPANELID);
                        stb.Append(" AND (B.SERIALNO= SUBSTRING(SerialNumber,15,2)+SUBSTRING(SerialNumber,7,6)+SUBSTRING(SerialNumber,17,20) OR B.SERIALNO=SERIALNUMBER)");
                        //stb.Append(" union ");
                        //stb.Append(stbU.ToString());
                        //stb.AppendFormat(" AND A.ENTITYID IN ('{0}')", strPANELID);
                    }
                    else
                    {
                        stb.AppendFormat(" AND ISNULL(C." + strTable2 + ",A.TOENTITYID) IN ('{0}')", strPANELID);
                    }
                }
            }
            else
            {
                txtSN.Enabled = false;
                txtSN.Text = "";
                //txtPANELID.Enabled = false;
                //txtPANELID.Text = "";
                txtLOTID.Enabled = true;    //ADD BY V1.1
                ReportViewer1.Visible = true;
                ReportViewer2.Visible = false;
                stb1.Append("SELECT * FROM ( ");//1.2 ADD
                if (txtPANELID.Text  !="" || txtCARTON.Text !="")
                {
                    ReportViewer1.Visible = false ;
                    ReportViewer2.Visible = true ;
                    //stb.Append(" SELECT   B.SERIALNO AS PANELID,A.CWORKCTR,NENTITYTQTY AS ENTITYTQTY,B.CARTID,B.PRODID,A.SOLOTID,A.BANKID,A.TRANSDATE,C.BARCODE AS SERIALNO,B.ENTITYID FROM VWCMSDA B,VWLTXA A ,TBL_AUTOMOTIVE C");
                    stb.Append(" SELECT   B.SERIALNO AS PANELID,A.CWORKCTR,NENTITYTQTY AS ENTITYTQTY,B.CARTID,B.PRODID,A.SOLOTID,A.BANKID,A.TRANSDATE,C.BARCODE AS SERIALNO,row_number() OVER (partition by B.SERIALNO ORDER BY A.TRANSDATE DESC) rn,B.ENTITYID FROM New_VWCMSDA B,New_VWLTXA A ,New_TBL_AUTOMOTIVE C");
                    stb.Append("  WHERE  A.FABID='B' AND B.FABID=A.FABID AND A.TOENTITYID=B.ENTITYID AND A.CARTID=B.CARTID AND A.TOPRODID=B.PRODID AND A.VERB IN('FILT','SFLT','PKCI') ");
                    stb.Append("  AND C.PANELID=B.SERIALNO ");
                    //if (txtSN.Text != "")
                    //{
                    //    stb.AppendFormat(" AND B.SERIALNO IN({0})","'"+txtSN.Text +"'");
                    //}
                    if (txtPANELID.Text != "")
                    {
                        stb.AppendFormat(" AND B.SERIALNO IN({0})", "'" + txtPANELID.Text+"'");
                    }
                    if (txtCARTON.Text != "")
                    {
                        stb.AppendFormat(" AND B.cartid IN({0})", "'" + txtCARTON.Text+"'");
                    }
                }
                else
                {
                    //stb.Append("SELECT TOENTITYID AS ENTITYID,CWORKCTR,NENTITYTQTY AS ENTITYTQTY,CARTID,TOPRODID AS PRODID,SOLOTID,BANKID,TRANSDATE FROM VWLTXA A WHERE  FABID='B' AND VERB IN('FILT','SFLT','PKCI')   ");
                    stb.Append("SELECT TOENTITYID AS ENTITYID,CWORKCTR,NENTITYTQTY AS ENTITYTQTY,CARTID,TOPRODID AS PRODID,SOLOTID,BANKID,TRANSDATE,row_number() OVER (partition by A.TOENTITYID ORDER BY A.TRANSDATE DESC) rn FROM New_VWLTXA A WHERE  FABID='B' AND VERB IN('FILT','SFLT','PKCI')   ");
                    
                }
                
            }

            if ((Chkdate.Checked) && ((txtFromTime.Text == string.Empty) || (txtToTime.Text == string.Empty)))
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
                {
                    stb.Append("  AND A.TRANSDATE>='" + strFromTime + "' AND A.TRANSDATE<='" + strToTime + "'");
                    stbW.Append("  AND A.TRANSDATE>='" + strFromTime + "' AND A.TRANSDATE<='" + strToTime + "'");
                }
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
                    stbW.AppendFormat(" AND A.SOLOTID IN ('{0}')", strLOTID);
                }
            }
            if (this.ChkCARTON.Checked)
            {
                if (txtCARTON.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "請先輸入CARTON再查詢，謝謝！\n";

                    Err = true;
                }
                else
                {
                    string strCARTON
                    = txtCARTON.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                    stb.AppendFormat(" AND A.CARTID IN ('{0}')", strCARTON);
                    stbW.AppendFormat(" AND A.CARTID IN ('{0}')", strCARTON);
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
                    stb.AppendFormat(" AND A.TOPRODID IN ('{0}')", strPRODID);
                    stbW.AppendFormat(" AND A.TOPRODID IN ('{0}')", strPRODID);
                }
            }
            //if (txtPANELID.Text != string.Empty && !ChkPRODID.Checked && !ChkCARTON.Checked && !ChkLOTID.Checked &&!ChkPRODID.Checked  && txtSN.Text == string.Empty)
            if (txtPANELID.Text != string.Empty && !ByLot.Checked)
            {

                string strPANELID = txtPANELID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                if (Audi.Checked)
                {
                    //stbU.Append(stb.ToString());
                   
                    stb.Append(" union ");
                    stb.Append(stbU.ToString());
                    stb.AppendFormat(" AND A.TOENTITYID IN ('{0}')", strPANELID);
                    stb.Append(stbW.ToString());
                }
               
            }
            if (ChkLOTID.Checked == false && ChkPRODID.Checked == false && Chkdate.Checked == false && ChkCARTON.Checked == false && txtSN.Enabled == false && txtPANELID.Enabled == true && txtPANELID.Text=="")
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

                //if ((ChkByPiece.Checked))
                if (ByLot.Checked == false)
                {
                    stb.AppendLine(" ) t where t.rn<=1 ");
                    stb.AppendLine("ORDER BY t.TRANSDATE");
                    //stb.AppendLine("DROP TABLE #TMP_1");
                 
                    //stb.Append("DROP TABLE #TMP_2");
                    
                }
                else
                {
                    stb.AppendLine(" ) t where t.rn<=1 ");
                    stb.AppendLine("ORDER BY t.TRANSDATE");
                    //stb.Append("DROP TABLE #TMP_1");
                    
                }
                String stb2 = stb1.ToString() + stb.ToString();
                dt = this.dbcon.GetDataSetBySql(stb2.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    lblErrMsg.Text = strErrorMsg;
                }
            }



           // if (ChkByPiece.Checked)
            if (ByLot.Checked == false)
            {
                this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer2.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\PieceQueryCarton.rdlc";
                ReportDataSource rsd = new ReportDataSource("PieceQueryCarton", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);

            }
            else
            {
                

                if (txtPANELID.Text != "" || txtCARTON.Text != "")
                {
                    this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    LocalReport localReportRoqcs = this.ReportViewer2.LocalReport;
                    localReportRoqcs.ReportPath = @"RDLC\PieceQueryCarton.rdlc";
                    ReportDataSource rsd = new ReportDataSource("PieceQueryCarton", dt);
                    localReportRoqcs.DataSources.Clear();
                    localReportRoqcs.DataSources.Add(rsd);

                }
                else
                {
                    this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                    localReportRoqcs.ReportPath = @"RDLC\LotQueryCarton.rdlc";
                    ReportDataSource rsd = new ReportDataSource("LotQueryCarton", dt);
                    localReportRoqcs.DataSources.Clear();
                    localReportRoqcs.DataSources.Add(rsd);
                }
               
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

        protected void ChkCARTON_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkCARTON.Checked)
            {
                this.txtCARTON.Enabled = true;
            }
            else
            {
                this.txtCARTON.Enabled = false;
                this.txtCARTON.Text = "";
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
        protected void Refresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
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
            if (ByLot.Checked)
            {
                txtSN.Enabled = false;
                txtSN.Text = "";
            }
            else
            {
                txtSN.Enabled = true;
                txtSN.Text = "";
            }
        }
        /*     END  by V1.1   */
 }
}
