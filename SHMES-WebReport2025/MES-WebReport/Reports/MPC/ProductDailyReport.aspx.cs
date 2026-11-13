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
using Report.Library;
using System.Text;

namespace WebReport.Reports
{
    public partial class ProductDailyReport : System.Web.UI.Page
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
                this.SetProductIDDataSource();
                this.SetStationDataSource();
                this.chkAll.Checked = false;
                this.chkAllProdID.Checked = false;
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        private void SetStationDataSource()
        {
            this.chkAll.Checked = false;
            this.lstStation.Items.Clear();
            string strSql = @"SELECT WORKCTR,WORKCTR+' '+LEFT(WORKSHORTNAME,10)  AS WORKNAME FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND WORKTYPE IN ('BC','AS','SS','CN','CW','S1','FC','PB','M1','MN','M2','M3','P2') AND WORKCTR<>'8084' AND  SHOPCD='CELL2' ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstStation.DataSource = dt;
            this.lstStation.DataTextField = "WORKNAME";
            this.lstStation.DataValueField = "WORKCTR";
            this.lstStation.DataBind();
            this.lstStation.ClearSelection();
        }

        private void SetProductIDDataSource()
        {
            string strSql = @"SELECT ProdID FROM MPIM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch Product ID!";
            }
            else
            {
                this.lstProductID.DataSource = dt;
                this.lstProductID.DataTextField = "ProdID";
                this.lstProductID.DataValueField = "ProdID";
                this.lstProductID.DataBind();
                this.lstProductID.Items.Insert(0, "");
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked)
            {
                foreach (ListItem i in this.lstStation.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstStation.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkAllProdID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllProdID.Checked)
            {
                foreach (ListItem i in this.lstProductID.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstProductID.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void lstStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstStation.Items)
            {
                if (!i.Selected)
                {
                    this.chkAll.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAll.Checked = true;
            }
        }


        protected void lstProductID_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstProductID.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllProdID.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllProdID.Checked = true;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = "";
            this.lblTargetQty.Text = "";
            if (this.txtFromTime.Text == "")
            {
                this.lblErrMsg.Text = "開始時間為空！\n";
                return;
            }
            string strFromTime = this.txtFromTime.Text;
            DateTime dtFromTime = new DateTime();
            if (!DateTime.TryParse(strFromTime, out dtFromTime))
            {
                this.lblErrMsg.Text = "開始時間為空！\n";
                return;
            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string strSql = string.Empty;
            string strFilter = string.Empty;
            DataSet dsTmp = new DataSet();
            DataTable dtTarger = new DataTable();
            dtTarger.Columns.Add(new DataColumn("Department", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("Workname", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("shift_id", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("CntSubline", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("Personal", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("Worktime", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("PLANQTY", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("T_CNT", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("FinishYield", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("OUTPUTQTY", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("TargetYield", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("actualYeild", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A1", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A1Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A2", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A2Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A3", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A3Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A4", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A4Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A5", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("A5Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S1", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S1Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S2", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S2Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S3", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S3Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S4", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S4Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S5", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("S5Percent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("DSMS", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("DSMSPercent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("BS", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("BSPercent", typeof(string)));
            dtTarger.Columns.Add(new DataColumn("CMT", typeof(string)));
            strSql = @"SELECT
                    case when A.operation_id in ('8020','8030','8040','8050','8060','8090') then '1.製造股'when A.operation_id='8080' then '2.測試股' end as Department,
                    case when A.operation_id ='8020' then'1.切割站' when A.operation_id='8040' then '3.液晶站'
                         when A.operation_id='8050'then '4.封口站' when A.operation_id='8060' then'5.R/C清洗'
	                     when A.operation_id='8090' then '6.F/C清洗'when A.operation_id='8080' then 'LOT1' 
	                     when A.operation_id='8030' then '2.VACUUM ANNEAL'end as Workname,
                    A.operation_id ,A.shift_id,B.PlanYield AS TargetYield,
					CASE WHEN ISNULL(C.PlanInputQty,0)=0 THEN 0 ELSE C.PlanInputQty END AS PlanInputQty,
					CASE WHEN ISNULL(C.SubLineQty,0)=0 THEN 0 ELSE C.SubLineQty END AS SubLineQty,
					CASE WHEN ISNULL(C.MANPOWER,0)=0 THEN 0 ELSE C.MANPOWER END AS MANPOWER,
					CASE WHEN ISNULL(C.WORKINGHOUR,0)=0 THEN 0 ELSE C.WORKINGHOUR END AS WORKINGHOUR,
                    SUM(A.A_CNT+A.R_CNT+A.S_CNT) AS T_CNT,                   
                    SUM(A.AA_CNT) AS OUTPUTQTY,                    
                    SUM(A.AA_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS actualYeild,
                    SUM(A.A1_CNT) AS A1,
                    SUM(A.A1_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS A1Percent,
                    SUM(A.A2_CNT) AS A2,
                    SUM(A.A2_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS A2Percent,
                    SUM(A.A3_CNT) AS A3,
                    SUM(A.A3_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS A3Percent,
                    SUM(ISNULL(A.A4_CNT,0))AS A4,
                    SUM(ISNULL(A.A4_CNT,0))/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS A4Percent,
                    SUM(ISNULL(A.A5_CNT,0)) AS A5,
                    SUM(ISNULL(A.A5_CNT,0))/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS A5Percent,
                    SUM(A.S1_CNT) AS S1,
                    SUM(A.S1_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS S1Percent,
                    SUM(A.S2_CNT) AS S2,
                    SUM(A.S2_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS S2Percent,
                    SUM(A.S3_CNT) AS S3,
                    SUM(A.S3_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS S3Percent,
                    SUM(A.S4_CNT) AS S4,
                    SUM(A.S4_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS S4Percent,
                    SUM(A.S5_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS S5,
                    SUM(A.S5_CNT)/SUM(A.A_CNT+A.R_CNT+A.S_CNT ) AS S5Percent
                    FROM  YIELD_TBL_LIGHTON_PRODUCT A
					LEFT JOIN MWCM B ON A.FAB_ID=B.FABID AND A.OPERATION_ID=B.WORKCTR
					LEFT JOIN  WRCDDM C 
					ON A.FAB_ID=C.FABID AND A.SHIFT_ID=C.SHIFTID
					                      AND A.OPERATION_ID=C.WORKCTR";
            strSql += " AND CONVERT(VARCHAR(10),C.TRANSDATE,120)='" + this.txtFromTime.Text.Substring(0, 10) + "'";
            strSql += " WHERE A.OPERATION_ID IN('8020','8040','8030','8050','8060','8090','8080')";
            strFilter = this.GetFilter(true);
            strSql += strFilter;
            strSql += "  GROUP BY A.OPERATION_ID,A.SHIFT_ID,B.PlanYield,C.PlanInputQty,C.SubLineQty,C.MANPOWER,C.WORKINGHOUR";
            dsTmp = this.dbConn.GetDataSetBySql(strSql);
            String defsql = @"SELECT 
                            CASE 
                            WHEN (A.operation_id ='8080')AND(A.def_code like 'MD%'OR A.def_code like 'MM%'OR A.def_code like 'MV%'OR A.def_code like 'ML%'
                            OR A.def_code='MS07' OR A.def_code='MS38') THEN '點燈' 
                            WHEN  (A.operation_id ='8080')AND(A.def_code like 'MS%' AND A.def_code<>'MS07' AND A.def_code<>'MS38')THEN '外觀' 
                            WHEN  (A.operation_id ='8080') THEN '8080'
                            when A.operation_id ='8020' then'1.切割站' when A.operation_id='8040' then '3.液晶站'
                            when A.operation_id='8050'then '4.封口站' when A.operation_id='8060' then'5.R/C清洗'
                            when A.operation_id='8090' then '6.F/C清洗'when A.operation_id='8030' then '2.VACUUM ANNEAL'
                            end as Workname,
                            A.SHIFT_ID,A.LOT_GRADE,A.LOT_LEVEL ,A.DEF_CODE,SUM(A.DEF_CNT)AS DEFCNT from NJ_TBL_LIGHTON_DEFECT A 
                            where 
                            A.operation_id IN('8020','8040','8030','8050','8060','8090','8080')";
            defsql += strFilter;
            defsql += " GROUP BY A.operation_id,A.SHIFT_ID,A.DEF_CODE,A.LOT_GRADE,A.LOT_LEVEL ORDER BY Workname,A.SHIFT_ID,A.DEF_CODE,A.LOT_GRADE,A.LOT_LEVEL";
            DataSet dsDef = this.dbConn.GetDataSetBySql(defsql);
            DataTable dtSGrade = new DataTable();
            dtSGrade.Columns.Add("WORKNAME");
            dtSGrade.Columns.Add("SHIFT_ID");
            dtSGrade.Columns.Add("LOT_GRADE");
            dtSGrade.Columns.Add("DEF_CODE");
            dtSGrade.Columns.Add("DEFCNT", typeof(Int32));
            string strWorkname = string.Empty;
            string strLotGrade = string.Empty;
            string strShiftID = string.Empty;
            string strDef_Code = string.Empty;
            int intDefcnt = 0;
            //for (int i = 0; dsDef.Tables[0].Select("LOT_GRADE='S'").GetUpperBound(0)>=0 && i < dsDef.Tables[0].Select("LOT_GRADE='S'").GetUpperBound(0) + 2; i++)
            for (int i = 0; dsDef.Tables[0].Select("LOT_GRADE='S'").GetUpperBound(0) >= 0 && i < dsDef.Tables[0].Select("LOT_GRADE='S'").GetUpperBound(0) + 1; i++)
            {
                if (dsDef.Tables[0].Rows[i]["LOT_GRADE"].Equals("S"))
                {
                    if (i == dsDef.Tables[0].Rows.Count || strWorkname.Equals("") || strLotGrade.Equals("") || strShiftID.Equals("") || strDef_Code.Equals("") || !strWorkname.Equals(dsDef.Tables[0].Rows[i]["WORKNAME"].ToString()) || !strLotGrade.Equals(dsDef.Tables[0].Rows[i]["LOT_GRADE"].ToString()) || !strShiftID.Equals(dsDef.Tables[0].Rows[i]["SHIFT_ID"].ToString()) || !strDef_Code.Equals(dsDef.Tables[0].Rows[i]["DEF_CODE"].ToString()))
                    {
                        if (i > 0)
                        {
                            DataRow dr = dtSGrade.NewRow();
                            dr.Table.Columns.Add();
                            dr["WORKNAME"] = strWorkname;
                            dr["SHIFT_ID"] = strShiftID;
                            dr["LOT_GRADE"] = strLotGrade;
                            dr["DEF_CODE"] = strDef_Code;
                            dr["DEFCNT"] = intDefcnt;
                            dtSGrade.Rows.Add(dr);
                        }
                        if (i < dsDef.Tables[0].Rows.Count)
                        {
                            strWorkname = dsDef.Tables[0].Rows[i]["WORKNAME"].ToString();
                            strLotGrade = dsDef.Tables[0].Rows[i]["LOT_GRADE"].ToString();
                            strShiftID = dsDef.Tables[0].Rows[i]["SHIFT_ID"].ToString();
                            strDef_Code = dsDef.Tables[0].Rows[i]["DEF_CODE"].ToString();
                            intDefcnt = Convert.ToInt32(dsDef.Tables[0].Rows[i]["DEFCNT"]);
                        }
                    }
                    else
                    {
                        strWorkname = dsDef.Tables[0].Rows[i]["WORKNAME"].ToString();
                        strLotGrade = dsDef.Tables[0].Rows[i]["LOT_GRADE"].ToString();
                        strShiftID = dsDef.Tables[0].Rows[i]["SHIFT_ID"].ToString();
                        strDef_Code = dsDef.Tables[0].Rows[i]["DEF_CODE"].ToString();
                        intDefcnt = intDefcnt + Convert.ToInt32(dsDef.Tables[0].Rows[i]["DEFCNT"]);
                    }
                }
            }
            foreach (DataRow dr in dsTmp.Tables[0].Rows)
            {
                if (!(dr["OPERATION_ID"].Equals("8080")))
                {
                    DataRow drTarger = dtTarger.NewRow();
                    drTarger["Department"] = dr["Department"];
                    drTarger["Workname"] = dr["Workname"];
                    drTarger["OPERATION_ID"] = dr["OPERATION_ID"];
                    drTarger["shift_id"] = dr["shift_id"];
                    drTarger["T_CNT"] = Convert.ToDouble(dr["T_CNT"]).ToString("0.00");
                    drTarger["OUTPUTQTY"] = Convert.ToDouble(dr["OUTPUTQTY"]).ToString("0.00");
                    drTarger["TargetYield"] = Convert.ToDouble(dr["TargetYield"]).ToString("0.00") + "%";
                    drTarger["actualYeild"] = Convert.ToDouble((Convert.ToDouble(dr["OUTPUTQTY"]) / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drTarger["PLANQTY"] = Convert.ToDouble(dr["PlanInputQty"]).ToString("0.00");
                    if (Convert.ToInt32(dr["PlanInputQty"]) == 0)
                    {
                        drTarger["FinishYield"] = 0;
                    }
                    else
                    {
                        drTarger["FinishYield"] = Convert.ToDouble((Convert.ToDouble(dr["OUTPUTQTY"]) / Convert.ToDouble(dr["PlanInputQty"])) * 100).ToString("0.00") + "%";
                    }
                    drTarger["CntSubline"] = Convert.ToDouble(dr["SubLineQty"]).ToString("0.00");
                    drTarger["Personal"] = Convert.ToDouble(dr["MANPOWER"]).ToString("0.00");
                    drTarger["Worktime"] = Convert.ToDouble(dr["WORKINGHOUR"]).ToString("0.00");
                    drTarger["A1"] = Convert.ToDouble(dr["A1"]).ToString("0.00");
                    drTarger["A1Percent"] = Convert.ToDouble((Convert.ToDouble(dr["A1Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["A2"] = Convert.ToDouble(dr["A2"]).ToString("0.00");
                    drTarger["A2Percent"] = Convert.ToDouble((Convert.ToDouble(dr["A2Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["A3"] = Convert.ToDouble(dr["A3"]).ToString("0.00");
                    drTarger["A3Percent"] = Convert.ToDouble((Convert.ToDouble(dr["A3Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["A4"] = Convert.ToDouble(dr["A4"]).ToString("0.00");
                    drTarger["A4Percent"] = Convert.ToDouble((Convert.ToDouble(dr["A4Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["A5"] = Convert.ToDouble(dr["A5"]).ToString("0.00");
                    drTarger["A5Percent"] = Convert.ToDouble((Convert.ToDouble(dr["A5Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["S1"] = Convert.ToDouble(dr["S1"]).ToString("0.00");
                    drTarger["S1Percent"] = Convert.ToDouble((Convert.ToDouble(dr["S1Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["S2"] = Convert.ToDouble(dr["S2"]).ToString("0.00");
                    drTarger["S2Percent"] = Convert.ToDouble((Convert.ToDouble(dr["S2Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["S3"] = Convert.ToDouble(dr["S3"]).ToString("0.00");
                    drTarger["S3Percent"] = Convert.ToDouble((Convert.ToDouble(dr["S3Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["S4"] = Convert.ToDouble(dr["S4"]).ToString("0.00");
                    drTarger["S4Percent"] = Convert.ToDouble((Convert.ToDouble(dr["S4Percent"]) * 100)).ToString("0.00") + "%";
                    drTarger["S5"] = Convert.ToDouble(dr["S5"]).ToString("0.00");
                    drTarger["S5Percent"] = Convert.ToDouble((Convert.ToDouble(dr["S5Percent"]) * 100)).ToString("0.00") + "%";
                    int i = 0;
                    int dsmsCnt = 0;
                    int bsCnt = 0;
                    double dsmsPercent = 0;
                    double bsPercent = 0;
                    StringBuilder strDefcode = new StringBuilder();
                    foreach (DataRow drDef in dtSGrade.Select("WORKNAME='" + dr["Workname"].ToString() + "' AND SHIFT_ID='" + dr["SHIFT_ID"].ToString() + "' AND LOT_GRADE='S'", "DEFCNT DESC"))
                    {
                        if (drDef["DEF_CODE"].ToString().Substring(0, 2).Equals("DS") || drDef["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                        {
                            dsmsCnt = dsmsCnt + Convert.ToInt32(drDef["DEFCNT"]);
                        }
                        if (drDef["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                        {
                            bsCnt = bsCnt + Convert.ToInt32(drDef["DEFCNT"]);
                        }
                        if (i < 5)
                        {
                            strDefcode.Append(" ").Append(drDef["DEF_CODE"]).Append("*").Append(Convert.ToInt32(drDef["DEFCNT"]).ToString()).Append(" (").Append(Convert.ToDouble((Convert.ToInt32(drDef["DEFCNT"]) / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00")).Append("%").Append(" )");
                        }
                        i = i + 1;
                    }
                    dsmsPercent = (dsmsCnt / Convert.ToDouble(dr["T_CNT"])) * 100;
                    dsmsPercent = double.Parse(dsmsPercent.ToString("0.00"));
                    bsPercent = (bsCnt / Convert.ToDouble(dr["T_CNT"])) * 100;
                    bsPercent = double.Parse(bsPercent.ToString("0.00"));
                    drTarger["DSMS"] = dsmsCnt.ToString();
                    drTarger["DSMSPercent"] = dsmsPercent.ToString() + "%";
                    drTarger["BS"] = bsCnt.ToString();
                    drTarger["BSPercent"] = bsPercent.ToString() + "%";
                    drTarger["CMT"] = strDefcode;
                    dtTarger.Rows.Add(drTarger);
                }
                else
                {
                    Double intPreLot1RS = 0, intNexLot1RS = 0, intPreLot1A1 = 0, intNexLot1A1 = 0, intPreLot1A2 = 0;
                    Double intNexLot1A2 = 0, intPreLot1A3 = 0, intNexLot1A3 = 0, intPreLot1A4 = 0, intNexLot1A4 = 0;
                    Double intPreLot1A5 = 0, intNexLot1A5 = 0, intPreLot1S1 = 0, intNexLot1S1 = 0, intPreLot1S2 = 0;
                    Double intNexLot1S2 = 0, intPreLot1S3 = 0, intNexLot1S3 = 0, intPreLot1S4 = 0, intNexLot1S4 = 0;
                    Double intPreLot1S5 = 0, intNexLot1S5 = 0;
                    foreach (DataRow drLot1Def in dsDef.Tables[0].Select(" WORKNAME IN ('點燈','外觀') AND SHIFT_ID='" + dr["SHIFT_ID"].ToString() + "'", "DEFCNT DESC"))
                    {
                        if (drLot1Def["WORKNAME"].Equals("點燈"))
                        {
                            if (drLot1Def["LOT_GRADE"].Equals("R") || drLot1Def["LOT_GRADE"].Equals("S"))
                            {
                                intPreLot1RS = intPreLot1RS + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A1"))
                            {
                                intPreLot1A1 = intPreLot1A1 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A2"))
                            {
                                intPreLot1A2 = intPreLot1A2 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A3"))
                            {
                                intPreLot1A3 = intPreLot1A3 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A4"))
                            {
                                intPreLot1A4 = intPreLot1A4 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A5"))
                            {
                                intPreLot1A5 = intPreLot1A5 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S1"))
                            {
                                intPreLot1S1 = intPreLot1S1 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S2"))
                            {
                                intPreLot1S2 = intPreLot1S2 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S3"))
                            {
                                intPreLot1S3 = intPreLot1S3 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S4"))
                            {
                                intPreLot1S4 = intPreLot1S4 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S5"))
                            {
                                intPreLot1S5 = intPreLot1S5 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                        }
                        if (drLot1Def["WORKNAME"].Equals("外觀"))
                        {
                            if (drLot1Def["LOT_GRADE"].Equals("R") || drLot1Def["LOT_GRADE"].Equals("S"))
                            {
                                intNexLot1RS = intNexLot1RS + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A1"))
                            {
                                intNexLot1A1 = intNexLot1A1 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A2"))
                            {
                                intNexLot1A2 = intNexLot1A2 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A3"))
                            {
                                intNexLot1A3 = intNexLot1A3 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A4"))
                            {
                                intNexLot1A4 = intNexLot1A4 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("A") && drLot1Def["LOT_LEVEL"].Equals("A5"))
                            {
                                intNexLot1A5 = intNexLot1A5 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S1"))
                            {
                                intNexLot1S1 = intNexLot1S1 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S2"))
                            {
                                intNexLot1S2 = intNexLot1S2 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S3"))
                            {
                                intNexLot1S3 = intNexLot1S3 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S4"))
                            {
                                intNexLot1S4 = intNexLot1S4 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                            if (drLot1Def["LOT_GRADE"].Equals("S") && drLot1Def["LOT_LEVEL"].Equals("S5"))
                            {
                                intNexLot1S5 = intNexLot1S5 + Convert.ToDouble(drLot1Def["DEFCNT"]);
                            }
                        }
                    }
                    DataRow drPreLot1 = dtTarger.NewRow();
                    DataRow drNexLot1 = dtTarger.NewRow();
                    drPreLot1["Department"] = dr["Department"];
                    drNexLot1["Department"] = dr["Department"];
                    drPreLot1["Workname"] = "1.點燈";
                    drNexLot1["Workname"] = "2.外觀";
                    drPreLot1["OPERATION_ID"] = dr["OPERATION_ID"];
                    drNexLot1["OPERATION_ID"] = dr["OPERATION_ID"];
                    drPreLot1["shift_id"] = dr["shift_id"];
                    drNexLot1["shift_id"] = dr["shift_id"];
                    drPreLot1["T_CNT"] = Convert.ToDouble(Convert.ToDouble(dr["T_CNT"])).ToString("0.00");
                    drNexLot1["T_CNT"] = Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS).ToString("0.00");
                    drPreLot1["OUTPUTQTY"] = Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS).ToString("0.00");
                    drNexLot1["OUTPUTQTY"] = Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS - intNexLot1RS).ToString("0.00");
                    drPreLot1["TargetYield"] = Convert.ToDouble(dr["TargetYield"]).ToString("0.00") + "%";
                    drNexLot1["TargetYield"] = Convert.ToDouble(dr["TargetYield"]).ToString("0.00") + "%";
                    drPreLot1["actualYeild"] = Convert.ToDouble((Convert.ToDouble(drPreLot1["OUTPUTQTY"]) / Convert.ToDouble(drPreLot1["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["actualYeild"] = Convert.ToDouble((Convert.ToDouble(drNexLot1["OUTPUTQTY"]) / Convert.ToDouble(drNexLot1["T_CNT"])) * 100).ToString("0.00") + "%";
                    drPreLot1["PLANQTY"] = Convert.ToDouble(dr["PlanInputQty"]).ToString("0.00");
                    drNexLot1["PLANQTY"] = Convert.ToDouble(dr["PlanInputQty"]).ToString("0.00");
                    if (Convert.ToInt32(dr["PlanInputQty"]) == 0)
                    {
                        drPreLot1["FinishYield"] = 0;
                        drNexLot1["FinishYield"] = 0;
                    }
                    else
                    {
                        drPreLot1["FinishYield"] = Convert.ToDouble((Convert.ToDouble(drPreLot1["OUTPUTQTY"]) / Convert.ToDouble(dr["PlanInputQty"])) * 100).ToString("0.00") + "%";
                        drNexLot1["FinishYield"] = Convert.ToDouble((Convert.ToDouble(drNexLot1["OUTPUTQTY"]) / Convert.ToDouble(dr["PlanInputQty"])) * 100).ToString("0.00") + "%";
                    }
                    drPreLot1["CntSubline"] = Convert.ToDouble(dr["SubLineQty"]).ToString("0.00");
                    drNexLot1["CntSubline"] = Convert.ToDouble(dr["SubLineQty"]).ToString("0.00");
                    drPreLot1["Personal"] = Convert.ToDouble(dr["MANPOWER"]).ToString("0.00");
                    drNexLot1["Personal"] = Convert.ToDouble(dr["MANPOWER"]).ToString("0.00");
                    drPreLot1["Worktime"] = Convert.ToDouble(dr["WORKINGHOUR"]).ToString("0.00");
                    drNexLot1["Worktime"] = Convert.ToDouble(dr["WORKINGHOUR"]).ToString("0.00");
                    drPreLot1["A1"] = Convert.ToDouble(intPreLot1A1).ToString("0.00");
                    drNexLot1["A1"] = Convert.ToDouble(intNexLot1A1).ToString("0.00");
                    drPreLot1["A1Percent"] = Convert.ToDouble((Convert.ToDouble(intPreLot1A1) / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["A1Percent"] = Convert.ToDouble((intNexLot1A1 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["A2"] = Convert.ToDouble(intPreLot1A2).ToString("0.00");
                    drNexLot1["A2"] = Convert.ToDouble(intNexLot1A2).ToString("0.00");
                    drPreLot1["A2Percent"] = Convert.ToDouble((intPreLot1A2 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["A2Percent"] = Convert.ToDouble((intNexLot1A2 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["A3"] = Convert.ToDouble(intPreLot1A3).ToString("0.00");
                    drNexLot1["A3"] = Convert.ToDouble(intNexLot1A3).ToString("0.00");
                    drPreLot1["A3Percent"] = Convert.ToDouble((intPreLot1A3 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["A3Percent"] = Convert.ToDouble((intNexLot1A3 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["A4"] = Convert.ToDouble(intPreLot1A4).ToString("0.00");
                    drNexLot1["A4"] = Convert.ToDouble(intNexLot1A4).ToString("0.00");
                    drPreLot1["A4Percent"] = Convert.ToDouble((intPreLot1A4 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["A4Percent"] = Convert.ToDouble((intNexLot1A4 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["A5"] = Convert.ToDouble(intPreLot1A5).ToString("0.00");
                    drNexLot1["A5"] = Convert.ToDouble(intNexLot1A5).ToString("0.00");
                    drPreLot1["A5Percent"] = Convert.ToDouble((intPreLot1A5 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["A5Percent"] = Convert.ToDouble((intNexLot1A5 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["S1"] = Convert.ToDouble(intPreLot1S1).ToString("0.00");
                    drNexLot1["S1"] = Convert.ToDouble(intNexLot1S1).ToString("0.00");
                    drPreLot1["S1Percent"] = Convert.ToDouble((intPreLot1S1 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["S1Percent"] = Convert.ToDouble((intNexLot1S1 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["S2"] = Convert.ToDouble(intPreLot1S2).ToString("0.00");
                    drNexLot1["S2"] = Convert.ToDouble(intNexLot1S2).ToString("0.00");
                    drPreLot1["S2Percent"] = Convert.ToDouble((intPreLot1S2 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["S2Percent"] = Convert.ToDouble((intNexLot1S2 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["S3"] = Convert.ToDouble(intPreLot1S3).ToString("0.00");
                    drNexLot1["S3"] = Convert.ToDouble(intNexLot1S3).ToString("0.00");
                    drPreLot1["S3Percent"] = Convert.ToDouble((intPreLot1S3 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["S3Percent"] = Convert.ToDouble((intNexLot1S3 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["S4"] = Convert.ToDouble(intPreLot1S4).ToString("0.00");
                    drNexLot1["S4"] = Convert.ToDouble(intNexLot1S4).ToString("0.00");
                    drPreLot1["S4Percent"] = Convert.ToDouble((intPreLot1S4 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["S4Percent"] = Convert.ToDouble((intNexLot1S4 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    drPreLot1["S5"] = Convert.ToDouble(intPreLot1S5).ToString("0.00");
                    drNexLot1["S5"] = Convert.ToDouble(intNexLot1S5).ToString("0.00");
                    drPreLot1["S5Percent"] = Convert.ToDouble((intPreLot1S5 / Convert.ToDouble(dr["T_CNT"])) * 100).ToString("0.00") + "%";
                    drNexLot1["S5Percent"] = Convert.ToDouble((intNexLot1S5 / Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS)) * 100).ToString("0.00") + "%";
                    int i = 0;
                    int dsmsCntPre = 0;
                    int bsCntPre = 0;
                    double dsmsPercentPre = 0;
                    double bsPercentPre = 0;
                    StringBuilder strDefcodePre = new StringBuilder();
                    int dsmsCntNex = 0;
                    int bsCntNex = 0;
                    double dsmsPercentNex = 0;
                    double bsPercentNex = 0;
                    StringBuilder strDefcodeNex = new StringBuilder();
                    foreach (DataRow drDefPre in dtSGrade.Select("WORKNAME='點燈' AND SHIFT_ID='" + dr["SHIFT_ID"].ToString() + "' AND LOT_GRADE='S'", "DEFCNT DESC"))
                    {
                        if (drDefPre["DEF_CODE"].ToString().Equals("MS07"))
                        {
                            dsmsCntPre = dsmsCntPre + Convert.ToInt32(drDefPre["DEFCNT"]);
                        }
                        else
                        {
                            bsCntPre = bsCntPre + Convert.ToInt32(drDefPre["DEFCNT"]);
                        }
                        if (i < 5)
                        {
                            strDefcodePre.Append(" ").Append(drDefPre["DEF_CODE"]).Append("*").Append(Convert.ToInt32(drDefPre["DEFCNT"]).ToString()).Append(" (").Append(Convert.ToDouble(((Convert.ToInt32(drDefPre["DEFCNT"])) / (Convert.ToDouble(Convert.ToDouble(dr["T_CNT"])))) * 100).ToString("0.00")).Append("%").Append(" )");
                        }
                        i = i + 1;
                    }
                    dsmsPercentPre = (dsmsCntPre / Convert.ToDouble(drPreLot1["T_CNT"])) * 100;
                    dsmsPercentPre = double.Parse(dsmsPercentPre.ToString("0.00"));
                    bsPercentPre = (bsCntPre / Convert.ToDouble(drPreLot1["T_CNT"])) * 100;
                    bsPercentPre = double.Parse(bsPercentPre.ToString("0.00"));
                    drPreLot1["DSMS"] = dsmsCntPre.ToString();
                    drPreLot1["DSMSPercent"] = dsmsPercentPre.ToString() + "%";
                    drPreLot1["BS"] = bsCntPre.ToString();
                    drPreLot1["BSPercent"] = bsPercentPre.ToString() + "%";
                    drPreLot1["CMT"] = strDefcodePre;
                    dtTarger.Rows.Add(drPreLot1);
                    i = 0;
                    foreach (DataRow drDefNex in dtSGrade.Select("WORKNAME='外觀' AND SHIFT_ID='" + dr["SHIFT_ID"].ToString() + "' AND LOT_GRADE='S'", "DEFCNT DESC"))
                    {
                        if (!drDefNex["DEF_CODE"].ToString().Equals("MS99"))
                        {
                            dsmsCntNex = dsmsCntNex + Convert.ToInt32(drDefNex["DEFCNT"]);
                        }
                        else
                        {
                            bsCntNex = bsCntNex + Convert.ToInt32(drDefNex["DEFCNT"]);
                        }
                        if (i < 5)
                        {
                            strDefcodeNex.Append(" ").Append(drDefNex["DEF_CODE"]).Append("*").Append(Convert.ToInt32(drDefNex["DEFCNT"]).ToString()).Append(" (").Append(Convert.ToDouble((Convert.ToInt32(drDefNex["DEFCNT"]) / (Convert.ToDouble(Convert.ToDouble(dr["T_CNT"]) - intPreLot1RS))) * 100).ToString("0.00")).Append("%").Append(" )");
                        }
                        i = i + 1;
                    }
                    dsmsPercentNex = (dsmsCntNex / Convert.ToDouble(drNexLot1["T_CNT"])) * 100;
                    dsmsPercentNex = double.Parse(dsmsPercentNex.ToString("0.00"));
                    bsPercentNex = (bsCntNex / Convert.ToDouble(drNexLot1["T_CNT"])) * 100;
                    bsPercentNex = double.Parse(bsPercentNex.ToString("0.00"));
                    drNexLot1["DSMS"] = dsmsCntNex.ToString();
                    drNexLot1["DSMSPercent"] = dsmsPercentNex.ToString() + "%";
                    drNexLot1["BS"] = bsCntNex.ToString();
                    drNexLot1["BSPercent"] = bsPercentNex.ToString() + "%";
                    drNexLot1["CMT"] = strDefcodeNex;
                    dtTarger.Rows.Add(drNexLot1);
                }
            }
            if (dtTarger.Rows.Count > 0)
            {
                double dbOutPut = 0, dbTcnt = 0, dbTotalYield = 1;
                double OutPut = 0, Tcnt = 0, SubLineCnt = 0, PersonalCnt = 0, WorkTimeCnt = 0, PlanTcnt = 0;
                String strOperation = string.Empty;
                foreach (DataRow dRow in dtTarger.Select("", "WORKNAME"))
                {
                    if (!strOperation.Equals("") && !dRow["WORKNAME"].ToString().Equals(strOperation))
                    {
                        dbTotalYield = dbTotalYield * (dbOutPut / dbTcnt);
                        dbOutPut = 0;
                        dbTcnt = 0;
                    }
                    strOperation = dRow["WORKNAME"].ToString();
                    //OutPut = OutPut+ Convert.ToDouble(dRow["OUTPUTQTY"]);
                    //Tcnt = Tcnt + Convert.ToDouble(dRow["T_CNT"]);
                    //if (dRow["CntSubline"].Equals(""))
                    //{
                    //    SubLineCnt = SubLineCnt + 0;
                    //}
                    //else
                    //{
                    //    SubLineCnt = SubLineCnt + Convert.ToDouble(dRow["CntSubline"]);
                    //}
                    //if (dRow["Personal"].Equals(""))
                    //{
                    //    PersonalCnt = PersonalCnt + 0;
                    //}
                    //else
                    //{
                    //    PersonalCnt = PersonalCnt + Convert.ToDouble(dRow["Personal"]);
                    //}
                    //if (dRow["Worktime"].Equals(""))
                    //{
                    //    WorkTimeCnt = WorkTimeCnt + 0;
                    //}
                    //else
                    //{
                    //    WorkTimeCnt = WorkTimeCnt + Convert.ToDouble(dRow["Worktime"]);
                    //}
                    //if (dRow["Worktime"].Equals(""))
                    //{
                    //    PlanTcnt = PlanTcnt + 0;
                    //}
                    //else
                    //{
                    //    PlanTcnt = PlanTcnt + Convert.ToDouble(dRow["PLANQTY"]);
                    //}
                    dbOutPut = dbOutPut + Convert.ToDouble(dRow["OUTPUTQTY"]);
                    dbTcnt = dbTcnt + Convert.ToDouble(dRow["T_CNT"]);
                }
                if (!dbOutPut.Equals(0) && !dbTcnt.Equals(0))
                {
                    dbTotalYield = dbTotalYield * (dbOutPut / dbTcnt);
                    dbOutPut = 0;
                    dbTcnt = 0;
                }
                this.lblTargetQty.Text = "直通率為:" + Convert.ToDouble(dbTotalYield * 100).ToString("0.00") + "%";
                //DataRow drTotal = dtTarger.NewRow();
                //dbTotalYield = dbTotalYield * 100;
                //drTotal["Department"] = "直通率";
                //drTotal["Workname"] = "直通率";
                //drTotal["shift_id"] = "直通率";
                //drTotal["CntSubline"] = SubLineCnt;
                //drTotal["OUTPUTQTY"] = Convert.ToDouble(OutPut).ToString("0.00");
                //drTotal["T_CNT"] = Convert.ToDouble(Tcnt).ToString("0.00");
                //drTotal["PLANQTY"] = Convert.ToDouble(PlanTcnt).ToString("0.00");
                //drTotal["CntSubline"] = SubLineCnt;
                //drTotal["Personal"] = PersonalCnt;
                //drTotal["Worktime"] = WorkTimeCnt;
                //drTotal["actualYeild"] = dbTotalYield.ToString("0.00") + "%";
                //dtTarger.Rows.Add(drTotal);
            }

            this.ReportViewerWip.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport1 = ReportViewerWip.LocalReport;
            localReport1.ReportPath = @"RDLC\ProductDailyRepot.rdlc";

            ReportDataSource rds3 = new ReportDataSource();
            rds3.Name = "ProductDailyReport";
            rds3.Value = dtTarger;
            localReport1.DataSources.Clear();
            localReport1.DataSources.Add(rds3);
            this.ReportViewerWip.LocalReport.Refresh();

            //this.GetLot1Defcode();
        }

        private string GetFilter(bool blCurrent)
        {
            string strFilter = string.Empty;
            strFilter += " AND  A.FAB_ID='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            StringBuilder strStation = new StringBuilder();
            if (this.lstStation.Items.Count != 0)
            {
                if (this.lstStation.SelectedIndex > 0)
                {
                    foreach (ListItem li in this.lstStation.Items)
                    {
                        if (li.Selected)
                        {
                            strStation.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strStation.Length > 0)
            {
                strFilter += " AND A.OPERATION_ID IN (" + strStation.ToString().Replace("''", "','") + ") ";
            }
            StringBuilder strProdID = new StringBuilder();
            if (this.lstProductID.Items.Count != 0)
            {
                if (this.lstProductID.SelectedIndex > 0)
                {
                    foreach (ListItem li in this.lstProductID.Items)
                    {
                        if (li.Selected)
                        {
                            strProdID.Append("'").Append(li).Append("'");
                        }
                    }
                }
            }
            if (strProdID.Length > 0)
            {
                strFilter += " AND A.PRODUCT_ID IN (" + strProdID.ToString().Replace("''", "','") + ") ";
            }
            if (blCurrent == true)
            {
                if (!this.txtFromTime.Text.ToString().Equals(""))
                {
                    strFilter += " AND CONVERT(VARCHAR(10),A.REPORTDATE,120)='" + this.txtFromTime.Text.ToString() + "'";
                }
            }
            return strFilter;
        }
        private void GetLot1Defcode()
        {
            DateTime dtFromTime1 = new DateTime();
            DateTime dtToTime1 = new DateTime();
            DateTime dtFromTime2 = new DateTime();
            DateTime dtToTime2 = new DateTime();
            DateTime dtFromTime3 = new DateTime();
            DateTime dtToTime3 = new DateTime();
            DateTime dtFromTimeCurWeek = new DateTime();
            DateTime dtTmp = new DateTime();
            int weekofCur = Report.Library.DateHelper.WeekOfYear(Convert.ToDateTime(this.txtFromTime.Text));
            Report.Library.DateHelper.GetDaysOfWeeks(Int16.Parse(this.txtFromTime.Text.Substring(0, 4)), weekofCur - 3, System.Globalization.CalendarWeekRule.FirstFourDayWeek, out dtFromTime1, out dtToTime1);
            Report.Library.DateHelper.GetDaysOfWeeks(Int16.Parse(this.txtFromTime.Text.Substring(0, 4)), weekofCur - 2, System.Globalization.CalendarWeekRule.FirstFourDayWeek, out dtFromTime2, out dtToTime2);
            Report.Library.DateHelper.GetDaysOfWeeks(Int16.Parse(this.txtFromTime.Text.Substring(0, 4)), weekofCur - 1, System.Globalization.CalendarWeekRule.FirstFourDayWeek, out dtFromTime3, out dtToTime3);
            Report.Library.DateHelper.GetDaysOfWeeks(Int16.Parse(this.txtFromTime.Text.Substring(0, 4)), weekofCur, System.Globalization.CalendarWeekRule.FirstFourDayWeek, out dtFromTimeCurWeek, out dtTmp);
            string strSqlAll = @"  SELECT SUM(A.DEF_CNT) AS DEF_CNT,SUM(A.LOT_T_CNT) AS LOT_T_CNT ,";
            strSqlAll += " A.OPERATION_ID,CONVERT(VARCHAR(10),A.REPORTDATE,120) AS REPORTDATE,A.DEF_CODE,";
            strSqlAll += " A.LOT_GRADE ,A.LOT_LEVEL FROM NJ_TBL_LIGHTON A WHERE 1=1";
            strSqlAll += this.GetFilter(false);
            strSqlAll += " AND CONVERT(VARCHAR(10),A.REPORTDATE,120) >= '" + dtFromTime1.AddDays(-1).ToString("yyyy-MM-dd") + "'AND CONVERT(VARCHAR(10),A.REPORTDATE,120) <= '" + dtTmp.AddDays(-1).ToString("yyyy-MM-dd") + "'";
            strSqlAll += " AND A.OPERATION_ID in ('8080','8050','8020')";
            strSqlAll += " GROUP BY A.OPERATION_ID,A.REPORTDATE,A.DEF_CODE,A.LOT_GRADE,A.LOT_LEVEL ORDER BY A.REPORTDATE DESC";
            DataSet dsTmp = this.dbConn.GetDataSetBySql(strSqlAll);
            DataRow[] drDef1 = dsTmp.Tables[0].Select("(OPERATION_ID='8080')");
            int inputqty1 = 0, defMDqty1 = 0, defMVqty1 = 0, defMLqty1 = 0, defMMqty1 = 0, defMSqty1 = 0;
            int inputqty2 = 0, defMDqty2 = 0, defMVqty2 = 0, defMLqty2 = 0, defMMqty2 = 0, defMSqty2 = 0;
            int inputqty3 = 0, defMDqty3 = 0, defMVqty3 = 0, defMLqty3 = 0, defMMqty3 = 0, defMSqty3 = 0;
            int inputqty4 = 0, defMDqty4 = 0, defMVqty4 = 0, defMLqty4 = 0, defMMqty4 = 0, defMSqty4 = 0;
            int inputqty5 = 0, defMDqty5 = 0, defMVqty5 = 0, defMLqty5 = 0, defMMqty5 = 0, defMSqty5 = 0;
            int inputqty6 = 0, defMDqty6 = 0, defMVqty6 = 0, defMLqty6 = 0, defMMqty6 = 0, defMSqty6 = 0;
            int inputqty7 = 0, defMDqty7 = 0, defMVqty7 = 0, defMLqty7 = 0, defMMqty7 = 0, defMSqty7 = 0;
            int inputqty8 = 0, defMDqty8 = 0, defMVqty8 = 0, defMLqty8 = 0, defMMqty8 = 0, defMSqty8 = 0;
            int inputqty9 = 0, defMDqty9 = 0, defMVqty9 = 0, defMLqty9 = 0, defMMqty9 = 0, defMSqty9 = 0;
            int inputqty10 = 0, defMDqty10 = 0, defMVqty10 = 0, defMLqty10 = 0, defMMqty10 = 0, defMSqty10 = 0;
            foreach (DataRow dr in drDef1)
            {
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()).Date >= dtFromTime1.AddDays(-1).Date && Convert.ToDateTime(dr["REPORTDATE"].ToString()).Date <= dtToTime1.AddDays(-1).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty1 = inputqty1 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty1 = defMDqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty1 = defMVqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty1 = defMSqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty1 = defMLqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty1 = defMMqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()) >= dtFromTime2.AddDays(-1) && Convert.ToDateTime(dr["REPORTDATE"].ToString()) <= dtToTime2.AddDays(-1))
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty2 = inputqty2 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty2 = defMDqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty2 = defMVqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty2 = defMSqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty2 = defMLqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty2 = defMMqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()) >= dtFromTime3.AddDays(-1) && Convert.ToDateTime(dr["REPORTDATE"].ToString()) <= dtToTime3.AddDays(-1))
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty3 = inputqty3 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty3 = defMDqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty3 = defMVqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty3 = defMSqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty3 = defMLqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty3 = defMMqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty4 = inputqty4 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty4 = defMDqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty4 = defMVqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty4 = defMSqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty4 = defMLqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty4 = defMMqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(1).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(1)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty5 = inputqty5 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty5 = defMDqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty5 = defMVqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty5 = defMSqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty5 = defMLqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty5 = defMMqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(2).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(2)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty6 = inputqty6 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty6 = defMDqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty6 = defMVqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty6 = defMSqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty6 = defMLqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty6 = defMMqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(3).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(3)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty7 = inputqty7 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty7 = defMDqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty7 = defMVqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty7 = defMSqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty7 = defMLqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty7 = defMMqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(4).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(4)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty8 = inputqty8 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty8 = defMDqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty8 = defMVqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty8 = defMSqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty8 = defMLqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty8 = defMMqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(5).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(5)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty9 = inputqty9 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty9 = defMDqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty9 = defMVqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty9 = defMSqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty9 = defMLqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty9 = defMMqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(6).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(6)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty10 = inputqty10 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MD"))
                    {
                        defMDqty10 = defMDqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MV"))
                    {
                        defMVqty10 = defMVqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MS"))
                    {
                        defMSqty10 = defMSqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("ML"))
                    {
                        defMLqty10 = defMLqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("MM"))
                    {
                        defMMqty10 = defMMqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
            }
            DataRow[] drDef2 = dsTmp.Tables[0].Select("(OPERATION_ID='8050')");
            int inputqty18050 = 0;
            int defMS07qty1 = 0, defBS40qty1 = 0, defDSqty1 = 0;
            int inputqty28050 = 0;
            int defMS07qty2 = 0, defBS40qty2 = 0, defDSqty2 = 0;
            int inputqty38050 = 0;
            int defMS07qty3 = 0, defBS40qty3 = 0, defDSqty3 = 0;
            int inputqty48050 = 0;
            int defMS07qty4 = 0, defBS40qty4 = 0, defDSqty4 = 0;
            int inputqty58050 = 0;
            int defMS07qty5 = 0, defBS40qty5 = 0, defDSqty5 = 0;
            int inputqty68050 = 0;
            int defMS07qty6 = 0, defBS40qty6 = 0, defDSqty6 = 0;
            int inputqty78050 = 0;
            int defMS07qty7 = 0, defBS40qty7 = 0, defDSqty7 = 0;
            int inputqty88050 = 0;
            int defMS07qty8 = 0, defBS40qty8 = 0, defDSqty8 = 0;
            int inputqty98050 = 0;
            int defMS07qty9 = 0, defBS40qty9 = 0, defDSqty9 = 0;
            int inputqty108050 = 0;
            int defMS07qty10 = 0, defBS40qty10 = 0, defDSqty10 = 0;
            foreach (DataRow dr in drDef2)
            {
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()).Date >= dtFromTime1.AddDays(-1).Date && Convert.ToDateTime(dr["REPORTDATE"].ToString()).Date <= dtToTime1.AddDays(-1).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty18050 = inputqty18050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty1 = defMS07qty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty1 = defBS40qty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty1 = defDSqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()) >= dtFromTime2.AddDays(-1) && Convert.ToDateTime(dr["REPORTDATE"].ToString()) <= dtToTime2.AddDays(-1))
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty28050 = inputqty28050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty2 = defMS07qty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty2 = defBS40qty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty2 = defDSqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()) >= dtFromTime3.AddDays(-1) && Convert.ToDateTime(dr["REPORTDATE"].ToString()) <= dtToTime3.AddDays(-1))
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty38050 = inputqty38050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty3 = defMS07qty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty3 = defBS40qty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty3 = defDSqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty48050 = inputqty48050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty4 = defMS07qty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty4 = defBS40qty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty4 = defDSqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(1).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(1)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty58050 = inputqty58050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty5 = defMS07qty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty5 = defBS40qty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty5 = defDSqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(2).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(2)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty68050 = inputqty68050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty6 = defMS07qty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty6 = defBS40qty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty6 = defDSqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(3).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(3)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty78050 = inputqty78050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty7 = defMS07qty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty7 = defBS40qty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty7 = defDSqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(4).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(4)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty88050 = inputqty88050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty8 = defMS07qty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty8 = defBS40qty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty8 = defDSqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(5).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(5)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty98050 = inputqty98050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty9 = defMS07qty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty9 = defBS40qty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty9 = defDSqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(6).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(6)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty108050 = inputqty108050 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("MS07"))
                    {
                        defMS07qty10 = defMS07qty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS40"))
                    {
                        defBS40qty10 = defBS40qty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defDSqty10 = defDSqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
            }
            DataRow[] drDef3 = dsTmp.Tables[0].Select("(OPERATION_ID='8020')");
            int inputqty18020 = 0;
            int defALLDS15qty1 = 0, defNOALLDS15qty1 = 0, defALLDSqty1 = 0, defALLBSqty1 = 0, defBS42qty1 = 0;
            int inputqty28020 = 0;
            int defALLDS15qty2 = 0, defNOALLDS15qty2 = 0, defALLDSqty2 = 0, defALLBSqty2 = 0, defBS42qty2 = 0;
            int inputqty38020 = 0;
            int defALLDS15qty3 = 0, defNOALLDS15qty3 = 0, defALLDSqty3 = 0, defALLBSqty3 = 0, defBS42qty3 = 0;
            int inputqty48020 = 0;
            int defALLDS15qty4 = 0, defNOALLDS15qty4 = 0, defALLDSqty4 = 0, defALLBSqty4 = 0, defBS42qty4 = 0;
            int inputqty58020 = 0;
            int defALLDS15qty5 = 0, defNOALLDS15qty5 = 0, defALLDSqty5 = 0, defALLBSqty5 = 0, defBS42qty5 = 0;
            int inputqty68020 = 0;
            int defALLDS15qty6 = 0, defNOALLDS15qty6 = 0, defALLDSqty6 = 0, defALLBSqty6 = 0, defBS42qty6 = 0;
            int inputqty78020 = 0;
            int defALLDS15qty7 = 0, defNOALLDS15qty7 = 0, defALLDSqty7 = 0, defALLBSqty7 = 0, defBS42qty7 = 0;
            int inputqty88020 = 0;
            int defALLDS15qty8 = 0, defNOALLDS15qty8 = 0, defALLDSqty8 = 0, defALLBSqty8 = 0, defBS42qty8 = 0;
            int inputqty98020 = 0;
            int defALLDS15qty9 = 0, defNOALLDS15qty9 = 0, defALLDSqty9 = 0, defALLBSqty9 = 0, defBS42qty9 = 0;
            int inputqty108020 = 0;
            int defALLDS15qty10 = 0, defNOALLDS15qty10 = 0, defALLDSqty10 = 0, defALLBSqty10 = 0, defBS42qty10 = 0;
            foreach (DataRow dr in drDef3)
            {
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()).Date >= dtFromTime1.AddDays(-1).Date && Convert.ToDateTime(dr["REPORTDATE"].ToString()).Date <= dtToTime1.AddDays(-1).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty18020 = inputqty18020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty1 = defALLDS15qty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty1 = defNOALLDS15qty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty1 = defALLDSqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty1 = defALLBSqty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty1 = defBS42qty1 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()) >= dtFromTime2.AddDays(-1) && Convert.ToDateTime(dr["REPORTDATE"].ToString()) <= dtToTime2.AddDays(-1))
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty28020 = inputqty28020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty2 = defALLDS15qty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty2 = defNOALLDS15qty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty2 = defALLDSqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty2 = defALLBSqty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty2 = defBS42qty2 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (Convert.ToDateTime(dr["REPORTDATE"].ToString()) >= dtFromTime3.AddDays(-1) && Convert.ToDateTime(dr["REPORTDATE"].ToString()) <= dtToTime3.AddDays(-1))
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty38020 = inputqty38020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty3 = defALLDS15qty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty3 = defNOALLDS15qty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty3 = defALLDSqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty3 = defALLBSqty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty3 = defBS42qty3 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty48020 = inputqty48020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty4 = defALLDS15qty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty4 = defNOALLDS15qty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty4 = defALLDSqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty4 = defALLBSqty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty4 = defBS42qty4 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(1).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(1)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty58020 = inputqty58020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty5 = defALLDS15qty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty5 = defNOALLDS15qty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty5 = defALLDSqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty5 = defALLBSqty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty5 = defBS42qty5 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(2).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(2)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty68020 = inputqty68020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty6 = defALLDS15qty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty6 = defNOALLDS15qty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty6 = defALLDSqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty6 = defALLBSqty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty6 = defBS42qty6 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(3).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(3)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty78020 = inputqty78020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty7 = defALLDS15qty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty7 = defNOALLDS15qty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty7 = defALLDSqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty7 = defALLBSqty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty7 = defBS42qty7 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(4).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(4)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty88020 = inputqty88020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty8 = defALLDS15qty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty8 = defNOALLDS15qty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty8 = defALLDSqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty8 = defALLBSqty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty8 = defBS42qty8 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(5).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(5)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty98020 = inputqty98020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty9 = defALLDS15qty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty9 = defNOALLDS15qty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty9 = defALLDSqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty9 = defALLBSqty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty9 = defBS42qty9 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
                if (dr["REPORTDATE"].ToString().Equals(dtToTime3.AddDays(6).ToString("yyyy-MM-dd")) && Convert.ToDateTime(dtToTime3.AddDays(6)).Date <= Convert.ToDateTime(this.txtFromTime.Text).Date)
                {
                    if (dr["LOT_GRADE"].ToString().Equals("A") || dr["LOT_GRADE"].ToString().Equals("R") || dr["LOT_GRADE"].ToString().Equals("S"))
                    {
                        inputqty108020 = inputqty108020 + Convert.ToInt32(dr["LOT_T_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15"))
                    {
                        defALLDS15qty10 = defALLDS15qty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if ((dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS")) && (dr["DEF_CODE"].ToString().Length >= 4 && !dr["DEF_CODE"].ToString().Substring(0, 4).Equals("DS15")))
                    {
                        defNOALLDS15qty10 = defNOALLDS15qty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("DS"))
                    {
                        defALLDSqty10 = defALLDSqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 2 && dr["DEF_CODE"].ToString().Substring(0, 2).Equals("BS"))
                    {
                        defALLBSqty10 = defALLBSqty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                    if (dr["DEF_CODE"].ToString().Length >= 4 && dr["DEF_CODE"].ToString().Substring(0, 4).Equals("BS42"))
                    {
                        defBS42qty10 = defBS42qty10 + Convert.ToInt32(dr["DEF_CNT"]);
                    }
                }
            }

            string strExcelFormatName = @"../../EXCModule/ProductDailyReport.xls";
            string strSavePathName = @"../../ExportFile/";
            string strSaveFilename = "";
            string strSheetName = "Sheet1";

            lblErrMsg.Text = "";
            BaseExcel baseExcel = new BaseExcel(strExcelFormatName, strSheetName);
            int m = 3;
            baseExcel.ExcelSheet.Cells[m, 2] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 3);
            baseExcel.ExcelSheet.Cells[m, 3] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 2);
            baseExcel.ExcelSheet.Cells[m, 4] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 1);
            baseExcel.ExcelSheet.Cells[m, 5] = "";
            baseExcel.ExcelSheet.Cells[m, 6] = dtToTime3.ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[m, 7] = dtToTime3.AddDays(1).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[m, 8] = dtToTime3.AddDays(2).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[m, 9] = dtToTime3.AddDays(3).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[m, 10] = dtToTime3.AddDays(4).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[m, 11] = dtToTime3.AddDays(5).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[m, 12] = dtToTime3.AddDays(6).ToString("yyyy-MM-dd");

            baseExcel.ExcelSheet.Cells[m + 1, 2] = inputqty1;
            baseExcel.ExcelSheet.Cells[m + 1, 3] = inputqty2;
            baseExcel.ExcelSheet.Cells[m + 1, 4] = inputqty3;
            baseExcel.ExcelSheet.Cells[m + 1, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 1, 6] = inputqty4;
            baseExcel.ExcelSheet.Cells[m + 1, 7] = inputqty5;
            baseExcel.ExcelSheet.Cells[m + 1, 8] = inputqty6;
            baseExcel.ExcelSheet.Cells[m + 1, 9] = inputqty7;
            baseExcel.ExcelSheet.Cells[m + 1, 10] = inputqty8;
            baseExcel.ExcelSheet.Cells[m + 1, 11] = inputqty9;
            baseExcel.ExcelSheet.Cells[m + 1, 12] = inputqty10;

            baseExcel.ExcelSheet.Cells[m + 2, 2] = defMDqty1;
            baseExcel.ExcelSheet.Cells[m + 2, 3] = defMDqty2;
            baseExcel.ExcelSheet.Cells[m + 2, 4] = defMDqty3;
            baseExcel.ExcelSheet.Cells[m + 2, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 2, 6] = defMDqty4;
            baseExcel.ExcelSheet.Cells[m + 2, 7] = defMDqty5;
            baseExcel.ExcelSheet.Cells[m + 2, 8] = defMDqty6;
            baseExcel.ExcelSheet.Cells[m + 2, 9] = defMDqty7;
            baseExcel.ExcelSheet.Cells[m + 2, 10] = defMDqty8;
            baseExcel.ExcelSheet.Cells[m + 2, 11] = defMDqty9;
            baseExcel.ExcelSheet.Cells[m + 2, 12] = defMDqty10;

            baseExcel.ExcelSheet.Cells[m + 3, 2] = (inputqty1 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty1) / inputqty1) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 3] = (inputqty2 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty2) / inputqty2) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 4] = (inputqty3 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty3) / inputqty3) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 3, 6] = (inputqty4 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty4) / inputqty4) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 7] = (inputqty5 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty5) / inputqty5) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 8] = (inputqty6 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty6) / inputqty6) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 9] = (inputqty7 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty7) / inputqty7) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 10] = (inputqty8 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty8) / inputqty8) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 11] = (inputqty9 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty9) / inputqty9) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 3, 12] = (inputqty10 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMDqty10) / inputqty10) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[m + 4, 2] = defMVqty1;
            baseExcel.ExcelSheet.Cells[m + 4, 3] = defMVqty2;
            baseExcel.ExcelSheet.Cells[m + 4, 4] = defMVqty3;
            baseExcel.ExcelSheet.Cells[m + 4, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 4, 6] = defMVqty4;
            baseExcel.ExcelSheet.Cells[m + 4, 7] = defMVqty5;
            baseExcel.ExcelSheet.Cells[m + 4, 8] = defMVqty6;
            baseExcel.ExcelSheet.Cells[m + 4, 9] = defMVqty7;
            baseExcel.ExcelSheet.Cells[m + 4, 10] = defMVqty8;
            baseExcel.ExcelSheet.Cells[m + 4, 11] = defMVqty9;
            baseExcel.ExcelSheet.Cells[m + 4, 12] = defMVqty10;

            baseExcel.ExcelSheet.Cells[m + 5, 2] = (inputqty1 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty1) / inputqty1) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 3] = (inputqty2 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty2) / inputqty2) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 4] = (inputqty3 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty3) / inputqty3) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 5, 6] = (inputqty4 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty4) / inputqty4) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 7] = (inputqty5 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty5) / inputqty5) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 8] = (inputqty6 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty6) / inputqty6) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 9] = (inputqty7 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty7) / inputqty7) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 10] = (inputqty8 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty8) / inputqty8) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 11] = (inputqty9 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty9) / inputqty9) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 5, 12] = (inputqty10 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMVqty10) / inputqty10) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[m + 6, 2] = defMLqty1;
            baseExcel.ExcelSheet.Cells[m + 6, 3] = defMLqty2;
            baseExcel.ExcelSheet.Cells[m + 6, 4] = defMLqty3;
            baseExcel.ExcelSheet.Cells[m + 6, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 6, 6] = defMLqty4;
            baseExcel.ExcelSheet.Cells[m + 6, 7] = defMLqty5;
            baseExcel.ExcelSheet.Cells[m + 6, 8] = defMLqty6;
            baseExcel.ExcelSheet.Cells[m + 6, 9] = defMLqty7;
            baseExcel.ExcelSheet.Cells[m + 6, 10] = defMLqty8;
            baseExcel.ExcelSheet.Cells[m + 6, 11] = defMLqty9;
            baseExcel.ExcelSheet.Cells[m + 6, 12] = defMLqty10;

            baseExcel.ExcelSheet.Cells[m + 7, 2] = (inputqty1 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty1) / inputqty1) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 3] = (inputqty2 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty2) / inputqty2) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 4] = (inputqty3 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty3) / inputqty3) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 7, 6] = (inputqty4 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty4) / inputqty4) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 7] = (inputqty5 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty5) / inputqty5) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 8] = (inputqty6 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty6) / inputqty6) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 9] = (inputqty7 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty7) / inputqty7) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 10] = (inputqty8 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty8) / inputqty8) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 11] = (inputqty9 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty9) / inputqty9) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 7, 12] = (inputqty10 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMLqty10) / inputqty10) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[m + 8, 2] = defMMqty1;
            baseExcel.ExcelSheet.Cells[m + 8, 3] = defMMqty2;
            baseExcel.ExcelSheet.Cells[m + 8, 4] = defMMqty3;
            baseExcel.ExcelSheet.Cells[m + 8, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 8, 6] = defMMqty4;
            baseExcel.ExcelSheet.Cells[m + 8, 7] = defMMqty5;
            baseExcel.ExcelSheet.Cells[m + 8, 8] = defMMqty6;
            baseExcel.ExcelSheet.Cells[m + 8, 9] = defMMqty7;
            baseExcel.ExcelSheet.Cells[m + 8, 10] = defMMqty8;
            baseExcel.ExcelSheet.Cells[m + 8, 11] = defMMqty9;
            baseExcel.ExcelSheet.Cells[m + 8, 12] = defMMqty10;

            baseExcel.ExcelSheet.Cells[m + 9, 2] = (inputqty1 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty1) / inputqty1) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 3] = (inputqty2 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty2) / inputqty2) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 4] = (inputqty3 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty3) / inputqty3) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 9, 6] = (inputqty4 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty4) / inputqty4) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 7] = (inputqty5 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty5) / inputqty5) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 8] = (inputqty6 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty6) / inputqty6) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 9] = (inputqty7 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty7) / inputqty7) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 10] = (inputqty8 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty8) / inputqty8) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 11] = (inputqty9 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty9) / inputqty9) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 9, 12] = (inputqty10 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMMqty10) / inputqty10) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[m + 10, 2] = defMSqty1;
            baseExcel.ExcelSheet.Cells[m + 10, 3] = defMSqty2;
            baseExcel.ExcelSheet.Cells[m + 10, 4] = defMSqty3;
            baseExcel.ExcelSheet.Cells[m + 10, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 10, 6] = defMSqty4;
            baseExcel.ExcelSheet.Cells[m + 10, 7] = defMSqty5;
            baseExcel.ExcelSheet.Cells[m + 10, 8] = defMSqty6;
            baseExcel.ExcelSheet.Cells[m + 10, 9] = defMSqty7;
            baseExcel.ExcelSheet.Cells[m + 10, 10] = defMSqty8;
            baseExcel.ExcelSheet.Cells[m + 10, 11] = defMSqty9;
            baseExcel.ExcelSheet.Cells[m + 10, 12] = defMSqty10;

            baseExcel.ExcelSheet.Cells[m + 11, 2] = (inputqty1 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty1) / inputqty1) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 3] = (inputqty2 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty2) / inputqty2) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 4] = (inputqty3 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty3) / inputqty3) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 5] = "";
            baseExcel.ExcelSheet.Cells[m + 11, 6] = (inputqty4 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty4) / inputqty4) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 7] = (inputqty5 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty5) / inputqty5) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 8] = (inputqty6 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty6) / inputqty6) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 9] = (inputqty7 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty7) / inputqty7) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 10] = (inputqty8 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty8) / inputqty8) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 11] = (inputqty9 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty9) / inputqty9) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[m + 11, 12] = (inputqty10 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMSqty10) / inputqty10) * 100).ToString("0.00") + "%";

            int s = 45;
            baseExcel.ExcelSheet.Cells[s, 2] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 3);
            baseExcel.ExcelSheet.Cells[s, 3] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 2);
            baseExcel.ExcelSheet.Cells[s, 4] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 1);
            baseExcel.ExcelSheet.Cells[s, 5] = "";
            baseExcel.ExcelSheet.Cells[s, 6] = dtToTime3.ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[s, 7] = dtToTime3.AddDays(1).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[s, 8] = dtToTime3.AddDays(2).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[s, 9] = dtToTime3.AddDays(3).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[s, 10] = dtToTime3.AddDays(4).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[s, 11] = dtToTime3.AddDays(5).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[s, 12] = dtToTime3.AddDays(6).ToString("yyyy-MM-dd");

            baseExcel.ExcelSheet.Cells[s + 1, 2] = inputqty18050;
            baseExcel.ExcelSheet.Cells[s + 1, 3] = inputqty28050;
            baseExcel.ExcelSheet.Cells[s + 1, 4] = inputqty38050;
            baseExcel.ExcelSheet.Cells[s + 1, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 1, 6] = inputqty48050;
            baseExcel.ExcelSheet.Cells[s + 1, 7] = inputqty58050;
            baseExcel.ExcelSheet.Cells[s + 1, 8] = inputqty68050;
            baseExcel.ExcelSheet.Cells[s + 1, 9] = inputqty78050;
            baseExcel.ExcelSheet.Cells[s + 1, 10] = inputqty88050;
            baseExcel.ExcelSheet.Cells[s + 1, 11] = inputqty98050;
            baseExcel.ExcelSheet.Cells[s + 1, 12] = inputqty108050;

            baseExcel.ExcelSheet.Cells[s + 2, 2] = defMS07qty1;
            baseExcel.ExcelSheet.Cells[s + 2, 3] = defMS07qty2;
            baseExcel.ExcelSheet.Cells[s + 2, 4] = defMS07qty3;
            baseExcel.ExcelSheet.Cells[s + 2, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 2, 6] = defMS07qty4;
            baseExcel.ExcelSheet.Cells[s + 2, 7] = defMS07qty5;
            baseExcel.ExcelSheet.Cells[s + 2, 8] = defMS07qty6;
            baseExcel.ExcelSheet.Cells[s + 2, 9] = defMS07qty7;
            baseExcel.ExcelSheet.Cells[s + 2, 10] = defMS07qty8;
            baseExcel.ExcelSheet.Cells[s + 2, 11] = defMS07qty9;
            baseExcel.ExcelSheet.Cells[s + 2, 12] = defMS07qty10;

            baseExcel.ExcelSheet.Cells[s + 3, 2] = (inputqty18050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty1) / inputqty18050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 3] = (inputqty28050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty2) / inputqty28050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 4] = (inputqty38050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty3) / inputqty38050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 3, 6] = (inputqty48050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty4) / inputqty48050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 7] = (inputqty58050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty5) / inputqty58050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 8] = (inputqty68050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty6) / inputqty68050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 9] = (inputqty78050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty7) / inputqty78050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 10] = (inputqty88050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty8) / inputqty88050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 11] = (inputqty98050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty9) / inputqty98050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 3, 12] = (inputqty108050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defMS07qty10) / inputqty108050) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[s + 4, 2] = defBS40qty1;
            baseExcel.ExcelSheet.Cells[s + 4, 3] = defBS40qty2;
            baseExcel.ExcelSheet.Cells[s + 4, 4] = defBS40qty3;
            baseExcel.ExcelSheet.Cells[s + 4, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 4, 6] = defBS40qty4;
            baseExcel.ExcelSheet.Cells[s + 4, 7] = defBS40qty5;
            baseExcel.ExcelSheet.Cells[s + 4, 8] = defBS40qty6;
            baseExcel.ExcelSheet.Cells[s + 4, 9] = defBS40qty7;
            baseExcel.ExcelSheet.Cells[s + 4, 10] = defBS40qty8;
            baseExcel.ExcelSheet.Cells[s + 4, 11] = defBS40qty9;
            baseExcel.ExcelSheet.Cells[s + 4, 12] = defBS40qty10;

            baseExcel.ExcelSheet.Cells[s + 5, 2] = (inputqty18050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty1) / inputqty18050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 3] = (inputqty28050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty2) / inputqty28050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 4] = (inputqty38050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty3) / inputqty38050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 5, 6] = (inputqty48050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty4) / inputqty48050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 7] = (inputqty58050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty5) / inputqty58050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 8] = (inputqty68050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty6) / inputqty68050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 9] = (inputqty78050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty7) / inputqty78050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 10] = (inputqty88050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty8) / inputqty88050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 11] = (inputqty98050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty9) / inputqty98050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 5, 12] = (inputqty108050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS40qty10) / inputqty108050) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[s + 6, 2] = defDSqty1;
            baseExcel.ExcelSheet.Cells[s + 6, 3] = defDSqty2;
            baseExcel.ExcelSheet.Cells[s + 6, 4] = defDSqty3;
            baseExcel.ExcelSheet.Cells[s + 6, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 6, 6] = defDSqty4;
            baseExcel.ExcelSheet.Cells[s + 6, 7] = defDSqty5;
            baseExcel.ExcelSheet.Cells[s + 6, 8] = defDSqty6;
            baseExcel.ExcelSheet.Cells[s + 6, 9] = defDSqty7;
            baseExcel.ExcelSheet.Cells[s + 6, 10] = defDSqty8;
            baseExcel.ExcelSheet.Cells[s + 6, 11] = defDSqty9;
            baseExcel.ExcelSheet.Cells[s + 6, 12] = defDSqty10;

            baseExcel.ExcelSheet.Cells[s + 7, 2] = (inputqty18050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty1) / inputqty18050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 3] = (inputqty28050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty2) / inputqty28050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 4] = (inputqty38050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty3) / inputqty38050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 5] = "";
            baseExcel.ExcelSheet.Cells[s + 7, 6] = (inputqty48050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty4) / inputqty48050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 7] = (inputqty58050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty5) / inputqty58050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 8] = (inputqty68050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty6) / inputqty68050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 9] = (inputqty78050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty7) / inputqty78050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 10] = (inputqty88050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty8) / inputqty88050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 11] = (inputqty98050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty9) / inputqty98050) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[s + 7, 12] = (inputqty108050 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defDSqty10) / inputqty108050) * 100).ToString("0.00") + "%";

            int h = 81;
            baseExcel.ExcelSheet.Cells[h, 2] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 3);
            baseExcel.ExcelSheet.Cells[h, 3] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 2);
            baseExcel.ExcelSheet.Cells[h, 4] = "W" + this.txtFromTime.Text.Substring(3, 1) + Convert.ToString(weekofCur - 1);
            baseExcel.ExcelSheet.Cells[h, 5] = "";
            baseExcel.ExcelSheet.Cells[h, 6] = dtToTime3.ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[h, 7] = dtToTime3.AddDays(1).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[h, 8] = dtToTime3.AddDays(2).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[h, 9] = dtToTime3.AddDays(3).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[h, 10] = dtToTime3.AddDays(4).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[h, 11] = dtToTime3.AddDays(5).ToString("yyyy-MM-dd");
            baseExcel.ExcelSheet.Cells[h, 12] = dtToTime3.AddDays(6).ToString("yyyy-MM-dd");

            baseExcel.ExcelSheet.Cells[h + 1, 2] = inputqty18020;
            baseExcel.ExcelSheet.Cells[h + 1, 3] = inputqty28020;
            baseExcel.ExcelSheet.Cells[h + 1, 4] = inputqty38020;
            baseExcel.ExcelSheet.Cells[h + 1, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 1, 6] = inputqty48020;
            baseExcel.ExcelSheet.Cells[h + 1, 7] = inputqty58020;
            baseExcel.ExcelSheet.Cells[h + 1, 8] = inputqty68020;
            baseExcel.ExcelSheet.Cells[h + 1, 9] = inputqty78020;
            baseExcel.ExcelSheet.Cells[h + 1, 10] = inputqty88020;
            baseExcel.ExcelSheet.Cells[h + 1, 11] = inputqty98020;
            baseExcel.ExcelSheet.Cells[h + 1, 12] = inputqty108020;

            baseExcel.ExcelSheet.Cells[h + 2, 2] = defALLDS15qty1;
            baseExcel.ExcelSheet.Cells[h + 2, 3] = defALLDS15qty2;
            baseExcel.ExcelSheet.Cells[h + 2, 4] = defALLDS15qty3;
            baseExcel.ExcelSheet.Cells[h + 2, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 2, 6] = defALLDS15qty4;
            baseExcel.ExcelSheet.Cells[h + 2, 7] = defALLDS15qty5;
            baseExcel.ExcelSheet.Cells[h + 2, 8] = defALLDS15qty6;
            baseExcel.ExcelSheet.Cells[h + 2, 9] = defALLDS15qty7;
            baseExcel.ExcelSheet.Cells[h + 2, 10] = defALLDS15qty8;
            baseExcel.ExcelSheet.Cells[h + 2, 11] = defALLDS15qty9;
            baseExcel.ExcelSheet.Cells[h + 2, 12] = defALLDS15qty10;

            baseExcel.ExcelSheet.Cells[h + 3, 2] = (inputqty18020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty1) / inputqty18020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 3] = (inputqty28020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty2) / inputqty28020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 4] = (inputqty38020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty3) / inputqty38020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 3, 6] = (inputqty48020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty4) / inputqty48020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 7] = (inputqty58020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty5) / inputqty58020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 8] = (inputqty68020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty6) / inputqty68020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 9] = (inputqty78020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty7) / inputqty78020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 10] = (inputqty88020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty8) / inputqty88020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 11] = (inputqty98020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty9) / inputqty98020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 3, 12] = (inputqty108020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDS15qty10) / inputqty108020) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[h + 4, 2] = defNOALLDS15qty1;
            baseExcel.ExcelSheet.Cells[h + 4, 3] = defNOALLDS15qty2;
            baseExcel.ExcelSheet.Cells[h + 4, 4] = defNOALLDS15qty3;
            baseExcel.ExcelSheet.Cells[h + 4, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 4, 6] = defNOALLDS15qty4;
            baseExcel.ExcelSheet.Cells[h + 4, 7] = defNOALLDS15qty5;
            baseExcel.ExcelSheet.Cells[h + 4, 8] = defNOALLDS15qty6;
            baseExcel.ExcelSheet.Cells[h + 4, 9] = defNOALLDS15qty7;
            baseExcel.ExcelSheet.Cells[h + 4, 10] = defNOALLDS15qty8;
            baseExcel.ExcelSheet.Cells[h + 4, 11] = defNOALLDS15qty9;
            baseExcel.ExcelSheet.Cells[h + 4, 12] = defNOALLDS15qty10;

            baseExcel.ExcelSheet.Cells[h + 5, 2] = (inputqty18020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty1) / inputqty18020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 3] = (inputqty28020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty2) / inputqty28020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 4] = (inputqty38020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty3) / inputqty38020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 5, 6] = (inputqty48020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty4) / inputqty48020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 7] = (inputqty58020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty5) / inputqty58020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 8] = (inputqty68020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty6) / inputqty68020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 9] = (inputqty78020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty7) / inputqty78020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 10] = (inputqty88020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty8) / inputqty88020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 11] = (inputqty98020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty9) / inputqty98020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 5, 12] = (inputqty108020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defNOALLDS15qty10) / inputqty108020) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[h + 6, 2] = defALLDSqty1;
            baseExcel.ExcelSheet.Cells[h + 6, 3] = defALLDSqty2;
            baseExcel.ExcelSheet.Cells[h + 6, 4] = defALLDSqty3;
            baseExcel.ExcelSheet.Cells[h + 6, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 6, 6] = defALLDSqty4;
            baseExcel.ExcelSheet.Cells[h + 6, 7] = defALLDSqty5;
            baseExcel.ExcelSheet.Cells[h + 6, 8] = defALLDSqty6;
            baseExcel.ExcelSheet.Cells[h + 6, 9] = defALLDSqty7;
            baseExcel.ExcelSheet.Cells[h + 6, 10] = defALLDSqty8;
            baseExcel.ExcelSheet.Cells[h + 6, 11] = defALLDSqty9;
            baseExcel.ExcelSheet.Cells[h + 6, 12] = defALLDSqty10;

            baseExcel.ExcelSheet.Cells[h + 7, 2] = (inputqty18020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty1) / inputqty18020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 3] = (inputqty28020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty2) / inputqty28020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 4] = (inputqty38020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty3) / inputqty38020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 7, 6] = (inputqty48020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty4) / inputqty48020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 7] = (inputqty58020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty5) / inputqty58020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 8] = (inputqty68020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty6) / inputqty68020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 9] = (inputqty78020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty7) / inputqty78020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 10] = (inputqty88020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty8) / inputqty88020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 11] = (inputqty98020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty9) / inputqty98020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 7, 12] = (inputqty108020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLDSqty10) / inputqty108020) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[h + 8, 2] = defALLBSqty1;
            baseExcel.ExcelSheet.Cells[h + 8, 3] = defALLBSqty2;
            baseExcel.ExcelSheet.Cells[h + 8, 4] = defALLBSqty3;
            baseExcel.ExcelSheet.Cells[h + 8, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 8, 6] = defALLBSqty4;
            baseExcel.ExcelSheet.Cells[h + 8, 7] = defALLBSqty5;
            baseExcel.ExcelSheet.Cells[h + 8, 8] = defALLBSqty6;
            baseExcel.ExcelSheet.Cells[h + 8, 9] = defALLBSqty7;
            baseExcel.ExcelSheet.Cells[h + 8, 10] = defALLBSqty8;
            baseExcel.ExcelSheet.Cells[h + 8, 11] = defALLBSqty9;
            baseExcel.ExcelSheet.Cells[h + 8, 12] = defALLBSqty10;

            baseExcel.ExcelSheet.Cells[h + 9, 2] = (inputqty18020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty1) / inputqty18020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 3] = (inputqty28020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty2) / inputqty28020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 4] = (inputqty38020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty3) / inputqty38020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 9, 6] = (inputqty48020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty4) / inputqty48020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 7] = (inputqty58020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty5) / inputqty58020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 8] = (inputqty68020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty6) / inputqty68020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 9] = (inputqty78020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty7) / inputqty78020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 10] = (inputqty88020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty8) / inputqty88020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 11] = (inputqty98020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty9) / inputqty98020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 9, 12] = (inputqty108020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defALLBSqty10) / inputqty108020) * 100).ToString("0.00") + "%";

            baseExcel.ExcelSheet.Cells[h + 10, 2] = defBS42qty1;
            baseExcel.ExcelSheet.Cells[h + 10, 3] = defBS42qty2;
            baseExcel.ExcelSheet.Cells[h + 10, 4] = defBS42qty3;
            baseExcel.ExcelSheet.Cells[h + 10, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 10, 6] = defBS42qty4;
            baseExcel.ExcelSheet.Cells[h + 10, 7] = defBS42qty5;
            baseExcel.ExcelSheet.Cells[h + 10, 8] = defBS42qty6;
            baseExcel.ExcelSheet.Cells[h + 10, 9] = defBS42qty7;
            baseExcel.ExcelSheet.Cells[h + 10, 10] = defBS42qty8;
            baseExcel.ExcelSheet.Cells[h + 10, 11] = defBS42qty9;
            baseExcel.ExcelSheet.Cells[h + 10, 12] = defBS42qty10;

            baseExcel.ExcelSheet.Cells[h + 11, 2] = (inputqty18020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty1) / inputqty18020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 3] = (inputqty28020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty2) / inputqty28020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 4] = (inputqty38020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty3) / inputqty38020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 5] = "";
            baseExcel.ExcelSheet.Cells[h + 11, 6] = (inputqty48020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty4) / inputqty48020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 7] = (inputqty58020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty5) / inputqty58020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 8] = (inputqty68020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty6) / inputqty68020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 9] = (inputqty78020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty7) / inputqty78020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 10] = (inputqty88020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty8) / inputqty88020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 11] = (inputqty98020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty9) / inputqty98020) * 100).ToString("0.00") + "%";
            baseExcel.ExcelSheet.Cells[h + 11, 12] = (inputqty108020 == 0) ? "0" : Convert.ToDouble((Convert.ToDouble(defBS42qty10) / inputqty108020) * 100).ToString("0.00") + "%";

            strSaveFilename = "ProductDailyReport" + Convert.ToString(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
            strSavePathName = strSavePathName + strSaveFilename;
            if (baseExcel.ExcelSave(strSavePathName + ".xls") == true)
            {

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(baseExcel.ExcelSheet);
                //baseExcel.ExcelSheet = null;
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(baseExcel.ExcelWork);
                //baseExcel.ExcelWork = null;
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(ThisApplication);
                //ThisApplication = null;
                baseExcel.ExcelClose();
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                baseExcel.DeleteExcelFile("ProductDailyReport");
                this.Response.Write("<script language=javascript>window.open('../LoadExcel.aspx?ExcelName=" + strSaveFilename + "');</script>");
                //this.lblErrMsg.Text = "匯出:" + strSavePathName + ".xls 成功!";
                //Excel.Application xApp = new Excel.ApplicationClass();
                //Excel.Workbook xBook = xApp.Workbooks._Open(strSavePathName,);             
            }
            else
            {
                lblErrMsg.Text = "Save Excel Error";
            }
        }

        protected void cmdExportDef_Click(object sender, EventArgs e)
        {
            this.GetLot1Defcode();
        }
    }
}
