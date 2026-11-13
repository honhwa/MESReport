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

namespace WebReport.Reports
{
    public partial class FrmUserID : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }
        private void SetSubLine()
        {
            string strsql = @"select  distinct sublineid   from mslm  where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'" + "and workctr='8380'";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Prodid！');</script>");
            }
            else
            {
                this.DDLineId.DataSource = dt;
                this.DDLineId.DataTextField = "sublineid";
                this.DDLineId.DataValueField = "sublineid";
                this.DDLineId.DataBind();
            }
        }

       

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                txtSn.Enabled = true;
            }
            else
            {
                txtSn.Text = "";
                txtSn.Enabled = false;
            }
        }


        private DataTable  DBWipConnQ(string sql)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection Conn = new SqlConnection();
                Conn.ConnectionString = @"Data Source=10.64.3.11\njlcm1wip;Initial Catalog=njplcm1wip;Persist Security Info=True;User ID=meswip;password=h@ppy2008";
                Conn.Open();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = sql;
                Cmd.Connection =Conn;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = Cmd;
                da.Fill(ds);
                Conn.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void ChkSubLine_CheckedChanged1(object sender, EventArgs e)
        {
            
            if (ChkSubLine.Checked)
            {
                DDLineId.Enabled = true;
                SetSubLine();
            }
            else
            {
                DDLineId.Items.Clear();
                DDLineId.Enabled = false ;
            }
    
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
           
            string strErrorMsg = "";
            StringBuilder stb = new StringBuilder();
            DataTable dt = new DataTable();
            lblErr.Text = "";
            bool Err = false;
            //dt.Clear();
            if  ((ChkSubLine.Checked == false) && (CheckBox1.Checked == false) && (CheckBox2.Checked ==false ))
            {
                strErrorMsg += "請選擇查詢條件！\n";
                //this.lblErr.Text = strErrorMsg;
               // return;
                Err = true;
            }

            stb.Append(" select TRANSDATE,userid,SUBLINEID,entityid from WSN2USERM  where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
            if (txtFromTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                //this.lblErr.Text = strErrorMsg;
                //return;
                Err = true;
            }
            else
            {
                stb.Append(" and TRANSDATE >='" + txtFromTime.Text.Trim().Replace("'", "") + "'");
            }

            if (txtToTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                //this.lblErr.Text = strErrorMsg;
                //return;
                Err = true;
            }
            else
            {
                stb.Append(" and TRANSDATE <='" + txtToTime.Text.Trim().Replace("'", "") + "'");
            }

            if (ChkSubLine.Checked)
             {
                 if (DDLineId.SelectedItem.ToString().Trim().Replace("'", "") == "")
                 {
                     strErrorMsg += "SubLineid為空！\n";
                     //this.lblErr.Text = strErrorMsg;
                     //return;
                     Err = true;
                 }
                 else
                 {
                    
                     stb.Append(" and SUBLINEID='" + DDLineId.SelectedItem.ToString().Trim().Replace("'", "") + "'");
                 }
             }

             if (CheckBox1.Checked)
             {
                 if (txtSn.Text.Trim().Replace("'", "") == "")
                 {
                     strErrorMsg += "SN為空！\n";
                     //this.lblErr.Text = strErrorMsg;
                     //return;
                     Err = true;
                 }
                 else
                 {
                     
                     stb.Append(" and entityid='" + txtSn.Text.Trim().Replace("'", "") + "'");
                 }
             }
             if (CheckBox2.Checked)
             {
                 if (txtUid.Text.Trim().Replace("'", "") == "")
                 {
                     strErrorMsg += "UserId為空！\n";
                     //this.lblErr.Text = strErrorMsg;
                     //return;
                     Err = true;
                 }
                 else
                 {
                  
                     stb.Append(" and userid='" + txtUid.Text.Trim().Replace("'", "") + "'");
                 }
             }

             if (Err)
             {
                 this.lblErr.Text = strErrorMsg;
                 //this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                 //LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                 //localReportRoqcs.ReportPath = @"RDLC\SNUserId.rdlc";
                 //ReportDataSource rsd = new ReportDataSource("SNUserId", dt);
                 //localReportRoqcs.DataSources.Clear();
                 //localReportRoqcs.DataSources.Add(rsd);
             }
             else
             {
                 dt = this.DBWipConnQ(stb.ToString());
                 // stb.ToString().Remove(1, stb.ToString().Length);
                 if (dt.Rows.Count < 1)
                 {
                     strErrorMsg += "查無資料！\n";
                     this.lblErr.Text = strErrorMsg;
                     // return;
                 }
                 //  else
                 //{
             }
                 this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                 LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                 localReportRoqcs.ReportPath = @"RDLC\SNUserId.rdlc";
                 ReportDataSource rsd = new ReportDataSource("SNUserId", dt);
                 localReportRoqcs.DataSources.Clear();
                 localReportRoqcs.DataSources.Add(rsd);
                 //}
          
       
        }

        protected void CheckBox2_CheckedChanged1(object sender, EventArgs e)
        {
           
            if (CheckBox2.Checked)
            {
                txtUid.Enabled = true;
            }
            else
            {
                txtUid.Text = "";
                txtUid.Enabled = false;
            }
      
        }
       
    }
}
