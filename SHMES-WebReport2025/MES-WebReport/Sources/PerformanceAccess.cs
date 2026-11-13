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
    public class PerformanceAccess
    {
        #region Parameters
        private string strSql = string.Empty;

        private string strFabID = string.Empty;
        private string queryYear;
        private string queryMonth;
        private string strProdGroup = string.Empty;
        private string strTeamID = string.Empty;
        private string strTemp = string.Empty;

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

        public string Year
        {
            get
            {
                return this.queryYear;
            }
            set
            {
                this.queryYear = value;
            }
        }

        public string Month
        {
            get
            {
                return this.queryMonth;
            }
            set
            {
                this.queryMonth = value;
            }
        }

        public string ProdGroup
        {
            get
            {
                return this.strProdGroup;
            }
            set
            {
                this.strProdGroup = value;
            }
        }

        public string TeamID
        {
            get
            {
                return this.strTeamID;
            }
            set
            {
                this.strTeamID = value;
            }
        }

        #endregion

        #region Constructor
        public PerformanceAccess()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                if (this.FabID == "")
                {
                    throw new Exception("缺少FabID！");
                }
                if (this.Year == null || this.Month == null)
                {
                    throw new Exception("缺少查詢時間！");
                }
                else
                {
                    strTemp = " Where YEAR(ReportDate)='" + this.Year + "' AND MONTH(ReportDate)='"+this.Month+"'  AND WORKCTR IN ('8180','8280','8380') ";
                }
                if (this.ProdGroup != "")
                {
                    strTemp += " AND ProdGroup='"+this.ProdGroup+"' ";
                }
                if (this.TeamID != "")
                {
                    strTemp += " AND TEAMID='"+this.TeamID+"' ";
                }


//                string strSql = @"SELECT ATEAMID AS TEAMID,
//	                           CAST((ISNULL(D1ARATE,0)+ISNULL(D1GRATE,0)+ISNULL(D1SRATE,0)+ISNULL(D2ARATE,0)+ISNULL(D2GRATE,0)+ISNULL(D2SRATE,0)+
//                               ISNULL(D3ARATE,0)+ISNULL(D3GRATE,0)+ISNULL(D3SRATE,0)+ISNULL(D4ARATE,0)+ISNULL(D4GRATE,0)+ISNULL(D4SRATE,0)+
//                               ISNULL(D5ARATE,0)+ISNULL(D5GRATE,0)+ISNULL(D5SRATE,0)+ISNULL(D6ARATE,0)+ISNULL(D6GRATE,0)+ISNULL(D6SRATE,0)+
//                               ISNULL(D7ARATE,0)+ISNULL(D7GRATE,0)+ISNULL(D7SRATE,0)+ISNULL(D8ARATE,0)+ISNULL(D8GRATE,0)+ISNULL(D8SRATE,0)+
//	                           ISNULL(D9ARATE,0)+ISNULL(D9GRATE,0)+ISNULL(D9SRATE,0)+ISNULL(D10ARATE,0)+ISNULL(D10GRATE,0)+ISNULL(D10SRATE,0)+
//                               ISNULL(D11ARATE,0)+ISNULL(D11GRATE,0)+ISNULL(D11SRATE,0)+ISNULL(D12ARATE,0)+ISNULL(D12GRATE,0)+ISNULL(D12SRATE,0)+
//                               ISNULL(D13ARATE,0)+ISNULL(D13GRATE,0)+ISNULL(D13SRATE,0)+ISNULL(D14ARATE,0)+ISNULL(D14GRATE,0)+ISNULL(D14SRATE,0)+
//                               ISNULL(D15ARATE,0)+ISNULL(D15GRATE,0)+ISNULL(D15SRATE,0)+ISNULL(D16ARATE,0)+ISNULL(D16GRATE,0)+ISNULL(D16SRATE,0)+
//	                           ISNULL(D17ARATE,0)+ISNULL(D17GRATE,0)+ISNULL(D17SRATE,0)+ISNULL(D18ARATE,0)+ISNULL(D18GRATE,0)+ISNULL(D18SRATE,0)+
//                               ISNULL(D19ARATE,0)+ISNULL(D19GRATE,0)+ISNULL(D19SRATE,0)+ISNULL(D20ARATE,0)+ISNULL(D20GRATE,0)+ISNULL(D20SRATE,0)+
//	                           ISNULL(D21ARATE,0)+ISNULL(D21GRATE,0)+ISNULL(D21SRATE,0)+ISNULL(D22ARATE,0)+ISNULL(D22GRATE,0)+ISNULL(D22SRATE,0)+
//                               ISNULL(D23ARATE,0)+ISNULL(D23GRATE,0)+ISNULL(D23SRATE,0)+ISNULL(D24ARATE,0)+ISNULL(D24GRATE,0)+ISNULL(D24SRATE,0)+
//                               ISNULL(D25ARATE,0)+ISNULL(D25GRATE,0)+ISNULL(D25SRATE,0)+ISNULL(D26ARATE,0)+ISNULL(D26GRATE,0)+ISNULL(D26SRATE,0)+
//                               ISNULL(D27ARATE,0)+ISNULL(D27GRATE,0)+ISNULL(D27SRATE,0)+ISNULL(D28ARATE,0)+ISNULL(D28GRATE,0)+ISNULL(D28SRATE,0)+
//                               ISNULL(D29ARATE,0)+ISNULL(D29GRATE,0)+ISNULL(D29SRATE,0)+ISNULL(D30ARATE,0)+ISNULL(D30GRATE,0)+ISNULL(D30SRATE,0)+
//                               ISNULL(D31ARATE,0)+ISNULL(D31GRATE,0)+ISNULL(D31SRATE,0))AS INT) AS TOTALSUM,
//	                          (ROUND((ARRIVEPRODDAY+GOODPRODDAY+SCRAPPRODDAY)/2,0)) AS PRODDAY,
//                               ROUND(CAST(((ISNULL(D1ARATE,0)+ISNULL(D1GRATE,0)+ISNULL(D1SRATE,0)+ISNULL(D2ARATE,0)+ISNULL(D2GRATE,0)+ISNULL(D2SRATE,0)+
//                               ISNULL(D3ARATE,0)+ISNULL(D3GRATE,0)+ISNULL(D3SRATE,0)+ISNULL(D4ARATE,0)+ISNULL(D4GRATE,0)+ISNULL(D4SRATE,0)+
//                               ISNULL(D5ARATE,0)+ISNULL(D5GRATE,0)+ISNULL(D5SRATE,0)+ISNULL(D6ARATE,0)+ISNULL(D6GRATE,0)+ISNULL(D6SRATE,0)+
//                               ISNULL(D7ARATE,0)+ISNULL(D7GRATE,0)+ISNULL(D7SRATE,0)+ISNULL(D8ARATE,0)+ISNULL(D8GRATE,0)+ISNULL(D8SRATE,0)+
//	                           ISNULL(D9ARATE,0)+ISNULL(D9GRATE,0)+ISNULL(D9SRATE,0)+ISNULL(D10ARATE,0)+ISNULL(D10GRATE,0)+ISNULL(D10SRATE,0)+
//                               ISNULL(D11ARATE,0)+ISNULL(D11GRATE,0)+ISNULL(D11SRATE,0)+ISNULL(D12ARATE,0)+ISNULL(D12GRATE,0)+ISNULL(D12SRATE,0)+
//                               ISNULL(D13ARATE,0)+ISNULL(D13GRATE,0)+ISNULL(D13SRATE,0)+ISNULL(D14ARATE,0)+ISNULL(D14GRATE,0)+ISNULL(D14SRATE,0)+
//                               ISNULL(D15ARATE,0)+ISNULL(D15GRATE,0)+ISNULL(D15SRATE,0)+ISNULL(D16ARATE,0)+ISNULL(D16GRATE,0)+ISNULL(D16SRATE,0)+
//	                           ISNULL(D17ARATE,0)+ISNULL(D17GRATE,0)+ISNULL(D17SRATE,0)+ISNULL(D18ARATE,0)+ISNULL(D18GRATE,0)+ISNULL(D18SRATE,0)+
//                               ISNULL(D19ARATE,0)+ISNULL(D19GRATE,0)+ISNULL(D19SRATE,0)+ISNULL(D20ARATE,0)+ISNULL(D20GRATE,0)+ISNULL(D20SRATE,0)+
//	                           ISNULL(D21ARATE,0)+ISNULL(D21GRATE,0)+ISNULL(D21SRATE,0)+ISNULL(D22ARATE,0)+ISNULL(D22GRATE,0)+ISNULL(D22SRATE,0)+
//                               ISNULL(D23ARATE,0)+ISNULL(D23GRATE,0)+ISNULL(D23SRATE,0)+ISNULL(D24ARATE,0)+ISNULL(D24GRATE,0)+ISNULL(D24SRATE,0)+
//                               ISNULL(D25ARATE,0)+ISNULL(D25GRATE,0)+ISNULL(D25SRATE,0)+ISNULL(D26ARATE,0)+ISNULL(D26GRATE,0)+ISNULL(D26SRATE,0)+
//                               ISNULL(D27ARATE,0)+ISNULL(D27GRATE,0)+ISNULL(D27SRATE,0)+ISNULL(D28ARATE,0)+ISNULL(D28GRATE,0)+ISNULL(D28SRATE,0)+
//                               ISNULL(D29ARATE,0)+ISNULL(D29GRATE,0)+ISNULL(D29SRATE,0)+ISNULL(D30ARATE,0)+ISNULL(D30GRATE,0)+ISNULL(D30SRATE,0)+
//                               ISNULL(D31ARATE,0)+ISNULL(D31GRATE,0)+ISNULL(D31SRATE,0))
//                               /CAST(ROUND((ARRIVEPRODDAY+GOODPRODDAY+SCRAPPRODDAY)/2,0)AS MONEY))AS MONEY),2) AS AVGDAY, 
//	                           D1ARATE,D1GRATE,D1SRATE,D2ARATE,D2GRATE,D2SRATE,D3ARATE,D3GRATE,D3SRATE,D4ARATE,D4GRATE,D4SRATE,
//                               D5ARATE,D5GRATE,D5SRATE,D6ARATE,D6GRATE,D6SRATE,D7ARATE,D7GRATE,D7SRATE,D8ARATE,D8GRATE,D8SRATE,
//	                           D9ARATE,D9GRATE,D9SRATE,D10ARATE,D10GRATE,D10SRATE,D11ARATE,D11GRATE,D11SRATE,D12ARATE,D12GRATE,D12SRATE,
//                               D13ARATE,D13GRATE,D13SRATE,D14ARATE,D14GRATE,D14SRATE,D15ARATE,D15GRATE,D15SRATE,D16ARATE,D16GRATE,D16SRATE,
//	                           D17ARATE,D17GRATE,D17SRATE,D18ARATE,D18GRATE,D18SRATE,D19ARATE,D19GRATE,D19SRATE,D20ARATE,D20GRATE,D20SRATE,
//	                           D21ARATE,D21GRATE,D21SRATE,D22ARATE,D22GRATE,D22SRATE,D23ARATE,D23GRATE,D23SRATE,D24ARATE,D24GRATE,D24SRATE,
//                               D25ARATE,D25GRATE,D25SRATE,D26ARATE,D26GRATE,D26SRATE,D27ARATE,D27GRATE,D27SRATE,D28ARATE,D28GRATE,D28SRATE,
//                               D29ARATE,D29GRATE,D29SRATE,D30ARATE,D30GRATE,D30SRATE,D31ARATE,D31GRATE,D31SRATE
//                        FROM 
//                        (SELECT TEAMID AS ATEAMID,ARRIVEPRODDAY,
//                                SUM([1]) AS 'D1ARATE',SUM([2]) AS 'D2ARATE',SUM([3]) AS 'D3ARATE',SUM([4]) AS 'D4ARATE'
//                               ,SUM([5]) AS 'D5ARATE',SUM([6]) AS 'D6ARATE',SUM([7]) AS 'D7ARATE',SUM([8]) AS 'D8ARATE'
//                               ,SUM([9]) AS 'D9ARATE',SUM([10]) AS 'D10ARATE',SUM([11]) AS 'D11ARATE',SUM([12]) AS 'D12ARATE'
//                               ,SUM([13]) AS 'D13ARATE',SUM([14]) AS 'D14ARATE',SUM([15]) AS 'D15ARATE',SUM([16]) AS 'D16ARATE'
//                               ,SUM([17]) AS 'D17ARATE',SUM([18]) AS 'D18ARATE',SUM([19]) AS 'D19ARATE',SUM([20]) AS 'D20ARATE'
//                               ,SUM([21]) AS 'D21ARATE',SUM([22]) AS 'D22ARATE',SUM([23]) AS 'D23ARATE',SUM([24]) AS 'D24ARATE'
//                               ,SUM([25]) AS 'D25ARATE',SUM([26]) AS 'D26ARATE',SUM([27]) AS 'D27ARATE',SUM([28]) AS 'D28ARATE'
//                               ,SUM([29]) AS 'D29ARATE',SUM([30]) AS 'D30ARATE',SUM([31]) AS 'D31ARATE' FROM 
//                               (SELECT TEAMID,DAY(ReportDate) AS ReportDay,1 AS ARRIVEPRODDAY,
//                                CASE WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END)>1 THEN 5
//                                     WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END) BETWEEN 0.91 AND 1 THEN 4       
//			                         WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END) BETWEEN 0.81 AND 0.9 THEN 3       
//                                     WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END) BETWEEN 0.71 AND 0.8 THEN 1       
//			                         WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END)<=0.7 THEN 0 END AS ARRIVERATE
//                                FROM RMDD ";
//                      strSql += this.strTemp;
//                      strSql += @" GROUP BY TEAMID,ReportDate) liming
//                                PIVOT(
//                                SUM(ARRIVERATE)
//                                FOR ReportDay IN 
//                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
//	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
//                               ,[29],[30],[31])) AS pvt
//	                           GROUP BY TEAMID,ARRIVEPRODDAY) AS A 
//                               INNER JOIN 
//                        (SELECT TEAMID AS GTEAMID,GOODPRODDAY,
//                                SUM([1]) AS 'D1GRATE',SUM([2]) AS 'D2GRATE',SUM([3]) AS 'D3GRATE',SUM([4]) AS 'D4GRATE'
//                               ,SUM([5]) AS 'D5GRATE',SUM([6]) AS 'D6GRATE',SUM([7]) AS 'D7GRATE',SUM([8]) AS 'D8GRATE'
//                               ,SUM([9]) AS 'D9GRATE',SUM([10]) AS 'D10GRATE',SUM([11]) AS 'D11GRATE',SUM([12]) AS 'D12GRATE'
//                               ,SUM([13]) AS 'D13GRATE',SUM([14]) AS 'D14GRATE',SUM([15]) AS 'D15GRATE',SUM([16]) AS 'D16GRATE'
//                               ,SUM([17]) AS 'D17GRATE',SUM([18]) AS 'D18GRATE',SUM([19]) AS 'D19GRATE',SUM([20]) AS 'D20GRATE'
//                               ,SUM([21]) AS 'D21GRATE',SUM([22]) AS 'D22GRATE',SUM([23]) AS 'D23GRATE',SUM([24]) AS 'D24GRATE'
//                               ,SUM([25]) AS 'D25GRATE',SUM([26]) AS 'D26GRATE',SUM([27]) AS 'D27GRATE',SUM([28]) AS 'D28GRATE'
//                               ,SUM([29]) AS 'D29GRATE',SUM([30]) AS 'D30GRATE',SUM([31]) AS 'D31GRATE' FROM 
//                               (SELECT TEAMID,DAY(ReportDate) AS ReportDay,COUNT(TEAMID) AS GOODPRODDAY,
//                                CASE WHEN (CASE WHEN CAST(SUM(ACTINPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(ACTINPUT) AS FLOAT) END)>=CAST(PYIELD AS FLOAT) THEN 5
//                                     WHEN (CASE WHEN CAST(SUM(ACTINPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(ACTINPUT) AS FLOAT) END) BETWEEN CAST(PYIELD AS FLOAT)-0.0001 AND CAST(PYIELD AS FLOAT)-0.005 THEN 4       
//			                         WHEN (CASE WHEN CAST(SUM(ACTINPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(ACTINPUT) AS FLOAT) END) BETWEEN CAST(PYIELD AS FLOAT)-0.0051 AND CAST(PYIELD AS FLOAT)-0.01 THEN 3       
//                                     WHEN (CASE WHEN CAST(SUM(ACTINPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(ACTINPUT) AS FLOAT) END) BETWEEN CAST(PYIELD AS FLOAT)-0.011 AND CAST(PYIELD AS FLOAT)-0.03 THEN 2       
//			                         WHEN (CASE WHEN CAST(SUM(ACTINPUT) AS FLOAT)=0 THEN 0
//                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(ACTINPUT) AS FLOAT) END) <-0.03 THEN 0 END AS GOODRATE
//                                FROM RMDD ";
//                      strSql += this.strTemp;
//                      strSql += @" GROUP BY TEAMID,ReportDate,PYIELD) liming
//                                PIVOT(
//                                SUM(GOODRATE)
//                                FOR ReportDay IN 
//                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
//	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
//                               ,[29],[30],[31])) AS pvt
//	                           GROUP BY TEAMID,GOODPRODDAY) AS B
//	                           ON A.ATEAMID=B.GTEAMID
//                               INNER JOIN 
//                        (SELECT TEAMID AS STEAMID,SCRAPPRODDAY,
//                                SUM([1]) AS 'D1SRATE',SUM([2]) AS 'D2SRATE',SUM([3]) AS 'D3SRATE',SUM([4]) AS 'D4SRATE'
//                               ,SUM([5]) AS 'D5SRATE',SUM([6]) AS 'D6SRATE',SUM([7]) AS 'D7SRATE',SUM([8]) AS 'D8SRATE'
//                               ,SUM([9]) AS 'D9SRATE',SUM([10]) AS 'D10SRATE',SUM([11]) AS 'D11SRATE',SUM([12]) AS 'D12SRATE'
//                               ,SUM([13]) AS 'D13SRATE',SUM([14]) AS 'D14SRATE',SUM([15]) AS 'D15SRATE',SUM([16]) AS 'D16SRATE'
//                               ,SUM([17]) AS 'D17SRATE',SUM([18]) AS 'D18SRATE',SUM([19]) AS 'D19SRATE',SUM([20]) AS 'D20SRATE'
//                               ,SUM([21]) AS 'D21SRATE',SUM([22]) AS 'D22SRATE',SUM([23]) AS 'D23SRATE',SUM([24]) AS 'D24SRATE'
//                               ,SUM([25]) AS 'D25SRATE',SUM([26]) AS 'D26SRATE',SUM([27]) AS 'D27SRATE',SUM([28]) AS 'D28SRATE'
//                               ,SUM([29]) AS 'D29SRATE',SUM([30]) AS 'D30SRATE',SUM([31]) AS 'D31SRATE' FROM 
//                               (SELECT TEAMID,DAY(ReportDate) AS ReportDay,COUNT(TEAMID) AS SCRAPPRODDAY,
//                                CASE WHEN ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/7)<=CAST(PMSYIELD AS FLOAT) THEN 10
//                                     WHEN ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/7) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0001 AND CAST(PMSYIELD AS FLOAT)+0.004 THEN 8       
//			                         WHEN ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/7) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0041 AND CAST(PMSYIELD AS FLOAT)+0.008 THEN 6 
//			                        WHEN  ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/7) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0081 AND CAST(PMSYIELD AS FLOAT)+0.012 THEN 4
//                                    WHEN  ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/7) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0121 AND CAST(PMSYIELD AS FLOAT)+0.016 THEN 2
//			                        WHEN  ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/7)>0.016 THEN 0 END AS SCRAPRATE
//                                FROM RMDD ";
//                      strSql += this.strTemp;
//                      strSql += @" GROUP BY TEAMID,ReportDate,PMSYIELD) liming
//                                PIVOT(
//                                SUM(SCRAPRATE)
//                                FOR ReportDay IN 
//                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
//	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
//                               ,[29],[30],[31])) AS pvt
//	                           GROUP BY TEAMID,SCRAPPRODDAY) AS C
//                               ON B.GTEAMID=C.STEAMID";

                string strSql = @"SELECT ATEAMID AS TEAMID,
	                           CAST((ISNULL(D1ARATE,0)+ISNULL(D1GRATE,0)+ISNULL(D1SRATE,0)+ISNULL(D2ARATE,0)+ISNULL(D2GRATE,0)+ISNULL(D2SRATE,0)+
                               ISNULL(D3ARATE,0)+ISNULL(D3GRATE,0)+ISNULL(D3SRATE,0)+ISNULL(D4ARATE,0)+ISNULL(D4GRATE,0)+ISNULL(D4SRATE,0)+
                               ISNULL(D5ARATE,0)+ISNULL(D5GRATE,0)+ISNULL(D5SRATE,0)+ISNULL(D6ARATE,0)+ISNULL(D6GRATE,0)+ISNULL(D6SRATE,0)+
                               ISNULL(D7ARATE,0)+ISNULL(D7GRATE,0)+ISNULL(D7SRATE,0)+ISNULL(D8ARATE,0)+ISNULL(D8GRATE,0)+ISNULL(D8SRATE,0)+
	                           ISNULL(D9ARATE,0)+ISNULL(D9GRATE,0)+ISNULL(D9SRATE,0)+ISNULL(D10ARATE,0)+ISNULL(D10GRATE,0)+ISNULL(D10SRATE,0)+
                               ISNULL(D11ARATE,0)+ISNULL(D11GRATE,0)+ISNULL(D11SRATE,0)+ISNULL(D12ARATE,0)+ISNULL(D12GRATE,0)+ISNULL(D12SRATE,0)+
                               ISNULL(D13ARATE,0)+ISNULL(D13GRATE,0)+ISNULL(D13SRATE,0)+ISNULL(D14ARATE,0)+ISNULL(D14GRATE,0)+ISNULL(D14SRATE,0)+
                               ISNULL(D15ARATE,0)+ISNULL(D15GRATE,0)+ISNULL(D15SRATE,0)+ISNULL(D16ARATE,0)+ISNULL(D16GRATE,0)+ISNULL(D16SRATE,0)+
	                           ISNULL(D17ARATE,0)+ISNULL(D17GRATE,0)+ISNULL(D17SRATE,0)+ISNULL(D18ARATE,0)+ISNULL(D18GRATE,0)+ISNULL(D18SRATE,0)+
                               ISNULL(D19ARATE,0)+ISNULL(D19GRATE,0)+ISNULL(D19SRATE,0)+ISNULL(D20ARATE,0)+ISNULL(D20GRATE,0)+ISNULL(D20SRATE,0)+
	                           ISNULL(D21ARATE,0)+ISNULL(D21GRATE,0)+ISNULL(D21SRATE,0)+ISNULL(D22ARATE,0)+ISNULL(D22GRATE,0)+ISNULL(D22SRATE,0)+
                               ISNULL(D23ARATE,0)+ISNULL(D23GRATE,0)+ISNULL(D23SRATE,0)+ISNULL(D24ARATE,0)+ISNULL(D24GRATE,0)+ISNULL(D24SRATE,0)+
                               ISNULL(D25ARATE,0)+ISNULL(D25GRATE,0)+ISNULL(D25SRATE,0)+ISNULL(D26ARATE,0)+ISNULL(D26GRATE,0)+ISNULL(D26SRATE,0)+
                               ISNULL(D27ARATE,0)+ISNULL(D27GRATE,0)+ISNULL(D27SRATE,0)+ISNULL(D28ARATE,0)+ISNULL(D28GRATE,0)+ISNULL(D28SRATE,0)+
                               ISNULL(D29ARATE,0)+ISNULL(D29GRATE,0)+ISNULL(D29SRATE,0)+ISNULL(D30ARATE,0)+ISNULL(D30GRATE,0)+ISNULL(D30SRATE,0)+
                               ISNULL(D31ARATE,0)+ISNULL(D31GRATE,0)+ISNULL(D31SRATE,0))AS INT) AS TOTALSUM,
	                           ARRIVEPRODDAY AS PRODDAY,
                               CAST(CAST(((ISNULL(D1ARATE,0)+ISNULL(D1GRATE,0)+ISNULL(D1SRATE,0)+ISNULL(D2ARATE,0)+ISNULL(D2GRATE,0)+ISNULL(D2SRATE,0)+
                               ISNULL(D3ARATE,0)+ISNULL(D3GRATE,0)+ISNULL(D3SRATE,0)+ISNULL(D4ARATE,0)+ISNULL(D4GRATE,0)+ISNULL(D4SRATE,0)+
                               ISNULL(D5ARATE,0)+ISNULL(D5GRATE,0)+ISNULL(D5SRATE,0)+ISNULL(D6ARATE,0)+ISNULL(D6GRATE,0)+ISNULL(D6SRATE,0)+
                               ISNULL(D7ARATE,0)+ISNULL(D7GRATE,0)+ISNULL(D7SRATE,0)+ISNULL(D8ARATE,0)+ISNULL(D8GRATE,0)+ISNULL(D8SRATE,0)+
	                           ISNULL(D9ARATE,0)+ISNULL(D9GRATE,0)+ISNULL(D9SRATE,0)+ISNULL(D10ARATE,0)+ISNULL(D10GRATE,0)+ISNULL(D10SRATE,0)+
                               ISNULL(D11ARATE,0)+ISNULL(D11GRATE,0)+ISNULL(D11SRATE,0)+ISNULL(D12ARATE,0)+ISNULL(D12GRATE,0)+ISNULL(D12SRATE,0)+
                               ISNULL(D13ARATE,0)+ISNULL(D13GRATE,0)+ISNULL(D13SRATE,0)+ISNULL(D14ARATE,0)+ISNULL(D14GRATE,0)+ISNULL(D14SRATE,0)+
                               ISNULL(D15ARATE,0)+ISNULL(D15GRATE,0)+ISNULL(D15SRATE,0)+ISNULL(D16ARATE,0)+ISNULL(D16GRATE,0)+ISNULL(D16SRATE,0)+
	                           ISNULL(D17ARATE,0)+ISNULL(D17GRATE,0)+ISNULL(D17SRATE,0)+ISNULL(D18ARATE,0)+ISNULL(D18GRATE,0)+ISNULL(D18SRATE,0)+
                               ISNULL(D19ARATE,0)+ISNULL(D19GRATE,0)+ISNULL(D19SRATE,0)+ISNULL(D20ARATE,0)+ISNULL(D20GRATE,0)+ISNULL(D20SRATE,0)+
	                           ISNULL(D21ARATE,0)+ISNULL(D21GRATE,0)+ISNULL(D21SRATE,0)+ISNULL(D22ARATE,0)+ISNULL(D22GRATE,0)+ISNULL(D22SRATE,0)+
                               ISNULL(D23ARATE,0)+ISNULL(D23GRATE,0)+ISNULL(D23SRATE,0)+ISNULL(D24ARATE,0)+ISNULL(D24GRATE,0)+ISNULL(D24SRATE,0)+
                               ISNULL(D25ARATE,0)+ISNULL(D25GRATE,0)+ISNULL(D25SRATE,0)+ISNULL(D26ARATE,0)+ISNULL(D26GRATE,0)+ISNULL(D26SRATE,0)+
                               ISNULL(D27ARATE,0)+ISNULL(D27GRATE,0)+ISNULL(D27SRATE,0)+ISNULL(D28ARATE,0)+ISNULL(D28GRATE,0)+ISNULL(D28SRATE,0)+
                               ISNULL(D29ARATE,0)+ISNULL(D29GRATE,0)+ISNULL(D29SRATE,0)+ISNULL(D30ARATE,0)+ISNULL(D30GRATE,0)+ISNULL(D30SRATE,0)+
                               ISNULL(D31ARATE,0)+ISNULL(D31GRATE,0)+ISNULL(D31SRATE,0))
                               /CAST(ARRIVEPRODDAY AS MONEY))AS MONEY) AS NUMERIC(17,2)) AS AVGDAY, 
	                           D1ARATE,D1GRATE,D1SRATE,D2ARATE,D2GRATE,D2SRATE,D3ARATE,D3GRATE,D3SRATE,D4ARATE,D4GRATE,D4SRATE,
                               D5ARATE,D5GRATE,D5SRATE,D6ARATE,D6GRATE,D6SRATE,D7ARATE,D7GRATE,D7SRATE,D8ARATE,D8GRATE,D8SRATE,
	                           D9ARATE,D9GRATE,D9SRATE,D10ARATE,D10GRATE,D10SRATE,D11ARATE,D11GRATE,D11SRATE,D12ARATE,D12GRATE,D12SRATE,
                               D13ARATE,D13GRATE,D13SRATE,D14ARATE,D14GRATE,D14SRATE,D15ARATE,D15GRATE,D15SRATE,D16ARATE,D16GRATE,D16SRATE,
	                           D17ARATE,D17GRATE,D17SRATE,D18ARATE,D18GRATE,D18SRATE,D19ARATE,D19GRATE,D19SRATE,D20ARATE,D20GRATE,D20SRATE,
	                           D21ARATE,D21GRATE,D21SRATE,D22ARATE,D22GRATE,D22SRATE,D23ARATE,D23GRATE,D23SRATE,D24ARATE,D24GRATE,D24SRATE,
                               D25ARATE,D25GRATE,D25SRATE,D26ARATE,D26GRATE,D26SRATE,D27ARATE,D27GRATE,D27SRATE,D28ARATE,D28GRATE,D28SRATE,
                               D29ARATE,D29GRATE,D29SRATE,D30ARATE,D30GRATE,D30SRATE,D31ARATE,D31GRATE,D31SRATE
                        FROM 
                        (SELECT TEAMID AS ATEAMID,ARRIVEPRODDAY,
                                SUM([1]) AS 'D1ARATE',SUM([2]) AS 'D2ARATE',SUM([3]) AS 'D3ARATE',SUM([4]) AS 'D4ARATE'
                               ,SUM([5]) AS 'D5ARATE',SUM([6]) AS 'D6ARATE',SUM([7]) AS 'D7ARATE',SUM([8]) AS 'D8ARATE'
                               ,SUM([9]) AS 'D9ARATE',SUM([10]) AS 'D10ARATE',SUM([11]) AS 'D11ARATE',SUM([12]) AS 'D12ARATE'
                               ,SUM([13]) AS 'D13ARATE',SUM([14]) AS 'D14ARATE',SUM([15]) AS 'D15ARATE',SUM([16]) AS 'D16ARATE'
                               ,SUM([17]) AS 'D17ARATE',SUM([18]) AS 'D18ARATE',SUM([19]) AS 'D19ARATE',SUM([20]) AS 'D20ARATE'
                               ,SUM([21]) AS 'D21ARATE',SUM([22]) AS 'D22ARATE',SUM([23]) AS 'D23ARATE',SUM([24]) AS 'D24ARATE'
                               ,SUM([25]) AS 'D25ARATE',SUM([26]) AS 'D26ARATE',SUM([27]) AS 'D27ARATE',SUM([28]) AS 'D28ARATE'
                               ,SUM([29]) AS 'D29ARATE',SUM([30]) AS 'D30ARATE',SUM([31]) AS 'D31ARATE' FROM 
                               (
                                SELECT LI1.TEAMID AS TEAMID,ReportDay,ARRIVERATE,LI2.ARRIVEPRODDAY AS ARRIVEPRODDAY FROM 
                                (
                                SELECT TEAMID,DAY(ReportDate) AS ReportDay,
                                CASE WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END)>=1 THEN 5
                                     WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END) BETWEEN 0.90000001 AND 0.9999999999 THEN 4       
			                         WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END) BETWEEN 0.80000001 AND 0.9 THEN 3       
                                     WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END) BETWEEN 0.70000001 AND 0.8 THEN 1       
			                         WHEN (CASE WHEN CAST(SUM(STDOUTPUT) AS FLOAT)=0 THEN 0
                                                ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(STDOUTPUT) AS FLOAT) END)<=0.7 THEN 0 END AS ARRIVERATE
                                FROM RMDD ";
                strSql += this.strTemp;
                strSql += @" GROUP BY TEAMID,ReportDate )LI1 
                            LEFT JOIN 
                            (SELECT TEAMID,SUM(ARRIVEPRODDAY) as ARRIVEPRODDAY FROM 
                            (
                                SELECT TEAMID,DAY(ReportDate) AS ReportDay,1 AS ARRIVEPRODDAY
                                FROM RMDD  ";
                strSql += this.strTemp;
                strSql+= @"GROUP BY TEAMID,ReportDate) LI21 GROUP BY TEAMID )LI2 
                            ON (LI1.TEAMID=LI2.TEAMID)
                                    ) liming
                                PIVOT(
                                SUM(ARRIVERATE)
                                FOR ReportDay IN 
                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
                               ,[29],[30],[31])) AS pvt
	                           GROUP BY TEAMID,ARRIVEPRODDAY) AS A 
                               INNER JOIN 
                        (SELECT TEAMID AS GTEAMID, 
                                SUM([1]) AS 'D1GRATE',SUM([2]) AS 'D2GRATE',SUM([3]) AS 'D3GRATE',SUM([4]) AS 'D4GRATE'
                               ,SUM([5]) AS 'D5GRATE',SUM([6]) AS 'D6GRATE',SUM([7]) AS 'D7GRATE',SUM([8]) AS 'D8GRATE'
                               ,SUM([9]) AS 'D9GRATE',SUM([10]) AS 'D10GRATE',SUM([11]) AS 'D11GRATE',SUM([12]) AS 'D12GRATE'
                               ,SUM([13]) AS 'D13GRATE',SUM([14]) AS 'D14GRATE',SUM([15]) AS 'D15GRATE',SUM([16]) AS 'D16GRATE'
                               ,SUM([17]) AS 'D17GRATE',SUM([18]) AS 'D18GRATE',SUM([19]) AS 'D19GRATE',SUM([20]) AS 'D20GRATE'
                               ,SUM([21]) AS 'D21GRATE',SUM([22]) AS 'D22GRATE',SUM([23]) AS 'D23GRATE',SUM([24]) AS 'D24GRATE'
                               ,SUM([25]) AS 'D25GRATE',SUM([26]) AS 'D26GRATE',SUM([27]) AS 'D27GRATE',SUM([28]) AS 'D28GRATE'
                               ,SUM([29]) AS 'D29GRATE',SUM([30]) AS 'D30GRATE',SUM([31]) AS 'D31GRATE' FROM 
                               (SELECT TEAMID,DAY(ReportDate) AS ReportDay, 
                                    CASE WHEN (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END)>=CAST(PYIELD AS FLOAT) THEN 5
                                    WHEN (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END) <CAST(PYIELD AS FLOAT)AND
                                    (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END)>= CAST(PYIELD AS FLOAT)-0.005 THEN 4       
                                    WHEN (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END) < CAST(PYIELD AS FLOAT)-0.005 AND
                                    (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END)>= CAST(PYIELD AS FLOAT)-0.01 THEN 3       
                                    WHEN (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END) < CAST(PYIELD AS FLOAT)-0.01  AND 
                                    (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END)>=CAST(PYIELD AS FLOAT)-0.03 THEN 2       
                                    WHEN (CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0
                                    ELSE CAST(SUM(AOUTPUT) AS FLOAT)/CAST(SUM(AOUTPUT)+SUM(ROUTPUT)+SUM(SOUTPUT) AS FLOAT) END) <CAST(PYIELD AS FLOAT)-0.03 THEN 0 END AS GOODRATE

                                FROM RMDD ";
                strSql += this.strTemp;
//                strSql += @" GROUP BY TEAMID,ReportDate,PYIELD) liming
//                                PIVOT(
//                                SUM(GOODRATE)
//                                FOR ReportDay IN 
//                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
//	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
//                               ,[29],[30],[31])) AS pvt
//	                           GROUP BY TEAMID ) AS B
//	                           ON A.ATEAMID=B.GTEAMID
//                               INNER JOIN 
//                        (SELECT TEAMID AS STEAMID, 
//                                SUM([1]) AS 'D1SRATE',SUM([2]) AS 'D2SRATE',SUM([3]) AS 'D3SRATE',SUM([4]) AS 'D4SRATE'
//                               ,SUM([5]) AS 'D5SRATE',SUM([6]) AS 'D6SRATE',SUM([7]) AS 'D7SRATE',SUM([8]) AS 'D8SRATE'
//                               ,SUM([9]) AS 'D9SRATE',SUM([10]) AS 'D10SRATE',SUM([11]) AS 'D11SRATE',SUM([12]) AS 'D12SRATE'
//                               ,SUM([13]) AS 'D13SRATE',SUM([14]) AS 'D14SRATE',SUM([15]) AS 'D15SRATE',SUM([16]) AS 'D16SRATE'
//                               ,SUM([17]) AS 'D17SRATE',SUM([18]) AS 'D18SRATE',SUM([19]) AS 'D19SRATE',SUM([20]) AS 'D20SRATE'
//                               ,SUM([21]) AS 'D21SRATE',SUM([22]) AS 'D22SRATE',SUM([23]) AS 'D23SRATE',SUM([24]) AS 'D24SRATE'
//                               ,SUM([25]) AS 'D25SRATE',SUM([26]) AS 'D26SRATE',SUM([27]) AS 'D27SRATE',SUM([28]) AS 'D28SRATE'
//                               ,SUM([29]) AS 'D29SRATE',SUM([30]) AS 'D30SRATE',SUM([31]) AS 'D31SRATE' FROM 
//                               (SELECT TEAMID,DAY(ReportDate) AS ReportDay, 
//                                CASE WHEN ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/2)<=CAST(PMSYIELD AS FLOAT) THEN 10
//                                     WHEN ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/2) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0001 AND CAST(PMSYIELD AS FLOAT)+0.004 THEN 8       
//			                         WHEN ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/2) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0041 AND CAST(PMSYIELD AS FLOAT)+0.008 THEN 6 
//			                        WHEN  ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/2) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0081 AND CAST(PMSYIELD AS FLOAT)+0.012 THEN 4
//                                    WHEN  ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/2) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0121 AND CAST(PMSYIELD AS FLOAT)+0.016 THEN 2
//			                        WHEN  ((CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(UPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(LPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(ICSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(FPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(BLSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(WBSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END
//				                           +CASE WHEN CAST(SUM(AOUTPUT) AS FLOAT)=0 THEN 0 
//                                                 ELSE CAST(SUM(TPSQTY) AS FLOAT)/CAST(SUM(AOUTPUT) AS FLOAT) END)/2)>0.016 THEN 0 END AS SCRAPRATE
//                                FROM RMDD ";
                strSql += @" GROUP BY TEAMID,ReportDate,PYIELD) liming
                                PIVOT(
                                SUM(GOODRATE)
                                FOR ReportDay IN 
                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
                               ,[29],[30],[31])) AS pvt
	                           GROUP BY TEAMID ) AS B
	                           ON A.ATEAMID=B.GTEAMID
                               INNER JOIN 
                        (
                        SELECT TEAMID AS STEAMID, 
                        SUM([1]) AS 'D1SRATE',SUM([2]) AS 'D2SRATE',SUM([3]) AS 'D3SRATE',SUM([4]) AS 'D4SRATE'
                        ,SUM([5]) AS 'D5SRATE',SUM([6]) AS 'D6SRATE',SUM([7]) AS 'D7SRATE',SUM([8]) AS 'D8SRATE'
                        ,SUM([9]) AS 'D9SRATE',SUM([10]) AS 'D10SRATE',SUM([11]) AS 'D11SRATE',SUM([12]) AS 'D12SRATE'
                        ,SUM([13]) AS 'D13SRATE',SUM([14]) AS 'D14SRATE',SUM([15]) AS 'D15SRATE',SUM([16]) AS 'D16SRATE'
                        ,SUM([17]) AS 'D17SRATE',SUM([18]) AS 'D18SRATE',SUM([19]) AS 'D19SRATE',SUM([20]) AS 'D20SRATE'
                        ,SUM([21]) AS 'D21SRATE',SUM([22]) AS 'D22SRATE',SUM([23]) AS 'D23SRATE',SUM([24]) AS 'D24SRATE'
                        ,SUM([25]) AS 'D25SRATE',SUM([26]) AS 'D26SRATE',SUM([27]) AS 'D27SRATE',SUM([28]) AS 'D28SRATE'
                        ,SUM([29]) AS 'D29SRATE',SUM([30]) AS 'D30SRATE',SUM([31]) AS 'D31SRATE' FROM 
                        (SELECT TEAMID,DAY(ReportDate) AS ReportDay, 
                            CASE 
                                WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0
                            WHEN    CAST(AOUTPUT AS FLOAT)>0 AND 
		                            ((CASE WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0 
                                             ELSE 
					                            (CAST(UPSQTY AS FLOAT)+  
					                             CAST(LPSQTY AS FLOAT)+ 
					                             CAST(ICSQTY AS FLOAT)+
					                            CAST(FPSQTY AS FLOAT)+
					                            CAST(BLSQTY AS FLOAT)+
					                            CAST(WBSQTY AS FLOAT)+
					                            CAST(TPSQTY AS FLOAT)
					                            )
					                            /CAST(AOUTPUT AS FLOAT) END
		                            )/
                            (
	                            CASE WHEN (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END)=0 THEN 1 ELSE 
                            (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END) END
                            )
                            )<=CAST(PMSYIELD AS FLOAT) THEN 10
                                 WHEN  CAST(AOUTPUT AS FLOAT)>0 AND 
		                            ((CASE WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0 
                                             ELSE 
					                            (CAST(UPSQTY AS FLOAT)+  
					                             CAST(LPSQTY AS FLOAT)+ 
					                             CAST(ICSQTY AS FLOAT)+
					                            CAST(FPSQTY AS FLOAT)+
					                            CAST(BLSQTY AS FLOAT)+
					                            CAST(WBSQTY AS FLOAT)+
					                            CAST(TPSQTY AS FLOAT)
					                            )
					                            /CAST(AOUTPUT AS FLOAT) END
		                            )/
                            (
	                            CASE WHEN (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END)=0 THEN 1 ELSE 
                            (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END) END
                            )
                            ) BETWEEN CAST(PMSYIELD AS FLOAT)+0.00000001 AND CAST(PMSYIELD AS FLOAT)+0.004 THEN 8       
                                 WHEN  CAST(AOUTPUT AS FLOAT)>0 AND 
		                            ((CASE WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0 
                                             ELSE 
					                            (CAST(UPSQTY AS FLOAT)+  
					                             CAST(LPSQTY AS FLOAT)+ 
					                             CAST(ICSQTY AS FLOAT)+
					                            CAST(FPSQTY AS FLOAT)+
					                            CAST(BLSQTY AS FLOAT)+
					                            CAST(WBSQTY AS FLOAT)+
					                            CAST(TPSQTY AS FLOAT)
					                            )
					                            /CAST(AOUTPUT AS FLOAT) END
		                            )/
                            (
	                            CASE WHEN (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END)=0 THEN 1 ELSE 
                            (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END) END
                            )
                            ) BETWEEN CAST(PMSYIELD AS FLOAT)+0.00400001 AND CAST(PMSYIELD AS FLOAT)+0.008 THEN 6 
                                WHEN   CAST(AOUTPUT AS FLOAT)>0 AND 
		                            ((CASE WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0 
                                             ELSE 
					                            (CAST(UPSQTY AS FLOAT)+  
					                             CAST(LPSQTY AS FLOAT)+ 
					                             CAST(ICSQTY AS FLOAT)+
					                            CAST(FPSQTY AS FLOAT)+
					                            CAST(BLSQTY AS FLOAT)+
					                            CAST(WBSQTY AS FLOAT)+
					                            CAST(TPSQTY AS FLOAT)
					                            )
					                            /CAST(AOUTPUT AS FLOAT) END
		                            )/
                            (
	                            CASE WHEN (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END)=0 THEN 1 ELSE 
                            (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END) END
                            )
                            ) BETWEEN CAST(PMSYIELD AS FLOAT)+0.00800001 AND CAST(PMSYIELD AS FLOAT)+0.012 THEN 4
                                WHEN   CAST(AOUTPUT AS FLOAT)>0 AND 
		                            ((CASE WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0 
                                             ELSE 
					                            (CAST(UPSQTY AS FLOAT)+  
					                             CAST(LPSQTY AS FLOAT)+ 
					                             CAST(ICSQTY AS FLOAT)+
					                            CAST(FPSQTY AS FLOAT)+
					                            CAST(BLSQTY AS FLOAT)+
					                            CAST(WBSQTY AS FLOAT)+
					                            CAST(TPSQTY AS FLOAT)
					                            )
					                            /CAST(AOUTPUT AS FLOAT) END
		                            )/
                            (
	                            CASE WHEN (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END)=0 THEN 1 ELSE 
                            (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END) END
                            )
                            ) BETWEEN CAST(PMSYIELD AS FLOAT)+0.0121 AND CAST(PMSYIELD AS FLOAT)+0.016 THEN 2
                                WHEN   CAST(AOUTPUT AS FLOAT)>0 AND 
		                            ((CASE WHEN CAST(AOUTPUT AS FLOAT)=0 THEN 0 
                                             ELSE 
					                            (CAST(UPSQTY AS FLOAT)+  
					                             CAST(LPSQTY AS FLOAT)+ 
					                             CAST(ICSQTY AS FLOAT)+
					                            CAST(FPSQTY AS FLOAT)+
					                            CAST(BLSQTY AS FLOAT)+
					                            CAST(WBSQTY AS FLOAT)+
					                            CAST(TPSQTY AS FLOAT)
					                            )
					                            /CAST(AOUTPUT AS FLOAT) END
		                            )/
                            (
	                            CASE WHEN (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END)=0 THEN 1 ELSE 
                            (CASE WHEN CAST(UPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(LPSQTY AS FLOAT)>0 then 1 else 0 END+ 
 	                            CASE WHEN CAST(ICSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(FPSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(BLSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(WBSQTY AS FLOAT)>0 then 1 else 0 END+
 	                            CASE WHEN CAST(TPSQTY AS FLOAT)>0 then 1 else 0 END) END
                            )
                            )>0.016 THEN 0 END AS SCRAPRATE 
                FROM 
                (SELECT TEAMID,ReportDate AS ReportDate,PMSYIELD,SUM(ISNULL(UPSQTY,0)) AS UPSQTY,SUM(ISNULL(LPSQTY,0)) AS LPSQTY,
                SUM(ISNULL(ICSQTY,0)) AS ICSQTY,SUM(ISNULL(FPSQTY,0)) AS FPSQTY,SUM(ISNULL(BLSQTY,0)) AS BLSQTY,SUM(ISNULL(WBSQTY,0)) AS WBSQTY,
                SUM(ISNULL(TPSQTY,0)) AS TPSQTY,SUM(ISNULL(AOUTPUT,0)) AS  AOUTPUT
                FROM RMDD  ";                   
                strSql += this.strTemp;
                strSql += @"  GROUP BY TEAMID,ReportDate,PMSYIELD )AA ) liming
                                PIVOT(
                                SUM(SCRAPRATE)
                                FOR ReportDay IN 
                               ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15]
	                           ,[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28]
                               ,[29],[30],[31])) AS pvt
	                           GROUP BY TEAMID) AS C
                               ON B.GTEAMID=C.STEAMID
                               ORDER BY TEAMID ";                

                dsResult = this.dbConn.GetDataSetBySql(strSql.ToString());
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
