using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class SubLineID : CollectionStandard
    {
        #region Private Parameters

        private string strFabID;
        private string strSubLineID = "";
        private string strWorkctr = "";

        #endregion

        public SubLineID()
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
        //strSubLineID
        public string STRSubLineID
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

        public string STRWorkctr
        {
            get
            {
                return this.strWorkctr;

            }
            set
            {
                this.strWorkctr = value;
            }

        }
        #endregion

        #region Monthed

    public DataSet GetM1M2M3SubLineID()
    {
        DataSet ds = new DataSet();
        try
        {
            string strSql=string.Empty;
            strSql="SELECT SUBLINEID FROM MSLM WHERE FABID='"+this.strFabID+"'";
            strSql+=" AND WORKCTR IN (SELECT WORKCTR FROM MWCM WHERE FABID='"+this.strFabID+"' AND WORKTYPE IN ('M1','M2','M3') )";

            ds=this.DBConn.GetDataSetBySql(strSql);
            return ds;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

        public DataSet GetSubLineID()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;

                strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID='" + this.strFabID + "'";
                if (this.strSubLineID.ToString().Trim() != "")
                {
                    strSql += " AND SUBLINEID='" + this.strSubLineID + "'";
                }

                ds = this.DBConn.GetDataSetBySql(strSql);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    public DataSet GetM1M2M3WorkCtr()
    {
        DataSet ds = new DataSet();
        try
        {
            string strSql = string.Empty;

            strSql = "SELECT WORKCTR FROM MWCM WHERE FABID='" + this.strFabID + "' AND WORKTYPE IN ('M1','M2','M3') ";
            if (this.strWorkctr.ToString().Trim() != "")
            {
                strSql += " AND WORKCTR='" + this.strWorkctr + "'";
            }

            ds = this.DBConn.GetDataSetBySql(strSql);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }      
        #endregion
    }


