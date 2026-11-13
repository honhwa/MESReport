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
    public partial class OnlineDefectSummary : System.Web.UI.Page
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
                this.txtFromDate.Text = "";
                this.txtToDate.Text = "";
                this.TxtLotID.Text = "";
                SetDropDownList_Grade();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        private void SetDropDownList_Grade()
        {
            string strSql = @"SELECT WORKCTR,workctr+'   '+workname AS workctrdesc
                              FROM MWCM
                              WHERE WORKCTR IS NOT NULL 
                              ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.DropDownList1.DataSource = dt;
            this.DropDownList1.DataTextField = "workctrdesc";
            this.DropDownList1.DataValueField = "WORKCTR";
            this.DropDownList1.DataBind();
            this.DropDownList1.Items.Insert(0, "ALL");
            this.DropDownList1.ClearSelection();
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
            stb.Append("SELECT CONVERT(VARCHAR(19),vwldxa.TRANSDATE,120) AS transdate,vwldxa.CWORKCTR,vwldxa.CSUBLINEID,vwldxa.ENTITYID,vwldxa.MLOTID,vwldxa.LOTGRADE,vwldxa.LOTLEVEL,vwldxa.DEFECTCD,vwldxa.PRODID,vwldxa.WONBR,vwldxa.LOTTYPE,vwldxa.SHIFTID,vwldxa.CSHOPCD,SUM(vwldxa.DEFECTQTY)DEFECTQTY ");
            stb.Append("FROM New_VWLDXA vwldxa,New_wltm wltm ");
            stb.Append(" where vwldxa.entityid=wltm.entityid and vwldxa.prodid=wltm.prodid and wltm.hideflg=0");

            if (txtFromDate.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                Err = true;
            }
            else
            {
                stb.Append(" and CONVERT(VARCHAR(10),vwldxa.TRANSDATE,120)>='" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
            }

            if (txtToDate.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                Err = true;
            }
            else
            {
                stb.Append(" and CONVERT(VARCHAR(10),vwldxa.TRANSDATE,120)<='" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
            }



            if (TxtLotID.Text.ToString() != "")
            {
                stb.Append(" and vwldxa.ENTITYID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "'");
            }




            if (this.DropDownList1.Text.ToString() != "ALL")
            {
                stb.Append(" and vwldxa.CWORKCTR='" + this.DropDownList1.Text.Trim().Replace("'", "") + "'");
            }


            if (Err)
            {
                this.lblErr.Text = strErrorMsg;
            }
            else
            {
                stb.Append("GROUP BY vwldxa.TRANSDATE,vwldxa.CWORKCTR,vwldxa.CSUBLINEID,vwldxa.ENTITYID,vwldxa.MLOTID,vwldxa.LOTGRADE,vwldxa.LOTLEVEL,vwldxa.DEFECTCD,vwldxa.PRODID,vwldxa.WONBR,vwldxa.LOTTYPE,vwldxa.SHIFTID,vwldxa.CSHOPCD");
                dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    this.lblErr.Text = strErrorMsg;
                }
            }
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\OnlineDefectSummary.rdlc";


            ReportDataSource rsd = new ReportDataSource("OnlineDefectSummary_VWLDXA", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
        }
    }
}


