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
    public partial class ZJStatusQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public string strFabid = "B";

        protected void Page_Load(object sender, EventArgs e)
        {
            StatusQuery();
            if (!this.IsPostBack)
            {

            }
        }
        private void StatusQuery()
        {

            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();

            sql2.Append(" SELECT SUM(CASE WHEN STATUS = 'RUN' THEN 1 ELSE 0 END) AS RCOUNT,COUNT(1) AS TCOUNT  FROM WZJM ");
            sql2.Append(" WHERE FABID = '" + strFabid + "'");
            DataTable dt2 = this.dbConn.GetDataSetBySql(sql2.ToString()).Tables[0];


            sql1.Append(" SELECT 'LOT1' AS PROCESS ,EQID,'['+CONVERT(VARCHAR(5),STARTDATE,108) + '] [' + PRODID + '] [' + isnull(C.EQCDDESC,'') + '] [' + isnull(D.USERNAME,'') + '] [' + isnull(A.CMT,'')+ ']' AS CMT,isnull(B.EQCCDCOLOR,16777215) as EQCCDCOLOR ");
            sql1.Append(" FROM WZJM A ");
            sql1.Append(" LEFT JOIN MECCU B ON(A.STATUS = B.EQCCDESC) ");
            sql1.Append(" LEFT JOIN MECM  C ON(A.FABID=C.FABID AND A.SUBEQCCD=C.EQCD) ");
            sql1.Append(" LEFT JOIN MEMM  D ON(A.FABID=D.FABID AND A.USERID=D.USERID) ");
            sql1.Append(" WHERE A.FABID = '" + strFabid + "'");
            sql1.Append(" ORDER BY EQID ");

            DataTable dt = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //------恶DataTable砞﹚Α-------------------------------------------------------
                DataTable dtm1 = new DataTable();

                dtm1.Columns.Add(new DataColumn("Process", typeof(string)));
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


                dtm1.Columns.Add(new DataColumn("TDown", typeof(string)));


                DataRow[] dr1 = dt.Select("(Process='Lot1')");

                /*M1 Add Data*/
                for (int k = 0; k <= Convert.ToInt32(Convert.ToInt32(dr1.Length) / 15); k++)
                {
                    DataRow drm1 = dtm1.NewRow();
                    drm1.Table.Columns.Add();
                    drm1["Process"] = "Lot1";

                    for (int j = 1; j <= 15; j++)
                    {
                        if (Convert.ToInt32(dr1.Length) >= k * 15 + j)
                        {
                            drm1["M" + j + ""] = dr1[k * 15 + j - 1].ItemArray[1].ToString().ToString();
                            //drm1["M"+j+""]=k*10+j;
                            drm1["C" + j + ""] = dr1[k * 15 + j - 1].ItemArray[3].ToString();
                            drm1["CMT" + j + ""] = dr1[k * 15 + j - 1].ItemArray[2].ToString();
                        }
                        else
                        {
                            drm1["M" + j + ""] = " ";
                            drm1["C" + j + ""] = " ";
                            drm1["CMT" + j + ""] = " ";
                        }
                    }
                    drm1["TDown"] = dt2.Rows[0][0].ToString() + "/" + dt2.Rows[0][1].ToString();
                    dtm1.Rows.Add(drm1);
                }
                this.GridView1.DataSource = dtm1;
                this.GridView1.DataBind();
                this.GridView1.Height = 20;
            }
            /*Summary Data*/
            StringBuilder sql4 = new StringBuilder();
            sql4.Append(" SELECT PRODID,EQID,isnull(EQCCDCOLOR,16777215) as EQCCDCOLOR  FROM WZJM A LEFT JOIN MECCU B ON(A.STATUS = B.EQCCDESC) WHERE FABID = '" + strFabid + "' order by prodid ");
            DataTable dt4 = this.dbConn.GetDataSetBySql(sql4.ToString()).Tables[0];
            this.GridView2.DataSource = dt4;
            this.GridView2.DataBind();
            this.GridView2.Height = 20;

            StringBuilder sql3 = new StringBuilder();
            sql3.Append(" select eqccdesc as status,eqccdcolor as col from MECCU order by eqccdesc ");
            DataTable dt3 = this.dbConn.GetDataSetBySql(sql3.ToString()).Tables[0];
            this.GridView3.DataSource = dt3;
            this.GridView3.DataBind();
            this.GridView3.Height = 20;
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
                    if (GridView1.Rows[i].Cells[46].Text == GridView1.Rows[t].Cells[46].Text)
                    {
                        colnum46++;
                        GridView1.Rows[i].Cells[46].RowSpan = colnum46;
                        GridView1.Rows[t].Cells[46].Visible = false;
                        //GridView1.Rows[i].Cells[31].RowSpan = colnum;
                        //GridView1.Rows[t].Cells[31].Visible = false;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int j = 0; j < 15; j++)
                {
                    if (this.GridView1.Rows[i].Cells[16 + j].Text != null && this.GridView1.Rows[i].Cells[16 + j].Text.Trim() != "")
                    {
                        this.GridView1.Rows[i].Cells[j + 1].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView1.Rows[i].Cells[16 + j].Text.ToString()));
                    }
                    if (this.GridView1.Rows[i].Cells[16 + 15 + j].Text != null)
                    {
                        this.GridView1.Rows[i].Cells[j + 1].ToolTip = this.GridView1.Rows[i].Cells[16 + 15 + j].Text;
                    }

                    if (this.GridView1.Rows[i].Cells[1 + j].Text.Trim() != "")
                    {
                        GridView1.Rows[i].Cells[j + 1].Attributes.Add("ondblclick", "window.open('../ems/ZJLot1Query.aspx?id=2&FabID=" + strFabid + "&EQID=" + this.GridView1.Rows[i].Cells[j + 1].Text + "')");
                    }

                    if (this.GridView1.Rows[i].Cells[0].Text == "Lot1")
                    {
                        GridView1.Rows[i].Cells[0].Attributes.Add("ondblclick", "window.open('../ems/ZJLot1Query.aspx?id=1&FabID=" + strFabid + "&EQID=Lot1')");
                    }
                }
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            for (int i = 0; i < this.GridView2.Rows.Count; i++)
            {
                int colnum = 1;
                int t;
                for (t = i + 1; t < GridView2.Rows.Count; t++)
                {
                    if (GridView2.Rows[i].Cells[0].Text == GridView2.Rows[t].Cells[0].Text)
                    {
                        colnum++;
                        GridView2.Rows[i].Cells[0].RowSpan = colnum;
                        GridView2.Rows[t].Cells[0].Visible = false;
                        //GridView2.Rows[i].Cells[31].RowSpan = colnum;
                        //GridView2.Rows[t].Cells[31].Visible = false;
                    }
                    else
                    {
                        break;
                    }
                }
                if (this.GridView2.Rows[i].Cells[2].Text != null && this.GridView2.Rows[i].Cells[2].Text.Trim() != "")
                {
                    this.GridView2.Rows[i].Cells[1].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView2.Rows[i].Cells[2].Text.ToString()));
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
    }
}
