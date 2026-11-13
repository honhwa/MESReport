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
 * 2013-04-15  Innolux/ssc      V1.01       8180歸類到BEOL了，故MODULE要取消M1(8180) yield
 * 2013-05-03  Innolux/ssc      V1.02       Add Garmin Station(8GCO,8GFP,8GL2,8GSL,8GBA,8GSD,8GAG,8GL3,8GTP,8GTT,8GOQ)
 * 2013-05-15  Innolux/QingQing V1.03       Add Lamination 外觀 Station(81LW)
 * 2013-06-04  Innolux/Ron      V1.07       Add Station 8260 8270 8355
 * 2014-05-12  Innolux/Power    V1.08       MARK 8180
 * 2014-07-31  Innolux/vivian   V1.09       Add 8351,8352,8353
 * 2014-08-14  Innolux/Runy     V1.10       Add LCD Craft篩選條件 & 添加 Summary功能
 * 2015-10-08  Innolux/durant   V1.11       Add Lamination 站點8290
 * 2019-01-03  Innolux/Jeff     V1.12       Add Lamination 站點8285,8292,8294,8296,
 * 2019-08-22  Innolux/Jeff     V1.13       Add TOD站點8240,
 * 2020-05-05  HuangMenghao     V1.14       Add FOG 站點8234
 * 2020-05-28  HuangMenghao     V1.15       Add VIA 站點8305
 * 2021-01-29  Innolux/Jeff     V1.16       Add 829X,8297,8298,8299
 * 2021-11-07  Innolux/Jeff     V1.17       Add 8220
*/
public partial class YMSReport_ModuleAllLightOn : System.Web.UI.Page
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

            lblTime04.Text = "";

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

        if (Apc.QuerySubject("ModuleAllLightOn", "ModuleAllLightOn", out strTitle, out strCon, out strDateTime) == true)
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
        //ddlStation.Items.Add("M1(8180)"); /*1.08*/
        ddlStation.Items.Add("M2-COG(8210)");
        ddlStation.Items.Add("M2-COF(8220)");/*1.17*/
        ddlStation.Items.Add("M2-FPC(8230)");
        ddlStation.Items.Add("FOG(8234)");/*1.14*/
        ddlStation.Items.Add("TOD(8240)");/*1.13*/
        ddlStation.Items.Add("M2-FOB(8250)");
        ddlStation.Items.Add("OCA(8260)");
        ddlStation.Items.Add("HalfModuleTest(8270)");
        ddlStation.Items.Add("M2(8280)");
        ddlStation.Items.Add("M2.5(8290)");/*1.11*/
        ddlStation.Items.Add("M2.5(8285)");/*1.12*/
        ddlStation.Items.Add("M2.5(8292)");/*1.12*/
        ddlStation.Items.Add("M2.5(8294)");/*1.12*/
        ddlStation.Items.Add("M2.5(8296)");/*1.12*/
        ddlStation.Items.Add("M2.5(829X)");/*1.16*/
        ddlStation.Items.Add("M2.5(8297)");/*1.16*/
        ddlStation.Items.Add("M2.5(8298)");/*1.16*/
        ddlStation.Items.Add("M2.5(8299)");/*1.16*/
        ddlStation.Items.Add("AssemblyReceived(8300)");
        ddlStation.Items.Add("VIA(8305)");/*1.15*/
        ddlStation.Items.Add("Assembly(8350)");
        //1.09 begin
        ddlStation.Items.Add("Burn-IN(8351)");
        ddlStation.Items.Add("Baking(8352)");
        ddlStation.Items.Add("Module test(8353)");
        //1.09 end
        ddlStation.Items.Add("AssemblyResorting(8355)");
        ddlStation.Items.Add("OQC(8360)");
        ddlStation.Items.Add("ModulePacking1(8380)");
    }
    protected void PageTitleBound()
    {
        Page.Title = tbFABName.Text + " Module All Light On Yield";
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

            lblTime04.Text = "";

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

            Session["dtMYield"] = null;
            Session["dtMYDef"] = null;

            IBGBYData.Enabled = false;
            cblLevel.Enabled = false;

            ddlByProdID.Items.Clear();
            ddlSDefList.Items.Clear();


            ddlShiftID.SelectedIndex = 0;

            ClearChartBound();

            switch (ddlStation.SelectedItem.Text.ToString())
            {

                case "M1(8180)": tbOPERATIONID.Text = "8180"; tbType.Text = "M1(8180)"; break;  /*V1.01*/
                case "M2-COG(8210)": tbOPERATIONID.Text = "8210"; tbType.Text = "M2-COG(8210)"; break;
                case "M2-COF(8220)": tbOPERATIONID.Text = "8220"; tbType.Text = "M2-COF(8220)"; break;/*1.17*/
                case "M2-FPC(8230)": tbOPERATIONID.Text = "8230"; tbType.Text = "M2-FPC(8230)"; break;
                case "TOD(8234)": tbOPERATIONID.Text = "8234"; tbType.Text = "FOG(8234)"; break;/*1.14*/
                case "TOD(8240)": tbOPERATIONID.Text = "8240"; tbType.Text = "TOD(8240)"; break;/*1.13*/
                case "M2-FOB(8250)": tbOPERATIONID.Text = "8250"; tbType.Text = "M2-FOB(8250)"; break;
                case "OCA(8260)": tbOPERATIONID.Text = "8260"; tbType.Text = "OCA(8260)"; break;
                case "HalfModuleTest(8270)": tbOPERATIONID.Text = "8270"; tbType.Text = "HalfModuleTest(8270)"; break;
                case "M2(8280)": tbOPERATIONID.Text = "8280"; tbType.Text = "M2(8280)"; break;
                case "M2.5(8290)": tbOPERATIONID.Text = "8290"; tbType.Text = "M2.5(8290)"; break;/*1.11*/
                case "M2.5(8285)": tbOPERATIONID.Text = "8285"; tbType.Text = "M2.5(8285)"; break;/*1.12*/
                case "M2.5(8292)": tbOPERATIONID.Text = "8292"; tbType.Text = "M2.5(8292)"; break;/*1.12*/
                case "M2.5(8294)": tbOPERATIONID.Text = "8294"; tbType.Text = "M2.5(8294)"; break;/*1.12*/
                case "M2.5(8296)": tbOPERATIONID.Text = "8296"; tbType.Text = "M2.5(8296)"; break;/*1.12*/
                case "M2.5(829X)": tbOPERATIONID.Text = "829X"; tbType.Text = "M2.5(829X)"; break;/*1.16*/
                case "M2.5(8297)": tbOPERATIONID.Text = "8297"; tbType.Text = "M2.5(8297)"; break;/*1.16*/
                case "M2.5(8298)": tbOPERATIONID.Text = "8298"; tbType.Text = "M2.5(8298)"; break;/*1.16*/
                case "M2.5(8299)": tbOPERATIONID.Text = "8299"; tbType.Text = "M2.5(8299)"; break;/*1.16*/
                case "AssemblyReceived(8300)": tbOPERATIONID.Text = "8300"; tbType.Text = "AssemblyReceived(8300)"; break;
                case "VIA(8305)": tbOPERATIONID.Text = "8305"; tbType.Text = "VIA(8305)"; break;/*1.15*/
                case "Assembly(8350)": tbOPERATIONID.Text = "8350"; tbType.Text = "Assembly(8350)"; break;
                //1.09 begin
                case "Burn-IN(8351)": tbOPERATIONID.Text = "8351"; tbType.Text = "Burn-IN(8351)"; break;
                case "Baking(8352)": tbOPERATIONID.Text = "8352"; tbType.Text = "Baking(8352)"; break;
                case "Module test(8353)": tbOPERATIONID.Text = "8353"; tbType.Text = "Module test(8353)"; break;
                //1.09 end 
                case "AssemblyResorting(8355)": tbOPERATIONID.Text = "8355"; tbType.Text = "AssemblyResorting(8355)"; break;
                case "OQC(8360)": tbOPERATIONID.Text = "8360"; tbType.Text = "OQC(8360)"; break;
                case "ModulePacking1(8380)": tbOPERATIONID.Text = "8380"; tbType.Text = "ModulePacking1(8380)"; break;
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
            strSql.Append(" SELECT DISTINCT SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID ");
            strSql.Append(" FROM YIELD_TBL_LIGHTON_PRODUCT WITH(NOLOCK) ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND FAB_ID = '" + tbFABID.Text + "' ");
            strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");
            strSql.Append(tbSqlWhere.Text);
            strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ")");
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
        strSql = "SELECT SUBLINEID FROM MSLM WHERE FABID = '" + tbFABID.Text + "' AND WORKCTR IN('8180-Mark','8210','8220','8230','8234','8240','8250','8280','8290','8285','8292','8294','8296','829X','8297','8298','8299','8300','8305','8350','8360','8380') ORDER BY WORKCTR,SUBLINEID";   /*V1.00*/ /*V1.01*//*V1.02*//*V1.03*//*1.08*//*1.11*//*1.12*//*1.13 8240*//*1.14 8234*//*1.15 8305*//*1.16 829X,8297,8298,8299*/
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

        Session["dtMYield"] = null;
        Session["dtMYDef"] = null;

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
        lblTime04.Text = "";
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
        DataTable dtMYield = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtMYield.Columns.Add(new DataColumn("ReportDT", typeof(string)));			            /*0 */
        dtMYield.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			        /*1 */
        dtMYield.Columns.Add(new DataColumn("OPERATION_ID", typeof(string)));                 /*2 */
        dtMYield.Columns.Add(new DataColumn("Chip_Qty", typeof(long)));                       /*3 */
        dtMYield.Columns.Add(new DataColumn("A_Yield", typeof(double)));                      /*4 */
        dtMYield.Columns.Add(new DataColumn("S_Yield", typeof(double)));                      /*5 */
        dtMYield.Columns.Add(new DataColumn("ItemNo", typeof(int)));                          /*6*/
        dtMYield.Columns.Add(new DataColumn("SHIFTID", typeof(string)));			        /*7 */
        dtMYield.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] keys = new DataColumn[4];
        keys[0] = dtMYield.Columns["ReportDT"];
        keys[1] = dtMYield.Columns["PRODUCT_ID"];
        keys[2] = dtMYield.Columns["OPERATION_ID"];
        keys[3] = dtMYield.Columns["SHIFTID"];
        dtMYield.PrimaryKey = keys;
        /*--------------------------------------------------------------------------------------*/
        Session["dtMYield"] = null;
        Session["dtMYield"] = dtMYield;
    }
    protected void CreateDefSessionDataTable()
    {
        /*-------------------------------------------------------------------------------------*/
        DataTable dtMYDef = new DataTable();
        //--創建數據集，將查詢的資料做邏輯處理后填寫資料集中供UI顯示
        dtMYDef.Columns.Add(new DataColumn("ReportDT", typeof(string)));			                /*0 */
        dtMYDef.Columns.Add(new DataColumn("PRODUCT_ID", typeof(string)));			            /*1 */
        dtMYDef.Columns.Add(new DataColumn("LOTGRADE", typeof(string)));                          /*2 */
        dtMYDef.Columns.Add(new DataColumn("DEFCODE", typeof(string)));                           /*3 */
        dtMYDef.Columns.Add(new DataColumn("DEFECTDESC", typeof(string)));                        /*4 */
        dtMYDef.Columns.Add(new DataColumn("TTL_CNT", typeof(long)));                             /*5 */
        dtMYDef.Columns.Add(new DataColumn("DefectRate", typeof(double)));                        /*6 */
        dtMYDef.Columns.Add(new DataColumn("Percentage", typeof(double)));                        /*7 */
        dtMYDef.Columns.Add(new DataColumn("ItemNo", typeof(int)));                               /*8 */
        dtMYDef.Clear();
        /*對數據集創建Key，供後續Rows.Find()使用*/
        DataColumn[] DFkeys = new DataColumn[4];
        DFkeys[0] = dtMYDef.Columns["ReportDT"];
        DFkeys[1] = dtMYDef.Columns["PRODUCT_ID"];
        DFkeys[2] = dtMYDef.Columns["LOTGRADE"];
        DFkeys[3] = dtMYDef.Columns["DEFCODE"];
        dtMYDef.PrimaryKey = DFkeys;
        /*-------------------------------------------------------------------------------------*/
        Session["dtMYDef"] = null;
        Session["dtMYDef"] = dtMYDef;
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

        lblTime04.Text = "";

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

        Session["dtMYield"] = null;
        Session["dtMYDef"] = null;

        IBGBYData.Enabled = false;
        cblLevel.Enabled = false;

        ddlByProdID.Items.Clear();
        ddlSDefList.Items.Clear();


        ddlShiftID.SelectedIndex = 0;

        ClearChartBound();

        switch (ddlStation.SelectedItem.Text.ToString())
        {

            case "M1(8180)": tbOPERATIONID.Text = "8180"; tbType.Text = "M1(8180)"; break;  /*V1.01*/
            case "M2-COG(8210)": tbOPERATIONID.Text = "8210"; tbType.Text = "M2-COG(8210)"; break;
            case "M2-COF(8220)": tbOPERATIONID.Text = "8220"; tbType.Text = "M2-COF(8220)"; break;/*1.17*/
            case "M2-FPC(8230)": tbOPERATIONID.Text = "8230"; tbType.Text = "M2-FPC(8230)"; break;
            case "FOG(8234)": tbOPERATIONID.Text = "8234"; tbType.Text = "FOG(8234)"; break;/*1.14*/
            case "TOD(8240)": tbOPERATIONID.Text = "8240"; tbType.Text = "TOD(8240)"; break;/*1.13*/
            case "M2-FOB(8250)": tbOPERATIONID.Text = "8250"; tbType.Text = "M2-FOB(8250)"; break;
            case "OCA(8260)": tbOPERATIONID.Text = "8260"; tbType.Text = "OCA(8260)"; break;
            case "HalfModuleTest(8270)": tbOPERATIONID.Text = "8270"; tbType.Text = "HalfModuleTest(8270)"; break;
            case "M2(8280)": tbOPERATIONID.Text = "8280"; tbType.Text = "M2(8280)"; break;
            case "M2.5(8290)": tbOPERATIONID.Text = "8290"; tbType.Text = "M2.5(8290)"; break;/*1.11*/
            case "M2.5(8285)": tbOPERATIONID.Text = "8285"; tbType.Text = "M2.5(8285)"; break;/*1.12*/
            case "M2.5(8292)": tbOPERATIONID.Text = "8292"; tbType.Text = "M2.5(8292)"; break;/*1.12*/
            case "M2.5(8294)": tbOPERATIONID.Text = "8294"; tbType.Text = "M2.5(8294)"; break;/*1.12*/
            case "M2.5(8296)": tbOPERATIONID.Text = "8296"; tbType.Text = "M2.5(8296)"; break;/*1.12*/
            case "M2.5(829X)": tbOPERATIONID.Text = "829X"; tbType.Text = "M2.5(829X)"; break;/*1.16*/
            case "M2.5(8297)": tbOPERATIONID.Text = "8297"; tbType.Text = "M2.5(8297)"; break;/*1.16*/
            case "M2.5(8298)": tbOPERATIONID.Text = "8298"; tbType.Text = "M2.5(8298)"; break;/*1.16*/
            case "M2.5(8299)": tbOPERATIONID.Text = "8299"; tbType.Text = "M2.5(8299)"; break;/*1.16*/
            case "AssemblyReceived(8300)": tbOPERATIONID.Text = "8300"; tbType.Text = "AssemblyReceived(8300)"; break;
            case "VIA(8305)": tbOPERATIONID.Text = "8305"; tbType.Text = "VIA(8305)"; break;/*1.15*/
            case "Assembly(8350)": tbOPERATIONID.Text = "8350"; tbType.Text = "Assembly(8350)"; break;
            //1.09 begin
            case "Burn-IN(8351)": tbOPERATIONID.Text = "8351"; tbType.Text = "Burn-IN(8351)"; break;
            case "Baking(8352)": tbOPERATIONID.Text = "8352"; tbType.Text = "Baking(8352)"; break;
            case "Module test(8353)": tbOPERATIONID.Text = "8353"; tbType.Text = "Module test(8353)"; break;
            //1.09 end 
            case "AssemblyResorting(8355)": tbOPERATIONID.Text = "8355"; tbType.Text = "AssemblyResorting(8355)"; break;
            case "OQC(8360)": tbOPERATIONID.Text = "8360"; tbType.Text = "OQC(8360)"; break;
            case "ModulePacking1(8380)": tbOPERATIONID.Text = "8380"; tbType.Text = "ModulePacking1(8380)"; break;
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
        /*-------------------------------------------------------------------------------------*/
        QueryFPYDataBound("TOTAL", "ALL", "TOTAL");
        QueryFPYDataBound("TOTAL", ddlByProdID.SelectedValue, "SHIFTID");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "TOTAL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "SHIFTID");
        /*V1.10 Add Start*/
        if (ChkMerge.Checked)
        {
            QueryFPYDataBound("TOTALPD", "ALL", "TOTAL");
            QueryFPYDataBound("TOTALPD", "ALL", "SHIFTID");
        }
        /*V1.10 Add End*/
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", "S", "");

        GridViewDataBound();
        ChartDataBound();
        DataTable dtTmp = (DataTable)Session["dtMYield"];
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
    }
    protected void QueryFPYDataBound(string strMode, string strTmpProdID, string strTmpShiftID)
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
                    strSql.Append(" AND OPERATION_ID = '" + tbOPERATIONID.Text + "' ");
                    strSql.Append(tbSqlWhere.Text);
                    strSql.Append(" ORDER BY SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ")");
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
            }/*V1.10 Add*/
            /*-------------------------------------------------------------------------------------*/

            strSql.Remove(0, strSql.Length);
            strSql.Append(" SELECT T1.REPORTDT");                /*0 */
            strSql.Append(" ,T1.PRODUCT_ID");                /*1 */
            strSql.Append(" ,T1.OPERATION_ID");                /*2 */
            strSql.Append(" ,T1.T_CNT");                /*3 */
            strSql.Append(" ,T1.A_CNT");                /*4 */
            strSql.Append(" ,T1.S_CNT");                /*6 */
            strSql.Append(" ,T1.SHIFT_ID");                     /*25 */
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
                /*V1.10 Add End*/
                strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
            }/*V1.10 Add*/
            strSql.Append(" SUM(ISNULL(T_CNT,0)) AS T_CNT,");
            strSql.Append(" SUM(ISNULL(A_CNT,0)) AS A_CNT,");
            strSql.Append(" SUM(ISNULL(S_CNT,0)) AS S_CNT");
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
            if (strMode == "Days" && !ChkMerge.Checked)//V1.10 Add !ChkMerge.Checked
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
            /*V1.10 Add Start*/
            if (ChkMerge.Checked && strMode == "Days")
            {
                strSql.Append(" GROUP BY OPERATION_ID ");
            }
            else if (strMode == "TOTALPD") /*1.08*/
            {
                strSql.Append(" GROUP BY OPERATION_ID ");
            }
            else
            {
                /*V1.10 Add End*/
                strSql.Append(" GROUP BY OPERATION_ID, SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN CPRODUCT_ID ELSE PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") ");
            }/*V1.10 Add*/
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
            /*V1.10 Add Start*/
            if (ChkMerge.Checked && strMode == "Days")
            {
                strTmpProdID = "ALL";
            }
            /*V1.10 Add End*/
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
            /*V1.10 Add Start*/
            if (ChkMerge.Checked)
            {
                strSql.Append(" 'ALL' AS PRODUCT_ID,");
            }
            else
            {
                /*V1.10 Add End*/
                strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,1," + ddlPdLen.SelectedValue + ") AS PRODUCT_ID,");
            }/*V1.10 Add*/
            //if (strGradeID != "")/*20121227*/
            //{
            //    strSql.Append(" LOT_GRADE, ");
            //}
            //if (strGradeID == "ALL")
            //{
            //    strSql.Append(" 'R+S' AS LOT_GRADE, ");
            //}
            //else if (strGradeID != "")
            //{
            strSql.Append(" LOT_GRADE, ");
            //}
            //if (strLevelID != "")
            //{
            //    if (strLevelID == "ALL")
            //    {
            //        strSql.Append(" 'ALL' AS LOT_GRADE, ");
            //    }
            //    else
            //    {
            //        strSql.Append(" LOT_LEVEL AS LOT_GRADE, ");
            //    }
            //}
            strSql.Append(" DEF_CODE, ");
            strSql.Append(" DEFECTDESC,  ");
            strSql.Append(" SUM(DEF_CNT) AS TTL_CNT ");
            strSql.Append(" FROM NJ_TBL_LIGHTON_DEFECT A WITH(NOLOCK),MDCM B WITH(NOLOCK) ");
            strSql.Append(tbSqlDate.Text);
            strSql.Append(" AND A.FAB_ID = '" + tbFABID.Text + "' ");
            strSql.Append(" AND A.OPERATION_ID ='" + tbOPERATIONID.Text + "' ");
            /*V1.10 Add start*/
            if (ChkMerge.Checked)
            {
                if (strTmpProdID != "'='" && strTmpProdID != "ALL")
                {
                    if (tbCPFlag.Text == "C")
                    {
                        strSql.Append(" AND SUBSTRING(CPRODUCT_ID,1," + ddlPdLen.SelectedValue + ")  in (" + strTmpProdID + ") ");
                    }
                    else
                    {
                        strSql.Append(" AND SUBSTRING(PRODUCT_ID,1," + ddlPdLen.SelectedValue + ")  in(" + strTmpProdID + ") ");
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
            if (strTmpShiftID != "TOTAL")
            {
                strSql.Append(" AND SHIFT_ID LIKE '%" + strTmpShiftID + "'");
            }
            //if (strGradeID == "ALL")
            //{
            //    strSql.Append(" AND A.LOT_GRADE = 'S'  ");
            //}
            //else 
            //{
            //    if (strGradeID != "")
            //    {
            strSql.Append(" AND A.LOT_GRADE = '" + strGradeID + "'");
            //    }
            //}

            //if (strLevelID != "" && strLevelID != "ALL")
            //{
            //    strSql.Append(" AND A.LOT_LEVEL = '" + strLevelID + "'");
            //}
            strSql.Append(tbSqlWhere.Text);
            strSql.Append(" AND A.FAB_ID=B.FABID");
            strSql.Append(" AND A.DEF_CODE=B.DEFECTCD");
            strSql.Append(" GROUP BY ");
            if (strMode == "Days")
            {
                strSql.Append(" CASE WHEN '" + tbDateType.Text + "'='D' THEN CONVERT(VARCHAR(10),ReportDate,120) WHEN '" + tbDateType.Text + "'='W' THEN 'W'+RIGHT('0'+CAST(ReportWeek AS VARCHAR(2)),2) WHEN '" + tbDateType.Text + "'='M' THEN 'M'+RIGHT('0'+CAST(ReportMonth AS VARCHAR(2)),2) ELSE '' END, ");
            }
            if (!ChkMerge.Checked)/*V1.10 Add*/
            {
                strSql.Append(" SUBSTRING(CASE WHEN '" + tbCPFlag.Text + "' = 'C' THEN A.CPRODUCT_ID ELSE A.PRODUCT_ID END,1," + ddlPdLen.SelectedValue + "), ");
            }
            //if (strGradeID != "") /*20121227*/
            //{
            //    strSql.Append(" LOT_GRADE,DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            //if (strGradeID == "ALL")
            //{
            //    strSql.Append(" DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            //else if (strGradeID != "")
            //{
            strSql.Append("LOT_GRADE, DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //}
            //if (strLevelID != "")
            //{
            //    if (strLevelID == "ALL")
            //    {
            //        strSql.Append(" DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //    }
            //    else
            //    {
            //        strSql.Append(" LOT_LEVEL,DEF_CODE,DEFECTDESC ORDER BY SUM(DEF_CNT) DESC");
            //    }
            //}
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
            DataTable dtMYield = (DataTable)Session["dtMYield"];
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
                            lngChipQty = lngMQty;
                            dblAYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[4])) / lngMQty, 4) * 100;
                            dblSYield = (lngMQty == 0) ? 0 : Math.Round((Convert.ToDouble(drType[0].ItemArray[5])) / lngMQty, 4) * 100;
                            strSHIFTID = drType[0].ItemArray[6].ToString();

                            DataRow drFPY = dtMYield.NewRow();
                            drFPY["ReportDT"] = strReportDT;
                            drFPY["PRODUCT_ID"] = strPRODUCTID;
                            drFPY["OPERATION_ID"] = tbType.Text;
                            drFPY["Chip_Qty"] = lngChipQty;
                            drFPY["A_Yield"] = dblAYield;
                            drFPY["S_Yield"] = dblSYield;
                            drFPY["ItemNo"] = k;
                            drFPY["SHIFTID"] = strSHIFTID;
                            dtMYield.Rows.Add(drFPY);
                            /*-------------------------------------------------------------------------------------*/
                        }
                    }
                }
            }
            Session["dtMYield"] = null;
            Session["dtMYield"] = dtMYield;
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
            DataTable dtMYDef = (DataTable)Session["dtMYDef"];
            DataTable dtMYield = (DataTable)Session["dtMYield"];
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
                    //if (strGradeID != "ALL")
                    //{
                    //    if (strGradeID == "" && strLevelID != "")
                    //    {
                    //        strGrade = strLevelID;
                    //    }
                    //    else
                    //    {
                    strGrade = strGradeID;
                    //    }
                    //    i = 2;
                    //}
                    //else
                    //{
                    //    //switch (i)
                    //    //{
                    //    //    case 0: strGrade = "S"; break;
                    //    //    case 1: strGrade = "R"; break;
                    //    //    default:
                    //    //        return;
                    //    //}  /*20121227*/
                    //    strGrade = "R+S";
                    //    i = 2;
                    //}
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
                            long lngDefectCnt = 0;
                            double dblDefectRate = 1;

                            DataRow[] drType = dtMYield.Select(" (REPORTDT='" + strReportDT + "' AND PRODUCT_ID='" + strPRODUCTID + "' AND OPERATION_ID='" + tbType.Text + "' AND SHIFTID LIKE '%" + strTmpShiftID + "')");
                            if (drType.Length > 0)
                            {
                                lngDefectCnt = Convert.ToInt32(drType[0].ItemArray[3]);
                                if (lngDefectCnt != 0)
                                {
                                    dblDefectRate = (1 - Convert.ToDouble(drDef[DefCnt].ItemArray[5]) / lngDefectCnt);
                                }
                            }
                            /*-------------------------------------------------------------------------------------*/
                            DataRow drNew = dtMYDef.NewRow();
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
                            dtMYDef.Rows.Add(drNew);
                            /*-------------------------------------------------------------------------------------*/
                        }
                    }
                }
            }
            Session["dtMYDef"] = null;
            Session["dtMYDef"] = dtMYDef;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void QueryDataGroupByFPY()
    {
        DataTable dtMYield = (DataTable)Session["dtMYield"];
        dtMYield.DefaultView.RowFilter = "";
        DataRow[] drChart = dtMYield.Select("ReportDT='TOTAL' AND OPERATION_ID='" + tbType.Text + "' AND SHIFTID='TOTAL' ", "PRODUCT_ID");
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
            ChartAll.Titles[0].Text = tbFABName.Text + " Module All Light On Yield";

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
        DataTable dtMYield = (DataTable)Session["dtMYield"];
        dtMYield.DefaultView.RowFilter = "";
        DataRow[] drChart = dtMYield.Select("ReportDT='TOTAL' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND SHIFTID <> 'TOTAL' ", "ItemNo");
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
        DataTable dtMYield = (DataTable)Session["dtMYield"];
        dtMYield.DefaultView.RowFilter = "";
        DataRow[] drChart = dtMYield.Select("ReportDT<>'TOTAL' AND OPERATION_ID='" + tbType.Text + "' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "'  AND SHIFTID= 'TOTAL' ", "ReportDT");
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
        DataTable dtMYDef = (DataTable)Session["dtMYDef"];
        DataRow[] drChart = dtMYDef.Select("ReportDT='Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = 'S' ", "ReportDT,DefectRate desc");
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
            ChartSDefectCode.Titles[0].Text = ddlByProdID.SelectedValue + " S Grade DefectCode Chart";

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
        DataTable dtMYDef = (DataTable)Session["dtMYDef"];
        DataRow[] drChart = dtMYDef.Select("ReportDT<>'Total' AND PRODUCT_ID= '" + ddlByProdID.SelectedValue + "' AND LOTGRADE = 'S' AND DEFCODE= '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "' ", "ReportDT,DefectRate desc");
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
            ChartSDefectDistr.Titles[0].Text = "S Grade DefectCode[" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].ToString() + "] By Time Chart";

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
        DataTable dtMYield = (DataTable)Session["dtMYield"];
        DataTable dtMYDef = (DataTable)Session["dtMYDef"];
        dtMYield.DefaultView.RowFilter = "";
        dtMYield.DefaultView.RowFilter = "(ReportDT='Total' AND OPERATION_ID='" + tbType.Text + "' AND SHIFTID='TOTAL')";
        dtMYield.DefaultView.Sort = "ReportDT,ItemNo";
        GridView1.DataSource = dtMYield;
        GridView1.DataBind();

        dtMYield.DefaultView.RowFilter = "";
        dtMYield.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "'  AND SHIFTID <> 'TOTAL' )";
        dtMYield.DefaultView.Sort = "ItemNo";
        GridView2.DataSource = dtMYield;
        GridView2.DataBind();
        dtMYield.DefaultView.RowFilter = "";

        dtMYield.DefaultView.RowFilter = "";
        dtMYield.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND OPERATION_ID='" + tbType.Text + "'  AND SHIFTID= 'TOTAL' )";
        dtMYield.DefaultView.Sort = "ReportDT,ItemNo";
        GridView3.DataSource = dtMYield;
        GridView3.DataBind();
        dtMYield.DefaultView.RowFilter = "";

        dtMYDef.DefaultView.RowFilter = "";
        dtMYDef.DefaultView.RowFilter = "(ReportDT='Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND LOTGRADE='S')";
        dtMYDef.DefaultView.Sort = "ReportDT,DefectRate DESC";
        GridView4.DataSource = dtMYDef;
        GridView4.DataBind();

        dtMYDef.DefaultView.RowFilter = "";
        dtMYDef.DefaultView.RowFilter = "(ReportDT<>'Total' AND PRODUCT_ID = '" + ddlByProdID.SelectedValue + "' AND DEFCODE = '" + ddlSDefList.SelectedValue.ToString().Split(' ')[0].Trim().ToString() + "' AND LOTGRADE='S')";
        dtMYDef.DefaultView.Sort = "ReportDT,DefectRate DESC";
        GridView5.DataSource = dtMYDef;
        GridView5.DataBind();

        dtMYDef.DefaultView.RowFilter = "";
    }
    protected void ddlByProdID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtMYield = (DataTable)Session["dtMYield"];
        AppClass Apc = new AppClass();
        ClearChartBound();
        int intSecCnt = 0;
        lblTime01.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtMYield.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtMYield.Select("ReportDT<>'Total'");
        foreach (DataRow drDel in drsDel)
        {
            dtMYield.Rows.Remove(drDel);
        }
        dtMYield.DefaultView.RowFilter = "";
        Session["dtMYield"] = null;
        Session["dtMYield"] = dtMYield;

        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "TOTAL");
        QueryFPYDataBound("Days", ddlByProdID.SelectedValue, "SHIFTID");
        ddlSDefList.Items.Clear();
        ddlShiftID.SelectedIndex = 0;
        CreateDefSessionDataTable();
        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlByProdID", "ProdID:" + ddlByProdID.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime01.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
        lblTime02.Text = "";
        lblTime04.Text = "";
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
        DataTable dtMYDef = (DataTable)Session["dtMYDef"];
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        lblTime02.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtMYDef.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtMYDef.Select("ReportDT<>'Total' AND LOTGRADE = 'S' ");
        foreach (DataRow drDel in drsDel)
        {
            dtMYDef.Rows.Remove(drDel);
        }
        dtMYDef.DefaultView.RowFilter = "";
        Session["dtMYDef"] = null;
        Session["dtMYDef"] = dtMYDef;

        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlSDefList", "S Grade DefectCode:" + ddlSDefList.SelectedValue.ToString(), "");

        DateTime dtT = System.DateTime.Now;
        TimeSpan tsDiff = dtT - dtF;
        intSecCnt = Convert.ToInt32(tsDiff.TotalSeconds);
        lblTime02.Text = "查詢耗時：" + intSecCnt.ToString() + "s";
    }
    protected void ddlShiftID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtMYDef = (DataTable)Session["dtMYDef"];
        AppClass Apc = new AppClass();
        int intSecCnt = 0;
        lblTime04.Text = "";
        DateTime dtF = System.DateTime.Now;
        dtMYDef.DefaultView.RowFilter = "";
        DataRow[] drsDel = dtMYDef.Select("");
        foreach (DataRow drDel in drsDel)
        {
            dtMYDef.Rows.Remove(drDel);
        }
        dtMYDef.DefaultView.RowFilter = "";
        Session["dtMYDef"] = null;
        Session["dtMYDef"] = dtMYDef;

        QueryDefectDataBound("TOTAL", ddlByProdID.SelectedValue, "TOTAL", "S", "");
        QueryDefectDataBound("Days", ddlByProdID.SelectedValue, ddlShiftID.SelectedValue.ToString(), "S", "");

        GridViewDataBound();
        ChartDataBound();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Query-ddlSDefList", "ShiftID:" + ddlSDefList.SelectedValue.ToString(), "");

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
    /*V1.10 Add Start*/
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
    /*V1.10 Add End*/
    protected void ChkMerge_CheckedChanged(object sender, EventArgs e)
    {
        DataBoundProdID();
    }
}
