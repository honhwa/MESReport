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
/// SubLines 的摘要描述
/// </summary>
public class SubLines : CollectionStandard
{

	public SubLines()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public SubLine this[int index]
    {
        get
        {
            return (index > -1 && this.Count > 0 && index < this.Count) ? (SubLine)this.List[index] : null;
        }
    }

    /// <summary>
    /// 通過對應不同的userControlID建立新的集合
    /// </summary>
    /// <param name="userControlID">userControlID</param>
    /// <returns></returns>
    public SubLines Create(string userControlID)
    {
        try
        {
            SubLines subs = new SubLines();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].UserControlID == userControlID)
                    subs.List.Add(this[i]);
            }
            return subs;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    /// <summary>
    /// 讀取SubLines資訊
    /// </summary>
    public void Read()
    {
        DataTable dt;
        string strSUBLINESTATUS = "";
        try
        {
            dt = this.FetchSubLines();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SubLine sl = new SubLine();
                sl.SetColor = dt.Rows[i]["EQCCDCOLOR"].ToString();
                sl.EqcCD = dt.Rows[i]["EQCCD"].ToString();
                sl.FabID = dt.Rows[i]["FABID"].ToString();
                //sl.Location = dt.Rows[i]["LOCATION"].ToString();
                sl.ShopCD = dt.Rows[i]["SHOPCD"].ToString();
                sl.SubLineID = dt.Rows[i]["SUBLINEID"].ToString();
                //sl.UserControlID = dt.Rows[i]["USERCONTROLID"].ToString();
                sl.WORKCTR = dt.Rows[i]["WORKCTR"].ToString();
                sl.TransDate = dt.Rows[i]["TRANSDATE"].ToString();
                strSUBLINESTATUS = "";
                strSUBLINESTATUS = dt.Rows[i]["SUBLINESTATUS"].ToString();
                sl.StringToolTip = StrFetchSubLineToolTip(dt.Rows[i]["FABID"].ToString(), dt.Rows[i]["SUBLINEID"].ToString(), dt.Rows[i]["EQCCD"].ToString(), strSUBLINESTATUS, dt.Rows[i]["SHOPCD"].ToString());
                this.List.Add(sl);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SubLine.Read:" + ex.Message);
        }   
    }

    /// <summary>
    /// 通過SUBLINEID取得當前SubLine
    /// </summary>
    /// <param name="subLineID">subLineID</param>
    /// <returns></returns>
    public SubLine GetSubLine(string subLineID)
    {
        int ndx = -1;
        string strMsg = "";
        try
        {
            if (this.Count == 0)
                this.Read();

            for (int i = 0; i < this.Count; i++ )
            {
                if (this[i].SubLineID == subLineID)
                {
                    ndx = i;
                    break;
                }
            }

            if (ndx == -1)
                return null;
            //strMsg = "當前SubLines集合中無SUBLINEID:" + subLineID + "的資料!";

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        if (strMsg != "")
            throw new ArgumentException(strMsg);

        return this[ndx];
    }

    /// <summary>
    /// 取得SubLines資料
    /// </summary>
    /// <returns></returns>
    private DataTable FetchSubLines()
    {

        string strRowFilter = "";
        string strSql = "";
        DataTable dt;

        try
        {
            //strRowFilter = (this.)
            strSql = " SELECT ES.FABID,ES.SUBLINEID,ES.SUBLINESTATUS,ES.SHOPCD,ES.WORKCTR,ES.EQCCD,ME.EQCCDCOLOR,ES.TRANSDATE " +
                        " FROM New_ESLS ES " +
                        " INNER JOIN MECCU ME " +
                        " ON ES.EQCCD = ME.EQCCD " +
                        " WHERE ES.FABID = 'B' " + strRowFilter + this.RowFilter + " ORDER BY  ES.SUBLINEID ";
            dt = this.DBConn.GetDataSetBySql(strSql, "New_ESLS").Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 取得SubLines資料
    /// </summary>
    /// <returns></returns>
    private string StrFetchSubLineToolTip(string strFabID,string strSubLineID,string strEQCCD,string strSUBLINESTATUS,string strShop)
    {

        string strSql = "";
        string strDownEQ = "";
        string strToolTip="";
        DataTable dt;
        DataTable dtDownEQ;

        try
        {
            strSql = @" SELECT EQCD+':'+EQCDDESC+'(部門:'+RTRIM(LTRIM(DEPART))+')' AS EQCDDESC,SHOPM FROM MECM WHERE FABID='" + strFabID.ToString() + "' AND EQCD='" + strSUBLINESTATUS + "' ";
            dt = this.DBConn.GetDataSetBySql(strSql, "MECM").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["SHOPM"].ToString().Trim() == strShop.ToString().Trim())
                {
                    strToolTip = dt.Rows[i]["EQCDDESC"].ToString().Trim();
                }
                if ((dt.Rows[i]["SHOPM"].ToString().Trim() =="ALL") && (strToolTip.ToString().Trim()=="") )
                {
                    strToolTip = dt.Rows[i]["EQCDDESC"].ToString().Trim();
                }
            }

            strSql = "SELECT EQID FROM EQSM WHERE FABID='" + strFabID.ToString() + "' AND SUBLINEID='" + strSubLineID.ToString() + "' AND EQCCD='D' GROUP BY EQID ORDER BY EQID";

            dtDownEQ = this.DBConn.GetDataSetBySql(strSql, "EQSM").Tables[0];

            for (int i = 0; i < dtDownEQ.Rows.Count; i++)
            {
                strDownEQ = strDownEQ + dtDownEQ.Rows[i]["EQID"].ToString().Trim();              
            }
            if (strDownEQ.ToString().Trim() != "")
            {
                strToolTip = strToolTip + " Down EQID:" + strDownEQ;        
            }
            return strToolTip.ToString().Trim();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public DataTable dtGetColor()
    {
        string strSql = "";
        DataTable dt;
        try
        {
            strSql = "SELECT  EQCCD,EQCCDESC,EQCCDCOLOR FROM MECCU ";
            dt = this.DBConn.GetDataSetBySql(strSql, "MECCU").Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
