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
using System.Text;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports
{
    public partial class ByCartidQLot : System.Web.UI.Page
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

        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder stb = new StringBuilder();
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            this.LblErr.Text = "";
            ReportViewer1.Visible = true;
            if (TxtCartId.Text.Trim() != string.Empty)
            {
                try
                {
                    stb.Append("SELECT transdate,cartid,toprodid prodid,toWONBR WONBR,toentityid  entityid,'8380LotID' as opt1,  userid,CUSTOMERID FROM New_VWLTXA");
                    stb.Append(" where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
                    stb.Append(" AND VERB='PKCI' AND CARTID ='" + TxtCartId.Text.Trim() + "'");
                    //stb.Append(" union  ");
                    //stb.Append("SELECT transdate,cartid,prodid,WONBR,entityid,  case when entitysts='C' AND CWORKCTR='8800' then '8800LotID' else '8380LotID' end as opt1,  userid,CUSTOMERID FROM WLTMH");
                    //stb.Append(" where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
                    //stb.Append(" AND HIDEFLG IN('0','1') AND CARTID ='" + TxtCartId.Text.Trim() + "'");
                    dt = dbcon.GetDataSetBySql(stb.ToString()).Tables[0];

                    if (dt.Rows.Count == 0)
                    {
                        this.LblErr.Text = "查無資料!!";
                        ReportViewer1.Visible = false;
                    }
                    else
                    {
                        this.Label2.Visible = true;
                        this.Label1.Visible = true;
                        this.Label1.Text = "8380LotID:8380站成箱的批號";
                        this.Label2.Text = "8800LotID:8800入賬後的批號";
                        this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;

                        localReportRoqcs.ReportPath = @"RDLC\ByCartidQLot.rdlc";


                        ReportDataSource rsd = new ReportDataSource("ByCartidQLot", dt);
                        localReportRoqcs.DataSources.Clear();
                        localReportRoqcs.DataSources.Add(rsd);
                    }
                    
                }
                catch (Exception ex )
                {
                    throw ex;
                }
            }
            else
            {
                this.LblErr.Text = "請輸入箱號後查詢!";
                ReportViewer1.Visible = false;
            }
        }
    }
}
