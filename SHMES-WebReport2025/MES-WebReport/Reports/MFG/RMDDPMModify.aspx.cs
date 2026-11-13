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

namespace Report.Reports.MFG
{
    public partial class RMDDPMModify : System.Web.UI.Page
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
                //this.BindUserID();
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

                this.DlProdID.DataSource = ds.Tables[0];
                this.DlProdID.DataTextField = "PRODID";
                this.DlProdID.DataValueField = "PRODID";
                this.DlProdID.DataBind();
                this.DlProdID.Items.Insert(0, "");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                this.DlSubLine.Items.Insert(0, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                this.DlTeamID.Items.Insert(0, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                this.DlLotType.Items.Insert(0, "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        //        this.DLUserID.Items.Insert(0, "");

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}
        #endregion

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblErrMsg.Text = string.Empty;
                string strErrorMsg = string.Empty;
                bool bHasError = false;
                string strSql = string.Empty;
                string strFabID = string.Empty;
                DateTime fr = new DateTime();

                if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
                {
                    strErrorMsg += "時間不能為空！\n";
                    bHasError = true;
                }

                if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "時間格式錯誤！\n";
                    bHasError = true;
                }

                //TimeSpan ts = this.txtFromTime - DateTime.Now;

                //if (ts.Seconds < 0)
                //{
                //    throw new Exception("開始時間小於結束時！");
                //}
                
                if (this.DlSubLine == null )
                {
                    throw new Exception("SubLine未選取！");
                }

                if (this.DlProdID == null )
                {
                    throw new Exception("機種未選取！");
                }

                if (this.DlTeamID == null)
                {
                    throw new Exception("Team ID未選取!");
                }

                //if (this.DLUserID == null)
                //{
                //    throw new Exception("User ID未選取!");
                //}

                if (this.dlShiftID == null)
                {
                    throw new Exception("Shift Type未選取!");
                }

                if (this.DlLotType == null)
                {
                    throw new Exception("Lot Type未選取!");
                }
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                strFabID = ((UserInfo)Session["UserInfo"]).FabId;

                strSql = " SELECT substring(convert(varchar,REPORTDATE,120),1,10) AS REPORTDATE,SHIFTID," +
                        " PRODID,PRODGROUP,REPORTUSERID AS USERID,TEAMID,TEAMMODE,SUBLINEID,LOTTYPE,WORKINGHOUR,MANPOWER" +
                        " FROM RMDD WHERE FABID='" + strFabID + "'  " +
                        " AND REPORTDATE = '" + this.txtFromTime.Text + "'"; 
                        if (this.DlSubLine.Text.ToString().Trim()!="")
                        {
                            strSql= strSql+" AND SUBLINEID='" + this.DlSubLine.Text.Trim() + "' ";
                        }

                        //if (this.DLUserID.Text.ToString().Trim()!="")
                        //{
                        //    strSql= strSql+ " AND REPORTUSERID='" + this.DLUserID.Text.Trim() + "' ";
                        //}                  
                        
                        if (this.DlTeamID.Text.ToString().Trim()!="")
                        {
                            strSql= strSql+  " AND TEAMID='" + this.DlTeamID.Text.Trim() + "' ";
                        }

                        if (this.DlProdID.Text.ToString().Trim() != "")
                        {
                            strSql = strSql + " AND PRODID='" + this.DlProdID.Text.Trim() + "'";
                        }

                        if (this.dlShiftID.Text.ToString().Trim() != "" &&  this.dlShiftID.Text.ToString().Trim() != "ALL")
                        {
                            strSql = strSql + " AND SHIFTID='" + this.dlShiftID.Text.Trim() + "'";
                        }

                        if (this.DlLotType.Text.ToString().Trim() != "")
                        {
                            strSql = strSql + " AND LOTTYPE='" + this.DlLotType.Text.Trim() + "'";
                        }


                ds = this.dbConn.GetDataSetBySql(strSql);
                GVRMDD.DataSource = (DataView)ds.Tables[0].DefaultView;
                GVRMDD.DataBind();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    this.lblErrMsg.Text = "查無資料!";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }



        protected void GVRMDD_DataBound(object sender, EventArgs e)
        {

            //e.Item.Attributes.Add("ondblclick", "window.open('CrystalReport/CryQWO.aspx?id=" + e.Item.Cells[2].Text + "&frdate=" + this.frtime + "&todate=" + this.totime + "')");	



        }

        protected void GVRMDD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
            //if (e.Row == ListItemType.AlternatingItem)
            //{
            //    e.Item.BackColor = Color.Gainsboro;
            //}
           // e.Row.Cells[1];
  
            e.Row.Attributes.Add("ondblclick", "window.open('RMDDPMModifySave.aspx?REPORTDATE="+e.Row.Cells[0].Text+"&PRODID="+e.Row.Cells[1].Text+"&SHIFTID="+e.Row.Cells[2].Text+"&USERID="+e.Row.Cells[3].Text+"&TEAMID="+e.Row.Cells[6].Text+"&TEAMMODE="+e.Row.Cells[9].Text+"&SUBLINEID="+e.Row.Cells[4].Text+"&LOTTYPE="+e.Row.Cells[5].Text+"&WORKINGHOUR="+e.Row.Cells[7].Text+"&MANPOWER="+e.Row.Cells[8].Text+"')");	

        }

        protected void GVRMDD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




    }
}
