using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class TeamID :CollectionStandard
{

      #region Private Parameters

        private string strFabID;
        private string strTeamID = "";

        #endregion


        public TeamID()
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
        public string Teamid
        {

            get
            {
                return this.strTeamID;
            }
            set
            {
                this.strTeamID = value;
            }
        }
        #endregion

        #region Monthed
        public DataSet GetTeamID()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;

                strSql = "SELECT TEAMID FROM MTEAM WHERE FABID='" + this.strFabID + "'";
                if (this.strTeamID.ToString().Trim() != "")
                {
                    strSql += " AND TEAMID='" + this.strTeamID + "'";
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

