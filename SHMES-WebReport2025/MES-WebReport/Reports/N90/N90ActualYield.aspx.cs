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
using Microsoft.Reporting.WebForms;
using System.Text;
using Report.Library;
using Report.Sources;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Report.Reports.N90
{
    public partial class N90ActualYield : System.Web.UI.Page
    {
        SPCDBConn dbconn = new SPCDBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDateTime();
                GridView1.ShowFooter = true;
            }

            string userinfo = ((UserInfo)Session["userinfo"]).UserID;
            StringBuilder stb = new StringBuilder();
            stb.Append(@"select * from [MUGMM] where fabid = 'B' and usergroupcd = 'RPMF' and userid='" + userinfo + "'");
            DataTable dt = new DataTable();
            dt = this.dbconn.GetDataSetBySql(stb.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                GridView1.Columns[0].Visible = false;
            }
            else
            {
                GridView1.ShowFooter = true;
                GridView1.Columns[0].Visible = true;
            }
        }

        protected void DataBound()
        {
            //當datasource為空的時候也顯示gridview的表頭


            string sql = "select Convert(varchar(10),[D_DATE], 120) AS 'D_DATE',P_QTY,A_QTY,P_YIELDRATE,A_YIELDRATE,INSERTTIME,LASTUPDATED  from PRO_MODULE";
            DataTable dt = this.dbconn.GetDataSetBySql(sql).Tables[0];
            //判斷datasource是否為空以及走哪條支線的處理邏輯.
            if (dt.Rows.Count == 0)
            {
                dt.Constraints.Clear();
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.AllowDBNull = true;
                    dt.Columns[0].AllowDBNull = true;
                    dt.Rows.Add(dt.NewRow());
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    int columnCount = GridView1.Rows[0].Cells.Count;
                    GridView1.Rows[0].Cells.Clear();
                    GridView1.Rows[0].Cells.Add(new TableCell());
                    GridView1.Rows[0].Cells[0].ColumnSpan = columnCount;
                    GridView1.Rows[0].Cells[0].Text = "";
                }

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridView1.Rows[i].Visible = false;
                }
            }

            else
            {
                string sqlquery = string.Empty;
                if (ddlDate.Text.ToString().Trim() == "All")
                {
                    sqlquery = " select Convert(varchar(10),[D_DATE], 120) AS 'D_DATE',P_QTY,A_QTY,P_YIELDRATE,A_YIELDRATE,INSERTTIME,LASTUPDATED  from PRO_MODULE where 1=1";
                }
                else if (ddlDate.Text.ToString().Trim() == "")
                {
                    sqlquery = " select Convert(varchar(10),[D_DATE], 120) AS 'D_DATE',P_QTY,A_QTY,P_YIELDRATE,A_YIELDRATE,INSERTTIME,LASTUPDATED  from PRO_MODULE where 1=1";
                }
                else
                {
                    sqlquery = " select Convert(varchar(10),[D_DATE], 120) AS 'D_DATE',P_QTY,A_QTY,P_YIELDRATE,A_YIELDRATE,INSERTTIME,LASTUPDATED  from PRO_MODULE where 1=1 and D_DATE='" + Convert.ToDateTime(ddlDate.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }

                DataTable dtquery = this.dbconn.GetDataSetBySql(sqlquery).Tables[0];

                if (dtquery.Rows.Count == 0)
                {
                    this.Response.Write("<script language=javascript>window.alert('查無資料,請確認!');</script>");
                    return;
                }
                GridView1.DataSource = dtquery;
                GridView1.DataBind();
            }
        }

        protected void SetDateTime()
        {
            string sql = string.Empty;
            sql = "select distinct Convert(varchar(10),[D_DATE], 120) AS 'D_DATE'  from PRO_MODULE";
            DataTable dt = this.dbconn.GetDataSetBySql(sql).Tables[0];

            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find D_DATE!');</script>");
            }
            else
            {
                this.ddlDate.DataSource = dt;
                this.ddlDate.DataTextField = "D_DATE";
                this.ddlDate.DataValueField = "D_DATE";
                this.ddlDate.DataBind();
                this.ddlDate.Items.Insert(0, "All");
            }

        }

        protected void butAdd_Click(object sender, EventArgs e)
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now.AddDays(-1);

            Button but1 = GridView1.FooterRow.FindControl("butAdd") as Button;
            but1.Visible = false;
            Button but2 = GridView1.FooterRow.FindControl("butYes") as Button;
            but2.Visible = true;
            Button but3 = GridView1.FooterRow.FindControl("butCancel") as Button;
            but3.Visible = true;

            TextBox txtMFGDate = GridView1.FooterRow.FindControl("TextBox8") as TextBox;
            txtMFGDate.Visible = true;
            TextBox txtPlanQty = GridView1.FooterRow.FindControl("TextBox9") as TextBox;
            txtPlanQty.Visible = true;
            TextBox txtActualQty = GridView1.FooterRow.FindControl("TextBox10") as TextBox;
            txtActualQty.Visible = true;
            TextBox txtPlanYieldRate = GridView1.FooterRow.FindControl("TextBox11") as TextBox;
            txtPlanYieldRate.Visible = true;
            TextBox txtActualYieldRate = GridView1.FooterRow.FindControl("TextBox12") as TextBox;
            txtActualYieldRate.Visible = true;

            txtPlanYieldRate.MaxLength = 6;
            txtActualYieldRate.MaxLength = 6;

            txtMFGDate.Text = currentTime.ToString().Substring(0, 10).Trim();
            //txtMFGDate.Enabled = false;
            txtPlanQty.Text = "0.00";
            txtActualQty.Text = "0.00";
            txtPlanYieldRate.Text = "100.00";
            txtActualYieldRate.Text = "100.00";

            int j = GridView1.Rows.Count;
            for (int i = 0; i < j; i++)
            {
                GridView1.Rows[i].Visible = false;
            }
        }

        protected void butYes_Click(object sender, EventArgs e)
        {
            string txtMFGDate = ((TextBox)GridView1.FooterRow.Cells[1].FindControl("TextBox8")).Text.ToString().Trim();
            string txtPlanQty = ((TextBox)GridView1.FooterRow.Cells[2].FindControl("TextBox9")).Text.ToString().Trim();
            string txtActualQty = ((TextBox)GridView1.FooterRow.Cells[3].FindControl("TextBox10")).Text.ToString().Trim();
            string txtPlanYieldRate = ((TextBox)GridView1.FooterRow.Cells[4].FindControl("TextBox11")).Text.ToString().Trim();
            string txtActualYieldRate = ((TextBox)GridView1.FooterRow.Cells[5].FindControl("TextBox12")).Text.ToString().Trim();

            string sqlcheck = string.Empty;
            sqlcheck = "select * from PRO_MODULE where D_DATE='" + txtMFGDate + "'";
            DataTable dtcheck = this.dbconn.GetDataSetBySql(sqlcheck).Tables[0];

            if (dtcheck.Rows.Count != 0)
            {
                this.Response.Write("<script language=javascript>window.alert('系統中已經有" + txtMFGDate + "天的資料!不能重複寫入哦 :-) !');</script>");
                return;
            }

            if (Convert.ToDouble(txtPlanYieldRate.Trim()) > 100.00 || Convert.ToDouble(txtPlanYieldRate.Trim()) < 0.00 || Convert.ToDouble(txtActualYieldRate.Trim()) > 100 || Convert.ToDouble(txtActualYieldRate.Trim()) < 0)
            {
                this.Response.Write("<script language=javascript>window.alert('計劃良率或實際良率錯誤,不能超過100或者小於0哦  :-) !');</script>");
                return;
            }



            string sql = string.Empty;
            sql = "insert into PRO_MODULE(D_DATE,P_QTY,A_QTY,P_YIELDRATE,A_YIELDRATE,INSERTTIME,LASTUPDATED) VALUES ";
            sql += "('" + txtMFGDate + "','" + txtPlanQty + "','" + txtActualQty + "','" + txtPlanYieldRate + "','" + txtActualYieldRate + "',getdate(),getdate())";

            this.dbconn.ExecuteSql(sql);
            GridView1.EditIndex = -1;
            DataBound();
        }

        protected void butCancel_Click(object sender, EventArgs e)
        {
            DataBound();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            DataBound();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            DataBound();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //string id = ((TextBox)GridView1.Rows[e.RowIndex].Cells[1].FindControl("TextBox8")).Text.ToString().Trim();
            string sqlupdate = string.Empty;

            string txtMFGDate = ((TextBox)GridView1.Rows[e.RowIndex].Cells[1].FindControl("TextBox1")).Text.ToString().Trim();
            string txtPlanQty = ((TextBox)GridView1.Rows[e.RowIndex].Cells[2].FindControl("TextBox2")).Text.ToString().Trim();
            string txtActualQty = ((TextBox)GridView1.Rows[e.RowIndex].Cells[3].FindControl("TextBox3")).Text.ToString().Trim();
            string txtPlanYieldRate = ((TextBox)GridView1.Rows[e.RowIndex].Cells[4].FindControl("TextBox4")).Text.ToString().Trim();
            string txtActualYieldRate = ((TextBox)GridView1.Rows[e.RowIndex].Cells[5].FindControl("TextBox5")).Text.ToString().Trim();

            if (Convert.ToDouble(txtPlanYieldRate.Trim()) > 100.00 || Convert.ToDouble(txtPlanYieldRate.Trim()) < 0.00 || Convert.ToDouble(txtActualYieldRate.Trim()) > 100 || Convert.ToDouble(txtActualYieldRate.Trim()) < 0)
            {
                this.Response.Write("<script language=javascript>window.alert('計劃良率或實際良率錯誤,不能超過100或者小於0哦  :-) !');</script>");
                return;
            }

            sqlupdate = " Update PRO_MODULE";
            sqlupdate += "  set P_QTY='" + txtPlanQty + "', A_QTY='" + txtActualQty + "',P_YIELDRATE='" + txtPlanYieldRate + "',A_YIELDRATE='" + txtActualYieldRate + "',lastupdated=getdate() ";
            sqlupdate += "  where D_DATE='" + txtMFGDate + "'";

            this.dbconn.ExecuteSql(sqlupdate);
            GridView1.EditIndex = -1;
            DataBound();
        }

        protected void butQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataBound();
        }
    }
}

