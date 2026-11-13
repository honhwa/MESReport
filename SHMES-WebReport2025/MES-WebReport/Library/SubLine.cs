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
/// SubLine 的摘要描述
/// </summary>
public class SubLine : Standard
{
    private string strUserControlid = "";
    private string strLocation = "";

	public SubLine()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    /// <summary>
    /// 取得或設定UserControlID
    /// </summary>
    public string UserControlID
    {
        get
        {
            return this.strUserControlid;
        }
        set
        {
            this.strUserControlid = value;
        }
    }

    /// <summary>
    /// 取得或設定UserControl中的位置
    /// </summary>
    public string Location
    {
        get
        {
            return this.strLocation;
        }
        set
        {
            this.strLocation = value;
        }
    }


}
