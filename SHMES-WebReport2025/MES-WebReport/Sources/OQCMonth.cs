using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Report.Library;
using System.Collections.Generic;

namespace Report.Sources
{
    public class OQCMonth
    {
        private string strGroup = "";
        private string strColumnsName = "";
        private DBConn dbConn;
        private string strFabid = "";
        private string strMonth = "";
        private string strYear = "";

        public OQCMonth()
        {
            this.dbConn = new DBConn();
        }

        #region Public Property

        /// <summary>
        /// Month
        /// </summary>
        public string Month
        {
            get
            {
                return this.strMonth;
            }
            set
            {
                this.strMonth = value;
            }
        }

        /// <summary>
        /// Year
        /// </summary>
        public string Year
        {
            get
            {
                return this.strYear;
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
        /// 取得欄位名稱
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
        #endregion

        #region Public Methods
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
                strSql = @" select (cast( (PLANDPPM ) as numeric) ) as PLANDPPM,
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
        /// 讀取12個月中ROQCS資料
        /// </summary>
        /// <returns></returns>
        public DataSet Read()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtTmp;
            int intIndex = 0;
            string[] arrStr;
            try
            {
                dt = this.getOQCMonth().Copy();
                arrStr = this.ColumnsName.Split(',');
                dtTmp = dt.Clone();
                intIndex = dt.Rows.Count;
                for (int i = 0; i < 12 ; i++)
                {
                    dtTmp.Rows.InsertAt(this.createNewRow(dtTmp), 0);
                }
                for (int i = 0; i < 12; i++)
                {
                    for(int j =0 ; j< dt.Rows.Count; j++)
                    {
                        if (Int32.Parse(arrStr[i].Substring(5, arrStr[i].Length-5)) == Int32.Parse(dt.Rows[j]["REPORTMONTH"].ToString()))
                        {
                            dtTmp.Rows[i]["REPORTYEAR"] = dt.Rows[j]["REPORTYEAR"].ToString();
                            dtTmp.Rows[i]["REPORTMONTH"] = dt.Rows[j]["REPORTMONTH"].ToString();
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

                    ds.Tables.Add(dtTmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable getDTRoqcd()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            try
            {
                //strSql = " SELECT OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' ORDER BY OQCNGQTY DESC ";
                strSql = " SELECT DEFECTCD,SUM(OQCNGQTY) AS OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' AND REPORTMONTH=" + strMonth + " GROUP BY DEFECTCD ORDER BY SUM(OQCNGQTY) DESC ";
                dt = this.dbConn.GetDataSetBySql(strSql, "ROQCD").Tables[0];

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
        public List<int> getRoqcd()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            List<int> arrList = new List<int>();
            int intCount = 0;

            try
            {
                strSql = " SELECT OQCNGQTY FROM ROQCD WHERE FABID = '" + this.strFabid + "' AND PRODGROUP = '" + this.strGroup + "' ORDER BY OQCNGQTY DESC ";
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


        #endregion

        #region Private Methods
        private DataTable getOQCMonth()
        {
            DataTable dt = new DataTable();
            string strSql = "";
            string strDate = "";
            string strEndDate = "";

            try
            {


                if (Int32.Parse( this.strMonth ) < 12)
                {
                    this.strColumnsName = (Int32.Parse( this.strYear) - 1).ToString() + "-" + (Int32.Parse( this.strMonth)+1).ToString("00");
                    strDate = (Int32.Parse( this.strYear) - 1).ToString() + "-" + (Int32.Parse( this.strMonth )+1).ToString() + "-" + "01 00:00:00.000";

                    for (int i = 0; i < 11; i++)
                    {
                        if (Int32.Parse( this.strMonth )+ 2 + i > 12)
                        {
                            this.strColumnsName = this.strColumnsName + "," + Int32.Parse( this.strYear).ToString() + "-" + (Int32.Parse( this.strMonth ) -10 + i).ToString();
                        }
                        else
                        {
                            this.strColumnsName = this.strColumnsName + "," + (Int32.Parse( this.strYear) - 1).ToString() + "-" + (Int32.Parse( this.strMonth ) + 2 + i).ToString();
                        }
                    }
                    strEndDate = this.strYear + "-"+(Int32.Parse( this.strMonth) +1).ToString("00")+"-"+"01 00:00:00.000";
                }
                else
                {
                    strDate = Int32.Parse( this.strYear).ToString() + "-" + "01-01 00:00:00.000";
                    this.strColumnsName = Int32.Parse( this.strYear).ToString() + "-" + (Int32.Parse( this.strMonth ) -11).ToString();
                    for (int i = 0; i < 11; i++)
                    {
                        this.strColumnsName = this.strColumnsName + (Int32.Parse( this.strMonth ) - 10 + i).ToString();
                    }
                    strEndDate =( Int32. Parse( this.strYear)+1) + "-"+"01-01 00:00:00.000";
                }

                strSql = @"  SELECT  REPORTYEAR,REPORTMONTH,DPPMTOTAL,DPPMTOTALTEST,
                        CASE WHEN DPPMTOTALTEST= 0 THEN 0 ELSE ((CAST(DPPMTOTAL AS  FLOAT)/CAST( DPPMTOTALTEST AS FLOAT ))*1000000) END AS DPPM,
                         DFQTY,LRRTOTAL,LRRDFLOT
                        ,CASE WHEN LRRTOTAL = 0 THEN '0' ELSE CAST( (CAST(LRRDFLOT AS FLOAT) /CAST(LRRTOTAL AS FLOAT)) * 100   AS VARCHAR(100)) + '%' END AS LRR 
                        FROM (SELECT REPORTYEAR,REPORTMONTH, SUM(AOUTQTY)AS DPPMTOTAL,SUM(QTQTY)AS DPPMTOTALTEST,SUM(NGQTY)AS DFQTY " +
                        " ,SUM(QTCNT)AS LRRTOTAL,SUM(NGCNT)AS LRRDFLOT FROM ROQCS R WHERE R.PRODGROUP ='" + this.Group + "' AND R.REPORTDATE >= '" + strDate + "' AND R.REPORTDATE<'" + strEndDate + "' " +
                       " GROUP BY R.REPORTYEAR,R.REPORTMONTH)AS A " ;
                dt = this.dbConn.GetDataSetBySql(strSql, "ROQCS").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        private DataRow createNewRow(DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["REPORTYEAR"] = "0";
            dr["REPORTMONTH"] = "0";
            //dr["REPORTWEEK"] = "0";
            dr["DPPMTOTAL"] = "0";
            dr["DPPMTOTALTEST"] = "0";
            dr["DPPM"] = "0";
            dr["DFQTY"] = "0";
            dr["LRRTOTAL"] = "0";
            dr["LRRDFLOT"] = "0";
            dr["LRR"] = "0";
            return dr;
        }
        #endregion

    }
}
