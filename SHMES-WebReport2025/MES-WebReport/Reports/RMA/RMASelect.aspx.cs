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

namespace WebReport.Reports.RMA
{
    public partial class RMASelect : System.Web.UI.Page
    {
        private string strQuerySQL = "";
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.GridView1.PageSize = 10;
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }

                txtStartDT.Text = System.DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                TxtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                btnExport.Enabled = false;
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
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
            //string strSN = txtLot.Text;
            string strSNTemp = txtLot.Text.Replace("\r\n", ",");
            string[] strtxtSN = strSNTemp.Split(',');
            string strSN = "";
            for (int i = 0; i < strtxtSN.Length; i++)
            {
                strSN += "'" + strtxtSN[i] + "',";

            }
            strSN = strSN.TrimEnd(',');
            string strProdTemp = txtProd.Text.Replace("\r\n", ",");
            string[] strtxtProd = strProdTemp.Split(',');
            string strProduct = "";
            for (int i = 0; i < strtxtProd.Length; i++)
            {
                strProduct += "'" + strtxtProd[i] + "',";

            }
            strProduct = strProduct.TrimEnd(',');
            //string strProduct = txtProd.Text;
            if ( txtLot.Text == "" && txtProd.Text=="")
            {
                strQuerySQL = @"SELECT TransDate as 復判日期,PANEL_ID as 復判系統掃描PanelID,SERIAL_NO as 復判系統掃描SN,CMO_SERIAL_NO as SN,
                                AppliP1 as Appli,RMA_NUMBER as RMANO,[Model Name],PRODUCT_ID as ProductID,CUSTOMER as Customer,SI,CUSTOMER_SYMPTOM_CODEP1 AS Defectcode,
                                CUSTOMER_SYMPTOMP1 AS Symptom,UserID AS 復判人員,[Decision screenP2] AS 判定畫面,[Mura NDP1] as MuraND,
                                DiameterP1 AS 點類異物直徑Dmm,LengthP1 AS 線類異物長度Lmm,WidthP1 AS 線類異物寬度Wmm,[9Grid PositionP1] as Defect位置坐標Or九宮格,
                                [Defect NumberP1] as Defect顆數Or條數,Result as 判定結果 FROM RMA_Rejudge WHERE TransDate BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND Rejudge = '1' ORDER BY TransDate DESC";
            }
            else if (txtLot.Text!= "" && txtProd.Text == "")
            {
                strQuerySQL = @"SELECT TransDate as 復判日期,PANEL_ID as 復判系統掃描PanelID,SERIAL_NO as 復判系統掃描SN,CMO_SERIAL_NO as SN,
                                AppliP1 as Appli,RMA_NUMBER as RMANO,[Model Name],PRODUCT_ID as ProductID,CUSTOMER as Customer,SI,CUSTOMER_SYMPTOM_CODEP1 AS Defectcode,
                                CUSTOMER_SYMPTOMP1 AS Symptom,UserID AS 復判人員,[Decision screenP2] AS 判定畫面,[Mura NDP1] as MuraND,
                                DiameterP1 AS 點類異物直徑Dmm,LengthP1 AS 線類異物長度Lmm,WidthP1 AS 線類異物寬度Wmm,[9Grid PositionP1] as Defect位置坐標Or九宮格,
                                [Defect NumberP1] as Defect顆數Or條數,Result as 判定結果 FROM RMA_Rejudge WHERE TransDate BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND PANEL_ID in (" + strSN + ")or SERIAL_NO in (" + strSN + ") or CMO_SERIAL_NO in (" + strSN + ") AND Rejudge = '1'  ORDER BY TransDate DESC ";
            }
            else if (txtLot.Text == "" && txtProd.Text != "")
            {
                strQuerySQL = @"SELECT TransDate as 復判日期,PANEL_ID as 復判系統掃描PanelID,SERIAL_NO as 復判系統掃描SN,CMO_SERIAL_NO as SN,
                                AppliP1 as Appli,RMA_NUMBER as RMANO,[Model Name],PRODUCT_ID as ProductID,CUSTOMER as Customer,SI,CUSTOMER_SYMPTOM_CODEP1 AS Defectcode,
                                CUSTOMER_SYMPTOMP1 AS Symptom,UserID AS 復判人員,[Decision screenP2] AS 判定畫面,[Mura NDP1] as MuraND,
                                DiameterP1 AS 點類異物直徑Dmm,LengthP1 AS 線類異物長度Lmm,WidthP1 AS 線類異物寬度Wmm,[9Grid PositionP1] as Defect位置坐標Or九宮格,
                                [Defect NumberP1] as Defect顆數Or條數,Result as 判定結果 FROM RMA_Rejudge WHERE TransDate BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND PRODUCT_ID IN (" + strProduct + ") AND Rejudge = '1'  ORDER BY TransDate DESC ";
            }
            else if (txtLot.Text != "" && txtProd.Text != "")
            {
                strQuerySQL = @"SELECT TransDate as 復判日期,PANEL_ID as 復判系統掃描PanelID,SERIAL_NO as 復判系統掃描SN,CMO_SERIAL_NO as SN,
                                AppliP1 as Appli,RMA_NUMBER as RMANO,[Model Name],PRODUCT_ID as ProductID,CUSTOMER as Customer,SI,CUSTOMER_SYMPTOM_CODEP1 AS Defectcode,
                                CUSTOMER_SYMPTOMP1 AS Symptom,UserID AS 復判人員,[Decision screenP2] AS 判定畫面,[Mura NDP1] as MuraND,
                                DiameterP1 AS 點類異物直徑Dmm,LengthP1 AS 線類異物長度Lmm,WidthP1 AS 線類異物寬度Wmm,[9Grid PositionP1] as Defect位置坐標Or九宮格,
                                [Defect NumberP1] as Defect顆數Or條數,Result as 判定結果 FROM RMA_Rejudge WHERE TransDate BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND PANEL_ID IN (" + strSN + ") or SERIAL_NO in (" + strSN + ") or CMO_SERIAL_NO IN (" + strSN + ") AND PRODUCT_ID IN (" + strProduct + ") AND  Rejudge = '1'  ORDER BY TransDate DESC";
            }
            return true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDT.Text = System.DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            TxtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtLot.Text = "";
            lblMsg.Text = "";
            btnExport.Enabled = false;
            GridView1.Visible = false;
        }

        //protected void btnExport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Export("application/ms-excel", "ClientData.xls");
        //    }
        //    catch
        //    {
        //        if (Session["UserInfo"] == null)
        //        {
        //            Response.Write("<script language='JavaScript'>window.alert('登錄超時！');window.location.href='../../Login.aspx'</script>");
        //        }
        //    }
        //}
        protected void btnExport_Click(object sender, EventArgs e)
        {
            GridView1.AllowPaging = false;
            DataTable table = new DataTable();
            BindGridView();
            table = ExcelIO.ExcelHelper.GridView2DataTable(GridView1);
            string filename = "RMA復判資料.xls";
            ExcelIO.ExcelHelper.ExportByWeb(table, "", filename);
            GridView1.AllowPaging = true;
        }
        //private void Export(string FileType, string FileName)
        //{
        //    GridView1.AllowPaging= false;
        //    BindGridView();
        //    Response.Charset = "GB2312";
        //    Response.ContentEncoding = System.Text.Encoding.UTF8;
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=RMA.xls");
        //    Response.ContentType = FileType;
        //    this.EnableViewState = false;
        //    StringWriter tw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(tw);
        //    GridView1.RenderControl(hw);
        //    Response.Write(tw.ToString());
        //    Response.End();
        //    GridView1.AllowPaging = true;
        //}

        private void BindGridView()
        {
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGridView();
 
        }
    }
}