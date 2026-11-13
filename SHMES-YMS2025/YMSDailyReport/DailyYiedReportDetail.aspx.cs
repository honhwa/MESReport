using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using Dundas.Charting.WebControl;
using Dundas.Charting;
using System.Drawing;
using System.Collections.Generic;

 /* 2014-07-09  Innolux/SSC     V1.1      Add Grade A1,A2,L2,LR,BP,PC,PT,P5              V1.15  Modify
*/
public partial class Cell2DailYiedReport_Cell2DailYiedDetailReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strPCName = "";
            string strPCIP = "";
            string strLoginNTName = "";
          
            AppClass apClass = new AppClass();
            apClass.GetLoginInfo(out strPCName, out strPCIP, out strLoginNTName);
            tbPCName.Text = strPCIP + "/" + strPCName;
            tbUserName.Text = strLoginNTName;
           
            tbFABID.Text = this.Request.QueryString["FABID"].ToString().Trim();
            tbOperation.Text = this.Request.QueryString["OPERATION_ID"].ToString().Trim();
            tbPRODID.Text = this.Request.QueryString["PRODUCT_ID"].ToString().Trim();
            tbFDate.Text = this.Request.QueryString["FDate"].ToString().Trim();
            tbTDate.Text = this.Request.QueryString["TDate"].ToString().Trim();
            tbInput.Text = this.Request.QueryString["INPUT"].ToString().Trim();
            tbyield.Text = this.Request.QueryString["YIELD"].ToString().Trim();
            tbAAQTY.Text = this.Request.QueryString["AAQTY"].ToString().Trim();
            //DDLFABID.SelectedValue = tbFABID.Text;
            Tbmode.Text = this.Request.QueryString["mode"].ToString().Trim();
            TbSIZE.Text = this.Request.QueryString["SIZE"].ToString().Trim();

            Label2.Text = tbPRODID.Text;
            Label3.Text = tbFDate.Text.Replace("-", "").ToString() + "0730";
            //Label4.Text =Convert.ToDateTime (tbTDate.Text).AddDays(1).ToShortDateString();
            Label4.Text = Convert.ToDateTime(tbTDate.Text).AddHours(24).ToString("yyyyMMdd") + "0730";
            DDLFABID.SelectedValue = tbFABID.Text;
            GridViewBind();
            GetSubject();
        }
    }
    protected void GetSubject()
    {
        /**************************************************************************************************************************/
        string strTitle = "";
        string strBody = "";
        string strDateTime = "";
        AppClass apClass = new AppClass();
        if (apClass.QuerySubject("DailyYiedReportDetail", "DailyYiedReportDetail", out strTitle, out strBody, out strDateTime) == true)
        {
            lblTitle.Text = strTitle.ToString();
            tbBody.Text = strBody.ToString();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>RunFunc();</script>");
        }
        else
        {
            lblTitle.Text = "";
            tbBody.Text = "";
            strDateTime = "";
        }
        /**************************************************************************************************************************/
    }
    protected void GridViewBind()
    {
        StringBuilder strSql = new StringBuilder();
        StringBuilder strSql1 = new StringBuilder();
        StringBuilder strSq2 = new StringBuilder();
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        string strFABID = tbFABID.Text;
        string strOPERATION = tbOperation.Text;
        string strPRODID = tbPRODID.Text;
        string strFdate = tbFDate.Text;
        string strTDATE = tbTDate.Text;
        tbOperation.Text = strOPERATION.Substring(1, 4);
        //string strsqlLevel = "";
        /**-------------------------------------*/
        //strSql1.Remove(0, strSql1.Length);
        //strSql1.Append(" SELECT DISTINCT  LOT_LEVEL FROM NJ_TBL_LIGHTON_DEFECT WHERE   ReportDate >= '"+ tbFDate.Text+"'AND ReportDate <='"+tbTDate.Text+"'");
        //strSql1.Append("   AND FAB_ID ='" + strFABID + "'AND PRODUCT_ID ='" + tbPRODID.Text + "'AND OPERATION_ID='" + tbOperation.Text + "' AND LOT_GRADE IN('R','S')");
        //DataSet dt014 = new DataSet();
        //SqlDataAdapter sqlApt014 = new SqlDataAdapter(strSql1.ToString(), dbConn.ConnectionString);
        //sqlApt014.Fill(dt014);
        //DataRow[] drDef = dt014.Tables[0].Select();
        ////string strLEVEL = drDef[DefCnt].ItemArray[0].ToString();
        //if (drDef.Length > 0)
        //{
        //    for (int DefCnt = 0; DefCnt < Convert.ToInt32(drDef.Length); DefCnt++)
        //    {
        //        tbsqlLevel.Text = tbsqlLevel.Text + ",sum(case LOT_LEVEL when '" + drDef[DefCnt].ItemArray[0].ToString() + "' then CAST(DEF_CNT AS int)  else 0 end) [" + drDef[DefCnt].ItemArray[0].ToString() + "]";
        //        tbsqlLevelA.Text = tbsqlLevelA.Text + ",'0'[" + drDef[DefCnt].ItemArray[0].ToString() + "]";
        //        tbsqlLevelB.Text = tbsqlLevelB.Text + ",SUM(" + drDef[DefCnt].ItemArray[0].ToString() + ") AS " +drDef[DefCnt].ItemArray[0].ToString();
        //        tbsqlLevelC.Text = tbsqlLevelC.Text + "," + drDef[DefCnt].ItemArray[0].ToString();
        //    }
        //}

        //strSql.Remove(0, strSql.Length);

        ////strSql.Append(" SELECT * ,CAST(Convert(NUMERIC(18,2), LOSSQTY/( SELECT SUM(DEF_CNT)+'" + tbAAQTY.Text + "'FROM NJ_TBL_LIGHTON_DEFECT ");
        //strSql.Append(" SELECT LOSSCODE");
        //strSql.Append(tbsqlLevelC.Text);
        //strSql.Append(" ,總計,lossqty,CAST(Convert(NUMERIC(18,2), LOSSQTY/( SELECT SUM(DEF_CNT)+'" + tbAAQTY.Text + "'FROM NJ_TBL_LIGHTON_DEFECT ");
        //strSql.Append("  WHERE FAB_ID='" + strFABID + "'  AND PRODUCT_ID ='" + tbPRODID.Text + "'");
        //strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
        //strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");
        //strSql.Append(") *100) AS NVARCHAR(20)) +'%'  AS lossRATE from ( ");

        //strSql.Append(" SELECT  'a' AS AB,DEF_CODE+DEFECTDESC  AS LOSSCODE,Convert(NUMERIC(18,0),'0') as AA");
        //strSql.Append(tbsqlLevel.Text);
        //strSql.Append(" ,SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY");
        //strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='"+tbPRODID.Text+"'");
        //strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
        //strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");
        //strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
        //strSql.Append(" UNION ALL");
        //strSql.Append(" SELECT  TOP 1 'b'AS AB,'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + " AS INT)AS AA");
        //strSql.Append(tbsqlLevelA.Text);
        //strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty ");
        //strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "'");
        ///**-------------------------------------*/
        //strSql.Append(" UNION ALL");
        //strSql.Append(" SELECT 'c'AS AB,'總計' AS LOSSCODE,SUM(AA) AS AA");
        //strSql.Append(tbsqlLevelB.Text);
        //strSql.Append(" ,SUM(總計) AS 總計,SUM(lossqty)  ");
        //strSql.Append(" FROM (");
        //strSql.Append(" SELECT  DEF_CODE+DEFECTDESC  AS LOSSCODE,CAST(0 AS INT) as AA");
        //strSql.Append(tbsqlLevel.Text);
        //strSql.Append(", SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY");
        //strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
        //strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
        //strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");
        //strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
        //strSql.Append(" UNION ALL");
        //strSql.Append(" SELECT  TOP 1'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + "AS INT)AS  AA");
        //strSql.Append(tbsqlLevelA.Text);
        //strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty");
        //strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "')T) TT ORDER BY ab ,TT.lossQTY desc");
        if (Tbmode.Text == "13碼料號")
        {
            strSql1.Remove(0, strSql1.Length);
            strSql1.Append(" SELECT DISTINCT  LOT_LEVEL FROM NJ_TBL_LIGHTON_DEFECT A WHERE   ReportDate >= '" + tbFDate.Text + "'AND ReportDate <='" + tbTDate.Text + "'");
            strSql1.Append("   AND FAB_ID ='" + strFABID + "'AND PRODUCT_ID ='" + tbPRODID.Text + "'AND OPERATION_ID='" + tbOperation.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  //V1.1 Modify
        }
        else if (Tbmode.Text == "PanelSize")
        {
            strSql1.Remove(0, strSql1.Length);
            strSql1.Append(" SELECT DISTINCT lot_level FROM (  SELECT  CASE WHEN  LEFT(T1.PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(T1.PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(T1.PALLETID,13,4))) ");
            strSql1.Append("  WHEN LEFT (T1.PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(T1.PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(T1.PALLETID,11,LEN(T1.PALLETID)))) ");
            strSql1.Append("  ELSE '' END AS product_id,LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE FROM dbo.NJ_TBL_LIGHTON_DEFECT  t1) A WHERE");
            strSql1.Append("  ReportDate >= '" + tbFDate.Text + "'AND ReportDate <='" + tbTDate.Text + "'");
            strSql1.Append("  AND FAB_ID ='" + strFABID + "'AND PRODUCT_ID ='" + tbPRODID.Text + "'AND OPERATION_ID='" + tbOperation.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  //V1.1 Modify
        }
        else
        {
            strSql1.Remove(0, strSql1.Length);
            strSql1.Append(" SELECT DISTINCT lot_level FROM (  SELECT  CASE WHEN  LEFT(T1.PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(T1.PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(T1.PALLETID,13,4))) ");
            strSql1.Append("  WHEN LEFT (T1.PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(T1.PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(T1.PALLETID,11,LEN(T1.PALLETID)))) ");
            strSql1.Append("  ELSE '' END AS SIZE,LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,PRODUCT_ID FROM dbo.NJ_TBL_LIGHTON_DEFECT  t1) A WHERE");
            strSql1.Append("  ReportDate >= '" + tbFDate.Text + "'AND ReportDate <='" + tbTDate.Text + "'");
            strSql1.Append("  AND FAB_ID ='" + strFABID + "'AND PRODUCT_ID ='" + tbPRODID.Text + "'AND OPERATION_ID='" + tbOperation.Text + "'AND SIZE='" + TbSIZE.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  //V1.1 Modify
        }
        DataSet dt014 = new DataSet();
        SqlDataAdapter sqlApt014 = new SqlDataAdapter(strSql1.ToString(), dbConn.ConnectionString);
        sqlApt014.Fill(dt014);
        DataRow[] drDef = dt014.Tables[0].Select();
        if (drDef.Length > 0)
        {
            for (int DefCnt = 0; DefCnt < Convert.ToInt32(drDef.Length); DefCnt++)
            {
                tbsqlLevel.Text = tbsqlLevel.Text + ",sum(case LOT_LEVEL when '" + drDef[DefCnt].ItemArray[0].ToString() + "' then CAST(DEF_CNT AS int)  else 0 end) [" + drDef[DefCnt].ItemArray[0].ToString() + "]";
                tbsqlLevelA.Text = tbsqlLevelA.Text + ",'0'[" + drDef[DefCnt].ItemArray[0].ToString() + "]";
                tbsqlLevelB.Text = tbsqlLevelB.Text + ",SUM(" + drDef[DefCnt].ItemArray[0].ToString() + ") AS " + drDef[DefCnt].ItemArray[0].ToString();
                tbsqlLevelC.Text = tbsqlLevelC.Text + "," + drDef[DefCnt].ItemArray[0].ToString();
            }
        }
        if (Tbmode.Text == "13碼料號")
        {
            strSql.Remove(0, strSql.Length);
            strSql.Append(" SELECT LOSSCODE,AA");
            strSql.Append(tbsqlLevelC.Text);
            strSql.Append(" ,總計,lossqty,CAST(Convert(NUMERIC(18,2), LOSSQTY/( SELECT SUM(DEF_CNT)+'" + tbAAQTY.Text + "'FROM NJ_TBL_LIGHTON_DEFECT ");
            strSql.Append("  WHERE FAB_ID='" + strFABID + "'  AND PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
           // strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')"); V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  //V1.15 ADD
            strSql.Append(") *100) AS NVARCHAR(20)) +'%'  AS lossRATE from ( ");

            strSql.Append(" SELECT 'a' AS AB, DEF_CODE+DEFECTDESC  AS LOSSCODE,Convert(NUMERIC(18,0),'0') as AA");
            strSql.Append(tbsqlLevel.Text);
            strSql.Append(" ,SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY");
            strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
           // strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");  V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  // V1.15 ADD
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  TOP 1 'b'AS AB,'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + " AS INT)AS AA");
            strSql.Append(tbsqlLevelA.Text);
            strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty ");
            strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "'");
            /**-------------------------------------*/
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  'c'AS AB,'總計' AS LOSSCODE,SUM(AA) AS AA");
            strSql.Append(tbsqlLevelB.Text);
            strSql.Append(" ,SUM(總計) AS 總計,SUM(lossqty)  ");
            strSql.Append(" FROM (");
            strSql.Append(" SELECT  DEF_CODE+DEFECTDESC  AS LOSSCODE,CAST(0 AS INT) as AA");
            strSql.Append(tbsqlLevel.Text);
            strSql.Append(", SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY");
            strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
           // strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");  V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");   // V1.15 ADD
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  TOP 1'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + "AS INT)AS  AA");
            strSql.Append(tbsqlLevelA.Text);
            strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty");
            strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "')T)TT  ORDER BY ab ,TT.lossQTY desc");
        }
        else if (Tbmode.Text == "PanelSize")
        {
            strSql.Remove(0, strSql.Length);
            strSql.Append(" SELECT LOSSCODE,AA");
            strSql.Append(tbsqlLevelC.Text);
            strSql.Append(" ,總計,lossqty,CAST(Convert(NUMERIC(18,2), LOSSQTY/( SELECT SUM(DEF_CNT)+'" + tbAAQTY.Text + "'FROM ");
            strSql.Append("  (SELECT CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
            strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID)))) ELSE '' END AS PRODUCT_ID ");
            strSql.Append(",LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,DEF_CNT  FROM  NJ_TBL_LIGHTON_DEFECT WHERE PALLETID!='')  A  ");
            strSql.Append("  WHERE FAB_ID='" + strFABID + "'  AND PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
           // strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");  V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5')) ");   // V1.15 ADD
            strSql.Append(") *100) AS NVARCHAR(20)) +'%'  AS lossRATE from ( ");

            strSql.Append(" SELECT 'a' AS AB, DEF_CODE+DEFECTDESC  AS LOSSCODE,Convert(NUMERIC(18,0),'0') as AA");
            strSql.Append(tbsqlLevel.Text);
            strSql.Append(" ,SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY FROM ");
            strSql.Append("  (SELECT CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
            strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID)))) ELSE '' END AS PRODUCT_ID ");
            strSql.Append(",LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,DEF_CNT  FROM  NJ_TBL_LIGHTON_DEFECT WHERE PALLETID!='')  A  ");
            strSql.Append("  ,MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
          //  strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");  V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5')) ");   // V1.15 ADD
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  TOP 1 'b'AS AB,'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + " AS INT)AS AA");
            strSql.Append(tbsqlLevelA.Text);
            strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty ");
            strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "'");
            /**-------------------------------------*/
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  'c'AS AB,'總計' AS LOSSCODE,SUM(AA) AS AA");
            strSql.Append(tbsqlLevelB.Text);
            strSql.Append(" ,SUM(總計) AS 總計,SUM(lossqty)  ");
            strSql.Append(" FROM (");
            strSql.Append(" SELECT  DEF_CODE+DEFECTDESC  AS LOSSCODE,CAST(0 AS INT) as AA");
            strSql.Append(tbsqlLevel.Text);
            strSql.Append(", SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY FROM ");
            strSql.Append("  (SELECT CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
            strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID)))) ELSE '' END AS PRODUCT_ID ");
            strSql.Append(",LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,DEF_CNT  FROM  NJ_TBL_LIGHTON_DEFECT WHERE PALLETID!='')  A  ");
            strSql.Append(" ,MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'");
           // strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");  V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5')) ");   //V1.15 ADD 
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  TOP 1'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + "AS INT)AS  AA");
            strSql.Append(tbsqlLevelA.Text);
            strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty");
            strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "')T)TT  ORDER BY ab ,TT.lossQTY desc");
        }
        else
        {
            strSql.Remove(0, strSql.Length);
            strSql.Append(" SELECT LOSSCODE,AA");
            strSql.Append(tbsqlLevelC.Text);
            strSql.Append(" ,總計,lossqty,CAST(Convert(NUMERIC(18,2), LOSSQTY/( SELECT SUM(DEF_CNT)+'" + tbAAQTY.Text + "'FROM ");
            strSql.Append("  (SELECT CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
            strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID)))) ELSE '' END AS SIZEID ");
            strSql.Append(",LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,DEF_CNT,PRODUCT_ID  FROM  NJ_TBL_LIGHTON_DEFECT )  A  ");
            strSql.Append("  WHERE FAB_ID='" + strFABID + "'  AND PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'AND SIZEID='" + TbSIZE.Text + "'");
            //strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')"); // V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  // V1.15 ADD
            strSql.Append(") *100) AS NVARCHAR(20)) +'%'  AS lossRATE from ( ");

            strSql.Append(" SELECT 'a' AS AB, DEF_CODE+DEFECTDESC  AS LOSSCODE,Convert(NUMERIC(18,0),'0') as AA");
            strSql.Append(tbsqlLevel.Text);
            strSql.Append(" ,SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY FROM ");
            strSql.Append("  (SELECT CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
            strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID)))) ELSE '' END AS SIZEID ");
            strSql.Append(",LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,DEF_CNT,PRODUCT_ID  FROM  NJ_TBL_LIGHTON_DEFECT )  A  ");
            strSql.Append("  ,MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'AND SIZEID='" + TbSIZE.Text + "'");
           // strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')");  V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  // V1.15 ADD
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  TOP 1 'b'AS AB,'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + " AS INT)AS AA");
            strSql.Append(tbsqlLevelA.Text);
            strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty ");
            strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "'");
            /**-------------------------------------*/
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  'c'AS AB,'總計' AS LOSSCODE,SUM(AA) AS AA");
            strSql.Append(tbsqlLevelB.Text);
            strSql.Append(" ,SUM(總計) AS 總計,SUM(lossqty)  ");
            strSql.Append(" FROM (");
            strSql.Append(" SELECT  DEF_CODE+DEFECTDESC  AS LOSSCODE,CAST(0 AS INT) as AA");
            strSql.Append(tbsqlLevel.Text);
            strSql.Append(", SUM(CAST(DEF_CNT AS int) ) AS 總計,SUM(CAST(DEF_CNT AS int) ) AS lossQTY FROM ");
            strSql.Append("  (SELECT CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
            strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID)))) ELSE '' END AS SIZEID ");
            strSql.Append(",LOT_LEVEL,OPERATION_ID, LOT_GRADE ,FAB_ID,ReportDate,DEF_CODE,DEF_CNT,PRODUCT_ID  FROM  NJ_TBL_LIGHTON_DEFECT)  A  ");
            strSql.Append(" ,MDCM B WITH(NOLOCK)  WHERE A.FAB_ID='" + strFABID + "'  AND a.PRODUCT_ID ='" + tbPRODID.Text + "'");
            strSql.Append(" AND OPERATION_ID='" + tbOperation.Text + "'AND SIZEID='" + TbSIZE.Text + "'");
            //strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND LOT_GRADE IN('R','S')"); V1.15 Mark
            strSql.Append(" AND ReportDate>='" + tbFDate.Text + "'AND ReportDate<='" + tbTDate.Text + "' AND (LOT_GRADE IN('R','S') or LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  // V1.15 ADD 
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD group by DEF_CODE,DEFECTDESC ");
            strSql.Append(" UNION ALL");
            strSql.Append(" SELECT  TOP 1'N0LOSSS' AS LOSSCODE,CAST(" + tbAAQTY.Text + "AS INT)AS  AA");
            strSql.Append(tbsqlLevelA.Text);
            strSql.Append(",CAST(" + tbAAQTY.Text + "AS INT)AS 總計,CAST(0 AS INT)AS lossqty");
            strSql.Append(" FROM MDCM  WITH(NOLOCK)  WHERE FABID='" + strFABID + "')T)TT  ORDER BY ab ,TT.lossQTY desc");
        }
         DataSet dt011 = new DataSet();
         SqlDataAdapter sqlApt011 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
         sqlApt011.Fill(dt011);
         if (dt011.Tables[0].Rows.Count == 0)
        {
            this.Response.Write("<script language=javascript>window.alert('查無資料');window.close()</script>");
            return;
        }

        BoudDefectDataTable(dt011);
    }
    private void BoudDefectDataTable(DataSet dsTmp)
    {
        try
        {
            DataRow newRow = dsTmp.Tables[0].NewRow();
            DataRow newRow1 = dsTmp.Tables[0].NewRow();
            DataRow newRow2= dsTmp.Tables[0].NewRow();
            DataRow newRow3 = dsTmp.Tables[0].NewRow();
            DataRow[] drDef = dsTmp.Tables[0].Select("losscode='總計'");

            int i = dsTmp.Tables[0].Columns.Count;
            int j = dsTmp.Tables[0].Rows.Count;
            if (drDef.Length > 0)
            {
                 newRow["LOSSCODE"]= "Grade Rate";
                 for (int Deffield = 1; Deffield < i - 3; Deffield++)
                 {
                     string name = dsTmp.Tables[0].Columns[Deffield].ColumnName;
                     newRow[name] = Math.Round(Convert.ToDouble(drDef[0].ItemArray[Deffield]) / Convert.ToDouble((drDef[0].ItemArray[i - 3])), 4) * 100;
                   
                 }
                    dsTmp.Tables[0].Rows.Add(newRow);
                    newRow1["LOSSCODE"] = "(N,D)(A+B+C)";
                    newRow1[dsTmp.Tables[0].Columns[1].ColumnName] =Convert.ToDouble(drDef[0].ItemArray[1]);
                    dsTmp.Tables[0].Rows.Add(newRow1);

                    newRow2["LOSSCODE"] = "INPUT";
                    newRow2[dsTmp.Tables[0].Columns[1].ColumnName] =Convert.ToDouble((drDef[0].ItemArray[i - 3]));
                    dsTmp.Tables[0].Rows.Add(newRow2);

                    newRow3["LOSSCODE"] = "Yield";
                    newRow3[dsTmp.Tables[0].Columns[1].ColumnName] =Math.Round(Convert.ToDouble(drDef[0].ItemArray[1]) / Convert.ToDouble((drDef[0].ItemArray[i - 3])), 4) * 100;
                    dsTmp.Tables[0].Rows.Add(newRow3);
            }

            GridView1.DataSource = dsTmp.Tables[0];
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ExportExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "UTF-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + DDLFABID.SelectedItem.Text + "addNewWorker.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        this.GridView1.RenderControl(oHtmlTextWriter);
        Response.Output.Write(oStringWriter.ToString());
        Response.Flush();
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[0].Text.ToString() == "Grade Rate")
        {
            for (int i = 1; i < e.Row.Cells.Count - 3; i++)
            {
                e.Row.Cells[i].Text = e.Row.Cells[i].Text + "%";
            }
        }

        if (e.Row.Cells[0].Text.ToString() == "Yield")
        {
            e.Row.Cells[1].Text = e.Row.Cells[1].Text + "%";
        }
    }
}
