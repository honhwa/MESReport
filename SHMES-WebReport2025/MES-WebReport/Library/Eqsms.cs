using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using DataPool;

/// <summary>
/// Eqsms 的摘要描述
/// </summary>
public class Eqsms : CollectionStandard
{
    private string strSubLineID = "";

    /// <summary>
    /// SubLineID
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

    public Eqsms()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    public Eqsm this[int index]
    {
        get
        {
            return (index > -1 && this.Count > 0 && index < this.Count) ? (Eqsm)this.List[index] : null;
        }
    }

    /// <summary>
    /// 讀取(EQSM設備狀態檔)資訊
    /// </summary>
    public void Read()
    {
        DataTable dt;
        try
        {
            if (this.Count > 0)
                this.Clear();

            dt = this.FetchEqsm();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Eqsm eq = new Eqsm();
                eq.EqcCD = dt.Rows[i]["EQCCD"].ToString();
                eq.EqID = dt.Rows[i]["EQID"].ToString();
                eq.FabID = dt.Rows[i]["FABID"].ToString();
                eq.ShopCD = dt.Rows[i]["SHOPCD"].ToString();
                eq.SubLineID = dt.Rows[i]["SUBLINEID"].ToString();
                eq.TransDate = dt.Rows[i]["TransDate"].ToString();
                eq.WORKCTR = dt.Rows[i]["WORKCTR"].ToString();
                eq.SetColor = dt.Rows[i]["EQCCDCOLOR"].ToString();
                this.List.Add(eq);
            }

        }
        catch (Exception ex)
        {
            throw new Exception("EQSM.Read:" + ex.Message);
        }
    }

    /// <summary>
    /// 通過EQID取得目前集合中EQSM資料
    /// </summary>
    /// <param name="eqid">eqid</param>
    /// <returns></returns>
    public Eqsm GetEqsm(string eqid)
    {
        int ndx = -1;
        string strMsg = "";
        try
        {
            if (this.Count == 0)
                this.Read();

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].EqID == eqid)
                {
                    ndx = i;
                    break;
                }
            }

            if (ndx == -1)

                strMsg = "當前EQSM集合中無EQID:" + eqid + "的資料!";
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
    /// 取得Eqsm
    /// </summary>
    /// <returns></returns>
    private DataTable FetchEqsm()
    {
        try
        {
            string strFilter = "";
            DataTable dt = new DataTable();
            string strSql = "";

            strFilter = (this.SubLineID == "") ? " AND SUBLINEID = ''  " : " AND SUBLINEID = '" + this.SubLineID + "' ";
            strSql = " SELECT  EQ.FABID, EQ.EQID, EQ.SUBLINEID, EQ.WORKCTR, EQ.SHOPCD, EQ.EQCCD ,EQ.TransDate, EQ.USERID,EQ.DEPTID, EQ.SHIFTID ,ME.EQCCDCOLOR "+
                         " FROM  EQSM EQ INNER JOIN MECCU ME ON EQ.EQCCD = ME.EQCCD WHERE FABID='B' " + strFilter + this.RowFilter;
            dt = this.DBConn.GetDataSetBySql(strSql, "EQSM").Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
