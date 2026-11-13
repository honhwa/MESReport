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
/// Slln 的摘要描述
/// </summary>
public class Slln
{
    private string strUserControlID = "";
    private string strLoaction = "";

	public Slln()
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
            return this.strUserControlID;
        }
        set
        {
            this.strUserControlID = value;
        }
    }

    /// <summary>
    /// 取得或設定UserControl中的位置
    /// </summary>
    public string Location
    {
        get
        {
            return this.strLoaction;
        }
        set
        {
            this.strLoaction = value;
        }
    }
}
