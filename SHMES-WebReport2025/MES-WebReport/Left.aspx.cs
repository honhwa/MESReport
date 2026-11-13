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

namespace WebReport
{
    public partial class Left : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //20211012 penny : fix session timeout throw exception 
            if (!this.IsPostBack)
            {
                if (Session["UserInfo"] == null)
                {
                    Session["IsLogin"] = "N";
                    Response.Write("<script language='javascript'>window.top.location='Login.aspx';</script>");
                }
                else
                {
                    this.lblIp.Text = Request.UserHostAddress.ToString();
                    this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
                }
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            //Response.Write("<script>window.opener=null;window.close('Index.aspx');</script>");
            //Response.Write("<script>window.open("~/Login.aspx",'','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no');</script>");

            //Response.Redirect("~/Login.aspx");
            Response.Write("<script language='javascript'>window.top.location='Login.aspx';</script>");
            //Response.Redirect(@"~\Login.aspx");
        }


    }
}
