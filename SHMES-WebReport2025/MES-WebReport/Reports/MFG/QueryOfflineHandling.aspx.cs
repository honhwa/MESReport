using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Report.Library;

namespace Report.Reports.MFG
{
    public partial class QueryOfflineHandling : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        private int MAX_SQL_ROWS = 5000;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataSet ds = findKeywordByType("OfflineHandling");
                if (ds.Tables[0].Rows.Count > 0)
                {                    
                    this.ddlReason.DataTextField = "description";
                    this.ddlReason.DataValueField = "description";
                    this.ddlReason.DataSource = ds;
                    this.ddlReason.DataBind();
                    this.ddlReason.Items.Insert(0, "全部");

                }

                ds = getLines();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.ddlSubline.DataTextField = "LINEID";
                    this.ddlSubline.DataValueField = "LINEID";
                    this.ddlSubline.DataSource = ds;
                    this.ddlSubline.DataBind();
                    this.ddlSubline.Items.Insert(0, "全部");
                }

                this.tbDateFrom.Text = DateTime.Now.AddDays(-7).ToString("yyyyMMdd");
                this.tbDateTo.Text = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            }
        }

        private bool ValidateInput(ref string strMsg)
        {
            bool result = true;
            if (this.tbDateFrom.Text == "" || this.tbDateTo.Text == "")
            {
                strMsg = "請輸入日期區間";
                return false;
            }
            return result;
        }

        private void ResetUI()
        {
            this.lbMsg.Text = "";
            this.gvResult.DataSource = null;
            this.gvResult.DataBind();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            ResetUI();
            string strMsg = "";            
            if (!ValidateInput(ref strMsg))
            {
                this.lbMsg.Text = strMsg;
                return;
            }

            string sql;
            sql = $@"select top {MAX_SQL_ROWS} * from [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[OfflineManualSampling] 
                     where timestamp between '{this.tbDateFrom.Text}' and '{this.tbDateTo.Text}' ";

            if (this.ddlSubline.SelectedIndex != 0)
            {                
                sql += $@" and SublineID = '{this.ddlSubline.SelectedValue}' ";
            }
            if (this.ddlReason.SelectedIndex != 0)
            {
                sql += $@" and Reason = '{this.ddlReason.SelectedValue}' ";
            }
            if (this.tbPanelID.Text.Trim() != "")
            {
                sql += $@" and panelid='{this.tbPanelID.Text.Trim()}'";
            }
            if (this.tbUserID.Text.Trim() != "")
            {
                sql += $@" and userid='{this.tbUserID.Text.Trim()}'";
            }


            sql += " order by timestamp asc";

            DataSet ds = this.dbConn.GetDataSetBySql(sql);
            this.gvResult.DataSource = ds;
            this.gvResult.DataBind();

            this.lbMsg.Text = $@"共{ds.Tables[0].Rows.Count.ToString()}筆";

            if (ds.Tables[0].Rows.Count == MAX_SQL_ROWS)
            {
                if(!(this.lbMsg.Text == ""))
                {
                    this.lbMsg.Text += " , ";
                }

                this.lbMsg.Text += "最大查詢5000筆資料, 請修改查詢條件";
            }
        }

        private DataSet findKeywordByType(string type, bool includeDisable = false)
        {
            string sql;
            sql = $@"select * from [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[mes_keyword] where type='{type}' ";
            if (!includeDisable)
            {
                sql += " and flag is null";
            }
            sql += " order by code ";
            DataSet ds = this.dbConn.GetDataSetBySql(sql);
            return ds;
        }

        private DataSet getLines()
        {
            string sql;
            sql = $@"SELECT LINEID FROM MLINEM 
                     WHERE FABID = 'B' AND LINEID IS NOT NULL 
                     and lineid not like '%-RW%' and lineid not like '%-ME%' and lineid not like '%-PE%' 
                     and (lineid like '%POL%' or lineid like '%COG%')                      
                     ORDER BY LINEID";
            DataSet ds = this.dbConn.GetDataSetBySql(sql);
            return ds;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (this.gvResult.Rows.Count == 0 || this.gvResult.Visible == false)
            {
                this.Response.Write("<script>alert('無數據可導出！')</script>");
            }
            else
            {
                DataTable table = new DataTable();
                table = ExcelIO.ExcelHelper.GridView2DataTable(this.gvResult);
                string filename = $@"OfflineHandling_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls";
                ExcelIO.ExcelHelper.ExportByWeb(table, "", filename);
            }
        }
    }
}