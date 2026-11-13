using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
/* 
 * 2014-09-12  Innolux\Runy  V1.01   Add EDC Report
 */
public partial class _MainButton : System.Web.UI.Page 
{
    AppClass Apc = new AppClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Login"] == null)
        {
            tbFABID.Text = "";
            tbFABName.Text = "";
            Page.Response.Redirect(@"~\Default.aspx");
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

                Apc.GetLoginInfo(out strLoginPCName, out strLoginPCIP, out strLoginNTName);

                tbLoginNTName.Text = strLoginNTName;
                tbLoginPCIP.Text = strLoginPCIP;
                tbLoginPCName.Text = strLoginPCName;

                LogRecord();
            }
            else
            {
                Page.Response.Redirect(@"~\Default.aspx");
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
        Label1.Text = Apc.QueryLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", "Browse", "/YMS/Default.aspx");
        LinkButton7.Visible = true;

        LinkButton1.Text = tbFABName.Text + " " + LinkButton1.Text;
        LinkButton1.PostBackUrl = "~/YMSReport/Cell2FPY.aspx?FABID=" + tbFABID.Text + "";

        LinkButton2.Text = tbFABName.Text + " " + LinkButton2.Text;
        LinkButton2.PostBackUrl = "~/YMSReport/ModuleFPY.aspx?FABID=" + tbFABID.Text + "";
        
        LinkButton3.Text = tbFABName.Text + " " + LinkButton3.Text;
        LinkButton3.PostBackUrl = "~/YMSReport/Cell2AllLightOn.aspx?FABID=" + tbFABID.Text + "";

        LinkButton4.Text = tbFABName.Text + " " + LinkButton4.Text;
        LinkButton4.PostBackUrl = "~/YMSReport/ModuleAllLightOn.aspx?FABID=" + tbFABID.Text + "";

        LinkButton5.Text = tbFABName.Text + " " + LinkButton5.Text;
        LinkButton5.PostBackUrl = "~/YMSReport/Cell2AllLightOnDaily.aspx?FABID=" + tbFABID.Text + "";

        LinkButton6.Text = tbFABName.Text + " " + LinkButton6.Text;
        LinkButton6.PostBackUrl = "~/YMSReport/OverAllFPY.aspx?FABID=" + tbFABID.Text + "";

        LinkButton8.Text = tbFABName.Text + " " + LinkButton8.Text;
        LinkButton8.PostBackUrl = "~/YMSReport/ByBUFPY.aspx?FABID=" + tbFABID.Text + "";

        LinkButton9.Text = tbFABName.Text + " " + LinkButton9.Text;
        LinkButton9.PostBackUrl = "~/YMSReport/InputOutpuDefect.aspx?FABID=" + tbFABID.Text + "";

        LinkButton10.Text = tbFABName.Text + " " + LinkButton10.Text;
        LinkButton10.PostBackUrl = "~/YMSDailyReport/Cell2DailyYiedReport.aspx?FABID=" + tbFABID.Text + "";

        LinkButton11.Text = tbFABName.Text + " " + LinkButton11.Text;
        LinkButton11.PostBackUrl = "~/YMSDailyReport/ModuleDailyYiedReport.aspx?FABID=" + tbFABID.Text + "";

        LinkButton12.Text = tbFABName.Text + " " + LinkButton12.Text;
        LinkButton12.PostBackUrl = "~/YMSReport/EDC.aspx?FABID=" + tbFABID.Text + "";   //V1.01 Add EDC
    }
    protected void LinkButton7_Click(object sender, EventArgs e)
    {
        Session["Login"] = null;
        tbFABID.Text = "";
        tbFABName.Text = "";
        Page.Response.Redirect(@"~\Default.aspx");
    }
   
}
