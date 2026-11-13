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
using Report.Library;
using System.Text;
using System.Data.SqlClient;

/* 2014/2/17         SSC             V1.0                  Modify Columns show  */
namespace WebReport.Reports
{
    public partial class RMSP : System.Web.UI.Page
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
                this.SetWoStatus();
                this.SetWotype();
                this.SetPartid();
                this.SetProdid();
                this.SetColumns();  // V1.0 ADD
                //this.SetLotype();
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


        private void SetWotype()
        {
            this.lstWotype.Items.Clear();
            string strSql = @"SELECT WOTYPE FROM MWOTM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY WOTYPE ORDER BY WOTYPE";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstWotype.DataSource = dt;
            this.lstWotype.DataTextField = "WOTYPE";
            this.lstWotype.DataValueField = "WOTYPE";
            this.lstWotype.DataBind();
            this.lstWotype.ClearSelection();
        }
        //V1.0 ADD begin
        private void SetColumns()
        {
            this.lstColumns.Items.Clear();
            this.lstColumns.Items.Add("MFGDATE");
            this.lstColumns.Items.Add("WO NUM");
            this.lstColumns.Items.Add("WORKCTR");
            this.lstColumns.Items.Add("DEFECT CODE");
            this.lstColumns.Items.Add("DefectDesc");
            this.lstColumns.Items.Add("BIPAME PART");
            this.lstColumns.Items.Add("COMP USAGE");
            this.lstColumns.ClearSelection();
        }
        //V1.0 ADD end

        private void SetWoStatus()
        {
            this.lstWostatus.Items.Clear();
            string strSql = @"SELECT WOSTATUS FROM MWOSU GROUP BY WOSTATUS ORDER BY WOSTATUS";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstWostatus.DataSource = dt;
            this.lstWostatus.DataTextField = "WOSTATUS";
            this.lstWostatus.DataValueField = "WOSTATUS";
            this.lstWostatus.DataBind();
            this.lstWostatus.ClearSelection();
        }

        //private void SetLotype()
        //{
        //    this.lstLottype.Items.Clear();
        //    string strSql = @"SELECT LOTTYPE FROM WLTM GROUP BY LOTTYPE ORDER BY LOTTYPE";
        //    DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
        //    this.lstLottype.DataSource = dt;
        //    this.lstLottype.DataTextField = "LOTTYPE";
        //    this.lstLottype.DataValueField = "LOTTYPE";
        //    this.lstLottype.DataBind();
        //    //this.lstLottype.ClearSelection();
        //}

        private void SetWonbr()
        {
            string strFilter = string.Empty;

            //if (this.lstLottype.SelectedIndex >= 0)
            //{
            //    StringBuilder sbLotstatus = new StringBuilder();
            //    foreach (ListItem li in this.lstLottype.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            sbLotstatus.Append("'").Append(li.Value).Append("'");
            //        }
            //    }
            //    if (sbLotstatus.ToString().Length > 0)
            //    {
            //        strFilter += " AND LOTTYPE IN (" + sbLotstatus.Replace("''", "','") + ") ";
            //    }
            //}

            if (this.lstWotype.SelectedIndex >= 0)
            {
                StringBuilder sbWotype = new StringBuilder();
                foreach (ListItem li in this.lstWotype.Items)
                {
                    if (li.Selected)
                    {
                        sbWotype.Append("'").Append(li.Value).Append("'");
                    }
                }
                if (sbWotype.ToString().Length > 0)
                {
                    strFilter += " AND WOTYPE IN (" + sbWotype.Replace("''", "','") + ") ";
                }
            }

            if (this.lstWostatus.SelectedIndex >= 0)
            {
                StringBuilder sbWostatus = new StringBuilder();
                foreach (ListItem Li in this.lstWostatus.Items)
                {
                    if (Li.Selected)
                    {
                        sbWostatus.Append("'").Append(Li.Value).Append("'");
                    }
                }
                if (sbWostatus.ToString().Length > 0)
                {
                    strFilter += "AND WOSTATUS IN (" + sbWostatus.Replace("''", "','") + ")";
                }
            }
            string strSql = "SELECT WONBR FROM New_SWOM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "'" + strFilter;

            DataTable dt = this.dbConn.GetDataSetBySql(strSql + strFilter).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                //Response.Write("Cannot Fetch SubLineID!");
                this.lstWonbr.Items.Clear();
                this.chkAllWonbr.Enabled = false;
            }
            else
            {
                this.chkAllWonbr.Enabled = true;
                this.lstWonbr.Items.Clear();
                this.lstWonbr.DataSource = dt;
                this.lstWonbr.DataTextField = "WONBR";
                this.lstWonbr.DataValueField = "WONBR";
                this.lstWonbr.DataBind();
                this.lstWonbr.ClearSelection();
            }
        }


        private void SetProdid()
        {
            this.lstProd.Items.Clear();
            string strSql = @"SELECT PRODID FROM MPIM WHERE FABID= '" +((UserInfo)Session["UserInfo"]).FabId+"' GROUP BY PRODID ORDER BY PRODID";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstProd.DataSource = dt;
            this.lstProd.DataTextField = "PRODID";
            this.lstProd.DataValueField = "PRODID";
            this.lstProd.DataBind();
            this.lstProd.ClearSelection();
        }


        private void SetPartid()
        {
            this.lstPartID.Items.Clear();
            string strSql = @"SELECT PARTID  FROM MPAM WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PARTID ORDER BY PARTID";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstPartID.DataSource = dt;
            this.lstPartID.DataTextField = "PARTID";
            this.lstPartID.DataValueField = "PARTID";
            this.lstPartID.DataBind();
            this.lstPartID.ClearSelection();
            strSql = "SELECT SOPRODID FROM MSPM WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY SOPRODID ORDER BY SOPRODID";
            dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                this.lstPartID.Items.Add(dt.Rows[i-1][0].ToString());
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
            this.SetWonbr();
        }


        protected void chkAllWostatus_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllWostatus.Checked)
            {
                foreach (ListItem i in this.lstWostatus.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstWostatus.Items)
                {
                    i.Selected = false;
                }
            }
            this.SetWonbr();
        }

        protected void chkAllWonbr_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllWonbr.Checked)
            {
                foreach (ListItem i in this.lstWonbr.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstWonbr.Items)
                {
                    i.Selected = false;
                }
            }
        }


        protected void chkAllPartID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllPartID.Checked)
            {
                foreach (ListItem i in this.lstPartID.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstPartID.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void chkAllProd_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllProd.Checked)
            {
                foreach (ListItem i in this.lstProd.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstProd.Items)
                {
                    i.Selected = false;
                }
            }

        }

        //V1.0 ADD Begin
        protected void chkColumns_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkColumns.Checked)
            {
                foreach (ListItem i in this.lstColumns.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstColumns.Items)
                {
                    i.Selected = false;
                }
            }
        }
        //V1.0 ADD End


        //protected void chkAllLottype_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.chkAllLottype.Checked)
        //    {
        //        foreach (ListItem i in this.lstLottype.Items)
        //        {
        //            i.Selected = true;
        //        }
        //    }
        //    else
        //    {
        //        foreach (ListItem i in this.lstLottype.Items)
        //        {
        //            i.Selected = false;
        //        }
        //    }
        //    this.SetWonbr();
        //}


        protected void btnQuery_Click(object sender, EventArgs e)
        {

            try
            {
                this.lblErrMsg.Text = string.Empty;
                string strErrorMsg = string.Empty;
                bool bHasError = false;

                if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "開始時間為空！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    bHasError = true;
                    return;
                }

                if (this.txtToTime.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "結束時間為空！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    bHasError = true;
                    return;
                }

                if (!bHasError && this.txtFromTime.Text.Trim().Replace("'", "") != "" && this.txtToTime.Text.Trim().Replace("'", "") != "")
                {
                    DateTime fr = new DateTime();
                    DateTime to = new DateTime();

                    if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                    {
                        strErrorMsg += "開始時間格式錯誤！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                        bHasError = true;
                    }

                    if (!DateTime.TryParse(this.txtToTime.Text.Trim().Replace("'", ""), out to))
                    {
                        strErrorMsg += "結束時間格式錯誤！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                        bHasError = true;
                    }

                    if (!bHasError)
                    {
                        TimeSpan s = to - fr;
                        if (fr > to)
                        {
                            strErrorMsg += "結束時間在開始時間之前！\n";
                            this.lblErrMsg.Text = strErrorMsg;
                            return;
                        }
                    }

                    if (bHasError)
                    {
                        this.lblErrMsg.Text = strErrorMsg;
                    }
                    else
                    {
                        RecordLog Log = new RecordLog();
                        if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                        {
                            this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                        }
                        Report.Sources.RMSP rmsp = new Report.Sources.RMSP();
                        rmsp.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                        rmsp.FromTime = DateTime.Parse(this.txtFromTime.Text.ToString().Trim().Replace("'", ""));
                        rmsp.ToTime = DateTime.Parse(this.txtToTime.Text.ToString().Trim().Replace("'",""));

                        foreach (ListItem i in this.lstPartID.Items)
                        {
                            if (i.Selected)
                            {
                                rmsp.ListPartID.Add(i.Value);
                            }
                        }
                        foreach (ListItem i in this.lstProd.Items)
                        {
                            if (i.Selected)
                            {
                                rmsp.ListProdID.Add(i.Value);
                            }
                        }
                        foreach (ListItem i in this.lstWonbr.Items)
                        {
                            if (i.Selected)
                            {
                                rmsp.ListWonbr.Add(i.Value);
                            }
                        }
                        //V1.0 ADD Begin
                        foreach (ListItem i in this.lstColumns.Items)
                        {
                            if (i.Selected)
                            {
                                rmsp.ListColumns.Add(i.Value);
                            }
                        }
                        //V1.0 ADD END
                        this.ReportViewer.ProcessingMode = ProcessingMode.Local;
                        LocalReport localReport = this.ReportViewer.LocalReport;
                        localReport.ReportPath = @"RDLC\RMSP.rdlc";

                        DataSet dataset = rmsp.GetDataSet();

                        if(dataset.Tables[0].Rows.Count>65536)
                        {
                            throw new Exception("資料行數已經超過EXCEL表所能承受的最大筆數65536，請分時間段進行查詢！");
                        } 

                        ReportDataSource dsRMSP = new ReportDataSource();
                        dsRMSP.Name = "RMSP";
                        dsRMSP.Value = dataset.Tables[0];
                        localReport.DataSources.Clear();
                        localReport.DataSources.Add(dsRMSP);
                        this.ReportViewer.LocalReport.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        protected void lstWotype_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetWonbr();
        }

        protected void lstWostatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetWonbr();
        }

        //protected void lstLottype_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.SetWonbr();
        //}


   }
}
