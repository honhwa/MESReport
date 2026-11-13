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
using System.Data.SqlClient;
using System.Text;
using Microsoft.Reporting.WebForms;
using Report.Library;
using Report.Sources;
using System.Linq;

namespace WebReport.Reports
{
    public partial class LotTraceQuery : System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
            }
        }

        protected void ButQ_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            ReportViewer1.Visible = true;
            if (txtLot.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的箱號！');</script>");
                ReportViewer1.Visible = false;
            }
            else
            {   
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                StringBuilder stb = new StringBuilder();
                string lot;
                lot = txtLot.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                lot = "'" + lot + "'";
                if (lot.Count(c => c == ',') + 1 > 100)
                {
                    this.Response.Write("<script language=javascript>window.alert('查詢Id不能超過100，請確認！');</script>");
                }

                stb.Append(" select a.verb,a.TransDate,a.TrackInDate,a.mlotid, a.PTrackOutDate,a.PWorkCtr,a.CWorkCtr,a.NWorkCtr,a.FrProdID,a.ToProdID, a.FrWoNbr,a.ToWoNbr,");
                stb.Append("a.FrEntityID,a.ToEntityID,a.MLotID,a.FrPEntityUnit,a.ToPEntityUnit,a.FrEntityUnit,a.ToEntityUnit, a.EntityCarry,a.CEntityTQty,a.Nentitytqty,");
                stb.Append("a.FrWoType,a.ToWoType,a.FrLotType,a.ToLotType,a.FrLotGrade,a.ToLotGrade,");
                stb.Append("a.FrLotLevel,a.ToLotLevel,a.CSUBLineID, a.NSUBLineID,a.userid,a.ShiftID,a.CustomerID,");
                stb.Append("a.SALEORDER,a.TmpFIELD  from ( select distinct * from ( ");
                stb.Append("select * from New_wltx WITH(NOLOCK) where FABID ='" + ((UserInfo)Session["UserInfo"]).FabId + "' and (frentityid in(" + lot + ")) ");
                stb.Append("union all ");
                stb.Append("select * from New_wltx WITH(NOLOCK) where FABID ='" + ((UserInfo)Session["UserInfo"]).FabId + "' and (toentityid in(" + lot + ")) ");
                stb.Append(") b) a ");
                stb.Append(" where  FABID = 'B' order by TransId");

                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];

                stb.Clear();
                stb.Append(" select a.verb,a.TransDate,a.TrackInDate,a.mlotid, a.PTrackOutDate,a.PWorkCtr,a.CWorkCtr,a.NWorkCtr,a.FrProdID,a.ToProdID, a.FrWoNbr,a.ToWoNbr,");
                stb.Append("a.FrEntityID,a.ToEntityID,a.MLotID,a.FrPEntityUnit,a.ToPEntityUnit,a.FrEntityUnit,a.ToEntityUnit, a.EntityCarry,a.CEntityTQty,a.Nentitytqty,");
                stb.Append("a.FrWoType,a.ToWoType,a.FrLotType,a.ToLotType,a.FrLotGrade,a.ToLotGrade,");
                stb.Append("a.FrLotLevel,a.ToLotLevel,a.CSUBLineID, a.NSUBLineID,a.userid,a.ShiftID,a.CustomerID,");
                stb.Append("a.SALEORDER,a.TmpFIELD  from ( select distinct * from ( ");
                stb.Append("select * from New_wltxh WITH(NOLOCK) where FABID ='" + ((UserInfo)Session["UserInfo"]).FabId + "' and (frentityid in(" + lot + ")) ");
                stb.Append("union all ");
                stb.Append("select * from New_wltxh WITH(NOLOCK) where FABID ='" + ((UserInfo)Session["UserInfo"]).FabId + "' and (toentityid in(" + lot + ")) ");
                stb.Append(") b) a ");
                stb.Append(" where  FABID = 'B' order by TransId");

                dt1 = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];

                dt.Merge(dt1);
                if (dt.Rows.Count < 1)
                {
                    this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
                }
                ReportViewer1.Visible = true;
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\LotTraceQuery.rdlc";


                ReportDataSource rsd = new ReportDataSource("LotTraceQuery", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
                
            }
        }
    }
}
