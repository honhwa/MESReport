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
    public partial class Product_Summary : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.txtStartDT.Text = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                //this.TxtEndDT.Text = DateTime.Now.ToString("yyyy-MM-dd");
                SetDropSi();
            }
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
            StringBuilder stb = new StringBuilder();
            stb.Append("SELECT PRODGROUP,PROJECT_NAME,PRODID,CAST (COUTPUTQTY AS FLOAT) AS COUTPUTQTY,CAST (CSCRAPQTY AS FLOAT)AS CSCRAPQTY,CAST(CASE WHEN CINPUTQTY != 0 THEN ROUND(COUTPUTQTY/CINPUTQTY,4)*100 ELSE 1 END AS FLOAT) AS CYIELD,CAST (FOUTPUTQTY AS FLOAT)AS  FOUTPUTQTY,CAST (FSCRAPQTY AS FLOAT)AS FSCRAPQTY,CAST(CASE WHEN FINPUTQTY != 0 THEN ROUND(FOUTPUTQTY/FINPUTQTY,4)*100 ELSE 1 END AS FLOAT) AS FYIELD,CAST (AOUTPUTQTY AS FLOAT)AS AOUTPUTQTY,CAST (ASCRAPQTY AS FLOAT)AS ASCRAPQTY,CAST(CASE WHEN AINPUTQTY != 0 THEN ROUND(AOUTPUTQTY/AINPUTQTY,4)*100 ELSE 1 END AS FLOAT) AS AYIELD,CAST(CASE WHEN CINPUTQTY != 0 THEN ROUND(COUTPUTQTY/CINPUTQTY,4) ELSE 1 END *CASE WHEN FINPUTQTY != 0 THEN ROUND(FOUTPUTQTY/FINPUTQTY,4) ELSE 1 END *CASE WHEN AINPUTQTY != 0 THEN ROUND(AOUTPUTQTY/AINPUTQTY,4) ELSE 1 END *100 AS FLOAT)AS TYIELD FROM(SELECT B.PRODGROUP,B.PROJECT_NAME,B.PRODID,SUM(CASE WHEN PROCESSID = 'CELL2' THEN (INPUTQTY) ELSE 0 END) AS CINPUTQTY,SUM(CASE WHEN PROCESSID = 'CELL2' THEN (OUTPUTQTY) ELSE 0 END) AS COUTPUTQTY,SUM(CASE WHEN PROCESSID = 'CELL2' THEN (INPUTQTY)-(OUTPUTQTY) ELSE 0 END) AS CSCRAPQTY,SUM(CASE WHEN PROCESSID = 'M2' THEN (INPUTQTY) ELSE 0 END) AS FINPUTQTY,SUM(CASE WHEN PROCESSID = 'M2' THEN (OUTPUTQTY) ELSE 0 END) AS FOUTPUTQTY,SUM(CASE WHEN PROCESSID = 'M2' THEN (INPUTQTY)-(OUTPUTQTY) ELSE 0 END) AS FSCRAPQTY,SUM(CASE WHEN PROCESSID = 'M3' THEN (INPUTQTY) ELSE 0 END) AS AINPUTQTY,SUM(CASE WHEN PROCESSID = 'M3' THEN (OUTPUTQTY) ELSE 0 END) AS AOUTPUTQTY,SUM(CASE WHEN PROCESSID = 'M3' THEN (INPUTQTY)-(OUTPUTQTY) ELSE 0 END) AS ASCRAPQTY FROM SH_PRODID_YIELD A,rep.MPIM B WHERE A.FABID = B.FABID AND A.PRODID = B.PRODID AND B.SIZEID='" + DropSi.Text + "' AND REPORTDATE >= '" + txtStartDT.Text + "' AND REPORTDATE <= '" + TxtEndDT.Text + "'GROUP BY B.PRODGROUP,B.PROJECT_NAME,B.PRODID)TS order by PROJECT_NAME");
            DataTable dt1 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            GridViewtest.DataSource = dt1;
            GridViewtest.DataBind();
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            this.txtStartDT.Text = "";
            this.TxtEndDT.Text = "";

        }
        private void SetDropSi()
        {

            string stb = @"SELECT sizeid FROM msim order BY sizeid";
            DataTable dt = this.dbConn.GetDataSetBySql(stb).Tables[0];
            this.DropSi.DataSource = dt;
            this.DropSi.DataTextField = "sizeid";
            this.DropSi.DataValueField = "sizeid";
            this.DropSi.DataBind();
        }
        protected void GridViewtest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ProdName = e.Row.Cells[2].Text.Trim();
                e.Row.Cells[3].Attributes.Add("ondblclick", "window.open('CFA.aspx?Prodid=" + ProdName + "&sdt=" + txtStartDT.Text + "&edt=" + TxtEndDT.Text + "&Process=Cell2&Type=OK')");
                e.Row.Cells[3].Font.Underline = true;

                e.Row.Cells[4].Attributes.Add("ondblclick", "window.open('CFA.aspx?Prodid=" + ProdName + "&sdt=" + txtStartDT.Text + "&edt=" + TxtEndDT.Text + "&Process=Cell2&Type=S')");
                e.Row.Cells[4].Font.Underline = true;

                e.Row.Cells[6].Attributes.Add("ondblclick", "window.open('CFA.aspx?Prodid=" + ProdName + "&sdt=" + txtStartDT.Text + "&edt=" + TxtEndDT.Text + "&Process=M2&Type=OK')");
                e.Row.Cells[6].Font.Underline = true;

                e.Row.Cells[7].Attributes.Add("ondblclick", "window.open('CFA.aspx?Prodid=" + ProdName + "&sdt=" + txtStartDT.Text + "&edt=" + TxtEndDT.Text + "&Process=M2&Type=S')");
                e.Row.Cells[7].Font.Underline = true;

                e.Row.Cells[9].Attributes.Add("ondblclick", "window.open('CFA.aspx?Prodid=" + ProdName + "&sdt=" + txtStartDT.Text + "&edt=" + TxtEndDT.Text + "&Process=M3&Type=OK')");
                e.Row.Cells[9].Font.Underline = true;

                e.Row.Cells[10].Attributes.Add("ondblclick", "window.open('CFA.aspx?Prodid=" + ProdName + "&sdt=" + txtStartDT.Text + "&edt=" + TxtEndDT.Text + "&Process=M3&Type=S')");
                e.Row.Cells[10].Font.Underline = true;
            }
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


            GridViewtest.RenderControl(hw);


            Response.Write(tw.ToString());


            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Export("application/ms-excel", "报表.xls");
        } 
    }
}
