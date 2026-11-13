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
namespace Report.Reports.IE
{
    public partial class TactTime : System.Web.UI.Page
    {
        DBConn dbconn = new DBConn();
        static DataSet dsquery;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) //首次loading page
            {
                bindcon();
            }
        }
        private void bindcon()// 綁定控件
        {
            try
            {
                string strsql;

                strsql = "";
                strsql = "select distinct plant ,process from mtactm;";
                strsql = strsql + " select distinct sublineid from mslm;select distinct project_name as prodid from mpim";

                dsquery = dbconn.GetDataSetBySql(strsql);
                if (dsquery == null)
                {
                    this.lblerror.Text = "綁定控件異常!!";
                }
                else
                {
                    this.lblerror.Text = "";
                    DataTable dstable = new DataTable();
                    dstable = dsquery.Tables[0].DefaultView.ToTable(true, "plant");
                    ddlfactory.DataValueField = "plant";
                    ddlfactory.DataTextField = "plant";
                    ddlfactory.DataSource = dstable.DefaultView;
                    ddlfactory.DataBind();
                    ddlfactory.Items.Insert(0, new ListItem("", ""));
                    ddlfactory.SelectedValue = "";

                    dstable = dsquery.Tables[0].DefaultView.ToTable(true, "process");
                    ddlpro.DataValueField = "process";
                    ddlpro.DataTextField = "process";
                    ddlpro.DataSource = dstable.DefaultView;
                    ddlpro.DataBind();
                    ddlpro.Items.Insert(0, new ListItem("", ""));
                    ddlpro.SelectedValue = "";

                    ddlline.DataTextField = "sublineid";
                    ddlline.DataValueField = "sublineid";
                    ddlline.DataSource = dsquery.Tables[1].DefaultView;
                    ddlline.DataBind();
                    ddlline.Items.Insert(0, new ListItem("", ""));
                    ddlline.SelectedValue = "";

                    ddlprod.DataTextField = "prodid";
                    ddlprod.DataValueField = "prodid";
                    ddlprod.DataSource = dsquery.Tables[2].DefaultView;
                    ddlprod.DataBind();
                    ddlprod.Items.Insert(0, new ListItem("", ""));
                    ddlprod.SelectedValue = "";

                }
            }
            catch (Exception ex)
            {
                this.lblerror.Text = ex.Message;
            }
            finally
            {

                GC.Collect();
            }
        }

        protected void cmdquery_Click(object sender, EventArgs e)
        {
            DataSet dsresult;
            string strsql = "";

            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }

            strsql = "select * from wtactm  WHERE MFG_DT between '" + dtfr.datatimer().ToString() + "' and '" + dtto.datatimer().ToString() + "'";
            if (!ddlfactory.SelectedValue.Equals(""))
            {
                strsql = strsql + " and plant_loc_id='" + ddlfactory.SelectedValue.ToString().Trim() + "'";
            }
            if (!ddlpro.SelectedValue.Equals(""))
            {
                strsql = strsql + " and equip_proc_type_cd='" + ddlpro.SelectedValue.ToString().Trim() + "'";
            }

            if (!ddlline.SelectedValue.Equals(""))
            {
                strsql = strsql + " and equip_id='" + ddlline.SelectedValue.ToString().Trim() + "'";
            }
            if (!ddlprod.SelectedValue.Equals(""))
            {
                strsql = strsql + " and productid='" + ddlprod.SelectedValue.ToString().Trim() + "'";
            }

            dsresult = dbconn.GetDataSetBySql(strsql);
            if (dsresult.Tables[0].Rows.Count == 0)
            {
                this.lblerror.Text = "查無資料!!";
            }
            else
            {
                this.lblerror.Text = "";
            }
            dgcontent.DataSource = dsresult;
            dgcontent.DataBind();

        }

        protected void cmtexport_Click(object sender, EventArgs e)
        {
            //AppClass Apc = new AppClass();
            //Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "btnexport_click", "", "");
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

            this.dgcontent.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }


    }
}
