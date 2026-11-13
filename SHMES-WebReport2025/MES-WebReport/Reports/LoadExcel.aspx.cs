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

namespace WebReport.Reports
{
    public partial class LoadExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }

				this.lblTitle.Text=this.Request.QueryString["ExcelName"];
                this.Response.Redirect("../ExportFile/"+this.lblTitle.Text.ToString().Trim()+".xls");
                

            }

        }
    }
}
