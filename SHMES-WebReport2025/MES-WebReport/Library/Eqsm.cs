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
/// Eqsm 的摘要描述
/// </summary>
public class Eqsm : Standard
{
    private string strEqID = "";
    private string strTransDate = "";

    

    /// <summary>
    /// 取得或設定設備編號
    /// </summary>
    public string EqID
    {
        get
        {
            return this.strEqID;
        }
        set
        {
            this.strEqID = value;
        }
    }

    /// <summary>
    /// 取得或設定作業時間
    /// </summary>
    new  public string TransDate
    {
        get
        {
            return this.strTransDate;
        }
        set
        {
            this.strTransDate = value;
        }
    }

	public Eqsm()
	{
		
	}




}
