using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Report.Library;
using System.IO;
using System.Text;
using ExcelIO;

namespace Report.Reports.IE
{
    public partial class CycleTime2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (txtStartDT.Text == "" || txtEndDT.Text == "")
            {
                this.Response.Write("<script>alert('時間選擇為空')</script>");
                return;
            }
            else if (Convert.ToDateTime(txtStartDT.Text) > Convert.ToDateTime(txtEndDT.Text))
            {
                Response.Write("<script>alert('选择开始时间大于结束时间，请重新选择')</script>");
                return;
            }
            try
            {
                string sql;
                if (DropDownReport.Text == "Report1")
                {

                    switch (DropDownPro.Text)
                    {
                        case "Module":
                                if (DropDownList_Group.Text == "分型號、工位、工位")
                                {
                                    //sql = "SELECT MVOUTIME,PRODID,WOTYPE,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME, MVINWORKCTR AS CWORKCTR, SUM(QTY) AS QTY FROM CYCLETIME2 AS c ";
                                    //sql += "WHERE FLG = '5' AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' GROUP BY PRODID,WOTYPE ,MVINWORKCTR ORDER BY PRODID";
                                    sql = "SELECT date,PRODID,WOTYPE,CONVERT(decimal(18,8),SUM(TIMEDIFF)) AS SUMTIME,CONVERT(decimal(18,8),SUM(TIMEDIFF)/SUM(QTY)) ";
                                    sql += "AS AVGTIME, MVINWORKCTR AS CWORKCTR, SUM(QTY) AS QTY FROM ";
                                    sql += "( ";
                                    sql += "SELECT CONVERT(varchar(100), MVOUTIME, 23) AS date, prodid, wotype, timediff, qty, mvinworkctr FROM CYCLETIME2 ";
                                    sql += "WHERE FLG = '5' AND MVOUTIME <= '" + txtEndDT.Text + "00:00' AND MVOUTIME >= '" + txtStartDT.Text + "00:00' "; 
                                    sql += ")C ";
                                    sql += "GROUP BY PRODID,WOTYPE,date,MVINWORKCTR  ORDER BY PRODID";
                                    BindDate(sql);
                                    break;
                                }
                                else
                                {
                                    //sql = "SELECT MVOUTIME,PRODID,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME, SUM(QTY) AS QTY FROM CYCLETIME2 AS c ";
                                    //sql += "WHERE FLG = '5' AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' GROUP BY PRODID ORDER BY PRODID";
                                    sql = "SELECT DATE,MVINWORKCTR,CONVERT(decimal(18,8),SUM(TIMEDIFF)) AS SUMTIME,CONVERT(decimal(18,8),SUM(TIMEDIFF)/SUM(QTY)) AS AVGTIME, SUM(QTY) AS QTY FROM ";
                                    sql += "( ";
                                    sql += "SELECT CONVERT(varchar(100), MVOUTIME, 23) AS date, timediff, MVINWORKCTR, qty FROM CYCLETIME2 ";
                                    sql += "WHERE FLG = '5' AND MVOUTIME <= '" + txtEndDT.Text + "00:00' AND MVOUTIME >= '" + txtStartDT.Text + "00:00' ";
                                    sql += ")A "; 
                                    sql += "GROUP BY MVINWORKCTR,date ORDER BY A.MVINWORKCTR ";
                                    BindDate(sql);
                                    break;
                                }
                            
                        case "Beol":
                            if (DropDownList_Group.Text == "分型號、工位、工位")
                            {
                                sql = "SELECT date,PRODID,WOTYPE,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME, MVINWORKCTR AS CWORKCTR, SUM(QTY) AS QTY FROM ";
                                sql += "( ";
                                sql += " SELECT CONVERT(varchar(100), MVOUTIME, 23) AS date, prodid, wotype, timediff, qty, mvinworkctr FROM CYCLETIME2 ";
                                sql += " WHERE FLG IN ('2','6','7') AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' "; 
                                sql += " )c ";
                                sql += " GROUP BY PRODID,WOTYPE ,date,MVINWORKCTR ORDER BY PRODID ";
                                BindDate(sql);
                                break;
                            }
                            else
                            {
                                sql = "SELECT date,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME, SUM(QTY) AS QTY FROM ";
                                sql += "(";
                                sql += "SELECT CONVERT(varchar(100), MVOUTIME, 23) AS date, timediff, MVINWORKCTR, qty  FROM CYCLETIME2 AS c ";
                                sql += "WHERE FLG IN ('2','6','7') AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' ";
                                sql += ")c ";
                                sql += "GROUP BY MVINWORKCTR,date ORDER BY MVINWORKCTR";
                                BindDate(sql);
                                break;
                            
                            }
                        default:
                            return;
                    }

                }

                if (DropDownReport.Text == "Report2")
                {

                    switch (DropDownPro.Text)
                    {
                        case "Module":
                            sql = "SELECT PRODID,WOTYPE,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME,SUM(QTY) AS QTY FROM CYCLETIME2 AS c ";
                            sql += "WHERE FLG='4' AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' GROUP BY PRODID,WOTYPE ORDER BY PRODID";
                            BindDate(sql);
                            break;
                        case "Beol3":
                            sql = "SELECT PRODID,WOTYPE,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME,SUM(QTY) AS QTY FROM CYCLETIME2 AS c ";
                            sql += "WHERE FLG='3' AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' GROUP BY PRODID,WOTYPE ORDER BY PRODID";
                            BindDate(sql);
                            break;
                        case "Beol2":
                            sql = "SELECT PRODID,WOTYPE,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME,SUM(QTY) AS QTY FROM CYCLETIME2 AS c ";
                            sql += "WHERE FLG='2' AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' GROUP BY PRODID,WOTYPE ORDER BY PRODID";
                            BindDate(sql);
                            break;
                        case "Beol1":
                            sql = "SELECT PRODID,WOTYPE,SUM(TIMEDIFF) AS SUMTIME,SUM(TIMEDIFF)/SUM(QTY) AS AVGTIME,SUM(QTY) AS QTY FROM CYCLETIME2 AS c ";
                            sql += "WHERE FLG='1' AND MVOUTIME < '" + txtEndDT.Text + "00:00' AND MVOUTIME > '" + txtStartDT.Text + "00:00' GROUP BY PRODID,WOTYPE ORDER BY PRODID";
                            BindDate(sql);
                            break;
                        default:
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("<script language=javascript>window.alert('" + ex.Message + "');</script>");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDT.Text = "";
            txtEndDT.Text = "";
            GridView1.DataSource = null;
            GridView1.Visible = false;
        }
        protected void BindDate(string strsql)
        {
            DataTable dt = MSSQLHelper.ExecuteDataTable(strsql);
            if (dt.Rows.Count > 0)
            {
                GridView1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                this.Response.Write("<script>alert('查無資料！')</script>");
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count == 0||GridView1.Visible==false)
            {
                this.Response.Write("<script>alert('無數據可導出！')</script>");
            }
            else
            {
                DataTable table = new DataTable();
                table = ExcelIO.ExcelHelper.GridView2DataTable(GridView1);
                string filename = "" + DropDownPro.Text + "段查詢結果.xls";
                ExcelIO.ExcelHelper.ExportByWeb(table, "", filename);
            }
        }


        protected void DropDownReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownReport.Text == "Report2")
            {
                string strsql1 = "SELECT 'Module' as a union select 'Beol1' as a union select 'Beol2' as a union select 'Beol3'as a";
                DataTable dt1 = MSSQLHelper.ExecuteDataTable(strsql1);
                DropDownPro.DataSource = dt1;
                DropDownPro.DataTextField = "a";
                DropDownPro.DataBind();
                Label6.Visible = false;
                DropDownList_Group.Visible = false;
            }
            else
            {
                string strsql1 = "SELECT 'Module' as a union select 'Beol' as a";
                DataTable dt1 = MSSQLHelper.ExecuteDataTable(strsql1);
                DropDownPro.DataSource = dt1;
                DropDownPro.DataTextField = "a";
                DropDownPro.DataBind();
                Label6.Visible = true;
                DropDownList_Group.Visible = true;

            }
            
        }

        protected void DropDownList_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DropDownReport.Text == "Report2")
            //{
            //    string strsql1 = "SELECT 'Module' as a union select 'Beol1' as a union select 'Beol2' as a union select 'Beol3'as a";
            //    DataTable dt1 = MSSQLHelper.ExecuteDataTable(strsql1);
            //    DropDownPro.DataSource = dt1;
            //    DropDownPro.DataTextField = "a";
            //    DropDownPro.DataBind();
            //}
            //else
            //{
            //    string strsql1 = "SELECT 'Module' as a union select 'Beol' as a";
            //    DataTable dt1 = MSSQLHelper.ExecuteDataTable(strsql1);
            //    DropDownPro.DataSource = dt1;
            //    DropDownPro.DataTextField = "a";
            //    DropDownPro.DataBind();
            //}

        }

        protected void DropDownReport_TextChanged(object sender, EventArgs e)
        {

        }
    }
}