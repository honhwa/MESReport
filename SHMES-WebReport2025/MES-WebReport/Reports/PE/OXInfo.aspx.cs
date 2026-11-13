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
using System.Text;
using Report.Library;
using Report.Sources;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;


namespace Report.Reports.PE
{
    public partial class OXInfo : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
            {
                Response.Redirect("../../Login.aspx");
            }
        }

        protected string ReturnXY(string num)
        {
            string pos = string.Empty;
            if (num == "1") { pos = "A"; } else if (num == "2") { pos = "B"; } if (num == "3") { pos = "C"; } else if (num == "4") { pos = "D"; }
            if (num == "5") { pos = "E"; } else if (num == "6") { pos = "F"; } if (num == "7") { pos = "G"; } else if (num == "8") { pos = "H"; }
            if (num == "9") { pos = "J"; } else if (num == "10") { pos = "K"; } if (num == "11") { pos = "L"; } else if (num == "12") { pos = "M"; }
            if (num == "13") { pos = "N"; } else if (num == "14") { pos = "P"; } if (num == "15") { pos = "Q"; } else if (num == "16") { pos = "R"; }
            if (num == "17") { pos = "S"; } else if (num == "18") { pos = "T"; } if (num == "19") { pos = "U"; } else if (num == "20") { pos = "V"; }
            if (num == "21") { pos = "W"; } else if (num == "22") { pos = "X"; } if (num == "23") { pos = "Y"; } else if (num == "24") { pos = "Z"; }
            if (num == "25") { pos = "0"; } else if (num == "26") { pos = "1"; } if (num == "27") { pos = "2"; } else if (num == "28") { pos = "3"; }
            if (num == "29") { pos = "4"; } else if (num == "30") { pos = "5"; } if (num == "31") { pos = "6"; } else if (num == "32") { pos = "7"; }
            if (num == "33") { pos = "8"; } else if (num == "34") { pos = "9"; }
            return pos;
        }

        protected DataTable ReadingOX(string subentityid)
        {
            int Xbar = 0;
            int Ybar = 0;
            string OX = string.Empty;
            string OXDefect = string.Empty;
            string OXGrade = string.Empty;
            string Glass = string.Empty;
            string Entityid = string.Empty;
            string time = string.Empty;
            int qty = 0;
            DataTable dt_final = new DataTable();

            dt_final.Columns.Add("GlassID", typeof(string));
            dt_final.Columns.Add("PanelID", typeof(string));
            dt_final.Columns.Add("CartonID", typeof(string));
            dt_final.Columns.Add("Time", typeof(string));
            dt_final.Columns.Add("Judge", typeof(string));
            dt_final.Columns.Add("DefectCode", typeof(string));

            StringBuilder stb = new StringBuilder();
            stb.Append("SELECT TOP 1* FROM EIF.BGOX WITH(NOLOCK) WHERE SUBENTITYID='" + subentityid + "' AND GSFLAG='G'");
            DataTable dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                //this.Response.Write("<script language=javascript>window.alert(" + subentityid + "查無資料,請確認!');</script>");
            }
            else
            {
                Glass = dt.Rows[0][2].ToString();
                Entityid = dt.Rows[0][0].ToString();
                OX = dt.Rows[0][6].ToString();
                OXDefect = dt.Rows[0][9].ToString();
                qty = Convert.ToInt32(dt.Rows[0][10]);
                OX = OX.Replace("N", " ");
                Xbar = OX.IndexOf(" ");
                Ybar = qty / Xbar;
                OX = OX.Replace(" ","1");
                OXDefect = OXDefect.Replace("N", "2");
                OXDefect = OXDefect.Replace(" ", "2");

                for (int j = 1; j <= Ybar; j++)
                {
                    for (int i = 1; i <= Xbar; i++)
                    {
                        dt_final.Rows.Add(subentityid, subentityid + ReturnXY(i.ToString()) + ReturnXY(j.ToString()), Entityid, dt.Rows[0][12].ToString(), OX.Substring((j - 1) * 34 + i - 1, 1) == "1" ? "O" : OX.Substring((j - 1) * 34 + i - 1, 1), OXDefect.Substring(((j - 1) * 34 + i - 1) * 4, 4) == "2222" ? "NA" : OXDefect.Substring(((j - 1) * 34 + i - 1) * 4, 4));
                    }
                }
            }
            return dt_final;
        }

        protected void butQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            string input = string.Empty;
            StringBuilder stb = new StringBuilder();
            DataTable dt_latest = new DataTable();
            string GlassID = string.Empty;
            DataTable P_data = new DataTable();
            if (txtInfo.Text.Trim().Length == 0)
            {
                this.Response.Write("<script language=javascript>window.alert('請輸入要查詢的資料!');</script>");
            }
            else
            {
                input = txtInfo.Text.Trim().Replace("\r\n", "");
                switch (DropDownList1.Text.ToString())
                {
                    case "PanelID":
                        int P_A = input.Length / 10;
                        if (P_A < 1)
                        {
                            this.Response.Write("<script language=javascript>window.alert('Panel ID至少為10碼,請確認!');</script>");
                        }
                        else
                        {
                            for (int P_B = 1; P_B <= P_A; P_B++)
                            {
                                string P_PanelID = input.Trim().Substring((P_B - 1) * 10, 10);
                                dt_latest = ReadingOX(P_PanelID.Substring(0, 8));
                                DataTable dtnew = dt_latest.Clone();
                                DataRow[] dt_tmp = dt_latest.Select("PanelID='" + P_PanelID.Trim().ToString() + "'");
                                if (dt_tmp == null || dt_tmp.Length == 0)
                                {
                                    this.Response.Write("<script language=javascript>window.alert('\"" + P_PanelID + "\"查無資料,請確認!');</script>");
                                }
                                else
                                {
                                    for (int k = 0; k < dt_tmp.Length; k++)
                                    {
                                        dtnew.ImportRow(dt_tmp[k]);
                                    }
                                    P_data.Merge(dtnew, true);
                                    dt_latest.Rows.Clear();
                                    dtnew.Rows.Clear();
                                }
                            }
                        }
                        GridView1.DataSource = P_data;
                        GridView1.DataBind();
                        lblCount.Text = P_data.Rows.Count.ToString();
                        butExport.Enabled = true;
                        P_data.Rows.Clear();
                        break;
                    case "CartonID":
                        double length = Convert.ToInt32(input.Length) / 10f;
                        if (length > 1)
                        {
                            this.Response.Write("<script language=javascript>window.alert('CartonID查詢模式不支持多箱,請一箱一箱查!');</script>");
                        }
                        else
                        {
                            stb.Append("SELECT SUBENTITYID FROM EIF.BGOX WITH(NOLOCK) WHERE ENTITYID='" + txtInfo.Text.Trim().ToString() + "' AND GSFLAG='G'");
                            DataTable datatable = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
                            DataTable dtfinal = new DataTable();
                            if (datatable.Rows.Count == 0)
                            {
                                this.Response.Write("<script language=javascript>window.alert('\"" + txtInfo.Text.Trim() + "\"查無資料,請確認!');</script>");
                            }
                            else
                            {
                                for (int l = 0; l < datatable.Rows.Count; l++)
                                {
                                    string glassid = datatable.Rows[l][0].ToString();
                                    dt_latest = ReadingOX(glassid.Trim());
                                    dtfinal.Merge(dt_latest, true);
                                    dt_latest.Rows.Clear();
                                }
                                GridView1.DataSource = dtfinal;
                                GridView1.DataBind();
                                lblCount.Text = dtfinal.Rows.Count.ToString();
                                dt_latest.Rows.Clear();
                                dtfinal.Rows.Clear();
                                datatable.Rows.Clear();
                                butExport.Enabled = true;
                            }
                        }
                        break;
                    case "GlassID":
                        int G_A = input.Length / 8;
                        DataTable dtcomplex = new DataTable();
                        DataTable dtreach = new DataTable();
                        if (G_A < 1)
                        {
                            this.Response.Write("<script language=javascript>window.alert('Glass ID至少為8碼,請確認!');</script>");
                        }
                        else
                        {
                            for (int G_B = 1; G_B <= G_A; G_B++)
                            {
                                string G_glassid = input.Trim().Substring((G_B - 1) * 8, 8);
                                dtcomplex = this.dbConn.GetDataSetBySql("SELECT SUBENTITYID FROM EIF.BGOX WITH(NOLOCK) WHERE SUBENTITYID='" + G_glassid.Trim() + "' AND GSFLAG='G'").Tables[0];
                                if (dtcomplex.Rows.Count == 0)
                                {
                                    this.Response.Write("<script language=javascript>window.alert('\"" + G_glassid + "\"查無資料,請確認!');</script>");
                                }
                                else
                                {
                                    for (int c = 0; c < dtcomplex.Rows.Count; c++)
                                    {
                                        dt_latest = ReadingOX(G_glassid.Trim().ToString());
                                        dtreach.Merge(dt_latest, true);
                                        dt_latest.Rows.Clear();
                                    }
                                }
                            }
                        }
                        GridView1.DataSource = dtreach;
                        GridView1.DataBind();
                        lblCount.Text = dtreach.Rows.Count.ToString();
                        dt_latest.Rows.Clear();
                        dtcomplex.Rows.Clear();
                        dt_latest.Rows.Clear();
                        butExport.Enabled = true;
                        break;
                }
            }
        }

        protected void butExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "UTF-8";
            Response.ContentType = "Content-Language;content=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.GridView1.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
        }

        protected void butRefresh_Click(object sender, EventArgs e)
        {
            DropDownList1.Text = "PanelID";
            txtInfo.Text = "";
            GridView1.DataSource = "";
            GridView1.DataBind();
            lblCount.Text = "0";
            butExport.Enabled = false;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
    }
}