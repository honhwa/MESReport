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
 * 2014-07-09  Innolux/SSC     V1.03       Add Grade A1,A2,L2,LR,BP,PC,PT,P5
 * 2014-07-15  Innolux/SSC     V1.04       Add Station 808A,808B,8082,8085
 * * 2014-08-14  Innolux/Runy    V1.05       Add LCD Craft篩選條件
 * 2019-12-11   LiZiQi               V1.06       Add 808E (Linecheck)
*/
public partial class YMSReport_Cell2AllLightOnDaily : System.Web.UI.Page
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
            DataBoundStation();
            DataBoundtxtDate();
            ChangeHeight();
            PageTitleBound();
            txtFDate.Attributes.Add("readonly", "readonly");
            txtTDate.Attributes.Add("readonly", "readonly");
            //Page.MaintainScrollPositionOnPostBack = true;   --保持當前滾動條位置            lblDataRunTime.Text = Apc.QueryDataRunTime(tbFABID.Text).ToString();

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

        if (Apc.QuerySubject("Cell2AllLightOnDaily", "Cell2AllLightOnDaily", out strTitle, out strCon, out strDateTime) == true)
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
        ddlStation.Items.Add("外觀(8081)");
        ddlStation.Items.Add("LOT1_LR(808A)");             // V1.04 ADD
        ddlStation.Items.Add("外觀_LR(808B)");             // V1.04 ADD
        ddlStation.Items.Add("Linecheck(808E)");             // V1.06 ADD
        ddlStation.Items.Add("Cell2Packing1(8082)");      // V1.04 ADD
        ddlStation.Items.Add("Cell2Packing2(8085)");      // V1.04 ADD
        ddlStation.Items.Add("FinalClean(8090)");
        ddlStation.Items.Add("M1(8180)");/*V1.02*/
    }
    protected void PageTitleBound()
    {
        Page.Title = tbFABName.Text + " BEOL All Light On Daily Yield";
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
        string strSql = "";
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        strSql = "";
        //strSql = "SELECT PRODID FROM MPIM WHERE FABID = '" + tbFABID.Text + "' AND PRODID NOT LIKE 'V%' AND PRODID NOT LIKE '#%' ORDER BY PRODID";
        if (CB13NC.Checked)
        {
            strSql = "select distinct T9NCNO as PRODID from MPM9M order by T9NCNO";
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
        strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8005','8015','8020','8040','8050','8060','8080','8081','808A','808B','808E','8082','8085','8090','8180') ORDER BY WORKCTR,SUBLINEID"; /*V1.00*//*V1.01*//*V1.02*/ /*V1.04 ADD*//*V1.06 ADD 808E*/
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
        tbAYPS.Text = "";

        tbFDate.Text = "";
        tbTDate.Text = "";
        tbDiffDate.Text = "0";

        tbSqlDate.Text = "";
        tbSqlWhere.Text = "";

        lblFabID.Text = tbFABName.Text;

        Session["dtCDYield"] = null;

        GetSubject();
        //cbCPFlag.Checked = true;             //V1.05 Mark

        PMain.Enabled = true;
        PMain.Height = 100;

        ChangeHeight();
        ClearChartBound();
        PageTitleBound();

        GridView1.Dispose();

        GridView1.DataBind();
        lblTTime.Text = "";
        GC.Collect();
    }
    protected void ClearChartBound()
    {
        ChartAll.DataSource = "";
        ChartAll.Series.Clear();
        ChartAll.Titles.Clear();
        ChartAll.UI.Toolbar.Enabled = false;
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
        if (PSubLineID.Visible == false && PLotType.Visible == false && PProdID.Visible == false && PWonbr.Visible == false && PGCraft.Visible == false) //V1.05 Add PGCraft.Visible=false
        {
            PMain.Height = 100;
        }
        else
        {
            PMain.Height = 240;
        }
    }
    protected void CreateYieldSessionDataTable()
    {
        DataTable dtCDYield = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtCDYield.Columns.Add(new DataColumn("DataType", typeof(string)));			        /*0 */
        dtCDYield.Columns.Add(new DataColumn("ReportDT", typeof(string)));			            /*1 */
        dtCDYield.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			        /*2 */
        dtCDYield.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));                 /*3 */
        dtCDYield.Columns.Add(new DataColumn("Chip_Qty", typeof(long)));                       /*4 */
        dtCDYield.Columns.Add(new DataColumn("A_Rate", typeof(double)));                      /*5*/
        dtCDYield.Columns.Add(new DataColumn("S_Rate", typeof(double)));                      /*6 */
        dtCDYield.Columns.Add(new DataColumn("Total_Qty", typeof(long)));                       /*7 */
        dtCDYield.Columns.Add(new DataColumn("LOTGRADE", typeof(string)));                          /*8 */
        dtCDYield.Columns.Add(new DataColumn("DefectCode", typeof(string)));                     /*9 */
        dtCDYield.Columns.Add(new DataColumn("DF_Rate", typeof(double)));                     /*10 */
        dtCDYield.Columns.Add(new DataColumn("D1Day_Rate", typeof(double)));                     /*11 */
        dtCDYield.Columns.Add(new DataColumn("D2Day_Rate", typeof(double)));                     /*12 */
        dtCDYield.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] keys = new DataColumn[6];
        keys[0] = dtCDYield.Columns["DataType"];
        keys[1] = dtCDYield.Columns["ReportDT"];
        keys[2] = dtCDYield.Columns["PRODUCT_ID"];
        keys[3] = dtCDYield.Columns["OPERATION_ID"];
        keys[4] = dtCDYield.Columns["LOTGRADE"];
        keys[5] = dtCDYield.Columns["DefectCode"];
        dtCDYield.PrimaryKey = keys;
        /*--------------------------------------------------------------------------------------*/
        Session["dtCDYield"] = null;
        Session["dtCDYield"] = dtCDYield;
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        string strSubLineID = "'='";
        string strProdID = "'='";
        string strLotType = "'='";
        string strWonbr = "'='";
        string strLCDProdID = "'='";  //V1.05 Add

        lblTTime.Text = "";
        DateTime dtF = System.DateTime.Now;
        /*--------------------------------------------------------------------------------------*/
        tbAYPS.Text = "";

        tbCPFlag.Text = "";

        tbOPERATIONID.Text = "";
        tbType.Text = "";
        tbDiffDate.Text = "0";
        tbFDate.Text = "";
        tbTDate.Text = "";

        lblFabID.Text = tbFABName.Text;

        Session["dtCDYield"] = null;
        ddlByProdID.Items.Clear();

        ClearChartBound();
        switch (ddlStation.SelectedItem.Text.ToString())
        {

        //ddlStation.Items.Add("BatchInspection(8005)");
        //ddlStation.Items.Add("AutoScribe(8015)");
        //ddlStation.Items.Add("SemiScribe(8020)");
        //ddlStation.Items.Add("LC(8040)");
        //ddlStation.Items.Add("封口(8050)");
        //ddlStation.Items.Add("RoughClean(8060)");
        //ddlStation.Items.Add("LOT1(8080)");
        //ddlStation.Items.Add("外觀(8081)");
        //ddlStation.Items.Add("FinalClean(8090)");

            case "BatchInspection(8005)": tbOPERATIONID.Text = "8005"; tbType.Text = "BatchInspection(8005)"; break;
            case "AutoScribe(8015)": tbOPERATIONID.Text = "8015"; tbType.Text = "AutoScribe(8015)"; break;
            case "SemiScribe(8020)": tbOPERATIONID.Text = "8020"; tbType.Text = "SemiScribe(8020)"; break;
            case "LC(8040)": tbOPERATIONID.Text = "8040"; tbType.Text = "LCInjection(8040)"; break;
            case "封口(8050)": tbOPERATIONID.Text = "8050"; tbType.Text = "封口(8050)"; break;
            case "RoughClean(8060)": tbOPERATIONID.Text = "8060"; tbType.Text = "RoughClean(8060)"; break;
            case "LOT1(8080)": tbOPERATIONID.Text = "8080"; tbType.Text = "LOT1(8080)"; break;
            case "外觀(8081)": tbOPERATIONID.Text = "8081"; tbType.Text = "外觀(8081)"; break;

            case "LOT1_LR(808A)": tbOPERATIONID.Text = "808A"; tbType.Text = "LOT1_LR(808A)"; break;                 // V1.04 ADD
            case "外觀_LR(808B)": tbOPERATIONID.Text = "808B"; tbType.Text = "外觀_LR(808B)"; break;                 // V1.04 ADD
            case "Linecheck(808E)": tbOPERATIONID.Text = "808E"; tbType.Text = "Linecheck(808E)"; break;   // V1.06 ADD
            case "Cell2Packing1(8082)": tbOPERATIONID.Text = "8082"; tbType.Text = "Cell2Packing1(8082)"; break;   // V1.04 ADD
            case "Cell2Packing2(8085)": tbOPERATIONID.Text = "8085"; tbType.Text = "Cell2Packing2(8085)"; break;   // V1.04 ADD

            case "FinalClean(8090)": tbOPERATIONID.Text = "8090"; tbType.Text = "FinalClean(8090)"; break;
            case "M1(8180)": tbOPERATIONID.Text = "8180"; tbType.Text = "M1(8180)"; break;/*V1.02*/
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

        DateTime dt1 = Convert.ToDateTime(txtFDate.Text);
        DateTime dt2 = Convert.ToDateTime(txtTDate.Text);
        TimeSpan ts = dt2 - dt1;
        tbDiffDate.Text = Convert.ToInt32(ts.TotalDays).ToString();

        tbFDate.Text = txtFDate.Text;
        tbTDate.Text = txtTDate.Text;

        if (Convert.ToInt32(tbDiffDate.Text) < 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('開始時間不能大於結束時間！');</script>");
            return;
        }
        /*--------------------------------------------------------------------------------------*/
        if (tbAYield.Enabled == true && tbAYield.Text.Trim() != "")
        {
            if (ddlAYield.SelectedValue.Trim() == "A Yield>")
            {
                tbAYPS.Text = tbAYPS.Text + " AND A_Rate> " + Convert.ToDouble(tbAYield.Text.Trim().ToString()) + " ";
            }
            if (ddlAYield.SelectedValue.Trim() == "A Yield=")
            {
                tbAYPS.Text = tbAYPS.Text + " AND A_Rate= " + Convert.ToDouble(tbAYield.Text.Trim().ToString()) + " ";
            }
            if (ddlAYield.SelectedValue.Trim() == "A Yield<")
            {
                tbAYPS.Text = tbAYPS.Text + " AND A_Rate< " + Convert.ToDouble(tbAYield.Text.Trim().ToString()) + " ";
            }
        }
        if (tbPCS.Enabled == true && tbPCS.Text.Trim() != "")
        {
            if (ddlPCS.SelectedValue.Trim() == "PCS>")
            {
                tbAYPS.Text = tbAYPS.Text + " AND Chip_Qty> " + Convert.ToInt32(tbPCS.Text.Trim().ToString()) + " ";
            }
            if (ddlPCS.SelectedValue.Trim() == "PCS=")
            {
                tbAYPS.Text = tbAYPS.Text + " AND Chip_Qty= " + Convert.ToInt32(tbPCS.Text.Trim().ToString()) + " ";
            }
            if (ddlPCS.SelectedValue.Trim() == "PCS<")
            {
                tbAYPS.Text = tbAYPS.Text + " AND Chip_Qty< " + Convert.ToInt32(tbPCS.Text.Trim().ToString()) + " ";
            }
        }

        if (cbCPFlag.Checked == true)   /*判斷是否有選擇"CELL2 PRODID"的選項*/
        {
            tbCPFlag.Text = "C";
        }
        else
        {
            tbCPFlag.Text = "P";
        }

        tbSqlDate.Text = "";
        tbSqlDate.Text = tbSqlDate.Text + " WHERE ReportDate >= CAST('" + tbFDate.Text + "' AS DATETIME)-2 AND ReportDate <= '" + tbTDate.Text + "' ";

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
        /*-------------------------------------------------------------------------------------*/
        CreateYieldSessionDataTable();
        /*--------------------------------------------------------------------------------------*/

        QueryYieldDataBound("ALL", "ALL");
        QueryDefectDataBound("ALL", "ALL");
        GridViewDataBound();
        ChartDataBound();
        DataTable dtTmp = (DataTable)Session["dtCDYield"];
        string strLogWhere = "Station:" + ddlStation.SelectedValue.ToString() +
                             ";From:" + tbFDate.Text.ToString() +
                             ";To:" + tbTDate.Text.ToString() +
                             ";Cell2Prodid:" + cbCPFlag.Checked.ToString() +
                             ";TopDefectCode:" +ddlDFCnt.SelectedValue.ToString() +
                             ddlPCS.SelectedValue.ToString() + ":" + tbPCS.Text.ToString() +
                             ddlAYield.SelectedValue.ToString() + ":" + tbAYield.Text.ToString() +
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
    protected void QueryYieldDataBound(string strMode, string strTmpProdID)
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());

            if (strMode == "ALL")
            {
                /*-------------------------------------------------------------------------------------*/
                /*機種清單*/
                if (CB13NC.Checked)
                {
                    strSql.Append(" SELECT DISTINCT CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END AS CPRODUCT_ID ");
                }
                else
                {
                    strSql.Append(" SELECT DISTINCT CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END AS CPRODUCT_ID ");
                }

                strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK) ");
                strSql.Append(" WHERE ReportDate >= '" + tbFDate.Text + "' AND ReportDate <= '" + tbTDate.Text + "' ");
                strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
                strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");
                strSql.Append(tbSqlWhere.Text);
                if (CB13NC.Checked)
                {
                    strSql.Append(" ORDER BY CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END");
                }
                else
                {
                    strSql.Append(" ORDER BY CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END");
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
                ddlByProdID.DataTextField = "CPRODUCT_ID";
                ddlByProdID.DataValueField = "CPRODUCT_ID";
                ddlByProdID.DataBind();
                dt00.Dispose();
                sqlApt00.Dispose();
                strSql.Remove(0,strSql.Length);
            }
            /*-------------------------------------------------------------------------------------*/
            strSql.Remove(0,strSql.Length);
            strSql.Append(" SELECT T1.REPORTDT");                /*0 */
            strSql.Append(" ,T1.PRODUCT_ID");                /*1 */
            strSql.Append(" ,'" + tbOPERATIONID.Text + "' AS OPERATION_ID");                /*2 */
            strSql.Append(" ,T1.T_CNT");               			/*3 */
            strSql.Append(" ,T1.A_CNT");               			/*4 */
            strSql.Append(" ,T1.S_CNT");               			/*5 */
            strSql.Append(" ,T1.A1_CNT");                       /*6 */  /*V1.03 ADD Begin*/
            strSql.Append(" ,T1.A2_CNT");                       /*7 */
            strSql.Append(" ,T1.L2_CNT");                       /*8 */
            strSql.Append(" ,T1.LR_CNT");                       /*9 */
            strSql.Append(" ,T1.BP_CNT");                       /*10 */
            strSql.Append(" ,T1.PC_CNT");                       /*11 */
            strSql.Append(" ,T1.PT_CNT");                       /*12 */
            strSql.Append(" ,T1.P5_CNT");                       /*13 */ /*V1.03 ADD End*/
            strSql.Append(" FROM(");
            strSql.Append(" SELECT ");
            if (strMode == "ALL")
            {
                strSql.Append(" CASE WHEN REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-2 OR REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-1 THEN CONVERT(VARCHAR(10),ReportDate,120) ELSE 'TOTAL' END AS ReportDT,");
            }
            else
            {
                strSql.Append(" CONVERT(VARCHAR(10),ReportDate,120) AS ReportDT,");
            }
            if (CB13NC.Checked)
            {
                strSql.Append(" CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END AS PRODUCT_ID,");
            }
            else
            {
                strSql.Append(" CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END AS PRODUCT_ID,");
            }
            strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");
            strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");
            strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT");
            strSql.Append(" , SUM(ISNULL(A1_CNT,0)) AS A1_CNT, ");  /*V1.03 ADD Begin*/
            strSql.Append(" SUM(ISNULL(A2_CNT,0)) AS A2_CNT, ");
            strSql.Append(" SUM(ISNULL(L2_CNT,0)) AS L2_CNT, ");
            strSql.Append(" SUM(ISNULL(LR_CNT,0)) AS LR_CNT, ");
            strSql.Append(" SUM(ISNULL(BP_CNT,0)) AS BP_CNT, ");
            strSql.Append(" SUM(ISNULL(PC_CNT,0)) AS PC_CNT, ");
            strSql.Append(" SUM(ISNULL(PT_CNT,0)) AS PT_CNT, ");
            strSql.Append(" SUM(ISNULL(P5_CNT,0)) AS P5_CNT ");    /*V1.03 ADD End*/
            strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK)");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            if (strMode != "ALL")
            {
                strSql.Append(" AND ReportDate >= '" + tbFDate.Text + "' AND ReportDate <= '" + tbTDate.Text + "' ");
            }
        
                strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");
            if (strTmpProdID != "ALL")
            {
                if (tbCPFlag.Text == "C")
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND CT9NCNO ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND CPRODUCT_ID ='" + strTmpProdID + "' ");
                    }
                }
                else
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND T9NCNO ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND PRODUCT_ID ='" + strTmpProdID + "' ");
                    }
                }
            }
            strSql.Append(tbSqlWhere.Text);
            if (CB13NC.Checked)
            {
                strSql.Append(" GROUP BY CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CT9NCNO ELSE T9NCNO END ");
            }
            else
            {
                strSql.Append(" GROUP BY CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END ");
            }
            if (strMode == "ALL")
            {
                strSql.Append(" ,CASE WHEN REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-2 OR REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-1 THEN CONVERT(VARCHAR(10),ReportDate,120) ELSE 'TOTAL' END");
            }
            else
            {
                strSql.Append(" ,CONVERT(VARCHAR(10),ReportDate,120)");
            }
            strSql.Append(" ) T1");
            DataSet dt011 = new DataSet();
            SqlDataAdapter sqlApt011 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt011.Fill(dt011);
            /*-------------------------------------------------------------------------------------*/
            BoudYieldDataTable(strMode, dt011, strTmpProdID);
            /*-------------------------------------------------------------------------------------*/
            dt011.Dispose();
            sqlApt011.Dispose();
            strSql.Remove(0,strSql.Length);
            AppClass Apc = new AppClass();
            lblDataRunTime.Text = Apc.QueryDataRunTime(tbFABID.Text).ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryDefectDataBound(string strMode, string strTmpProdID)
    {
        try
        {
            StringBuilder strSql =new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());

            strSql.Append(" SELECT   ");
            if (strMode == "ALL")
            {
                strSql.Append(" CASE WHEN REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-2 OR REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-1 THEN CONVERT(VARCHAR(10),ReportDate,120) ELSE 'TOTAL' END AS ReportDT,");
            }
            else
            {
                strSql.Append(" CONVERT(VARCHAR(10),ReportDate,120) AS ReportDT,");
            }
            if (CB13NC.Checked)
            {
                strSql.Append(" CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CT9NCNO ELSE A.T9NCNO END AS PRODUCT_ID,  ");
            }
            else
            {
                strSql.Append(" CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END AS PRODUCT_ID,  ");   /*1 */
            }
            if (strMode == "ALL")
            {
                strSql.Append(" CASE WHEN LOT_GRADE = 'R' THEN LOT_GRADE ELSE '非R' END AS LOT_GRADE,   ");
            }
            else
            {
                strSql.Append(" LOT_GRADE,   ");
            }
            strSql.Append(" DEF_CODE,   ");             /*3 */
            strSql.Append(" DEFECTDESC,    ");           /*4 */
            strSql.Append(" SUM(DEF_CNT) AS TTL_CNT   ");   /*5 */
            strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK)   ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            if (strMode != "ALL")
            {
                strSql.Append(" AND ReportDate >= '" + tbFDate.Text + "' AND ReportDate <= '" + tbTDate.Text + "' ");
            }

                strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");

            if (strTmpProdID != "ALL")
            {
                if (tbCPFlag.Text == "C")
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND CT9NCNO ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND CPRODUCT_ID ='" + strTmpProdID + "' ");
                    }
                }
                else
                {
                    if (CB13NC.Checked)
                    {
                        strSql.Append(" AND T9NCNO ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND PRODUCT_ID ='" + strTmpProdID + "' ");
                    }
                }
            }
            strSql.Append(tbSqlWhere.Text);
            strSql.Append(" AND ((LOT_GRADE = 'R' AND DEF_CODE NOT IN('A000','B000','C000','F000')) OR LOT_GRADE != 'R')");
            strSql.Append(" AND A.FAB_ID=B.FABID AND A.DEF_CODE=B.DEFECTCD  ");
            strSql.Append(" GROUP BY   ");
            if (strMode == "ALL")
            {
                strSql.Append(" CASE WHEN REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-2 OR REPORTDATE = CAST('" + tbFDate.Text + "' AS DATETIME)-1 THEN CONVERT(VARCHAR(10),ReportDate,120) ELSE 'TOTAL' END,");
                strSql.Append(" CASE WHEN LOT_GRADE = 'R' THEN LOT_GRADE ELSE '非R' END,   ");
            }
            else
            {
                strSql.Append(" CONVERT(VARCHAR(10),ReportDate,120),LOT_GRADE,");
            }
            if (CB13NC.Checked)
            {
                strSql.Append(" CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CT9NCNO ELSE A.T9NCNO END,DEF_CODE,DEFECTDESC ");
            }
            else
            {
                strSql.Append(" CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,DEF_CODE,DEFECTDESC ");
            }

            DataSet dt011 = new DataSet();
            SqlDataAdapter sqlApt011 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt011.Fill(dt011);
            /*-------------------------------------------------------------------------------------*/
            BoudDefectDataTable(strMode, dt011, strTmpProdID);
            /*-------------------------------------------------------------------------------------*/
            dt011.Dispose();
            sqlApt011.Dispose();
            strSql.Remove(0, strSql.Length);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BoudYieldDataTable(string strMode, DataSet dsTmp, string strTmpProdID)
    {
        try
        {
            DataTable dtCDYield = (DataTable)Session["dtCDYield"];
            int intDaysCnt = 0;
            string strDataType = "";
            if (strMode == "ALL")
            {
                strDataType = "Q1";
                intDaysCnt = 2;
            }

            /*-------------------------------------------------------------------------------------*/
            /*將各站點用編號區分，循環處理這幾個站點的資料*/
            for (int DTCnt = 0; DTCnt <= intDaysCnt; DTCnt++)
            {
                string strReportDT = "";
                if (strMode == "ALL")
                {
                    switch (DTCnt)
                    {
                        case 0: strReportDT = "TOTAL"; break;
                        case 1: strReportDT = Convert.ToDateTime(tbFDate.Text).AddDays(-1).ToString("yyyy-MM-dd"); break;
                        case 2: strReportDT = Convert.ToDateTime(tbFDate.Text).AddDays(-2).ToString("yyyy-MM-dd"); break;
                    }
                }
                else
                {
                    strReportDT = Convert.ToDateTime(tbFDate.Text).AddDays(DTCnt).ToString("yyyy-MM-dd");
                }
                /*-------------------------------------------------------------------------------------*/
                /*循環處理各產品的數據*/
                for (int k = 0; k < Convert.ToInt32(ddlByProdID.Items.Count); k++)
                {
                    string strPRODUCTID = "";         /*產品型號*/
                    if (strTmpProdID == "ALL")
                    {
                        strPRODUCTID = ddlByProdID.Items[k].Value.ToString();       /*獲取產品型號*/
                    }
                    else
                    {
                        strPRODUCTID = strTmpProdID.ToString();       /*獲取產品型號*/
                        k = Convert.ToInt32(ddlByProdID.Items.Count);
                    }
                    /*-------------------------------------------------------------------------------------*/
                    /*查詢個站點的資料*/
                    DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "')");
                    if (drType.Length > 0)
                    {
                        int intDr = 0;
                        long lngMQty = 0, lngChipQty = 0, lngTotalQty = 0;
                        double dblAYield = 0, dblSYield = 0;

                        /* V1.03 Mark Begin
                        ChipQty=A+S     -----和FPY有差異，FPY為A+R+S
                        AYield=A/(ChipQty)
                        RYield=R/(ChipQty+R)
                        SYield=S/(ChipQty)
                        AAYield=AA/(ChipQty)
                        A?Yield=A?/(ChipQty)
                        (AA+A0+A1+A2)Yield=A?/(ChipQty)
                        V1.03 Mark End  */

                        /* V1.03 ADD Begin
                        ChipQty=A+S-A1-A2-L2-LR-BP-PC-PT-P5     -----和FPY有差異，FPY為A+R+S
                        AYield=(A-A1-A2-L2-LR-BP-PC-PT-P5)/(ChipQty)
                        RYield=R/(ChipQty+R)
                        SYield=S/(ChipQty)
                        AAYield=AA/(ChipQty)
                        A?Yield=A?/(ChipQty)
                        (AA+A0+A1+A2)Yield=A?/(ChipQty)                   
                         V1.03 ADD End */

                        //lngMQty = Convert.ToInt32(drType[intDr].ItemArray[3]);  V1.03 Mark
                        lngMQty = Convert.ToInt32(drType[intDr].ItemArray[3]) - Convert.ToInt32(drType[0].ItemArray[6]) - Convert.ToInt32(drType[0].ItemArray[7]) - Convert.ToInt32(drType[0].ItemArray[8]) - Convert.ToInt32(drType[0].ItemArray[9]) - Convert.ToInt32(drType[0].ItemArray[10]) - Convert.ToInt32(drType[0].ItemArray[11]) - Convert.ToInt32(drType[0].ItemArray[12]) - Convert.ToInt32(drType[0].ItemArray[13]);   //V1.03 ADD
                        lngChipQty = lngMQty;
                       // dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[intDr].ItemArray[4])) / lngMQty, 4) * 100; V1.03 Mark
                        dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[intDr].ItemArray[4]) - Convert.ToDouble(drType[0].ItemArray[6]) - Convert.ToDouble(drType[0].ItemArray[7]) - Convert.ToDouble(drType[0].ItemArray[8]) - Convert.ToDouble(drType[0].ItemArray[9]) - Convert.ToDouble(drType[0].ItemArray[10]) - Convert.ToDouble(drType[0].ItemArray[11]) - Convert.ToDouble(drType[0].ItemArray[12]) - Convert.ToDouble(drType[0].ItemArray[13])) / lngMQty, 4) * 100;  // V1.03 ADD
                        dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[intDr].ItemArray[5])) / lngMQty, 4) * 100;

                        lngTotalQty = Convert.ToInt32(drType[intDr].ItemArray[3]);

                        DataRow drFPY = dtCDYield.NewRow();
                        drFPY["DataType"] = strDataType;
                        drFPY["ReportDT"] = strReportDT;
                        drFPY["PRODUCT_ID"] = strPRODUCTID;
                        drFPY["OPERATION_ID"] = tbType.Text;
                        drFPY["Chip_Qty"] = lngChipQty;
                        drFPY["A_Rate"] = dblAYield;
                        drFPY["S_Rate"] = dblSYield;
                        drFPY["Total_Qty"] = lngTotalQty;
                        drFPY["LOTGRADE"] = "";
                        drFPY["DefectCode"] = "";
                        drFPY["DF_Rate"] = 0;
                        drFPY["D1Day_Rate"] = 0;
                        drFPY["D2Day_Rate"] = 0;
                        dtCDYield.Rows.Add(drFPY);
                        /*-------------------------------------------------------------------------------------*/
                    }
                }
            }
            Session["dtCDYield"] = null;
            Session["dtCDYield"] = dtCDYield;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BoudDefectDataTable(string strMode, DataSet dsTmp, string strTmpProdID)
    {
        try
        {
            DataTable dtCDYield = (DataTable)Session["dtCDYield"];
            /*-------------------------------------------------------------------------------------*/
            DataRow[] drQuery;
            string strDataType = "";
            string strReportDT = "";
            string strPRODUCTID = "";         /*產品型號*/
            if (strMode == "ALL")
            {
                strDataType = "Q2";
                drQuery = dtCDYield.Select(" (REPORTDT='TOTAL' AND DataType = 'Q1')", "PRODUCT_ID");
            }
            else
            {
                strDataType = "";
                drQuery = null;
            }
            for (int i = 0; i < Convert.ToInt32(drQuery.Length); i++)
            {
                int intCnt = 0;
                long lngChipQty = 0, lngTotalQty = 0;
                double dblAYield = 0, dblRYield = 0, dblSYield = 0, dblAAYield = 0;

                strReportDT = drQuery[i].ItemArray[1].ToString();
                strPRODUCTID = drQuery[i].ItemArray[2].ToString();
                lngChipQty = Convert.ToInt32(drQuery[i].ItemArray[4].ToString());
                dblAYield = Convert.ToDouble(drQuery[i].ItemArray[5].ToString());
                dblSYield = Convert.ToDouble(drQuery[i].ItemArray[6].ToString());
                lngTotalQty = Convert.ToInt32(drQuery[i].ItemArray[7].ToString());

                for (int g = 0; g < 1; g++)
                {
                    string strGradeID = "";
                    long lngTTLQty = 0;
                    if (strMode == "ALL")
                    {
                        switch (g)
                        {
                            case 0: strGradeID = "非R"; lngTTLQty = lngChipQty; break;
                            //case 1: strGradeID = "R"; lngTTLQty = lngTotalQty; break;
                        }
                    }
                    else
                    {
                        intCnt = 1;
                        switch (g)
                        {
                            case 0: strGradeID = "S"; lngTTLQty = lngChipQty; break;
                            //case 1: strGradeID = "R"; lngTTLQty = lngTotalQty; break;
                        }
                    }
                    /*查詢個站點的資料*/
                    DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND LOT_GRADE = '" + strGradeID + "')", "LOT_GRADE,TTL_CNT DESC");
                    if (drType.Length > 0)
                    {
                        intCnt = intCnt + 1;
                        for (int intDr = 0; intDr < Convert.ToInt32(drType.Length); intDr++)
                        {
                            string strDefectCode = "", strDefectCodeDesc = "";
                            long lngDFQty = 0;
                            double dblD1DFYield = 0, dblD2DFYield = 0;

                            if (intDr >= Convert.ToInt32(ddlDFCnt.SelectedValue))
                            {
                                intDr = Convert.ToInt32(drType.Length);
                                break;
                            }
                            strDefectCode = drType[intDr].ItemArray[3].ToString();
                            strDefectCodeDesc = drType[intDr].ItemArray[4].ToString();
                            lngDFQty = Convert.ToInt32(drType[intDr].ItemArray[5]);

                            if (strMode == "ALL")
                            {
                                long lngD1TotalQty = 0;
                                long lngD2TotalQty = 0;

                                string strD1Day = Convert.ToDateTime(tbFDate.Text).AddDays(-1).ToString("yyyy-MM-dd");
                                string strD2Day = Convert.ToDateTime(tbFDate.Text).AddDays(-2).ToString("yyyy-MM-dd");

                                DataRow[] drD1DayDef = dsTmp.Tables[0].Select(" (REPORTDT='" + strD1Day + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND LOT_GRADE = '" + strGradeID + "' AND DEF_CODE = '" + strDefectCode + "' )");
                                DataRow[] drD2DayDef = dsTmp.Tables[0].Select(" (REPORTDT='" + strD2Day + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND LOT_GRADE = '" + strGradeID + "' AND DEF_CODE = '" + strDefectCode + "' )");

                                DataRow[] drD1DayChipQty = dtCDYield.Select(" (DataType = 'Q1' AND REPORTDT='" + strD1Day + "' AND PRODUCT_ID='" + strPRODUCTID + "'  )");
                                DataRow[] drD2DayChipQty = dtCDYield.Select(" (DataType = 'Q1' AND REPORTDT='" + strD2Day + "' AND PRODUCT_ID='" + strPRODUCTID + "'  )");

                                if (drD1DayChipQty.Length == 1)
                                {
                                    lngD1TotalQty = Convert.ToInt32(drD1DayChipQty[0].ItemArray[4].ToString());
                                }
                                if (drD2DayChipQty.Length == 1)
                                {
                                    lngD2TotalQty = Convert.ToInt32(drD2DayChipQty[0].ItemArray[4].ToString());
                                }

                                if (drD1DayDef.Length == 1)
                                {
                                    if (lngD1TotalQty == 0)
                                    {
                                        dblD1DFYield = 100;
                                    }
                                    else if (drD1DayDef[0].ItemArray[5].ToString() != "0")
                                    {
                                        dblD1DFYield = Math.Round(Convert.ToDouble(drD1DayDef[0].ItemArray[5]) / Convert.ToDouble(lngD1TotalQty), 4) * 100;
                                    }
                                }
                                if (drD2DayDef.Length == 1)
                                {
                                    if (lngD2TotalQty == 0)
                                    {
                                        dblD2DFYield = 100;
                                    }
                                    if (drD2DayDef[0].ItemArray[5].ToString() != "0")
                                    {
                                        dblD2DFYield = Math.Round(Convert.ToDouble(drD2DayDef[0].ItemArray[5]) / Convert.ToDouble(lngD2TotalQty), 4) * 100;
                                    }
                                }
                            }

                            DataRow drFPY = dtCDYield.NewRow();
                            drFPY["DataType"] = strDataType;
                            drFPY["ReportDT"] = strReportDT;
                            drFPY["PRODUCT_ID"] = strPRODUCTID;
                            drFPY["OPERATION_ID"] = tbType.Text;
                            drFPY["Chip_Qty"] = lngChipQty;
                            drFPY["A_Rate"] = dblAYield;
                            drFPY["S_Rate"] = dblSYield;
                            drFPY["Total_Qty"] = lngTotalQty;
                            drFPY["LOTGRADE"] = strGradeID;
                            drFPY["DefectCode"] = strDefectCode + (strGradeID == "R" ? "*" : "");
                            drFPY["DF_Rate"] = Math.Round(Convert.ToDouble(lngDFQty) / Convert.ToDouble(lngTTLQty), 4) * 100;
                            drFPY["D1Day_Rate"] = dblD1DFYield;
                            drFPY["D2Day_Rate"] = dblD2DFYield;
                            dtCDYield.Rows.Add(drFPY);
                            /*-------------------------------------------------------------------------------------*/
                        }
                    }
                }
                if (intCnt == 0)
                {
                    /*-------------------------------------------------------------------------------------*/
                    DataRow drFPY = dtCDYield.NewRow();
                    drFPY["DataType"] = strDataType;
                    drFPY["ReportDT"] = strReportDT;
                    drFPY["PRODUCT_ID"] = strPRODUCTID;
                    drFPY["OPERATION_ID"] = tbType.Text;
                    drFPY["Chip_Qty"] = lngChipQty;
                    drFPY["A_Rate"] = dblAYield;
                    drFPY["S_Rate"] = dblSYield;
                    drFPY["Total_Qty"] = lngTotalQty;
                    drFPY["LOTGRADE"] = "";
                    drFPY["DefectCode"] = "";
                    drFPY["DF_Rate"] = 0;
                    drFPY["D1Day_Rate"] = 0;
                    drFPY["D2Day_Rate"] = 0;
                    dtCDYield.Rows.Add(drFPY);
                    /*-------------------------------------------------------------------------------------*/
                }
            }
            Session["dtCDYield"] = null;
            Session["dtCDYield"] = dtCDYield;
            /*-------------------------------------------------------------------------------------*/
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryDataGroupByYield()
    {
        DataTable dtCDYield = (DataTable)Session["dtCDYield"];
        DataRow[] drChart = dtCDYield.Select("DataType ='Q1' AND ReportDT = 'TOTAL'  " + tbAYPS.Text + " ", "PRODUCT_ID");
        if (drChart.Length > 0)
        {
            ChartAll.Series.Clear();
            ChartAll.DataSource = "";
            ChartAll.Series.Clear();

            ChartAll.Series.Add("Series1");
            ChartAll.Series.Add("Series2");
            ChartAll.Titles.Add("Title1");
            ChartAll.DataSource = drChart;

            ChartAll.Series["Series1"].Color = System.Drawing.Color.FromArgb(71, 94, 118);
            ChartAll.Series["Series1"].BackGradientType = GradientType.VerticalCenter;
            ChartAll.Series["Series1"].Name = "Chip_Qty";
            ChartAll.Series["Chip_Qty"].ValueMemberX = "PRODUCT_ID";
            ChartAll.Series["Chip_Qty"].ValueMembersY = "Chip_Qty";
            ChartAll.Series["Chip_Qty"].XValueType = ChartValueTypes.String;
            ChartAll.Series["Chip_Qty"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            ChartAll.Series["Series2"].Color = Color.Red;
            ChartAll.Series["Series2"].Name = "A_Yield";
            ChartAll.Series["A_Yield"].ValueMemberX = "PRODUCT_ID";
            ChartAll.Series["A_Yield"].ValueMembersY = "A_Rate";
            ChartAll.Series["A_Yield"].Type = SeriesChartType.Line;
            ChartAll.Series["A_Yield"].YAxisType = AxisType.Secondary;
            ChartAll.Series["A_Yield"].MarkerStyle = MarkerStyle.Triangle;
            ChartAll.Series["A_Yield"].XValueType = ChartValueTypes.String;
            ChartAll.Series["A_Yield"].ToolTip = " #VALX:  #VALY{##,##0.##}";

            ChartAll.Titles[0].Alignment = ContentAlignment.TopCenter;
            ChartAll.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
            ChartAll.Titles[0].Color = Color.Blue;
            ChartAll.Titles[0].Text = tbFABName.Text + " BEOL All Light On Daily Yield";

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
    protected void GridViewDataBound()
    {
        DataTable dtCDYield = (DataTable)Session["dtCDYield"];
        dtCDYield.DefaultView.RowFilter = "";
        dtCDYield.DefaultView.RowFilter = "(DataType='Q2' AND ReportDT = 'TOTAL' " + tbAYPS.Text + " )";
        dtCDYield.DefaultView.Sort = "PRODUCT_ID,LOTGRADE desc,DF_Rate desc";
        GridView1.DataSource = dtCDYield;
        GridView1.DataBind();

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            int colnum = 1;
            int j;
            for (j = i + 1; j < GridView1.Rows.Count; j++)
            {
                if (GridView1.Rows[i].Cells[0].Text == GridView1.Rows[j].Cells[0].Text)
                {
                    colnum++;
                    for (int k = 0; k < 8; k++)
                    {
                        GridView1.Rows[i].Cells[k].RowSpan = colnum;
                        GridView1.Rows[j].Cells[k].Visible = false;
                    }
                }
                else
                {
                    break;
                }
            }
            i = j - 1;
        }
    }
    protected void ChartDataBound()
    {
        lblFabID.Text = tbFABName.Text;
        PageTitleBound();
        ClearChartBound();
        QueryDataGroupByYield();
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
    protected void ddlPCS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPCS.SelectedValue == "PCS")
        {
            tbPCS.Enabled = false;
            tbPCS.Text = "";
        }
        else
        {
            tbPCS.Enabled =true;
            tbPCS.Text = "";
        }
    }
    protected void ddlAYield_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAYield.SelectedValue == "A Yield")
        {
            tbAYield.Enabled = false;
            tbAYield.Text = "";
        }
        else
        {
            tbAYield.Enabled = true;
            tbAYield.Text = "";
        }
    }
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
}
