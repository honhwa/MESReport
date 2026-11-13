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

namespace WebReport.Reports
{
    public partial class NonKeyMaterialExchangeQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.SetWorkctrDataSource();
                this.SetSubLineDataSource();
                this.SetProductIDDataSource("");
                this.SetPartIDDataSource("","");
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                this.lblErrMsg.Text = "No FabId!";
                Response.Redirect("../../Login.aspx");
            }
        }

        /// <summary>
        /// 設置DataSource: Workctr
        /// </summary>
        private void SetWorkctrDataSource()
        {
            string strSql = @"SELECT USERCTR AS WORKCTR FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND FLG='0'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text ="Cannot Fetch Workctr!";
            }
            else
            {
                this.cmbWorkctr.DataSource = dt;
                this.cmbWorkctr.DataTextField = "WORKCTR";
                this.cmbWorkctr.DataValueField = "WORKCTR";
                this.cmbWorkctr.DataBind();
                this.cmbWorkctr.Items.Insert(0, "");
            }
        }

        /// <summary>
        /// 設置DataSource: Subline 
        /// </summary>
        private void SetSubLineDataSource()
        {
            string strSql = @"SELECT USERCTR AS WORKCTR FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND FLG='1'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch SubLine!";
            }
            else
            {
                this.cmbSubLine.DataSource = dt;
                this.cmbSubLine.DataTextField = "WORKCTR";
                this.cmbSubLine.DataValueField = "WORKCTR";
                this.cmbSubLine.DataBind();
                this.cmbSubLine.Items.Insert(0, "");
            }
        }

        /// <summary>
        /// 設置DataSource: Product ID
        /// </summary>
        private void SetProductIDDataSource(string strUserCtr)
        {
            string strSql = "";

            if (this.cmbProductID.SelectedIndex > 0)
            {
                return;
            }

            if (strUserCtr == "")
            {
                strSql = @"SELECT PRODID FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PRODID";
            }
            else
            {
                strSql = @"SELECT PRODID FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND USERCTR='" + strUserCtr + "' GROUP BY PRODID";
            }
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch ProductID!";
            }
            else
            {
                this.cmbProductID.DataSource = dt;
                this.cmbProductID.DataTextField = "PRODID";
                this.cmbProductID.DataValueField = "PRODID";
                this.cmbProductID.DataBind();
                this.cmbProductID.Items.Insert(0, "");
            }
        }

        /// <summary>
        /// 設置DataSource: Material ID
        /// </summary>
        private void SetPartIDDataSource(string strUserCtr,string strProdID)
        {
            string strSql = "";

            if (this.cmbPartlID.SelectedIndex > 0)
            {
                return;
            }

            if (strUserCtr == "" && strProdID == "")
            {
                strSql = @"SELECT PARTID FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PARTID";
            }
            else
            {
                if (strUserCtr == "")
                {
                    strSql = @"SELECT PARTID FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND PRODID='" + strProdID + "' GROUP BY PARTID";
                }
                else
                    if (strProdID == "")
                    { 
                        strSql = @"SELECT PARTID FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND USERCTR='" + strUserCtr + "' GROUP BY PARTID";
                    }
                    else
                    { 
                        strSql = @"SELECT PARTID FROM MNKMWSM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND USERCTR='" + strUserCtr + "' AND PRODID='" + strProdID + "' GROUP BY PARTID";
                    }
            }

            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch PartID!";
            }
            else
            {
                this.cmbPartlID.DataSource = dt;
                this.cmbPartlID.DataTextField = "PARTID";
                this.cmbPartlID.DataValueField = "PARTID";
                this.cmbPartlID.DataBind();
                this.cmbPartlID.Items.Insert(0, "");
            }
        }

        protected void cmbWorkctr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSubLine.SelectedIndex <=0)
            {
                this.SetProductIDDataSource(this.cmbWorkctr.SelectedValue);
                this.SetPartIDDataSource(this.cmbWorkctr.SelectedValue, "");
            }
            else
            {
                this.SetProductIDDataSource("");
                this.SetPartIDDataSource("", "");
            }
        }

        protected void cmbProductID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbWorkctr.SelectedIndex >0 && this.cmbSubLine.SelectedIndex >0)
            {
                this.SetPartIDDataSource("", this.cmbProductID.SelectedValue);
            }
            else
            {
                if (this.cmbWorkctr.SelectedIndex >0)
                {
                    this.SetPartIDDataSource(this.cmbWorkctr.SelectedValue, this.cmbProductID.SelectedValue);
                }
                else
                {
                    this.SetPartIDDataSource(this.cmbSubLine.SelectedValue , this.cmbProductID.SelectedValue);
                }
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked)
            {
                foreach (ListItem i in this.lstVerb.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstVerb.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;
               
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
                    else if (s.TotalSeconds > 1728000)
                    {
                        strErrorMsg += "時間超出20天範圍！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }
                }

                bool b = false;
                foreach (ListItem i in this.lstVerb.Items)
                {
                    if (i.Selected)
                    {
                        b = true;
                        break;
                    }
                }
                if (!b)
                {
                    strErrorMsg += "交易類型未選擇！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                }

                #region LocalReport DataSource Setting
                try
                {
                    RecordLog Log = new RecordLog();
                    if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                    {
                        this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                    }
                    this.reportViewer.ProcessingMode = ProcessingMode.Local;
                    LocalReport localReport = reportViewer.LocalReport;
                    localReport.ReportPath = @"RDLC\NonKeyMaterialExchange.rdlc";
                    Report.Sources.NonKeyMaterialExchangQuery rs = new Report.Sources.NonKeyMaterialExchangQuery();

                    rs.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                    rs.FromTime = DateTime.Parse(this.txtFromTime.Text.ToString().Trim().Replace("'", ""));
                    rs.ToTime = DateTime.Parse(this.txtToTime.Text.ToString().Trim().Replace("'", ""));
                    //VERB List
                    List<string> verbList = new List<string>();
                    foreach (ListItem li in this.lstVerb.Items)
                    {
                        if (li.Selected)
                        {
                            verbList.Add(li.Value);
                        }
                    }
                    rs.VerbList = verbList;

                    if (this.cmbSubLine.SelectedIndex >0)
                    {
                        rs.SubLine = this.cmbSubLine.SelectedValue;
                    }

                    if (this.cmbWorkctr.SelectedIndex >0)
                    {
                        rs.Workctr = this.cmbWorkctr.SelectedValue ;
                    }

                    if (this.cmbProductID.SelectedIndex >0)
                    {
                        rs.ProdID = this.cmbProductID.SelectedValue;
                    }

                    if (this.cmbPartlID.SelectedIndex >0)
                    {
                        rs.PartID = this.cmbPartlID.SelectedValue;
                    }

                    DataSet dataset = rs.GetDataSet();
                    
                    ReportDataSource dsNonKeyMaterialExchange = new ReportDataSource();
                    dsNonKeyMaterialExchange.Name = "NonKeyMaterialExchange";
                    dsNonKeyMaterialExchange.Value = dataset.Tables[0];
                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(dsNonKeyMaterialExchange);
                }
                catch (Exception ex)
                {
                    this.lblErrMsg.Text = ex.Message;
                }
                #endregion
            }
        }

        protected void lstVerb_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstVerb.Items)
            {
                if (!i.Selected)
                {
                    this.chkAll.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAll.Checked = true;
            }
        }

        protected void cmbSubLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbWorkctr.SelectedIndex <=0)
            {
                this.SetProductIDDataSource(this.cmbSubLine.SelectedValue);
                this.SetPartIDDataSource(this.cmbSubLine.SelectedValue, "");
            }
            else
            {
                this.SetProductIDDataSource("");
                this.SetPartIDDataSource("", "");
            }
        }
    }
}
