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
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Text;
using System.Data.OracleClient;


namespace WebReport.Reports
{
    public partial class tptReport : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        OracleConnection Oracon;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
                this.txtrate.Text = "50";
                this.DdlFab.Text = "LCM1";
                this.txtStartDT.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.TxtEndDT.Text = DateTime.Now.ToString("yyyy-MM-dd");
                SetGroup(this.DdlFab.Text.Trim());
            }
            //Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            //string strConnectionString = cfg.AppSettings.Settings["Commhis"].Value;
            //Oracon = new OracleConnection(strConnectionString);

        }
        private void SetGroup(string sFab)
        {
            //string sqlrtptu = "SELECT GroupName FROM RTPTU ";
            string sqlrtptu = "select DISTINCT REF2 from New_SWOM order by REF2 ";
            DataSet dsRtptu = new DataSet();

            //if (sFab == "LCM2")
            //{
                dsRtptu = dbConn.GetDataSetBySql(sqlrtptu, "rtptu");
            //}
            //else
            //{
            //    OracleDataAdapter orada = new OracleDataAdapter(sqlrtptu, Oracon);
            //    orada.Fill(dsRtptu);

            //}
            ddldata.Items.Clear();
            ddldata.Items.Add("ALL");
            for (int i = 0; i < dsRtptu.Tables[0].Rows.Count; i++)
            {
                ddldata.Items.Add(dsRtptu.Tables[0].Rows[i]["REF2"].ToString());
            }
           // ddldata.Items.Add("ALL");
        }
       
        /*ModiDate:20090804  Editor:Tommy.hu
         * 屏蔽Ddlfab中原Lcm1,將Lcm2改為Lcm1
         */
        protected void Seac_but_Click(object sender, EventArgs e)
        {
            if (DateIsOk() == false)
            {
                return;
            }
            string sqlstr = GetSql(ddldata.Text.Trim());
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataTable dt = new DataTable();
            if (DdlFab.Text.Trim() == "LCM1")
            {
                dt = this.dbConn.GetDataSetBySql(sqlstr).Tables[0];
            }
           /* 
            else
            {
                OracleDataAdapter Orada = new OracleDataAdapter(sqlstr, Oracon);
                DataSet DS = new DataSet();
                Orada.Fill(DS);
                dt = DS.Tables[0];
            }
            */
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\TPTReport.rdlc";


            ReportDataSource rsd = new ReportDataSource("TPTReport", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);

        }
        private Boolean DateIsOk()
        {
            if (this.txtStartDT.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇開始日期！');</script>");
                return false;
            }

            if (this.TxtEndDT.Text == String.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇結束日期！');</script>");
                return false;
            }
            if (this.txtrate.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請輸入比例！');</script>");
                return false;
            }
            if (Convert.ToInt16(this.txtrate.Text) > 100)
            {
                this.Response.Write("<script language=javascript>window.alert('輸入比例不能大於100！');</script>");
                return false;
            }
            return true;
        }
        private string GetSql(string sGroupName)
        {
            StringBuilder sb = new StringBuilder();
            //if (DdlFab.Text.Trim() == "LCM2")
            if (DdlFab.Text.Trim() == "LCM1")
            {
                sb.Append(" SELECT ");
                sb.Append(" A.PLANT, ");
                sb.Append(" CASE WHEN A.SPFLG='SPLT' THEN 'YES' ELSE 'NO' END AS SPLTFLG, ");
                sb.Append(" C.REF2 AS BU, ");
                sb.Append(" A.WONBR, ");
                sb.Append(" A.PRODID, ");
                sb.Append(" A.LOTTYPE, ");
                sb.Append(" A.LOTID, ");
                sb.Append(" A.STARTDATE, ");
                sb.Append(" A.ENDDATE, ");
                sb.Append(" A.TPT, ");
                sb.Append(" A.SPFLG AS STATUS, ");
                sb.Append(" A.SHOPCD, ");
                sb.Append(" ISNULL(B.PRODID,'') AS BFPRODID, ");
                sb.Append(" ISNULL(B.WONBR,'') AS BFWONBR, ");
                sb.Append(" ISNULL(B.LOTID,'') AS BFLOTID, ");
                sb.Append(" ISNULL(B.LOTTYPE,'') AS BFLOTTYPE, ");
                sb.Append(" ISNULL(B.STARTDATE,'') AS BFSTARTDATE, ");
                sb.Append(" ISNULL(B.ENDDATE,'') AS BFENDDATE, ");
                sb.Append(" ISNULL(B.TPT,0) AS BFTPT, ");
                sb.Append(" ISNULL(B.SPFLG,'') AS BFSTATUS,");
                sb.Append(" ISNULL(B.SHOPCD,'') AS BFSHOPCD, ");
                sb.Append(" A.TPT+ISNULL(B.TPT,0) AS TotalTPT ");
                sb.Append(" FROM RTPT A ");
                sb.Append(" LEFT JOIN RTPT B ON(B.RDATE<='" + this.TxtEndDT.Text + "' AND A.SHOPCD != B.SHOPCD AND A.MLOTID=B.LOTID AND B.SHOPCD = 'CELL2') ");
                sb.Append(" LEFT JOIN New_SWOM C ON(A.WONBR=C.WONBR) ");
            }
            sb.Append("WHERE A.RDATE BETWEEN '" + this.txtStartDT.Text + "' AND '" + this.TxtEndDT.Text + "' AND (A.TPT != '0' OR ISNULL(B.TPT,0) != '0') ");
            if (sGroupName == "ALL")
            {
                sb.Append("ORDER BY A.SHOPCD,C.REF2,A.PRODID,A.WONBR,A.LOTID ");
            }
            else
            {
                sb.Append("AND C.REF2 = '" + sGroupName + "' ORDER BY A.SHOPCD,C.REF2,A.PRODID,A.WONBR,A.LOTID ");
            }
            return sb.ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (DateIsOk() == false)
            {
                return;
            }
            string sqlRate = " SELECT '' AS  Rate,'' AS GroupName,0.00 AS TPT,0 AS COUNT FROM RTPTU where GroupName='' ";
            //string sqlrtptu = "SELECT GroupName FROM RTPTU ";
            string sqlrtptu = "SELECT DISTINCT REF2 FROM New_SWOM ";
            DataSet dsRate = new DataSet();
            DataSet myds = new DataSet();

            myds = dbConn.GetDataSetBySql(sqlrtptu, "rtptu");
//            if (this.DdlFab.Text.Trim() == "LCM2")
            if (this.DdlFab.Text.Trim() == "LCM1")  
            {
                dsRate = dbConn.GetDataSetBySql(sqlRate, "Rate");
            }
            else
            {
                sqlRate = " SELECT '' AS  Rate,'' AS GroupName,0.00 AS TPT,0 AS COUNT FROM MFABM WHERE FABID= '' ";
                OracleDataAdapter orada = new OracleDataAdapter(sqlRate, Oracon);
                orada.Fill(dsRate, "Rate");
            }
            for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
            {
                DataRow Dr = dsRate.Tables[0].NewRow();
                DataTable mydt = new DataTable();
                string sGroupName = myds.Tables[0].Rows[i]["REF2"].ToString();
                string mysql = GetSql(sGroupName);
               // if (this.DdlFab.Text.Trim() == "LCM2")
                if (this.DdlFab.Text.Trim() == "LCM1")
                {
                    mydt = dbConn.GetDataSetBySql(mysql).Tables[0];
                }
                else
                {
                    OracleDataAdapter orada2 = new OracleDataAdapter(mysql, Oracon);
                    orada2.Fill(mydt);
                }
                int iRate = 0;
                int iCount = 0;
                iRate = Convert.ToInt16(this.txtrate.Text.ToString());
                Dr[0] = iRate;
                Dr[1] = myds.Tables[0].Rows[i]["REF2"].ToString();

                if (mydt.Rows.Count == 0)
                {
                    Dr[2] = 0;
                    Dr[3] = 0;
                }
                else
                {

                    iCount = mydt.Rows.Count * iRate / 100;
                    Dr[3] = iCount;
                    Decimal dtpt = 0.00M;
                    for (int j = 0; j < iCount; j++)
                    {
                        dtpt = dtpt + Convert.ToDecimal(mydt.Rows[j]["TOTALTPT"].ToString());
                    }
                    if (iCount == 0)
                    {
                        Dr[2] = 0;
                    }
                    else
                    {
                        Dr[2] = Decimal.Round(dtpt / iCount, 2);
                    }
                }

                dsRate.Tables[0].Rows.Add(Dr);
            }
            this.gvdata.DataSource = dsRate.Tables[0];
            this.gvdata.DataBind();
        }

    }
}
