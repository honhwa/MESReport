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
using Dundas.Charting.WebControl;
using Dundas.Charting;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using Report.Library;
using Report.Sources;
using System.IO;

namespace Report.Reports.MFG
{
    public partial class YaHeTest : System.Web.UI.Page
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

            strSql1 += " SELECT DISTINCT PRODID FROM New_YaheTest ORDER BY PRODID ASC";
            DataTable dt1 = this.dbConn.GetDataSetBySql(strSql1).Tables[0];
            this.ddlProdid.DataSource = dt1;
            this.ddlProdid.DataTextField = "prodid";
            this.ddlProdid.DataValueField = "prodid";
            this.ddlProdid.DataBind();
            ddlProdid.Items.Insert(0, new ListItem("-----All-----", "All"));

            strSql2 += " SELECT DISTINCT OperationID FROM New_YaheTest ORDER BY OperationID ASC";
            DataTable dt2 = this.dbConn.GetDataSetBySql(strSql2).Tables[0];
            this.ddlOperationID.Enabled = true;
            this.ddlOperationID.DataSource = dt2;
            this.ddlOperationID.DataTextField = "OperationID";
            this.ddlOperationID.DataValueField = "OperationID";
            this.ddlOperationID.DataBind();
            ddlOperationID.Items.Insert(0, new ListItem("-----All-----", "All"));


        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            if (getcheckS())
            {
                try
                {
                    DataTable dt = dbConn.GetDataSetBySql(strQuerySQL).Tables[0];
                    if (dt.Rows.Count >= 1)
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
            string strPanelID = "";
            for (int i = 0; i < strtxtLot.Length; i++)
            {
                strPanelID += "'" + strtxtLot[i] + "',";
            }
            strPanelID = strPanelID.TrimEnd(',');
            if (ddlOperationID.SelectedItem.ToString() == "-----All-----" && ddlProdid.SelectedItem.ToString() == "-----All-----" && txtLot.Text == "")
            {
                this.Response.Write("<script language=javascript>window.alert('查詢條件：機種&機台&PANELID不可同時為空！');</script>");
            }
            else
            {
                if (txtLot.Text != "")
                {
                    strQuerySQL += " SELECT PRODID,OperationID,DATATIME,PANELID,RESULT,FLG,Pressure,PressurePro,PressureTime,PressureTimePro \n" +
                               " FROM New_YaheTest \n" +
                               " WHERE  \n" +
                               " DATATIME BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'  \n" +
                                (ddlProdid.SelectedValue == "All" ? " AND PANELID IN (" + strPanelID + ") \n" : " AND (PANELID IN (" + strPanelID + ") AND PRODID='" + ddlProdid.SelectedValue + "')  \n") +
                                (rblResult.Items[0].Selected ? "" : rblResult.Items[1].Selected ? " AND RESULT='OK' \n" : " AND RESULT='NG' \n") +
                               " ORDER BY DATATIME DESC";
                }
                else
                {
                    strQuerySQL += " SELECT PRODID,OperationID,DATATIME,PANELID,RESULT,FLG,Pressure,PressurePro,PressureTime,PressureTimePro \n" +
                               " FROM New_YaheTest \n" +
                               " WHERE  \n" +
                                " DATATIME BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'  \n" +
                                (ddlProdid.SelectedValue == "All" ? "" : " AND PRODID = '" + ddlProdid.SelectedValue + "' \n") +
                                (ddlOperationID.SelectedValue == "All" ? "" : " AND OperationID= '" + ddlOperationID.SelectedValue + "' \n") +
                                (rblResult.Items[0].Selected ? "" : rblResult.Items[1].Selected ? " AND RESULT='OK' \n" : " AND RESULT='NG' \n") +
                                " ORDER BY DATATIME DESC";
                }

            }
            return true;
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Export("application/ms-excel", "YaHeTest.xls");
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
            Response.AppendHeader("Content-Disposition", "attachment;filename=YaHeTest.xls");
            Response.ContentType = FileType;
            this.EnableViewState = false;
            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            GridView1.RenderControl(hw);
            Response.Write(tw.ToString());
            Response.End();
            GridView1.AllowPaging = true;
        }

        private void BindGridView()
        {
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }
    }
}