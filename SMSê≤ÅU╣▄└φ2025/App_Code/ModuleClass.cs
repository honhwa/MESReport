using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Collections;
/// <summary>
/// ModuleClass 的摘要描述
/// 問題描述                                                                                                                    解決辦法
/// 'Microsoft.Jet.OLEDB.4.0' 提供者並未登錄於本機電腦上                                                 IIS應用池將【啟用32位應用程式】啟用即可
/* V1.01 2017-07-25   Li.Miao  add*/
/// </summary>
public class ModuleClass
{
    public const string CstrErrorLogType = "Error";      /*ErrorLog FileName自定義開頭*/
    public const string CstrLogType = "";   /*Log FileName自定義開頭*/

    public const int CintLogDays = 60;      /*Log保留的天數*/
    public const int CintXlsDays = 60;      /*Excel保留的天數*/

    //public const string TitleVersion = "上海MES報表系統 V1.04"; 

	public ModuleClass()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    public string GetVersion()
    {
        string strVersion = "上海MES報表系統 V1.04"; 
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

   
    public bool SetWinPop(out string strTitle, out string strCon)
    {
        bool blShow = true;
        int intDays = 6;
        DateTime dtDate = new DateTime();

        dtDate = Convert.ToDateTime("2017-01-03");

        strTitle = dtDate.ToString("yyyy.MM.dd") + "更新說明";

        strCon = "";
        strCon = strCon + "如下報表已更新,請參考[報表說明]" + "\n";
        strCon = strCon + "•報廢率數據查詢" + "\n";
        strCon = strCon + "•部材耗損率" + "\n";
        strCon = strCon + "•部材報廢率" + "\n";
        strCon = strCon + "•部材超耗查詢" + "\n";
        strCon = strCon + "•SR報表-區間明細查詢" + "\n";
        strCon = strCon + "•SR報表-趨勢查詢" + "\n";

        if (dtDate.AddDays(intDays) < System.DateTime.Now)
        {
            blShow = false;
        }
        return blShow;
    }
    /// <summary>
    /// 文本記錄處理
    /// </summary>
    /// <param name="strLogFileName">檔案名稱</param>
    /// <param name="strLog">需記錄的信息</param>
    public void SaveLog(string strLogType, string strLog)
    {
        try
        {
            string strLogFileName = "";
            if (strLogType == CstrErrorLogType)
            {
                strLogFileName = CstrErrorLogType + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            else
            {
                strLogFileName = System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Log");
            if (!System.IO.Directory.Exists(strPath))
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            StreamWriter sw = File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/Log/" + strLogFileName));
            sw.WriteLine(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ":" + strLog);
            sw.Flush();
            sw.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 定期刪除舊的LOG文件
    /// </summary>
    public void DeleteLog()
    {
        try
        {
            string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Log");
            DirectoryInfo di = new DirectoryInfo(strPath);
            if (di.Exists)
            {
                FileInfo[] files = di.GetFiles("*.txt");
                foreach (FileInfo fs in files)//删除目录下*.txt
                {
                    if (fs.CreationTime <= DateTime.Now.AddDays(CintLogDays * -1))
                    {
                        fs.Delete();
                    }
                }
            }

            strPath = System.Web.HttpContext.Current.Server.MapPath("~/ExcelUpload");
            DirectoryInfo dixls = new DirectoryInfo(strPath);
            if (dixls.Exists)
            {
                FileInfo[] files = dixls.GetFiles("*.xls");
                foreach (FileInfo fs in files)//删除目录下*.txt
                {
                    if (fs.CreationTime <= DateTime.Now.AddDays(CintXlsDays * -1))
                    {
                        fs.Delete();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            throw ex;
        }
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

    public bool ExportExcel(DataTable dtGrid)
    {
        try
        {
            string strPutText = "";
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = false;
            System.Web.HttpContext.Current.Response.Charset = "UTF-8";
            System.Web.HttpContext.Current.Response.ContentType = "Content-Language;content=UTF-8";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
            for (int i = 0; i < Convert.ToInt32(dtGrid.Columns.Count); i++)
            {
                strPutText = strPutText + dtGrid.Columns[i].ToString().Replace("\t", "").Replace("\n", "") + "\t";
            }
            strPutText = strPutText + "\n";
            System.Web.HttpContext.Current.Response.Output.Write(strPutText);

            for (int j = 0; j < Convert.ToInt32(dtGrid.Rows.Count); j++)
            {
                strPutText = "";
                for (int i = 0; i < Convert.ToInt32(dtGrid.Columns.Count); i++)
                {
                    strPutText = strPutText + dtGrid.Rows[j][i].ToString().Replace("\t", "").Replace("\n", "") + "\t";
                }
                strPutText = strPutText + "\n";
                System.Web.HttpContext.Current.Response.Output.Write(strPutText);
            }
            System.Web.HttpContext.Current.Response.Clear();
            return true;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return false;
        }
        finally
        {
            System.Web.HttpContext.Current.Response.End();
        }
    }
    public bool ExportExcel(GridView gvData)
    {
        try
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = false;
            System.Web.HttpContext.Current.Response.Charset = "UTF-8";
            //System.Web.HttpContext.Current.Response.ContentType = "Content-Language;content=UTF-8";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
            System.Web.HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            gvData.RenderControl(oHtmlTextWriter);
            System.Web.HttpContext.Current.Response.Output.Write(oStringWriter.ToString());
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.Response.Clear();
            return true;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return false;
        }
        finally
        {
            System.Web.HttpContext.Current.Response.End();
        }
    }
    public void GetLoginInfo(out string strNTName, out string strPCIP, out string strPCName)
    {
        try
        {
            DeleteLog();    /*將歷史Log刪除*/
            /*IIS設置的時候要將匿名驗證方式去掉*/
            string Auth_users;
            string[] logon;
            Auth_users = System.Web.HttpContext.Current.Request.ServerVariables["Auth_user"].ToString();
            logon = Auth_users.Split('\\');
            strNTName = logon[1].Trim().ToString();
            strPCName = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress).HostName.Split('.')[0].ToString();
            strPCIP = HttpContext.Current.Request.UserHostAddress.ToString();
        }
        catch
        {
            strNTName = HttpContext.Current.Request.UserHostAddress;
            strPCName = HttpContext.Current.Request.UserHostAddress.ToString();
            strPCIP = HttpContext.Current.Request.UserHostAddress.ToString();
        }
    }
    public bool GetMPCIPM(string strFunctionName, string strPCIP)
    {
        bool blRole = false;
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "SELECT LOGPCIP FROM dbo.MPCIPM WHERE LOGPCIP = LEFT('" + strPCIP + "',LEN(LOGPCIP));";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dtTmp.Rows.Count > 0)
            {
                blRole = true;
            }
            return blRole;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return blRole;
        }
    }
    public string GetFABID(string strPlant)
    {
        string strValue = "";
        try
        {
            switch (strPlant)
            {
                case "A": strValue = "A"; break;
                case "B": strValue = "B"; break;
                case "WE01": strValue = "B"; break;
                case "WE02": strValue = "A"; break;

                default:
                    break;
            }
            return strValue;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return "";
        }
    }
    public string GetStationShop(string strPlant)
    {
        string strValue = "";
        try
        {
            switch (strPlant)
            {
                case "WE01": strValue = "B"; break;
                default:
                    break;
            }
            return strValue;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return "";
        }
    }
    public string GetProductShopCode(string strPlant)
    {
        string strValue = "";
        try
        {
            switch (strPlant)
            {
                case "CELL2": strValue = "BEOL"; break;
                case "BEOL": strValue = "BEOL"; break;
                case "LCM": strValue = "Module"; break;
                case "LAM": strValue = "Module"; break;
                case "Module": strValue = "Module"; break;
                default:
                    break;
            }
            return strValue;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return "";
        }
    }
    public string stringFormat(string strValue, int intLen)
    {
        string strReturn = "";
        try
        {
            if (strValue.Length > intLen)
            {
                strReturn = strValue.Substring(0, intLen);
            }
            else
            {
                strReturn = strValue;
            }
        }
        catch
        {
            strReturn = "Err";
        }
        return strReturn;
    }
    public void RecordLogInfo(string strFabID, string strLogUserType, string strSSID, string LogRPTType, string strRPTPath, string strRPTName, string strOPT1, string strOPT2, string strOPT3)
    {
        try
        {
            if (strSSID.Trim().ToString() == "")
            {
                return;
            }
            string strNTName = "", strPCIP = "", strPCName = "";
            GetLoginInfo(out strNTName, out strPCIP, out strPCName);

            string strSql = "";
            DBConn dbConn = new DBConn();
            strSql = strSql + " UPDATE RPTUSELOGM SET  ";
            strSql = strSql + " SESSIONID = '" + strSSID + "', ";
            strSql = strSql + " LOGDATE=GETDATE(), ";
            strSql = strSql + " DAYLOGCOUNT=CASE WHEN LOGDATE>=CAST((CONVERT(VARCHAR(10),GETDATE(),120)+' 00:00:00') AS DATETIME) THEN DAYLOGCOUNT+1 ELSE 1 END, ";
            strSql = strSql + " LOGTTLCOUNT=LOGTTLCOUNT+1, ";
            strSql = strSql + " LOGPCNAME='" + strPCName + "', ";
            strSql = strSql + " LOGPCIP='" + strPCIP + "', ";
            strSql = strSql + " OPT1='" + strOPT1 + "', ";
            strSql = strSql + " OPT2='" + strOPT2 + "', ";
            strSql = strSql + " OPT3='" + strOPT3 + "' ";
            strSql = strSql + " WHERE FABID = '" + strFabID.ToString() + "' ";
            strSql = strSql + " AND LOGUSERNAME = '" + strNTName + "'  ";
            strSql = strSql + " AND LOGUSERTYPE = '" + strLogUserType + "' ";
            strSql = strSql + " AND RPTNAME = '" + strRPTName + "'  ";
            strSql = strSql + " AND LOGRPTTYPE = '" + LogRPTType + "' ";
            strSql = strSql + " IF @@RowCount = 0 ";
            strSql = strSql + " BEGIN";
            strSql = strSql + "     INSERT INTO RPTUSELOGM(FABID,LOGUSERNAME,LOGUSERTYPE,SessionID,LOGDATE,LOGRPTTYPE,RPTName,DayLOGCount,LOGTTLCount,LOGPCNAME,LOGPCIP,OPT1,OPT2,OPT3)";
            strSql = strSql + "     SELECT '" + strFabID.ToString() + "','" + strNTName + "','" + strLogUserType + "','" + strSSID + "', GETDATE(),'" + LogRPTType + "',";
            strSql = strSql + "     '" + strRPTName + "',1,1,'" + strPCName + "','" + strPCIP + "','" + strOPT1 + "','" + strOPT2.Replace("'", "''") + "','" + strOPT3 + "'";
            strSql = strSql + " END";
            string strErrDesc = "";
            int intErrCode = 0;
            if (dbConn.ExecuteSql(strSql, out strErrDesc, out intErrCode) == false)
            {
                SaveLog(CstrErrorLogType.ToString(), intErrCode.ToString() + strErrDesc.ToString());
                return;
            }
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType.ToString(), ex.ToString());
        }
    }
    public DataTable GetReportUseLog()
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT ROW_NUMBER()OVER(ORDER BY SUM(LOGTTLCount) DESC) AS SortID,RPTName,SUM(LOGTTLCount) AS Cnt";
            strSql = strSql + " FROM dbo.RPTUSELOGM WHERE LOGRPTTYPE = 'Query' GROUP BY RPTName";
            strSql = strSql + " ORDER BY SUM(LOGTTLCount) DESC";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetReportUseLog(string strUser)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT ROW_NUMBER()OVER(ORDER BY SUM(LOGTTLCount) DESC) AS SortID,RPTName,SUM(LOGTTLCount) AS Cnt";
            strSql = strSql + " FROM dbo.RPTUSELOGM WHERE LOGUSERNAME='"+strUser+"' AND LOGRPTTYPE = 'Query' GROUP BY RPTName";
            strSql = strSql + " ORDER BY SUM(LOGTTLCount) DESC";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataSet  GetReportUseLog(string strFR,string strTO,string strUser,string strReport)
    {
        try
        {
            DBConn dbConn = new DBConn();
            StringBuilder  strSql=new StringBuilder ();
            strSql.Append(" SELECT logusername as '查詢人員' ,rptname as '報表名稱',count(1) as '查詢次數' ");
            strSql.AppendFormat ("  FROM dbo.rptuselogx WHERE LOGRPTTYPE = 'Query' and logdate between '{0}' and '{1}' ",strFR,strTO );
            if(strUser !="")
            {
                    strSql.AppendFormat("  and logusername like '%{0}%' ",strUser );
            }
            if (strReport  != "")
            {
                strSql.AppendFormat("  and rptname like '%{0}%' ", strReport );
            }
            strSql.Append(" GROUP BY logusername,rptname ORDER BY RPTNAME,logusername;");            
          
            DataSet  dsTmp = dbConn.GetDataSetBySql(strSql.ToString ());
            return dsTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetYieldType()
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT ID,IDDesc FROM(";
            strSql = strSql + " SELECT '1' AS ITEM,'FPY' AS ID, 'FPY' AS IDDesc";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT '2' AS ITEM,'OneRework' AS ID, '一次Rework' AS IDDesc";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT '3' AS ITEM,'TwoRework' AS ID, '二次Rework' AS IDDesc";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT '4' AS ITEM,'ThreeRework' AS ID, '大於Rework二次' AS IDDesc";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT '5' AS ITEM,'TotalRework' AS ID, '所有Rework' AS IDDesc";
            strSql = strSql + " UNION ALL";
            strSql = strSql + " SELECT '6' AS ITEM,'ALL' AS ID, 'ALL' AS IDDesc";
            strSql = strSql + " )T ORDER BY ITEM";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public string  ListBoxToValue(ListBox lb)
    {
        try
        {
            string strToValue = "";
            for (int i = 0; i < lb.Items.Count; i++)
            {
                strToValue = strToValue + "'" + lb.Items[i].Value.ToString() + "',";           
            }
            if (strToValue != "")
            {
                strToValue = strToValue.TrimEnd(',');
            }
            return strToValue;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public string RowColumn(ListBox lb)
    {
        try
        {
            string strToValue = "";
            for (int i = 0; i < lb.Items.Count; i++)
            {
                strToValue = strToValue + "[" + lb.Items[i].Text.ToString() + "_Def],";
                strToValue = strToValue + "[" + lb.Items[i].Text.ToString() + "_OUT],";
            }
            if (strToValue != "")
            {
                strToValue = strToValue.TrimEnd(',');
            }
            return strToValue;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetTop(int intTop)
    {
        try
        {
            DataTable dtTop = new DataTable();
            dtTop.Columns.Add(new DataColumn("ID", typeof(string)));
            dtTop.Columns.Add(new DataColumn("IDDesc", typeof(string)));
            dtTop.Clear();
            for (int i = 1; i <= intTop; i++)
            {
                DataRow drTop = dtTop.NewRow();
                drTop["ID"] = i.ToString();
                drTop["IDDesc"] = "TOP " + i.ToString();
                dtTop.Rows.Add(drTop);
            }
            return dtTop;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetYear(int intCnt)
    {
        try
        {
            DataTable dtYear = new DataTable();
            dtYear.Columns.Add(new DataColumn("Year", typeof(string)));
            dtYear.Clear();
            for (int i = 0; i < intCnt; i++)
            {
                DataRow drYear = dtYear.NewRow();
                drYear["Year"] = DateTime.Now.AddYears(-1 * i).ToString("yyyy");
                dtYear.Rows.Add(drYear);
            }
            return dtYear;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMonth()
    {
        try
        {
            DataTable dtMonth = new DataTable();
            dtMonth.Columns.Add(new DataColumn("Month", typeof(string)));
            dtMonth.Clear();
            for (int i = 1; i <= 12; i++)
            {
                DataRow drMonth = dtMonth.NewRow();
                drMonth["Month"] = i.ToString();
                dtMonth.Rows.Add(drMonth);
            }
            return dtMonth;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetPlant(string strMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            if (strMode == "FABID")
            {
                strSql = " SELECT ID,IDDesc FROM(";
                strSql = strSql + " SELECT 'B' AS ID, 'WE01' AS IDDesc";
                strSql = strSql + " )T ORDER BY ID DESC";
            }
        else
            {
                strSql = " SELECT ID,IDDesc FROM(";
                strSql = strSql + " SELECT 'CELL2' AS ID, 'CELL2' AS IDDesc";
                strSql = strSql + " UNION ALL";
                strSql = strSql + " SELECT 'MODULE' AS ID, 'MODULE' AS IDDesc";
                strSql = strSql + " )T ORDER BY ID";
            }
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMWCM(string strFabID, string strPlant, string strTransMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT WORKCTR AS ID,WORKCTR+' '+MAX(WORKNAME) AS IDDesc FROM V_MWCM WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            if (strTransMode != "") { strSql = strSql + " AND TransMode = '" + strTransMode + "' "; }
            if (strPlant != "") { strSql = strSql + " AND Plant LIKE '%" + strPlant + "%' "; }
            strSql = strSql + " GROUP BY WORKCTR";
            strSql = strSql + " ORDER BY WORKCTR";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetOperation()
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT 'P230' AS ID, 'PCBI(P230)' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT 'PD10' AS ID, 'M2.5 C檢(PD10)' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT 'PL40' AS ID, 'LAM C/D檢(PL40)' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT 'P370' AS ID, 'M3 C檢(P370)' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT 'P430' AS ID, 'M4 C檢(P430)' AS IDDesc ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMSLM(string strFabID, string strTransMode, string strWorkCtr)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT SUBLINEID AS ID,SUBLINEID + ' ' + MAX(SUBLINENAME) AS IDDesc FROM V_MSLM WHERE 1 = 1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            if (strTransMode != "") { strSql = strSql + " AND TransMode = '" + strTransMode + "' "; }
            if (strWorkCtr != "") { strSql = strSql + " AND WORKCTR = '" + strWorkCtr + "' "; }
            strSql = strSql + " GROUP BY SUBLINEID";
            strSql = strSql + " ORDER BY SUBLINEID";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetLineStock(string strFabID, string strTransMode, string strWorkCtr)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT StockID AS ID,StockID + ' ' + StockID AS IDDesc FROM V_MSLM WHERE 1 = 1 AND StockID!='' ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            if (strTransMode != "") { strSql = strSql + " AND TransMode = '" + strTransMode + "' "; }
            if (strWorkCtr != "") { strSql = strSql + " AND WORKCTR = '" + strWorkCtr + "' "; }
            strSql = strSql + " ORDER BY StockID";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMWOTM(string strFabID)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT WOTYPE AS ID,WOTYPE+' '+MAX(WOTYPEDESC) AS IDDesc FROM MWOTM WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            strSql = strSql + " GROUP BY WOTYPE";
            strSql = strSql + " ORDER BY WOTYPE";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMLTM(string strFabID)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT LOTTYPE AS ID,LOTTYPE+' '+MAX(LOTTYPEDESC) AS IDDesc FROM V_MLTM WHERE 1 = 1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            strSql = strSql + " GROUP BY LOTTYPE";
            strSql = strSql + " ORDER BY LOTTYPE";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetSIZE(string strFabID)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT '' AS ID,'' AS IDDesc UNION";
            strSql = strSql + " SELECT DISTINCT SIZEID AS ID, SIZEID + ' 寸' AS IDDesc FROM dbo.MSIM WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            strSql = strSql + " GROUP BY SIZEID";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }

    /*1.01 Add Begin*/
    public DataTable GetMWCM_WIP(string strFabID, string strPlant, string strTransMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT WORKCTR AS ID,WORKCTR+' '+MAX(WORKNAME) AS IDDesc FROM VWIP_MWCM WITH(NOLOCK) WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            if (strTransMode != "ALL") { strSql = strSql + " AND TransMode = '" + strTransMode + "' "; }
            if (strPlant != "") { strSql = strSql + " AND Plant LIKE '%" + strPlant + "%' "; }
            strSql = strSql + " GROUP BY WORKCTR ";
            strSql = strSql + " ORDER BY WORKCTR ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetProductSize(string strFabID, string strPlant, string strTransMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT 'ALL' AS ID,'ALL' AS IDDesc UNION ALL ";
            strSql = strSql + " SELECT DISTINCT T1.SIZEID AS ID,T1.SIZEID AS IDDesc FROM dbo.VWIP_MPIM T1 WITH(NOLOCK) ";
            strSql = strSql + " INNER JOIN dbo.V_MPROJM T2 WITH(NOLOCK) ON(T1.FABID = T2.FABID AND T1.TransMode = T2.TransMode AND T1.PROJECT_NAME=T2.ProjectName) ";
            strSql = strSql + " WHERE T1.PRODID NOT LIKE '#%' ";
            if (strFabID != "") { strSql = strSql + " AND T1.FABID = '" + strFabID + "' "; }
            //if (strPlant != "") { strSql = strSql + " AND T1.Plant Like '%" + strPlant + "%' "; }
            if (strTransMode != "ALL") { strSql = strSql + " AND T1.TransMode = '" + strTransMode + "' "; }
            strSql = strSql + " ORDER BY ID DESC; ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }

    public DataTable GeProductListForWIP(string strFabID, string strPlant, string strSize, string strTransMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT DISTINCT T1.PRODID AS ID,T1.PRODID AS IDDesc FROM dbo.VWIP_WLTM T1 WITH(NOLOCK) ";
            strSql = strSql + " INNER JOIN dbo.VWIP_MPIM T2 WITH(NOLOCK) ON(T1.FABID = T2.FABID AND T1.TransMode = T2.TransMode AND T1.PRODID=T2.PRODID)";
            strSql = strSql + " WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND T1.FABID = '" + strFabID + "' "; }
            if (strSize != "ALL") { strSql = strSql + " AND T1.SIZEID = '" + strSize + "' "; }
            if (strTransMode != "ALL") { strSql = strSql + " AND T1.TransMode = '" + strTransMode + "' "; }
            strSql = strSql + " ORDER BY T1.PRODID; ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }

    /*1.01 Add End*/
    public DataTable GetProjectName(string strFabID ,string strddlSIZE)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT '' AS ID,'' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT DISTINCT T1.PROJECT_NAME AS ID,T1.PROJECT_NAME AS IDDesc FROM dbo.MPIM T1";
            strSql = strSql + " WHERE LEN(PRODID)=13";
            if (strFabID != "") { strSql = strSql + " AND T1.FABID = '" + strFabID + "' "; }
            if (strddlSIZE != "") { strSql = strSql + " AND T1.SIZEID = '" + strddlSIZE + "' "; }
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetProductList(string strFabID, string strPlant, string strddlSIZE, string strProjectName)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT DISTINCT T1.PRODID AS ID,T1.PRODID AS IDDesc FROM dbo.MPIM T1";
            strSql = strSql + " WHERE LEN(PRODID)=13";
            if (strFabID != "") { strSql = strSql + " AND T1.FABID = '" + strFabID + "' "; }
            if (strddlSIZE != "") { strSql = strSql + " AND T1.SIZEID= '" + strddlSIZE + "' "; }
            if (strProjectName != "") { strSql = strSql + " AND T1.PROJECT_NAME = '" + strProjectName + "' "; }
            strSql = strSql + " ORDER BY T1.PRODID; ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMDCM(string strFabID, string strTransMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT DISTINCT DEFECTCD AS ID,DEFECTCD+' '+MAX(DEFECTDESC) AS IDDesc FROM V_MDCM WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            if (strTransMode != "") { strSql = strSql + " AND TransMode = '" + strTransMode + "' "; }
            strSql = strSql + " GROUP BY DEFECTCD ";
            strSql = strSql + " ORDER BY DEFECTCD ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public void MoveData(ListBox lbFrom, ListBox lbTo, string strSelectValue, string strMoveType)
    {
        if (strMoveType.ToUpper() == "DOWN")
        {
            ArrayList alTmp = new ArrayList();
            foreach (ListItem item in lbFrom.Items)
            {
                if (strSelectValue == "")
                {
                    alTmp.Add(item);
                }
                else
                {
                    if (item.Selected)
                    {
                        alTmp.Add(item);
                    }
                }
            }
            if (alTmp.Count > 100)
            {
                lbFrom.ToolTip = "一次最多選擇100項, 不選擇就代表是全部選擇";
                lbTo.ToolTip = "一次最多選擇100項, 不選擇就代表是全部選擇";
                return;
            }
            foreach (ListItem item in alTmp)
            {
                lbTo.Items.Add((new ListItem(item.Text, item.Value)));
                lbFrom.Items.Remove(item);
            }
        }
        if (strMoveType.ToUpper() == "UP")
        {
            ArrayList alTmp = new ArrayList();
            foreach (ListItem item in lbTo.Items)
            {
                if (strSelectValue == "")
                {
                    alTmp.Add(item);
                }
                else
                {
                    if (item.Selected)
                    {
                        alTmp.Add(item);
                    }
                }
            }
            if (alTmp.Count > 100)
            {
                lbFrom.ToolTip = "一次最多選擇100項, 不選擇就代表是全部選擇";
                lbTo.ToolTip = "一次最多選擇100項, 不選擇就代表是全部選擇";
                return;
            }
            foreach (ListItem item in alTmp)
            {
                lbFrom.Items.Add((new ListItem(item.Text, item.Value)));
                lbTo.Items.Remove(item);
            }
        }
        /*From ListBox 排序*/
        SortedList sltFrom = new SortedList();
        foreach (ListItem item in lbFrom.Items)
        {
            sltFrom.Add(item.Text, item.Value);
        }
        lbFrom.Items.Clear();//清空原listbox
        foreach (DictionaryEntry obj in sltFrom)
        {
            ListItem myit = new ListItem();
            myit.Text = obj.Key.ToString();
            myit.Value = obj.Value.ToString();
            lbFrom.Items.Add(myit);               //再重新将sortlist集合复制回listbox，这样，复制回来的数组是按值排序过的
        }
        /*To ListBox 排序*/
        SortedList sltTo = new SortedList();
        foreach (ListItem item in lbTo.Items)
        {
            sltTo.Add(item.Text, item.Value);
        }
        lbTo.Items.Clear();//清空原listbox
        foreach (DictionaryEntry obj in sltTo)
        {
            ListItem myit = new ListItem();
            myit.Text = obj.Key.ToString();
            myit.Value = obj.Value.ToString();
            lbTo.Items.Add(myit);               //再重新将sortlist集合复制回listbox，这样，复制回来的数组是按值排序过的
        }
    }
    ///   <summary>   
    ///   合并GridView列中相同的行   
    ///   </summary>   
    ///   <param   name="GridView1">GridView对象</param>   
    ///   <param   name="cellNum">需要合并的列</param>   
    public void GroupRows(GridView GridView1, int cellNum)
    {
        int i = 0, rowSpanNum = 1;
        while (i < GridView1.Rows.Count - 1)
        {
            GridViewRow gvr = GridView1.Rows[i];
            for (++i; i < GridView1.Rows.Count; i++)
            {
                GridViewRow gvrNext = GridView1.Rows[i];
                if (gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text)
                {
                    gvrNext.Cells[cellNum].Visible = false;
                    rowSpanNum++;
                }
                else
                {
                    gvr.Cells[cellNum].RowSpan = rowSpanNum;
                    rowSpanNum = 1;
                    break;
                }

                if (i == GridView1.Rows.Count - 1)
                {
                    gvr.Cells[cellNum].RowSpan = rowSpanNum;
                }
            }
        }
    }
    ///   <summary>   
    ///   合并GridView列中第一個列值相同的多列做合併
    ///   </summary>   
    ///   <param   name="GridView1">GridView对象</param>   
    ///   <param   name="cellNum">需要合并起始的列</param>   
    ///   ///   <param   name="cellNum">需要合并的多列</param>   
    public void GroupMoreRows(GridView GridView1, int cellStartNum, int cellEndNum)
    {
        int i = 0, rowSpanNum = 1;
        while (i < GridView1.Rows.Count - 1)
        {
            GridViewRow gvr = GridView1.Rows[i];
            for (++i; i < GridView1.Rows.Count; i++)
            {
                GridViewRow gvrNext = GridView1.Rows[i];
                if (gvr.Cells[cellStartNum].Text == gvrNext.Cells[cellStartNum].Text)
                {
                    for (int intCnt = cellStartNum; intCnt <= cellEndNum; intCnt++)
                    {
                        gvrNext.Cells[intCnt].Visible = false;
                    }
                    rowSpanNum++;
                }
                else
                {
                    for (int intCnt = cellStartNum; intCnt <= cellEndNum; intCnt++)
                    {
                        gvr.Cells[intCnt].RowSpan = rowSpanNum;
                    }
                    rowSpanNum = 1;
                    break;
                }

                if (i == GridView1.Rows.Count - 1)
                {
                    for (int intCnt = cellStartNum; intCnt <= cellEndNum; intCnt++)
                    {
                        gvr.Cells[intCnt].RowSpan = rowSpanNum;
                    }
                }
            }
        }
    }
    public string GetEngMonth(string strMonth)
    {
        string strEngMonth = "";
        switch (strMonth)
        {
            case "01": strEngMonth = "Jan."; break;
            case "02": strEngMonth = "Feb."; break;
            case "03": strEngMonth = "Mar."; break;
            case "04": strEngMonth = "Apr."; break;
            case "05": strEngMonth = "May."; break;
            case "06": strEngMonth = "Jun."; break;
            case "07": strEngMonth = "Jul."; break;
            case "08": strEngMonth = "Aug."; break;
            case "09": strEngMonth = "Sep."; break;
            case "10": strEngMonth = "Oct."; break;
            case "11": strEngMonth = "Nov."; break;
            case "12": strEngMonth = "Dec."; break;
            default:
                break;
        }
        return strEngMonth;
    }
    /// <summary>
    /// 處理周別對應的日期
    /// </summary>
    /// <param name="strWeek">YYYY-MM</param>
    /// <param name="intWeekDay">週一到周日；對應 1~7</param>
    /// <returns></returns>
    public string GetDateOfWeek(string strWeek, int intWeekDay)
    {
        string strDateOfWeek = "";
        DateTime dtFirstDate = Convert.ToDateTime(strWeek.Substring(0, 4) + "-01-01").AddYears(1);
        for (int i = 0; i < 54; i++)
        {
            if (GetYearWeek(dtFirstDate.AddDays(-1 * 7 * i)) == strWeek)
            {
                int intDay = GetNumWeek(Convert.ToInt32(dtFirstDate.DayOfWeek));
                DateTime dtFromDate = dtFirstDate.AddDays(-1 * 7 * i - intDay - 6 + 7);
                DateTime dtToDate = dtFirstDate.AddDays(-1 * 7 * i - intDay + 7);

                strDateOfWeek = dtFromDate.AddDays(intWeekDay - 1).ToString("yyyy-MM-dd");
                break;
            }
        }
        return strDateOfWeek;
    }
    public string GetYearWeek(DateTime dtDate)
    {
        /*周的定義是 周一到周日*/
        string strWeek = "";
        int year = dtDate.Year;
        int month = dtDate.Month;
        int day = dtDate.Day;
        int a = Convert.ToInt32((14 - month) / 12);
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        int b = Convert.ToInt32(y / 4) - Convert.ToInt32(y / 100) + Convert.ToInt32(y / 400);
        int J = day + Convert.ToInt32((153 * m + 2) / 5) + 365 * y + b - 32045;
        int d4 = (((J + 31741 - (J % 7)) % 146097) % 36524) % 1461;
        int L = Convert.ToInt32(d4 / 1460);
        int d1 = ((d4 - L) % 365) + L;
        int intWeek=Convert.ToInt32(d1 / 7) + 1;
        if (intWeek == 1)
        {
            strWeek = dtDate.AddDays(7).ToString("yyyy-") + intWeek.ToString().PadLeft(3, '0').Substring(1, 2);
        }
        else if (intWeek >= 52)
        {
            strWeek = dtDate.AddDays(-7).ToString("yyyy-") + intWeek.ToString().PadLeft(3, '0').Substring(1, 2);
        }
        else
        {
            strWeek = dtDate.ToString("yyyy-") + intWeek.ToString().PadLeft(3, '0').Substring(1, 2);
        }
        return strWeek; /*返回格式  yyyy-ww*/
    }
    public string GetWeek(DateTime dtDate)
    {
        ///*周的定義是 周一到周日*/
        string strWeek = "";
        int year = dtDate.Year;
        int month = dtDate.Month;
        int day = dtDate.Day;
        int a = Convert.ToInt32((14 - month) / 12);
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        int b = Convert.ToInt32(y / 4) - Convert.ToInt32(y / 100) + Convert.ToInt32(y / 400);
        int J = day + Convert.ToInt32((153 * m + 2) / 5) + 365 * y + b - 32045;
        int d4 = (((J + 31741 - (J % 7)) % 146097) % 36524) % 1461;
        int L = Convert.ToInt32(d4 / 1460);
        int d1 = ((d4 - L) % 365) + L;
        strWeek = "W" + Convert.ToString(Convert.ToInt32(d1 / 7) + 1);
        return strWeek; /*返回格式  W01*/
    }
    public int GetNumWeek(int intDay)
    {
        int intWeekNum;
        if (intDay == 0)
        {
            intWeekNum = 7;
        }
        else
        {
            intWeekNum = intDay;
        }
        return intWeekNum;
    }
    public string GetYearWeekFormatString(string strFormat, string strValue)
    {
        /*strValue 傳入值的格式YYYY-WW如有其他格式，請再以下加*/
        string strTmp = "";
        if (strFormat == "WYYWW")
        {
            strTmp = "W" + strValue.Replace("-", "").Substring(2, 4);
        }
        else
        {
            strTmp = strValue;
        }
        return strTmp;
    }
    /// <summary>
    /// 查詢EXCEL的數據
    /// <summary>
    public DataSet ExcelRead(string strFilePath, string TableName)
    {
        string conn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";
        OleDbConnection ExcelConn = new OleDbConnection(conn);
        try
        {
            string strCom = string.Format("SELECT * FROM [" + TableName + "]");
            ExcelConn.Open();
            OleDbDataAdapter dr = new OleDbDataAdapter(strCom, ExcelConn);
            DataSet ds = new DataSet();
            dr.Fill(ds, TableName);
            ExcelConn.Close();
            return ds;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            ExcelConn.Close();
            return null;
        }
    }
    public bool GetUserGroupCode(string strGroup, string strBRMUserID)
    {
        bool blRole = false;
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT USERGROUPCD FROM MUGM_Sc ";
            strSql = strSql + " WHERE USERGROUPCD = '" + strGroup + "' ";
            if (strBRMUserID != "") { strSql = strSql + " AND BRMUserID = '" + strBRMUserID + "'"; }
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dtTmp.Rows.Count > 0)
            {
                blRole = true;
            }
            if (strBRMUserID.ToUpper() == "jeff.liang".ToUpper())/*默認jeff.liang的賬號是都是權限的*/
            {
                blRole = true;
            }
            return blRole;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return blRole;
        }
    }
    public bool GetRole(string strGroup, string strUserID)
    {
        bool blRole = false;
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT USERID FROM MUGMM_Sc WHERE USERID = '" + strUserID + "' AND USERGROUPCD = '" + strGroup + "' ";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dtTmp.Rows.Count > 0)
            {
                blRole = true;
            }
            if (strUserID.ToUpper() == "jeff.liang".ToUpper())/*默認jeff.liang的賬號是都是權限的*/
            {
                blRole = true;
            }
            return blRole;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return blRole;
        }
    }
    public DataTable GetMUCM(int intYear, int intMonth, string strMaterial)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT [Year],[Month],Material,Unit_Cost,Currency,CONVERT(VARCHAR(30),UpdateTime,120) AS UpdateTime,UpdateUserID,PCName FROM dbo.MUCM";
            strSql = strSql + " WHERE [Year]= " + intYear + " ";
            strSql = strSql + " AND [Month] = " + intMonth + " ";
            if (strMaterial != "") { strSql = strSql + " AND Material = '" + strMaterial + "' "; }
            strSql = strSql + " ORDER BY [Year],[Month],Material";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMPlanRateM(int intYear, int intMonth, string strPlanType, string strPlantID, string strProjectName, string strPartType)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT [Year],[Month],PlanType,PLANTID,PROJECT_NAME,PartType,PlanRate,Userid,CONVERT(VARCHAR(30),Time_Stamp,120) AS Time_Stamp,PCName FROM dbo.MPlanRateM";
            strSql = strSql + " WHERE [Year]= " + intYear + " ";
            strSql = strSql + " AND [Month] = " + intMonth + " ";
            strSql = strSql + " AND PlanType = '" + strPlanType + "' ";
            if (strPlantID != "") { strSql = strSql + " AND PLANTID = '" + strPlantID + "' "; }
            if (strProjectName != "") { strSql = strSql + " AND PROJECT_NAME = '" + strProjectName + "' "; }
            if (strPartType != "") { strSql = strSql + " AND PartType = '" + strPartType + "' "; }
            strSql = strSql + " ORDER BY [Year],[Month],PlanType,PLANTID,PROJECT_NAME,PartType";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMSAPPlanRateM(int intYear, int intMonth, string strTP, string strSHOP)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT [Year],[Month],TP_PART_NUM,SHOP,PlanRate";
            strSql = strSql + " ,Userid,CONVERT(VARCHAR(30),Time_Stamp,120) AS Time_Stamp,PCName FROM dbo.MSAPPlanRateM";
            strSql = strSql + " WHERE [Year]= " + intYear + " ";
            strSql = strSql + " AND [Month] = " + intMonth + " ";
            if (strTP != "") { strSql = strSql + " AND TP_PART_NUM = '" + strTP + "' "; }
            if (strSHOP != "") { strSql = strSql + " AND SHOP = '" + strSHOP + "' "; }
            strSql = strSql + " ORDER BY [Year],[Month],TP_PART_NUM";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMGDM(string strGroup,string strDefect)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT * FROM MGDM";
            strSql = strSql + " WHERE 1=1 ";
            if (strGroup != "")
            {
                strSql = strSql + " AND DGROUP= '" + strGroup + "' ";
            }
            if (strDefect != "")
            {
                strSql = strSql + " AND DEFECT = '" + strDefect  + "' ";
            }    
            strSql = strSql + " ORDER BY DEFECT";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMKWMM(string strGroup, string strDefect)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT * FROM MKWMM";
            strSql = strSql + " WHERE 1=1 ";
            if (strGroup != "")
            {
                strSql = strSql + " AND OWNER like '%" + strGroup + "%'";
            }
            if (strDefect != "")
            {
                strSql = strSql + " AND DEFECTCD  like '%" + strDefect + "%' ";
            }
            strSql = strSql + " ORDER BY CMT,OWNER,DEFECTCD";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMKWBM(string strStation,string strGroup, string strDefect)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT * FROM MKWBM";
            strSql = strSql + " WHERE 1=1 ";
            if (strStation  != "")
            {
                strSql = strSql + " AND WORKCTR like '%" + strStation  + "%'";
            }
            if (strGroup != "")
            {
                strSql = strSql + " AND OWNER like '%" + strGroup + "%'";
            }
            if (strDefect != "")
            {
                strSql = strSql + " AND DEFECTCD  like '%" + strDefect + "%' ";
            }
            strSql = strSql + " ORDER BY WORKCTR,OWNER,DEFECTCD";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public double GetPlanRate(DataTable dtTmp, string strPlanMonth, string strPlantID, string strProjectName, string strPartType, string strPlanType)
    {
        try
        {
            double dblRate = 2;
            DataRow[] drTmp = dtTmp.Select("PlanMonth ='" + strPlanMonth + "' AND PlanType ='" + strPlanType + "' AND PLANTID='" + strPlantID + "' AND PROJECT_NAME = '" + strProjectName + "' AND PartType = '" + strPartType + "' ");
            if (drTmp.Length == 1) { dblRate = Convert.ToDouble(drTmp[0].ItemArray[5]); }
            return dblRate;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return 1;
        }
    }
    public DataTable GetSourceFab(string strFabID, string strTransMode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = " SELECT 'Total' AS ID,'' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT 'SH' AS ID,'上海' AS IDDesc UNION ALL";
            strSql = strSql + " SELECT DISTINCT SourceFab AS ID,SourceFab AS IDDesc FROM MSFM WHERE 1=1 ";
            if (strFabID != "") { strSql = strSql + " AND FABID = '" + strFabID + "' "; }
            if (strTransMode != "") { strSql = strSql + " AND TransMode = '" + strTransMode + "' "; }
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public DataTable GetMOQCDefectGroupM(string strGroup, string strDefectCode)
    {
        try
        {
            DBConn dbConn = new DBConn();
            string strSql = "";
            strSql = strSql + " SELECT DefectCode,DefectGroup,CMT,UserID,CONVERT(VARCHAR(30),Time_Stamp) AS Time_Stamp,PCName FROM MOQCDefectGroupM";
            strSql = strSql + " WHERE 1=1 ";
            if (strGroup != "")
            {
                strSql = strSql + " AND DefectGroup= '" + strGroup + "' ";
            }
            if (strDefectCode != "")
            {
                strSql = strSql + " AND DefectCode = '" + strDefectCode + "' ";
            }
            strSql = strSql + " ORDER BY DefectGroup,DefectCode";
            DataTable dtTmp = dbConn.GetDataSetBySql(strSql).Tables[0];
            return dtTmp;
        }
        catch (Exception ex)
        {
            SaveLog(CstrErrorLogType, ex.ToString());
            return null;
        }
    }
    public void GridViewOnMouse(GridView gvTmp)
    {
        for (int i = 0; i < gvTmp.Rows.Count; i++)
        {
            for (int j = 0; j < gvTmp.Columns.Count; j++)
            {
                gvTmp.Rows[i].Cells[j].Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
                gvTmp.Rows[i].Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            }
        }
    }
}