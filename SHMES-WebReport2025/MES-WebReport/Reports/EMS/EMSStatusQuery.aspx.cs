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
    public partial class EMSStatusQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public string strFabid = "B";
        public string strFrTime = "";
        public string strToTime = "";
        public static string[] strProcessOrder ={ "TP", "POL", "COG", "BOD", "LAM", "ASSY", "OVEN" };//V1.01 Add 排列順序
        ArrayList arrOrder = new ArrayList(strProcessOrder);//V1.01 Add 是數組成為ArrayList

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {

            }
        }
        private void StatusQuery()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT CONVERT(VARCHAR(20),getdate(),120),CONVERT(VARCHAR(20),getdate()-1,120) ");
            DataTable DTTime = this.dbConn.GetDataSetBySql(sql.ToString()).Tables[0];

            strFrTime = DTTime.Rows[0][1].ToString();
            strToTime = DTTime.Rows[0][0].ToString();


            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();

            //sql2.Append(" SELECT CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE B.PROCID END AS PROCESS, ");
            //sql2.Append(" SUM(CASE WHEN C.EQCCDESC = 'RUN' THEN 1 ELSE 0 END) AS RCOUNT,COUNT(1) AS TCOUNT,B.WORKCTR ");
            //sql2.Append(" FROM ESLS A ");
            //sql2.Append(" INNER JOIN MSLM AA ON(A.FABID=AA.FABID AND A.SUBLINEID=AA.SUBLINEID AND AA.FLG = '0') ");
            //sql2.Append(" LEFT JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            //sql2.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD) ");
            //sql2.Append(" WHERE A.FABID = '" + strFabid + "' GROUP BY B.SHOPCD,B.WORKNAME,B.PROCID,B.WORKCTR ORDER BY B.WORKCTR ");

            //sql2.Append(" SELECT CASE WHEN PROCESS = 'MODULE' THEN 'M4' ELSE PROCESS END AS PROCESS,SUM(RCOUNT)  AS RCOUNT,COUNT(1) AS TCOUNT,STATION FROM(");//V1.01 Mark
            sql2.Append(" SELECT * FROM ( SELECT PROCESS,[FLOOR],SUM(RCOUNT)  AS RCOUNT,COUNT(1) AS TCOUNT,STATION FROM( ");//V1.01 Add 
            //sql2.Append(" SELECT DISTINCT CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE B.PROCID END AS PROCESS, ");//V1.01 Mark
            sql2.Append(" SELECT DISTINCT CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE LEFT(RIGHT(AA.sublineid,LEN(AA.sublineid)-3),CHARINDEX('-',RIGHT(AA.sublineid,LEN(AA.sublineid)-3))-1) END AS PROCESS, ");//V1.01 Add
            sql2.Append(" CASE WHEN B.SHOPCD = 'CELL2' THEN '' ELSE SUBSTRING(AA.SUBLINEID,1,2) END AS [FLOOR],");//V1.01 Add 增加樓層
            sql2.Append(" CASE WHEN C.EQCCDESC = 'RUN' THEN 1 ELSE 0 END AS RCOUNT,TRANSDATE, ");
            sql2.Append(" CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKCTR ELSE B.PROCID END AS STATION ");
            sql2.Append(" FROM New_ESLS A ");
            sql2.Append(" INNER JOIN MSLM AA ON(A.FABID=AA.FABID AND A.SUBLINEID=AA.SUBLINEID AND AA.FLG = '0') ");
            sql2.Append(" LEFT JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            sql2.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD) ");
            sql2.Append(" WHERE A.FABID = '" + strFabid + "')AA GROUP BY PROCESS,[FLOOR],STATION ) T WHERE T.[FLOOR] NOT LIKE 'F%' ORDER BY STATION ");//V1.01 Add ,[FLOOR]

            DataTable dt2 = this.dbConn.GetDataSetBySql(sql2.ToString()).Tables[0];

            //sql1.Append(" SELECT DISTINCT CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE CASE WHEN B.PROCID = 'MODULE' THEN 'M4' ELSE B.PROCID END END AS PROCESS,RIGHT(A.SUBLINEID,4) AS EQID, "); //V1.01 Mark
            sql1.Append(" SELECT DISTINCT CASE WHEN B.SHOPCD = 'CELL2' THEN B.WORKNAME ELSE LEFT(RIGHT(AA.sublineid,LEN(AA.sublineid)-3),CHARINDEX('-',RIGHT(AA.sublineid,LEN(AA.sublineid)-3))-1) END AS PROCESS,CASE WHEN B.SHOPCD = 'CELL2' THEN '' ELSE SUBSTRING(AA.SUBLINEID,1,2) END AS [FLOOR],RIGHT(A.SUBLINEID,4) AS EQID, ");//V1.01 Add
            sql1.Append(" '[LineID:'+A.SUBLINEID+'][Time:'+SUBSTRING(CONVERT(VARCHAR(8),TRANSDATE,108),1,5) +'][ProdID:'+PRODID+'][Status Desc:'+EQCDDESC+'][UserName:'+E.USERNAME+']['+A.CMT+']' AS CMT,D.EQCCDCOLOR,A.SUBLINEID ");
            sql1.Append(" FROM New_ESLS A ");
            sql1.Append(" INNER JOIN MSLM AA ON(A.FABID=AA.FABID AND A.SUBLINEID=AA.SUBLINEID AND AA.FLG = '0') ");
            sql1.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
            sql1.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD) ");
            sql1.Append(" LEFT JOIN MECCU D ON(A.EQCCD = D.EQCCD) ");
            sql1.Append(" LEFT JOIN MEMM E ON(A.FABID = B.FABID AND A.USERID = E.USERID) ");
            sql1.Append(" WHERE A.FABID = '" + strFabid + "' ");
            //sql1.Append(" ORDER BY B.PROCID,A.WORKCTR,A.SUBLINEID,B.WORKNAME ");

            DataTable dt = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //------恶DataTable砞﹚Α-------------------------------------------------------
                DataTable dtm1 = new DataTable();

                dtm1.Columns.Add(new DataColumn("Process", typeof(string)));
                dtm1.Columns.Add(new DataColumn("Floor", typeof(string)));//V1.01 Add
                dtm1.Columns.Add(new DataColumn("M1", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M2", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M3", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M4", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M5", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M6", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M7", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M8", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M9", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M10", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M11", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M12", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M13", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M14", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M15", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M16", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M17", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M18", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M19", typeof(string)));
                dtm1.Columns.Add(new DataColumn("M20", typeof(string)));


                /*Color*/
                dtm1.Columns.Add(new DataColumn("C1", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C2", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C3", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C4", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C5", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C6", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C7", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C8", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C9", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C10", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C11", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C12", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C13", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C14", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C15", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C16", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C17", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C18", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C19", typeof(string)));
                dtm1.Columns.Add(new DataColumn("C20", typeof(string)));

                /*CMT*/
                dtm1.Columns.Add(new DataColumn("CMT1", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT2", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT3", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT4", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT5", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT6", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT7", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT8", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT9", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT10", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT11", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT12", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT13", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT14", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT15", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT16", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT17", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT18", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT19", typeof(string)));
                dtm1.Columns.Add(new DataColumn("CMT20", typeof(string)));

                dtm1.Columns.Add(new DataColumn("TDown", typeof(string)));
                string[] strProcessArr ={ "FINISH", "AGING ST303", "BLU" };//V1.01 Add 排除掉的Process

                for (int p = 0; p < Convert.ToInt32(dt2.Rows.Count); p++)
                {
                    //V1.01 Add Start
                    bool isExcepted = false;
                    for (int i = 0; i < strProcessArr.Length; i++)
                    {
                        isExcepted = false;
                        if (dt2.Rows[p][0].ToString() == strProcessArr[i])
                        {
                            isExcepted = true;
                            break;
                        }
                    }
                    if (isExcepted)
                    {
                        continue;
                    }
                    DataRow[] dr1 = dt.Select("(Process='" + dt2.Rows[p][0].ToString() + "' AND [Floor]='" + dt2.Rows[p][1].ToString() + "')", "SUBLINEID");
                    //V1.01 Add End
                    /*M1 Add Data*/
                    for (int k = 0; k <= Convert.ToInt32(Convert.ToInt32(dr1.Length) / 20); k++)
                    {
                        DataRow drm1 = dtm1.NewRow();
                        drm1.Table.Columns.Add();
                        drm1["Process"] = dt2.Rows[p][0].ToString();
                        drm1["Floor"] = dt2.Rows[p][1].ToString();//V1.01 Add
                        for (int j = 1; j <= 20; j++)
                        {
                            if (Convert.ToInt32(dr1.Length) >= k * 20 + j)
                            {
                                drm1["M" + j + ""] = dr1[k * 20 + j - 1].ItemArray[2].ToString().ToString();//V1.01 Modify 1-->2
                                //drm1["M"+j+""]=k*10+j;
                                drm1["C" + j + ""] = dr1[k * 20 + j - 1].ItemArray[4].ToString();//V1.01 Modify 3-->4
                                drm1["CMT" + j + ""] = dr1[k * 20 + j - 1].ItemArray[3].ToString();//V1.01 Modify 2-->3
                            }
                            else
                            {
                                drm1["M" + j + ""] = " ";
                                drm1["C" + j + ""] = " ";
                                drm1["CMT" + j + ""] = " ";
                            }
                        }
                        drm1["TDown"] = dt2.Rows[p][2].ToString() + "/" + dt2.Rows[p][3].ToString();//V1.01 Modify 1-->2,2-->3
                        dtm1.Rows.Add(drm1);
                    }
                }
                //V1.01 Add Start  調整Process的顯示順序

                DataTable dtsource = dtm1.Clone();
                bool isContinue = true;
                while (isContinue)
                {
                    if (dtm1.Rows[0][1].ToString() == "")
                    {
                        dtsource.ImportRow(dtm1.Rows[0]);
                        dtm1.Rows.RemoveAt(0);
                    }
                    else
                    {
                        if (arrOrder.Count > 0)
                        {
                            DataRow[] drtemp = dtm1.Select("[Process]='" + arrOrder[0] + "'");
                            for (int i = 0; i < drtemp.Length; i++)
                            {
                                dtsource.ImportRow(drtemp[i]);
                                dtm1.Rows.Remove(drtemp[i]);
                            }
                            arrOrder.RemoveAt(0);
                        }
                    }

                    if (dtm1.Rows.Count == 0 || arrOrder.Count == 0)
                    {
                        isContinue = false;
                    }
                }
                arrOrder = new ArrayList(strProcessOrder);
                dtm1 = dtsource.Copy();
                //V1.01 Add End

                this.GridView1.DataSource = dtm1;
                this.GridView1.DataBind();
                this.GridView1.Height = 20;
            }
            StringBuilder sql3 = new StringBuilder();
            sql3.Append(" select eqccdesc as status,eqccdcolor as col from MECCU order by eqccdesc ");
            DataTable dt3 = this.dbConn.GetDataSetBySql(sql3.ToString()).Tables[0];
            this.GridView3.DataSource = dt3;
            this.GridView3.DataBind();
            this.GridView3.Height = 20;

            this.lbUpdateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                int colnum = 1;
                int colnum46 = 1;
                int t;
                for (t = i + 1; t < GridView1.Rows.Count; t++)
                {
                    if (GridView1.Rows[i].Cells[0].Text == GridView1.Rows[t].Cells[0].Text)
                    {
                        colnum++;
                        GridView1.Rows[i].Cells[0].RowSpan = colnum;
                        GridView1.Rows[t].Cells[0].Visible = false;
                        //GridView1.Rows[i].Cells[31].RowSpan = colnum;
                        //GridView1.Rows[t].Cells[31].Visible = false;
                    }
                    else
                    {
                        break;
                    }
                    if (GridView1.Rows[i].Cells[61].Text == GridView1.Rows[t].Cells[61].Text)
                    {
                        colnum46++;
                        GridView1.Rows[i].Cells[61].RowSpan = colnum46;
                        GridView1.Rows[t].Cells[61].Visible = false;
                        //GridView1.Rows[i].Cells[31].RowSpan = colnum;
                        //GridView1.Rows[t].Cells[31].Visible = false;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int j = 1; j < 20; j++)//V1.01 Modify j=1
                {
                    if (this.GridView1.Rows[i].Cells[21 + j].Text != null && this.GridView1.Rows[i].Cells[21 + j].Text.Trim() != "")
                    {
                        this.GridView1.Rows[i].Cells[j + 1].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView1.Rows[i].Cells[21 + j].Text.ToString()));
                    }
                    if (this.GridView1.Rows[i].Cells[21 + 20 + j].Text != null)
                    {
                        this.GridView1.Rows[i].Cells[j + 1].ToolTip = this.GridView1.Rows[i].Cells[21 + 20 + j].Text;
                    }

                    if (this.GridView1.Rows[i].Cells[1 + j].Text.Trim() != "")
                    {
                        //V1.01 Add Start
                        if (!arrOrder.Contains(GridView1.Rows[i].Cells[0].Text.Trim()))
                        {
                            GridView1.Rows[i].Cells[j + 1].Attributes.Add("ondblclick", "window.open('../ems/EMSQuery.aspx?id=2&FabID=" + strFabid + "&Group=" + this.GridView1.Rows[i].Cells[0].Text + "&EQID=" + this.GridView1.Rows[i].Cells[j + 1].Text + "&FrDateTime=" + strFrTime + "&ToDateTime=" + strToTime + "')");//原語句

                        }
                        else
                        {
                            string strProcess = GridView1.Rows[i].Cells[0].Text.Trim() == "ASSY" || GridView1.Rows[i].Cells[0].Text.Trim() == "OVEN" ? "M3" : "M2";
                            GridView1.Rows[i].Cells[j + 1].Attributes.Add("ondblclick", "window.open('../ems/EMSQuery.aspx?id=3&FabID=" + strFabid + "&Group=" + strProcess + "&EQID=" + this.GridView1.Rows[i].Cells[1].Text + "-" + this.GridView1.Rows[i].Cells[0].Text + this.GridView1.Rows[i].Cells[j + 1].Text + "&FrDateTime=" + strFrTime + "&ToDateTime=" + strToTime + "')");
                        }
                        //V1.01 Add End
                    }

                    if (this.GridView1.Rows[i].Cells[0].Text != "Process")
                    {
                        //V1.01 Add Start
                        if (!arrOrder.Contains(GridView1.Rows[i].Cells[0].Text.Trim()))
                        {
                            GridView1.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('../ems/EMSQuery.aspx?id=1&FabID=" + strFabid + "&Group=" + this.GridView1.Rows[i].Cells[0].Text + "&EQID=&FrDateTime=" + strFrTime + "&ToDateTime=" + strToTime + "')");//原語句

                        }
                        else
                        {
                            string strProcess = GridView1.Rows[i].Cells[0].Text.Trim() == "ASSY" || GridView1.Rows[i].Cells[0].Text.Trim() == "OVEN" ? "M3" : "M2";
                            GridView1.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('../ems/EMSQuery.aspx?id=4&FabID=" + strFabid + "&Group=" + strProcess + "&EQID=" + this.GridView1.Rows[i].Cells[0].Text + "&FrDateTime=" + strFrTime + "&ToDateTime=" + strToTime + "')");
                        }
                        //V1.01 Add End
                    }
                }
            }

        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < this.GridView3.Rows.Count; i++)
            {
                this.GridView3.Rows[i].Cells[1].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView3.Rows[i].Cells[1].Text.ToString()));
                this.GridView3.Rows[i].Cells[1].ForeColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView3.Rows[i].Cells[1].Text.ToString()));
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                StatusQuery();
            }
            catch (Exception ex)
            {
                this.lbMsg.Text = ex.ToString();
            }
        }
    }
}
