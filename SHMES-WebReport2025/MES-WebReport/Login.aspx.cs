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
using System.Xml;
using Report.Library;
using System.Text;

namespace WebReport
{
    public partial class Login : System.Web.UI.Page
    {
        #region parameters
        private string strUid = string.Empty;
        private string strInputPwd = string.Empty;
        private string strFabId = string.Empty;

        private UserInfo uInfo;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CmbFabIdDataBind();
            }
        }

        private void CmbFabIdDataBind()
        {
            FabInfo fab = new FabInfo();
            DataSet dsFab = fab.GetFabIdDataSet();
            this.cmbFabId.DataSource = dsFab;
            this.cmbFabId.DataTextField = "FABID";
            this.cmbFabId.DataValueField = "FABID";
            this.cmbFabId.DataBind();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            this.strFabId = this.cmbFabId.SelectedValue;
            this.strUid = this.txtUId.Text.Trim().Replace("'", "");
            this.strInputPwd = this.txtPwd.Text.Trim().Replace("'", "");
            
            try
            {
                if(this.cmbFabId.SelectedIndex < 0)
                {
                    this.lblErrorMsg.Text = "請選擇FabID！";
                    this.cmbFabId.Focus();
                }
                else if(this.txtUId.Text.Trim().Replace("'","").Equals(string.Empty))
                {
                    this.lblErrorMsg.Text = "請輸入用戶名！";
                    this.txtUId.Focus();

                }
                else if(this.txtPwd.Text.Trim().Replace("'","").Equals(string.Empty))
                {
                    this.lblErrorMsg.Text = "請輸入密碼！";
                    this.txtPwd.Focus();
                }
                //else if (Session["Password"] == null || Session["Password"].ToString().Trim() == string.Empty)
                else if (Session["UserInfo"] == null)
                {
                    this.lblErrorMsg.Text = "沒有此用戶信息！";
                    this.txtUId.Focus();
                }
                else
                {                    
                    string strUserInfoPwd = ((UserInfo)Session["UserInfo"]).Password;
                    if (strUserInfoPwd != this.strInputPwd)
                    {
                        //Session["UserInfo"] = null;
                        this.lblErrorMsg.Text = "密碼錯誤！";
                        this.txtPwd.Focus();
                    }
                    else
                    {
                        RecordLog Log = new RecordLog();
                        if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Login", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                        {
                            this.lblErrorMsg.Text = "RecordLog Error！";
                        }
                        Session["IsLogin"] = "Y";
                        Response.Redirect("~/Index.aspx");                        
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblErrorMsg.Text = ex.Message;
            }
        }

        protected void txtUId_TextChanged(object sender, EventArgs e)
        {
            if (this.txtUId.Text.Trim().Replace("'", "") != "")
            {
                this.strFabId = this.cmbFabId.SelectedValue;
                this.strUid = this.txtUId.Text.Trim().Replace("'", "");

                try
                {
                    this.uInfo = UserInfo.Create(this.strFabId, this.strUid);
                    if (this.uInfo == null)
                    {
                        this.lblErrorMsg.Text = "沒有此用戶信息!";
                        this.lblUserName.Text = "";
                        this.txtUId.Focus();
                        
                    }
                    else
                    {
                        if (this.uInfo.OnJob != "0")
                        {
                            this.lblUserName.Text = this.uInfo.UserName;
                            this.lblErrorMsg.Text = "用戶已離職!";
                            this.txtUId.Focus();
                        }
                        else
                        {
                            this.lblUserName.Text = this.uInfo.UserName;
                            Session["UserInfo"] = this.uInfo;
                            this.lblErrorMsg.Text = string.Empty;
                            this.txtPwd.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.lblErrorMsg.Text = ex.Message;
                    this.txtUId.Focus();
                }
            }
        }

        protected void cmbFabId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
                //if (Session["UserInfo"] == null)
                //{
            
                    Session["UserInfo"] = null;
                    this.uInfo = null;
                    this.lblUserName.Text = string.Empty;
                    this.txtUId.Text = string.Empty;
                    this.txtPwd.Text = string.Empty;
                    this.lblErrorMsg.Text = string.Empty;
                    this.txtUId.Focus();
                //}
            //}
        }
    }
}
