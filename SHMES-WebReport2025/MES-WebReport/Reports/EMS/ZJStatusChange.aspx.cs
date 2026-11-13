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
using System.Text;
using System.Web.Hosting;
using Report.Library;
using System.Data.SqlClient;

namespace Report.Reports.EMS
{
    public partial class ZJStatusChange : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public string Eqline;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UserInfo"] == null) || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Redirect("../../Login.aspx");
            }
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                StringBuilder sql1 = new StringBuilder();
                sql1.Append(" select Userid from MUGMM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' and userid='" + ((UserInfo)Session["UserInfo"]).UserID + "' and usergroupcd='MFG'  ");
                DataTable dt = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    this.Button.Enabled = true;
                }
                else
                {
                    this.Button.Enabled = false;
                }
                CheckQuert();
                EQStaus_PK();
            }
        }
        private void CheckQuert()
        {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" select distinct eqid from WZJM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY eqid ");
            DataTable dt = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.CheckBoxList1.DataSource = dt;
                this.CheckBoxList1.DataTextField = "EQID";
                this.CheckBoxList1.DataBind();
            }
        }
        private void EQStaus_PK()
        {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" Select DISTINCT EQCCDESC,EQCCD from MECM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' order by EQCCDESC ");
            DataTable dt1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                this.DropDownList2.DataSource = dt1;
                this.DropDownList2.DataTextField = "EQCCDESC";
                this.DropDownList2.DataBind();
            }
            LoadEqcd();
            LoadEqcdDesc();
            LoadEqccdd();

            StringBuilder sql2 = new StringBuilder();
            sql2.Append(" select distinct prodid from MPIM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            DataTable dt2 = this.dbConn.GetDataSetBySql(sql2.ToString()).Tables[0];
            if (dt2.Rows.Count > 0)
            {
                this.DropDownList1.DataSource = dt2;
                this.DropDownList1.DataTextField = "prodid";
                this.DropDownList1.DataBind();
            }
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEqcd();
            LoadEqcdDesc();
            LoadEqccdd();
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEqcdDesc();
        }
        private void LoadEqccdd()
        {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" Select DISTINCT EQCCDESC,EQCCD from MECM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQCCDESC = '" + this.DropDownList2.SelectedValue.Trim().ToString() + "'  ");
            DataTable dt1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                this.TextBox2.Text = dt1.Rows[0][1].ToString();
            }
        }
        private void LoadEqcd()
        {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" Select EQCD from MECM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQCCDESC = '" + this.DropDownList2.SelectedValue.Trim().ToString() + "' order by EQCD ");
            DataTable dt1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                this.DropDownList3.DataSource = dt1;
                this.DropDownList3.DataTextField = "EQCD";
                this.DropDownList3.DataBind();
            }
        }
        private void LoadEqcdDesc()
        {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" Select EQCDDESC from MECM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQCCDESC = '" + this.DropDownList2.SelectedValue.Trim().ToString() + "' AND EQCD =  '" + this.DropDownList3.SelectedValue.Trim().ToString() + "' ");
            DataTable dt1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                this.TextBox3.Text = dt1.Rows[0][0].ToString();
            }
        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadZJStatus();
        }
        private void LoadZJStatus()
        {

            Eqline = "";
            for (int i = 0; i < this.CheckBoxList1.Items.Count; i++)
            {
                if (this.CheckBoxList1.Items[i].Selected == true)
                {
                    if (Eqline != "")
                    {
                        Eqline = Eqline + ",'" + this.CheckBoxList1.Items[i].Text.ToString() + "'";
                    }
                    else
                    {
                        Eqline = "'" + this.CheckBoxList1.Items[i].Text.ToString() + "'";
                    }
                }
            }
            if (Eqline == "")
            {
                Eqline = "''";
            }
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(" SELECT A.EQID,A.PSTATUS,A.STATUS,CONVERT(VARCHAR(20),STARTDATE,120) as STARTDATE,D.EQCDDESC,A.PRODID,A.USERID,A.CMT,isnull(B.EQCCDCOLOR,16777215) as PCol,isnull(C.EQCCDCOLOR,16777215) as Col FROM WZJM A ");
            sql1.Append(" LEFT JOIN MECCU B ON(A.PEQCCD = B.EQCCD)");
            sql1.Append(" LEFT JOIN MECCU C ON(A.EQCCD = C.EQCCD)");
            sql1.Append(" LEFT JOIN MECM   D ON(A.FABID=D.FABID AND A.EQCCD = D.EQCCD AND A.SUBEQCCD = D.EQCD)");
            sql1.Append(" WHERE A.FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQID IN( " + Eqline + ") ");
            DataTable dt1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
            this.GridView1.DataSource = dt1;
            this.GridView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            String aa = "";
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            //			if(e.Item.ItemType==ListItemType.AlternatingItem)
            //			{
            //				e.Item.BackColor=Color.Gainsboro;
            //			}
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells[8].Text != null)
                {
                    this.GridView1.Rows[i].Cells[1].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView1.Rows[i].Cells[8].Text));
                }
                if (this.GridView1.Rows[i].Cells[9].Text != null)
                {
                    this.GridView1.Rows[i].Cells[2].BackColor = System.Drawing.ColorTranslator.FromOle(int.Parse(this.GridView1.Rows[i].Cells[9].Text));
                }
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            string PStatus = "";
            if (Eqline == "")
            {
                this.Response.Write("<script language=javascript>window.alert('請先選擇EQLine,謝謝');</script>");
                return;
            }
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (PStatus == "" || this.GridView1.Rows[i].Cells[2].Text.Trim() == PStatus.Trim())
                {
                    PStatus = this.GridView1.Rows[i].Cells[2].Text.Trim();
                }
                else
                {
                    this.Response.Write("<script language=javascript>window.alert('所選擇EQLine轉換狀態,需當前狀態一致,謝謝');</script>");
                    return;
                }
                if (this.GridView1.Rows[i].Cells[2].Text.Trim() == this.DropDownList2.SelectedValue.Trim())
                {
                    this.Response.Write("<script language=javascript>window.alert('所選擇狀態不能與當前狀態一致,謝謝');</script>");
                    return;
                }
            }
            //if (this.DropDownList2.SelectedValue.Trim() != "RUN" && this.TextBox1.Text.Trim() == "")
            //{
            //    this.Response.Write("<script language=javascript>window.alert('CMT不能為空,謝謝');</script>");
            //    return;
            //}

            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select SHIFTID from MSSM where FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' and SSBEGIN <= getdate() and SSEND >=getdate()  ");
            DataTable dt2 = this.dbConn.GetDataSetBySql(strsql.ToString()).Tables[0];
            if (dt2.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('班別異常，請先確認班別主檔,謝謝');</script>");
                return;
            }

            /*Update Data*/

            SqlConnection TracConn = new SqlConnection(dbConn.ConnectionString.ToString());

            TracConn.Open();

            SqlTransaction trans = TracConn.BeginTransaction();
            SqlCommand strComm = TracConn.CreateCommand();

            strComm.Transaction = trans;
            try
            {
                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {
                    StringBuilder sql1 = new StringBuilder();
                    StringBuilder sql2 = new StringBuilder();
                    StringBuilder sql3 = new StringBuilder();
                    StringBuilder sql4 = new StringBuilder();

                    sql1.Append(" Select STATUS,EQCCD,SUBEQCCD,StartDate from WZJM A  where A.fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'  and a.eqid='" + this.GridView1.Rows[i].Cells[0].Text.Trim() + "'  ");
                    DataTable dt1 = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];

                    /*寫WZJM*/
                    if (dt1.Rows.Count == 0)
                    {
                        sql2.Append(" Insert Into WZJM(PRODID,FABID,EQID,StartDate,PSTATUS,PEQCCD,PSUBEQCCD,STATUS,EQCCD,SUBEQCCD,USERID,SHIFTID,CMT) ");
                        sql2.Append(" values('" + this.DropDownList1.SelectedItem.ToString() + "','" + ((UserInfo)Session["UserInfo"]).FabId + "','" + this.GridView1.Rows[i].Cells[0].Text.Trim() + "',GETDATE(),'','','', ");
                        sql2.Append(" '" + this.DropDownList2.SelectedItem.ToString() + "','" + this.TextBox2.Text.ToString() + "','" + this.DropDownList3.SelectedItem.ToString() + "',");
                        sql2.Append(" '" + ((UserInfo)Session["UserInfo"]).UserID + "','" + dt2.Rows[0][0].ToString() + "','" + this.TextBox1.Text.Trim() + "' ");
                    }
                    else
                    {
                        sql2.Append(" Update WZJM Set prodid='" + this.DropDownList1.SelectedItem.ToString() + "',StartDate=getdate(),USERID='" + ((UserInfo)Session["UserInfo"]).UserID + "',SHIFTID='" + dt2.Rows[0][0].ToString() + "', ");
                        sql2.Append(" PSTATUS='" + dt1.Rows[0][0].ToString() + "',PEQCCD= '" + dt1.Rows[0][1].ToString() + "' ,PSUBEQCCD= '" + dt1.Rows[0][2].ToString() + "',  ");
                        sql2.Append(" STATUS='" + this.DropDownList2.SelectedItem.ToString() + "',EQCCD= '" + this.TextBox2.Text + "' ,SUBEQCCD= '" + this.DropDownList3.SelectedItem.ToString() + "',  ");
                        sql2.Append(" CMT='" + this.TextBox1.Text.Trim() + "' ");
                        sql2.Append(" Where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' and EQID='" + this.GridView1.Rows[i].Cells[0].Text.Trim() + "' ");
                    }
                    strComm.CommandText = sql2.ToString();
                    strComm.ExecuteNonQuery();

                    /*寫WZJX*/
                    sql3.Append(" UPDATE WZJX SET ENDDATE = GETDATE() ");
                    sql3.Append(" WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQID = '" + this.GridView1.Rows[i].Cells[0].Text.Trim() + "' ");
                    sql3.Append(" AND ENDDATE IS NULL ");
                    strComm.CommandText = sql3.ToString();
                    strComm.ExecuteNonQuery();

                    sql4.Append(" Insert Into WZJX(PRODID,FABID,EQID,StartDate,PSTATUS,PEQCCD,PSUBEQCCD,STATUS,EQCCD,SUBEQCCD,USERID,SHIFTID,CMT) ");
                    sql4.Append(" values('" + this.DropDownList1.SelectedItem.ToString() + "','" + ((UserInfo)Session["UserInfo"]).FabId + "','" + this.GridView1.Rows[i].Cells[0].Text.Trim() + "',GETDATE(),'','','', ");
                    sql4.Append(" '" + this.DropDownList2.SelectedItem.ToString() + "','" + this.TextBox2.Text.ToString() + "','" + this.DropDownList3.SelectedItem.ToString() + "',");
                    sql4.Append(" '" + ((UserInfo)Session["UserInfo"]).UserID + "','" + dt2.Rows[0][0].ToString() + "','" + this.TextBox1.Text.Trim() + "') ");
                    strComm.CommandText = sql4.ToString();
                    strComm.ExecuteNonQuery();

                }
                trans.Commit();
            }
            catch
            {
                //回滚事物
                trans.Rollback();
            }
            LoadZJStatus();
        }
    }
}
