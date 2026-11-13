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

    public partial class UnTactTime : System.Web.UI.Page
    {
        DBConn dbconn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) //首次loading page
            {
                ////bindcon();
            }
        }
        //private void bindcon()// 綁定控件
        //{
        //    try
        //    {
        //        DataSet dsquery;
        //        string strsql;
        //        strsql = "";
        //        strsql = strsql + "select distinct plant from mtactm;";
        //        dsquery = dbconn.GetDataSetBySql(strsql);
        //        if (dsquery == null)
        //        {
        //            this.lblerr.Text = "綁定控件異常!!";
        //        }
        //        else
        //        {
        //            this.lblerr.Text = "";

        //            ddlfactory.DataTextField = "plant";
        //            ddlfactory.DataValueField = "plant";
        //            ddlfactory.DataSource = dsquery.Tables[0].DefaultView;
        //            ddlfactory.DataBind();
        //            ddlfactory.Items.Insert(0, new ListItem("", ""));
        //            ddlfactory.SelectedValue = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lblerr.Text = ex.Message;
        //    }
        //    finally
        //    {

        //        GC.Collect();
        //    }
        //}

        protected void btnquery_Click(object sender, EventArgs e)
        {
            try
            {
                string strsql = "";

                DataSet dsquery;
                strsql = "select * from wutactm";
                strsql = strsql + " where mfg_dt='" + dt.datatimer().Replace("-", "/").ToString() + "'";

                dsquery = dbconn.GetDataSetBySql(strsql);
                if (dsquery == null)
                {
                    this.lblerr.Text = "查詢異常！！";
                    gvcontent.DataSource = null;
                    gvcontent.DataBind();
                }
                else if (dsquery.Tables[0].Rows.Count == 0)
                {
                    this.lblerr.Text = "查無資料！！";
                    gvcontent.DataSource = dsquery;
                    gvcontent.DataBind();
                }
                else
                {
                    this.lblerr.Text = "";
                    gvcontent.DataSource = dsquery;
                    gvcontent.DataBind();
                }
            }
            catch (Exception ex)
            {
                this.lblerr.Text = ex.Message;
            }
            finally
            {

                GC.Collect();
            }
        }
    }
}
