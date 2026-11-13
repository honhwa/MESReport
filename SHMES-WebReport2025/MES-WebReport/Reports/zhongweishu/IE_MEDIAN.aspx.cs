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
using Report.Library;
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Text;

namespace Report.zhongweishu
{
    public partial class IE_MEDIAN : System.Web.UI.Page
    {
        //DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ModuleClass mc = new ModuleClass();
                //string strNTName = "";
                //string strPCIP = "";
                //string strPCName = "";
                //mc.GetLoginInfo(out strNTName, out strPCIP, out strPCName);
                //mc.RecordLogInfo("", "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "IE_MEDIAN", "".ToString(), "", "");
                //if (mc.GetMPCIPM(HttpContext.Current.Request.Url.AbsolutePath.ToString(), strPCIP) == false)
                //{
                //    Response.Redirect("../ShowMessage.aspx");
                //    return;
                //}

                txtDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                txtDate2.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                GetWorkctr();
                GetLineID();
                GetProdid();
                GetSizeID();



            }
        }
        protected void GetLineID()
        {
            DBConn dbConn;
            DataTable dtLineID = new DataTable();

            dbConn = new DBConn();

            string strSql = "SELECT '' AS EQLINE,'--請選擇--' AS EQLINEDESC";
            strSql += " UNION ALL";
            strSql += " SELECT DISTINCT EQLINE,EQLINE AS EQLINEDESC FROM Median_TT_REPORT   ";
            strSql += "WHERE WORKCTR ='" + ddlWorkctr.SelectedValue + "'AND";
            strSql += " EQLINE != '' AND  EQLINE IS NOT NULL ORDER BY EQLINE";
            dtLineID = dbConn.GetDataSetBySql(strSql.ToString()).Tables[0];
            ddlLineID.DataSource = dtLineID;
            ddlLineID.DataTextField = "EQLINEDESC";
            ddlLineID.DataValueField = "EQLINE";
            ddlLineID.DataBind();
        }
        protected void GetWorkctr()
        {
            DBConn dbConn;
            DataTable dtWorkctr = new DataTable();

            dbConn = new DBConn();

            string strSql = "SELECT '' AS WORKCTR,'--請選擇--' AS WORKDESC";
            strSql += " UNION ALL";
            strSql += " SELECT distinct WORKCTR,WORKCTR AS WORKDESC FROM Median_TT_REPORT ";
            strSql += "  ORDER BY WORKCTR";
            dtWorkctr = dbConn.GetDataSetBySql(strSql.ToString()).Tables[0];
            ddlWorkctr.DataSource = dtWorkctr;
            ddlWorkctr.DataTextField = "WORKDESC";
            ddlWorkctr.DataValueField = "WORKCTR";
            ddlWorkctr.DataBind();

        }
        /// <summary>
        /// 穫取機种
        /// xiaoshuai
        /// 2019/07/09
        /// </summary>
        /// <param name="strFab"></param>
        protected void GetProdid()
        {
            DBConn dbConn;
            DataTable dtProdid = new DataTable();
            //if (strFab == "A")
            //{
            //    dbConn = new DBConn("PLCM1HIS");
            //}
            //else
            //{
            //dbConn = new DBConn("PLCM2HIS");
            //}
            dbConn = new DBConn();

            string strSql = "SELECT '' AS PRODID,'--請選擇--' AS PRODIDDESC";
            strSql += " UNION ALL";
            strSql += " SELECT DISTINCT PRODID,PRODID AS PRODIDDESC FROM Median_TT_REPORT  ";
            strSql += " WHERE EQLINE='" + ddlLineID.SelectedValue + "' AND EQLINE !='' AND EQLINE IS NOT NULL ORDER BY PRODID";
            dtProdid = dbConn.GetDataSetBySql(strSql.ToString()).Tables[0];
            ddlProdid.DataSource = dtProdid;
            ddlProdid.DataTextField = "PRODIDDESC";
            ddlProdid.DataValueField = "PRODID";
            ddlProdid.DataBind();
        }
        /// <summary>
        /// 根據所選廠別，穫得機种
        /// xiaoshuai
        /// 2019/07/09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*protected void ddlFabId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFab = ddlFabId.SelectedValue;

            if (strFab == "")
            {

                return;
            }
            else
            {
                GetProdid(strFab);
            }
        }*/
        // protected void ddlFabId_SelectedIndexChanged(object sender, EventArgs e)
        //{ 
        //     GetProdid();   
        //}
        /// <summary>
        /// 根據選擇的機种，穫取sizeid
        /// xiaoshuai
        /// 2019/07/09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetSizeID()
        {
            DBConn dbConn;
            DataTable dtSizeid = new DataTable();

            //string strFab = ddlFabId.SelectedValue;
            //if (strFab == "A")
            //{
            //    dbConn = new DBConn("PLCM1HIS");
            //}
            //else
            //{
            //    dbConn = new DBConn("PLCM2HIS");
            //}
            dbConn = new DBConn();
            string strSql = "SELECT '' AS SIZEID,'--請選擇--' AS SIZEIDDESC";
            strSql += " UNION ALL ";
            strSql += "SELECT DISTINCT SIZEID , SIZEID AS SIZEIDDESC FROM Median_TT_REPORT  ";
            //strSql += "LEFT JOIN ";
            //strSql += "(SELECT SIZEID,PRODID FROM MPIM ";
            //strSql += ")K ON TOPRODID = PRODID ";
            strSql += "WHERE EQLINE = '" + ddlLineID.SelectedValue + "' AND EQLINE !='' AND EQLINE IS NOT NULL ";
            //strSql += " SELECT distinct SIZEID ,SIZEID AS SIZEIDDESC FROM rep.MPIM WHERE CSUBLINEID='" + ddlLineID.SelectedValue + "' ORDER BY SIZEID ";
            dtSizeid = dbConn.GetDataSetBySql(strSql.ToString()).Tables[0];
            ddlProdidSize.DataSource = dtSizeid;
            ddlProdidSize.DataTextField = "SIZEIDDESC";
            ddlProdidSize.DataValueField = "SIZEID";
            ddlProdidSize.DataBind();
        }
        /// <summary>
        /// 根據查詢條件，查詢數據
        /// XIAOSHUAI 
        /// 2019/07/09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strDate = txtDate.Text.ToString();
            string strDate2 = txtDate2.Text.ToString();
            string strWorkctr = ddlWorkctr.SelectedValue;
            string strLineID = ddlLineID.SelectedValue;
            string strProdid = ddlProdid.SelectedValue;
            string strProdSize = ddlProdidSize.SelectedValue;
            DBConn dbConn = new DBConn();
            DataTable dtInfo = new DataTable();
            //DBConn dbConn = new DBConn("CNDBConnectionStringName");

            //查詢信息驗證
            //if (strDate == "")
            //{
            //    if (strWorkctr != "" || strLineID != "" || ddlProdid.SelectedValue != "" || ddlProdidSize.SelectedValue != "")

            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇日期！');</script>");
            //    return;
            // }

            //else if (strWorkctr == "")
            //{
            //    if (strLineID != "" || strProdid != "" || strProdSize != "")
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇站點');</script>");
            //    return;
            //}
            //else if (strLineID == "")
            //{
            //    if (strProdid != "" || strProdSize != "")
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇線別');</script>");
            //    return;
            //}
            //else if (strProdid == "" && strProdSize == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇機種 或SIZE！');</script>");
            //    return;
            //}
            if (strProdid != "" && strProdSize != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('機種 或 SIZE 請擇一選填！');</script>");
                return;
            }
            if (strDate == "" && strProdSize != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇日期！');</script>");
                return;
            }


            //string strSql = " SELECT A.SIZEID,A.PRODID_ID,A.SITE ,A.PLANT ,A.EQLINE ,A.QTY ,TACTTIMEALLOW_BN AS TARGET , A.MEDIAN AS ACTUAL,";
            //strSql += " CAST(ROUND((A.MEDIAN-B.TACTTIMEALLOW_BN)/B.TACTTIMEALLOW_BN,2) AS DECIMAL(18,2)) AS DIFF";
            //strSql += " FROM    dbo.RPT_MEDIAN A JOIN IE_MEDIAN_TEST B";
            //strSql += " ON A.FABID=B.FACILITY_NAME AND A.PRODID_ID=B.PRODID AND A.EQLINE=B.EQID";
            //strSql += " WHERE REPORTDATE ='" + strDate + "'";
            //strSql += " AND A.FABID='" + strFabid + "'";
            //strSql += " AND A.PRODID_ID='" + strProdid + "'";
            //strSql += " AND A.SIZEID='" + strProdSize + "'";




            string strSql = " SELECT * FROM Median_TT_REPORT  ";
            //strSql +=


            //string strSql = " SELECT MFGDATE AS DATE ,SIZEID, PROJECT_NAME , PRODGROUP, TOPRODID AS PRODID, SITE = 'SH',FABID, CSUBLINEID AS EQLINE,  C AS QTY, TARGET= CAST(AVG(TT*1.0) / 1000.0 AS NUMERIC(9,2) ) , ACTUAL ='1' ,DIFF ='2'";
            //strSql += " FROM";
            //strSql += " 	(";
            //strSql += " SELECT TOPRODID,ScrapMode, FABID, CSUBLINEID, TT, rn, c, a, CWORKCTR,s,SIZEID,MFGDATE, PROJECT_NAME, PRODGROUP";
            //strSql += " 	 FROM ";
            //strSql += " 		(SELECT MFGDATE,";
            //strSql += " 				ScrapMode,";
            //strSql += " 				TOPRODID,";
            //strSql += " 				FABID,";
            //strSql += " 				CSUBLINEID, ";
            //strSql += " 				TT,";
            //strSql += " 				rn=ROW_NUMBER() OVER (PARTITION BY MFGDATE,TOPRODID ORDER BY TT), ";
            //strSql += " 				c=COUNT(0)OVER(PARTITION BY MFGDATE,TOPRODID),";
            //strSql += " 				a= AVG(TT)OVER (PARTITION BY MFGDATE,TOPRODID),";
            //strSql += " 			    s=STDEV(TT)OVER(PARTITION BY MFGDATE,TOPRODID),";
            //strSql += " 			    CWORKCTR,";
            //strSql += " 			    SIZEID, PROJECT_NAME, PRODGROUP";
            //strSql += " 		 FROM ";
            //strSql += " 			(SELECT MFGDATE,";
            //strSql += " 					TOPRODID,";
            //strSql += " 					ScrapMode,";
            //strSql += " 					FABID,";
            //strSql += " 					CSUBLINEID,";
            ////strSql += " 					-- TT CALCULATE START ";
            //strSql += " 					TT, ";
            //strSql += " 					rn=ROW_NUMBER() OVER (PARTITION BY MFGDATE,TOPRODID ORDER BY TT), ";
            //strSql += " 					c=COUNT(0)OVER(PARTITION BY MFGDATE,TOPRODID),";
            //strSql += " 					a= AVG(TT)OVER (PARTITION BY MFGDATE,TOPRODID),";
            //strSql += " 			    	s=STDEV(TT)OVER(PARTITION BY MFGDATE,TOPRODID), ";
            //strSql += " 			    	CWORKCTR,";
            //strSql += " 			    	SIZEID, PROJECT_NAME, PRODGROUP";
            //strSql += " 			 FROM";
            //strSql += "   				(SELECT w1.*,";
            ////strSql += "   				--DB DATA START"; 
            //strSql += " 						w2.PANELID PANELID1,";
            //strSql += " 						w2.TRANSDATE TRANSDATE1,";
            ////strSql += " 						DT=CONVERT(varchar(20),w1.TRANSDATE, 23),";
            //strSql += " 						case when w2.TRANSDATE is not null then datediff(s, w1.TRANSDATE,w2.TRANSDATE) END [TT],";
            //strSql += " 						PRODID, SIZEID, PROJECT_NAME, PRODGROUP";
            //strSql += " 				 FROM ";
            //strSql += " 					(SELECT  u.TOPRODID,u.MFGDATE,";
            //strSql += " 							 u.FABID,";
            //strSql += " 							 u.ScrapMode,";
            //strSql += " 							 u.PANELID, ";
            //strSql += " 							 u.CWORKCTR,";
            //strSql += " 							 u.FRPRODID,";
            //strSql += " 							 u.CSUBLINEID,";
            //strSql += " 							 u.TRANSDATE,";
            //strSql += " 							 ROW_NUMBER() over (partition by u.CWORKCTR,";
            //strSql += " 							 u.FRPRODID,";
            //strSql += " 							 u.CSUBLINEID order BY u.TRANSDATE ";
            //strSql += " 							 ) sn";
            //strSql += " 					 FROM";
            //strSql += " 						(SELECT w.TOPRODID,w.MFGDATE,";
            //strSql += " 								w.FABID,";
            //strSql += " 								w.ScrapMode,";
            //strSql += " 								w.FRENTITYID PANELID,";
            //strSql += " 								w.CWORKCTR,";
            //strSql += " 								w.FRPRODID,";
            //strSql += " 								w.CSUBLINEID,";
            //strSql += " 								w.TRANSDATE";
            //strSql += "    						 FROM WLTX w ";//WITH(NOLOCK)
            ////strSql += "    						 WHERE w.FABID='B'  AND  w.VERB='MVOU' AND CWORKCTR='8230'-- w.CSUBLINEID  IN ('2F-BOD-L03') ";


            //strSql += "                          WHERE w.FABID='B' AND  w.VERB='MVOU' ";


            //if (txtDate.Text.ToString() == "")
            //{
            //    if(ddlWorkctr.SelectedValue!=""||ddlLineID.SelectedValue!=""||ddlProdid.SelectedValue!=""||ddlProdidSize.SelectedValue!="")

            //    strSql += " AND MFGDATE     ='" + strDate + "'";


            //}
            //else if (txtDate.Text.ToString() == "" && ddlWorkctr.SelectedValue != "" || ddlLineID.SelectedValue != "" || ddlProdid.SelectedValue != "" || ddlProdidSize.SelectedValue != "")
            //{
            //    strSql += " AND MFGDATE     ='" + strDate + "'";
            //    strSql += " AND CWORKCTR    ='" + strWorkctr + "'";
            //    strSql += " AND CSUBLINEID  ='" + strLineID + "'";
            //    strSql += " AND FRPRODID    ='" + strProdid + "'";
            //}
            //0812
            if (/*txtDate.Text.ToString() != "" && */ddlWorkctr.SelectedValue == "" && ddlLineID.SelectedValue == "" && ddlProdid.SelectedValue == "" && ddlProdidSize.SelectedValue == "")
            {
                strSql += " WHERE DATE     >='" + strDate + "'";
                strSql += " AND DATE     <='" + strDate2 + "'";

            }

            else if (ddlWorkctr.SelectedValue != "" && ddlLineID.SelectedValue == "" && ddlProdid.SelectedValue == "" && ddlProdidSize.SelectedValue == "")
            {
                strSql += " WHERE DATE     >='" + strDate + "'";
                strSql += " AND DATE     <='" + strDate2 + "'";
                strSql += " AND WORKCTR    ='" + strWorkctr + "'";

            }
            else if (ddlLineID.SelectedValue != "" && ddlProdid.SelectedValue == "" && ddlProdidSize.SelectedValue == "")
            {
                strSql += " WHERE DATE     >='" + strDate + "'";
                strSql += " AND DATE     <='" + strDate2 + "'";
                strSql += " AND WORKCTR    ='" + strWorkctr + "'";
                strSql += " AND EQLINE  ='" + strLineID + "'";

            }
            else if (ddlProdid.SelectedValue != "")
            {
                strSql += " WHERE DATE     >='" + strDate + "'";
                strSql += " AND DATE     <='" + strDate2 + "'";
                strSql += " AND WORKCTR    ='" + strWorkctr + "'";
                strSql += " AND EQLINE  ='" + strLineID + "'";
                strSql += " AND PRODID    ='" + strProdid + "'";
            }
            else if (ddlProdid.SelectedValue != "")
            {
                strSql += " WHERE DATE     >='" + strDate + "'";
                strSql += " AND DATE     <='" + strDate2 + "'";
                strSql += " AND WORKCTR    ='" + strWorkctr + "'";
                strSql += " AND EQLINE  ='" + strLineID + "'";
                strSql += " AND PRODID    ='" + strProdid + "'";
            }

            else if (ddlProdid.SelectedValue == "")
            {

                strSql += " WHERE DATE     >='" + strDate + "'";
                strSql += " AND DATE     <='" + strDate2 + "'";
                strSql += " AND WORKCTR    ='" + strWorkctr + "'";
                strSql += " AND EQLINE  ='" + strLineID + "'";


            }

            strSql += " order by date asc, prodid";








            //strSql += " 						)u";
            //strSql += " 					 )w1";
            //strSql += " LEFT JOIN ";
            //strSql += " (SELECT v.TOPRODID, ";
            //strSql += " 		v.PANELID, ";
            //strSql += " 		v.CWORKCTR,";
            //strSql += " 		v.FRPRODID,";
            //strSql += " 		v.CSUBLINEID,";
            //strSql += " 		v.TRANSDATE,";
            //strSql += " 		ROW_NUMBER() over (partition by v.CWORKCTR,";
            //strSql += " 		v.FRPRODID,";
            //strSql += " 		v.CSUBLINEID order BY v.TRANSDATE ";
            //strSql += " 		) sn";
            //strSql += "    FROM";
            //strSql += " 	(SELECT v.TOPRODID, ";
            //strSql += " 			v.FRENTITYID PANELID,";
            //strSql += " 			v.CWORKCTR,";
            //strSql += " 			v.FRPRODID,";
            //strSql += " 			v.CSUBLINEID,";
            //strSql += " 			v.TRANSDATE ";
            //strSql += " 	  FROM WLTX  v WITH(NOLOCK)";
            //strSql += "       WHERE v.FABID='B' AND  v.VERB='MVOU' ";
            ////strSql += " 	  AND CWORKCTR='8230'";
            ////strSql += " 	  AND v.CSUBLINEID IN ('2F-BOD-L03')";


            //if (/*txtDate.Text.ToString() != "" &&*/ddlWorkctr.SelectedValue == "" && ddlLineID.SelectedValue == "" && ddlProdid.SelectedValue == "" && ddlProdidSize.SelectedValue == "")
            //{
            //    strSql += " AND MFGDATE     >='" + strDate + "'";
            //    strSql += " AND MFGDATE     <='" + strDate2 + "'";

            //}

            //else if (ddlWorkctr.SelectedValue != "" && ddlLineID.SelectedValue == "" && ddlProdid.SelectedValue == "" && ddlProdidSize.SelectedValue == "")
            //{
            //    strSql += " AND MFGDATE     >='" + strDate + "'";
            //    strSql += " AND MFGDATE     <='" + strDate2 + "'";
            //    strSql += " AND CWORKCTR    ='" + strWorkctr + "'";

            //}
            //else if (ddlLineID.SelectedValue != "" && ddlProdid.SelectedValue == "" && ddlProdidSize.SelectedValue == "")
            //{
            //    strSql += " AND MFGDATE     >='" + strDate + "'";
            //    strSql += " AND MFGDATE     <='" + strDate2 + "'";
            //    strSql += " AND CWORKCTR    ='" + strWorkctr + "'";
            //    strSql += " AND CSUBLINEID  ='" + strLineID + "'";

            //}
            //else if (ddlProdid.SelectedValue != "")
            //{
            //    strSql += " AND MFGDATE     >='" + strDate + "'";
            //    strSql += " AND MFGDATE     <='" + strDate2 + "'";
            //    strSql += " AND CWORKCTR    ='" + strWorkctr + "'";
            //    strSql += " AND CSUBLINEID  ='" + strLineID + "'";
            //    strSql += " AND FRPRODID    ='" + strProdid + "'";
            //}

            //else if (ddlProdid.SelectedValue == "")
            //{

            //    strSql += " AND MFGDATE     >='" + strDate + "'";
            //    strSql += " AND MFGDATE     <='" + strDate2 + "'";
            //    strSql += " AND CWORKCTR    ='" + strWorkctr + "'";
            //    strSql += " AND CSUBLINEID  ='" + strLineID + "'";
            //    strSql += " AND SIZEID      ='" + strProdSize + "'";


            //}

            //strSql += " 	)v";
            //strSql += " )w2 ON w1.CWORKCTR=w2.CWORKCTR AND w1.FRPRODID=w2.FRPRODID  AND w1.sn+1=w2.sn AND w1.CSUBLINEID=w2.CSUBLINEID ";
            //strSql += " LEFT JOIN ";
            //strSql += " (";
            //strSql += "  SELECT PRODID, SIZEID, PROJECT_NAME, PRODGROUP";
            //strSql += "  FROM MPIM";
            //if (ddlProdid.SelectedValue == "")
            //{

            //    strSql += " WHERE  SIZEID      ='" + strProdSize + "'";

            //}

            //if (ddlProdidSize.SelectedValue != "")
            //{
            //    strSql += " WHERE  SIZEID      ='" + strProdSize + "'";
            //}
            //if (ddlProdid.SelectedValue == "")
            //{
            //    strSql += " WHERE SIZEID ='" + strProdSize + "'";

            //}
            //strSql += " )SZ ON SZ.PRODID = w2.TOPRODID ";
            //strSql += " )d     ";
            ////strSql += " --THE END OF DB DATA    ";
            //strSql += " WHERE TT > 3 AND TT IS NOT NULL ";
            //strSql += " )E";
            //strSql += " WHERE TT <= a+3*s AND TT>= a-3*s";
            //strSql += " )S";
            //strSql += " WHERE TT <= a+1*s AND TT>= a-1*s";
            //strSql += " )S2";
            //strSql += " WHERE rn IN ((c + 1)/2, (c + 2)/2)";
            //strSql += " GROUP BY  CSUBLINEID,TOPRODID,FABID,C, ScrapMode, CWORKCTR,SIZEID, MFGDATE, PROJECT_NAME, PRODGROUP;";















            dtInfo = dbConn.GetDataSetBySql(strSql.ToString()).Tables[0];
            if (dtInfo.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查無信息！');</script>");
                return;
            }
            else
            {
                gvMedian.DataSource = dtInfo;
                gvMedian.DataBind();
            }
        }
        /// <summary>
        /// 構造復合表頭
        /// xiaoshuai
        /// 2019/07/09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMedian_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;
                //清除自动生成的表头
                tcHeader.Clear();
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[0].RowSpan = 2;
                //tcHeader[0].Text = "Size";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[1].RowSpan = 2;
                //tcHeader[1].Text = "Prod.ID";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[2].RowSpan = 2;
                //tcHeader[2].Text = "SITE";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[3].RowSpan = 2;
                //tcHeader[3].Text = "PLANT";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[4].RowSpan = 2;
                //tcHeader[4].Text = "EQ LINE";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[5].RowSpan = 2;
                //tcHeader[5].Text = "QTY";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[6].ColumnSpan = 3;
                //tcHeader[6].Text = "TactTime</th></tr><tr>";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[7].Text = "Target";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[8].Text = "Actual";
                //tcHeader.Add(new TableHeaderCell());
                //tcHeader[9].Text = "Diff(%)";


                tcHeader.Add(new TableHeaderCell());
                tcHeader[0].RowSpan = 1;
                tcHeader[0].Text = "DATE";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[1].RowSpan = 1;
                tcHeader[1].Text = "SIZEID";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[2].RowSpan = 1;
                tcHeader[2].Text = "PROJECT_NAME";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[3].RowSpan = 1;
                tcHeader[3].Text = "PRODGROUP";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[4].RowSpan = 1;
                tcHeader[4].Text = "PRODID";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[5].RowSpan = 1;
                tcHeader[5].Text = "WORKCTR";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[6].RowSpan = 1;
                tcHeader[6].Text = "EQTYPE";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[7].RowSpan = 1;
                tcHeader[7].Text = "SITE";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[8].RowSpan = 1;
                tcHeader[8].Text = "PLANT";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[9].RowSpan = 1;
                tcHeader[9].Text = "FABID";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[10].RowSpan = 1;
                tcHeader[10].Text = "EQ LINE";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[11].ColumnSpan = 1;
                tcHeader[11].Text = "QTY";
                tcHeader.Add(new TableHeaderCell());

                //tcHeader[9].ColumnSpan = 2;
                //tcHeader[9].Text = "TACTTIME<th></tr><tr>";
                //tcHeader.Add(new TableHeaderCell());
                tcHeader[12].ColumnSpan = 1;
                tcHeader[12].Text = "TARGET";
                tcHeader.Add(new TableHeaderCell());

                tcHeader[13].ColumnSpan = 1;
                tcHeader[13].Text = "ACTUAL";
                tcHeader.Add(new TableHeaderCell());

                tcHeader[14].ColumnSpan = 1;
                tcHeader[14].Text = "DIFF(%)";
                tcHeader.Add(new TableHeaderCell());







            }
        }
        /// <summary>
        /// 修改數據格式，給DIFF欄位添加百分號
        /// xiaoshuai
        /// 2019/07/09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /* protected void gvMedian_RowDataBound(object sender, GridViewRowEventArgs e)
         {
             /*if (e.Row.RowType == DataControlRowType.DataRow)
             {
                 if (e.Row.Cells[9].Text.ToUpper() != "&nbsp;".ToUpper())
                 {
                     if (Convert.ToDouble(e.Row.Cells[9].Text.ToString()) > 0)
                     {
                         e.Row.Cells[9].ForeColor = Color.Red;
                     }
                     else
                     {
                         e.Row.Cells[9].ForeColor = Color.Blue;
                     }

                     e.Row.Cells[9].Text = string.Format("{0:0.00%}", Convert.ToDouble(e.Row.Cells[9].Text.ToString()));
                 }
             }
         }*/
        

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {

        }
        protected void dllWorkctr_SelectIndexChanged(object sender, EventArgs e)
        {
            GetLineID();
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProdid();
            GetSizeID();
        }
        protected void ddlProdid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlProdidSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ImageMap1_Click(object sender, ImageMapEventArgs e)
        {

        }
        protected void gvMedian_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}