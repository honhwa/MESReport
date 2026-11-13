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
using Report.Reports;
using Report.Sources;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports.MFG
{
    public partial class RMDDQuery : System.Web.UI.Page
    {

        #region Para
        private DBConn dbConn = new DBConn();
        private DataSet ds;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }

                this.BindProdID();
                this.BindSubLineID();
                this.BindTeamID();
                this.BindLotType();
                this.BindProdGroup();
                this.BindWorkctr();
                //this.BindUserID();                

            }

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            try
            {



                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }

                BaseRMDD bDao = new BaseRMDD();

                bDao.TeamID = "";
                bDao.TeamMode = "";
                bDao.LotType = "";
                bDao.Workctr = "";
                bDao.ShiftID = "";

                bDao.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                bDao.ReportDate = txtFromTime.Text.ToString().Trim();
                bDao.ReportToDate = txtToTime.Text.ToString().Trim();

                if (dlShiftID.Text.ToString() == "ALL" || dlShiftID.Text.ToString() == "白晚班")
                {
                    bDao.ShiftID = "";
                }
                else if (dlShiftID.Text.ToString() == "D" || dlShiftID.Text.ToString() == "白班")
                {
                    bDao.ShiftID = "D";
                }
                else
                {
                    bDao.ShiftID = "N";
                }



                foreach (ListItem i in this.lstProd.Items)
                {
                    if (i.Selected)
                    {
                        bDao.ListProdID.Add(i.Value);
                    }
                }

                foreach (ListItem i in this.lstSubLineId.Items)
                {
                    if (i.Selected)
                    {
                        bDao.ListSubLineID.Add(i.Value);
                    }
                }
                if (DlProdGroup.Text.ToString().Trim() !="")
                {
                    bDao.ProdGroup = DlProdGroup.Text.ToString().Trim();
                }
                //bDao.ProdID = DlProdID.Text.ToString().Trim();
                //bDao.SubLineID = DlSubLine.Text.ToString().Trim();
                bDao.TeamID = DlTeamID.Text.ToString().Trim();
                bDao.TeamMode = dlTeamMode.Text.ToString().Trim();
                bDao.LotType = DlLotType.SelectedValue.ToString().Trim();
                bDao.Workctr = DLWorkctr.SelectedValue.ToString().Trim();

                DataSet ds= bDao.GetRMDD();

                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReport = this.ReportViewer1.LocalReport;
                localReport.ReportPath = @"RDLC\RMDD2.rdlc";

                ReportDataSource rdSource = new ReportDataSource();
                rdSource.Name = "RMDD2";
                rdSource.Value = ds.Tables[0];
                localReport.DataSources.Clear();
                localReport.DataSources.Add(rdSource);
                this.ReportViewer1.LocalReport.Refresh();
                //this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //LocalReport localReport = this.ReportViewer1.LocalReport;
                //localReport.ReportPath = @"RDLC\DailyInputAndOutput2.rdlc";

                //ReportDataSource rdSource = new ReportDataSource();
                //rdSource.Name = "localReport";
                //rdSource.Value = ds.Tables[0];
                //localReport.DataSources.Clear();
                //localReport.DataSources.Add(rdSource);
                
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }            
        }

        #region Bind
        private void BindProdID()
        {
            try
            {
                ProdID prodid = new ProdID();
                DataSet ds = new DataSet();
                prodid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = prodid.GetProdID();
                //this.DlProdID.DataSource = ds.Tables[0];
                //this.DlProdID.DataTextField = "PRODID";
                //this.DlProdID.DataValueField = "PRODID";
                //this.DlProdID.DataBind();
                //this.DlProdID.Items.Insert(0," ");
                this.lstProd.Items.Clear();
                this.lstProd.DataSource = ds.Tables[0];
                this.lstProd.DataTextField = "PRODID";
                this.lstProd.DataValueField = "PRODID";
                this.lstProd.DataBind();
                this.lstProd.ClearSelection();
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        private void BindSubLineID()
        {
            try
            {
                SubLineID sublineid = new SubLineID();
                DataSet ds = new DataSet();

                sublineid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = sublineid.GetM1M2M3SubLineID();

                this.lstSubLineId.Items.Clear();
                this.lstSubLineId.DataSource = ds.Tables[0];
                this.lstSubLineId.DataTextField = "SUBLINEID";
                this.lstSubLineId.DataValueField = "SUBLINEID";
                this.lstSubLineId.DataBind();
                this.lstSubLineId.ClearSelection();

                //this.DlSubLine.DataSource = ds.Tables[0];
                //this.DlSubLine.DataTextField = "SUBLINEID";
                //this.DlSubLine.DataValueField = "SUBLINEID";
                //this.DlSubLine.DataBind();
                //this.DlSubLine.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        private void BindTeamID()
        {
            try
            {
                TeamID teamid = new TeamID();
                DataSet ds = new DataSet();

                teamid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = teamid.GetTeamID();
                this.DlTeamID.DataSource = ds.Tables[0];
                this.DlTeamID.DataTextField = "TEAMID";
                this.DlTeamID.DataValueField = "TEAMID";
                this.DlTeamID.DataBind();
                this.DlTeamID.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        private void BindLotType()
        {
            try
            {
                LOTTYPE lottype = new LOTTYPE();
                DataSet ds = new DataSet();

                lottype.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = lottype.GetLotType();
                this.DlLotType.DataSource = ds.Tables[0];
                this.DlLotType.DataTextField = "LOTTYPE";
                this.DlLotType.DataValueField = "LOTTYPE";
                this.DlLotType.DataBind();
                this.DlLotType.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }

        }

        //private void BindUserID()
        //{
        //    try
        //    {
        //        UserID userid = new UserID();
        //        DataSet ds = new DataSet();
        //        userid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
        //        ds = userid.GetUserID();
        //        this.DLUserID.DataSource = ds.Tables[0];
        //        this.DLUserID.DataTextField = "USERID";
        //        this.DLUserID.DataValueField = "USERID";
        //        this.DLUserID.DataBind();
        //        this.DLUserID.Items.Insert(0, " ");

        //    }
        //    catch (Exception ex)
        //    {
        //         this.lblErrMsg.Text = ex.Message;
        //    }

        //}

        private void BindProdGroup()
        {
            try
            {
                ProdID prodid = new ProdID();
                DataSet ds = new DataSet();
                prodid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = prodid.GetProdGroup();
                this.DlProdGroup.DataSource = ds.Tables[0];
                this.DlProdGroup.DataTextField = "PRODGROUP";
                this.DlProdGroup.DataValueField = "PRODGROUP";
                this.DlProdGroup.DataBind();
                this.DlProdGroup.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        private void BindWorkctr()
        {
            try
            {

                SubLineID sublineid = new SubLineID();
                DataSet ds = new DataSet();

                sublineid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = sublineid.GetM1M2M3WorkCtr();
                this.DLWorkctr.DataSource = ds.Tables[0];
                this.DLWorkctr.DataTextField = "WORKCTR";
                this.DLWorkctr.DataValueField = "WORKCTR";
                this.DLWorkctr.DataBind();
                this.DLWorkctr.Items.Insert(0, " ");


            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message.ToString();
            }

        }
        #endregion

        protected void chkAllSubLineIDs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkAllSubLineIDs.Checked)
                {
                    foreach (ListItem i in this.lstSubLineId.Items)
                    {
                        i.Selected = true;
                    }
                }
                else
                {
                    foreach (ListItem i in this.lstSubLineId.Items)
                    {
                        i.Selected = false;
                    }
                }
            }

            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message.ToString();
            }
        }

        protected void chkAllProd_CheckedChanged(object sender, EventArgs e)
        {
            try
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

            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message.ToString();
            }
        }



    }
}
