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
using System.Data.SqlClient;
using Report.Library;
using System.Text;
/*
 *   2015-01-28    V1.01    Inx/Runy   由於修改了Process和線別的命名規則，修正線別和Process
 */
namespace Report.Reports.EMS
{
    public partial class EMSQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public string strID="";
        public string strFabID="";
        public string strEQID = "";
        public string strGroup = "";
        public string strFrTime = "";
        public string strToTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //StringBuilder sql = new StringBuilder();

            //sql.Append(" SELECT CONVERT(VARCHAR(20),getdate(),120),CONVERT(VARCHAR(20),getdate()-1,120) ");
            //DataTable DT = this.dbConn.GetDataSetBySql(sql.ToString()).Tables[0];

            //strFrTime = DT.Rows[0][1].ToString();
            //strToTime = DT.Rows[0][0].ToString();
            if (!this.IsPostBack)
            {
                strID = "";
                strFabID = "";
                strGroup = "";
                strEQID = "";

                strFrTime = "";
                strToTime = "";

                strID = this.Request.QueryString["id"].ToString();
                strFabID = this.Request.QueryString["FabID"].ToString();
                strGroup = this.Request.QueryString["Group"].ToString();
                strEQID = this.Request.QueryString["EQID"].ToString();

                strFrTime = this.Request.QueryString["FrDateTime"].ToString();
                strToTime = this.Request.QueryString["ToDateTime"].ToString();

                QueryData();
            }
        }

        private void QueryData()
        {
            StringBuilder sqlTable = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            StringBuilder sql1 = new StringBuilder();
            StringBuilder strsql = new StringBuilder();

            if (strID == "2")
            {
                sqlTable.Append(" FROM (SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLSH WHERE  FABID='" + strFabID + "' AND RIGHT(SUBLINEID,4)=  '" + strEQID + "' AND ENDDATE>  '" + strFrTime + "' ");
                sqlTable.Append(" UNION ");
                sqlTable.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,'" + strToTime + "'  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLS WHERE   FABID='" + strFabID + "' AND RIGHT(SUBLINEID,4)=  '" + strEQID + "' )A ");
            }
            //V1.01 Add Start
            else if (strID == "3")
            {
                sqlTable.Append(" FROM (SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLSH WHERE  FABID='" + strFabID + "' AND SUBLINEID=  '" + strEQID + "' AND ENDDATE>  '" + strFrTime + "' ");
                sqlTable.Append(" UNION ");
                sqlTable.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,'" + strToTime + "'  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLS WHERE   FABID='" + strFabID + "' AND SUBLINEID= '" + strEQID + "' )A ");
            }
            else if (strID == "4")
            {
                sqlTable.Append(" FROM (SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLSH WHERE  FABID='" + strFabID + "' AND SUBLINEID LIKE  '%" + strEQID + "%' AND ENDDATE>  '" + strFrTime + "' ");
                sqlTable.Append(" UNION ");
                sqlTable.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,'" + strToTime + "'  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
                sqlTable.Append(" FROM New_ESLS WHERE   FABID='" + strFabID + "' AND SUBLINEID LIKE '%" + strEQID + "%' )A ");
            }
            //V1.01 Add End
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
                double SDown= 0,SENG= 0, SIDLE= 0, SNonOperation= 0, SOther= 0, SPM= 0, SRUN= 0, SSetup= 0, SPartialDown= 0,  SWait= 0;
                for (int i = 0; i < strDT.Rows.Count; i++)
                {
                    if(strDT.Rows[i][1].ToString()=="Down")
                    {
                        SDown = SDown + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="ENG")
                    {
                        SENG = SENG + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="IDLE")
                    {
                        SIDLE = SIDLE + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="NonOperation")
                    {
                        SNonOperation = SNonOperation + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="Other")
                    {
                        SOther = SOther + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="PM")
                    {
                        SPM = SPM + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="RUN")
                    {
                        SRUN = SRUN + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="Setup")
                    {
                        SSetup = SSetup + Convert.ToDouble(strDT.Rows[i][3].ToString());
                    }
                    if(strDT.Rows[i][1].ToString()=="PartialDown")
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
	            for(int i=0;i<288;i++)
	            {
                    if (i % 24 != 0)
                    {
                        e.Row.Cells[i + 1].Visible = false;
                    }
                    else
                    {
                        aa = aa + 1;
                        e.Row.Cells[i+1].ColumnSpan = 24;
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
