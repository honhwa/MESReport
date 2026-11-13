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
using System.IO;



namespace Report.Reports.AOI_FP
{
    public partial class AOIFP : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        public DataSet dsDetail = new DataSet();
        string plid;
        string plline;
        string plcworkctr;
        string pltolotlevel;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtDate.Attributes.Add("onclick", "setday(this)");
            this.txtToDate.Attributes.Add("onclick", "setday(this)");
            if (!Page.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
                this.txtDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", System.DateTime.Now.AddDays(0));
                this.txtToDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", System.DateTime.Now.AddDays(0));
            }
            lbl_MediaPlayer.Visible = true;

        }

        public static bool BCreateDirectory(string path)
        {
            uint state = 0;
            if (!Directory.Exists("Q:"))
            {
                state = WNetHelper.WNetAddConnection(@"shanghai\qnapr01", "qr0137,", @"\\10.63.70.10\vedio_aoi", "Q:");
            }
            if (state.Equals(0))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            else
            {
                throw new Exception("添加网络驱动器错误，错误号：" + state.ToString());
            }
        }

        void GetRecord()
        {
            RecordLog Log = new RecordLog();

            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string StrSql = string.Empty;
            if(tbpld.Text!="")
            {
                StrSql = @"SELECT LEFT(TRANSID,10) AS TRANSDATE,TRANSDATE AS TRANSTIME,v.TOPRODID,v.FRENTITYID,v.CWORKCTR,v.CSUBLINEID,v.TOLOTLEVEL FROM New_VWLTXA AS v";
                StrSql += " " + "WHERE v.FABID='B' AND v.VERB='MVOU'";
                if (this.CheckBoxList1.SelectedIndex>=0)
                {
                    if (CheckBoxList1.SelectedValue != "")
                    {
                        if (CheckBoxList1.SelectedValue == "0")
                        {
                            StrSql += " " + "AND v.CWORKCTR='8210'";
                        }
                        else if (CheckBoxList1.SelectedValue == "1")
                        {
                            StrSql += " " + "AND v.CWORKCTR='8230'";
                        }
                    }
                    
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請配合站別查詢！')</script>");
                    return;
                }
                string strEntityId = tbpld.Text.ToUpper().Trim().TrimEnd(",".ToCharArray()).Replace(",", "','");
                StrSql += " " + "AND v.FRENTITYID IN ('" + strEntityId + "')";
                StrSql += " " + "ORDER BY V.TRANSDATE ASC";
            }
            else
            {
                
                if (txtDate.Text == "" && txtToDate.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請輸入查詢日期！')</script>");
                    return;
                }
                if (tbnm.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請輸入查詢數量！')</script>");
                    return;
                }
                if (chkline.SelectedValue == "")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請選擇拉別！')</script>");
                    return;
                }
                else
                {
                    StrSql = @"SELECT TOP " + tbnm.Text.Trim() + " LEFT(TRANSID,10) AS TRANSDATE,TRANSDATE AS TRANSTIME,v.TOPRODID,v.FRENTITYID,v.CWORKCTR,v.CSUBLINEID,v.TOLOTLEVEL FROM New_VWLTXA AS v";
                    StrSql +=" "+"WHERE v.FABID='B' AND v.VERB='MVOU'";
                    
                }

                StringBuilder strchkProd = new StringBuilder();
                if (this.chkProd.Items.Count != 0)
                {
                    if (this.chkProd.SelectedIndex >= 0)
                    {
                        foreach (ListItem li in this.chkProd.Items)
                        {
                            if (li.Selected)
                            {
                                strchkProd.Append("'").Append(li.Value).Append("'");
                            }
                        }
                    }
                }
                if (strchkProd.Length > 0)
                {
                    StrSql += " " + "AND v.TOPRODID IN (" + strchkProd.ToString().Replace("''", "','") + ")";
                }

                StringBuilder strchkline = new StringBuilder();
                if (this.chkline.Items.Count != 0)
                {
                    if (this.chkline.SelectedIndex >= 0)
                    {
                        foreach (ListItem li in this.chkline.Items)
                        {
                            if (li.Selected)
                            {
                                strchkline.Append("'").Append(li.Value).Append("'");
                            }
                        }
                    }
                }
                if (strchkline.Length > 0)
                {
                    StrSql += " " + "AND v.CSUBLINEID IN (" + strchkline.ToString().Replace("''", "','") + ")";
                }

                if (this.CheckBoxList1.SelectedIndex >= 0)
                {
                    if (CheckBoxList1.SelectedValue == "0")
                    {
                        StrSql += " " + "AND v.CWORKCTR='8210'";
                    }
                    else if (CheckBoxList1.SelectedValue == "1")
                    {
                        StrSql += " " + "AND v.CWORKCTR='8230'";
                    }
                }

                if (CheckBoxList2.SelectedValue != "")
                {
                    if (CheckBoxList2.SelectedValue == "0")
                    {
                        StrSql += " " + "AND v.TOLOTLEVEL='AA'";
                    }
                    else if (CheckBoxList2.SelectedValue == "1")
                    {
                        StrSql += " " + "AND v.TOLOTLEVEL='S'";
                    }
                }
                StrSql += " " + "AND v.TRANSDATE BETWEEN '" + txtDate.Text.Trim() + "' AND '" + txtToDate.Text.Trim() + "'";
                StrSql += " " + " order by NEWID()";
            }
            dsDetail = this.dbConn.GetDataSetBySql(StrSql);
            this.GridView1.DataSource = dsDetail;
            this.GridView1.DataBind();


        }

        protected void Query_Click(object sender, EventArgs e)
        {
            lbl_MediaPlayer.Visible = false;
            GetRecord();
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            
            lbl_MediaPlayer.Visible = false;
            chkProd.ClearSelection();
            chkline.ClearSelection();
            CheckBoxList1.ClearSelection();
            CheckBoxList2.ClearSelection();
            tbnm.Text = "";
            tbpanel.Text = "";
            tbusr.Text = "";
            tbpwd.Text = "";
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataBind();
            GetRecord();
        }

        protected void DataGridSum_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            
            string pltime = "";
            string movnm = "";
            foreach (GridViewRow row in this.GridView1.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ckb = row.Cells[0].FindControl("CheckBox1") as CheckBox;
                    if (ckb.Checked)
                    {
                        pltime = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(row.Cells[1].Text));
                        plid = row.Cells[4].Text;
                        plline = row.Cells[6].Text;
                    }

                }
            }

            this.tbpanel.Text = plid;

            BCreateDirectory("Q:");

            DirectoryInfo theFolder = new DirectoryInfo(@"Q:\vedio\" + pltime + "\\" + plline);
            //DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            ////遍历文件夹
            //foreach (DirectoryInfo NextFolder in dirInfo)
            //{
            FileInfo[] fileInfo = theFolder.GetFiles("*" + plid + "*", SearchOption.TopDirectoryOnly);
                foreach (FileInfo NextFile in fileInfo) //遍历文件
                    movnm = NextFile.Name;
            //}
            if (movnm == "")
            {
                lbl_MediaPlayer.Visible = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('沒有影像文件！')</script>");
                return;
            }
            else
            {
                System.IO.DirectoryInfo dirMOV = new System.IO.DirectoryInfo("Q:\\vedio\\" + pltime + "\\" + plline + "\\" + movnm);
                DateTime dtMOV = dirMOV.CreationTime;
                //this.tbmovtime.Text = dtMOV.ToString();
                this.lbl_MediaPlayer.Text = this.PlayMedia("Q:\\vedio\\" + pltime + "\\" + plline + "\\" + movnm);

            }
        }
        public string PlayMedia(string MediaFile)
        {
            string strScript =
             "<TABLE id=\"Table2\" align=\"center\" border=\"0\" runat=\"server\" width = '900' height ='700'>"
             + "<TR>"
             + "<TD align=\"center\">"
             + "<OBJECT id=\"mdpTrailer\" classid=\"clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95\" name=\"mdpTrailer\" width='900' height = '700'>"
             + "<param name=\"autoStart\" value=\"false\"/>"
             + "<param name=\"wmode\"     value=\"transparent\" />"
             + "<param name=\"uiMode\"    value=\"full\" />"
             + "<param name=\"loop\"      value=\"false\" />"
             + "<PARAM NAME=\"Filename\" VALUE=\"" + MediaFile + "\">"
             + "</OBJECT>"
             + "</TD>"
             + "</TR>"
             + "</TABLE>";

            return strScript;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (tbpanel.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('沒有要復判PANELID！')</script>");
                return;
            }
            if (tbusr.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請輸入復判人員工號！')</script>");
                return;
            }
            if (tbpwd.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請輸入密碼！')</script>");
                return;
            }
            if (RadioButtonList1.SelectedIndex == -1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('請選擇復判結果！')</script>");
                return;
            }

            string sqlstr = "select * from memm where userid='" + tbusr.Text + "' and password= '" + tbpwd.Text + "'";
            DataSet ds = dbConn.GetDataSetBySql(sqlstr);
            if (ds.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('人員帳號或密碼不正確！')</script>");
                return;
            }

            foreach (GridViewRow row in this.GridView1.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ckb = row.Cells[0].FindControl("CheckBox1") as CheckBox;
                    if (ckb.Checked)
                    {
                        plcworkctr = row.Cells[5].Text;
                        plline = row.Cells[6].Text;
                        pltolotlevel = row.Cells[7].Text;
                    }

                }
            }

            string sqlinsert = "insert into aoifp([panelid],[userid],[CSUBLINEID],[CWORKCTR],[TOLOTLEVEL],[fpjg])";
            sqlinsert += "values('" + tbpanel.Text + "','" + tbusr.Text + "','" + plline + "','" + plcworkctr + "','" + pltolotlevel + "','" + RadioButtonList1.SelectedItem.Text + "')";
            this.dbConn.ExecuteSql(sqlinsert);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('復判成功！')</script>");
        }
    }
}
