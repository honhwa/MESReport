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
    public partial class ModuleMaterielScrapQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.txtFromTime.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                //this.lblIp.Text = "IP: " + Request.UserHostAddress.ToString();
                //this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
                this.SetProdIdDatSource();
                this.SetPartIdDataSource();
                this.SetSubLineDataSource();
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        private void SetProdIdDatSource()
        {
            string strSql = @"SELECT PRODID FROM MMPM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PRODID";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch ProdId!");
            }
            else
            {
                this.cmbProdId.DataSource = dt;
                this.cmbProdId.DataTextField = "PRODID";
                this.cmbProdId.DataValueField = "PRODID";
                this.cmbProdId.DataBind();
                this.cmbProdId.Items.Insert(0, "");
            }
        }

        private void SetPartIdDataSource()
        {
            this.chkAll.Checked = false;
            if (this.cmbProdId.SelectedValue != "")
            {
                this.lstPartId.Items.Clear();
                string strProdId = this.cmbProdId.SelectedValue;
                string strSql = @"SELECT PARTID FROM (SELECT FABID,PRODID,PARTID FROM MMPM UNION SELECT FABID,PRODID,SUBPARTID AS PARTID FROM MPSM)AA WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND PRODID='" + this.cmbProdId.SelectedValue + "' GROUP BY PARTID";
                DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                this.lstPartId.DataSource = dt;
                this.lstPartId.DataTextField = "PARTID";
                this.lstPartId.DataValueField = "PARTID";
                this.lstPartId.DataBind();
                this.lstPartId.ClearSelection();
            }
            else
            {
                this.lstPartId.Items.Clear();
                string strSql = @"SELECT PARTID FROM MPAM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
                DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                this.lstPartId.DataSource = dt;
                this.lstPartId.DataTextField = "PARTID";
                this.lstPartId.DataValueField = "PARTID";
                this.lstPartId.DataBind();
                this.lstPartId.ClearSelection();
            }
        }

        private void SetSubLineDataSource()
        {
            string strSql = @"SELECT MSLM.SUBLINEID FROM MSLM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND STOCKID<>''";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch SubLineId!");
            }
            else
            {
                this.cmbSubLine.DataSource = dt;
                this.cmbSubLine.DataTextField = "SUBLINEID";
                this.cmbSubLine.DataValueField = "SUBLINEID";
                this.cmbSubLine.DataBind();
                this.cmbSubLine.Items.Insert(0, "");
            }
        }

        protected void cmbProdId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetPartIdDataSource();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;
            bool bHasError = false;
            if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                bHasError = true;
            }

            if (this.txtToTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                bHasError = true;
            }

            if (this.txtFromTime.Text.Trim().Replace("'", "") != "" && this.txtToTime.Text.Trim().Replace("'", "") != "")
            {
                DateTime fr = new DateTime();
                DateTime to = new DateTime();
                bool bTimeFormatError = false;

                if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "開始時間格式錯誤！\n";
                    bHasError = true;
                    bTimeFormatError = true;
                }

                if (!DateTime.TryParse(this.txtToTime.Text.Trim().Replace("'", ""), out to))
                {
                    strErrorMsg += "結束時間格式錯誤！\n";
                    bHasError = true;
                    bTimeFormatError = true;
                }

                if (!bTimeFormatError)
                {
                    TimeSpan s = to - fr;
                    if (fr > to)
                    {
                        strErrorMsg += "結束時間在開始時間之前！\n";
                        bHasError = true;
                    }
                    else if (s.TotalSeconds > 2592000)
                    {
                        strErrorMsg += "結束時間超出30天範圍！\n";
                        bHasError = true;
                    }
                }

            }
            if (this.lstPartId.SelectedItem == null)
            {
                
                bool b = false;
                foreach (ListItem i in this.lstPartId.Items)
                {
                    if (i.Selected)
                    {
                        b = true;
                        break;
                    }
                }
                //if (!b)
                //{
                //    strErrorMsg += "物料料號未選擇！\n";
                //    bHasError = true;
                //}
                //else
                //{
                //    bHasError = false;
                //}
                bHasError = false;
            }

            if (bHasError)
            {
                this.lblErrMsg.Text = strErrorMsg;
            }
            else
            {
                #region LocalReport DataSource Setting
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                this.reportViewer.ProcessingMode = ProcessingMode.Local;
                LocalReport localReport = reportViewer.LocalReport;
                localReport.ReportPath = @"RDLC/ModuleMaterielScrapQuery.rdlc";


                Report.Sources.ModuleMaterielScrapQuery s = new Report.Sources.ModuleMaterielScrapQuery();
                s.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                s.FromTime = this.txtFromTime.Text.ToString().Trim().Replace("'", "");
                s.ToTime = this.txtToTime.Text.ToString().Trim().Replace("'", "");
                s.ProdID = this.cmbProdId.SelectedValue;
                //s.PartIDList = this.lstPartId.SelectedItem.Value
                List<string> partIDList = new List<string>();
                foreach (ListItem li in this.lstPartId.Items)
                {
                    if (li.Selected)
                    {
                        partIDList.Add(li.Value);
                    }
                }
                s.PartIDList = partIDList;
                s.SubLine = this.cmbSubLine.SelectedValue;
                DataSet dataset = s.GetDataSet();

                ReportDataSource dsModuleMaterielScrapQuery = new ReportDataSource();
                dsModuleMaterielScrapQuery.Name = "ModuleMaterielScrapQuery";
                dsModuleMaterielScrapQuery.Value = dataset.Tables[0];
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dsModuleMaterielScrapQuery);

                #endregion
            }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        protected void lstPartId_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstPartId.Items)
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

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked)
            {
                foreach (ListItem i in this.lstPartId.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstPartId.Items)
                {
                    i.Selected = false;
                }
            }

        }
            
    }
}
