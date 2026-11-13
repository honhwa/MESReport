/*2016/11/10      1.01   JEFF   ADD  */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Report.Library;

namespace Report.Reports.MFG
{
    public partial class OperationIDQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.txtFromTime.Text = System.DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.SetProdIdList();
                this.SetSubLineIdList();
            }
        }

        private void SetProdIdList()
        {
            this.lstProdId.Items.Clear();
            string strSql = @"SELECT PRODID FROM MPIM WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PRODID ORDER BY PRODID";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstProdId.DataSource = dt;
            this.lstProdId.DataTextField = "PRODID";
            this.lstProdId.DataValueField = "PRODID";
            this.lstProdId.DataBind();
            this.lstProdId.ClearSelection();
        }

        private void SetSubLineIdList()
        {
            this.lstSubLineID.Items.Clear();
            string strSql = @"SELECT SUBLINEID FROM MSLM WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY SUBLINEID ORDER BY SUBLINEID";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstSubLineID.DataSource = dt;
            this.lstSubLineID.DataTextField = "SUBLINEID";
            this.lstSubLineID.DataValueField = "SUBLINEID";
            this.lstSubLineID.DataBind();
            this.lstSubLineID.ClearSelection();
        }

        protected void chkAllProdID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllProdID.Checked)
            {
                foreach (ListItem i in this.lstProdId.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstProdId.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void chkAllSubLineID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllSubLineID.Checked)
            {
                foreach (ListItem i in this.lstSubLineID.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstSubLineID.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void lstProdId_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstProdId.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllProdID.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllProdID.Checked = true;
            }
        }

        protected void lstSubLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstSubLineID.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllSubLineID.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllSubLineID.Checked = true;
            }
        }

        protected void btnSerch_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            BindData();
        }
        private void BindData()
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;
            string strProdID = "";
            string strSubLineID = "";
            if (this.txtFromTime.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的開始日期！');</script>");
                return;
            }
            if (this.txtToTime.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇您需要的結束日期！');</script>");
                return;
            }

            DateTime fr = new DateTime();
            DateTime to = new DateTime();
            if (this.txtFromTime.Text.Trim().Replace("'", "") != "" && this.txtToTime.Text.Trim().Replace("'", "") != "")
            {
                bool bTimeFormatError = false;

                if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "開始時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!DateTime.TryParse(this.txtToTime.Text.Trim().Replace("'", ""), out to))
                {
                    strErrorMsg += "結束時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!bTimeFormatError)
                {
                    TimeSpan s = to - fr;
                    if (fr > to)
                    {
                        strErrorMsg += "結束時間在開始時間之前！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }
                }

                //if (to.Subtract(fr).Days > 365)
                //{
                //    strErrorMsg += "開始時間結束時間跨度不能大於365天!\n";
                //    this.lblErrMsg.Text = strErrorMsg;
                //    return;
                //}
            }

            if (lstProdId.Items.Count > 0)
            {

                for (int i = 0; i < lstProdId.Items.Count; i++)
                {
                    if (((CheckBoxList)lstProdId).Items[i].Selected == true)
                    {

                        strProdID = strProdID + "'" + ((CheckBoxList)lstProdId).Items[i].Value + "'" + ",";
                    }

                }
                if (strProdID != "")
                {
                    strProdID = strProdID.Substring(0, strProdID.Length - 1);
                }
            }

            if (lstSubLineID.Items.Count > 0)
            {

                for (int i = 0; i < lstSubLineID.Items.Count; i++)
                {
                    if (((CheckBoxList)lstSubLineID).Items[i].Selected == true)
                    {

                        strSubLineID = strSubLineID + "'" + ((CheckBoxList)lstSubLineID).Items[i].Value + "'" + ",";
                    }

                }
                if (strSubLineID != "")
                {
                    strSubLineID = strSubLineID.Substring(0, strSubLineID.Length - 1);
                }
            }

            //stb.AppendFormat(" AND D." + strTable2 + " IN ('{0}')", strPANELID);

            //StringBuilder stb = new StringBuilder();

            string strPANELID = txtpld.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");

            if (string.IsNullOrEmpty(txtpld.Text) && !(this.chkAllProdID.Checked == false && strProdID != "") && !(this.chkAllSubLineID.Checked == false && strSubLineID != ""))
            {
                if (to.Subtract(fr).Days > 7)
                {
                    strErrorMsg += "查詢條件ProdId/SubLineid/PanelId都為空時,查詢開始|結束時間跨度不能大於7天!\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                }
            }

            string strWhereSql = string.Empty;
            if (!string.IsNullOrEmpty(txtpld.Text))
                strWhereSql += $" AND  ENTITYID IN ('{strPANELID}')";
            if (this.chkAllProdID.Checked == false && strProdID != "")
                strWhereSql += $" AND PRODID IN ({strProdID})";
            if (this.chkAllSubLineID.Checked == false && strSubLineID != "")
                strWhereSql += $" AND SUBLINEID IN ({strSubLineID})";

            string sql = $@"SELECT FABID,
		ENTITYID,
		SubLineID,
		PRODID,
		OperationID,
		WORKCTR,
		TRANSDATE,
		USERID 
	FROM (  SELECT * FROM New_WEQPARAM WITH(NOLOCK)
			WHERE FabId = 'B'
				AND TRANSDATE BETWEEN '{this.txtFromTime.Text.Trim()}' AND '{this.txtToTime.Text.Trim()}'
				{strWhereSql}
			UNION ALL
		    SELECT * FROM WEQPARAMH WITH(NOLOCK)
			WHERE FabId = 'B'
				AND TRANSDATE BETWEEN '{this.txtFromTime.Text.Trim()}' AND '{this.txtToTime.Text.Trim()}'
				{strWhereSql}
		) A
ORDER BY A.ENTITYID,A.TRANSDATE ASC";

            DataTable dt1 = this.dbConn.GetDataSetBySql(sql).Tables[0];
            Session["dsData"] = dt1;
            GridView1.DataSource = dt1;
            GridView1.DataBind();
            this.lblErrMsg.Text = "查詢成功!記錄數：" + dt1.Rows.Count + "條";
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "GB2312";
            ////显示标头
            //Response.AddHeader("Content-Disposition", "attachment;   filename=" + System.Web.HttpUtility.UrlEncode("OperationIDQuery", System.Text.Encoding.UTF8) + ".xls");//这样的话，可以设置文件名为中文，且文件名不会乱码。其实就是将汉字转换成UTF8
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //// 如果设置为 GetEncoding("GB2312");导出的文件将会出现乱码！

            //Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。     
            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            //HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //GridView1.RenderControl(htmlWrite);
            //Response.Write(stringWrite.ToString());
            //Response.End();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            GridView1.AllowPaging = false;
            BindGridView();
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < GridView1.Columns.Count; k++)
            {
                //add separator
                sb.Append(GridView1.Columns[k].HeaderText + ',');
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                for (int k = 0; k < GridView1.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(GridView1.Rows[i].Cells[k].Text.ToString() + ',');
                }
                //append new line
                sb.Append("\r\n");
            }
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            GridView1.AllowPaging = true;
        }

        private void BindGridView()
        {
            GridView1.DataSource = (DataTable)Session["dsData"];
            GridView1.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
