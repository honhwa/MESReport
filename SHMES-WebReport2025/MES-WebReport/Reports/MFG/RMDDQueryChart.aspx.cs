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
    public partial class RMDDQueryChart : System.Web.UI.Page
    {
        #region param
        private DBConn dbConn = new DBConn();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }

                BindMonth();
                BindWeek();
                BindSubLineID();
                BindProdID();
                BindTeamID();
                BindProdGroup();
                BindWorkctr();
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

            i = 0;
            for (IYear = 2008; IYear <= 2009; IYear++)
            {
                for (IMonth = 1; IMonth <= 12; IMonth++)
                {
                    if (IMonth < 10)
                    {
                        this.dlfrMonth.Items.Insert(i, IYear.ToString() + "0" + IMonth.ToString());
                        this.dltoMonth.Items.Insert(i, IYear.ToString() + "0" + IMonth.ToString());
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

        private void BindProdGroup()
        {
            try
            {
                ProdID prodid = new ProdID();
                DataSet ds = new DataSet();

                prodid.FabID = ((UserInfo)Session["UserInfo"]).FabId;
                ds = prodid.GetProdGroup();

                this.dlProdGrop.DataSource = ds.Tables[0];
                this.dlProdGrop.DataTextField = "PRODGROUP";
                this.dlProdGrop.DataValueField = "PRODGROUP";
                this.dlProdGrop.DataBind();
                this.dlProdGrop.Items.Insert(0, " ");

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
            DataSet ds = new DataSet();
            try
            {
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                int i = 0;
                Boolean blReturnFlg = false;
                string strExcelFormatName = @"../../EXCModule/RMDD.xls";
                string strSavePathName = @"../../ExportFile/";
                string strSaveFilename = "";
                string strSheetName = "Sheet1";

                lblErrMsg.Text = "";
                BaseExcel baseExcel = new BaseExcel(strExcelFormatName, strSheetName);

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

                if ((rbtnmonth.Checked == false) && (rbtnweek.Checked == false))
                {
                    lblErrMsg.Text = "Please Choose Month Or Week!";
                    return;
                }


                if ((this.DlTeamID.Text.ToString().Trim()=="") && (rblDataGroup.SelectedValue.ToString().Trim()=="0"))
                {
                    lblErrMsg.Text = "Please Choose Team ID!";
                    return;
                }

                
                if ((this.DlSubLine.Text.ToString().Trim()=="") && (rblDataGroup.SelectedValue.ToString().Trim()=="1"))
                {
                    lblErrMsg.Text = "Please Choose Sub Line ID!";
                    return;
                }

                
                if ((this.DlProdID.Text.ToString().Trim()=="") && (rblDataGroup.SelectedValue.ToString().Trim()=="2"))
                {
                    lblErrMsg.Text = "Please Choose Product ID!";
                    return;
                }

                if ((this.dlProdGrop.Text.ToString().Trim() == "") && (rblDataGroup.SelectedValue.ToString().Trim() == "3"))
                {
                    lblErrMsg.Text = "Please Choose Product Group!";
                    return;
                }

                baseRmdd.DataGroup = rblDataGroup.SelectedValue.ToString().Trim();
                baseRmdd.DataFormat = rblDataFormat.SelectedValue.ToString().Trim();
                baseRmdd.ProdGroup = dlProdGrop.Text.ToString().Trim();
                baseRmdd.Workctr = DLWorkctr.Text.ToString().Trim();
                ds = baseRmdd.GetMaterialChart();

                //Excel Process Begin
                for (i = 2; i <= ds.Tables[0].Rows.Count; i++)
                {
                    baseExcel.ExcelSheet.Cells[i, 1] = ds.Tables[0].Rows[i - 2]["TEAMMODE"].ToString().Trim();
                    baseExcel.ExcelSheet.Cells[i, 2] = ds.Tables[0].Rows[i - 2]["STRID"].ToString().Trim();
                    baseExcel.ExcelSheet.Cells[i, 3] = ds.Tables[0].Rows[i - 2]["REPORTDATE"].ToString().Trim();
                    baseExcel.ExcelSheet.Cells[i, 4] = ds.Tables[0].Rows[i - 2]["QTY"].ToString().Trim();
                    baseExcel.ExcelSheet.Cells[i, 5] = ds.Tables[0].Rows[i - 2]["RATE"].ToString().Trim();
                }

                strSaveFilename = "RMDD" + Convert.ToString(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                strSavePathName = strSavePathName + strSaveFilename;
                if (baseExcel.ExcelSave(strSavePathName + ".xls") == true)
                {

                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(baseExcel.ExcelSheet);
                    //baseExcel.ExcelSheet = null;
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(baseExcel.ExcelWork);
                    //baseExcel.ExcelWork = null;
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(ThisApplication);
                    //ThisApplication = null;
                    baseExcel.ExcelClose();
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    baseExcel.DeleteExcelFile("RMDD");
                    this.Response.Write("<script language=javascript>window.open('../LoadExcel.aspx?ExcelName=" + strSaveFilename + "');</script>");
                }
                else
                {
                    lblErrMsg.Text = "Save Excel Error";
                }

                //Excel Process End

            }
            catch (Exception ex)
            {
               lblErrMsg.Text=ex.Message.ToString();
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
