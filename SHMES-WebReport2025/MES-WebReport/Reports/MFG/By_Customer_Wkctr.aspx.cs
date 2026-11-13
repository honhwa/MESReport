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
using System.Text;
using Report.Library;
using Report.Sources;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;

namespace Report.Reports.MFG
{
    public partial class By_Customer_Wkctr : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SetDropCs();
                SetDropPs();
            }
        }
        private void SetDropCs()
        {
            string stb = @"SELECT DISTINCT A.PRODDESC+'   '+A.PRODID AS PRODIDD,A.PRODID  FROM rep.MPIM A,New_WLDX B WHERE A.PRODID=B.PRODID ORDER BY PRODIDD";
            DataTable dt = this.dbConn.GetDataSetBySql(stb).Tables[0];
            this.DropCs.DataSource = dt;
            this.DropCs.DataTextField = "PRODIDD";
            this.DropCs.DataValueField = "PRODID";
            this.DropCs.DataBind();
            this.DropCs.Items.Insert(0, "ALL");
            //this.DropCs.ClearSelection();
        }
        private void SetDropPs()
        {
            string stb = @"SELECT DISTINCT PROCESSID FROM dbo.SH_DEFECT_YIELD ORDER BY PROCESSID";
            DataTable dt = this.dbConn.GetDataSetBySql(stb).Tables[0];
            this.DropPs.DataSource = dt;
            this.DropPs.DataTextField = "PROCESSID";
            this.DropPs.DataValueField = "PROCESSID";
            this.DropPs.DataBind();
            this.DropPs.Items.Insert(0, "ALL");
            //this.DropPs.ClearSelection();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }

            if (this.txtStartDT.Text.Trim().Replace("'", "") == "")
            {
                this.Response.Write("<script language=javascript>window.alert('開始時間與結束時間不可以為空');</script>");
                return;
            }
            if (this.TxtEndDT.Text.Trim().Replace("'", "") == "")
            {
                this.Response.Write("<script language=javascript>window.alert('開始時間與結束時間不可以為空');</script>");
                return;
            }
            if (Convert.ToDateTime(this.txtStartDT.Text.ToString()) > Convert.ToDateTime(this.TxtEndDT.Text.ToString()))
            {
                this.Response.Write("<script language=javascript>window.alert('開始時間應該小與結束時間');</script>");
                return;
            }
            if (Convert.ToDateTime(this.txtStartDT.Text.ToString()).AddMonths(2) < Convert.ToDateTime(this.TxtEndDT.Text.ToString()))
            {
                this.Response.Write("<script language=javascript>window.alert('請將查詢時間設在2個月內');</script>");
                return;
            }
            BindData();
        }

        private void BindData()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("SELECT 'a',");
            stb.Append(" STUFF((");
            stb.Append(" SELECT ', ' +  CAST(FRENTITYID AS VARCHAR(MAX))");
            stb.Append(" FROM New_WLTX");
            stb.Append(" WHERE ( VERB = 'CRLT' AND TOWONBR=Ra.WONBR)");
            stb.Append(" FOR XML PATH (''))");
            stb.Append(",1,2,'') AS FRENTITYID,ra.*");
            stb.Append(" FROM (");

            stb.Append("SELECT A.PROCESSID,A.WONBR, B.INPUTQTY,B.OUTPUTQTY, ");
            DataTable dt2 = GetDF(txtStartDT.Text, TxtEndDT.Text);
            if (dt2 != null)
            {
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    string DFcode = dt2.Rows[j][0].ToString().Trim();
                    stb.Append("SUM(cast(CASE WHEN A.DEFECTCD='" + DFcode + "'THEN (A.DFQTY) ELSE 0 END as float)) AS '" + DFcode + "',");
                }
            }

            stb.Append("A.PRODID FROM dbo.SH_DEFECT_YIELD A,dbo.SH_PRODID_YIELD B");

            stb.Append(" WHERE  A.PROCESSID=B.PROCESSID AND A.PRODID=B.PRODID  AND A.WONBR=B.WONBR  AND ");

            string Cs = DropCs.Text.ToString();
            string Ps = DropPs.Text.ToString();

            if (Cs == "ALL")
            {
                if (Ps == "ALL")
                {
                    stb.Append("");
                }
                else
                {
                    stb.Append(" A.PROCESSID='" + DropPs.Text + "'AND ");
                }
            }
            else
            {
                stb.Append("  A.PROCESSID='" + DropPs.Text + "'AND ");

                stb.Append(" A.PRODID='" + DropCs.Text + "'AND ");
            }


            stb.Append(" B.ReportDate>='" + txtStartDT.Text + "' AND B.ReportDate<='" + TxtEndDT.Text + "'");

            stb.Append(" GROUP BY A.PROCESSID,A.WONBR, B.INPUTQTY,B.OUTPUTQTY,A.PRODID)Ra ");

            stb.Append("union ");
            stb.Append("SELECT 'b','Total','','',SUM(Ra.INPUTQTY),SUM(Ra.OUTPUTQTY),");
            if (dt2 != null)
            {
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    string DFcode = dt2.Rows[j][0].ToString().Trim();
                    stb.Append("SUM(Ra.\""+DFcode+"\"),");
                }
            }
            stb.Append("'N'");
            stb.Append(" FROM (");
            stb.Append("SELECT A.PROCESSID,A.WONBR, B.INPUTQTY,B.OUTPUTQTY, ");
            if (dt2 != null)
            {
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    string DFcode = dt2.Rows[j][0].ToString().Trim();
                    stb.Append("SUM(cast(CASE WHEN A.DEFECTCD='" + DFcode + "'THEN (A.DFQTY) ELSE 0 END as float)) AS '" + DFcode + "',");
                }
            }

            stb.Append("A.PRODID FROM dbo.SH_DEFECT_YIELD A,dbo.SH_PRODID_YIELD B");

            stb.Append(" WHERE  A.PROCESSID=B.PROCESSID AND A.PRODID=B.PRODID  AND A.WONBR=B.WONBR  AND ");

            if (Cs == "ALL")
            {
                if (Ps == "ALL")
                {
                    stb.Append("");
                }
                else
                {
                    stb.Append(" A.PROCESSID='" + DropPs.Text + "'AND ");
                }
            }
            else
            {
                stb.Append("  A.PROCESSID='" + DropPs.Text + "'AND ");

                stb.Append(" A.PRODID='" + DropCs.Text + "'AND ");
            }


            stb.Append(" B.ReportDate>='" + txtStartDT.Text + "' AND B.ReportDate<='" + TxtEndDT.Text + "'");

            stb.Append(" GROUP BY A.PROCESSID,A.WONBR, B.INPUTQTY,B.OUTPUTQTY,A.PRODID)Ra ");

            #region
            //stb.Append("SELECT C.FRENTITYID,ts.WONBR,CAST(ts.INPUTQTY AS FLOAT) AS INPUTQTY,CAST(ts.OUTPUTQTY AS FLOAT) AS OUTPUTQTY,");
            //stb.Append("cast(CASE WHEN ts.INPUTQTY != 0 THEN ROUND(ts.OUTPUTQTY/ts.INPUTQTY,4)*100 ELSE 1 END AS FLOAT) AS YIELD,");
            //DataTable dt2 = GetDF(txtStartDT.Text, TxtEndDT.Text);
            //if (dt2 != null)
            //{
            //    for (int j = 0; j < dt2.Rows.Count; j++)
            //    {
            //        string DFcode = dt2.Rows[j][0].ToString().Trim();
            //        stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='" + DFcode + "'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS '" + DFcode + "',");
            //    }
            //}
            #region
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB02'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB02,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB03'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB03,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB04'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB04,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB06'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB06,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB08'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB08,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB09'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB09,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB11'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB11,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB12'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB12,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB14'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB14,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB16'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB16,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB19'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB19,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CB20'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CB20,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CBC8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CBC8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CBDI'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CBDI,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CBDM'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CBDM,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CBDN'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CBDN,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CBDO'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CBDO,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CBK1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CBK1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CC00'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CC00,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CC07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CC07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CC11'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CC11,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CC20'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CC20,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CCA1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CCA1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CCC4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CCC4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CCC5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CCC5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CCCA'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CCCA,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CCFF'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CCFF,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CCR3'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CCR3,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CD05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CD05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CD09'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CD09,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CD21'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CD21,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDA1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDA1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDAC'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDAC,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDB5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDB5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDB6'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDB6,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDB7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDB7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDBB'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDBB,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDBC'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDBC,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDBH'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDBH,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDC8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDC8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDC9'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDC9,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDD4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDD4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDD7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDD7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDK1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDK1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDK2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDK2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CDZ1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CDZ1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CE04'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CE04,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CE07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CE07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CE26'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CE26,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CEC9'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CEC9,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CECA'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CECA,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CEZ1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CEZ1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CEZ5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CEZ5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CEZ7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CEZ7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CEZ8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CEZ8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CEZ9'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CEZ9,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CH04'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CH04,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CH05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CH05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CH09'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CH09,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CH19'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CH19,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CH23'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CH23,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CHC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CHC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CI02'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CI02,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CIC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CIC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CJ00'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CJ00,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CJ09'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CJ09,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK12'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK12,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK13'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK13,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK15'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK15,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK16'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK16,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK17'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK17,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CK18'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CK18,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ3'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ3,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ6'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ6,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZ9'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZ9,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZA'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZA,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CKZB'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CKZB,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLCB'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLCB,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLCC'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLCC,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLCD'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLCD,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLCE'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLCE,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLCF'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLCF,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLH1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLH1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLH2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLH2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLH4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLH4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLH6'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLH6,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLK1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLK1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLK4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLK4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLV1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLV1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLV2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLV2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLV4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLV4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLV6'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLV6,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLV8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLV8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLX1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLX1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CLZ1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CLZ1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM00'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM00,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM06'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM06,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM16'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM16,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM17'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM17,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM18'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM18,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM19'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM19,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM22'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM22,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CM55'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CM55,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CMAJ'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CMAJ,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CMBQ'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CMBQ,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CMC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CMC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CNC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CNC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CNC2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CNC2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CNC5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CNC5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CNC6'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CNC6,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CO04'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CO04,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CO05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CO05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CO07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CO07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CO12'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CO12,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CP00'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CP00,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CP05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CP05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CP06'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CP06,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CP23'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CP23,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPB1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPB1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPB2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPB2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPCN'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPCN,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPCO'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPCO,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPO1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPO1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPO2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPO2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPP1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPP1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPP2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPP2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPS1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPS1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CPS2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CPS2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CQ16'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CQ16,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CS01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CS01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CS02'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CS02,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CS03'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CS03,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CS05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CS05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CTC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CTC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CU45'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CU45,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CU50'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CU50,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV12'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV12,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV73'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV73,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV76'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV76,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV91'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV91,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CV94'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CV94,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CVC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CVC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CW01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CW01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CW02'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CW02,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CW03'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CW03,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CW07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CW07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CW08'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CW08,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CW11'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CW11,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CWC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CWC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CWC3'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CWC3,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CX01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CX01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CX02'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CX02,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CX03'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CX03,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CX04'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CX04,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CX10'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CX10,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CXC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CXC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CXC2'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CXC2,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CXC3'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CXC3,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CXC4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CXC4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ00'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ00,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ01'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ01,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ02'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ02,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ03'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ03,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ05'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ05,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ07'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ07,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ09'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ09,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ14'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ14,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ43'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ43,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ44'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ44,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZ46'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZ46,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZC1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZC1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZC5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZC5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZC7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZC7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZCL'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZCL,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZCV'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZCV,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZD8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZD8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZK1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZK1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZK3'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZK3,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZK4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZK4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZK5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZK5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZK7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZK7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZKB'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZKB,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ1'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ1,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ4'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ4,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ5'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ5,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ6'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ6,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ7'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ7,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ8'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ8,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZ9'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZ9,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZA'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZA,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='CZZB'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS CZZB,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='F204'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS F204,");
            ////stb.Append("SUM(cast(CASE WHEN B.DEFECTCD='F206'THEN (B.DEFECTQTY) ELSE NULL END as float)) AS F206 ");
            #endregion
            //stb.Append("'' FROM ");
            //stb.Append("(SELECT A.WONBR,");
            //stb.Append("MAX(CASE WHEN A.processid='CELL2' THEN (A.INPUTQTY) ELSE NULL END)AS INPUTQTY,");
            //stb.Append("MIN(CASE WHEN A.processid='M3' THEN (A.OUTPUTQTY) ELSE NULL END) AS OUTPUTQTY");
            //stb.Append(" FROM SH_PRODID_YIELD A WHERE A.ReportDate>='" + txtStartDT.Text + "' AND A.ReportDate<='" + TxtEndDT.Text + "'GROUP BY A.WONBR) TS");
            //stb.Append(",rep.WLDX B,rep.WLTX C WHERE TS.WONBR=B.WONBR AND C.TOWONBR=B.WONBR AND C.VERB = 'CRLT'");

            //string Cs = DropCs.Text.ToString();
            //string Ps = DropPs.Text.ToString();


            //if (Cs == "ALL")
            //{
            //    if (Ps == "ALL")
            //    {
            //        stb.Append("");
            //    }
            //    else
            //    {
            //        stb.Append("AND B.CWorkCtr='" + DropPs.Text + "'");
            //    }
            //}
            //else
            //{
            //    if (Ps == "ALL")
            //    {
            //        stb.Append("AND B.PRODID='" + DropCs.Text + "'");
            //    }
            //    else
            //    {
            //        stb.Append("AND B.CWorkCtr='" + DropPs.Text + "'AND B.PRODID='" + DropCs.Text + "'");
            //    }
            //}
            //stb.Append("GROUP BY ts.WONBR,ts.INPUTQTY,ts.OUTPUTQTY,C.FRENTITYID");
            #endregion
            DataTable dt1 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];

            GridView1.DataSource = dt1;
            
            GridView1.DataBind();

        }

        private object GridViewRowEventArgs(object e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            TxtEndDT.Text = "";
            txtStartDT.Text = "";

        }
        public DataTable GetDF(string A, string B)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  DISTINCT B.DEFECTCD  FROM (SELECT A.WONBR FROM  SH_PRODID_YIELD A WHERE A.ReportDate>='" + A + "' AND A.ReportDate<='" + B + "'GROUP BY A.WONBR) TS,rep.WLDX B,rep.WLTX C WHERE TS.WONBR=B.WONBR AND C.TOWONBR=B.WONBR AND C.VERB = 'CRLT'";
            dt = dbConn.GetDataSetBySql(sql).Tables[0];
            if (dt != null)
            {
                return dt;
            }
            return null;
        }
        //欄位值一樣就合併
        public static void GroupRows(GridView GridView1, int cellNum)
        {
            int i = 0, rowSpanNum = 1;
            while (i < GridView1.Rows.Count - 1)
            {
                GridViewRow gvr = GridView1.Rows[i];
                for (++i; i < GridView1.Rows.Count; i++)
                {
                    GridViewRow gvrNext = GridView1.Rows[i];
                    if (gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text)
                    {
                        gvrNext.Cells[cellNum].Visible = false;
                        rowSpanNum++;
                    }
                    else
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;
                        rowSpanNum = 1;
                        break;
                    }
                    if (i == GridView1.Rows.Count - 1)
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;

                    }
                }
            }

        }
        //導出EXCEL
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            GridView1.AllowPaging = false;

            BindData();
            
            Export("application/ms-excel", "报表.xls");

        }
        private void Export(string FileType, string FileName)
        {

            Response.Charset = "GB2312";


            Response.ContentEncoding = System.Text.Encoding.UTF7;


            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.UTF8).ToString());


            Response.ContentType = FileType;


            this.EnableViewState = false;


            StringWriter tw = new StringWriter();


            HtmlTextWriter hw = new HtmlTextWriter(tw);


            GridView1.RenderControl(hw);

            

            Response.Write(tw.ToString());


            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridViewRow bottomPagerRow = GridView1.BottomPagerRow;
            Label bottomLabel = new Label();
            bottomLabel.Text = "目前所在分頁：（" + (GridView1.PageIndex + 1) + "/" + GridView1.PageCount + ")";
            if (GridView1.PageCount != 0)
            {
                bottomPagerRow.Cells[0].Controls.Add(bottomLabel);
            }
            
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow ||e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //如果想使第1列不可见,则将它的可见性设为false
            } 

        }
    }
}