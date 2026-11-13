using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Report.Library;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Threading;

namespace WebReport
{
    public partial class OnLineReportAssyVerBMW : System.Web.UI.Page
    {

        string[] Finishing_A = { "Assy", "Module Test", "OQC" };
        string[] Finishing_L = { "'2F-ASSY-L01'", "'2F-ASSY-L01'", "'2F-ASSY-L01'" };
        string[] Finishing_S = { "8350", "8353", "8360" };
        string[] Finishing_V = { "MVOU", "MVOU", "MVOU", "MVOU" };
        int[] planAcTarget = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] planPerTarget = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        long current = 0;  //當前線別
        //float fAssyYieldTarget = 99.5F, fModuleTestYieldTarget = 99, fOQCYieldTarget = 100;
        //DataSet dsActAndTarget;
        DataSet[] dsPlanPerTarget = new DataSet[3];
        string strProjectName, strProdID, strCustomer, strModTProjectName, strOQCProjectName;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //string strProjectName, strProdID, strCustomer, strModTProjectName, strOQCProjectName;
                string[] strLine = Page.Request.QueryString["Line"].ToString().Split(',');
                long idx = ((DateTime.Now.ToFileTime() - DateTime.Today.ToFileTime()) / 10000 / TimerPage.Interval) % strLine.Length;
                current = idx;
                if (strLine.Length > 0 && strLine[current] != string.Empty)
                {
                    Finishing_L[0] = "'" + strLine[0] + "'";
                    Finishing_L[1] = "'" + strLine[0] + "'";
                    Finishing_L[2] = "'" + strLine[0] + "'";
                }
                // Setup Title & Get ProjectName
                //lblTitle.Text = "線別Line: {0}";
                DataSet ds = GetProjectNameInWIP(Finishing_L[0], Finishing_S[0]);
                DataSet dsModT = GetProjectNameInWIP(Finishing_L[0], Finishing_S[1]);
                DataSet dsOqc = GetProjectNameInWIP(Finishing_L[0], Finishing_S[2]);
                strProjectName = "";
                strModTProjectName = "";
                strOQCProjectName = "";
                strProdID = "";
                strCustomer = "";
                // Assy段 ProjectName
                if (ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strProjectName = ds.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        }
                        else
                        {
                            strProjectName = strProjectName + "," + ds.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        }

                    }

                }
                if (dsModT.Tables[0] != null)
                {
                    for (int i = 0; i < dsModT.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strModTProjectName = dsModT.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        }
                        else
                        {
                            strModTProjectName = strModTProjectName + "," + dsModT.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        }

                    }

                }
                if (dsOqc.Tables[0] != null)
                {

                    for (int i = 0; i < dsOqc.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strOQCProjectName = dsOqc.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        }
                        else
                        {
                            strOQCProjectName = strOQCProjectName + "," + dsOqc.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        }

                    }

                }

                if (strProjectName.Length > 1)
                {
                    // 算標準產量的參數
                    strProdID = ds.Tables[0].Rows[0]["ProdID"].ToString();
                    // Get客戶名
                    strCustomer = ds.Tables[0].Rows[0]["PRODGROUP"].ToString();
                }
                else if (strModTProjectName.Length > 1)
                {
                    // 算標準產量的參數
                    strProdID = dsModT.Tables[0].Rows[0]["ProdID"].ToString();
                    // Get客戶名
                    strCustomer = dsModT.Tables[0].Rows[0]["PRODGROUP"].ToString();
                }
                else if (strOQCProjectName.Length > 1)
                {
                    // 算標準產量的參數
                    strProdID = dsOqc.Tables[0].Rows[0]["ProdID"].ToString();
                    // Get客戶名
                    strCustomer = dsOqc.Tables[0].Rows[0]["PRODGROUP"].ToString();
                }


                if (string.IsNullOrEmpty(strCustomer))
                {
                    LabelNAME.Text = "BMW專線";
                    imgLogo.ImageUrl = "getLogo.ashx?Customer=BMW";
                    ImgSHOW1.ImageUrl = "getLogo.ashx?Customer=BMW1";
                    ImgSHOW2.ImageUrl = "getLogo.ashx?Customer=BMW2";
                    ImgSHOW3.ImageUrl = "getLogo.ashx?Customer=BMW3";
                    switch (strLine[0].Substring(0, 2))
                    {
                        case "2F":
                            imgLogo.ImageUrl = "getLogo.ashx?Customer=BMW";
                            break;
                        case "3F":
                            imgLogo.ImageUrl = "getLogo.ashx?Customer=DELPHI";
                            break;
                        default:
                            break;
                    }
                    if (strLine[0].ToString() == "4F-ASSY-L09")
                    {
                        imgLogo.ImageUrl = "getLogo.ashx?Customer=BMW";
                    }
                }
                else
                {
                    imgLogo.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim();
                    if (strCustomer.Trim()=="BMW")
                    {
                        LabelNAME.Text = "BMW專線";
                    }
                    else
                    {
                        LabelNAME.Text = strCustomer.Trim() + "專線";
                    }
                    ImgSHOW1.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim() + "1";
                    ImgSHOW2.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim() + "2";
                    ImgSHOW3.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim() + "3";
                }


                //if (strProjectName != "")
                //{
                //    lblProduct.Text = strProjectName;
                //}
                //else if (strModTProjectName != "")
                //{
                //    lblProduct.Text = strModTProjectName;
                //}
                //else
                //{
                //    lblProduct.Text = strOQCProjectName;
                //}
                //ImgSHOW1.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim() + "1";
                //ImgSHOW2.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim() + "2";
                //ImgSHOW3.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim() + "3";

                ds.Dispose();
                dsModT.Dispose();
                dsOqc.Dispose();
            }
            catch (SqlException)
            {
                Response.Write(DateTime.Now.ToLongTimeString() + " Database is busy or cannot connect... , System will wait 30sec then retry!");
                // wait 30s
                Thread.Sleep(30000);
                // 重新載入
                Response.Redirect(Request.RawUrl);
            }
            catch (InvalidOperationException ex)
            {
                Response.Write(DateTime.Now.ToLongTimeString() + ex.Message + "System will wait 30sec then retry!");
                // wait 30s
                Thread.Sleep(30000);
                // 重新載入
                Response.Redirect(Request.RawUrl);

            }





        }

        private static DataSet GetProjectNameInWIP(string strLine, string strStation)
        {
            string BeginTime;
            string EndTime;

            GetTime(out BeginTime, out EndTime);

            DBConn conn = new DBConn("");
            string strsql = "select TOPRODID,PROJECT_NAME,ProdID,MPIM.PRODGROUP " +
                              "from (select  TOPRODID From  [dbo].[New_WLTX] TX ,MPIM " +
                                                    " with (Nolock)" +
                                                    "where transdate >= cast('{2}' as datetime)" +
                                                    "  and transdate < cast('{3}' as datetime)" +
                                                    "  and  csublineid in ({0})" +
                                                    "  and  verb = 'MVOU'" +
                                                    "  and  [CworkCTR] ='{1}'" +
                                                    " group by TOPRODID ) A , MPIM" +
                               " with (Nolock)" +
                               " WHERE MPIM.ProdID = A.TOPRODID";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql); ;
            return ds;
        }

        private static void GetTime(out string BeginTime, out string EndTime)
        {

            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 07:00";
                EndTime = DateTime.Now.ToShortDateString() + " 19:00";
            }
            else if (DateTime.Now.Hour >= 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 19:00";
                EndTime = DateTime.Now.AddDays(1).ToShortDateString() + " 07:00";
            }
            else
            {
                BeginTime = DateTime.Now.AddDays(-1).ToShortDateString() + " 19:00";
                EndTime = DateTime.Now.ToShortDateString() + " 07:00";
            }
        }

        protected void TimerPage_Tick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

    }
}