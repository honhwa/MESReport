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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Text;


namespace Report.Reports.PC

{
    public partial class SGradeDfectPanelDQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        string strErrorMsg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }

                this.SetFrProdIDList();
                //this.SetToProdIDList();
                //this.SetStationsList();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
            //this.SetFrProdIDList();

        }

        protected void ButQ_Click1(object sender, EventArgs e)
        {
            /*20130626*/
            this.lblErrMsg.Text = string.Empty;


            if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtToTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtFromTime.Text.Trim().Replace("'", "") != "" && this.txtToTime.Text.Trim().Replace("'", "") != "")
            {
                DateTime fr = new DateTime();
                DateTime to = new DateTime();
                bool bTimeFormatError = false;

                if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "開始時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;

                }

                if (!DateTime.TryParse(this.txtToTime.Text.Trim().Replace("'", ""), out to))
                {
                    strErrorMsg += "結束時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;

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
            }







            /**/
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }



            StringBuilder strFrProdID = new StringBuilder();
            strFrProdID.Remove(0, strFrProdID.Length);       
            if (!this.chkFrAllProdID.Checked)
            {
                
                foreach (ListItem itemFrProdID in this.lstFrProdId.Items)
                {

                    if (itemFrProdID.Selected)
                    {
                        strFrProdID.Append("and prodid = '" + itemFrProdID.Text + "'");
                    }
                }
 
            }
           


            StringBuilder stb = new StringBuilder();
            stb.Remove(0, stb.Length);
            stb.Append("select trackoutdate as TIMESTAMP ,substring(panelid,1,len(panelid)-2) as GLASSID,SO9NCNO AS GLASSPRODUCTID,TRANSID AS DEFECTTIMESTAMP,N9NCNO  AS PRODUCTID,PRODID  AS PRODUCTSPECNAME,CWORKCTR AS OPERATIONNAME,ENTITYID AS LOTID,TRACKOUTDATE AS LOTOUTPUTTIME,PANELID ,'S' AS GRADE,'SS' AS LOTLEVEL,DEFECTCD,'N' AS UPFLAG From New_WPMLM ");
            stb.Append("where fabid ='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
            //stb.Append("and  a.verb = 'crlt' ");
            stb.Append(" and  transid between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "'");
            
            stb.Append(strFrProdID.ToString());
            //stb.Append(strToProdID.ToString());
            //stb.Append(strFrEntityID.ToString());
            //stb.Append(strToEntityID.ToString());
            //stb.Append(strStations.ToString());



            DataTable dt = new DataTable();
            dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];

            GridView1.DataSource = dt;
            GridView1.Visible = true;

            GridView1.PageSize = 20;
            GridView1.ShowFooter = true;
            GridView1.DataBind();

            int count = GridView1.Rows.Count;
            if (count == 0)
            {
                strErrorMsg += "查詢結果為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                GridView1.Visible = false;
                return;
            }

            else
            {
                strErrorMsg += "查詢筆數為：" + count;
                this.lblErrMsg.Text = strErrorMsg;

            }
        }
        private void SetFrProdIDList()//  lstFrProdId
        {
            this.lstFrProdId.Items.Clear();
            string strSql = @"SELECT DISTINCT PRODID FROM mpim   where  fabid = '" + ((UserInfo)Session["UserInfo"]).FabId + "' and prodid is not null and prodid !='' ORDER BY   PRODID ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstFrProdId.DataSource = dt;
            this.lstFrProdId.DataTextField = "PRODID";
            this.lstFrProdId.DataValueField = "PRODID";
            this.lstFrProdId.DataBind();
            this.lstFrProdId.ClearSelection();
        }
        protected void chkFrAllProdID_CheckedChanged(object sender, EventArgs e)  //FrProdID 1
        {
            if (this.chkFrAllProdID.Checked)
            {
                foreach (ListItem i in this.lstFrProdId.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstFrProdId.Items)
                {
                    i.Selected = false;
                }
            }
        }


        protected void ButExcel_Click(object sender, EventArgs e)
        {
            if (GridView1.Visible == false)
            {
                strErrorMsg += "下方表格為空，無法匯出資料！\n";
                this.lblErrMsg.Text = strErrorMsg;

                return;
            }
            else
            {
                ExportExcel();
            }
        }

        protected void ExportExcel()
        {

            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Test" + ".xls");

            Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=BIG5\">");
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.GridView1.RenderControl(oHtmlTextWriter);

            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中   
        }
    }
}
