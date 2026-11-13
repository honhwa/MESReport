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
/// Standard 的摘要描述
/// </summary>
public class Standard
{
    private string strEqcCD = "";
    private string strSubLineID = "";
    private string strWorkCtr = "";
    private string strShopCD = "";
    private string strFabid = "";
    private string strColor = "";
    private string strTransDate = "";
    private string strTooltip = "";

	public Standard()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    /// <summary>
    /// 取得或設定母狀態識別碼
    /// </summary>
    public string EqcCD
    {
        get
        {
            return this.strEqcCD;
        }
        set
        {
            this.strEqcCD = value;
        }
    }

    /// <summary>
    /// 取得或設定線別
    /// </summary>
    public string SubLineID
    {
        get
        {
            return this.strSubLineID;
        }
        set
        {
            this.strSubLineID = value;
        }
    }

    /// <summary>
    /// 取得或設定製程段
    /// </summary>
    public string ShopCD
    {
        get
        {
            return this.strShopCD;
        }
        set
        {
            this.strShopCD = value;
        }
    }

    /// <summary>
    /// 取得或設定站別
    /// </summary>
    public string WORKCTR
    {
        get
        {
            return this.strWorkCtr;
        }
        set
        {
            this.strWorkCtr = value;
        }
    }

    /// <summary>
    /// 取得或設定廠別代號
    /// </summary>
    public string FabID
    {
        get
        {
            return this.strFabid;
        }
        set
        {
            this.strFabid = value;
        }
    }

    /// <summary>
    /// 取得或設定狀態顏色
    /// </summary>
    public string SetColor
    {
        get
        {
            return this.strColor;
        }
        set
        {
            this.strColor = value;
        }
    }

    /// <summary>
    /// 作業時間
    /// </summary>
    public string TransDate
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

    /// <summary>
    /// 設定提示資料
    /// </summary>
    public string StringToolTip
    {

        get
        {
            return this.strTooltip;
        }
        set
        {
            this.strTooltip = value;

        }

    }
}
