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

namespace WebReport.Reports.PC
{
    public partial class WipInfoQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
            }
        }
        private void SetProdID()
        {
            string strsql = @"select prodid from mpim";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find ProductID！');</script>");
            }
            else
            {
                this.ddlProdid.DataSource = dt;
                this.ddlProdid.DataTextField = "prodid";
                this.ddlProdid.DataValueField = "prodid";
                this.ddlProdid.DataBind();
            }
        }

        protected void chkProdid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProdid.Checked == true)
            {
                ddlProdid.Enabled = true;
                SetProdID();
            }
            else
            {
                ddlProdid.Enabled = false;
                ddlProdid.Items.Clear();
            }
        }


        protected void DataBound()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string s = string.Empty;
            StringBuilder stb = new StringBuilder();
            stb.Append("select FABID,prodid,LOTLEVEL,entitysts,");
            stb.Append("sum(isnull([6001],0)+isnull([8005],0)+isnull([8010],0)+isnull([8015],0)+isnull([8020],0)+isnull([8030],0)+isnull([8040],0)+isnull([8050],0)+isnull([8060],0)+isnull([8068],0)+isnull([8080],0)+isnull([8081],0)+isnull([8082],0)+isnull([8083],0)+isnull([8084],0)+");
            stb.Append("isnull([8085],0)+isnull([8088],0)+isnull([808A],0)+isnull([808B],0)+isnull([808C],0)+isnull([8090],0)+isnull([80V0],0)+isnull([8100],0)+isnull([8180],0)+isnull([8190],0)+isnull([8195],0)+isnull([8198],0)+isnull([8210],0)+isnull([8230],0)+isnull([8280],0)+");
            stb.Append("isnull([8298],0)+isnull([8350],0)+isnull([8380],0)+isnull([838S],0)+isnull([8450],0)+isnull([8500],0)+isnull([8501],0)+isnull([8600],0)+isnull(0,[8700])+isnull([8800],0)+isnull([9001],0)+isnull([9002],0)+isnull([CA0L],0)+isnull([CR0F],0)+isnull([CR0H],0)+");
            stb.Append("isnull([CR0J],0)+isnull([CR0L],0)+isnull([CR0M],0)+isnull([CS0S],0)+isnull([CTR1],0)+isnull([DUAL],0)+isnull([MR0A],0)+isnull([MR0B],0)+isnull([MR0C],0)+isnull([MR0X],0)+isnull([MS0S],0)+isnull([OFBL],0)+isnull([OR01],0)+isnull([ORPK],0)+isnull([RF01],0)+");
            stb.Append("isnull([RS01],0)+isnull([RS02],0)+isnull([RS0C],0)+isnull([WR01],0)) as total,isnull([6001],0)as [6001],isnull([8005],0)as [8005],isnull([8010],0)as [8010],isnull([8015],0)as [8015],isnull([8020],0)as [8020],isnull([8030],0)as [8030],isnull([8040],0)as [8040],isnull([8050],0)as [8050],isnull([8060],0)as [8060],isnull([8068],0)as [8068],");
            stb.Append("isnull([8080],0)as [8080],isnull([8081],0)as [8081],isnull([8082],0)as [8082],isnull([8083],0)as [8083],isnull([8084],0)as [8084],isnull([8085],0)as [8085],isnull([8088],0)as [8088],isnull([808A],0)as [808A],isnull([808B],0)as [808B],isnull([808C],0)as [808C],isnull([8090],0)as [8090],isnull([80V0],0)as [80V0],isnull([8100],0)as [8100],isnull([8180],0)as [8180],isnull([8190],0)as [8190],");
            stb.Append("isnull([8195],0)as [8195],isnull([8198],0)as [8198],isnull([8210],0)as [8210],isnull([8230],0)as [8230],isnull([8280],0)as [8280],");
            stb.Append("isnull([8298],0)as [8298],isnull([8350],0)as [8350],isnull([8380],0)as [8380],isnull([838S],0)as [838S],isnull([8450],0)as [8450],isnull([8500],0)as [8500],isnull([8501],0)as [8501],isnull([8600],0)as [8600],isnull(0,[8700])as [8700],isnull([8800],0)as [8800],isnull([9001],0)as [9001],isnull([9002],0)as [9002],isnull([CA0L],0)as [CA0L],isnull([CR0F],0)as [CR0F],isnull([CR0H],0)as [CR0H],");
            stb.Append("isnull([CR0J],0)as [CR0J],isnull([CR0L],0)as [CR0L],isnull([CR0M],0)as [CR0M],isnull([CS0S],0)as [CS0S],isnull([CTR1],0)as [CTR1],isnull([DUAL],0)as [DUAL],isnull([MR0A],0)as [MR0A],isnull([MR0B],0)as [MR0B],isnull([MR0C],0)as [MR0C],isnull([MR0X],0)as [MR0X],isnull([MS0S],0)as [MS0S],isnull([OFBL],0)as [OFBL],isnull([OR01],0)as [OR01],isnull([ORPK],0)as [ORPK],isnull([RF01],0)as [RF01],");
            stb.Append("isnull([RS01],0)as [RS01],isnull([RS02],0)as [RS02],isnull([RS0C],0)as [RS0C],isnull([WR01],0)as [WR01]");
            stb.Append("from(");
            stb.Append("select fabid,prodid,cworkctr,lotlevel,entitysts,entitytqty from New_wltm) as A");
            stb.Append("   pivot(sum(entitytqty) for cworkctr in ([6001],[8005],[8010],[8015],[8020],[8030],[8040],[8050],[8060],[8068],[8080],[8081],[8082],");
            stb.Append("[8083],[8084],[8085],[8088],[808A],[808B],[808C],[8090],[80V0],[8100],[8180],[8190],[8195],[8198],[8210],[8230],[8280],[8298],[8350],[8380],[838S],[8450],[8500],[8501],[8600],[8700],[8800],[9001],[9002],[CA0L],[CR0F],[CR0H],[CR0J],[CR0L],[CR0M],[CS0S],[CTR1],[DUAL],[MR0A],[MR0B],[MR0C],[MR0X],[MS0S],[OFBL],[OR01],[ORPK],[RF01],[RS01],[RS02],[RS0C],[WR01] ");
            stb.Append("))as pvt where 1=1   ");

            if (chkProdid.Checked == false && chklevel.Checked == false && chkStatus.Checked == false)
            {
                if (chkStatus.Checked == false)
                {
                    foreach (ListItem i in listlevel.Items)
                    {
                        if (i.Selected == false)
                        {
                            stb.Append("");
                        }
                        else
                        {
                            s += "'" + i.Text.Trim().ToString() + "',";
                        }
                    }
                    if (s != "")
                    {
                        s = s.Substring(0, s.Length - 1);
                        stb.Append("   and entitysts in (" + s + ")");
                    }
                    else
                    {
                        stb.Append("");
                    }
                }
                else
                {
                    stb.Append("");
                }
            }
            else
            {
                if (chkProdid.Checked == true)
                {
                    stb.Append("    and prodid='" + ddlProdid.Text.Trim().ToString() + "'");
                }
                if (chklevel.Checked == true)
                {
                    stb.Append("    and lotlevel='" + ddlLevel.Text.Trim().ToString() + "'");
                }
                if (chkStatus.Checked == true)
                {
                    stb.Append("");
                }
                else
                {
                    string x = string.Empty;
                    foreach (ListItem j in listlevel.Items)
                    {
                        if (j.Selected == false)
                        {
                            stb.Append("");
                        }
                        else
                        {
                            x += "'" + j.Text.Trim().ToString() + "',";
                        }
                    }
                    if (x != "")
                    {
                        x = x.Substring(0, x.Length - 1);
                        stb.Append("   and entitysts in (" + x + ")");
                    }
                }
            }

            stb.Append("   group by FABID,prodid,LOTLEVEL,entitysts,[6001],[8005],[8010],[8015],[8020],[8030],[8040],[8050],[8060],[8068],[8080],[8081],[8082],[8083],[8084],[8085],[8088],[808A],[808B],[808C],[8090],[80V0],[8100],[8180],[8190],[8195],[8198],[8210],[8230],[8280],[8298],[8350],[8380],[838S],[8450],[8500],[8501],[8600],[8700],");
            stb.Append("[8800],[9001],[9002],[CA0L],[CR0F],[CR0H],[CR0J],[CR0L],[CR0M],[CS0S],[CTR1],[DUAL],[MR0A],[MR0B],[MR0C],[MR0X],[MS0S],[OFBL],[OR01],[ORPK],[RF01],[RS01],[RS02],[RS0C],[WR01] order by prodid");
            DataTable dt = new DataTable();
            dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];

            this.reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.reportViewer.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\WipInfoQuery.rdlc";


            ReportDataSource rsd = new ReportDataSource("WipInfoQuery", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);

            if (dt.Rows.Count == 0)
            {
                Label5.Text = "查無資料!";
            }
            else
            {
                Label5.Text = "";
            }
        //    GridView1.Visible = true;
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        }

        protected void butQuery_Click(object sender, EventArgs e)
        {
            DataBound();
            //ImageButton1.Visible = true;
        }

        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStatus.Checked == true)
            {
                foreach (ListItem i in this.listlevel.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in listlevel.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chklevel_CheckedChanged(object sender, EventArgs e)
        {
            if (chklevel.Checked == true)
            {
                ddlLevel.Enabled = true;
            }
            else
            {
                ddlLevel.Enabled = false;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            //this.GridView1.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

    }
}
