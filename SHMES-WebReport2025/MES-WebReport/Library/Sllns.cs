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
/// Sllns 的摘要描述
/// </summary>
public class Sllns : CollectionStandard
{
	public Sllns()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public bool Add(Slln sl)
    {
        string strSql = "";
        string strMsg = "";

        try
        {
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
    }

    
}
