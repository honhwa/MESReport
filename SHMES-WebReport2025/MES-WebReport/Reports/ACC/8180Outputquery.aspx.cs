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


namespace Report.Reports.ACC
{
    public partial class _180Outputquery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public DataSet dsDetail = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtDate.Attributes.Add("onclick", "setday(this)");
            this.txtToDate.Attributes.Add("onclick", "setday(this)");
            if (!Page.IsPostBack)
            {
                this.txtDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", System.DateTime.Now.AddDays(0));
                this.txtToDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", System.DateTime.Now.AddDays(0));
                this.CheckBox1.Checked = false;
                this.WORDataSource();
                this.MLRDataSource();
                this.MGRDataSource();
            }

        }
        private void WORDataSource()
        {
            //this.line.Items.Clear();
            string strSql = @"select DISTINCT WORKCTR,(WORKCTR+'    '+WORKNAME) AS WORKCTR1 from MWCM order by WORKCTR asc ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.chkWOR.DataSource = dt;
            this.chkWOR.DataTextField = "WORKCTR1";
            this.chkWOR.DataValueField = "WORKCTR";
            this.chkWOR.DataBind();
            //this.line.ClearSelection();
        }
        private void MLRDataSource()
        {
            //this.line.Items.Clear();
            string strSql = @"select DISTINCT LOTTYPE,(LOTTYPE+'    '+LOTTYPEDESC) AS LOTTYPE1 from MLTM order by LOTTYPE asc ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.chkML.DataSource = dt;
            this.chkML.DataTextField = "LOTTYPE1";
            this.chkML.DataValueField = "LOTTYPE";
            this.chkML.DataBind();
            //this.line.ClearSelection();
        }
        private void MGRDataSource()
        {
            //this.line.Items.Clear();
            string strSql = @"select DISTINCT GRADEID,GRADEDESC from MGRM order by GRADEID asc ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.chkMG.DataSource = dt;
            this.chkMG.DataTextField = "GRADEDESC";
            this.chkMG.DataValueField = "GRADEID";
            this.chkMG.DataBind();
            //this.line.ClearSelection();
        }

        protected void Query_Click(object sender, EventArgs e)
        {
            GetRecord();

        }

        void GetRecord()
        {
            RecordLog Log = new RecordLog();
            string StrSql = string.Empty;
            StrSql = @"SELECT B.*,s.M1ProdID FROM ( ";
            StrSql += " " + "SELECT A.CWORKCTR,TIMES,A.FRCPRODID,A.FRWONBR,COUNT(CENTITYTQTY) AS NUMBER ";
            StrSql += " " + "FROM (SELECT v.CWORKCTR,LEFT(TRANSID,10) AS TIMES,v.FRWONBR,v.FRCPRODID,v.CENTITYTQTY,v.CENTITYSTS,v.VERB,v.FRLOTTYPE,v.TRANSID,v.TOLOTGRADE FROM New_WLTX AS v) A";
            StrSql += " " + "WHERE A.VERB='MVOU' AND";
            StringBuilder strchkWOR = new StringBuilder();
            if (this.chkWOR.Items.Count != 0)
            {
                if (this.chkWOR.SelectedIndex >= 0)
                {
                    foreach (ListItem li in this.chkWOR.Items)
                    {
                        if (li.Selected)
                        {
                            strchkWOR.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strchkWOR.Length > 0)
            {

                StrSql += " " + "A.CWORKCTR IN (" + strchkWOR.ToString().Replace("''", "','") + ") AND";

            }
            StringBuilder strchkML = new StringBuilder();
            if (this.chkML.Items.Count != 0)
            {
                if (this.chkML.SelectedIndex >= 0)
                {
                    foreach (ListItem li in this.chkML.Items)
                    {
                        if (li.Selected)
                        {
                            strchkML.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strchkML.Length > 0)
            {
                if (CheckBox2.Checked == true)
                {
                    StrSql += " " + "A.FRLOTTYPE NOT IN (" + strchkML.ToString().Replace("''", "','") + ") AND";
                }
                else
                {
                    StrSql += " " + "A.FRLOTTYPE IN (" + strchkML.ToString().Replace("''", "','") + ") AND";
                }
            }
            StringBuilder strchkMG = new StringBuilder();
            if (this.chkMG.Items.Count != 0)
            {
                if (this.chkMG.SelectedIndex >= 0)
                {
                    foreach (ListItem li in this.chkMG.Items)
                    {
                        if (li.Selected)
                        {
                            strchkMG.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strchkMG.Length > 0)
            {
                StrSql += " " + "A.TOLOTGRADE IN (" + strchkMG.ToString().Replace("''", "','") + ") AND";
            }
            StrSql += " " + "A.TRANSID BETWEEN LEFT('" + txtDate.Text.Trim() + "',10) AND LEFT('" + txtToDate.Text.Trim() + "',10)";
            StrSql += " " + "GROUP BY A.CWORKCTR,TIMES,A.FRCPRODID,A.FRWONBR) B";
            StrSql += " " + "LEFT JOIN New_SWOM AS s ON s.WONBR=B.FRWONBR";
            StrSql += " " + "ORDER BY TIMES,B.FRCPRODID,B.FRWONBR";
            dsDetail = this.dbConn.GetDataSetBySql(StrSql);
            if (dsDetail == null || dsDetail.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('查無數據，請確認！')</script>");
                return;
            }
            this.GridView1.DataSource = dsDetail;
            this.GridView1.DataBind();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1)
            {
                int id = e.Row.RowIndex + 1;
                e.Row.Cells[0].Text = id.ToString();
            }
        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 得到该控件
            GridView theGrid = sender as GridView;
            int newPageIndex = 0;
            if (e.NewPageIndex == -3)
            {
                //点击了Go按钮
                TextBox txtNewPageIndex = null;

                //GridView较DataGrid提供了更多的API，获取分页块可以使用BottomPagerRow 或者TopPagerRow，当然还增加了HeaderRow和FooterRow
                GridViewRow pagerRow = theGrid.BottomPagerRow;

                if (pagerRow != null)
                {
                    //得到text控件
                    txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
                }
                if (txtNewPageIndex != null)
                {
                    //得到索引
                    newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
                }
            }
            else
            {
                //点击了其他的按钮
                newPageIndex = e.NewPageIndex;
            }
            //防止新索引溢出
            newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
            newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;

            //得到新的值
            theGrid.PageIndex = newPageIndex;

            //重新绑定
            GetRecord();

        }


        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            this.GridView1.AllowPaging = false;
            this.GetRecord();
            Response.Charset = "UTF-8";
            Response.ClearContent();
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.HeaderEncoding = System.Text.Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment; filename=MyExpress.xls");
            Response.ContentType = "application/excel";
            string strStyle = "<style>td{mso-number-format:\"\\@\";}</style>";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            sw.WriteLine(strStyle);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView1.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
