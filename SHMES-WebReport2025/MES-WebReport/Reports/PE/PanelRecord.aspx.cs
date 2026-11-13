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
using Report.Library;

namespace Report.Reports.PE
{
    public partial class PanelRecord : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetProductID();
            }
        }

        private void GetProductID()
        {
            string strProdid = " SELECT PRODID,PROJECT_NAME FROM rep.MPIM ORDER BY PRODID ";
            DataSet dsProdid = new DataSet();
            dsProdid = dbConn.GetDataSetBySql(strProdid);
            chklProdid.DataSource = dsProdid;
            chklProdid.DataTextField = "PRODID";
            chklProdid.DataValueField = "PROJECT_NAME";
            chklProdid.DataBind();
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                foreach(ListItem item in chklProdid.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                chklProdid.ClearSelection();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            Panel1.Visible = false;
            string strCondition = GetQueryCondition();
            string strSQL = "";
            if (strCondition == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('查詢條件不可為空！');</script>");
                return;
            }
            else
            {
                strSQL += " SELECT ENTITYID,PANELID,PWORKCTR,CWORKCTR,DEFECTCD,PRODID,N9NCNO,PSUBLINEID,SUBLINEID,WONBR,LOTTYPE,TRANSID,LOTGRADE,LOTLEVEL,PSHOPCD,CSHOPCD,SOPRODID,B.USERID,B.USERNAME FROM New_WPMLM A ,MEMM B WHERE A.USERID=B.USERNAME  AND A.FABID='B' ";
                strSQL += strCondition;
            }
            try
            {
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                DataSet dsPanelInfo = new DataSet();
                dsPanelInfo = dbConn.GetDataSetBySql(strSQL);
                if (dsPanelInfo.Tables[0].Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('未查詢到相關Panel Record信息！');</script>");
                }
                else
                {
                    GridView1.DataSource = dsPanelInfo;
                    GridView1.DataBind();
                    Panel1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.ToString();
            }
        }

        private string GetQueryCondition()
        {
            string strCondition = "";
            if (txtLotID.Text.Trim() != "")//獲取LOTID
            {
                strCondition += " AND A.ENTITYID LIKE ('" + txtLotID.Text + "%') ";
            }
            if (txtPanelID.Text.Trim() != "")//獲取PanelID
            {
                strCondition += " AND A.PANELID='" + txtPanelID.Text + "' ";
            }
            string strProdid = "";
            foreach (ListItem item in chklProdid.Items)//獲取Prodid
            {
                if (item.Selected)
                {
                    strProdid += "'" + item.Text + "',";
                }
            }
            if (strProdid != "")
            {
                strProdid = strProdid.Substring(0, strProdid.Length - 1);
                strCondition += " AND A.PRODID IN (" + strProdid + ")";
            }
            return strCondition;
        }

        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            ToExcel(GridView1, "LOT/PanelRecord");
        }

        private static void ToExcel(System.Web.UI.Control ctl, string FileName)
        {
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + "" + FileName + ".xls");
            ctl.Page.EnableViewState = false;
            System.IO.StringWriter tw = new System.IO.StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            ctl.RenderControl(hw);
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();
        }
        //導出EXCEL時必须要有下面这句！否则不会通过！

        public override void VerifyRenderingInServerForm(Control control)
        { 
            // Confirms that an HtmlForm control is rendered for        
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            Panel1.Visible = false;
            txtLotID.Text = "";
            txtPanelID.Text = "";
            chkAll.Checked = false;
            chklProdid.ClearSelection();
        }

    }
}
