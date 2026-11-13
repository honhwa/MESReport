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

namespace Report.Reports.MFG
{
    public partial class RMDDPMModifySave : System.Web.UI.Page
    {
        #region param
        private static string strReportDate;
        private static string strProdID;
        private static string strShiftID;
        private static string strSubLineID;
        private static string strUserID;
        private static string strTeamID;
        private static string strLotType;
        private static string strTeamMode;
        private static string strManPower;
        private static string strWorkingHour;
        private DBConn dbConn = new DBConn();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strReportDate=this.Request.QueryString["REPORTDATE"].ToString().Trim();
                strProdID=this.Request.QueryString["PRODID"].ToString().Trim();
                strShiftID=this.Request.QueryString["SHIFTID"].ToString().Trim();
                strSubLineID=this.Request.QueryString["SUBLINEID"].ToString().Trim();
                strUserID=this.Request.QueryString["USERID"].ToString().Trim();
                strTeamID=this.Request.QueryString["TEAMID"].ToString().Trim();
                strTeamMode=this.Request.QueryString["TEAMMODE"].ToString().Trim();
                strLotType=this.Request.QueryString["LOTTYPE"].ToString().Trim();
                strWorkingHour=this.Request.QueryString["WORKINGHOUR"].ToString().Trim();
                strManPower=this.Request.QueryString["MANPOWER"].ToString().Trim();

                txtReportDate.Text = strReportDate;
                txtprodid.Text = strProdID;
                txtShiftID.Text = strShiftID;
                txtSubLineID.Text = strSubLineID;
                txtuserid.Text = strUserID;
                txtTeamID.Text = strTeamID;
                txtTeamMode.Text = strTeamMode;
                txtLotType.Text = strLotType;
                txtWORKINGHOUR.Text = strWorkingHour;
                txtMANPOWER.Text = strManPower;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtWORKINGHOUR.Text = "";
            this.txtMANPOWER.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strSql = string.Empty;
                string strFabID = string.Empty;

                strFabID = ((UserInfo)Session["UserInfo"]).FabId;

                if ((txtWORKINGHOUR.Text.ToString().Trim() == strWorkingHour.ToString().Trim()) && (txtMANPOWER.Text.ToString().Trim() == strManPower.ToString().Trim()))
                {
                    this.lblErrMsg.Text = "資料沒有變化";

                }
                else
                {

                    strSql = " UPDATE RMDD SET " +
                             " WORKINGHOUR=" + this.txtWORKINGHOUR.Text.ToString() + " ," +
                             " MANPOWER=" + this.txtMANPOWER.Text + "" +
                             " WHERE FABID='" + strFabID + " ' " +
                             " AND REPORTDATE='" + txtReportDate.Text + "' " +
                             " AND PRODID='" + txtprodid.Text.ToString().Trim() + "' " +
                             " AND SHIFTID='" + txtShiftID.Text.ToString().Trim() + "' " +
                             " AND SUBLINEID='" + txtSubLineID.Text.ToString().Trim() + "' " +
                             //" AND REPORTUSERID='" + txtuserid.Text.ToString().Trim() + "' " +
                             " AND TEAMID='" + txtTeamID.Text.ToString().Trim() + "' " +
                             " AND TEAMMODE='" + txtTeamMode.Text.ToString().Trim() + "' ";
                    if (this.dbConn.ExecuteSql(strSql) == true)
                    {
                        lblErrMsg.Text = "作業成功!";
                    }
                    else
                    {
                        lblErrMsg.Text = "作業失敗!";
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }



    }
}
