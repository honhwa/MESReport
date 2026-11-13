using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using Report.Library;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Dundas.Charting;
using System.Drawing;
using Dundas.Charting.WebControl;
/*
 * 2014-08-14  Innolux/Runy    V1.01       Add LCD Craft篩選條件
*/
public partial class YMSReport_InputOutpuDefect : System.Web.UI.Page
{
    const int mlyear = 3;
    const int Im = 0;
    DBConn dbConn = new DBConn("");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Login"] == null)
        {
            tbFABID.Text = "";
            tbFABName.Text = "";
            Page.Response.Redirect(@"~\Default.aspx");
            return;
        }
        if (!IsPostBack)
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
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('廠別資訊不正確,無法使用！');</script>");
                Page.Response.Redirect(@"~\Default.aspx");
                return;
            }
            boundyear(ddlYear);
            boundWD(ddlWD, ddltype, ddlYear);
            boundPROD(cblprod);
        }
    }
    private void boundPROD(CheckBoxList CBLPROD)
    {
        string strsql;
        DataSet dsprod = new DataSet();
        cblprod.Items.Clear();
        strsql = "select distinct project_name,sizeid from VMPIM order by sizeid,project_name";
        dsprod = dbConn.GetDataSetBySql(strsql);
        for (int i = 0; i < dsprod.Tables[0].Rows.Count; i++)
        {
            cblprod.Items.Add(new ListItem(dsprod.Tables[0].Rows[i][0].ToString(), dsprod.Tables[0].Rows[i][0].ToString()));
        }

    }
    private void boundyear(DropDownList ddlyear)
    {
        string stryear;
        long lyear;
        long lloop;
        ddlyear.Items.Clear();
        stryear = System.DateTime.Now.Year.ToString();
        lyear = Convert.ToInt64(stryear);
        for (int i = 0; i < mlyear; i++)
        {
            lloop = lyear - i;
            ListItem lst = new ListItem(lloop.ToString(), lloop.ToString());
            ddlyear.Items.Add(lst);
        }

    }
    private void boundWD(DropDownList ddlwd, DropDownList ddltype, DropDownList ddlyear)
    {
        string stryear;
        string strtype;
        string strsql;
        int imonth;
        int iweek;
        int j;
        j = 1;
        DataSet dsMD = new DataSet();
        ddlwd.Items.Clear();
        stryear = ddlyear.SelectedValue.ToString();
        strtype = ddltype.SelectedValue.ToString();
        if (stryear == System.DateTime.Now.Year.ToString())
        {
            strsql = "select datepart(wk,getdate()) as Sweek,datepart(mm,getdate()) as Smonth";
            dsMD = dbConn.GetDataSetBySql(strsql);
            imonth = Convert.ToInt16(dsMD.Tables[0].Rows[0]["smonth"].ToString()) - 1;
            iweek = Convert.ToInt16(dsMD.Tables[0].Rows[0]["sweek"].ToString()) - 1;

        }
        else
        {
            imonth = 12;
            iweek = 55;
        }

        if (ddltype.SelectedValue == "W")
        {
            j = iweek;
        }
        if (ddltype.SelectedValue == "M")
        {
            j = imonth;
        }
        for (int i = 1; i <= j; i++)
        {
            ListItem lst = new ListItem(i.ToString(), i.ToString());
            ddlwd.Items.Add(lst);

        }

    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        boundWD(ddlWD, ddltype, ddlYear);

    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        boundWD(ddlWD, ddltype, ddlYear);

    }
    protected void btnquery_Click(object sender, EventArgs e)
    {



        string strprod;
        //string strerr;
        //string strerrdesc;
        string mweek;
        string mmonth;
        string strsql;
        DataSet dsrate = new DataSet();
        DataView dtdefect = new DataView();
        DataView TBdefect = new DataView();
        DataTable disdefect = new DataTable();

        strprod = getPROD(cblprod);
        Fresh();
        PNLMAIN.Visible = true;
        if (strprod == "")
        {
            lblerror.Text = "Please Select Product!!";
            return;
        }
        else
        {
            lblerror.Text = "";
        }
        mweek = getweek(ddlWD, ddltype, ddlYear);
        mmonth = getMONTH(ddlWD, ddltype, ddlYear);
        if (mweek == "")
        {
            PNLMAIN.Visible = false;
            return;
        }
        if (mmonth == "")
        {
            PNLMAIN.Visible = false;
            return;
        }

        strsql = getSQL(ddlYear.SelectedValue.ToString(), ddltype.SelectedValue.ToString(), Convert.ToInt16(ddlWD.SelectedValue.ToString()), mweek, strprod, mmonth);
        //dsrate = dbConn.GetDataSetBySp("usp_rpt_yield_dataset", para);
        //绑定投入产出良率数据
        dsrate = dbConn.GetDataSetBySql(strsql);
        if (dsrate.Tables[0].Rows.Count == 0)
        {
            lblerror.Text = "No Data!!";
            PNLMAIN.Visible = false;
        }

        else
        {
            lblerror.Text = "";
        }
        gvmain.DataSource = dsrate.Tables[0];
        gvmain.DataBind();
        //图表显示投入产出良率数据
        BindChart(dsrate.Tables[1]);

        //get info for 6
        lbl10.Visible = true;
        dtdefect = dsrate.Tables[2].DefaultView;// DEFECT RATE A
        dtdefect.RowFilter = "processid='cell2'";
        //TBdefect = dsrate.Tables[3].DefaultView;//DEFECT RATE A TO B
        //TBdefect.RowFilter = "processid='cell2'";
        ////disdefect = dsrate.Tables[4];// DISTINCT DEFECT
        BindChartDEFECT(dsrate.Tables[3], disdefect);
        gvcell2_d.DataSource = dtdefect;
        gvcell2_d.DataBind();
        lbl20.Visible = true;
        dtdefect.RowFilter = "processid='m2'";
        gvfini_d.DataSource = dtdefect;
        gvfini_d.DataBind();
        BindChartDEFECTF(dsrate.Tables[3], disdefect);
        lbl30.Visible = true;
        dtdefect.RowFilter = "processid='m3'";
        gvassy_d.DataSource = dtdefect;
        gvassy_d.DataBind();
        BindChartDEFECTA(dsrate.Tables[3], disdefect);
        // get info for cell-defect 3

        lbl40.Visible = true;
        dtdefect = dsrate.Tables[4].DefaultView;
        dtdefect.RowFilter = "processid='cell2'";
        gv_d_cell2.DataSource = dtdefect;
        gv_d_cell2.DataBind();
        BINDCHAR1(dsrate.Tables[5]);
        // get into for fini-defect 4

        LBL50.Visible = true;
        dtdefect.RowFilter = "processid='m2'";
        gv_d_m2.DataSource = dtdefect;
        gv_d_m2.DataBind();
        BINDCHAR2(dsrate.Tables[5]);
        // GET INfo for assy-defect 5

        lbl60.Visible = true;
        dtdefect.RowFilter = "processid='m3'";
        gv_d_m3.DataSource = dtdefect;
        gv_d_m3.DataBind();
        BINDCHAR3(dsrate.Tables[5]);
    }
    //绑定CHART3
    private void BINDCHAR3(DataTable DVDEFECT)
    {

        DataTable dt = new DataTable();

        Chart3.Visible = true;
        Chart3.Series.Clear();
        DVDEFECT.DefaultView.Sort = "PROCESSID,GROUPID,NUMBER,date";
        dt = DVDEFECT.DefaultView.ToTable();
        //Chartcell2_D.DataBindCrossTab(dt.DefaultView, "defectcd", "date", "rate", "Tooltip=rate{N}");
        string seriesName;
        seriesName = "";
        foreach (DataRow row in dt.Rows)
        {
            if (row["processid"].ToString().Trim().ToUpper() == "M3".ToUpper())
            {

                if (seriesName == row["GROUPID"].ToString())
                { }
                else
                {
                    seriesName = row["GROUPID"].ToString();
                    Chart3.Series.Add(seriesName);
                    if (seriesName.ToString().ToUpper().Trim() == "target".ToString().ToUpper() || seriesName.ToString().ToUpper().Trim() == "total".ToString().ToUpper())
                    {
                        Chart3.Series[seriesName].Type = SeriesChartType.Line;
                        //if (seriesName.ToString().ToUpper().Trim() == "total".ToString().ToUpper())
                        //{
                        Chart3.Series[seriesName].MarkerStyle = MarkerStyle.Diamond;
                        Chart3.Series[seriesName].MarkerSize = 4;
                        Chart3.Series[seriesName].ShowLabelAsValue = true;
                        //}

                    }
                    else
                    {
                        Chart3.Series[seriesName].Type = SeriesChartType.StackedColumn;
                    }

                    Chart3.Series[seriesName].BorderWidth = 2;
                }
                string yVal;
                yVal = "";

                string colName = row["DATE"].ToString();
                if (row["qty"].ToString() == "")
                {
                    yVal = "0";
                }
                else
                {
                    yVal = row["qty"].ToString();
                }

                Chart3.Series[seriesName].Points.AddXY(colName, yVal);
            }
        }

        //chartassy_d.DataBind();

        Chart3.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        Chart3.ChartAreas["Default"].AxisX.Interval = 1;

        Chart3.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        Chart3.Legends[0].Alignment = StringAlignment.Far;
        Chart3.Legends[0].AutoFitText = true;
        Chart3.Titles[0].Text = "";
        Chart3.Titles[0].Alignment = ContentAlignment.TopCenter;
        Chart3.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        Chart3.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in Chart3.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        Chart3.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
        Chart3.Visible = true;
        Chart3.DataBind();
    }
    //绑定CHART2
    private void BINDCHAR2(DataTable DVDEFECT)
    {

        DataTable dt = new DataTable();

        Chart2.Visible = true;
        Chart2.Series.Clear();
        DVDEFECT.DefaultView.Sort = "PROCESSID,GROUPID,NUMBER,date";
        dt = DVDEFECT.DefaultView.ToTable();
        //Chartcell2_D.DataBindCrossTab(dt.DefaultView, "defectcd", "date", "rate", "Tooltip=rate{N}");
        string seriesName;
        seriesName = "";
        foreach (DataRow row in dt.Rows)
        {
            if (row["processid"].ToString().Trim().ToUpper() == "M2".ToUpper())
            {

                if (seriesName == row["GROUPID"].ToString())
                { }
                else
                {
                    seriesName = row["GROUPID"].ToString();
                    Chart2.Series.Add(seriesName);
                    if (seriesName.ToString().ToUpper().Trim() == "target".ToString().ToUpper() || seriesName.ToString().ToUpper().Trim() == "total".ToString().ToUpper())
                    {
                        Chart2.Series[seriesName].Type = SeriesChartType.Line;
                        //if (seriesName.ToString().ToUpper().Trim() == "total".ToString().ToUpper())
                        //{
                        Chart2.Series[seriesName].MarkerStyle = MarkerStyle.Diamond;
                        Chart2.Series[seriesName].MarkerSize = 4;
                        Chart2.Series[seriesName].ShowLabelAsValue = true;
                        //}
                    }
                    else
                    {
                        Chart2.Series[seriesName].Type = SeriesChartType.StackedColumn;
                    }

                    Chart2.Series[seriesName].BorderWidth = 2;
                }
                string yVal;
                yVal = "";

                string colName = row["DATE"].ToString();
                if (row["QTY"].ToString() == "")
                {
                    yVal = "0";
                }
                else
                {
                    yVal = row["QTY"].ToString();
                }

                Chart2.Series[seriesName].Points.AddXY(colName, yVal);
            }
        }

        //chartassy_d.DataBind();

        Chart2.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        Chart2.ChartAreas["Default"].AxisX.Interval = 1;

        Chart2.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        Chart2.Legends[0].Alignment = StringAlignment.Far;
        Chart2.Legends[0].AutoFitText = true;
        Chart2.Titles[0].Text = "";
        Chart2.Titles[0].Alignment = ContentAlignment.TopCenter;
        Chart2.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        Chart2.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in Chart2.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        Chart2.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
        Chart2.Visible = true;
        Chart2.DataBind();
    }
    //绑定CHART1
    private void BINDCHAR1(DataTable DVDEFECT)
    {


        DataTable dt = new DataTable();

        Chart1.Visible = true;
        Chart1.Series.Clear();
        DVDEFECT.DefaultView.Sort = "PROCESSID,GROUPID,NUMBER,date";
        dt = DVDEFECT.DefaultView.ToTable();
        //Chartcell2_D.DataBindCrossTab(dt.DefaultView, "defectcd", "date", "rate", "Tooltip=rate{N}");
        string seriesName;
        seriesName = "";
        foreach (DataRow row in dt.Rows)
        {
            if (row["processid"].ToString().Trim().ToUpper() == "CELL2".ToUpper())
            {

                if (seriesName == row["GROUPID"].ToString())
                { }
                else
                {
                    seriesName = row["GROUPID"].ToString();
                    Chart1.Series.Add(seriesName);
                    if (seriesName.ToString().ToUpper().Trim() == "target".ToString().ToUpper() || seriesName.ToString().ToUpper().Trim() == "total".ToString().ToUpper())
                    {
                        Chart1.Series[seriesName].Type = SeriesChartType.Line;
                        //if (seriesName.ToString().ToUpper().Trim() == "total".ToString().ToUpper())
                        //{
                        //    Chart1.Series[seriesName].MarkerStyle = MarkerStyle.Diamond;
                        Chart1.Series[seriesName].MarkerSize = 4;
                        Chart1.Series[seriesName].ShowLabelAsValue = true;
                        //}
                    }
                    else
                    {
                        Chart1.Series[seriesName].Type = SeriesChartType.StackedColumn;
                    }

                    Chart1.Series[seriesName].BorderWidth = 2;
                }
                string yVal;
                yVal = "";

                string colName = row["DATE"].ToString();
                if (row["QTY"].ToString() == "")
                {
                    yVal = "0";
                }
                else
                {
                    yVal = row["QTY"].ToString();
                }

                Chart1.Series[seriesName].Points.AddXY(colName, yVal);
            }
        }

        //chartassy_d.DataBind();

        Chart1.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        Chart1.ChartAreas["Default"].AxisX.Interval = 1;

        Chart1.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        Chart1.Legends[0].Alignment = StringAlignment.Far;
        Chart1.Legends[0].AutoFitText = true;
        Chart1.Titles[0].Text = "";
        Chart1.Titles[0].Alignment = ContentAlignment.TopCenter;
        Chart1.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        Chart1.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in Chart1.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        Chart1.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
        Chart1.Visible = true;
        Chart1.DataBind();
    }





    //绑定DEFECT FOR FINI
    private void BindChartDEFECTA(DataTable DVDEFECT, DataTable disdefect)
    {
        chartassy_d.Visible = true;
        chartassy_d.Series.Clear();
        DVDEFECT.DefaultView.Sort = "PROCESSID,defectcd,NUMBER,date";
        DataTable dt = DVDEFECT.DefaultView.ToTable();
        //Chartcell2_D.DataBindCrossTab(dt.DefaultView, "defectcd", "date", "rate", "Tooltip=rate{N}");
        string seriesName;
        seriesName = "";
        foreach (DataRow row in dt.Rows)
        {
            if (row["processid"].ToString().Trim().ToUpper() == "M3".ToUpper())
            {

                if (seriesName == row["defectcd"].ToString())
                { }
                else
                {
                    seriesName = row["defectcd"].ToString();
                    chartassy_d.Series.Add(seriesName);
                    chartassy_d.Series[seriesName].Type = SeriesChartType.StackedColumn;
                    chartassy_d.Series[seriesName].BorderWidth = 2;
                }
                string yVal;
                yVal = "";

                string colName = row["date"].ToString();
                if (row["qty"].ToString() == "")
                {
                    yVal = "0";
                }
                else
                {
                    yVal = row["qty"].ToString();
                }

                chartassy_d.Series[seriesName].Points.AddXY(colName, yVal);
            }
        }

        //chartassy_d.DataBind();

        chartassy_d.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        chartassy_d.ChartAreas["Default"].AxisX.Interval = 1;

        chartassy_d.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        chartassy_d.Legends[0].Alignment = StringAlignment.Far;
        chartassy_d.Legends[0].AutoFitText = true;
        chartassy_d.Titles[0].Text = "Module(Assembly) total yield loss defects breakdown";
        chartassy_d.Titles[0].Alignment = ContentAlignment.TopCenter;
        chartassy_d.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        chartassy_d.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in chartassy_d.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        chartassy_d.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
        chartassy_d.Visible = true;
        chartassy_d.DataBind();
    }
    //绑定DEFECT FOR FINI
    private void BindChartDEFECTF(DataTable DVDEFECT, DataTable disdefect)
    {

        chartfini_d.Visible = true;
        chartfini_d.Series.Clear();
        DVDEFECT.DefaultView.Sort = "PROCESSID,defectcd,NUMBER,date";
        DataTable dt = DVDEFECT.DefaultView.ToTable();
        //Chartcell2_D.DataBindCrossTab(dt.DefaultView, "defectcd", "date", "rate", "Tooltip=rate{N}");
        string seriesName;
        seriesName = "";
        foreach (DataRow row in dt.Rows)
        {
            if (row["processid"].ToString().Trim().ToUpper() == "M2".ToUpper())
            {

                if (seriesName == row["defectcd"].ToString())
                { }
                else
                {
                    seriesName = row["defectcd"].ToString();
                    chartfini_d.Series.Add(seriesName);
                    chartfini_d.Series[seriesName].Type = SeriesChartType.StackedColumn;
                    chartfini_d.Series[seriesName].BorderWidth = 2;
                }
                string yVal;
                yVal = "";

                string colName = row["date"].ToString();
                if (row["qty"].ToString() == "")
                {
                    yVal = "0";
                }
                else
                {
                    yVal = row["qty"].ToString();
                }

                chartfini_d.Series[seriesName].Points.AddXY(colName, yVal);
            }
        }

        //chartfini_d.DataBind();

        chartfini_d.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        chartfini_d.ChartAreas["Default"].AxisX.Interval = 1;

        chartfini_d.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        chartfini_d.Legends[0].Alignment = StringAlignment.Far;
        chartfini_d.Legends[0].AutoFitText = true;
        chartfini_d.Titles[0].Text = "Module(Finishing) total yield loss defects breakdown";
        chartfini_d.Titles[0].Alignment = ContentAlignment.TopCenter;
        chartfini_d.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        chartfini_d.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in chartfini_d.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        chartfini_d.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
        chartfini_d.Visible = true;
        chartfini_d.DataBind();
    }
    //绑定DEFECT FOR CELL2
    private void BindChartDEFECT(DataTable DVDEFECT, DataTable disdefect)
    {
        //string mweek;
        //string mmonth;
        //string strdate;
        //string[] STRWD;
        //int i;
        //int j;
        DataTable dt = new DataTable();
        //mweek = getweek(ddlWD, ddltype,ddlYear );
        //mweek = mweek.Replace("[", "WK");
        //mweek = mweek.Replace("]", "");
        //mmonth=getMONTH (ddlWD,ddltype,ddlYear );
        //mmonth = mmonth.Replace("[", "MN");
        //mmonth = mmonth.Replace("]", "");
        //strdate = mweek + "," + mmonth;
        //STRWD = strdate.Split(new char[] {',' });
        //i = STRWD.Length;
        Chartcell2_D.Visible = true;
        Chartcell2_D.Series.Clear();
        DVDEFECT.DefaultView.Sort = "PROCESSID,defectcd,NUMBER,date";
        dt = DVDEFECT.DefaultView.ToTable();
        //Chartcell2_D.DataBindCrossTab(dt.DefaultView, "defectcd", "date", "rate", "Tooltip=rate{N}");
        string seriesName;
        seriesName = "";
        foreach (DataRow row in dt.Rows)
        {
            if (row["processid"].ToString().Trim().ToUpper() == "Cell2".ToUpper())
            {

                if (seriesName == row["defectcd"].ToString())
                { }
                else
                {
                    seriesName = row["defectcd"].ToString();
                    Chartcell2_D.Series.Add(seriesName);
                    Chartcell2_D.Series[seriesName].Type = SeriesChartType.StackedColumn;
                    Chartcell2_D.Series[seriesName].BorderWidth = 2;
                }
                string yVal;
                yVal = "";

                string colName = row["date"].ToString();
                if (row["qty"].ToString() == "")
                {
                    yVal = "0";
                }
                else
                {
                    yVal = row["qty"].ToString();
                }

                Chartcell2_D.Series[seriesName].Points.AddXY(colName, yVal);
            }
        }

        //Chartcell2_D.DataBind();     

        Chartcell2_D.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        Chartcell2_D.ChartAreas["Default"].AxisX.Interval = 1;

        Chartcell2_D.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        Chartcell2_D.Legends[0].Alignment = StringAlignment.Far;
        Chartcell2_D.Legends[0].AutoFitText = true;
        Chartcell2_D.Titles[0].Text = "Cell2 total yield loss defects breakdown";
        Chartcell2_D.Titles[0].Alignment = ContentAlignment.TopCenter;
        Chartcell2_D.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        Chartcell2_D.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in Chartcell2_D.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        Chartcell2_D.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##%";
        Chartcell2_D.Visible = true;
        Chartcell2_D.DataBind();
    }
    //绑定投入产出良率图表
    private void BindChart(DataTable DtSUM)
    {
        ChartAll.Series.Clear();
        //ChartAll.DataBindCrossTab(DtSUM.DefaultView, "TYPE", "DATE", "QTY", "label=QTY{N}");
        ChartAll.Series.Add("Series1");
        ChartAll.Series.Add("Series2");
        ChartAll.DataSource = DtSUM;
        Series Series3 = new Series();
        ChartAll.Series.Add(Series3);
        Series Series4 = new Series();
        ChartAll.Series.Add(Series4);
        Series Series5 = new Series();
        ChartAll.Series.Add(Series5);
        Series Series6 = new Series();
        ChartAll.Series.Add(Series6);
        Series Series7 = new Series();
        ChartAll.Series.Add(Series7);

        ChartAll.Series["Series1"].Color = Color.Gold;
        ChartAll.Series["Series2"].Color = Color.Goldenrod;
        ChartAll.Series["Series3"].Color = Color.CadetBlue;
        ChartAll.Series["Series4"].Color = Color.Green;
        ChartAll.Series["Series5"].Color = Color.Blue;
        ChartAll.Series["Series6"].Color = Color.Red;
        ChartAll.Series["Series7"].Color = Color.DarkGray;

        ChartAll.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -60;
        ChartAll.ChartAreas["Default"].AxisX.Interval = 1;

        ChartAll.ChartAreas["Default"].AxisY2.MajorGrid.Enabled = false;
        ChartAll.Titles[0].Text = "Yield Trend by process(Cell 2+Module)";
        ChartAll.Titles[0].Alignment = ContentAlignment.TopCenter;
        ChartAll.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);
        ChartAll.Titles[0].Color = Color.FromArgb(72, 72, 72);
        foreach (Series series in ChartAll.Series)
        {
            series.MapAreaAttributes = " target='_blank'";
        }

        ChartAll.Series["Series1"].Name = "Cell2_OUTPUT";
        ChartAll.Series["Series2"].Name = "Module(Finishing)_OUTPUT";
        ChartAll.Series["Series3"].Name = "Module(Assembly)_OUTPUT";
        ChartAll.Series["Series4"].Name = "Cell2_Yield";
        ChartAll.Series["Series5"].Name = "Overall_Yield";
        ChartAll.Series["Series6"].Name = "Target_Yield";
        ChartAll.Series["Series7"].Name = "Module_Yield";


        ChartAll.Series["Cell2_OUTPUT"].ValueMemberX = "date";
        ChartAll.Series["Cell2_OUTPUT"].XValueType = ChartValueTypes.String;
        ChartAll.Series["Cell2_OUTPUT"].ValueMembersY = "cell2_output";
        ChartAll.Series["Cell2_OUTPUT"].YValueType = ChartValueTypes.Int;
        //ChartAll.Series["計畫產出"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Cell2_OUTPUT"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##}";

        ChartAll.Series["Module(Finishing)_OUTPUT"].ValueMemberX = "date";
        ChartAll.Series["Module(Finishing)_OUTPUT"].XValueType = ChartValueTypes.String;
        ChartAll.Series["Module(Finishing)_OUTPUT"].ValueMembersY = "m2_output";
        ChartAll.Series["Module(Finishing)_OUTPUT"].YValueType = ChartValueTypes.Int;
        //ChartAll.Series["實際產出"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Module(Finishing)_OUTPUT"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##}";


        ChartAll.Series["Module(Assembly)_OUTPUT"].ValueMemberX = "date";
        ChartAll.Series["Module(Assembly)_OUTPUT"].ValueMembersY = "m3_output";
        //ChartAll.Series["Series3"].Type = SeriesChartType.Line;
        //ChartAll.Series["Series3"].YAxisType = AxisType.Secondary;
        ChartAll.Series["Module(Assembly)_OUTPUT"].MarkerStyle = MarkerStyle.Square;
        ChartAll.Series["Module(Assembly)_OUTPUT"].XValueType = ChartValueTypes.String;
        //ChartAll.Series["達成率"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Module(Assembly)_OUTPUT"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##%}";

        ChartAll.Series["Cell2_Yield"].ValueMemberX = "date";
        ChartAll.Series["Cell2_Yield"].ValueMembersY = "cell2_yield";
        ChartAll.Series["Cell2_Yield"].Type = SeriesChartType.Line;
        ChartAll.Series["Cell2_Yield"].YAxisType = AxisType.Secondary;
        ChartAll.Series["Cell2_Yield"].MarkerStyle = MarkerStyle.Square;
        ChartAll.Series["Cell2_Yield"].XValueType = ChartValueTypes.String;
        //ChartAll.Series["達成率"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Cell2_Yield"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##%}";

        ChartAll.Series["Overall_Yield"].ValueMemberX = "date";
        ChartAll.Series["Overall_Yield"].ValueMembersY = "overall_yield";
        ChartAll.Series["Overall_Yield"].Type = SeriesChartType.Line;
        ChartAll.Series["Overall_Yield"].YAxisType = AxisType.Secondary;
        ChartAll.Series["Overall_Yield"].MarkerStyle = MarkerStyle.Square;
        ChartAll.Series["Overall_Yield"].XValueType = ChartValueTypes.String;
        //ChartAll.Series["達成率"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Overall_Yield"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##%}";

        ChartAll.Series["Target_Yield"].ValueMemberX = "date";
        ChartAll.Series["Target_Yield"].ValueMembersY = "target_OVERALL";
        ChartAll.Series["Target_Yield"].Type = SeriesChartType.Line;
        ChartAll.Series["Target_Yield"].YAxisType = AxisType.Secondary;
        ChartAll.Series["Target_Yield"].MarkerStyle = MarkerStyle.Square;
        ChartAll.Series["Target_Yield"].XValueType = ChartValueTypes.String;
        //ChartAll.Series["達成率"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Target_Yield"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##%}";

        ChartAll.Series["Module_Yield"].ValueMemberX = "date";
        ChartAll.Series["Module_Yield"].ValueMembersY = "module_yield";
        ChartAll.Series["Module_Yield"].Type = SeriesChartType.Line;
        ChartAll.Series["Module_Yield"].YAxisType = AxisType.Secondary;
        ChartAll.Series["Module_Yield"].MarkerStyle = MarkerStyle.Square;
        ChartAll.Series["Module_Yield"].XValueType = ChartValueTypes.String;
        //ChartAll.Series["達成率"].Href = "DrillDownBySize.aspx?mfgdate=" + "#VALX" + "&fabid=" + DDFABID.Text + "&shop=" + DDLProdLine.Text + "&type=" + DDLCustomer.Text;
        ChartAll.Series["Module_Yield"].ToolTip = " #VALX " + "  " + ":#VALY{##,##0.##%}";

        ChartAll.ChartAreas["Default"].AxisX.Title = "日期/時間";
        ChartAll.ChartAreas["Default"].AxisY.Title = "產出";
        ChartAll.ChartAreas["Default"].AxisY2.Title = "達成率";
        ChartAll.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##";
        ChartAll.ChartAreas["Default"].AxisY2.LabelStyle.Format = "##,##0.##%";


        ChartAll.Visible = true;

        ChartAll.DataBind();
    }

    //抓取投入产出及良率汇总表
    private string getSQL(string stryear, string type, Int16 strwd, string mweek, string strprod, string mmonth)
    {
        string strsql;
        //int i;
        int istart;//FOR WEEK
        int iend;//FOR WEEK
        int istart1;// FOR MONTH
        int iend1;//FOR MONTH       
        DataSet dsDATE = new DataSet();
        istart1 = 0;
        iend1 = 0;
        string[] Aweek;
        string[] Amonth;
        String STRDATE;
        STRDATE = "";
        Aweek = mweek.Trim().Split(new char[] { ',' });
        istart = Convert.ToInt16(Aweek[0].ToString().Replace("[WK", "").Replace("]", ""));
        iend = Convert.ToInt16(Aweek[Aweek.Length - 1].ToString().Replace("[WK", "").Replace("]", ""));
        Amonth = mmonth.Trim().Split(new char[] { ',' });
        istart1 = Convert.ToInt16(Amonth[0].ToString().Replace("[MN", "").Replace("]", ""));
        iend1 = Convert.ToInt16(Amonth[Amonth.Length - 1].ToString().Replace("[MN", "").Replace("]", ""));
        for (int i = 0; i < Aweek.Length; i++)
        {
            STRDATE = STRDATE + "SUM(ISNULL(" + Aweek[i].ToString() + ",'0')),";
        }
        for (int i = 0; i < Amonth.Length; i++)
        {
            if (i == Amonth.Length - 1)
            {
                STRDATE = STRDATE + "SUM(ISNULL(" + Amonth[i].ToString() + ",'0'))";
            }
            else
            {
                STRDATE = STRDATE + "SUM(ISNULL(" + Amonth[i].ToString() + ",'0')),";
            }

        }
        strsql = "";
        strsql = strsql + " select 'TARGET' AS SHOPCD,'TARGET' AS PROCESSID,CASE FLAG WHEN 'W' THEN 'WK'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),date))) + convert(nvarchar(3),date) WHEN 'M' THEN 'MN'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),date))) + convert(nvarchar(3),date) END AS DATE,min(Cell2) as Cell2,min(finish) as finish,min(Assy)  as Assy ,ROUND(MIN(FINISH)*MIN(ASSY),4) AS MODULE ,round(min(cell2)*min(finish)*min(assy),4) as 'overall' INTO #TARGET from mpytm where project_name in(";
        strsql = strsql + " " + strprod + " )";
        strsql = strsql + "  and year='" + stryear + "' and  (( flag='W' and date  between " + istart + " and " + iend + ") OR (flag='M' and date  between " + istart1 + " and " + iend1 + "))  group by year,CASE FLAG WHEN 'W' THEN 'WK'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),date))) + convert(nvarchar(3),date) WHEN 'M' THEN 'MN'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),date))) + convert(nvarchar(3),date) END";
        strsql = strsql + "  SELECT * INTO #TARGET_SUM FROM (";
        strsql = strsql + "  SELECT SHOPCD,PROCESSID,DATE,'CELL2' AS TYPE,CELL2 AS QTY FROM #TARGET";
        strsql = strsql + "  UNION";
        strsql = strsql + "  SELECT SHOPCD,PROCESSID,DATE,'FINISH' AS TYPE,FINISH AS QTY FROM #TARGET";
        strsql = strsql + "  UNION";
        strsql = strsql + "  SELECT SHOPCD,PROCESSID,DATE,'ASSY' AS TYPE,ASSY AS QTY FROM #TARGET";
        strsql = strsql + "  UNION";
        strsql = strsql + "  SELECT SHOPCD,PROCESSID,DATE,'MODULE' AS TYPE,MODULE AS QTY FROM #TARGET";
        strsql = strsql + "  UNION";
        strsql = strsql + "  SELECT SHOPCD,PROCESSID,DATE,'OVERALL' AS TYPE,OVERALL AS QTY FROM #TARGET";
        strsql = strsql + "  ) MM";

        strsql = strsql + " select shopcd,processid ,'WK'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),REPORTWEEK))) + convert(nvarchar(3),REPORTWEEK) AS REPORTWEEK,'MN'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),REPORTMONTH))) + convert(nvarchar(3),REPORTMONTH) AS REPORTMONTH,sum(INPUTqty) as INPUTqty ,sum(OUTPUTqty) as OUTPUTqty INTO #MOUTM   from SH_PRODID_Yield  where reportyear='" + stryear + "' and ((reportweek between " + istart + " and  " + iend + ") or (reportmonth between " + istart1 + " and " + iend1 + ") )and prodid in(   select distinct prodid from VMPIM where display_mode='normal' and  project_name in(";
        strsql = strsql + " " + strprod + " ))group by shopcd,processid,reportweek,reportmonth";
        strsql = strsql + " select * into #MOUTM_YIELD from (";
        strsql = strsql + " sELECT SHOPCD,PROCESSID,REPORTWEEK AS DATE,SUM(INPUTQTY) AS INPUTQTY,SUM(OUTPUTQTY) AS OUTPUTQTY , round(cast(SUM(OUTPUTqty)/SUM(INPUTqty) as float) ,4) as Yield FROM #MOUTM WHERE REPORTWEEK BETWEEN 'WK" + istart.ToString().PadLeft(2, '0') + "' AND 'WK" + iend.ToString().PadLeft(2, '0') + "' GROUP BY SHOPCD,PROCESSID,REPORTWEEK";
        strsql = strsql + " UNION ";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,REPORTMONTH AS DATE,SUM(INPUTQTY) AS INPUTQTY,SUM(OUTPUTQTY) AS OUTPUTQTY, round(cast(SUM(OUTPUTqty)/SUM(INPUTqty) as float) ,4) as Yield FROM #MOUTM WHERE REPORTMONTH BETWEEN 'MN" + istart1.ToString().PadLeft(2, '0') + "' AND 'MN" + iend1.ToString().PadLeft(2, '0') + "'GROUP BY SHOPCD,PROCESSID,REPORTMONTH";
        strsql = strsql + " ) aa ";
        strsql = strsql + " select date,[cell2],[m2],[m3] into #MOUTM_OVERALL from (";
        strsql = strsql + " select processid,date,yield from #MOUTM_YIELD) p pivot (  max(yield) for processid in([cell2],[m2],[m3])) as pvt";
        strsql = strsql + " SELECT * INTO #MOUTM_YIELD_SUM FROM (";
        strsql = strsql + "  SELECT *FROM #MOUTM_YIELD ";
        strsql = strsql + " UNION";
        strsql = strsql + " select 'OVERALL' AS SHOPCD,'OVERALL' AS PROCESSID,DATE,'0' AS INPUTQTY,'0' AS OUTPUTQTY , round(CELL2*M2*M3 ,4) as Yield FROM #MOUTM_OVERALL ";
        strsql = strsql + " UNION";
        strsql = strsql + " select 'MODULE' AS SHOPCD,'MODULE' AS PROCESSID,DATE,'0' AS INPUTQTY,'0' AS OUTPUTQTY , round(M2*M3 ,4) as Yield FROM #MOUTM_OVERALL ";
        strsql = strsql + "  ) BB";
        strsql = strsql + " SELECT * INTO #RESULT FROM (";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,DATE,'INPUT' AS TYPE,INPUTQTY AS QTY FROM #MOUTM_YIELD_SUM";
        strsql = strsql + "  UNION";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,DATE,'OUTPUT'AS TYPE,OUTPUTQTY AS QTY FROM #MOUTM_YIELD_SUM";
        strsql = strsql + " UNION";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,DATE,'YIELD'AS TYPE,YIELD AS QTY FROM #MOUTM_YIELD_SUM";
        strsql = strsql + " UNION";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,DATE,TYPE,QTY FROM #TARGET_SUM";
        strsql = strsql + " ) CC";
        strsql = strsql + " SELECT  case processid when 'M2' THEN 'Module(finish)' when 'm3' then 'Module(Assembly)' else";
        strsql = strsql + " processid end as Process";
        strsql = strsql + " , Type," + mweek + "," + mmonth + " FROM (";
        strsql = strsql + " select case when processid='target' and type='overall' then 'A' ";
        strsql = strsql + " WHEN PROCESSID='TARGET' AND TYPE='CELL2' THEN 'B'";
        strsql = strsql + " WHEN PROCESSID='TARGET' AND TYPE='FINISH' THEN 'C'";
        strsql = strsql + " WHEN PROCESSID='TARGET' AND TYPE='ASSY' THEN 'D'";
        strsql = strsql + " WHEN PROCESSID='TARGET' AND TYPE='MODULE' THEN 'E'";
        strsql = strsql + " WHEN PROCESSID='OVERALL' AND TYPE='YIELD' THEN 'F'";
        strsql = strsql + " WHEN PROCESSID='CELL2' AND TYPE='INPUT' THEN 'G'";
        strsql = strsql + " WHEN PROCESSID='CELL2' AND TYPE='OUTPUT' THEN 'H'";
        strsql = strsql + " WHEN PROCESSID='CELL2' AND TYPE='YIELD' THEN 'I'";
        strsql = strsql + " WHEN PROCESSID='M2' AND TYPE='INPUT' THEN 'J'";
        strsql = strsql + " WHEN PROCESSID='M2' AND TYPE='OUTPUT' THEN 'K'";
        strsql = strsql + " WHEN PROCESSID='M2' AND TYPE='YIELD' THEN 'L'";
        strsql = strsql + " WHEN PROCESSID='M3' AND TYPE='INPUT' THEN 'M'";
        strsql = strsql + " WHEN PROCESSID='M3' AND TYPE='OUTPUT' THEN 'N'";
        strsql = strsql + " WHEN PROCESSID='M3' AND TYPE='YIELD' THEN 'O'";
        strsql = strsql + " WHEN PROCESSID='MODULE' AND TYPE='YIELD' THEN 'P' END AS NUMBER,";
        strsql = strsql + "  SHOPCD, PROCESSID,TYPE, " + mweek + "," + mmonth + "   from   (  select SHOPCD,PROCESSID,DATE,TYPE,QTY  from #RESULT  )p  pivot  (  MAX(QTY ) for DATE in( " + mweek + "," + mmonth + ")  ) as pvt";
        strsql = strsql + "  ) MM WHERE NUMBER IS NOT NULL ORDER BY NUMBER";

        strsql = strsql + " select CASE LEFT(LTRIM(DATE),2) WHEN 'WK' THEN 'A' ELSE 'B' END AS NUMBER,DATE, [CELL2_OUTPUT],[M2_OUTPUT],[M3_OUTPUT],[CELL2_YIELD] * 100 AS [CELL2_YIELD] ,[MODULE_YIELD] * 100 AS [MODULE_YIELD] ,[OVERALL_YIELD] * 100 AS [OVERALL_YIELD],[TARGET_OVERALL] * 100 AS [TARGET_OVERALL]   ";
        strsql = strsql + " from   (  select date,processid+'_'+type as type,qty FROM #RESULT where type in('output','yield') or processid ='target' )p  pivot  (  MAX(QTY ) for TYPE in( [CELL2_OUTPUT],[M2_OUTPUT],[M3_OUTPUT],[CELL2_YIELD],[MODULE_YIELD],[OVERALL_YIELD],[TARGET_OVERALL] ) ) as pvt ORDER BY NUMBER";

        //DEFECT info
        strsql = strsql + " select shopcd,processid ,'WK'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),REPORTWEEK))) + convert(nvarchar(3),REPORTWEEK) AS REPORTWEEK,'MN'+REPLICATE('0', 2 - LEN(convert(nvarchar(3),REPORTMONTH))) + convert(nvarchar(3),REPORTMONTH) AS REPORTMONTH,DEFECTCD,sum(DFqty) as Dqty INTO #MOUTDM  from SH_defect_Yield  where reportyear='" + stryear + "' and ((reportweek between " + istart + " and  " + iend + ") or (reportmonth between " + istart1 + " and " + iend1 + ") )and prodid in(   select distinct prodid from VMPIM where FABID='B' AND  display_mode='normal' and  project_name in(";
        strsql = strsql + " " + strprod + "))group by shopcd,processid,reportweek,reportmonth,DEFECTCD";
        strsql = strsql + " select * into #MOUTDM_YIELD from (";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,REPORTWEEK AS DATE,DEFECTCD,SUM(DQTY) AS DQTY FROM #MOUTDM";
        strsql = strsql + " WHERE   REPORTWEEK BETWEEN 'WK" + istart.ToString().PadLeft(2, '0') + "' AND 'WK" + iend.ToString().PadLeft(2, '0') + "' GROUP BY SHOPCD,PROCESSID,REPORTWEEK,DEFECTCD";
        strsql = strsql + " UNION ";
        strsql = strsql + " SELECT SHOPCD,PROCESSID,REPORTMONTH AS DATE,DEFECTCD,SUM(DQTY) AS DQTY FROM #MOUTDM ";
        strsql = strsql + "  WHERE  REPORTMONTH BETWEEN 'MN" + istart1.ToString().PadLeft(2, '0') + "' AND 'MN" + iend1.ToString().PadLeft(2, '0') + "'GROUP BY SHOPCD,PROCESSID,REPORTMONTH,DEFECTCD";
        strsql = strsql + " ) aa ";

        strsql = strsql + " SELECT  BB.*,B.DEFECTCD,DEFECTDESC INTO #DEFECT_ALL FROM (";
        strsql = strsql + " SELECT DISTINCT SHOPCD,PROCESSID,DATE FROM #MOUTDM_YIELD) BB LEFT JOIN MDCM B";
        strsql = strsql + " ON 1=1";
        strsql = strsql + " SELECT C.SHOPCD,C.PROCESSID,C.DATE,C.DEFECTCD,C.defectdesc,";
        strsql = strsql + " ROUND(A.DQTY/B.INPUTQTY,6) AS QTY INTO #DEFECT_RESULT";
        strsql = strsql + "  FROM #DEFECT_ALL C LEFT JOIN #MOUTDM_YIELD A ON C.SHOPCD=A.SHOPCD AND C.PROCESSID=A.PROCESSID";
        strsql = strsql + " AND C.DATE=A.DATE AND C.DEFECTCD=A.DEFECTCD";
        strsql = strsql + " LEFT JOIN #MOUTM_YIELD  B ON ";
        strsql = strsql + " A.SHOPCD=B.SHOPCD AND A.PROCESSID=B.PROCESSID AND A.DATE=B.DATE";


        strsql = strsql + " SELECT  PROCESSID,DEFECTCD,DEFECTDESC, " + mweek + ',' + mmonth + "  ";
        strsql = strsql + " into #DEFECT from   (  select SHOPCD,PROCESSID,DATE,DEFECTCD,DEFECTDESC,QTY  from #DEFECT_RESULT  )p  pivot  (  MAX(QTY ) for DATE in( " + mweek + ',' + mmonth + ")  ) as pvt";
        strsql = strsql + " ORDER BY PROCESSID,DEFECTCD";
        strsql = strsql + " select * from #defect";
        strsql = strsql + " union";
        strsql = strsql + " SELECT PROCESSID,'ZZZZ' as DEFECTCD,'' as DEFECTDESC,";
        strsql = strsql + " " + STRDATE;
        strsql = strsql + " FROM #DEFECT GROUP BY PROCESSID";
        strsql = strsql + " select processid,CASE LEFT(LTRIM(DATE),2) WHEN 'WK' THEN 'A' ELSE 'B' END AS NUMBER,DATE,defectcd,qty *100 AS QTY from #defect_result ORDER BY PROCESSID,defectcd,NUMBER";

        strsql = strsql + " SELECT C.SHOPCD,C.PROCESSID,C.DATE,C.DEFECTCD,C.defectdesc,";
        strsql = strsql + " B.MGROUP,B.GROUPID,A.DQTY INTO #DEFECT_GROUP FROM #DEFECT_ALL C LEFT JOIN #MOUTDM_YIELD A ON C.SHOPCD=A.SHOPCD AND C.PROCESSID=A.PROCESSID";
        strsql = strsql + " AND C.DATE=A.DATE AND C.DEFECTCD=A.DEFECTCD";
        strsql = strsql + " LEFT JOIN MPYDM B ON A.PROCESSID=B.PROCESSID AND A.DEFECTCD=B.DEFECT";


        strsql = strsql + " SELECT shopcd,processid,date,ISNULL(MGROUP,'UNDEFINE') AS MGROUP,";
        strsql = strsql + "  ISNULL(GROUPID,'UNDEFINE') AS GROUPID ,sum(isnull(dqty,'0')) as dqty ";
        strsql = strsql + "  INTO #DEFECT_GROUP_D";
        strsql = strsql + "  FROM #DEFECT_GROUP group by shopcd,processid,date,mgroup,groupid";


        strsql = strsql + " SELECT * INTO #DEFECT_GROUP_S FROM (";
        strsql = strsql + " SELECT * FROM #DEFECT_GROUP_D";
        strsql = strsql + " UNION";
        strsql = strsql + " SELECT shopcd,processid,date,MGROUP,'TOTAL' AS GROUPID,SUM(DQTY) AS DQTY";
        strsql = strsql + "  FROM #DEFECT_GROUP_D WHERE MGROUP<>'UNDEFINE' GROUP BY SHOPCD,PROCESSID,DATE,MGROUP";
        strsql = strsql + " UNION";
        strsql = strsql + " SELECT shopcd,processid,date,'TOTAL' AS MGROUP,'TOTAL' AS GROUPID,SUM(DQTY) AS DQTY";
        strsql = strsql + "  FROM #DEFECT_GROUP_D  WHERE MGROUP IN('BIPANE','SH') GROUP BY SHOPCD,PROCESSID,DATE) DD";
        strsql = strsql + "  select AA.SHOPCD,AA.PROCESSID,AA.DATE,AA.MGROUP,AA.GROUPID,AA.DQTY AS QTY1,BB.DQTY AS QTY2";
        strsql = strsql + "   INTO #FEBE from (";
        strsql = strsql + "  SELECT shopcd,processid,date,MGROUP,'TOTAL' AS GROUPID,SUM(DQTY) AS DQTY";
        strsql = strsql + "   FROM #DEFECT_GROUP_D WHERE MGROUP NOT IN('UNDEFINE','DL') GROUP BY SHOPCD,PROCESSID,DATE,MGROUP) AA";
        strsql = strsql + "  left join ";
        strsql = strsql + "   (SELECT shopcd,processid,date,'TOTAL' AS MGROUP,'TOTAL' AS GROUPID,SUM(DQTY) AS DQTY";
        strsql = strsql + "   FROM #DEFECT_GROUP_D WHERE MGROUP  IN('BIPANE','SH')GROUP BY SHOPCD,PROCESSID,DATE)BB ";
        strsql = strsql + "  ON AA.SHOPCD=BB.SHOPCD AND AA.PROCESSID=BB.PROCESSID";
        strsql = strsql + "  AND AA.DATE=BB.DATE AND AA.GROUPID=BB.GROUPID";
        strsql = strsql + "  SELECT SHOPCD,PROCESSID,DATE,CASE MGROUP WHEN 'BIPANE' THEN 'FE%'";
        strsql = strsql + "  WHEN 'SH' THEN 'BE%' END AS MGROUP,GROUPID,ROUND(QTY1/QTY2,6) AS QTY INTO #FEBE_R FROM #FEBE";

        strsql = strsql + " select * INTO #DEFECT_GROUP_R from(";
        strsql = strsql + " SELECT A.SHOPCD,A.PROCESSID,A.DATE,A.MGROUP,A.GROUPID";
        strsql = strsql + " ,ROUND(A.DQTY/B.INPUTQTY,6) AS QTY";
        strsql = strsql + "  FROM #DEFECT_GROUP_S A LEFT JOIN #MOUTM_YIELD B";
        strsql = strsql + " ON A.SHOPCD=B.SHOPCD AND A.PROCESSID=B.PROCESSID";
        strsql = strsql + " AND A.DATE=B.DATE ";
        strsql = strsql + " union SELECT * from #FEBE_R) MN";

        strsql = strsql + " SELECT  PROCESSID,CASE MGROUP WHEN 'TARGET' THEN 'A' WHEN 'BIPANE' THEN 'B'";
        strsql = strsql + " WHEN 'SH' THEN 'C' WHEN 'DL' THEN 'D' WHEN 'UNDEFINE' THEN 'E'";
        strsql = strsql + " WHEN 'FE%' THEN 'G' WHEN 'BE%' THEN 'H' WHEN 'TOTAL' THEN 'F' END AS NUMBER, MGROUP,GROUPID, " + mweek + ',' + mmonth + "   from   (  select SHOPCD,PROCESSID,DATE,MGROUP,GROUPID,QTY  from #DEFECT_GROUP_R";
        strsql = strsql + " UNION SELECT  SHOPCD,PROCESSID,DATE,MGROUP,GROUPID,QTY  from #FEBE_R UNION SELECT 'TARGET' AS SHOPCD, CASE TYPE WHEN 'FINISH'THEN 'M2' WHEN 'ASSY' THEN 'M3' ELSE TYPE END  AS PROCESSID,DATE,'TARGET' AS MGROUP,'TARGET'AS GROUPID,ROUND((1-QTY),6) AS QTY FROM #TARGET_SUM WHERE  ";
        strsql = strsql + "  TYPE NOT IN('MODULE','OVERALL') )p  pivot  (  MAX(QTY ) for DATE in(  " + mweek + ',' + mmonth + ")  ) as pvt";
        strsql = strsql + " ORDER BY PROCESSID,NUMBER,MGROUP,GROUPID";

        strsql = strsql + " SELECT AA.SHOPCD,AA.PROCESSID,AA.DATE,BB.GROUPID INTO #GROUP FROM (";
        strsql = strsql + " SELECT DISTINCT SHOPCD,PROCESSID,DATE FROM #MOUTDM_YIELD )";
        strsql = strsql + " AA LEFT JOIN (SELECT DISTINCT PROCESSID,GROUPID FROM MPYDM WHERE MGROUP<>'DL') BB ON AA.PROCESSID=BB.PROCESSID";

        strsql = strsql + " SELECT A.SHOPCD,CASE LEFT(LTRIM(A.DATE),2) WHEN 'WK' THEN 'A' ELSE 'B' END AS NUMBER, A.PROCESSID,A.DATE,A.GROUPID,AA.QTY*100 AS QTY FROM #GROUP A LEFT JOIN ";
        strsql = strsql + " (select *from #DEFECT_GROUP_R WHERE GROUPID<>'TOTAL' AND MGROUP<>GROUPID";
        strsql = strsql + " union";
        strsql = strsql + " select * from #FEBE_R WHERE GROUPID<>'TOTAL' AND MGROUP<>GROUPID) AA";
        strsql = strsql + " ON A.PROCESSID=AA.PROCESSID AND A.DATE=AA.DATE AND A.GROUPID=AA.GROUPID";

        strsql = strsql + " UNION ";
        strsql = strsql + " select SHOPCD,CASE LEFT(LTRIM(DATE),2) WHEN 'WK' THEN 'A' ELSE 'B' END AS NUMBER, PROCESSID,DATE,GROUPID ,QTY *100 AS QTY from #DEFECT_GROUP_R WHERE GROUPID IN('TARGE','TOTAL')";
        strsql = strsql + " AND GROUPID=MGROUP";
        strsql = strsql + " UNION";
        strsql = strsql + " SELECT 'TARGET' AS SHOPCD,CASE LEFT(LTRIM(DATE),2) WHEN 'WK' THEN 'A' ELSE 'B' END AS NUMBER, CASE TYPE WHEN 'FINISH'THEN 'M2' WHEN 'ASSY' THEN 'M3' ELSE TYPE END  AS PROCESSID,DATE,'TARGET'AS GROUPID,ROUND((1-QTY),6) *100 AS QTY FROM #TARGET_SUM WHERE DATE IN(SELECT DISTINCT DATE FROM #DEFECT_GROUP_R) AND TYPE NOT IN('MODULE','OVERALL')";

        return strsql;
    }
    private string getPROD(CheckBoxList cblprod)
    {
        string strprod = "";                              //V1.01 Add
        if (ddlGCraft.SelectedIndex == 0)                  //V1.01 Add
        {                                                  //V1.01 Add
            //string strprod;                              //V1.01 Mark
            int i;
            i = cblprod.Items.Count;
            strprod = "";
            if (ddlprodtype.SelectedValue.ToString().ToUpper() == "ALL")
            {
                for (int j = 0; j < i; j++)
                {

                    strprod = strprod + "'" + cblprod.Items[j].Value.ToString() + "',";

                }
            }
            else
            {
                for (int j = 0; j < i; j++)
                {
                    if (cblprod.Items[j].Selected)
                    {
                        strprod = strprod + "'" + cblprod.Items[j].Value.ToString() + "',";
                    }
                }
                if (strprod == "")
                {
                    lblerror.Text = "Please Select Product!! ";
                    return "";
                }
                else
                {
                    lblerror.Text = "";
                }
            }
        }
        /* V1.01 Add start*/
        else
        {
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
                strprod += "'" + dsLCDProd.Tables[0].Rows[i][0].ToString() + "',";
            }
            if (strprod == "")
            {
                lblerror.Text = "該玻璃形態下未設定任何機種!! ";
                return "";
            }
            else
            {
                lblerror.Text = "";
            }
        }
        /* V1.01 Add End*/
        strprod = strprod.Substring(0, strprod.Length - 1);
        return strprod;
    }
    private string getweek(DropDownList ddlwd, DropDownList ddltype, DropDownList ddlyear)
    {
        string mweek;
        int i;

        i = Convert.ToInt16(ddlwd.SelectedValue.ToString());
        int iweek;
        string strsql;
        DataSet dsMD = new DataSet();
        mweek = "";
        iweek = 0;
        if (ddltype.SelectedValue == "W")
        {
            for (int m = i - 4; m <= i; m++)
            {
                if (m >= 0)
                {
                    mweek = mweek + "[WK" + m.ToString().PadLeft(2, '0') + "],";

                }


            }
        }
        if (ddltype.SelectedValue == "M")
        {
            //选择的月是当前系统的月


            if (ddlwd.SelectedValue.ToString().Trim() == System.DateTime.Now.Month.ToString().Trim())
            {
                strsql = "select datepart(wk,getdate()) as Sweek,datepart(mm,getdate()) as Smonth";
                dsMD = dbConn.GetDataSetBySql(strsql);
                iweek = Convert.ToInt16(dsMD.Tables[0].Rows[0]["sweek"].ToString()) - 1;

            }
            else
            {
                strsql = "select max(reportweek) as reportweek from SH_PRODID_Yield where reportyear='" + ddlyear.SelectedValue.ToString() + "' and reportmonth<=" + Convert.ToInt16(ddlwd.SelectedValue.ToString());
                dsMD = dbConn.GetDataSetBySql(strsql);
                if (dsMD.Tables[0].Rows[0]["reportweek"].ToString().Trim() == "".ToString().Trim())
                {
                    lblerror.Text = "No Data!!";
                    return "";
                }
                else
                {
                    lblerror.Text = "";
                    iweek = Convert.ToInt16(dsMD.Tables[0].Rows[0]["reportweek"].ToString());
                }

            }
            for (int m = iweek - 4; m <= iweek; m++)
            {
                if (m >= 0)
                {
                    mweek = mweek + "[WK" + m.ToString().PadLeft(2, '0') + "],";
                }

            }
        }

        mweek = mweek.Substring(0, mweek.Length - 1);
        return mweek;
    }
    //行列转换的MONTH
    private string getMONTH(DropDownList ddlwd, DropDownList ddltype, DropDownList ddlyear)
    {
        string mMONTH;
        string strsql;
        int i;
        DataSet dsDATE = new DataSet();

        mMONTH = "";
        i = 0;
        if (ddltype.SelectedValue == "W")
        {
            strsql = "select max(reportmonth) AS reportmonth from SH_PRODID_Yield where reportyear='" + ddlyear.SelectedValue.ToString() + "' and reportweek<=" + Convert.ToInt16(ddlwd.SelectedValue.ToString());
            dsDATE = dbConn.GetDataSetBySql(strsql);
            if (dsDATE.Tables[0].Rows[0]["reportmonth"].ToString().Trim() == "".ToString().Trim())
            {
                lblerror.Text = "No Data!!";
                return "";

            }
            else
            {
                lblerror.Text = "";
                i = Convert.ToInt16(dsDATE.Tables[0].Rows[0]["reportmonth"].ToString());
                for (int m = 1; m <= i; m++)
                {

                    mMONTH = mMONTH + "[MN" + m.ToString().PadLeft(2, '0') + "],";

                }
            }

        }
        if (ddltype.SelectedValue == "M")
        {
            i = Convert.ToInt16(ddlwd.SelectedValue.ToString());
            for (int j = 1; j <= i; j++)
            {

                mMONTH = mMONTH + "[MN" + j.ToString().PadLeft(2, '0') + "],";

            }
        }

        mMONTH = mMONTH.Substring(0, mMONTH.Length - 1);
        return mMONTH;
    }


    protected void gvmain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text.ToString().ToUpper() == "CELL2".ToUpper())
            {
                e.Row.ForeColor = Color.CadetBlue;
            }
            if (e.Row.Cells[0].Text.ToString().ToUpper() == "overall".ToUpper())
            {
                e.Row.ForeColor = Color.Chartreuse;
            }
            if (e.Row.Cells[0].Text.ToString().ToUpper() == "module(finish)".ToUpper())
            {
                e.Row.ForeColor = Color.Coral;
            }
            if (e.Row.Cells[0].Text.ToString().ToUpper() == "module(assembly)".ToUpper())
            {
                e.Row.ForeColor = Color.CornflowerBlue;
            }
            if (e.Row.Cells[0].Text.ToString().ToUpper() == "module".ToUpper())
            {
                e.Row.ForeColor = Color.DarkGoldenrod;
            }

            for (int m = 2; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1" && e.Row.Cells[1].Text.ToString().ToUpper() == "yield".ToUpper())
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
        }

    }
    protected void gvcell2_d_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text.ToString().ToUpper() == "ZZZZ".ToUpper())
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.ForeColor = Color.DarkGoldenrod;
            }
            for (int m = 3; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                {
                    if (Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 == 0)
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }

                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "0")
                {
                    e.Row.Cells[m].Text = "";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
            int rowindex = e.Row.RowIndex;
            if (rowindex > 0)
            {
                if (gvcell2_d.Rows[Im].Cells[0].RowSpan == 0) gvcell2_d.Rows[Im].Cells[0].RowSpan++;
                gvcell2_d.Rows[Im].Cells[0].RowSpan++;
                e.Row.Cells[0].Visible = false;
            }
        }
    }
    protected void gvfini_d_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvfini_d_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = "Module (Finish)";
            if (e.Row.Cells[1].Text.ToString().ToUpper() == "ZZZZ".ToUpper())
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.ForeColor = Color.DarkGoldenrod;
            }
            for (int m = 3; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                {
                    if (Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 == 0)
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }

                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "0")
                {
                    e.Row.Cells[m].Text = "";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
            int rowindex = e.Row.RowIndex;
            if (rowindex > 0)
            {
                if (gvfini_d.Rows[Im].Cells[0].RowSpan == 0) gvfini_d.Rows[Im].Cells[0].RowSpan++;
                gvfini_d.Rows[Im].Cells[0].RowSpan++;
                e.Row.Cells[0].Visible = false;
            }
        }
    }

    protected void gvassy_d_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;


        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Cells[0].Text = "Module (Assembly)";
            if (e.Row.Cells[1].Text.ToString().ToUpper() == "ZZZZ".ToUpper())
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.ForeColor = Color.DarkGoldenrod;
            }
            for (int m = 3; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                {
                    if (Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 == 0)
                    {
                        e.Row.Cells[m].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                    }

                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "0" || e.Row.Cells[m].Text.ToString().Trim() == "&nbsp;")
                {
                    e.Row.Cells[m].Text = "";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
            int rowindex = e.Row.RowIndex;
            if (rowindex > 0)
            {
                if (gvassy_d.Rows[Im].Cells[0].RowSpan == 0) gvassy_d.Rows[Im].Cells[0].RowSpan++;
                gvassy_d.Rows[Im].Cells[0].RowSpan++;
                e.Row.Cells[0].Visible = false;
            }
        }
    }
    protected void gv_d_cell2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            if (e.Row.Cells[3].Text.Trim().ToUpper() == "target".ToUpper() || e.Row.Cells[3].Text.Trim().ToUpper() == "total".ToUpper())
            {
                e.Row.Cells[3].Text = "";
                e.Row.ForeColor = Color.DarkGoldenrod;
            }
            for (int m = 4; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().ToString().IndexOf(".") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
        }
    }
    protected void gv_d_m2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            if (e.Row.Cells[2].Text.Trim() == "DL")
            {
                e.Row.Cells[2].Text = "Bipane/SH";
            }
            if (e.Row.Cells[3].Text.Trim().ToUpper() == "target".ToUpper() || e.Row.Cells[3].Text.Trim().ToUpper() == "total".ToUpper())
            {
                e.Row.Cells[3].Text = "";
                e.Row.ForeColor = Color.DarkGoldenrod;
            }
            for (int m = 4; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString() == "0")
                {
                    e.Row.Cells[m].Text = "";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
        }
    }
    protected void gv_d_m3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i;

        i = e.Row.Cells.Count;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            if (e.Row.Cells[2].Text.Trim() == "DL")
            {
                e.Row.Cells[2].Text = "Bipane/SH";
            }
            if (e.Row.Cells[3].Text.Trim().ToUpper() == "target".ToUpper() || e.Row.Cells[3].Text.Trim().ToUpper() == "total".ToUpper())
            {
                e.Row.Cells[3].Text = "";
                e.Row.ForeColor = Color.DarkGoldenrod;
            }


            for (int m = 4; m < i; m++)
            {
                if (e.Row.Cells[m].Text.ToString().Length > 2 && e.Row.Cells[m].Text.ToString().IndexOf(".") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString() == "0")
                {
                    e.Row.Cells[m].Text = "";
                }
                if (e.Row.Cells[m].Text.ToString().ToString().IndexOf("-") > 0)
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
                if (e.Row.Cells[m].Text.ToString().Trim() == "1")
                {
                    e.Row.Cells[m].Text = Convert.ToDouble(e.Row.Cells[m].Text.ToString()) * 100 + "%";
                }
            }
        }
    }
    protected void btnfresh_Click(object sender, EventArgs e)
    {
        ddlYear.Enabled = true;
        ddltype.Enabled = true;
        ddlWD.Enabled = true;
        cblprod.Enabled = true;
        PNLMAIN.Visible = false;
        AppClass Apc = new AppClass();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Clear", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnfresh_Click", "", "");
    }
    private void Fresh()
    {
        ddlYear.Enabled = false;
        ddltype.Enabled = false;
        ddlWD.Enabled = false;
        cblprod.Enabled = false;
        PNLMAIN.Visible = false;
    }
    protected void gv_d_cell2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        AppClass Apc = new AppClass();
        Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnexport_click", "", "");
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
        //this.ChartAll.Page = this;
        //this.ChartAll.RenderControl (oHtmlTextWriter);
        //this.gvmain.RenderControl(oHtmlTextWriter);
        //this.Chart1.RenderControl(oHtmlTextWriter);
        //this.gv_d_cell2.RenderControl(oHtmlTextWriter);
        //this.Chart2.RenderControl(oHtmlTextWriter);
        //this.gv_d_m2.RenderControl(oHtmlTextWriter);
        //this.Chart3.RenderControl(oHtmlTextWriter);
        //this.gv_d_m3.RenderControl(oHtmlTextWriter);

        //this.Chartcell2_D.RenderControl(oHtmlTextWriter);
        //this.gvcell2_d.RenderControl(oHtmlTextWriter);
        //this.chartfini_d.RenderControl(oHtmlTextWriter);
        //this.gvfini_d.RenderControl(oHtmlTextWriter);
        //this.chartassy_d.RenderControl(oHtmlTextWriter);
        //this.gvassy_d.RenderControl(oHtmlTextWriter);

        this.PNLMAIN.RenderControl(oHtmlTextWriter);
        Response.Output.Write(oStringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void ddlprodtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlprodtype.SelectedValue.ToString().ToUpper() == "ALL")
        {
            pnl.Visible = false;
        }
        if (ddlprodtype.SelectedValue.ToString().ToUpper() == "Product".ToUpper())
        {
            pnl.Visible = true;
        }
        PNLMAIN.Visible = false;
    }
    /*V1.01 Add Start*/
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
    /*V1.01 Add End*/
}
