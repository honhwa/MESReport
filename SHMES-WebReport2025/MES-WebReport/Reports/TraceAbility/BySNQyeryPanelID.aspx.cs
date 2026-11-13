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
using System.Text;
/*
 * 2014-04-30  Innolux/Li.Miao V1.01      Update 由查WLTM表變更為查詢VIEW的方式（VWLTMA）

 * 2014-10-08  Innolux/Leo     V1.02      增加多行查詢
*/

namespace WebReport.Reports
{
    public partial class By_Lot_Qyery_Cartid : System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        protected void butQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
             //if (txtEntityId.Text.Trim().Replace("'", "") == "")
            bool Err = false;
            this.LblErr.Visible =false;
            DataTable dt = new DataTable();
            StringBuilder stb = new StringBuilder();
            ReportViewer1.Visible = true;
            if (txtEntityId.Text==string.Empty)
            {
                 Err = true;
                //this.Response.Write("<script language=javascript>window.alert('請先輸入SN再查詢，謝謝！');</script>");
                 this.LblErr.Visible = true;
                this.LblErr.Text = "請先輸入SN再查詢，謝謝！";
                ReportViewer1.Visible = false;
            }

            if (!Err)
            {
                /*V1.01 Begin*/
                //stb.Append("  select A.WONBR AS MLOTID,A.MLOTID AS LOTID,A.ENTITYID AS PANELID,A.LOTGRADE,A.LOTLEVEL,A.DEFECTCD,B.SN,B.DATATIME From WLTM A,fileparm B where B.SN ='" + this.txtEntityId.Text.Trim().Replace("'", "") + "'" + " AND A.ENTITYID=B.LOTID");
                //stb.Append("  select A.WONBR AS MLOTID,A.MLOTID AS LOTID,A.ENTITYID AS PANELID,A.LOTGRADE,A.LOTLEVEL,A.DEFECTCD,B.SN,B.DATATIME From VWLTMA A,fileparm B where B.SN ='" + this.txtEntityId.Text.Trim().Replace("'", "") + "'" + " AND A.ENTITYID=B.LOTID");
                /*V1.02 BEGIN*/
                //stb.Append("  select B.LOTID AS PANELID,B.SN,B.DATATIME From fileparm B where B.SN ='" + this.txtEntityId.Text.Trim().Replace("'", "") + "' ");
                /*V1.01 End*/
                string strEntityId = txtEntityId.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                stb.Append("  select B.LOTID AS PANELID,B.SN,B.DATATIME From New_VfileparmA B where  ");
                stb.AppendFormat(" B.SN IN ('{0}')", strEntityId);
                stb.Append(" union  select B.panelid AS PANELID,B.customersn as SN,B.printtime From New_Vtbl_pansonicsnA B where  ");
                stb.AppendFormat(" B.CUSTOMERSN IN ('{0}')", strEntityId);
                stb.Append(" union  select B.panelid AS PANELID,B.customersn as SN,B.printtime From New_Vtbl_audisnA B where  ");
                stb.AppendFormat(" B.CUSTOMERSN IN ('{0}')", strEntityId);
                stb.Append(" union  select B.panelid AS PANELID,B.serialnumber as SN,B.DATATIME From New_Vfileparm_AUDIA B where   ");
                stb.AppendFormat(" B.SERIALNUMBER IN ('{0}')", strEntityId);
                stb.Append(" ORDER BY SN,PANELID,DATATIME");
                /*V1.02 END*/

                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
            }
            
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;

                localReportRoqcs.ReportPath = @"RDLC\SNQueryPanelID.rdlc";


                ReportDataSource rsd = new ReportDataSource("SNQueryPanelID", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
         
            bool Err = false;
            this.LblErr.Visible = false;
            DataTable dt = new DataTable();
            StringBuilder stb = new StringBuilder();
            ReportViewer1.Visible = true;
            if (this.txtPanelID.Text == string.Empty)
            {
                Err = true;               
                this.LblErr.Visible = true;
                this.LblErr.Text = "請先輸入PanelID再查詢，謝謝！";
                ReportViewer1.Visible = false;
            }

            if (!Err)
            {

                string strEntityId = txtPanelID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                stb.Append("  select B.LOTID AS PANELID,B.SN,B.DATATIME From New_VfileparmA B where  ");
                stb.AppendFormat(" B.LOTID IN ('{0}')", strEntityId);
                stb.Append(" union  select B.panelid AS PANELID,B.customersn as SN,B.printtime From New_Vtbl_pansonicsnA B where   ");
                stb.AppendFormat(" B.PANELID IN ('{0}')", strEntityId);

                stb.AppendFormat(" union  select isnull(B.panelid,c.panelid) AS PANELID,isnull(B.serialnumber,c.customersn) as SN,isnull(B.DATATIME,c.printtime) From New_Vfileparm_AUDIA B LEFT JOIN New_VTBL_AUDISNA C ON  C.PANELID IN('{0}')  ", strEntityId);
                stb.AppendFormat(" where  B.PANELID IN ('{0}')", strEntityId);
                stb.Append(" ORDER BY PANELID,DATATIME,SN" );

                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
            }

            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;

            localReportRoqcs.ReportPath = @"RDLC\SNQueryPanelID.rdlc";


            ReportDataSource rsd = new ReportDataSource("SNQueryPanelID", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
        }
    }
}
