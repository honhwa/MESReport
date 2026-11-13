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

namespace OEE_REPORT
{
    public partial class _Default : System.Web.UI.Page
    {
        DBConn dbconn = new DBConn();
        static DataSet dsquery ;    
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
                
                strsql = "";
                strsql = " select '' as process,'' as eq_id,'' as eq_type union select  case process when 'COG1' THEN 'COG' ELSE PROCESS END AS  process, CASE EQ_ID WHEN 'AMSB06' THEN 'AMCOG050'  WHEN 'AMSB01' THEN 'AMCOG010'";
                strsql = strsql + " WHEN 'AMSB02' THEN 'AMCOG020' WHEN 'AMSB03' THEN 'AMCOG030' WHEN 'AMSB04' THEN 'AMCOG040' WHEN 'AMSB16' THEN 'AMCOG150'  ELSE eq_id END AS EQ_ID,eq_type as col from MEQTGTM;";
                strsql = strsql + "select convert(nvarchar(30),dateadd(day,-1,getdate()),120) as strfr , convert(nvarchar(30),getdate(),120) as strto;select ID from msumu where typeid='OEEP';";
                 dsquery = dbconn.GetDataSetBySql(strsql);
                if (dsquery == null)
                {
                    this.lblerror.Text = "綁定控件異常!!";
                }
                else
                {
                    DataTable dstable = dsquery.Tables[0].DefaultView.ToTable(true, "process");
                    ddlpro.DataValueField = "process";
                    ddlpro.DataTextField  = "process";
                    ddlpro.DataSource  = dstable.DefaultView   ;
                    ddlpro.DataBind();                    
                    ddlpro.SelectedValue = "";
                    ddlfactory.DataTextField = "ID";
                    ddlfactory .DataValueField ="ID";
                    ddlfactory.DataSource = dsquery.Tables[2].DefaultView;
                    ddlfactory.DataBind();

                }
            }
            catch(Exception ex)
            {
                this.lblerror.Text = ex.Message;
            }
            finally
            {               
                
                GC.Collect();
            }
        }

        protected void cmdquery_Click(object sender, EventArgs e) //查詢結果，LOADING INTO REPORTVIEWER
        {
            DataSet dsresult;
            string strsql = "";
            string strcon="";
            string strlcm1 = "";
            string strlcm2="";
            string strquery = "";
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
           // SqlParameter[] dspara = new SqlParameter[]{
           //new SqlParameter("@startdt", txtfrom.Text.ToString().Trim()),
           //new SqlParameter("@enddt", txtto.Text.ToString().Trim ()),
           //new SqlParameter("@process", ddlpro.SelectedValue.ToString ().Trim ()),
           //  new SqlParameter("@eq_type", ddltype.SelectedValue.ToString ().Trim ()),
           // new SqlParameter("@eq_id", ddlid.SelectedValue.ToString ().Trim ())};
           // dsresult = dbconn.GetDataSetBySp("OEEQuery", dspara);
            //dsresult = dbconn.GetDataSetBySql("select * from meqtgtm");
            strsql = "SELECT PLANT_LOC_ID AS PLANT,MFG_DT AS DATE,PROCESS,EQ_TYPE,EQUIP_ID AS EQ_ID,MFG_DT AS DT_TIME ,TGT_MOVE_PROD_QTY AS NORMAL_TGT,";
            strsql = strsql +"NORMALQTY AS NORMAL_ACT,ENGQTY AS ENG_ACT,TGT_UPH_MEAS AS UPH_TGT,UPH_MEAS AS UPH_ACT,OFFTIME_MEAS AS OFF_TTL";
            strsql = strsql +",MRUN_MEAS AS M_RUN,ERUN_MEAS AS ERUN,ESETUP_MEAS AS ESETUP,PSETUP_MEAS AS PSETUP,";
            strsql = strsql +"CHG_MTL_MEAS AS CHMT,FAC_IDLE_MEAS AS FAC_IDLE,NONFAC_IDLE_MEAS AS NONFAC_IDLE";
            strsql = strsql +",UP_TIME_MEAS AS UP_TTL,FAC_DOWN_MEAS AS FACEDOWN,REPAIR_MEAS AS REPAIR,PRD_DOWN_MEAS AS PRODOWN";
            strsql = strsql +",ENG_MEAS AS ENG, PM_MEAS AS PM,WARM_UP_MEAS AS WARM_UP,DOWN_TIME_MEAS AS DOWN_TTL,";
            strsql = strsql +"UP_TIME_MEAS+DOWN_TIME_MEAS+OFFTIME_MEAS AS TOTAL_TTL,TGT_UP_RATE AS UPP_TGT,";
            strsql = strsql +"UP_RATE AS UPP_ACT,UP_MOD_RATE AS UPP_MODIFY,TGT_RUN_RATE AS RUNP_TGT,";
            strsql = strsql +"RUN_RATE AS RUNP_ACT,RUN_MOD_RATE AS RUNP_MODIFY,TGT_UPH_RATE AS UPHP_TGT,";
            strsql = strsql +"UPH_RATE AS UPHP_ACT,TGT_PSETUP_RATE AS PSETUPP_TGT,PSETUP_RATE AS PSERUPP_ACT,";
            strsql = strsql +"TGT_ELOT_RATE AS ELOTP_TGT,ELOT_RATE AS ELOTP_ACT,TGT_OEE_RATE AS OEEP_TGT,OEE_RATE AS OEEP_ACT,OEE_MOD_RATE AS OEEP_MODIFY,CASE WHEN TGT_UPH_MEAS=0 THEN '0' ELSE (NORMALQTY * 3600) /TGT_UPH_MEAS END AS TT";
            strsql = strsql +" FROM ";
            strlcm1 = " WEDWM ";
            //strcon=strcon+"  WHERE MFG_DT between '" +  dtfr.datatimer ().ToString () +"' and '" + dtto.datatimer().ToString()  +"'";
            strcon = strcon + "  WHERE MFG_DT between '" + txtStartDT.Text + "' and '" + TxtEndDT.Text + "'";
            if (!ddlpro.SelectedValue.Equals(""))
            {
                strcon = strcon + " and process='" + ddlpro.SelectedValue.ToString().Trim() + "'";
            }
            else
            {
                this.lblerror.Text = "Process 不能为空!!";
                return;
            }
            if (!ddltype.SelectedValue.Equals(""))
            {
                strcon = strcon + " and eq_type='" + ddltype.SelectedValue.ToString().Trim() + "'";
            }
            if (!ddlid.SelectedValue.Equals(""))
            {
                strcon = strcon + " and EQUIP_ID='" + ddlid.SelectedValue.ToString().Trim() + "'";
            }
            //if (ddlfactory.SelectedValue.ToString().ToUpper () == "LCM2")//LCM1
            //{
            //    strquery = strsql + strlcm1 + strcon;
            //}
            //if (ddlfactory.SelectedValue.ToString().ToUpper() == "LCM1")//LCM2
            //{
            //    strquery =  strsql + strlcm2 + strcon;
            //}
            //if (ddlfactory.SelectedValue.ToString().ToUpper() == "LCM1+LCM2") // LCM1+LCM2
            //{
            //    strquery = strsql + strlcm1 + strcon + " union " + strsql + strlcm2 + strcon;
            //}
            strquery = strsql + strlcm1 + strcon;
            dsresult = dbconn.GetDataSetBySql(strquery);
            this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
            if (dsresult.Tables [0].Rows.Count==0)
            {
                this.lblerror.Text = "查無資料!!";
            }
            this.lblerror.Text = "";
            ReportViewer1.LocalReport.ReportPath  = @"RDLC\Report_OEE.rdlc";//指定路径
            ReportViewer1.LocalReport.DataSources.Clear();//清掉原來的資料來源

            this.ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_WEDWM", dsresult.Tables[0]));//塞数据

            this.ReportViewer1.LocalReport.Refresh(); // 刷新控件            

        }

        protected void ddlpro_SelectedIndexChanged(object sender, EventArgs e)//級聯DDLTYPE,DDLID
        {
            DataTable dstable = dsquery.Tables[0];            
            dstable.DefaultView.RowFilter = "process='" + ddlpro.SelectedValue + "'";
            DataTable tabletype =dstable.DefaultView.ToTable(true, "eq_type");
            tabletype.Rows.Add("");
            ddltype.DataValueField = "eq_type";
            ddltype.DataTextField = "eq_type";
            ddltype.DataSource = tabletype.DefaultView;
            ddltype.DataBind();
            ddltype.SelectedValue = "";
            DataTable tableid = dstable.DefaultView.ToTable(true, "eq_id");
            tableid.Rows.Add("");
            ddlid.DataValueField = "eq_id";
            ddlid.DataTextField = "eq_id";
            ddlid.DataSource = tableid.DefaultView;
            ddlid.DataBind();
            ddlid.SelectedValue = "";   
        }
    }
}
