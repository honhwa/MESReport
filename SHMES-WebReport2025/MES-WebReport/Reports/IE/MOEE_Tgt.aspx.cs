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
using Microsoft.Reporting.WebForms;
using System.Text;
using Report.Library;
using Report.Sources;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;

namespace Report.Reports.IE
{
    public partial class MOEE_Tgt : System.Web.UI.Page
    {
        int count;
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                else
                {
                    SetProcessDatasource();
                    SetEQIDDatasource();
                    SetEQTYPEDatasource();
                    DataBound();
                }
            }

        }

        private void SetProcessDatasource()
        {
            string strsql = @"select DISTINCT PROCESS from MEQTGTM";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find MFGDate！');</script>");
            }
            else
            {
                this.ddlProcess.DataSource = dt;
                this.ddlProcess.DataTextField = "PROCESS";
                this.ddlProcess.DataValueField = "PROCESS";
                this.ddlProcess.DataBind();
            }
        }
        private void SetEQIDDatasource()
        {
            string strsql;
            strsql = @"select DISTINCT EQ_ID from MEQTGTM ";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('找不到EQ_ID！');</script>");
            }
            else
            {
                this.ddlEQID.DataSource = dt;
                this.ddlEQID.DataTextField = "EQ_ID";
                this.ddlEQID.DataValueField = "EQ_ID";
                this.ddlEQID.DataBind();
            }
        }
        private void SetEQTYPEDatasource()
        {
            string strsql = @"select DISTINCT EQ_TYPE from MEQTGTM  ";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.Response.Write("<script language=javascript>window.alert('Cannot Find Status！');</script>");
            }
            else
            {
                this.ddlEQTYPE.DataSource = dt;
                this.ddlEQTYPE.DataTextField = "EQ_TYPE";
                this.ddlEQTYPE.DataValueField = "EQ_TYPE";
                this.ddlEQTYPE.DataBind();

            }
        }
        protected void DataBound()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder stb = new StringBuilder();
            stb.Append("select PLANT,PROCESS,EQ_ID,EQ_TYPE,OEE_Tgt,Quality,UPDATER,TIME_STAMP from MOEE_Tgt");
            stb.Append("  order by TIME_STAMP DESC");
            DataTable dt1 = new DataTable();
            dt1 = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            gvData.DataSource = dt1;
            gvData.DataBind();

            if (gvData.Rows.Count == 0)
            {
                Label1.Text = "查無資料!";
                Label1.Visible = true;
            }
            else
            {
                Label1.Visible = false;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            //Response.ContentEncoding = System.Text.Encoding.UTF7;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.gvData.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.End();
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (checkInfo())
            {

                string strPLANT = this.ddlfab.SelectedValue.Trim().Replace("'", "");
                string strProcess = this.ddlProcess.SelectedValue.Trim().Replace("'", "");
                string strEQID = this.ddlEQID.SelectedValue.Trim().Replace("'", "");
                string strEQTYPE = this.ddlEQTYPE.SelectedValue.Trim().Replace("'", "");
                string userid = ((UserInfo)Session["UserInfo"]).UserID;
                string strSql = "UPDATE MOEE_Tgt SET OEE_Tgt=" + txtOEE_Tgt.Text.Trim() + ", Quality=" + txtQuality.Text.Trim() + " ,UPDATER='" + userid + "',TIME_STAMP=GETDATE() WHERE PLANT= '" + strPLANT + "' and Process='" + strProcess + "' and EQ_ID='" + strEQID + "' and EQ_TYPE='" + strEQTYPE + "'";
                if (dbConn.ExecuteSql(strSql) == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('修改失敗！');</script>");
                }
                Refresh();

            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i <= gvData.Rows.Count - 1; i++)
            {
                CheckBox cbox = (CheckBox)gvData.Rows[i].FindControl("Selected");

                if (cbox.Checked == true)
                {
                    j++;
                    string strErrDesc;
                    int intErrCode;
                    string strSql = "DELETE FROM MOEE_Tgt where PLANT ='" + gvData.DataKeys[i].Values["PLANT"] + "' AND PROCESS='" + gvData.DataKeys[i].Values["PROCESS"] + "' AND EQ_ID='" + gvData.DataKeys[i].Values["EQ_ID"] + "' AND EQ_TYPE='" + gvData.DataKeys[i].Values["EQ_TYPE"] + "'";
                    if (this.dbConn.ExecuteSql(strSql) == false)
                    {
                        Refresh();
                        return;
                    }
                }
            }
            if (j == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('请至少选择一條記錄！');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('刪除成功！');</script>");
                Refresh();
            }
        }
        protected bool String2Numeric(string strValue)
        {
            Boolean blReturn = false;
            try
            {
                Double dblValue = Convert.ToDouble(strValue);
                blReturn = true;
            }
            catch
            {
                blReturn = false;
            }
            return blReturn;
        }
        protected bool checkInfo()
        {
            bool isok = false;
            if (this.ddlfab.Text.Trim().Replace("'", "").Equals(string.Empty))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇廠別！');</script>");
                this.ddlfab.Focus();
            }
            if (this.ddlProcess.Text.Trim().Replace("'", "").Equals(string.Empty))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇Process！');</script>");
                this.ddlProcess.Focus();
            }
            if (this.ddlEQID.Text.Trim().Replace("'", "").Equals(string.Empty))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇EQ_ID！');</script>");
                this.ddlEQID.Focus();
            }
            if (this.ddlEQTYPE.Text.Trim().Replace("'", "").Equals(string.Empty))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請選擇EQ_TYPE！');</script>");
                this.ddlEQTYPE.Focus();
            }

            else if (this.txtOEE_Tgt.Text.Trim().Replace("'", "").Equals(string.Empty))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請輸入OEE_Tgt值！');</script>");
                this.txtOEE_Tgt.Focus();
            }
            else if (String2Numeric(txtOEE_Tgt.Text.Trim()) == false)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('輸入的OEE_Tgt值不為數值類型！');</script>");
                this.txtOEE_Tgt.Focus();
            }
            else if (this.txtQuality.Text.Trim().Replace("'", "").Equals(string.Empty))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('請輸入Quality值！');</script>");
                this.txtQuality.Focus();
            }
            else if (String2Numeric(txtQuality.Text.Trim()) == false)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('輸入的Quality值不為數值類型！');</script>");
                this.txtQuality.Focus();
            }
            else
            {
                isok = true;
            }
            return isok;
        }
        protected int getcheckbox()
        {
            int j = 0;
            for (int i = 0; i <= gvData.Rows.Count - 1; i++)
            {
                CheckBox cbox = (CheckBox)gvData.Rows[i].FindControl("Selected");

                if (cbox.Checked == true)
                {
                    j++;
                }
            }
            return j;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (getcheckbox() == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('请选择一條記錄！');</script>");
            }
            else if (getcheckbox() > 1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('只能选择一條記錄！');</script>");
            }
            else
            {
                this.btnAdd.Visible = false;
                this.btnUpdate.Visible = false;
                this.btnDelete.Visible = false;
                this.btnSubmit.Visible = true;
                this.btnCancel.Visible = true;

                for (int i = 0; i <= gvData.Rows.Count - 1; i++)
                {
                    CheckBox cbox = (CheckBox)gvData.Rows[i].FindControl("Selected");

                    if (cbox.Checked == true)
                    {
                        string strSql = @"SELECT PLANT,PROCESS,EQ_ID,EQ_TYPE,OEE_Tgt,Quality FROM MOEE_Tgt WHERE PLANT = '" + gvData.DataKeys[i].Values["PLANT"] + "' AND PROCESS='" + gvData.DataKeys[i].Values["PROCESS"] + "' AND EQ_ID='" + gvData.DataKeys[i].Values["EQ_ID"] + "' AND EQ_TYPE='" + gvData.DataKeys[i].Values["EQ_TYPE"] + "'";
                        DBConn dbConn = new DBConn();
                        DataTable dt = dbConn.GetDataSetBySql(strSql).Tables[0];
                        ddlfab.SelectedValue = dt.Rows[0][0].ToString();
                        ddlProcess.SelectedValue = dt.Rows[0][1].ToString();
                        ddlEQID.SelectedValue = dt.Rows[0][2].ToString();
                        ddlEQTYPE.SelectedValue = dt.Rows[0][3].ToString();
                        txtOEE_Tgt.Text = dt.Rows[0][4].ToString();
                        txtQuality.Text = dt.Rows[0][5].ToString();


                        this.ddlfab.Enabled = false;
                        this.ddlProcess.Enabled = false;
                        this.ddlEQID.Enabled = false;
                        this.ddlEQTYPE.Enabled = false;
                        
                    }
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkInfo())
            {
                string strPLANT = this.ddlfab.SelectedValue.Trim().Replace("'", "");
                string strProcess = this.ddlProcess.SelectedValue.Trim().Replace("'", "");
                string strEQID = this.ddlEQID.SelectedValue.Trim().Replace("'", "");
                string strEQTYPE = this.ddlEQTYPE.SelectedValue.Trim().Replace("'", "");
                string userid = ((UserInfo)Session["UserInfo"]).UserID;

                string strSql = @"SELECT PLANT FROM MOEE_Tgt WHERE  PLANT= '" + strPLANT + "' and Process='" + strProcess + "' and EQ_ID='" + strEQID + "' and EQ_TYPE='" + strEQTYPE + "'";
                DataTable dtQuery = dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dtQuery.Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('此條已存在！');</script>");
                    this.ddlProcess.Focus();
                }

                else
                {
                    strSql = "INSERT INTO MOEE_Tgt(PLANT,PROCESS,EQ_ID,EQ_TYPE,OEE_Tgt,Quality,UPDATER,TIME_STAMP) ";
                    strSql = strSql + " VALUES ( '" + strPLANT + "', '" + strProcess + "', '" + strEQID + "', '" + strEQTYPE + "', '" + txtOEE_Tgt.Text + "', '" + txtQuality.Text + "', '" + userid + "', GETDATE() ) ";
                    if (dbConn.ExecuteSql(strSql) == false)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript'>alert('修改失敗！');</script>");
                    }
                    Refresh();
                }
            }
        }
        protected void Selected_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox allSelected = (CheckBox)sender;

            if (allSelected.Checked)
            {
                for (int i = 0; i < gvData.Rows.Count; i++)
                {
                    CheckBox selected = (CheckBox)gvData.Rows[i].FindControl("Selected");
                    selected.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < gvData.Rows.Count; i++)
                {
                    CheckBox selected = (CheckBox)gvData.Rows[i].FindControl("Selected");
                    selected.Checked = false;
                }
            }
        }
        protected void Refresh()
        {
            this.ddlfab.Enabled = true;
            this.ddlProcess.Enabled = true;
            this.ddlEQID.Enabled = true;
            this.ddlEQTYPE.Enabled = true;

            ddlfab.Items.Clear();
            ddlProcess.Items.Clear();
            ddlEQID.Items.Clear();
            ddlEQTYPE.Items.Clear();

            txtOEE_Tgt.Text = "";
            txtQuality.Text = "";
            this.btnAdd.Visible = true;
            this.btnUpdate.Visible = true;
            this.btnDelete.Visible = true;
            this.btnSubmit.Visible = false;
            this.btnCancel.Visible = false;
            SetProcessDatasource();
            SetEQIDDatasource();
            SetEQTYPEDatasource();
            DataBound();
        }
        protected void SaveLog(string strLog)
        {
            try
            {
                string strLogFileName = System.DateTime.Now.ToString("yyyyMM") + ".txt";

                string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Log/");
                if (!System.IO.Directory.Exists(strPath))
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                StreamWriter sw = File.AppendText(strPath + strLogFileName);
                sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Message:" + strLog);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }

    }
}