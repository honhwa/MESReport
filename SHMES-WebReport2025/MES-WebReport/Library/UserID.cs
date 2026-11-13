using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class UserID : CollectionStandard
{

      #region Private Parameters

        private string strFabID;
        private string strUserID = "";

        #endregion


    public UserID()
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
        public string UserId
        {

            get
            {
                return this.strUserID;
            }
            set
            {
                this.strUserID = value;
            }
        }
        #endregion

        #region Monthed
        public DataSet GetUserID()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;

                strSql = "SELECT USERID,USERID+USERNAME AS USERNAME FROM MEMM WHERE FABID='" + this.strFabID + "'";
                if (this.strUserID.ToString().Trim() != "")
                {
                    strSql += " AND USERID='" + this.strUserID + "'";
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

