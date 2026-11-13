using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using Report.Library;

/// <summary>
/// CollectionStandard 的摘要描述
/// </summary>
public class CollectionStandard : CollectionBase
{
    private string strRowFilter = "";
    //private DataPool.DataPool pool = new DataPool.DataPool(DBConn.StrDBConn(), "SQLSERVER");
    private DBConn dbconn = new DBConn();

	public CollectionStandard()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    /// <summary>
    /// 取得或設定查詢的條件
    /// </summary>
    public string RowFilter
    {
        get
        {
            return this.strRowFilter;
        }
        set
        {
            this.strRowFilter = value;
        }
    }

    /// <summary>
    /// 取得DB連接類 
    /// </summary>
    protected DBConn DBConn
    {
        get
        {
            return this.dbconn;
        }
        set
        {
            this.dbconn = value;
        }
    }

    ///// <summary>
    ///// 取得系統連接Pool
    ///// </summary>
    //protected DataPool.DataPool DataPool
    //{
    //    get
    //    {
    //        return this.pool;
    //    }
    //}





}
