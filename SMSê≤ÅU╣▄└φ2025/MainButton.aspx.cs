using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MainButton : System.Web.UI.Page
{

    ModuleClass Apc = new ModuleClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Login"] == null)
        {
            tbFABID.Text = "";
            tbFABName.Text = "";
            Page.Response.Redirect(@"~\ShowMessage.aspx");
            return;
        }
        if (!Page.IsPostBack)
        {
            Page.Title = Apc.GetVersion().ToString();
            if (Request.QueryString["FABID"] != null)
            {
                tbFABID.Text = this.Request.QueryString["FABID"].ToString();
                if (tbFABID.Text == "B")
                {
                    tbFABName.Text = "SH";
                }

                string strLoginPCName = "";
                string strLoginPCIP = "";
                string strLoginNTName = "";
                tbLoginNTName.Text = "";
                tbLoginPCIP.Text = "";
                tbLoginPCName.Text = "";

                Apc.GetLoginInfo(out strLoginNTName, out strLoginPCIP, out strLoginPCName);

                tbLoginNTName.Text = strLoginNTName;
                tbLoginPCIP.Text = strLoginPCIP;
                tbLoginPCName.Text = strLoginPCName;

                LogRecord();
            }
            else
            {
                Page.Response.Redirect(@"~\ShowMessage.aspx");
            }
            Label2.Text = "注意:頁面超過" + Session.Timeout.ToString() + "分鐘無操作,系統會自動登出";
            GetSubject();
        }
    }
    protected void GetSubject()
    {
        /**************************************************************************************************************************/
        string strTitle = "";
        string strCon = "";
        string strDateTime = "";

        if (Apc.QuerySubject("MainButton", "MainButton", out strTitle, out strCon, out strDateTime) == true)
        {
            lblTitle.Text = strTitle.ToString();
            tbCon.Text = strCon.ToString();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>RunFunc();</script>");
        }
        else
        {
            lblTitle.Text = "";
            tbCon.Text = "";
            strDateTime = "";
        }
        /**************************************************************************************************************************/
    }
    protected void LogRecord()
    {
        //Label1.Text = Apc.QueryLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", "Browse", HttpContext.Current.Request.Url.AbsolutePath.ToString());
        Label1.Text = Apc.QueryLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", "Browse", "/SMS/ShowMessage.aspx");
        LinkButton7.Visible = true;

        HyperLink1.Text = tbFABName.Text + " " + HyperLink1.Text;
        HyperLink1.NavigateUrl = "~/Material/RPT_EIS.aspx";

        HyperLink2.Text = tbFABName.Text + " " + HyperLink2.Text;
        HyperLink2.NavigateUrl = "~/Material/ScrapRateQuery.aspx";

        HyperLink3.Text = tbFABName.Text + " " + HyperLink3.Text;
        HyperLink3.NavigateUrl = "~/Material/ScrapRateSRQuery.aspx";

        HyperLink4.Text = tbFABName.Text + " " + HyperLink4.Text;
        HyperLink4.NavigateUrl = "~/Material/ScrapRateSRTrendQuery.aspx";

        HyperLink5.Text = tbFABName.Text + " " + HyperLink5.Text;
        HyperLink5.NavigateUrl = "~/Material/ScrapRate.aspx?name=PEPlan";

        HyperLink6.Text = tbFABName.Text + " " + HyperLink6.Text;
        HyperLink6.NavigateUrl = "~/Material/ScrapRate.aspx?name=Cost";

        LinkButton13.Text = tbFABName.Text + " " + LinkButton13.Text;
        LinkButton13.PostBackUrl = "~/Material/EDC.aspx?FABID=" + tbFABID.Text + "";   //V1.01 Add EDC
    }
    protected void LinkButton7_Click(object sender, EventArgs e)
    {
        Session["Login"] = null;
        tbFABID.Text = "";
        tbFABName.Text = "";
        Page.Response.Redirect(@"~\ShowMessage.aspx");
    }

}
