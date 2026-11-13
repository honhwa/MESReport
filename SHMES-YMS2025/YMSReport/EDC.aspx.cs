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
using System.Windows.Forms;

public partial class YMSReport_EDC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Login"] == null)
        {
            tbFABID.Text = "";
            tbFABName.Text = "";
            Page.Response.Redirect(@"~\Default.aspx");
            return;
        }
        if (!Page.IsPostBack)
        {
            AppClass Apc = new AppClass();
            if (Request.QueryString["FABID"] != null)
            {
                tbFABID.Text = this.Request.QueryString["FABID"].ToString();
                if (tbFABID.Text == "B") 
                {
                    tbFABName.Text = "SH";
                }
                string strLoginPCName = "";
                string strLoginPCIP = "";
                string strLoginNTName = "";
                tbLoginNTName.Text = "";
                tbLoginPCIP.Text = "";
                tbLoginPCName.Text = "";

                Apc.GetLoginInfo(out strLoginPCName, out strLoginPCIP, out strLoginNTName);

                tbLoginNTName.Text = strLoginNTName;
                tbLoginPCIP.Text = strLoginPCIP;
                tbLoginPCName.Text = strLoginPCName;
                HyperLink1.NavigateUrl = "~/MainButton.aspx?FABID=" + tbFABID.Text.Trim();
                Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Browse", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Choose Report", tbFABID.Text.ToString(), "");
            }
            else
            {
                Page.Response.Redirect(@"~\Default.aspx");
            }
            GetStation();
            lblFabID.Text = tbFABName.Text;
            txtFDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            txtTDate.Text = DateTime.Now.ToShortDateString();
        }
        Page.Title = tbFABName.Text + " EDC Report";
    }

    protected void ddlStation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ( ddlStation.Items.Count == 1)//如果Station未被加載過

        {
            //InitPanelInfo("ddlStation");
            GetStation();
        }
    }
    protected void ddlQueryBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtQueryBy.Text = "";//清空已輸入內容

        SelectedIndexChanged("txtQueryBy", ddlQueryBy.SelectedValue);//為USER手動KEY IN無需加載
    }
    protected void ddlShiftID_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklShiftID.ClearSelection();//清除已選中內容

        if (SelectedIndexChanged("pShiftID", ddlShiftID.SelectedValue) && chklShiftID.Items.Count == 0)//如果顯示對應的Panel，且該Panel內的加載項未加載過，則加載內容

        {
            InitPanelInfo("chklShiftID");
        }
    }
    protected void ddlFloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklFloor.ClearSelection();//清除已選擇內容

        if (SelectedIndexChanged("pFloor", ddlFloor.SelectedValue) && chklFloor.Items.Count == 0)//如果顯示對應的Panel，且該Panel內的加載項未加載過，則加載內容

        {
            InitPanelInfo("chklFloor");
        }
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklProject.ClearSelection();//清除已選擇內容

        if (SelectedIndexChanged("pProject", ddlProject.SelectedValue) && chklProject.Items.Count == 0)//如果顯示對應的Panel，且該Panel內的加載項未加載過，則加載內容

        {
            InitPanelInfo("chklProject");
        }
    }
    /// <summary>
    /// 任意DropDownList選擇時，傳入對應的Panel名和選擇的Value值，即可顯示或隱藏該Panel，定義Value值為1時顯示，否則隱藏
    /// </summary>
    /// <param name="PanelName">該控件對應的Panel名</param>
    /// <param name="selectedIndex">所選擇的Value值</param>
    /// <returns>返回值表示是否顯示該Panel</returns>
    private bool SelectedIndexChanged(string PanelName,string selectedValue)
    {
        bool isShowPanel;
        if (selectedValue != "All")
        {
            form1.FindControl(PanelName).Visible = true;
            isShowPanel = true;
        }
        else
        {
            form1.FindControl(PanelName).Visible = false;
            isShowPanel = false;
        }
        return isShowPanel;
    }
    /// <summary>
    /// 預設Station
    /// </summary>
    private void GetStation()
    {
        //chklStation.Items.Clear();
        string[] strStationID ={"8005", "8015", "8020", "8040", "8050", "8060", "8080", "8081", "8082", "8085", "808A", "808B", "8090", "8180" };
        string[] strStationDesc ={"Batch inspection", "AURO SCRIBE", "SEMI SCRIBE", "LC", "封口", "ROUGH CLEAN", "LOT1", "外觀", "Cell2 Packing1", "Cell2 Packing2", "LOT1-LR", "外觀-LR", "FINAL CLEAN", "M1" };
        for (int i = 0; i < strStationID.Length; i++)
        {
            ListItem litem = new ListItem();
            litem.Text = strStationID[i].ToString() + " " + strStationDesc[i];
            litem.Value = strStationID[i].ToString();
            ddlStation.Items.Add(litem);
        }
    }
    /// <summary>
    /// 加載Panel內綁定CheckBoxList的值    /// </summary>
    /// <param name="PanelName">該控件對應的Panel名</param>
    private void InitPanelInfo(string chklName)
    {
        string strSQL = "";
        DBConn dbConn = new DBConn(tbFABID.Text.ToString());
        DataSet ds = new DataSet();
        //if (chklName == "chklStation")      //預留，防止以後需要多選        //{
        //    //chklStation.Items.Clear();
        //    string[] strStationID ={ "8015", "8020", "8040", "8050", "8060", "8080", "8081", "8082", "8085", "808A", "808B", "8090", "8180" };
        //    string[] strStationDesc ={ "AURO SCRIBE", "SEMI SCRIBE", "LC", "封口", "ROUGH CLEAN", "LOT1", "外觀", "Cell2 Packing1", "Cell2 Packing2", "LOT1-LR", "外觀-LR", "FINAL CLEAN", "M1" };
        //    for (int i = 0; i < strStationID.Length; i++)
        //    {
        //        ListItem litem = new ListItem();
        //        litem.Text = strStationID[i].ToString() + " " + strStationDesc[i];
        //        litem.Value = strStationID[i].ToString();
        //        ddlStation.Items.Add(litem);
        //    }
        //}
        if (chklName == "chklShiftID")
        {
            strSQL = " SELECT DISTINCT CASE SHIFTID WHEN 'D' THEN '白班' WHEN 'N' THEN '夜班' END AS SHIFTID,SHIFTID FROM MSSM ";
        }
        else if (chklName == "chklFloor")
        {
            strSQL = " SELECT DISTINCT CASE MCSTOCKID WHEN '5103' THEN '1F'" +
                     " WHEN '5105' THEN '2F' " +
                     " WHEN '5107' THEN '3F' " +
                     " WHEN '5109' THEN '4F' " +
                     " WHEN '51A3' THEN '1F' " +
                     " WHEN '51A5' THEN '2F' " +
                     " WHEN '51A7' THEN '3F' " +
                     " WHEN '51A9' THEN '4F' " +
                     " END AS Floor,MCSTOCKID " +
                     " FROM MSLM ";
        }
        else if (chklName == "chklProject")
        {
            strSQL = " SELECT DISTINCT PROJECT_NAME AS PROJECT,PROJECT_NAME FROM MPIM ";
        }

        ds = dbConn.GetDataSetBySql(strSQL, chklName);
        if (ds.Tables[chklName].Rows.Count == 0)
        {
            lblErrorMsg.Text ="Error：未取到 "+ chklName.Substring(4)+" 的相關信息！";
        }
        else
        {
            for (int i = 0; i < ds.Tables[chklName].Rows.Count; i++)
            {
                ListItem litem = new ListItem();
                litem.Text = ds.Tables[chklName].Rows[i][0].ToString();
                litem.Value = ds.Tables[chklName].Rows[i][1].ToString();
                ((CheckBoxList)form1.FindControl(chklName)).Items.Add(litem);
            }
        }
    }
    private string strFilterTermsGlassID = "";
    private string strFilterTermsLotID = "";
    /// <summary>
    /// 獲取查詢條件，即USER選擇的篩選條件    /// </summary>
    /// <returns></returns>
    private void GetFilterTerms()
    {
        try
        {
            string strGlassFilter = "";
            //獲取時間
            if (txtFDate.Text == "" || txtTDate.Text == "")
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "開始時間和結束時間均不可以為空！";
                if (txtFDate.Text == "")
                    txtFDate.Focus();
                else
                    txtTDate.Focus();
                return;
            }
            else
            {
                string strFTime = txtFDate.Text + " " + ddlFHour.SelectedValue + ":" + ddlFMinute.SelectedValue + ":00";
                string strTTime = txtTDate.Text + " " + ddlTHour.SelectedValue + ":" + ddlTMinute.SelectedValue + ":00";
                TimeSpan timeSpan = Convert.ToDateTime(strTTime).Subtract(Convert.ToDateTime(strFTime));
                double days = timeSpan.TotalDays;
                if (Convert.ToDateTime(strFTime) > Convert.ToDateTime(strTTime))
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "開始時間不能大於結束時間！";
                    return;
                }
                else if(days>15)
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "時間範圍超出15天！";
                    return;
                }

                strFilterTermsGlassID += " AND A.TRANSDATE >='" + strFTime + "' \n" + " AND A.TRANSDATE <='" + strTTime + "' \n";
            }
            //獲取GLASSID或LOTID信息
            if (ddlQueryBy.SelectedIndex == 1)
            {
                string strGlassID = "('" + txtQueryBy.Text.Replace("\r\n", "','") + "') ";//\r\n是換行符
                strFilterTermsGlassID += strGlassID == "('') " ? "" : " AND MF.GLASSID IN" + strGlassID + " \n";
                strGlassFilter = " AND MF.GLASSID IN" + strGlassID + " \n";
            }
            else if (ddlQueryBy.SelectedIndex == 2)
            {
                string strLOTID = "('" + txtQueryBy.Text.Replace("\r\n", "','") + "') ";//\r\n是換行符
                strFilterTermsGlassID += strLOTID == "('') " ? "" : " AND A.TOENTITYID IN" + strLOTID + " \n";
            }
            //獲取站點
            if (ddlStation.SelectedValue != "All")
            {
                strFilterTermsGlassID += " AND A.NWORKCTR='" + ddlStation.SelectedValue.ToString() + "' \n";
            }
            else
            {
                strFilterTermsGlassID += " AND A.NWORKCTR IN('8005', '8015','8020','8040','8050','8060','8080','808A','8082','8085','808A','808B','8090','8180') \n";
            }
            //獲取ShiftID的信息            if (pShiftID.Visible)
            {
                string strShiftID = "";
                foreach (ListItem litem in chklShiftID.Items)
                {
                    if (litem.Selected)
                    {
                        strShiftID += "'" + litem.Value + "',";
                    }
                }
                if (strShiftID != "")
                {
                    strFilterTermsGlassID += " AND A.SHIFTID IN (" + strShiftID.Substring(0, strShiftID.Length - 1) + ") \n";
                }
            }
            
            //獲取Project_Name
            if (pProject.Visible)
            {
                string strProject = "";
                foreach (ListItem litem in chklProject.Items)
                {
                    if (litem.Selected)
                    {
                        strProject += "'" + litem.Value + "',";
                    }
                }
                if (strProject != "")
                {
                    //strFilterTermsLotID += " AND MP.PROJECT_NAME IN (" + strProject.Substring(0, strProject.Length - 1) + ") \n";
                    strFilterTermsGlassID += " AND A.TOPRODID IN(SELECT PRODID FROM MPIM WHERE PROJECT_NAME IN (" + strProject.Substring(0, strProject.Length - 1) + ")) \n";
                }
            }
            //判斷選擇的是否為GlassID，是則產生兩個SQL(其中一個排除GLASSID)，否則複製一個相同的SQL語句
            if (ddlQueryBy.SelectedIndex == 1)
            {
                strFilterTermsLotID = strFilterTermsGlassID.Replace(strGlassFilter, "");
                strFilterTermsLotID = strFilterTermsLotID.Replace("NWORKCTR", "CWORKCTR");//LotID對應的站點欄位是NWORKCTR,GlassID對應的是CWORKCTR
            }
            else
            {
                strFilterTermsLotID = strFilterTermsGlassID;
                strFilterTermsLotID = strFilterTermsLotID.Replace("NWORKCTR", "CWORKCTR");
            }

        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "獲取查詢條件錯誤：" + ex.ToString();
        }
        //strFilterTermsGlassID.Replace(""
    }

    protected void btQuery_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dtimebf = DateTime.Now;

            lblErrorMsg.Visible = false;
            GetFilterTerms();//獲取USER設置的過濾條件

            if (strFilterTermsGlassID != "")
            {
                string strSubLineid = "";//Add 層別篩選線別
                string strSQLGlassID = "";
                string strSQLLotID = "";
                DBConn dbconn = new DBConn(tbFABID.Text);
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                //通過樓層獲取線別  //Add
                if (pFloor.Visible)
                {
                    string strFloor = "";
                    foreach (ListItem litem in chklFloor.Items)
                    {
                        if (litem.Selected)
                        {
                            strFloor += "'" + litem.Value + "',";
                        }
                    }
                    if (strFloor != "")
                    {
                        string StrSQL = "";
                        DataSet ds = new DataSet();
                        StrSQL += "SELECT DISTINCT sublineid FROM MSLM WHERE mcstockid IN (" + strFloor.Substring(0, strFloor.Length - 1) + ") ";
                        ds = dbconn.GetDataSetBySql(StrSQL, "SubLineID");
                        strSubLineid += " AND A.CSUBLINEID IN (";
                        for (int i = 0; i < ds.Tables["SubLineID"].Rows.Count; i++)
                        {
                            strSubLineid += "'" + ds.Tables["SubLineID"].Rows[i][0].ToString() + "',";
                        }
                        strSubLineid = strSubLineid.Substring(0, strSubLineid.Length - 1) + ") \n";
                    }
                }

                strSQLGlassID += " SELECT MP.PROJECT_NAME, " + (ddlStation.SelectedValue == "All" ? "'OVER ALL' AS NWORKCTR" : "A.NWORKCTR") + ",CONVERT(varchar(30),A.TRANSDATE,120) TRANSDATE, A.TOENTITYID AS LOTID, A.FRENTITYID AS GLASSID,  ISNULL(MF.CARTON_ID,'') AS CARTON, \n" +
                                 " A.SHIFTID \n" +
                                 " FROM New_VWLTXA A \n" +
                                 " LEFT JOIN MPIM MP WITH(NOLOCK) ON(A.FABID = MP.FABID AND A.TOPRODID = MP.PRODID) \n" +
                                 " LEFT JOIN MFGINFO MF ON A.FRENTITYID = MF.GLASSID \n" +
                                 " WHERE A.FABID = '" + tbFABID.Text + "' \n" +
                                 " AND A.VERB = 'CRLT' \n" +
                                 " AND A.TOENTITYID LIKE 'Z%' \n" +
                                 " AND A.TOENTITYID NOT LIKE '%.%' \n" +
                                   strFilterTermsGlassID +
                                 " ORDER BY A.TOENTITYID ";

                strSQLLotID += " SELECT PROJECT_NAME," + (ddlStation.SelectedValue == "All" ? "'OVER ALL' AS CWORKCTR" : "CWORKCTR") + ",LOTID,CONVERT(varchar(30),MAX(TRANSDATE),120) TRANSDATE,MAX(INPUTQTY) INPUTQTY,MAX(OUTPUTQTY) OUTPUTQTY,MAX(USERID) USERID,MAX(CSUBLINEID) CSUBLINEID,MAX(SHIFTID) SHIFTID FROM ( \n" +
                               " SELECT \n" +
                               " MP.PROJECT_NAME, A.CWORKCTR, A.TOENTITYID AS LOTID,  \n" +
                               " MAX(A.TRANSDATE) AS TRANSDATE,  \n" +
                               " MAX(CASE WHEN VERB = 'MVIN' OR (VERB = 'MVOU' AND CWORKCTR = '8005') THEN A.CENTITYTQTY ELSE 0 END) AS INPUTQTY, \n" +
                               " MIN(A.CENTITYTQTY) AS OUTPUTQTY, \n" +
                               " MAX(CASE WHEN VERB = 'MVIN' THEN '' ELSE A.USERID END) AS USERID, \n" +
                               " MAX(CASE WHEN VERB = 'MVIN' THEN '' ELSE A.CSUBLINEID END) AS CSUBLINEID \n" +
                               " ,(SELECT SHIFTID FROM MSSM WHERE FABID = 'B' AND MAX(A.TRANSDATE) BETWEEN SSBEGIN AND SSEND) AS SHIFTID \n" +
                               " FROM New_VWLTXA A WITH(NOLOCK) \n" +
                               " LEFT JOIN MPIM MP WITH(NOLOCK) ON(A.FABID = MP.FABID AND A.TOPRODID = MP.PRODID) \n" +
                               " WHERE A.FABID = 'B' \n" +
                               " AND A.VERB IN('MVIN', 'MVOU') \n" +
                               " AND A.TOENTITYID LIKE 'Z%' \n" +
                               " AND A.TOENTITYID NOT LIKE '%.%' \n" +
                                 strFilterTermsLotID +
                               " GROUP BY MP.PROJECT_NAME, A.CWORKCTR, A.TOENTITYID,A.SHIFTID \n" +
                               " ) T \n" +
                               strSubLineid.Replace("AND A.", "WHERE T.") +                                               //Add
                               " GROUP BY PROJECT_NAME,LOTID" + (ddlStation.SelectedValue == "All" ? "" : ",T.CWORKCTR") +//Add
                               " ORDER BY PROJECT_NAME,LOTID \n";

                ds1 = dbconn.GetDataSetBySql(strSQLGlassID, "ByGlassID");
                ds2 = dbconn.GetDataSetBySql(strSQLLotID, "ByLotID");

                if (ds1.Tables["ByGlassID"].Rows.Count > 0)
                {
                    Panel1.Visible = true;
                    GridView1.DataSource = "";
                    GridView1.DataSource = ds1.Tables["ByGlassID"];
                    GridView1.DataBind();
                }
                else
                {
                    Panel1.Visible = false;
                    GridView1.DataSource = "";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Infomation：GlassID對應箱號、LOTID的信息未查詢到！";
                }
                if (ds2.Tables["ByLotID"].Rows.Count > 0)
                {
                    Panel2.Visible = true;
                    GridView2.DataSource = "";
                    ds2.Tables["ByLotID"].Columns.Add("YIELD", typeof(string));
                    for (int i = 0; i < ds2.Tables["ByLotID"].Rows.Count; i++)
                    {
                        ds2.Tables[0].Rows[i]["YIELD"] = Convert.ToString((Convert.ToDouble(ds2.Tables["ByLotID"].Rows[i]["INPUTQTY"].ToString()) == 0) ? 100 : Math.Round(Convert.ToDouble(ds2.Tables["ByLotID"].Rows[i]["OUTPUTQTY"]) / Convert.ToDouble(ds2.Tables["ByLotID"].Rows[i]["INPUTQTY"]), 4) * 100) + "%";
                    }
                    GridView2.DataSource = ds2.Tables["ByLotID"];
                    GridView2.DataBind();
                }
                else
                {
                    lblErrorMsg.Visible = true;
                    if (lblErrorMsg.Text == "")
                    {
                        Panel2.Visible = false;
                        GridView2.DataSource = "";
                        lblErrorMsg.Text = "Infomation:Project_Name對應各站點投入產出的信息未查詢到！";
                    }
                    else
                    {
                        Panel1.Visible = false;
                        GridView1.DataSource = "";
                        Panel2.Visible = false;
                        GridView2.DataSource = "";
                        lblErrorMsg.Text = "查無資料！";
                    }
                }
                Session["ByGlassID"] = ds1.Tables["ByGlassID"].Copy();
                Session["ByLotID"] = ds2.Tables["ByLotID"].Copy();
            }
            DateTime dtimeaf = DateTime.Now;
            TimeSpan tspan = dtimeaf.Subtract(dtimebf);
            lblTimeSpan.Visible = true;
            lblTimeSpan.Text = "查詢耗時：" + Convert.ToInt32(tspan.TotalSeconds).ToString() + " s";
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "錯誤描述：" + ex.ToString();
        }
    }

    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            AppClass Apc = new AppClass();
            Apc.RecordLogInfo(tbFABID.Text, tbLoginNTName.Text, "UnLogin", Session.SessionID.ToString(), "Export", HttpContext.Current.Request.Url.AbsolutePath.ToString(), tbLoginPCName.Text, tbLoginPCIP.Text, "Export-Excel", "", "");
            ToExcel();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "導出EXCEL出錯：" + ex.ToString();
        }
    }

    private void ToExcel()
    {
        Response.Clear();
        Response.Charset = "UTF-8";
        Response.ContentEncoding = System.Text.Encoding.Default;
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + tbFABName.Text + " EDC Report.xls");
        
        GridView1.Page.EnableViewState = false;
        GridView2.Page.EnableViewState = false;

        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

        this.GridView1.RenderControl(oHtmlTextWriter);
        this.GridView2.RenderControl(oHtmlTextWriter);

        Response.Output.Write(oStringWriter.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        lblErrorMsg.Visible = false;
        lblErrorMsg.Text = "";

        GridView1.DataSource = "";
        GridView2.DataSource = "";
        Panel1.Visible = false;
        Panel2.Visible = false;
        lblTimeSpan.Visible = false;
        lblTimeSpan.Text = "";

        txtQueryBy.Text = "";
        chklFloor.ClearSelection();
        chklProject.ClearSelection();
        chklShiftID.ClearSelection();
        chklStation.ClearSelection();
        Session["GlassID"] = null;
        Session["ByLotID"] = null;
    }
}
