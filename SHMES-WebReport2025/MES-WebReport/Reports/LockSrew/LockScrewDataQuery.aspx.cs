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
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.IO;

namespace WebReport.Reports.LockScrew
{
    public partial class LockScrewDataQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        //private string strSql1 = "";
        //private string strSql2 = "";
        //private string strQuerySQL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                txtStartDT.Text = System.DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                TxtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                DDL_DataBind();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }
        private void DDL_DataBind()
        {
            string strSql1 = $@" SELECT ProdId,ProdId + ' ' + ProdDesc As ProdDesc from mpim order by ProdGroup asc, ProdId asc";
            DataTable dt1 = this.dbConn.GetDataSetBySql(strSql1).Tables[0];
            this.ddlProdid.DataSource = dt1;
            this.ddlProdid.DataTextField = "ProdDesc";
            this.ddlProdid.DataValueField = "ProdId";
            this.ddlProdid.DataBind();
            ddlProdid.Items.Insert(0, new ListItem("-----All-----", "All"));

            string strSql2 = $@" SELECT distinct MachineID FROM dbo.MPCTM order by MachineID ASC";
            DataTable dt2 = this.dbConn.GetDataSetBySql(strSql2).Tables[0];
            this.ddlMachineID.Enabled = true;
            this.ddlMachineID.DataSource = dt2;
            this.ddlMachineID.DataTextField = "MachineID";
            this.ddlMachineID.DataValueField = "MachineID";
            this.ddlMachineID.DataBind();
            ddlMachineID.Items.Insert(0, new ListItem("-----All-----", "All"));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            GridView1.Visible = false;
            string strQuerySQL = getcheckS();
            if (!string.IsNullOrEmpty(strQuerySQL))
            {
                try
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "查詢中......";
                    DataTable dt = dbConn.GetDataSetBySql(strQuerySQL).Tables[0];
                    Session["dsData"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    if (dt.Rows.Count >= 1)
                    {
                        //Session["dsData"] = dt;
                        //GridView1.DataSource = dt;
                        //GridView1.DataBind();
                        GridView1.Visible = true;
                        //DataView dv = dt.DefaultView;
                        int recordCount = dt.Rows.Count;
                        lblMsg.Text = "查詢成功!記錄數：" + recordCount.ToString() + "條";
                        btnExport.Enabled = true;
                    }
                    else
                        lblMsg.Text = "查無資料!";
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                }
            }
        }

        private bool IsDate(string strDate)
        {
            try { DateTime.Parse(strDate); return true; }
            catch { return false; }
        }

        private string getcheckS()
        {
            try
            {
                string strQuerySQL = string.Empty;
                string strTimeStart = txtStartDT.Text + " 00:00:00";
                if (!IsDate(strTimeStart)) throw new Exception("開始時間格式不正確!");
                string strTimeEnd = TxtEndDT.Text + " 23:59:59";
                if (!IsDate(strTimeEnd)) throw new Exception("結束時間格式不正確!");

                int day = Convert.ToDateTime(strTimeEnd).Subtract(Convert.ToDateTime(strTimeStart)).Days;
                if(day <= 0) throw new Exception("開始時間不能大於等於結束時間!");
                if (day > 60) throw new Exception("開始時間結束時間跨度不能大於60天!");

                string strPanelID = string.Empty;
                string strProdId = string.Empty;
                string strMachineId = string.Empty;
                string strWhereSql = string.Empty;
                string strResult = string.Empty;

                if (!string.IsNullOrEmpty(txtLot.Text))
                {
                    string strlotTemp = txtLot.Text.Replace("\r\n", ",");
                    string[] strtxtLot = strlotTemp.Split(',');
                    for (int i = 0; i < strtxtLot.Length; i++)
                    {
                        strPanelID += "'" + strtxtLot[i] + "',";
                    }
                    strWhereSql += $" and REPLACE(REPLACE(RTRIM(LTRIM(CODE)),CHAR(10),''),CHAR(13),'') in ({strPanelID.TrimEnd(',')})";
                }

                if (!"ALL".Equals(ddlMachineID.SelectedValue, StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(ddlMachineID.SelectedValue))
                {
                    strWhereSql += $" And MachineId = '{ddlMachineID.SelectedValue}'";
                }
                if (!"ALL".Equals(ddlProdid.SelectedValue, StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(ddlProdid.SelectedValue))
                {
                    strWhereSql += $" And ProdId ='{ddlProdid.SelectedValue}'";
                }

                if (rblResult.Items[1].Selected || rblResult.Items[2].Selected)
                    strWhereSql += $" And Result = '{(rblResult.Items[1].Selected == true ? "OK" : "NG")}'";
                else
                {
                    if (string.IsNullOrEmpty(strWhereSql))
                        throw new Exception("查詢條件機台編號/產品型號/玻璃Id不能都為空!");
                }

                strQuerySQL = $@" Select MACHINEID
    ,PRODID
    ,REPLACE(REPLACE(RTRIM(LTRIM(CODE)),CHAR(10),''),CHAR(13),'') AS PANELID
    ,COUNT
    ,CONVERT(VARCHAR(30),TIME,121) AS TRANSNDATE
    ,PROF
    ,MEAS
    ,TIMEMEAS
    ,TIMEPROF
    ,ISNULL(ANGLE,'') AS ANGLE
    ,ISNULL(ANGLEPROF,'') AS ANGLEPROF
    ,ISNULL(SPINDLE,'') AS SPINDLE
    ,ISNULL(RESULT,'') AS RESULT
    ,ISNULL(A2,'') AS A2
    ,ISNULL(TorqueUSL,'') AS TorqueUSL
    ,ISNULL(TorqueLSL,'') AS TorqueLSL
    ,ISNULL(ANGLEUSL,'') AS ANGLEUSL
    ,ISNULL(ANGLELSL,'') AS ANGLELSL
    ,ISNULL(A2USL,'') AS A2USL
    ,ISNULL(A2LSL,'') AS A2LSL
FROM (
    Select SUBSTRING(Replace(NAME,'-','_'),0,CHARINDEX('_',Replace(NAME,'-','_'),0)) AS MACHINEID,
            SUBSTRING(Replace(NAME,'-','_'),CHARINDEX('_',Replace(NAME,'-','_'),0) + 1,LEN(Replace(NAME,'-','_'))) AS PRODID,
            * From New_symbol with(nolock)
        Where Time Between '{strTimeStart}' And '{strTimeEnd}'
        And CHARINDEX('_',Replace(NAME,'-','_')) > 0
        And ISNULL(Code,'') <> ''
    UNION ALL
    Select SUBSTRING(Replace(NAME,'-','_'),0,CHARINDEX('_',Replace(NAME,'-','_'),0)) AS MACHINEID,
            SUBSTRING(Replace(NAME,'-','_'),CHARINDEX('_',Replace(NAME,'-','_'),0) + 1,LEN(Replace(NAME,'-','_'))) AS PRODID,
            *  From symbolh with(nolock)
        Where Time Between '{strTimeStart}' And '{strTimeEnd}'
        And CHARINDEX('_',Replace(NAME,'-','_')) > 0 
        And ISNULL(Code,'') <> ''
    ) A
where 1 = 1 
{strWhereSql}
ORDER BY A.NUM ASC;";
                return strQuerySQL;
            }
            catch(Exception ex)
            {
                this.Response.Write($"<script language=javascript>window.alert('{ex.Message}');</script>");
                return null;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Export("application/ms-excel", "LockScrewData.xls");
            }
            catch
            {
                if (Session["UserInfo"] == null)
                {
                    Response.Write("<script language='JavaScript'>window.alert('登錄超時！');window.location.href='../../Login.aspx'</script>");
                }
            }
        }

        private void Export(string FileType, string FileName)
        {
            //GridView1.AllowPaging = false;
            //BindGridView();
            //Response.Charset = "GB2312";
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.AppendHeader("Content-Disposition", "attachment;filename=LockScrewData.xls");
            //Response.ContentType = FileType;
            //this.EnableViewState = false;
            //StringWriter tw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(tw);
            //GridView1.RenderControl(hw);
            //Response.Write(tw.ToString());
            //Response.End();
            //GridView1.AllowPaging = true;

            // set the resulting file attachment name to the name of the report...
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            GridView1.AllowPaging = false;
            BindGridView();
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < GridView1.Columns.Count; k++)
            {
                //add separator
                sb.Append(GridView1.Columns[k].HeaderText + ',');
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                for (int k = 0; k < GridView1.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(GridView1.Rows[i].Cells[k].Text.ToString() + ',');
                }
                //append new line
                sb.Append("\r\n");
            }
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            GridView1.AllowPaging = true;
        }

        private void BindGridView()
        {
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            txtStartDT.Text = System.DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            TxtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtLot.Text = "";
            lblMsg.Text = "";
            btnExport.Enabled = false;
            DDL_DataBind();
            GridView1.Visible = false;
        }
    }
}