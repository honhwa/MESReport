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
    public partial class FrmOQC : System.Web.UI.Page
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
                this.txtFromDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        protected void ChkProd_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkProd.Checked)
            {
                DDLPrd.Enabled = true;
                TxtPrd.Enabled = true;
                this.SetProdDatasource();
            }
            else
            {
                this.DDLPrd.Items.Clear();
                DDLPrd.Enabled = false;
                TxtPrd.Enabled = false;
                TxtPrd.Text = "";
            }

        }

       
        private void SetProdDatasource()
        {
            string strsql = @"select prodid from mpim where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' order by prodid";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Prodid！');</script>");
            }
            else
            {
                this.DDLPrd.DataSource = dt;
                this.DDLPrd.DataTextField = "prodid";
                this.DDLPrd.DataValueField = "prodid";
                this.DDLPrd.DataBind();
                this.DDLPrd.Items.Insert(0, " ");
                this.DDLPrd.ClearSelection();
            }
        }
        private void SetWbrDatasource()
        {
            string strsql = @"select wonbr from New_swom  where fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Wonbr！');</script>");
            }
            else
            {
                this.DDlWbr.DataSource = dt;
                this.DDlWbr.DataTextField = "wonbr";
                this.DDlWbr.DataValueField = "wonbr";
                this.DDlWbr.DataBind();
            }
        }

        protected void ChkWbr_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWbr.Checked)
            {
                DDlWbr.Enabled = true;
                this.SetWbrDatasource();
            }
            else
            {
                this.DDlWbr.Items.Clear();
                DDlWbr.Enabled = false;
            }
                
        }

        protected void ChkSn_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSn.Checked )
            {
                TxtSn.Enabled = true;
            }
            else
            {
                TxtSn.Enabled = false;
            }
        }

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
            stb.Append("select transDATE,wonbr,prodid,SUBentityid,SUBLINEID,entityTqty,case when okflg='1' then 'OK' else 'NG' end as okflg,");
            stb.Append(" defectcd,shiftid,userid from wQAx  ");
            stb.Append(" where  fabid='"+((UserInfo)Session["UserInfo"]).FabId+"'");
            if (txtFromDate.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "開始時間為空！\n";
                   // this.lblErr.Text = strErrorMsg;
                    //return;
                    Err = true;
                }
                else
                {
                    stb.Append(" and transDATE>='" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
                }
                if (txtToDate.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "結束時間為空！\n";
                    //this.lblErr.Text = strErrorMsg;
                    //return;
                    Err = true;
                }
                else
                {
                    stb.Append(" and transDATE<='" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
                }
                if (this.ChkProd.Checked)
                {
                    if (DDLPrd.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "Prodid為空！\n";
                        //this.lblErr.Text = strErrorMsg;
                        //return;
                        Err = true;
                    }
                    else
                    {
                        //  stb.Append(" and prodid='"+this.DDLPrd.Text.Trim().Replace("'","")+"'");
                        stb.Append(" and prodid='" + this.TxtPrd.Text.Trim().Replace("'", "") + "'");
                    }
                }
                if (this.ChkWbr.Checked)

                    {
                        if (ChkWbr.Text.Trim().Replace("'", "") == "")
                        {
                            strErrorMsg += "工單為空！\n";
                            //this.lblErr.Text = strErrorMsg;
                            //return;
                            Err = true;
                        }
                        else
                        {
                            stb.Append(" and WONBR='" + this.DDlWbr.Text.Trim().Replace("'", "") + "'");
                        }
                   }
                   if (ChkSn.Checked)
                   {
                       if (TxtSn.Text.Trim().Replace("'", "") == "")
                       {
                           strErrorMsg += "SN為空！\n";
                           //this.lblErr.Text = strErrorMsg;
                           //return;
                           Err = true;
                       }
                       else
                       {
                           stb.Append(" and SUBentityid='" + TxtSn.Text.Trim().Replace("'", "")+"'");
                       }
                   }
                   if (ChkProd.Checked == false && ChkWbr.Checked == false && ChkSn.Checked == false)
                   {
                       strErrorMsg += "請選擇查詢條件！\n";
                       Err = true;
                   }
               
                   if (Err)
                   {
                       this.lblErr.Text = strErrorMsg;
                   }
                   else
                   {
                       stb.Append("order by transDATE");
                       dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                       if (dt.Rows.Count < 1)
                       {
                           strErrorMsg += "查無資料！\n";
                           this.lblErr.Text = strErrorMsg;
                       }
                   }
                       this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                       LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                       localReportRoqcs.ReportPath = @"RDLC\SNChk.rdlc";


                       ReportDataSource rsd = new ReportDataSource("SNChk", dt);
                       localReportRoqcs.DataSources.Clear();
                       localReportRoqcs.DataSources.Add(rsd);
        }

        protected void DDLPrd_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtPrd.Text = DDLPrd.Text;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
           this.Calendar1.Visible = true;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            txtFromDate.Text = this.Calendar1.SelectedDate.ToShortDateString();

            txtFromDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            this.Calendar1.Visible = false;
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            this.Calendar2.Visible = true;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            txtToDate.Text = this.Calendar2.SelectedDate.ToShortDateString();

            txtToDate.Text += " " + System.DateTime.Now.AddDays(-1).ToString("HH:mm:ss");
            this.Calendar2.Visible = false;
        }
     
    }
}
