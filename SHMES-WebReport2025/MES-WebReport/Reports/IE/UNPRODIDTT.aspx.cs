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
namespace OEE_REPORT
{
    public partial class UNPRODIDTT : System.Web.UI.Page
    {
        DBConn dbconn = new DBConn();
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
                DataSet dsquery;
                string strsql;
                strsql = "";                
                strsql = strsql + "select ID from msumu where typeid='OEEP';";
                dsquery = dbconn.GetDataSetBySql(strsql);
                if (dsquery == null)
                {
                    this.lblerr.Text = "綁定控件異常!!";
                }
                else
                {
                    
                    ddlfactory.DataTextField = "ID";
                    ddlfactory.DataValueField = "ID";
                    ddlfactory.DataSource = dsquery.Tables[0].DefaultView;
                    ddlfactory.DataBind();
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

        protected void btnquery_Click(object sender, EventArgs e)
        {
            try
            {
                string strsql = "";
                string strquery = "";
                string strlcm2 = "";
                string strlcm1 = "";
                string strcon = "";
                DataSet dsquery;
                strsql = "select * from ";
                strlcm1 = " mequttm ";
                strcon = " where report_dt='" + dt.datatimer().Replace("-", "/").ToString() + "'";
                //if (ddlfactory.SelectedValue.ToString().ToUpper() == "LCM2")//LCM1
                //{
                //    strquery = strsql + strlcm1 + strcon;
                //}
                //if (ddlfactory.SelectedValue.ToString().ToUpper() == "LCM1")//LCM2
                //{
                //    strquery = strsql + strlcm2 + strcon;
                //}
                //if (ddlfactory.SelectedValue.ToString().ToUpper() == "LCM1+LCM2") // LCM1+LCM2
                //{
                //    strquery = strsql + strlcm1 + strcon + " union " + strsql + strlcm2 + strcon;
                //}
                strquery = strsql + strlcm1 + strcon;
                dsquery = dbconn.GetDataSetBySql(strquery);
                if (dsquery == null)
                {
                    this.lblerr.Text = "查詢異常！！";
                    gvcontent.DataSource =null;
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
