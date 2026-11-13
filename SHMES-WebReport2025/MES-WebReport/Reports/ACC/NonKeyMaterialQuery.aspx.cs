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
using System.Text;

namespace WebReport.Reports
{
    public partial class NonKeyMaterialQuery : System.Web.UI.Page
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
                this.SetStationList();
                this.SetProdIdList();
                this.BindGroup();
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
            string strSql = @"SELECT DISTINCT WORKCTR FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY WORKCTR ORDER BY WORKCTR";
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
            string strSql = @"SELECT DISTINCT PRODID FROM MPIM WHERE FABID= '" + ((UserInfo)Session["UserInfo"]).FabId + "' GROUP BY PRODID ORDER BY PRODID";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstProdId.DataSource = dt;
            this.lstProdId.DataTextField = "PRODID";
            this.lstProdId.DataValueField = "PRODID";
            this.lstProdId.DataBind();
            this.lstProdId.ClearSelection();
        }


        /// <summary>
        /// 綁定群組資料
        /// </summary>
        private void BindGroup()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT DISTINCT PRODGROUP FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";

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
                this.dlGroup.Items.Insert(0, "ALL");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string prodid = string.Empty;
            string workctr = string.Empty;

            foreach (ListItem i in lstProdId.Items)
            {
                if (i.Selected)
                {
                    prodid += "'" + i.Text.Trim() + "',";
                }
            }

            foreach (ListItem i in lstStations.Items)
            {
                if (i.Selected)
                {
                    workctr += "'" + i.Text.Trim() + "',";
                }
            }
            if (this.lstStations.SelectedItem == null)
            {
                bool b = false;
                foreach (ListItem i in this.lstStations.Items)
                {
                    if (i.Selected)
                    {
                        b = true;
                        break;
                    }
                }
                if (!b)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "站點未選擇！";
                    //this.Response.Write("<script language=javascript>window.alert('請選擇站點！');</script>");
                    ReportViewer1.Visible = false;
                    return;
                }

            }

            if (this.lstProdId.SelectedItem == null)
            {
                bool a = false;
                foreach (ListItem i in this.lstProdId.Items)
                {
                    if (i.Selected)
                    {
                        a = true;
                        break;
                    }
                }
                if (!a)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "機種未選擇！";
                    //this.Response.Write("<script language=javascript>window.alert('請選擇機種！');</script>");
                    ReportViewer1.Visible = false;
                    return;
                }

            }
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder stb = new StringBuilder();
            stb.Append("select A.prodid,E.prodgroup,A.cworkctr,A.entitysts,sum(A.entitytqty) as entitytqty,C.partid,C.parttype,D.partqty,sum(A.entitytqty * D.partqty) as Qty from New_wltm A,New_sapswmpd B,mnkpam C,mnkmqm D,mpim E where A.fabid=B.fabid and A.fabid=C.fabid and A.fabid=D.fabid and A.prodid=E.prodid and A.wonbr=B.wonbr and B.partid=C.partid and A.prodid=D.prodid and A.cworkctr=D.workctr and C.parttype=D.parttype and D.partid= C.partid and A.fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' and A.hideflg='0' ");
            if (prodid.Length != 0)
            {
                prodid = prodid.Substring(0, prodid.Length - 1);
                stb.Append(" and A.prodid in (" + prodid + ")");
            }
            if (workctr.Length != 0)
            {
                workctr = workctr.Substring(0, workctr.Length - 1);
                stb.Append(" and  A.cworkctr in (" + workctr + ")");
            }
            if (dlGroup.Text.Trim() != "ALL")
            {
                stb.Append(" and  E.prodgroup ='" + dlGroup.Text.Trim() + "'");
            }
            stb.Append("group by A.prodid,E.prodgroup,A.cworkctr,A.entitysts,C.partid,C.parttype,D.partqty");

            DataTable dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];

            if (dt.Rows.Count == 0)
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "查無資料!";
                ReportViewer1.Visible = false;
            }
            else
            {
                lblErrMsg.Visible = false;
                ReportViewer1.Visible = true;
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer1.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\NonKeyMaterialQuery.rdlc";
                ReportDataSource rsd = new ReportDataSource("NonKeyMaterialQuery", dt);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
            }
        }

        protected void chkAllStations_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllStations.Checked)
            {
                foreach (ListItem i in lstStations.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstStations.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkAllProdID_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllProdID.Checked)
            {
                foreach (ListItem j in lstProdId.Items)
                {
                    j.Selected = true;
                }
            }
            else
            {
                foreach (ListItem j in lstProdId.Items)
                {
                    j.Selected = false;
                }
            }
        }

        protected void lstStations_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
