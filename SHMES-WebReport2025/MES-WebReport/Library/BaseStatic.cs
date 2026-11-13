using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// BaseStatic 的摘要描述
/// </summary>
public class BaseStatic
{
    public static string strSubLine = "";

	public BaseStatic()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public static string Leach(string Leachcontent)
    {
        string str = Leachcontent.Trim();
        str = str.TrimStart(' ');
        str = str.Replace("<", "&lt;");
        str = str.Replace(">", "&gt;");
        str = str.Replace("&nbsp;", "   ");
        str = str.Replace("   ", "&nbsp;");
        return str;
    }
}
