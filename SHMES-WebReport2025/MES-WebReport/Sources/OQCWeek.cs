using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Report.Library;
using System.Collections.Generic;

namespace Report.Sources
{
    public class OQCWeek
    {
        private string strGroup = "";
        private string strYear = "";
        private string strMonth = "";
        private string strWeek = "";
        private int intWeek;
        private DBConn dbConn;
        private string strColumnsName = "";
        private string strFabid = "";

        #region Public Property
        /// <summary>
        /// Week
        /// </summary>
        public string Week
        {
            get
            {
                return strWeek;
            }
            set
            {
                this.strWeek = value;
            }
        }

        /// <summary>
        /// Year
        /// </summary>
        public string Year
        {
            get
            {
                return strYear;
            }
            set
            {
                this.strYear = value;
            }
        }

        /// <summary>
        /// Fabid
        /// </summary>
        public string Fabid
        {
            get
            {
                return this.strFabid;
            }
            set
            {
                this.strFabid = value;
            }
        }

        /// <summary>
        /// Group
        /// </summary>
        public string Group
        {
            get
            {
                return this.strGroup;
            }
            set
            {
                this.strGroup = value;
            }
        }

        /// <summary>
        /// ColumnsName
        /// </summary>
        public string ColumnsName
        {
            get
            {
                return this.strColumnsName;
            }
            set
            {
                this.strColumnsName = value;
            }
        }

        public OQCWeek()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Public Methods
        public DataSet Read()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int intIndex = 0;
            int intNum = 0;
            DataTable dtTmp = new DataTable();
            string[] arrStr ;
            try
            {
                dt = this.getOQCWeek().Copy();
                arrStr = this.ColumnsName.Split(',');
                dtTmp = dt.Clone();
                for (int i = 0; i < 8; i++)
                {
                    DataRow dr = dtTmp.NewRow();
                    dtTmp.Rows.InsertAt(this.createNewRow(dtTmp), 0);
                }

                    #region 補Month資料
                for (int i = 0; i < 4; i++)
                {
                    for(int j =0; j < dt.Rows.Count; j++)
                    {
                        if (Int32.Parse( arrStr[i].Substring(5, 2) )== Int32.Parse( dt.Rows[j]["REPORTMONTH"].ToString()))
                        {
                            dtTmp.Rows[i]["REPORTYEAR"] = dt.Rows[j]["REPORTYEAR"].ToString();
                            dtTmp.Rows[i]["REPORTMONTH"] = dt.Rows[j]["REPORTMONTH"].ToString();
                            dtTmp.Rows[i]["REPORTWEEK"] = dt.Rows[j]["REPORTWEEK"].ToString();
                            dtTmp.Rows[i]["DPPMTOTAL"] = dt.Rows[j]["DPPMTOTAL"].ToString();
                            dtTmp.Rows[i]["DPPMTOTALTEST"] = dt.Rows[j]["DPPMTOTALTEST"].ToString();
                            dtTmp.Rows[i]["DPPM"] = dt.Rows[j]["DPPM"].ToString();
                            dtTmp.Rows[i]["DFQTY"] = dt.Rows[j]["DFQTY"].ToString();
                            dtTmp.Rows[i]["LRRTOTAL"] = dt.Rows[j]["LRRTOTAL"].ToString();
                            dtTmp.Rows[i]["LRRDFLOT"] = dt.Rows[j]["LRRDFLOT"].ToString();
                            dtTmp.Rows[i]["LRR"] = dt.Rows[j]["LRR"].ToString();
                        }
                    }
                }
                    //for (intNum = 0; intNum < dt.Rows.Count; intNum++)
                    //{
                    //    if (dt.Rows[intNum]["REPORTWEEK"].ToString() != "0")
                    //    {
                    //        break;
                    //    }
                    //}
                    //intIndex = 4 - intNum;

                    //for (int i = 0; i < intIndex; i++ )
                    //{
                    //    for (int j = 0; j < dt.Rows.Count; j++)
                    //    {
                    //        if ((Int32.Parse(returnMonthDay()[0]) + i) == Int32.Parse( dt.Rows[j]["REPORTMONTH"].ToString()))
                    //        {
                    //            dt.Rows.InsertAt(this.createNewRow(dt), i);
                    //        }
                    //    }
                    //}

                    #endregion

                    #region 補Week資料
                for (int i = 4; i < 8; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (Int32.Parse(arrStr[i].Substring(6, 2)) == Int32.Parse(dt.Rows[j]["REPORTWEEK"].ToString()))
                        {
                            dtTmp.Rows[i]["REPORTYEAR"] = dt.Rows[j]["REPORTYEAR"].ToString();
                            dtTmp.Rows[i]["REPORTMONTH"] = dt.Rows[j]["REPORTMONTH"].ToString();
                            dtTmp.Rows[i]["REPORTWEEK"] = dt.Rows[j]["REPORTWEEK"].ToString();
                            dtTmp.Rows[i]["DPPMTOTAL"] = dt.Rows[j]["DPPMTOTAL"].ToString();
                            dtTmp.Rows[i]["DPPMTOTALTEST"] = dt.Rows[j]["DPPMTOTALTEST"].ToString();
                            dtTmp.Rows[i]["DPPM"] = dt.Rows[j]["DPPM"].ToString();
                            dtTmp.Rows[i]["DFQTY"] = dt.Rows[j]["DFQTY"].ToString();
                            dtTmp.Rows[i]["LRRTOTAL"] = dt.Rows[j]["LRRTOTAL"].ToString();
                            dtTmp.Rows[i]["LRRDFLOT"] = dt.Rows[j]["LRRDFLOT"].ToString();
                            dtTmp.Rows[i]["LRR"] = dt.Rows[j]["LRR"].ToString();
                        }
                    }
                }

                    #endregion
                    ds.Tables.Add(dtTmp);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        /// <summary>
        /// 取得當天在全年中的星期
        /// </summary>
        /// <returns></returns>
        public int getWeek(DateTime dte)
        {
            int intWeek = 0;

            CultureInfo ci = CultureInfo.CurrentCulture;
            System.Globalization.Calendar cl = ci.Calendar;
            System.Globalization.CalendarWeekRule cr = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek dw = ci.DateTimeFormat.FirstDayOfWeek;
            intWeek = Int32.Parse(cl.GetWeekOfYear(dte, cr, dw).ToString());

            return intWeek;
        }

        /// <summary>
        /// 取得所選Group得PLANDPPM,PLANLRR
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoal()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            try
            {
                strSql = @" select (cast( (PLANDPPM) as NUMERIC)  ) as PLANDPPM,
                                (cast( (PLANLRR * 100) as varchar(100)) + '%' ) as PLANLRR from MPGTM " +
                                   " WHERE PRODGROUP = '" + this.Group + "' ";
                dt = this.dbConn.GetDataSetBySql(strSql, "MPGTM").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// 取得Roqcd資料
        /// </summary>
        /// <returns></returns>
        public DataTable getDTRoqcd()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            try
            {
                //strSql = " SELECT OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' ORDER BY OQCNGQTY DESC ";
                strSql = " SELECT DEFECTCD,SUM(OQCNGQTY) AS OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' AND REPORTWEEK=" + strWeek + " GROUP BY DEFECTCD ORDER BY SUM(OQCNGQTY) DESC ";
                dt = this.dbConn.GetDataSetBySql(strSql, "ROQCD").Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        #endregion

        /// <summary>
        /// 取得Roqcd資料
        /// </summary>
        /// <returns></returns>
        public List<int> getRoqcd()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            List<int> arrList = new List<int>();
            int intCount = 0;

            try
            {
                //strSql = " SELECT OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' ORDER BY OQCNGQTY DESC ";
                strSql = " SELECT DEFECTCD,SUM(OQCNGQTY) AS OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' AND REPORTWEEK=" + strWeek + " GROUP BY DEFECTCD ORDER BY SUM(OQCNGQTY) DESC ";
                dt = this.dbConn.GetDataSetBySql(strSql, "ROQCD").Tables[0];
                intCount = dt.Rows.Count;
                if (dt.Rows.Count < 11)
                {
                    for (int i = 0; i < 11 - intCount; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["OQCNGQTY"] = 0;
                        dt.Rows.Add(dr);
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 10)
                    {
                        arrList[10] = arrList[10] + Int32.Parse(dt.Rows[i]["OQCNGQTY"].ToString());
                    }
                    else
                    {
                        arrList.Add(Int32.Parse(dt.Rows[i]["OQCNGQTY"].ToString()));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrList;
        }
        


        #region Private Methods

        private DataTable getOQCWeek()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            string strMonthN = "";
            string strWeekN = "";

            try
            {
                strWeekN = returnWeekDay()[0];
                strMonthN = returnMonthDay()[0];
                this.strColumnsName = (DateTime.Parse(strMonthN) + TimeSpan.FromDays(0)).ToString("yyyy-MM-dd HH:mm:ss.000").Substring(0, 7);
                for (int i = 1; i < 4; i++)
                {
                    this.strColumnsName += "," + (DateTime.Parse(strMonthN) + TimeSpan.FromDays(31*i)).ToString("yyyy-MM-dd HH:mm:ss.000").Substring(0, 7);
                }
                
                for (int i = 0; i < 4; i++)
                {
                    this.strColumnsName += "," + (DateTime.Parse(strWeekN) + TimeSpan.FromDays(7 * i)).ToString("yyyy-MM-dd HH:mm:ss.000").Substring(0, 4) + "-" + "W" + this.getWeek(DateTime.Parse(strWeekN) + TimeSpan.FromDays(7 * i)).ToString();
                }

                    strSql = @"SELECT  REPORTYEAR,REPORTMONTH,'0' AS REPORTWEEK, DPPMTOTAL,DPPMTOTALTEST,
                        CASE WHEN DPPMTOTALTEST= 0 THEN 0 ELSE ((CAST(DPPMTOTAL AS  FLOAT)/CAST( DPPMTOTALTEST AS FLOAT ))*1000000) END AS DPPM,
                         DFQTY,LRRTOTAL,LRRDFLOT
                        ,CASE WHEN LRRTOTAL = 0 THEN '0' ELSE CAST( (CAST(LRRDFLOT AS FLOAT) /CAST(LRRTOTAL AS FLOAT)) * 100   AS VARCHAR(100)) + '%' END AS LRR 
                        FROM
                        (
                        SELECT REPORTYEAR,REPORTMONTH, SUM(AOUTQTY)AS DPPMTOTAL,SUM(QTQTY)AS DPPMTOTALTEST,SUM(NGQTY)AS DFQTY
                         ,SUM(QTCNT)AS LRRTOTAL,SUM(NGCNT)AS LRRDFLOT " +
                            " FROM ROQCS R WHERE R.FABID = '" + this.strFabid + "' AND  R.PRODGROUP = '" + this.Group + "' " + "AND R.REPORTDATE >= '" + this.returnMonthDay()[0] + "' AND R.REPORTDATE<'" + this.returnMonthDay()[1] + "'  " +
                            @" GROUP BY R.REPORTYEAR,R.REPORTMONTH ) AS A  
                        UNION
                        SELECT REPORTYEAR, '0' AS REPORTMONTH,REPORTWEEK,DPPMTOTAL,DPPMTOTALTEST,
                        CASE WHEN DPPMTOTALTEST= 0 THEN 0 ELSE ((CAST(DPPMTOTAL AS  FLOAT)/CAST( DPPMTOTALTEST AS FLOAT ))*1000000) END AS DPPM,
                         DFQTY,LRRTOTAL,LRRDFLOT
                        ,CASE WHEN LRRTOTAL = 0 THEN '0' ELSE CAST( (CAST(LRRDFLOT AS FLOAT) /CAST(LRRTOTAL AS FLOAT)) * 100   AS VARCHAR(100)) + '%' END AS LRR 
                        FROM
                        (SELECT REPORTYEAR,REPORTWEEK,SUM(AOUTQTY)AS DPPMTOTAL,SUM(QTQTY)AS DPPMTOTALTEST,SUM(NGQTY)AS DFQTY
                         ,SUM(QTCNT)AS LRRTOTAL,SUM(NGCNT)AS LRRDFLOT " +
                            " FROM ROQCS R WHERE R.FABID = '" + this.strFabid + "'  AND R.PRODGROUP = '" + this.Group + "' " + "AND R.REPORTDATE BETWEEN '" + this.returnWeekDay()[0] + "' AND '" + this.returnWeekDay()[1] + "' " +
                            @" GROUP BY R.REPORTYEAR,R.REPORTWEEK)AS B 
                        ORDER BY REPORTYEAR,REPORTMONTH,REPORTWEEK";
                dt = this.dbConn.GetDataSetBySql(strSql, "ROQCS").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        private string GetColumnsName(string year, string month, string week)
        {
            string[] arrStrYear = year.Split(',');
            string[] arrStrMonth = month.Split(',');
            string[] arrStrWeek = week.Split(',');
            if (arrStrYear.Length > 1)
            {

            }
            return this.ColumnsName;
        }

        private DataRow createNewRow(DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["REPORTYEAR"] = "0";
            dr["REPORTMONTH"] = "0";
            dr["REPORTWEEK"] = "0";
            dr["DPPMTOTAL"] = "0";
            dr["DPPMTOTALTEST"] = "0";
            dr["DPPM"] = "0";
            dr["DFQTY"] = "0";
            dr["LRRTOTAL"] = "0";
            dr["LRRDFLOT"] = "0";
            dr["LRR"] = "0";
            return dr;
        }

        private double getTheDayOfWeek(DateTime dte)
        {
            double douTmp = 0;
            switch (dte.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    douTmp = 1;
                    break;
                case DayOfWeek.Tuesday:
                    douTmp = 2;
                    break;
                case DayOfWeek.Wednesday:
                    douTmp = 3;
                    break;
                case DayOfWeek.Thursday:
                    douTmp = 4;
                    break;
                case DayOfWeek.Friday:
                    douTmp = 5;
                    break;
                case DayOfWeek.Saturday:
                    douTmp = 6;
                    break;
                case DayOfWeek.Sunday:
                    douTmp = 0;
                    break;
            }
            
            return douTmp;
        }

        private string[] returnMonthDay()
        {
            string[] arrStr = new string[2];
            string strBeginDay = "";
            string strEndDay = "";

            strEndDay = getChooseDay().Substring(0, 7) + "-01 00:00:00.000";
            strBeginDay = (DateTime.Parse(strEndDay) - TimeSpan.FromDays(118)).ToString("yyyy-MM-dd HH:mm:ss.000").Substring(0, 7) + "-01 00:00:00.000";
            arrStr[0] = strBeginDay;
            arrStr[1] = strEndDay;
            return arrStr;
        }

        private string[] returnWeekDay()
        {
            string[] arrStr = new string[2];
            string strBeginDay = "";
            string strEndDay = "";

            strEndDay = (DateTime.Parse( this.getChooseDay()) + TimeSpan.FromDays(6-this.getTheDayOfWeek(DateTime.Parse( this.getChooseDay())))).ToString("yyyy-MM-dd HH:mm:ss.000").Substring(0,10)+" 00:00:00.000";
            strBeginDay = (DateTime.Parse(strEndDay) - TimeSpan.FromDays(27)).ToString("yyyy-MM-dd HH:mm:ss.000").Substring(0, 10) + " 00:00:00.000";
            arrStr[0] = strBeginDay;
            arrStr[1] = strEndDay;
            return arrStr;
        }

        private string getChooseDay()
        {
            string strDate = "";

            strDate = (DateTime.Parse(this.strYear + "-01-01 00:00:00.000") + TimeSpan.FromDays(Int32.Parse(this.strWeek)*7)).ToString("yyyy-MM-dd HH:mm:ss.000");
            return strDate;
        }


        #endregion
    }
}
