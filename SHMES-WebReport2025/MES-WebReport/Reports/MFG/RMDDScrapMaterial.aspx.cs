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
using Report.Sources;
using Report.Library;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports.MFG
{
    public partial class RMDDScrapMaterial : System.Web.UI.Page
    {
        #region Param
        private DBConn dbConn = new DBConn();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }

                BindMonth();
                BindWeek();
                //BindWorctr();
                BindSubLineID();
                BindProdID();
                BindTeamID();
                rbtnmonth.Checked = true;
                rbtnweek.Checked = false;
                dlfrWeek.Enabled = false;
                dltoWeek.Enabled = false;
                dlfrMonth.Enabled = true;
                dltoMonth.Enabled = true;

            }
        }


        #region Bind Data
        private void BindMonth()
        {
            int i;
            int IYear;
            int IMonth;

            i=0;
            for (IYear=2008; IYear<=2009 ; IYear++)
            {
                for (IMonth = 1; IMonth <= 12; IMonth++)
                {
                    if (IMonth < 10)
                    {
                        this.dlfrMonth.Items.Insert(i, IYear.ToString() +"0"+IMonth.ToString());
                        this.dltoMonth.Items.Insert(i, IYear.ToString() + "0" +IMonth.ToString());
                    }
                    else
                    {
                        this.dlfrMonth.Items.Insert(i, IYear.ToString() + IMonth.ToString());
                        this.dltoMonth.Items.Insert(i, IYear.ToString() + IMonth.ToString());
                    }
                    i++;
                }   
            }
        }

        private void BindWeek()
        {
            int i;
            int IWeek;

            i = 0;
            for (IWeek = 1; IWeek <= 54; IWeek++)
            {
                dlfrWeek.Items.Insert(i, IWeek.ToString());
                dltoWeek.Items.Insert(i, IWeek.ToString());
                i++;
            }

        }


        private void BindProdID()
        {
            try
            {
                ProdID prodid = new ProdID();
                DataSet ds = new DataSet();

                prodid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = prodid.GetProdID();

                this.DlProdID.DataSource = ds.Tables[0];
                this.DlProdID.DataTextField = "PRODID";
                this.DlProdID.DataValueField = "PRODID";
                this.DlProdID.DataBind();
                this.DlProdID.Items.Insert(0, " ");

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

                this.DlSubLine.DataSource = ds.Tables[0];
                this.DlSubLine.DataTextField = "SUBLINEID";
                this.DlSubLine.DataValueField = "SUBLINEID";
                this.DlSubLine.DataBind();
                this.DlSubLine.Items.Insert(0, " ");
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

        #endregion
        
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();

                BaseRMDD baseRmdd = new BaseRMDD();
                baseRmdd.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                baseRmdd.SubLineID = DlSubLine.Text.ToString().Trim();
                baseRmdd.ProdID = DlProdID.Text.ToString().Trim();
                baseRmdd.ProdGroup = dlProdGrop.Text.ToString().Trim();
                baseRmdd.TeamID = DlTeamID.Text.ToString().Trim();
                baseRmdd.TeamMode = dlTeamMode.Text.ToString().Trim();
                if (rbtnmonth.Checked == true)
                {
                    baseRmdd.FrMonth = dlfrMonth.Text.ToString().Trim();
                    baseRmdd.ToMonth = dltoMonth.Text.ToString().Trim();
                }
                if (rbtnweek.Checked == true)
                {
                    baseRmdd.FrWeek = dlfrWeek.Text.ToString().Trim();
                    baseRmdd.ToWeek = dltoWeek.Text.ToString().Trim();
                }

                if ((rbtnmonth.Checked ==false) && (rbtnweek.Checked ==false)) 
                {
                    lblErrMsg.Text = "Please Choose Month Or Week!";
                    return;
                }


                ds1 = baseRmdd.GetRMDDSMTABLE();

                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReport1 = this.ReportViewer1.LocalReport;
                localReport1.ReportPath = @"RDLC\RMDDSMATERIAL.rdlc";
                ReportDataSource rsSource1 = new ReportDataSource();
                rsSource1.Name = "RMDDSMATERIAL";
                rsSource1.Value = ds1.Tables[0];
                localReport1.DataSources.Clear();
                localReport1.DataSources.Add(rsSource1);


                ds2 = baseRmdd.GetRMDDSMCHART();

                this.ReportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReport2 = this.ReportViewer2.LocalReport;
                localReport2.ReportPath = @"RDLC\RMDDSMC.rdlc";
                ReportDataSource rsSource2 = new ReportDataSource();
                rsSource2.Name = "RMDDSMC";
                rsSource2.Value = ds2.Tables[0];
                localReport2.DataSources.Clear();
                localReport2.DataSources.Add(rsSource2);
            }
            catch (Exception ex)
            {
               this.lblErrMsg.Text=ex.Message;
            }
        }

        protected void rbtnmonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnmonth.Checked == true)
            {
                rbtnweek.Checked = false;
                dlfrWeek.Enabled = false;
                dltoWeek.Enabled = false;
                dlfrMonth.Enabled = true;
                dltoMonth.Enabled = true;
            }
            else
            {
                rbtnweek.Checked = true;
                dlfrWeek.Enabled = true;
                dltoWeek.Enabled = true;
                dlfrMonth.Enabled = false;
                dltoMonth.Enabled = false;
            }
        }

        protected void rbtnweek_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnweek.Checked == true)
            {
                rbtnmonth.Checked = false;
                dlfrMonth.Enabled = false;
                dltoMonth.Enabled = false;
                dlfrWeek.Enabled = true;
                dltoWeek.Enabled = true;
            }
            else
            {
                rbtnmonth.Checked = true;
                dlfrMonth.Enabled = true;
                dltoMonth.Enabled = true;
                dlfrWeek.Enabled = false;
                dltoWeek.Enabled = false;

            }
        }


    }
}
