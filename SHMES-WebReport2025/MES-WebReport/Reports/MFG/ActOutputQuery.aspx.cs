







using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Report.Library;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Dundas.Charting;
using System.Drawing;
using Dundas.Charting.WebControl;
/*
 * 2014-10-17  Innolux/runy     V1.01       修改獲取班別的時間段
 * 2014-11-03  Innolux/runy     V1.02       修正重複累加Defect數量的問題



 * 2014-10-27  Innolux/runy     V1.03       Chart的Y軸範圍從80--100
 * 2014-12-29  Innolux/runy     V1.04       Chart增加TargetYield
*/
namespace WebReport.Reports
{
    public partial class ActOutputQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public string strID = "";
        public string strFabID = "";
        public string strEQID = "";
        public string strGroup = "";
        public string strFrTime = "";
        public string strToTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string strline = "";
                string strAddress = "";
                strAddress = HttpContext.Current.Request.Url.AbsoluteUri;
                if (strAddress.IndexOf("?LINEID") < 0)
                {
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('No Paramet LINEID !!!！');</script>");
                    Response.Write("No Paramet LINEID !!!！");
                    //Chart1.Visible = false;
                    Chart2.Visible = false;
                    return;
                }
                SetProdLineLine();
                if (Request.QueryString["LINEID"].ToString() != null)
                {
                    strline = Request.QueryString["LINEID"].ToString().ToUpper();
                    if (!DDLProdLine.Items.Contains(new ListItem(strline, strline)))
                    {
                        //txtError.Text = "Product Line not Exist for 8270 & 8360 Station!";
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('Product Line not Exist for 8270 & 8360 Station!！');</script>");
                        Response.Write("Product Line not Exist for 8270 & 8360 Station!");
                        //Chart1.Visible = false;
                        Chart2.Visible = false;
                        return;
                    }
                    //Chart1.Visible = true;
                    Chart2.Visible = true;
                    DDLProdLine.SelectedValue = strline;// System.Text.Encoding.Default.GetString(Convert.FromBase64String(strline));
                    DDLProdLine.Enabled = false;
                    txtError.Text = "";
                }
                else
                {
                    //Chart1.Visible = false;
                    Chart2.Visible = false;
                    //txtError.Text = "No Paramet LINEID !!";
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('No Paramet LINEID !!');</script>");
                    Response.Write("No Paramet LINEID !!!！");
                    return;
                }
                txtfrom.Text = "";
                txtto.Text = "";
                txtError.Text = "";
                txtdate.Text = "";
                txtIsSwitch.Text = getSwitchStatus(Request.QueryString["LINEID"].ToString());  //V1.01 Modify
                if (txtIsSwitch.Text.ToUpper() == "Y")
                {
                    //txtLine.Text = Request.QueryString["LINEID"].ToString();//V1.04 Mark
                    txtLine.Text = strline.IndexOf("BOD") > 0 ? "FINISH" : strline;//V1.04 Add
                }
                else
                {
                    txtLine.Text = "NoSwitch";
                }
                btnQuery_Click(btnQuery, e);
            }
            else
            {
                btnQuery_Click(btnQuery, e);
            }
        }
        /// <summary>
        /// 獲取是否切換畫面
        /// </summary>
        /// <param name="lineid"></param>
        private string getSwitchStatus(string lineid)
        {
            string strIsSwitch = "N";
            //string strSQL = " SELECT TYPEID,ID,IDNAME FROM MSUMU WHERE TYPEID='DZKB' AND ID=";   //V1.04 Mark
            //V1.04 Add Start
            string strSQL = " SELECT TYPEID,ID,IDNAME FROM MSUMU WHERE TYPEID='DZKB' AND ID " +
                            " IN (SELECT DISTINCT CASE WHEN Shop='FINISHING' THEN 'FNSH' ELSE 'ASSY' END FROM ropprd WHERE ProdLine='" + Request.QueryString["LINEID"].ToString() + "')";
            //V1.04 Add End
            DataSet ds = new DataSet();
            //V1.04 Mark Start
            //if (lineid.IndexOf("ASSY") > 0)
            //{
            //    strSQL += "'ASSY'";
            //}
            //else if (lineid.IndexOf("FINISH") > 0)
            //{
            //    strSQL += "'FNSH'";
            //}
            //V1.04 Mark End
            ds = dbConn.GetDataSetBySql(strSQL);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["IDNAME"].ToString() == "1")
            {
                strIsSwitch = "Y";
            }
            return strIsSwitch;
        }

        private void SetProdLineLine()
        {
            this.DDLProdLine.Items.Clear();
            string strSql = @" SELECT 'PRODLINE' AS SUBLINEID,'0' AS ROWID UNION ALL ";
            strSql = strSql + @" SELECT distinct UPPER(SUBLINEID) AS SUBLINEID, '1' AS ROWID FROM [REP].[MSLM] WHERE WORKCTR  in('8270', '8360')";
            strSql = strSql + @" ORDER BY ROWID,SUBLINEID ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.DDLProdLine.DataSource = dt;
            this.DDLProdLine.DataTextField = "SUBLINEID";
            this.DDLProdLine.DataValueField = "SUBLINEID";
            this.DDLProdLine.DataBind();
            this.DDLProdLine.ClearSelection();
        }
        private void BoundData()
        {

            string strProdLine = "";
            Boolean BFinish = false;
            Boolean BAssy = false;
            string strworkctr = "";
            string strctr = "";
            string strtitle = "";
            string strProd = "";
            try
            {

                strProdLine = DDLProdLine.SelectedValue.ToString();
                txtError.Text = "";
                DDLProdLine.Enabled = false;

                BFinish = (strProdLine.IndexOf("FINISH") > 0 || strProdLine.IndexOf("BOD") > 0);//V1.04 Add strProdLine.IndexOf("BOD")>0 
                BAssy = (strProdLine.IndexOf("ASSY") > 0);
                if (!BFinish && !BAssy)
                {
                    //txtError.Text = "Please Select Product Line !!";
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('No Paramet LINEID !!!！');</script>");
                    Response.Write("No Paramet LINEID !!!！");
                    return;
                }
                if (BFinish && BAssy)
                {
                    //txtError.Text = "Please Select Product Line !!";
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('No Paramet LINEID !!!！');</script>");
                    Response.Write("No Paramet LINEID !!!！");
                    return;
                }
                if (BFinish)
                {
                    strworkctr = "'8270','8280'";
                    strctr = "'8210','8230','8250','8270','8280'";
                    strtitle = "Finish   Yield";
                }
                if (BAssy)
                {
                    strworkctr = "'8360'";
                    strctr = "'8300','8350','8351','8352','8353','8360'";
                    strtitle = "Assembly   Yield";
                }
                StringBuilder strsql = new StringBuilder();

                strsql.Append("select *  INTO #TARGET from ( \n");
                strsql.Append(" select '02' as number,'Hour_8' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '03' as number,'Hour_9' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '04' as number,'Hour_10' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '05' as number,'Hour_11' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '06' as number,'Hour_12' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '07' as number,'Hour_13' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '08' as number,'Hour_14' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '09' as number,'Hour_15' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '10' as number,'Hour_16' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '11' as number,'Hour_17' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '12' as number,'Hour_18' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '13' as number,'Hour_19' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '14' as number,'Hour_20' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '15' as number,'Hour_21' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '16' as number,'Hour_22' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '17' as number,'Hour_23' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '18' as number,'Hour_24' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '19' as number,'Hour_1' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '20' as number,'Hour_2' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '21' as number,'Hour_3' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '22' as number,'Hour_4' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '23' as number,'Hour_5' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '24' as number,'Hour_6' as type,'100' as target \n");
                strsql.Append(" union  \n");
                strsql.Append(" select '25' as number,'Hour_7' as type,'100' as target \n");
                strsql.Append(" ) AA;  \n");
                //hour,qty,A GRADE
                strsql.Append(" select 'Hour_'+convert(nvarchar(2),datepart(hour,transdate)+1)   as HH,SUM(NENTITYTQTY) AS AQTY INTO #WLTX_QTY \n");
                strsql.Append(" from New_wltx where transdate between '" + txtfrom.Text.ToString() + "' and '" + txtto.Text.ToString() + "' \n");
                strsql.Append(" and fabid='B' and verb='MVOU' and cworkctr  in( " + strworkctr + " ) \n");
                strsql.Append(" and csublineid='" + strProdLine + "' and tolotgrade='A' \n");
                strsql.Append(" GROUP BY datepart(hour,transdate); \n");
                //hour, detail prodid(by hour,record one or some prodid),A GRADE
                strsql.Append(" SELECT 'Hour_'+convert(nvarchar(2),datepart(hour,transdate)+1)   as HH,b.project_name as TOPRODID, \n");
                strsql.Append(" MAX(TRANSDATE) AS TRANSDATE INTO #WLTX_PROD_DETAIL from New_wltx a,mpim b where a.transdate between '" + txtFrShift.Text.ToString() + "' and '" + txtToShift.Text.ToString() + "' \n");  //V1.01 Modify
                strsql.Append(" and a.fabid='B' and a.verb='MVOU' and a.cworkctr in ( " + strworkctr + " ) \n");
                strsql.Append(" and a.csublineid='" + strProdLine + "' and a.tolotgrade='A' and a.toprodid=b.prodid \n");
                strsql.Append(" GROUP BY datepart(hour,transdate),project_name; \n");
                //hour,  prodid(by hour,record one prodid),A GRADE
                strsql.Append(" SELECT A.HH,A.TOPRODID INTO #WLTX_PROD FROM #WLTX_PROD_DETAIL A,( \n");
                strsql.Append(" select HH,MAX(TRANSDATE) AS TRANSDATE FROM #WLTX_PROD_DETAIL   GROUP BY HH) B WHERE A.HH=B.HH \n");
                strsql.Append(" AND A.TRANSDATE=B.TRANSDATE; \n");
                //hour,prodid,qty,actual
                strsql.Append(" SELECT A.HH,B.TOPRODID AS PRODID,A.AQTY INTO #ACTUAL FROM #WLTX_QTY A,#WLTX_PROD B WHERE A.HH=B.HH  \n");
                // target left join actual
                strsql.Append(" select a.number,a.type,isnull(b.prodid,'') as prodid,isnull(b.aqty,'0') as aqty, \n");
                strsql.Append(" a.target INTO #TARGET_ACT \n");
                strsql.Append(" from #target a left join #actual b on a.type=b.hh; \n");
                //update prodid, after current hour
                strsql.Append(" DECLARE @NUMBER NVARCHAR(2); \n");
                strsql.Append(" DECLARE @PRODID NVARCHAR(50); \n");
                strsql.Append(" SELECT @NUMBER=MAX(A.NUMBER)  FROM #TARGET  A, #ACTUAL B WHERE A.TYPE=B.HH; \n");
                strsql.Append("  IF @NUMBER IS NULL \n");
                strsql.Append("     BEGIN \n");
                strsql.Append("  select @NUMBER=NUMBER from #target where type='Hour_'++convert(nvarchar(2),datepart(hour,getdate())+1) \n");
                strsql.Append("  END  \n");

                strsql.Append(" SELECT @PRODID=ISNULL(B.PRODID,'')  FROM #TARGET  A, #ACTUAL B WHERE A.TYPE=B.HH AND A.NUMBER=@NUMBER; \n");
                strsql.Append(" IF @PRODID IS NULL  BEGIN  SET @PRODID='' END ; \n");
                strsql.Append(" UPDATE #TARGET_ACT SET PRODID=@PRODID \n");
                strsql.Append(" WHERE NUMBER>@NUMBER; \n");
                // plan by hour,prodid, user upload information
                strsql.Append(" select * INTO #ROPPRD from ropprd where FABID='SHLCM' \n");
                strsql.Append(" AND  MFGDATE =  '" + txtdate.Text.ToString() + "' \n");
                strsql.Append(" AND PRODLINE='" + strProdLine + "'; \n");
                strsql.Append(" SELECT HH=attribute,productid,num=value INTO #PLAN FROM #ROPPRD \n");
                strsql.Append(" unpivot \n");
                strsql.Append(" ( \n");
                strsql.Append("    value FOR attribute IN([Hour_8], [Hour_9], [Hour_10],[Hour_11], [Hour_12], [Hour_13], \n");
                strsql.Append(" [Hour_14], [Hour_15], [Hour_16],[Hour_17], [Hour_18], [Hour_19], \n");
                strsql.Append(" [Hour_20], [Hour_21], [Hour_22],[Hour_23], [Hour_24], [Hour_1], \n");
                strsql.Append(" [Hour_2], [Hour_3], [Hour_4],[Hour_5], [Hour_6], [Hour_7]) \n");
                strsql.Append("  ) AS UPV; \n");
                // chart1 final result
                strsql.Append(" SELECT A.*,ISNULL(C.NUM,'0') AS PQTY INTO #FINAL FROM #TARGET_ACT A LEFT JOIN #plan c on a.type=c.hh and A.prodid=c.productid; \n");
                strsql.Append(" select NUMBER,TYPE,PRODID,CASE WHEN NUMBER<=@NUMBER THEN AQTY ELSE NULL END AS AQTY,CASE WHEN NUMBER<=@NUMBER THEN TARGET ELSE NULL END AS TARGET,CASE WHEN NUMBER<=@NUMBER THEN PQTY ELSE NULL END AS PQTY,CASE WHEN NUMBER<=@NUMBER THEN case when pqty=0 and aqty>0 then '100' \n");
                strsql.Append(" WHEN pqty=0 and aqty=0 then '0' \n");
                strsql.Append("  when pqty>0 then round(aqty/cast(pqty as float),4) *100 end ELSE NULL END as rate from ( \n");
                strsql.Append(" SELECT '01' AS NUMBER,'Total' as type,'Total' as prodid,sum(aqty) as aqty,'100' as target,sum(pqty) as pqty FROM #FINAL WHERE NUMBER<=@NUMBER \n");
                strsql.Append(" union \n");
                strsql.Append(" select * from #FINAL \n");
                strsql.Append(" )AA; \n");
                // get latest prodid information
                strsql.Append(" select @prodid as prodid  \n");
                // get Rate ,chart2
                strsql.Append("  SELECT HH,SUM(AQTY) AS AQTY,SUM(NGQTY) AS NGQTY INTO #RATE FROM (  \n");
                strsql.Append("   select 'Hour_'+convert(nvarchar(2),datepart(hour,transdate)+1)   as HH,  \n");
                strsql.Append("  CASE WHEN CWORKCTR in ( " + strworkctr + " ) AND TOLOTGRADE='A' THEN NENTITYTQTY ELSE '0' END AS AQTY ,  \n");
                strsql.Append("  CASE WHEN TOLOTGRADE IN('R','S') THEN NENTITYTQTY ELSE '0'END AS NGQTY  \n");
                strsql.Append("   from New_wltx where transdate between '" + txtfrom.Text.ToString() + "' and '" + txtto.Text.ToString() + "' and fabid='B' and verb='MVOU' and cworkctr  IN(" + strctr + ") and csublineid='" + strProdLine + "'  \n");
                strsql.Append("  ) AA  GROUP BY HH;  \n");
                strsql.Append("  select a.number,a.type,isnull(b.aqty,'0') as AQTY,ISNULL(B.NGQTY,'0') AS NGQTY   \n");
                strsql.Append("  INTO #RATE_FINAL FROM #TARGET A LEFT JOIN #RATE B ON A.TYPE=B.HH ; \n");
                strsql.Append("  DECLARE @NUMBER1 NVARCHAR(2);  SELECT @NUMBER1=MAX(A.NUMBER)  FROM #TARGET  A, #rate B WHERE A.TYPE=B.HH;  \n");
                strsql.Append("  IF @NUMBER1 IS NULL \n");
                strsql.Append("     BEGIN \n");
                strsql.Append("  select @NUMBER1=NUMBER from #target where type='Hour_'++convert(nvarchar(2),datepart(hour,getdate())+1) \n");
                strsql.Append("  END  \n");
                //V1.04 Add Start
                //strsql.Append("  SELECT NUMBER,TYPE,AQTY,NGQTY, ");
                //strsql.Append(" CASE WHEN NUMBER<=@NUMBER1 THEN CASE WHEN (AQTY+NGQTY)=0 THEN '0' ELSE  ");
                //strsql.Append("  ROUND(CAST (AQTY AS FLOAT)/(AQTY+NGQTY),4) *100  END ELSE NULL END AS RATE ");
                //strsql.Append("  FROM ( ");
                //strsql.Append("  SELECT * FROM #RATE_FINAL ");
                //strsql.Append("  UNION  ");
                //strsql.Append("  SELECT '01' NUMBER,'Total Yield' type,SUM(AQTY) AS AQTY,SUM(NGQTY) AS NGQTY FROM  ");
                //strsql.Append("  #RATE_FINAL WHERE NUMBER<=@NUMBER1 ");
                //strsql.Append("  ) AA ");

                strsql.Append(" SELECT AA.NUMBER,AA.[TYPE],AQTY,NGQTY,  \n");
                strsql.Append(" CASE WHEN AA.NUMBER<=@NUMBER1 THEN CASE WHEN (AQTY+NGQTY)=0 THEN '0' ELSE \n");
                strsql.Append(" ROUND(CAST (AQTY AS FLOAT)/(AQTY+NGQTY),4) *100  END ELSE NULL END AS RATE,AB.TargetYield \n");
                strsql.Append(" FROM (   \n");
                strsql.Append(" SELECT * FROM #RATE_FINAL  \n");
                strsql.Append(" UNION  \n");
                strsql.Append(" SELECT '01' NUMBER,'Total Yield' type,SUM(AQTY) AS AQTY,SUM(NGQTY) AS NGQTY FROM \n");
                strsql.Append(" #RATE_FINAL WHERE NUMBER<=@NUMBER1  \n");
                strsql.Append(" ) AA \n");
                strsql.Append(" left join \n");
                strsql.Append(" ( \n");
                strsql.Append(" SELECT '02' AS Number,'Hour_8' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE HOUR_8 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '03' AS Number,'Hour_9' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE HOUR_9 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '04' AS Number,'Hour_10' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE HOUR_10 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '05' AS Number,'Hour_11' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_11 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '06' AS Number,'Hour_12' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_12 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '07' AS Number,'Hour_13' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_13 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '08' AS Number,'Hour_14' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_14 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '09' AS Number,'Hour_15' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_15 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '10' AS Number,'Hour_16' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_16 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '11' AS Number,'Hour_17' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_17 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '12' AS Number,'Hour_18' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_18 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '13' AS Number,'Hour_19' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_19 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '14' AS Number,'Hour_20' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_20 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '15' AS Number,'Hour_21' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_21 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '16' AS Number,'Hour_22' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_22 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '17' AS Number,'Hour_23' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_23 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '18' AS Number,'Hour_24' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_24 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '19' AS Number,'Hour_1' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_1 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '20' AS Number,'Hour_2' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_2 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '21' AS Number,'Hour_3' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_3 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '22' AS Number,'Hour_4' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_4 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '23' AS Number,'Hour_5' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_5 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '24' AS Number,'Hour_6' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_6 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t \n");
                strsql.Append(" UNION \n");
                strsql.Append(" SELECT '25' AS Number,'Hour_7' AS [TYPE],SUM(cast(TargetYield as float))*100 AS TargetYield FROM(SELECT CASE Hour_7 WHEN 0 THEN NULL ELSE TargetYield END AS TargetYield FROM #ROPPRD) t) AB ON AA.NUMBER=AB.Number AND AA.TYPE=AB.TYPE \n");

                //V1.04 Add End

                DataSet DSPA = this.dbConn.GetDataSetBySql(strsql.ToString());
                //if (DSPA.Tables[1].Rows[0]["prodid"].ToString() =="")
                //{
                //    //this.txtError.Text = "No Data!!";
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('No Data!！');</script>");
                //    Chart1.Visible = false;
                //    Chart2.Visible = false;
                //    return;
                //}
                strProd = (DSPA.Tables[1].Rows[0]["prodid"].ToString() == "") ? "No Production" : DSPA.Tables[1].Rows[0]["prodid"].ToString();
                txtProject.Text = strProd;//把project保存在text控件內







                lblNote.Text = "Line : " + strProdLine + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + " Project : " + strProd;
                txtActOut.Text = DSPA.Tables[0].Rows[0][3].ToString();
                txtBaln.Text = Convert.ToString(Convert.ToDouble(txtActOut.Text) - Convert.ToDouble(txtPOut_now.Text));
                //設置Balance的顏色







                if (Convert.ToDouble(txtBaln.Text) > 0)
                {
                    //tdBalanceTop.BgColor = "blue";               //修改Balance的背景色
                    //tdBalanceBottom.BgColor = "blue";
                    txtBaln.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    //tdBalanceTop.BgColor = "yellow";            //修改Balance的背景色
                    //tdBalanceBottom.BgColor = "yellow";
                    txtBaln.ForeColor = System.Drawing.Color.Red;
                }
                // bound chart1                
                //Chart1.Series.Clear();
                //Chart1.Series.Add("Series1");
                //Chart1.Series.Add("Series2");
                //Chart1.DataSource = DSPA.Tables[0].DefaultView;
                //Series Series3 = new Series();
                //Chart1.Series.Add(Series3);
                //Series Series4 = new Series();
                //Chart1.Series.Add(Series4);
                //Chart1.Series["Series1"].Color = Color.Brown;
                //Chart1.Series["Series2"].Color = Color.BlueViolet;
                //Chart1.Series["Series3"].Color = Color.Orange;
                //Chart1.Series["Series4"].Color = Color.GreenYellow;
                //Chart1.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
                //Chart1.ChartAreas["Default"].AxisX.Interval = 1;

                //Chart1.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;

                //Chart1.Titles[0].Text = "Plan/Actual Output";
                //Chart1.Titles[0].Alignment = ContentAlignment.TopCenter;
                //Chart1.Titles[0].Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                //Chart1.Titles[0].Color = Color.FromArgb(72, 72, 72);
                //foreach (Series series in Chart1.Series)
                //{
                //    series.MapAreaAttributes = " target='_blank'";
                //    series.Font = new Font("Microsoft Sans Serif", 12);

                //}

                //Chart1.Series["Series1"].Name = "Plan Output"; //"PLAN_OUTPUT";
                //Chart1.Series["Series2"].Name = "Actual Output"; //"ACT_OUTPUT";
                //Chart1.Series["Series3"].Name = "Hit Rate"; //"REACH_RATE";
                //Chart1.Series["Series4"].Name = "Target";

                //Chart1.Series["Plan Output"].ValueMemberX = "type";
                //Chart1.Series["Plan Output"].XValueType = ChartValueTypes.String;
                //Chart1.Series["Plan Output"].ValueMembersY = "PQTY";
                //Chart1.Series["Plan Output"].YValueType = ChartValueTypes.Int;
                //Chart1.Series["Plan Output"].Label = "#VALY{##,##0.##}";
                ////Chart1.Series["Plan Output"].Font.Size = Unit.Pixel(12);
                //Chart1.Series["Plan Output"].MarkerStyle = MarkerStyle.Star4;
                //Chart1.Series["Plan Output"].SmartLabels.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                //Chart1.Series["Plan Output"].SmartLabels.Enabled = true;
                //Chart1.Series["Plan Output"].EmptyPointStyle.Color = Color.White;
                //Chart1.Series["Plan Output"].EmptyPointStyle.FontColor = Color.White;
                ////Chart1.Series["計畫產出"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
                ////Chart1.Series["計畫產出"].ToolTip = " #VALX " + DDLApplication.Text + ":#VALY{##,##0.##}";

                //Chart1.Series["Actual Output"].ValueMemberX = "TYPE";
                //Chart1.Series["Actual Output"].XValueType = ChartValueTypes.String;
                //Chart1.Series["Actual Output"].ValueMembersY = "AQTY";
                //Chart1.Series["Actual Output"].MarkerStyle = MarkerStyle.Star4;
                //Chart1.Series["Actual Output"].YValueType = ChartValueTypes.Int;
                //Chart1.Series["Actual Output"].Label = "#VALY{##,##0.##}";
                //Chart1.Series["Actual Output"].SmartLabels.Enabled = true;
                //Chart1.Series["Actual Output"].SmartLabels.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                ////Chart1.Series["Actual Output"].Font.Size = Unit.Pixel(12);
                //Chart1.Series["Actual Output"].EmptyPointStyle.Color = Color.White;
                //Chart1.Series["Actual Output"].EmptyPointStyle.FontColor = Color.White;
                ////Chart1.Series["實際產出"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
                ////Chart1.Series["實際產出"].ToolTip = " #VALX " + DDLApplication.Text + ":#VALY{##,##0.##}";


                //Chart1.Series["Hit Rate"].ValueMemberX = "TYPE";
                //Chart1.Series["Hit Rate"].ValueMembersY = "RATE";
                //Chart1.Series["Hit Rate"].Type = SeriesChartType.Line;
                //Chart1.Series["Hit Rate"].YAxisType = AxisType.Secondary;
                //Chart1.Series["Hit Rate"].MarkerStyle = MarkerStyle.Star4;
                //Chart1.Series["Hit Rate"].XValueType = ChartValueTypes.String;
                //Chart1.Series["Hit Rate"].Label = "#VALY{##,##0.##%}";
                //Chart1.Series["Hit Rate"].SmartLabels.Enabled = true;
                //Chart1.Series["Hit Rate"].SmartLabels.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                //Chart1.Series["Hit Rate"].EmptyPointStyle.Color = Color.White;
                //Chart1.Series["Hit Rate"].EmptyPointStyle.FontColor = Color.White;
                ////Chart1.Series["達成率"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
                ////Chart1.Series["達成率"].ToolTip = " #VALX " + DDLApplication.Text + ":#VALY{##,##0.##%}";

                //Chart1.Series["Target"].ValueMemberX = "TYPE";
                //Chart1.Series["Target"].ValueMembersY = "TARGET";
                //Chart1.Series["Target"].Type = SeriesChartType.Line;
                //Chart1.Series["Target"].YAxisType = AxisType.Secondary;
                //Chart1.Series["Target"].MarkerStyle = MarkerStyle.Star4;
                //Chart1.Series["Target"].XValueType = ChartValueTypes.String;
                //Chart1.Series["Target"].ToolTip = "#VALY{##,##0.##%}";
                //Chart1.Series["Target"].EmptyPointStyle.Color = Color.White;
                //Chart1.Series["Target"].EmptyPointStyle.FontColor = Color.White;
                ////Chart1.Series["Target"].SmartLabels.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                //Chart1.ChartAreas["Default"].AxisX.Title = "";
                //Chart1.ChartAreas["Default"].AxisY.Title = "";
                //Chart1.ChartAreas["Default"].AxisY2.Title = "";
                //Chart1.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
                //Chart1.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##%";
                //Chart1.ChartAreas["Default"].InnerPlotPosition.X = 5;
                //Chart1.ChartAreas["Default"].InnerPlotPosition.Y = 5;
                //Chart1.ChartAreas["Default"].InnerPlotPosition.Height = 75;
                //Chart1.ChartAreas["Default"].InnerPlotPosition.Width = 90;
                //Chart1.Visible = true;
                //Chart1.DataBind();

                // bind chart2

                Chart2.Series.Clear();
                Chart2.Series.Add("Series1");
                Chart2.Series.Add("TargetYield");//V1.04 Add
                Chart2.Series["TargetYield"].Color = Color.Red;//V1.04 Add
                Chart2.DataSource = DSPA.Tables[2].DefaultView;
                Chart2.Series["Series1"].Color = Color.Orange;
                Chart2.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
                Chart2.ChartAreas["Default"].AxisX.Interval = 1;
                Chart2.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
                Chart2.Titles[0].Text = strtitle;
                Chart2.Titles[0].Alignment = ContentAlignment.TopCenter;
                Chart2.Titles[0].Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                Chart2.Titles[0].Color = Color.FromArgb(72, 72, 72);
                foreach (Series series in Chart2.Series)
                {
                    series.MapAreaAttributes = " target='_blank'";
                    series.Font = new Font("Microsoft Sans Serif", 12);
                }

                Chart2.Series["Series1"].Name = "Yield"; //"PLAN_OUTPUT";


                Chart2.Series["Yield"].ValueMemberX = "type";
                Chart2.Series["Yield"].XValueType = ChartValueTypes.String;
                Chart2.Series["Yield"].ValueMembersY = "rate";
                Chart2.Series["Yield"].Type = SeriesChartType.Line;
                Chart2.Series["Yield"].Label = "#VALY{##,##0.##%}";
                
                Chart2.Series["Yield"].YValueType = ChartValueTypes.Int;
                Chart2.Series["Yield"].MarkerStyle = MarkerStyle.Star4;
                Chart2.Series["Yield"].SmartLabels.Enabled = true;
                Chart2.Series["Yield"].SmartLabels.MovingDirection = LabelAlignment.Top;//V1.04 Add
                Chart2.Series["Yield"].SmartLabels.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                Chart2.Series["Yield"].EmptyPointStyle.Color = Color.White;
                Chart2.Series["Yield"].EmptyPointStyle.FontColor = Color.White;
                //Chart2.Series["Yield"].ToolTip = "#VALY{##,##0.##%}";
                //V1.04 Add Start
                Chart2.Series["TargetYield"].ValueMemberX = "type";
                Chart2.Series["TargetYield"].XValueType = ChartValueTypes.String;
                Chart2.Series["TargetYield"].ValueMembersY = "TargetYield";
                Chart2.Series["TargetYield"].Type = SeriesChartType.Line;
                Chart2.Series["TargetYield"].Label = "#VALY{##,##0.##%}";
                Chart2.Series["TargetYield"].FontColor = Color.Red;//V1.04 Add
                Chart2.Series["TargetYield"].YValueType = ChartValueTypes.Int;
                Chart2.Series["TargetYield"].MarkerStyle = MarkerStyle.Star4;
                Chart2.Series["TargetYield"].SmartLabels.Enabled = true;
                Chart2.Series["TargetYield"].SmartLabels.MovingDirection = LabelAlignment.Bottom;//V1.04 Add
                Chart2.Series["TargetYield"].SmartLabels.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                Chart2.Series["TargetYield"].EmptyPointStyle.Color = Color.White;
                Chart2.Series["TargetYield"].EmptyPointStyle.FontColor = Color.White;
                //V1.04 Add End
                Chart2.ChartAreas["Default"].AxisX.Title = "";
                Chart2.ChartAreas["Default"].AxisY.Title = "";
                Chart2.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
                Chart2.ChartAreas["Default"].InnerPlotPosition.X = 5;
                Chart2.ChartAreas["Default"].InnerPlotPosition.Y = 5;
                Chart2.ChartAreas["Default"].InnerPlotPosition.Height = 75;
                Chart2.ChartAreas["Default"].InnerPlotPosition.Width = 90;
                //V1.03 Add Start
                Chart2.ChartAreas["Default"].AxisY.Minimum = 80;          //設定Y軸顯示的最小值






                //Chart2.ChartAreas["Default"].AxisY.Interval = 2;          //設置縱向間距
                Chart2.ChartAreas["Default"].AxisY.Maximum = 100;         //設定Y軸顯示的最大值






                //V1.03 Add End
                Chart2.Visible = true;
                Chart2.DataBind();

            }
            catch (Exception ex)
            {
                //txtError.Text = ex.Message;
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('"+ ex.Message +"');</script>");
                Response.Write(ex.Message);
                //Chart1.Visible = false;
                Chart2.Visible = false;
                return;
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {      
            RecordLog Log = new RecordLog();
            try
            {
                GetPlanOutData();//獲取PC導入的計劃產出







                lblNote.Text = "";
                if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
                {
                    //this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                //string strfrom = "";
                //string strto = "";
                //string strdate = "";
                string strnow = "";
                strnow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //strdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                //strfrom = strdate + " 07:00:00";
                //strto = Convert.ToDateTime(strdate).AddDays(1).ToString("yyyy-MM-dd") + " 07:00:00";
                //if (Convert.ToDateTime(strnow) >= Convert.ToDateTime(strfrom) && Convert.ToDateTime(strnow) >= Convert.ToDateTime(strfrom))
                //{

                //}
                //else
                //{
                //    strdate = Convert.ToDateTime(strdate).AddDays(-1).ToString("yyyy-MM-dd");
                //    strfrom = strdate + " 07:00:00";
                //    strto = Convert.ToDateTime(strdate).AddDays(1).ToString("yyyy-MM-dd") + " 07:00:00";
                //}
                //txtfrom.Text = strfrom;
                //txtto.Text = strto;
                //txtdate.Text = strdate;
                lbltime.Text = "每2分鐘會自動刷新一次,或者按鍵盤{F5}鍵手動刷新;上次更新時間：   " + strnow;
                BoundData();//綁定層1的內容








                if (txtLine.Text.IndexOf("ASSY") > 0)//判断是否切换，如果不切换则不加载
                {
                    InitDiv2Crtls();
                    InitDiv2();//綁定層2的內容







                }
                else if (txtLine.Text.IndexOf("FINISH") > 0)
                {
                    InitDiv3Ctrls();
                    InitDiv3();//綁定層3的內容






                }
            }
            catch (Exception ex)
            {
                //this.txtError.Text = ex.Message;
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('"+ex.Message +"');</script>");
                Response.Write(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 獲取計劃產出的信息






        /// </summary>
        private void GetPlanOutData()
        {
            DataSet ds = new DataSet();
            bool isDA = false;
            string strSQL_ROPPRD = " SELECT mfgdate,prodline,productid,";
            DateTime dtNow = DateTime.Now;
            string StrMfgdate = "";
            TimeSpan tspan = dtNow.Subtract(Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 19:00:00"));
            double hourdiff = tspan.TotalHours;
            if (hourdiff >= -12 && hourdiff <= 0)
            {
                StrMfgdate = dtNow.ToShortDateString().ToString();
                txtfrom.Text = StrMfgdate + " 07:00:00";                                      
                txtto.Text = dtNow.AddDays(1).ToShortDateString().ToString() + " 07:00:00";             
                txtFrShift.Text = StrMfgdate + " 07:00:00";                                        //V1.01 Add
                txtToShift.Text = dtNow.ToShortDateString().ToString() + " 19:00:00";              //V1.01 Add
                isDA = true;
                
            }
            else if (hourdiff > 0)
            {
                StrMfgdate = dtNow.ToShortDateString().ToString();
                txtfrom.Text = StrMfgdate + " 07:00:00";                                     
                txtto.Text = dtNow.AddDays(1).ToShortDateString().ToString() + " 07:00:00";
                txtFrShift.Text = StrMfgdate + " 19:00:00";                                       //V1.01 Add
                txtToShift.Text = dtNow.AddDays(1).ToShortDateString().ToString() + " 07:00:00";  //V1.01 Add
                isDA = false;
            }
            else if (hourdiff < -12)
            {
                StrMfgdate = dtNow.AddDays(-1).ToShortDateString().ToString();
                txtfrom.Text = StrMfgdate + " 07:00:00";                                     
                txtto.Text = dtNow.ToShortDateString().ToString() + " 07:00:00";            
                txtFrShift.Text = StrMfgdate + " 19:00:00";                                       //V1.01 Add
                txtToShift.Text = dtNow.ToShortDateString().ToString() + " 07:00:00";             //V1.01 Add
                isDA = false;
            }
            //StrMfgdate = "2015/01/05";// for test
            txtdate.Text = StrMfgdate;//V1.04 Add
            if (isDA)
            {
                strSQL_ROPPRD += " Hour_8,Hour_9,Hour_10,Hour_11,Hour_12,Hour_13,Hour_14,Hour_15,Hour_16,Hour_17,Hour_18,Hour_19 FROM ROPPRD ";
            }
            else
            {
                strSQL_ROPPRD += " Hour_20,Hour_21,Hour_22,Hour_23,Hour_24,Hour_1,Hour_2,Hour_3,Hour_4,Hour_5,Hour_6,Hour_7 FROM ROPPRD ";
            }
            strSQL_ROPPRD += " WHERE MFGDate='" + StrMfgdate + "' AND ProdLine='" + DDLProdLine.SelectedValue.ToString() + "'";
            ds = dbConn.GetDataSetBySql(strSQL_ROPPRD);
            if (ds.Tables[0].Rows.Count > 0)
            {
                double dbPOut = 0;
                for (int j = 3; j <= 14; j++)
                {
                    double dblcurrent = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dbPOut += Convert.ToDouble(ds.Tables[0].Rows[i][j].ToString());
                        dblcurrent += Convert.ToDouble(ds.Tables[0].Rows[i][j].ToString());
                    }
                    //獲取當前時間段的計劃產出的數量







                    if ((dtNow.Hour + 1) >= 8 && (dtNow.Hour + 1) <= 19)
                    {
                        if (j + 5 == (dtNow.Hour + 1))
                        {
                            txtPOut_now.Text = (dbPOut-dblcurrent+Convert.ToInt64(dblcurrent * dtNow.Minute/60)).ToString();
                        }
                    }
                    else if ((dtNow.Hour + 1) >= 20 && (dtNow.Hour + 1) <= 24)
                    {
                        if (j + 17 == (dtNow.Hour + 1))
                        {
                            //txtPOut_now.Text = dbPOut.ToString();
                            txtPOut_now.Text = (dbPOut - dblcurrent + Convert.ToInt64(dblcurrent * dtNow.Minute / 60)).ToString();
                        }
                    }
                    else if ((dtNow.Hour + 1) >= 1 && (dtNow.Hour + 1) <= 7)
                    {
                        if (j - 7 == (dtNow.Hour + 1))
                        {
                            //txtPOut_now.Text = dbPOut.ToString();
                            txtPOut_now.Text = (dbPOut - dblcurrent + Convert.ToInt64(dblcurrent * dtNow.Minute / 60)).ToString();
                        }
                    }
                }
                txtPOut.Text = dbPOut.ToString();
            }
            else
            {
                txtPOut.Text = "0";
                txtPOut_now.Text = "0";
            }
        }
       
        /// <summary>
        /// 獲取線別對應的Defect信息（供層2和層3共同使用）






        /// </summary>
        /// <param name="isFuncDef">是否是Functional Defect只有DIV2的下半段是Functional Defect</param>
        /// <returns></returns>
        private DataSet GetDefInfo(bool isFuncDef)
        {
            TimeSpan tstemp = DateTime.Now.Subtract(Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 19:00:00"));
            double hourdiff = tstemp.TotalHours;
            string strFromTime = "";
            string strToTime = "";
            DataSet ds = new DataSet();
            string strSQL = "";
            if (hourdiff >= -12 && hourdiff <= 0)
            {
                strFromTime = txtfrom.Text;
                strToTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                strFromTime = Convert.ToDateTime(txtfrom.Text).AddHours(12).ToString("yyyy-MM-dd HH:mm:ss");
                strToTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!isFuncDef)
            {
                 strSQL += " SELECT A.DEFECTCD,SUM(A.CENTITYTQTY) CENTITYTQTY,B.DEFECTDESC,A.CWORKCTR FROM New_WLTX A,rep.MDCM B \n" +
                                " WHERE A.DEFECTCD=B.DEFECTCD \n" +
                                " AND TransDate BETWEEN '" + strFromTime + "' AND '" + strToTime + "' \n" +
                                " AND A.CSUBLINEID='" + DDLProdLine.SelectedValue.ToString() + "' \n" +
                                " AND A.VERB='MVOU' \n" +                                //V1.02 Add
                                " GROUP BY A.DEFECTCD,B.DEFECTDESC,A.CWORKCTR \n" +
                                " ORDER BY CENTITYTQTY DESC";
            }
            else
            {
                 strSQL += "SELECT DISTINCT DEFECTCD,DEFECTDESC,MAX(TRANSDATE) TRANSDATE FROM( \n" +
               " SELECT A.DEFECTCD,B.DEFECTDESC,TRANSDATE TRANSDATE FROM New_WLTX A,rep.MDCM B \n" +
               " WHERE A.DEFECTCD=B.DEFECTCD \n" +
               " AND TRANSDATE BETWEEN '" + strFromTime + "' AND '" + strToTime + "' \n" +
               " AND A.CSUBLINEID='" + DDLProdLine.SelectedValue.ToString() + "' \n" +
               " AND B.FunctionalDef='1'" +
               " AND A.VERB='MVOU' \n" +                                              //V1.02 Add
               " GROUP BY A.DEFECTCD,B.DEFECTDESC,A.TRANSDATE \n" +
               " ) A \n" +
               " GROUP BY DEFECTCD,DEFECTDESC \n" +
               " ORDER BY TRANSDATE DESC";
            }
            ds = dbConn.GetDataSetBySql(strSQL);
            return ds;
        }

        #region  加載div2的Defect信息

        /// <summary>
        /// 初始化label控件，使得visible为false
        /// </summary>
        private void InitDiv2Crtls()
        {
            lblAISOtherDef.Text = "";
            lblAISOtherDef.Visible = false;
            lblbibotherdef.Text = "";
            lblbibotherdef.Visible = false;
            lbloqcotherdef.Text = "";
            lbloqcotherdef.Visible = false;

            for (int i = 0; i < 5; i++)
            {
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDef" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDef" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDesc" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDesc" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISqty" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISqty" + i)).Visible = false;

                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibdef" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibdef" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibdesc" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibdesc" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibqty" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibqty" + i)).Visible = false;

                ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcdef" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcdef" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcdesc" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcdesc" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcqty" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcqty" + i)).Visible = false;
            }

            for (int i = 0; i < 9; i++)
            {
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdef" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdef" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdesc" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdesc" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFunctime" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFunctime" + i)).Visible = false;
            }
        }
        /// <summary>
        /// 加載層2
        /// </summary>
        private void InitDiv2()
        {
            DataSet ds = GetDefInfo(false);
            DataRow[] dr8353 = ds.Tables[0].Select(" CWORKCTR='8353'");
            for (int i = 0; i < dr8353.Length; i++)
            {
                if (i < 5)
                {
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDef" + i)).Text = dr8353[i][0].ToString();
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDef" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDesc" + i)).Text = dr8353[i][2].ToString();
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISDesc" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISqty" + i)).Text = Convert.ToString(Math.Round(Convert.ToDouble(dr8353[i][1].ToString()), 0));
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblAISqty" + i)).Visible = true;
                }
                else
                {
                    lblAISOtherDef.Text += dr8353[i][0].ToString() + "：" + dr8353[i][2].ToString() + "------数量：" + Convert.ToString(Math.Round(Convert.ToDouble(dr8353[i][1].ToString()), 0)) + "<br>";
                    lblAISOtherDef.Visible = true;
                }
            }
            DataRow[] drBurnBaking = ds.Tables[0].Select(" CWORKCTR IN('8351','8352')");
            for (int i = 0; i < drBurnBaking.Length; i++)
            {
                if (i < 5)
                {
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibDef" + i)).Text = drBurnBaking[i][0].ToString();
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibDef" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibDesc" + i)).Text = drBurnBaking[i][2].ToString();
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibDesc" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibqty" + i)).Text = Convert.ToString(Math.Round(Convert.ToDouble(drBurnBaking[i][1].ToString()), 0));
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lblbibqty" + i)).Visible = true;
                }
                else
                {
                    lblbibotherdef.Text += drBurnBaking[i][0].ToString() + "：" + drBurnBaking[i][2].ToString() + "------数量：" + Convert.ToString(Math.Round(Convert.ToDouble(drBurnBaking[i][1].ToString()), 0)) + "<br>";
                    lblbibotherdef.Visible = true;
                }
            }
            DataRow[] DrOQC = ds.Tables[0].Select(" CWORKCTR IN ('8360')");
            for (int i = 0; i < DrOQC.Length; i++)
            {
                if (i < 5)
                {
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcDef" + i)).Text = DrOQC[i][0].ToString();
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcDef" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcDesc" + i)).Text = DrOQC[i][2].ToString();
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcDesc" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcqty" + i)).Text = Convert.ToString(Math.Round(Convert.ToDouble(DrOQC[i][1].ToString()), 0));
                    ((System.Web.UI.WebControls.Label)table_md.FindControl("lbloqcqty" + i)).Visible = true;
                }
                else
                {
                    lbloqcotherdef.Text += DrOQC[i][0].ToString() + "：" + DrOQC[i][2].ToString() + "------数量：" + Convert.ToString(Math.Round(Convert.ToDouble(DrOQC[i][1].ToString()), 0)) + "<br>";
                    lbloqcotherdef.Visible = true;
                }
            }

            DataSet ds1 = GetDefInfo(true);
            //Modify Start
            int countrow=ds1.Tables[0].Rows.Count;     
            if (countrow > 10)
            {
                countrow = 10;
            }
            //Modify End
            for (int i = 0; i < countrow; i++)//預設了10個Defect的位置






            {
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdef" + i)).Text = ds1.Tables[0].Rows[i][0].ToString();
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdef" + i)).Visible = true;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdesc" + i)).Text = ds1.Tables[0].Rows[i][1].ToString();
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFuncdesc" + i)).Visible = true;
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFunctime" + i)).Text = Convert.ToDateTime(ds1.Tables[0].Rows[i][2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                ((System.Web.UI.WebControls.Label)table_md.FindControl("lblFunctime" + i)).Visible = true;
            }
        }

        #endregion
        #region DIV3的Defect信息加載和控件初始化

        /// <summary>
        /// 加載層3
        /// </summary>
        private void InitDiv3()
        {
            strID = "";
            strFabID = "";
            strGroup = "";
            strEQID = "";

            strFrTime = "";
            strToTime = "";

            strID = "2";
            strFabID = "B";
            strGroup = "M2";
            strEQID = txtLine.Text;

            strFrTime = Convert.ToString(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));//格式只能這麼寫，YYYY和DD不能被識別，MM代表月，mm代表分鐘，HH代表24進制，hh代表12進制
            strToTime = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            QueryDiv3Defect();
            QueryDiv3StatusData();
        }
        /// <summary>
        /// 初始化Label控件，使得visib為false
        /// </summary>
        private void InitDiv3Ctrls()
        {
            lblCOGotherdef.Text = "";
            lblCOGotherdef.Visible = false;
            lblFOGotherdef.Text = "";
            lblFOGotherdef.Visible = false;

            for (int i = 0; i < 5; i++)
            {
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGdef" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGdef" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGdesc" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGdesc" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGqty" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGqty" + i)).Visible = false;

                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdef" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdef" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdesc" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdesc" + i)).Visible = false;
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGqty" + i)).Text = "";
                ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGqty" + i)).Visible = false;
            }
        }
        private void QueryDiv3Defect()
        {
            DataSet ds = GetDefInfo(false);
            DataRow[] drcog = ds.Tables[0].Select(" CWORKCTR='8210'");
            for (int i = 0; i < drcog.Length; i++)
            {
                if (i < 5)
                {
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGDef" + i)).Text = drcog[i][0].ToString();
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGDef" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGdesc" + i)).Text = drcog[i][2].ToString();
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGdesc" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGqty" + i)).Text = Convert.ToString(Math.Round(Convert.ToDouble(drcog[i][1].ToString()), 0));
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblCOGqty" + i)).Visible = true;
                }
                else
                {
                    lblCOGotherdef.Text += drcog[i][0].ToString() + "：" + drcog[i][2].ToString() + "------数量：" + Convert.ToString(Math.Round(Convert.ToDouble(drcog[i][1].ToString()), 0)) + "<br>";
                    lblCOGotherdef.Visible = true;
                }
            }
            DataRow[] drFog = ds.Tables[0].Select(" CWORKCTR IN('8230','8250','8270','8280')");
            for (int i = 0; i < drFog.Length; i++)
            {
                if (i < 5)
                {
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdef" + i)).Text = drFog[i][0].ToString();
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdef" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdesc" + i)).Text = drFog[i][2].ToString();
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGdesc" + i)).Visible = true;
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGqty" + i)).Text = Convert.ToString(Math.Round(Convert.ToDouble(drFog[i][1].ToString()), 0));
                    ((System.Web.UI.WebControls.Label)table_cf.FindControl("lblFOGqty" + i)).Visible = true;
                }
                else
                {
                    lblFOGotherdef.Text += drFog[i][0].ToString() + "：" + drFog[i][2].ToString() + "------数量：" + Convert.ToString(Math.Round(Convert.ToDouble(drFog[i][1].ToString()), 0)) + "<br>";
                    lblFOGotherdef.Visible = true;
                }
            }
        }

        #endregion
        #region  DIV3的24hours Status信息加載

        private void QueryDiv3StatusData()
        {
            StringBuilder sqlTable = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            StringBuilder sql1 = new StringBuilder();
            StringBuilder strsql = new StringBuilder();

            if (strID == "2")
            {
                sqlTable.Append(" FROM (SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLSH WHERE  FABID='" + strFabID + "' AND SUBLINEID=  '" + strEQID + "' AND ENDDATE>  '" + strFrTime + "' ");
                sqlTable.Append(" UNION ");
                sqlTable.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,'" + strToTime + "'  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLS WHERE   FABID='" + strFabID + "' AND SUBLINEID=  '" + strEQID + "' )A ");
            }
            else
            {
                sqlTable.Append(" FROM (SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLSH WHERE  FABID='" + strFabID + "'  AND ENDDATE>  '" + strFrTime + "' ");
                sqlTable.Append(" UNION ");
                sqlTable.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,'" + strToTime + "'  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLS WHERE   FABID='" + strFabID + "' )A ");
            }

            strsql.Append(" SELECT DISTINCT A.SUBLINEID,RIGHT(A.SUBLINEID,4) AS EQID,CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE B.PROCID END AS PROCESS ");
            //strsql.Append(" FROM ESLS A ");
            strsql.Append(sqlTable);
            strsql.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            strsql.Append(" INNER JOIN MSLM AA ON(A.FABID=AA.FABID AND A.SUBLINEID=AA.SUBLINEID AND AA.FLG = '0') ");
            strsql.Append(" WHERE A.FABID ='" + strFabID + "' ");
            strsql.Append(" AND CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE B.PROCID END =  '" + strGroup + "'");
            strsql.Append(" ORDER BY A.SUBLINEID ");
            DataTable strDT = this.dbConn.GetDataSetBySql(strsql.ToString()).Tables[0];

            sql.Append(" SELECT DISTINCT A.SUBLINEID,C.EQCCDESC,D.EQCCDCOLOR, ");
            sql.Append(" DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + strFrTime + "' THEN  '" + strFrTime + "' ELSE A.STARTDATE END,CASE WHEN ENDDATE>='" + strToTime + "' THEN '" + strToTime + "' ELSE ENDDATE END)  AS DMINUTE,   ");
            sql.Append(" '[Time:'+SUBSTRING(CONVERT(VARCHAR(8),STARTDATE,108),1,5) +'][ProdID:'+PRODID+'][Status Desc:'+EQCDDESC+'][UserName:'+E.USERNAME+']['+A.CMT+']' AS CMT,STARTDATE ");
            //sql.Append(" FROM ESLSH A ");
            sql.Append(sqlTable);
            sql.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            sql.Append(" INNER JOIN MSLM AA ON(A.FABID=AA.FABID AND A.SUBLINEID=AA.SUBLINEID AND AA.FLG = '0') ");
            sql.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD) ");
            sql.Append(" LEFT JOIN MECCU D ON(A.EQCCD = D.EQCCD) ");
            sql.Append(" LEFT JOIN MEMM E ON(A.FABID = B.FABID AND A.USERID = E.USERID) ");
            sql.Append(" WHERE A.FABID =  '" + strFabID + "'");
            sql.Append(" AND CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE B.PROCID END =  '" + strGroup + "'");
            sql.Append(" AND  CASE WHEN ENDDATE IS NULL THEN '" + strToTime + "' ELSE ENDDATE END >=  '" + strFrTime + "' ");
            sql.Append(" AND STARTDATE <=  '" + strToTime + "' ");
            sql.Append(" ORDER BY A.SUBLINEID,STARTDATE  ");
            DataTable DT = this.dbConn.GetDataSetBySql(sql.ToString()).Tables[0];

            sql1.Append(" SELECT A.SUBLINEID,C.EQCCDESC,D.EQCCDCOLOR, ");
            sql1.Append(" SUM(DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + strFrTime + "' THEN  '" + strFrTime + "' ELSE A.STARTDATE END,CASE WHEN ENDDATE>='" + strToTime + "' THEN '" + strToTime + "' ELSE ENDDATE END))  AS DMINUTE   ");
            //sql1.Append(" FROM ESLSH A ");
            sql1.Append(sqlTable);
            sql1.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            //sql1.Append(" INNER JOIN MSLM AA ON(A.FABID=AA.FABID AND A.SUBLINEID=AA.SUBLINEID AND AA.FLG = '0') ");
            sql1.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD) ");
            sql1.Append(" LEFT JOIN MECCU D ON(A.EQCCD = D.EQCCD) ");
            sql1.Append(" LEFT JOIN MEMM E ON(A.FABID = B.FABID AND A.USERID = E.USERID) ");
            sql1.Append(" WHERE A.FABID =  '" + strFabID + "'");
            sql1.Append(" AND CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE B.PROCID END =  '" + strGroup + "'");
            sql1.Append(" AND  CASE WHEN ENDDATE IS NULL THEN '" + strToTime + "' ELSE ENDDATE END >=  '" + strFrTime + "' ");
            sql1.Append(" AND STARTDATE <=  '" + strToTime + "' ");
            sql1.Append(" GROUP BY A.SUBLINEID,C.EQCCDESC,EQCCDCOLOR ORDER BY SUBLINEID  ");
            DataTable DT1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];

            if (strDT.Rows.Count > 0)
            {
                this.GridView1.DataSource = strDT;
                this.GridView1.DataBind();
                this.GridView1.Height = 20;

                this.GridView2.DataSource = strDT;
                this.GridView2.DataBind();
                this.GridView2.Height = 20;

                /*24Status*/
                for (int i = 0; i < strDT.Rows.Count; i++)
                {
                    int intCount = 1;
                    int intMod = 0;
                    int dblMod = 0;

                    DataRow[] dr00 = DT.Select("(SUBLINEID='" + strDT.Rows[i][0].ToString() + "')", "SUBLINEID,startdate");
                    for (int j = 0; j < Convert.ToInt32(dr00.Length); j++)
                    {
                        intMod = 0;
                        dblMod = dblMod + Convert.ToInt32(dr00[j].ItemArray[3].ToString()) % 5;

                        if (dblMod >= 5)
                        {
                            intMod = 1;
                            dblMod = dblMod - 5;
                        }
                        for (int k = 0; k < Convert.ToInt32(dr00[j].ItemArray[3].ToString()) / 5 + intMod; k++)
                        {
                            this.GridView1.Rows[i].Cells[intCount].ToolTip = dr00[j].ItemArray[4].ToString();
                            this.GridView1.Rows[i].Cells[intCount].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(dr00[j].ItemArray[2].ToString()));
                            intCount = intCount + 1;
                        }
                    }
                }

                /*Status %*/
                for (int i = 0; i < strDT.Rows.Count; i++)
                {
                    int intCount = 1;
                    int intMod = 0;
                    int dblMod = 0;

                    DataRow[] dr00 = DT1.Select("(SUBLINEID='" + strDT.Rows[i][0].ToString() + "')", "SUBLINEID");
                    for (int j = 0; j < Convert.ToInt32(dr00.Length); j++)
                    {
                        intMod = 0;
                        dblMod = dblMod + Convert.ToInt32(dr00[j].ItemArray[3].ToString()) % 5;

                        if (dblMod >= 5)
                        {
                            intMod = 1;
                            dblMod = dblMod - 5;
                        }
                        for (int k = 0; k < Convert.ToInt32(dr00[j].ItemArray[3].ToString()) / 5 + intMod; k++)
                        {
                            this.GridView2.Rows[i].Cells[intCount].ToolTip = dr00[j].ItemArray[3].ToString() + "(Mintue)";
                            this.GridView2.Rows[i].Cells[intCount].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(dr00[j].ItemArray[2].ToString()));
                            intCount = intCount + 1;
                        }
                    }
                }

                StringBuilder sql3 = new StringBuilder();
                sql3.Append(" select eqccdesc as status,eqccdcolor as col from MECCU order by eqccdesc ");
                DataTable dt3 = this.dbConn.GetDataSetBySql(sql3.ToString()).Tables[0];
                this.GridView4.DataSource = dt3;
                this.GridView4.DataBind();
                this.GridView4.Height = 20;

                BoundData(DT1, strDT);
            }
        }
        private void BoundData(DataTable strDT, DataTable DT01)
        {
            DataTable dtm1 = new DataTable();

            dtm1.Columns.Add(new DataColumn("SUBLINEID", typeof(string)));
            dtm1.Columns.Add(new DataColumn("RunTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("IdelTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("SetupTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("WaitTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("DownTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("PartialDownTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("PMTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("NonOperationTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("ENGTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("OtherTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("Availability", typeof(string)));
            dtm1.Columns.Add(new DataColumn("Utilization", typeof(string)));
            dtm1.Columns.Add(new DataColumn("SetupRate", typeof(string)));

            if (strID == "1")
            {
                /*計算TTL的值 Begin*/
                double SDown = 0, SENG = 0, SIDLE = 0, SNonOperation = 0, SOther = 0, SPM = 0, SRUN = 0, SSetup = 0, SPartialDown = 0, SWait = 0;
                for (int i = 0; i < strDT.Rows.Count; i++)
                {
                    if (strDT.Rows[i][1].ToString() == "Down")
                    {
                        SDown = SDown + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "ENG")
                    {
                        SENG = SENG + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "IDLE")
                    {
                        SIDLE = SIDLE + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "NonOperation")
                    {
                        SNonOperation = SNonOperation + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "Other")
                    {
                        SOther = SOther + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "PM")
                    {
                        SPM = SPM + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "RUN")
                    {
                        SRUN = SRUN + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "Setup")
                    {
                        SSetup = SSetup + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "PartialDown")
                    {
                        SPartialDown = SPartialDown + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if (strDT.Rows[i][1].ToString() == "Wait")
                    {
                        SWait = SWait + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                }
                DataRow drm0 = dtm1.NewRow();
                drm0.Table.Columns.Add();

                drm0["SUBLINEID"] = "Total";
                drm0["RunTime"] = Math.Round(SRUN / 60, 2);
                drm0["IdelTime"] = Math.Round(SIDLE / 60, 2);
                drm0["SetupTime"] = Math.Round(SSetup / 60, 2);
                drm0["WaitTime"] = Math.Round(SWait / 60, 2);
                drm0["DownTime"] = Math.Round(SDown / 60, 2);
                drm0["PartialDownTime"] = Math.Round(SPartialDown / 60, 2);
                drm0["PMTime"] = Math.Round(SPM / 60, 2);
                drm0["NonOperationTime"] = Math.Round(SNonOperation / 60, 2);
                drm0["ENGTime"] = Math.Round(SENG / 60, 2);
                drm0["OtherTime"] = Math.Round(SOther / 60, 2);

                if ((SRUN + SIDLE + SSetup + SWait + SDown + SPartialDown + SPM + SNonOperation + SENG + SOther) != 0)
                {
                    drm0["Availability"] = Math.Round((SRUN + SIDLE + SSetup) / (SRUN + SIDLE + SSetup + SWait + SDown + SPartialDown + SPM + SNonOperation + SENG + SOther) * 100, 2) + "%";
                }
                else
                {
                    drm0["Availability"] = "100%";
                }
                if ((SRUN + SIDLE + SSetup) != 0)
                {
                    drm0["Utilization"] = Math.Round((SRUN) / (SRUN + SIDLE + SSetup) * 100, 2) + "%";
                    drm0["SetupRate"] = Math.Round((SSetup) / (SRUN + SIDLE + SSetup) * 100, 2) + "%";
                }
                else
                {
                    drm0["Utilization"] = "100%";
                    drm0["SetupRate"] = "100%";
                }
                dtm1.Rows.Add(drm0);
                /*End*/
            }
            for (int i = 0; i < DT01.Rows.Count; i++)
            {
                double TDown = 0, TENG = 0, TIDLE = 0, TNonOperation = 0, TOther = 0, TPM = 0, TRUN = 0, TSetup = 0, TPartialDown = 0, TWait = 0;
                DataRow drm1 = dtm1.NewRow();
                drm1.Table.Columns.Add();

                DataRow[] dr01 = strDT.Select("(SUBLINEID='" + DT01.Rows[i][0].ToString() + "')");
                for (int j = 0; j < Convert.ToInt32(dr01.Length); j++)
                {
                    drm1["SUBLINEID"] = dr01[j].ItemArray[0].ToString();
                    if (dr01[j].ItemArray[1].ToString() == "RUN")
                    {
                        TRUN = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["RunTime"] = TRUN;
                    if (dr01[j].ItemArray[1].ToString() == "IDLE")
                    {
                        TIDLE = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["IdelTime"] = TIDLE;
                    if (dr01[j].ItemArray[1].ToString() == "Setup")
                    {
                        TSetup = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["SetupTime"] = TSetup;
                    if (dr01[j].ItemArray[1].ToString() == "Wait")
                    {
                        TWait = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["WaitTime"] = TWait;
                    if (dr01[j].ItemArray[1].ToString() == "Down")
                    {
                        TDown = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["DownTime"] = TDown;
                    if (dr01[j].ItemArray[1].ToString() == "PartialDown")
                    {
                        TPartialDown = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["PartialDownTime"] = TPartialDown;
                    if (dr01[j].ItemArray[1].ToString() == "PM")
                    {
                        TPM = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["PMTime"] = TPM;
                    if (dr01[j].ItemArray[1].ToString() == "NonOperation")
                    {
                        TNonOperation = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["NonOperationTime"] = TNonOperation;
                    if (dr01[j].ItemArray[1].ToString() == "ENG")
                    {
                        TENG = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["ENGTime"] = TENG;
                    if (dr01[j].ItemArray[1].ToString() == "Other")
                    {
                        TOther = Math.Round(Convert.ToDouble(dr01[j].ItemArray[3].ToString()) / 60, 2);
                    }
                    drm1["OtherTime"] = TOther;
                }
                if ((TDown + TENG + TIDLE + TNonOperation + TOther + TPM + TRUN + TSetup + TPartialDown + TWait) != 0)
                {
                    drm1["Availability"] = Math.Round((TRUN + TIDLE + TSetup) / (TDown + TENG + TIDLE + TNonOperation + TOther + TPM + TRUN + TSetup + TPartialDown + TWait) * 100, 2) + "%";
                }
                else
                {
                    drm1["Availability"] = "100%";
                }
                if ((TRUN + TIDLE + TSetup) != 0)
                {
                    drm1["Utilization"] = Math.Round((TRUN) / (TRUN + TIDLE + TSetup) * 100, 2) + "%";
                    drm1["SetupRate"] = Math.Round((TSetup) / (TRUN + TIDLE + TSetup) * 100, 2) + "%";
                }
                else
                {
                    drm1["Utilization"] = "100%";
                    drm1["SetupRate"] = "100%";
                }
                dtm1.Rows.Add(drm1);
            }

            this.GridView3.DataSource = dtm1;
            this.GridView3.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int aa = 0;
                for (int i = 0; i < 288; i++)
                {
                    if (i % 24 != 0)
                    {
                        e.Row.Cells[i + 1].Visible = false;
                    }
                    else
                    {
                        aa = aa + 1;
                        e.Row.Cells[i + 1].ColumnSpan = 24;
                        e.Row.Cells[i + 1].Text = "|" + Convert.ToDateTime(strFrTime).AddHours((aa - 1) * 2).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }
            }
            if (GridView1.Rows.Count > 1)
            {
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    if (this.GridView1.Rows[j].Cells[0].Text != "LineID")
                    {
                        GridView1.Rows[j].Cells[0].Attributes.Add("ondblclick", "window.open('../ems/EMSQuery.aspx?id=2&FabID=" + strFabID + "&Group=" + strGroup + "&EQID=" + this.GridView1.Rows[j].Cells[0].Text.Substring(this.GridView1.Rows[j].Cells[0].Text.Length - 4, 4) + "&FrDateTime=" + strFrTime + "&ToDateTime=" + strToTime + " ')");
                    }
                }
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int aa = 0;
                for (int i = 0; i < 288; i++)
                {
                    if (i % 24 != 0)
                    {
                        e.Row.Cells[i + 1].Visible = false;
                    }
                    else
                    {
                        aa = aa + 1;
                        e.Row.Cells[i + 1].ColumnSpan = 24;
                        e.Row.Cells[i + 1].Text = "|" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }
            }
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        }
        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < this.GridView4.Rows.Count; i++)
            {
                this.GridView4.Rows[i].Cells[1].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView4.Rows[i].Cells[1].Text.ToString()));
                this.GridView4.Rows[i].Cells[1].ForeColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView4.Rows[i].Cells[1].Text.ToString()));
            }
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF7;
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.GridView3.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }

        #endregion
    }
}