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
    public partial class WipStatusQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        //DataSet  ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetProdid();
                SetProjectCode();
                SetWonbr();
                SetStation();
                foreach (ListItem i in lstProdid.Items)
                {
                    StringBuilder stbcheck = new StringBuilder();
                    stbcheck.Append(@"SELECT PRODID FROM (select prodid from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] union select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem]) A");
                    stbcheck.Append("   where prodid='" + i.Text.Trim() + "'");
                    DataTable prodidcheck = new DataTable();
                    prodidcheck = this.dbConn.GetDataSetBySql(stbcheck.ToString()).Tables[0];
                    if (prodidcheck.Rows.Count == 0)
                    {
                        i.Selected = false;
                    }
                    else
                    {
                        i.Selected = true;
                    }
                }

                Label1.Visible = false;
                ImageButton1.Visible = false;
                lstGrade.Items[0].Selected = true;
                foreach (ListItem i in lstProjectCode.Items)
                {
                    i.Selected = true;
                }
                foreach (ListItem i in lstStation.Items)
                {
                    i.Selected = true;
                }
                foreach (ListItem i in lstWonbr.Items)
                {
                    i.Selected = true;
                }
            }
        }

        private void SetStation()
        {
            string strsql = @"select workctr from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mwcm] union select workctr from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mwcm]";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點!');</script>");
            }
            else
            {
                this.lstStation.DataSource = dt;
                this.lstStation.DataTextField = "workctr";
                this.lstStation.DataValueField = "workctr";
                this.lstStation.DataBind();
            }
        }

        private void SetProjectCode()
        {
            string strsql = @"select project_code from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] union select project_code from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem]";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點!');</script>");
            }
            else
            {
                this.lstProjectCode.DataSource = dt;
                this.lstProjectCode.DataTextField = "project_code";
                this.lstProjectCode.DataValueField = "project_code";
                this.lstProjectCode.DataBind();
            }
        }

        private void SetProdid()
        {
            string strsql = @"select prodid from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] union select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem]";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點!');</script>");
            }
            else
            {
                this.lstProdid.DataSource = dt;
                this.lstProdid.DataTextField = "prodid";
                this.lstProdid.DataValueField = "prodid";
                this.lstProdid.DataBind();
            }
        }

        private void SetWonbr()
        {
            string strsql = @"select wonbr from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[swom] where prodid in (select prodid from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem]) union select wonbr from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[swom] where prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem])";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點!');</script>");
            }
            else
            {
                this.lstWonbr.DataSource = dt;
                this.lstWonbr.DataTextField = "wonbr";
                this.lstWonbr.DataValueField = "wonbr";
                this.lstWonbr.DataBind();
            }
        }

        protected void chkProjectCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProjectCode.Checked)
            {
                foreach (ListItem i in lstProjectCode.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstProjectCode.Items)
                {
                    //StringBuilder stb = new StringBuilder();
                    //stb.Append(@"select project_code from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] union select project_code from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem]");
                    //stb.Append(@"select prodid from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mpim] where prodid in ('TJ035FC01AA7','TJ035FC01AA6','AP035FUPC05','AP035FUPC03','AP035F0PC04','AP035F0PC04') union select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mpim] where prodid in ('TJ035FC01AA7','TJ035FC01AA6','AP035FUPC05','AP035FUPC03','AP035F0PC04','AP035F0PC04')");
                    //stb.Append(" where 1=1 and prodid='"+i.Text.Trim()+"'");
                    // DataTable dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                    i.Selected = false;

                }
            }
        }


        protected void chkProdid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProdid.Checked)
            {
                foreach (ListItem i in lstProdid.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstProdid.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkWo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWo.Checked)
            {
                foreach (ListItem i in lstWonbr.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstWonbr.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkGrade_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGrade.Checked)
            {
                foreach (ListItem i in lstGrade.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstGrade.Items)
                {
                    i.Selected = false;
                }
            }
        }


        private void DataBound()
        {
            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            string s5 = "";

            StringBuilder stb = new StringBuilder();
            stb.Append(@"select * from (select case when a.fabid='A' then 'LCM1' else 'LCM2' end as fabid,a.cworkctr,c.workname,b.prodtype,b.project_code,a.prodid,a.wonbr,a.lotgrade,sum(case a.entitysts when 'W' then entitytqty else 0 end) as 'QTY_WAIT',sum(case a.entitysts when 'R' then entitytqty else 0 end) as 'QTY_RUN',sum(case when (a.entitysts='W' or a.entitysts='R') then entitytqty else 0 end) as 'QTY_W+R',sum(case a.entitysts when 'H' then entitytqty else 0 end) as 'QTY_HOLD'from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[wltm] a left join [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] b on a.prodid=b.prodid inner join [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mwcm] c on a.cworkctr=c.workctr and substring((case when a.cartid is null or a.cartid = null then '0' when a.cartid = '' then '1' else a.cartid end ),1,1)<> 'F' where 1=1 and a.Fabid='B' and a.hideflg='0'and a.prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem])  group by a.cworkctr,c.workname,b.prodtype,a.fabid,b.project_code,a.prodid,a.wonbr,a.lotgrade  ");
            stb.Append(@"  union all");
            stb.Append(@"  select case when a.fabid='A' then 'LCM1' else 'LCM2' end as fabid,a.cworkctr,c.workname,b.prodtype,b.project_code,a.prodid,a.wonbr,a.lotgrade,sum(case a.entitysts when 'W' then entitytqty else 0 end) as 'QTY_WAIT',sum(case a.entitysts when 'R' then entitytqty else 0 end) as 'QTY_RUN',sum(case when (a.entitysts='W' or a.entitysts='R') then entitytqty else 0 end) as 'QTY_W+R',sum(case a.entitysts when 'H' then entitytqty else 0 end) as 'QTY_HOLD' from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[wltm] a left join [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem] b on a.prodid=b.prodid inner join [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mwcm] c on a.cworkctr=c.workctr and substring((case when a.cartid is null or a.cartid = null then '0' when a.cartid = '' then '1' else a.cartid end ),1,1)<> 'F' where 1=1 and a.Fabid='A' and a.hideflg='0'and a.prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem]) group by a.cworkctr,c.workname,b.prodtype,a.fabid,b.project_code,a.prodid,a.wonbr,a.lotgrade) F where 1=1   ");

            if (ddlFabid.Text.Trim() == "LCM1+LCM2")
            {
                stb.Append("");
            }
            else
            {
                stb.Append(" and fabid='" + ddlFabid.Text.Trim() + "'");
            }


            if (chkProjectCode.Checked)
            {
                stb.Append("");
            }
            else
            {
                foreach (ListItem item in lstProjectCode.Items)
                {
                    if (item.Selected)
                    {
                        s1 += "'" + item.Text.Trim() + "',";
                    }
                }

                if (s1 == "")
                {
                    stb.Append("");
                }
                else
                {
                    s1 = s1.Substring(0, s1.Length - 1);
                    stb.Append(" and f.project_code  in (" + s1 + ")");
                }
            }

            if (chkProdid.Checked)
            {
                stb.Append("");
            }
            else
            {
                foreach (ListItem item in lstProdid.Items)
                {
                    if (item.Selected)
                    {
                        s2 += "'" + item.Text.Trim() + "',";
                    }
                }

                if (s2 == "")
                {
                    stb.Append("");
                }
                else
                {
                    s2 = s2.Substring(0, s2.Length - 1);
                    stb.Append(" and f.prodid  in (" + s2 + ")");
                }
            }

            //if (chkWo.Checked)
            //{
            //    stb.Append("");
            //}
            //else
            //{
            foreach (ListItem item in lstWonbr.Items)
            {
                if (item.Selected)
                {
                    s3 += "'" + item.Text.Trim() + "',";
                }
            }

            if (s3 == "")
            {
                stb.Append("");
            }
            else
            {
                s3 = s3.Substring(0, s3.Length - 1);
                stb.Append(" and f.wonbr  in (" + s3 + ")");
            }

            if (chkGrade.Checked)
            {
                stb.Append("");
            }
            else
            {
                foreach (ListItem item in lstGrade.Items)
                {
                    if (item.Selected)
                    {
                        s4 += "'" + item.Text.Trim() + "',";
                    }
                }

                if (s4 == "")
                {
                    stb.Append("");
                }
                else
                {
                    s4 = s4.Substring(0, s4.Length - 1);
                    stb.Append(" and f.lotgrade  in (" + s4 + ")");
                }
            }

            if (chkStation.Checked)
            {
                stb.Append("");
            }
            else
            {
                foreach (ListItem item in lstStation.Items)
                {
                    if (item.Selected)
                    {
                        s5 += "'" + item.Text.Trim() + "',";
                    }
                }

                if (s5 == "")
                {
                    stb.Append("");
                }
                else
                {
                    s5 = s5.Substring(0, s5.Length - 1);
                    stb.Append(" and f.cworkctr  in (" + s5 + ")");
                }
            }

            stb.Append("  order by fabid,cworkctr,prodid");
            DataTable dtN90 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            this.GridView1.DataSource = dtN90;
            this.GridView1.DataBind();

            if (GridView1.Rows.Count == 0)
            {
                Label1.Visible = true;
                Label1.Text = "查無資料!";
            }
            else
            {
                Label1.Text = "";
            }
        }

        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataBound();
            ImageButton1.Visible = true;
        }

        //protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridView1.PageIndex = e.NewPageIndex;
        //    DataBound();
        //}

        protected void chkStation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStation.Checked)
            {
                foreach (ListItem i in lstStation.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstStation.Items)
                {
                    i.Selected = false;
                }
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
            this.GridView1.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
