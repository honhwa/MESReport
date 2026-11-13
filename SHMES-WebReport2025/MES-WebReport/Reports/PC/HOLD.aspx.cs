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
using Microsoft.Reporting.WebForms;
using Report.Library;
using System.Text;
using System.Data.SqlClient;

namespace WebReport.Reports
{
    public partial class HOLD : System.Web.UI.Page
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
                this.SetProdid();
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

        private void SetProdid()
        {
            this.lstProd.Items.Clear();
            string strSql = @"SELECT PRODID FROM MPIM WHERE FABID= '" +((UserInfo)Session["UserInfo"]).FabId+"' GROUP BY PRODID ORDER BY PRODID DESC";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstProd.DataSource = dt;
            this.lstProd.DataTextField = "PRODID";
            this.lstProd.DataValueField = "PRODID";
            this.lstProd.DataBind();
            this.lstProd.ClearSelection();
        }


       protected void chkAllProd_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllProd.Checked)
            {
                foreach (ListItem i in this.lstProd.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstProd.Items)
                {
                    i.Selected = false;
                }
            }

        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
                        Report.Sources.HOLD hold = new Report.Sources.HOLD();
                             
                                    foreach (ListItem i in this.lstProd.Items)
                        {
                            if (i.Selected)
                            {
                                hold.ListProdID.Add(i.Value);
                            }
                        }

                        this.ReportViewer.ProcessingMode = ProcessingMode.Local;
                        LocalReport localReport = this.ReportViewer.LocalReport;
                        localReport.ReportPath = @"RDLC\HOLD.rdlc";

                        DataSet dataset = hold.GetDataSet();
                        ReportDataSource dsHOLD= new ReportDataSource();
                        dsHOLD.Name = "HOLD_HOLD";
                        dsHOLD.Value = dataset.Tables[0];
                        localReport.DataSources.Clear();
                        localReport.DataSources.Add(dsHOLD);
                        this.ReportViewer.LocalReport.Refresh();
         
       
      }
   }
}
