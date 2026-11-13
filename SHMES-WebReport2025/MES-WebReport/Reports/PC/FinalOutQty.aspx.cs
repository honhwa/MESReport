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
/*
 *  V1.01    2014-12-26    Inx/Runy    增加目標良率 
 */
namespace Report.Reports.PC
{
    public partial class FinalOutQty : System.Web.UI.Page
    {
        int count;
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
            }
            ddlDate.Enabled = false;
            ddlfab.Enabled = false;
            ddlSub.Enabled = false;
            ddlProdid.Enabled = false;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 20;
            GridView1.ShowFooter = true;
            string userinfo = ((UserInfo)Session["userinfo"]).UserID;
            StringBuilder stb = new StringBuilder();
            stb.Append("select * from MUGMM where fabid = 'B' and usergroupcd = 'RPMF' and userid='" + userinfo + "'");
            DataTable dt = new DataTable();
            dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                GridView1.Columns[0].Visible = false;
            }
            else
            {
                labUpload.Visible = true;
                FileUpload1.Visible = true;
                butUpload.Visible = true;
                GridView1.ShowFooter = true;
                GridView1.Columns[0].Visible = true;
            }
        }

        private void SetMFGDateDatasource()
        {
            string strsql = @"select DISTINCT  CONVERT(NVARCHAR(30),mfgdate,120) AS mfgdate from ROPPRD ORDER BY MFGDATE DESC";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find MFGDate！');</script>");
            }
            else
            {
                this.ddlDate.DataSource = dt;
                this.ddlDate.DataTextField = "mfgdate";
                this.ddlDate.DataValueField = "mfgdate";
                this.ddlDate.DataBind();
            }
        }
        private void SetSubDatasource()
        {
            string strsql;
            strsql = @"select DISTINCT sublineid from mslm WHERE WORKCTR IN('8270','8360') ORDER BY SUBLINEID ";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點所對應的線別！');</script>");
                ChkSub.Checked = false;
            }
            else
            {
                this.ddlSub.DataSource = dt;
                this.ddlSub.DataTextField = "SUBLINEID";
                this.ddlSub.DataValueField = "SUBLINEID";
                this.ddlSub.DataBind();
            }
        }
        private void SetProductDatasource()
        {
            string strsql = @"select DISTINCT PROJECT_NAME from mpim ORDER BY PROJECT_NAME ";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Status！');</script>");
            }
            else
            {
                this.ddlProdid.DataSource = dt;
                this.ddlProdid.DataTextField = "prodid";
                this.ddlProdid.DataValueField = "prodid";
                this.ddlProdid.DataBind();
            }
        }

        protected void ChkFab_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkFab.Checked)
            {
                ddlfab.Enabled = true;
            }
            else
            {
                ddlfab.Enabled = false;
            }

        }
        protected void ChkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkDate.Checked)
            {
                ddlDate.Enabled = true;
                this.SetMFGDateDatasource();
            }
            else
            {
                this.ddlDate.Items.Clear();
                ddlDate.Enabled = false;
            }
        }

        protected void ChkPro_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPro.Checked)
            {
                ddlProdid.Enabled = true;
                this.SetProductDatasource();
            }
            else
            {
                this.ddlProdid.Items.Clear();
                ddlProdid.Enabled = false;
            }
        }
        protected void ChkSub_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSub.Checked)
            {
                ddlSub.Enabled = true;
                this.SetSubDatasource();
            }
            else
            {
                this.ddlSub.Items.Clear();
                ddlSub.Enabled = false;
            }
        }

        protected void DataBound()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder stb = new StringBuilder();
            stb.Append("select CONVERT(NVARCHAR(30),DataEnd_Time,120) AS DataEnd_Time ,FabID,Shop,Year,Month,WeekNum,CONVERT(NVARCHAR(30),MFGDATE,120) AS MFGDATE,ProdLine,ProductID,Hour_8,Hour_9,Hour_10,Hour_11,Hour_12,");
            stb.Append("Hour_13,Hour_14,Hour_15,Hour_16,Hour_17,Hour_18,Hour_19,Hour_20,Hour_21,Hour_22,Hour_23,Hour_24,Hour_1,Hour_2,Hour_3,Hour_4,Hour_5,");
            stb.Append("Hour_6,Hour_7,CONVERT(NVARCHAR(30),Create_Time,120) AS Create_Time,Create_By,CONVERT(NVARCHAR(30),Modify_Time,120) AS Modify_Time,Modify_by,TargetYield from ropprd where 1=1");//V1.01 Add TargetYield
            if (this.ChkFab.Checked)
            {
                stb.Append(" and fabid='" + ddlfab.Text.ToString().Trim() + "'");
            }
            if (this.ChkDate.Checked)
            {
                stb.Append(" and mfgdate='" + (Convert.ToDateTime(ddlDate.Text.Trim())).ToString("yyyy-MM-dd") + "'");
            }
            if (ChkSub.Checked)
            {
                stb.Append(" and prodline='" + ddlSub.Text.ToString().Trim() + "'");
            }
            if (this.ChkPro.Checked)
            {
                stb.Append("and productid='" + ddlProdid.Text.ToString().Trim() + "'");
            }

            if (ChkFab.Checked == false && ChkDate.Checked == false && ChkSub.Checked == false && ChkPro.Checked == false)
            {
                stb.Append("");
            }
            stb.Append("  order by dataend_time");
            DataTable dt1 = new DataTable();
            dt1 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            GridView1.DataSource = dt1;
            GridView1.DataBind();

            if (GridView1.Rows.Count == 0)
            {
                Label1.Text = "查無資料!";
                Label1.Visible = true;
            }
            else
            {
                Label1.Visible = false;
            }
        }
        protected void ButQuery_Click(object sender, EventArgs e)
        {
            DataBound();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            DataBound();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            DataBound();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            string Data_EndTime1 = Convert.ToDateTime(((TextBox)GridView1.Rows[e.RowIndex].Cells[1].FindControl("TextBoxDateEnd_Time")).Text.ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            string FabID1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[2].FindControl("TextBoxFabID")).Text.ToString().Trim();
            string Shop1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[3].FindControl("TextBoxShop")).Text.ToString().Trim();
            string Year1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[4].FindControl("TextBoxYear")).Text.ToString().Trim();
            string Month1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[5].FindControl("TextBoxMonth")).Text.ToString().Trim();
            string WeekNum1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[6].FindControl("TextBoxWeekNum")).Text.ToString().Trim();
            string MFGdate1 = Convert.ToDateTime(((TextBox)GridView1.Rows[e.RowIndex].Cells[7].FindControl("TextBoxMFGDate")).Text.ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            string ProdLine1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[8].FindControl("TextBoxProdLine")).Text.ToString().Trim();
            string ProductID1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[9].FindControl("TextBoxProductID")).Text.ToString().Trim();
            string Hour8_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[10].FindControl("TextBox2")).Text.ToString().Trim();
            string Hour9_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[11].FindControl("TextBox3")).Text.ToString().Trim();
            string Hour10_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[12].FindControl("TextBox4")).Text.ToString().Trim();
            string Hour11_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[13].FindControl("TextBox5")).Text.ToString().Trim();
            string Hour12_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[14].FindControl("TextBox6")).Text.ToString().Trim();
            string Hour13_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[15].FindControl("TextBox7")).Text.ToString().Trim();
            string Hour14_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[16].FindControl("TextBox8")).Text.ToString().Trim();
            string Hour15_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[17].FindControl("TextBox9")).Text.ToString().Trim();
            string Hour16_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[18].FindControl("TextBox10")).Text.ToString().Trim();
            string Hour17_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[19].FindControl("TextBox11")).Text.ToString().Trim();
            string Hour18_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[20].FindControl("TextBox12")).Text.ToString().Trim();
            string Hour19_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[21].FindControl("TextBox13")).Text.ToString().Trim();
            string Hour20_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[22].FindControl("TextBox14")).Text.ToString().Trim();
            string Hour21_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[23].FindControl("TextBox15")).Text.ToString().Trim();
            string Hour22_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[24].FindControl("TextBox16")).Text.ToString().Trim();
            string Hour23_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[25].FindControl("TextBox17")).Text.ToString().Trim();
            string Hour24_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[26].FindControl("TextBox18")).Text.ToString().Trim();
            string Hour1_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[27].FindControl("TextBox19")).Text.ToString().Trim();
            string Hour2_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[28].FindControl("TextBox20")).Text.ToString().Trim();
            string Hour3_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[29].FindControl("TextBox21")).Text.ToString().Trim();
            string Hour4_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[30].FindControl("TextBox22")).Text.ToString().Trim();
            string Hour5_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[31].FindControl("TextBox23")).Text.ToString().Trim();
            string Hour6_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[32].FindControl("TextBox24")).Text.ToString().Trim();
            string Hour7_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[33].FindControl("TextBox25")).Text.ToString().Trim();
            string TargetYield_1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[34].FindControl("TextBox30")).Text.ToString().Trim();//V1.01 Add TargetYield，下面四行Cells +1
            string Create_Time1 = Convert.ToDateTime(((TextBox)GridView1.Rows[e.RowIndex].Cells[35].FindControl("TextBox26")).Text).ToString("yyyy-MM-dd HH:mm:ss");
            string Create_By1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[36].FindControl("TextBox27")).Text.ToString().Trim();
            string Modify_Time1 = Convert.ToDateTime(((TextBox)GridView1.Rows[e.RowIndex].Cells[37].FindControl("TextBox28")).Text).ToString("yyyy-MM-dd HH:mm:ss");
            string Modify_By1 = ((TextBox)GridView1.Rows[e.RowIndex].Cells[38].FindControl("TextBox29")).Text.ToString().Trim();
            string userid = ((UserInfo)Session["UserInfo"]).UserID;
            string username = ((UserInfo)Session["UserInfo"]).UserName;

            string sql = "UPDATE ROPPRD SET FabID='" + FabID1 + "',";
            sql += "Shop='" + Shop1 + "',";
            sql += "Year='" + Year1 + "',";
            sql += "Month='" + Month1 + "',";
            sql += "WeekNum='" + WeekNum1 + "',";
            sql += "MFGDate='" + MFGdate1 + "',";
            sql += "ProdLine='" + ProdLine1 + "',";
            sql += "Hour_8='" + Hour8_1 + "',";
            sql += "Hour_9='" + Hour9_1 + "',";
            sql += "Hour_10='" + Hour10_1 + "',";
            sql += "Hour_11='" + Hour11_1 + "',";
            sql += "Hour_12='" + Hour12_1 + "',";
            sql += "Hour_13='" + Hour13_1 + "',";
            sql += "Hour_14='" + Hour14_1 + "',";
            sql += "Hour_15='" + Hour15_1 + "',";
            sql += "Hour_16='" + Hour16_1 + "',";
            sql += "Hour_17='" + Hour17_1 + "',";
            sql += "Hour_18='" + Hour18_1 + "',";
            sql += "Hour_19='" + Hour19_1 + "',";
            sql += "Hour_20='" + Hour20_1 + "',";
            sql += "Hour_21='" + Hour21_1 + "',";
            sql += "Hour_22='" + Hour22_1 + "',";
            sql += "Hour_23='" + Hour23_1 + "',";
            sql += "Hour_24='" + Hour24_1 + "',";
            sql += "Hour_1='" + Hour1_1 + "',";
            sql += "Hour_2='" + Hour2_1 + "',";
            sql += "Hour_3='" + Hour3_1 + "',";
            sql += "Hour_4='" + Hour4_1 + "',";
            sql += "Hour_5='" + Hour5_1 + "',";
            sql += "Hour_6='" + Hour6_1 + "',";
            sql += "Hour_7='" + Hour7_1 + "',";
            /*V1.01 Add Start*/
            sql += "TargetYield='" + TargetYield_1 + "',";
            /*V1.01 Add End*/
            sql += "Create_Time=getdate(),";
            sql += "Create_by='" + userid + "',";
            sql += "Modify_Time=getdate(),";
            sql += "Modify_by='" + username + "'";
            sql += "where fabid='" + FabID1 + "'";
            sql += "and shop='" + Shop1 + "'";
            sql += "and year='" + Year1 + "'";
            sql += "and month='" + Month1 + "'";
            sql += "and weeknum='" + WeekNum1 + "'";
            sql += "and mfgdate='" + MFGdate1 + "'";
            sql += "and prodline='" + ProdLine1 + "'";
            sql += "and productid='" + ProductID1 + "'";

            this.dbConn.ExecuteSql(sql);
            GridView1.EditIndex = -1;
            DataBound();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            DataBound();
        }

        protected void butAdd_Click(object sender, EventArgs e)
        {
            ((Button)GridView1.FooterRow.FindControl("butYes")).Visible = true;
            ((Button)GridView1.FooterRow.FindControl("butCancel")).Visible = true;

            StringBuilder stbweek = new StringBuilder();
            stbweek.Append("select datepart(wk,getdate())");
            DataTable dtweek = new DataTable();
            dtweek = this.dbConn.GetDataSetBySql(stbweek.ToString()).Tables[0];
            DataRow dr = dtweek.Rows[0];

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            Button but1 = GridView1.FooterRow.FindControl("butAdd") as Button;
            but1.Visible = false;
            Button but2 = GridView1.FooterRow.FindControl("butYes") as Button;
            but2.Visible = true;
            Button but3 = GridView1.FooterRow.FindControl("butCancel") as Button;
            but3.Visible = true;

            DropDownList ddlFabID_1 = GridView1.FooterRow.FindControl("ddlFabID") as DropDownList;
            ddlFabID_1.Visible = true;
            DropDownList ddlShop_1 = GridView1.FooterRow.FindControl("ddlShop") as DropDownList;
            ddlShop_1.Visible = true;
            TextBox txtYear_1 = GridView1.FooterRow.FindControl("txtYear") as TextBox;
            txtYear_1.Visible = true;
            txtYear_1.Enabled = false;
            txtYear_1.Text = currentTime.Year.ToString();
            TextBox txtMonth_1 = GridView1.FooterRow.FindControl("txtMonth") as TextBox;
            txtMonth_1.Visible = true;
            txtMonth_1.Enabled = false;
            txtMonth_1.Text = currentTime.Month.ToString();
            TextBox txtWeekNum_1 = GridView1.FooterRow.FindControl("txtWeekNum") as TextBox;
            txtWeekNum_1.Visible = true;
            txtWeekNum_1.Enabled = false;
            txtWeekNum_1.Text = dr[0].ToString();
            TextBox txtMFGDate_1 = GridView1.FooterRow.FindControl("txtMFGDate") as TextBox;
            txtMFGDate_1.Visible = true;
            txtMFGDate_1.Enabled = false;
            txtMFGDate_1.Text = currentTime.ToString();
            DropDownList ddlProdLine_1 = GridView1.FooterRow.FindControl("ddlProdLine") as DropDownList;
            ddlProdLine_1.Visible = true;
            DropDownList ddlProductID_2 = GridView1.FooterRow.FindControl("ddlProductID_1") as DropDownList;
            ddlProductID_2.Visible = true;
            TextBox txt8_1 = GridView1.FooterRow.FindControl("txt8") as TextBox;
            txt8_1.Visible = true;
            TextBox txt9_1 = GridView1.FooterRow.FindControl("txt9") as TextBox;
            txt9_1.Visible = true;
            TextBox txt10_1 = GridView1.FooterRow.FindControl("txt10") as TextBox;
            txt10_1.Visible = true;
            TextBox txt11_1 = GridView1.FooterRow.FindControl("txt11") as TextBox;
            txt11_1.Visible = true;
            TextBox txt12_1 = GridView1.FooterRow.FindControl("txt12") as TextBox;
            txt12_1.Visible = true;
            TextBox txt13_1 = GridView1.FooterRow.FindControl("txt13") as TextBox;
            txt13_1.Visible = true;
            TextBox txt14_1 = GridView1.FooterRow.FindControl("txt14") as TextBox;
            txt14_1.Visible = true;
            TextBox txt15_1 = GridView1.FooterRow.FindControl("txt15") as TextBox;
            txt15_1.Visible = true;
            TextBox txt16_1 = GridView1.FooterRow.FindControl("txt16") as TextBox;
            txt16_1.Visible = true;
            TextBox txt17_1 = GridView1.FooterRow.FindControl("txt17") as TextBox;
            txt17_1.Visible = true;
            TextBox txt18_1 = GridView1.FooterRow.FindControl("txt18") as TextBox;
            txt18_1.Visible = true;
            TextBox txt19_1 = GridView1.FooterRow.FindControl("txt19") as TextBox;
            txt19_1.Visible = true;
            TextBox txt20_1 = GridView1.FooterRow.FindControl("txt20") as TextBox;
            txt20_1.Visible = true;
            TextBox txt21_1 = GridView1.FooterRow.FindControl("txt21") as TextBox;
            txt21_1.Visible = true;
            TextBox txt22_1 = GridView1.FooterRow.FindControl("txt22") as TextBox;
            txt22_1.Visible = true;
            TextBox txt23_1 = GridView1.FooterRow.FindControl("txt23") as TextBox;
            txt23_1.Visible = true;
            TextBox txt24_1 = GridView1.FooterRow.FindControl("txt24") as TextBox;
            txt24_1.Visible = true;
            TextBox txt1_1 = GridView1.FooterRow.FindControl("txt1") as TextBox;
            txt1_1.Visible = true;
            TextBox txt2_1 = GridView1.FooterRow.FindControl("txt2") as TextBox;
            txt2_1.Visible = true;
            TextBox txt3_1 = GridView1.FooterRow.FindControl("txt3") as TextBox;
            txt3_1.Visible = true;
            TextBox txt4_1 = GridView1.FooterRow.FindControl("txt4") as TextBox;
            txt4_1.Visible = true;
            TextBox txt5_1 = GridView1.FooterRow.FindControl("txt5") as TextBox;
            txt5_1.Visible = true;
            TextBox txt6_1 = GridView1.FooterRow.FindControl("txt6") as TextBox;
            txt6_1.Visible = true;
            TextBox txt7_1 = GridView1.FooterRow.FindControl("txt7") as TextBox;
            txt7_1.Visible = true;
            //V1.01 Add Start
            TextBox txt_Target_1 = GridView1.FooterRow.FindControl("txt_Target") as TextBox;
            txt_Target_1.Visible = true;
            //V1.01 Add End
            TextBox txtCreate_By1 = GridView1.FooterRow.FindControl("txtCreate_By") as TextBox;
            txtCreate_By1.Visible = true;
            txtCreate_By1.Enabled = false;
            txtCreate_By1.Text = ((UserInfo)Session["UserInfo"]).UserID;
            TextBox txtModify_by1 = GridView1.FooterRow.FindControl("txtModify_by") as TextBox;
            txtModify_by1.Visible = true;
            txtModify_by1.Enabled = false;
            txtModify_by1.Text = ((UserInfo)Session["UserInfo"]).UserName;

            int j = GridView1.Rows.Count;
            for (int i = 0; i < j; i++)
            {
                GridView1.Rows[i].Visible = false;
            }
        }

        protected void butCancel_Click(object sender, EventArgs e)
        {
            DataBound();
        }

        protected void butYes_Click(object sender, EventArgs e)
        {
            string fabid_1 = ((DropDownList)GridView1.FooterRow.Cells[2].FindControl("ddlFabid")).Text.ToString().Trim();
            string shop_1 = ((DropDownList)GridView1.FooterRow.Cells[3].FindControl("ddlShop")).Text.ToString().Trim();
            string year_1 = ((TextBox)GridView1.FooterRow.Cells[4].FindControl("txtYear")).Text.ToString().Trim();
            string month_1 = ((TextBox)GridView1.FooterRow.Cells[5].FindControl("txtMonth")).Text.ToString().Trim();
            string weeknum_1 = ((TextBox)GridView1.FooterRow.Cells[6].FindControl("txtWeekNum")).Text.ToString().Trim();
            string mfgdate_1 = Convert.ToDateTime(((TextBox)GridView1.FooterRow.Cells[7].FindControl("txtMFGDate")).Text).ToString("yyyy-MM-dd");
            string prodline_1 = ((DropDownList)GridView1.FooterRow.Cells[8].FindControl("ddlProdLine")).Text.ToString().Trim();
            string productid_1 = ((DropDownList)GridView1.FooterRow.Cells[9].FindControl("ddlProductID_1")).Text.ToString().Trim();
            string hour8_1 = ((TextBox)GridView1.FooterRow.Cells[10].FindControl("txt8")).Text.ToString().Trim();
            string hour9_1 = ((TextBox)GridView1.FooterRow.Cells[11].FindControl("txt9")).Text.ToString().Trim();
            string hour10_1 = ((TextBox)GridView1.FooterRow.Cells[12].FindControl("txt10")).Text.ToString().Trim();
            string hour11_1 = ((TextBox)GridView1.FooterRow.Cells[13].FindControl("txt11")).Text.ToString().Trim();
            string hour12_1 = ((TextBox)GridView1.FooterRow.Cells[14].FindControl("txt12")).Text.ToString().Trim();
            string hour13_1 = ((TextBox)GridView1.FooterRow.Cells[15].FindControl("txt13")).Text.ToString().Trim();
            string hour14_1 = ((TextBox)GridView1.FooterRow.Cells[16].FindControl("txt14")).Text.ToString().Trim();
            string hour15_1 = ((TextBox)GridView1.FooterRow.Cells[17].FindControl("txt15")).Text.ToString().Trim();
            string hour16_1 = ((TextBox)GridView1.FooterRow.Cells[18].FindControl("txt16")).Text.ToString().Trim();
            string hour17_1 = ((TextBox)GridView1.FooterRow.Cells[19].FindControl("txt17")).Text.ToString().Trim();
            string hour18_1 = ((TextBox)GridView1.FooterRow.Cells[20].FindControl("txt18")).Text.ToString().Trim();
            string hour19_1 = ((TextBox)GridView1.FooterRow.Cells[21].FindControl("txt19")).Text.ToString().Trim();
            string hour20_1 = ((TextBox)GridView1.FooterRow.Cells[22].FindControl("txt20")).Text.ToString().Trim();
            string hour21_1 = ((TextBox)GridView1.FooterRow.Cells[23].FindControl("txt21")).Text.ToString().Trim();
            string hour22_1 = ((TextBox)GridView1.FooterRow.Cells[24].FindControl("txt22")).Text.ToString().Trim();
            string hour23_1 = ((TextBox)GridView1.FooterRow.Cells[25].FindControl("txt23")).Text.ToString().Trim();
            string hour24_1 = ((TextBox)GridView1.FooterRow.Cells[26].FindControl("txt24")).Text.ToString().Trim();
            string hour1_1 = ((TextBox)GridView1.FooterRow.Cells[27].FindControl("txt1")).Text.ToString().Trim();
            string hour2_1 = ((TextBox)GridView1.FooterRow.Cells[28].FindControl("txt2")).Text.ToString().Trim();
            string hour3_1 = ((TextBox)GridView1.FooterRow.Cells[29].FindControl("txt3")).Text.ToString().Trim();
            string hour4_1 = ((TextBox)GridView1.FooterRow.Cells[30].FindControl("txt4")).Text.ToString().Trim();
            string hour5_1 = ((TextBox)GridView1.FooterRow.Cells[31].FindControl("txt5")).Text.ToString().Trim();
            string hour6_1 = ((TextBox)GridView1.FooterRow.Cells[32].FindControl("txt6")).Text.ToString().Trim();
            string hour7_1 = ((TextBox)GridView1.FooterRow.Cells[33].FindControl("txt7")).Text.ToString().Trim();
            //V1.01 Add Start
            string targetyield_1 = ((TextBox)GridView1.FooterRow.Cells[34].FindControl("txt_Target")).Text.ToString().Trim();
            //V1.01 Add End
            string userid = ((UserInfo)Session["UserInfo"]).UserID;
            string username = ((UserInfo)Session["UserInfo"]).UserName;

            string sqlinsert = "INSERT INTO ROPPRD([DataEnd_Time], [FabID], [Shop], [Year], [Month], [WeekNum], [MFGDate], [ProdLine], [ProductID], [Hour_8], [Hour_9], [Hour_10], [Hour_11], [Hour_12], [Hour_13], [Hour_14], [Hour_15], [Hour_16], [Hour_17], [Hour_18], [Hour_19], [Hour_20], [Hour_21], [Hour_22], [Hour_23], [Hour_24], [Hour_1], [Hour_2], [Hour_3], [Hour_4], [Hour_5], [Hour_6], [Hour_7],[TargetYield], [Create_Time], [Create_By], [Modify_Time], [Modify_by])";//V1.01 Add [TargetYield]
            sqlinsert += "values(getdate(),'" + fabid_1 + "','" + shop_1 + "','" + year_1 + "','" + month_1 + "','" + weeknum_1 + "','" + mfgdate_1 + "','";
            sqlinsert += "" + prodline_1 + "','" + productid_1 + "','" + hour8_1 + "','" + hour9_1 + "','" + hour10_1 + "','" + hour11_1 + "','" + hour12_1 + "','" + hour13_1 + "','" + hour14_1 + "','" + hour15_1 + "','" + hour16_1 + "','" + hour17_1 + "','" + hour18_1 + "','" + hour19_1 + "','" + hour20_1 + "','" + hour21_1 + "','" + hour22_1 + "','" + hour23_1 + "','" + hour24_1 + "','" + hour1_1 + "','" + hour2_1 + "','" + hour3_1 + "','" + hour4_1 + "','" + hour5_1 + "','" + hour6_1 + "','" + hour7_1 + "','" + targetyield_1 + "',";//V1.01 Add targetyield_1
            sqlinsert += "getdate(),'" + userid + "'," + "getdate(),'" + username + "')";

            this.dbConn.ExecuteSql(sqlinsert);
            GridView1.EditIndex = -1;
            DataBound();
        }

        protected void butUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                DataSet ds = new DataSet();
                StringBuilder strMsg = new StringBuilder("提示信息： <hr color=red>");
                string filePath = "", fileExtName = "", mFilename, mPath;
                if ("" != FileUpload1.PostedFile.FileName)
                {
                    filePath = FileUpload1.PostedFile.FileName;
                    fileExtName = filePath.Substring(filePath.LastIndexOf(".") + 1);
                    mFilename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    try
                    {
                        mPath = Server.MapPath("~/Upload/");
                        DirectoryInfo dirinfo = new DirectoryInfo(mPath.ToString());
                        FileInfo fi;
                        foreach (FileSystemInfo fsi in dirinfo.GetFileSystemInfos())
                        {
                            fi = (FileInfo)fsi;
                            if (Convert.ToString(fi) == mFilename)
                            {
                                this.Response.Write("<script language=javascript>window.alert('存在相同的文件名,請更名后再傳!');</script>");
                                return;
                            }
                        }
                        if (filePath.Substring(filePath.LastIndexOf(".") + 1) == "xls")
                        {
                            strMsg.Append("上傳的文件名：" + mFilename + "<br>");
                            strMsg.Append("上傳文件的擴展名：" + fileExtName + "<br>");
                            FileUpload1.PostedFile.SaveAs(mPath + mFilename);

                            #region  ----read excel to memory
                            string path = Server.MapPath("~/Upload/") + mFilename;
                            string conn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'", mFilename);
                            //OleDbConnection thisconn = new OleDbConnection(conn);
                            OleDbDataAdapter ada = new OleDbDataAdapter("SELECT * FROM [sheet1$] where MFGDATE is not null", conn);
                            //thisconn.Open();
                            //string sql = "Select * from [Sheet1$] where DATAEND_TIME is not null";
                            //OleDbDataAdapter mycmd = new OleDbDataAdapter(sql, thisconn);
                            ada.Fill(ds, "[Sheet1$]");
                            count = ds.Tables["[Sheet1$]"].Rows.Count;
                            strMsg.Append("上傳文件的資料數：" + count + "筆" + "<br>");
                            //thisconn.Close();
                            #endregion
                        }
                        else
                        {
                            this.Response.Write("<script language=javascript>window.alert('請確認上傳的文件類型為 Xls !');</script>");
                            return;
                        }
                    }
                    catch (Exception error)
                    {
                        throw error;
                        Label1.Visible = true;
                        Label1.Text = "EXCEL的第一頁名稱不為Sheet1或EXCEL資料表錯誤!";
                        return;
                    }
                }
                #region  -----insert data to DB
                //string strCn2 = @"Provider=SQLOLEDB;server=10.65.33.150;database=NJPLCM2HIS;uid=qashis;pwd=qashis;Max Pool Size=10;";
                //OleDbConnection inconn = new OleDbConnection(strCn2);
                //inconn.Open();
                for (int i = 0; i < count; i++)
                {
                    string userid = ((UserInfo)Session["UserInfo"]).UserID;
                    string username = ((UserInfo)Session["UserInfo"]).UserName;
                    string FabID = ds.Tables["[Sheet1$]"].Rows[i]["廠別"].ToString();
                    string MFGDATE = ds.Tables["[Sheet1$]"].Rows[i]["MFGDATE"].ToString();
                    string ProdLine = ds.Tables["[Sheet1$]"].Rows[i]["生產線別"].ToString();
                    string SHOP = ds.Tables["[Sheet1$]"].Rows[i]["SHOP"].ToString();
                    string ProductID = ds.Tables["[Sheet1$]"].Rows[i]["產品號碼"].ToString();
                    string H8 = ds.Tables["[Sheet1$]"].Rows[i]["H8"].ToString();
                    if (H8 == "") { H8 = "0"; }
                    string H9 = ds.Tables["[Sheet1$]"].Rows[i]["H9"].ToString();
                    if (H9 == "") { H9 = "0"; }
                    string H10 = ds.Tables["[Sheet1$]"].Rows[i]["H10"].ToString();
                    if (H10 == "") { H10 = "0"; }
                    string H11 = ds.Tables["[Sheet1$]"].Rows[i]["H11"].ToString();
                    if (H11 == "") { H11 = "0"; }
                    string H12 = ds.Tables["[Sheet1$]"].Rows[i]["H12"].ToString();
                    if (H12 == "") { H12 = "0"; }
                    string H13 = ds.Tables["[Sheet1$]"].Rows[i]["H13"].ToString();
                    if (H13 == "") { H13 = "0"; }
                    string H14 = ds.Tables["[Sheet1$]"].Rows[i]["H14"].ToString();
                    if (H14 == "") { H14 = "0"; }
                    string H15 = ds.Tables["[Sheet1$]"].Rows[i]["H15"].ToString();
                    if (H15 == "") { H15 = "0"; }
                    string H16 = ds.Tables["[Sheet1$]"].Rows[i]["H16"].ToString();
                    if (H16 == "") { H16 = "0"; }
                    string H17 = ds.Tables["[Sheet1$]"].Rows[i]["H17"].ToString();
                    if (H17 == "") { H17 = "0"; }
                    string H18 = ds.Tables["[Sheet1$]"].Rows[i]["H18"].ToString();
                    if (H18 == "") { H18 = "0"; }
                    string H19 = ds.Tables["[Sheet1$]"].Rows[i]["H19"].ToString();
                    if (H19 == "") { H19 = "0"; }
                    string H20 = ds.Tables["[Sheet1$]"].Rows[i]["H20"].ToString();
                    if (H20 == "") { H20 = "0"; }
                    string H21 = ds.Tables["[Sheet1$]"].Rows[i]["H21"].ToString();
                    if (H21 == "") { H21 = "0"; }
                    string H22 = ds.Tables["[Sheet1$]"].Rows[i]["H22"].ToString();
                    if (H22 == "") { H22 = "0"; }
                    string H23 = ds.Tables["[Sheet1$]"].Rows[i]["H23"].ToString();
                    if (H23 == "") { H23 = "0"; }
                    string H24 = ds.Tables["[Sheet1$]"].Rows[i]["H24"].ToString();
                    if (H24 == "") { H24 = "0"; }
                    string H1 = ds.Tables["[Sheet1$]"].Rows[i]["H1"].ToString();
                    if (H1 == "") { H1 = "0"; }
                    string H2 = ds.Tables["[Sheet1$]"].Rows[i]["H2"].ToString();
                    if (H2 == "") { H2 = "0"; }
                    string H3 = ds.Tables["[Sheet1$]"].Rows[i]["H3"].ToString();
                    if (H3 == "") { H3 = "0"; }
                    string H4 = ds.Tables["[Sheet1$]"].Rows[i]["H4"].ToString();
                    if (H4 == "") { H4 = "0"; }
                    string H5 = ds.Tables["[Sheet1$]"].Rows[i]["H5"].ToString();
                    if (H5 == "") { H5 = "0"; }
                    string H6 = ds.Tables["[Sheet1$]"].Rows[i]["H6"].ToString();
                    if (H6 == "") { H6 = "0"; }
                    string H7 = ds.Tables["[Sheet1$]"].Rows[i]["H7"].ToString();
                    if (H7 == "") { H7 = "0"; }
                    /* V1.01 Add Start */
                    string TargetYield = ds.Tables["[Sheet1$]"].Rows[i]["TargetYield"].ToString();
                    /* V1.01 Add End */
                    string sql = @"select * from REP.mslm A where WORKCTR IN('8270','8360') AND  sublineid='" + ProdLine + "'";
                    DataTable dt = dbConn.GetDataSetBySql(sql).Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write("<Script Language='JavaScript'>window.alert('" + ProdLine + "線別不在MSLM線別主檔中,請修正后繼續上傳!');</script>");
                        return;
                    }

                    string sql_1 = @"select * from mpim  a where PROJECT_NAME='" + ProductID + "'";
                    DataTable dt_1 = dbConn.GetDataSetBySql(sql_1).Tables[0];
                    if (dt_1.Rows.Count == 0)
                    {
                        Response.Write("<Script Language='JavaScript'>window.alert('" + ProductID + "產品不在MPIM產品主檔中,請修正后繼續上傳!');</script>");
                        return;
                    }

                    string insql;
                    insql = " DELETE FROM ROPPRD WHERE FABID='" + FabID.ToString() + "' AND MFGDATE='" + Convert.ToDateTime(MFGDATE).ToString("yyyy-MM-dd") + "' AND PRODLINE='" + ProdLine.ToString() + "' AND PRODUCTID='" + ProductID.ToString() + "';";
                    insql += "INSERT INTO ROPPRD([DataEnd_Time], [FabID], [Shop], [Year], [Month], [WeekNum], [MFGDate], [ProdLine],  [ProductID], [Hour_8], [Hour_9], [Hour_10], [Hour_11], [Hour_12], [Hour_13], [Hour_14], [Hour_15], [Hour_16], [Hour_17], [Hour_18], [Hour_19], [Hour_20],  [Hour_21], [Hour_22], [Hour_23], [Hour_24], [Hour_1], [Hour_2], [Hour_3], [Hour_4], [Hour_5], [Hour_6], [Hour_7],[TargetYield], [Create_Time], [Create_By], [Modify_Time],  [Modify_by])";  //V1.01 Add [TargetYield]
                    insql += "values(getdate(),'" + FabID.ToString() + "','" + SHOP.ToString() + "',";
                    insql += "datepart(year,'" + Convert.ToDateTime(MFGDATE).ToString("yyyy-MM-dd HH:mm:ss") + "'),datepart (month,'" + Convert.ToDateTime(MFGDATE).ToString("yyyy-MM-dd HH:mm:ss") + "'),datepart(wk,'" + Convert.ToDateTime(MFGDATE).ToString("yyyy-MM-dd HH:mm:ss") + "'),";
                    insql += "'" + Convert.ToDateTime(MFGDATE).ToString("yyyy-MM-dd") + "','" + ProdLine.ToString() + "','" + ProductID.ToString() + "',";
                    insql += "isnull(" + H8.ToString() + ",0),isnull(" + H9.ToString() + ",0),isnull(" + H10.ToString() + ",0),";
                    insql += "isnull(" + H11.ToString() + ",0),isnull(" + H12.ToString() + ",0),isnull(" + H13.ToString() + ",0),";
                    insql += "isnull(" + H14.ToString() + ",0),isnull(" + H15.ToString() + ",0),isnull(" + H16.ToString() + ",0),";
                    insql += "isnull(" + H17.ToString() + ",0),isnull(" + H18.ToString() + ",0),isnull(" + H19.ToString() + ",0),";
                    insql += "isnull(" + H20.ToString() + ",0),isnull(" + H21.ToString() + ",0),isnull(" + H22.ToString() + ",0),";
                    insql += "isnull(" + H23.ToString() + ",0),isnull(" + H24.ToString() + ",0),isnull(" + H1.ToString() + ",0),";
                    insql += "isnull(" + H2.ToString() + ",0),isnull(" + H3.ToString() + ",0),isnull(" + H4.ToString() + ",0),";
                    insql += "isnull(" + H5.ToString() + ",0),isnull(" + H6.ToString() + ",0),isnull(" + H7.ToString() + ",0),";
                    /* V1.01 Add Start */
                    insql += TargetYield.ToString() + ",";
                    /* V1.01 Add End */
                    insql += "getdate(),'" + userid + "',getdate(),'" + username + "') ";

                    try
                    {
                        //inconn.Close();
                        //SqlConnection sqlin = new SqlConnection(dbConn.ConnectionString);
                        this.dbConn.ExecuteSql(insql);
                    }
                    catch
                    {
                        strMsg.Append("<b>狀態:錯誤!!成功插入行數" + i + "行,未完成的行數:" + (count - i) + ",插入錯誤的行對應機種為" + ProductID + ",對應的線別為" + ProdLine + ",請檢查該行資料是否有的欄位值為空!</b>");
                        Label1.Visible = true;
                        Label1.Text = strMsg.ToString();
                        ds.Clear();
                        GC.Collect();
                        //inconn.Close();
                        return;
                    }
                }
                Response.Write("<Script Language='JavaScript'>window.alert('導入數據成功！');</script>");
                ds.Clear();
                //inconn.Close();
                DataBound();
                return;
                #endregion
            }
            else
            {
                Response.Write("<Script Language='JavaScript'>window.alert('上传文件为空,请选择文件!');</script>");
            }
        }

    }
}

