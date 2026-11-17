using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Limit : System.Web.UI.Page
{
    const string strGroup = "UGC";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ModuleClass mc = new ModuleClass();
            string strNTName = "", strPCIP = "", strPCName = "";
            mc.GetLoginInfo(out strNTName, out strPCIP, out strPCName);

            string strUGC = "";
            if (Request.QueryString["UGC"] != null) { strUGC = this.Request.QueryString["UGC"].ToString(); }

            //strUGC = strGroup + strUGC;

            tbNTName.Text = strNTName;
            tbPCName.Text = strPCIP + "(" + strPCName + ")";
            tbUGC.Text = strUGC;

            if (mc.GetRole(strGroup + strUGC, tbNTName.Text) == false)
            {
                Main.Visible = false;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('無權限操作！');</script>");
                return;
            }

            //ExportExcel.HRef = "../ExcelFormat/MUCM.xls";
        }
    }
    protected void QueryDataMaterial()
    {
        DBConn dbConn = new DBConn();
        string strSql = "";

        strSql = strSql + " SELECT DISTINCT r.USERID,r.USERGROUPCD,MU.USERGROUPDESC";
        strSql = strSql + " FROM dbo.MUGMM_Sc AS r WITH(NOLOCK) ";
        strSql = strSql + " LEFT JOIN dbo.MUGM_Sc MU ON MU.USERGROUPCD=r.USERGROUPCD ";
        strSql = strSql + " WHERE  r.USERGROUPCD='" + tbUGC.Text + "'";
        if (tbMaterial.Text!="")
        {
            strSql = strSql + " AND r.USERID = '" + tbMaterial.Text + "'";
        }

        DataSet dsTmp = dbConn.GetDataSetBySql(strSql);
        gvMaterial.DataSource = dsTmp.Tables[0];
        gvMaterial.DataBind();
        if (dsTmp.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('查詢無數據！');</script>");
        }
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        
        //gvMaterial.Visible = true;
        QueryDataMaterial();

    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        }
    }
    protected void btExport_Click(object sender, EventArgs e)
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
        this.gvMaterial.RenderControl(oHtmlTextWriter);
        Response.Output.Write(oStringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btUPLoad_Click(object sender, EventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        try
        {
            //if (FileUpload1.HasFile == false)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇上傳檔案！');</script>");
            //    return;
            //}
            //string strServerPath = "MUCM" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            //if (!System.IO.Directory.Exists(Server.MapPath(@"~/ExcelUpload/")))
            //{
            //    System.IO.Directory.CreateDirectory(Server.MapPath(@"~/ExcelUpload/"));
            //}
            //FileUpload1.SaveAs(Server.MapPath(@"~/ExcelUpload/" + strServerPath));
            //string strPath = Server.MapPath(@"~/ExcelUpload/" + strServerPath);
            //int intErrCount = 0;

            DBConn dbConn = new DBConn();
            //DataSet dsReadExcel = mc.ExcelRead(strPath, "Sheet1$");     /*讀取EXCEL表并返回結果集*/
            //if (dsReadExcel != null)
            //{
                //DataTable dtReadExcel = dsReadExcel.Tables[0];     /*讀取EXCEL表并返回結果集*/
                //int intRowCnt = dtReadExcel.Rows.Count;
                //int intColCnt = dtReadExcel.Columns.Count;
                //if (intRowCnt > 0)
                //{
            if (tbMaterial.Text != "")
            {
                string strSql = "";
                //for (int i = 0; i < intRowCnt; i++)
                //{
                //    int intYear = Convert.ToInt32(dtReadExcel.Rows[i][0].ToString().Trim());
                //    int intMonth = Convert.ToInt32(dtReadExcel.Rows[i][1].ToString().Trim());
                //    string strMaterial = dtReadExcel.Rows[i][2].ToString().Trim();
                //    double dblCost = Convert.ToDouble(dtReadExcel.Rows[i][3].ToString().Trim());
                //    string strCurrency = dtReadExcel.Rows[i][4].ToString().Trim();
                //    strSql = "";
                //    //if (dblCost == 0)
                //    //{
                //    //    strSql = strSql + " DELETE FROM dbo.MUCM";
                //    //    strSql = strSql + " WHERE [Year] = " + intYear + " AND [Month] = " + intMonth + " AND Material = '" + strMaterial + "' ";
                //    //}
                //    //else
                //    //{
                strSql = strSql + " SELECT FABID FROM dbo.MUGMM_Sc";
                strSql = strSql + " WHERE USERID = '" + tbMaterial.Text + "' AND USERGROUPCD = '" + tbUGC.Text + "'";
                strSql = strSql + " IF @@ROWCOUNT = 0";
                strSql = strSql + " BEGIN";
                strSql = strSql + " INSERT INTO dbo.MUGMM_Sc";
                strSql = strSql + " (FABID ,";
                strSql = strSql + " USERID ,";
                strSql = strSql + " USERGROUPCD ,";
                strSql = strSql + " UPDATER ,";
                strSql = strSql + " TIME_STAMP";
                strSql = strSql + " )";
                strSql = strSql + " VALUES  ( 'B'";
                strSql = strSql + " ,'" + tbMaterial.Text + "'";
                strSql = strSql + " ,'" + tbUGC.Text + "' ";
                strSql = strSql + " ,'" + tbNTName.Text + "' ";
                strSql = strSql + " ,GETDATE() ";
                strSql = strSql + " )";
                strSql = strSql + " END";
                //}
                string strErrDesc = "";
                int intErrCode = 0;
                if (dbConn.ExecuteSql(strSql, out strErrDesc, out intErrCode) == false)
                {
                    //intErrCount++;
                    mc.SaveLog(ModuleClass.CstrLogType, intErrCode.ToString() + strErrDesc);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('用戶名:" + tbMaterial.Text + "開通權限OK！');</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('輸入需要開通的用戶名！');</script>");
            }
                    //    mc.SaveLog(ModuleClass.CstrLogType, "上傳檔案名稱：" + strPath + "; 總筆數" + intRowCnt.ToString() + "; 失敗筆數:" + intErrCount.ToString());
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('已上傳完畢！共" + intRowCnt.ToString() + "筆，失敗" + intErrCount.ToString() + "筆');</script>");
                    //}
                    //else
                    //{
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請確認上傳數據的格式是否正確！');</script>");
                    //    return;
                    //}
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('無可上傳的數據！');</script>");
                //    return;
                //}
            //}
        }
        catch (Exception ex)
        {
            mc.SaveLog(ModuleClass.CstrErrorLogType, ex.ToString());
        }
    }
    protected void gvMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
        }
    }
}