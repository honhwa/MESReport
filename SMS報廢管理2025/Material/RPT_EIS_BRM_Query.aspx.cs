using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Material_RPT_EIS_BRM_Query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strName = "";
            if (Request.QueryString["Name"] != null) { strName = this.Request.QueryString["Name"].ToString().ToUpper(); }
            QueryData(strName);
        }
    }
    protected void QueryData(string strName)
    {
        ModuleClass mc = new ModuleClass();
        DBConn dbConn = new DBConn();
        DataTable dtQuery = new DataTable();

        string strSql = "";
        if (strName == "DFRATE".ToUpper())
        {
            strSql = strSql + " SELECT CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID,T1.REPORTYEAR,T1.REPORTMONTH,T1.SHOPCD,T1.DEFECTCODE,REPLACE(REPLACE(ISNULL(T2.DEFECTDESC,''),CHAR(10),''),CHAR(13),'') AS DEFECTDESC,T1.RATE";
            strSql = strSql + " FROM dbo.MDCRATEM T1";
            strSql = strSql + " LEFT JOIN dbo.MDCM T2 ON(T1.FABID = T2.FABID AND T1.DEFECTCODE=T2.DEFECTCD)";
            strSql = strSql + " WHERE CAST(REPORTYEAR AS VARCHAR)+'-'+RIGHT('0'+CAST(REPORTMONTH AS VARCHAR),2) >= CONVERT(VARCHAR(7),DATEADD(YEAR,-1,GETDATE()),120)";
            strSql = strSql + " ORDER BY CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END,T1.REPORTYEAR,T1.REPORTMONTH,T1.SHOPCD,T1.DEFECTCODE";
        }
        if (strName == "BOM".ToUpper())
        {
            strSql = strSql + " SELECT CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID,T1.PRODID,T1.PARTTYPE,T1.PARTID FROM MMPM T1";
            strSql = strSql + " WHERE LEN(T1.PRODID)=13";
            strSql = strSql + " ORDER BY CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END,T1.PRODID,T1.PARTTYPE";
        }
        if (strName == "PI".ToUpper())
        {
            strSql = strSql + " SELECT CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID,T1.PRODID,T1.SIZEID,T1.SHOP_CODE,T1.PROJECT_NAME FROM MPIM T1";
            strSql = strSql + " WHERE T1.SHOP_CODE!='BL' AND LEN(T1.PRODID)=13";
            strSql = strSql + " ORDER BY CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END,T1.PRODID";
        }
        if (strName == "SF".ToUpper())
        {
            strSql = strSql + " SELECT CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID,T1.PRODID,T1.SourceFab FROM MSFM T1";
            strSql = strSql + " WHERE LEN(T1.PRODID)=13";
            strSql = strSql + " ORDER BY CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END,T1.PRODID";
        }
        if (strName == "PMSP".ToUpper())
        {
            strSql = strSql + " SELECT CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END AS FABID,T1.PRODID,T1.T9NCNO,T1.F9NCNO FROM MPM9M T1";
            strSql = strSql + " WHERE LEN(T1.PRODID)=13";
            strSql = strSql + " ORDER BY CASE T1.FABID WHEN 'B' THEN 'WE01' ELSE '' END,T1.PRODID,T1.F9NCNO";
        }
        dtQuery = dbConn.GetDataSetBySql(strSql).Tables[0];

        mc.ExportExcel(dtQuery);
    }
}