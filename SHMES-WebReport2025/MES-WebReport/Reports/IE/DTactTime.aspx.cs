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
using Report.Library;

namespace Report.Reports.IE
{
    public partial class DTactTime : System.Web.UI.Page
    {
        DBConn dbconn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack) //首次loading page
            {
                bindcon();
            }
        }
        private void bindcon()// 綁定控件
        {
            try
            {
                string strsql;
                DataSet dsquery = new DataSet();
                strsql = "";
                strsql = "select distinct plant ,process from mtactm;";
                strsql = strsql + " select distinct sublineid from mslm;select distinct project_name as prodid from mpim";

                dsquery = dbconn.GetDataSetBySql(strsql);
                if (dsquery == null)
                {
                    this.lblerror.Text = "綁定控件異常!!";
                }
                else
                {
                    this.lblerror.Text = "";
                    DataTable dstable = new DataTable();
                    dstable = dsquery.Tables[0].DefaultView.ToTable(true, "plant");
                    ddlfactory.DataValueField = "plant";
                    ddlfactory.DataTextField = "plant";
                    ddlfactory.DataSource = dstable.DefaultView;
                    ddlfactory.DataBind();
                    ddlfactory.Items.Insert(0, new ListItem("", ""));
                    ddlfactory.SelectedValue = "";

                    dstable = dsquery.Tables[0].DefaultView.ToTable(true, "process");
                    ddlpro.DataValueField = "process";
                    ddlpro.DataTextField = "process";
                    ddlpro.DataSource = dstable.DefaultView;
                    ddlpro.DataBind();
                    ddlpro.Items.Insert(0, new ListItem("", ""));
                    ddlpro.SelectedValue = "";

                    ddlline.DataTextField = "sublineid";
                    ddlline.DataValueField = "sublineid";
                    ddlline.DataSource = dsquery.Tables[1].DefaultView;
                    ddlline.DataBind();
                    ddlline.Items.Insert(0, new ListItem("", ""));
                    ddlline.SelectedValue = "";

                    ddlprod.DataTextField = "prodid";
                    ddlprod.DataValueField = "prodid";
                    ddlprod.DataSource = dsquery.Tables[2].DefaultView;
                    ddlprod.DataBind();
                    ddlprod.Items.Insert(0, new ListItem("", ""));
                    ddlprod.SelectedValue = "";

                }
            }
            catch (Exception ex)
            {
                this.lblerror.Text = ex.Message;
            }
            finally
            {

                GC.Collect();
            }
        }

        protected void cmdquery_Click(object sender, EventArgs e)
        {
            DataSet dsresult;
            string strsql = "";

            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            strsql = " DECLARE @STARTDT VARCHAR(30);DECLARE @DATE VARCHAR(30);";
            strsql = strsql + " DECLARE @ENDDT VARCHAR(30);";
            strsql = strsql + " DECLARE @MFG_DT NVARCHAR(20);";
            strsql = strsql + " SET @DATE='" + dtfr.datatimer().ToString() + "'";
            strsql = strsql + " SELECT @MFG_DT=REPLACE(CONVERT(NVARCHAR(10),@DATE,121),'-','/'),";
            strsql = strsql + "  @STARTDT=CONVERT(NVARCHAR(30),@DATE +' 08:00:00',121),";
            strsql = strsql + "    @ENDDT=CONVERT(NVARCHAR(30),CAST(DATEADD(DAY,1,CONVERT(NVARCHAR(10),@DATE,121)) AS DATETIME)+'08:00:00',121);";
            strsql = strsql + "   if object_id('tempdb..#wltx') is not null ";
            strsql = strsql + "   drop table TMP.#wltx ";
            strsql = strsql + "   if object_id('tempdb..#mtactm') is not null ";
            strsql = strsql + "   drop table TMP.#mtactm ";
            strsql = strsql + "   select distinct station,plant,process into #mtactm from mtactm;";
            strsql = strsql + " select row_number() over( order by MFG_DT,PRODUCT,STATION,SUBLINEID,transdate) as ROWID,*";
            strsql = strsql + " into #wltx";
            strsql = strsql + " from (";
            strsql = strsql + " select @MFG_DT as MFG_DT,replace(frprodid,'/','') as PRODUCT,CWORKCTR AS STATION,csublineid AS SUBLINEID, toentityid AS LOTNO,TRANSDATE,nentitytqty AS QTY";
            //strsql=strsql +" plant,station,process,process_unit";
            strsql = strsql + " from New_wltx a,#MTACTM B where a.transdate between @STARTDT and @ENDDT";
            strsql = strsql + " and a.fabid= '" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            strsql = strsql + " and a.verb='mvou' ";
            strsql = strsql + " and a.cworkctr =b.station";
            if (!ddlfactory.SelectedValue.Equals(""))
            {
                strsql = strsql + " and b.plant='" + ddlfactory.SelectedValue.ToString() + "'"; ;
            }
            if (!ddlpro.SelectedValue.Equals(""))
            {
                strsql = strsql + " and b.process='" + ddlpro.SelectedValue.ToString() + "'"; ;
            }
            if (!ddlline.SelectedValue.Equals(""))
            {
                strsql = strsql + " and csublineid='" + ddlline.SelectedValue.ToString().Trim() + "'";
            }
            if (!ddlprod.SelectedValue.Equals(""))
            {
                strsql = strsql + " and trim(replace(frprodid,'/','')) ='" + ddlprod.SelectedValue.ToString().Trim() + "'";
            }
            strsql = strsql + "  ) aa ";
            strsql = strsql + " select * ,round(cast(tact_time as float)/QTY,2) as AVG_TIME";
            strsql = strsql + "  from (";
            strsql = strsql + " select b.*, case when ( b.PRODUCT<>a.PRODUCT or ";
            strsql = strsql + "  b.STATION<>a.STATION or";
            strsql = strsql + " b.SUBLINEID<>a.SUBLINEID ) then '0' else";
            strsql = strsql + " datediff(ss,a.transdate,b.transdate) end  as Tact_Time";
            strsql = strsql + " from #wltx a,#wltx b where b.rowid=a.rowid+1";
            strsql = strsql + " union ";
            strsql = strsql + " select a.* ,'0' as tact_time from #wltx a  where rowid=1";
            strsql = strsql + " ) aa ";
            strsql = strsql + " order by rowid";



            dsresult = dbconn.GetDataSetBySql(strsql);
            if (dsresult.Tables[0].Rows.Count == 0)
            {
                this.lblerror.Text = "查無資料!!";
            }
            else
            {
                this.lblerror.Text = "";
            }
            dgcontent.DataSource = dsresult;
            dgcontent.DataBind();

        }

        protected void cmtexport_Click(object sender, EventArgs e)
        {
            //AppClass Apc = new AppClass();
            //Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnexport_click", "", "");
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            //Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            Response.ContentType = "application/ms-excel";
            //Response.Write("<meta http-equiv=Content-Type content=\"text/html | image/png; charset=BIG5\"/>");
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            this.dgcontent.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
