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

namespace Report.Reports.EMS
{
    public partial class ZJLot1Query : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public string strID = "";
        public string strFabID = "";
        public string strEQID = "";
        public string strFrTime = "";
        public string strToTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT CONVERT(VARCHAR(20),getdate(),120),CONVERT(VARCHAR(20),getdate()-1,120) ");
            DataTable DT = this.dbConn.GetDataSetBySql(sql.ToString()).Tables[0];

            strFrTime = DT.Rows[0][1].ToString();
            strToTime = DT.Rows[0][0].ToString();

            strID = this.Request.QueryString["id"].ToString();
            strFabID = this.Request.QueryString["FabID"].ToString();
            strEQID = this.Request.QueryString["EQID"].ToString();

            QueryData();
        }

        private void QueryData()
        {

            StringBuilder sql = new StringBuilder();
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            StringBuilder strsql = new StringBuilder();

            strsql.Append(" SELECT DISTINCT EQID");
            strsql.Append(" FROM WZJX A ");
            strsql.Append(" WHERE A.FABID = '" + strFabID + "' ");
            if (strID == "2")
            {
                strsql.Append(" AND A.EQID =  '" + strEQID + "'  ");
            }
            strsql.Append(" AND  CASE WHEN ENDDATE IS NULL THEN GETDATE() ELSE ENDDATE END >=  '" + strFrTime + "' ");
            strsql.Append(" AND STARTDATE <=  '" + strToTime + "' ");
            strsql.Append(" ORDER BY EQID ");
            DataTable strDT = this.dbConn.GetDataSetBySql(strsql.ToString()).Tables[0];

            sql.Append(" SELECT  EQID,STATUS,B.EQCCDCOLOR, ");
            sql.Append(" DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + strFrTime + "' THEN  '" + strFrTime + "' ELSE STARTDATE END,ISNULL(ENDDATE, '" + strToTime + "'  ))  AS DMINUTE,  ");
            sql.Append(" '['+CONVERT(VARCHAR(5),STARTDATE,108) + '][' + PRODID + '][' + C.EQCDDESC + '][' + D.USERNAME + ']' + A.CMT AS CMT,STARTDATE ");
            sql.Append(" FROM WZJX A ");
            sql.Append(" LEFT JOIN MECCU B ON(A.STATUS = B.EQCCDESC) ");
            sql.Append(" LEFT JOIN MECM  C ON(A.FABID=C.FABID AND A.SUBEQCCD=C.EQCD AND A.STATUS = C.EQCCDESC) ");
            sql.Append(" LEFT JOIN MEMM  D ON(A.FABID=D.FABID AND A.USERID=D.USERID) ");
            sql.Append(" WHERE A.FABID = '" + strFabID + "' ");
            if (strID == "2")
            {
                sql.Append(" AND A.EQID =  '" + strEQID + "'  ");
            }
            sql.Append(" AND  CASE WHEN ENDDATE IS NULL THEN GETDATE() ELSE ENDDATE END >=  '" + strFrTime + "' ");
            sql.Append(" AND STARTDATE <=  '" + strToTime + "' ");
            sql.Append(" ORDER BY EQID,STARTDATE ");
            DataTable DT = this.dbConn.GetDataSetBySql(sql.ToString()).Tables[0];

            //sql1.Append(" SELECT  EQID,STATUS,B.EQCCDCOLOR, ");
            //sql1.Append(" SUM(DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + strFrTime + "' THEN  '" + strFrTime + "' ELSE STARTDATE END,ISNULL(ENDDATE, '" + strToTime + "'  ))) AS DMINUTE, ");
            //sql1.Append(" round(SUM(DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + strFrTime + "' THEN  '" + strFrTime + "' ELSE STARTDATE END,ISNULL(ENDDATE, '" + strToTime + "'  ))),2) AS DHours ");
            //sql1.Append(" FROM WZJX A ");
            //sql1.Append(" LEFT JOIN MECCU B ON(A.STATUS = B.EQCCDESC) ");
            //sql1.Append(" WHERE A.FABID = '" + strFabID + "' ");
            //if (strID == "2")
            //{
            //    sql1.Append(" AND A.EQID =  '" + strEQID + "'  ");
            //}
            //sql1.Append(" AND  CASE WHEN ENDDATE IS NULL THEN GETDATE() ELSE ENDDATE END >=  '" + strFrTime + "' ");
            //sql1.Append(" AND STARTDATE <=  '" + strToTime + "' ");
            //sql1.Append(" GROUP BY EQID,STATUS,B.EQCCDCOLOR ORDER BY EQID ");

            //DataTable DT1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];

            sql2.Append(" SELECT  SHIFTID,EQID,PRODID,STATUS,EQCDDESC,CONVERT(VARCHAR(20),STARTDATE,120) AS STARTDATE,ISNULL(CONVERT(VARCHAR(20),ENDDATE,120),'') AS ENDDATE, ");
            sql2.Append(" DATEDIFF(MINUTE,CASE WHEN STARTDATE <=  '" + strFrTime + "' THEN  '" + strFrTime + "' ELSE STARTDATE END,ISNULL(ENDDATE,  '" + strToTime + "' )) AS DMINUTE,USERID,CMT  ");
            sql2.Append(" FROM WZJX A  LEFT JOIN MECCU B ON(A.STATUS = B.EQCCDESC)    ");
            sql2.Append(" LEFT JOIN MECM  C ON(A.FABID=C.FABID AND A.SUBEQCCD=C.EQCD)    ");
            sql2.Append(" WHERE A.FABID =  '" + strFabID + "'   ");
            if (strID == "2")
            {
                sql2.Append(" AND A.EQID =  '" + strEQID + "'  ");
            }
            sql2.Append(" AND  CASE WHEN ENDDATE IS NULL THEN GETDATE() ELSE ENDDATE END >=   '" + strFrTime + "'   ");
            sql2.Append(" AND STARTDATE <=  '" + strToTime + "'   ");
            sql2.Append(" ORDER BY EQID,STARTDATE  ");
            DataTable DT2 = this.dbConn.GetDataSetBySql(sql2.ToString()).Tables[0];

            if (strDT.Rows.Count > 0)
            {
                this.GridView1.DataSource = strDT;
                this.GridView1.DataBind();
                this.GridView1.Height = 20;

                this.GridView2.DataSource = strDT;
                this.GridView2.DataBind();
                this.GridView2.Height = 20;

                this.GridView3.DataSource = DT2;
                this.GridView3.DataBind();
                this.GridView3.Height = 20;

                /*24Status*/
                for (int i = 0; i < strDT.Rows.Count; i++)
                {
                    int intCount = 1;
                    int intMod = 0;
                    int dblMod = 0;

                    //DataRow[] dr00 = DT.Select("(eqid='" + GridView1.Rows[i].Cells[0].ToString() + "')", "eqid,startdate ");
                    DataRow[] dr00 = DT.Select("(eqid='" + strDT.Rows[i][0].ToString() + "')", "eqid,startdate");
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
                //this.GridView1.Rows[0].Cells[1].Text = Convert.ToDateTime(strFrTime).AddHours(0).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(strFrTime).AddHours(2).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(strFrTime).AddHours(4).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(strFrTime).AddHours(6).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(strFrTime).AddHours(8).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(strFrTime).AddHours(10).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(strFrTime).AddHours(12).ToString("HH:mm");

                ///*Status %*/
                //for (int i = 0; i < strDT.Rows.Count; i++)
                //{
                //    int intCount = 1;
                //    int intMod = 0;
                //    int dblMod = 0;

                //    //DataRow[] dr00 = DT.Select("(eqid='" + GridView1.Rows[i].Cells[0].ToString() + "')", "eqid,startdate ");
                //    DataRow[] dr00 = DT1.Select("(eqid='" + strDT.Rows[i][0].ToString() + "')", "eqid");
                //    for (int j = 0; j < Convert.ToInt32(dr00.Length); j++)
                //    {
                //        intMod = 0;
                //        dblMod = dblMod + Convert.ToInt32(dr00[j].ItemArray[3].ToString()) % 5;

                //        if (dblMod >= 5)
                //        {
                //            intMod = 1;
                //            dblMod = dblMod - 5;
                //        }
                //        for (int k = 0; k < Convert.ToInt32(dr00[j].ItemArray[3].ToString()) / 5 + intMod; k++)
                //        {
                //            this.GridView2.Rows[i].Cells[intCount].ToolTip = dr00[j].ItemArray[4].ToString()+"(Mintue)";
                //            this.GridView2.Rows[i].Cells[intCount].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(dr00[j].ItemArray[2].ToString()));
                //            intCount = intCount + 1;
                //        }
                //    }
                //}

                StringBuilder sql3 = new StringBuilder();
                sql3.Append(" select eqccdesc as status,eqccdcolor as col from MECCU order by eqccdesc ");
                DataTable dt3 = this.dbConn.GetDataSetBySql(sql3.ToString()).Tables[0];
                this.GridView4.DataSource = dt3;
                this.GridView4.DataBind();
                this.GridView4.Height = 20;
            }
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
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
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
    }
}
