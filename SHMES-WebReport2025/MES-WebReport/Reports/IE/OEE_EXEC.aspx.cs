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
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Report.Library;
using System.Text;

namespace WebReport.Reports
{
    public partial class OEE_EXEC : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
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
                sql1.Append(" select Userid from MUGMM where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' and userid='" + ((UserInfo)Session["UserInfo"]).UserID + "' and usergroupcd='OEE'  ");
                DataTable dt = this.dbConn.GetDataSetBySql(sql1.ToString()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    this.BtnOEEExec.Enabled = true;
                }
                else
                {
                    this.BtnOEEExec.Enabled = false;
                    lblError.Visible = true;
                    lblError.Text = "用戶：" + ((UserInfo)Session["UserInfo"]).UserID + " 不在OEE群組內，不可以執行！";
                }
            }
        }

        protected void BtnOEEExec_Click(object sender, EventArgs e)
        {
                Refresh();
                SqlConnection DbConnection = new SqlConnection(dbConn.ConnectionString);
                SqlCommand OEECommand = new SqlCommand("[USP_RPT_EDW]", DbConnection);

                OEECommand.CommandType = CommandType.StoredProcedure;
                OEECommand.CommandTimeout = 90;
                DbConnection.Open();
                try
                {
                    OEECommand.Parameters.Add("@DATE", SqlDbType.NVarChar, 30);
                    OEECommand.Parameters.Add("@PROCESS", SqlDbType.NVarChar, 20).IsNullable = true;//設置是否可為NULL
                    OEECommand.Parameters.Add("@EQ_ID", SqlDbType.NVarChar, 20).IsNullable = true;//設置是否可為NULL
                    OEECommand.Parameters.Add("@EQ_TYPE", SqlDbType.NVarChar, 30).IsNullable = true;//設置是否可為NULL
                    OEECommand.Parameters.Add("@OUT_CODE", SqlDbType.NVarChar, 30).Direction = ParameterDirection.Output;//設置是否為輸出值



                    OEECommand.Parameters.Add("@OUT_DESC", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;//設置是否為輸出值



                    //傳值    傳入的日期需自增一天，查出來的才是選擇的日期



                    OEECommand.Parameters["@DATE"].Value = Convert.ToDateTime(txtDT.Text).AddDays(1).ToShortDateString();
                    OEECommand.Parameters["@PROCESS"].Value = "";
                    OEECommand.Parameters["@EQ_ID"].Value = "";
                    OEECommand.Parameters["@EQ_TYPE"].Value = "";
                    //執行
                    OEECommand.ExecuteNonQuery();
                    string out_code = OEECommand.Parameters["@OUT_CODE"].Value.ToString();
                    string out_desc = OEECommand.Parameters["@OUT_DESC"].Value.ToString();

                    if (out_code != "" && out_desc != "")
                    {
                        lblError.Visible = true;
                        lblErrorCD.Visible = true;
                        lblErrorCD.Text = "執行失敗！    " + "錯誤代碼：" + out_code;
                        lblError.Text = "錯誤描述：" + out_desc;
                        Insert_Exec_log(0, "錯誤代碼：" + out_code + "詳情描述：" + out_desc);
                    }
                    else
                    {
                        lblError.Visible = false;
                        lblErrorCD.Visible = false;
                        Response.Write("<SCRIPT language=JavaScript>alert('OEE執行成功！');</SCRIPT>");
                        Insert_Exec_log(1,"OK");
                    }
                    DbConnection.Close();
                    if (ReportViewer1.LocalReport.ReportPath !=null)//判斷是否已經顯示ReportViewer控件
                    {
                        Log_Query();
                    }
                }
                catch (Exception ex)
                {
                    lblError.Visible = true;
                    lblErrorCD.Visible = true;
                    lblErrorCD.Text = "執行失敗！";
                    lblError.Text = "錯誤描述：" + ex.ToString();
                    DbConnection.Close();
                    Insert_Exec_log(0,ex.ToString());
                    if (ReportViewer1.LocalReport.ReportPath != null) //判斷是否已經顯示ReportViewer控件
                    {
                        Log_Query();
                    }
                }
        }
        /// <summary>
        /// 向LOG表插入執行記錄



        /// </summary>
        /// <param name="isOK">是否OK</param>
        /// <param name="Out_desc">報錯信息</param>
        private void Insert_Exec_log(int isOK,string Out_desc)  
        {
            Out_desc = Out_desc.Replace("'", "");
            string strSQL = "INSERT INTO R_OEE_EXEC_LOG VALUES('";
            strSQL = strSQL + ((UserInfo)Session["UserInfo"]).FabId + "','";
            strSQL = strSQL + ((UserInfo)Session["UserInfo"]).UserID + "','";
            strSQL = strSQL + isOK+"','";
            strSQL = strSQL + Out_desc + "','";
            strSQL = strSQL + txtDT.Text + "',";
            strSQL = strSQL + "getdate(),NULL,NULL,NULL)";
            try
            {
                dbConn.ExecuteSql(strSQL);
                lbl_LOG_ERROR.Visible = true;
                lbl_LOG_ERROR.Text = "執行動作已成功記入LOG表！";
            }
            catch(Exception ex)
            {
                lbl_LOG_ERROR.Visible = true;
                lbl_LOG_ERROR.Text = "記入LOG表有誤：" + ex.ToString();
            }
        }

        protected void btnQueryLog_Click(object sender, EventArgs e)
        {
            try
            {
                Refresh();
                Log_Query();
            }
            catch (Exception ex)
            {
                lblQueryError.Visible = true;
                lblQueryError.Text = ex.ToString();
            }
        }
        /// <summary>
        /// 查詢LOG表信息



        /// </summary>
        private void Log_Query()
        {
            string strSQL = "SELECT	A.FABID,A.USERID,B.USERNAME," +
                    "case A.ISEXEC_OK " +
                    "when 0 then '否' " +
                    "when 1 then '是' " +
                    "END AS ISEXEC_OK,A.CMT,A.EXEC_DATE,A.TIME_STAMP " +
                    "FROM R_OEE_EXEC_LOG A,MEMM B " +
                    "WHERE A.USERID=B.USERID " +
                    "ORDER BY Time_Stamp DESC ";
            DataSet DTLog = new DataSet();
            DTLog = dbConn.GetDataSetBySql(strSQL);
            this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
            if (DTLog.Tables[0].Rows.Count == 0)
            {
                lblQueryError.Visible = true;
                this.lblQueryError.Text = "查無執行記錄!!";
                return;
            }
            ReportViewer1.LocalReport.ReportPath = @"RDLC\Report_OEE_LOG.rdlc";//指定路径
            ReportViewer1.LocalReport.DataSources.Clear();//清掉原來的資料來源




            this.ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("OEE_EXEC_LOG", DTLog.Tables[0]));//上傳数据

            this.ReportViewer1.LocalReport.Refresh(); // 刷新控件    
        }

        private void Refresh()
        {
            lbl_LOG_ERROR.Visible = false;
            lblError.Visible = false;
            lblErrorCD.Visible = false;
            lblQueryError.Visible = false;
        }
    }
}
