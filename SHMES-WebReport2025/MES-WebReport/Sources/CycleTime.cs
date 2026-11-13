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
using System.Data.SqlClient;

namespace Report.Sources
{
    public class CycleTime
    {
         #region Parameters
        private string strSql = string.Empty;

        private string strFabID = string.Empty;
        private DateTime fromTime;
        private DateTime toTime;
        private string strProdID = string.Empty;
        private string strWOType = string.Empty;
        private string strLotSize = string.Empty;
        private string strDateScope = string.Empty;
        private List<string> stationList = new List<string>();
        private List<string> stationModule = new List<string>();
        private string strTemp = string.Empty;
        private string strLotType = string.Empty;
        private DataSet dsResult;
        private DBConn dbConn;
        #endregion

        #region Properties
        public string FabID
        {
            get
            {
                return this.strFabID;
            }
            set
            {
                this.strFabID = value;
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

        public string ProdID
        {
            get
            {
                return this.strProdID;
            }
            set
            {
                this.strProdID = value;
            }
        }

        public string WOType
        {
            get
            {
                return this.strWOType;
            }
            set
            {
                this.strWOType = value;
            }
        }

        public string LotSize
        {
            get
            {
                return this.strLotSize;
            }
            set
            {
                this.strLotSize = value;
            }
        }

        public string DateScope
        {
            get
            {
                return this.strDateScope;
            }
            set
            {
                this.strDateScope = value;
            }
        }

        public List<string> StationList
        {
            get
            {
                return this.stationList;
            }
            set
            {
                this.stationList = value;
            }
        }

        public List<string> StationModuleList
        {
            get
            {
                return this.stationModule;
            }
            set
            {
                this.stationModule = value;
            }

        }

        public string LotType
        {
            get
            {
                return this.strLotType;
            }
            set
            {
                this.strLotType = value;
            }

        }

        #endregion

        #region Constructor
        public CycleTime()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            string strSqlTemp = "";
            try
            {
                int i;
                int j;
                if (this.FabID == "")
                {
                    throw new Exception("缺少FabID！");
                }
                if (this.FromTime == null)
                {
                    throw new Exception("缺少開始時間！");
                }
                if (this.ToTime == null)
                {
                    throw new Exception("缺少結束時間！");
                }

                TimeSpan ts = this.ToTime - this.FromTime;
                
                if (ts.Seconds < 0)
                {
                    throw new Exception("開始時間小於結束時！");
                }

                strTemp = strTemp + " WHERE FABID = '" + this.strFabID + "'";
                strTemp += " AND ReportDate BETWEEN Convert(Varchar,'" + this.fromTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "',120) AND Convert(Varchar,'" + this.toTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "',120) ";
                i=stationList.Count;
                j=stationModule.Count;
                if ((i> 0) && (j> 0))
                {
                    strTemp += " AND Workctr IN (" + this.AdjustString(this.stationList) + "," + this.AdjustString(this.stationModule) + ") ";
                }
                else if ((i > 0) && (j <= 0))
                {
                    strTemp += " AND Workctr IN (" + this.AdjustString(this.stationList)  + ") ";
                }
                else
                {
                    strTemp += " AND Workctr IN (" + this.AdjustString(this.stationModule) + ") ";
                }

                if(this.WOType!="")
                {
                    strTemp += " AND WoType='" + this.WOType + "' ";
                }
                if (this.ProdID != "")
                {
                    strTemp += " AND ProdID='" + this.ProdID + "' ";
                }
                if (this.strLotType != "")
                {
                    strTemp += " AND LOTTYPE='"+ this.strLotType+"' ";
                }

                /*
                if (this.LotSize != "")
                {
                    strTemp += " AND LotSize='" + this.LotSize + "' ";
                }
                */

                if ((this.LotSize != "") && (this.LotSize != "0"))
                {
                    //strTemp += " AND LotSize='" + this.LotSize + "' ";
                    if (this.LotSize == "1")
                    {
                        strTemp += " AND AVGQTY>0 AND AVGQTY<100 ";     
                    }
                    else if (this.LotSize == "2")
                    {
                        strTemp += " AND AVGQTY>=100  AND AVGQTY<300 "; 
                    }
                    else
                    {
                        strTemp += "  AND AVGQTY>=300 "; 
                    }
                }

                string strSql = "SELECT ";
                if (this.ProdID == "")
                {
                    strSql += "'ALL' AS ";
                }
                strSql += "ProdID,";
                if (this.DateScope == "0")
                {
                    //strSql += "DATENAME(month,ReportDate)+'-'+DATENAME(year, ReportDate)"; 
                    strSql += "CAST(ReportMonth AS VARCHAR)+'月-'+CAST(Reportyear AS VARCHAR)";
                }
                else
                {
                    //strSql += "DATENAME(week,ReportDate)+'周-'+DATENAME(year, ReportDate)";
                    strSql += "CAST(ReportWeek AS VARCHAR)+'周-'+CAST(Reportyear AS VARCHAR)";
                }
//                strSql += @" AS Month_Week,CAST(SUM(BankTime) AS NUMERIC(17,5)) AS BankTime,
//                         CAST(SUM(RunTime) AS NUMERIC(17,5)) AS RunTime,
//                         CAST(SUM(WaitTime) AS NUMERIC(17,5)) AS WaitTime,
//                         CAST(SUM(HoldTime) AS NUMERIC(17,5)) AS HoldTime,
//                         CAST(SUM(AVGQTY) AS NUMERIC(17,5)) AS AVGQty,
//                         CAST(((SUM(BankTime)+SUM(RunTime)+SUM(WaitTime)+SUM(HoldTime))/3600) AS NUMERIC(17,5)) AS TotalTime_H,
//                         CAST(((SUM(BankTime)+SUM(RunTime)+SUM(WaitTime)+SUM(HoldTime))/86400) AS NUMERIC(17,5)) AS TotalTime_D
//                         FROM RCYCLETIMES";
//                strSql += @" AS Month_Week,
//                            CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) AS BankTime,
//                            CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) AS RunTime,
//                            CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) AS WaitTime,
//                            CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) AS HoldTime,
//                            CAST(SUM(AVGQTY)/COUNT(*) AS NUMERIC(17,0)) AS AVGQty ,
//                            CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            + CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            +CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            +CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            AS TotalTime_H,
//                            CAST((CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            + CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            +CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                            +CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)))
//                            /CAST(24 AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                             AS TotalTime_D
//                            FROM RCYCLETIMES";

//                strSql += @" AS Month_Week,
//                                    CASE WHEN SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END AS BankTime,
//                                    CASE WHEN SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ 
//                                    CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END AS RunTime,
//                                    CASE WHEN SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                                    ELSE 0.00 END AS WaitTime,
//                                    CASE WHEN SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END AS HoldTime,
//                                     CAST(SUM(AVGQTY)/COUNT(*) AS NUMERIC(17,0)) AS AVGQty ,
//                                    (CASE WHEN SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END +
//                                    CASE WHEN SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ 
//                                    CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END +
//                                    CASE WHEN SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                                    ELSE 0.00 END +
//                                    CASE WHEN SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END) 
//                                     AS TotalTime_H,
//                                    CAST(((CASE WHEN SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END +
//                                    CASE WHEN SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ 
//                                    CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END +
//                                    CASE WHEN SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                                    ELSE 0.00 END +
//                                    CASE WHEN SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
//                                    CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/
//                                    CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
//                                    ELSE 0.00 END))
//                                     /CAST(24 AS NUMERIC(17,5)) AS NUMERIC(17,2))
//                                     AS TotalTime_D
//                                            FROM RCYCLETIMES";
                strSql += @" AS Month_Week,
                                WORKCTR,
                                CASE WHEN SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)>0 THEN
                                CAST(CAST(SUM(ISNULL(BankTime,0)) AS NUMERIC(17,5))/
                                CAST(60*SUM(CASE WHEN BankTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
                                ELSE 0.00 END AS BankTime,
                                CASE WHEN SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
                                CAST(CAST(SUM(ISNULL(RunTime,0)) AS NUMERIC(17,5))/ 
                                CAST(60*SUM(CASE WHEN RunTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
                                ELSE 0.00 END AS RunTime,
                                CASE WHEN SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
                                CAST(CAST(SUM(ISNULL(WaitTime,0)) AS NUMERIC(17,5))/
                                CAST(60*SUM(CASE WHEN WaitTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2))
                                ELSE 0.00 END AS WaitTime,
                                CASE WHEN SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)>0 THEN 
                                CAST(CAST(SUM(ISNULL(HoldTime,0)) AS NUMERIC(17,5))/
                                CAST(60*SUM(CASE WHEN HoldTime>0 THEN LOTCNT ELSE 0 END)  AS NUMERIC(17,5)) AS NUMERIC(17,2)) 
                                ELSE 0.00 END AS HoldTime,
                                CAST(SUM(AVGQTY)/COUNT(*) AS NUMERIC(17,0)) AS AVGQty
                                FROM RCYCLETIMES";
                strSql+=strTemp;
                strSql += " GROUP BY ";
                if (this.ProdID != "")
                {
                    strSql += "ProdID,";
                }
                if (this.DateScope == "0")
                {
                    strSql += "CAST(ReportMonth AS VARCHAR)+'月-'+CAST(Reportyear AS VARCHAR),WORKCTR ";
                }
                else
                {
                    strSql += "CAST(ReportWeek AS VARCHAR)+'周-'+CAST(Reportyear AS VARCHAR),WORKCTR ";
                }
                strSqlTemp = @"SELECT H.PRODID AS PRODID,H.Month_Week AS Month_Week,SUM(H.BankTime) AS BankTime,SUM(H.RunTime) AS RunTime,SUM(H.WaitTime) AS WaitTime,SUM(H.HoldTime) AS HoldTime,
                        CAST(CAST(SUM(H.AVGQTY) AS NUMERIC)/CAST(COUNT(*) AS NUMERIC) AS NUMERIC(17,2)) AS AVGQTY,
                        (SUM(H.BankTime)+SUM(H.RunTime)+SUM(H.WaitTime)+SUM(H.HoldTime)) AS TotalTime_H,
                        CAST((SUM(H.BankTime)+SUM(H.RunTime)+SUM(H.WaitTime)+SUM(H.HoldTime))/CAST(24 AS NUMERIC) AS NUMERIC(17,2)) AS TotalTime_H_D,
                        CAST((SUM(H.BankTime)+SUM(H.RunTime)+SUM(H.WaitTime))/CAST(24 AS NUMERIC) AS NUMERIC(17,2)) AS TotalTime_D
                        FROM 
                        ( ";
                strSql =strSqlTemp+ strSql;
                strSql += " )H GROUP BY H.PRODID,H.Month_Week ORDER BY  H.PRODID,H.Month_Week";
                dsResult = this.dbConn.GetDataSetBySql(strSql.ToString());
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 拼合字串以便SQL使用IN的方式串
        /// </summary>
        /// <param name="list">條件的範型</param>
        /// <returns>拼合后的字串</returns>
        private string AdjustString(List<string> list)
        {
            string strResult = string.Empty;
            StringBuilder sbList = new StringBuilder();
            foreach (string s in list)
            {
                sbList.Append("'").Append(s).Append("'");
            }
            strResult = sbList.ToString().Replace("''", "','");

            return strResult;

        }

        #endregion
    }
}
