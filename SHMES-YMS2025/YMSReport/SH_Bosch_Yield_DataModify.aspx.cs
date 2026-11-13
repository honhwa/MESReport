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

public partial class YMSReport_SH_Bosch_Yield_DataModify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtDate.Text = System.DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd");
            txtDate.Attributes.Add("readonly", "readonly");

            tbQueryDate.Text = System.DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd");
            tbQueryDate.Attributes.Add("readonly", "readonly");
            
            lblCmt.Text = "新增";
            Refresh();
        }
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        QueryData();
    }
    protected void QueryData()
    {
        DBConn dbConn = new DBConn("B");
        string strSql = "";
        strSql = strSql + " SELECT CONVERT(VARCHAR(30),TransDate,120) AS TransDate,CONVERT(VARCHAR(10),ReportDate,120) AS ReportDate";
        strSql = strSql + " ,FAB_ID,PRODUCT_ID,OPERATION_ID,LOT_GRADE,DEF_CODE,LOT_T_CNT FROM dbo.SH_Bosch_Yield";
        strSql = strSql + " WHERE ReportDate = '" + tbQueryDate.Text + "'";
        strSql = strSql + " ORDER BY ReportDate,PRODUCT_ID,OPERATION_ID,LOT_GRADE,DEF_CODE";
        DataSet ds = dbConn.GetDataSetBySql(strSql);

        GridView1.DataSource = ds.Tables[0].DefaultView;
        GridView1.DataBind();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        lblCmt.Text = "修改";
        txtDate.Text = GridView1.Rows[e.NewSelectedIndex].Cells[1].Text;
        tbProductID.Text = GridView1.Rows[e.NewSelectedIndex].Cells[2].Text;
        ddlStation.SelectedValue = GridView1.Rows[e.NewSelectedIndex].Cells[3].Text;
        ddlGrade.SelectedValue = GridView1.Rows[e.NewSelectedIndex].Cells[4].Text;
        tbDefectCode.Text = GridView1.Rows[e.NewSelectedIndex].Cells[5].Text.Replace("&nbsp;", "");
        tbQty.Text = GridView1.Rows[e.NewSelectedIndex].Cells[6].Text.ToString().Replace(".0000000","");

        txtDate.Enabled = false;
        tbProductID.Enabled = false;
        ddlStation.Enabled = false;
        ddlGrade.Enabled = false;
        tbDefectCode.Enabled = false;

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    protected void Refresh()
    {
        txtDate.Enabled = true;
        tbProductID.Enabled = true;
        ddlStation.Enabled = true;
        ddlGrade.Enabled = true;
        tbDefectCode.Enabled = true;

        tbProductID.Text = "DJ103FA-01A";
        tbDefectCode.Text = "";
        tbQty.Text = "";

        lblCmt.Text = "新增";

        GridView1.DataSource = "";
        GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (tbProductID.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請輸入產品號碼！');</script>");
            return;
        }
        if (tbQty.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請輸入數量！');</script>");
            return;
        }

        DBConn dbConn = new DBConn("B");
        string strSql = "";
        if (lblCmt.Text == "新增")
        {
            strSql = strSql + " INSERT INTO dbo.SH_Bosch_Yield( TransDate ,ReportDate ,FAB_ID ,PRODUCT_ID ,OPERATION_ID ,LOT_GRADE ,DEF_CODE ,LOT_T_CNT)";
            strSql = strSql + " VALUES  ( GETDATE(),'" + txtDate.Text + "','B','" + tbProductID.Text + "','" + ddlStation.SelectedValue + "','" + ddlGrade.SelectedValue + "','" + tbDefectCode.Text + "'," + tbQty.Text + ")";
        }
        else
        {
            strSql = strSql + " UPDATE SH_Bosch_Yield SET LOT_T_CNT = " + tbQty.Text + ", TransDate = GETDATE() ";
            strSql = strSql + " WHERE ReportDate = '" + txtDate.Text + "' ";
            strSql = strSql + " AND FAB_ID = 'B' ";
            strSql = strSql + " AND PRODUCT_ID = '" + tbProductID.Text + "' ";
            strSql = strSql + " AND OPERATION_ID = '" + ddlStation.SelectedValue + "' ";
            strSql = strSql + " AND LOT_GRADE = '" + ddlGrade.SelectedValue + "' ";
            strSql = strSql + " AND DEF_CODE = '" + tbDefectCode.Text.Replace("&nbsp;", "") + "' ";
        }
        if (dbConn.ExecuteSql(strSql) == false)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('更新失敗，請確認數據！');</script>");
            return;
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('更新成功！');</script>");
            QueryData();
            Refresh();
        }
    }
}
