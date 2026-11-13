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
using Report.Sources;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports
{
    public partial class ModuleReworkWeeklyReport : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtFromTime.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect(@"~\Login.aspx");
                }

                //this.lblIp
                //    .Text = "IP: " + Request.UserHostAddress.ToString();
                //this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
                this.BindGroup();
                this.BindProd();
            }
        }

        #region  Event
        protected void Button1_Click1(object sender, EventArgs e)
        {
            foreach (ListItem li in this.cblProdID.Items)
            {
                li.Selected = true;
            }
        }

        /// <summary>
        /// [查詢]事件               
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSerch_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataSet ds = new DataSet();
            try
            {
                if (this.CheckFormControl())
                {
                    ModuleReworkWeekly mrw = new ModuleReworkWeekly();
                    mrw.EndTime = this.txtToTime.Text.Trim().Substring(0, 19) ;
                    mrw.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                    ////mrw.FabID = "B";
                    mrw.Group = this.dlGroup.Text;
                    mrw.ProdID = this.getProdID();
                    mrw.StartTime = this.txtFromTime.Text.Trim().Substring(0, 19) ;
                    mrw.CheckBox1all = this.CheckBox1.Checked;
                    ds = mrw.Read();

                    this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    LocalReport localReport = this.ReportViewer1.LocalReport;
                    //localReport.ReportPath =@"ModuleReworkWeekly2.rdlc";
                    localReport.ReportPath = @"RDLC\ModuleReworkWeekly3.rdlc";

                    ReportDataSource rsd = new ReportDataSource();
                    rsd.Name = "localReport";
                    rsd.Value = ds.Tables[0];
                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(rsd);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 群組資料變更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dlGroup.SelectedIndex < 0)
            {
                this.BindProd();
            }
        }

        /// <summary>
        /// [全選]事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (this.cblProdID.Items.Count == 0)
            {
                return;
            }

            
        }

        #endregion

        #region Methods
        /// <summary>
        /// 綁定群組資料
        /// </summary>
        private void BindGroup()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID ='A' ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.dlGroup.DataSource = null;
                    return;
                }
                this.dlGroup.DataSource = dt;
                this.dlGroup.DataTextField = "PRODGROUP";
                this.dlGroup.DataValueField = "PRODGROUP";
                this.dlGroup.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 綁定產品號碼
        /// </summary>
        private void BindProd()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                //strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' AND PRODGROUP = '" + this.dlGroup.Items[this.dlGroup.SelectedIndex].Text + "' ";
                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODID FROM MPIM WHERE FABID = 'A' AND PRODGROUP = '" + this.dlGroup.Text + "' ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.cblProdID.DataSource = null;
                    return;
                }
                this.cblProdID.DataSource = dt;
                this.cblProdID.DataTextField = "PRODID";
                this.cblProdID.DataValueField = "PRODID";
                this.cblProdID.DataBind();
                this.cblProdID.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 檢查控件合法性


        /// </summary>
        /// <returns></returns>
        private bool CheckFormControl()
        {
            if (this.txtFromTime.Text == "")
            {
                this.lblErrMsg.Text = "請選擇開始時間!";
                return false;
            }
            if(this.txtToTime.Text== "")
            {
                this.lblErrMsg.Text = "請選擇結束時間!";
                return false;
            }

            if (DateTime.Parse(this.txtToTime.Text) < DateTime.Parse(this.txtFromTime.Text))
            {
                this.lblErrMsg.Text = "結束時間不能小於開始時間!";
                return false;
            }

            if (!this.CheckTime())
            {
                this.lblErrMsg.Text = "開始時間,結束時間不能大於7天!";
                return false;
            }

            if (!this.checkProd())
            {
                this.lblErrMsg.Text = "請選擇Prod!";
                return false;
            }

            this.lblErrMsg.Text = "";
            return true;
        }

        /// <summary>
        /// 檢查時間是否大於7天


        /// </summary>
        /// <returns></returns>
        private bool CheckTime()
        {
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Parse(this.txtToTime.Text) - DateTime.Parse(this.txtFromTime.Text);
            if (ts.Days > 31)
            {
                return false;
            }
            return true;
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        /// <summary>
        /// 取得多選的產品號碼資料


        /// </summary>
        /// <returns></returns>
        private string getProdID()
        {
            string strPordID = "''";
            if (this.cblProdID.Items.Count == 0)
            {
                return strPordID;
            }
            else
            {

                foreach (ListItem li in this.cblProdID.Items)
                {
                    if (li.Selected)
                    {
                        strPordID = strPordID + "," + "'" + li.Text + "'";
                    }
                }

                return strPordID;
            }
        }

        /// <summary>
        /// 判斷Prod是否為空
        /// </summary>
        /// <returns></returns>
        private bool checkProd()
        {
            foreach (ListItem li in this.cblProdID.Items)
            {
                if (li.Selected)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBox1.Checked)
            {
                foreach (ListItem li in this.cblProdID.Items)
                {
                    li.Selected = true;
                }
            }
            else
            {
                foreach (ListItem li in this.cblProdID.Items)
                {
                    li.Selected = false;
                }
            }
        }









    }
}
