using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class ProdID : CollectionStandard
{
    #region Private Parameters

    private string strFabID;
    private string strProdGroup="";

    #endregion

    public ProdID()
    {

    }

    #region Properties
    // Fab ID
    public string FabID
    {
        get
        {
            return this.strFabID;
        }
        set
        {
            this.strFabID = value;
        }
    }
    //ProdGroup
    public string ProdGroup
    {

        get
        {
            return this.strProdGroup;
        }
        set
        {
            this.strProdGroup=value;
        }
    }
    #endregion

    #region Monthed
    public DataSet GetProdID()
        {
            DataSet ds=new DataSet();
            try
            {
                string strSql =string.Empty;

                strSql = "SELECT PRODID FROM MPIM WHERE FABID='" + this.strFabID + "'";
                if (this.strProdGroup.ToString().Trim() !="")
                {
                    strSql+=" AND PRODGROUP='"+this.strProdGroup+"'";
                }

                ds=this.DBConn.GetDataSetBySql(strSql);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }
        #endregion
    public DataSet GetProdGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            string strSql = string.Empty;

            strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID='" + this.strFabID + "'";
            if (this.strProdGroup.ToString().Trim() != "")
            {
                strSql += " AND PRODGROUP='" + this.strProdGroup + "'";
            }

            ds = this.DBConn.GetDataSetBySql(strSql);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}
