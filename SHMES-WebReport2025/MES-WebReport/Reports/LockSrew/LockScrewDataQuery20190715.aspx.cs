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
    public partial class LockScrewDataQuery20190715 : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        private string strSql1 = "";
        private string strSql2 = "";
        private string strQuerySQL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                txtStartDT.Text = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
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
            strSql1 += " SELECT DISTINCT SUBSTRING(NAME,CHARINDEX('_',NAME,0) + 1,13) AS PRODID INTO #TEMP1 \n" +
                            " FROM New_symbol \n" +
                            " WHERE \n" +
                            " NAME LIKE '%*_%' escape '*' \n";
            strSql1 += " SELECT PRODID FROM #TEMP1 ORDER BY PRODID ASC";                
            DataTable dt1 = this.dbConn.GetDataSetBySql(strSql1).Tables[0];
            this.ddlProdid.DataSource = dt1;
            this.ddlProdid.DataTextField = "prodid";
            this.ddlProdid.DataValueField = "prodid";
            this.ddlProdid.DataBind();
            ddlProdid.Items.Insert(0, new ListItem("-----All-----", "All"));

            strSql2 += " SELECT DISTINCT SUBSTRING(NAME,1,CHARINDEX('_',NAME,0)-1) AS MACHINEID INTO #TEMP2 \n" +
                            " FROM New_symbol \n" +
                            " WHERE \n" +
                            " NAME LIKE '%*_%' escape '*' \n";
            strSql2 += " SELECT MACHINEID FROM #TEMP2 ORDER BY MACHINEID ASC";
            DataTable dt2 = this.dbConn.GetDataSetBySql(strSql2).Tables[0];
            this.ddlMachineID.Enabled = true;
            this.ddlMachineID.DataSource = dt2;
            this.ddlMachineID.DataTextField = "machineid";
            this.ddlMachineID.DataValueField = "machineid";
            this.ddlMachineID.DataBind();
            ddlMachineID.Items.Insert(0, new ListItem("-----All-----", "All"));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            if (getcheckS())
            {
                try
                {
                    DataTable dt = dbConn.GetDataSetBySql(strQuerySQL).Tables[0];
                    if (dt.Rows.Count > 1)
                    {
                        Session["dsData"] = dt;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        GridView1.Visible = true;
                        DataView dv = dt.DefaultView;
                        int recordCount = dv.Count;
                        lblMsg.Text = "查詢成功!記錄數：" + recordCount.ToString() + "條";
                        lblMsg.Visible = true;
                        btnExport.Enabled = true;
                    }
                    else
                        this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
                    
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                }
            }
        }

        private bool getcheckS()
        {
            string strTimeStart = txtStartDT.Text + " 00:00:00";
            string strTimeENd = TxtEndDT.Text + " 23:59:59";
            try
            {
                DateTime dt1 = Convert.ToDateTime(strTimeStart);
                DateTime dt2 = Convert.ToDateTime(strTimeENd);
            }
            catch 
            {
                throw new Exception("時間格式不正確！");
            }
            string strlotTemp = txtLot.Text.Replace("\r\n", ",");
            string[] strtxtLot = strlotTemp.Split(',');
            //string strPanelID = "''";
            string strPanelID = "";
            for (int i = 0; i < strtxtLot.Length; i++)
            {
                //strPanelID += ",'" + strtxtLot[i] + "'";
                strPanelID += "'" + strtxtLot[i] + "',";
            }
            strPanelID = strPanelID.TrimEnd(',');
            if (ddlProdid.SelectedItem.ToString() == "-----All-----" && ddlMachineID.SelectedItem.ToString() == "-----All-----" && txtLot.Text == "")
            {
                this.Response.Write("<script language=javascript>window.alert('查詢條件：機種&機台&PANELID不可同時為空！');</script>");
            }
            else
            {
                if (txtLot.Text != "")
                {
                    strQuerySQL += " SELECT SUBSTRING(NAME,1,CHARINDEX('_',NAME,0)-1) AS MACHINEID,SUBSTRING(NAME,CHARINDEX('_',NAME,0) + 1,13) AS PRODID,CODE AS PANELID,COUNT,TIME,MEAS,PROF,TIMEMEAS,TIMEPROF,RESULT \n" +
                                " FROM New_symbol \n" +
                                " WHERE  \n" +
                                " TIME BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'  \n" +
                                 (ddlProdid.SelectedValue == "All" ? " AND CODE IN (" + strPanelID + ") \n" : " AND (CODE IN (" + strPanelID + ") AND SUBSTRING(NAME,CHARINDEX('_',NAME,0) + 1,13)='" + ddlProdid.SelectedValue + "')  \n") +
                                " ORDER BY TIME DESC";
                }
                else
                {
                    strQuerySQL += " SELECT SUBSTRING(NAME,1,CHARINDEX('_',NAME,0)-1) AS MACHINEID,SUBSTRING(NAME,CHARINDEX('_',NAME,0) + 1,13) AS PRODID,CODE AS PANELID,COUNT,TIME,MEAS,PROF,TIMEMEAS,TIMEPROF,RESULT \n" +
                                " FROM New_symbol \n" +
                                " WHERE  \n" +
                                " TIME BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'  \n" +
                                (ddlProdid.SelectedValue == "All" ? "" : " AND [NAME] LIKE '%" + ddlProdid.SelectedValue + "' \n") +
                                (ddlMachineID.SelectedValue == "All" ? "" : " AND [NAME] LIKE '" + ddlMachineID.SelectedValue + "%' \n") +
                                (rblResult.Items[0].Selected ? "" : rblResult.Items[1].Selected ? " AND RESULT='OK' \n" : " AND RESULT='NG' \n") +
                                " ORDER BY TIME DESC";
                }
            }
            return true;
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
            GridView1.AllowPaging = false;
            BindGridView();
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=LockScrewData.xls");
            Response.ContentType = FileType;
            this.EnableViewState = false;
            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            GridView1.RenderControl(hw);
            Response.Write(tw.ToString());
            Response.End();
            GridView1.AllowPaging = true;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        /// <summary>
        /// 僅導出時用

        /// </summary>
        private void BindGridView()
        {
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDT.Text = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            TxtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtLot.Text = "";
            lblMsg.Text = "";
            btnExport.Enabled = false;
            DDL_DataBind();
            GridView1.Visible = false;
        }
    }
}