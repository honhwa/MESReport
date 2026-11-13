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
 * 2013-03-21  Innolux/lss      V1.00       Add 8192&838L Station
 * 2013-04-07  Innolux/ssc      V1.01       Add defect by group 分類
 * 2013-04-15  Innolux/ssc      V1.02       8180歸類到BEOL了，故MODULE要取消M1(8180) yield
 * 2013-05-03  Innolux/ssc      V1.03       Add Garmin Station(8GCO,8GFP,8GL2,8GSL,8GBA,8GSD,8GAG,8GL3,8GTP,8GTT,8GOQ)
 * 2013-05-14  Innolux/ssc      V1.04       Add M1(8180) yield供USER選擇 
 * 2013-05-17  Innolux/QingQing V1.05       Add Lamination 外观站点81LW
 * 2013-06-04  Innolux/Ron      V1.06       Add By Defect Query Defect Group欄位
 * 2013-06-04  Innolux/Ron      V1.07       Add Station 8260 8270 8355
 * 2014-05-14  Innolux/Power    V1.08       MARK 取消 M1(8180) yield
 * 2014-08-02  Innolux/vivian   V1.09       Add 8351,8352,8353
 * 2014-08-14  Innolux/Runy     V1.10       Add LCD Craft篩選條件 & 添加 Summary功能
 * 2015-10-08  Innolux/durant   V1.11       Add Lamination 站點8290
 * 2019-01-02  Innolux/Jeff     V1.12       Add Lamination 站點8285,8292,8294,8296
 * 2018-12-05  Innolux/ssc      V2.00       在下拉框Craft中增加TN FAB3 和TB FAB5選項
 * 2018-12-05  Innolux/ssc      V2.01       在查詢結果LOT1后加lot1.5和在M1 后加Relot1.5
 * 2019-08-22  Innolux/Jeff     V1.13       Add TOD 站點8240
 * 2020-05-05  HuangMenghao     V1.14       Add FOG 站點8234
 * 2020-05-27  HuangMenghao     V1.15       Add VIA 站點8305
 * 2021-01-29  Innolux/Jeff     V1.16       Add 829X,8297,8298,8299
 * 2021-11-07  Innolux/Jeff     V1.17       Add 8220
 * 
*/
public partial class YMSReport_ModuleFPY : System.Web.UI.Page
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
            lblTime01.Text = "";
            lblTime02.Text = "";
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

        if (Apc.QuerySubject("ModuleFPY", "ModuleFPY", out strTitle, out strCon, out strDateTime) == true)
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
        Page.Title = tbFABName.Text + " Module First Pass Yield";
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
        /*V1.10 Add Start*/
        if (ChkMerge.Checked && PProdID.Visible)
        {
            StringBuilder strSql = new StringBuilder();
            AppClass Apc = new AppClass();
            string strSubLineID = "'='";
            string strProdID = "'='";
            string strLotType = "'='";
            string strWonbr = "'='";
            string strLCDProdID = "'='";  //V1.10 Add

            lblTTime.Text = "";
            lblTime01.Text = "";
            lblTime02.Text = "";
            DateTime dtF = System.DateTime.Now;
            /*--------------------------------------------------------------------------------------*/
            tbDateType.Text = "";
            tbDiffDate.Text = "0";
            tbCPFlag.Text = "";
            tbFDate.Text = "";
            tbTDate.Text = "";

            lblFabID.Text = tbFABName.Text;

            Session["dtMFPY"] = null;
            Session["dtMDef"] = null;

            IBGBYData.Enabled = false;
            cblLevel.Enabled = false;
            /*V1.01 Begin*/
            RadioButtonList1.Enabled = false;
            /*V1.01 End*/

            ddlByProdID.Items.Clear();
            ddlSDefList.Items.Clear();

            ClearChartBound();

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
            /*V1.10 Add Start*/
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
            /*V1.10 Add End*/
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
                //tbFDate.Text = ddlFDate.SelectedValue.Replace("W0", "").Replace("W", "");
                //tbTDate.Text = ddlTDate.SelectedValue.Replace("W0", "").Replace("W", "");
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
            if (ddlShiftID.SelectedValue != "ShiftID") { tbSqlWhere.Text = tbSqlWhere.Text + " AND SHIFT_ID LIKE '%" + ddlShiftID.SelectedValue + "'"; }
            if (strSubLineID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND SUBLINEID IN(" + strSubLineID + ")"; }
            if (strLotType != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND LOT_TYPE IN(" + strLotType + ")"; }
            if (tbCPFlag.Text == "C")
            {
                if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }
            }
            else
            {
                if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }
            }
            if (strWonbr != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND WORK_ORDER IN(" + strWonbr + ")"; }
            strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
            strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK) ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            /*V1.04 Begin*/
            if (ddlSelType.SelectedValue == "MODULE")
            {
                strSql.Append(" AND OPERATION_ID IN('8180-Mark','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380')");       /*V1.00*/ /*V1.02*/ /*V1.03*//*V1.05 ADD 81LW*//*V1.08*/ //1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
            }
            else
            {
                strSql.Append(" AND OPERATION_ID IN('8180','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8360','8380')");       /*V1.04*/ /*V1.05 ADD 81LW*/ //1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
            }
            /*V1.04 End*/
            strSql.Append(tbSqlWhere.Text);
            strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ")");
            DataSet dt00 = new DataSet();
            SqlDataAdapter sqlApt00 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt00.Fill(dt00);
            if (dt00.Tables[0].Rows.Count == 0)
            {
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
            /*V1.10 Add End*/
            string strSql = "";
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            strSql = "";
            //strSql = "SELECT PRODID FROM MPIM WHERE FABID = '" + tbFABID.Text + "' AND PRODID NOT LIKE 'V%' AND PRODID NOT LIKE '#%' ORDER BY PRODID";
            strSql = "select distinct project_name as PRODID from mpim order by project_name";
            DataSet ds = new DataSet();
            ds = dbConn.GetDataSetBySql(strSql, "");
            cblProdID.DataSource = ds;
            cblProdID.DataTextField = "PRODID";
            cblProdID.DataValueField = "PRODID";
            cblProdID.DataBind();
            ds.Dispose();
        }/*V1.10 Add*/
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
        /*V1.04 Begin*/
        if (ddlSelType.SelectedValue == "MODULE")
        {
            strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8180-Mark','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380') ORDER BY WORKCTR,SUBLINEID";        /*V1.00*/ /*V1.02*//*V1.03*//*V1.05*//*V1.08*/ //1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
        }
        else
        {
            strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8180','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380') ORDER BY WORKCTR,SUBLINEID";  /*V1.04*//*V1.05*///1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
        }
        /*V1.04 End*/
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

        Session["dtMFPY"] = null;
        Session["dtMDef"] = null;

        PMain.Enabled = true;
        PMain.Height = 60;

        ChangeHeight();
        ClearChartBound();
        PageTitleBound();

        IBGBYData.Enabled = false;
        PTotal.Visible = false;

        ddlByProdID.Items.Clear();
        ddlSDefList.Items.Clear();
        GetSubject();
        cblLevel.SelectedIndex = 0;
        cblLevel.Enabled = false;
        /*V1.01 Begin*/
        RadioButtonList1.SelectedIndex = 0;
        RadioButtonList1.Enabled = false;
        /*V1.01 End*/
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
                        ddlFDate.Items.Add("W0" + i.ToString());
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
        if (PSubLineID.Visible == false && PLotType.Visible == false && PProdID.Visible == false && PWonbr.Visible == false && PGCraft.Visible == false) //V1.10 Add PGCraft.Visible=false
        {
            PMain.Height = 60;
        }
        else
        {
            PMain.Height = 200;
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
        DataTable dtMFPY = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtMFPY.Columns.Add(new DataColumn("ReportDT", typeof(string)));			            /*0 */
        dtMFPY.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			        /*1 */
        dtMFPY.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));                 /*2 */
        dtMFPY.Columns.Add(new DataColumn("Chip_Qty", typeof(long)));                       /*3 */
        dtMFPY.Columns.Add(new DataColumn("A_Yield", typeof(double)));                      /*4 */
        dtMFPY.Columns.Add(new DataColumn("S_Yield", typeof(double)));                      /*5 */
        dtMFPY.Columns.Add(new DataColumn("ItemNo", typeof(int)));                          /*6*/
        dtMFPY.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] keys = new DataColumn[3];
        keys[0] = dtMFPY.Columns["ReportDT"];
        keys[1] = dtMFPY.Columns["PRODUCT_ID"];
        keys[2] = dtMFPY.Columns["OPERATION_ID"];
        dtMFPY.PrimaryKey = keys;
        /*-------------------------------------------------------------------------------------*/
        Session["dtMFPY"] = null;
        Session["dtMFPY"] = dtMFPY;

    }
    protected void CreateDefSessionDataTable()
    {

        /*-------------------------------------------------------------------------------------*/
        DataTable dtMDef = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtMDef.Columns.Add(new DataColumn("ReportDT", typeof(string)));			              /*0 */
        dtMDef.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			          /*1 */
        dtMDef.Columns.Add(new DataColumn("LOTGRADE", typeof(string)));                       /*2 */
        dtMDef.Columns.Add(new DataColumn("DEFCODE", typeof(string)));                        /*3 */
        dtMDef.Columns.Add(new DataColumn("DEFECTDESC", typeof(string)));                     /*4 */
        dtMDef.Columns.Add(new DataColumn("TTL_CNT", typeof(long)));                          /*5 */
        dtMDef.Columns.Add(new DataColumn("S8180", typeof(long)));                            /*6 */
        dtMDef.Columns.Add(new DataColumn("S8210", typeof(long)));                            /*7 */
        dtMDef.Columns.Add(new DataColumn("S8220", typeof(long)));                            /*1.17 */
        dtMDef.Columns.Add(new DataColumn("S8230", typeof(long)));                            /*8 */
        dtMDef.Columns.Add(new DataColumn("S8234", typeof(long)));                            /*1.14*/
        dtMDef.Columns.Add(new DataColumn("S8240", typeof(long)));                            /*1.13*/
        dtMDef.Columns.Add(new DataColumn("S8250", typeof(long)));                            /*9 */
        dtMDef.Columns.Add(new DataColumn("S8280", typeof(long)));                            /*10 */

        dtMDef.Columns.Add(new DataColumn("S8290", typeof(long)));                            /*1.11*/

        dtMDef.Columns.Add(new DataColumn("S8285", typeof(long)));                            /*1.12*/
        dtMDef.Columns.Add(new DataColumn("S8292", typeof(long)));                            /*1.12*/
        dtMDef.Columns.Add(new DataColumn("S8294", typeof(long)));                            /*1.12*/
        dtMDef.Columns.Add(new DataColumn("S8296", typeof(long)));                            /*1.12*/

        dtMDef.Columns.Add(new DataColumn("S829X", typeof(long)));                            /*1.16*/
        dtMDef.Columns.Add(new DataColumn("S8297", typeof(long)));                            /*1.16*/
        dtMDef.Columns.Add(new DataColumn("S8298", typeof(long)));                            /*1.16*/
        dtMDef.Columns.Add(new DataColumn("S8299", typeof(long)));                            /*1.16*/

        dtMDef.Columns.Add(new DataColumn("S8300", typeof(long)));                            /*11 */
        dtMDef.Columns.Add(new DataColumn("S8305", typeof(long)));                            /*1.15 */
        dtMDef.Columns.Add(new DataColumn("S8350", typeof(long)));                            /*12 */
        dtMDef.Columns.Add(new DataColumn("S8360", typeof(long)));                            /*13 */
        dtMDef.Columns.Add(new DataColumn("S8380", typeof(long)));                            /*14 */
        dtMDef.Columns.Add(new DataColumn("DefectRate", typeof(double)));                     /*15 */
        dtMDef.Columns.Add(new DataColumn("Percentage", typeof(double)));                     /*16 */
        dtMDef.Columns.Add(new DataColumn("ItemNo", typeof(int)));                            /*17*/
        dtMDef.Columns.Add(new DataColumn("S8260", typeof(long)));                            /*18 */
        dtMDef.Columns.Add(new DataColumn("S8270", typeof(long)));                            /*19 */
        dtMDef.Columns.Add(new DataColumn("S8355", typeof(long)));                            /*20 */
        //1.09 begin
        dtMDef.Columns.Add(new DataColumn("S8351", typeof(long)));                            /*21 */
        dtMDef.Columns.Add(new DataColumn("S8352", typeof(long)));                            /*22 */
        dtMDef.Columns.Add(new DataColumn("S8353", typeof(long)));                            /*23 */
        //1.09 end
        dtMDef.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] DFkeys = new DataColumn[4];
        DFkeys[0] = dtMDef.Columns["ReportDT"];
        DFkeys[1] = dtMDef.Columns["PRODUCT_ID"];
        DFkeys[2] = dtMDef.Columns["LOTGRADE"];
        DFkeys[3] = dtMDef.Columns["DEFCODE"];
        dtMDef.PrimaryKey = DFkeys;
        /*-------------------------------------------------------------------------------------*/
        Session["dtMDef"] = null;
        Session["dtMDef"] = dtMDef;
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        string strSubLineID = "'='";
        string strProdID = "'='";
        string strLotType = "'='";
        string strWonbr = "'='";
        string strLCDProdID = "'='";  //V1.10 Add

        lblTTime.Text = "";
        lblTime01.Text = "";
        lblTime02.Text = "";
        DateTime dtF = System.DateTime.Now;
        /*--------------------------------------------------------------------------------------*/
        tbDateType.Text = "";
        tbDiffDate.Text = "0";
        tbCPFlag.Text = "";
        tbFDate.Text = "";
        tbTDate.Text = "";

        lblFabID.Text = tbFABName.Text;

        Session["dtMFPY"] = null;
        Session["dtMDef"] = null;

        IBGBYData.Enabled = false;
        cblLevel.Enabled = false;
        /*V1.01 Begin*/
        RadioButtonList1.Enabled = false;
        /*V1.01 End*/

        ddlByProdID.Items.Clear();
        ddlSDefList.Items.Clear();

        ClearChartBound();

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
        /*V1.10 Add Start*/
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
        /*V1.10 Add End*/
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
            //tbFDate.Text = ddlFDate.SelectedValue.Replace("W0", "").Replace("W", "");
            //tbTDate.Text = ddlTDate.SelectedValue.Replace("W0", "").Replace("W", "");
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
        if (ddlShiftID.SelectedValue != "ShiftID") { tbSqlWhere.Text = tbSqlWhere.Text + " AND SHIFT_ID LIKE '%" + ddlShiftID.SelectedValue + "'"; }
        if (strSubLineID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND SUBLINEID IN(" + strSubLineID + ")"; }
        if (strLotType != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND LOT_TYPE IN(" + strLotType + ")"; }
        if (tbCPFlag.Text == "C")
        {
            //if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }                              //V1.10 Mark
            if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strProdID + ")"; }       //V1.10 Add
            if (strLCDProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND CPRODUCT_ID IN(" + strLCDProdID + ")"; }                          //V1.10 Add
        }
        else
        {
            //if (strProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }                              //V1.10 Mark
            if (strProdID != "'='" && strLCDProdID == "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strProdID + ")"; }       //V1.10 Add
            if (strLCDProdID != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND PRODUCT_ID IN(" + strLCDProdID + ")"; }                          //V1.10 Add
        }
        if (strWonbr != "'='") { tbSqlWhere.Text = tbSqlWhere.Text + " AND WORK_ORDER IN(" + strWonbr + ")"; }
        /*-------------------------------------------------------------------------------------*/
        CreateYieldSessionDataTable();
        CreateDefSessionDataTable();
        /*--------------------------------------------------------------------------------------*/

        /*--------------------------------------------------------------------------------------*/
        QueryFPYDataBound("TOTAL", "ALL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue);
        /*V1.10 Add Start*/
        if (ChkMerge.Checked)
        {
            QueryFPYDataBound("TOTALPD", "ALL");
        }
        /*V1.10 Add End*/
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "S");//20121226
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "S");
        GridViewDataBound();
        ChartDataBound();
        DataTable dtTmp = (DataTable)Session["dtMFPY"];
        string strLogWhere = "DateType:" + ddlDateType.SelectedValue.ToString() +
                             ";From:" + tbFDate.Text.ToString() +
                             ";To:" + tbTDate.Text.ToString() +
                             ";Year:" + ddlYear.SelectedValue.ToString() +
                             ";Cell2Prodid:" + cbCPFlag.Checked.ToString() +
                             ";Length:" + ddlPdLen.SelectedValue.ToString() +
                             ";ShiftID:" + ddlShiftID.SelectedValue.ToString() +
                             ";SelType:" + ddlSelType.SelectedValue.ToString() +        /*V1.04*/
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
        /*V1.01 Begin*/
        RadioButtonList1.Enabled = true;
        /*V1.01 End*/
    }
    protected void QueryFPYDataBound(string strMode, string strTmpProdID)
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            /*-------------------------------------------------------------------------------------*/
            /*機種清單*/
            /*V1.10 Add Start*/
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
                /*V1.10 Add End*/
                if (strMode != "Days")
                {
                    strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
                    strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK) ");
                    strSql.Append(tbSqlDate.Text);
                    strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
                    /*V1.04 Begin*/
                    if (ddlSelType.SelectedValue == "MODULE")
                    {
                        strSql.Append(" AND OPERATION_ID IN('8180-Mark','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380')");       /*V1.00*/ /*V1.02*/ /*V1.03*//*V1.05 ADD 81LW*//*V1.08*/ //1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
                    }
                    else
                    {
                        strSql.Append(" AND OPERATION_ID IN('8180','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8360','8380')");       /*V1.04*/ /*V1.05 ADD 81LW*/ //1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
                    }
                    /*V1.04 End*/
                    strSql.Append(tbSqlWhere.Text);
                    strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ")");
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
            }/*V1.10 Add*/
            /*-------------------------------------------------------------------------------------*/

            strSql.Remove(0, strSql.Length);
            if (strMode == "Days")
            {
                strSql.Append(" SELECT CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek+1 AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END AS ReportDT,");
            }
            else
            {
                strSql.Append(" SELECT 'TOTAL' AS ReportDT,");
            }
            /*V1.10 Add Start*/
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
                strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");	/*1*/
            }
            /*V1.10 Add End*/
            //strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");	/*1*//*V1.10 Mark*/
            strSql.Append(" OPERATION_ID,");     					     /*2 */
            strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");                             /*3 */
            strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");                             /*4 */
            strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT ");                             /*6 */
            strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK)");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            /*V1.04 Begin*/
            if (ddlSelType.SelectedValue == "MODULE")
            {
                strSql.Append(" AND OPERATION_ID IN('8180-Mark','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380')");    /*V1.00*/ /*V1.02*/ /*V1.03*//*V1.05 ADD 81LW*//*V1.08*///1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
            }
            else
            {
                strSql.Append(" AND OPERATION_ID IN('8180','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8355','8350','8351','8352','8353','8360','8380')");    /*V1.04*//*V1.05 ADD 81LW*///1.09 /*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
            }
            /*V1.04 End*/
            //if (strMode == "Days")     //V1.10 Mark
            if (strMode == "Days" && !ChkMerge.Checked)     //V1.10 Add
            {
                if (tbCPFlag.Text == "C")
                {
                    strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                }
                else
                {
                    strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                }
            }
            strSql.Append(tbSqlWhere.Text);
            strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") ");
            if (strMode == "Days")
            {
                strSql.Append(" ,CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek+1 AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END");
            }
            DataSet dt011 = new DataSet();
            SqlDataAdapter sqlApt011 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt011.Fill(dt011);
            /*-------------------------------------------------------------------------------------*/
            BoudFPYDataTable(dt011, strMode, strTmpProdID);
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
    protected void QueryDefectDataBound(string strMode, string strTmpProdID, string strGradeID)
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            DBConn dbConn = new DBConn(tbFABID.Text.ToString());
            //if (strMode != "Days")
            //{
            //    ddlRDefList.Items.Clear();
            //    ddlRDefList.Items.Clear();
            //}
            /*-------------------------------------------------------------------------------------*/
            /*計算各機種DefectCode數量*/
            strSql.Append(" SELECT ");
            if (strMode == "Days")
            {
                strSql.Append(" CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek+1 AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END AS ReportDT,");
            }
            else
            {
                strSql.Append(" 'TOTAL' AS ReportDT,");
            }
            /*V1.10 Add start*/
            if (ChkMerge.Checked)
            {
                strSql.Append(" 'ALL' AS PRODUCT_ID,");
            }
            else
            {
                /*V1.10 Add End*/
                strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
            }
            //strSql.Append(" LOT_GRADE, ");										                                    /*2 */
            if (strGradeID != "ALL")
            {
                strSql.Append(" LOT_GRADE, ");										                                    /*2 */
            }
            else
            {
                strSql.Append("'R+S' AS LOT_GRADE, ");
            }
            /*V1.01 Begin*/
            /*
strSql.Append(" DEF_CODE, ");                                                                            
strSql.Append(" DEFECTDESC,  ");                                                                         
*/
            //if (RadioButtonList1.Items.FindByText("DefectGroup").Selected)
            //{
            //    strSql.Append(" (CASE WHEN C.DEFECTGROUPCD IS NULL THEN 'Other' ELSE C.DEFECTGROUPCD END) AS 'DEFCGROUP', ");                /*3 */
            //    strSql.Append(" ISNULL(C.DEFGROUPDESC,'其他') AS DEFGROUPDESC, ");                                                                /*4 */
            //}
            //else
            //{
            strSql.Append(" DEF_CODE, ");                                                                          /*3 */
            strSql.Append(" DEFECTDESC,  ");                                                                       /*4 */
            //}
            /*V1.01 End*/


            strSql.Append(" SUM(DEF_CNT) AS TTL_CNT,  ");                                                           /*5 */
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8180' THEN DEF_CNT ELSE 0 END) AS 'S8180',  ");           /*6 */
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8210' THEN DEF_CNT ELSE 0 END) AS 'S8210',  ");           /*7 */
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8220' THEN DEF_CNT ELSE 0 END) AS 'S8220',  ");           /*33    1.17*/
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8230' THEN DEF_CNT ELSE 0 END) AS 'S8230',  ");           /*8 */
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8250' THEN DEF_CNT ELSE 0 END) AS 'S8250',  ");           /*9 */
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8280' THEN DEF_CNT ELSE 0 END) AS 'S8280',  ");           /*10*/

           

            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8300' THEN DEF_CNT ELSE 0 END) AS 'S8300',  ");           /*11*/
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8350' THEN DEF_CNT ELSE 0 END) AS 'S8350',  ");           /*12*/
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8360' THEN DEF_CNT ELSE 0 END) AS 'S8360',  ");           /*13*/
            strSql.Append(" SUM(CASE WHEN OPERATION_ID = '8380' THEN DEF_CNT ELSE 0 END) AS 'S8380'   ");           /*14*/

            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8260' THEN DEF_CNT ELSE 0 END) AS 'S8260'   ");          /*15*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8270' THEN DEF_CNT ELSE 0 END) AS 'S8270'   ");          /*16*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8355' THEN DEF_CNT ELSE 0 END) AS 'S8355'   ");          /*17*/
            //1.09 begin
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8351' THEN DEF_CNT ELSE 0 END) AS 'S8351'   ");          /*18*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8352' THEN DEF_CNT ELSE 0 END) AS 'S8352'   ");          /*19*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8353' THEN DEF_CNT ELSE 0 END) AS 'S8353'   ");          /*20*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8290' THEN DEF_CNT ELSE 0 END) AS 'S8290'  ");           /*21    1.11*/
            //1.09 end


            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8285' THEN DEF_CNT ELSE 0 END) AS 'S8285'  ");           /*22    1.12*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8292' THEN DEF_CNT ELSE 0 END) AS 'S8292'  ");           /*23    1.12*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8294' THEN DEF_CNT ELSE 0 END) AS 'S8294'  ");           /*24    1.12*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8296' THEN DEF_CNT ELSE 0 END) AS 'S8296'  ");           /*25    1.12*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8240' THEN DEF_CNT ELSE 0 END) AS 'S8240'  ");           /*26    1.13*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8234' THEN DEF_CNT ELSE 0 END) AS 'S8234'  ");           /*27    1.14*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8305' THEN DEF_CNT ELSE 0 END) AS 'S8305'  ");           /*28    1.15*/

            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '829X' THEN DEF_CNT ELSE 0 END) AS 'S829X'  ");           /*29    1.16*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8297' THEN DEF_CNT ELSE 0 END) AS 'S8297'  ");           /*30    1.16*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8298' THEN DEF_CNT ELSE 0 END) AS 'S8298'  ");           /*31    1.16*/
            strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8299' THEN DEF_CNT ELSE 0 END) AS 'S8299'  ");           /*32    1.16*/

            //strSql.Append(" ,SUM(CASE WHEN OPERATION_ID = '8220' THEN DEF_CNT ELSE 0 END) AS 'S8220'  ");           /*33    1.17*/

            strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK) ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND A.FAB_ID = '" + tbFABID.Text + "' ");
            /*V1.04 Begin*/
            if (ddlSelType.SelectedValue == "MODULE")
            {
                strSql.Append(" AND A.OPERATION_ID IN('8180-Mark','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380')");     /*V1.00*/ /*V1.02*/ /*V1.03*//*V1.05 ADD 81LW*//*V1.08*///1.09 /*1.11*//1.12*//1.13 8240*//1.14 8234*//1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
            }
            else
            {
                strSql.Append(" AND A.OPERATION_ID IN('8180','8210','8220','8230','8234','8240','8250','8260','8270','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8351','8352','8353','8355','8360','8380')");     /*V1.04*//*V1.05 ADD 81LW*///1.09 /*1.11*//1.12*//1.13 8240*//1.14 8234*//1.15 8305*//*1.16 829X,8297,8298,8299*//*1.17 8220*/
            }
            /*V1.04 End*/
            /*V1.10 Add Start*/
            if (ChkMerge.Checked)
            {
                if (strTmpProdID != "'='" && strTmpProdID != "ALL")
                {
                    if (tbCPFlag.Text == "C")
                    {
                        strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                    else
                    {
                        strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                    }
                }
            }
            else
            {
                /*V1.10 Add End*/
                if (tbCPFlag.Text == "C")
                {
                    strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                }
                else
                {
                    strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ") ='" + strTmpProdID + "' ");
                }
            }/*V1.10 Add*/
            if (strGradeID == "ALL")
            {
                strSql.Append(" AND A.LOT_GRADE IN('R','S')");
            }
            else
            {
                strSql.Append(" AND A.LOT_GRADE = '" + strGradeID + "'");
            }
            strSql.Append(tbSqlWhere.Text);
            strSql.Append(" AND A.FAB_ID=B.FABID");
            strSql.Append(" AND A.DEF_CODE=B.DEFECTCD");
            strSql.Append(" GROUP BY ");
            if (strMode == "Days")
            {
                strSql.Append(" CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek+1 AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END, ");
            }
            if (!ChkMerge.Checked)
            {
                strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,1," + ddlPdLen.SelectedValue + "), ");
            }
            //strSql.Append(" LOT_GRADE,DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            if (strGradeID != "ALL")
            {
                strSql.Append(" LOT_GRADE,");
            }
            /*V1.06 Begin*/
            ///*V1.01 Begin*//*
            //strSql.Append(" DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");*/
            //if (RadioButtonList1.Items.FindByText("DefectGroup").Selected)
            //{
            //    strSql.Append(" C.DEFECTGROUPCD,C.DEFGROUPDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            //else
            //{
            //    strSql.Append(" DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            ///*V1.01 End*/
            //if (RadioButtonList1.Items.FindByText("DefectGroup").Selected)
            //{
            //    strSql.Append(" C.DEFECTGROUPCD,C.DEFGROUPDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            //else
            //{
            strSql.Append(" DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            /*V1.06 Begin*/
            DataSet dt012 = new DataSet();
            SqlDataAdapter sqlApt012 = new SqlDataAdapter(strSql.ToString(), dbConn.ConnectionString);
            sqlApt012.Fill(dt012);
            /*V1.10 Add Start*/
            if (ChkMerge.Checked)
            {
                strTmpProdID = "ALL";
            }
            /*V1.10 Add End*/
            /*-------------------------------------------------------------------------------------*/
            BoudDefectDataTable(dt012, strMode, strTmpProdID, strGradeID);
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
    private void BoudFPYDataTable(DataSet dsTmp, string strMode, string strTmpProdID)
    {
        try
        {
            DataTable dtMFPY = (DataTable)Session["dtMFPY"];
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
                            if (tbDateType.Text == "M") { strReportDT = "M0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }
                        else
                        {
                            if (tbDateType.Text == "M") { strReportDT = "M" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }

                        if (Convert.ToString((Convert.ToInt32(tbFDate.Text) + 1 + DTCnt)).Length == 1)
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + 1 + DTCnt)); }
                        }
                        else
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + 1 + DTCnt)); }
                        }
                    }
                }
                /*-------------------------------------------------------------------------------------*/
                /*循環處理各站點的產品的數據*/
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
                    //for (int i = 0; i <= 14; i++)    /*V1.00 9->11*//*V1.03 11->22*//*V1.05 22->23*/ //1.09 11==>14
                    for (int i = 0; i <= 27; i++)    /*V1.00 9->11*//*V1.03 11->22*//*V1.05 22->23*/ //1.09 11==>14//Modify by xuchen 2015.10.08 /*1.12*//*1.13 19==>20*//*1.14 20==>21*//*1.15 21==>22*//*1.16 22==>26*//*1.17 26==>27*/
                    {
                        string strOPERATIONID = "";       /*站點代碼*/
                        string strType = "";              /*站點類型描述*/
                        switch (i)
                        {
                            case 0: strOPERATIONID = "8180"; strType = "S8180"; break;
                            case 1: strOPERATIONID = "8210"; strType = "S8210"; break;
                            case 2: strOPERATIONID = "8220"; strType = "S8220"; break;/*1.17*/
                            case 3: strOPERATIONID = "8230"; strType = "S8230"; break;
                            case 4: strOPERATIONID = "8234"; strType = "S8234"; break; /*1.14*/
                            case 5: strOPERATIONID = "8240"; strType = "S8240"; break; /*1.13*//*1.14 3==>4*/
                            case 6: strOPERATIONID = "8250"; strType = "S8250"; break;/*1.13 3==>4*//*1.14 4==>5*/
                            case 7: strOPERATIONID = "8260"; strType = "S8260"; break;/*1.13 4==>5*//*1.14 5==>6*/
                            case 8: strOPERATIONID = "8270"; strType = "S8270"; break;/*1.13 5==>6*//*1.14 6==>7*/
                            case 9: strOPERATIONID = "8280"; strType = "S8280"; break;/*1.13 6==>7*//*1.14 7==>8*/
                            case 10: strOPERATIONID = "8290"; strType = "S8290"; break; /*1.11*//*1.13 7==>8*//*1.14 8==>9*/
                            case 11: strOPERATIONID = "8285"; strType = "S8285"; break; /*1.12*//*1.13 8==>9*//*1.14 9==>10*/
                            case 12: strOPERATIONID = "8292"; strType = "S8292"; break; /*1.12*//*1.13 9==>10*//*1.14 10==>11*/
                            case 13: strOPERATIONID = "8294"; strType = "S8294"; break; /*1.12*//*1.13 10==>11*//*1.14 11==>12*/
                            case 14: strOPERATIONID = "8296"; strType = "S8296"; break; /*1.12*//*1.13 11==>12*//*1.14 12==>13*/
                            case 15: strOPERATIONID = "829X"; strType = "S829X"; break;/*1.16 */
                            case 16: strOPERATIONID = "8297"; strType = "S8297"; break;/*1.16 */
                            case 17: strOPERATIONID = "8298"; strType = "S8298"; break;/*1.16 */
                            case 18: strOPERATIONID = "8299"; strType = "S8299"; break;/*1.16 */
                            case 19: strOPERATIONID = "8300"; strType = "S8300"; break;/*1.13 12==>13*//*1.14 13==>14*//*1.16 14==>18*/
                            case 20: strOPERATIONID = "8305"; strType = "S8305"; break;/*1.15 *//*1.16 15==>19*/
                            case 21: strOPERATIONID = "8350"; strType = "S8350"; break;/*1.13 13==>14*//*1.14 14==>15*//*1.15 15==>16*//*1.16 16==>20*/
                            case 22: strOPERATIONID = "8351"; strType = "S8351"; break;/*1.13 14==>15*//*1.14 15==>16*//*1.15 16==>17*//*1.16 17==>21*/
                            case 23: strOPERATIONID = "8352"; strType = "S8352"; break;/*1.13 15==>16*//*1.14 16==>17*//*1.15 17==>18*//*1.16 18==>22*/
                            case 24: strOPERATIONID = "8353"; strType = "S8353"; break;/*1.13 16==>17*//*1.14 17==>18*//*1.15 18==>19*//*1.16 19==>23*/
                            case 25: strOPERATIONID = "8355"; strType = "S8355"; break;/*1.13 17==>18*//*1.14 18==>19*//*1.15 19==>20*//*1.16 20==>24*/
                            case 26: strOPERATIONID = "8360"; strType = "S8360"; break;/*1.13 18==>19*//*1.14 19==>20*//*1.15 20==>21*//*1.16 21==>25*/
                            case 27: strOPERATIONID = "8380"; strType = "S8380"; break;/*1.13 19==>20*//*1.14 20==>21*//*1.15 21==>22*//*1.16 22==>26*/



                            //case 2: strOPERATIONID = "8230"; strType = "S8230"; break;
                            //case 3: strOPERATIONID = "8234"; strType = "S8234"; break; /*1.14*/
                            //case 4: strOPERATIONID = "8240"; strType = "S8240"; break; /*1.13*//*1.14 3==>4*/
                            //case 5: strOPERATIONID = "8250"; strType = "S8250"; break;/*1.13 3==>4*//*1.14 4==>5*/

                            //case 6: strOPERATIONID = "8260"; strType = "S8260"; break;/*1.13 4==>5*//*1.14 5==>6*/
                            //case 7: strOPERATIONID = "8270"; strType = "S8270"; break;/*1.13 5==>6*//*1.14 6==>7*/
                            //case 8: strOPERATIONID = "8280"; strType = "S8280"; break;/*1.13 6==>7*//*1.14 7==>8*/

                            ////case 7: strOPERATIONID = "8300"; strType = "S8300"; break;//Modify by xuchen 2015.10.08
                            ////case 8: strOPERATIONID = "8350"; strType = "S8350"; break;
                            ////    //1.09 being
                            //////case 9: strOPERATIONID = "8355"; strType = "S8355"; break;

                            //////case 10: strOPERATIONID = "8360"; strType = "S8360"; break;
                            //////case 11: strOPERATIONID = "8380"; strType = "S8380"; break;
                            ////case 9: strOPERATIONID = "8351"; strType = "S8351"; break;

                            ////case 10: strOPERATIONID = "8352"; strType = "S8352"; break;
                            ////case 11: strOPERATIONID = "8353"; strType = "S8353"; break;
                            ////case 12: strOPERATIONID = "8355"; strType = "S8355"; break;

                            ////case 13: strOPERATIONID = "8360"; strType = "S8360"; break;
                            ////case 14: strOPERATIONID = "8380"; strType = "S8380"; break;
                            ////1.09 end

                            //case 9: strOPERATIONID = "8290"; strType = "S8290"; break; /*1.11*//*1.13 7==>8*//*1.14 8==>9*/
                            //case 10: strOPERATIONID = "8285"; strType = "S8285"; break; /*1.12*//*1.13 8==>9*//*1.14 9==>10*/
                            //case 11: strOPERATIONID = "8292"; strType = "S8292"; break; /*1.12*//*1.13 9==>10*//*1.14 10==>11*/
                            //case 12: strOPERATIONID = "8294"; strType = "S8294"; break; /*1.12*//*1.13 10==>11*//*1.14 11==>12*/
                            //case 13: strOPERATIONID = "8296"; strType = "S8296"; break; /*1.12*//*1.13 11==>12*//*1.14 12==>13*/
                            ////1.12 begin mark
                            ////case 8: strOPERATIONID = "8300"; strType = "S8300"; break;
                            ////case 9: strOPERATIONID = "8350"; strType = "S8350"; break;
                            ////case 10: strOPERATIONID = "8351"; strType = "S8351"; break;
                            ////case 11: strOPERATIONID = "8352"; strType = "S8352"; break;
                            ////case 12: strOPERATIONID = "8353"; strType = "S8353"; break;
                            ////case 13: strOPERATIONID = "8355"; strType = "S8355"; break;
                            ////case 14: strOPERATIONID = "8360"; strType = "S8360"; break;
                            ////case 15: strOPERATIONID = "8380"; strType = "S8380"; break;
                            ////1.12 end
                            ////1.12 begin add
                            //case 14: strOPERATIONID = "829X"; strType = "S829X"; break;/*1.16 */
                            //case 15: strOPERATIONID = "8297"; strType = "S8297"; break;/*1.16 */
                            //case 16: strOPERATIONID = "8298"; strType = "S8298"; break;/*1.16 */
                            //case 17: strOPERATIONID = "8299"; strType = "S8299"; break;/*1.16 */

                            //case 18: strOPERATIONID = "8300"; strType = "S8300"; break;/*1.13 12==>13*//*1.14 13==>14*//*1.16 14==>18*/
                            //case 19: strOPERATIONID = "8305"; strType = "S8305"; break;/*1.15 *//*1.16 15==>19*/
                            //case 20: strOPERATIONID = "8350"; strType = "S8350"; break;/*1.13 13==>14*//*1.14 14==>15*//*1.15 15==>16*//*1.16 16==>20*/
                            //case 21: strOPERATIONID = "8351"; strType = "S8351"; break;/*1.13 14==>15*//*1.14 15==>16*//*1.15 16==>17*//*1.16 17==>21*/
                            //case 22: strOPERATIONID = "8352"; strType = "S8352"; break;/*1.13 15==>16*//*1.14 16==>17*//*1.15 17==>18*//*1.16 18==>22*/
                            //case 23: strOPERATIONID = "8353"; strType = "S8353"; break;/*1.13 16==>17*//*1.14 17==>18*//*1.15 18==>19*//*1.16 19==>23*/
                            //case 24: strOPERATIONID = "8355"; strType = "S8355"; break;/*1.13 17==>18*//*1.14 18==>19*//*1.15 19==>20*//*1.16 20==>24*/
                            //case 25: strOPERATIONID = "8360"; strType = "S8360"; break;/*1.13 18==>19*//*1.14 19==>20*//*1.15 20==>21*//*1.16 21==>25*/
                            //case 26: strOPERATIONID = "8380"; strType = "S8380"; break;/*1.13 19==>20*//*1.14 20==>21*//*1.15 21==>22*//*1.16 22==>26*/
                            //1.12 end
                            default:
                                break;
                        }
                        /*查詢個站點的資料*/
                        DataRow[] drType = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + strOPERATIONID + "')", "PRODUCT_ID DESC");
                        if (drType.Length > 0)
                        {
                            long lngMQty = 0;
                            long lngChipQty = 0;
                            double dblAYield = 0, dblSYield = 0;

                            /*
                            ChipQty=A+R+S
                            AYield=A/(ChipQty)
                            RYield=R/(ChipQty)
                            SYield=S/(ChipQty)
                            AAYield=AA/(ChipQty)
                            A?Yield=A?/(ChipQty)
                            (AA+A0+A1+A2)Yield=A?/(ChipQty)
                             */
                            lngMQty = Convert.ToInt32(drType[0].ItemArray[3]);
                            lngChipQty = Convert.ToInt32(drType[0].ItemArray[3]);
                            dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4])) / lngMQty, 4) * 100;
                            dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[5])) / lngMQty, 4) * 100;

                            DataRow drFPY = dtMFPY.NewRow();
                            drFPY["ReportDT"] = strReportDT;
                            drFPY["PRODUCT_ID"] = strPRODUCTID;
                            drFPY["OPERATION_ID"] = strType;
                            drFPY["Chip_Qty"] = lngChipQty;
                            drFPY["A_Yield"] = dblAYield;
                            drFPY["S_Yield"] = dblSYield;
                            drFPY["ItemNo"] = i;
                            dtMFPY.Rows.Add(drFPY);
                            /*-------------------------------------------------------------------------------------*/
                            /*處理FPY的數據*/
                            object[] findTheseVals = new object[3];
                            if (strMode != "Days")
                            {
                                findTheseVals[0] = "TOTAL";
                            }
                            else
                            {
                                findTheseVals[0] = strReportDT;
                            }
                            findTheseVals[1] = strPRODUCTID;
                            findTheseVals[2] = "FPY";
                            DataRow drFPYCheck = dtMFPY.Rows.Find(findTheseVals);

                            if (drFPYCheck == null)   /*不存在數據則新增到數據集*/
                            {
                                DataRow drFPYNew = dtMFPY.NewRow();
                                drFPYNew.Table.Columns.Add();
                                drFPYNew["ReportDT"] = strReportDT;
                                drFPYNew["PRODUCT_ID"] = strPRODUCTID;
                                drFPYNew["OPERATION_ID"] = "FPY";
                                drFPYNew["Chip_Qty"] = lngChipQty;
                                drFPYNew["A_Yield"] = dblAYield;
                                drFPYNew["S_Yield"] = dblSYield;
                                drFPYNew["ItemNo"] = 999;  /*設定為無限大排定優先*/
                                dtMFPY.Rows.Add(drFPYNew);
                            }
                            else  /*存在數據則更新現有數據集*/
                            {
                                drFPYCheck.BeginEdit();
                                drFPYCheck["Chip_Qty"] = lngChipQty;
                                drFPYCheck["A_Yield"] = Math.Round(Convert.ToDouble(drFPYCheck["A_Yield"]) * dblAYield / 100, 2);
                                drFPYCheck["S_Yield"] = Math.Round(1 - (1 - Convert.ToDouble(drFPYCheck["S_Yield"]) / 100) * (1 - dblSYield / 100), 4) * 100;
                                drFPYCheck.EndEdit();
                            }
                        }
                    }
                }
            }
            Session["dtMFPY"] = null;
            Session["dtMFPY"] = dtMFPY;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BoudDefectDataTable(DataSet dsTmp, string strMode, string strTmpProdID, string strGradeID)
    {
        try
        {
            DataTable dtMDef = (DataTable)Session["dtMDef"];
            DataTable dtMFPY = (DataTable)Session["dtMFPY"];
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
                            if (tbDateType.Text == "M") { strReportDT = "M0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }
                        else
                        {
                            if (tbDateType.Text == "M") { strReportDT = "M" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + DTCnt)); }
                        }

                        if (Convert.ToString((Convert.ToInt32(tbFDate.Text) + 1 + DTCnt)).Length == 1)
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W0" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + 1 + DTCnt)); }
                        }
                        else
                        {
                            if (tbDateType.Text == "W") { strReportDT = "W" + Convert.ToString((Convert.ToInt32(tbFDate.Text) + 1 + DTCnt)); }
                        }
                    }
                }
                /*-------------------------------------------------------------------------------------*/
                string strPRODUCTID = "";         /*產品型號*/
                strPRODUCTID = strTmpProdID.ToString();       /*獲取產品型號*/
                /*-------------------------------------------------------------------------------------*/
                /*處理各產品的S&R DefectCode的數據*/
                //for (int i = 0; i < 2; i++)
                //{
                string strGrade = "";
                strGrade = "S";
                /*-------------------------------------------------------------------------------------*/
                DataRow[] drDef = dsTmp.Tables[0].Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND LOT_GRADE = '" + strGrade + "' )", "TTL_CNT DESC");
                if (drDef.Length > 0)
                {
                    for (int DefCnt = 0; DefCnt < Convert.ToInt32(drDef.Length); DefCnt++)
                    {

                        if (strMode != "Days")
                        {
                            if (strGrade == "S")
                            {
                                ddlSDefList.Items.Add(drDef[DefCnt].ItemArray[3].ToString() + " " + drDef[DefCnt].ItemArray[4].ToString());
                            }
                        }

                        //ddlStation.Items.Add("M1(8180)");
                        //ddlStation.Items.Add("M2-COG(8210)");
                        //ddlStation.Items.Add("M2-FPC(8230)");
                        //ddlStation.Items.Add("涂膠(8250)");
                        //ddlStation.Items.Add("OCA(8260)");
                        //ddlStation.Items.Add("HalfModuleTest(8270)");
                        //ddlStation.Items.Add("M2(8280)");
                        //ddlStation.Items.Add("AssemblyReceived(8300)");
                        //ddlStation.Items.Add("Assembly(8350)");
                        //ddlStation.Items.Add("AssemblyResorting(8355)");
                        //ddlStation.Items.Add("OQC(8360)");



                        long lng8180 = 0, lng8210 = 0, lng8220 = 0, lng8230 = 0, lng8234 = 0, lng8240 = 0, lng8250 = 0, lng8260 = 0, lng8270 = 0, lng8280 = 0, lng8290 = 0, lng8285 = 0, lng8292 = 0, lng8294 = 0, lng8296 = 0, lng829X = 0, lng8297 = 0, lng8298 = 0, lng8299 = 0, lng8300 = 0, lng8305 = 0, lng8350 = 0, lng8355 = 0, lng8360 = 0, lng8380 = 0;/*1.11*//*1.12*//*1.13*//*1.14*//*1.15*//*1.17 8220*/
                        long lng8351 = 0, lng8352 = 0, lng8353 = 0; //1.09
                        double dblDefectRate = 1;

                        DataRow[] dr8180 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8180')");
                        DataRow[] dr8210 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8210')");
                        DataRow[] dr8220 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8220')");/*1.17*/
                        DataRow[] dr8230 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8230')");
                        DataRow[] dr8234 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8234')");/*1.14*/
                        DataRow[] dr8240 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8240')");/*1.13*/
                        DataRow[] dr8250 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8250')");

                        DataRow[] dr8260 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8260')");
                        DataRow[] dr8270 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8270')");

                        DataRow[] dr8280 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8280')");

                        DataRow[] dr8290 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8290')"); /*1.11*/
                        DataRow[] dr8285 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8285')"); /*1.12*/
                        DataRow[] dr8292 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8292')"); /*1.12*/
                        DataRow[] dr8294 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8294')"); /*1.12*/
                        DataRow[] dr8296 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8296')"); /*1.12*/

                        DataRow[] dr829X = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S829X')"); /*1.16*/
                        DataRow[] dr8297 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8297')"); /*1.16*/
                        DataRow[] dr8298 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8298')"); /*1.16*/
                        DataRow[] dr8299 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8299')"); /*1.16*/

                        DataRow[] dr8300 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8300')");
                        DataRow[] dr8305 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8305')");/*1.15*/
                        DataRow[] dr8350 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8350')");
                        //1.09 begin
                        DataRow[] dr8351 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8351')");
                        DataRow[] dr8352 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8352')");
                        DataRow[] dr8353 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8353')");
                        //1.09 end 
                        DataRow[] dr8355 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8355')");

                        DataRow[] dr8360 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8360')");
                        DataRow[] dr8380 = dtMFPY.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='S8380')");
                        if (dr8180.Length > 0)
                        {
                            lng8180 = Convert.ToInt32(dr8180[0].ItemArray[3]);
                            if (lng8180 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[6]) / lng8180);
                            }
                        }
                        if (dr8210.Length > 0)
                        {
                            lng8210 = Convert.ToInt32(dr8210[0].ItemArray[3]);
                            if (lng8210 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[7]) / lng8210);
                            }
                        }
                        if (dr8220.Length > 0)
                        {
                            lng8220 = Convert.ToInt32(dr8220[0].ItemArray[3]);
                            if (lng8220 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[8]) / lng8220);
                            }
                        }
                        if (dr8230.Length > 0)
                        {
                            lng8230 = Convert.ToInt32(dr8230[0].ItemArray[3]);
                            if (lng8230 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[9]) / lng8230);
                            }
                        }
                        if (dr8250.Length > 0)
                        {
                            lng8250 = Convert.ToInt32(dr8250[0].ItemArray[3]);
                            if (lng8250 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[10]) / lng8250);
                            }
                        }

                        if (dr8260.Length > 0)
                        {
                            lng8260 = Convert.ToInt32(dr8260[0].ItemArray[3]);
                            if (lng8260 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[16]) / lng8260);
                            }
                        }
                        if (dr8270.Length > 0)
                        {
                            lng8270 = Convert.ToInt32(dr8270[0].ItemArray[3]);
                            if (lng8270 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[17]) / lng8270);
                            }
                        }

                        if (dr8280.Length > 0)
                        {
                            lng8280 = Convert.ToInt32(dr8280[0].ItemArray[3]);
                            if (lng8280 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[11]) / lng8280);
                            }
                        }

                        /*1.11 Begin*/
                        if (dr8290.Length > 0)
                        {
                            lng8290 = Convert.ToInt32(dr8290[0].ItemArray[3]);
                            if (lng8290 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[22]) / lng8290);
                            }
                        }
                        /*1.11 End*/

                        /*1.12 Begin*/
                        if (dr8285.Length > 0)
                        {
                            lng8285 = Convert.ToInt32(dr8285[0].ItemArray[3]);
                            if (lng8285 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[23]) / lng8285);
                            }
                        }
                        if (dr8292.Length > 0)
                        {
                            lng8292 = Convert.ToInt32(dr8292[0].ItemArray[3]);
                            if (lng8292 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[24]) / lng8292);
                            }
                        }
                        if (dr8294.Length > 0)
                        {
                            lng8294 = Convert.ToInt32(dr8294[0].ItemArray[3]);
                            if (lng8294 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[25]) / lng8294);
                            }
                        }
                        if (dr8296.Length > 0)
                        {
                            lng8296 = Convert.ToInt32(dr8296[0].ItemArray[3]);
                            if (lng8296 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[26]) / lng8296);
                            }
                        }
                        /*1.12 End*/

                        /*1.16 Begin*/
                        if (dr829X.Length > 0)
                        {
                            lng829X = Convert.ToInt32(dr829X[0].ItemArray[3]);
                            if (lng829X != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[30]) / lng829X);
                            }
                        }
                        if (dr8297.Length > 0)
                        {
                            lng8297 = Convert.ToInt32(dr8297[0].ItemArray[3]);
                            if (lng8297 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[31]) / lng8297);
                            }
                        }
                        if (dr8298.Length > 0)
                        {
                            lng8298 = Convert.ToInt32(dr8298[0].ItemArray[3]);
                            if (lng8298 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[32]) / lng8298);
                            }
                        }
                        if (dr8299.Length > 0)
                        {
                            lng8299 = Convert.ToInt32(dr8299[0].ItemArray[3]);
                            if (lng8299 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[33]) / lng8299);
                            }
                        }
                        /*1.16 End*/

                        /*1.13 Begin*/
                        if (dr8240.Length > 0)
                        {
                            lng8240 = Convert.ToInt32(dr8240[0].ItemArray[3]);
                            if (lng8240 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[27]) / lng8240);
                            }
                        }
                        /*1.13 End*/
                        /*1.14 Begin*/
                        if (dr8234.Length > 0)
                        {
                            lng8234 = Convert.ToInt32(dr8234[0].ItemArray[3]);
                            if (lng8234 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[28]) / lng8234);
                            }
                        }
                        /*1.14 End*/

                        if (dr8300.Length > 0)
                        {
                            lng8300 = Convert.ToInt32(dr8300[0].ItemArray[3]);
                            if (lng8300 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[12]) / lng8300);
                            }
                        }
                        /*1.15 Begin*/
                        if (dr8305.Length > 0)
                        {
                            lng8305 = Convert.ToInt32(dr8305[0].ItemArray[3]);
                            if (lng8305 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[29]) / lng8305);
                            }
                        }
                        /*1.15 End*/
                        if (dr8350.Length > 0)
                        {
                            lng8350 = Convert.ToInt32(dr8350[0].ItemArray[3]);
                            if (lng8350 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[13]) / lng8350);
                            }
                        }
                        //1.09 begin
                        if (dr8351.Length > 0)
                        {
                            lng8351 = Convert.ToInt32(dr8351[0].ItemArray[3]);
                            if (lng8351 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[19]) / lng8351);
                            }
                        }
                        if (dr8352.Length > 0)
                        {
                            lng8352 = Convert.ToInt32(dr8352[0].ItemArray[3]);
                            if (lng8352 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[20]) / lng8352);
                            }
                        }
                        if (dr8353.Length > 0)
                        {
                            lng8353 = Convert.ToInt32(dr8353[0].ItemArray[3]);
                            if (lng8353 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[21]) / lng8353);
                            }
                        }
                        //1.09 end

                        if (dr8355.Length > 0)
                        {
                            lng8355 = Convert.ToInt32(dr8355[0].ItemArray[3]);
                            if (lng8355 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[18]) / lng8355);
                            }
                        }

                        if (dr8360.Length > 0)
                        {
                            lng8360 = Convert.ToInt32(dr8360[0].ItemArray[3]);
                            if (lng8360 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[14]) / lng8360);
                            }
                        }
                        if (dr8380.Length > 0)
                        {
                            lng8380 = Convert.ToInt32(dr8380[0].ItemArray[3]);
                            if (lng8380 != 0)
                            {
                                dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[15]) / lng8380);
                            }
                        }


                        //if (dr8230.Length > 0)
                        //{
                        //    lng8230 = Convert.ToInt32(dr8230[0].ItemArray[3]);
                        //    if (lng8230 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[8]) / lng8230);
                        //    }
                        //}
                        //if (dr8250.Length > 0)
                        //{
                        //    lng8250 = Convert.ToInt32(dr8250[0].ItemArray[3]);
                        //    if (lng8250 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[9]) / lng8250);
                        //    }
                        //}

                        //if (dr8260.Length > 0)
                        //{
                        //    lng8260 = Convert.ToInt32(dr8260[0].ItemArray[3]);
                        //    if (lng8260 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[15]) / lng8260);
                        //    }
                        //}
                        //if (dr8270.Length > 0)
                        //{
                        //    lng8270 = Convert.ToInt32(dr8270[0].ItemArray[3]);
                        //    if (lng8270 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[16]) / lng8270);
                        //    }
                        //}

                        //if (dr8280.Length > 0)
                        //{
                        //    lng8280 = Convert.ToInt32(dr8280[0].ItemArray[3]);
                        //    if (lng8280 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[10]) / lng8280);
                        //    }
                        //}

                        ///*1.11 Begin*/
                        //if (dr8290.Length > 0)
                        //{
                        //    lng8290 = Convert.ToInt32(dr8290[0].ItemArray[3]);
                        //    if (lng8290 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[21]) / lng8290);
                        //    }
                        //}
                        ///*1.11 End*/

                        ///*1.12 Begin*/
                        //if (dr8285.Length > 0)
                        //{
                        //    lng8285 = Convert.ToInt32(dr8285[0].ItemArray[3]);
                        //    if (lng8285 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[22]) / lng8285);
                        //    }
                        //}
                        //if (dr8292.Length > 0)
                        //{
                        //    lng8292 = Convert.ToInt32(dr8292[0].ItemArray[3]);
                        //    if (lng8292 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[23]) / lng8292);
                        //    }
                        //}
                        //if (dr8294.Length > 0)
                        //{
                        //    lng8294 = Convert.ToInt32(dr8294[0].ItemArray[3]);
                        //    if (lng8294 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[24]) / lng8294);
                        //    }
                        //}
                        //if (dr8296.Length > 0)
                        //{
                        //    lng8296 = Convert.ToInt32(dr8296[0].ItemArray[3]);
                        //    if (lng8296 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[25]) / lng8296);
                        //    }
                        //}
                        ///*1.12 End*/

                        ///*1.16 Begin*/
                        //if (dr829X.Length > 0)
                        //{
                        //    lng829X = Convert.ToInt32(dr829X[0].ItemArray[3]);
                        //    if (lng829X != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[29]) / lng829X);
                        //    }
                        //}
                        //if (dr8297.Length > 0)
                        //{
                        //    lng8297 = Convert.ToInt32(dr8297[0].ItemArray[3]);
                        //    if (lng8297 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[30]) / lng8297);
                        //    }
                        //}
                        //if (dr8298.Length > 0)
                        //{
                        //    lng8298 = Convert.ToInt32(dr8298[0].ItemArray[3]);
                        //    if (lng8298 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[31]) / lng8298);
                        //    }
                        //}
                        //if (dr8299.Length > 0)
                        //{
                        //    lng8299 = Convert.ToInt32(dr8299[0].ItemArray[3]);
                        //    if (lng8299 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[32]) / lng8299);
                        //    }
                        //}
                        ///*1.16 End*/

                        ///*1.13 Begin*/
                        //if (dr8240.Length > 0)
                        //{
                        //    lng8240 = Convert.ToInt32(dr8240[0].ItemArray[3]);
                        //    if (lng8240 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[26]) / lng8240);
                        //    }
                        //}
                        ///*1.13 End*/
                        ///*1.14 Begin*/
                        //if (dr8234.Length > 0)
                        //{
                        //    lng8234 = Convert.ToInt32(dr8234[0].ItemArray[3]);
                        //    if (lng8234 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[27]) / lng8234);
                        //    }
                        //}
                        ///*1.14 End*/

                        //if (dr8300.Length > 0)
                        //{
                        //    lng8300 = Convert.ToInt32(dr8300[0].ItemArray[3]);
                        //    if (lng8300 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[11]) / lng8300);
                        //    }
                        //}
                        ///*1.15 Begin*/
                        //if (dr8305.Length > 0)
                        //{
                        //    lng8305 = Convert.ToInt32(dr8305[0].ItemArray[3]);
                        //    if (lng8305 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[28]) / lng8305);
                        //    }
                        //}
                        ///*1.15 End*/
                        //if (dr8350.Length > 0)
                        //{
                        //    lng8350 = Convert.ToInt32(dr8350[0].ItemArray[3]);
                        //    if (lng8350 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[12]) / lng8350);
                        //    }
                        //}
                        ////1.09 begin
                        //if (dr8351.Length > 0)
                        //{
                        //    lng8351 = Convert.ToInt32(dr8351[0].ItemArray[3]);
                        //    if (lng8351 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[18]) / lng8351);
                        //    }
                        //}
                        //if (dr8352.Length > 0)
                        //{
                        //    lng8352 = Convert.ToInt32(dr8352[0].ItemArray[3]);
                        //    if (lng8352 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[19]) / lng8352);
                        //    }
                        //}
                        //if (dr8353.Length > 0)
                        //{
                        //    lng8353 = Convert.ToInt32(dr8353[0].ItemArray[3]);
                        //    if (lng8353 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[20]) / lng8353);
                        //    }
                        //}
                        ////1.09 end

                        //if (dr8355.Length > 0)
                        //{
                        //    lng8355 = Convert.ToInt32(dr8355[0].ItemArray[3]);
                        //    if (lng8355 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[17]) / lng8355);
                        //    }
                        //}

                        //if (dr8360.Length > 0)
                        //{
                        //    lng8360 = Convert.ToInt32(dr8360[0].ItemArray[3]);
                        //    if (lng8360 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[13]) / lng8360);
                        //    }
                        //}
                        //if (dr8380.Length > 0)
                        //{
                        //    lng8380 = Convert.ToInt32(dr8380[0].ItemArray[3]);
                        //    if (lng8380 != 0)
                        //    {
                        //        dblDefectRate = dblDefectRate * (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[14]) / lng8380);
                        //    }
                        //}
                        /*-------------------------------------------------------------------------------------*/
                        DataRow drNew = dtMDef.NewRow();
                        drNew.Table.Columns.Add();
                        drNew["ReportDT"] = strReportDT;
                        drNew["PRODUCT_ID"] = strPRODUCTID;
                        drNew["LOTGRADE"] = strGrade;
                        drNew["DEFCODE"] = drDef[DefCnt].ItemArray[3].ToString();
                        drNew["DEFECTDESC"] = drDef[DefCnt].ItemArray[4].ToString();
                        drNew["TTL_CNT"] = Convert.ToInt32(drDef[DefCnt].ItemArray[5]);
                        drNew["S8180"] = Convert.ToInt32(drDef[DefCnt].ItemArray[6]);
                        drNew["S8210"] = Convert.ToInt32(drDef[DefCnt].ItemArray[7]);
                        drNew["S8220"] = Convert.ToInt32(drDef[DefCnt].ItemArray[8]);/*1.17*/
                        drNew["S8230"] = Convert.ToInt32(drDef[DefCnt].ItemArray[9]);
                        drNew["S8250"] = Convert.ToInt32(drDef[DefCnt].ItemArray[10]);
                        drNew["S8280"] = Convert.ToInt32(drDef[DefCnt].ItemArray[11]);
                        drNew["S8300"] = Convert.ToInt32(drDef[DefCnt].ItemArray[12]);
                        drNew["S8350"] = Convert.ToInt32(drDef[DefCnt].ItemArray[13]);
                        drNew["S8360"] = Convert.ToInt32(drDef[DefCnt].ItemArray[14]);
                        drNew["S8380"] = Convert.ToInt32(drDef[DefCnt].ItemArray[15]);
                        drNew["DefectRate"] = Math.Round(1 - dblDefectRate, 4) * 100;
                        drNew["Percentage"] = 0;
                        drNew["ItemNo"] = DefCnt;
                        //1.09 BEGIN
                        drNew["S8351"] = Convert.ToInt32(drDef[DefCnt].ItemArray[19]);
                        drNew["S8352"] = Convert.ToInt32(drDef[DefCnt].ItemArray[20]);
                        drNew["S8353"] = Convert.ToInt32(drDef[DefCnt].ItemArray[21]);
                        //1.09 END 
                        drNew["S8260"] = Convert.ToInt32(drDef[DefCnt].ItemArray[16]);
                        drNew["S8270"] = Convert.ToInt32(drDef[DefCnt].ItemArray[17]);
                        drNew["S8355"] = Convert.ToInt32(drDef[DefCnt].ItemArray[18]);
                        dtMDef.Rows.Add(drNew);
                        /*-------------------------------------------------------------------------------------*/

                        drNew["S8290"] = Convert.ToInt32(drDef[DefCnt].ItemArray[22]); /*1.11*/
                        drNew["S8285"] = Convert.ToInt32(drDef[DefCnt].ItemArray[23]); /*1.12*/
                        drNew["S8292"] = Convert.ToInt32(drDef[DefCnt].ItemArray[24]); /*1.12*/
                        drNew["S8294"] = Convert.ToInt32(drDef[DefCnt].ItemArray[25]); /*1.12*/
                        drNew["S8296"] = Convert.ToInt32(drDef[DefCnt].ItemArray[26]); /*1.12*/
                        drNew["S8240"] = Convert.ToInt32(drDef[DefCnt].ItemArray[27]); /*1.13*/
                        drNew["S8234"] = Convert.ToInt32(drDef[DefCnt].ItemArray[28]); /*1.14*/
                        drNew["S8305"] = Convert.ToInt32(drDef[DefCnt].ItemArray[29]); /*1.15*/

                        drNew["S829X"] = Convert.ToInt32(drDef[DefCnt].ItemArray[30]); /*1.16*/
                        drNew["S8297"] = Convert.ToInt32(drDef[DefCnt].ItemArray[31]); /*1.16*/
                        drNew["S8298"] = Convert.ToInt32(drDef[DefCnt].ItemArray[32]); /*1.16*/
                        drNew["S8299"] = Convert.ToInt32(drDef[DefCnt].ItemArray[33]); /*1.16*/

                        //drNew["S8230"] = Convert.ToInt32(drDef[DefCnt].ItemArray[8]);
                        //drNew["S8250"] = Convert.ToInt32(drDef[DefCnt].ItemArray[9]);
                        //drNew["S8280"] = Convert.ToInt32(drDef[DefCnt].ItemArray[10]);
                        //drNew["S8300"] = Convert.ToInt32(drDef[DefCnt].ItemArray[11]);
                        //drNew["S8350"] = Convert.ToInt32(drDef[DefCnt].ItemArray[12]);
                        //drNew["S8360"] = Convert.ToInt32(drDef[DefCnt].ItemArray[13]);
                        //drNew["S8380"] = Convert.ToInt32(drDef[DefCnt].ItemArray[14]);
                        //drNew["DefectRate"] = Math.Round(1 - dblDefectRate, 4) * 100;
                        //drNew["Percentage"] = 0;
                        //drNew["ItemNo"] = DefCnt;
                        ////1.09 BEGIN
                        //drNew["S8351"] = Convert.ToInt32(drDef[DefCnt].ItemArray[18]);
                        //drNew["S8352"] = Convert.ToInt32(drDef[DefCnt].ItemArray[19]);
                        //drNew["S8353"] = Convert.ToInt32(drDef[DefCnt].ItemArray[20]);
                        ////1.09 END 
                        //drNew["S8260"] = Convert.ToInt32(drDef[DefCnt].ItemArray[15]);
                        //drNew["S8270"] = Convert.ToInt32(drDef[DefCnt].ItemArray[16]);
                        //drNew["S8355"] = Convert.ToInt32(drDef[DefCnt].ItemArray[17]);
                        //dtMDef.Rows.Add(drNew);
                        ///*-------------------------------------------------------------------------------------*/

                        //drNew["S8290"] = Convert.ToInt32(drDef[DefCnt].ItemArray[21]); /*1.11*/
                        //drNew["S8285"] = Convert.ToInt32(drDef[DefCnt].ItemArray[22]); /*1.12*/
                        //drNew["S8292"] = Convert.ToInt32(drDef[DefCnt].ItemArray[23]); /*1.12*/
                        //drNew["S8294"] = Convert.ToInt32(drDef[DefCnt].ItemArray[24]); /*1.12*/
                        //drNew["S8296"] = Convert.ToInt32(drDef[DefCnt].ItemArray[25]); /*1.12*/
                        //drNew["S8240"] = Convert.ToInt32(drDef[DefCnt].ItemArray[26]); /*1.13*/
                        //drNew["S8234"] = Convert.ToInt32(drDef[DefCnt].ItemArray[27]); /*1.14*/
                        //drNew["S8305"] = Convert.ToInt32(drDef[DefCnt].ItemArray[28]); /*1.15*/

                        //drNew["S829X"] = Convert.ToInt32(drDef[DefCnt].ItemArray[29]); /*1.16*/
                        //drNew["S8297"] = Convert.ToInt32(drDef[DefCnt].ItemArray[30]); /*1.16*/
                        //drNew["S8298"] = Convert.ToInt32(drDef[DefCnt].ItemArray[31]); /*1.16*/
                        //drNew["S8299"] = Convert.ToInt32(drDef[DefCnt].ItemArray[32]); /*1.16*/

                    }
                }
                //}
            }
            Session["dtMDef"] = null;
            Session["dtMDef"] = dtMDef;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryDataGroupByFPY()
    {
        DataTable dtMFPY = (DataTable)Session["dtMFPY"];
        dtMFPY.DefaultView.RowFilter = "";
        DataRow[] drChart = dtMFPY.Select("ReportDT='TOTAL' AND OPERATION_ID='FPY'", "PRODUCT_ID");
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
            ChartAll.Titles[0].Text = tbFABName.Text + " Module First Pass Yield";

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
        DataTable dtMFPY = (DataTable)Session["dtMFPY"];
        dtMFPY.DefaultView.RowFilter = "";
        DataRow[] drChart = dtMFPY.Select("ReportDT='TOTAL' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "'", "ItemNo");
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
            ChartProdID.Series["Chip_Qty"].ValueMemberX = "OPERATION_ID";
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
                    ChartProdID.Series[li.Value + "_Yield"].ValueMemberX = "OPERATION_ID";
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
            ChartProdID.Titles[0].Text = ddlByProdID.SelectedValue + " FPY By Station Chart";

            ChartProdID.ChartAreas["Default"].BackColor = System.Drawing.Color.FromArgb(196, 189, 219);
            ChartProdID.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
            ChartProdID.ChartAreas["Default"].AxisX.Interval = 1;
            ChartProdID.ChartAreas["Default"].AxisX.Title = "OPERATION_ID";
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
        DataTable dtMFPY = (DataTable)Session["dtMFPY"];
        dtMFPY.DefaultView.RowFilter = "";
        DataRow[] drChart = dtMFPY.Select("ReportDT<>'TOTAL' AND OPERATION_ID='FPY' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "'", "ReportDT");
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
            ChartDT.Titles[0].Text = ddlByProdID.SelectedValue + " FPY By Time Chart";

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
        DataTable dtMDef = (DataTable)Session["dtMDef"];
        DataRow[] drChart = dtMDef.Select("ReportDT='Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = 'S' ", "ReportDT,DefectRate desc");
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
            /*V1.01 Begin*/
            /*
ChartSDefectCode.Titles[0].Text = ddlByProdID.SelectedValue + " S Grade DefectCode Chart";*/
            //if (RadioButtonList1.Items.FindByText("DefectGroup").Selected)
            //{
            //    ChartSDefectCode.Titles[0].Text = ddlByProdID.SelectedValue + " S Grade DefectGroup Chart";
            //}
            //else
            //{
            ChartSDefectCode.Titles[0].Text = ddlByProdID.SelectedValue + " S Grade DefectCode Chart";
            //}
            /*V1.01 End*/


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
        DataTable dtMDef = (DataTable)Session["dtMDef"];
        DataRow[] drChart = dtMDef.Select("ReportDT<>'Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = 'S' AND DEFCODE= '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "' ", "ReportDT,DefectRate desc");
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
            /*V1.01 Begin*/
            /*
            ChartSDefectDistr.Titles[0].Text ="S Grade DefectCode[" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "] By Time Chart";*/
            //if (RadioButtonList1.Items.FindByText("DefectGroup").Selected)
            //{
            //    ChartSDefectDistr.Titles[0].Text = "S Grade DefectGroup[" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "] By Time Chart";
            //}
            //else
            //{
            ChartSDefectDistr.Titles[0].Text = "S Grade DefectCode[" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "] By Time Chart";
            //}
            /*V1.01 End*/
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
        DataTable dtMFPY = (DataTable)Session["dtMFPY"];
        DataTable dtMDef = (DataTable)Session["dtMDef"];
        dtMFPY.DefaultView.RowFilter = "";
        dtMFPY.DefaultView.RowFilter = "(ReportDT='Total' AND OPERATION_ID='FPY')";
        dtMFPY.DefaultView.Sort = "ReportDT,ItemNo";
        GridView1.DataSource = dtMFPY;
        GridView1.DataBind();

        dtMFPY.DefaultView.RowFilter = "";
        dtMFPY.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' )";
        dtMFPY.DefaultView.Sort = "ItemNo";
        GridView2.DataSource = dtMFPY;
        GridView2.DataBind();
        dtMFPY.DefaultView.RowFilter = "";

        dtMFPY.DefaultView.RowFilter = "";
        dtMFPY.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND OPERATION_ID='FPY'  )";
        dtMFPY.DefaultView.Sort = "ReportDT,ItemNo";
        GridView3.DataSource = dtMFPY;
        GridView3.DataBind();
        dtMFPY.DefaultView.RowFilter = "";

        dtMDef.DefaultView.RowFilter = "";
        dtMDef.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND LOTGRADE='S')";
        dtMDef.DefaultView.Sort = "ReportDT,DefectRate DESC";
        GridView4.DataSource = dtMDef;
        GridView4.DataBind();

        dtMDef.DefaultView.RowFilter = "";
        dtMDef.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND DEFCODE = '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].Trim().ToString() + "' AND LOTGRADE='S')";
        dtMDef.DefaultView.Sort = "ReportDT,DefectRate DESC";
        GridView5.DataSource = dtMDef;
        GridView5.DataBind();
        dtMDef.DefaultView.RowFilter = "";
    }
    protected void ddlByProdID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtMFPY = (DataTable)Session["dtMFPY"];
        AppClass Apc = new AppClass();
        ClearChartBound();
        int intSecCnt = 0;
        lblTime01.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtMFPY.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtMFPY.Select("ReportDT<>'Total'");
        foreach (DataRow drDel in drsDel)
        {
            dtMFPY.Rows.Remove(drDel);
        }
        dtMFPY.DefaultView.RowFilter = "";
        Session.Remove("dtMFPY");
        Session["dtMFPY"] = dtMFPY;

        QueryFPYDataBound("Days", ddlByProdID.SelectedValue);
        ddlSDefList.Items.Clear();
        CreateDefSessionDataTable();
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "S");//20121226
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "S");
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlByProdID", "ProdID:" + ddlByProdID.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime01.Text = "查詢耗時：" + intSecCnt.ToString() + "s";

        lblTime02.Text = "";
    }
    protected void cblLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cblLevel.Items.Count >= 4)
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
        DataTable dtMDef = (DataTable)Session["dtMDef"];
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        lblTime02.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtMDef.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtMDef.Select("ReportDT<>'Total' AND LOTGRADE = 'S' ");
        foreach (DataRow drDel in drsDel)
        {
            dtMDef.Rows.Remove(drDel);
        }
        dtMDef.DefaultView.RowFilter = "";
        Session["dtMDef"] = null;
        Session["dtMDef"] = dtMDef;

        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "S");
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlSDefList", "S Grade DefectCode:" + ddlSDefList.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime02.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
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
    /*V1.01 Begin*/
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSDefList.Items.Clear();
        CreateDefSessionDataTable();
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "S");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "S");
        GridViewDataBound();
        ChartDataBound();
    }
    /*V1.01 End*/
    /*V1.10 Add Start*/
    /*V2.00 增加TN FAB3和TN FAB5選項*/
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
                //V2.00   begin
            case 3:
                PGCraft.Visible = true;
                cblGCraft.Items.Clear();
                cblSoFab.Items.Clear();
                strSQL = "SELECT DISTINCT SOURCEFAB FROM MSOFABMLCDM WHERE SourceFAB='TN FAB3'";
                dsgetSoFab = dbconn.GetDataSetBySql(strSQL);
                for (int i = 0; i < dsgetSoFab.Tables[0].Rows.Count; i++)
                {
                    cblSoFab.Items.Add(dsgetSoFab.Tables[0].Rows[i][0].ToString());
                }
                break;
            case 4:
                PGCraft.Visible = true;
                cblGCraft.Items.Clear();
                cblSoFab.Items.Clear();
                strSQL = "SELECT DISTINCT SOURCEFAB FROM MSOFABMLCDM WHERE SourceFAB='TN FAB5'";
                dsgetSoFab = dbconn.GetDataSetBySql(strSQL);
                for (int i = 0; i < dsgetSoFab.Tables[0].Rows.Count; i++)
                {
                    cblSoFab.Items.Add(dsgetSoFab.Tables[0].Rows[i][0].ToString());
                }
                break;
                //V2.00  end
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
    /*V1.10 Add End*/
    protected void ChkMerge_CheckedChanged(object sender, EventArgs e)
    {
        DataBoundProdID();
    }
}
