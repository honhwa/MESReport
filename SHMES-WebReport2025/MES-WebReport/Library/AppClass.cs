using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Report.Library
{
    public class AppClass
    {
        public AppClass()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
        public string GetVersion()
        {
            string strVersion = "YMS(良率管理系統) V1.04";
            return strVersion;
        }
        public Boolean QuerySubject(string strFunctionID, string strType, out string strTitle, out string strCon, out string strDateTime)
        {
            bool strReturn = false;
            try
            {
                StringBuilder strSQL = new StringBuilder();
                strSQL.Append(" SELECT FunctionID,FuncType,Title,Con,DateTime From Information");
                strSQL.Append(" WHERE FunctionID = '" + strFunctionID + "' AND FuncType = '" + strType + "' AND DateTime > Now() - MQty/1440  ");
                //strSQL.Append(" WHERE FunctionID = '" + strFunctionID + "' AND FuncType = '" + strType + "' ");
                string strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Request.PhysicalApplicationPath + "Information.mdb";
                OleDbConnection conn = new OleDbConnection(strConn);
                OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL.ToString(), strConn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Information");
                conn.Close();
                if (ds.Tables[0].Rows.Count == 1)
                {
                    strTitle = ds.Tables[0].Rows[0][2].ToString();
                    strCon = ds.Tables[0].Rows[0][3].ToString();
                    strDateTime = ds.Tables[0].Rows[0][4].ToString();
                    strReturn = true;
                }
                else
                {
                    strTitle = "";
                    strCon = "";
                    strDateTime = "";
                    strReturn = false;
                }
            }
            catch
            {
                strTitle = "";
                strCon = "";
                strDateTime = "";
                strReturn = false;
            }
            return strReturn;
        }
        public void GetLoginInfo(out string strLoginPCName, out string strLoginPCIP, out string strLoginNTName)
        {
            try
            {
                /*IIS設置的時候要將匿名驗證方式去掉*/
                string Auth_users;
                string[] logon;
                Auth_users = System.Web.HttpContext.Current.Request.ServerVariables["Auth_user"].ToString();
                logon = Auth_users.Split('\\');
                strLoginNTName = logon[1].Trim();

                strLoginPCName = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress).HostName.Split('.')[0].ToString();
                strLoginPCIP = HttpContext.Current.Request.UserHostAddress.ToString();
            }
            catch
            {
                strLoginPCName = HttpContext.Current.Request.UserHostAddress;
                strLoginPCIP = HttpContext.Current.Request.UserHostAddress.ToString();
                strLoginNTName = "";
            }
        }
        public void RecordLogInfo(string strFID, string strLogUserName, string strLogUserType, string strSSID, string LogRPTType,
                                    string strRPTName, string strLPCName, string strLPCIP, string strOPT1, string strOPT2, string strOPT3)
        {

            if (strSSID.Trim().ToString() == "")
            {
                return;
            }
            StringBuilder strSQL = new StringBuilder();
            DBConn dbconn = new DBConn();
            strSQL.Append(" UPDATE [RPTUSELOGM] SET  ");
            strSQL.Append(" SESSIONID = '" + strSSID + "', ");
            strSQL.Append(" LOGDATE=GETDATE(), ");
            strSQL.Append(" DAYLOGCOUNT=CASE WHEN LOGDATE>=CAST((CONVERT(VARCHAR(10),GETDATE(),120)+' 00:00:00') AS DATETIME) THEN DAYLOGCOUNT+1 ELSE 1 END, ");
            strSQL.Append(" LOGTTLCOUNT=LOGTTLCOUNT+1, ");
            strSQL.Append(" LOGPCNAME='" + strLPCName + "', ");
            strSQL.Append(" LOGPCIP='" + strLPCIP + "', ");
            strSQL.Append(" OPT1='" + strOPT1 + "', ");
            strSQL.Append(" OPT2='" + strOPT2 + "', ");
            strSQL.Append(" OPT3='" + strOPT3 + "' ");
            strSQL.Append(" WHERE FABID = '" + strFID + "'  ");
            strSQL.Append(" AND LOGUSERNAME = '" + strLogUserName + "'  ");
            strSQL.Append(" AND LOGUSERTYPE = '" + strLogUserType + "' ");
            strSQL.Append(" AND RPTNAME = '" + strRPTName + "'  ");
            strSQL.Append(" AND LOGRPTTYPE = '" + LogRPTType + "' ");
            strSQL.Append(" IF @@RowCount = 0 ");
            strSQL.Append(" BEGIN");
            strSQL.Append("     INSERT INTO RPTUSELOGM(FABID,LOGUSERNAME,LOGUSERTYPE,SessionID,LOGDATE,LOGRPTTYPE,RPTName,DayLOGCount,LOGTTLCount,LOGPCNAME,LOGPCIP,OPT1,OPT2,OPT3)");
            strSQL.Append("     SELECT '" + strFID + "','" + strLogUserName + "','" + strLogUserType + "','" + strSSID + "', GETDATE(),'" + LogRPTType + "',");
            strSQL.Append("     '" + strRPTName + "',1,1,'" + strLPCName + "','" + strLPCIP + "','" + strOPT1 + "','" + strOPT2 + "','" + strOPT3 + "'");
            strSQL.Append(" END");
            if (dbconn.ExecuteSql(strSQL.ToString()) == false)
            {
                return;
            }
        }
        public string QueryDataRunTime(string strFID)
        {
            StringBuilder strSQL = new StringBuilder();
            DBConn dbconn = new DBConn();
            string strDataRunTime = "";
            strSQL.Append(" SELECT 'Detail Data最後執行時間：' AS FLG,CONVERT(VARCHAR(30),LastDT,120) AS LASTRUNTIME FROM RPTU WHERE TYPEID = 'RPTS' AND ID = 'ENG' ");
            strSQL.Append(" UNION ALL ");
            strSQL.Append(" SELECT 'Summary Data最後執行時間：' AS FLG,CONVERT(VARCHAR(30),LastTime,120) AS LASTRUNTIME FROM T_COMM_PJOBM WHERE JTYPE = 'ENGSUM01' ");

            DataSet ds = new DataSet();
            ds = dbconn.GetDataSetBySql(strSQL.ToString());
            if (ds.Tables[0].Rows.Count == 2)
            {
                strDataRunTime = ds.Tables[0].Rows[0][0].ToString() + ds.Tables[0].Rows[0][1].ToString() + "; " + ds.Tables[0].Rows[1][0].ToString() + ds.Tables[0].Rows[1][1].ToString();
            }
            else
            {
                strDataRunTime = "";
            }
            return strDataRunTime;
        }
        public string QueryLogInfo(string strFID, string strLogUserName, string strLogUserType, string LogRPTType, string strRPTName)
        {
            StringBuilder strSQL = new StringBuilder();
            DBConn dbconn = new DBConn();
            string strReturn = "";
            strSQL.Append(" SELECT FABID,LOGUSERNAME,LOGUSERTYPE,SessionID,LOGDATE,LOGRPTTYPE,RPTName,DayLOGCount,LOGTTLCount,LOGPCNAME,LOGPCIP");
            strSQL.Append(" FROM RPTUSELOGM WHERE FABID = '" + strFID + "'  ");
            strSQL.Append(" AND LOGUSERNAME = '" + strLogUserName + "'  ");
            strSQL.Append(" AND LOGUSERTYPE = '" + strLogUserType + "' ");
            strSQL.Append(" AND RPTNAME = '" + strRPTName + "'  ");
            strSQL.Append(" AND LOGRPTTYPE = '" + LogRPTType + "' ");
            DataSet ds = new DataSet();
            ds = dbconn.GetDataSetBySql(strSQL.ToString());
            if (ds.Tables[0].Rows.Count == 1)
            {
                strReturn = "WelCome " + strLogUserName + " 第" + ds.Tables[0].Rows[0][8].ToString() + "次訪問!!!";
            }
            else
            {
                strReturn = "WelCome 訪問!!!";
            }
            return strReturn;
        }
    }

}