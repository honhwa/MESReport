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

//-------------------------------------------------------------------------------------------------------------------------------
//Last Modify Date    Author          Version     Description
//--------------------    -------------   -----------  ----------------------------------------------------------------------------
//  2012/08/01         TPO/Li.Miao    1.00         由之前的GridView變更為ReportViewer呈現數據
//-------------------------------------------------------------------------------------------------------------------------------


//namespace Report.Reports.PE        1.00 
namespace WebReport.Reports.PE
{
    public partial class glasssemiscribe : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strsql;
                strsql = "select distinct soprodid from mspm where fabid='" + ((UserInfo)Session["userinfo"]).FabId + "' and left(soprodid,1)not in('#','v') and class='panel' ";
                DataSet dtprod = dbConn.GetDataSetBySql(strsql);
                if (dtprod.Tables[0].Rows.Count > 0)
                {
                    lstProduct.DataTextField = "soprodid";
                    lstProduct.DataValueField = "soprodid";
                    this.lstProduct.DataSource = dtprod;
                    this.lstProduct.DataBind();
                }
            }
        }
        protected void chkprodall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkprodall.Checked)
            {
                foreach (ListItem i in lstProduct.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstProduct.Items)
                {
                    i.Selected = false;
                }
            }
        }

        private string rstrprod(CheckBoxList cbl)
        {
            int i;
            string strgrade;
            strgrade = "";
            for (i = 0; i <= cbl.Items.Count - 1; i++)
            {
                if (cbl.Items[i].Selected)
                {
                    strgrade += "'" + cbl.Items[i].Value + "',";
                }
            }
            if (strgrade == "")
            {
                strgrade = "''";
            }
            else
            {
                strgrade = strgrade.Substring(0, strgrade.Length - 1);
            }

            return strgrade;
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strsql;
            string strprod;
            string datefr;
            string dateto;       
            datefr = dtfr.datatimer() + " 07:30:00";
            dateto = dtto.datatimer() + " 07:30:00";          
            strprod = rstrprod(lstProduct);
            strsql = "select  c.aft2prodid as 'CELL-2 Product ID',b.glsid as 'Glass ID',right(b.shtid,1) as 'Sheet ID' ,replace(convert(nvarchar(16),a.transdate,120),'-','/') as 'Semi Scribe Track Out Time'";
            strsql += "  from New_wltm a,New_WSHTM b,New_swom c   where a.fabid='" + ((UserInfo)Session["userinfo"]).FabId + "' and   a.hideflg='1' and a.entityunit='b' and b.fabid=a.fabid and b.entityid=a.entityid and c.fabid=a.fabid and c.wonbr=a.wonbr";
            if (txtgls.Text == "")
            {
                if (Convert.ToDateTime(datefr) > Convert.ToDateTime(dateto))
                {
                    strsql += " and  a.transdate >'" + dateto + "' and a.transdate < '" + Convert.ToDateTime(datefr).AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "'";
                }
                else
                {
                    strsql += "  and a.transdate >'" + datefr + "' and a.transdate < '" + Convert.ToDateTime(dateto).AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "'";
                }

                if (chkprodall.Checked)
                { }
                else
                {
                    strsql += " and c.aft2prodid in (" + strprod + ")";
                }

            }
            else
            {
                strsql += " and b.glsid in ('"+ txtgls.Text.Replace ("\r\n","','")+"')";
            }
            strsql += " order by c.aft2prodid,b.glsid,right(b.shtid,1) ";
            //1.00 Begin By Li.Miao Mark
            //DataSet resultds = dbConn.GetDataSetBySql(strsql);
            //if (resultds.Tables[0].Rows.Count >= 0)
            //{
            //    gvcontent.DataSource = resultds;
            //    gvcontent.DataBind();
            //}

            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count >= 0)
            {
                this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\GlassSemiScribe.rdlc";

                ReportDataSource rsd = new ReportDataSource("GlassSemiScribe", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }
            //1.00 End
        }

        //1.00 Begin By Li.Miao Mark
        //protected void btnexport_Click(object sender, EventArgs e)
        //{
        //    gridviewtoexcel(gvcontent);
        //}
        //1.00 End
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        //1.00 Begin By Li.Miao Mark
        //private void gridviewtoexcel(GridView ctrl)
        //{
        //    string filename;
        //    filename = "glasstrackou";
        //    HttpContext.Current.Response.Charset = "GB2312";
        //    HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
        //    HttpContext.Current.Response.AppendHeader("Content-Disposition",
        //        "attachment;filename=" + HttpUtility.UrlEncode(filename, Encoding.UTF8).ToString());
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //    //ctrl.Page.EnableViewState = false;//分页的时候用到

        //    StringWriter tw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(tw);
        //    ctrl.RenderControl(hw);
        //    HttpContext.Current.Response.Write(tw.ToString());
        //    HttpContext.Current.Response.End();
        //}
        //1.00 End
    }
}
