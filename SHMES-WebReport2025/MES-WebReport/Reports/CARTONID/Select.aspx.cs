using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Report.Library;
using System.Data;
using System.IO;
using System.Text;
using ExcelIO;

namespace Report.Reports.CARTONID
{ 
    public partial class Select : System.Web.UI.Page
    {
        #region 定義全局變量
        string strQuerySQL;
        string StrQuerySQL;
        string STRQuerySQL;
        DBConn dbConn = new DBConn();
        string name;
        string strTimeStart;
        string strTimeENd;
        string strPanelID;
        object Count;
        string PageIndex;
        #endregion

        #region 頁面加載事件
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AspNetPager1.PageSize = 20;
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                BindGridview();
                txtStartDT.Text = System.DateTime.Now.AddYears(-5).ToString("yyyy-MM-dd");
                txtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                //Response.Write("<script>alert('請不要單次輸入兩萬筆以上的數據進行查詢');</script>");
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }
        #endregion

        #region 下拉框一選擇事件
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            name = DropDownList1.SelectedValue;//定義下拉列表选中值
            if (name == "PANELID查生產記錄")
            {
                DropDownList2.Enabled = false;
            }
            else
            {
                DropDownList2.Enabled = true;
            }
        }
        #endregion

        #region 下拉框二選擇事件
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            name = DropDownList2.SelectedValue;//定義下拉列表选中值
            if (name == "ALL" && DropDownList1.Text == "箱號查PANELID")
            {
                //Response.Write("<script>alert('相較分類查詢速度較慢，只顯示數據條數和首頁數據，請直接導出');</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('相較分類查詢速度較慢，只顯示數據條數和首頁數據，請直接導出');</script>"); 
            }

        }
        #endregion

        #region 頁面加載中效果
        /// <summary>
        /// 页面加载中效果
        /// </summary>
        public static void initJavascript()
        {
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
            HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
            HttpContext.Current.Response.Write("function animate(){");
            HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
            HttpContext.Current.Response.Write("if(elem != null) {");
            HttpContext.Current.Response.Write("if (pos==0) len += dir;");
            HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
            HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
            HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
            HttpContext.Current.Response.Write("elem.style.left = pos;");
            HttpContext.Current.Response.Write("elem.style.width = len;");
            HttpContext.Current.Response.Write("}}");
            HttpContext.Current.Response.Write("function remove_loading() {");
            HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
            HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
            HttpContext.Current.Response.Write("targelem.style.display='none';");
            HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
            HttpContext.Current.Response.Write("}");
            HttpContext.Current.Response.Write("</script>");
            HttpContext.Current.Response.Write("<style>");
            HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
            HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
            HttpContext.Current.Response.Write("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
            HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("<div id=loader_container>");
            HttpContext.Current.Response.Write("<div id=loader>");
            HttpContext.Current.Response.Write("<div align=center>數據加载中 ...</div>");
            HttpContext.Current.Response.Write("<div id=loader_bg><div id=progress> </div></div>");
            HttpContext.Current.Response.Write("</div></div>");
            HttpContext.Current.Response.Flush();
        }
        #endregion

        #region 查詢點擊事件
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            if (TxtPanelId.Text!="")
            {
                initJavascript();
                try
                {
                    lblMsg.Visible = false;
                    BindGridview();
                    
                    if ((int)Count >= 1)
                    {
                        GridView1.Visible = true;
                        AspNetPager1.Visible = true;
                        lblMsg.Text = "查詢成功!記錄數：" + Count.ToString() + "條";
                        lblMsg.Visible = true;
                        btnExport.Enabled = true;
                        
                    }
                    else
                    {
                        this.Response.Write("<script language=javascript>window.alert('查無資料，請確認！');</script>");
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                }
            }
            else
            {
                this.Response.Write("<script language=javascript>window.alert('PANELID或箱號不允許為空！');</script>");
            }
        }
        #endregion

        #region 數據綁定方法
        private bool BindGridview()
        {
            int PageIndex = AspNetPager1.CurrentPageIndex;
            string strTimeStart = txtStartDT.Text + " 00:00:00";
            string strTimeENd = txtEndDT.Text + " 23:59:59";
            name = DropDownList1.SelectedValue;//定義下拉列表选中值
            string PanelID = TxtPanelId.Text;
            try
            {
                DateTime dt1 = Convert.ToDateTime(strTimeStart);
                DateTime dt2 = Convert.ToDateTime(strTimeENd);
            }
            catch
            {
                throw new Exception("時間格式不正確！");
            }

            //PANELID轉換字符串格式
            string strlotTemp1 = TxtPanelId.Text.Replace("\r\n", ",");
            string[] strtxtLot = strlotTemp1.Split(',');
            string strPanelID = "";
            for (int i = 0; i < strtxtLot.Length; i++)
            {
                strPanelID += "'" + strtxtLot[i] + "',";

            }
            strPanelID = strPanelID.TrimEnd(',');

            if (name == "PANELID查箱號")
            {

                switch (DropDownList2.Text)
                {

                    case "BMW":
                        strQuerySQL += "SELECT TOP 20 PanelID,CARTID FROM (SELECT ROW_NUMBER()OVER(ORDER BY PanelID )AS ROWNUMBER,PanelID,CARTID FROM (SELECT B.LOTID AS PanelID,A.CARTID FROM New_WCMSD A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'AND B.UPFLG='N' AND B.LOTID in (" + strPanelID + ") UNION SELECT B.LOTID AS PANELID,A.CARTID FROM WCMSDH A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'AND B.UPFLG='N' AND B.LOTID in (" + strPanelID + "))AS D)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT B.LOTID,A.CARTID FROM New_WCMSD A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND B.LOTID in (" + strPanelID + ") UNION SELECT B.LOTID,A.CARTID FROM WCMSDH A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND B.LOTID in (" + strPanelID + "))AS A";
                        break;
                    case "Automotine":
                        strQuerySQL += "SELECT TOP 20 PanelID,CARTID FROM (SELECT ROW_NUMBER()OVER(ORDER BY PanelID )AS ROWNUMBER,PanelID,CARTID FROM (SELECT SerialNo AS PanelID,CARTID FROM New_WCMSD WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND SerialNo in (" + strPanelID + ")UNION SELECT SerialNo AS PanelID,CARTID FROM WCMSDH WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND SerialNo in (" + strPanelID + "))AS D)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT SerialNo,CARTID FROM New_WCMSD WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND SerialNo in (" + strPanelID + ") UNION SELECT SerialNo,CARTID FROM WCMSDH WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND SerialNo in (" + strPanelID + "))AS A";
                        break;
                    case "Panasonic":
                        strQuerySQL += "SELECT TOP 20 PANELID,CARTID FROM (SELECT ROW_NUMBER()OVER(ORDER BY PANELID )AS ROWNUMBER,PANELID,CARTID FROM (SELECT B.PANELID,A.CARTID FROM New_WCMSD A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ")UNION SELECT B.PANELID,A.CARTID FROM WCMSDH A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + "))AS D)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT B.PANELID,A.CARTID FROM New_WCMSD A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ") UNION SELECT B.PANELID,A.CARTID FROM WCMSDH A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + "))AS A";
                        break;
                    case "Audi":
                        strQuerySQL += "SELECT TOP 20 PANELID,CARTID FROM ((SELECT ROW_NUMBER()OVER(ORDER BY PANELID )AS ROWNUMBER,PANELID,CARTID FROM (SELECT B.PANELID,A.CARTID FROM New_WCMSD A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")UNION SELECT B.PANELID,A.CARTID FROM WCMSDH A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + "))AS D))C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)ORDER BY PANELID";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT B.PANELID,A.CARTID FROM New_WCMSD A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ") UNION SELECT B.PANELID,A.CARTID FROM WCMSDH A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + "))AS A";
                        break;
                    case "ALL":
                        strQuerySQL += "SELECT TOP 20 FRENTITYID AS PanelID,CARTID FROM(SELECT ROW_NUMBER()OVER(ORDER BY ENTITYID )AS ROWNUMBER,w2.FRENTITYID,w.CARTID FROM(SELECT ENTITYID,CARTID,TRANSDATE FROM  New_WLTM WITH(NOLOCK) WHERE FABID='B' UNION ALL SELECT ENTITYID,CARTID,TRANSDATE FROM WLTMH WITH(NOLOCK)WHERE FABID='B') AS w LEFT JOIN (SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM New_WLTX WITH(NOLOCK) WHERE FABID='B' UNION ALL SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM WLTXH WITH(NOLOCK)WHERE FABID='B') AS w2 ON W.ENTITYID=W2.TOENTITYID WHERE w2.VERB='MRLT' AND w2.FRENTITYID IN(" + strPanelID + "))AS C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM (SELECT w2.FRENTITYID,w.CARTID FROM (SELECT ENTITYID,CARTID,TRANSDATE FROM  New_WLTM WITH(NOLOCK) WHERE FABID='B' UNION ALL SELECT ENTITYID,CARTID,TRANSDATE FROM WLTMH WITH(NOLOCK) WHERE FABID='B') AS w LEFT JOIN (SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM New_WLTX WITH(NOLOCK) WHERE FABID='B' UNION ALL SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM WLTXH WITH(NOLOCK)WHERE FABID='B') AS w2 ON W.ENTITYID=W2.TOENTITYID WHERE w2.VERB='MRLT' AND w2.FRENTITYID IN(" + strPanelID + "))AS A";
                        break;
                }

            }
            else if (name == "箱號查PANELID")
            {
                switch (DropDownList2.Text)
                {
                    case "BMW":
                        strQuerySQL += "SELECT TOP 20 CARTID,PanelID FROM (SELECT ROW_NUMBER()OVER(ORDER BY CARTID )AS ROWNUMBER,CARTID,PanelID FROM (SELECT A.CARTID,B.LOTID AS PanelID FROM New_WCMSD A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'AND B.UPFLG='N' AND A.CARTID in (" + strPanelID + ") UNION SELECT A.CARTID,B.LOTID AS PANELID FROM WCMSDH A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "'AND B.UPFLG='N' AND A.CARTID in (" + strPanelID + "))AS D)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT A.CARTID,B.LOTID FROM New_WCMSD A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND A.CARTID in (" + strPanelID + ") UNION SELECT A.CARTID,B.LOTID FROM WCMSDH A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND A.CARTID in (" + strPanelID + "))AS A";
                        break;
                    case "Automotine":
                        strQuerySQL += "SELECT TOP 20 CARTID,PanelID FROM (SELECT ROW_NUMBER()OVER(ORDER BY CARTID )AS ROWNUMBER,CARTID,PanelID FROM (SELECT CARTID,SerialNo AS PanelID FROM New_WCMSD WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND CARTID in (" + strPanelID + ")UNION SELECT CARTID,SerialNo AS PanelID FROM WCMSDH WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND CARTID in (" + strPanelID + "))AS D)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT CARTID,SerialNo FROM New_WCMSD WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND CARTID in (" + strPanelID + ") UNION SELECT CARTID,SerialNo FROM WCMSDH WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND CARTID in (" + strPanelID + "))AS A";
                        break;
                    case "Panasonic":
                        strQuerySQL += "SELECT TOP 20 CARTID,PANELID FROM (SELECT ROW_NUMBER()OVER(ORDER BY CARTID )AS ROWNUMBER,CARTID,PANELID FROM (SELECT A.CARTID,B.PANELID FROM New_WCMSD A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")UNION SELECT A.CARTID,B.PANELID FROM WCMSDH A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + "))AS D)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT A.CARTID,B.PANELID FROM New_WCMSD A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ") UNION SELECT A.CARTID,B.PANELID FROM WCMSDH A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + "))AS A";
                        break;
                    case "Audi":
                        strQuerySQL += "SELECT TOP 20 CARTID,PANELID FROM ((SELECT ROW_NUMBER()OVER(ORDER BY CARTID )AS ROWNUMBER,CARTID,PANELID FROM (SELECT A.CARTID,B.PANELID FROM New_WCMSD A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")UNION SELECT A.CARTID,B.PANELID FROM WCMSDH A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + "))AS D))C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)ORDER BY CARTID";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT A.CARTID,B.PANELID FROM New_WCMSD A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ") UNION SELECT A.CARTID,B.PANELID FROM WCMSDH A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + "))AS A";
                        break;
                    case "ALL":
                        strQuerySQL += "SELECT TOP 20 w.CARTID,w2.FRENTITYID FROM (SELECT ENTITYID,CARTID,TRANSDATE FROM  New_WLTM WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT ENTITYID,CARTID,TRANSDATE FROM WLTMH WITH(NOLOCK)WHERE FABID='B') AS w LEFT JOIN (SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM New_WLTX WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM WLTXH WITH(NOLOCK)WHERE FABID='B') AS w2 ON W.ENTITYID=W2.TOENTITYID WHERE w2.VERB='MRLT'AND w.CARTID IN (" + strPanelID + ")";
                        StrQuerySQL += "SELECT COUNT(*) FROM ( SELECT w.CARTID,w2.FRENTITYID FROM (SELECT ENTITYID,CARTID,TRANSDATE FROM  New_WLTM WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT ENTITYID,CARTID,TRANSDATE FROM WLTMH WITH(NOLOCK)WHERE FABID='B') AS w LEFT JOIN (SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM New_WLTX WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM WLTXH WITH(NOLOCK)WHERE FABID='B') AS w2 ON W.ENTITYID=W2.TOENTITYID WHERE w2.VERB='MRLT'AND w.CARTID IN (" + strPanelID + "))AS A";
                        break;
                }
            }

            else if (name == "PANELID查生產記錄")
            {
                strQuerySQL += "SELECT TOP 20 * FROM (SELECT ROW_NUMBER()OVER(ORDER BY USERID )AS ROWNUMBER, * FROM (SELECT VERB,FRENTITYID,TOENTITYID,CWORKCTR,NWORKCTR,CSUBLINEID,NSUBLINEID,FRPRODID,TOPRODID,FRWONBR,TOWONBR,TRANSDATE,USERID FROM dbo.WLTXH WHERE [FABID]='B' AND FRENTITYID in (" + strPanelID + ") Union SELECT VERB,FRENTITYID,TOENTITYID,CWORKCTR,NWORKCTR,CSUBLINEID,NSUBLINEID,FRPRODID,TOPRODID,FRWONBR,TOWONBR,TRANSDATE,USERID FROM dbo.WLTXH WHERE [FABID]='B' AND TOENTITYID in (" + strPanelID + ") ) A)C WHERE C.ROWNUMBER>((" + PageIndex + "-1 )*20)";
                StrQuerySQL += "SELECT COUNT(*) FROM (SELECT VERB,FRENTITYID,TOENTITYID,CWORKCTR,NWORKCTR,CSUBLINEID,NSUBLINEID,FRPRODID,TOPRODID,FRWONBR,TOWONBR,TRANSDATE,USERID FROM dbo.WLTXH WHERE [FABID]='B' AND FRENTITYID in (" + strPanelID + ") Union SELECT VERB,FRENTITYID,TOENTITYID,CWORKCTR,NWORKCTR,CSUBLINEID,NSUBLINEID,FRPRODID,TOPRODID,FRWONBR,TOWONBR,TRANSDATE,USERID FROM dbo.WLTXH WHERE [FABID]='B' AND TOENTITYID in (" + strPanelID + ") ) A ";
            }
            Count = MSSQLHelper.ExecuteScalar(StrQuerySQL);
            DataTable dt = dbConn.GetDataSetBySql(strQuerySQL).Tables[0];
            GridView1.DataSource = dt;
            GridView1.DataBind();
            PagedDataSource pds = new PagedDataSource();
            //设置总行数
            AspNetPager1.RecordCount = (int)Count;
            //设置分页的数据源    
            pds.DataSource = dt.DefaultView;
            //设置当前页
            pds.CurrentPageIndex = PageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            //启用分页
            pds.AllowPaging = true;
            return true;
        }
        #endregion
             
        #region 清除點擊事件
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDT.Text = System.DateTime.Now.AddYears(-5).ToString("yyyy-MM-dd");
            txtEndDT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            TxtPanelId.Text = "";
            lblMsg.Text = "";
            btnExport.Enabled = false;
            GridView1.Visible = false;
            AspNetPager1.Visible = false;
        }
        #endregion

        #region 導出報表事件
        protected void btnExport_Click(object sender, EventArgs e)
        {

            string strTimeStart = txtStartDT.Text + " 00:00:00";
            string strTimeENd = txtEndDT.Text + " 23:59:59";
            name = DropDownList1.SelectedValue;//定義下拉列表选中值
            string PanelID = TxtPanelId.Text;

            //PANELID轉換字符串格式
            string strlotTemp1 = TxtPanelId.Text.Replace("\r\n", ",");
            string[] strtxtLot = strlotTemp1.Split(',');
            string strPanelID = "";
            for (int i = 0; i < strtxtLot.Length; i++)
            {
                strPanelID += "'" + strtxtLot[i] + "',";

            }
            strPanelID = strPanelID.TrimEnd(',');

            if (name == "PANELID查箱號")
            {

                switch (DropDownList2.Text)
                {

                    case "BMW":
                        STRQuerySQL += "SELECT B.LOTID AS PanelID,A.CARTID FROM New_WCMSD A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND B.LOTID in (" + strPanelID + ")UNION SELECT DISTINCT B.LOTID AS PanelID,A.CARTID FROM WCMSDH A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND B.LOTID in (" + strPanelID + ")";
                        break;
                    case "Automotine":
                        STRQuerySQL += "SELECT SerialNo AS PanelID,CARTID FROM New_WCMSD WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND SerialNo in (" + strPanelID + ")UNION SELECT DISTINCT SerialNo AS PanelID,CARTID FROM WCMSDH WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND SerialNo in (" + strPanelID + ")";
                        break;
                    case "Panasonic":
                        STRQuerySQL += "SELECT B.PANELID,A.CARTID FROM New_WCMSD A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ")UNION SELECT DISTINCT B.PANELID,A.CARTID FROM WCMSDH A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ")";
                        break;
                    case "Audi":
                        STRQuerySQL += "SELECT B.PANELID,A.CARTID FROM New_WCMSD A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ")UNION SELECT DISTINCT B.PANELID,A.CARTID FROM WCMSDH A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.PANELID in (" + strPanelID + ") ";
                        break;
                    case"ALL":
                        STRQuerySQL += "SELECT w2.FRENTITYID,w.CARTID FROM (SELECT ENTITYID,CARTID,TRANSDATE FROM  New_WLTM WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT ENTITYID,CARTID,TRANSDATE FROM WLTMH WITH(NOLOCK)WHERE FABID='B') AS w LEFT JOIN (SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM New_WLTX WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM WLTXH WITH(NOLOCK)WHERE FABID='B') AS w2 ON W.ENTITYID=W2.TOENTITYID WHERE w2.VERB='MRLT' AND w2.FRENTITYID IN(" + strPanelID + ")";
                        break;
                }

            }
            else if (DropDownList1.Text == "箱號查PANELID")
            {
                switch (DropDownList2.Text)
                {
                    case "BMW":
                        STRQuerySQL += "SELECT A.CARTID,B.LOTID AS PanelID FROM New_WCMSD A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND A.CARTID in (" + strPanelID + ")UNION SELECT DISTINCT A.CARTID,B.LOTID AS PanelID FROM WCMSDH A JOIN New_FILEPARM B ON A.SerialNo=B.SN WHERE fabid='b' and  A.TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND B.UPFLG='N' AND A.CARTID in (" + strPanelID + ")";
                        break;   
                    case "Automotine":
                        STRQuerySQL += "SELECT CARTID,SerialNo AS PanelID FROM New_WCMSD WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND CARTID in (" + strPanelID + ")UNION SELECT DISTINCT CARTID,SerialNo AS PanelID FROM WCMSDH WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND CARTID in (" + strPanelID + ")";
                        break;
                    case "Panasonic":
                        STRQuerySQL += "SELECT A.CARTID,B.PANELID FROM New_WCMSD A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")UNION SELECT DISTINCT A.CARTID,B.PANELID FROM WCMSDH A JOIN New_TBL_PANSONICSN B ON A.SerialNo=B.CUSTOMERSN WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")";                        
                        break;
                    case "Audi":
                        STRQuerySQL += "SELECT A.CARTID,B.PANELID FROM New_WCMSD A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")UNION SELECT DISTINCT A.CARTID,B.PANELID FROM WCMSDH A JOIN New_FILEPARM_Audi B ON A.SerialNo=SUBSTRING(B.SerialNumber,15,2)+SUBSTRING(B.SerialNumber,7,6)+SUBSTRING(B.SerialNumber,17,4) WHERE fabid='b' and TIME_STAMP BETWEEN '" + strTimeStart + "' AND '" + strTimeENd + "' AND A.CARTID in (" + strPanelID + ")";
                        break;
                    case "ALL":
                        STRQuerySQL += "SELECT w.CARTID,w2.FRENTITYID FROM (SELECT ENTITYID,CARTID,TRANSDATE FROM  New_WLTM WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT ENTITYID,CARTID,TRANSDATE FROM WLTMH WITH(NOLOCK)WHERE FABID='B') AS w LEFT JOIN (SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM New_WLTX WITH(NOLOCK)WHERE FABID='B' UNION ALL SELECT FRENTITYID,TOENTITYID,VERB,TRANSDATE FROM WLTXH WITH(NOLOCK)WHERE FABID='B') AS w2 ON W.ENTITYID=W2.TOENTITYID WHERE w2.VERB='MRLT'AND w.CARTID IN (" + strPanelID + ")";
                        break;
                }
            }

            else if (DropDownList1.Text == "PANELID查生產記錄")
            {
                STRQuerySQL += "SELECT * FROM (SELECT VERB,FRENTITYID,TOENTITYID,CWORKCTR,NWORKCTR,CSUBLINEID,NSUBLINEID,FRPRODID,TOPRODID,FRWONBR,TOWONBR,TRANSDATE,USERID FROM dbo.WLTXH WHERE [FABID]='B' AND FRENTITYID in (" + strPanelID + ") Union SELECT VERB,FRENTITYID,TOENTITYID,CWORKCTR,NWORKCTR,CSUBLINEID,NSUBLINEID,FRPRODID,TOPRODID,FRWONBR,TOWONBR,TRANSDATE,USERID FROM dbo.WLTXH WHERE [FABID]='B' AND TOENTITYID in (" + strPanelID + ") ) A order by A.Transdate";
            }
            DataTable table = new DataTable();
            DataTable dt = dbConn.GetDataSetBySql(STRQuerySQL).Tables[0];
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (GridView1.Rows.Count > 0)
            {

                switch (DropDownList1.Text)
                {
                    case "PANELID查箱號":
                        table = ExcelIO.ExcelHelper.GridView2DataTable(GridView1);
                        string filename = "PANELID查箱號.xls";
                        ExcelIO.ExcelHelper.ExportByWeb(table, "", filename);
                        break;
                    case "箱號查PANELID":
                        table = ExcelIO.ExcelHelper.GridView2DataTable(GridView1);
                        string filename1 = "箱號查PANELID.xls";
                        ExcelIO.ExcelHelper.ExportByWeb(table, "", filename1);
                        break;
                    case "PANELID查生產記錄":
                        table = ExcelIO.ExcelHelper.GridView2DataTable(GridView1);
                        string filename2 = "PANELID查生產記錄.xls";
                        ExcelIO.ExcelHelper.ExportByWeb(table, "", filename2);
                        break;
                }

            }
            else
            {
                Response.Write("<script>alert('沒有數據可以進行導出');</script>");
            }
        }
        #endregion

        #region 數據分頁綁定
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindGridview();
        }
        #endregion

    }
}