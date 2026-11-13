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
using System.Text;
using System.Drawing;


namespace Report.Reports.EMS
{
    public partial class SubLine24STS : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        private string strEQID;
        private string strBeginTime;
        private string strEndTime;
        private SubLines sls = new SubLines();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }

                this.SetSubLineDataSource();
                this.BindColor();
                this.GetTime();
                this.SetEQID();
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }



        private void SetSubLineDataSource()
        {

            this.lstsls.Items.Clear();
            string strSql = @"SELECT DISTINCT WORKCTR FROM MSLM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'  ";
            strSql += " AND WORKCTR IN (SELECT WORKCTR FROM MWCM WHERE WORKTYPE NOT IN ('MR','CR','CS','MS','RS','P2','FB') ) ORDER BY WORKCTR  ";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstsls.DataSource = dt;
            this.lstsls.DataTextField = "WORKCTR";
            this.lstsls.DataValueField = "WORKCTR";
            this.lstsls.DataBind();
            this.lstsls.ClearSelection();
        }
        private void SetEQID()
        {
            try
            {
         
                string strFilter = string.Empty;

                if (lstsls.SelectedIndex >= 0)
                {
                    StringBuilder sbSLS = new StringBuilder();
                    foreach (ListItem li in this.lstsls.Items)
                    {
                        if (li.Selected)
                        {
                            sbSLS.Append("'").Append(li.Value).Append("'");
                        }
                    }
                    if (sbSLS.ToString().Length > 0)
                    {
                        strFilter += " AND WORKCTR IN (" + sbSLS.Replace("''", "','") + ") ";
                    }
                }
                string strSql = @"SELECT DISTINCT SUBLINEID FROM MSLM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' " + strFilter + " AND SUBLINEID NOT IN ('BCXXX-01','BMXXX-01','BMACF001','BMCOG002') ORDER BY SUBLINEID  ";
                DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count <= 0)
                {

                    this.lstEqID.Items.Clear();
                    this.CheckEqID.Enabled = false;
                }
                else
                {
                    this.CheckEqID.Enabled = true;
                    this.lstEqID.Items.Clear();
                    this.lstEqID.DataSource = dt;
                    this.lstEqID.DataTextField = "SUBLINEID";
                    this.lstEqID.DataValueField = "SUBLINEID";
                    this.lstEqID.DataBind();
                    this.lstEqID.ClearSelection();
                }


            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text = ex.Message;
            }
        }

        private void GetTime()
        {
            string strSql = @" SELECT  GETDATE() AS EDT,GETDATE()-1 AS  SDT";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            BeginTime = dt.Rows[0][1].ToString();
            EndTime = dt.Rows[0][0].ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string strSqlEqID = string.Empty;
                string strSqlEq24 = string.Empty;
                string strFilter = string.Empty;
                bool bHasError = false;
                this.lblErrMsg.Text = "";

                GetTime();
                strFilter = @" FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ";

                if (lstsls.SelectedIndex >= 0)
                {
                    StringBuilder sbSLS = new StringBuilder();
                    foreach (ListItem li in this.lstsls.Items)
                    {
                        if (li.Selected)
                        {
                            sbSLS.Append("'").Append(li.Value).Append("'");
                        }
                    }
                    if (sbSLS.ToString().Length > 0)
                    {
                        strFilter += " AND WORKCTR IN (" + sbSLS.Replace("''", "','") + ") ";
                        bHasError = true;
                    }
                }

                if (lstEqID.SelectedIndex >= 0)
                {
                    StringBuilder sbEQID = new StringBuilder();
                    foreach (ListItem li in this.lstEqID.Items)
                    {
                        if (li.Selected)
                        {
                            sbEQID.Append("'").Append(li.Value).Append("'");
                        }
                    }
                    if (sbEQID.ToString().Length > 0)
                    {
                        strFilter += " AND SUBLINEID IN (" + sbEQID.Replace("''", "','") + ") ";
                        bHasError = true;
                    }
                }

                if (bHasError == false)
                {
                    this.lblErrMsg.Text = "Please Choose Workctr Or SubLineID!";
                    return;
                }
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                strSqlEqID = @" SELECT WORKCTR,SUBLINEID, 
                            SUM(CASE WHEN EQCCD='R' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS DownTIME,
                            SUM(CASE WHEN EQCCD='D' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS IDLETIME,
                            SUM(CASE WHEN EQCCD='E' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS ENGTIME,
                            SUM(CASE WHEN EQCCD='N' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS NonOperationTIME,
                            SUM(CASE WHEN EQCCD='O' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS OtherTIME,
                            SUM(CASE WHEN EQCCD='P' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS PMTIME,
                            SUM(CASE WHEN EQCCD='S' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS ENGTIME,
                            SUM(CASE WHEN EQCCD='T' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS PartialDownTIME,
                            SUM(CASE WHEN EQCCD='W' THEN  
                            DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()  then GETDATE()-1 ELSE STARTDATE 
                            END ,ENDDATE) ELSE 0 END ) AS WaitTIME
                            FROM 
                            (SELECT WORKCTR,SUBLINEID,PREVSTATUS AS EQCD,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE
                            FROM New_ESLSH WHERE ";

                strSqlEqID += strFilter;
                strSqlEqID += @" AND ENDDATE>GETDATE()-1
                             UNION
                             SELECT WORKCTR,SUBLINEID,SUBLINESTATUS AS EQCD,EQCCD,TRANSDATE AS STARTDATE,GETDATE() AS ENDATE
                             FROM New_ESLS  WHERE  ";
                strSqlEqID += strFilter;
                strSqlEqID += @")A GROUP BY WORKCTR,SUBLINEID ORDER BY WORKCTR,SUBLINEID  ";

                DataTable dtEqID = this.dbConn.GetDataSetBySql(strSqlEqID).Tables[0];
                GridView1.DataSource = (DataView)dtEqID.DefaultView;
                GridView1.DataBind();

                strSqlEq24 = @" SELECT WORKCTR,SUBLINEID,A.EQCD,A.EQCCD AS EQCCD,B.EQCCDCOLOR AS COLOR,
                                    CONVERT(VARCHAR(16),CASE WHEN STARTDATE<(GETDATE()-1) then GETDATE()-1 ELSE STARTDATE 
                                    END ,120)AS STARTDATE, 
                                    DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()-1  then GETDATE()-1 ELSE STARTDATE 
                                    END ,ENDDATE) AS DTIME,
                                    CAST(ROUND(
                                    DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()-1  then GETDATE()-1 ELSE STARTDATE 
                                    END ,ENDDATE)/480,0)  AS INT) AS DBUFFER,CONVERT(VARCHAR(16),GETDATE()-1,120) AS SDT, 
                                    CONVERT(VARCHAR(16),GETDATE(),120) AS EDT,'子狀態:'+A.EQCD+C.EQCDDESC+'(母狀態:'+A.EQCCD+'<'+C.EQCCDESC+'>) 站別:'+WORKCTR AS CMT  
                                    FROM 
                                    (SELECT FABID,WORKCTR,SUBLINEID,PREVSTATUS AS EQCD,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE
                                    FROM New_ESLSH WHERE ";
                strSqlEq24 += strFilter;
                strSqlEq24 += @" AND ENDDATE>GETDATE()-1
                                    UNION
                                    SELECT FABID,WORKCTR,SUBLINEID,SUBLINESTATUS AS EQCD,EQCCD,TRANSDATE AS STARTDATE,GETDATE() AS ENDATE
                                     FROM New_ESLS  WHERE  ";
                strSqlEq24 += strFilter;
                strSqlEq24 += @" )A  INNER JOIN  MECCU B ON (A.EQCCD=B.EQCCD) 
                    LEFT JOIN (SELECT  FABID,EQCCD,EQCD,EQCCDESC,EQCDDESC FROM MECM 
                            GROUP BY  FABID,EQCCD,EQCD,EQCCDESC,EQCDDESC) C ON(A.FABID=C.FABID AND A.EQCCD=C.EQCCD AND A.EQCD=C.EQCD) 
                   WHERE CAST(ROUND( DATEDIFF(SS,CASE WHEN STARTDATE<GETDATE()-1  then GETDATE()-1 ELSE STARTDATE END ,ENDDATE)/480,0)  AS INT)>0
                    ORDER BY WORKCTR,SUBLINEID,STARTDATE  ";

                DataTable dtSqlEq24 = this.dbConn.GetDataSetBySql(strSqlEq24).Tables[0];

                for (int i = 0; i < dtEqID.Rows.Count; i++)/*Process Every EQID*/
                {

                    int aa;
                    int bb;
                    int cc;
                    bb = 0;
                    cc = 180;

                    DataRow[] dr = dtSqlEq24.Select(" ( SUBLINEID= '" + dtEqID.Rows[i]["SUBLINEID"].ToString() + "' )", "SUBLINEID,STARTDATE DESC");
                    for (int j = 0; j < dr.Length; j++)
                    {
                        if (j < dr.Length - 1)
                        {
                            for (aa = cc; aa > (cc-bb - Convert.ToInt32(dr[j]["DBUFFER"].ToString())); aa--)
                            {   
                                BeginTime = dr[j]["SDT"].ToString();
                                EndTime = dr[j]["EDT"].ToString();

                                if (aa < 1)
                                {

                                    break;
                                }


                                Color cl1 = ColorTranslator.FromOle(int.Parse(dr[j]["COLOR"].ToString().Trim()));
                                GridView1.Rows[i].Cells[aa].BackColor = cl1;
                                GridView1.Rows[i].Cells[aa].ToolTip = dr[j]["CMT"].ToString();

                                //if (aa >= 180)
                                //{
                                //    break;
                                //}

                                //if (j == (Convert.ToInt32(dr.Length) - 1) && (bb + Convert.ToInt32(dr[j]["DBUFFER"].ToString())) == 179 && aa == 178)
                                //{
                                //    Color cl1 = ColorTranslator.FromOle(int.Parse(dr[j]["COLOR"].ToString().Trim()));
                                //    GridView1.Rows[i].Cells[aa + 1].BackColor = cl1;
                                //    GridView1.Rows[i].Cells[aa + 1].ToolTip = dr[j]["CMT"].ToString();


                                //}
                                //else
                                //{
                                //    Color cl2 = ColorTranslator.FromOle(int.Parse(dr[j]["COLOR"].ToString().Trim()));

                                //    GridView1.Rows[i].Cells[aa + 1].BackColor = cl2;
                                //    GridView1.Rows[i].Cells[aa + 1].ToolTip = dr[j]["CMT"].ToString();
                                //}

                            }
                        }
                        else
                        {
                            for (aa = cc; aa >0; aa--)
                            {
                                BeginTime = dr[j]["SDT"].ToString();
                                EndTime = dr[j]["EDT"].ToString();

                                if (aa < 1)
                                {

                                    break;
                                }

                                Color cl2 = ColorTranslator.FromOle(int.Parse(dr[j]["COLOR"].ToString().Trim()));

                                GridView1.Rows[i].Cells[aa].BackColor = cl2;
                                GridView1.Rows[i].Cells[aa].ToolTip = dr[j]["CMT"].ToString();

                            }
                        }

                        bb = bb + Convert.ToInt32(dr[j]["DBUFFER"].ToString());
                        cc = 180 - bb;
                        
                    } 

                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }

        private string EQID
        {
            get
            {
                return this.strEQID;
            }
            set
            {
                this.strEQID = value;
            }
        }

        private string BeginTime
        {
            get
            {
                return this.strBeginTime;
            }
            set
            {
                this.strBeginTime = value;
            }
        }

        private string EndTime
        {
            get
            {
                return this.strEndTime;
            }
            set
            {
                this.strEndTime = value;

            }

        }

        private void BindColor()
        {
            DataTable dt = new DataTable();
            dt = this.sls.dtGetColor();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Color cl = ColorTranslator.FromOle(int.Parse(dt.Rows[i]["EQCCDCOLOR"].ToString().Trim()));
                switch (dt.Rows[i]["EQCCD"].ToString().Trim())
                {
                    /*
                    D	Down
                    E	ENG
                    I	IDLE
                    N	NonOperation
                    O	Other
                    P	PM
                    R	RUN
                    S	Setup
                    T	PartialDown 
                    W	Wait
                    */
                    case "D":
                        this.lblDown.BackColor = cl;
                        break;
                    case "E":
                        this.lblENG.BackColor = cl;
                        break;
                    case "I":
                        this.lblIDLE.BackColor = cl;
                        break;
                    case "N":
                        this.lblNonOperation.BackColor = cl;
                        break;
                    case "O":
                        this.lblOther.BackColor = cl;
                        break;
                    case "P":
                        this.lblPM.BackColor = cl;
                        break;
                    case "R":
                        this.lblRUN.BackColor = cl;
                        break;
                    case "S":
                        this.lblSetup.BackColor = cl;
                        break;
                    case "T":
                        this.lblPartial.BackColor = cl;
                        break;
                    case "W":
                        this.lblWait.BackColor = cl;
                        break;

                }

            }
        }


        protected void chkAllSls_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllSls.Checked)
            {
                foreach (ListItem i in this.lstsls.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstsls.Items)
                {
                    i.Selected = false;
                }
            }
            SetEQID();
        }



        protected void CheckEqID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckEqID.Checked)
            {
                foreach (ListItem i in this.lstEqID.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstEqID.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            e.Row.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#33cc33'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 1; i < 181; i++)
                {
                    e.Row.Cells[i].ColumnSpan = 180;
                    if (i < 180)
                    {
                        e.Row.Cells[i + 1].Visible = false;
                    }
                    //e.Item.Cells[i].Text = "" + Convert.ToDateTime(this.TextBox4.Text).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-22).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-20).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-18).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-16).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-14).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-12).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-10).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-8).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-6).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-4).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).AddHours(-2).ToString("HH:mm") + "&nbsp;&nbsp;" + Convert.ToDateTime(this.TextBox5.Text).ToString("HH:mm") + " ";
                    e.Row.Cells[i].Text = "" + Convert.ToDateTime(strBeginTime).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp" +
                        Convert.ToDateTime(strEndTime).AddHours(-22).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-20).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-18).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-16).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-14).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-12).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-10).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-8).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-6).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-4).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).AddHours(-2).ToString("HH:mm") + "&nbsp;&nbsp;&nbsp;" +
                        Convert.ToDateTime(strEndTime).ToString("HH:mm") + " ";
                }
            }
        }

        protected void lstsls_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEQID();
        }

        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.Charset = "GB2312";
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=work.xls");
        //    Response.ContentEncoding = System.Text.Encoding.UTF7;
        //    Response.ContentType = "application/ms-excel";
        //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        //    HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);

        //     this.GridView1.RenderControl(oHtmlTextWriter);

        //    Response.Output.Write(oStringWriter.ToString());
        //    Response.Flush();
        //    Response.End();
        //}

    }
}
