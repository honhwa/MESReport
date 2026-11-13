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
using Dundas.Charting.WebControl;
using Dundas.Charting;
using System.Drawing;
using System.Text;
/*
 * 2013-03-21  Innolux/scc     V1.00       Add M1 Station
 * 2013-04-24  Innolux/scc     V1.01       Add 808T
 * 2014-05-12  Innolux/Power   V1.02       Add 8180
 * 2014-07-08  Innolux/ssc     V1.03       Add Grade A1,A2,L2,LR,BP,PC,PT,P5
 * 2014-07-15  Innolux/ssc     V1.04       Add Station 808A,808B,8082,8085
 * * 2014-08-14  Innolux/Runy    V1.05       Add LCD Craft篩選條件
 * 2018-12-26   Innolux/ssc    V2.00     ddlStation add (808T)lot1.5  (818T)relot1.5
 * 2019-12-10   LiZiQi            V2.01     ddlStation add (808E)Linecheck
*/
public partial class YMSReport_Cell2AllLightOn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Login"] == null)
        {
            tbFABID.Text = "";
            tbFABName.Text = "";
            Page.Response.Redirect(@"~\Default.aspx");
            return;
        }
        if (!Page.IsPostBack)
        {
            AppClass Apc = new AppClass();
            if (Request.QueryString["FABID"] != null)
            {
                tbFABID.Text=this.Request.QueryString["FABID"].ToString();
                if (tbFABID.Text == "B")
                {
                    tbFABName.Text = "SH";
                }
                string strLoginPCName = "";
                string strLoginPCIP = "";
                string strLoginNTName = "";
                tbLoginNTName.Text = "";
                tbLoginPCIP.Text = "";
                tbLoginPCName.Text = "";

                Apc.GetLoginInfo(out strLoginPCName, out strLoginPCIP, out strLoginNTName);

                tbLoginNTName.Text = strLoginNTName;
                tbLoginPCIP.Text = strLoginPCIP;
                tbLoginPCName.Text = strLoginPCName;
                HyperLink1.NavigateUrl = "~/MainButton.aspx?FABID=" + tbFABID.Text.Trim();
                Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Browse", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Choose Report", tbFABID.Text.ToString(), "");
            }
            else
            {
                Page.Response.Redirect(@"~\Default.aspx");
            }

            if (tbFABName.Text == "")
            {
                Page.Response.Redirect(@"~\Default.aspx");
                return;
            }
            lblFabID.Text = tbFABName.Text;
            lblTTime.Text = "";
            lblTime01.Text = "";
            lblTime02.Text = "";
            lblTime03.Text = "";  //V1.03 ADD
            lblTime04.Text = "";
            lblTime05.Text = "";
            DataBoundStation();
            DataBoundtxtDate();
            ChangeHeight();
            PageTitleBound();
            txtFDate.Attributes.Add("readonly", "readonly");
            txtTDate.Attributes.Add("readonly", "readonly");
            //Page.MaintainScrollPositionOnPostBack = true;   --保持當前滾動條位置

            lblDataRunTime.Text = Apc.QueryDataRunTime(tbFABID.Text).ToString();

            GetSubject();
        }
    }
    protected void GetSubject()
    {
        AppClass Apc = new AppClass();
        /**************************************************************************************************************************/
        string strTitle = "";
        string strCon = "";
        string strDateTime = "";

        if (Apc.QuerySubject("Cell2AllLightOn", "Cell2AllLightOn", out strTitle, out strCon, out strDateTime) == true)
        {
            lblTitle.Text = strTitle.ToString();
            tbCon.Text = strCon.ToString();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>RunFunc();</script>");
        }
        else
        {
            lblTitle.Text = "";
            tbCon.Text = "";
            strDateTime = "";
        }
        /**************************************************************************************************************************/
    }
    protected void DataBoundStation()
    {
        //ddlStation.Items.Add("BatchInspection(8005)");
        //ddlStation.Items.Add("AutoScribe(8015)");
        ddlStation.Items.Add("SemiScribe(8020)");
        ddlStation.Items.Add("LC(8040)");
        ddlStation.Items.Add("封口(8050)");
        ddlStation.Items.Add("RoughClean(8060)");
        ddlStation.Items.Add("LOT1(8080)");
        ddlStation.Items.Add("LOT1.5(808T)");//V2.00
        ddlStation.Items.Add("外觀(8081)");
        ddlStation.Items.Add("Linecheck(808E)");   //V2.01
        ddlStation.Items.Add("LOT1_LR(808A)");             // V1.04 ADD
        ddlStation.Items.Add("外觀_LR(808B)");             // V1.04 ADD
        ddlStation.Items.Add("Cell2Packing1(8082)");      // V1.04 ADD
        ddlStation.Items.Add("Cell2Packing2(8085)");      // V1.04 ADD
        ddlStation.Items.Add("FinalClean(8090)");
        ddlStation.Items.Add("M1(8180)");/*V1.02*/
        ddlStation.Items.Add("Relot1.5(818T)");   //V2.00
    }
    protected void PageTitleBound()
    {
        Page.Title = tbFABName.Text + " BEOL All Light On Yield";
    }
    protected void DataBoundtxtDate()
    {
        txtFDate.Text = System.DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd");
        txtTDate.Text = System.DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd");
    }
    protected void DataBoundWonbr()
    {
        string strSql = "";
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        strSql = "";
        strSql = "SELECT WONBR FROM New_SWOM WHERE FABID = '" + tbFABID.Text + "' AND ISNULL(CLOSEDATE,GETDATE())>= '" + txtFDate.Text + "' ORDER BY WONBR";
        DataSet ds = new DataSet();
        ds = dbConn.GetDataSetBySql(strSql, "");
        cblWonbr.DataSource = ds;
        cblWonbr.DataTextField = "WONBR";
        cblWonbr.DataValueField = "WONBR";
        cblWonbr.DataBind();
        ds.Dispose();
    }
    protected void DataBoundProdID()
    {
        if (ChkMerge.Checked && PProdID.Visible)
        {
            StringBuilder strSql = new StringBuilder();
            AppClass Apc = new AppClass();
            string strSubLineID = "'='";
            string strProdID = "'='";
            string strLotType = "'='";
            string strWonbr = "'='";
            string strLCDProdID = "'='";  //V1.05 Add

            lblTTime.Text = "";
            lblTime01.Text = "";
            lblTime02.Text = "";
            lblTime03.Text = ""; //V1.03 ADD
            lblTime04.Text = "";
            lblTime05.Text = "";
            DateTime dtF = System.DateTime.Now;
            /*--------------------------------------------------------------------------------------*/
            tbOPERATIONID.Text = "";
            tbType.Text = "";
            tbDateType.Text = "";
            tbDiffDate.Text = "0";
            tbCPFlag.Text = "";
            tbFDate.Text = "";
            tbTDate.Text = "";

            lblFabID.Text = tbFABName.Text;

            //dtCYield.Dispose();
            //dtCYield.Clear();

            //dtCYDef.Dispose();
            //dtCYDef.Clear();
            Session["dtCYield"] = null;
            Session["dtCYDef"] = null;

            IBGBYData.Enabled = false;
            cblLevel.Enabled = false;

            ddlByProdID.Items.Clear();
            ddlSDefList.Items.Clear();


            ddlShiftID.SelectedIndex = 0;

            ClearChartBound();
            switch (ddlStation.SelectedItem.Text.ToString())
            {
                case "BatchInspection(8005)": tbOPERATIONID.Text = "8005"; tbType.Text = "BatchInspection(8005)"; break;
                case "AutoScribe(8015)": tbOPERATIONID.Text = "8015"; tbType.Text = "AutoScribe(8015)"; break;
                case "SemiScribe(8020)": tbOPERATIONID.Text = "8020"; tbType.Text = "SemiScribe(8020)"; break;

                case "LC(8040)": tbOPERATIONID.Text = "8040"; tbType.Text = "LC(8040)"; break;
                case "封口(8050)": tbOPERATIONID.Text = "8050"; tbType.Text = "封口(8050)"; break;
                case "RoughClean(8060)": tbOPERATIONID.Text = "8060"; tbType.Text = "RoughClean(8060)"; break;

                case "LOT1(8080)": tbOPERATIONID.Text = "8080"; tbType.Text = "LOT1(8080)"; break;
                case "LOT1.5(808T)": tbOPERATIONID.Text = "808T"; tbType.Text = "LOT1.5(808T)"; break; ////V2.00
                case "外觀(8081)": tbOPERATIONID.Text = "8081"; tbType.Text = "外觀(8081)"; break;

                case "LOT1_LR(808A)": tbOPERATIONID.Text = "808A"; tbType.Text = "LOT1_LR(808A)"; break;                 // V1.04 ADD
                case "外觀_LR(808B)": tbOPERATIONID.Text = "808B"; tbType.Text = "外觀_LR(808B)"; break;                 // V1.04 ADD
                case "Linecheck(808E)": tbOPERATIONID.Text = "808E"; tbType.Text = "Linecheck(808E)"; break;  ////V2.01
                case "Cell2Packing1(8082)": tbOPERATIONID.Text = "8082"; tbType.Text = "Cell2Packing1(8082)"; break;   // V1.04 ADD
                case "Cell2Packing2(8085)": tbOPERATIONID.Text = "8085"; tbType.Text = "Cell2Packing2(8085)"; break;   // V1.04 ADD

                case "FinalClean(8090)": tbOPERATIONID.Text = "8090"; tbType.Text = "FinalClean(8090)"; break;
                case "M1(8180)": tbOPERATIONID.Text = "8180"; tbType.Text = "M1(8180)"; break;/*V1.02*/
                case "Relot1.5(818T)": tbOPERATIONID.Text = "818T"; tbType.Text = "Relot1.5(818T)"; break;  ////V2.00
                default:
                    break;
            }
            if (cblSubLineID.Items.Count > 0)
            {
                foreach (ListItem li in this.cblSubLineID.Items)
                {
                    if (li.Selected)
                    {
                        strSubLineID = strSubLineID + "," + "'" + li.Text + "'";
                    }
                }
            }
            if (cblLotType.Items.Count > 0)
            {
                foreach (ListItem li in this.cblLotType.Items)
                {
                    if (li.Selected)
                    {
                        strLotType = strLotType + "," + "'" + li.Text + "'";
                    }
                }
            }
            if (cblProdID.Items.Count > 0)
            {
                foreach (ListItem li in this.cblProdID.Items)
                {
                    if (li.Selected)
                    {
                        strProdID = strProdID + "," + "'" + li.Text + "'";
                    }
                }
            }
            if (cblWonbr.Items.Count > 0)
            {
                foreach (ListItem li in this.cblWonbr.Items)
                {
                    if (li.Selected)
                    {
                        strWonbr = strWonbr + "," + "'" + li.Text + "'";
                    }
                }
            }
            /*V1.05 Add Start*/
            string strSoFabID = "'='";
            string strGCraft = "'='";
            string strLCDProdSQL = " SELECT ProjName FROM dbo.MSOFABMLCDM WHERE HEADFAB='" + ddlGCraft.SelectedItem.Text + "'";
            if (cblGCraft.Items.Count > 0)
            {
                foreach (ListItem li in cblSoFab.Items)
                {
                    if (li.Selected)
                    {
                        strSoFabID = strSoFabID + "," + "'" + li.Text + "'";
                    }
                }
                if (strSoFabID != "'='") { strLCDProdSQL += " AND SourceFAB IN (" + strSoFabID + ")"; }
                foreach (ListItem li in cblGCraft.Items)
                {
                    string str = li.Text;
                    if (li.Selected)
                    {
                        strGCraft = strGCraft + "," + "'" + li.Text + "'";
                    }
                }
                if (strGCraft != "'='") { strLCDProdSQL += " AND GLASS_CRAFT IN (" + strGCraft + ")"; }
            }
            DataSet dsLCDProd = new DataSet();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            SqlDataAdapter sqlAptLCD = new SqlDataAdapter(strLCDProdSQL, dbConn.ConnectionString);
            sqlAptLCD.Fill(dsLCDProd);
            for (int i = 0; i < dsLCDProd.Tables[0].Rows.Count; i++)
            {
                strLCDProdID += "," + "'" + dsLCDProd.Tables[0].Rows[i][0].ToString() + "'";
            }
            /*V1.05 Add End*/
            /*-------------------------------------------------------------------------------------*/
            if (ddlDateType.SelectedValue == "Date")
            {
                DateTime dt1 = Convert.ToDateTime(txtFDate.Text);
                DateTime dt2 = Convert.ToDateTime(txtTDate.Text);
                TimeSpan ts = dt2 - dt1;
                tbDiffDate.Text = Convert.ToInt32(ts.TotalDays).ToString();

                tbDateType.Text = "D";

                tbFDate.Text = txtFDate.Text;
                tbTDate.Text = txtTDate.Text;
            }
            if (ddlDateType.SelectedValue == "Week")
            {
                tbDiffDate.Text = Convert.ToString(Convert.ToInt32(ddlTDate.SelectedValue.Replace("W", "")) - Convert.ToInt32(ddlFDate.SelectedValue.Replace("W", "")));
                tbDateType.Text = "W";
                tbFDate.Text = (Convert.ToInt32(ddlFDate.SelectedValue.Replace("W0", "").Replace("W", "")) - 1).ToString();
                tbTDate.Text = (Convert.ToInt32(ddlTDate.SelectedValue.Replace("W0", "").Replace("W", "")) - 1).ToString();
            }
            if (ddlDateType.SelectedValue == "Month")
            {
                tbDiffDate.Text = Convert.ToString(Convert.ToInt32(ddlTDate.SelectedValue.Replace("M", "")) - Convert.ToInt32(ddlFDate.SelectedValue.Replace("M", "")));
                tbDateType.Text = "M";
                tbFDate.Text = ddlFDate.SelectedValue.Replace("M0", "").Replace("M", "");
                tbTDate.Text = ddlTDate.SelectedValue.Replace("M0", "").Replace("M", "");
            }
            if (Convert.ToInt32(tbDiffDate.Text) < 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('開始時間不能大於結束時間！');</script>");
                return;
            }
            if (cbCPFlag.Checked == true)   /*判斷是否有選擇"CELL2 PRODID"的選項*/
            {
                tbCPFlag.Text = "C";
            }
            else
            {
                tbCPFlag.Text = "P";
            }
            /*--------------------------------------------------------------------------------------*/
            tbSqlDate.Text = "";
            if (tbDateType.Text == "D") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportDate >= '" + tbFDate.Text + "' AND ReportDate <= '" + tbTDate.Text + "' "; }
            if (tbDateType.Text == "W") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportWeek >= " + tbFDate.Text + " AND ReportWeek <= " + tbTDate.Text + " "; }
            if (tbDateType.Text == "M") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportMonth >= " + tbFDate.Text + " AND ReportMonth <= " + tbTDate.Text + " "; }

            tbSqlWhere.Text = "";
            if (strSubLineID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND SUBLINEID IN(" + strSubLineID + ")"; }
            if (strLotType != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND LOT_TYPE IN(" + strLotType + ")"; }
            if (tbCPFlag.Text == "C")
            {
                //if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }                              //V1.05 Mark
                if (CB13NC.Checked)
                {
                    if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CT9NCNO IN(" + strProdID + ")"; }       
                    
                }
                else
                {
                    if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }       //V1.05 Add
                    if (strLCDProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strLCDProdID + ")"; }                          //V1.05 Add
                }
            }
            else
            {
                //if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }                              //V1.05 Mark
                if (CB13NC.Checked)
                {
                    if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND T9NCNO IN(" + strProdID + ")"; } 
                }
                else
                {
                    if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }       //V1.05 Add
                    if (strLCDProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strLCDProdID + ")"; }                          //V1.05 Add
                }
            }
            if (strWonbr != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND WORK_ORDER IN(" + strWonbr + ")"; }
            if (CB13NC.Checked)
            {
                strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
            }
            else
            {
                strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
            }
            strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK) ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");
            strSql.Append(tbSqlWhere.Text);
            if (CB13NC.Checked)
            {
                strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ")");
            }
            else
            {
                strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ")");
            }
            DataSet dt00 = new DataSet();
            SqlDataAdapter sqlApt00 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt00.Fill(dt00);
            if (dt00.Tables[0].Rows.Count == 0)
            {
                //ClearDataBound();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查無資料！');</script>");
                return;
            }
            cblProdID.DataSource = dt00;
            cblProdID.DataTextField = "PRODUCT_ID";
            cblProdID.DataValueField = "PRODUCT_ID";
            cblProdID.DataBind();
            dt00.Dispose();
            sqlApt00.Dispose();
            strSql.Remove(0, strSql.Length);
        }
        else if (PProdID.Visible)
        {
            string strSql = "";
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            strSql = "";
            //strSql = "SELECT PRODID FROM MPIM WHERE FABID = '" + tbFABID.Text + "' AND PRODID NOT LIKE 'V%' AND PRODID NOT LIKE '#%' ORDER BY PRODID";
            if (CB13NC.Checked)
            {
                strSql = "select distinct t9ncno as PRODID from mpm9m order by t9ncno";
            }
            else
            {
                strSql = "select distinct project_name as PRODID from mpim order by project_name";
            }
            DataSet ds = new DataSet();
            ds = dbConn.GetDataSetBySql(strSql, "");
            cblProdID.DataSource = ds;
            cblProdID.DataTextField = "PRODID";
            cblProdID.DataValueField = "PRODID";
            cblProdID.DataBind();
            ds.Dispose();
        }
    }
    protected void DataBoundLotType()
    {
        string strSql = "";
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        strSql = "";
        strSql = "SELECT LOTTYPE FROM MLTM WHERE FABID = '" + tbFABID.Text + "' ORDER BY LOTTYPE";
        DataSet ds = new DataSet();
        ds = dbConn.GetDataSetBySql(strSql, "");
        cblLotType.DataSource = ds;
        cblLotType.DataTextField = "LOTTYPE";
        cblLotType.DataValueField = "LOTTYPE";
        cblLotType.DataBind();
        ds.Dispose();
    }
    protected void DataBoundSubLineID()
    {
        string strSql = "";
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        strSql = "";
        //strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8005','8015','8020','8040','8050','8060','8080','8081''8090','8180') ORDER BY WORKCTR,SUBLINEID";/*V1.02*/  V1.04 Mark
        strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8005','8015','8020','8040','8050','8060','8080','8081','808A','808B','8082','8085','8090','8180') ORDER BY WORKCTR,SUBLINEID";  // V1.04 ADD
        DataSet ds = new DataSet();
        ds = dbConn.GetDataSetBySql(strSql, "");
        cblSubLineID.DataSource = ds;
        cblSubLineID.DataTextField = "SUBLINEID";
        cblSubLineID.DataValueField = "SUBLINEID";
        cblSubLineID.DataBind();
        ds.Dispose();
    }
    protected void ClearDataBound()
    {
        lblDataRunTime.Text = "";
        tbDateType.Text = "";
        tbCPFlag.Text = "";
        tbFDate.Text = "";
        tbTDate.Text = "";
        tbDiffDate.Text = "0";

        tbSqlDate.Text = "";
        tbSqlWhere.Text = "";

        lblFabID.Text = tbFABName.Text;

        Session["dtCYield"] = null;
        Session["dtCYDef"] = null;

        PMain.Enabled = true;
        PMain.Height = 100;

        ChangeHeight();
        ClearChartBound();
        PageTitleBound();

        IBGBYData.Enabled = false;
        PTotal.Visible = false;

        ddlByProdID.Items.Clear();
        ddlSDefList.Items.Clear();
        GetSubject();
        ddlShiftID.SelectedIndex = 0;
        cblLevel.SelectedIndex = 0;
        cblLevel.Enabled = false;

        /*V1.03 ADD Begin*/
        ddlByGrade.SelectedValue = "S";
        ddlByGradeList.SelectedValue = "S";
        ddlByGrade.Enabled = false;
        ddlByGradeList.Enabled = false;
        /*V1.03 ADD End*/

        GridView1.Dispose();
        GridView2.Dispose();
        GridView3.Dispose();
        GridView4.Dispose();
        GridView5.Dispose();

        GridView1.DataBind();
        GridView2.DataBind();
        GridView3.DataBind();
        GridView4.DataBind();
        GridView5.DataBind();

        lblTTime.Text = "";
        lblTime01.Text = "";
        lblTime02.Text = "";
        lblTime03.Text = "";  //V1.03 ADD
        lblTime04.Text = "";
        lblTime05.Text = "";
        GC.Collect();
    }
    protected void ClearChartBound()
    {
        ChartAll.DataSource = "";
        ChartAll.Series.Clear();
        ChartAll.Titles.Clear();
        ChartAll.UI.Toolbar.Enabled = false;

        ChartProdID.DataSource = "";
        ChartProdID.Series.Clear();
        ChartProdID.Titles.Clear();
        ChartProdID.Visible = false;

        ChartDT.DataSource = "";
        ChartDT.Series.Clear();
        ChartDT.Titles.Clear();
        ChartDT.Visible = false;

        ChartSDefectCode.DataSource = "";
        ChartSDefectCode.Series.Clear();
        ChartSDefectCode.Titles.Clear();
        ChartSDefectCode.Visible = false;

        ChartSDefectDistr.DataSource = "";
        ChartSDefectDistr.Series.Clear();
        ChartSDefectDistr.Titles.Clear();
        ChartSDefectDistr.Visible = false;

        ChartLevelDefectCode.DataSource = "";
        ChartLevelDefectCode.Series.Clear();
        ChartLevelDefectCode.Titles.Clear();
        ChartLevelDefectCode.Visible = false;
    }
    protected void ddlDateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearDataBound();
        if (ddlDateType.SelectedValue == "Date")
        {
            txtFDate.Visible = true;
            txtTDate.Visible = true;
            imgF.Visible = true;
            imgT.Visible = true;
            ddlFDate.Visible = false;
            ddlTDate.Visible = false;
            ddlYear.Enabled = false;
            ddlFDate.Items.Clear();
            ddlTDate.Items.Clear();
            ddlYear.Items.Clear();
            DataBoundtxtDate();
        }
        else
        {
            txtFDate.Visible = false;
            txtTDate.Visible = false;
            imgF.Visible = false;
            imgT.Visible = false;
            ddlFDate.Visible = true;
            ddlTDate.Visible = true;
            ddlYear.Enabled = true;

            ddlFDate.Items.Clear();
            ddlTDate.Items.Clear();
            ddlYear.Items.Clear();
            txtFDate.Text = "";
            txtTDate.Text = "";

            for (int i = 0; i <= 2; i++)
            {
                ddlYear.ClearSelection();
                ddlYear.Items.Add(System.DateTime.Now.AddYears(-1 * i).Year.ToString());
            }

            if (ddlDateType.SelectedValue == "Week")
            {
                for (int i = 1; i <= 53; i++)
                {
                    ddlFDate.ClearSelection();
                    ddlTDate.ClearSelection();
                    if (i <= 9)
                    {
                        ddlFDate.Items.Add("W0"+i.ToString());
                        ddlTDate.Items.Add("W0" + i.ToString());
                    }
                    else
                    {
                        ddlFDate.Items.Add("W" + i.ToString());
                        ddlTDate.Items.Add("W" + i.ToString());
                    }
                }
            }
            if (ddlDateType.SelectedValue == "Month")
            {
                for (int i = 1; i <= 12; i++)
                {
                    ddlFDate.ClearSelection();
                    ddlTDate.ClearSelection();
                    if (i <= 9)
                    {
                        ddlFDate.Items.Add("M0" + i.ToString());
                        ddlTDate.Items.Add("M0" + i.ToString());
                    }
                    else
                    {
                        ddlFDate.Items.Add("M" + i.ToString());
                        ddlTDate.Items.Add("M" + i.ToString());
                    }
                }
            }
        }
    }
    protected void ddlSubLineID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubLineID.SelectedValue == "UserDefine")
        {
            PSubLineID.Visible = true;
            DataBoundSubLineID();
        }
        else
        {
            cblSubLineID.Items.Clear();
            PSubLineID.Visible = false;
        }
        ClearDataBound();
        ChangeHeight();
    }
    protected void ddlLotType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLotType.SelectedValue == "UserDefine")
        {
            PLotType.Visible = true;
            DataBoundLotType();
        }
        else
        {
            cblLotType.Items.Clear();
            PLotType.Visible = false;
        }
        ClearDataBound();
        ChangeHeight();
    }
    protected void ddlProdID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProdID.SelectedValue == "UserDefine")
        {
            PProdID.Visible = true;
            DataBoundProdID();
        }
        else
        {
            cblProdID.Items.Clear();
            PProdID.Visible = false;
        }
        ClearDataBound();
        ChangeHeight();
    }
    protected void ddlWonbr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWonbr.SelectedValue == "UserDefine")
        {
            PWonbr.Visible = true;
            DataBoundWonbr();
        }
        else
        {
            cblWonbr.Items.Clear();
            PWonbr.Visible = false;
        }
        ClearDataBound();
        ChangeHeight();
    }
    protected void ChangeHeight()
    {
        //if (PSubLineID.Visible == false && PLotType.Visible == false && PProdID.Visible == false && PWonbr.Visible == false)
        if (PSubLineID.Visible == false && PLotType.Visible == false && PProdID.Visible == false && PWonbr.Visible == false && PGCraft.Visible == false) //V1.05 Add PGCraft.Visible=false
        {
            PMain.Height = 100;
        }
        else
        {
            PMain.Height = 240;
        }
    }
    protected void IBGBYData_Click(object sender, ImageClickEventArgs e)
    {
        if (PTotal.Visible == false)
        {
            PTotal.Visible = true;
        }
        else
        {
            PTotal.Visible = false;
        }
        ChartDataBound();
    }
    protected void CreateYieldSessionDataTable()
    {
        DataTable dtCYield = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        /*V1.03 Mark Begin*/
        //dtCYield.Columns.Add(new DataColumn("ReportDT", typeof(string)));			            /*0 */
        //dtCYield.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			        /*1 */
        //dtCYield.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));                 /*2 */
        //dtCYield.Columns.Add(new DataColumn("Chip_Qty", typeof(long)));                       /*3 */
        //dtCYield.Columns.Add(new DataColumn("A_Yield", typeof(double)));                      /*4 */
        //dtCYield.Columns.Add(new DataColumn("S_Yield", typeof(double)));                      /*5 */
        //dtCYield.Columns.Add(new DataColumn("SHIFTID", typeof(string)));			        /*6 */
        //dtCYield.Columns.Add(new DataColumn("ItemNo", typeof(int)));                          /*7*/
        /*V1.03 Mark End*/
        /*V1.03 ADD Begin*/
        dtCYield.Columns.Add(new DataColumn("ReportDT", typeof(string)));			          /*0 */
        dtCYield.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			          /*1 */
        dtCYield.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));                 /*2 */
        dtCYield.Columns.Add(new DataColumn("Chip_Qty", typeof(long)));                       /*3 */
        dtCYield.Columns.Add(new DataColumn("A_Yield", typeof(double)));                      /*4 */
        dtCYield.Columns.Add(new DataColumn("S_Yield", typeof(double)));                      /*5 */
        dtCYield.Columns.Add(new DataColumn("A1_Yield", typeof(double)));                     /*6 */
        dtCYield.Columns.Add(new DataColumn("A2_Yield", typeof(double)));                     /*7 */
        dtCYield.Columns.Add(new DataColumn("L2_Yield", typeof(double)));                     /*8 */
        dtCYield.Columns.Add(new DataColumn("LR_Yield", typeof(double)));                     /*9 */
        dtCYield.Columns.Add(new DataColumn("BP_Yield", typeof(double)));                     /*10 */
        dtCYield.Columns.Add(new DataColumn("PC_Yield", typeof(double)));                     /*11 */
        dtCYield.Columns.Add(new DataColumn("PT_Yield", typeof(double)));                     /*12 */
        dtCYield.Columns.Add(new DataColumn("P5_Yield", typeof(double)));                     /*13 */
        dtCYield.Columns.Add(new DataColumn("SHIFTID", typeof(string)));			          /*14 */
        dtCYield.Columns.Add(new DataColumn("ItemNo", typeof(int)));                          /*15*/
        /*V1.03 ADD End*/
        dtCYield.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] keys = new DataColumn[4];
        keys[0] = dtCYield.Columns["ReportDT"];
        keys[1] = dtCYield.Columns["PRODUCT_ID"];
        keys[2] = dtCYield.Columns["OPERATION_ID"];
        keys[3] = dtCYield.Columns["SHIFTID"];
        dtCYield.PrimaryKey = keys;
        /*--------------------------------------------------------------------------------------*/
        Session["dtCYield"] = null;
        Session["dtCYield"] = dtCYield;
    }
    protected void CreateDefSessionDataTable()
    {
        /*-------------------------------------------------------------------------------------*/
        DataTable dtCYDef = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtCYDef.Columns.Add(new DataColumn("ReportDT", typeof(string)));			                /*0 */
        dtCYDef.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			            /*1 */
        dtCYDef.Columns.Add(new DataColumn("LOTGRADE", typeof(string)));                          /*2 */
        dtCYDef.Columns.Add(new DataColumn("DEFCODE", typeof(string)));                           /*3 */
        dtCYDef.Columns.Add(new DataColumn("DEFECTDESC", typeof(string)));                        /*4 */
        dtCYDef.Columns.Add(new DataColumn("TTL_CNT", typeof(long)));                             /*5 */
        dtCYDef.Columns.Add(new DataColumn("DefectRate", typeof(double)));                        /*6 */
        dtCYDef.Columns.Add(new DataColumn("Percentage", typeof(double)));                        /*7 */
        dtCYDef.Columns.Add(new DataColumn("ItemNo", typeof(int)));                               /*8 */
        dtCYDef.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] DFkeys = new DataColumn[4];
        DFkeys[0] = dtCYDef.Columns["ReportDT"];
        DFkeys[1] = dtCYDef.Columns["PRODUCT_ID"];
        DFkeys[2] = dtCYDef.Columns["LOTGRADE"];
        DFkeys[3] = dtCYDef.Columns["DEFCODE"];
        dtCYDef.PrimaryKey = DFkeys;
        /*-------------------------------------------------------------------------------------*/
        Session["dtCYDef"] = null;
        Session["dtCYDef"] = dtCYDef;
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        int intSecCnt = 0;
        AppClass Apc = new AppClass();
        string strSubLineID = "'='";
        string strProdID = "'='";
        string strLotType = "'='";
        string strWonbr = "'='";
        string strLCDProdID = "'='";  //V1.05 Add

        lblTTime.Text = "";
        lblTime01.Text = "";
        lblTime02.Text = "";
        lblTime03.Text = ""; //V1.03 ADD
        lblTime04.Text = "";
        lblTime05.Text = "";
        DateTime dtF = System.DateTime.Now;
        /*--------------------------------------------------------------------------------------*/
        tbOPERATIONID.Text = "";
        tbType.Text = "";
        tbDateType.Text = "";
        tbDiffDate.Text = "0";
        tbCPFlag.Text = "";
        tbFDate.Text = "";
        tbTDate.Text = "";

        lblFabID.Text = tbFABName.Text;

        //dtCYield.Dispose();
        //dtCYield.Clear();

        //dtCYDef.Dispose();
        //dtCYDef.Clear();
        Session["dtCYield"] = null;
        Session["dtCYDef"] = null;

        IBGBYData.Enabled = false;
        cblLevel.Enabled = false;

        ddlByProdID.Items.Clear();
        ddlSDefList.Items.Clear();


        ddlShiftID.SelectedIndex = 0;

        ClearChartBound();
        switch (ddlStation.SelectedItem.Text.ToString())
        {
            case "BatchInspection(8005)": tbOPERATIONID.Text = "8005"; tbType.Text = "BatchInspection(8005)"; break;
            case "AutoScribe(8015)": tbOPERATIONID.Text = "8015"; tbType.Text = "AutoScribe(8015)"; break;
            case "SemiScribe(8020)": tbOPERATIONID.Text = "8020"; tbType.Text = "SemiScribe(8020)"; break;

            case "LC(8040)": tbOPERATIONID.Text = "8040"; tbType.Text = "LC(8040)"; break;
            case "封口(8050)": tbOPERATIONID.Text = "8050"; tbType.Text = "封口(8050)"; break;
            case "RoughClean(8060)": tbOPERATIONID.Text = "8060"; tbType.Text = "RoughClean(8060)"; break;

            case "LOT1(8080)": tbOPERATIONID.Text = "8080"; tbType.Text = "LOT1(8080)"; break;
            case "LOT1.5(808T)": tbOPERATIONID.Text = "808T"; tbType.Text = "LOT1.5(808T)"; break; ////V2.00
            case "外觀(8081)": tbOPERATIONID.Text = "8081"; tbType.Text = "外觀(8081)"; break;
            case "Linecheck(808E)": tbOPERATIONID.Text = "808E"; tbType.Text = "Linecheck(808E)"; break;           ////V2.01

            case "LOT1_LR(808A)": tbOPERATIONID.Text = "808A"; tbType.Text = "LOT1_LR(808A)"; break;                 // V1.04 ADD
            case "外觀_LR(808B)": tbOPERATIONID.Text = "808B"; tbType.Text = "外觀_LR(808B)"; break;                 // V1.04 ADD
            case "Cell2Packing1(8082)": tbOPERATIONID.Text = "8082"; tbType.Text = "Cell2Packing1(8082)"; break;   // V1.04 ADD
            case "Cell2Packing2(8085)": tbOPERATIONID.Text = "8085"; tbType.Text = "Cell2Packing2(8085)"; break;   // V1.04 ADD

            case "FinalClean(8090)": tbOPERATIONID.Text = "8090"; tbType.Text = "FinalClean(8090)"; break;
            case "M1(8180)": tbOPERATIONID.Text = "8180"; tbType.Text = "M1(8180)"; break;/*V1.02*/
            case "Relot1.5(818T)": tbOPERATIONID.Text = "818T"; tbType.Text = "Relot1.5(818T)"; break;  ////V2.00
            default:
                break;
        }
        if (cblSubLineID.Items.Count > 0)
        {
            foreach (ListItem li in this.cblSubLineID.Items)
            {
                if (li.Selected)
                {
                    strSubLineID = strSubLineID + "," + "'" + li.Text + "'";
                }
            }
        }
        if (cblLotType.Items.Count > 0)
        {
            foreach (ListItem li in this.cblLotType.Items)
            {
                if (li.Selected)
                {
                    strLotType = strLotType + "," + "'" + li.Text + "'";
                }
            }
        }
        if (cblProdID.Items.Count > 0)
        {
            foreach (ListItem li in this.cblProdID.Items)
            {
                if (li.Selected)
                {
                    strProdID = strProdID + "," + "'" + li.Text + "'";
                }
            }
        }
        if (cblWonbr.Items.Count > 0)
        {
            foreach (ListItem li in this.cblWonbr.Items)
            {
                if (li.Selected)
                {
                    strWonbr = strWonbr + "," + "'" + li.Text + "'";
                }
            }
        }
        /*V1.05 Add Start*/
        string strSoFabID = "'='";
        string strGCraft = "'='";
        string strLCDProdSQL = " SELECT ProjName FROM dbo.MSOFABMLCDM WHERE HEADFAB='" + ddlGCraft.SelectedItem.Text + "'";
        if (cblGCraft.Items.Count > 0)
        {
            foreach (ListItem li in cblSoFab.Items)
            {
                if (li.Selected)
                {
                    strSoFabID = strSoFabID + "," + "'" + li.Text + "'";
                }
            }
            if (strSoFabID != "'='") { strLCDProdSQL += " AND SourceFAB IN (" + strSoFabID + ")"; }
            foreach (ListItem li in cblGCraft.Items)
            {
                string str = li.Text;
                if (li.Selected)
                {
                    strGCraft = strGCraft + "," + "'" + li.Text + "'";
                }
            }
            if (strGCraft != "'='") { strLCDProdSQL += " AND GLASS_CRAFT IN (" + strGCraft + ")"; }
        }
        DataSet dsLCDProd = new DataSet();
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        SqlDataAdapter sqlAptLCD = new SqlDataAdapter(strLCDProdSQL, dbConn.ConnectionString);
        sqlAptLCD.Fill(dsLCDProd);
        for (int i = 0; i < dsLCDProd.Tables[0].Rows.Count; i++)
        {
            strLCDProdID += "," + "'" + dsLCDProd.Tables[0].Rows[i][0].ToString() + "'";
        }
        /*V1.05 Add End*/
        /*-------------------------------------------------------------------------------------*/
        if (ddlDateType.SelectedValue == "Date")
        {
            DateTime dt1 = Convert.ToDateTime(txtFDate.Text);
            DateTime dt2 = Convert.ToDateTime(txtTDate.Text);
            TimeSpan ts = dt2 - dt1;
            tbDiffDate.Text = Convert.ToInt32(ts.TotalDays).ToString();

            tbDateType.Text = "D";

            tbFDate.Text = txtFDate.Text;
            tbTDate.Text = txtTDate.Text;
        }
        if (ddlDateType.SelectedValue == "Week")
        {
            tbDiffDate.Text = Convert.ToString(Convert.ToInt32(ddlTDate.SelectedValue.Replace("W", "")) - Convert.ToInt32(ddlFDate.SelectedValue.Replace("W", "")));
            tbDateType.Text = "W";
            tbFDate.Text = (Convert.ToInt32(ddlFDate.SelectedValue.Replace("W0", "").Replace("W", "")) - 1).ToString();
            tbTDate.Text = (Convert.ToInt32(ddlTDate.SelectedValue.Replace("W0", "").Replace("W", "")) - 1).ToString();
        }
        if (ddlDateType.SelectedValue == "Month")
        {
            tbDiffDate.Text = Convert.ToString(Convert.ToInt32(ddlTDate.SelectedValue.Replace("M", "")) - Convert.ToInt32(ddlFDate.SelectedValue.Replace("M", "")));
            tbDateType.Text = "M";
            tbFDate.Text = ddlFDate.SelectedValue.Replace("M0", "").Replace("M", "");
            tbTDate.Text = ddlTDate.SelectedValue.Replace("M0", "").Replace("M", "");
        }
        if (Convert.ToInt32(tbDiffDate.Text) < 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('開始時間不能大於結束時間！');</script>");
            return;
        }
        if (cbCPFlag.Checked == true)   /*判斷是否有選擇"CELL2 PRODID"的選項*/
        {
            tbCPFlag.Text = "C";
        }
        else
        {
            tbCPFlag.Text = "P";
        }
        /*--------------------------------------------------------------------------------------*/
        tbSqlDate.Text = "";
        if (tbDateType.Text == "D") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportDate >= '" + tbFDate.Text + "' AND ReportDate <= '" + tbTDate.Text + "' "; }
        if (tbDateType.Text == "W") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportWeek >= " + tbFDate.Text + " AND ReportWeek <= " + tbTDate.Text + " "; }
        if (tbDateType.Text == "M") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportMonth >= " + tbFDate.Text + " AND ReportMonth <= " + tbTDate.Text + " "; }

        tbSqlWhere.Text = "";
        if (strSubLineID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND SUBLINEID IN(" + strSubLineID + ")"; }
        if (strLotType != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND LOT_TYPE IN(" + strLotType + ")"; }
        if (tbCPFlag.Text == "C")
        {
            //if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }                              //V1.05 Mark
            if (CB13NC.Checked)
            {
                if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CT9NCNO IN(" + strProdID + ")"; } 
            }
            else
            {
                if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }       //V1.05 Add
                if (strLCDProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strLCDProdID + ")"; }                          //V1.05 Add
            }
        }
        else
        {
            //if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }                              //V1.05 Mark
            if (CB13NC.Checked)
            {
                if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND T9NCNO IN(" + strProdID + ")"; }  
            }
            else
            {
                if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }       //V1.05 Add
                if (strLCDProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strLCDProdID + ")"; }                          //V1.05 Add
            }
        }
        if (strWonbr != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND WORK_ORDER IN(" + strWonbr + ")"; }
        /*--------------------------------------------------------------------------------------*/
        CreateYieldSessionDataTable();
        CreateDefSessionDataTable();
        /*--------------------------------------------------------------------------------------*/
        QueryFPYDataBound("TOTAL", "ALL", "TOTAL");
        QueryFPYDataBound("TOTAL", ddlByProdID.SelectedValue, "SHIFTID");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "TOTAL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "SHIFTID");
        /*V1.05 Add Start*/
        if (ChkMerge.Checked)
        {
            QueryFPYDataBound("TOTALPD", "ALL", "TOTAL");
            QueryFPYDataBound("TOTALPD", "ALL", "SHIFTID");
        }
        /*V1.05 Add End*/
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", "ALL","");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        GridViewDataBound();
        ChartDataBound();
        DataTable dtTmp = (DataTable)Session["dtCYield"];
        string strLogWhere = "Station:" + ddlStation.SelectedValue.ToString() +
                             ";DateType:" + ddlDateType.SelectedValue.ToString() +
                             ";From:" + tbFDate.Text.ToString() +
                             ";To:" + tbTDate.Text.ToString() +
                             ";Year:" + ddlYear.SelectedValue.ToString() +
                             ";Cell2Prodid:" + cbCPFlag.Checked.ToString() +
                             ";Length:" + ddlPdLen.SelectedValue.ToString() +
                             ";ShiftID:" + ddlShiftID.SelectedValue.ToString() +
                             ";SubLineID:" + strSubLineID.ToString() +
                             ";LotType:" + strLotType.ToString() +
                             ";ProdID:" + strProdID.ToString() +
                             ";Wo:" + strWonbr.ToString();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-btQuery;筆數:" + dtTmp.Rows.Count.ToString(), strLogWhere.Replace("'", ""), "");
        /*--------------------------------------------------------------------------------------*/
        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTTime.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
        PMain.Enabled = false;
        /*--------------------------------------------------------------------------------------*/
        IBGBYData.Enabled = true;
        cblLevel.Enabled = true;
        ddlByGrade.Enabled = true;      // V1.03 ADD
        ddlByGradeList.Enabled = true;  // V1.03 ADD
    }
    protected void QueryFPYDataBound(string strMode, string strTmpProdID, string strTmpShiftID)
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            /*-------------------------------------------------------------------------------------*/
            /*機種清單*/
            /*V1.05 Add Start*/
            if (ChkMerge.Checked && strMode == "Days") 
            {
                ddlByProdID.Items.Clear();
                ddlByProdID.Items.Add(new ListItem("ALL", "ALL"));
            }
            else if (strMode == "TOTALPD")
            {
                ddlByProdID.Items.Clear();
                ddlByProdID.Items.Add(new ListItem("ALL", "ALL"));
            }
            else
            {
                /*V1.05 Add End*/
                if (strMode != "Days")
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
                    }
                    else
                    {
                        strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
                    }
                    strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK) ");
                    strSql.Append(tbSqlDate.Text);
                    strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
                    strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");
                    strSql.Append(tbSqlWhere.Text);
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ")");
                    }
                    else
                    {
                        strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ")");
                    }
                    DataSet dt00 = new DataSet();
                    SqlDataAdapter sqlApt00 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
                    sqlApt00.Fill(dt00);
                    if (dt00.Tables[0].Rows.Count == 0)
                    {
                        //ClearDataBound();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查無資料！');</script>");
                        return;
                    }
                    ddlByProdID.DataSource = dt00;
                    ddlByProdID.DataTextField = "PRODUCT_ID";
                    ddlByProdID.DataValueField = "PRODUCT_ID";
                    ddlByProdID.DataBind();
                    dt00.Dispose();
                    sqlApt00.Dispose();
                    strSql.Remove(0, strSql.Length);
                }
            }/*V1.05 Add End*/
            /*-------------------------------------------------------------------------------------*/

            strSql.Remove(0, strSql.Length);
            strSql.Append(" SELECT T1.REPORTDT");                /*0 */
            strSql.Append(" ,T1.PRODUCT_ID");                /*1 */
            strSql.Append(" ,T1.OPERATION_ID");                /*2 */
            strSql.Append(" ,T1.T_CNT");                /*3 */
            strSql.Append(" ,T1.A_CNT");                /*4 */
            strSql.Append(" ,T1.S_CNT");                /*5 */
            strSql.Append(" ,T1.A1_CNT");                /*6 */  /*V1.03 ADD Begin*/
            strSql.Append(" ,T1.A2_CNT");                /*7 */
            strSql.Append(" ,T1.L2_CNT");                /*8 */
            strSql.Append(" ,T1.LR_CNT");                /*9 */
            strSql.Append(" ,T1.BP_CNT");                /*10 */
            strSql.Append(" ,T1.PC_CNT");                /*11 */
            strSql.Append(" ,T1.PT_CNT");                /*12 */
            strSql.Append(" ,T1.P5_CNT");                /*13 */ /*V1.03 ADD End*/
            strSql.Append(" ,T1.SHIFT_ID");              /*14 */ /*V1.03 Change 6 to 14*/
            strSql.Append(" FROM(");
            strSql.Append(" SELECT OPERATION_ID,");
            if (strMode == "Days")
            {
                strSql.Append(" CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END AS ReportDT,");
            }
            else
            {
                strSql.Append(" 'TOTAL' AS ReportDT,");
            }
            /*V1.05 Add Start*/
            if (ChkMerge.Checked && strMode == "Days") 
            {
                strSql.Append("'ALL' AS PRODUCT_ID,");
            }
            else if (strMode == "TOTALPD")
            {
                strSql.Append("'ALL' AS PRODUCT_ID,");
            }
            else
            {
                /*V1.05 Add End*/
                if (CB13NC.Checked)
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
                }
                else
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
                }
            }
            strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");
            strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");
            strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT,");
            strSql.Append(" SUM(ISNULL(A1_CNT,0)) AS A1_CNT, ");  /*V1.03 ADD Begin*/
            strSql.Append(" SUM(ISNULL(A2_CNT,0)) AS A2_CNT, ");
            strSql.Append(" SUM(ISNULL(L2_CNT,0)) AS L2_CNT, ");
            strSql.Append(" SUM(ISNULL(LR_CNT,0)) AS LR_CNT, ");
            strSql.Append(" SUM(ISNULL(BP_CNT,0)) AS BP_CNT, ");
            strSql.Append(" SUM(ISNULL(PC_CNT,0)) AS PC_CNT, ");
            strSql.Append(" SUM(ISNULL(PT_CNT,0)) AS PT_CNT, ");
            strSql.Append(" SUM(ISNULL(P5_CNT,0)) AS P5_CNT, ");    /*V1.03 ADD End*/
            strSql.Append(" SUM(ISNULL(UNDEFINE_CNT,0)) AS UNDEFINE_CNT");
            if (strTmpShiftID != "TOTAL")
            {
                strSql.Append(" ,SHIFT_ID");
            }
            else
            {
                strSql.Append(" ,'TOTAL' AS SHIFT_ID");
            }
            strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK)");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            strSql.Append(" AND OPERATION_ID ='" + tbOPERATIONID.Text + "' ");
            if (strMode == "Days" && !ChkMerge.Checked)//V1.05 Add !ChkMerge.Checked
            {
                if (tbCPFlag.Text == "C")
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND SUBSTRING(CT9NCNO,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                }
                else
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND SUBSTRING(T9NCNO,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                }
                //strSql.Append(" AND PRODUCT_ID ='" + strTmpProdID + "' ");
            }
            strSql.Append(tbSqlWhere.Text);
            /*V1.05 Add Start*/
            if (ChkMerge.Checked && strMode=="Days") 
            {
                strSql.Append(" GROUP BY OPERATION_ID ");
            }
            else if (strMode == "TOTALPD") 
            {
                strSql.Append(" GROUP BY OPERATION_ID ");
            }
            else
            {
                if (CB13NC.Checked)
                {
                    strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ") ");
                }
                else
                {
                    /*V1.05 Add End*/
                    strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") ");
                }
            }
            if (strMode == "Days")
            {
                strSql.Append(" ,CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END");
            }
            if (strTmpShiftID != "TOTAL")
            {
                strSql.Append(" ,SHIFT_ID");
            }
            strSql.Append(" ) T1");
            DataSet dt011 = new DataSet();
            SqlDataAdapter sqlApt011 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt011.Fill(dt011);
            /*V1.05 Add Start*/
            if (ChkMerge.Checked && strMode == "Days")
            {
                strTmpProdID = "ALL";
            }
            /*V1.05 Add End*/
            /*-------------------------------------------------------------------------------------*/
            BoudFPYDataTable(dt011, strMode, strTmpProdID, strTmpShiftID);
            /*-------------------------------------------------------------------------------------*/
            dt011.Dispose();
            sqlApt011.Dispose();
            strSql.Remove(0, strSql.Length);
            AppClass Apc = new AppClass();
            lblDataRunTime.Text = Apc.QueryDataRunTime(tbFABID.Text).ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryDefectDataBound(string strMode, string strTmpProdID, string strTmpShiftID, string strGradeID, string strLevelID)
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            /*-------------------------------------------------------------------------------------*/
            /*計算各機種DefectCode數量*/
            strSql.Append(" SELECT ");
            if (strMode == "Days")
            {
                strSql.Append(" CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END AS ReportDT,");
            }
            else
            {
                strSql.Append(" 'TOTAL' AS ReportDT,");
            }
            /*V1.05 Add start*/
            if (ChkMerge.Checked)
            {
                strSql.Append(" 'ALL' AS PRODUCT_ID,");
            }
            else
            {
                /*V1.05 Add End*/
                if (CB13NC.Checked)
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CT9NCNO ELSE A.T9NCNO END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
                }
                else
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
                }
            }/*V1.05 Add*/
            if (strGradeID != "")
            {
                strSql.Append(" LOT_GRADE, ");
            }
            if (strLevelID != "")
            {
                if (strLevelID == "ALL")
                {
                    strSql.Append(" 'ALL' AS LOT_GRADE, ");
                }
                else
                {
                    strSql.Append(" LOT_LEVEL AS LOT_GRADE, ");
                }
            }
            strSql.Append(" DEF_CODE, ");
            strSql.Append(" DEFECTDESC,  ");
            strSql.Append(" SUM(DEF_CNT) AS TTL_CNT ");
            strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK) ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND A.FAB_ID = '" + tbFABID.Text + "' ");
            strSql.Append(" AND A.OPERATION_ID ='" + tbOPERATIONID.Text + "' ");
            /*V1.05 Add Start*/
            if (ChkMerge.Checked)
            {
                if (strTmpProdID != "'='" && strTmpProdID != "ALL")
                {
                    if (tbCPFlag.Text == "C")
                    {
                        if (CB13NC.Checked)
                        {
                            strSql.Append(" AND SUBSTRING(CT9NCNO,1," + ddlPdLen.SelectedValue + ")  in (" + strTmpProdID + ") ");
                        }
                        else
                        {
                            strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ")  in (" + strTmpProdID + ") ");
                        }
                    }
                    else
                    {
                        if (CB13NC.Checked)
                        {
                            strSql.Append(" AND SUBSTRING(T9NCNO,1," + ddlPdLen.SelectedValue + ")  in(" + strTmpProdID + ") ");
                        }
                        else
                        {
                            strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ")  in(" + strTmpProdID + ") ");
                        }
                    }
                }
            }
            else
            {
                /*V1.05 Add End*/
                if (tbCPFlag.Text == "C")
                {
                    //strSql.Append(" AND CPRODUCT_ID ='" + strTmpProdID + "' ");
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND SUBSTRING(CT9NCNO,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }

                    else
                    {
                        strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                }
                else
                {
                    //strSql.Append(" AND PRODUCT_ID ='" + strTmpProdID + "' ");
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND SUBSTRING(T9NCNO,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                }
            }/*V1.05 Add*/
            if (strTmpShiftID != "TOTAL")
            {
                strSql.Append(" AND SHIFT_ID LIKE '%" + strTmpShiftID + "'");
            }

            if (strGradeID == "ALL")
            {
                strSql.Append(" AND A.LOT_GRADE  = 'S' ");
            }
            else 
            {
                if (strGradeID != "")
                {
                    strSql.Append(" AND A.LOT_GRADE = '" + strGradeID + "'");
                }
            }

            if (strLevelID != "" && strLevelID != "ALL")
            {
                strSql.Append(" AND A.LOT_LEVEL = '" + strLevelID + "'");
            }

            strSql.Append(tbSqlWhere.Text);

            strSql.Append(" AND A.FAB_ID=B.FABID");
            strSql.Append(" AND A.DEF_CODE=B.DEFECTCD");
            strSql.Append(" GROUP BY ");
            if (strMode == "Days")
            {
                strSql.Append(" CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END, ");
            }
            if (!ChkMerge.Checked)/*V1.05 Add*/
            {
                if (CB13NC.Checked)
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CT9NCNO ELSE A.T9NCNO END,1," + ddlPdLen.SelectedValue + "), ");
                }
                else
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,1," + ddlPdLen.SelectedValue + "), ");
                }
            }
            if (strGradeID != "")
            {
                strSql.Append(" LOT_GRADE,DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            }
            if (strLevelID != "")
            {
                if (strLevelID == "ALL")
                {
                    strSql.Append(" DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
                }
                else
                {
                    strSql.Append(" LOT_LEVEL,DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
                }
            }
            DataSet dt012 = new DataSet();
            SqlDataAdapter sqlApt012 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt012.Fill(dt012);
            /*V1.05 Add Start*/
            if (ChkMerge.Checked)
            {
                strTmpProdID = "ALL";
            }
            /*V1.05 Add End*/
            /*-------------------------------------------------------------------------------------*/
            BoudDefectDataTable(dt012, strMode, strTmpProdID, strTmpShiftID, strGradeID, strLevelID);
            /*-------------------------------------------------------------------------------------*/
            dt012.Dispose();
            sqlApt012.Dispose();
            strSql.Remove(0, strSql.Length);
            /*-------------------------------------------------------------------------------------*/
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BoudFPYDataTable(DataSet dsTmp, string strMode, string strTmpProdID, string strTmpShiftID)
    {
        try
        {
            DataTable dtCYield = (DataTable)Session["dtCYield"];
            /*-------------------------------------------------------------------------------------*/
            /*將各站點用編號區分，循環處理這幾個站點的資料*/
            for (int DTCnt = 0; DTCnt <= Convert.ToInt32(tbDiffDate.Text); DTCnt++)
            {
                string strReportDT = "";
                if (strMode != "Days")
                {
                    strReportDT = "TOTAL";
                    DTCnt = Convert.ToInt32(tbDiffDate.Text);
                }
                else
                {
                    if (tbDateType.Text == "D")
                    {
                        strReportDT = Convert.ToDateTime(tbFDate.Text).AddDays(DTCnt).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        if (Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)).Length == 1)
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                            if (tbDateType.Text == "M") { strReportDT = "M0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }
                        else
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                            if (tbDateType.Text == "M") { strReportDT = "M" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }
                    }
                }
                /*-------------------------------------------------------------------------------------*/
                /*循環處理各產品的數據*/
                for (int k = 0; k < Convert.ToInt32(ddlByProdID.Items.Count); k++)
                {
                    string strPRODUCTID = "";         /*產品型號*/
                    if (strMode != "Days")
                    {
                        strPRODUCTID = ddlByProdID.Items[k].Value.ToString();       /*獲取產品型號*/
                    }
                    else
                    {
                        strPRODUCTID = strTmpProdID.ToString();       /*獲取產品型號*/
                        k = Convert.ToInt32(ddlByProdID.Items.Count);
                    }
                    /*-------------------------------------------------------------------------------------*/
                    for (int i = 0; i <= 1; i++)
                    {
                        string strQueryShiftID = "";
                        if (strTmpShiftID != "TOTAL")
                        {
                            switch (i)
                            {
                                case 0: strQueryShiftID = "A"; break;
                                case 1: strQueryShiftID = "B"; break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            strQueryShiftID = "TOTAL";
                            i = 1;
                        }
                        /*查詢個站點的資料*/
                        DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + tbOPERATIONID.Text + "' AND SHIFT_ID LIKE '%" + strQueryShiftID + "')", "PRODUCT_ID DESC");
                        if (drType.Length > 0)
                        {
                            string strSHIFTID = "";
                            long lngMQty = 0;
                            long lngChipQty = 0;
                            double dblAYield = 0,dblSYield = 0;
                            /*  V1.03 Mark Begin
                            ChipQty=A+S     -----和FPY有差異，FPY為A+R+S
                            AYield=A/(ChipQty)
                            RYield=R/(ChipQty+R)
                            SYield=S/(ChipQty)
                            V1.03 Mark End  */

                            /* V1.03 ADD Begin
                            ChipQty=A+S-A1-A2-L2-LR-BP-PC-PT-P5   -----和FPY有差異，FPY為A+R+S
                            AYield=(A-A1-A2-L2-LR-BP-PC-PT-P5)/(ChipQty)
                            RYield=R/(ChipQty+R)
                            SYield=S/(ChipQty)                         
                             V1.03 ADD End */

                            /*V1.03 Mark Begin*/
                            //lngMQty = Convert.ToInt32(drType[0].ItemArray[3]);
                            //lngChipQty = lngMQty;
                            //dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4])) / lngMQty, 4) * 100;
                            //dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[5])) / lngMQty, 4) * 100;

                            //strSHIFTID = drType[0].ItemArray[6].ToString();
                            /*V1.03 Mark End*/

                            /*V1.03 ADD Begin*/
                            double dblA1Yield = 0, dblA2Yield = 0, dblL2Yield = 0, dblLRYield = 0, dblBPYield = 0, dblPCYield = 0, dblPTYield = 0, dblP5Yield = 0;

                            lngMQty = Convert.ToInt32(drType[0].ItemArray[3]) - Convert.ToInt32(drType[0].ItemArray[6]) - Convert.ToInt32(drType[0].ItemArray[7]) - Convert.ToInt32(drType[0].ItemArray[8]) - Convert.ToInt32(drType[0].ItemArray[9]) - Convert.ToInt32(drType[0].ItemArray[10]) - Convert.ToInt32(drType[0].ItemArray[11]) - Convert.ToInt32(drType[0].ItemArray[12]) - Convert.ToInt32(drType[0].ItemArray[13]);
                            lngChipQty = lngMQty;
                            dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4]) - Convert.ToDouble(drType[0].ItemArray[6]) - Convert.ToDouble(drType[0].ItemArray[7]) - Convert.ToDouble(drType[0].ItemArray[8]) - Convert.ToDouble(drType[0].ItemArray[9]) - Convert.ToDouble(drType[0].ItemArray[10]) - Convert.ToDouble(drType[0].ItemArray[11]) - Convert.ToDouble(drType[0].ItemArray[12]) - Convert.ToDouble(drType[0].ItemArray[13])) / lngMQty, 4) * 100;
                            dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[5])) / lngMQty, 4) * 100;

                            dblA1Yield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[6])) / lngMQty, 4) * 100;
                            dblA2Yield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[7])) / lngMQty, 4) * 100;
                            dblL2Yield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[8])) / lngMQty, 4) * 100;
                            dblLRYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[9])) / lngMQty, 4) * 100;
                            dblBPYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[10])) / lngMQty, 4) * 100;
                            dblPCYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[11])) / lngMQty, 4) * 100;
                            dblPTYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[12])) / lngMQty, 4) * 100;
                            dblP5Yield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[13])) / lngMQty, 4) * 100;

                            strSHIFTID = drType[0].ItemArray[14].ToString();
                            /*V1.03 ADD End*/

                            DataRow drFPY = dtCYield.NewRow();
                            drFPY["ReportDT"] = strReportDT;
                            drFPY["PRODUCT_ID"] = strPRODUCTID;
                            drFPY["OPERATION_ID"] = tbType.Text;
                            drFPY["Chip_Qty"] = lngChipQty;
                            drFPY["A_Yield"] = dblAYield;
                            drFPY["S_Yield"] = dblSYield;
                            /*V1.03 ADD Begin*/
                            drFPY["A1_Yield"] = dblA1Yield;
                            drFPY["A2_Yield"] = dblA2Yield;
                            drFPY["L2_Yield"] = dblL2Yield;
                            drFPY["LR_Yield"] = dblLRYield;
                            drFPY["BP_Yield"] = dblBPYield;
                            drFPY["PC_Yield"] = dblPCYield;
                            drFPY["PT_Yield"] = dblPTYield;
                            drFPY["P5_Yield"] = dblP5Yield;
                            /*V1.03 ADD End*/
                            drFPY["SHIFTID"] = strSHIFTID;
                            drFPY["ItemNo"] = k;

                            dtCYield.Rows.Add(drFPY);
                            /*-------------------------------------------------------------------------------------*/
                        }
                    }
                }
            }
            Session["dtCYield"] = null;
            Session["dtCYield"] = dtCYield;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BoudDefectDataTable(DataSet dsTmp, string strMode, string strTmpProdID, string strTmpShiftID, string strGradeID, string strLevelID)
    {
        try
        {
            DataTable dtCYDef = (DataTable)Session["dtCYDef"];
            DataTable dtCYield = (DataTable)Session["dtCYield"];
            /*-------------------------------------------------------------------------------------*/
            /*將各站點用編號區分，循環處理這幾個站點的資料*/
            for (int DTCnt = 0; DTCnt <= Convert.ToInt32(tbDiffDate.Text); DTCnt++)
            {
                string strReportDT = "";
                if (strMode != "Days")
                {
                    strReportDT = "TOTAL";
                    DTCnt = Convert.ToInt32(tbDiffDate.Text);
                }
                else
                {
                    if (tbDateType.Text == "D")
                    {
                        strReportDT = Convert.ToDateTime(tbFDate.Text).AddDays(DTCnt).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        if (Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)).Length == 1)
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                            if (tbDateType.Text == "M") { strReportDT = "M0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }
                        else
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                            if (tbDateType.Text == "M") { strReportDT = "M" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }
                    }
                }
                /*-------------------------------------------------------------------------------------*/
                string strPRODUCTID = "";         /*產品型號*/
                strPRODUCTID = strTmpProdID.ToString();       /*獲取產品型號*/
                /*-------------------------------------------------------------------------------------*/
                /*處理各產品的S&R DefectCode的數據*/
                for (int i = 0; i < 1; i++)
                {
                    string strGrade = "";
                    if (strGradeID != "ALL")
                    {
                        if (strGradeID == "" && strLevelID != "")
                        {
                            strGrade = strLevelID;
                        }
                        else
                        {
                            strGrade = strGradeID;
                        }
                        i = 2;
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0: strGrade = "S"; break;
                            //case 1: strGrade = "R"; break;
                            default:
                                return;
                        }
                    }
                    /*-------------------------------------------------------------------------------------*/
                    DataRow[] drDef = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND LOT_GRADE = '" + strGrade + "' )", "TTL_CNT DESC");
                    if (drDef.Length > 0)
                    {
                        for (int DefCnt = 0; DefCnt < Convert.ToInt32(drDef.Length); DefCnt++)
                        {

                            if (strMode != "Days")
                            {
                               // if (strGrade == "S")  V1.03 Mark
                                if (strGrade == "S" || strGrade == "A1" || strGrade == "A2" || strGrade == "L2" || strGrade == "LR" || strGrade == "BP" || strGrade == "PC" || strGrade == "PT" || strGrade == "P5")  // V1.03 ADD
                                {
                                    ddlSDefList.Items.Add(drDef[DefCnt].ItemArray[3].ToString() + " " + drDef[DefCnt].ItemArray[4].ToString());
                                }
                            }
                            long lngDefectCnt = 0;
                            double dblDefectRate = 1;

                            DataRow[] drType = dtCYield.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + tbType.Text + "' AND SHIFTID LIKE '%" + strTmpShiftID + "')");
                            if (drType.Length > 0)
                            {
                                lngDefectCnt = Convert.ToInt32(drType[0].ItemArray[3]);
                                if (lngDefectCnt != 0)
                                {
                                    dblDefectRate = (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[5]) / lngDefectCnt);
                                }
                            }
                            /*-------------------------------------------------------------------------------------*/
                            DataRow drNew = dtCYDef.NewRow();
                            drNew.Table.Columns.Add();
                            drNew["ReportDT"] = strReportDT;
                            drNew["PRODUCT_ID"] = strPRODUCTID;
                            drNew["LOTGRADE"] = strGrade;
                            drNew["DEFCODE"] = drDef[DefCnt].ItemArray[3].ToString();
                            drNew["DEFECTDESC"] = drDef[DefCnt].ItemArray[4].ToString();
                            drNew["TTL_CNT"] = Convert.ToInt32(drDef[DefCnt].ItemArray[5]);
                            drNew["DefectRate"] = Math.Round(1 - dblDefectRate, 4) * 100;
                            drNew["Percentage"] = 0;
                            drNew["ItemNo"] = DefCnt;
                            dtCYDef.Rows.Add(drNew);
                            /*-------------------------------------------------------------------------------------*/
                        }
                    }
                }
            }
            Session["dtCYDef"] = null;
            Session["dtCYDef"] = dtCYDef;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryDataGroupByFPY()
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        dtCYield.DefaultView.RowFilter = "";
        DataRow[] drChart = dtCYield.Select("ReportDT='TOTAL' AND OPERATION_ID='" + tbType.Text + "' AND SHIFTID='TOTAL' ", "PRODUCT_ID");
        if (drChart.Length > 0)
        {
            ChartAll.Series.Clear();
            ChartAll.DataSource = "";
            ChartAll.Series.Clear();

            ChartAll.Series.Add("Series1");
            ChartAll.Titles.Add("Title1");
            ChartAll.DataSource = drChart;

            ChartAll.Series["Series1"].Color = System.Drawing.Color.FromArgb(71, 94, 118);
            ChartAll.Series["Series1"].BackGradientType = GradientType.VerticalCenter;
            ChartAll.Series["Series1"].Name = "Chip_Qty";
            ChartAll.Series["Chip_Qty"].ValueMemberX = "PRODUCT_ID";
            ChartAll.Series["Chip_Qty"].ValueMembersY = "Chip_Qty";
            ChartAll.Series["Chip_Qty"].XValueType = ChartValueTypes.String;
            ChartAll.Series["Chip_Qty"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            int Cnt = 1;
            foreach (ListItem li in this.cblLevel.Items)
            {
                if (li.Selected)
                {
                    Cnt = Cnt + 1;
                    if (Cnt == 2) { ChartAll.Series.Add("Series2"); ChartAll.Series["Series2"].Color = Color.Red; }
                    if (Cnt == 3) { ChartAll.Series.Add("Series3"); ChartAll.Series["Series3"].Color = Color.Lime; }
                    if (Cnt == 4) { ChartAll.Series.Add("Series4"); ChartAll.Series["Series4"].Color = Color.Olive; }
                    if (Cnt == 5) { ChartAll.Series.Add("Series5"); ChartAll.Series["Series4"].Color = Color.White; }

                    ChartAll.Series["Series" + Cnt.ToString()].Name = li.Value + "_Yield";
                    ChartAll.Series[li.Value + "_Yield"].ValueMemberX = "PRODUCT_ID";
                    ChartAll.Series[li.Value + "_Yield"].ValueMembersY = li.Value + "_Yield";
                    ChartAll.Series[li.Value + "_Yield"].Type = SeriesChartType.Line;
                    ChartAll.Series[li.Value + "_Yield"].YAxisType = AxisType.Secondary;
                    ChartAll.Series[li.Value + "_Yield"].MarkerStyle = MarkerStyle.Triangle;
                    ChartAll.Series[li.Value + "_Yield"].XValueType = ChartValueTypes.String;
                    ChartAll.Series[li.Value + "_Yield"].ToolTip = " #VALX:  #VALY{##,##0.##}";
                }
            }
            ChartAll.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartAll.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            ChartAll.Titles[0].Color = Color.Blue;
            ChartAll.Titles[0].Text = tbFABName.Text + " BEOL All Light On Yield";

            ChartAll.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartAll.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
            ChartAll.ChartAreas["Default"].AxisX.Interval = 1;
            ChartAll.ChartAreas["Default"].AxisX.Title = "PRODUCT_ID";
            ChartAll.ChartAreas["Default"].AxisY.Title = "Chip_Qty";
            ChartAll.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
            ChartAll.ChartAreas["Default"].AxisY2.Title = "Yield(%) ";
            ChartAll.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##";
            ChartAll.ChartAreas["Default"].AxisY.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartAll.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.White;
            ChartAll.ChartAreas["Default"].AxisY2.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartAll.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = Color.White;
            ChartAll.ChartAreas["Default"].AxisX.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartAll.ChartAreas["Default"].AxisX.MajorGrid.LineColor = Color.White;

            ChartAll.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);

            ChartAll.UI.Toolbar.Enabled = true;
            ChartAll.UI.Toolbar.Position.Width = 28;
            ChartAll.UI.Toolbar.BorderStyle = 0;

            ChartAll.Visible = true;
            ChartAll.DataBind();
        }
    }
    protected void QueryDataGroupByProdID()
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        dtCYield.DefaultView.RowFilter = "";
        DataRow[] drChart = dtCYield.Select("ReportDT='TOTAL' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND SHIFTID <> 'TOTAL' ", "ItemNo");
        if (drChart.Length > 0)
        {
            ChartProdID.Series.Clear();
            ChartProdID.DataSource = "";
            ChartProdID.Series.Clear();

            ChartProdID.Series.Add("Series1");
            ChartProdID.Titles.Add("Title1");
            ChartProdID.DataSource = drChart;

            ChartProdID.Series["Series1"].Color = System.Drawing.Color.FromArgb(71, 94, 118);
            ChartProdID.Series["Series1"].BackGradientType = GradientType.VerticalCenter;
            ChartProdID.Series["Series1"].Name = "Chip_Qty";
            ChartProdID.Series["Chip_Qty"].ValueMemberX = "SHIFTID";
            ChartProdID.Series["Chip_Qty"].ValueMembersY = "Chip_Qty";
            ChartProdID.Series["Chip_Qty"].XValueType = ChartValueTypes.String;
            ChartProdID.Series["Chip_Qty"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            int Cnt = 1;
            foreach (ListItem li in this.cblLevel.Items)
            {
                if (li.Selected)
                {
                    Cnt = Cnt + 1;
                    if (Cnt == 2) { ChartProdID.Series.Add("Series2"); ChartProdID.Series["Series2"].Color = Color.Red; }
                    if (Cnt == 3) { ChartProdID.Series.Add("Series3"); ChartProdID.Series["Series3"].Color = Color.Lime; }
                    if (Cnt == 4) { ChartProdID.Series.Add("Series4"); ChartProdID.Series["Series4"].Color = Color.Olive; }
                    if (Cnt == 5) { ChartProdID.Series.Add("Series5"); ChartProdID.Series["Series4"].Color = Color.White; }

                    ChartProdID.Series["Series" + Cnt.ToString()].Name = li.Value + "_Yield";
                    ChartProdID.Series[li.Value + "_Yield"].ValueMemberX = "SHIFTID";
                    ChartProdID.Series[li.Value + "_Yield"].ValueMembersY = li.Value + "_Yield";
                    ChartProdID.Series[li.Value + "_Yield"].Type = SeriesChartType.Line;
                    ChartProdID.Series[li.Value + "_Yield"].YAxisType = AxisType.Secondary;
                    ChartProdID.Series[li.Value + "_Yield"].MarkerStyle = MarkerStyle.Triangle;
                    ChartProdID.Series[li.Value + "_Yield"].XValueType = ChartValueTypes.String;
                    ChartProdID.Series[li.Value + "_Yield"].ToolTip = " #VALX:  #VALY{##,##0.##}";
                }
            }

            ChartProdID.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartProdID.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            ChartProdID.Titles[0].Color = Color.Blue;
            ChartProdID.Titles[0].Text = ddlByProdID.SelectedValue + " Yield By ShiftID Chart";

            ChartProdID.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartProdID.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
            ChartProdID.ChartAreas["Default"].AxisX.Interval = 1;
            ChartProdID.ChartAreas["Default"].AxisX.Title = "SHIFTID";
            ChartProdID.ChartAreas["Default"].AxisY.Title = "Chip_Qty";
            ChartProdID.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
            ChartProdID.ChartAreas["Default"].AxisY2.Title = "Yield(%) ";
            ChartProdID.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##";

            ChartProdID.ChartAreas["Default"].AxisY.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartProdID.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.White;
            ChartProdID.ChartAreas["Default"].AxisY2.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartProdID.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = Color.White;
            ChartProdID.ChartAreas["Default"].AxisX.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartProdID.ChartAreas["Default"].AxisX.MajorGrid.LineColor = Color.White;

            ChartProdID.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);

            ChartProdID.UI.Toolbar.Enabled = true;
            ChartProdID.UI.Toolbar.Position.Width = 28;
            ChartProdID.UI.Toolbar.BorderStyle = 0;

            ChartProdID.Visible = true;
            ChartProdID.DataBind();
        }
    }
    protected void QueryDataGroupByReportDT()
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        dtCYield.DefaultView.RowFilter = "";
        DataRow[] drChart = dtCYield.Select("ReportDT<>'TOTAL' AND OPERATION_ID='" + tbType.Text + "' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "'  AND SHIFTID= 'TOTAL' ", "ReportDT");
        if (drChart.Length > 0)
        {
            ChartDT.Series.Clear();
            ChartDT.DataSource = "";
            ChartDT.Series.Clear();

            ChartDT.Series.Add("Series1");
            ChartDT.Titles.Add("Title1");
            ChartDT.DataSource = drChart;

            ChartDT.Series["Series1"].Color = System.Drawing.Color.FromArgb(71, 94, 118);
            ChartDT.Series["Series1"].BackGradientType = GradientType.VerticalCenter;
            ChartDT.Series["Series1"].Name = "Chip_Qty";
            ChartDT.Series["Chip_Qty"].ValueMemberX = "ReportDT";
            ChartDT.Series["Chip_Qty"].ValueMembersY = "Chip_Qty";
            ChartDT.Series["Chip_Qty"].XValueType = ChartValueTypes.String;
            ChartDT.Series["Chip_Qty"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            int Cnt = 1;
            foreach (ListItem li in this.cblLevel.Items)
            {
                if (li.Selected)
                {
                    Cnt = Cnt + 1;
                    if (Cnt == 2) { ChartDT.Series.Add("Series2"); ChartDT.Series["Series2"].Color = Color.Red; }
                    if (Cnt == 3) { ChartDT.Series.Add("Series3"); ChartDT.Series["Series3"].Color = Color.Lime; }
                    if (Cnt == 4) { ChartDT.Series.Add("Series4"); ChartDT.Series["Series4"].Color = Color.Olive; }
                    if (Cnt == 5) { ChartDT.Series.Add("Series5"); ChartDT.Series["Series4"].Color = Color.White; }

                    ChartDT.Series["Series" + Cnt.ToString()].Name = li.Value + "_Yield";
                    ChartDT.Series[li.Value + "_Yield"].ValueMemberX = "ReportDT";
                    ChartDT.Series[li.Value + "_Yield"].ValueMembersY = li.Value + "_Yield";
                    ChartDT.Series[li.Value + "_Yield"].Type = SeriesChartType.Line;
                    ChartDT.Series[li.Value + "_Yield"].YAxisType = AxisType.Secondary;
                    ChartDT.Series[li.Value + "_Yield"].MarkerStyle = MarkerStyle.Triangle;
                    ChartDT.Series[li.Value + "_Yield"].XValueType = ChartValueTypes.String;
                    ChartDT.Series[li.Value + "_Yield"].ToolTip = " #VALX:  #VALY{##,##0.##}";
                }
            }

            ChartDT.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartDT.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            ChartDT.Titles[0].Color = Color.Blue;
            ChartDT.Titles[0].Text = ddlByProdID.SelectedValue + " Yield By Time Chart";

            ChartDT.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartDT.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
            ChartDT.ChartAreas["Default"].AxisX.Interval = 1;
            ChartDT.ChartAreas["Default"].AxisX.Title = "ReportDT";
            ChartDT.ChartAreas["Default"].AxisY.Title = "Chip_Qty";
            ChartDT.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
            ChartDT.ChartAreas["Default"].AxisY2.Title = "Yield(%) ";
            ChartDT.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##";

            ChartDT.ChartAreas["Default"].AxisY.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartDT.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.White;
            ChartDT.ChartAreas["Default"].AxisY2.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartDT.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = Color.White;
            ChartDT.ChartAreas["Default"].AxisX.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartDT.ChartAreas["Default"].AxisX.MajorGrid.LineColor = Color.White;

            ChartDT.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);

            ChartDT.UI.Toolbar.Enabled = true;
            ChartDT.UI.Toolbar.Position.Width = 28;
            ChartDT.UI.Toolbar.BorderStyle = 0;

            ChartDT.Visible = true;
            ChartDT.DataBind();
        }
    }
    protected void QueryDataGroupBySDefectCode()
    {
        DataTable dtCYDef = (DataTable)Session["dtCYDef"];
       // DataRow[] drChart = dtCYDef.Select("ReportDT='Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = 'S' ", "ReportDT,DefectRate desc");  V1.03 Mark
        DataRow[] drChart = dtCYDef.Select("ReportDT='Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = '" + ddlByGrade.SelectedValue + "' ", "ReportDT,DefectRate desc");  //V1.03 ADD
        if (drChart.Length > 0)
        {
            ChartSDefectCode.Series.Clear();
            ChartSDefectCode.DataSource = "";
            ChartSDefectCode.Series.Clear();

            ChartSDefectCode.Series.Add("Series1");
            ChartSDefectCode.Series.Add("Series2");
            ChartSDefectCode.Titles.Add("Title1");
            ChartSDefectCode.DataSource = drChart;

            ChartSDefectCode.Series["Series1"].Color = System.Drawing.Color.FromArgb(71, 94, 118);
            ChartSDefectCode.Series["Series1"].BackGradientType = GradientType.VerticalCenter;
            ChartSDefectCode.Series["Series1"].Name = "Defect_Qty";
            ChartSDefectCode.Series["Defect_Qty"].ValueMemberX = "DEFCODE";
            ChartSDefectCode.Series["Defect_Qty"].ValueMembersY = "TTL_CNT";
            ChartSDefectCode.Series["Defect_Qty"].XValueType = ChartValueTypes.String;
            ChartSDefectCode.Series["Defect_Qty"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            ChartSDefectCode.Series["Series2"].Color = Color.Red;
            ChartSDefectCode.Series["Series2"].Name = "DefectRate";

            ChartSDefectCode.Series["DefectRate"].ValueMemberX = "DEFCODE";
            ChartSDefectCode.Series["DefectRate"].ValueMembersY = "DefectRate";
            ChartSDefectCode.Series["DefectRate"].XValueType = ChartValueTypes.String;
            ChartSDefectCode.Series["DefectRate"].YValueType = ChartValueTypes.Double;
            ChartSDefectCode.Series["DefectRate"].YAxisType = AxisType.Secondary;
            ChartSDefectCode.Series["DefectRate"].MarkerStyle = MarkerStyle.Triangle;
            ChartSDefectCode.Series["DefectRate"].ToolTip = " #VALX:  #VALY{##,##0.##}";
            ChartSDefectCode.Series["DefectRate"].Type = SeriesChartType.Line;

            ChartSDefectCode.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartSDefectCode.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            ChartSDefectCode.Titles[0].Color = Color.Blue;
           // ChartSDefectCode.Titles[0].Text = ddlByProdID.SelectedValue + " S Grade DefectCode Chart"; V1.03 mark
            ChartSDefectCode.Titles[0].Text = ddlByProdID.SelectedValue + " " + ddlByGrade.SelectedValue + " Grade DefectCode Chart"; //V1.03 ADD

            ChartSDefectCode.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartSDefectCode.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
            ChartSDefectCode.ChartAreas["Default"].AxisX.Interval = 1;
            ChartSDefectCode.ChartAreas["Default"].AxisX.Title = "DefectCode";
            ChartSDefectCode.ChartAreas["Default"].AxisY.Title = "Defect_Qty";
            ChartSDefectCode.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
            ChartSDefectCode.ChartAreas["Default"].AxisY2.Title = "DefectRate(%) ";
            ChartSDefectCode.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##";

            ChartSDefectCode.ChartAreas["Default"].AxisY.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartSDefectCode.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.White;
            ChartSDefectCode.ChartAreas["Default"].AxisY2.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartSDefectCode.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = Color.White;
            ChartSDefectCode.ChartAreas["Default"].AxisX.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartSDefectCode.ChartAreas["Default"].AxisX.MajorGrid.LineColor = Color.White;

            ChartSDefectCode.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);

            ChartSDefectCode.UI.Toolbar.Enabled = true;
            ChartSDefectCode.UI.Toolbar.Position.Width = 28;
            ChartSDefectCode.UI.Toolbar.BorderStyle = 0;

            ChartSDefectCode.Visible = true;
            ChartSDefectCode.DataBind();
        }
    }
    protected void QueryDataGroupBySDefectCodeDT()
    {
        DataTable dtCYDef = (DataTable)Session["dtCYDef"];
        //DataRow[] drChart = dtCYDef.Select("ReportDT<>'Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = 'S' AND DEFCODE= '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "' ", "ReportDT,DefectRate desc");  V1.03 Mark
        DataRow[] drChart = dtCYDef.Select("ReportDT<>'Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = '" + ddlByGrade.SelectedValue.ToString() + "' AND DEFCODE= '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "' ", "ReportDT,DefectRate desc");  //V1.03 ADD
        if (drChart.Length > 0)
        {
            ChartSDefectDistr.Series.Clear();
            ChartSDefectDistr.DataSource = "";
            ChartSDefectDistr.Series.Clear();

            ChartSDefectDistr.Series.Add("Series1");
            ChartSDefectDistr.Series.Add("Series2");
            ChartSDefectDistr.Titles.Add("Title1");
            ChartSDefectDistr.DataSource = drChart;

            ChartSDefectDistr.Series["Series1"].Color = System.Drawing.Color.FromArgb(71, 94, 118);
            ChartSDefectDistr.Series["Series1"].BackGradientType = GradientType.VerticalCenter;
            ChartSDefectDistr.Series["Series1"].Name = "Defect_Qty";
            ChartSDefectDistr.Series["Defect_Qty"].ValueMemberX = "ReportDT";
            ChartSDefectDistr.Series["Defect_Qty"].ValueMembersY = "TTL_CNT";
            ChartSDefectDistr.Series["Defect_Qty"].XValueType = ChartValueTypes.String;
            ChartSDefectDistr.Series["Defect_Qty"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            ChartSDefectDistr.Series["Series2"].Color = Color.Red;
            ChartSDefectDistr.Series["Series2"].Name = "DefectRate";
            ChartSDefectDistr.Series["DefectRate"].ValueMemberX = "ReportDT";
            ChartSDefectDistr.Series["DefectRate"].ValueMembersY = "DefectRate";
            ChartSDefectDistr.Series["DefectRate"].XValueType = ChartValueTypes.String;
            ChartSDefectDistr.Series["DefectRate"].YValueType = ChartValueTypes.Double;
            ChartSDefectDistr.Series["DefectRate"].YAxisType = AxisType.Secondary;
            ChartSDefectDistr.Series["DefectRate"].MarkerStyle = MarkerStyle.Triangle;
            ChartSDefectDistr.Series["DefectRate"].ToolTip = " #VALX:  #VALY{##,##0.##}";
            ChartSDefectDistr.Series["DefectRate"].Type = SeriesChartType.Line;

            ChartSDefectDistr.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartSDefectDistr.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            ChartSDefectDistr.Titles[0].Color = Color.Blue;
          // ChartSDefectDistr.Titles[0].Text ="S Grade DefectCode[" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "] By Time Chart";  V1.03 Mark
            ChartSDefectDistr.Titles[0].Text = ddlByGradeList.SelectedValue.ToString() + " Grade DefectCode[" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "] By Time Chart"; // V1.03 ADD


            ChartSDefectDistr.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartSDefectDistr.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
            ChartSDefectDistr.ChartAreas["Default"].AxisX.Interval = 1;
            ChartSDefectDistr.ChartAreas["Default"].AxisX.Title = "ReportDT";
            ChartSDefectDistr.ChartAreas["Default"].AxisY.Title = "Defect_Qty";
            ChartSDefectDistr.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
            ChartSDefectDistr.ChartAreas["Default"].AxisY2.Title = "DefectRate(%) ";
            ChartSDefectDistr.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##";

            ChartSDefectDistr.ChartAreas["Default"].AxisY.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartSDefectDistr.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.White;
            ChartSDefectDistr.ChartAreas["Default"].AxisY2.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartSDefectDistr.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = Color.White;
            ChartSDefectDistr.ChartAreas["Default"].AxisX.MajorGrid.LineStyle = ChartDashStyle.Dot;
            ChartSDefectDistr.ChartAreas["Default"].AxisX.MajorGrid.LineColor = Color.White;

            ChartSDefectDistr.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);

            ChartSDefectDistr.UI.Toolbar.Enabled = true;
            ChartSDefectDistr.UI.Toolbar.Position.Width = 28;
            ChartSDefectDistr.UI.Toolbar.BorderStyle = 0;

            ChartSDefectDistr.Visible = true;
            ChartSDefectDistr.DataBind();
        }
    }
    protected void GridViewDataBound()
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        DataTable dtCYDef = (DataTable)Session["dtCYDef"];
        dtCYield.DefaultView.RowFilter = "";
        dtCYield.DefaultView.RowFilter = "(ReportDT='Total' AND OPERATION_ID='" + tbType.Text + "' AND SHIFTID='TOTAL')";
        dtCYield.DefaultView.Sort = "ReportDT,ItemNo";
        GridView1.DataSource = dtCYield;
        GridView1.DataBind();

        dtCYield.DefaultView.RowFilter = "";
        dtCYield.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "'  AND SHIFTID <> 'TOTAL' )";
        dtCYield.DefaultView.Sort = "ItemNo";
        GridView2.DataSource = dtCYield;
        GridView2.DataBind();
        dtCYield.DefaultView.RowFilter = "";

        dtCYield.DefaultView.RowFilter = "";
        dtCYield.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND OPERATION_ID='" + tbType.Text + "'  AND SHIFTID= 'TOTAL' )";
        dtCYield.DefaultView.Sort = "ReportDT,ItemNo";
        GridView3.DataSource = dtCYield;
        GridView3.DataBind();
        dtCYield.DefaultView.RowFilter = "";

        dtCYDef.DefaultView.RowFilter = "";
       // dtCYDef.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND LOTGRADE='S')"; V1.03 Mark
        dtCYDef.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND LOTGRADE='" + ddlByGrade.SelectedValue.ToString() + "')";  // V1.03 ADD
        dtCYDef.DefaultView.Sort = "ReportDT,DefectRate DESC";
        GridView4.DataSource = dtCYDef;
        GridView4.DataBind();

        dtCYDef.DefaultView.RowFilter = "";
        //dtCYDef.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND DEFCODE = '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].Trim().ToString() + "' AND LOTGRADE='S')";  V1.03 Mark
        dtCYDef.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND DEFCODE = '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].Trim().ToString() + "' AND LOTGRADE='" + ddlByGrade.SelectedValue.ToString() + "')";  // V1.03 ADD
        dtCYDef.DefaultView.Sort = "ReportDT,DefectRate DESC";
        GridView5.DataSource = dtCYDef;
        GridView5.DataBind();

        dtCYDef.DefaultView.RowFilter = "";
    }
    protected void ddlByProdID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        AppClass Apc = new AppClass();
        ClearChartBound();
        int intSecCnt = 0;
        lblTime01.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtCYield.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtCYield.Select("ReportDT<>'Total'");
        foreach (DataRow drDel in drsDel)
        {
            dtCYield.Rows.Remove(drDel);
        }
        dtCYield.DefaultView.RowFilter = "";
        Session["dtCYield"] = null;
        Session["dtCYield"] = dtCYield;

        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "TOTAL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "SHIFTID");
        ddlSDefList.Items.Clear();
        ddlShiftID.SelectedIndex = 0;

        CreateDefSessionDataTable();
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", "ALL", "");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        ddlByGrade.SelectedValue = "S";     //V1.03 ADD
        ddlByGradeList.SelectedValue = "S"; //V1.03 ADD
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlByProdID", "ProdID:" + ddlByProdID.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime01.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
        lblTime02.Text = "";
        lblTime03.Text = "";  //V1.03 ADD 
        lblTime04.Text = "";
        lblTime05.Text = "";
    }
    protected void cblLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cblLevel.Items.Count >=4)
        {
            int Cnt = 0;
            foreach (ListItem li in this.cblLevel.Items)
            {
                if (li.Selected)
                {
                    Cnt = Cnt + 1;
                }
            }
            if (Cnt >= 4)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('選擇項目過多，不能操作3個，請重新選擇！');</script>");
                cblLevel.SelectedIndex = 0;
            }
        }
        ChartDataBound();
    }
    protected void ChartDataBound()
    {
        lblFabID.Text = tbFABName.Text;
        PageTitleBound();
        ClearChartBound();
        QueryDataGroupByFPY();
        QueryDataGroupByProdID();
        QueryDataGroupByReportDT();
        QueryDataGroupBySDefectCode();
        QueryDataGroupBySDefectCodeDT();
    }
    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        AppClass Apc = new AppClass();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Export-Excel", "", "");
        Response.Clear();
        Response.Buffer = false;
        Response.Charset = "BIG5";
        //Response.ContentType = "Content-Language;content=UTF-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
        this.EnableViewState = false;
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        this.GridView1.RenderControl(oHtmlTextWriter);
        this.GridView2.RenderControl(oHtmlTextWriter);
        this.GridView3.RenderControl(oHtmlTextWriter);
        this.GridView4.RenderControl(oHtmlTextWriter);
        this.GridView5.RenderControl(oHtmlTextWriter);

        Response.Output.Write(oStringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        PMain.Enabled = true;
        ClearDataBound();
        AppClass Apc = new AppClass();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Clear", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btClear_Click", "", "");
    }
    protected void ddlSDefList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCYDef = (DataTable)Session["dtCYDef"];
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        lblTime02.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtCYDef.DefaultView.RowFilter = "";
       // DataRow[] drsDel = dtCYDef.Select("ReportDT<>'Total' AND LOTGRADE = 'S' ");  V1.03 Mark
        DataRow[] drsDel = dtCYDef.Select("ReportDT<>'Total' AND LOTGRADE = '" + ddlByGradeList.SelectedValue.ToString() + "' ");   // V1.03 ADD
        foreach (DataRow drDel in drsDel)
        {
            dtCYDef.Rows.Remove(drDel);
        }
        dtCYDef.DefaultView.RowFilter = "";
        Session["dtCYDef"] = null;
        Session["dtCYDef"] = dtCYDef;

       // QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", "S", "");  V1.03 Mark
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", ddlByGradeList.SelectedValue.ToString(), "");   // V1.03 ADD
        GridViewDataBound();
        ChartDataBound();
       // Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlSDefList", "S Grade DefectCode:" + ddlSDefList.SelectedValue.ToString(), "");  V1.03 Mark
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlSDefList", ddlByGrade.SelectedValue.ToString() + " Grade DefectCode:" + ddlSDefList.SelectedValue.ToString(), "");  // V1.03 ADD

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime02.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
        lblTime03.Text = ""; //V1.03 ADD 
    }
    protected void ddlShiftID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCYDef = (DataTable)Session["dtCYDef"];
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        lblTime04.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtCYDef.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtCYDef.Select("");
        foreach (DataRow drDel in drsDel)
        {
            dtCYDef.Rows.Remove(drDel);
        }
        dtCYDef.DefaultView.RowFilter = "";
        Session["dtCYDef"] = null;
        Session["dtCYDef"] = dtCYDef;

        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, ddlShiftID.SelectedValue.ToString(), "ALL", "");
       // QueryDefectDataBound("Days", ddlByProdID.SelectedValue, ddlShiftID.SelectedValue.ToString(), "S", "");  V1.03 Mark
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, ddlShiftID.SelectedValue.ToString(), ddlByGrade.SelectedValue.ToString(), "");  // V1.03 ADD

        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlShiftID", "ShiftID:" + ddlShiftID.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime04.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
    }
    protected void ChartAll_Click(object sender, ImageClickEventArgs e)
    {
        HitTestResult HTR = ChartAll.HitTest(e.X, e.Y);
        if (HTR.ChartElementType == ChartElementType.DataPoint)
        {
            if (ChartAll.Series[HTR.Series.Name.ToString()].Label == "")
            {
                ChartAll.Series[HTR.Series.Name.ToString()].Label = " #VALY{##,##0.##}";
                ChartAll.Series[HTR.Series.Name.ToString()].LabelBackColor = Color.White;
            }
            else
            {
                ChartAll.Series[HTR.Series.Name.ToString()].Label = "";
            }
        }
    }
    protected void ChartProdID_Click(object sender, ImageClickEventArgs e)
    {
        HitTestResult HTR = ChartProdID.HitTest(e.X, e.Y);
        if (HTR.ChartElementType == ChartElementType.DataPoint)
        {
            if (ChartProdID.Series[HTR.Series.Name.ToString()].Label == "")
            {
                ChartProdID.Series[HTR.Series.Name.ToString()].Label = " #VALY{##,##0.##}";
                ChartProdID.Series[HTR.Series.Name.ToString()].LabelBackColor = Color.White;
            }
            else
            {
                ChartProdID.Series[HTR.Series.Name.ToString()].Label = "";
            }
        }
    }
    protected void ChartDT_Click(object sender, ImageClickEventArgs e)
    {
        HitTestResult HTR = ChartDT.HitTest(e.X, e.Y);
        if (HTR.ChartElementType == ChartElementType.DataPoint)
        {
            if (ChartDT.Series[HTR.Series.Name.ToString()].Label == "")
            {
                ChartDT.Series[HTR.Series.Name.ToString()].Label = " #VALY{##,##0.##}";
                ChartDT.Series[HTR.Series.Name.ToString()].LabelBackColor = Color.White;
            }
            else
            {
                ChartDT.Series[HTR.Series.Name.ToString()].Label = "";
            }
        }
    }
    protected void ChartSDefectCode_Click(object sender, ImageClickEventArgs e)
    {
        HitTestResult HTR = ChartSDefectCode.HitTest(e.X, e.Y);
        if (HTR.ChartElementType == ChartElementType.DataPoint)
        {
            if (ChartSDefectCode.Series[HTR.Series.Name.ToString()].Label == "")
            {
                ChartSDefectCode.Series[HTR.Series.Name.ToString()].Label = " #VALY{##,##0.##}";
                ChartSDefectCode.Series[HTR.Series.Name.ToString()].LabelBackColor = Color.White;
            }
            else
            {
                ChartSDefectCode.Series[HTR.Series.Name.ToString()].Label = "";
            }
        }
    }
    protected void ChartSDefectDistr_Click(object sender, ImageClickEventArgs e)
    {
        HitTestResult HTR = ChartSDefectDistr.HitTest(e.X, e.Y);
        if (HTR.ChartElementType == ChartElementType.DataPoint)
        {
            if (ChartSDefectDistr.Series[HTR.Series.Name.ToString()].Label == "")
            {
                ChartSDefectDistr.Series[HTR.Series.Name.ToString()].Label = " #VALY{##,##0.##}";
                ChartSDefectDistr.Series[HTR.Series.Name.ToString()].LabelBackColor = Color.White;
            }
            else
            {
                ChartSDefectDistr.Series[HTR.Series.Name.ToString()].Label = "";
            }
        }
    }
    protected void ChartLevelDefectCode_Click(object sender, ImageClickEventArgs e)
    {
        HitTestResult HTR = ChartLevelDefectCode.HitTest(e.X, e.Y);
        if (HTR.ChartElementType == ChartElementType.DataPoint)
        {
            if (ChartLevelDefectCode.Series[HTR.Series.Name.ToString()].Label == "")
            {
                ChartLevelDefectCode.Series[HTR.Series.Name.ToString()].Label = " #VALY{##,##0.##}";
                ChartLevelDefectCode.Series[HTR.Series.Name.ToString()].LabelBackColor = Color.White;
            }
            else
            {
                ChartLevelDefectCode.Series[HTR.Series.Name.ToString()].Label = "";
            }
        }
    }
    /*V1.03 ADD Begin*/
    protected void ddlByGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        AppClass Apc = new AppClass();
        ClearChartBound();
        int intSecCnt = 0;
        lblTime03.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtCYield.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtCYield.Select("ReportDT<>'Total'");
        foreach (DataRow drDel in drsDel)
        {
            dtCYield.Rows.Remove(drDel);
        }
        dtCYield.DefaultView.RowFilter = "";
        Session["dtCYield"] = null;
        Session["dtCYield"] = dtCYield;

        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "TOTAL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "SHIFTID");
        ddlSDefList.Items.Clear();
        ddlShiftID.SelectedIndex = 0;

        CreateDefSessionDataTable();
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", ddlByGrade.SelectedValue.ToString(), "");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", ddlByGrade.SelectedValue.ToString(), "");
        ddlByGradeList.SelectedValue = ddlByGrade.SelectedValue.ToString();
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlByGrade", "LotLevel:" + ddlByGrade.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime03.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
        lblTime02.Text = "";
        lblTime01.Text = "";  
        lblTime04.Text = "";
        lblTime05.Text = "";

    }
    protected void ddlByGradeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCYield = (DataTable)Session["dtCYield"];
        AppClass Apc = new AppClass();
        ClearChartBound();
        int intSecCnt = 0;
        lblTime02.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtCYield.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtCYield.Select("ReportDT<>'Total'");
        foreach (DataRow drDel in drsDel)
        {
            dtCYield.Rows.Remove(drDel);
        }
        dtCYield.DefaultView.RowFilter = "";
        Session["dtCYield"] = null;
        Session["dtCYield"] = dtCYield;

        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "TOTAL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "SHIFTID");
        ddlSDefList.Items.Clear();
        ddlShiftID.SelectedIndex = 0;

        CreateDefSessionDataTable();
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", ddlByGradeList.SelectedValue.ToString(), "");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", ddlByGradeList.SelectedValue.ToString(), "");
        ddlByGrade.SelectedValue = ddlByGradeList.SelectedValue.ToString();
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlByGradeList", "LotLevel:" + ddlByGradeList.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime02.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
        lblTime03.Text = "";
        lblTime01.Text = "";
        lblTime04.Text = "";
        lblTime05.Text = "";
    }
    /*V1.03 ADD End*/
    /*V1.05 Add Start*/
    protected void ddlGCraft_SelectedIndexChanged(object sender, EventArgs e)
    {
        DBConn dbconn = new DBConn(tbFABID.Text);
        DataSet dsgetSoFab = new DataSet();
        string strSQL = "";
        switch (ddlGCraft.SelectedIndex)
        {
            case 0:
                PGCraft.Visible = false;
                cblGCraft.Items.Clear();
                cblSoFab.Items.Clear();
                break;
            case 1:
                PGCraft.Visible = true;
                cblGCraft.Items.Clear();
                cblSoFab.Items.Clear();
                strSQL = "SELECT DISTINCT SOURCEFAB FROM MSOFABMLCDM WHERE HEADFAB='TN'";
                dsgetSoFab = dbconn.GetDataSetBySql(strSQL);
                for (int i = 0; i < dsgetSoFab.Tables[0].Rows.Count; i++)
                {
                    cblSoFab.Items.Add(dsgetSoFab.Tables[0].Rows[i][0].ToString());
                }
                break;
            case 2:
                PGCraft.Visible = true;
                cblGCraft.Items.Clear();
                cblSoFab.Items.Clear();
                strSQL = "SELECT DISTINCT SOURCEFAB FROM MSOFABMLCDM WHERE HEADFAB='T3'";
                dsgetSoFab = dbconn.GetDataSetBySql(strSQL);
                for (int i = 0; i < dsgetSoFab.Tables[0].Rows.Count; i++)
                {
                    cblSoFab.Items.Add(dsgetSoFab.Tables[0].Rows[i][0].ToString());
                }
                break;
            default: break;
        }
        ChangeHeight();
    }
    //替換如下控件的功能

    protected void cblSoFab_SelectedIndexChanged(object sender, EventArgs e)
    {
        cblGCraft.Items.Clear();
        DBConn dbconn = new DBConn(tbFABID.Text);
        DataSet dsGetGlassCraft = new DataSet();
        string strSoFabId = "'='";
        foreach (ListItem ltitem in cblSoFab.Items)
        {
            if (ltitem.Selected)
            {
                strSoFabId += ",'" + ltitem.Text + "'";
            }
        }
        string strSQL = " SELECT DISTINCT GLASS_CRAFT FROM MSOFABMLCDM WHERE HEADFAB='" + ddlGCraft.SelectedItem.Text + "' AND SourceFAB IN (" + strSoFabId + ")";
        dsGetGlassCraft = dbconn.GetDataSetBySql(strSQL);
        for (int i = 0; i < dsGetGlassCraft.Tables[0].Rows.Count; i++)
        {
            ListItem lstmtemp = new ListItem();
            lstmtemp.Text = dsGetGlassCraft.Tables[0].Rows[i][0].ToString();
            
            cblGCraft.Items.Add(lstmtemp);
        }
    }
    /*V1.05 Add End*/
    protected void ChkMerge_CheckedChanged(object sender, EventArgs e)
    {
        DataBoundProdID();
    }
}
