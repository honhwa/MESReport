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
    public partial class OnLineReportAssyVer8 : System.Web.UI.Page
    {
        string[] Finishing_A = { "Assy", "Module Test", "OQC" };
        string[] Finishing_L = { "'2F-ASSY-L01'", "'2F-ASSY-L01'", "'2F-ASSY-L01'" };
        //string[] Finishing_S = { "8350", "8353", "8360" };
        //string[] Finishing_S = { "8350", "8353" };
        string[] Finishing_S = { "8350" };
        string[] Finishing_V = { "MVOU", "MVOU", "MVOU", "MVOU" };
        int[] planAcTarget = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] planPerTarget = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        long current = 0;  //當前線別
        float fAssyYieldTarget = 99.5F, fModuleTestYieldTarget = 99, fOQCYieldTarget = 100;
        DataSet dsActAndTarget;
        DataSet[] dsPlanPerTarget = new DataSet[3];
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strProjectName, strProdID, strCustomer, strModTProjectName, strOQCProjectName;
                string[] strLine = Page.Request.QueryString["Line"].ToString().Split(',');
                long idx = ((DateTime.Now.ToFileTime() - DateTime.Today.ToFileTime()) / 10000 / TimerPage.Interval) % strLine.Length;
                current = idx;
                if (strLine.Length > 0 && strLine[current] != string.Empty)
                {
                    Finishing_L[0] = "'" + strLine[current] + "'";
                    Finishing_L[1] = "'" + strLine[current] + "'";
                    Finishing_L[2] = "'" + strLine[current] + "'";
                }
                // Setup Title & Get ProjectName
                lblTitle.Text = "{0}";
                DataSet ds = GetProjectNameInWIP(Finishing_L[0], Finishing_S[0]);
                //DataSet dsModT = GetProjectNameInWIP(Finishing_L[0], Finishing_S[1]);
                //DataSet dsOqc = GetProjectNameInWIP(Finishing_L[0], Finishing_S[2]);
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
                //if (dsModT.Tables[0] != null)
                //{
                //    for (int i = 0; i < dsModT.Tables[0].Rows.Count; i++)
                //    {
                //        if (i == 0)
                //        {
                //            strModTProjectName = dsModT.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                //        }
                //        else
                //        {
                //            strModTProjectName = strModTProjectName + "," + dsModT.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                //        }

                //    }

                //}
                //if (dsOqc.Tables[0] != null)
                //{

                //    for (int i = 0; i < dsOqc.Tables[0].Rows.Count; i++)
                //    {
                //        if (i == 0)
                //        {
                //            strOQCProjectName = dsOqc.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                //        }
                //        else
                //        {
                //            strOQCProjectName = strOQCProjectName + "," + dsOqc.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                //        }

                //    }

                //}

                if (strProjectName.Length > 1)
                {
                    // 算標準產量的參數
                    strProdID = ds.Tables[0].Rows[0]["ProdID"].ToString();
                    // Get客戶名
                    strCustomer = ds.Tables[0].Rows[0]["PRODGROUP"].ToString();
                }
                else if (strModTProjectName.Length > 1)
                {
                    //// 算標準產量的參數
                    //strProdID = dsModT.Tables[0].Rows[0]["ProdID"].ToString();
                    //// Get客戶名
                    //strCustomer = dsModT.Tables[0].Rows[0]["PRODGROUP"].ToString();
                }
                //else if (strOQCProjectName.Length > 1)
                //{
                //    // 算標準產量的參數
                //    strProdID = dsOqc.Tables[0].Rows[0]["ProdID"].ToString();
                //    // Get客戶名
                //    strCustomer = dsOqc.Tables[0].Rows[0]["PRODGROUP"].ToString();
                //}


                // Show customer logo
                // for Audit
                //if (strLine[0].ToString() == "4F-ASSY-L10")
                //{
                //    strCustomer = "BMW";
                //    imgLogo.ImageUrl = "getLogo.ashx?Customer=BMW";

                //}

                if (string.IsNullOrEmpty(strCustomer))
                {
                    switch (strLine[0].Substring(0, 2))
                    {
                        case "2F":
                            imgLogo.ImageUrl = "getLogo.ashx?Customer=GM";
                            break;
                        case "3F":
                            imgLogo.ImageUrl = "getLogo.ashx?Customer=DELPHI";
                            break;
                        //case "4F":
                        //    imgLogo.ImageUrl = "getLogo.ashx?Customer=BMW";
                        //    break;
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
                    //imgLogo.ImageUrl = "getLogo.ashx?Customer=" + strCustomer.Trim();
                    imgLogo.ImageUrl = "getLogo.ashx?Customer=RUN";
                }

                // Set  Title Text

                lblTitle.Text = string.Format(lblTitle.Text, strLine[current]);


                if (strProjectName != "")
                {
                    lblProduct.Text = strProjectName;
                }
                else if (strModTProjectName != "")
                {
                    lblProduct.Text = strModTProjectName;
                }
                else
                {
                    lblProduct.Text = strOQCProjectName;
                }
                //lblMoudleTest.Text = "Module Test: " + strModTProjectName;
                //lblOQC.Text = "OQC: " + strOQCProjectName;

                ds.Dispose();
                //dsModT.Dispose();
                //dsOqc.Dispose();

                // Show Chart
                InitiateChart(crtPerHour, "各站分時產出");
                AddSeries(crtPerHour, "Target", SeriesChartType.Column);

                dsActAndTarget = new DataSet();



                //for (int i = 0; i < 2; i++)
                for (int i = 0; i < 1; i++)
                {


                    // Set Target
                    DataSet dsTarget = GetTarget(Finishing_L[i].ToUpper(), Finishing_S[i].ToUpper(), strProdID); //整段時間的Target
                    if (dsTarget.Tables[0] != null && dsTarget.Tables[0].Rows.Count > 0)
                    {
                        switch (i)  //2019/6/29抓取良率目標
                        {
                            case 0:
                                fAssyYieldTarget = float.Parse(dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString().Replace('%', ' ').Trim());

                                pnlAssy.GroupingText = string.Format("Assy Yield Target {0}", dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString());
                                break;
                            //case 1:
                            //    fModuleTestYieldTarget = float.Parse(dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString().Replace('%', ' ').Trim());
                            //    pnlModuleTest.GroupingText = string.Format("Module Test Yield Target {0}", dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString());
                            //    break;
                            //case 2:
                            //    if (dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString() != "")
                            //    {
                            //        fOQCYieldTarget = float.Parse(dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString().Replace('%', ' ').Trim());
                            //    }
                            //    else
                            //    {
                            //        fOQCYieldTarget = 0;
                            //    }

                            //pnlOQC.GroupingText = string.Format("OQC Yield Target {0}", dsTarget.Tables[0].Rows[0]["YieldTarget"].ToString());
                            //break;
                            default:
                                break;
                        }
                    }


                    SetPerTarget(dsTarget); // set planPerTarget value
                    dsPlanPerTarget[i] = GetTargetData(planPerTarget);  //加上對應時間
                    // Chart圖上的Target只抓第一站(8350)
                    if (i == 0)
                    {
                        // create table ActAndTarget

                        DataTable tableA = new DataTable("ActAndTarget");
                        tableA.Columns.Add("Station");

                        for (int j = 0; j < dsPlanPerTarget[i].Tables[0].Rows.Count; j++)
                        {
                            if (dsPlanPerTarget[i].Tables[0].Rows[j] != null)
                            {
                                tableA.Columns.Add(dsPlanPerTarget[i].Tables[0].Rows[j]["Thour"].ToString());
                            }

                        }

                        dsActAndTarget.Tables.Add(tableA);
                        //
                        BindChart(crtPerHour, "各站分時產出", dsPlanPerTarget[i].Tables[0], "Target");
                    }
                    //依序加入Assy 8350 , Test 8353 , OQC 分時Actual資料 8360
                    AddSeries(crtPerHour, Finishing_A[i], SeriesChartType.Column);
                    DataSet dsAcHourData = GetHourData(Finishing_L[i], Finishing_S[i], Finishing_V[i]);
                    dsAcHourData = AdjustDatasetforShift(dsAcHourData);

                    BindChart(crtPerHour, "各站分時產出", dsAcHourData.Tables[0], Finishing_A[i]);

                    //  add a record to ActAndTarget
                    if (dsActAndTarget.Tables[0] != null)
                    {
                        DataRow dr;
                        dr = dsActAndTarget.Tables[0].NewRow();
                        dr.BeginEdit();
                        dr["Station"] = Finishing_S[i];
                        for (int j = 0; j < dsAcHourData.Tables[0].Rows.Count; j++)
                        {
                            for (int k = 0; k < dsPlanPerTarget[i].Tables[0].Rows.Count; k++)
                            {
                                if (dsAcHourData.Tables[0].Rows[j]["Thour"].ToString() == dsPlanPerTarget[i].Tables[0].Rows[k]["Thour"].ToString())
                                {
                                    //分時實際產出與目標產能對應
                                    dr[dsAcHourData.Tables[0].Rows[j]["Thour"].ToString()] = dsAcHourData.Tables[0].Rows[j]["Qty"].ToString() + @" / " + dsPlanPerTarget[i].Tables[0].Rows[k]["Qty"].ToString();
                                    break;
                                }
                                else if (dsPlanPerTarget[i].Tables[0].Rows.Count >= dsAcHourData.Tables[0].Rows.Count)
                                {
                                    dr[dsAcHourData.Tables[0].Rows[j]["Thour"].ToString()] = "0" + @" / " + dsPlanPerTarget[i].Tables[0].Rows[k]["Qty"].ToString();
                                }
                            }


                        }
                        dr.EndEdit();
                        dsActAndTarget.Tables[0].Rows.Add(dr);
                        dr.AcceptChanges();

                    }


                }
                gvActAndTarget.DataSource = dsActAndTarget;
                gvActAndTarget.DataBind();

                InitiateChart(crtAccumulate, "各站累計產出");
                AddSeries(crtAccumulate, "Target", SeriesChartType.Line);

                BindChart(crtAccumulate, "各站累計產出", GetTargetData(planAcTarget).Tables[0], "Target");
                //for (int i = 0; i < 2; i++)
                for (int i = 0; i < 1; i++)
                {
                    AddSeries(crtAccumulate, Finishing_A[i], SeriesChartType.Line);
                    DataSet dsAccuData = GetAccuData(Finishing_L[i], Finishing_S[i], Finishing_V[i]);
                    AdjustDatasetforShift(dsAccuData);
                    BindChart(crtAccumulate, "各站累計產出", dsAccuData.Tables[0], Finishing_A[i]);
                }
                gvPOL.DataSource = GetYieldData(Finishing_L[0], Finishing_S[0], Finishing_V[0]);
                gvPOL.DataBind();
                //gvCog.DataSource = GetYieldData(Finishing_L[1], Finishing_S[1], Finishing_V[1]);
                //gvCog.DataBind();
                //gvFOG.DataSource = GetYieldData(Finishing_L[2], Finishing_S[2], Finishing_V[2]);
                //gvFOG.DataBind();
                //lblupdateTime.Text = "Updated Time:" + DateTime.Now.ToLongTimeString();
            }
            catch (SqlException)
            {
                Response.Write(DateTime.Now.ToLongTimeString() + " Database is busy or cannot connect... , System will wait 30sec then retry!");
                // wait 30s
                Thread.Sleep(30000);
                // 重新載入
                Response.Redirect(Request.RawUrl);
                //lblupdateTime.Text = "Updated Time:" + DateTime.Now.ToLongTimeString();
            }
            catch (ArgumentException ex)
            {
                Response.Write(DateTime.Now.ToLongTimeString() + ex.Message + "System will wait 30sec then retry!");
                // wait 30s
                Thread.Sleep(30000);
                // 重新載入
                Response.Redirect(Request.RawUrl);
                //lblupdateTime.Text = "Updated Time:" + DateTime.Now.ToLongTimeString();
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

        private DataSet AdjustDatasetforShift(DataSet dsAcHourData)
        {
            //抓取當班開始結束時間
            int hbase, iFirstHour = 0, iLastHour = 0;
            //抓取當班有生產資料的第一個小時
            if (dsAcHourData.Tables[0] != null && dsAcHourData.Tables[0].Rows.Count > 0)
            {
                iFirstHour = int.Parse(dsAcHourData.Tables[0].Rows[0]["THour"].ToString());
                iLastHour = int.Parse(dsAcHourData.Tables[0].Rows[dsAcHourData.Tables[0].Rows.Count - 1]["THour"].ToString()); //當班目前有資料的最後一個小時
            }
            if (iFirstHour == 0)
            {
                return dsAcHourData;  //如果沒生產不需調整
            }
            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
            {

                hbase = 7;  //早班最早一個小時   
                DataSet dsResult = dsAcHourData.Clone();
                for (int j = hbase; j <= iLastHour; j++)
                {
                    DataRow dr;
                    dr = dsResult.Tables[0].NewRow();
                    dr.BeginEdit();
                    dr["THour"] = j.ToString();
                    dr["Qty"] = 0;
                    for (int k = 0; k < dsAcHourData.Tables[0].Rows.Count; k++)
                    {
                        if (j.ToString() == dsAcHourData.Tables[0].Rows[k]["THour"].ToString())
                        {
                            dr["Qty"] = dsAcHourData.Tables[0].Rows[k]["Qty"].ToString();
                        }
                    }
                    dr["TDate"] = dsAcHourData.Tables[0].Rows[0]["TDate"].ToString();
                    dr["Station"] = dsAcHourData.Tables[0].Rows[0]["Station"].ToString();
                    dr.EndEdit();
                    dsResult.Tables[0].Rows.InsertAt(dr, j - hbase);
                    dr.AcceptChanges();
                }
                dsAcHourData = dsResult.Copy();
                return dsAcHourData;
            }
            else
            {
                hbase = 19; //晚班最早一個小時
                if (iFirstHour >= 19)
                {
                    for (int j = hbase; j < iFirstHour; j++)
                    {
                        if (true)
                        {
                            DataRow dr;
                            dr = dsAcHourData.Tables[0].NewRow();
                            dr.BeginEdit();
                            dr["THour"] = j.ToString();
                            dr["Qty"] = 0;
                            dr["TDate"] = dsAcHourData.Tables[0].Rows[0]["TDate"].ToString();
                            dr["Station"] = dsAcHourData.Tables[0].Rows[0]["Station"].ToString();
                            dr.EndEdit();
                            dsAcHourData.Tables[0].Rows.InsertAt(dr, j - hbase);
                            dr.AcceptChanges();
                        }
                    }
                }
                else //已到隔天
                {
                    for (int j = hbase; j < 24; j++)  //先補到凌晨
                    {
                        if (true)
                        {
                            DataRow dr;
                            dr = dsAcHourData.Tables[0].NewRow();
                            dr.BeginEdit();
                            dr["THour"] = j.ToString();
                            dr["Qty"] = 0;
                            dr["TDate"] = dsAcHourData.Tables[0].Rows[0]["TDate"].ToString();
                            dr["Station"] = dsAcHourData.Tables[0].Rows[0]["Station"].ToString();
                            dr.EndEdit();
                            dsAcHourData.Tables[0].Rows.InsertAt(dr, j - hbase);
                            dr.AcceptChanges();
                        }
                    }
                    for (int j = 0; j < iFirstHour; j++)  //凌晨開始
                    {
                        if (true)
                        {
                            DataRow dr;
                            dr = dsAcHourData.Tables[0].NewRow();
                            dr.BeginEdit();
                            dr["THour"] = j.ToString();
                            dr["Qty"] = 0;
                            dr["TDate"] = dsAcHourData.Tables[0].Rows[0]["TDate"].ToString();
                            dr["Station"] = dsAcHourData.Tables[0].Rows[0]["Station"].ToString();
                            dr.EndEdit();
                            dsAcHourData.Tables[0].Rows.InsertAt(dr, j);
                            dr.AcceptChanges();
                        }
                    }
                    //
                }
                return dsAcHourData;
            }
        }

        private void SetPerTarget(DataSet dsTarget)
        {
            if (dsTarget.Tables[0] != null && dsTarget.Tables[0].Rows.Count > 0)
            {
                int iSTD = 0;
                int.TryParse(dsTarget.Tables[0].Rows[0]["STD"].ToString(), out iSTD);
                if (iSTD > 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //每小時應有產能
                        planPerTarget[i] = (int)Math.Round((double)iSTD / 10.83);
                    }
                    planPerTarget[0] = (int)Math.Round((double)iSTD / 10.83 * .83);
                    planPerTarget[3] = (int)Math.Round((double)iSTD / 10.83 * .33);
                    planPerTarget[8] = (int)Math.Round((double)iSTD / 10.83 * .66);
                    planAcTarget[0] = planPerTarget[0];
                    for (int i = 1; i < 12; i++)
                    {
                        planAcTarget[i] = planAcTarget[i - 1] + planPerTarget[i];
                    }
                }
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

        private static DataSet GetTarget(string strLine, string strStation, string strProdID)
        {

            DBConn conn = new DBConn("");
            string strsql = "select  STD, ProductID,YieldTarget " +
                              "from  [dbo].[MPIEM] MPIEM " +
                             "where [Line] = {0} " +
                             "  and [Station] = '{1}'" +
                             "  and ProductID = '{2}'";
            strsql = string.Format(strsql, strLine, strStation, strProdID);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql); ;
            return ds;
        }

        private static DataSet GetHourData(string strLine, string strStation, string Verb)
        {
            string BeginTime;
            string EndTime;
            GetTime(out BeginTime, out EndTime);



            DBConn conn = new DBConn("");
            string strsql = "SELECT  Sum([centitytqty]) Qty ,Convert(varchar(10),transdate,120) TDate,Datepart(hh,transdate) THour,max([CworkCTR]) Station  " +
                              "FROM  [dbo].[New_WLTX]  " +
                              " with (Nolock)" +
                             "WHERE transdate >= cast('{2}' as datetime)  " +
                             "  AND transdate < cast('{3}' as datetime)  " +
                             "  AND csublineid in ({0})" +
                             "  AND verb = '{4}'  " +
                             "  AND [CworkCTR] ='{1}'" +
                             "  AND TOLOTGrade = 'A' " +
                             "and centitytqty=1" +
                             "GROUP by  Convert(varchar(10),transdate,120) ,datepart(hh,transdate)" +
                             "ORDER by Convert(varchar(10),transdate,120)";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime, Verb);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql); ;
            return ds;
        }

        private static DataSet GetAccuData(string strLine, string strStation, string Verb)
        {
            string BeginTime;
            string EndTime;

            GetTime(out BeginTime, out EndTime);
            DBConn conn = new DBConn("");
            string strsql = "SELECT  Sum([centitytqty]) Qty ,Convert(varchar(10),transdate,120) TDate,Datepart(hh,transdate) THour,max([CworkCTR]) Station  " +
                              "FROM  [dbo].[New_WLTX]  " +
                              " with (Nolock)" +
                             "WHERE transdate >= cast('{2}' as datetime)  " +
                             "  AND transdate < cast('{3}' as datetime)  " +
                             "  AND csublineid in ({0})" +
                             "  AND verb = '{4}'  " +
                             "  AND [CworkCTR] ='{1}'" +
                             "  AND TOLOTGrade = 'A' " +
                             "and centitytqty=1" +
                             "GROUP by  Convert(varchar(10),transdate,120) ,datepart(hh,transdate)" +
                             "ORDER by Convert(varchar(10),transdate,120)";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime, Verb);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql); ;
            if (ds.Tables[0] != null)
            {
                for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i].BeginEdit();
                    ds.Tables[0].Rows[i]["Qty"] = int.Parse(ds.Tables[0].Rows[i - 1]["Qty"].ToString()) + int.Parse(ds.Tables[0].Rows[i]["Qty"].ToString());
                    ds.Tables[0].Rows[i].EndEdit();
                    ds.Tables[0].Rows[i].AcceptChanges();
                }
            }
            return ds;
        }

        private static DataSet GetYieldData(string strLine, string strStation, string Verb)
        {
            string BeginTime;
            string EndTime;

            GetTime(out BeginTime, out EndTime);
            DBConn conn = new DBConn("");
            string strsql = "SELECT  top 4 (Sum([centitytqty])+0.0)*100/ (select Sum([centitytqty]) " +
                            "  From  [dbo].[New_WLTX] TX " +
                            " WHERE transdate >= cast('{2}' as datetime)" +
                            "   AND transdate < cast('{3}' as datetime) " +
                            "   AND csublineid in ({0})   " +
                            "   AND  verb = '{4}'" +
                            "   AND  [CworkCTR] ='{1}'" +
                            " GROUP by [CworkCTR]) Yield ,Sum([centitytqty]) Qty" +
                            "        ,max([CworkCTR]) Station " +
                            "       ,TOLOTGRADE,TX.DefectCD ,CM.DefectDesc " +
                            " From (select centitytqty,transdate,csublineid,verb,CworkCTR,TOLOTGRADE,(CASE When TOLOTGRADE='A' THEN '' ELSE DefectCD END) DefectCD  from  [dbo].[New_WLTX]) TX " +
                            " left outer join  [dbo].[MDCM] CM ON  TX.DefectCD = CM.DefectCD" +
                            " where transdate >= cast('{2}' as datetime)" +
                               "and transdate < cast('{3}' as datetime)" +
                               "and  csublineid in ({0})" +
                               "and  verb = '{4}'" +
                               "and  [CworkCTR] ='{1}'" +
                               "and centitytqty=1" +
                               " group by  [CworkCTR],TOLOTGRADE,Tx.DefectCD,CM.DefectDesc" +
                               " order by yield desc";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime, Verb);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql); ;
            return ds;
        }

        private static DataSet GetTargetData(int[] planTarget)
        {
            //string BeginTime;
            //string EndTime;
            //GetTime(out BeginTime, out EndTime);
            int hoffset;
            int hbase;
            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
            {
                hbase = 7;
                hoffset = DateTime.Now.Hour - hbase + 1;
            }
            else
            {
                hbase = 19;
                if (DateTime.Now.Hour >= 19)
                {
                    hoffset = DateTime.Now.Hour - hbase + 1;
                }
                else
                {
                    hoffset = DateTime.Now.Hour + 6;
                }
                //hoffset
            }

            ////Hard Code for test begin
            //hbase = 19;
            //hoffset = 12;
            ////Hard Code for test end

            DataSet ds = new DataSet();

            DataTable table1 = new DataTable("STDCapa");
            table1.Columns.Add("Qty");
            table1.Columns.Add("THour");
            for (int i = 0; i < hoffset; i++)
            {
                int hour = hbase + i;
                if (hour >= 24)
                {
                    hour = hour - 24;
                }
                table1.Rows.Add(planTarget[i], hour);
            }
            ds.Tables.Add(table1);

            DataTable table2 = new DataTable("RealCapa");
            table2.Columns.Add("Qty");
            table2.Columns.Add("THour");
            for (int i = 0; i < hoffset; i++)
            {
                int hour = hbase + i;
                if (hour >= 24)
                {
                    hour = hour - 24;
                }
                if (DateTime.Now.Hour >= 7)  //本日7點到24點的邏輯
                {
                    if (hour > DateTime.Now.Hour)
                    {
                        table2.Rows.Add(0, hour);
                    }
                    else if (hour == DateTime.Now.Hour)
                    {
                        table2.Rows.Add((DateTime.Now.Minute * planTarget[i]) / 60, hour);
                    }
                    else
                    {
                        table2.Rows.Add(planTarget[i], hour);
                    }
                }
                else  //本日0點到7點的邏輯
                {
                    if (hour > DateTime.Now.Hour && hour < 7)
                    {
                        table2.Rows.Add(0, hour);
                    }
                    else if (hour == DateTime.Now.Hour)
                    {
                        table2.Rows.Add((DateTime.Now.Minute * planTarget[i]) / 60, hour);
                    }
                    else
                    {  // 實際時間為第二天凌晨，設定第一天晚上的目標產能
                        table2.Rows.Add(planTarget[i], hour);
                    }
                }

            }
            ds.Tables.Add(table2);
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
            //  for debug
            //BeginTime = "2019/7/02 07:00";
            //EndTime = "2019/7/02 19:00";
        }

        //chart初始化
        #region InitiateChart

        public void InitiateChart(System.Web.UI.DataVisualization.Charting.Chart Chart, string title)
        {
            Chart.ChartAreas.Clear();
            Chart.Series.Clear();
            Chart.Titles.Add(title);
            Chart.Titles[0].Text = title;
            Chart.Titles[0].Font = new Font("新細明體", 10, FontStyle.Bold);

            Chart.ChartAreas.Add(title);
            Chart.ChartAreas[title].AxisY.LineColor = Color.Black;
            Chart.ChartAreas[title].AxisY.LineDashStyle = ChartDashStyle.Solid;
            Chart.ChartAreas[title].AxisX.MinorGrid.Enabled = false;
            Chart.ChartAreas[title].AxisX.MajorGrid.Enabled = false;
            Chart.ChartAreas[title].AxisY.MajorGrid.Enabled = false;
            Chart.ChartAreas[title].AxisY.MinorGrid.Enabled = false;
            Chart.ChartAreas[title].AxisX.MinorTickMark.Enabled = false;
            Chart.ChartAreas[title].AxisY.MinorTickMark.Enabled = false;
            Chart.ChartAreas[title].AxisX.MajorTickMark.LineDashStyle = ChartDashStyle.Solid;
            Chart.ChartAreas[title].AxisY.Minimum = 0;


            Chart.Legends.Add(new System.Web.UI.DataVisualization.Charting.Legend());



            Chart.ChartAreas[title].Position.Auto = false;
            Chart.ChartAreas[title].Position.X = 10;
            Chart.ChartAreas[title].Position.Y = 10;
            Chart.ChartAreas[title].Position.Width = 85;
            Chart.ChartAreas[title].Position.Height = 75;
            Chart.ChartAreas[title].InnerPlotPosition.X = 5;
            Chart.ChartAreas[title].InnerPlotPosition.Y = 5;
            Chart.ChartAreas[title].InnerPlotPosition.Width = 85;
            Chart.ChartAreas[title].InnerPlotPosition.Height = 85;

            Chart.BorderSkin.BackColor = Color.FromArgb(175, 250, 250);
            Chart.BorderSkin.SkinStyle = BorderSkinStyle.FrameThin1;
            Chart.BorderlineDashStyle = ChartDashStyle.Dash;
            Chart.BorderSkin.BorderColor = Color.Green;
            Chart.BorderWidth = 10;
            Chart.BorderlineColor = System.Drawing.Color.Gray;
            Chart.BackColor = Color.FromArgb(235, 250, 235);//(220, 250, 230);//(230, 250, 240);(248, 237, 222);
            Chart.ChartAreas[title].BackColor = System.Drawing.Color.FromArgb(235, 250, 235);//(255, 243, 220);//(235, 250, 235);//(248, 237, 222);
            Chart.BackSecondaryColor = System.Drawing.Color.FromArgb(255, 255, 255);
            Chart.ChartAreas[title].BackSecondaryColor = System.Drawing.Color.FromArgb(255, 255, 255);
            Chart.BackGradientStyle = GradientStyle.HorizontalCenter;
            Chart.ChartAreas[title].BackGradientStyle = GradientStyle.HorizontalCenter;
            Chart.ChartAreas[title].BorderDashStyle = ChartDashStyle.NotSet;
            Chart.ChartAreas[title].BorderWidth = 0;
            Chart.BackImageWrapMode = ChartImageWrapMode.TileFlipX;
            Chart.BackImageAlignment = ChartImageAlignmentStyle.BottomLeft;
            Chart.Legends[0].Alignment = StringAlignment.Center;
            Chart.Legends[0].LegendStyle = LegendStyle.Table;
            Chart.Legends[0].Docking = Docking.Bottom;
            Chart.Legends[0].TableStyle = LegendTableStyle.Wide;
            Chart.Legends[0].Position.Auto = false;
            //Chart.Legends[0].Position.X = 40;
            //Chart.Legends[0].Position.Y = 90;
            //Chart.Legends[0].Position.Height = 5;
            //Chart.Legends[0].Position.Width = 50;
            //Chart.Legends[0].IsTextAutoFit = false;
            //Chart.Legends[0].Font = new Font("Arial", 5, FontStyle.Bold);
            Chart.Legends[0].Position.X = 83;
            Chart.Legends[0].Position.Y = 25;
            Chart.Legends[0].Position.Height = 30;
            Chart.Legends[0].Position.Width = 13;
            Chart.Legends[0].IsTextAutoFit = true;
            Chart.Legends[0].Font = new Font("Arial", 6, FontStyle.Bold);


            Chart.Legends[0].BorderColor = Color.Green;
            Chart.Legends[0].BorderDashStyle = ChartDashStyle.DashDot;
            Chart.Legends[0].BorderWidth = 1;
            Chart.Legends[0].IsDockedInsideChartArea = false;
            Chart.Legends[0].BackColor = System.Drawing.Color.FromArgb(255, 240, 255);
            Chart.Legends[0].BackGradientStyle = GradientStyle.Center;
            Chart.Legends[0].BackSecondaryColor = System.Drawing.Color.FromArgb(250, 255, 255);



            Chart.ChartAreas[title].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            Chart.ChartAreas[title].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            Chart.ChartAreas[title].AxisX.LabelStyle.IsEndLabelVisible = true;
            Chart.ChartAreas[title].AxisY.LabelStyle.IsEndLabelVisible = true;
            //Chart.ChartAreas[title].AxisX.LabelStyle.Format = "yyyy/MM";
            Chart.ChartAreas[title].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;
            Chart.ChartAreas[title].AxisX.LabelStyle.IsStaggered = true;
            Chart.ChartAreas[title].AxisX.IsLabelAutoFit = true;

            Chart.ChartAreas[title].AxisY.IsLabelAutoFit = true;
            Chart.ChartAreas[title].AxisX.IsStartedFromZero = false;
            Chart.ChartAreas[title].AxisY.IsStartedFromZero = false;

        }

        private static void AddSeries(System.Web.UI.DataVisualization.Charting.Chart Chart, string strSeries, SeriesChartType CType)
        {
            Chart.Series.Add(strSeries);
            Chart.Series[strSeries].ChartType = CType;
            Chart.Series[strSeries].BorderWidth = 1;
            Chart.Series[strSeries].IsVisibleInLegend = true;
            Chart.Series[strSeries].BackImageWrapMode = ChartImageWrapMode.Scaled;
            Chart.Series[strSeries].IsValueShownAsLabel = true;
            Chart.Series[strSeries].IsVisibleInLegend = true;
            Chart.Series[strSeries].XValueType = ChartValueType.Time;
            Chart.Series[strSeries].BorderColor = Color.FromArgb(0, 100, 200);
            Chart.Series[strSeries].IsVisibleInLegend = true;
            Chart.Series[strSeries].MarkerSize = 4;
            Chart.Series[strSeries].BorderWidth = 1;

            Chart.Series[strSeries].MarkerStyle = MarkerStyle.Diamond;
            Chart.Series[strSeries].MarkerBorderWidth = 0;
            Chart.Series[strSeries].EmptyPointStyle.BorderDashStyle = ChartDashStyle.NotSet;
        }

        public void BindChart(System.Web.UI.DataVisualization.Charting.Chart InChart, string title, DataTable dt, string seriesName)
        {

            foreach (DataRow row in dt.Rows)
            {
                double YVal = Double.Parse(row["Qty"].ToString());
                int XVal = int.Parse(row["THour"].ToString());
                string strXVal = XVal.ToString() + "-" + (XVal + 1).ToString();

                InChart.Series[seriesName].Points.AddXY(strXVal, YVal);
                //InChart.Series[seriesName].ToolTip = "百分比 \t： #VALY \n日    期 \t： #AXISLABEL";
                //InChart.Series[seriesName].PostBackValue = "#AXISLABEL;#INDEX";




                InChart.ChartAreas[title].AxisY.Title = "QTY";
                InChart.ChartAreas[title].AxisY.TextOrientation = TextOrientation.Auto;
                InChart.ChartAreas[title].AxisX.Title = "Hour";
                InChart.ChartAreas[title].AxisX.TextOrientation = TextOrientation.Auto;

            }
        }

        #endregion

        protected void TimerPage_Tick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
            //lblupdateTime.Text = "Updated Time:" + DateTime.Now.ToLongTimeString();
        }

        protected void gvActAndTarget_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (e.Row.Cells[0] != null)
                {
                    e.Row.Cells[0].Text = "站點/時間";
                    e.Row.Cells[0].Width = Unit.Pixel(75);
                }

                for (int i = 1; i < dsActAndTarget.Tables[0].Columns.Count; i++)
                {
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text + ":00" + "-" + (int.Parse(e.Row.Cells[i].Text) + 1).ToString() + ":00";
                }
                //添加第二行表头
                int columncount = dsActAndTarget.Tables[0].Columns.Count;
                e.Row.Cells[columncount - 1].Text = e.Row.Cells[columncount - 1].Text + "</tr><tr>";  //表头换行
                e.Row.Cells.Add(new TableHeaderCell());
                e.Row.Cells[columncount].Attributes.Add("bgcolor", "DarkSeaGreen");
                e.Row.Cells[columncount].Attributes.Add("rowspan", "1");
                e.Row.Cells[columncount].Text = "OUTPUT";
                for (int i = columncount + 1; i < columncount * 2; i++)
                {
                    e.Row.Cells.Add(new TableHeaderCell());
                    e.Row.Cells[i].Attributes.Add("style", "word-break :break-all ; word-wrap:break-word");
                    e.Row.Cells[i].Text = "ACT./Plan";
                    e.Row.Cells[i].Attributes.Add("bgcolor", "DarkSeaGreen");
                    //e.Row.Cells[i].Attributes.CssStyle.Add("font-size", "6px");
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0] != null)
                {
                    switch (e.Row.Cells[0].Text)
                    {
                        case "8350":
                            e.Row.Cells[0].Text = "Assy";
                            break;
                        //case "8353":
                        //    e.Row.Cells[0].Text = "Module Test";
                        //    break;
                        //case "8360":
                        //    e.Row.Cells[0].Text = "OQC";
                        //    break;
                        default:
                            break;
                    }
                }
                for (int i = 1; i < dsActAndTarget.Tables[0].Columns.Count; i++)
                {
                    string[] iData = e.Row.Cells[i].Text.Split('/');
                    try
                    {
                        int iAct = int.Parse(iData[0]);
                        int iTar = 0;
                        //iTar = int.Parse(iData[1]);
                        switch (e.Row.Cells[0].Text)
                        {
                            case "Assy":
                                iTar = int.Parse(dsPlanPerTarget[0].Tables[1].Rows[i - 1]["Qty"].ToString());
                                break;
                            //case "Module Test":
                            //    iTar = int.Parse(dsPlanPerTarget[1].Tables[1].Rows[i - 1]["Qty"].ToString());
                            //    break;
                            //case "OQC":
                            //    iTar = int.Parse(dsPlanPerTarget[2].Tables[1].Rows[i - 1]["Qty"].ToString());
                            //    break;
                            default:
                                break;
                        }
                        if (iAct >= iTar)
                        {
                            e.Row.Cells[i].BackColor = Color.Green;
                        }
                        else if (iAct >= iTar * .9)
                        {
                            e.Row.Cells[i].BackColor = Color.Yellow;
                        }
                        else
                        {
                            if (iAct != 0)
                            {
                                e.Row.Cells[i].BackColor = Color.Red;
                            }
                            else
                            {
                                e.Row.Cells[i].Text = ""; ;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        protected void gvPOL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0] != null && e.Row.Cells[1] != null && e.Row.Cells[2] != null)
                {
                    if (e.Row.Cells[2].Text == "&nbsp;")  //表示良率的資料列 
                    {
                        if (fAssyYieldTarget > float.Parse(e.Row.Cells[0].Text))
                        {
                            e.Row.Cells[0].BackColor = Color.Red;
                        }

                    }
                }

            }
        }

        protected void gvCog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0] != null && e.Row.Cells[1] != null && e.Row.Cells[2] != null)
                {
                    if (e.Row.Cells[2].Text == "&nbsp;")  //表示良率的資料列 
                    {
                        if (fModuleTestYieldTarget > float.Parse(e.Row.Cells[0].Text))
                        {
                            e.Row.Cells[0].BackColor = Color.Red;
                        }

                    }
                }

            }
        }

        protected void gvFOG_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0] != null && e.Row.Cells[1] != null && e.Row.Cells[2] != null)
                {
                    if (e.Row.Cells[2].Text == "&nbsp;")  //表示良率的資料列 
                    {
                        if (fOQCYieldTarget > float.Parse(e.Row.Cells[0].Text))
                        {
                            e.Row.Cells[0].BackColor = Color.Red;
                        }

                    }
                }

            }
        }










    }
}