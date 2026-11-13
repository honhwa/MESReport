using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Report.Library;

using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Dundas.Charting;
using System.Drawing;
using Dundas.Charting.WebControl;
using System.Text.RegularExpressions;

namespace Report.Reports.TraceAbility
{
    public partial class OneClickQuery : System.Web.UI.Page
    {
        const int mlyear = 3;
        const int Im = 0;
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }

                string strLoginPCName = "";
                string strLoginPCIP = "";
                string strLoginNTName = "";
                tbLoginNTName.Text = "";
                tbLoginPCIP.Text = "";
                tbLoginPCName.Text = "";

                AppClass Apc = new AppClass();
                Apc.GetLoginInfo(out strLoginPCName, out strLoginPCIP, out strLoginNTName);

                tbFABID.Text = "B";
                tbLoginNTName.Text = strLoginNTName;
                tbLoginPCIP.Text = strLoginPCIP;
                tbLoginPCName.Text = strLoginPCName;
                //HyperLink1.NavigateUrl = "~/MainButton.aspx?FABID=" + tbFABID.Text.Trim();
                Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Browse", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Choose Report", tbFABID.Text.ToString(), "");
            }

            //if (Session["Login"] == null)
            //{
            //    tbFABID.Text = "";
            //    tbFABName.Text = "";
            //    Page.Response.Redirect(@"~\Default.aspx");
            //    return;
            //}
            //if (!IsPostBack)
            //{
            //    AppClass Apc = new AppClass();
            //    if (Request.QueryString["FABID"] != null)
            //    {
            //        tbFABID.Text = this.Request.QueryString["FABID"].ToString();
            //        if (tbFABID.Text == "B")
            //        {
            //            tbFABName.Text = "SH";
            //        }
            //        string strLoginPCName = "";
            //        string strLoginPCIP = "";
            //        string strLoginNTName = "";
            //        tbLoginNTName.Text = "";
            //        tbLoginPCIP.Text = "";
            //        tbLoginPCName.Text = "";

            //        Apc.GetLoginInfo(out strLoginPCName, out strLoginPCIP, out strLoginNTName);

            //        tbLoginNTName.Text = strLoginNTName;
            //        tbLoginPCIP.Text = strLoginPCIP;
            //        tbLoginPCName.Text = strLoginPCName;
            //        HyperLink1.NavigateUrl = "~/MainButton.aspx?FABID=" + tbFABID.Text.Trim();
            //        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Browse", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Choose Report", tbFABID.Text.ToString(), "");
            //    }
            //    else
            //    {
            //        Page.Response.Redirect(@"~\Default.aspx");
            //    }
            //    if (tbFABName.Text == "")
            //    {
            //        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('廠別資訊不正確,無法使用！');</script>");
            //        Page.Response.Redirect(@"~\Default.aspx");
            //        return;
            //    }
            //}

        }

        private void boundyear(DropDownList ddlyear)
        {
            string stryear;
            long lyear;
            long lloop;
            ddlyear.Items.Clear();
            stryear = System.DateTime.Now.Year.ToString();
            lyear = Convert.ToInt64(stryear);
            for (int i = 0; i < mlyear; i++)
            {
                lloop = lyear - i;
                ListItem lst = new ListItem(lloop.ToString(), lloop.ToString());
                ddlyear.Items.Add(lst);
            }

        }
        private void boundWD(DropDownList ddlwd, DropDownList ddltype, DropDownList ddlyear)
        {
            string stryear;
            string strtype;
            string strsql;
            int imonth;
            int iweek;
            int j;
            j = 1;
            DataSet dsMD = new DataSet();
            ddlwd.Items.Clear();
            stryear = ddlyear.SelectedValue.ToString();
            strtype = ddltype.SelectedValue.ToString();
            if (stryear == System.DateTime.Now.Year.ToString())
            {
                strsql = "select datepart(wk,getdate()) as Sweek,datepart(mm,getdate()) as Smonth";
                dsMD = dbConn.GetDataSetBySql(strsql);
                imonth = Convert.ToInt16(dsMD.Tables[0].Rows[0]["smonth"].ToString()) - 1;
                iweek = Convert.ToInt16(dsMD.Tables[0].Rows[0]["sweek"].ToString()) - 1;

            }
            else
            {
                imonth = 12;
                iweek = 55;
            }

            if (ddltype.SelectedValue == "W")
            {
                j = iweek;
            }
            if (ddltype.SelectedValue == "M")
            {
                j = imonth;
            }
            for (int i = 1; i <= j; i++)
            {
                ListItem lst = new ListItem(i.ToString(), i.ToString());
                ddlwd.Items.Add(lst);

            }

        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            DateTime dtBegin = DateTime.Now;
            txtENTITYID.Enabled = false;
            lblRunTime.Text = "";
            string strsql;
            string strENTITYID;
            DataSet dsrate = new DataSet();
            DataSet dsNUM = new DataSet();
            DataView dtdefect = new DataView();
            DataView TBdefect = new DataView();
            DataTable disdefect = new DataTable();
            AppClass Apc = new AppClass();

            strENTITYID = txtENTITYID.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
            //strENTITYID = txtENTITYID.Text.ToUpper().Trim();

            if (strENTITYID == "")
            {
                lblerror.Text = "輸入查詢panelID為空!!";
                return;
            }

            if (Regex.Matches(strENTITYID, ",").Count > 4)
            {
                lblerror.Text = "一次最多查詢5個PanelID!!";
                return;
            }

            string strSql = "";
            strSql = strSql + "select * from RPTUSELOGM where RPTName = '" + HttpContext.Current.Request.Url.AbsolutePath.ToString() + "' and opt3 = '0'";
            dsNUM = dbConn.GetDataSetBySql(strSql);
            if (dsNUM.Tables[0].Rows.Count > 3)
            {
                lblerror.Text = "不得多於 3 人同時查詢這隻報表，請稍後再查 !!";
                return;
            }

            lblqueryRS.Text = "當前有 " + dsNUM.Tables[0].Rows.Count.ToString() + " 人在查詢！";

            Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnquery_Click", "", "0");

            Fresh();
            PNLMAIN.Visible = true;

            lblerror.Text = "";

            //查詢OEE資料
            DataTable dt = GetAllOEEData(strENTITYID);
            if (dt != null)
            {
                gvOEE.DataSource = dt;
                gvOEE.DataBind();
            }
            

            strsql = getSQL(strENTITYID);
            //dsrate = dbConn.GetDataSetBySp("usp_rpt_yield_dataset", para);
            //绑定投入产出良率数据
            dsrate = dbConn.GetDataSetBySql(strsql);
            if (dsrate.Tables[0].Rows.Count == 0)
            {
                lblerror.Text = "No Data!!";
                PNLMAIN.Visible = false;
            }

            else
            {
                lblerror.Text = "";
            }
            gvmain.DataSource = dsrate.Tables[0];
            gvmain.DataBind();

            DateTime dtEnd = DateTime.Now;
            TimeSpan tsDiff = dtEnd - dtBegin;
            lblRunTime.Text = "查詢耗時:" + tsDiff.TotalSeconds.ToString() + "s";

            Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnquery_Click", "", "1");

        }

        //抓取投入产出及良率汇总表
        private string getSQL(string strENTITYID)
        {
            string strsql;
            DataSet dsDATE = new DataSet();


            strsql = string.Format(@"--'{0}'
                              

                                    DECLARE @STARTTIME AS VARCHAR(30);
                                    DECLARE @ENDTIME AS VARCHAR(30);
                                    DECLARE @SPLTENDTIME AS VARCHAR(30);
                                    DECLARE @CARTIDALL AS VARCHAR(8000);
                                    DECLARE @sqlcartidDN AS VARCHAR(8000);
                                    DECLARE @sqlOBA AS VARCHAR(8000);
                                    DECLARE @sql AS VARCHAR(8000);

                                    if object_id('tempdb..#VWLTXA1') is not null drop table #VWLTXA1
                                    if object_id('tempdb..#VWLTXA2') is not null drop table #VWLTXA2
                                    if object_id('tempdb..#VWLTXAA') is not null drop table #VWLTXAA
                                    if object_id('tempdb..#VWLTXA') is not null drop table #VWLTXA
                                    if object_id('tempdb..#VWPKXA') is not null drop table #VWPKXA
                                    if object_id('tempdb..#WEQPARAM') is not null drop table #WEQPARAM
                                    if object_id('tempdb..#data') is not null drop table #data
                                    if object_id('tempdb..#Sorting') is not null drop table #Sorting
                                    if object_id('tempdb..#Rework0') is not null drop table #Rework0
                                    if object_id('tempdb..#Rework') is not null drop table #Rework
                                    if object_id('tempdb..#labelRPNT') is not null drop table #labelRPNT
                                    if object_id('tempdb..#WLTMWIP') is not null drop table #WLTMWIP

                                    --生產履歷
                                    SELECT DISTINCT A.entityid,A.VERB,A.TRANSID,A.TRANSDATE,A.FRENTITYID,A.TOENTITYID,A.TRACKINDATE,A.MLOTID,A.PTRACKOUTDATE,A.PWORKCTR,A.CWORKCTR,A.NWORKCTR,A.FRPRODID,A.TOPRODID
                                    ,A.FRWONBR,A.TOWONBR,A.MLOTID MLOTID1,A.FRPENTITYUNIT,A.TOPENTITYUNIT,A.FRENTITYUNIT,A.TOENTITYUNIT
                                    ,A.ENTITYCARRY,A.CENTITYTQTY,A.NENTITYTQTY,A.FRWOTYPE,A.TOWOTYPE,A.FRLOTTYPE,A.TOLOTTYPE,A.FRLOTGRADE,A.TOLOTGRADE
                                    ,A.FRLOTLEVEL,A.TOLOTLEVEL,A.CSUBLINEID,A.NSUBLINEID,A.USERID,A.SHIFTID,A.CUSTOMERID,A.SALEORDER,A.TMPFIELD
                                    ,ISNULL(A.CARTID,'') CARTID
                                    INTO #VWLTXA1
                                        FROM (
                                        SELECT v.TOENTITYID AS entityid,v.* FROM New_VWLTXA AS v WITH(NOLOCK)
                                        WHERE v.FABID = 'B' 
                                        AND v.TRANSDATE BETWEEN GETDATE() - 365*3 AND GETDATE()
                                        AND v.TOENTITYID IN('{0}')
    
                                        UNION ALL
                                        SELECT v.FRENTITYID AS entityid,v.* FROM New_VWLTXA AS v WITH(NOLOCK)
                                        WHERE v.FABID = 'B' 
                                        AND v.TRANSDATE BETWEEN GETDATE() - 365*3 AND GETDATE()
                                        AND v.FRENTITYID IN('{0}') AND v.VERB <> 'SPLT'

                                        UNION ALL
                                        SELECT v2.FRENTITYID AS entityid,v.* FROM New_VWLTXA AS v WITH(NOLOCK)
                                        LEFT JOIN (
                                            SELECT va.TOENTITYID,va.FRENTITYID 
                                            FROM New_VWLTXA va WITH(NOLOCK) 
                                            WHERE va.FABID = 'B' AND va.TRANSDATE BETWEEN GETDATE() - 365*3 AND GETDATE() AND va.verb = 'MRLT' 
                                            AND va.FRENTITYID IN('{0}')) v2 ON v2.TOENTITYID = v.FRENTITYID
                                        WHERE v.FABID = 'B'
                                        AND v.TRANSDATE BETWEEN GETDATE() - 365*3 AND GETDATE()
                                        AND v.FRENTITYID IN(
                                            SELECT V1.TOENTITYID 
                                            FROM New_VWLTXA AS v1 WITH(NOLOCK) 
                                            WHERE v1.FABID = 'B' AND v1.TRANSDATE BETWEEN GETDATE() - 365*3 AND GETDATE() AND v1.VERB = 'MRLT' 
                                            AND v1.FRENTITYID IN('{0}')) AND V.VERB <> 'CRLT'
                                    ) A
                                    ORDER BY A.entityid,A.transid ASC

                                    --SELECT v.entityid,v.frentityid,v.toentityid, * FROM #VWLTXA1 v ORDER BY v.entityid,v.transid asc

                                    SELECT entityid, frentityid,towonbr,TRANSDATE
                                    INTO #VWLTXAA 
                                    FROM #VWLTXA1 WITH(NOLOCK) WHERE verb = 'SPLT'

                                    --SELECT * FROM #VWLTXAA v

                                    SELECT @SPLTENDTIME = MAX(TRANSDATE) FROM  #VWLTXAA
                                    --SELECT MAX(TRANSDATE) FROM  #VWLTXAA

                                    SELECT * 
                                    INTO #VWLTXA2 
                                    FROM (
                                    SELECT v.entityid AS entityid,A.VERB,A.TRANSID,A.TRANSDATE,A.FRENTITYID,A.TOENTITYID,A.TRACKINDATE,A.MLOTID,A.PTRACKOUTDATE,A.PWORKCTR,A.CWORKCTR,A.NWORKCTR,A.FRPRODID,A.TOPRODID
                                    ,A.FRWONBR,A.TOWONBR,A.MLOTID MLOTID1,A.FRPENTITYUNIT,A.TOPENTITYUNIT,A.FRENTITYUNIT,A.TOENTITYUNIT
                                    ,A.ENTITYCARRY,A.CENTITYTQTY,A.NENTITYTQTY,A.FRWOTYPE,A.TOWOTYPE,A.FRLOTTYPE,A.TOLOTTYPE,A.FRLOTGRADE,A.TOLOTGRADE
                                    ,A.FRLOTLEVEL,A.TOLOTLEVEL,A.CSUBLINEID,A.NSUBLINEID,A.USERID,A.SHIFTID,A.CUSTOMERID,A.SALEORDER,A.TMPFIELD
                                    ,ISNULL(A.CARTID,'') CARTID
                                    FROM New_VWLTXA A WITH(NOLOCK) 
                                    LEFT JOIN #VWLTXAA v ON A.toentityid = v.frentityid
                                    WHERE A.TRANSDATE BETWEEN GETDATE() - 365*3 AND @SPLTENDTIME 
                                    --WHERE A.TRANSDATE BETWEEN GETDATE() - 365*3 AND '2020-09-07 23:02:12.000'
                                    AND A.FABID = 'B' 
                                    AND A.towonbr IN(SELECT towonbr FROM #VWLTXAA WITH(NOLOCK))
                                    AND A.toentityid IN(SELECT frentityid FROM #VWLTXAA WITH(NOLOCK))
                                    AND A.TOENTITYID NOT IN (SELECT entityid FROM #VWLTXAA WITH(NOLOCK))
                                    ) B
                                    WHERE ((B.VERB <> 'CRLT' AND NWORKCTR NOT IN('CS0S','AS0S','MS0S'))
                                    OR (B.VERB = 'CRLT' AND B.CWORKCTR IN('5201','5203','5205','5207','5209')) 
                                    OR (B.VERB = 'CRLT' AND B.CWORKCTR NOT IN('5201','5203','5205','5207','5209') AND B.FRENTITYID IN(SELECT DISTINCT TOENTITYID FROM #VWLTXA1)))


                                    SELECT distinct * 
                                    INTO #VWLTXA 
                                    FROM(
                                        SELECT * FROM #VWLTXA1 
                                        UNION ALL 
                                        SELECT * FROM #VWLTXA2
                                    ) A
                                    --ORDER BY A.entityid,A.TRANSID ASC

                                    --SELECT * FROM #VWLTXA1
                                    --ORDER BY entityid,transid ASC
                                    --
                                    --SELECT * FROM #VWLTXA2
                                    --ORDER BY entityid,transid asc

                                    SELECT @STARTTIME = MIN(TRANSDATE),@ENDTIME = MAX(TRANSDATE) FROM #VWLTXA


                                    --是否經過Sorting
                                    SELECT DISTINCT ENTITYID, WOTYPE
                                    INTO #Sorting 
                                    FROM (
                                        SELECT ENTITYID,TOWOTYPE AS WOTYPE FROM #VWLTXA 
                                        WHERE TOWOTYPE = 'S' 
                                        UNION ALL 
                                        SELECT ENTITYID,FRWOTYPE AS WOTYPE FROM #VWLTXA 
                                        WHERE FRWOTYPE = 'S'
                                    ) A

                                    --是否經過Rework站點
                                    SELECT DISTINCT ENTITYID, CWORKCTR AS WORKCTR 
                                    INTO #Rework0
                                    FROM #VWLTXA
                                    WHERE CWORKCTR IN('MR0A','MR0B','MR0C','MR0L','MR0Q','MR0T')

                                    SELECT p2.ENTITYID, WORKCTRALL = STUFF((
                                                SELECT DISTINCT ',' + WORKCTR
                                                FROM #Rework0 AS p1
                                                WHERE p1.ENTITYID = p2.ENTITYID
                                                FOR XML PATH('')
                                                ), 1, 1, '') 
                                    INTO #Rework 
                                    FROM #Rework0 AS p2
                                    GROUP BY p2.ENTITYID

                                    --工位Kitting記錄
                                    SELECT A.FABID,A.ENTITYID,A.SubLineID,A.PRODID,A.OperationID,A.WORKCTR,A.TRANSDATE,A.USERID 
                                    INTO #WEQPARAM 
                                    FROM (
                                        SELECT w.FABID,w.ENTITYID,w.SubLineID,w.PRODID,w.OperationID,w.WORKCTR,w.TRANSDATE,w.USERID 
                                        FROM New_WEQPARAM AS w WITH(NOLOCK)
                                        WHERE w.FABID = 'B'  
                                        AND w.TRANSDATE BETWEEN @STARTTIME AND @ENDTIME 
                                        AND w.ENTITYID IN('{0}')
                                        UNION ALL
                                        SELECT w.FABID,w.ENTITYID,w.SubLineID,w.PRODID,w.OperationID,w.WORKCTR,w.TRANSDATE,w.USERID
                                        FROM WEQPARAMH AS w WITH(NOLOCK)
                                        WHERE w.FABID = 'B' 
                                        AND w.TRANSDATE BETWEEN @STARTTIME AND @ENDTIME 
                                        AND w.ENTITYID IN('{0}')
                                    ) A

                                    --物料記錄
                                    SELECT v.FABID,v.TRANSDATE,v.PRODID,v.ENTITYID,v.PARTID,v.PARTTYPE,v.PARTLOTID,v.SubLineID,v.WONBR,v.WORKCTR,v.USERID,v.SHIFTID,v.CMT,'' AS REWORK_TIMES
                                    INTO #VWPKXA
                                    FROM New_VWPKXA AS v WITH(NOLOCK)
                                    WHERE v.TRANSDATE BETWEEN @STARTTIME AND @ENDTIME
                                    AND v.ENTITYID IN('{0}')
                                    AND v.VERB = 'MKIT' 
                                    AND v.FABID = 'B'

                                    --將生產履歷、物料信息、工位Kitting記錄join在一起
                                    SELECT ROW_NUMBER() OVER(ORDER BY b.entityid, B.TRANSDATE, B.TRANSID,CASE  WHEN b.VERB1 = '' THEN 1 WHEN  b.VERB1 = 'MVOU' THEN 2 END,B.opttransdate) AS 'RowNumber'
                                    ,b.entityid
                                    ,B.TRANSID
                                    ,b.FRENTITYID,b.TOENTITYID
                                    ,B.TRACKINDATE,MLOTID
                                    ,ISNULL(CONVERT(VARCHAR(30),b.PTRACKOUTDATE,121),'') PTRACKOUTDATE
                                    ,b.PWORKCTR
                                    ,b.CWORKCTR,b.NWORKCTR
                                    ,b.VERB1 AS VERB
                                    ,b.TRANSDATE AS WORKCTRTRANSDATE
                                    ,b.USERID AS WORKCYRUSERID
                                    ,b.operationid,b.opttransdate,b.userid1 AS OPTUSERID
                                    ,b.partid,b.parttype,b.partlotid,b.parttransdate,b.partuserid
                                    ,b.FRPRODID,b.TOPRODID,b.FRWONBR,b.TOWONBR,b.MLOTID1,b.FRPENTITYUNIT
                                    ,b.TOPENTITYUNIT,b.FRENTITYUNIT,b.TOENTITYUNIT,b.ENTITYCARRY,b.CENTITYTQTY,b.NENTITYTQTY,b.FRWOTYPE
                                    ,b.TOWOTYPE,b.FRLOTTYPE,b.TOLOTTYPE,b.FRLOTGRADE,b.TOLOTGRADE,b.FRLOTLEVEL
                                    ,b.TOLOTLEVEL,b.CSUBLINEID,b.NSUBLINEID,b.SHIFTID,b.CUSTOMERID,b.SALEORDER,b.TMPFIELD
                                    ,B.CARTID
                                    ,B.WORKCTR
                                    ,B.sublineid
                                    INTO #data
                                    FROM(
                                        SELECT CASE WHEN a.operationid <> '' AND a.VERB = 'MVOU' THEN '' ELSE a.VERB END VERB1, a.*
                                        ,isnull(CONVERT(VARCHAR(30),va.transdate,121),'*') parttransdate
                                        ,isnull(va.partid,'*') AS partid
                                        ,isnull(va.parttype,'*') AS parttype
                                        ,isnull(va.partlotid,'*') AS partlotid
                                        ,isnull(a.sublineid,'*') AS partsublineid
                                        ,isnull(va.userid,'*') AS partuserid
                                        FROM(
                                            SELECT v.*,'*' AS operationid,'*' AS WORKCTR,'*' AS sublineid,'*' AS opttransdate,'*' AS userid1 FROM #VWLTXA v 
                                            WHERE v.verb <> 'MVOU' 
                                            UNION ALL 
                                            SELECT v.*,w.operationid,w.WORKCTR,w.sublineid,CONVERT(VARCHAR(30),w.transdate,121) AS opttransdate,w.userid AS userid1 FROM #VWLTXA v,#WEQPARAM w 
                                            WHERE v.entityid = w.entityid 
                                            AND v.verb = 'MVOU' 
                                            AND v.cworkctr = w.workctr 
                                            AND v.csublineid = w.sublineid 
                                            AND v.TRACKINDATE <= w.transdate AND v.TRANSDATE >= w.transdate 
                                        ) a 
                                        left join #VWPKXA va ON a.FRENTITYID = va.entityid AND a.CSUBLINEID = va.sublineid AND a.CWORKCTR = va.workctr AND a.opttransdate = CONVERT(VARCHAR(30),va.transdate,121)
                                        UNION ALL 
                                        SELECT va.verb verb1,va.*,'*' AS operationid,'*' AS WORKCTR,'*' AS sublineid,'*' AS opttransdate,'*' AS userid1,'*' AS parttransdate,'*' AS partid,'*' AS parttype,'*' AS partlotid,'*' AS partsublineid,'*' AS partuserid  FROM #VWLTXA va 
                                        WHERE va.verb = 'MVOU'
                                    ) b

                                    --SELECT * FROM #data

                                    --FG Barcode

                                    --SN

                                    --Label補印次數,最后一次補印工號,最后一次補印時間
                                    SELECT DISTINCT COUNT(0) PrintNum,w.Panelid,CONVERT(VARCHAR(30),MAX(w.TIME_STAMP),121) LastTime
                                    ,(SELECT TOP 1 w2.UserID FROM New_WRPNT AS w2 WHERE w2.Panelid = w.panelid AND w2.TIME_STAMP = MAX(w.TIME_STAMP)) userid
                                    INTO #labelRPNT
                                    FROM New_WRPNT AS w
                                    WHERE w.Panelid IN('{0}')
                                    GROUP BY w.Panelid 

                                    --目前WIP站點
                                    SELECT w.ENTITYID,w.CWORKCTR
                                    INTO #WLTMWIP 
                                    FROM New_WLTM AS w
                                    WHERE w.FABID = 'B' AND w.HIDEFLG = '0' AND w.ENTITYID IN('{0}')


                                    --SELECT @CARTIDALL = a.CARTIDALL FROM (
                                    --SELECT DISTINCT CARTIDALL = STUFF((
                                    --        SELECT DISTINCT ',''' + CARTID + ''''
                                    --        FROM #data AS p1 WHERE p1.CARTID<>''
                                    --        FOR XML PATH('')
                                    --        ), 1, 1, '')  
                                    --FROM #data AS p2 where p2.CARTID<>''
                                    --) a

                                    --在庫倉別

                                    --是否經過OBA
                                    --SET @sqlOBA = 'SELECT DISTINCT CARTID,CONVERT(VARCHAR(30),MIN(TIME_STAMP),121) OBATIME FROM OBA_DATA AS od WHERE od.CARTID IN(' + @CARTIDALL + ') GROUP BY CARTID'
                                    --
                                    --原始箱號
                                    --
                                    --OBA后箱號
                                    --

                                    --出貨信息(出貨DN,出貨時間,出貨客戶,客戶代碼)
                                    --SET @sqlcartidDN = 'SELECT * FROM dbo.wms_cartid_dn WHERE CARTID IN(' + @CARTIDALL + ')'

                                    
                                    SELECT distinct t.RowNumber,t.entityid
                                    ,cast(cast(isnull(datediff(s,(select top 1 workctrtransdate from #data where RowNumber < t.RowNumber order by RowNumber desc),workctrtransdate),0) AS DECIMAL(15,2))/60 AS DECIMAL(15,2)) AS intervaltime
                                    ,CASE WHEN S.WOTYPE IS NULL THEN 'N' WHEN S.WOTYPE = 'S' THEN 'Y' END AS Sorting
                                    ,CASE WHEN r.WORKCTRALL IS NULL THEN '' ELSE r.WORKCTRALL END AS Reworkworkctr
                                    ,isnull(L.PrintNum,'') AS LabelPrintNum
                                    ,isnull(L.userid,'') AS LabelPrintLastUserid
                                    ,isnull(L.LastTime,'') AS LabelPrintLastTime
                                    ,ISNULL(W.CWORKCTR,'') AS DQworkctr
                                    ,'NA' BANKID
                                    ,t.RowNumber
                                    ,t.entityid
                                    ,t.TRANSID
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE CONVERT(VARCHAR(30),t.TRACKINDATE,121) END TrackInDate
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.MLOTID END mlotid
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.PTRACKOUTDATE END PTrackOutDate
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.PWORKCTR END PWorkCtr
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.CWORKCTR END CWorkCtr
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.NWORKCTR END NWorkCtr
                                    ,t.VERB verb
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE CONVERT(VARCHAR(30),t.WORKCTRTRANSDATE,121) END TransDate
                                    ,t.WORKCYRUSERID userid
                                    ,t.sublineid SUBLINEID
                                    ,t.operationid OperationID
                                    ,t.opttransdate OPTRANSDATE
                                    ,t.WORKCTR WORKCTR
                                    ,t.OPTUSERID USERID
                                    ,'NA' SHIFTID1
                                    ,'NA' CMT
                                    ,'NA' Rework_times
                                    ,'NA' FGBarcode
                                    ,'NA' SN
                                    ,'NA' HOIDSTATE
                                    ,t.OPTUSERID
                                    ,t.partid PARTID
                                    ,t.parttype PARTTYPE
                                    ,t.partlotid PARTLOTID
                                    ,t.parttransdate
                                    ,t.partuserid
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.FRPRODID END FrProdID
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.TOPRODID END ToProdID
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.FRWONBR END FrWoNbr
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.TOWONBR END ToWoNbr
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.FRENTITYID END FrEntityID
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE t.TOENTITYID END ToEntityID
                                    --,t.MLOTID1 MLotID1
                                    ,t.FRPENTITYUNIT FrPEntityUnit
                                    ,t.TOPENTITYUNIT ToPEntityUnit
                                    ,t.FRENTITYUNIT FrEntityUnit
                                    ,t.TOENTITYUNIT ToEntityUnit
                                    ,t.ENTITYCARRY EntityCarry
                                    ,t.CENTITYTQTY CEntityTQty
                                    ,t.NENTITYTQTY Nentitytqty
                                    ,t.FRWOTYPE FrWoType
                                    ,t.TOWOTYPE ToWoType
                                    ,t.FRLOTTYPE FrLotType
                                    ,t.TOLOTTYPE ToLotType
                                    ,t.FRLOTGRADE FrLotGrade
                                    ,t.TOLOTGRADE ToLotGrade
                                    ,t.FRLOTLEVEL FrLotLevel
                                    ,t.TOLOTLEVEL ToLotLevel
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE ISNULL(t.CSUBLINEID,'') END CSUBLineID
                                    ,CASE WHEN t.VERB = '' THEN '*' ELSE ISNULL(t.NSUBLINEID,'') END NSUBLineID
                                    ,t.SHIFTID ShiftID
                                    ,t.CUSTOMERID CustomerID
                                    ,t.SALEORDER SALEORDER
                                    ,t.TMPFIELD TmpFIELD
                                    ,t.CARTID AS YSCartid
                                    ,'NA' OBACartid

                                    ,CASE WHEN OBA.OBATIME <> '' THEN 'Y' 
                                            WHEN ISNULL(OBA.OBATIME,'') = '' AND t.CARTID <> '' THEN 'N' 
                                            WHEN t.CARTID = '' THEN '' END AS OBA
                                    ,ISNULL(OBA.OBATIME,'') AS OBATIME

                                    ,ISNULL(D.DN,'') AS DN
                                    ,ISNULL(CONVERT(VARCHAR(30),D.SHIPDATE,121),'') AS SHIPDATE
                                    ,ISNULL(D.ASN_GROUPKEY,'') AS ASN_GROUPKEY
                                    ,ISNULL(D.ASN_SHIP_TO,'') AS ASN_SHIP_TO

                                    from #data t 
                                    LEFT JOIN #Sorting S ON S.ENTITYID = T.ENTITYID
                                    LEFT JOIN #Rework r ON r.entityid  = T.ENTITYID
                                    LEFT JOIN #labelRPNT L ON l.panelid = T.ENTITYID
                                    LEFT JOIN #WLTMWIP W ON W.ENTITYID =  T.ENTITYID
                                    LEFT JOIN (SELECT DISTINCT CARTID,CONVERT(VARCHAR(30),MIN(TIME_STAMP),121) OBATIME FROM OBA_DATA AS od WHERE od.CARTID IN(SELECT DISTINCT a.CARTID FROM #data AS a WHERE a.CARTID <> '') GROUP BY CARTID) OBA ON OBA.CARTID = T.CARTID
                                    LEFT JOIN dbo.wms_cartid_dn D ON D.CARTID = T.CARTID
                                    ORDER BY t.RowNumber asc
                             



                             ", strENTITYID);
            return strsql;
        }

        private string getweek(DropDownList ddlwd, DropDownList ddltype, DropDownList ddlyear)
        {
            string mweek;
            int i;

            i = Convert.ToInt16(ddlwd.SelectedValue.ToString());
            int iweek;
            string strsql;
            DataSet dsMD = new DataSet();
            mweek = "";
            iweek = 0;
            if (ddltype.SelectedValue == "W")
            {
                for (int m = i - 4; m <= i; m++)
                {
                    if (m >= 0)
                    {
                        mweek = mweek + "[WK" + m.ToString().PadLeft(2, '0') + "],";

                    }


                }
            }
            if (ddltype.SelectedValue == "M")
            {
                //选择的月是当前系统的月


                if (ddlwd.SelectedValue.ToString().Trim() == System.DateTime.Now.Month.ToString().Trim())
                {
                    strsql = "select datepart(wk,getdate()) as Sweek,datepart(mm,getdate()) as Smonth";
                    dsMD = dbConn.GetDataSetBySql(strsql);
                    iweek = Convert.ToInt16(dsMD.Tables[0].Rows[0]["sweek"].ToString()) - 1;

                }
                else
                {
                    strsql = "select max(reportweek) as reportweek from SH_PRODID_Yield where reportyear='" + ddlyear.SelectedValue.ToString() + "' and reportmonth<=" + Convert.ToInt16(ddlwd.SelectedValue.ToString());
                    dsMD = dbConn.GetDataSetBySql(strsql);
                    if (dsMD.Tables[0].Rows[0]["reportweek"].ToString().Trim() == "".ToString().Trim())
                    {
                        lblerror.Text = "No Data!!";
                        return "";
                    }
                    else
                    {
                        lblerror.Text = "";
                        iweek = Convert.ToInt16(dsMD.Tables[0].Rows[0]["reportweek"].ToString());
                    }

                }
                for (int m = iweek - 4; m <= iweek; m++)
                {
                    if (m >= 0)
                    {
                        mweek = mweek + "[WK" + m.ToString().PadLeft(2, '0') + "],";
                    }

                }
            }

            mweek = mweek.Substring(0, mweek.Length - 1);
            return mweek;
        }
        //行列转换的MONTH
        private string getMONTH(DropDownList ddlwd, DropDownList ddltype, DropDownList ddlyear)
        {
            string mMONTH;
            string strsql;
            int i;
            DataSet dsDATE = new DataSet();

            mMONTH = "";
            i = 0;
            if (ddltype.SelectedValue == "W")
            {
                strsql = "select max(reportmonth) AS reportmonth from SH_PRODID_Yield where reportyear='" + ddlyear.SelectedValue.ToString() + "' and reportweek<=" + Convert.ToInt16(ddlwd.SelectedValue.ToString());
                dsDATE = dbConn.GetDataSetBySql(strsql);
                if (dsDATE.Tables[0].Rows[0]["reportmonth"].ToString().Trim() == "".ToString().Trim())
                {
                    lblerror.Text = "No Data!!";
                    return "";

                }
                else
                {
                    lblerror.Text = "";
                    i = Convert.ToInt16(dsDATE.Tables[0].Rows[0]["reportmonth"].ToString());
                    for (int m = 1; m <= i; m++)
                    {

                        mMONTH = mMONTH + "[MN" + m.ToString().PadLeft(2, '0') + "],";

                    }
                }

            }
            if (ddltype.SelectedValue == "M")
            {
                i = Convert.ToInt16(ddlwd.SelectedValue.ToString());
                for (int j = 1; j <= i; j++)
                {

                    mMONTH = mMONTH + "[MN" + j.ToString().PadLeft(2, '0') + "],";

                }
            }

            mMONTH = mMONTH.Substring(0, mMONTH.Length - 1);
            return mMONTH;
        }


        protected void gvmain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.ToString().ToUpper() == "CELL2".ToUpper())
                {
                    e.Row.ForeColor = Color.CadetBlue;
                }
                if (e.Row.Cells[0].Text.ToString().ToUpper() == "overall".ToUpper())
                {
                    e.Row.ForeColor = Color.Chartreuse;
                }
                if (e.Row.Cells[0].Text.ToString().ToUpper() == "module(finish)".ToUpper())
                {
                    e.Row.ForeColor = Color.Coral;
                }
                if (e.Row.Cells[0].Text.ToString().ToUpper() == "module(assembly)".ToUpper())
                {
                    e.Row.ForeColor = Color.CornflowerBlue;
                }
                if (e.Row.Cells[0].Text.ToString().ToUpper() == "module".ToUpper())
                {
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }

                //for (int m = 2; m < i; m++)
                //{
                //    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                //    {
                //        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                //    }
                //    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                //    {
                //        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                //    }
                //    if (e.Row.Cells[m].Text.ToString().Trim() == "1" && e.Row.Cells[1].Text.ToString().ToUpper() == "yield".ToUpper())
                //    {
                //        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                //    }
                //}
            }

        }
        protected void gvcell2_d_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text.ToString().ToUpper() == "ZZZZ".ToUpper())
                {
                    e.Row.Cells[1].Text = "Total";
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }
                for (int m = 3; m < i; m++)
                {
                    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                    {
                        if (Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 == 0)
                        {
                            e.Row.Cells[m].Text = "";
                        }
                        else
                        {
                            e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                        }

                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "0")
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                }
                int rowindex = e.Row.RowIndex;
                if (rowindex > 0)
                {
                    if (gvcell2_d.Rows[Im].Cells[0].RowSpan == 0) gvcell2_d.Rows[Im].Cells[0].RowSpan++;
                    gvcell2_d.Rows[Im].Cells[0].RowSpan++;
                    e.Row.Cells[0].Visible = false;
                }
            }
        }
        protected void gvfini_d_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvfini_d_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = "Module (Finish)";
                if (e.Row.Cells[1].Text.ToString().ToUpper() == "ZZZZ".ToUpper())
                {
                    e.Row.Cells[1].Text = "Total";
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }
                for (int m = 3; m < i; m++)
                {
                    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                    {
                        if (Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 == 0)
                        {
                            e.Row.Cells[m].Text = "";
                        }
                        else
                        {
                            e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                        }

                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "0")
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                }
                int rowindex = e.Row.RowIndex;
                if (rowindex > 0)
                {
                    if (gvfini_d.Rows[Im].Cells[0].RowSpan == 0) gvfini_d.Rows[Im].Cells[0].RowSpan++;
                    gvfini_d.Rows[Im].Cells[0].RowSpan++;
                    e.Row.Cells[0].Visible = false;
                }
            }
        }

        protected void gvassy_d_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Text = "Module (Assembly)";
                if (e.Row.Cells[1].Text.ToString().ToUpper() == "ZZZZ".ToUpper())
                {
                    e.Row.Cells[1].Text = "Total";
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }
                for (int m = 3; m < i; m++)
                {
                    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                    {
                        if (Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 == 0)
                        {
                            e.Row.Cells[m].Text = "";
                        }
                        else
                        {
                            e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                        }

                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "0" || e.Row.Cells[m].Text.ToString().Trim() == "&nbsp;")
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                }
                int rowindex = e.Row.RowIndex;
                if (rowindex > 0)
                {
                    if (gvassy_d.Rows[Im].Cells[0].RowSpan == 0) gvassy_d.Rows[Im].Cells[0].RowSpan++;
                    gvassy_d.Rows[Im].Cells[0].RowSpan++;
                    e.Row.Cells[0].Visible = false;
                }
            }
        }
        protected void gv_d_cell2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                if (e.Row.Cells[3].Text.Trim().ToUpper() == "target".ToUpper() || e.Row.Cells[3].Text.Trim().ToUpper() == "total".ToUpper())
                {
                    e.Row.Cells[3].Text = "";
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }
                for (int m = 4; m < i; m++)
                {
                    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().ToString().IndexOf(".") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                }
            }
        }
        protected void gv_d_m2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                if (e.Row.Cells[2].Text.Trim() == "DL")
                {
                    e.Row.Cells[2].Text = "Bipane/SH";
                }
                if (e.Row.Cells[3].Text.Trim().ToUpper() == "target".ToUpper() || e.Row.Cells[3].Text.Trim().ToUpper() == "total".ToUpper())
                {
                    e.Row.Cells[3].Text = "";
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }
                for (int m = 4; m < i; m++)
                {
                    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString() == "0")
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                }
            }
        }
        protected void gv_d_m3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i;

            i = e.Row.Cells.Count;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                if (e.Row.Cells[2].Text.Trim() == "DL")
                {
                    e.Row.Cells[2].Text = "Bipane/SH";
                }
                if (e.Row.Cells[3].Text.Trim().ToUpper() == "target".ToUpper() || e.Row.Cells[3].Text.Trim().ToUpper() == "total".ToUpper())
                {
                    e.Row.Cells[3].Text = "";
                    e.Row.ForeColor = Color.DarkGoldenrod;
                }


                for (int m = 4; m < i; m++)
                {
                    if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString() == "0")
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                    if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }
                }
            }
        }
        protected void btnfresh_Click(object sender, EventArgs e)
        {
            PNLMAIN.Visible = false;
            lblRunTime.Text = "";
            lblqueryRS.Text = "";
            txtENTITYID.Enabled = true;
            txtENTITYID.Text = "";
            AppClass Apc = new AppClass();
            Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Clear", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnfresh_Click", "", "");
        }
        private void Fresh()
        {
            PNLMAIN.Visible = false;
        }
        protected void gv_d_cell2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            AppClass Apc = new AppClass();
            Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnexport_click", "", "");
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            //Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            Response.ContentType = "application/ms-excel";
            //Response.Write("<meta http-equiv=Content-Type content=\"text/html | image/png; charset=BIG5\"/>");
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            //this.ChartAll.Page = this;
            //this.ChartAll.RenderControl (oHtmlTextWriter);
            //this.gvmain.RenderControl(oHtmlTextWriter);
            //this.Chart1.RenderControl(oHtmlTextWriter);
            //this.gv_d_cell2.RenderControl(oHtmlTextWriter);
            //this.Chart2.RenderControl(oHtmlTextWriter);
            //this.gv_d_m2.RenderControl(oHtmlTextWriter);
            //this.Chart3.RenderControl(oHtmlTextWriter);
            //this.gv_d_m3.RenderControl(oHtmlTextWriter);

            //this.Chartcell2_D.RenderControl(oHtmlTextWriter);
            //this.gvcell2_d.RenderControl(oHtmlTextWriter);
            //this.chartfini_d.RenderControl(oHtmlTextWriter);
            //this.gvfini_d.RenderControl(oHtmlTextWriter);
            //this.chartassy_d.RenderControl(oHtmlTextWriter);
            //this.gvassy_d.RenderControl(oHtmlTextWriter);

            this.PNLMAIN.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void ddlprodtype_SelectedIndexChanged(object sender, EventArgs e)
        {

            PNLMAIN.Visible = false;
        }
        private DataTable GetPanelIDHistory(string panelID)
        {
            string sql = string.Empty;
            try
            {
                sql = "SELECT v.TOENTITYID AS ENTITYID,v.TRANSDATE,v.CWORKCTR,v.VERB, v.CSUBLINEID,v.NSUBLINEID FROM VWLTXA AS v WITH(NOLOCK)"
                    + " WHERE v.TRANSDATE BETWEEN GETDATE() - 365*3 AND GETDATE()"
                    + "  AND V.VERB IN('MVIN','MVOU')"
                    + "  AND v.TOENTITYID IN('"+panelID+"')"
                    + "  order by TRANSDATE ";
                DataSet ds = dbConn.GetDataSetBySql(sql);
                if (ds == null || ds.Tables.Count == 0) throw new Exception("查詢PanelID:"+panelID+"生產履歷失敗!");
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        private DataTable GetSublineOEEData(string sublineID, string workctr, string startDate, string endDate)
        {
            string sql = string.Empty;
            try
            {
                sql = "SELECT A.SUBLINEID,A.PRODID,A.WORKCTR,A.PREVSTATUS,A.STARTDATE,A.ENDDATE,B.EQDES,B.DEPART,A.CMT"
                    + "  FROM New_ESLSH A LEFT JOIN mecm B ON A.PREVSTATUS=B.EQCD"
                    + string.Format( "  WHERE A.TRANSDATE BETWEEN '{0}' AND '{1}'",startDate,endDate)
                    + "  AND A.SUBLINESTATUS='EM_01'"
                    + string.Format("  and SUBLINEID='{0}' AND A.WORKCTR='{1}'", sublineID,workctr)
                    + "  order by transdate ";
                DataSet ds = dbConn.GetDataSetBySql(sql);
                if (ds == null || ds.Tables.Count == 0) throw new Exception("查詢SublineID:"+sublineID+" OEE資料失敗!");
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
          
        }
        private DataTable GetAllOEEData(string panelID)
        {
            try
            {
                DataTable panelHistoryDt = GetPanelIDHistory(panelID);
                if (panelHistoryDt == null || panelHistoryDt.Rows.Count == 0) throw new Exception("查詢PanelID:" + panelID + "生產履歷失敗!");

                List<string> workctrList= panelHistoryDt.AsEnumerable().Select(r => Convert.ToString(r["CWORKCTR"])).Distinct().ToList();
                //List<PanelHistoryModel> modelList = new List<PanelHistoryModel>();
                DataTable finalDt = null;
                foreach (string workctr in workctrList)
                {                    
                    string sublineID=string.Empty;
                    string startDate=string.Empty;
                    string endDate=string.Empty;
                    //PanelHistoryModel model = new PanelHistoryModel();
                    DataRow[] tempRows = null;
                    DataRow moveInRow = null;
                    DataRow moveOutRow = null;
                    //檢查入賬時間
                    tempRows = panelHistoryDt.Select(string.Format("CWORKCTR='{0}' AND VERB='MVIN'", workctr));
                    if (tempRows != null && tempRows.Length > 0)
                    {
                        moveInRow = tempRows.First();
                    }
                   
                    //檢查出賬時間
                    tempRows = panelHistoryDt.Select(string.Format("CWORKCTR='{0}' AND VERB='MVOU'", workctr));
                    if (tempRows != null && tempRows.Length > 0)
                    {
                        moveOutRow = tempRows.First();
                    }
                    
                    //if (moveInRow == null && moveOutRow == null) continue;

                    if (moveInRow != null && moveOutRow !=null)
                    {
                        //有入賬有出賬
                        sublineID=Convert.ToString(moveOutRow["CSUBLINEID"]);
                        startDate = Convert.ToDateTime(moveInRow["TRANSDATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                        endDate = Convert.ToDateTime(moveOutRow["TRANSDATE"]).ToString("yyyy-MM-dd HH:mm:ss");

                    }
                    else if(moveInRow == null && moveOutRow !=null)
                    {
                        //有出賬無入賬
                        sublineID = Convert.ToString(moveOutRow["CSUBLINEID"]);
                        //拿出賬前一小時當做入賬
                        startDate = Convert.ToDateTime(moveOutRow["TRANSDATE"]).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                        endDate = Convert.ToDateTime(moveOutRow["TRANSDATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else if(moveInRow != null && moveOutRow ==null)
                    {
                        //有入賬無出賬
                        sublineID = Convert.ToString(moveInRow["NSUBLINEID"]);
                        startDate = Convert.ToDateTime(moveInRow["TRANSDATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                        //拿入賬后一小時當做出賬
                        endDate = Convert.ToDateTime(moveInRow["TRANSDATE"]).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        continue;
                    }
                    //封裝數據
                    //model.panelID = panelID;
                    //model.workctr = workctr;
                    //model.sublineID = sublineID;
                    //model.startDate = startDate;
                    //model.endDate = endDate;

                    //modelList.Add(model);

                    //查詢OEE數據
                    DataTable dt=GetSublineOEEData(sublineID, workctr, startDate, endDate);
                    if (dt == null || dt.Rows.Count == 0) continue;

                    if (finalDt == null)
                    {
                        finalDt = dt;
                    }
                    else
                    {
                        //合併Table
                        finalDt = finalDt.AsEnumerable().Union(dt.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();
                    }
                }

                return finalDt;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    
    }

    //public class PanelHistoryModel
    //{
    //    public string panelID { get; set; }
    //    public string workctr { get; set; }
    //    public string sublineID { get; set; }
    //    public string startDate { get; set; }
    //    public string endDate { get; set; }

    //}
}