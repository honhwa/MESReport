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
using System.Text.RegularExpressions;


namespace WebReport.Reports
{
    public partial class IbeWipQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        string tablename = string.Empty;
        UserID aaa = new UserID();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                
                //if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                //{
                //    Response.Redirect("../Login.aspx");
                //}
            }
            if (raRecent.Checked)
            {
                tablename = "EIF.WLTM A WITH(NOLOCK)";
            }
            else if (raHistory.Checked)
            {
                tablename = "EIF.WLTMH A WITH(NOLOCK)";
            }
            else if (raCurretWIP.Checked)
            {
                tablename = "(SELECT CONVERT(CHAR(10),GETDATE(),120) MFGDATE, * FROM New_WLTM WITH(NOLOCK) where fabid = 'B' AND Hideflg='0') A";
            }
        }

        protected int DateValidate()
        {
            //string date = txtDate.Text.Trim();
            //StringBuilder stbdate = new StringBuilder();
            //stbdate.Append("SELECT MFGDATE FROM " + tablename);
            //stbdate.Append(" WHERE MFGDATE='"+date+"'");

            if (raRecent.Checked)
                tablename = "EIF.WLTM A WITH(NOLOCK)";
            else if (raHistory.Checked)
                tablename = "EIF.WLTMH A WITH(NOLOCK)";
            else if (raCurretWIP.Checked)
                tablename = "(SELECT CONVERT(CHAR(10),GETDATE(),120) MFGDATE, * FROM New_WLTM WITH(NOLOCK) where fabid = 'B' AND Hideflg='0') A";

            string date = txtDate.Text.Trim();
            StringBuilder stbdate = new StringBuilder();
            stbdate.Append("SELECT MFGDATE FROM " + tablename + " ");
            if (raRecent.Checked || raHistory.Checked)
                stbdate.Append(" WHERE MFGDATE='" + date + "'");

            DataTable dt = this.dbConn.GetDataSetBySql(stbdate.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        protected void Seac_but_Click(object sender, EventArgs e)
        {
            if (DateValidate() == 1)
            {
                this.Response.Write("<script language=javascript>window.alert('所選擇的日期不在查詢條件之內,請重新選擇查詢條件!');</script>");
                ReportViewer1.Visible = false;
                return;
            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            
            DataTable dtresult = this.dbConn.GetDataSetBySql(this.buildSql()).Tables[0];

            if (DDlstShowStyle.SelectedValue == "0")                  //Runy Add
            {
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\IBEWIPQuery.rdlc";
                ReportDataSource rsd = new ReportDataSource("IbeWipQuery", dtresult);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
                //ReportViewer1.ShowExportControls = false;
                ReportViewer1.Visible = true;
                ReportViewer2.Visible = false;
                
            }
            //Runy Add Start
            else
            {
                this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer2.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\IBEWipQueryForDetail.rdlc";
                ReportDataSource rsd = new ReportDataSource("IBEWipQueryForDetail", dtresult);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
                ReportViewer1.Visible = false;
                ReportViewer2.Visible = true;
            }
            //Runy Add End            
        }

        private void Page_Load()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected void raRecent_CheckedChanged(object sender, EventArgs e)
        {
            if (raRecent.Checked)
            {
                raHistory.Checked = false;
                raCurretWIP.Checked = false;
            }
            //else
            //{
            //    raHistory.Checked = true;
            //}
        }

        protected void raHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (raHistory.Checked)
            {
                raRecent.Checked = false;
                raCurretWIP.Checked = false;
            }
            //else
            //{
            //    raRecent.Checked = true;
            //}
        }

        protected void raCurretWIP_CheckedChanged(object sender, EventArgs e)
        {
            if (raCurretWIP.Checked)
            {
                raRecent.Checked = false;
                raHistory.Checked = false;

                txtDate.Text = "";
            }
        }

        ////20211012 penny : temporarily use, now disabled
        //protected void btnExport_Click(object sender, EventArgs e)
        //{
        //    DataTable dtresult = this.dbConn.GetDataSetBySql(this.buildSql()).Tables[0];            
        //    string filename = "IEB-WIP-Report.xls";
        //    ExcelIO.ExcelHelper.ExportByWeb(dtresult, "", filename);
        //}

        protected string buildSql()
        {            
            StringBuilder stb = new StringBuilder();
            if (DDlstShowStyle.SelectedValue == "0")            //Runy Add
            {
                stb.Append(" SELECT CWORKCTR,N9NCNO AS PRODID,C9NCNO,SUM(ENTITYTQTY)QTY FROM (");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,C9NCNO,SUM(ENTITYTQTY)ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "'GROUP BY CWORKCTR,N9NCNO,C9NCNO  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,COMPONENT AS C9NCNO,SUM(CONVERT(FLOAT,COMPONENTQTY)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK)");
                
                //stb.Append(" SELECT CWORKCTR,N9NCNO,CASE WHEN B.SHOP_CODE = 'BEOL' OR A.WOTYPE IN('C','M','S','K') THEN C.AFT29NCNO ELSE A.SO9NCNO END AS C9NCNO,SUM(ENTITYTQTY)ENTITYTQTY FROM " + tablename + " A WITH(NOLOCK) LEFT JOIN MPIM B ON(A.FABID = B.FABID AND A.PRODID = B.PRODID) 	LEFT JOIN SWOM C ON(A.FABID = C.FABID AND A.WONBR = C.WONBR) WHERE MFGDATE='" + txtDate.Text.Trim() + "'GROUP BY CWORKCTR,N9NCNO,CASE WHEN B.SHOP_CODE = 'BEOL' OR A.WOTYPE IN('C','M','S','K') THEN C.AFT29NCNO ELSE A.SO9NCNO END  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,COMPONENT AS C9NCNO,SUM(CONVERT(FLOAT,COMPONENTQTY)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK)");
                stb.Append(" SELECT CWORKCTR,N9NCNO,CASE WHEN B.SHOP_CODE = 'BEOL' OR A.WOTYPE IN('C','M','S','K') THEN C.AFT29NCNO ELSE A.SO9NCNO END AS C9NCNO,SUM(ENTITYTQTY)ENTITYTQTY");
                stb.Append(" FROM " + tablename + " LEFT JOIN MPIM B ON(A.FABID = B.FABID AND A.PRODID = B.PRODID)");
                stb.Append(" LEFT JOIN New_SWOM C ON(A.FABID = C.FABID AND A.WONBR = C.WONBR)");
                if (raRecent.Checked || raHistory.Checked)
                    stb.Append(" WHERE MFGDATE='" + txtDate.Text.Trim() + "'");
                stb.Append(" GROUP BY CWORKCTR,N9NCNO,CASE WHEN B.SHOP_CODE = 'BEOL' OR A.WOTYPE IN('C','M','S','K') THEN C.AFT29NCNO ELSE A.SO9NCNO END  UNION");
                stb.Append(" SELECT CWORKCTR,N9NCNO,COMPONENT AS C9NCNO,SUM(CONVERT(FLOAT,COMPONENTQTY)) AS ENTITYTQTY FROM " + tablename + "");

                stb.Append(" UNPIVOT (COMPONENT FOR COMPONENTTYPE IN(PARTID26,PARTIDAF,PARTIDBC,PARTIDBF,PARTIDBL,PARTIDCO,PARTIDDF,");
                stb.Append("PARTIDDT,PARTIDFP,PARTIDHS,PARTIDIC,PARTIDLC,PARTIDLD,PARTIDLE,PARTIDLF,PARTIDLG,PARTIDLP,PARTIDLT,");
                stb.Append("PARTIDM1,PARTIDM2,PARTIDM3,PARTIDM4,PARTIDM5,PARTIDM6,PARTIDM7,PARTIDM8,PARTIDMC,PARTIDPB,PARTIDPL,");
                stb.Append("PARTIDPS,PARTIDR1,PARTIDR2,PARTIDR3,PARTIDR4,PARTIDRB,PARTIDRF,PARTIDSI,PARTIDSP,PARTIDST,PARTIDTC,");

                //stb.Append("PARTIDTP,PARTIDUC,PARTIDUD,PARTIDUP,PARTIDUT,PARTIDUV,PARTIDWB,PARTIDWC,PARTIDYJ))AS PARTID");
                stb.Append("PARTIDTP,PARTIDUC,PARTIDUD,PARTIDUP,PARTIDUT,PARTIDUV,PARTIDWB,PARTIDWC,PARTIDYJ,");
                stb.Append("PARTIDAA,PARTIDBB,PARTIDCC,PARTIDDD,PARTIDEE,PARTIDFF,PARTIDGG,PARTIDHH,PARTIDII,PARTIDJJ,PARTIDKK,PARTIDLL,PARTIDMM,PARTIDNN,PARTIDOO,PARTIDPP,PARTIDQQ");
                stb.Append("))AS PARTID");

                stb.Append(" UNPIVOT (COMPONENTQTY FOR PARTQTYNAME IN(PARTQTY26,PARTQTYAF,PARTQTYBC,PARTQTYBF,PARTQTYBL,PARTQTYCO,PARTQTYDF,");
                stb.Append("PARTQTYDT,PARTQTYFP,PARTQTYHS,PARTQTYIC,PARTQTYLC,PARTQTYLD,PARTQTYLE,PARTQTYLF,PARTQTYLG,PARTQTYLP,PARTQTYLT,");
                stb.Append("PARTQTYM1,PARTQTYM2,PARTQTYM3,PARTQTYM4,PARTQTYM5,PARTQTYM6,PARTQTYM7,PARTQTYM8,PARTQTYMC,PARTQTYPB,PARTQTYPL,");
                stb.Append("PARTQTYPS,PARTQTYR1,PARTQTYR2,PARTQTYR3,PARTQTYR4,PARTQTYRB,PARTQTYRF,PARTQTYSI,PARTQTYSP,PARTQTYST,PARTQTYTC,");

                //stb.Append("PARTQTYTP,PARTQTYUC,PARTQTYUD,PARTQTYUP,PARTQTYUT,PARTQTYUV,PARTQTYWB,PARTQTYWC,PARTQTYYJ)) AS PARTQTY");
                stb.Append("PARTQTYTP,PARTQTYUC,PARTQTYUD,PARTQTYUP,PARTQTYUT,PARTQTYUV,PARTQTYWB,PARTQTYWC,PARTQTYYJ,");
                stb.Append("PARTQTYAA,PARTQTYBB,PARTQTYCC,PARTQTYDD,PARTQTYEE,PARTQTYFF,PARTQTYGG,PARTQTYHH,PARTQTYII,PARTQTYJJ,PARTQTYKK,PARTQTYLL,PARTQTYMM,PARTQTYNN,PARTQTYOO,PARTQTYPP,PARTQTYQQ");
                stb.Append(")) AS PARTQTY");

                //stb.Append(" WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND COMPONENTQTY<>0 AND COMPONENT<>'' AND RIGHT(COMPONENTTYPE,2)=RIGHT(PARTQTYNAME,2)");
                stb.Append(" WHERE ");
                if (raRecent.Checked || raHistory.Checked)
                    stb.Append(" MFGDATE='" + txtDate.Text.Trim() + "' AND ");
                stb.Append(" COMPONENTQTY<>0 AND COMPONENT<>'' AND RIGHT(COMPONENTTYPE,2)=RIGHT(PARTQTYNAME,2)");

                stb.Append(" GROUP BY CWORKCTR,N9NCNO,COMPONENT )A");

                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTID26 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTY26)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTY26!=0 GROUP BY CWORKCTR,N9NCNO,PARTID26  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDAF AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYAF)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYAF!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDAF  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDBC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYBC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYBC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDBC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDBF AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYBF)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYBF!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDBF  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDBL AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYBL)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYBL!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDBL  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDCO AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYCO)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYCO!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDCO  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDDF AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYDF)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYDF!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDDF  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDDT AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYDT)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYDT!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDDT  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDFP AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYFP)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYFP!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDFP  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDHS AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYHS)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYHS!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDHS  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDIC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYIC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYIC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDIC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLD AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLD)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLD!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLD  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLE AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLE)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLE!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLE  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLF AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLF)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLF!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLF  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLG AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLG)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLG!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLG  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLP AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLP)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLP!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLP  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDLT AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYLT)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYLT!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDLT  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM1 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM1)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM1!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM1  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM2 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM2)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM2!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM2  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM3 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM3)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM3!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM3  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM4 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM4)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM4!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM4  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM5 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM5)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM5!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM5  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM6 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM6)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM6!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM6  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM7 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM7)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM7!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM7  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDM8 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYM8)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYM8!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDM8  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDMC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYMC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYMC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDMC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDPB AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYPB)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYPB!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDPB  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDPL AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYPL)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYPL!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDPL  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDPS AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYPS)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYPS!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDPS  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDR1 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYR1)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYR1!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDR1  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDR2 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYR2)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYR2!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDR2  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDR3 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYR3)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYR3!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDR3  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDR4 AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYR4)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYR4!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDR4  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDRB AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYRB)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYRB!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDRB  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDRF AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYRF)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYRF!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDRF  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDSI AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYSI)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYSI!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDSI  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDSP AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYSP)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYSP!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDSP  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDST AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYST)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYST!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDST  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDTC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYTC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYTC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDTC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDTP AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYTP)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYTP!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDTP  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDUC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYUC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYUC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDUC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDUD AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYUD)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYUD!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDUD  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDUP AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYUP)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYUP!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDUP  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDUT AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYUT)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYUT!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDUT  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDUV AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYUV)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYUV!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDUV  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDWB AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYWB)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYWB!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDWB  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDWC AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYWC)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYWC!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDWC  UNION");
                //stb.Append(" SELECT CWORKCTR,N9NCNO,PARTIDYJ AS C9NCNO,SUM(CONVERT(FLOAT,PARTQTYYJ)) AS ENTITYTQTY FROM " + tablename + " WITH(NOLOCK) WHERE MFGDATE='" + txtDate.Text.Trim() + "' AND PARTQTYYJ!=0 GROUP BY CWORKCTR,N9NCNO,PARTIDYJ )A");
                stb.Append(" GROUP BY CWORKCTR,C9NCNO,N9NCNO");
                stb.Append(" ORDER BY CWORKCTR,N9NCNO,C9NCNO");

            }
            //Runy Add Start
            else
            {
                stb.Append(" SELECT CSHOPCD,PRODID,N9NCNO,CPRODID,C9NCNO,PLOTGRADE,CWORKCTR +'        '+ B.WORKNAME AS CWORKCTR ,WONBR,ENTITYID,ENTITYTQTY");

                //stb.Append(" FROM " + tablename + " A,MWCM B WITH(NOLOCK) WHERE A.MFGDATE='" + txtDate.Text.Trim() + "' and A.cworkctr=B.workctr");
                stb.Append(" FROM " + tablename + ",MWCM B WITH(NOLOCK) ");
                stb.Append(" WHERE ");
                if (raRecent.Checked || raHistory.Checked)
                    stb.Append(" A.MFGDATE='" + txtDate.Text.Trim() + "' and");
                stb.Append("  A.cworkctr=B.workctr");

                stb.Append(" GROUP BY CSHOPCD,prodid,N9ncno,cprodid,c9ncno,plotgrade,wonbr,CWORKCTR,entityid,B.workname,entitytqty");
                stb.Append(" ORDER BY PRODID,CPRODID,CWORKCTR");
            }
            return stb.ToString();
        }

    }
}
