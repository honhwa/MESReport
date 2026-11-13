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
    public partial class Cell2PanelRecord : System.Web.UI.Page
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

        protected void ChkLotID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkLotID.Checked)
            {
                TxtLotID.Enabled = true;
            }
            else
            {
                TxtLotID.Enabled = false;
                TxtLotID.Text = "";
            }
        }

        protected void ChkKind_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkKind.Checked)
            {
                DDLKind.Enabled = true;
                TxtKind.Enabled = true;
                this.SetKindDatasource();
            }
            else
            {
                this.DDLKind.Items.Clear();
                DDLKind.Enabled = false;
                TxtKind.Enabled = false;
                TxtKind.Text = "";
            }
        }

        protected void ChkProdID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkProdID.Checked)
            {
                DDLProdID.Enabled = true;
                TxtProdID.Enabled = true;
                this.SetProdIDDatasource();
            }
            else
            {
                this.DDLProdID.Items.Clear();
                DDLProdID.Enabled = false;
                TxtProdID.Enabled = false;
                TxtProdID.Text = "";
            }
        }

        protected void ChkWonbr_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWonbr.Checked)
            {
                DDLWonbr.Enabled = true;
                TxtWonbr.Enabled = true;
                this.SetWonbrDatasource();
            }
            else
            {
                this.DDLWonbr.Items.Clear();
                DDLWonbr.Enabled = false;
                TxtWonbr.Enabled = false;
                TxtWonbr.Text = "";
            }
        }
        private void SetKindDatasource()
        {
            string strsql = @"SELECT DISTINCT KIND,
                             CASE KIND
                                 WHEN 'CSPL' THEN 'CSPL--CELL2 SCRAP PANEL '
                                 WHEN 'CLPL' THEN 'CLPL--LASER REPAIR'
                                 WHEN 'CAPL' THEN 'CAPL--嚴苛AGING'
                              END AS KINDDESC
                              FROM WLSX
                              WHERE KIND IS NOT NULL  
                              ORDER BY KIND";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find KIND！');</script>");
            }
            else
            {
                this.DDLKind.DataSource = dt;
                this.DDLKind.DataTextField = "KINDDESC";
                this.DDLKind.DataValueField = "KIND";
                this.DDLKind.DataBind();
                this.DDLKind.Items.Insert(0, " ");
                this.DDLKind.ClearSelection();
            }
        }
        private void SetProdIDDatasource()
        {
            string strsql = @"SELECT DISTINCT PRODID
                              FROM WLSX
                              WHERE PRODID IS NOT NULL  
                              ORDER BY PRODID";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find PRODID！');</script>");
            }
            else
            {
                this.DDLProdID.DataSource = dt;
                this.DDLProdID.DataTextField = "PRODID";
                this.DDLProdID.DataValueField = "PRODID";
                this.DDLProdID.DataBind();
                this.DDLProdID.Items.Insert(0, " ");
                this.DDLProdID.ClearSelection();
            }
        }
        private void SetWonbrDatasource()
        {
            string strsql = @"SELECT DISTINCT WONBR
                              FROM WLSX
                              WHERE WONBR IS NOT NULL  
                              ORDER BY WONBR";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find WONBR！');</script>");
            }
            else
            {
                this.DDLWonbr.DataSource = dt;
                this.DDLWonbr.DataTextField = "WONBR";
                this.DDLWonbr.DataValueField = "WONBR";
                this.DDLWonbr.DataBind();
                this.DDLWonbr.Items.Insert(0, " ");
                this.DDLWonbr.ClearSelection();
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
//                              (select SN from FILEPARM 
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
            stb.Append("SELECT CONVERT(VARCHAR(19),TRANSDATE,120) AS transdate,entityid as panelid,lotid,workctr,hideflg,kind,prodid,wonbr,userid ");
            stb.Append("FROM WLSX ");
            stb.Append(" where 1=1");

                if (txtFromDate.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "開始時間為空！\n";
                    Err = true;
                }
                else
                {
                    stb.Append(" and CONVERT(VARCHAR(10),TRANSDATE,120)>='" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
                }

                if (txtToDate.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "結束時間為空！\n";
                    Err = true;
                }
                else
                {
                    stb.Append(" and CONVERT(VARCHAR(10),TRANSDATE,120)<='" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
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
                        stb.Append(" and ENTITYID='" + this.TxtPanelID.Text.Trim().Replace("'", "") + "'");
                    }
                }


                if (this.ChkLotID.Checked)
                {
                    if (TxtLotID.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "LotID為空！\n";
                        Err = true;
                    }
                    else
                    {
                        stb.Append(" and LOTID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "'");
                    }
                }


                if (this.ChkKind.Checked)
                {
                    if (TxtKind.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "Kind為空！\n";
                        Err = true;
                    }
                    else
                    {
                        stb.Append(" and KIND='" + this.TxtKind.Text.Trim().Replace("'", "") + "'");
                    }
                }


                if (this.ChkProdID.Checked)
                {
                    if (TxtProdID.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "ProdID為空！\n";
                        Err = true;
                    }
                    else
                    {
                        stb.Append(" and PRODID='" + this.TxtProdID.Text.Trim().Replace("'", "") + "'");
                    }
                }


                if (this.ChkWonbr.Checked)
                {
                    if (TxtWonbr.Text.Trim().Replace("'", "") == "")
                    {
                        strErrorMsg += "Wonbr為空！\n";
                        Err = true;
                    }
                    else
                    {
                        stb.Append(" and WONBR='" + this.TxtWonbr.Text.Trim().Replace("'", "") + "'");
                    }
                }

                //if (this.ChkSN.Checked)
                //{
                //    if (TxtSN.Text.Trim().Replace("'", "") == "")
                //    {
                //        strErrorMsg += "SN為空！\n";
                //        Err = true;
                //    }
                //    else
                //    {
                //        stb.Append(" and B.CMT='" + this.TxtSN.Text.Trim().Replace("'", "") + "'");
                //    }
                //}


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
                       stb.Append("order by TRANSDATE,ENTITYID");
                       dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                       if (dt.Rows.Count < 1)
                       {
                           strErrorMsg += "查無資料！\n";
                           this.lblErr.Text = strErrorMsg;
                       }
                   }
                       this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                       LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                       localReportRoqcs.ReportPath = @"RDLC\LaserRepair.rdlc";


                       ReportDataSource rsd = new ReportDataSource("LaserRepair_WLSX", dt);
                       localReportRoqcs.DataSources.Clear();
                       localReportRoqcs.DataSources.Add(rsd);
        }

        //protected void DDLPanelID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TxtPanelID.Text = DDLPanelID.Text;
        //}

        protected void DDLKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtKind.Text = DDLKind.Text;
        }

        protected void DDLProdID_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtProdID.Text = DDLProdID.Text;
        }

        protected void DDLWonbr_SelectedIndexChanged(object sender, EventArgs e)
        {
                TxtWonbr.Text = DDLWonbr.Text;
        }



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
