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
using System.Text.RegularExpressions;

namespace Report.Reports.N90
{
    public partial class Cell2Report : System.Web.UI.Page
    {
        string DB = string.Empty;
        string Fabid = string.Empty;
        DBConn dbConn = new DBConn();
        string errMsg = string.Empty;
        DataTable dt2 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadioButton1.Checked = true;
            }
            Judge();
        }

        protected void Judge()
        {
            //if (RadioButton1.Checked)
            //{
            //    DB = @"[LCM1MESHIS\NJLCM1HIS].[NJPLCM1HIS].[DBO].[VWLTXA]";
            //    Fabid = "A";
            //}
            //else
            //{
            //    DB = @"[NJLCM2MES02\NJLCM2HIS].[NJPLCM2HIS].[DBO].[VWLTXA]";
            //    Fabid = "B";
            //}
            DB = @" [DBO].[New_VWLTXA]";
            Fabid = "B";
        }
        protected void butYes_Click(object sender, EventArgs e)
        {
            try
            {
                string txtFr = txtFromTime.Text.Trim();
                string txtTo = txtToTime.Text.Trim();

                DateTime fr = new DateTime();
                DateTime to = new DateTime();

                if (!DateTime.TryParse(txtFr,out fr))
                {
                    errMsg = "開始時間格式錯誤! \n";
                }
                else if (!DateTime.TryParse(txtTo, out to))
                {
                    errMsg += "結束時間格式錯誤! \n";
                }
                else if (fr > to)
                {
                    errMsg += "開始時間不能大於結束時間!! \n";
                    lblMsg.Text = errMsg + " 請重新輸入!";
                }
                else
                {
                    DataBound();
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("<script language=javascript>window.alert('"+ex.ToString()+"');</script>");
            }
        }

        private void DataBound()
        {
            DataTable dt = new DataTable();
            StringBuilder stb = new StringBuilder();
            errMsg = "";
            stb.Append(@"select fabid,verb,cworkctr,tocprodid,frprodid,toprodid,frlottype,tolottype,");
            stb.Append(@" frlotlevel,tolotlevel,defectcd,sum(centitytqty)qty1,sum(nentitytqty)qty2,datepart(week,mfgdate)Week");
            stb.Append(@" from "+DB+" with (nolock)");
            stb.Append(@" where fabid='" + Fabid + "'");
            stb.Append(@" and transdate >= '" + txtFromTime.Text.Trim() + "' and transdate <='" + txtToTime.Text.Trim() + "'");
            stb.Append(@" and cshopcd='CELL2'");
            stb.Append(@" and tolotlevel not like 'A%'");
            stb.Append(@" and tolottype in ('PR','EK','PY','RW','RS')");
            stb.Append(@" and verb='MVOU'");
            stb.Append(@" group by fabid,verb,cworkctr,tocprodid,frprodid,toprodid,frlottype,tolottype,datepart(week,mfgdate),");
            stb.Append(@" frlotlevel,tolotlevel,defectcd order by datepart(week,mfgdate)");

            dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                errMsg = "查無資料,請確定!";
                lblMsg.Text = errMsg;
            }
            else
            {
                System.Collections.Generic.Dictionary<string, string> fields = new Dictionary<string, string>();
                fields.Add("Fabid","Fabid");
                fields.Add("Verb","verb");
                fields.Add("Cworkctr","cworkctr");
                fields.Add("Tocprodid","tocprodid");
                fields.Add("Frprodid","frprodid");
                fields.Add("Toprodid","toprodid");
                fields.Add("Frlottype","frlottype");
                fields.Add("Tolottype","tolottype");
                fields.Add("Frlotlevel","frlotlevel");
                fields.Add("Tolotlevel","tolotlevel");
                fields.Add("Defectcd","defectcd");
                fields.Add("Qty1","qty1");
                fields.Add("Qty2","qty2");
                fields.Add("Week","Week");
                BaseExcel.ExportDataToExcel(Response, dt, fields, txtFromTime.Text.Trim().Replace("-", "").Substring(0, 8) + "~" + txtToTime.Text.Trim().Replace("-", "").Substring(0, 8));
            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked)
            {
                RadioButton2.Checked = false;
            }
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton2.Checked)
            {
                RadioButton1.Checked = false;
            }
        }
    }
}
