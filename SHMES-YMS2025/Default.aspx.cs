using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page 
{
    AppClass Apc = new AppClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Page.Title = Apc.GetVersion().ToString();
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

            tbFABID.Text = "";
            tbFABName.Text = "";

            Session["Login"] = null;

            Label2.Text = "注意:頁面超過" + Session.Timeout.ToString() + "分鐘無操作,系統會自動登出";

            GetSubject();
            //GetVersion();

            rblFabID.Items[0].Selected = true;
            Choose();
        }
    }
    protected void GetSubject()
    {
        /**************************************************************************************************************************/
        string strTitle = "";
        string strCon = "";
        string strDateTime = "";

        if (Apc.QuerySubject("Default", "Default", out strTitle, out strCon, out strDateTime) == true)
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
    protected void GetVersion()
    {
        string userAgent = Request.UserAgent == null ? "無" : Request.UserAgent;
    
        string osVersion = "未知";

        if (userAgent.Contains("NT 6.0"))
        {
            osVersion = "Windows Vista/Server 2008";
        }
        else if (userAgent.Contains("NT 5.2"))
        {
            osVersion = "Windows Server 2003";
        }
        else if (userAgent.Contains("NT 5.1"))
        {
            osVersion = "Windows XP";
        }
        else if (userAgent.Contains("NT 5"))
        {
            osVersion = "Windows 2000";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('當前操作系統版本(" + osVersion + ")版本較低,會影響部份功能使用,請聯繫IT升級版本');</script>");
        }
        else if (userAgent.Contains("NT 4"))
        {
            osVersion = "Windows NT4";
        }
        else if (userAgent.Contains("Me"))
        {
            osVersion = "Windows Me";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('當前操作系統版本(" + osVersion + ")版本較低,會影響部份功能使用,請聯繫IT升級版本');</script>");
        }
        else if (userAgent.Contains("98"))
        {
            osVersion = "Windows 98";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('當前操作系統版本(" + osVersion + ")版本較低,會影響部份功能使用,請聯繫IT升級版本');</script>");
        }
        else if (userAgent.Contains("95"))
        {
            osVersion = "Windows 95";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('當前操作系統版本(" + osVersion + ")版本較低,會影響部份功能使用,請聯繫IT升級版本');</script>");
        }
        else if (userAgent.Contains("Mac"))
        {
            osVersion = "Mac";
        }
        else if (userAgent.Contains("Unix"))
        {
            osVersion = "UNIX";
        }
        else if (userAgent.Contains("Linux"))
        {
            osVersion = "Linux";
        }
        else if (userAgent.Contains("SunOS"))
        {
            osVersion = "SunOS";
        }
    }
    //protected void Page_UnLoad(object sender, EventArgs e)
    //{
    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('開始時間不能大於結束時間！');</script>");
    //    return;
    //}


    protected void LogRecord()
    {
        Session["Login"] = tbFABID.Text.ToString();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Browse", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Choose FabID", tbFABName.Text.ToString(), "");
    }
    protected void rblFabID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Choose();
    }
    private void Choose()
    {
        if (rblFabID.Items[0].Selected == true)
        {
            tbFABName.Text = "SH";
            tbFABID.Text = "B";
        }
        LogRecord();
        this.Response.Redirect("~/MainButton.aspx?FABID=" + tbFABID.Text + "");
    }
}
