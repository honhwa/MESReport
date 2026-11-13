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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Report.Reports.MFG
{
    public partial class Burn_In_RecordQuery : System.Web.UI.Page
    {

        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                txtStartDT.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                TxtEndDT.Text = DateTime.Now.ToShortDateString();
                InitProdid_Wonbr();
                Session.Timeout = 60;//設定Session保存的時間

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //彈出選擇存儲路徑的對話框
                Response.Clear();
                Response.Buffer = false;
                Response.Charset = "UTF-8";
                Response.ContentType = "Content-Language;content=UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=Burn In Query.xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/vnd.ms-excel";
                this.EnableViewState = false;
                StringWriter stringWriter = new StringWriter();//一个用于将信息写入字符串的 TextWriter。该信息存储在基础 StringBuilder 中。 
                HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                //HtmlTextWriter的构造函数要求必须传递一个与TextWriter接口兼容的实例。

                //可以根据需要选择合适的TextWriter接口实现类，因为该类的选择将决定最终生成的Html文本的输出方式

                DataGrid excel = new DataGrid();
                //Excel中的顯示樣式設置
                //TableItemStyle AlternatingStyle = new TableItemStyle();
                TableItemStyle headerStyle = new TableItemStyle();
                TableItemStyle itemStyle = new TableItemStyle();
                //AlternatingStyle.BackColor = System.Drawing.Color.LightGray;
                headerStyle.BackColor = System.Drawing.Color.LightGray;
                headerStyle.Font.Bold = true;
                headerStyle.HorizontalAlign = HorizontalAlign.Center;
                itemStyle.HorizontalAlign = HorizontalAlign.Center;

                //excel.AlternatingItemStyle.MergeWith(AlternatingStyle);//轮流的;交替的;间隔的

                excel.HeaderStyle.MergeWith(headerStyle);
                excel.ItemStyle.MergeWith(itemStyle);
                excel.GridLines = GridLines.Both;
                excel.HeaderStyle.Font.Bold = true;

                excel.DataSource = ((DataTable)Session["dsData"]).DefaultView;//输出DataTable的内容

                excel.DataBind();
                excel.RenderControl(htmlWriter);//将服务器控件的内容输出到所提供的 HtmlTextWriter 对象中


                //使用Response輸出
                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            catch
            {
                if (Session["UserInfo"] == null)
                {
                    Response.Write("<script language='JavaScript'>window.alert('登錄超時！');window.location.href='../../Login.aspx'</script>");
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //記錄LOG
                RecordLog Log = new RecordLog();

                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }

                lblERROR.Visible = false;

                getFilterCondition();
                //先在表WSTORD中查詢是否為By Lot過賬
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" SELECT CONVERT(VARCHAR(20),TRANINDATE,120 ) AS TRANINDATE ,CONVERT(VARCHAR(20),TRANOUTDATE,120 ) AS TRANOUTDATE,PanelID,ENTITYID,WORKCTR,QTY,A.PRODID,B.PRODGROUP,WONBR,STORAGEID,SUBLINEID,DEFECTCD,USERID \n");
                strSql.Append(" FROM New_WSTORD A,rep.MPIM B \n");
                strSql.Append(" WHERE A.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
                strSql.Append(" AND A.PRODID=B.PRODID \n");
                if (strFrTime == "" || strToTime == "")
                {
                    Response.Write("<script language='JavaScript'>window.alert('時間為必選項！')</script>");
                    txtStartDT.Focus();
                    return;//跳出
                }
                else
                {
                    strSql.Append(" AND TRANINDATE>='" + strFrTime + "' AND TRANINDATE<='" + strToTime + "' \n");
                }
                if (listProdid.Count > 0)
                {
                    strSql.Append(" AND A.PRODID IN('-'");
                    for (int i = 0; i < listProdid.Count; i++)
                    {
                        strSql.Append(",'" + listProdid[i] + "'");
                    }
                    strSql.Append(") \n");
                }
                if (listWonbr.Count > 0)
                {
                    strSql.Append(" AND WONBR IN('-'");
                    for (int i = 0; i < listWonbr.Count; i++)
                    {
                        strSql.Append(",'" + listWonbr[i] + "'");
                    }
                    strSql.Append(") \n");
                }
                if (strPnlID != "")
                {
                    strSql.Append(" AND (PanelID='" + strPnlID + "' OR ENTITYID ='" + strPnlID + "')");
                }
                if (chkY.Checked || chkN.Checked)
                {
                    strSql.Append(" AND (DEFECTCD " + (chkY.Checked ? "IS NULL" : "IS NOT NULL") + " \n");
                    strSql.Append((chkY.Checked ? "OR" : "AND") + " DEFECTCD " + (chkY.Checked ? "=''" : "<>''") + ") \n");
                }
                strSql.Append(" ORDER BY ENTITYID,PanelID,TRANINDATE ");

                DataSet dsWSTORD = new DataSet();
                dsWSTORD = dbConn.GetDataSetBySql(strSql.ToString());
                //如果在WSTORD表中無記錄，則是以By Piece過賬的，需要查WLTX
                if (dsWSTORD.Tables[0].Rows.Count==0 || txtPanelID.Text=="" )
                {
                    strSql.Remove(0, strSql.Length);
                    strSql.Append(" SELECT MAX(TRANINDATE) AS TRANINDATE,MAX(TRANOUTDATE) AS TRANOUTDATE, PanelID,ENTITYID,WORKCTR,QTY,PRODID,PRODGROUP,WONBR,MAX(STORAGEID) STORAGEID,MAX(SUBLINEID) SUBLINEID,MAX(DEFECTCD) DEFECTCD,MAX(USERID) USERID FROM ( \n");
                    strSql.Append(" SELECT CASE VERB WHEN 'MVOU' THEN CONVERT(VARCHAR(20),TRACKINDATE,120 ) ELSE '' END AS TRANINDATE,CASE WHEN verb='MVOU' AND DEFECTCD<>'' THEN CONVERT(VARCHAR(20),TRANSDATE,120 ) ELSE CASE VERB WHEN 'MVIN' THEN CONVERT(VARCHAR(20),PTRACKOUTDATE,120 ) ELSE '' END END AS TRANOUTDATE,TOENTITYID AS PanelID,SOLOTID AS ENTITYID,'8351' WORKCTR,STORAGEID,CSUBLINEID SUBLINEID,CENTITYTQTY AS QTY,A.TOPRODID AS PRODID,B.PRODGROUP,TOWONBR AS WONBR,DEFECTCD,USERID \n");
                    strSql.Append(" FROM New_VWLTXA A,rep.MPIM B \n");
                    strSql.Append(" WHERE A.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
                    strSql.Append(" AND A.TOPRODID=B.PRODID AND A.CENTITYTQTY='1' \n");
                    strSql.Append(" AND ((CWORKCTR='8351' AND VERB='MVOU') OR (CWORKCTR='8352' AND VERB='MVIN' AND STORAGEID<>'')) \n");
                    strSql.Append(" AND CSUBLINEID LIKE '%F-OVEN%' \n");
                    strSql.Append(" AND TRANSDATE>='" + strFrTime + "' AND TRANSDATE<='" + strToTime + "' \n");

                    if (listProdid.Count > 0)
                    {
                        strSql.Append(" AND A.TOPRODID IN('-'");
                        for (int i = 0; i < listProdid.Count; i++)
                        {
                            strSql.Append(",'" + listProdid[i] + "'");
                        }
                        strSql.Append(") \n");
                    }
                    if (listWonbr.Count > 0)
                    {
                        strSql.Append(" AND A.TOWONBR IN('-'");
                        for (int i = 0; i < listWonbr.Count; i++)
                        {
                            strSql.Append(",'" + listWonbr[i] + "'");
                        }
                        strSql.Append(") \n");
                    }
                    if (strPnlID != "")
                    {
                        strSql.Append(" AND (FRENTITYID='" + strPnlID + "' OR FRENTITYID ='" + strPnlID + "')");
                    }

                    if (chkY.Checked != chkN.Checked)
                    {
                        strSql.Append(" AND (DEFECTCD " + (chkY.Checked ? "IS NULL" : "IS NOT NULL") + " \n");
                        strSql.Append((chkY.Checked ? "OR" : "AND") + " DEFECTCD " + (chkY.Checked ? "=''" : "<>''") + ") \n");
                    }
                    strSql.Append(" ) T GROUP BY PanelID,ENTITYID,WORKCTR,QTY,PRODID,PRODGROUP,WONBR \n");
                    strSql.Append(" ORDER BY ENTITYID,PanelID,TRANINDATE ");
                    DataSet dsWltx = new DataSet();
                    dsWltx = dbConn.GetDataSetBySql(strSql.ToString());
                    dsWSTORD.Merge(dsWltx);//合併兩次查詢結果
                }
                if (dsWSTORD.Tables[0].Rows.Count > 0)
                {
                    Session["dsData"] = dsWSTORD.Tables[0];
                    GridView1.DataSource = dsWSTORD.Tables[0];
                    GridView1.DataBind();
                }
                else
                {
                    Response.Write("<script language='javascript'> window.alert('未查到相關的Burn In的記錄！');</script>");
                }
            }
            catch (Exception ex)
            {
                lblERROR.Text = ex.ToString();
                lblERROR.Visible = true;
                if (Session["UserInfo"] == null)
                {
                    Response.Write("<script language='JavaScript'>window.alert('登錄超時！');window.location.href='../../Login.aspx'</script>");
                }
            }
        }
        string strFrTime = "", strToTime = "", strPnlID = "";
        List<string> listProdid = null, listWonbr = null;
        /// <summary>
        /// 獲取所有篩選條件

        /// </summary>
        private void getFilterCondition()
        {
            if (txtStartDT.Text != "")
            {
                strFrTime = Convert.ToDateTime(txtStartDT.Text).ToShortDateString();
            }
            if (TxtEndDT.Text != "")
            {
                strToTime = Convert.ToDateTime(TxtEndDT.Text).AddDays(1).ToShortDateString();
            }

            listProdid = new List<string>();
            foreach (ListItem item in chklProdid.Items)
            {
                if (item.Selected)
                {
                    listProdid.Add(item.Text);
                }
            }

            listWonbr = new List<string>();
            foreach (ListItem item in chklWonbr.Items)
            {
                if (item.Selected)
                {
                    listWonbr.Add(item.Text);
                }
            }

            if (txtPanelID.Text != "")
            {
                strPnlID = txtPanelID.Text;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //ddlProdid.SelectedIndex = 0;
            chklProdid.ClearSelection();
            //ddlWonbr.SelectedIndex = 0;
            chklWonbr.ClearSelection();
            txtPanelID.Text = "";
            chkY.Checked = false;
            chkN.Checked = false;
            lblERROR.Visible = false;
        }
        
        private void InitProdid_Wonbr()
        {
            if (chklProdid.Items.Count == 0)
            {
                string strSql_prodid = " SELECT PRODID FROM rep.MPIM ORDER BY PRODID  ";
                DataSet dsProdid = new DataSet();
                dsProdid = dbConn.GetDataSetBySql(strSql_prodid);
                chklProdid.DataSource = dsProdid.Tables[0];
                chklProdid.DataTextField = "PRODID";
                chklProdid.DataBind();
            }
            if (chklWonbr.Items.Count == 0)
            {
                string strSql_Wonbr = " SELECT WONBR FROM New_SWOM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY WONBR  ";
                DataSet dsWonbr = new DataSet();
                dsWonbr = dbConn.GetDataSetBySql(strSql_Wonbr);
                chklWonbr.DataSource = dsWonbr.Tables[0];
                chklWonbr.DataTextField = "WONBR";
                chklWonbr.DataBind();
                ViewState["isWonbrLoad"] = true;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }
    }
}
