using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Report.Library;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Data.SqlClient;

namespace WebReport
{
    public partial class OnLineReport2FAssy01 : System.Web.UI.Page
    {
        string[] Finishing_A = { "Assy", "Module Test", "OQC" };
        string[] Finishing_L = { "'2F-ASSY-L01'", "'2F-ASSY-L01'", "'2F-ASSY-L01'" };
        string[] Finishing_S = { "8350", "8353", "8360" };
        string[] Finishing_V = { "MVOU", "MVOU", "MVOU", "MVOU" };
        int[] planAcTarget = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] planPerTarget = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strProjectName, strProdID;
                // Setup Title & Get ProjectName
                lblTitle.Text = "2F-Assy-L01({0})";
                DataSet ds = GetProjectNameInWIP(Finishing_L[0], Finishing_S[0]);
                strProjectName = "";
                strProdID = "";
                if (ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strProjectName = strProjectName + ds.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                        if (i == 0)
                        {
                            strProdID = ds.Tables[0].Rows[i]["ProdID"].ToString();
                        }
                    }

                }
                lblTitle.Text = string.Format(lblTitle.Text, strProjectName);
                ds.Dispose();
                // Set Target
                DataSet dsTarget = GetTarget(Finishing_L[0].ToUpper(), Finishing_S[0].ToUpper(), strProdID);
                if (dsTarget.Tables[0] != null && dsTarget.Tables[0].Rows.Count > 0)
                {
                    int iSTD = 0;
                    int.TryParse(dsTarget.Tables[0].Rows[0]["STD"].ToString(), out iSTD);
                    if (iSTD > 0)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            planPerTarget[i] = (int)Math.Round((double)iSTD / 11);
                        }
                        planPerTarget[3] = (int)Math.Round((double)iSTD / 11 * .33);
                        planPerTarget[8] = (int)Math.Round((double)iSTD / 11 * .66);
                        planAcTarget[0] = planPerTarget[0];
                        for (int i = 1; i < 12; i++)
                        {
                            planAcTarget[i] = planAcTarget[i - 1] + planPerTarget[i];
                        }
                    }
                }
                // Show Chart
                InitiateChart(crtPerHour, "各站分時產出");
                AddSeries(crtPerHour, "Target", SeriesChartType.Column);
                BindChart(crtPerHour, "各站分時產出", GetTargetData(planPerTarget).Tables[0], "Target");
                for (int i = 0; i < 3; i++)
                {
                    AddSeries(crtPerHour, Finishing_A[i], SeriesChartType.Column);
                    BindChart(crtPerHour, "各站分時產出", GetHourData(Finishing_L[i], Finishing_S[i], Finishing_V[i]).Tables[0], Finishing_A[i]);
                }

                InitiateChart(crtAccumulate, "各站累計產出");
                AddSeries(crtAccumulate, "Target", SeriesChartType.Line);
                BindChart(crtAccumulate, "各站累計產出", GetTargetData(planAcTarget).Tables[0], "Target");
                for (int i = 0; i < 3; i++)
                {
                    AddSeries(crtAccumulate, Finishing_A[i], SeriesChartType.Line);
                    BindChart(crtAccumulate, "各站累計產出", GetAccuData(Finishing_L[i], Finishing_S[i], Finishing_V[i]).Tables[0], Finishing_A[i]);
                }
                gvPOL.DataSource = GetYieldData(Finishing_L[0], Finishing_S[0], Finishing_V[0]);
                gvPOL.DataBind();
                gvCog.DataSource = GetYieldData(Finishing_L[1], Finishing_S[1], Finishing_V[1]);
                gvCog.DataBind();
                gvFOG.DataSource = GetYieldData(Finishing_L[2], Finishing_S[2], Finishing_V[2]);
                gvFOG.DataBind();
                lblupdateTime.Text = "Updated Time:" + DateTime.Now.ToLongTimeString();
                imgAssyLed.ImageUrl = imgAssyLed.ImageUrl + "&time=" + DateTime.Now.ToString();
                imgAssyMain.ImageUrl = imgAssyMain.ImageUrl + "&time=" + DateTime.Now.ToString();
                imgAssyOQC.ImageUrl = imgAssyOQC.ImageUrl + "&time=" + DateTime.Now.ToString();
            }
            catch (SqlException)
            {
                lblTitle.Text = "Database is busy or cannot connect... , System will wait 2 minutes then retry!(" + DateTime.Now.ToLongTimeString() + ")";
            }
            
            

        }

        private static DataSet GetProjectNameInWIP(string strLine, string strStation)
        {
            string BeginTime;
            string EndTime;
             
            GetTime(out BeginTime, out EndTime);          

            DBConn conn = new DBConn("");
            string strsql = "select TOPRODID,PROJECT_NAME,ProdID " +
                              "from (select  TOPRODID From  [dbo].[New_WLTX] TX ,MPIM " +
                                                    "where transdate >= cast('{2}' as datetime)" +
                                                    "  and transdate < cast('{3}' as datetime)" +
                                                    "  and  csublineid in ({0})" +
                                                    "  and  verb = 'MVOU'" +
                                                    "  and  [CworkCTR] ='{1}'" +
                                                    " group by TOPRODID ) A , MPIM" +
                               " WHERE MPIM.ProdID = A.TOPRODID";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql);;
            return ds;
        }

        private static DataSet GetTarget(string strLine, string strStation, string strProdID)
        {
             
            DBConn conn = new DBConn("");
            string strsql = "select  STD, ProductID " +
                              "from  [dbo].[MPIEM] MPIEM " +
                             "where [Line] = {0} " +
                             "  and [Station] = '{1}'" +
                             "  and ProductID = '{2}'";
            strsql = string.Format(strsql, strLine, strStation, strProdID);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql);;
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
                             "WHERE transdate >= cast('{2}' as datetime)  " +
                             "  AND transdate < cast('{3}' as datetime)  " +
                             "  AND csublineid in ({0})" +
                             "  AND verb = '{4}'  " +
                             "  AND [CworkCTR] ='{1}'" +
                             "  AND TOLOTGrade = 'A' " +
                             "GROUP by  Convert(varchar(10),transdate,120) ,datepart(hh,transdate)" +
                             "ORDER by Convert(varchar(10),transdate,120)";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime, Verb);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql);;
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
                             "WHERE transdate >= cast('{2}' as datetime)  " +
                             "  AND transdate < cast('{3}' as datetime)  " +
                             "  AND csublineid in ({0})" +
                             "  AND verb = '{4}'  " +
                             "  AND [CworkCTR] ='{1}'" +
                             "  AND TOLOTGrade = 'A' " +
                             "GROUP by  Convert(varchar(10),transdate,120) ,datepart(hh,transdate)" +
                             "ORDER by Convert(varchar(10),transdate,120)";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime, Verb);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql);;
            if (ds.Tables[0]!=null)
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

        private static DataSet GetYieldData(string strLine, string strStation,string Verb)
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
                               " group by  [CworkCTR],TOLOTGRADE,Tx.DefectCD,CM.DefectDesc" +
                               " order by yield desc";
            strsql = string.Format(strsql, strLine, strStation, BeginTime, EndTime, Verb);
            DataSet ds = new DataSet();
            ds = conn.GetDataSetBySql(strsql);;           
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
            DataSet ds = new DataSet();

            DataTable table1 = new DataTable("STDCapa");
            table1.Columns.Add("Qty");
            table1.Columns.Add("THour");
            for (int i = 0; i < hoffset; i++)
            {
                int hour = hbase + i;
                if (hour > 24)
                {
                    hour = hour - 24;
                }
                table1.Rows.Add(planTarget[i], hour);
            }
            ds.Tables.Add(table1);

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
            lblupdateTime.Text = "Updated Time:" + DateTime.Now.ToLongTimeString();
        }
    }
}