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
    public partial class CFA : System.Web.UI.Page
    {
        string pn = "";
        string pc = "";
        string type = "";
        string sdt = "";
        string edt = "";
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.Attributes.Add("style", "word-break:keep-all;word-wrap:normal");


            pn = Request.QueryString["prodid"].ToString();
            pc = Request.QueryString["Process"].ToString();
            type = Request.QueryString["Type"].ToString();
            sdt = Request.QueryString["sdt"].ToString();
            edt = Request.QueryString["edt"].ToString();

            StringBuilder stb = new StringBuilder();
            

            if (type == "OK")
            {
                stb.Append("SELECT A.WONBR,A.PROCESSID,A.PRODID,cast(A.INPUTQTY as float) as INPUTQTY,CAST(A.OUTPUTQTY AS FLOAT) AS OUTPUTQTY  FROM dbo.SH_PRODID_YIELD A JOIN rep.MPIM B ON A.PRODID=B.PRODID ");
                stb.Append(" WHERE B.PRODID='" + pn + "' AND A.PROCESSID='" + pc + "' AND A.ReportDate>='" + sdt + "' AND A.ReportDate<='" + edt + "'ORDER BY WONBR");
                DataTable dt1 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];

                GridView1.DataSource = dt1;
                GridView1.DataBind();

            }
            else
            {
                stb.Append("SELECT A.WONBR,A.PROCESSID,A.PRODID,");
                DataTable dt2 = GetDF(pn,pc,sdt,edt);
                if (dt2 != null)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        string DFcode = dt2.Rows[j][0].ToString().Trim();
                        stb.Append("SUM(cast(CASE WHEN A.DEFECTCD='" + DFcode + "'THEN (A.DFQTY) ELSE NULL END as float)) AS '" + DFcode + "',");
                    }
                }
                #region
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB02'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB02,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB03'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB03,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB04'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB04,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB06'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB06,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB08'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB08,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB09'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB09,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB11'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB11,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB12'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB12,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB14'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB14,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB16'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB16,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB19'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB19,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CB20'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CB20,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CBC8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CBC8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CBDI'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CBDI,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CBDM'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CBDM,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CBDN'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CBDN,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CBDO'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CBDO,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CBK1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CBK1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CC00'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CC00,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CC07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CC07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CC11'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CC11,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CC20'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CC20,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CCA1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CCA1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CCC4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CCC4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CCC5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CCC5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CCCA'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CCCA,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CCFF'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CCFF,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CCR3'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CCR3,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CD05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CD05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CD09'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CD09,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CD21'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CD21,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDA1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDA1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDAC'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDAC,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDB5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDB5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDB6'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDB6,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDB7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDB7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDBB'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDBB,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDBC'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDBC,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDBH'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDBH,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDC8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDC8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDC9'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDC9,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDD4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDD4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDD7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDD7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDK1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDK1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDK2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDK2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CDZ1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CDZ1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CE04'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CE04,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CE07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CE07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CE26'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CE26,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CEC9'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CEC9,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CECA'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CECA,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CEZ1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CEZ1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CEZ5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CEZ5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CEZ7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CEZ7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CEZ8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CEZ8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CEZ9'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CEZ9,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CH04'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CH04,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CH05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CH05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CH09'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CH09,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CH19'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CH19,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CH23'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CH23,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CHC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CHC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CI02'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CI02,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CIC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CIC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CJ00'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CJ00,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CJ09'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CJ09,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK12'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK12,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK13'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK13,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK15'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK15,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK16'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK16,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK17'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK17,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CK18'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CK18,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ3'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ3,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ6'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ6,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZ9'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZ9,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZA'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZA,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CKZB'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CKZB,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLCB'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLCB,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLCC'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLCC,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLCD'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLCD,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLCE'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLCE,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLCF'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLCF,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLH1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLH1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLH2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLH2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLH4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLH4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLH6'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLH6,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLK1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLK1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLK4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLK4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLV1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLV1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLV2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLV2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLV4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLV4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLV6'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLV6,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLV8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLV8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLX1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLX1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CLZ1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CLZ1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM00'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM00,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM06'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM06,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM16'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM16,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM17'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM17,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM18'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM18,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM19'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM19,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM22'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM22,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CM55'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CM55,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CMAJ'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CMAJ,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CMBQ'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CMBQ,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CMC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CMC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CNC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CNC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CNC2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CNC2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CNC5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CNC5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CNC6'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CNC6,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CO04'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CO04,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CO05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CO05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CO07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CO07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CO12'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CO12,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CP00'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CP00,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CP05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CP05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CP06'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CP06,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CP23'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CP23,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPB1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPB1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPB2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPB2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPCN'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPCN,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPCO'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPCO,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPO1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPO1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPO2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPO2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPP1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPP1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPP2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPP2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPS1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPS1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CPS2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CPS2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CQ16'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CQ16,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CS01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CS01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CS02'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CS02,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CS03'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CS03,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CS05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CS05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CTC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CTC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CU45'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CU45,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CU50'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CU50,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV12'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV12,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV73'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV73,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV76'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV76,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV91'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV91,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CV94'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CV94,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CVC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CVC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CW01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CW01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CW02'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CW02,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CW03'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CW03,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CW07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CW07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CW08'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CW08,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CW11'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CW11,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CWC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CWC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CWC3'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CWC3,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CX01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CX01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CX02'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CX02,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CX03'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CX03,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CX04'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CX04,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CX10'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CX10,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CXC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CXC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CXC2'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CXC2,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CXC3'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CXC3,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CXC4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CXC4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ00'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ00,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ01'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ01,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ02'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ02,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ03'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ03,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ05'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ05,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ07'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ07,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ09'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ09,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ14'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ14,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ43'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ43,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ44'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ44,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZ46'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZ46,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZC1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZC1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZC5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZC5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZC7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZC7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZCL'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZCL,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZCV'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZCV,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZD8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZD8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZK1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZK1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZK3'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZK3,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZK4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZK4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZK5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZK5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZK7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZK7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZKB'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZKB,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ1'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ1,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ4'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ4,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ5'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ5,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ6'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ6,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ7'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ7,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ8'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ8,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZ9'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZ9,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZA'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZA,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='CZZB'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS CZZB,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='F204'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS F204,");
                //stb.Append("SUM(CAST(CASE WHEN A.DEFECTCD='F206'THEN (A.DFQTY) ELSE NULL END AS FLOAT)) AS F206 ");
                #endregion
                stb.Append("'' FROM dbo.SH_DEFECT_YIELD A JOIN rep.MPIM B ON A.PRODID=B.PRODID ");
                stb.Append("WHERE B.PRODID='" + pn + "' AND A.PROCESSID='" + pc + "'AND A.ReportDate>='" + sdt + "' AND A.ReportDate<='" + edt + "'  GROUP BY A.WONBR,A.PROCESSID,A.PRODID");
                DataTable dt1 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                GridView2.DataSource = dt1;
                GridView2.DataBind();
            }

        }
        public DataTable GetDF(string pn,string pc,string sdt,string edt)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT DISTINCT A.DEFECTCD FROM dbo.SH_DEFECT_YIELD A JOIN rep.MPIM B ON A.PRODID=B.PRODID WHERE B.PRODID='" + pn + "' AND A.PROCESSID='" + pc + "'AND A.ReportDate>='" + sdt + "' AND A.ReportDate<='" + edt + "'";
            dt = dbConn.GetDataSetBySql(sql).Tables[0];
            if (dt != null)
            {
                return dt;
            }
            return null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
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

            if (type == "OK")
            {
                GridView1.RenderControl(hw);

            }
            else
            {
                GridView2.RenderControl(hw);
            
            }

            Response.Write(tw.ToString());


            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        } 
    }
}
