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
using System.Data.SqlClient;
using System.Text;
using Microsoft.Reporting.WebForms;
using Report.Library;
using Report.Sources;

namespace WebReport.Reports
{
    public partial class LotWIP: System.Web.UI.Page
    {
        DBConn dbcon = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.BindProdID();
                this.Bindstatus();
                this.Bindworkctr();
                this.Bindgrade();
                this.Bindwonbr();
            }
        }

        private void BindProdID()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
            
                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY PRODID";

                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.DlProdID.DataSource = null;
                    return;
                }
                this.DlProdID.DataSource = dt;
                this.DlProdID.DataTextField = "PRODID";
                this.DlProdID.DataValueField = "PRODID";
                this.DlProdID.DataBind();
                this.DlProdID.Items.Insert(0, "ALL");
                this.DlProdID.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Bindstatus()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {

                strSql = " SELECT ENTITYSTS,ENTITYSTS+'   '+ENTITYSTSDESC AS ENTITY  FROM MLTSU  WHERE    ENTITYSTS IS NOT NULL  AND ENTITYSTS IN ('C','H','P','R','W')  ORDER  BY ENTITYSTS";
                
                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.Dlstatus.DataSource = null;
                    return;
                }
                this.Dlstatus.DataSource = dt;
                this.Dlstatus.DataTextField = "ENTITY";
                this.Dlstatus.DataValueField = "ENTITYSTS";
                this.Dlstatus.DataBind();
                this.Dlstatus.Items.Insert(0, "ALL");
                this.Dlstatus.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Bindworkctr()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {

                strSql = " select workctr,workctr+'   '+workname AS workctrdesc from mwcm WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY workctr";

                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.Dlworkctr.DataSource = null;
                    return;
                }
                this.Dlworkctr.DataSource = dt;
                this.Dlworkctr.DataTextField = "workctrdesc";
                this.Dlworkctr.DataValueField = "workctr";
                this.Dlworkctr.DataBind();
                this.Dlworkctr.Items.Insert(0, "ALL");
                this.Dlworkctr.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Bindgrade()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {

                strSql = " select gradeid,gradeid+'   '+gradedesc AS gradedesc from mgrm WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY gradeid";

                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.Dlgrade.DataSource = null;
                    return;
                }
                this.Dlgrade.DataSource = dt;
                this.Dlgrade.DataTextField = "gradeid";
                this.Dlgrade.DataValueField = "gradeid";
                this.Dlgrade.DataBind();
                this.Dlgrade.Items.Insert(0, "ALL");
                this.Dlgrade.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Bindwonbr()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {

                strSql = " select DISTINCT wonbr from New_WLTM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' and HIDEFLG='0' ORDER BY wonbr";

                dt = this.dbcon.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.Dlwonbr.DataSource = null;
                    return;
                }
                this.Dlwonbr.DataSource = dt;
                this.Dlwonbr.DataTextField = "wonbr";
                this.Dlwonbr.DataValueField = "wonbr";
                this.Dlwonbr.DataBind();
                this.Dlwonbr.Items.Insert(0, "ALL");
                this.Dlwonbr.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        protected void ButQ_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            ReportViewer1.Visible = true;
            if ((txtLot.Text == string.Empty) && DlProdID.Text==string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('查詢條件不能全為空！');</script>");
                ReportViewer1.Visible = false;
            }
            else
            {   
                DataTable dt = new DataTable();
                StringBuilder stb = new StringBuilder();
                string lot;
                lot = txtLot.Text.Trim().Replace("'","");
                lot = "'" + lot + "'";

                stb.Append("select FABID, ENTITYID, ENTITYUNIT, RTGCD, PWORKCTR, CWORKCTR, PENTITYSTS, ENTITYSTS, PLOTGRADE, PLOTLEVEL, LOTGRADE, LOTLEVEL,");
                stb.Append("DEFECTCD, ENTITYTQTY, SubWorkCtr, SubENTITYSTS, HIDEFLG, PRODID, N9NCNO, WOTYPE, WONBR, RMANO, LOTTYPE, LOTPTY, TRANSDATE,");
                stb.Append("PENTITYQTY, PENTITYUNIT, BOMVersion, SOLOTID, SOPRODID, SO9NCNO, SOFAB, SOBANK, SOGRADE, SOLEVEL, SOLOTTYPE, SOPACKFLG,");
                stb.Append("SOBatchID, CPRODID, C9NCNO, WOPSTARTDATE, WOPDUEDATE, ENTITYCARRY, ENTITYPOS, MLOTID, PACKLOTID, PLOTID, RLOTID, LIDTYPE,");
                stb.Append("LARGESN, GROUPID, QANBR, DCARTID, CARTID, BOXID, PALLETID, ReasonCD, STARTDATE, DUEDATE, TRANSID, TRANSSeq, TRANSMonth,");
                stb.Append(" TRANSWeek, TRACKINDATE, TRACKOUTDATE, RunCardID, TeamID, USERID, TrackINTeamID, TRACKINUSERID, SHIFTID, RRTGCD, OPRSEQNO,");
                stb.Append(" MAXOPRSEQ, PSHOPCD, CSHOPCD, PPROCID, CPROCID, CAUCTR, RWKCTR, RWKCNT, HOLDCD, HOLDCTR, FHOLDID, HOLDID, BANKID, ");
                stb.Append(" STORAGEID, PACKFLG, UNITTRACEFLG, SALEORDER, CUSTOMERID, PSubLineID, SubLineID, LineID, EQID, PASSBOXID, SNFLG, USGFLG, AgingID,");
                stb.Append(" ListID, ListType, WOOWNER, LOTOWNER, ReworkFlg, ScrapMode, ToLocation, PostingDate from New_wltm ");
                stb.Append(" where  FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "'and hideflg='0' ");

                if (txtFromDate.Text.Trim().Replace("'", "") == "")
                {
                    //strErrorMsg += "開始時間為空！\n";
                    //Err = true;
                }
                else
                {
                    stb.Append(" and CONVERT(VARCHAR(10),TRANSDATE,120)>='" + this.txtFromDate.Text.Trim().Replace("'", "") + "'");
                }

                if (txtToDate.Text.Trim().Replace("'", "") == "")
                {
                    //strErrorMsg += "結束時間為空！\n";
                    //Err = true;
                }
                else
                {
                    stb.Append(" and CONVERT(VARCHAR(10),TRANSDATE,120)<='" + this.txtToDate.Text.Trim().Replace("'", "") + "'");
                }


                if (this.DlProdID.Text.ToString() != "ALL")
                {
                    stb.Append(" And prodid='" + this.DlProdID.Text.ToString() + "' ");
                }

                if (this.Dlstatus.Text.ToString() != "ALL")
                {
                    stb.Append(" And ENTITYSTS='" + this.Dlstatus.Text.ToString() + "' ");
                }

                if (this.Dlworkctr.Text.ToString() != "ALL")
                {
                    stb.Append(" And CWORKCTR='" + this.Dlworkctr.Text.ToString() + "' ");
                }

                if (this.Dlgrade.Text.ToString() != "ALL")
                {
                    stb.Append(" And LOTGRADE='" + this.Dlgrade.Text.ToString() + "' ");
                }

                if (this.Dlwonbr.Text.ToString() != "ALL")
                {
                    stb.Append(" And wonbr='" + this.Dlwonbr.Text.ToString() + "' ");
                }
                

                if (this.txtLot.Text.ToString() != "")
                {
                    stb.Append("and  entityid =" + lot);
                }

                stb.Append(" order by ENTITYID");

                dt = this.dbcon.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
                }
                ReportViewer1.Visible = true;
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\LotWIP.rdlc";


                ReportDataSource rsd = new ReportDataSource("LotWIP_WLTM", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
                
            }
        }
    }
}
