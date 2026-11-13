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
using Report.Sources;
using Report.Library;
using System.Collections.Generic;

namespace Report.Reports.OQC
{
    public partial class OQCMonthReprot : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.BindGroup();
            }
        }

        private void BindGroup()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID ='B' ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.DlGroup.DataSource = null;
                    return;
                }
                this.DlGroup.DataSource = dt;
                this.DlGroup.DataTextField = "PRODGROUP";
                this.DlGroup.DataValueField = "PRODGROUP";
                this.DlGroup.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSerch_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            int j = 0;
            int x = 0;
            string strPlanDppm = "";
            string strPlanLrr = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string strSavePath = "";
            string strPath = "";
            List<int> arrList = new List<int>();
            DataTable dtdf = new DataTable();
            int h = 0;
            try
            {
                OQCMonth month = new OQCMonth();
                month.Group = this.DlGroup.Text;
                month.Year = this.dlYear.Text;
                month.Month = this.dlMonth.Text;
                month.Fabid = ((UserInfo)Session["UserInfo"]).FabId;
                ds = month.Read();
                dt = month.GetGoal();
                if (dt.Rows.Count == 0)
                {
                    this.lblErrMsg.Text = "選擇的Group在MPGTM中無資料!";
                    return;
                }
                strPlanDppm = dt.Rows[0]["PLANDPPM"].ToString();
                strPlanLrr = dt.Rows[0]["PLANLRR"].ToString();

                BaseExcel be = new BaseExcel(@"../../EXCModule/OQCMONTH.xls", "Sheet1");
                string[] arrStr = month.ColumnsName.Split(',');
                be.ExcelSheet.Cells[2, 2] = this.DlGroup.Text;
                be.ExcelSheet.Cells[3, 1] = "Total   " + this.DlGroup.Text + "   Summary-M" + this.dlYear.Text.Substring(3, 1) + this.dlMonth.Text;
                be.ExcelSheet.Cells[26, 1] = be.ExcelSheet.Cells[3, 1];
                be.ExcelSheet.Cells[47, 1] = "M" + this.dlYear.Text.Substring(3, 1) + this.dlMonth.Text.ToString() + " Type of Failure Definition";
                for (int z = 0; z < 2; z++)
                {
                    j = 0;
                    for (int i = 2; i < 14; i++)
                    {
                        be.ExcelSheet.Cells[4 + x, i] = arrStr[j];
                        j++;
                    }
                    x = x + 23;
                }

                for (int i = 0; i < 12; i++)
                {
                    be.ExcelSheet.Cells[5, i + 2] = ds.Tables[0].Rows[i]["DPPMTOTAL"].ToString();
                    be.ExcelSheet.Cells[6, 2 + i] = ds.Tables[0].Rows[i]["DPPMTOTALTEST"].ToString();
                    be.ExcelSheet.Cells[7, 2 + i] = ds.Tables[0].Rows[i]["DFQTY"].ToString();
                    //be.ExcelSheet.Cells[8, 2 + i] = ds.Tables[0].Rows[i]["DPPM"].ToString();
                    be.ExcelSheet.Cells[9, 2 + i] = strPlanDppm;
                    be.ExcelSheet.Cells[28, 2 + i] = ds.Tables[0].Rows[i]["LRRTOTAL"].ToString();
                    be.ExcelSheet.Cells[29, 2 + i] = ds.Tables[0].Rows[i]["LRRDFLOT"].ToString();
                    //be.ExcelSheet.Cells[30, 2 + i] = ds.Tables[0].Rows[i]["LRR"].ToString();
                    be.ExcelSheet.Cells[31, 2 + i] = strPlanLrr;
                }

                //arrList = month.getRoqcd();
                //for (int i = 0; i < arrList.Count; i++)
                //{
                //    if (arrList[i].ToString() == "0")
                //    {
                //        be.ExcelSheet.Cells[49 + i, 2] = "";
                //    }
                //    be.ExcelSheet.Cells[49 + i, 3] = arrList[i].ToString();
                //}

                dtdf = month.getDTRoqcd();
                for (int k = 0; k < 11; k++)
                {
                    be.ExcelSheet.Cells[49 + k, 2] = "";
                    be.ExcelSheet.Cells[49 + k, 3] = 0;
                }
                for (int i = 0; i < dtdf.Rows.Count; i++)
                {
                    if (i < 11)
                    {
                        be.ExcelSheet.Cells[49 + i, 2] = dtdf.Rows[i]["DEFECTCD"].ToString();
                        be.ExcelSheet.Cells[49 + i, 3] = dtdf.Rows[i]["OQCNGQTY"].ToString();
                        if (i == 10)
                        {
                            h = int.Parse(dtdf.Rows[i]["OQCNGQTY"].ToString());
                        }
                    }
                    else
                    {
                        be.ExcelSheet.Cells[59, 2] = "Other";
                        be.ExcelSheet.Cells[59, 3] = h + int.Parse(dtdf.Rows[i]["OQCNGQTY"].ToString());
                        h = h + int.Parse(dtdf.Rows[i]["OQCNGQTY"].ToString());
                    }
                }

                strPath =  "OQCMONTH" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
                strSavePath = @"../../ExportFile/" + strPath;
                if (be.ExcelSave(strSavePath + ".xls") == true)
                {
                    be.ExcelClose();
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    be.DeleteExcelFile("OQCMONTH");
                    //this.Response.Write("<script language=javascript>window.open('../LoadExcel.aspx?ExcelName=" + strSavePath + "');</script>");
                    this.Response.Write("<script language=javascript>window.open('../LoadExcel.aspx?ExcelName=" + strPath + "');</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
