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

public partial class YMSReport_SH_Bosch_Yield : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            tbFABID.Text = "B";
            DataBoundtxtDate();

            txtFDate.Attributes.Add("readonly", "readonly");
            txtTDate.Attributes.Add("readonly", "readonly");
        }
    }
    protected void DataBoundtxtDate()
    {
        txtFDate.Text = System.DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd");
        txtTDate.Text = System.DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd");
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
            ddlYear.SelectedValue = "2015";
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
    protected void btQuery_Click(object sender, EventArgs e)
    {
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        string strSql = "";
        strSql = strSql + " SELECT ";
        if (ddlDateType.SelectedValue == "Date")
        {
            strSql = strSql + " ReportDate";
        }
        if (ddlDateType.SelectedValue == "Week")
        {
            strSql = strSql + " CONVERT(VARCHAR(4),ReportDate,120)+'-W'+RIGHT('0'+CAST(DATEDIFF(ww,cast(year(ReportDate) as varchar)  + '-01-01',ReportDate) AS VARCHAR),2) AS ReportDate";
        }
        if (ddlDateType.SelectedValue == "Month")
        {
            strSql = strSql + " REPLACE(CONVERT(VARCHAR(7),ReportDate,120),'-','-M') AS ReportDate";
        }
        strSql = strSql + " ,FAB_ID";
        strSql = strSql + " ,PRODUCT_ID";
        strSql = strSql + " ,OPERATION_ID";
        strSql = strSql + " ,LOT_GRADE";
        strSql = strSql + " ,DEF_CODE";
        strSql = strSql + " ,LOT_T_CNT";
        strSql = strSql + " INTO #SHBoschYield";
        strSql = strSql + " FROM SH_Bosch_Yield";
        strSql = strSql + " WHERE OPERATION_ID = '" + ddlStation.SelectedValue.ToString() + "'";
        if (ddlDateType.SelectedValue == "Date")
        {
            strSql = strSql + " AND ReportDate BETWEEN '" + txtFDate.Text + "' AND '" + txtTDate.Text + "'";
        }
        if (ddlDateType.SelectedValue == "Week")
        {
            strSql = strSql + " AND DATEPART(YEAR,ReportDate) = " + ddlYear.SelectedValue + "";
            strSql = strSql + " AND DATEDIFF(ww,cast(year(ReportDate) as varchar)  + '-01-01',ReportDate) BETWEEN " + Convert.ToInt32(ddlFDate.SelectedValue.Replace("W", "")) + " AND " + Convert.ToInt32(ddlTDate.SelectedValue.Replace("W", "")) + " ";
        }
        if (ddlDateType.SelectedValue == "Month")
        {

            strSql = strSql + " AND DATEPART(YEAR,ReportDate) = " + ddlYear.SelectedValue + "";
            strSql = strSql + " AND DATEPART(MONTH,ReportDate) BETWEEN " + Convert.ToInt32(ddlFDate.SelectedValue.Replace("M", "")) + " AND " + Convert.ToInt32(ddlTDate.SelectedValue.Replace("M", "")) + " ";
        }
        strSql = strSql + " AND PRODUCT_ID = 'DJ103FA-01A' ";
        strSql = strSql + " AND LOT_T_CNT>0; ";

        strSql = strSql + " SELECT PRODUCT_ID,OPERATION_ID";
        strSql = strSql + " ,SUM(LOT_T_CNT) AS Qty";
        strSql = strSql + " ,CASE WHEN SUM(LOT_T_CNT) = 0 THEN 0 ELSE 1.00 * SUM(CASE WHEN LOT_GRADE = 'A' THEN LOT_T_CNT ELSE 0 END) / SUM(LOT_T_CNT) END AS ARate";
        strSql = strSql + " ,CASE WHEN SUM(LOT_T_CNT) = 0 THEN 0 ELSE 1.00 * SUM(CASE WHEN LOT_GRADE = 'S' THEN LOT_T_CNT ELSE 0 END) / SUM(LOT_T_CNT) END AS SRate";
        strSql = strSql + " FROM #SHBoschYield";
        strSql = strSql + " GROUP BY PRODUCT_ID,OPERATION_ID";
        strSql = strSql + " ORDER BY PRODUCT_ID,OPERATION_ID;";

        strSql = strSql + " SELECT A.PRODUCT_ID,A.OPERATION_ID,A.LOT_GRADE,A.DEF_CODE,ISNULL(C.DEFECTDESC,'') AS DEFECTDESC,1.00 * A.DFQty / B.Qty AS DefectRate,A.DFQty AS Qty";
        strSql = strSql + " FROM(";
        strSql = strSql + " SELECT PRODUCT_ID,OPERATION_ID,LOT_GRADE,DEF_CODE,SUM(LOT_T_CNT) AS DFQty";
        strSql = strSql + " FROM #SHBoschYield";
        strSql = strSql + " WHERE LOT_GRADE = 'S'";
        strSql = strSql + " GROUP BY PRODUCT_ID,OPERATION_ID,LOT_GRADE,DEF_CODE";
        strSql = strSql + " )A LEFT JOIN (";
        strSql = strSql + " SELECT PRODUCT_ID,OPERATION_ID,SUM(LOT_T_CNT) AS Qty";
        strSql = strSql + " FROM #SHBoschYield";
        strSql = strSql + " GROUP BY PRODUCT_ID,OPERATION_ID";
        strSql = strSql + " )B ON(A.PRODUCT_ID = B.PRODUCT_ID AND A.OPERATION_ID = B.OPERATION_ID)";
        strSql = strSql + " LEFT JOIN rep.MDCM C ON(A.DEF_CODE=C.DEFECTCD AND C.FABID='B')";
        strSql = strSql + " ORDER BY A.PRODUCT_ID,A.OPERATION_ID,A.LOT_GRADE,A.DFQty DESC;";

        strSql = strSql + " SELECT CONVERT(VARCHAR(10),ReportDate,120) AS ReportDate,PRODUCT_ID,OPERATION_ID";
        strSql = strSql + " ,SUM(LOT_T_CNT) AS Qty";
        strSql = strSql + " ,CASE WHEN SUM(LOT_T_CNT) = 0 THEN 0 ELSE 1.00 * SUM(CASE WHEN LOT_GRADE = 'A' THEN LOT_T_CNT ELSE 0 END) / SUM(LOT_T_CNT) END AS ARate";
        strSql = strSql + " ,CASE WHEN SUM(LOT_T_CNT) = 0 THEN 0 ELSE 1.00 * SUM(CASE WHEN LOT_GRADE = 'S' THEN LOT_T_CNT ELSE 0 END) / SUM(LOT_T_CNT) END AS SRate";
        strSql = strSql + " FROM #SHBoschYield";
        strSql = strSql + " GROUP BY CONVERT(VARCHAR(10),ReportDate,120),PRODUCT_ID,OPERATION_ID";
        strSql = strSql + " ORDER BY CONVERT(VARCHAR(10),ReportDate,120),PRODUCT_ID,OPERATION_ID;";

        strSql = strSql + " SELECT A.ReportDate,A.PRODUCT_ID,A.OPERATION_ID,A.LOT_GRADE,A.DEF_CODE,ISNULL(C.DEFECTDESC,'') AS DEFECTDESC,1.00 * A.DFQty / B.Qty AS DefectRate,A.DFQty AS Qty";
        strSql = strSql + " FROM(";
        strSql = strSql + " SELECT CONVERT(VARCHAR(10),ReportDate,120) AS ReportDate,PRODUCT_ID,OPERATION_ID,LOT_GRADE,DEF_CODE,SUM(LOT_T_CNT) AS DFQty";
        strSql = strSql + " FROM #SHBoschYield";
        strSql = strSql + " WHERE LOT_GRADE = 'S'";
        strSql = strSql + " GROUP BY CONVERT(VARCHAR(10),ReportDate,120),PRODUCT_ID,OPERATION_ID,LOT_GRADE,DEF_CODE";
        strSql = strSql + " )A LEFT JOIN (";
        strSql = strSql + " SELECT CONVERT(VARCHAR(10),ReportDate,120) AS ReportDate,PRODUCT_ID,OPERATION_ID,SUM(LOT_T_CNT) AS Qty";
        strSql = strSql + " FROM #SHBoschYield";
        strSql = strSql + " GROUP BY CONVERT(VARCHAR(10),ReportDate,120),PRODUCT_ID,OPERATION_ID";
        strSql = strSql + " )B ON(A.ReportDate = B.ReportDate AND A.PRODUCT_ID = B.PRODUCT_ID AND A.OPERATION_ID = B.OPERATION_ID)";
        strSql = strSql + " LEFT JOIN rep.MDCM C ON(A.DEF_CODE=C.DEFECTCD AND C.FABID='B')";
        strSql = strSql + " ORDER BY A.ReportDate,A.PRODUCT_ID,A.OPERATION_ID,A.LOT_GRADE,A.DFQty DESC;";
        DataSet ds = dbConn.GetDataSetBySql(strSql);

        GridView1.DataSource = ds.Tables[0].DefaultView;
        GridView1.DataBind();

        GridView2.DataSource = ds.Tables[1].DefaultView;
        GridView2.DataBind();

        GridView3.DataSource = ds.Tables[2].DefaultView;
        GridView3.DataBind();

        GridView4.DataSource = ds.Tables[3].DefaultView;
        GridView4.DataBind();

        if (ds.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查無數據！');</script>");
            return;
        }

    }
    protected void ExportExcel_Click(object sender, EventArgs e)
    {
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
        Response.Output.Write(oStringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        ClearDataBound();
    }
    protected void ClearDataBound()
    {
        GridView1.DataSource = "";
        GridView1.DataBind();

        GridView2.DataSource = "";
        GridView2.DataBind();

        GridView3.DataSource = "";
        GridView3.DataBind();

        GridView4.DataSource = "";
        GridView4.DataBind();
    }
}
