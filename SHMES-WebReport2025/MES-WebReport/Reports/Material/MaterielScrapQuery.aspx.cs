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

namespace WebReport.Reports
{
    public partial class MaterielScrapQuery : System.Web.UI.Page
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
                this.SetddlApplication();
                this.txtFrtime.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd hh:mm:ss");
                this.txtToDatatime.Text = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                //this.Setddlpetype();
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        private void SetddlApplication()
        {
            string strSql = @"SELECT DISTINCT PRODGROUP FROM MPIM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY PRODGROUP";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch ProdId!");
            }
            else
            {
                this.ddlApplication.DataSource = dt;
                this.ddlApplication.DataTextField = "PRODGROUP";
                this.ddlApplication.DataValueField = "PRODGROUP";
                this.ddlApplication.DataBind();
                this.ddlApplication.Items.Insert(0, "ALL");
            }
        }

        private void Setddlpetype()
        {
            //this.ddlpetype.Items.Clear();
            //string strSql = @"SELECT DISTINCT wotype FROM swom WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            //DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            //this.ddlpetype.DataSource = dt;
            //this.ddlpetype.DataTextField = "wotype";
            //this.ddlpetype.DataValueField = "wotype";
            //this.ddlpetype.DataBind();
            //this.ddlpetype.Items.Insert(0, "ALL");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;
            bool bHasError = false;
            //if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
            //{
            //    strErrorMsg += "開始時間為空！\n";
            //    bHasError = true;
            //}

            //if (this.txtToTime.Text.Trim().Replace("'", "") == "")
            //{
            //    strErrorMsg += "結束時間為空！\n";
            //    bHasError = true;
            //}

            if (bHasError)
            {
                this.lblErrMsg.Text = strErrorMsg;
            }
            else
            {
                #region LocalReport DataSource Setting

                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                string strSql = "";
                //strSql = "select RMSDP.*,SWOM.REF2,SWOM.WOTYPE,MMDDM.REASONCDESC,SWOM.WOTYPE AS peType,rmsdp.mtrflg AS L003Flag,rmsdp.packflg AS packFlag  ";
                //strSql = strSql + " FROM RMSDP LEFT JOIN MMDDM  ON rmsdp.REASONCD=MMDDM.REASONCD ";
                //strSql = strSql + " INNER JOIN SWOM ON RMSDP.WONBR=SWOM.WONBR WHERE convert(char(10),ReportDate,120) BETWEEN '" + this.txtFromTime.Text.Trim() + "' AND '";
                //strSql = strSql + this.txtToTime.Text.Trim() + "'";

                strSql = "SELECT Transdate,ReportDate,Reportweek,cworkctr,shopcd,F9NCNO,T9NCNO,Typename,SubPartId,SubPartName,copmflag,compmgroup,wonbr,wotype,lottype,petype,";
                strSql = strSql + " reasoncd,tolocation,REASONCDESC,plant,SUM(ScrapQty) AS ScrapQty,MtrFlg,L003Flag,SubPartType,pnlqty,REF2,ProdSize,packflag,prodgroup,LCFlag FROM (";
                strSql = strSql+" SELECT a.Transdate,CONVERT(VARCHAR(10),a.mfgdate,120) AS ReportDate,a.transweek as Reportweek,a.cworkctr,";
                //strSql = strSql + " 'MODULE'AS SHOPCD,CONVERT(INT,d.F9NCNO) AS F9NCNO,CONVERT(INT,D.T9NCNO) AS T9NCNO,c.prodid as Typename,a.partid as  SubPartId,CONVERT(INT,a.partid) as SubPartName,";
                strSql = strSql + " 'MODULE'AS SHOPCD,d.F9NCNO AS F9NCNO,D.T9NCNO AS T9NCNO,c.prodid as Typename,a.partid as  SubPartId,a.partid as SubPartName,";
                strSql = strSql+" 'Material' as COPMFLAG,a.PARTTYPE AS COMPMGROUP,e.wonbr,e.wotype,";
                strSql = strSql + " e.lottype,CASE WHEN e.WOTYPE IN ('B','D','E') THEN 'E' ELSE 'P' END AS PETYPE,A.reasoncd,A.tolocation,f.REASONCDESC,CASE WHEN a.TOWONBR <>'' then 'NP87' ELSE 'NP82' END AS plant, ";
                strSql = strSql + " a.partqty AS ScrapQty,'M' AS MtrFlg,CASE WHEN a.VERB='SFLT' THEN 'H' ELSE 'M' END AS L003Flag,a.PartType AS SubPartType,0 AS pnlqty,e.REF2,c.SizeID AS ProdSize,e.packflg AS packflag, ";
                strSql = strSql + " c.prodgroup,'Y' AS LCFlag ";
                strSql = strSql+ " FROM New_VwpaxA a left join mmddm f on (a.reasoncd = f.reasoncd)";
                strSql = strSql+ "   LEFT JOIN New_SWOM E ON (a.towonbr = e.wonbr) INNER JOIN mpim c ON (e.prodid = c.prodid) INNER JOIN mpm9m d ON (e.prodid = d.prodid) ";
                strSql = strSql + " WHERE a.transdate BETWEEN '" + this.txtFrtime.Text.Trim() + "' AND '" + this.txtToDatatime.Text.Trim() + "' AND a.VERB IN ('WRES','SFLT') ";
                strSql = strSql + " and a.cpartmode='S' and a.Npartmode='S' ";
                strSql = strSql+" UNION ALL" ;
                strSql = strSql+" SELECT  a.Transdate,CONVERT(VARCHAR(10),a.mfgdate,120) AS ReportDate,a.transweek as Reportweek,a.cworkctr,";
                ///strSql = strSql+" 'MODULE'AS SHOPCD,'' AS F9NCNO,'' AS T9NCNO,'' AS Typename,a.partid as  SubPartId,CONVERT(INT,a.partid) as SubPartName,";
                strSql = strSql + " 'MODULE'AS SHOPCD,'' AS F9NCNO,'' AS T9NCNO,'' AS Typename,a.partid as  SubPartId,a.partid as SubPartName,";
                strSql = strSql+" 'Material' as COPMFLAG,a.PARTTYPE AS COMPMGROUP,'' AS wonbr,'' AS wotype,";
                strSql = strSql + " '' AS lottype,'' AS PETYPE,A.reasoncd,A.tolocation,f.REASONCDESC,CASE WHEN a.TOWONBR <>'' then 'NP87' ELSE 'NP82' END AS plant,";
                strSql = strSql+" a.partqty AS ScrapQty,'M' AS MtrFlg,'M' AS L003Flag,a.PartType AS SubPartType,";
                strSql = strSql + " 0 AS pnlqty,'' AS REF2,'' AS ProdSize,'' AS packflag, '' AS prodgroup,'Y' AS LCFlag ";
                strSql = strSql+ " FROM New_vwpaxa a LEFT JOIN MMDDM F ON (a.reasoncd = f.reasoncd) ";
                strSql = strSql + " WHERE  a.transdate BETWEEN '" + this.txtFrtime.Text.Trim() + "' AND '" + this.txtToDatatime.Text.Trim() + "' AND a.VERB IN ('WREC') ";
                strSql = strSql + " AND a.cpartmode='S' and a.Npartmode='S' ";
                strSql = strSql+" UNION ALL" ;
                strSql = strSql+" SELECT a.Transdate,CONVERT(VARCHAR(10),a.mfgdate,120) AS ReportDate,a.transweek as Reportweek,a.cworkctr,";
                //strSql = strSql + " CSHOPCD AS SHOPCD,CONVERT(INT,d.F9NCNO) AS F9NCNO,CONVERT(INT,D.T9NCNO),c.prodid as Typename,a.frcprodid as  SubPartId,CONVERT(INT,a.frc9ncno) as SubPartName,";
                strSql = strSql + " CSHOPCD AS SHOPCD,d.F9NCNO AS F9NCNO,D.T9NCNO,c.prodid as Typename,a.frcprodid as  SubPartId,a.frc9ncno as SubPartName,";
                strSql = strSql+" 'PANEL' as COPMFLAG,'PANEL' AS COMPMGROUP,e.wonbr,e.wotype,";
                strSql = strSql + " e.lottype,CASE WHEN e.WOTYPE IN ('B','D','E') THEN 'E' ELSE 'P' END AS PETYPE,A.reasoncd,A.tolocation,f.REASONCDESC,CASE WHEN e.lottype ='EN' then 'NP87' ELSE 'NP82' END AS plant,"; 
                strSql = strSql+" a.centitytqty AS ScrapQty,'P' AS MtrFlg,'H' AS L003Flag,'PANEL' AS SubPartType,g.pnlqty,e.REF2,c.SizeID AS ProdSize,e.packflg AS packflag,";
                strSql = strSql + "  c.prodgroup,CASE WHEN a.CSHOPCD='CELL2' AND a.cworkctr IN ('8020','8230') THEN 'N' ELSE 'Y' END AS LCFlag ";
                strSql = strSql + " FROM New_wltx a left join mmddm f on a.reasoncd = f.reasoncd ,mpim c,mpm9m d,New_SWOM e,MSPM G ";
                strSql = strSql + " WHERE a.FRprodid = c.prodid AND e.prodid = c.prodid and e.prodid = d.prodid AND a.SOPRODID = G.SOPRODID AND ";
                strSql = strSql + " a.fabid=e.fabid and a.towonbr=e.wonbr and a.transdate BETWEEN '" + this.txtFrtime.Text.Trim() +"' AND '";
                strSql = strSql+ this.txtToDatatime.Text.Trim()+"' AND a.VERB ='SFLT')X WHERE 1=1 ";

                if (ddlcopmflag.Text.Trim() != "ALL")
                {
                    strSql = strSql + " AND mtrflg='" + ddlcopmflag.Text.Trim() + "'";
                }
                if (DdlShop.Text.Trim() != "ALL")
                {
                    strSql = strSql + " AND SHOPCD='" + DdlShop.Text.Trim() + "'";
                }
                if (ddlpetype.Text.Trim() != "ALL")
                {
                    strSql = strSql + " AND PETYPE = '" + ddlpetype.Text.Trim() + "'";
                }
                if (ddlApplication.Text.Trim() != "ALL")
                {
                    strSql = strSql + " AND ProdGroup='" + ddlApplication.Text.Trim() + "'";
                }

                strSql = strSql + " GROUP BY Transdate,ReportDate,Reportweek,cworkctr,shopcd,F9NCNO,T9NCNO,Typename,SubPartId,SubPartName,copmflag,compmgroup,wonbr,wotype,lottype,petype,";
                strSql = strSql + " reasoncd,tolocation,REASONCDESC,plant,MtrFlg,L003Flag,SubPartType,pnlqty,REF2,ProdSize,packflag,prodgroup,LCFlag ORDER BY copmflag";

                DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];

                this.rvdata.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.rvdata.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\ModuleScrapQuery.rdlc";


                ReportDataSource rsd = new ReportDataSource("ModuleScrap", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);

                #endregion
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

           protected void ddlcopmflag_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
            
    }
}
