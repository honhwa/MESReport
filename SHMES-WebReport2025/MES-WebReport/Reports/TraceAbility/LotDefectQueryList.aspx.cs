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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Text;
using Report.Sources;
using System.Data.SqlClient;

namespace WebReport.Reports
{
    public partial class LotDefectQueryList : System.Web.UI.Page
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
                //this.txtFromDate.Text = "";
                //this.txtToDate.Text = "";
                this.TxtLotID.Text = "";
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strErrorMsg = "";
            this.lblErr.Text = "";
            StringBuilder stb = new StringBuilder();
            DataTable dt = new DataTable();
            bool Err = false;
            stb.Append("SELECT CONVERT(VARCHAR(19),TRANSDATE,120) AS transdate,CWORKCTR,CSUBLINEID,ENTITYID,MLOTID,LOTGRADE,LOTLEVEL,DEFECTCD,PRODID,WONBR,LOTTYPE,SHIFTID,CSHOPCD,SUM(DEFECTQTY)DEFECTQTY ");
            stb.Append("FROM New_VWLDXA ");
            stb.Append(" where 1=1");

            
            if (TxtLotID.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "LotID為空！\n";
                Err = true;
            }
            else
            {
                stb.Append(" and (ENTITYID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "'");
                stb.Append(" or MLOTID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "')");
            }

            if (Err)
            {
                this.lblErr.Text = strErrorMsg;
            }
            else
            {
                stb.Append("GROUP BY TRANSDATE,CWORKCTR,CSUBLINEID,ENTITYID,MLOTID,LOTGRADE,LOTLEVEL,DEFECTCD,PRODID,WONBR,LOTTYPE,SHIFTID,CSHOPCD");
                dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    this.lblErr.Text = strErrorMsg;
                }
            }
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\LotDefectQueryList.rdlc";


            ReportDataSource rsd = new ReportDataSource("LotDefectQueryList_VWLDXA", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
            
        }
    }
}


