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
using System.IO;

namespace WebReport.Reports
{
    public partial class WIPAnalysis : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public DataSet dsDetail = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.txtWotype.Text = "";
                this.SetProductIDDataSource();
                this.SetStationDataSource();
                this.SetModuleStationDataSource();
                this.SetProdGroupList();
                this.chkAll.Checked = false;
                this.chkAllProdGroup.Checked = false;
                this.chkAllProdID.Checked = false;
                this.CheckModuleALL.Checked = false;
            }
            //if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            //{
            //    Response.Write("No FabId!");
            //    Response.Redirect("../../Login.aspx");
            //}
        }

        private void SetStationDataSource()
        {
            this.chkAll.Checked = false;
            this.lstStation.Items.Clear();
            string strSql = @"SELECT WORKCTR,WORKCTR+' '+LEFT(WORKSHORTNAME,10)  AS WORKNAME FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'  AND  SHOPCD='CELL2' ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstStation.DataSource = dt;
            this.lstStation.DataTextField = "WORKNAME";
            this.lstStation.DataValueField = "Workctr";
            this.lstStation.DataBind();
            this.lstStation.ClearSelection();
        }

        /// <summary>
        /// 設置DataSource:Module Station
        /// </summary>
        private void SetModuleStationDataSource()
        {
            this.CheckModuleALL.Checked = false;
            this.lstModuleStation.Items.Clear();
            string strSql = @"SELECT WORKCTR,WORKCTR+' '+LEFT(WORKSHORTNAME,10)  AS WORKNAME FROM MWCM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND SHOPCD='MODULE' ORDER BY WORKCTR";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            this.lstModuleStation.DataSource = dt;
            this.lstModuleStation.DataTextField = "WORKNAME";
            this.lstModuleStation.DataValueField = "Workctr";
            this.lstModuleStation.DataBind();
            this.lstModuleStation.ClearSelection();
        }

        private void SetProductIDDataSource()
        {
            string strSql = @"SELECT ProdID FROM MPIM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                this.lblErrMsg.Text = "Cannot Fetch Product ID!";
            }
            else
            {
                this.lstProductID.DataSource = dt;
                this.lstProductID.DataTextField = "ProdID";
                this.lstProductID.DataValueField = "ProdID";
                this.lstProductID.DataBind();
                this.lstProductID.Items.Insert(0, "");
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked)
            {
                foreach (ListItem i in this.lstStation.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstStation.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void lstStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstStation.Items)
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

        protected void CheckModuleALL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckModuleALL.Checked)
            {
                foreach (ListItem i in this.lstModuleStation.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstModuleStation.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void lstModuleStation_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool bResult = true;
            foreach (ListItem i in this.lstModuleStation.Items)
            {
                if (!i.Selected)
                {
                    this.CheckModuleALL.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.CheckModuleALL.Checked = true;
            }

        }

        protected void chkAllProdGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllProdGroup.Checked)
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

        protected void lstProdGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool bResult = true;
            foreach (ListItem i in this.lstProdGroup.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllProdGroup.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllProdGroup.Checked = true;
            }

        }

        protected void chkAllProdID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllProdID.Checked)
            {
                foreach (ListItem i in this.lstProductID.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstProductID.Items)
                {
                    i.Selected = false;
                }
            }

        }

        protected void lstProductID_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool bResult = true;
            foreach (ListItem i in this.lstProductID.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllProdID.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllProdID.Checked = true;
            }

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

        protected void Button1_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string StrSql = string.Empty;
            String strFilter1 = String.Empty;
            StrSql = @"select A.CSHOPCD,'WIP'AS WIP_TYPE,A.ENTITYID,A.LOTTYPE,A.ENTITYSTS,B.PRODGROUP AS PRODGROUP,
                    A.PRODID,A.SOPRODID,A.CWORKCTR,C.WORKNAME,A.PACKFLG,A.LOTPTY,A.LOTGRADE,A.LOTLEVEL,
                    A.ENTITYTQTY,CONVERT(VARCHAR(21),A.STARTDATE,120) AS STARTDATE,CONVERT(VARCHAR(21),A.TRACKINDATE,120) AS TRACKINDATE,
                    CONVERT(VARCHAR(21),A.TRACKOUTDATE,120) AS TRACKOUTDATE,A.CMT,A.SUBLINEID,A.USERID,
                    CASE WHEN A.ENTITYSTS='H' THEN A.HOLDCD ELSE '' END AS HOLDCD,
                    CASE WHEN A.ENTITYSTS='H' THEN A.HOLDID ELSE '' END AS HOLDID,
                    A.HOLDCTR,
                    CASE WHEN A.ENTITYSTS='W'AND ISNULL(A.TRACKOUTDATE,'')=''THEN DATEDIFF(SECOND,CAST(A.TRANSDATE AS VARCHAR),GETDATE())
	                     WHEN A.ENTITYSTS='W'AND ISNULL(A.TRACKOUTDATE,'')<>''THEN  DATEDIFF(SECOND,CAST(A.TRACKOUTDATE AS VARCHAR),GETDATE())
	                     ELSE ''
	                     END AS WAITIME,
                    CASE WHEN A.ENTITYSTS='H'THEN DATEDIFF(SECOND,CONVERT(VARCHAR(21),A.HOLDID,120),GETDATE())
	                     ELSE ''
	                     END AS HOLDTIME,
                    CASE WHEN A.ENTITYSTS='R'AND ISNULL(A.TRACKINDATE,'')=''THEN DATEDIFF(SECOND,CAST(A.TRANSDATE AS VARCHAR),GETDATE())
	                     WHEN A.ENTITYSTS='R'AND ISNULL(A.TRACKINDATE,'')<>''THEN  DATEDIFF(SECOND,CAST(A.TRACKINDATE AS VARCHAR),GETDATE())
	                     ELSE ''
	                     END AS PROCESSTIME,
                    CASE WHEN A.ENTITYSTS='P'AND ISNULL(A.TRACKOUTDATE,'')=''THEN DATEDIFF(SECOND,CAST(A.TRANSDATE AS VARCHAR),GETDATE())
	                     WHEN A.ENTITYSTS='P'AND ISNULL(A.TRACKOUTDATE,'')<>''THEN  DATEDIFF(SECOND,CAST(A.TRACKOUTDATE AS VARCHAR),GETDATE())
	                     ELSE ''
	                     END AS Pendingtime,
                    A.WONBR,
                    A.LOTOWNER
                     from New_wltm A 
                    LEFT JOIN MPIM B ON A.FABID=B.FABID AND A.PRODID=B.PRODID
                    LEFT JOIN MWCM C ON A.FABID=B.FABID AND A.CWORKCTR= C.WORKCTR
                    where A.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND A.HIDEFLG='0' AND B.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'  AND C.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'";
            StringBuilder strStation = new StringBuilder();
            if (this.lstStation.Items.Count != 0)
            {
                if (this.lstStation.SelectedIndex >= 0)
                {
                    foreach (ListItem li in this.lstStation.Items)
                    {
                        if (li.Selected)
                        {
                            strStation.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (this.lstModuleStation.Items.Count != 0)
            {
                if (this.lstModuleStation.SelectedIndex >=0)
                {
                    foreach (ListItem li in this.lstModuleStation.Items)
                    {
                        if (li.Selected)
                        {
                            strStation.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strStation.Length > 0)
            {
                strFilter1 += " AND A.CWORKCTR IN (" + strStation.ToString().Replace("''", "','") + ") ";
            }
            StringBuilder strProdGroup =new StringBuilder ();
            if (this.lstProdGroup.Items.Count != 0)
            {
                if (this.lstProdGroup.SelectedIndex >=0)
                {
                    foreach (ListItem li in this.lstProdGroup.Items)
                    {
                        if (li.Selected)
                        {
                            strProdGroup.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strProdGroup.Length > 0)
            {
                strFilter1 += " AND B.PRODGROUP IN (" + strProdGroup.ToString().Replace("''", "','") + ") ";
            }
            StringBuilder strProdID = new StringBuilder();
            if (this.lstProductID.Items.Count != 0)
            {
                if (this.lstProductID.SelectedIndex >= 0)
                {
                    foreach (ListItem li in this.lstProductID.Items)
                    {
                        if (li.Selected)
                        {
                            strProdID.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }
            }
            if (strProdID.Length > 0)
            {
                strFilter1 += " AND A.PRODID IN (" + strProdID.ToString().Replace("''", "','") + ") ";
            }
            String strWonbr = string.Empty;
            if (this.txtWotype.Text!="")
            {
                strFilter1 += " AND A.WONBR= '" + this.txtWotype.Text+"'"; 
            }
            StrSql += strFilter1 +" ORDER BY PRODID,CWORKCTR,ENTITYSTS";
            dsDetail = this.dbConn.GetDataSetBySql(StrSql);
            Session["DsDetail"] = dsDetail;
            DataTable dtSUM = new DataTable();
            long lngHoldTime =0;
            long lngWaitTime=0;
            long lngPendTime=0;
            long lngRunTime=0;
            long lngTtlTme =0;
            string disProdid = string.Empty;
            string disWorkctr = string.Empty;
            dtSUM.Columns.Add(new DataColumn("PARTID", typeof(string)));
            dtSUM.Columns.Add(new DataColumn("WORKCTR", typeof(string)));
            dtSUM.Columns.Add(new DataColumn("TTL_QTY", typeof(long)));
            dtSUM.Columns.Add(new DataColumn("HOLD", typeof(long)));
            dtSUM.Columns.Add(new DataColumn("PENDING", typeof(long)));
            dtSUM.Columns.Add(new DataColumn("RUN", typeof(long)));
            dtSUM.Columns.Add(new DataColumn("WAIT", typeof(long)));
            for (int i = 0; i < dsDetail.Tables[0].Rows.Count+1; i++)
            {
                if (i==dsDetail.Tables [0].Rows.Count|| disProdid.Equals("") || !disProdid.Equals(dsDetail.Tables[0].Rows[i][6].ToString()) || disWorkctr.Equals("") || !disWorkctr.Equals(dsDetail.Tables[0].Rows[i][8].ToString()))
                {   
                    if (i >0 )
                    {
                        DataRow dr = dtSUM.NewRow();
                        dr.Table.Columns.Add();
                        dr["PARTID"] = disProdid;
                        dr["WORKCTR"] = disWorkctr;
                        dr["TTL_QTY"] = lngTtlTme;
                        dr["HOLD"] = lngHoldTime;
                        dr["PENDING"] = lngPendTime;
                        dr["RUN"] = lngRunTime;
                        dr["WAIT"] = lngWaitTime;
                        dtSUM.Rows.Add(dr);
                    }
                    if (i < dsDetail.Tables[0].Rows.Count)
                    {
                        lngHoldTime = 0;
                        lngWaitTime = 0;
                        lngPendTime = 0;
                        lngRunTime = 0;
                        disProdid = dsDetail.Tables[0].Rows[i][6].ToString();
                        disWorkctr = dsDetail.Tables[0].Rows[i][8].ToString();
                        lngTtlTme = Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                        if (dsDetail.Tables[0].Rows[i][4].Equals("H"))
                        {
                            lngHoldTime = Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                        }
                        if (dsDetail.Tables[0].Rows[i][4].Equals("W"))
                        {
                            lngWaitTime = Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                        }
                        if (dsDetail.Tables[0].Rows[i][4].Equals("P"))
                        {
                            lngPendTime = Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                        }
                        if (dsDetail.Tables[0].Rows[i][4].Equals("R"))
                        {
                            lngRunTime = Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                        }
                    }
                }
                else
                {
                    lngTtlTme = lngTtlTme + Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                    if (dsDetail.Tables[0].Rows[i][4].Equals("H"))
                    {
                        lngHoldTime =lngHoldTime + Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                    }
                    if (dsDetail.Tables[0].Rows[i][4].Equals("W"))
                    {
                        lngWaitTime = lngWaitTime + Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                    }
                    if (dsDetail.Tables[0].Rows[i][4].Equals("P"))
                    {
                        lngPendTime = lngPendTime + Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                    }
                    if (dsDetail.Tables[0].Rows[i][4].Equals("R"))
                    {
                        lngRunTime = lngRunTime + Convert.ToInt64(dsDetail.Tables[0].Rows[i][14]);
                    }
                }
            }
            this.DataGridSum.DataSource = dtSUM;
            this.DataGridSum.DataBind();
            this.GridViewTmp.DataSource = dtSUM;
            this.GridViewTmp.DataBind();

        }

        protected DataTable GetDetailDataset(string strProdID,string strWorkctr)
        {
            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add(new DataColumn("COUNT", typeof(int)));
            dtDetail.Columns.Add(new DataColumn("CSHOPCD", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("WIP_TYPE", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("ENTITYID", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("LOTTYPE", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("ENTITYSTS", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("PRODGROUP", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("PRODID", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("SOPRODID", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("CWORKCTR", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("WORKNAME", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("PACKFLG", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("LOTPTY", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("LOTGRADE", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("LOTLEVEL", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("ENTITYTQTY", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("STARTDATE", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("TRACKINDATE", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("TRACKOUTDATE", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("CMT", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("SUBLINEID", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("USERID", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("HOLDCD", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("HOLDDESC", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("HOLDCTR", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("WAITIME", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("HOLDTIME", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("PROCESSTIME", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("Pendingtime", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("WONBR", typeof(string)));
            dtDetail.Columns.Add(new DataColumn("LOTOWNER", typeof(string)));
            DataSet dsTmp = (DataSet)(Session["DsDetail"]);
            DataRow[] drs =dsTmp.Tables[0].Select("(PRODID= '" + strProdID + "'  AND CWORKCTR='" + strWorkctr + "')");
            int i=0;
            if (drs.Length > 60000)
            {
                dtDetail.Clear();
                this.lblErrMsg.Text = "資料量過大超過，無法顯示查詢結果，謝謝！\n";
                return dtDetail;
            }
            foreach (DataRow drsa in drs)
            {   
                i=i+1;
                DataRow dr = dtDetail.NewRow();
                dr.Table.Columns.Add();
                dr["COUNT"] = i;
                dr["CSHOPCD"] = drsa["CSHOPCD"];
                dr["WIP_TYPE"] = drsa["WIP_TYPE"];
                dr["ENTITYID"] = drsa["ENTITYID"];
                dr["LOTTYPE"] = drsa["LOTTYPE"];
                dr["ENTITYSTS"] = drsa["ENTITYSTS"];
                dr["PRODGROUP"] = drsa["PRODGROUP"];
                dr["PRODID"] = drsa["PRODID"];
                dr["SOPRODID"] = drsa["SOPRODID"];
                dr["CWORKCTR"] = drsa["CWORKCTR"];
                dr["WORKNAME"] = drsa["WORKNAME"];
                dr["PACKFLG"] = drsa["PACKFLG"];
                dr["LOTPTY"] = drsa["LOTPTY"];
                dr["LOTGRADE"] = drsa["LOTGRADE"];
                dr["LOTLEVEL"] = drsa["LOTLEVEL"];
                dr["ENTITYTQTY"] = drsa["ENTITYTQTY"].ToString();
                dr["STARTDATE"] = drsa["STARTDATE"];
                dr["TRACKINDATE"] = drsa["TRACKINDATE"];
                dr["TRACKOUTDATE"] = drsa["TRACKOUTDATE"];
                dr["CMT"] = drsa["CMT"];
                dr["SUBLINEID"] = drsa["SUBLINEID"];
                dr["USERID"] = drsa["USERID"];
                dr["WAITIME"] = drsa["WAITIME"].ToString();
                dr["PROCESSTIME"] = drsa["PROCESSTIME"].ToString ();
                dr["Pendingtime"] =drsa["Pendingtime"].ToString();
                dr["HOLDTIME"] = drsa["HOLDTIME"];
                dr["WONBR"] = drsa["WONBR"];               
                dr["LOTOWNER"] = drsa["LOTOWNER"];
                if ( drsa["ENTITYSTS"].ToString().Equals("H"))
                {   
                    dr["HOLDCD"] = drsa["HOLDCD"];
                    dr["HOLDCTR"] = drsa["HOLDCTR"];
                    string strSqlHoldesc = string.Empty;
                    strSqlHoldesc = @"SELECT HOLDDESC FROM New_WHLD WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND HOLDID='" + drsa["HOLDID"] + "' AND ENTITYID='" + drsa["ENTITYID"] + "'";
                    DataSet  dsHold=new DataSet () ;
                    dsHold = this.dbConn.GetDataSetBySql(strSqlHoldesc);
                    if (dsHold.Tables[0].Rows.Count > 0)
                    {
                        dr["HOLDDESC"] = dsHold.Tables[0].Rows[0]["HOLDDESC"];
                    }
                    else
                    {
                        dr["HOLDDESC"] = "";
                    }
                }
                else
                {
                    dr["HOLDDESC"] = "";
                    dr["HOLDCD"] = "";
                    dr["HOLDCTR"] ="";
                    //dr["HOLDTIME"] = "";
                }
                dtDetail.Rows.Add(dr);
           }
           return dtDetail;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            if (this.GridViewTmp.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=WIPAnalysisSum.xls");
                //this.DataGridDetail.EnableViewState =false;
                //DataGridDetail.AllowSorting = false;
                //DataGridDetail.AllowPaging = false;
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.ContentType = "application/ms-excel";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw);
                HtmlForm hf = new HtmlForm();
                Controls.Add(hf);
                hf.Controls.Add(GridViewTmp);
                hf.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            if (this.DataGridDetail.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=WIPAnalysisDetail.xls");
                Response.ContentEncoding = System.Text.Encoding.UTF7;
                Response.ContentType = "application/ms-excel";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw);
                HtmlForm hf = new HtmlForm();
                Controls.Add(hf);
                hf.Controls.Add(DataGridDetail);
                hf.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
        }

        protected void DataGridSum_RowCommand(object sender, GridViewCommandEventArgs e)
        {  
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = DataGridSum.Rows[index];
            //string strPartid = DataGridSum.DataKeys[row.DataItemIndex][0].ToString();
            //string strWorkctr = DataGridSum.DataKeys[row.DataItemIndex][1].ToString();  
            string StrPartID = row.Cells[1].Text.ToString();
            string StrWorkctr = row.Cells[2].Text.ToString();
            this.DataGridDetail.DataSource = this.GetDetailDataset(StrPartID.ToString(), StrWorkctr.ToString());
            this.DataGridDetail.DataBind();
            //DataGridSum.FindControl("linkbutton").
        }


        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //       server control at run time. */
        //}

        

    }
}
