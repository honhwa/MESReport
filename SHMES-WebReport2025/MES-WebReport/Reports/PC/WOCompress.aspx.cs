using Report.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Report.Reports.PC
{
    public partial class WOCompress : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        string str1 = "";
        string str2 = "";
        string str3 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!this.IsPostBack)
                {
                    if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                    {
                        Response.Redirect("../../Login.aspx");
                    }
                }
                //AppClass Apc = new AppClass();
                //string strPcName = "";
                //string strPcIP = "";
                //string strNTName = "";
                //Apc.GetLoginInfo(out strPcName, out strPcIP, out strNTName);
                if (!BoundMonth(ddlMonth))
                {
                    lblNote.Text = "綁定月分異常";
                    return;
                }
                else
                {
                    lblNote.Text = "";
                }
                //Apc.RecordLogInfo("", strNTName, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), strPcName, strNTName, "ExportExcel_Click", "");
            }
        }

        private bool BoundMonth(DropDownList ddlMonth)
        {
            StringBuilder strSql = new StringBuilder();
            DataSet dsResult = new DataSet();
            try
            {
                strSql.Append(" with cte as ( ");
                strSql.Append(" select   cast(getdate() as datetime) as date     ");
                strSql.Append(" union all  ");
                strSql.Append(" select  dateadd(mm,-1,date) as date ");
                strSql.Append(" from cte where date >=dateadd(mm,-3,cast(getdate() as datetime)) ");
                strSql.Append(" ) ");
                strSql.Append(" select  convert(nvarchar(7),date,120) as date from cte order by date desc ");
                dsResult = dbConn.GetDataSetBySql(strSql.ToString());
                ddlMonth.Items.Clear();
                ddlMonth.DataSource = dsResult;
                ddlMonth.DataTextField = "date";
                ddlMonth.DataValueField = "date";
                ddlMonth.DataBind();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void BtnQuery_Click(object sender, EventArgs e)
        {
            DataSet dsResult = new DataSet();
            DataView dv = new DataView();
            dsResult = QueryData();
            DataTable dt1 = dsResult.Tables[0].Copy();

            if (dt1.Rows.Count > 0)
            {
                dv = dt1.DefaultView;
                dv.Sort = "廠別 asc,資料種類 desc";
                gvResult.DataSource = dv.ToTable(true);
                gvResult.DataBind();
                return;
            }
            gvResult.DataSource = null;
            gvResult.DataBind();
        }

        private DataSet QueryData()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }


            string queryMonth = ddlMonth.SelectedValue;
            string queryMonthLast = DateTime.ParseExact(ddlMonth.SelectedValue, "yyyy-MM", null).AddMonths(-1).ToString("yyyy-MM");
            StringBuilder strSql = new StringBuilder();
            DataSet dsResult = new DataSet();
            DBConn dbCon = new DBConn();
            strSql.Append(" DECLARE @sql varchar(8000);");
            strSql.Append(" DECLARE @sqldv varchar(500);");
            //strSql.AppendFormat(" SELECT @sqldv = isnull(@sqldv + '],[' , '') + MODE from WOCOMPRESS WHERE ReportMonth = '{0}' group by MODE", ddlMonth.SelectedValue);
            strSql.AppendFormat(" SELECT @sqldv = isnull(@sqldv + '],[' , '') + MODE from WOCOMPRESS WHERE (ReportMonth = '{0}' AND SEQ = 'SEQ1') OR (ReportMonth = '{1}' AND SEQ = 'SEQ2') group by MODE", queryMonth, queryMonthLast);
            strSql.Append(" SET @sql = 'SELECT CASE FABID WHEN ''A'' THEN ''WD01'' WHEN ''B'' THEN ''WE01'' ELSE '''' END AS ''廠別'',MODE0 AS ''資料種類'',/*FRWONBR as ''轉出工單'',*/ TOWONBR as ''工單號碼'',CONVERT(NVARCHAR(30),PSTARTDATE,120) as ''Plan StartDate''");
            strSql.Append(" ,[' + @sqldv + ']");
            strSql.Append(" ,T9NCNO AS ''機種名稱'',SHOP_CODE as ''Shop Code'',WOENTITYQTY AS ''工單開立數量'' FROM(");
            strSql.Append(" SELECT A.FABID,A.SEQ,A.MODE AS MODE0,A.MODE,/*A.FRWONBR,*/A.TOWONBR,B.T9NCNO,A.PSTARTDATE,A.SHOP_CODE,A.QTY,WOENTITYQTY FROM (");
            strSql.Append(" SELECT w.FABID,w.SEQ,W.MODE,/*W.FRWONBR,*/W.TOWONBR,W.FRPRODID,W.PSTARTDATE,SUM(W.QTY) AS QTY,SHOP_CODE,S.WOENTITYQTY");
            strSql.Append(" FROM WOCOMPRESS w ");
            strSql.Append(" LEFT JOIN New_SWOM S ON S.WONBR = W.TOWONBR ");
            //strSql.AppendFormat(" WHERE W.ReportMonth = ''{0}''", ddlMonth.SelectedValue);
            strSql.AppendFormat(" WHERE (W.ReportMonth = ''{0}'' AND W.SEQ = ''SEQ1'') OR (W.ReportMonth = ''{1}'' AND W.SEQ = ''SEQ2'')", queryMonth, queryMonthLast);
            strSql.Append(" GROUP BY w.FABID,w.SEQ,W.MODE,/*W.FRWONBR,*/W.TOWONBR,W.FRPRODID,W.PSTARTDATE,SHOP_CODE,S.WOENTITYQTY) A, MPM9M B");
            strSql.Append(" WHERE A.FABID=B.FABID AND A.FRPRODID=B.PRODID");
            strSql.Append(" ) AS AA PIVOT (MAX(QTY) FOR MODE IN([' + @sqldv + '])) AS PVT  order by 2 desc'");
            strSql.Append(" exec( @sql);");
            dsResult = dbCon.GetDataSetBySql(strSql.ToString());
            return dsResult;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            //Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            //Response.Write("<meta http-equiv=Content-Type content=\"text/html | image/png; charset=BIG5\"/>");
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            this.gvResult.RenderControl(oHtmlTextWriter);

            //this.GridView5.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].Text.Contains("SEQ"))
                    {
                        //e.Row.Cells[i].Text = str1;
                        e.Row.Cells[i].BackColor = Color.LightSeaGreen;
                    }
                    if (e.Row.Cells[i].Text.Contains("SEQ1"))
                    {
                        e.Row.Cells[i].Text = str1;
                        e.Row.Cells[i].BackColor = Color.LightSeaGreen;
                    }
                    if (e.Row.Cells[i].Text.Contains("SEQ2"))
                    {
                        e.Row.Cells[i].Text = str2;
                        e.Row.Cells[i].BackColor = Color.LightSeaGreen;
                    }
                    if (e.Row.Cells[i].Text.Contains("SEQ3"))
                    {
                        e.Row.Cells[i].Text = str3;
                        e.Row.Cells[i].BackColor = Color.LightSeaGreen;
                    }
                }
            }
        }
    }
}