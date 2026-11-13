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

/*
 * 2013-05-22  V1.00    innolux/qingqing
 * 2020-08-06  V1.01    innolux/Jeff     ADD剔除非出貨料號 8380 8700 站點產出 選項
 */
namespace WebReport.Reports
{
    public partial class InputAndOutput : System.Web.UI.Page
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
                this.txtDate.Text = "";
                this.txtToDate.Text ="";
                this.SetStationList();
                this.SetProdIdList();
                this.SetProdGroupList();
                this.SetWotypeList();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }
        protected void btnLogout_Click(object sender, ImageClickEventArgs e) 
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Redirect("~/Login.aspx");
        }

        private void SetStationList()
        {
            this.lstStations.Items.Clear();
            string strSql = @"SELECT WORKCTR FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY WORKCTR ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstStations.DataSource = dt;
            this.lstStations.DataTextField = "WORKCTR";
            this.lstStations.DataValueField = "WORKCTR";
            this.lstStations.DataBind();
            this.lstStations.ClearSelection();
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

        private void SetWotypeList()
        {
            this.lstWotype.Items.Clear();
            string strSql = @"SELECT WOTYPE FROM (SELECT WOTYPE FROM MWOTM  WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' UNION ALL SELECT 'S-RW' AS WOTYPE)A ORDER BY WOTYPE";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstWotype.DataSource = dt;
            this.lstWotype.DataTextField = "WOTYPE";
            this.lstWotype.DataValueField = "WOTYPE";
            this.lstWotype.DataBind();
            this.lstWotype.ClearSelection();
        }


        private void SetProdGroupList()
        {
            this.lstProdGroup.Items.Clear();
            string strSql = @"SELECT PRODGROUP FROM MPGTM WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PRODGROUP ORDER BY PRODGROUP";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstProdGroup.DataSource = dt;
            this.lstProdGroup.DataTextField = "PRODGROUP";
            this.lstProdGroup.DataValueField = "PRODGROUP";
            this.lstProdGroup.DataBind();
            this.lstProdGroup.ClearSelection();
        }

        protected void chkAllStations_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllStations.Checked)
            {
                foreach (ListItem i in this.lstStations.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstStations.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void chkAllProdID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllStations.Checked)
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

        protected void chkAllProdGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllStations.Checked)
            {
                foreach (ListItem i in this.lstProdGroup.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstProdGroup.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void chkAllWotype_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllWotype.Checked)
            {
                foreach (ListItem i in this.lstWotype.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstWotype.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReportViewer.ProcessingMode = ProcessingMode.Local;
                LocalReport localReport = ReportViewer.LocalReport;
                localReport.ReportPath = @"RDLC\InputAndOutput.rdlc";

                this.lblErrMsg.Text = string.Empty;
                string strErrorMsg = string.Empty;
                bool bHasError = false;
                
                if (this.txtDate.Text.Trim().Replace("'", "") == "")
                {
                    this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd ");
                }
                if (this.txtToDate.Text.Trim().Replace("'","")=="")
                {
                    this.txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd ");
                }

                if (this.txtDate.Text.Trim().Replace("'", "") != "" && this.txtToDate.Text.Trim().Replace("'", "") != "")
                {
                    DateTime fr = new DateTime();
                    DateTime to = new DateTime();
                    bool bTimeFormatError = false;

                    if (!DateTime.TryParse(this.txtDate.Text.Trim().Replace("'", ""), out fr))
                    {
                        strErrorMsg += "開始時間格式錯誤！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                        bTimeFormatError = true;
                    }

                    if (!DateTime.TryParse(this.txtToDate.Text.Trim().Replace("'", ""), out to))
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
                }
                if (this.lstProdId.SelectedIndex < 0)
                {
                    this.chkAllProdID.Checked = true;
                    foreach (ListItem itemProdID in this.lstProdId.Items)
                    {
                        itemProdID.Selected = true;
                    }
                }

                if (this.chkAllProdID.Checked)
                {
                    foreach (ListItem itemProdID in this.lstProdId.Items)
                    {
                        itemProdID.Selected = true;
                    }
                }

                if (this.lstProdGroup.SelectedIndex < 0)
                {
                    this.chkAllProdGroup.Checked = true;
                    foreach (ListItem itemProdGroup in this.lstProdGroup.Items)
                    {
                        itemProdGroup.Selected = true;
                    }
                }

                if (this.chkAllProdGroup.Checked)
                {
                    foreach (ListItem itemProdGroup in this.lstProdGroup.Items)
                    {
                        itemProdGroup.Selected = true;
                    }
                }

                if (this.lstStations.SelectedIndex < 0)
                {
                    this.chkAllStations.Checked = true;
                    foreach (ListItem itemStation in this.lstStations.Items)
                    {
                        itemStation.Selected = true;
                    }
                }

                if (this.chkAllStations.Checked)
                {
                    foreach (ListItem itemStation in this.lstStations.Items)
                    {
                        itemStation.Selected = true;
                    }
                }

                if (this.chkAllWotype.Checked)
                {
                    foreach (ListItem itemWotype in this.lstWotype.Items)
                    {
                        itemWotype.Selected = true;
                    }
                }
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                Report.Sources.InputAndOutput InputAndOutput = new Report.Sources.InputAndOutput();
                InputAndOutput.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                if (this.txtDate.Text == "")
                {
                    InputAndOutput.TimeBy = DateTime.Now.Day.ToString("yyyy-MM-dd");
                }
                else
                {
                    InputAndOutput.TimeBy = this.txtDate.Text;
                }
                if (this.txtToDate.Text == "")
                {
                    InputAndOutput.ToTimeBy = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    InputAndOutput.ToTimeBy = this.txtToDate.Text;
                }
                /*Begin add 1.01*/
                if (this.chkDelWorkctr.Checked == true)
                {
                    InputAndOutput.DelWorkctr = true;
                }
                else
                {
                    InputAndOutput.DelWorkctr = false;
                }
                /*End add 1.01*/
                foreach (ListItem itemStation in this.lstStations.Items)
                {
                    string strStation = string.Empty;
                    if (itemStation.Selected)
                    {
                        InputAndOutput.WorkCTR.Add(itemStation.Text);
                    }
                }

                foreach (ListItem itemProdID in this.lstProdId.Items)
                {
                    string strProdID = string.Empty;
                    if (itemProdID.Selected)
                    {
                        InputAndOutput.ProdID.Add(itemProdID.Text);
                    }
                }

                foreach (ListItem itemProdGroup in this.lstProdGroup.Items)
                {
                    string strProdGroup = string.Empty;
                    if (itemProdGroup.Selected)
                    {
                        InputAndOutput.ProdGroup.Add(itemProdGroup.Text);
                    }
                }

                foreach (ListItem itemWotype in this.lstWotype.Items)
                {
                    string strProdID = string.Empty;
                    if (itemWotype.Selected)
                    {
                        InputAndOutput.Wotype.Add(itemWotype.Text);
                    }
                }
                /*V1.00 Begin*/
                if (RadioButtype.SelectedItem.Text != "")
                {
                    InputAndOutput.Type = RadioButtype.SelectedItem.Value;
                }
                /*V1.00 End*/
                DataSet dataset = InputAndOutput.GetDataSet();

                ReportDataSource InAndOut = new ReportDataSource();
                InAndOut.Name = "InputAndOutput";
                InAndOut.Value = dataset.Tables[0];
                localReport.DataSources.Clear();
                localReport.DataSources.Add(InAndOut);
                this.ReportViewer.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        protected void lstWotype_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkAllWotype.Checked = false;
        }
    }
}


