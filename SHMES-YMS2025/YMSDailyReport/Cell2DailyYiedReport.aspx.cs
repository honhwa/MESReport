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
V1.01 2014/05/16    QingQing  ADD TOP6 TO TOP10
 * 2014-07-09  Innolux/SSC     V1.03      Add Grade A1,A2,L2,LR,BP,PC,PT,P5      V1.035  Modify
 * 2014-07-15  Innolux/SSC     V1.04      Add Station 808A,808B,8082,8085
 * 2014-12-04  Innolux/Runy    V1.05      Add LotType條件選擇
 * 2018-12-26  Innolux/SSC     V2.00      Add Station 808T
 * 2019-12-11  LiZiQi               V2.01      Add Station 808E(Linecheck)
*/
public partial class YMSReport_ModuleDailyYield : System.Web.UI.Page
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
                tbFABID.Text = this.Request.QueryString["FABID"].ToString();
                if (tbFABID.Text == "A")
                {
                    tbFABName.Text = "LCM1";
                }
                if (tbFABID.Text == "B")
                {
                    tbFABName.Text = "LCM2";
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
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('廠別資訊不正確,無法使用！');</script>");
                Page.Response.Redirect(@"~\Default.aspx");
                return;
            }
            lblFabID.Text = tbFABName.Text;
            lblTTime.Text = "";
            //lblTime01.Text = "";
            //lblTime02.Text = "";
            //lblTime03.Text = "";
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

        if (Apc.QuerySubject("Cell2DailYiedReport", "Cell2DailYiedReport", out strTitle, out strCon, out strDateTime) == true)
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
    protected void PageTitleBound()
    {
        Page.Title = tbFABName.Text + " CELL2 Daily Yield Report";
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
        cblWTLTM.DataSource = ds;
        cblWTLTM.DataTextField = "WONBR";
        cblWTLTM.DataValueField = "WONBR";
        cblWTLTM.DataBind();
        ds.Dispose();
    }
    protected void DataBoundProdID()
    {
        string strSql = "";
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        strSql = "";
        strSql = "SELECT PRODID FROM MPIM WHERE FABID = '" + tbFABID.Text + "' AND PRODID NOT LIKE 'V%' AND PRODID NOT LIKE '#%' ORDER BY PRODID";
        DataSet ds = new DataSet();
        ds = dbConn.GetDataSetBySql(strSql, "");
        cblProdID.DataSource = ds;
        cblProdID.DataTextField = "PRODID";
        cblProdID.DataValueField = "PRODID";
        cblProdID.DataBind();
        ds.Dispose();
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
        //strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8020','8040','8050','8060','8080','8081','8090') ORDER BY WORKCTR,SUBLINEID";  /*V1.00*//*V1.02*/  V1.04 Mark
        strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090') ORDER BY WORKCTR,SUBLINEID"; // V1.04 ADD   V2.00 /*V2.01*/
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

        Session["dtCFPY"] = null;
        Session["dtCDef"] = null;

        PMain.Enabled = true;
        PMain.Height = 60;

        ChangeHeight();
        PageTitleBound();

       
        GridView1.Dispose();
        GridView1.DataBind();
        GC.Collect();
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
        if (ddlWTLTM.SelectedValue == "UserDefine")
        {
            PWTLTM.Visible = true;
            DataBoundWonbr();
        }
        else
        {
            cblWTLTM.Items.Clear();
            PWTLTM.Visible = false;
        }
        ClearDataBound();
        ChangeHeight();
    }
    protected void ChangeHeight()
    {
        if (PSubLineID.Visible == false && PLotType.Visible == false && PProdID.Visible == false && PWTLTM.Visible == false)
        {
            PMain.Height = 60;
        }
        else
        {
            PMain.Height = 200;
        }
    }
   
    protected void CreateYieldSessionDataTable()
    {
        DataTable dtCFPY = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtCFPY.Columns.Add(new DataColumn("ReportDT", typeof(string)));			            /*0 */
        dtCFPY.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));                 /*1 */
        dtCFPY.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			        /*2 */
        dtCFPY.Columns.Add(new DataColumn("Panel_Size", typeof(string)));                   /*3 */
        dtCFPY.Columns.Add(new DataColumn("Chip_Qty", typeof(long)));                       /*4 */
        dtCFPY.Columns.Add(new DataColumn("A_Qty", typeof(long)));                          /*5 */
        dtCFPY.Columns.Add(new DataColumn("A_Yield", typeof(double)));                      /*6 */
        dtCFPY.Columns.Add(new DataColumn("S_Yield", typeof(double)));                      /*7 */
        dtCFPY.Columns.Add(new DataColumn("AA_Yield", typeof(double)));                     /*8 */
        dtCFPY.Columns.Add(new DataColumn("Top1_Code", typeof(string)));                    /*9*/
        dtCFPY.Columns.Add(new DataColumn("Rate01", typeof(double)));                       /*10*/
        dtCFPY.Columns.Add(new DataColumn("Top2_Code", typeof(string)));                    /*11*/
        dtCFPY.Columns.Add(new DataColumn("Rate02", typeof(double)));                       /*12*/
        dtCFPY.Columns.Add(new DataColumn("Top3_Code", typeof(string)));                    /*13*/
        dtCFPY.Columns.Add(new DataColumn("Rate03", typeof(double)));                       /*14*/
        dtCFPY.Columns.Add(new DataColumn("Top4_Code", typeof(string)));                    /*15*/
        dtCFPY.Columns.Add(new DataColumn("Rate04", typeof(double)));                       /*16*/
        dtCFPY.Columns.Add(new DataColumn("Top5_Code", typeof(string)));                    /*17*/
        dtCFPY.Columns.Add(new DataColumn("Rate05", typeof(double)));                       /*18*/
        dtCFPY.Columns.Add(new DataColumn("Top6_Code", typeof(string)));                     /*23*//*1.01 ADD Begin*/
        dtCFPY.Columns.Add(new DataColumn("Rate06", typeof(double)));                        /*24*/
        dtCFPY.Columns.Add(new DataColumn("Top7_Code", typeof(string)));                     /*25*/
        dtCFPY.Columns.Add(new DataColumn("Rate07", typeof(double)));                        /*26*/
        dtCFPY.Columns.Add(new DataColumn("Top8_Code", typeof(string)));                     /*27*/
        dtCFPY.Columns.Add(new DataColumn("Rate08", typeof(double)));                        /*28*/
        dtCFPY.Columns.Add(new DataColumn("Top9_Code", typeof(string)));                     /*29*/
        dtCFPY.Columns.Add(new DataColumn("Rate09", typeof(double)));                        /*30*/
        dtCFPY.Columns.Add(new DataColumn("Top10_Code", typeof(string)));                    /*31*/
        dtCFPY.Columns.Add(new DataColumn("Rate10", typeof(double)));                        /*32*//*1.01 ADD End*/
        dtCFPY.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] keys = new DataColumn[4];
        keys[0] = dtCFPY.Columns["ReportDT"];
        keys[1] = dtCFPY.Columns["PRODUCT_ID"];
        keys[2] = dtCFPY.Columns["OPERATION_ID"];
        keys[3] = dtCFPY.Columns["Panel_Size"];
        dtCFPY.PrimaryKey = keys;
        /*--------------------------------------------------------------------------------------*/
        Session["dtCFPY"] = null;
        Session["dtCFPY"] = dtCFPY;

    }

    protected void btQuery_Click(object sender, EventArgs e)
    {
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        string strSubLineID = "'='";
        string strProdID = "'='";
        string strLotType = "'='";
        string strWonbr = "'='";
        lblTTime.Text = "";
        DateTime dtF = System.DateTime.Now;
        /*--------------------------------------------------------------------------------------*/
        tbDateType.Text = "";
        tbDiffDate.Text = "0";
        tbCPFlag.Text = "";
        tbFDate.Text = "";
        tbTDate.Text = "";

        lblFabID.Text = tbFABName.Text;

        Session["dtCFPY"] = null;
        Session["dtCDef"] = null;


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
        if (cblWTLTM.Items.Count > 0)
        {
            foreach (ListItem li in this.cblWTLTM.Items)
            {
                if (li.Selected)
                {
                    strWonbr = strWonbr + "," + "'" + li.Text + "'";
                }
            }
        }
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
        //if (tbDateType.Text == "W") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportWeek >= " + tbFDate.Text + " AND ReportWeek <= " + tbTDate.Text + " "; }
        if (tbDateType.Text == "W")
        { 
            tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportWeek >= " + tbFDate.Text + " AND ReportWeek <= " + tbTDate.Text + " ";
        }
        if (tbDateType.Text == "M") { tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportYear = '" + ddlYear.SelectedValue + "' AND ReportMonth >= " + tbFDate.Text + " AND ReportMonth <= " + tbTDate.Text + " "; }
        //V1.05 Add Start
        tbSqlWhere.Text = "";
        if (strLotType != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND LOT_TYPE IN(" + strLotType + ")"; }
        //V1.05 Add End
        /*-------------------------------------------------------------------------------------*/
        CreateYieldSessionDataTable();
        /*--------------------------------------------------------------------------------------*/
        //QueryFPYDataBound("TOTAL","ALL");
        //GridViewDataBound();
        BYPRODDataBound();
        BYWTLTMDataBound();

        if (ddlTYPE.Text == "13碼料號")
        {
            QueryFPYDataBound("PRODID", "ALL");
            GridViewDataBound();
        }
        else if (ddlTYPE.Text == "PanelSize")
        {
            QueryFPYDataBound("PANELSIZE", "ALL");
            GridViewDataBound();
        }
        else if (ddlTYPE.Text == "13碼料號+PanelSize")
        {
            QueryFPYDataBound("13碼料號+PanelSize", "ALL");
            GridViewDataBound();
        }
        DataTable dtTmp = (DataTable)Session["dtCFPY"];
        string strLogWhere = "DateType:" + ddlDateType.SelectedValue.ToString() +
                             ";From:" + tbFDate.Text.ToString() +
                             ";To:" + tbTDate.Text.ToString() +
                             ";Year:" + ddlYear.SelectedValue.ToString() +
                             ";Cell2Prodid:" + cbCPFlag.Checked.ToString() +
                             ";Length:" + ddlPdLen.SelectedValue.ToString() +
                             ";ShiftID:" + ddlShiftID.SelectedValue.ToString() +
                             ";SelType:" + ddlSelType.SelectedValue.ToString() +        /*V1.01*/
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

    }
    protected void BYPRODDataBound()
    {
        StringBuilder strSql = new StringBuilder();
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());

        /*-------------------------------------------------------------------------------------*/
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
        strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");    /*V1.00*/  /*V1.04 ADD   V2.00 */  /*V2.01*/ 
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
    protected void BYWTLTMDataBound()
    {
        StringBuilder strSql = new StringBuilder();
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        /*-------------------------------------------------------------------------------------*/
        /*WTLTM 取得前廠的信息*/
        strSql.Append("SELECT DISTINCT CASE WHEN LEFT(AR_FactoryName,2)='TN' THEN AR_FactoryName+'_'+SUBSTRING(LCD_PRODUCT_ID,3,4)");
        strSql.Append(" WHEN  AR_FactoryName IN('JN_T1','JN_T0') THEN AR_FactoryName+''+LCD_PRODUCT_ID  ELSE'' END AS PRODUCT_ID ");
        strSql.Append("FROM New_wtltm  WHERE AR_FactoryName!='' AND AR_FactoryName !='JN_T3'");
        DataSet dt00 = new DataSet();
        SqlDataAdapter sqlApt00 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
        sqlApt00.Fill(dt00);
        if (dt00.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查無資料！');</script>");
            return;
        }
        ddlByWtltm.DataSource = dt00;
        ddlByWtltm.DataTextField = "PRODUCT_ID";
        ddlByWtltm.DataValueField = "PRODUCT_ID";
        ddlByWtltm.DataBind();
        dt00.Dispose();
        sqlApt00.Dispose();
        strSql.Remove(0, strSql.Length);
    }
    protected void QueryFPYDataBound(string strMode, string strTmpProdID)
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            /*-------------------------------------------------------------------------------------*/
            /*機種清單*/
        
            if (strMode == "PRODID")
            {
                strSql.Remove(0, strSql.Length);
                strSql.Append(" SELECT 'TOTAL' AS ReportDT,");
                if (CB13NC.Checked)
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
                }
                else
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");	/*1*/
                }
                strSql.Append(" OPERATION_ID,");     					     /*2 */
                strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");                             /*3 */
                strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");                             /*4 */
                strSql.Append(" SUM(ISNULL(R_CNT,0)) AS R_CNT,");                             /*5 */
                strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT,");                             /*6 */
                strSql.Append(" SUM(ISNULL(AA_CNT,0)) AS AA_CNT,");                           /*7 */
                strSql.Append(" SUM(ISNULL(A0_CNT,0)) AS A0_CNT,");                           /*8 */
                strSql.Append(" SUM(ISNULL(A1_CNT,0)) AS A1_CNT,");                           /*9 */
                strSql.Append(" SUM(ISNULL(A2_CNT,0)) AS A2_CNT,");                           /*10*/
                strSql.Append(" SUM(ISNULL(A3_CNT,0)) AS A3_CNT,");                           /*11*/
                strSql.Append(" SUM(ISNULL(UNDEFINE_CNT,0)) AS UNDEFINE_CNT");                /*12*/
                strSql.Append(" ,B.SIZEID");
                strSql.Append(" , SUM(ISNULL(L2_CNT,0)) AS L2_CNT, ");   /*V1.03 ADD Begin*/   /*14*/
                strSql.Append(" SUM(ISNULL(LR_CNT,0)) AS LR_CNT, ");
                strSql.Append(" SUM(ISNULL(BP_CNT,0)) AS BP_CNT, ");
                strSql.Append(" SUM(ISNULL(PC_CNT,0)) AS PC_CNT, ");
                strSql.Append(" SUM(ISNULL(PT_CNT,0)) AS PT_CNT, ");
                strSql.Append(" SUM(ISNULL(P5_CNT,0)) AS P5_CNT ");    /*V1.03 ADD End*/      /*19*/
                if (CB13NC.Checked)
                {
                    strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT A WITH(NOLOCK)  LEFT JOIN MPIM B ON A.FAB_ID=B.FABID AND A.T9NCNO=B.PRODID");
                }
                else
                {
                    strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT A WITH(NOLOCK)  LEFT JOIN MPIM B ON A.FAB_ID=B.FABID AND A.PRODUCT_ID=B.PRODID");
                }
                strSql.Append(tbSqlDate.Text);
                strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");   /*V1.04 ADD    V2.00 */ /*V2.01*/
                strSql.Append(tbSqlWhere.Text);    //V1.05 取消Mark
                if (CB13NC.Checked)
                {
                    strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + "),B.SIZEID ");
                }
                else
                {
                    strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + "),B.SIZEID ");
                }
            }
            else if (strMode == "PANELSIZE")
            {
                strSql.Remove(0, strSql.Length);
                strSql.Append(" SELECT 'TOTAL' AS ReportDT,");
                strSql.Append(" CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ELSE '' END AS PRODUCT_ID,");/*1*/
                strSql.Append(" OPERATION_ID,");     					     /*2 */
                strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");                             /*3 */
                strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");                             /*4 */
                strSql.Append(" SUM(ISNULL(R_CNT,0)) AS R_CNT,");                             /*5 */
                strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT,");                             /*6 */
                strSql.Append(" SUM(ISNULL(AA_CNT,0)) AS AA_CNT,");                           /*7 */
                strSql.Append(" SUM(ISNULL(A0_CNT,0)) AS A0_CNT,");                           /*8 */
                strSql.Append(" SUM(ISNULL(A1_CNT,0)) AS A1_CNT,");                           /*9 */
                strSql.Append(" SUM(ISNULL(A2_CNT,0)) AS A2_CNT,");                           /*10*/
                strSql.Append(" SUM(ISNULL(A3_CNT,0)) AS A3_CNT,");                           /*11*/
                strSql.Append(" SUM(ISNULL(UNDEFINE_CNT,0)) AS UNDEFINE_CNT");                /*12*/
                strSql.Append(" ,''  as SIZEID");                                              /*13*/
                strSql.Append(" , SUM(ISNULL(L2_CNT,0)) AS L2_CNT, ");   /*V1.03 ADD Begin*/   /*14*/
                strSql.Append(" SUM(ISNULL(LR_CNT,0)) AS LR_CNT, ");
                strSql.Append(" SUM(ISNULL(BP_CNT,0)) AS BP_CNT, ");
                strSql.Append(" SUM(ISNULL(PC_CNT,0)) AS PC_CNT, ");
                strSql.Append(" SUM(ISNULL(PT_CNT,0)) AS PT_CNT, ");
                strSql.Append(" SUM(ISNULL(P5_CNT,0)) AS P5_CNT ");    /*V1.03 ADD End*/      /*19*/
                strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT A WITH(NOLOCK) ");
                strSql.Append(tbSqlDate.Text);
                strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append("  AND PALLETID!='' AND LEFT(PALLETID,5)!='JN_T3'");
                strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");     /*V1.00*/  /*V1.04 ADD    V2.00 */ /*V2.01*/
                strSql.Append(tbSqlWhere.Text);      //V1.05 取消Mark
                strSql.Append(" GROUP BY OPERATION_ID, ");
                strSql.Append(" CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ELSE '' END ");/*1*/    
      
            }
            else if (strMode == "13碼料號+PanelSize")
            {
                strSql.Remove(0, strSql.Length);
                strSql.Append(" SELECT 'TOTAL' AS ReportDT,");
                if (CB13NC.Checked)
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
                }
                else
                {
                    strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");	/*1*/
                }
                strSql.Append(" OPERATION_ID,");     					     /*2 */
                strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");                             /*3 */
                strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");                             /*4 */
                strSql.Append(" SUM(ISNULL(R_CNT,0)) AS R_CNT,");                             /*5 */
                strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT,");                             /*6 */
                strSql.Append(" SUM(ISNULL(AA_CNT,0)) AS AA_CNT,");                           /*7 */
                strSql.Append(" SUM(ISNULL(A0_CNT,0)) AS A0_CNT,");                           /*8 */
                strSql.Append(" SUM(ISNULL(A1_CNT,0)) AS A1_CNT,");                           /*9 */
                strSql.Append(" SUM(ISNULL(A2_CNT,0)) AS A2_CNT,");                           /*10*/
                strSql.Append(" SUM(ISNULL(A3_CNT,0)) AS A3_CNT,");                           /*11*/
                strSql.Append(" SUM(ISNULL(UNDEFINE_CNT,0)) AS UNDEFINE_CNT,");                /*12*/
                strSql.Append(" CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))");
                strSql.Append(" ELSE '' END AS SIZEID ");
                strSql.Append(" , SUM(ISNULL(L2_CNT,0)) AS L2_CNT, ");/*V1.03 ADD Begin*/   /*14*/
                strSql.Append(" SUM(ISNULL(LR_CNT,0)) AS LR_CNT, ");
                strSql.Append(" SUM(ISNULL(BP_CNT,0)) AS BP_CNT, ");
                strSql.Append(" SUM(ISNULL(PC_CNT,0)) AS PC_CNT, ");
                strSql.Append(" SUM(ISNULL(PT_CNT,0)) AS PT_CNT, ");
                strSql.Append(" SUM(ISNULL(P5_CNT,0)) AS P5_CNT ");    /*V1.03 ADD End*/      /*19*/
                strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT A WITH(NOLOCK) ");
                strSql.Append(tbSqlDate.Text);
                strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");     /*V1.00*/  /*V1.04 ADD   V2.00 */ /*V2.01*/
                strSql.Append(tbSqlWhere.Text);  //V1.05 取消Mark
                if (CB13NC.Checked)
                {
                    strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END,1," + ddlPdLen.SelectedValue + "), ");
                }
                else
                {
                    strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + "), ");
                }
                strSql.Append(" CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4))) ");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ELSE '' END ");/*1*/    
      
            }

            DataSet dt011 = new DataSet();
            SqlDataAdapter sqlApt011 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt011.Fill(dt011);
            /**/
            if (strMode == "PRODID")
            {
                strSql.Remove(0, strSql.Length);
                strSql.Append(" SELECT ");
                strSql.Append(" 'TOTAL' AS ReportDT,");
                if (CB13NC.Checked)
                {
                    strSql.Append(" OPERATION_ID,T9NCNO AS PRODUCT_ID,  DEF_CODE,  DEFECTDESC,  SUM(DEF_CNT) AS DEF_CNT");
                }
                else
                {
                    strSql.Append(" OPERATION_ID,PRODUCT_ID,  DEF_CODE,  DEFECTDESC,  SUM(DEF_CNT) AS DEF_CNT");
                }
                strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK) ");
                strSql.Append(tbSqlDate.Text);
                strSql.Append(" AND A.FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");  /*V1.04 ADD   V2.00 */ /*V2.01*/
                strSql.Append(tbSqlWhere.Text);  //V1.05 Add
                strSql.Append(" AND (A.LOT_GRADE IN('R','S') or A.LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");         //V1.035 Modify
                strSql.Append(" AND A.FAB_ID=B.FABID");
                strSql.Append(" AND A.DEF_CODE=B.DEFECTCD ");
                if (CB13NC.Checked)
                {
                    strSql.Append(" GROUP BY OPERATION_ID,T9NCNO, DEF_CODE,  DEFECTDESC  ORDER BY  OPERATION_ID,T9NCNO ,DEF_CNT DESC");
                }
                else
                {
                    strSql.Append(" GROUP BY OPERATION_ID,PRODUCT_ID, DEF_CODE,  DEFECTDESC  ORDER BY  OPERATION_ID,PRODUCT_ID ,DEF_CNT DESC");
                }
            }
            else if (strMode == "PANELSIZE")
            {
                strSql.Remove(0, strSql.Length);
                strSql.Append(" SELECT ");
                strSql.Append(" 'TOTAL' AS ReportDT,");
                strSql.Append(" OPERATION_ID,");
                strSql.Append(" CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4)))");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ");/*1 */
                strSql.Append(" ELSE PALLETID END AS PRODUCT_ID ,");
                strSql.Append(" DEF_CODE,  DEFECTDESC,  SUM(DEF_CNT) AS DEF_CNT");
                strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK) ");
                strSql.Append(tbSqlDate.Text);
                strSql.Append(" AND A.FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append(" AND a.PALLETID!='' AND LEFT(PALLETID,5)!='JN_T3'");
                strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");   /* V1.04 ADD   V2.00 */ /*V2.01*/
                strSql.Append(tbSqlWhere.Text);  //V1.05 Add
                strSql.Append(" AND (A.LOT_GRADE IN('R','S') or A.LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  // V1.035 Modify
                strSql.Append(" AND A.FAB_ID=B.FABID");
                strSql.Append(" AND A.DEF_CODE=B.DEFECTCD ");
                strSql.Append(" GROUP BY OPERATION_ID, DEF_CODE,  DEFECTDESC, ");
                strSql.Append(" CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4)))");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ");/*1 */
                strSql.Append(" ELSE PALLETID END");
                strSql.Append(" ORDER BY  OPERATION_ID ,DEF_CNT DESC");
            }
            else if (strMode == "13碼料號+PanelSize")
            {
                strSql.Remove(0, strSql.Length);
                strSql.Append(" SELECT * FROM (");
                strSql.Append(" SELECT ");
                strSql.Append(" 'TOTAL' AS ReportDT,");
                if (CB13NC.Checked)
                {
                    strSql.Append(" OPERATION_ID,T9NCNO AS PRODUCT_ID,  DEF_CODE,  DEFECTDESC,  SUM(DEF_CNT) AS DEF_CNT");
                }
                else
                {
                    strSql.Append(" OPERATION_ID,PRODUCT_ID,  DEF_CODE,  DEFECTDESC,  SUM(DEF_CNT) AS DEF_CNT");
                }
                strSql.Append(" ,CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4)))");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ");/*1 */
                strSql.Append("ELSE '' END AS SIZEID ");
                strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK) ");
                strSql.Append(tbSqlDate.Text);
                strSql.Append(" AND A.FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append(" AND OPERATION_ID IN('8020','8040','8050','8060','8080','808T','8081','808A','808B','808E','8082','8085','8090')");   /*V1.04 ADD   V2.00 */  /*V2.01*/
                strSql.Append(tbSqlWhere.Text);  //V1.05 Add
                strSql.Append(" AND (A.LOT_GRADE IN('R','S') or A.LOT_LEVEL IN ('A1','A2','L2','LR','BP','PC','PT','P5'))");  // V1.035 Modify
                strSql.Append(" AND A.FAB_ID=B.FABID");
                strSql.Append(" AND A.DEF_CODE=B.DEFECTCD ");
                strSql.Append(" GROUP BY CASE WHEN  LEFT(PALLETID,2)='TN' THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,13,4)))");
                strSql.Append(" WHEN LEFT (PALLETID,5) IN ('JN_T1','JN_T0') THEN LTRIM(RTRIM(SUBSTRING(PALLETID,1,10)))+'_' +LTRIM(RTRIM(SUBSTRING(PALLETID,11,LEN(PALLETID))))  ");/*1 */
                if (CB13NC.Checked)
                {
                    strSql.Append("ELSE '' END, T9NCNO,DEF_CODE,  DEFECTDESC ,OPERATION_ID)A ORDER BY   OPERATION_ID,PRODUCT_ID ,SIZEID,DEF_CNT DESC");
                }
                else
                {
                    strSql.Append("ELSE '' END, PRODUCT_ID,DEF_CODE,  DEFECTDESC ,OPERATION_ID)A ORDER BY   OPERATION_ID,PRODUCT_ID ,SIZEID,DEF_CNT DESC");
                }
            }
            DataSet dt012 = new DataSet();
            SqlDataAdapter sqlApt012 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt012.Fill(dt012);
            /**/
            /*-------------------------------------------------------------------------------------*/
            //BoudFPYDataTable(dt012,dt011);
            if (strMode == "13碼料號+PanelSize")
            {
                BoudFPYDataTable1(dt012, dt011);
            }
            else
            {
                BoudFPYDataTable(dt012, dt011);
            }
            /*---
            /*-------------------------------------------------------------------------------------*/
            dt011.Dispose();
            sqlApt011.Dispose();
            /**/
            dt012.Dispose();
            sqlApt012.Dispose();
            strSql.Remove(0, strSql.Length);
            /**/
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    private void BoudFPYDataTable(DataSet dsTmp1,DataSet dsTmp)
    {
        try
        {
            DataTable dtCFPY = (DataTable)Session["dtCFPY"];
            /*-------------------------------------------------------------------------------------*/
            /*將各站點用編號區分，循環處理這幾個站點的資料*/
            for (int DTCnt = 0; DTCnt <=Convert.ToInt32(tbDiffDate.Text); DTCnt++)
            {
                string strReportDT = "";
                strReportDT = "TOTAL";
                DTCnt = Convert.ToInt32(tbDiffDate.Text);
                /*-------------------------------------------------------------------------------------*/
                /*循環處理各站點的產品的數據*/
                int intcount = 0;
                if (ddlTYPE.Text == "PanelSize")
                {
                    intcount = Convert.ToInt32(ddlByWtltm.Items.Count);
                }
                else
                {
                    intcount = Convert.ToInt32(ddlByProdID.Items.Count);
                }
                for (int k = 0; k < intcount; k++)
                {
                    string strPRODUCTID = "";         /*產品型號*/
                    //strPRODUCTID = ddlByProdID.Items[k].Value.ToString();       /*獲取產品型號*/
                    if (ddlTYPE.Text == "PanelSize")
                    {
                        strPRODUCTID = ddlByWtltm.Items[k].Value.ToString();       /*獲取產品型號*/
                    }
                    else
                    {
                        strPRODUCTID = ddlByProdID.Items[k].Value.ToString();       /*獲取產品型號*/
                    }
                    /*-------------------------------------------------------------------------------------*/
                    for (int i = 0; i <= 12; i++)      /*V1.04  6 to 10   V2.00  11 */  /*V2.01   to 12*/
                    {
                        string strOPERATIONID = "";       /*站點代碼*/
                        string strType = "";       /*站點代碼*/
                        switch (i)
                        {
                          /* V1.04 Mark begin
                            case 6: strOPERATIONID = "8090"; strType = "(8090)FinalClean"; break;
                            case 5: strOPERATIONID = "8081"; strType = "(8081)外觀"; break;
                            case 4: strOPERATIONID = "8080"; strType = "(8080)LOT1"; break;
                            case 3: strOPERATIONID = "8060"; strType = "(8060)RoughClean"; break;
                            case 2: strOPERATIONID = "8050"; strType = "(8050)EndSealing"; break;
                            case 1: strOPERATIONID = "8040"; strType = "(8040)LCInjection"; break;
                            case 0: strOPERATIONID = "8020"; strType = "(8020)SemiScribe"; break;
                           * V1.04 mark End*/
                            case 12: strOPERATIONID = "8090"; strType = "(8090)FinalClean"; break;    /*V1.04 ADD begin*/
                            case 11: strOPERATIONID = "8085"; strType = "(8085)Cell2Packing2"; break;  
                            case 10: strOPERATIONID = "8082"; strType = "(8082)Cell2Packing1"; break;
                            case 9: strOPERATIONID = "808E"; strType = "(808E)Linecheck"; break;    /*V2.01 ADD*/
                            case 8: strOPERATIONID = "808B"; strType = "(808B)外觀_LR"; break;
                            case 7: strOPERATIONID = "808A"; strType = "(808A)LOT1_LR"; break;
                            case 6: strOPERATIONID = "8081"; strType = "(8081)外觀"; break;
                            case 5: strOPERATIONID = "808T"; strType = "(808T)LOT1.5"; break;          //V2.00 
                            case 4: strOPERATIONID = "8080"; strType = "(8080)LOT1"; break;
                            case 3: strOPERATIONID = "8060"; strType = "(8060)RoughClean"; break;
                            case 2: strOPERATIONID = "8050"; strType = "(8050)EndSealing"; break;
                            case 1: strOPERATIONID = "8040"; strType = "(8040)LCInjection"; break;
                            case 0: strOPERATIONID = "8020"; strType = "(8020)SemiScribe"; break;      /*V1.04 ADD End*/
                            default:
                                break;
                        }
                    
                        /*查詢個站點的資料*/
                        DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "')", "PRODUCT_ID DESC");
                        if (drType.Length > 0)
                        {
                            long lngMQty = 0;
                            long lngChipQty = 0;
                            long lngAQty = 0;
                            double dblAYield = 0, dblSYield = 0;
                            /* V1.03 Mark Begin
                            ChipQty=A+R+S
                            AYield=A/(ChipQty-R)
                            RYield=R/(ChipQty-R)
                            SYield=S/(ChipQty-R)
                            AAYield=AA/(ChipQty-R)
                            A?Yield=A?/(ChipQty-R)
                            (AA+A0+A1+A2)Yield=A?/(ChipQty-R)
                            V1.03 Mark End  */

                            /* V1.03 ADD Begin
                            ChipQty=A+R+S-A1-A2-L2-LR-BP-PC-PT-P5
                            AYield=(A-A1-A2-L2-LR-BP-PC-PT-P5)/(ChipQty-R)
                            RYield=R/(ChipQty-R)
                            SYield=S/(ChipQty-R)
                            AAYield=AA/(ChipQty-R)
                            A?Yield=A?/(ChipQty-R)
                            (AA+A0+A1+A2)Yield=A?/(ChipQty-R)
                            V1.03 ADD End  */
                           // lngMQty = Convert.ToInt32(drType[0].ItemArray[3]);  V1.03 mark
                            lngMQty = Convert.ToInt32(drType[0].ItemArray[3]) - Convert.ToInt32(drType[0].ItemArray[9]) - Convert.ToInt32(drType[0].ItemArray[10]) - Convert.ToInt32(drType[0].ItemArray[14]) - Convert.ToInt32(drType[0].ItemArray[15]) - Convert.ToInt32(drType[0].ItemArray[16]) - Convert.ToInt32(drType[0].ItemArray[17]) - Convert.ToInt32(drType[0].ItemArray[18]) - Convert.ToInt32(drType[0].ItemArray[19]);  // V1.03 ADD
                            //lngChipQty = Convert.ToInt32(drType[0].ItemArray[3]); V1.03 Mark
                            lngChipQty = lngMQty;  // V1.03 ADD
                            //lngAQty = Convert.ToInt32(drType[0].ItemArray[4]);  V1.03 Mark
                            lngAQty = Convert.ToInt32(drType[0].ItemArray[4]) - Convert.ToInt32(drType[0].ItemArray[9]) - Convert.ToInt32(drType[0].ItemArray[10]) - Convert.ToInt32(drType[0].ItemArray[14]) - Convert.ToInt32(drType[0].ItemArray[15]) - Convert.ToInt32(drType[0].ItemArray[16]) - Convert.ToInt32(drType[0].ItemArray[17]) - Convert.ToInt32(drType[0].ItemArray[18]) - Convert.ToInt32(drType[0].ItemArray[19]);  // V1.03 ADD
                           // dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4])) / lngMQty, 4) * 100; V1.03 Mark
                            dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4]) - Convert.ToDouble(drType[0].ItemArray[9]) - Convert.ToDouble(drType[0].ItemArray[10]) - Convert.ToDouble(drType[0].ItemArray[14]) - Convert.ToDouble(drType[0].ItemArray[15]) - Convert.ToDouble(drType[0].ItemArray[16]) - Convert.ToDouble(drType[0].ItemArray[17]) - Convert.ToDouble(drType[0].ItemArray[18]) - Convert.ToDouble(drType[0].ItemArray[19])) / lngMQty, 4) * 100;  // V1.03 ADD
                            dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[6])) / lngMQty, 4) * 100;  //V1.03 Modify 5 to 6
                            /*-----------------------------------------------------------------------------------------------------------------------------*/
                            /*處理defecode 數據*/
                            string[] Defe = new string[10]; /*1.01 5 to 10*/
                            double[] Def_QTY = new double[10]; /*1.01 5 to 10*/
                            DataRow[] drTypedef = dsTmp1.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "')");
                            if (drTypedef.Length > 0)
                            {
                             
                                int A = 0;
                                if (drTypedef.Length < 10) /*1.01 5 to 10*/
                                {
                                    A = Convert.ToInt32(drTypedef.Length);
                                }
                                else
                                {
                                    A = 10;/*1.01 5 to 10*/
                                }
                                for (int j = 0; j < A; j++)
                                {
                                    string DefeDesc;
                                    DefeDesc = "";
                                    Defe[j] = null;
                                    Def_QTY[j] = 0;
                                    if (drTypedef[j].ItemArray[4].ToString() == null)
                                    {
                                        DefeDesc = " ";
                                    }
                                    else
                                    {
                                        DefeDesc = drTypedef[j].ItemArray[4].ToString();
                                    }
                                    Defe[j] = drTypedef[j].ItemArray[3].ToString() + "	" + DefeDesc;
                                    Def_QTY[j] = Math.Round(Convert.ToDouble(drTypedef[j].ItemArray[5]) / lngChipQty,4)*100;
                                }
                            }
                            
                            /*-----------------------------------------------------------------------------------------------------------------------------*/
                            DataRow drFPY = dtCFPY.NewRow();
                            drFPY["ReportDT"] = strReportDT;
                            drFPY["OPERATION_ID"] = strType;
                            drFPY["PRODUCT_ID"] = strPRODUCTID;
                            drFPY["Panel_Size"] = Convert.ToString (drType[0].ItemArray[13]);
                            drFPY["Chip_Qty"] = lngChipQty;
                            drFPY["A_Qty"] = lngAQty;
                            drFPY["A_Yield"] = dblAYield;
                            drFPY["S_Yield"] = dblSYield;
                            drFPY["AA_Yield"] = dblAYield;
                            drFPY["Top1_Code"] = Defe[0];
                            drFPY["Rate01"] = Def_QTY[0];
                            drFPY["Top2_Code"] = Defe[1];
                            drFPY["Rate02"] = Def_QTY[1];
                            drFPY["Top3_Code"] = Defe[2];
                            drFPY["Rate03"] = Def_QTY[2];
                            drFPY["Top4_Code"] = Defe[3];
                            drFPY["Rate04"] = Def_QTY[3];
                            drFPY["Top5_Code"] = Defe[4];
                            drFPY["Rate05"] = Def_QTY[4];
                            drFPY["Top6_Code"] = Defe[5];
                            drFPY["Rate06"] = Def_QTY[5];
                            drFPY["Top7_Code"] = Defe[6];
                            drFPY["Rate07"] = Def_QTY[6];
                            drFPY["Top8_Code"] = Defe[7];
                            drFPY["Rate08"] = Def_QTY[7];
                            drFPY["Top9_Code"] = Defe[8];
                            drFPY["Rate09"] = Def_QTY[8];
                            drFPY["Top10_Code"] = Defe[9];
                            drFPY["Rate10"] = Def_QTY[9];
                            dtCFPY.Rows.Add(drFPY);
                            /*-------------------------------------------------------------------------------------*/

                        }
                    }
                }
            }
            Session["dtCFPY"] = null;
            Session["dtCFPY"] = dtCFPY;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void BoudFPYDataTable1(DataSet dsTmp1, DataSet dsTmp)
    {
        try
        {
            DataTable dtCFPY = (DataTable)Session["dtCFPY"];
            /*-------------------------------------------------------------------------------------*/
            /*將各站點用編號區分，循環處理這幾個站點的資料*/
            for (int DTCnt = 0; DTCnt <= Convert.ToInt32(tbDiffDate.Text); DTCnt++)
            {
                string strReportDT = "";
                strReportDT = "TOTAL";
                DTCnt = Convert.ToInt32(tbDiffDate.Text);
                /*-------------------------------------------------------------------------------------*/
                /*循環處理各站點的產品的數據*/
                int intcount = 0;
                if (ddlTYPE.Text == "PanelSize")
                {
                    intcount = Convert.ToInt32(ddlByWtltm.Items.Count);
                }
                else
                {
                    intcount = Convert.ToInt32(ddlByProdID.Items.Count);
                }
                for (int k = 0; k < intcount; k++)
                {
                    string strPRODUCTID = "";         /*產品型號*/
                    //strPRODUCTID = ddlByProdID.Items[k].Value.ToString();       /*獲取產品型號*/
                    /*-------------------------------------------------------------------------------------*/
                    //if (ddlTYPE.Text == "PanelSize")
                    //{
                    //    strPRODUCTID = ddlByWtltm.Items[k].Value.ToString();       /*獲取產品型號*/
                    //}
                    //else
                    //{
                    strPRODUCTID = ddlByProdID.Items[k].Value.ToString();       /*獲取產品型號*/
                    //}
                    /*-------------------------------------------------------------------------------------*/
                    for (int i = 0; i <= 12; i++)   /*V1.04 6 to 10    V2.00  11*/ /*V2.01   to 12*/
                    {
                        string strOPERATIONID = "";       /*站點代碼*/
                        string strType = "";
                        switch (i)
                        {
                            /* V1.04 Mark begin
                            case 6: strOPERATIONID = "8090"; strType = "(8090)FinalClean"; break;
                            case 5: strOPERATIONID = "8081"; strType = "(8081)外觀"; break;
                            case 4: strOPERATIONID = "8080"; strType = "(8080)LOT1"; break;
                            case 3: strOPERATIONID = "8060"; strType = "(8060)RoughClean"; break;
                            case 2: strOPERATIONID = "8050"; strType = "(8050)EndSealing"; break;
                            case 1: strOPERATIONID = "8040"; strType = "(8040)LCInjection"; break;
                            case 0: strOPERATIONID = "8020"; strType = "(8020)SemiScribe"; break;
                            * V1.04 mark End*/
                            case 12: strOPERATIONID = "808E"; strType = "(808E)Linecheck"; break;    /*V2.01 ADD begin*/
                            case 11: strOPERATIONID = "8090"; strType = "(8090)FinalClean"; break;    /*V1.04 ADD begin*/
                            case 10: strOPERATIONID = "8085"; strType = "(8085)Cell2Packing2"; break;  
                            case 9: strOPERATIONID = "8082"; strType = "(8082)Cell2Packing1"; break;
                            case 8: strOPERATIONID = "808B"; strType = "(808B)外觀_LR"; break;
                            case 7: strOPERATIONID = "808A"; strType = "(808A)LOT1_LR"; break;
                            case 6: strOPERATIONID = "8081"; strType = "(8081)外觀"; break;
                            case 5: strOPERATIONID = "808T"; strType = "(808T)LOT1.5"; break;   // V2.00 
                            case 4: strOPERATIONID = "8080"; strType = "(8080)LOT1"; break;
                            case 3: strOPERATIONID = "8060"; strType = "(8060)RoughClean"; break;
                            case 2: strOPERATIONID = "8050"; strType = "(8050)EndSealing"; break;
                            case 1: strOPERATIONID = "8040"; strType = "(8040)LCInjection"; break;
                            case 0: strOPERATIONID = "8020"; strType = "(8020)SemiScribe"; break;      /*V1.04 ADD End*/
                            default:
                                break;
                        }
                        for (int m = 0; m <= Convert.ToInt32(ddlByWtltm.Items.Count); m++)
                        {
                            string strPanelsize = "";

                            if (m == Convert.ToInt32(ddlByWtltm.Items.Count))
                            {
                                strPanelsize = "";
                            }
                            else
                            {
                                strPanelsize = ddlByWtltm.Items[m].Value.ToString();
                            }
                            /*查詢個站點的資料*/
                            //DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "')", "PRODUCT_ID DESC");
                            DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "' AND SIZEID='" + strPanelsize + "')", "PRODUCT_ID DESC");
                            if (drType.Length > 0)
                            {
                                long lngMQty = 0;
                                long lngChipQty = 0;
                                long lngAQty = 0;

                                double dblAYield = 0, dblRYield = 0, dblSYield = 0, dblAAYield = 0;
                                long lngTarget = 0;

                                /*  V1.03 Mark Begin
                                ChipQty=A+R+S
                                AYield=A/(ChipQty)
                                RYield=R/(ChipQty)
                                SYield=S/(ChipQty)
                                AAYield=AA/(ChipQty)
                                A?Yield=A?/(ChipQty)
                                (AA+A0+A1+A2)Yield=A?/(ChipQty)
                                V1.03 Mark End */

                                /* V1.03 ADD Begin
                                ChipQty=A+R+S-A1-A2-L2-LR-BP-PC-PT-P5
                                AYield=(A-A1-A2-L2-LR-BP-PC-PT-P5)/(ChipQty)
                                RYield=R/(ChipQty)
                                SYield=S/(ChipQty)
                                AAYield=AA/(ChipQty)
                                A?Yield=A?/(ChipQty)
                                (AA+A0+A1+A2)Yield=A?/(ChipQty)
                                V1.03 ADD End  */
                               
                                /*V1.03 Mark Begin*/
                                //lngMQty = Convert.ToInt32(drType[0].ItemArray[3]);
                                //lngChipQty = Convert.ToInt32(drType[0].ItemArray[3]);
                               // lngAQty = Convert.ToInt32(drType[0].ItemArray[4]);
                               // dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4])) / lngMQty, 4) * 100;
                               // //dblRYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[5])) / lngMQty, 4) * 100;
                                /*V1.03 Mark End*/

                                /*V1.03 ADD Begin*/
                                lngMQty = Convert.ToInt32(drType[0].ItemArray[3]) - Convert.ToInt32(drType[0].ItemArray[9]) - Convert.ToInt32(drType[0].ItemArray[10]) - Convert.ToInt32(drType[0].ItemArray[14]) - Convert.ToInt32(drType[0].ItemArray[15]) - Convert.ToInt32(drType[0].ItemArray[16]) - Convert.ToInt32(drType[0].ItemArray[17]) - Convert.ToInt32(drType[0].ItemArray[18]) - Convert.ToInt32(drType[0].ItemArray[19]);
                                lngChipQty = lngMQty;
                                lngAQty = Convert.ToInt32(drType[0].ItemArray[4]) - Convert.ToInt32(drType[0].ItemArray[9]) - Convert.ToInt32(drType[0].ItemArray[10]) - Convert.ToInt32(drType[0].ItemArray[14]) - Convert.ToInt32(drType[0].ItemArray[15]) - Convert.ToInt32(drType[0].ItemArray[16]) - Convert.ToInt32(drType[0].ItemArray[17]) - Convert.ToInt32(drType[0].ItemArray[18]) - Convert.ToInt32(drType[0].ItemArray[19]);
                                dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4]) - Convert.ToDouble(drType[0].ItemArray[9]) - Convert.ToDouble(drType[0].ItemArray[10]) - Convert.ToDouble(drType[0].ItemArray[14]) - Convert.ToDouble(drType[0].ItemArray[15]) - Convert.ToDouble(drType[0].ItemArray[16]) - Convert.ToDouble(drType[0].ItemArray[17]) - Convert.ToDouble(drType[0].ItemArray[18]) - Convert.ToDouble(drType[0].ItemArray[19])) / lngMQty, 4) * 100;  
                                /*V1.03 ADD End*/

                                dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[6])) / lngMQty, 4) * 100;
                                dblAAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[7])) / lngMQty, 4) * 100;
                                //dblA0Yield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[8])) / lngMQty, 4) * 100;


                                lngTarget = Convert.ToInt32(drType[0].ItemArray[12]);

                                /*-----------------------------------------------------------------------------------------------------------------------------*/
                                /*處理defecode 數據*/
                                string[] Defe = new string[10];/*1.01 5 to 10*/
                                double[] Def_QTY = new double[10];/*1.01 5 to 10*/
                                //DataRow[] drTypedef = dsTmp1.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "')");
                                DataRow[] drTypedef = dsTmp1.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "' AND SIZEID='" + strPanelsize + "')");
                                if (drTypedef.Length > 0)
                                {

                                    int A = 0;
                                    if (drTypedef.Length < 10)/*1.01 5 to 10*/
                                    {
                                        A = Convert.ToInt32(drTypedef.Length);
                                    }
                                    else
                                    {
                                        A = 10;/*1.01 5 to 10*/
                                    }
                                    for (int j = 0; j < A; j++)
                                    {
                                        string DefeDesc;
                                        DefeDesc = "";
                                        Defe[j] = null;
                                        Def_QTY[j] = 0;
                                        if (drTypedef[j].ItemArray[4].ToString() == null)
                                        {
                                            DefeDesc = " ";
                                        }
                                        else
                                        {
                                            DefeDesc = drTypedef[j].ItemArray[4].ToString();
                                        }
                                        Defe[j] = drTypedef[j].ItemArray[3].ToString() + "	" + DefeDesc;
                                        Def_QTY[j] = Math.Round(Convert.ToDouble(drTypedef[j].ItemArray[5]) / lngChipQty, 4) * 100;
                                    }
                                }

                                /*-----------------------------------------------------------------------------------------------------------------------------*/
                                DataRow drFPY = dtCFPY.NewRow();
                                drFPY["ReportDT"] = strReportDT;
                                drFPY["OPERATION_ID"] = strType;
                                drFPY["PRODUCT_ID"] = strPRODUCTID;
                                drFPY["Panel_Size"] = Convert.ToString(drType[0].ItemArray[13]);
                                drFPY["Chip_Qty"] = lngChipQty;
                                drFPY["A_Qty"] = lngAQty;
                                drFPY["A_Yield"] = dblAYield;
                                //drFPY["R_Yield"] = dblRYield;
                                drFPY["S_Yield"] = dblSYield;
                                drFPY["AA_Yield"] = dblAAYield;
                                drFPY["Top1_Code"] = Defe[0];
                                drFPY["Rate01"] = Def_QTY[0];
                                drFPY["Top2_Code"] = Defe[1];
                                drFPY["Rate02"] = Def_QTY[1];
                                drFPY["Top3_Code"] = Defe[2];
                                drFPY["Rate03"] = Def_QTY[2];
                                drFPY["Top4_Code"] = Defe[3];
                                drFPY["Rate04"] = Def_QTY[3];
                                drFPY["Top5_Code"] = Defe[4];
                                drFPY["Rate05"] = Def_QTY[4];
                                drFPY["Top6_Code"] = Defe[5];
                                drFPY["Rate06"] = Def_QTY[5];
                                drFPY["Top7_Code"] = Defe[6];
                                drFPY["Rate07"] = Def_QTY[6];
                                drFPY["Top8_Code"] = Defe[7];
                                drFPY["Rate08"] = Def_QTY[7];
                                drFPY["Top9_Code"] = Defe[8];
                                drFPY["Rate09"] = Def_QTY[8];
                                drFPY["Top10_Code"] = Defe[9];
                                drFPY["Rate10"] = Def_QTY[9];
                                //drFPY["DFQty"] = lngDFQty;
                                //drFPY["S_DFQty"] = lngSDFQty;
                                //drFPY["R_DFQty"] = lngRDFQty;
                                dtCFPY.Rows.Add(drFPY);
                                /*-------------------------------------------------------------------------------------*/

                            }
                        }
                    }
                }
            }
            Session["dtCFPY"] = null;
            Session["dtCFPY"] = dtCFPY;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void GridViewDataBound()
    {
        DataTable dtCFPY = (DataTable)Session["dtCFPY"];
        DataTable dtCDef = (DataTable)Session["dtCDef"];
        dtCFPY.DefaultView.RowFilter = "";
        dtCFPY.DefaultView.Sort = "OPERATION_ID,PRODUCT_ID";
        GridView1.DataSource = dtCFPY;
        GridView1.DataBind();
        for (int n = 0; n < GridView1.Rows.Count; n++)
        {
            GridView1.Rows[n].Cells[4].Text = GridView1.Rows[n].Cells[4].Text + "%";
            GridView1.Rows[n].Cells[5].Text = GridView1.Rows[n].Cells[5].Text + "%";
            GridView1.Rows[n].Cells[7].Text = GridView1.Rows[n].Cells[7].Text + "%";
            GridView1.Rows[n].Cells[9].Text = GridView1.Rows[n].Cells[9].Text + "%";
            GridView1.Rows[n].Cells[11].Text = GridView1.Rows[n].Cells[11].Text + "%";
            GridView1.Rows[n].Cells[13].Text = GridView1.Rows[n].Cells[13].Text + "%";
            GridView1.Rows[n].Cells[15].Text = GridView1.Rows[n].Cells[15].Text + "%";
        }
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            int colnum = 1;
            int j;
            for (j = i + 1; j < GridView1.Rows.Count; j++)
            {
                if (GridView1.Rows[i].Cells[0].Text == GridView1.Rows[j].Cells[0].Text)
                {
                    colnum++;
                    GridView1.Rows[i].Cells[0].RowSpan = colnum;
                    GridView1.Rows[j].Cells[0].Visible = false;
                }
                else
                {
                    break;
                }
            }
            i = j - 1;
        }

        for (int m = 0; m < GridView1.Rows.Count; m++)
        {
            if (GridView1.Rows[m].Cells[0].Text == "(8020)SemiScribe") { GridView1.Rows[m].Cells[0].BackColor = Color.MistyRose; }
            if (GridView1.Rows[m].Cells[0].Text == "(8040)LCInjection") { GridView1.Rows[m].Cells[0].BackColor = Color.LightYellow; }
            if (GridView1.Rows[m].Cells[0].Text == "(8050)EndSealing") { GridView1.Rows[m].Cells[0].BackColor = Color.Pink; }
            if (GridView1.Rows[m].Cells[0].Text == "(8060)RoughClean") { GridView1.Rows[m].Cells[0].BackColor = Color.PowderBlue; }
            if (GridView1.Rows[m].Cells[0].Text == "(8080)LOT1") { GridView1.Rows[m].Cells[0].BackColor = Color.Plum; }
            if (GridView1.Rows[m].Cells[0].Text == "(808T)LOT1.5") { GridView1.Rows[m].Cells[0].BackColor = Color.MistyRose; }          ////V2.00
            if (GridView1.Rows[m].Cells[0].Text == "(8081)外觀") { GridView1.Rows[m].Cells[0].BackColor = Color.GreenYellow; }
            if (GridView1.Rows[m].Cells[0].Text == "(808A)LOT1_LR") { GridView1.Rows[m].Cells[0].BackColor = Color.Olive; }                   // V1.04 ADD
            if (GridView1.Rows[m].Cells[0].Text == "(808B)外觀_LR") { GridView1.Rows[m].Cells[0].BackColor = Color.Goldenrod; }               // V1.04 ADD
            if (GridView1.Rows[m].Cells[0].Text == "(808E)Linecheck") { GridView1.Rows[m].Cells[0].BackColor = Color.ForestGreen; }               // V2.01 ADD
            if (GridView1.Rows[m].Cells[0].Text == "(8082)Cell2Packing1") { GridView1.Rows[m].Cells[0].BackColor = Color.Aqua; }             // V1.04 ADD
            if (GridView1.Rows[m].Cells[0].Text == "(8085)Cell2Packing2") { GridView1.Rows[m].Cells[0].BackColor = Color.MediumPurple; }     // V1.04 ADD
            if (GridView1.Rows[m].Cells[0].Text == "(8090)FinalClean") { GridView1.Rows[m].Cells[0].BackColor = Color.LightCyan; }
        }
    }
 
    
    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        AppClass Apc = new AppClass();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Export-Excel", "", "");

        Response.Clear();
        Response.Buffer = false;
        Response.Charset = "UTF-8";
        //Response.ContentType = "Content-Language;content=UTF-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
        this.EnableViewState = false;
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        this.GridView1.RenderControl(oHtmlTextWriter);
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
         
                if (e.Row.Cells[1].Visible == true)
                {
                    string strFABID = tbFABID.Text.ToString();
                    string strType = e.ToString();
                    string strOPERATION = e.Row.Cells[0].Text.Trim().ToString();
                    string strPRODID = e.Row.Cells[1].Text.Trim().ToString();
                    string strFDate = tbFDate.Text.ToString() ;
                    string strTDate = tbTDate.Text.ToString();
                    string strAAQTY = e.Row.Cells[16].Text.Trim().ToString();
                    string strInput = e.Row.Cells[3].Text.Trim().ToString();
                    string strYield = e.Row.Cells[4].Text.Trim().ToString();
                    string strmode = ddlTYPE.Text.ToString();
                    string strSIZE = e.Row.Cells[2].Text.Trim().ToString();

                    //e.Row.Cells[1].Attributes.Add("ondblclick", "window.open('DailyYiedReportDetail.aspx?FABID=" + strFABID + "&OPERATION_ID=" + strOPERATION + "&PRODUCT_ID=" + strPRODID + "&FDate=" + strFDate + "&TDate=" + strTDate + "&INPUT=" + strInput + "&YIELD=" + strYield + "&AAQTY=" + strAAQTY + "')");
                    e.Row.Cells[1].Attributes.Add("onclick", "window.open('DailyYiedReportDetail.aspx?FABID=" + strFABID + "&OPERATION_ID=" + strOPERATION + "&PRODUCT_ID=" + strPRODID + "&FDate=" + strFDate + "&TDate=" + strTDate + "&INPUT=" + strInput + "&YIELD=" + strYield + "&AAQTY=" + strAAQTY + "&mode=" + strmode + "&SIZE=" + strSIZE + "')");
                    e.Row.Cells[1].Font.Underline = true;
                }
        }
    }
}
