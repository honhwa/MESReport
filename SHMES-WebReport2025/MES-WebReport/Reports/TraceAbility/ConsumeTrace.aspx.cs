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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Text;
using Report.Sources;
using System.Data.SqlClient;

namespace WebReport.Reports
{
    public partial class ConsumeTrace : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                //this.txtDate.Text = "";
                //this.txtToDate.Text = "";
                this.TxtLotID.Text = "";
                this.TxtSN.Text = "";
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
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
            if (TxtLotID.Text.Trim().Replace("'", "") == "" && TxtSN.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "LotID為空！\nSN為空！\n";
                this.lblErr.Text = strErrorMsg;
            }
            else
            {
                stb.Append(@"SELECT A.* FROM (select ENTITYID,PARTID,RIGHT(PARTNAME,2) AS PARTTYPE,PARTQTY,CWORKCTR AS WORKCTR,flg='Key'
				from New_WLTM
				UNPIVOT ( PARTID FOR PARTNAME in (PARTID26,PARTIDBL,PARTIDCO,PARTIDFP,PARTIDIC,PARTIDLC,PARTIDLP,PARTIDPB,PARTIDRB
				,PARTIDTC,PARTIDTP,PARTIDUC,PARTIDUP,PARTIDWB,PARTIDPL,PARTIDR1,PARTIDR2,PARTIDR3,PARTIDRF,PARTIDLG,PARTIDWC,PARTIDDF
				,PARTIDLT,PARTIDUT,PARTIDAF,PARTIDDT,PARTIDSI,PARTIDST,PARTIDUV,PARTIDYJ,PARTIDR4,PARTIDHS,PARTIDMC,PARTIDBC,PARTIDLE
                ,PARTIDBF,PARTIDSP,PARTIDUD,PARTIDLD,PARTIDPS,PARTIDLF,PARTIDM1,PARTIDM2,PARTIDM3,PARTIDM4,PARTIDM5,PARTIDM6,PARTIDM7
                ,PARTIDM8,PARTIDM9) ) AS WLTXPvtPARTID
				UNPIVOT ( PARTQTY FOR PARTQTYNAME in (PARTQTY26,PARTQTYBL,PARTQTYCO,PARTQTYFP,PARTQTYIC,PARTQTYLC,PARTQTYLP,PARTQTYPB
				,PARTQTYRB,PARTQTYTC,PARTQTYTP,PARTQTYUC,PARTQTYUP,PARTQTYWB,PARTQTYPL,PARTQTYR1,PARTQTYR2,PARTQTYR3,PARTQTYRF,PARTQTYLG
				,PARTQTYWC,PARTQTYDF,PARTQTYLT,PARTQTYUT,PARTQTYAF,PARTQTYDT,PARTQTYSI,PARTQTYST,PARTQTYUV,PARTQTYYJ,PARTQTYR4,PARTQTYHS
                ,PARTQTYMC,PARTQTYBC,PARTQTYLE,PARTQTYBF,PARTQTYSP,PARTQTYUD,PARTQTYLD,PARTQTYPS,PARTQTYLF,PARTQTYM1,PARTQTYM2,PARTQTYM3
                ,PARTQTYM4,PARTQTYM5,PARTQTYM6,PARTQTYM7,PARTQTYM8,PARTQTYM9) ) AS WLTXPvtPARTQTY ");
                stb.Append(" WHERE PARTQTY<>0 AND PARTID<>''AND RIGHT(PARTNAME,2)=RIGHT(PARTQTYNAME,2) ");

                if (TxtLotID.Text.ToString() != "")
                {
                    stb.Append(" and ENTITYID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "'");
                }
                stb.Append(" ) A ");
                stb.Append(" left join wssd  B on A.ENTITYID=RTRIM(LTRIM(B.ENTITYID )) WHERE 1=1");

                if (TxtSN.Text.ToString() != "")
                {
                    stb.Append(" and B.CMT='" + this.TxtSN.Text.Trim().Replace("'", "") + "'");
                }
                stb.Append(" union all ");
                stb.Append(" select distinct wltm.ENTITYID,MNKMWSM.PARTID,MNKMWSM.partname as PARTTYPE, MNKMWSM.PARTQTY*(select CASE WHEN C.entitytqty IS NULL THEN 0 ELSE C.entitytqty END ENTITYTQTY from New_wltm C LEFT JOIN WSSD D ON c.entityid=d.entityid  WHERE 1=1 ");
              
                if (TxtLotID.Text.ToString() != "")
                {
                    stb.Append(" and C.ENTITYID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "'");
                }

                if (TxtSN.Text.ToString() != "")
                {
                    stb.Append(" and D.CMT='" + this.TxtSN.Text.Trim().Replace("'", "") + "'");
                }

                stb.Append(" ) as partqty ,MNKMWSM.WORKCTR,flg='Non-key' from MNKMWSM,New_wltm wltm WHERE wltm.PRODID=MNKMWSM.PRODID ");
                stb.Append(" AND MNKMWSM.PARTQTY<>0 AND MNKMWSM.PARTID<>'' and wltm.ENTITYTQTY<>0 ");

                if (TxtLotID.Text.ToString() != "")
                {
                    stb.Append(" and wltm.ENTITYID='" + this.TxtLotID.Text.Trim().Replace("'", "") + "'");
                }

                if (TxtSN.Text.ToString() != "")
                {
                    stb.Append(" and wltm.ENTITYID IN (SELECT ENTITYID FROM WSSD WHERE CMT='"+this.TxtSN.Text.Trim().Replace("'", "") + "')");
                }



                dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    strErrorMsg += "查無資料！\n";
                    this.lblErr.Text = strErrorMsg;
                }
                //}
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\ConsumeTrace.rdlc";


                ReportDataSource rsd = new ReportDataSource("ConsumeTrace_WLTM", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }
            
        }
    }
}


