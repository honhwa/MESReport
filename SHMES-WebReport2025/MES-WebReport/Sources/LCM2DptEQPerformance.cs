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
using System.Text;

namespace Report.Sources
{
    public class LCM2DptEQPerformance
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strFabId = string.Empty;
        private string strTimeBy = string.Empty;
        private DateTime fromTime;
        private DateTime toTime;
        private string strProcess = string.Empty;
        private List<string> workCtrList = new List<string>();
        private List<string> subLineIdList = new List<string>();
        private List<string> eqIdList = new List<string>();
        private List<string> eqTypeList = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// Fab ID
        /// </summary>
        public string FabID
        {
            get
            {
                return this.strFabId;
            }
            set
            {
                this.strFabId = value;
            }
        }
        public string TimeBy
        {
            get
            {
                return this.strTimeBy;
            }
            set
            {
                this.strTimeBy = value;
            }
        }
        public DateTime FromTime
        {
            get
            {
                return this.fromTime;
            }
            set
            {
                this.fromTime = value;
            }
        }

        public DateTime ToTime
        {
            get
            {
                return this.toTime;
            }
            set
            {
                this.toTime = value;
            }
        }
        public string Process
        {
            get
            {
                return this.strProcess;
            }
            set
            {
                this.strProcess = value;
            }
        }

        public List<string> WorkCTRList
        {
            get
            {
                return this.workCtrList;
            }
            set
            {
                this.workCtrList = value;
            }
        }

        public List<string> SubLineIDList
        {
            get
            {
                return this.subLineIdList;
            }
            set
            {
                this.subLineIdList = value;
            }
        }

        public List<string> EQIDList
        {
            get
            {
                return this.eqIdList;
            }
            set
            {
                this.eqIdList = value;
            }
        }

        public List<string> EQTypeList
        {
            get
            {
                return this.eqTypeList;
            }
            set
            {
                this.eqTypeList = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LCM2DptEQPerformance()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                if (this.fromTime == null)
                {
                    throw new Exception("沒有開始時間！");
                }
                if (this.toTime == null)
                {
                    throw new Exception("沒有結束時間！");
                }
                if (this.fromTime > this.toTime)
                {
                    throw new Exception("結束時間早于開始時間！");
                }
                if (this.Process == string.Empty)
                {
                    throw new Exception("Process未選取！");
                }
                if (this.workCtrList == null || this.workCtrList.Count <= 0)
                {
                    throw new Exception("Station未選取！");
                }
                //if (this.subLineIdList == null || this.subLineIdList.Count <= 0)
                //{
                //    throw new Exception("SubLineID未選取！");
                //}
                if (this.eqIdList == null || this.eqIdList.Count <= 0)
                {
                    throw new Exception("EQID未選取！");
                }
                //if (this.eqTypeList == null || this.eqTypeList.Count <= 0)
                //{
                //    throw new Exception("EQType未選取！");
                //}

                string strCol = string.Empty;
                string strFilter1 = string.Empty;
                string strGroupFilter = string.Empty;

                if (this.strTimeBy == "Month")
                {
                    strCol += "cast( RSL.TRANSMONTH as varchar) ) ";
                    strGroupFilter += " cast( RSL.TRANSMONTH as varchar) ), ";
                }
                else if (this.strTimeBy == "Week")
                {
                    strCol += " CAST(DATEPART(WEEK,RSL.TRANSDATE) AS VARCHAR))";
                    strGroupFilter += " CAST(DATEPART(WEEK,RSL.TRANSDATE) AS VARCHAR)),";
                }
                else if (this.strTimeBy == "Date")
                {
                    strCol += "SUBSTRING(CONVERT(VARCHAR(10),RSL.TRANSDATE ,120),6,9))";
                    strGroupFilter += "SUBSTRING(CONVERT(VARCHAR(10),RSL.TRANSDATE ,120),6,9)),";
                }
                strFilter1 += " RSL.FABID = '" + this.strFabId + "' ";

                StringBuilder sbEQID = new StringBuilder();
                foreach (string s in this.eqIdList)
                {
                    sbEQID.Append("'").Append(s).Append("'");
                }
                strFilter1 += " AND RSL.EQID IN (" + sbEQID.ToString().Replace("''", "','") + ") ";

                if (this.EQTypeList != null && this.EQTypeList.Count > 0)
                {
                    StringBuilder sbEQType = new StringBuilder();
                    foreach (string s in this.eqTypeList)
                    {
                        sbEQType.Append("'").Append(s).Append("'");
                    }
                    strFilter1 += " AND EQ.EQTID IN (" + sbEQType.ToString().Replace("''", "','") + ") ";
                }

                StringBuilder sbWorkCTR = new StringBuilder();
                foreach (string s in this.workCtrList)
                {
                    sbWorkCTR.Append("'").Append(s).Append("'");
                }
                strFilter1 += " AND RSL.WORKCTR IN (" + sbWorkCTR.ToString().Replace("''", "','") + ") ";

                StringBuilder sbSubLineID = new StringBuilder();
                if (this.subLineIdList != null && this.subLineIdList.Count > 0)
                {

                    foreach (string s in this.subLineIdList)
                    {
                        sbSubLineID.Append("'").Append(s).Append("'");
                    }
                    strFilter1 += " AND RSL.SUBLINEID IN (" + sbSubLineID.ToString().Replace("''", "','") + ") ";
                }

                if (this.strProcess.ToUpper() == "MODULE")
                {
                    strFilter1 += " AND RSL.SHOPCD = 'Module' ";
                }
                else if (this.strProcess.ToUpper() == "CELL2")
                {
                    strFilter1 += " AND RSL.SHOPCD = 'Cell2' ";
                }
                //strFilter1 += "  AND CONVERT(VARCHAR,RSL.TRANSDATE ,120)  BETWEEN '" + this.fromTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND  '" + this.toTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

                if (this.strTimeBy == "Date")
                {
                    strFilter1 += "  AND CONVERT(VARCHAR(10),RSL.TRANSDATE ,120) ='" + this.fromTime.ToString("yyyy-MM-dd") + "'";
                }
                if (this.strTimeBy == "Week")
                {
                    strFilter1 += "  AND CONVERT(VARCHAR(10),RSL.TRANSDATE ,120)  BETWEEN '" + this.fromTime.AddDays(-1).ToString("yyyy-MM-dd") + "' AND  '" + this.toTime.AddDays(-1).ToString("yyyy-MM-dd") + "'";
                }
                if (this.strTimeBy == "Month")
                {
                    strFilter1 += "  AND CONVERT(VARCHAR(10),RSL.TRANSDATE ,120)  BETWEEN '" + this.fromTime.ToString("yyyy-MM-dd") + "' AND  '" + this.toTime.ToString("yyyy-MM-dd") + "'";
                }
                string strSql = @"
                        SELECT 
                        (cast (YEAR(RSL.TRANSDATE) as varchar) + ' - ' + "
                         + strCol + @" AS TRANSDATE,
                         RSL.WORKCTR,
                         SL.LINEID,
                         '' AS SUBLINEID, -- EMPTY
                         RSL.EQID,
                        SUM(CASE WHEN EC.DEPART='ACC' THEN RSL.SUMDATE ELSE 0 END) AS ACC,
                        SUM(CASE WHEN EC.DEPART='ENG' THEN RSL.SUMDATE ELSE 0 END) AS ENG,
                        SUM (CASE WHEN EC.DEPART='FAC' THEN RSL.SUMDATE ELSE 0 END) AS FAC,
                        SUM(CASE WHEN EC.DEPART='IQC' THEN RSL.SUMDATE ELSE 0 END) AS IQC,
                        SUM(CASE WHEN EC.DEPART='MFG' THEN RSL.SUMDATE ELSE 0 END) AS MFG,
                        SUM(CASE WHEN EC.DEPART='OTHER' THEN RSL.SUMDATE ELSE 0 END) AS OTHER,
                        SUM(CASE WHEN EC.DEPART='PC' THEN RSL.SUMDATE ELSE 0 END) AS PC,
                        SUM(CASE WHEN EC.DEPART='PE' THEN RSL.SUMDATE ELSE 0 END) AS PE,
                        SUM(CASE WHEN EC.DEPART='PUR' THEN RSL.SUMDATE ELSE 0 END) AS PUR,
                        SUM(CASE WHEN EC.DEPART='QRA' THEN RSL.SUMDATE ELSE 0 END) AS QRA,
                        SUM (CASE WHEN EC.DEPART IN ('ACC','ENG','FAC','IQC','MFG','OTHER','PC','PE','PUR','QRA') THEN RSL.SUMDATE ELSE 0 END ) AS ALLTIME
                          FROM RSUMEQSHM RSL
                         LEFT JOIN MSLM SL ON SL.FABID = RSL.FABID
                                           AND SL.SUBLINEID = RSL.SUBLINEID
                         LEFT JOIN MECM EC ON RSL.FABID = EC.FABID
                                           AND RSL.PREVEQCCD = EC.EQCCD
                                           AND RSL.PREVSTATUS = EC.EQCD
                         LEFT JOIN MEQM EQ ON EQ.FABID=EC.FABID AND EQ.EQID=RSL.EQID
                         WHERE " + strFilter1 + @"
                         GROUP BY  (cast (YEAR(RSL.TRANSDATE) as varchar) + ' - ' + " + strGroupFilter + @" RSL.WORKCTR, SL.LINEID,RSL.SUBLINEID, RSL.EQID ";
                this.dsResult = this.dbConn.GetDataSetBySql(strSql);

                return this.dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
