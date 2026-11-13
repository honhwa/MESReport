using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Report.Library
{
    public class RecordLog
    {
        #region Parameters
        private DBConn dbConn;
        #endregion
                /// </summary>
        public RecordLog()
        {
            this.dbConn = new DBConn();
        }

        public bool InsertLog(string strFabID, string strLogType, string strRPTName, string strUserID,string strIP)
        {
            string strSql = "INSERT INTO RPTRECORDLOG(FABID,LOGDATA,LOGTYPE,RPTName,USERID,LOGIP,OPT1,OPT2,OPT3) ";
            strSql = strSql + " VALUES('" + strFabID + "',GETDATE(),'" + strLogType + "','" + strRPTName + "','" + strUserID + "','" + strIP + "','','','')";
            if (dbConn.ExecuteSql(strSql)==false)
            {
                return false;
            }
            return true;
        }
    }
}
