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
using System.Data.SqlClient;
using System.Text;
using Microsoft.Reporting.WebForms;
using Report.Library;
using Report.Sources;
using System.IO;

namespace Report.Reports.ACC
{
    public partial class WOQuery : System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {       
            DataTable dt=new DataTable();
            StringBuilder stb = new StringBuilder();
            string sqlwhere = "''";

            if (TextBox1.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的工單！');</script>");
                return;
            }
            else
            {

                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                string[] txtRows = TextBox1.Text.Split(Environment.NewLine.ToCharArray());
                foreach (string str in txtRows)
                {
                    if (str != "")
                    {

                        sqlwhere = sqlwhere + ",'" + str + "'";
                    }
                }
                sqlwhere = sqlwhere.Replace("'',", "");
                stb.Append("SELECT FABID,WONBR,CONVERT(varchar(100),CREATEDATE, 20) AS CREATEDATE ,CONVERT(varchar(100),STBDATE, 20)AS STBDATE,CONVERT(varchar(100),CLOSEDATE, 20) AS CLOSEDATE FROM New_SWOM WHERE ");
                stb.Append("FABID='B' AND ");
                stb.Append("WONBR IN(" + sqlwhere + ") ");
            }
            
            dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
            if (dt.Rows.Count < 1)
            {
                this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("报表.xls", Encoding.UTF8).ToString());
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            this.GridView1.AllowPaging = false;
            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("zh-cn", true);
            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            GridView1.RenderControl(hw);


            Response.Write(tw.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for
        }
    }
}
