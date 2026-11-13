using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;
using System.Web.Hosting;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using Report.Library;

namespace WebReport.Reports
{
    /// <summary>
    /// FrmInputANDOutput 的摘要描述。
    /// </summary>
    public partial class FrmInputANDOutput : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        private string sql = string.Empty;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在這裡放置使用者程式碼以初始化網頁
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                FrmInputANDOutputQuery wf6;
                wf6 = (FrmInputANDOutputQuery)Context.Handler;
                this.TextBox1.Text = wf6.FrTime;
                this.TextBox2.Text = wf6.ToTime;
                this.TextBox3.Text = wf6.Prodid;
                this.TextBox4.Text = wf6.Wonbr;
                this.TextBox5.Text = wf6.ProcessID;
                this.TextBox6.Text = wf6.Type;
                this.TextBox7.Text = wf6.Station;
                DataBound();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        #region Web Form 設計工具產生的程式碼
        //override protected void OnInit(EventArgs e)
        //{
        //    //
        //    // CODEGEN: 此為 ASP.NET Web Form 設計工具所需的呼叫。
        //    //
        //    InitializeComponent();
        //    base.OnInit(e);
        //}

        /// <summary>
        /// 此為設計工具支援所必須的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.ImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.ImageButton2_Click);
        //    this.ReportViewer.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
        //    this.ReportViewer.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
        //    this.GridView1.PageIndexChanging += new GridViewPageEventHandler(Page_Grid); 
        //}
        #endregion

        //public void Page_Grid(object sender, GridViewPageEventArgs e)
        //{

        //    //this.GridView1.PageIndex = e.NewPageIndex;
        //    this.DataBound();

        //} 


        //private void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    Response.Clear();
        //    Response.Buffer = false;
        //    Response.Charset = "UTF-8";
        //    Response.ContentType = "Content-Language;content=UTF-8";
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        //    Response.ContentType = "application/ms-excel";
        //    this.EnableViewState = false;
        //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        //    if (TextBox6.Text.Trim() == "input&output")
        //    {
        //        this.ReportViewer.RenderControl(oHtmlTextWriter);
        //    }
        //    if (TextBox6.Text.Trim() == "defect")
        //    {
        //        this.ReportViewerDefect.RenderControl(oHtmlTextWriter);
        //    }

        //    Response.Output.Write(oStringWriter.ToString());
        //    Response.End();
        //}
        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    return;   //強迫執行匯出excel
        //}

        //private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //    e.Item.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
        //    e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        //    if (e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        e.Item.BackColor = Color.Gainsboro;
        //    }
        //    //e.Item.Attributes.Add("ondblclick","window.open('CrystalReport/Cry_SBInput.aspx?id="+e.Item.Cells[1].Text+"&frdate="+this.TextBox1.Text+"&todate="+this.TextBox2.Text+"')");
        //}

        //private void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    this.Response.Redirect("FrmInputANDOutputQuery.aspx");
        //}

        //private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        //{
        //    this.ReportViewer.CurrentPageIndex = e.NewPageIndex;
        //    this.DataBound();
        //}

        /*    protected void GridView1_PageIndexChanged(object source, System.Web.UI.WebControls.GridViewPageEventArgs e)
            {
                this.GridView1.PageIndex = e.NewPageIndex;
                this.DataBound();
            }*/


        private void DataBound()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder Lchar = new StringBuilder();
            if (TextBox6.Text.Trim() == "input&output")
            {
                sql = @"select A.Processid,A.Subline,A.Station,A.Wonbr,A.Prodid,A.Grade,A.Level,A.Customer,sum(inputqty) as Inputqty,sum(outputqty) as Outputqty,b.t9ncno from ( ";
                sql += @"SELECT cprocid as processid,csublineid as subline,cworkctr as station,towonbr as wonbr,toProdid as prodid,tolotGrade as grade,tolotLevel as level,customerid as customer,
                                case when verb = 'MVIN' then centitytqty else 0 end as inputqty,case when verb = 'MVOU' then centitytqty else 0 end as outputqty
                                FROM New_WLTX
                                where FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'" + "  AND (VERB = 'MVIN' OR VERB = 'MVOU') ";
                sql += @" and TRANSDATE>=convert(datetime,'" + TextBox1.Text + "',20) " + " and TRANSDATE<=convert(datetime,'" + TextBox2.Text + "',20) ";
                if (TextBox5.Text != "")
                {
                    sql += @" and cprocid='" + TextBox5.Text + "'";
                }
                if (TextBox7.Text != "")
                {
                    sql += @" and cworkctr='" + TextBox7.Text + "'";
                }
                if (TextBox3.Text != "")
                {
                    sql += @" and toProdid='" + TextBox3.Text + "'";
                }
                if (TextBox4.Text != "")
                {
                    sql += @" and towonbr='" + TextBox4.Text + "'";
                }
                sql += " and centitytqty<>0";
                sql += @" union all ";
                sql += @"SELECT cprocid as processid,csublineid as subline,cworkctr as station,towonbr as wonbr,toProdid as prodid,tolotGrade as grade,tolotLevel as level,customerid as customer,
                                case when verb = 'MVIN' then centitytqty else 0 end as inputqty,case when verb = 'MVOU' then centitytqty else 0 end as outputqty
                                FROM WLTXH
                                where FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'" + " AND (VERB = 'MVIN' OR VERB = 'MVOU') ";
                sql += @" and TRANSDATE>=convert(datetime,'" + TextBox1.Text + "',20) " + " and TRANSDATE<=convert(datetime,'" + TextBox2.Text + "',20) ";
                if (TextBox5.Text != "")
                {
                    sql += @" and cprocid='" + TextBox5.Text + "'";
                }
                if (TextBox7.Text != "")
                {
                    sql += @" and cworkctr='" + TextBox7.Text + "'";
                }
                if (TextBox3.Text != "")
                {
                    sql += @" and toProdid='" + TextBox3.Text + "'";
                }
                if (TextBox4.Text != "")
                {
                    sql += @" and towonbr='" + TextBox4.Text + "'";
                }
                sql += " and centitytqty<>0";
                sql += @" ) A left join mpm9m b on A.prodid=B.prodid group by A.processid,A.subline,A.station,A.wonbr,A.prodid,A.grade,A.level,A.customer,B.t9ncno  ";
                sql += @" order by A.processid,A.subline,A.station,A.wonbr,A.prodid,A.grade,A.level,A.customer";

                Label3.Text = "Input Output Query Result";
                this.ReportViewer.Visible = true;
                this.ReportViewerDefect.Visible = false;
            }

            if (TextBox6.Text.Trim() == "defect")
            {
                sql = @"select A.Processid,A.Station,A.Prodid,A.Subline,A.Wonbr,A.Defectcd,sum(defectqty) as Defectqty,B.t9ncno from ( ";
                sql += @"SELECT cprocid as processid,csublineid as subline,cworkctr as station,wonbr,prodid,defectcd,defectqty
                                 FROM New_WLDX
                                where FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'" + " ";
                sql += @" and TRANSDATE>=convert(datetime,'" + TextBox1.Text + "',20) " + " and TRANSDATE<=convert(datetime,'" + TextBox2.Text + "',20) ";
                if (TextBox5.Text != "")
                {
                    sql += @" and cprocid='" + TextBox5.Text + "'";
                }
                if (TextBox7.Text != "")
                {
                    sql += @" and cworkctr='" + TextBox7.Text + "'";
                }
                if (TextBox3.Text != "")
                {
                    sql += @" and Prodid='" + TextBox3.Text + "'";
                }
                if (TextBox4.Text != "")
                {
                    sql += @" and wonbr='" + TextBox4.Text + "'";
                }
                sql += " and defectqty<>0";
                sql += @" union all ";
                sql += @"SELECT cprocid as processid,csublineid as subline,cworkctr as station,wonbr,prodid,defectcd,defectqty
                                 FROM WLDXH
                                where FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'" + " ";
                sql += @" and TRANSDATE>=convert(datetime,'" + TextBox1.Text + "',20) " + " and TRANSDATE<=convert(datetime,'" + TextBox2.Text + "',20) ";
                if (TextBox5.Text != "")
                {
                    sql += @" and cprocid='" + TextBox5.Text + "'";
                }
                if (TextBox7.Text != "")
                {
                    sql += @" and cworkctr='" + TextBox7.Text + "'";
                }
                if (TextBox3.Text != "")
                {
                    sql += @" and Prodid='" + TextBox3.Text + "'";
                }
                if (TextBox4.Text != "")
                {
                    sql += @" and wonbr='" + TextBox4.Text + "'";
                }
                sql += " and defectqty<>0";
                sql += @" ) A left join mpm9m B on A.prodid=B.prodid group by A.processid,A.station,A.prodid,A.subline,A.wonbr,A.defectcd,B.t9ncno  ";
                sql += @" order by A.processid,A.station,A.prodid,A.subline,A.wonbr,A.defectcd";

                Label3.Text = "Defect Query Result";
                this.ReportViewer.Visible = false;
                this.ReportViewerDefect.Visible = true;
            }

            DataTable dt = this.dbConn.GetDataSetBySql(sql).Tables[0];
            if (TextBox6.Text.Trim() == "input&output")
            {
                this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\InputOutputQueryResult.rdlc";

                ReportDataSource rsd = new ReportDataSource("InputOutputQueryResult", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }
            if (TextBox6.Text.Trim() == "defect")
            {
                this.ReportViewerDefect.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewerDefect.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\DefectQueryResult.rdlc";

                ReportDataSource rsd = new ReportDataSource("DefectQueryResult", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }


            //if (dt.Rows.Count > 1000)
            //{
            //    this.DataGrid1.AllowPaging = true;
            //    this.GridView1.AllowPaging = true;
            //}
            //if (TextBox6.Text.Trim() == "input&output")
            //{
            //    //this.ReportViewer.DataSource = dt;
            //    //this.ReportViewer.DataBind();
            //    //this.ReportViewer.Height = 20;
            //}
            //if (TextBox6.Text.Trim() == "defect")
            //{
            //    //this.GridView1.DataSource = dt;
            //    //this.GridView1.DataBind();
            //    //this.GridView1.Height = 20;
            //}
            if (dt.Rows.Count == 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Sorry, no data under this category！');</script>");
            }

            if (TextBox5.Text.Trim() != "") { Lchar.Append("ProcessID:" + TextBox5.Text + ";"); }
            if (TextBox7.Text.Trim() != "") { Lchar.Append("Station:" + TextBox7.Text + ";"); }
            if (TextBox3.Text.Trim() != "") { Lchar.Append("Prodid:" + TextBox3.Text + ";"); }
            if (TextBox4.Text.Trim() != "") { Lchar.Append("Wonbr:" + TextBox4.Text + ";"); }
            this.Label2.Text = Lchar.ToString() + "Rows:" + dt.Rows.Count.ToString();

        }
    }
}
