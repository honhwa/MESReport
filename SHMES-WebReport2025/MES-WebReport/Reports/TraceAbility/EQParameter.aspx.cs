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
    public partial class EQParameter : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.txtFromDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.txtToDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void ChkPanelID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPanelID.Checked)
            {
                //DDLPanelID.Enabled = true;
                TxtPanelID.Enabled = true;
                //this.SetPanelIDDatasource();
            }
            else
            {
                //this.DDLPanelID.Items.Clear();
                //DDLPanelID.Enabled = false;
                TxtPanelID.Enabled = false;
                TxtPanelID.Text = "";
            }

        }

        protected void ChkSN_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSN.Checked)
            {
                //DDLSN.Enabled = true;
                TxtSN.Enabled = true;
                //this.SetSNDatasource();
            }
            else
            {
                //this.DDLSN.Items.Clear();
                //DDLSN.Enabled = false;
                TxtSN.Enabled = false;
                TxtSN.Text = "";
            }

        }


//        private void SetPanelIDDatasource()
//        {
//            string strsql = @"SELECT DISTINCT A.ENTITYID AS LOTID
//                              FROM WLTM A
//                              WHERE PRODID IN (SELECT PRODID FROM MPIM WHERE PFLAG IN ('1','2'))
//                              ORDER  BY A.ENTITYID";
//            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
//            if (dt.Rows.Count <= 0)
//            {
//                this.Response.Write("<script language=javascript>window.alert('Cannot Find PanelID！');</script>");
//            }
//            else
//            {
//                this.DDLPanelID.DataSource = dt;
//                this.DDLPanelID.DataTextField = "lotid";
//                this.DDLPanelID.DataValueField = "lotid";
//                this.DDLPanelID.DataBind();
//                this.DDLPanelID.Items.Insert(0, " ");
//                this.DDLPanelID.ClearSelection();
//            }
//        }
//        private void SetSNDatasource()
//        {
//            string strsql = @"select DISTINCT A.SN FROM
//                              (select SN from
//                              
//                              union all
//                              select cmt from wssd where cmt<>'' ) A order by A.SN";
//            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
//            if (dt.Rows.Count <= 0)
//            {
//                this.Response.Write("<script language=javascript>window.alert('Cannot Find SN！');</script>");
//            }
//            else
//            {
//                this.DDLSN.DataSource = dt;
//                this.DDLSN.DataTextField = "SN";
//                this.DDLSN.DataValueField = "SN";
//                this.DDLSN.DataBind();
//                this.DDLSN.Items.Insert(0, " ");
//                this.DDLSN.ClearSelection();
//            }
//        }
        protected void ButQry_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strErrorMsg="";
            this.lblErr.Text = "";
            StringBuilder stb = new StringBuilder();
            DataTable dt = new DataTable();
            bool Err = false;
            stb.Append("SELECT CONVERT(VARCHAR(19),A.TRANSDATE,120) TRANSDATE,A.ENTITYID,A.SUBLINEID,A.PRODID,A.EQID,A.WORKCTR,A.PARAMETER,A.VALUE,A.USERID,A.TEAMID,B.CMT,A.OperationID ");
            stb.Append("FROM New_WEQPARAM A ");
            stb.Append("LEFT JOIN WSSD B ON A.ENTITYID=B.ENTITYID");
            stb.Append(" where 1=1");

                if (txtFromDate.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "開始時間為空！\n";
                    Err = true;
                }
                else
                {
                    stb.Append(" and CONVERT(VARCHAR(10),A.TRANSDATE,120)>='" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
                }

                if (txtToDate.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "結束時間為空！\n";
                    Err = true;
                }
                else
                {
                    stb.Append(" and CONVERT(VARCHAR(10),A.TRANSDATE,120)<='" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
                }


                if (this.ChkPanelID.Checked)
                {
                    if (TxtPanelID.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "PanelID為空！\n";
                        Err = true;
                    }
                    else
                    {
                        stb.Append(" and A.ENTITYID='" + this.TxtPanelID.Text.Trim().Replace("'", "") + "'");
                    }
                }

                if (this.ChkSN.Checked)
                {
                    if (TxtSN.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "SN為空！\n";
                        Err = true;
                    }
                    else
                    {
                        stb.Append(" and B.CMT='" + this.TxtSN.Text.Trim().Replace("'", "") + "'");
                    }
                }


                //if (this.ChkPanelID.Checked)
                //{
                //    if (DDLPanelID.Text.Trim().Replace("'", "") == "")
                //    {
                //        strErrorMsg += "PanelID為空！\n";
                //        Err = true;
                //    }
                //    else
                //    {
                //        stb.Append(" and A.ENTITYID='" + this.TxtPanelID.Text.Trim().Replace("'", "") + "'");
                //    }
                //}

                //if (this.ChkSN.Checked)
                //{
                //    if (DDLSN.Text.Trim().Replace("'", "") == "")
                //    {
                //        strErrorMsg += "SN為空！\n";
                //        Err = true;
                //    }
                //    else
                //    {
                //        stb.Append(" and B.CMT='" + this.TxtSN.Text.Trim().Replace("'", "") + "'");
                //    }
                //}




               //if (ChkPanelID.Checked == false && ChkSN.Checked == false)
               //{
               //    strErrorMsg += "請選擇查詢條件！\n";
               //    Err = true;
               //}
               
                   if (Err)
                   {
                       this.lblErr.Text = strErrorMsg;
                   }
                   else
                   {
                       stb.Append("order by A.TRANSDATE,A.ENTITYID,B.CMT");
                       dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                       if (dt.Rows.Count < 1)
                       {
                           strErrorMsg += "查無資料！\n";
                           this.lblErr.Text = strErrorMsg;
                       }
                   }
                       this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                       LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                       localReportRoqcs.ReportPath = @"RDLC\EQParameter.rdlc";


                       ReportDataSource rsd = new ReportDataSource("EQParameter_DataTable1", dt);
                       localReportRoqcs.DataSources.Clear();
                       localReportRoqcs.DataSources.Add(rsd);
        }

        //protected void DDLPanelID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TxtPanelID.Text = DDLPanelID.Text;
        //}



        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
           this.Calendar1.Visible = true;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            txtFromDate.Text = this.Calendar1.SelectedDate.ToString("yyyy-MM-dd");

           // txtFromDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            this.Calendar1.Visible = false;
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            this.Calendar2.Visible = true;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            txtToDate.Text = this.Calendar2.SelectedDate.ToString("yyyy-MM-dd");

          //  txtToDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            this.Calendar2.Visible = false;
        }

        //protected void DDLSN_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TxtSN.Text = DDLSN.Text;
        //}

    
     
    }
}
