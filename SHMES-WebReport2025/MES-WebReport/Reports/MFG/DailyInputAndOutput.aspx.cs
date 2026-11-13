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

namespace WebReport.Reports
{
    public partial class DailyInputAndOutput : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect(@"~\Login.aspx");
                }
                //this.lblIp.Text = "IP: " + Request.UserHostAddress.ToString();
                //this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
                this.BindGroup();
                this.BindProd();
                //this.BindSubLine();
            }
        }

        protected void cmbProdId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }


        /// <summary>
        /// 綁定群組
        /// </summary>
        private void BindGroup()
        {
            string strSql = "";
            try
            {
                strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID = 'B' ";
                DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.DlGroup.DataSource = null;
                    return;
                }
                this.DlGroup.DataSource = dt;
                this.DlGroup.DataTextField = "PRODGROUP";
                this.DlGroup.DataValueField = "PRODGROUP";
                this.DlGroup.DataBind();
                this.DlGroup.Items.Insert(0, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 綁定產品號碼
        /// </summary>
        private void BindProd()
        {
            string strSql = "";
            try
            {
                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODID FROM MPIM WHERE FABID = 'B' ";

                DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.DlProdID.DataSource = null;
                    return;
                }
                this.DlProdID.DataSource = dt;
                this.DlProdID.DataTextField = "PRODID";
                this.DlProdID.DataValueField = "PRODID";
                this.DlProdID.DataBind();
                this.DlProdID.Items.Insert(0, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 綁定Subline
        /// </summary>
        //private void BindSubLine()
        //{
        //    string strSql = "";
        //    try
        //    {
        //        strSql = " SELECT SUBLINEID FROM MSLM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
        //        //strSql = " SELECT SUBLINEID FROM MSLM WHERE FABID = 'B' ";
        //        DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
        //        if (dt.Rows.Count == 0)
        //        {
        //            this.DLSubLine.DataSource = null;
        //            return;
        //        }
        //        this.DLSubLine.DataSource = dt;
        //        this.DLSubLine.DataTextField = "SUBLINEID";
        //        this.DLSubLine.DataValueField = "SUBLINEID";
        //        this.DLSubLine.DataBind();
        //        this.DLSubLine.Items.Insert(0, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// [查詢]事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSerch_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                BaseDailyInputAndOutput bDao = new BaseDailyInputAndOutput();
                bDao.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                //bDao.FabID = "B";
                bDao.Year = this.dlYear.Text;
                bDao.ReportMonth = this.dlMonth.Text;
                bDao.ProdID = this.DlProdID.Text;
                bDao.ProdGroup = this.DlGroup.Text;
                //bDao.SubLineID = this.DLSubLine.Text;
                ds = bDao.Read();

                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReport = this.ReportViewer1.LocalReport;
                //localReport.ReportPath = "DailyInputAndOutput2.rdlc";
                localReport.ReportPath = @"RDLC\DailyInputAndOutput.rdlc";

                ReportDataSource rdSource = new ReportDataSource();
                rdSource.Name = "localReport";
                rdSource.Value = ds.Tables[0];
                localReport.DataSources.Clear();
                localReport.DataSources.Add(rdSource);
                //localReport.DataSources.Add(rdSource);


    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
