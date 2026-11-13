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
    public partial class MachineRunHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.txtFromTime.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        //protected void ChkWctr_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ChkWctr.Checked)
        //    {
        //        DDLCTR.Enabled = true;
        //        this.SetProdDatasource();
        //    }
        //    else
        //    {
        //        this.DDLCTR.Items.Clear();
        //        DDLPrd.Enabled = false;
        //    }

        //}

        protected void ChkSub_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSub.Checked)
            {
                DDLSUB.Enabled = true;
                this.SetSubDatasource();
            }
            else
            {
                this.DDLSUB.Items.Clear();
                DDLSUB.Enabled = false;
            }

        }
        protected void ChkSta_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSta.Checked)
            {
                DDLSTA.Enabled = true;
                this.SetStaDatasource();
            }
            else
            {
                this.DDLSTA.Items.Clear();
                DDLSTA.Enabled = false;
            }
        }

        protected void ChkWctr_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWctr.Checked)
            {
                DDLCTR.Enabled = true;
                this.SetWctrDatasource();
            }
            else
            {
                this.DDLCTR.Items.Clear();
                DDLCTR.Enabled = false;
            }

        }
        DBConn dbConn = new DBConn();

        private void SetWctrDatasource()
        {
            string strsql = @"select workctr+'  '+workname as workdesc from mwcm";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find WorkStation！');</script>");
            }
            else
            {
                this.DDLCTR.DataSource = dt;
                this.DDLCTR.DataTextField = "workdesc";
                this.DDLCTR.DataValueField = "workdesc";
                this.DDLCTR.DataBind();
            }
        }
        private void SetSubDatasource()
        {
            string strsql;
            if (ChkWctr.Checked == false)
            {
                strsql = @"select SUBLINEID AS SUB from MSLM";
                //DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            }
            else
            //string strsqla=DDLCTR.Text.Trim ().Substring(0,4);
            {
                strsql = @"select SUBLINEID AS SUB from MSLM where workctr='" + DDLCTR.Text.Trim().Substring(0, 4) + "'";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到相應站點所對應的線別！');</script>");
                ChkSub.Checked = false;
            }
            else
            {
                this.DDLSUB.DataSource = dt;
                this.DDLSUB.DataTextField = "SUB";
                this.DDLSUB.DataValueField = "SUB";
                this.DDLSUB.DataBind();
            }
        }

        private void SetStaDatasource()
        {
            string strsql = @"select EQCD+'  '+EQDES as status from mecm";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Status！');</script>");
            }
            else
            {
                this.DDLSTA.DataSource = dt;
                this.DDLSTA.DataTextField = "status";
                this.DDLSTA.DataValueField = "status";
                this.DDLSTA.DataBind();
            }
        }

        protected void ButQry_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strErrorMsg = "";
            this.lblErr.Text = "";
            StringBuilder stb = new StringBuilder();
            DataTable dt = new DataTable();
            bool Err = false;
            //stb.Append("select a.*,b.eqdes from (select transdate,workctr,sublineid,shopcd,sublinestatus from esls where eqccd='P' union select transdate,workctr,sublineid,shopcd,sublinestatus from eslsh where eqccd='P') a left join mecm b on a.sublinestatus=b.eqcd");
            //if (txtFromTime.Text.Trim().Replace("'", "") == "")
            //{
            //    strErrorMsg += "開始時間為空！\n";
            //    // this.lblErr.Text = strErrorMsg;
            //    //return;
            //    Err = true;
            //}
            //else
            //{
            stb.Append("select a.*,b.eqdes from (select transdate,workctr,sublineid,shopcd,sublinestatus from New_ESLS");
            stb.Append(" where transdate between '" + txtFromTime.Text.Trim() + "'");
            stb.Append(" and '" + txtToTime.Text.Trim() + "'");
            stb.Append("union");
            stb.Append(" select transdate,workctr,sublineid,shopcd,sublinestatus from New_ESLSH");
            stb.Append(" where transdate between '" + txtFromTime.Text.Trim() + "'");
            stb.Append(" and '" + txtToTime.Text.Trim() + "'");
            stb.Append(")a left join mecm b on a.sublinestatus=b.eqcd where 1=1");

            //}
            //if (txtToTime.Text.Trim().Replace("'", "") == "")
            //{
            //    strErrorMsg += "結束時間為空！\n";
            //    //this.lblErr.Text = strErrorMsg;
            //    //return;
            //    Err = true;
            //}
            //else
            //{
            //stb.Append(" and '" + txtToTime.Text.Trim() + "'");
            //}
            if (this.ChkWctr.Checked)
            {
                if (DDLCTR.Text.Trim().Replace("'", "") == "")
                {
                    //stb.Append("select a.*,b.eqdes from (select transdate,workctr,sublineid,shopcd,sublinestatus from esls where eqccd='P' union select transdate,workctr,sublineid,shopcd,sublinestatus from eslsh where eqccd='P') a left join mecm b on a.sublinestatus=b.eqcd");
                    stb.Append("");
                }
                else
                {
                    stb.Append(" and workctr='" + DDLCTR.Text.Trim().Substring(0, 4) + "'");
                }
            }
            if (this.ChkSub.Checked)
            {
                if (ChkSub.Text.Trim().Replace("'", "") == "")
                {
                    //stb.Append("select a.*,b.eqdes from (select transdate,workctr,sublineid,shopcd,sublinestatus from esls where eqccd='P' union select transdate,workctr,sublineid,shopcd,sublinestatus from eslsh where eqccd='P') a left join mecm b on a.sublinestatus=b.eqcd");
                    //this.lblErr.Text = strErrorMsg;
                    //return;.
                    stb.Append("");
                    //Err = true;
                }
                else
                {
                    stb.Append(" and sublineid='" + DDLSUB.Text.Trim()+ "'");
                }
            }
            if (ChkSta.Checked)
            {
                if (DDLSTA.Text.Trim().Replace("'", "") == "")
                {
                    //stb.Append("select a.*,b.eqdes from (select transdate,workctr,sublineid,shopcd,sublinestatus from esls where eqccd='P' union select transdate,workctr,sublineid,shopcd,sublinestatus from eslsh where eqccd='P') a left join mecm b on a.sublinestatus=b.eqcd");
                    //this.lblErr.Text = strErrorMsg;
                    //return;
                    //Err = true;
                    stb.Append("");
                }
                else
                {
                    stb.Append(" and sublinestatus='" + DDLSTA.Text.Trim().Substring(0, 5) + "'");
                }
            }
            if (ChkWctr.Checked == false && ChkSub.Checked == false && ChkSta.Checked == false)
            {
                //stb.Append("select a.*,b.eqdes from (select transdate,workctr,sublineid,shopcd,sublinestatus from esls where eqccd='P' union select transdate,workctr,sublineid,shopcd,sublinestatus from eslsh where eqccd='P') a left join mecm b on a.sublinestatus=b.eqcd");
                //stb.Append(" where transdate between '" + txtFromTime.Text.Trim() + "'");
                //stb.Append(" and '" + txtToTime.Text.Trim() + "'");
                //Err = true;
                stb.Append("");
            }

            if (Err)
            {
                this.lblErr.Text = strErrorMsg;
            }
            else
            {
                stb.Append(" order by transDATE");
                dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    this.lblErr.Text = strErrorMsg;
                }
            }
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\MachineRunHistory.rdlc";



            ReportDataSource rsd = new ReportDataSource("MachineRunHistory_DataTable1", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);

        }

    }
}
