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

/* 
 * 2014/2/17         SSC             V1.0                  Modify Columns show  
 * 2015/4/1          Runy            V1.1                  修改8180站的SHOP為CELL2
 */
namespace Report.Sources
{
    public class RMSP
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strFabId = string.Empty;
        private DateTime fromTime;
        private DateTime toTime;
        private List<string> PartIDList = new List<string>();
        private List<string> ProdIDList = new List<string>();
        private List<string> WonbrList=new List<string>() ;
        private List<string> ColumnsList = new List<string>();    //V1.0
        //V1.0 ADD Begin
        private bool ShowMfg = false;
        private bool ShowWoNum = false;
        private bool ShowWorkctr = false;
        private bool ShowDc= false;
        private bool ShowDd = false;
        private bool ShowBip = false;
        private bool ShowComp = false;
        //V1.0 ADD End
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

        public List<string> ListPartID
        {
            get
            {
                return this.PartIDList;
            }
            set
            {
                this.PartIDList = value;
            }
        }

        public List<string> ListProdID
        {
            get
            {
                return this.ProdIDList;
            }
            set
            {
                this.ProdIDList = value;
            }
        }

        public List<string> ListWonbr
        {
            get
            {
                return this.WonbrList;
            }
            set
            {
                this.ListWonbr = value;
            }
        }
        //V1.0 ADD Begin
        public List<string> ListColumns
        {
            get
            {
                return this.ColumnsList;
            }
            set
            {
                this.ColumnsList = value;
            }
        }
        //V1.0 ADD End
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public RMSP()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            //V1.0 ADD Begin
            foreach (string s in this.ColumnsList)
            {
                if (s == "MFGDATE")
                {
                    ShowMfg = true;
                }
                else if (s == "WO NUM")
                {
                    ShowWoNum = true;
                }
                else if (s == "WORKCTR")
                {
                    ShowWorkctr = true;
                }
                else if (s == "DEFECT CODE")
                {
                    ShowDc = true;
                }
                else if (s == "DefectDesc")
                {
                    ShowDd = true;
                }
                else if (s == "BIPAME PART")
                {
                    ShowBip = true;
                }
                else if (s == "COMP USAGE")
                {
                    ShowComp = true;
                }
            }
            //V1.0 ADD End
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
                /*
                string strCol = string.Empty;
                string strFilter1 = string.Empty;
                string strGroupFilter = string.Empty;

                //strFilter1 += " WHERE FABID = '" + this.strFabId + "' ";
                strFilter1 += " WHERE RMSP.FABID = '" + this.strFabId + "' ";

                if (this.PartIDList != null && this.PartIDList.Count > 0)
                {
                    StringBuilder sbPartID = new StringBuilder();
                    foreach (string s in this.PartIDList)
                    {
                        sbPartID.Append("'").Append(s).Append("'");
                    }
                    strFilter1 += " AND RMSP.SUBPARTID IN (" + sbPartID.ToString().Replace("''", "','") + ") ";
                }

                if (this.ProdIDList != null && this.ProdIDList.Count > 0)
                {
                    StringBuilder sbProdID = new StringBuilder();
                    foreach (string s in this.ProdIDList)
                    {
                        sbProdID.Append("'").Append(s).Append("'");
                    }
                    strFilter1 += " AND RMSP.PRODID IN (" + sbProdID.ToString().Replace("''", "','") + ") ";
                }

                if (this.WonbrList != null && this.WonbrList.Count > 0)
                {
                    StringBuilder sbWonbr = new StringBuilder();
                    foreach (string s in this.WonbrList)
                    {
                        sbWonbr.Append("'").Append(s).Append("'");
                    }
                    strFilter1 += " AND RMSP.WONBR IN (" + sbWonbr.ToString().Replace("''", "','") + ") ";
                }

                strFilter1 += "  AND (RMSP.OUTPUTQTY != 0 OR RMSP.SCRAPQTY != 0) ";

                strFilter1 += "  AND CONVERT(VARCHAR,RMSP.REPORTDATE ,120)  BETWEEN '" + this.fromTime.ToString("yyyy-MM-dd") + " 00:00:00' AND  '" + this.toTime.ToString("yyyy-MM-dd") + " 59:59:59'";

//                string strSql = @"
//                       SELECT CASE WHEN SHOPCD='MODULE' THEN 'NJSR06' WHEN SHOPCD='CELL2' THEN 'NJSR05' END AS DATA_YPE,
//                        CONVERT(VARCHAR(10),TRANSDATE,120) AS RECORD_DATA,
//                        CONVERT(VARCHAR(10),REPORTDATE,120) AS MFGDATE,
//                        REPORTWEEK AS WEEK_NUM,'NP80'AS SITE,SHOPCD AS SHOP, F9NCNO AS FP_PART_NUM,
//                        T9NCNO AS TP_PART_NUM, PRODGROUP AS APPLICATION,PRODSIZE AS PART_SIZE,
//                        WONBR AS WO_NUM,LOTTYPE AS LOT_TYPE,
//                        CASE WHEN MTRFLG='M'THEN SUB9NCNO WHEN MTRFLG='P' THEN AFT2PRODID END AS COMP_PART ,
//                        CASE WHEN MTRFLG='P'THEN 'Panel' WHEN MTRFLG='M'THEN 'Material'END AS COPM_FLAG,
//                        LCFLAG AS LC_FLAG,PACKFLG AS PACK_FLAG,
//                        SUM(CASE WHEN MTRFLG='M' THEN 0 WHEN MTRFLG='P' THEN CAST(PNLQTY AS INT) END)AS UPS,
//                        CASE WHEN MTRFLG='M' THEN '' WHEN MTRFLG='P' THEN SUB9NCNO END AS BIPAME_PART,
//                        SUBPARTTYPE AS COMP_M_GROUP,USAGE AS COMP_USAGE  ,
//                        SUM(CASE WHEN MTRFLG='P'AND LOTTYPE IN('RS','RM') THEN 0 ELSE CAST(OUTPUTQTY AS INT) END) AS OUT_QTY,
//                        SUM(CAST(SCRAPQTY AS INT)) AS SCRAP_QTY FROM RMSP" + strFilter1;
//                         strSql += @" 
//                        GROUP BY CASE WHEN SHOPCD='MODULE' THEN 'NJSR06' WHEN SHOPCD='CELL2' THEN 'NJSR05'END,
//                        CONVERT(VARCHAR(10),TRANSDATE,120),CONVERT(VARCHAR(10),REPORTDATE,120),
//                        REPORTWEEK,SHOPCD , F9NCNO,T9NCNO , PRODGROUP ,PRODSIZE,WONBR ,
//						CASE WHEN MTRFLG='M' THEN '' WHEN MTRFLG='P' THEN SUB9NCNO END,
//                        LOTTYPE ,CASE WHEN MTRFLG='M'THEN SUB9NCNO WHEN MTRFLG='P' THEN AFT2PRODID END,
//						CASE WHEN MTRFLG='P'THEN 'Panel' WHEN MTRFLG='M'THEN 'Material'END ,
//                        LCFLAG ,SUB9NCNO ,PACKFLG,SUBPARTTYPE ,USAGE";
//                strSql += @" 
//                        ORDER BY SHOPCD,CONVERT(VARCHAR(10),TRANSDATE,120),CONVERT(VARCHAR(10),REPORTDATE,120),
//                        F9NCNO,T9NCNO,PRODGROUP ,PRODSIZE,WONBR ,SUB9NCNO ";
//                string strSql = @"
//                       SELECT CASE WHEN RMSP.SHOPCD='MODULE' THEN 'NJSR06' WHEN RMSP.SHOPCD='CELL2' THEN 'NJSR05' END AS DATA_YPE,
//                        CONVERT(VARCHAR(10),RMSP.TRANSDATE,120) AS RECORD_DATA,
//                        CONVERT(VARCHAR(10),RMSP.REPORTDATE,120) AS MFGDATE,
//                        RMSP.REPORTWEEK AS WEEK_NUM,'WD01'AS SITE,RMSP.SHOPCD AS SHOP, RMSP.F9NCNO AS FP_PART_NUM,
//                        RMSP.T9NCNO AS TP_PART_NUM, RMSP.PRODGROUP AS APPLICATION,RMSP.PRODSIZE AS PART_SIZE,
//                        RMSP.WONBR AS WO_NUM,RMSP.LOTTYPE AS WO_TYPE,
//                        WO.LOTTYPE AS LOT_TYPE,
//                        CASE WHEN RMSP.MTRFLG='M'THEN RMSP.SUB9NCNO WHEN RMSP.MTRFLG='P' THEN RMSP.AFT2PRODID END AS COMP_PART ,
//                        CASE WHEN RMSP.MTRFLG='P'THEN 'Panel' WHEN RMSP.MTRFLG='M'THEN 'Material'END AS COPM_FLAG,
//                        RMSP.LCFLAG AS LC_FLAG,RMSP.PACKFLG AS PACK_FLAG,
//                        SUM(CASE WHEN RMSP.MTRFLG='M' THEN 0 WHEN RMSP.MTRFLG='P' THEN CAST(RMSP.PNLQTY AS INT) END)AS UPS,
//                        CASE WHEN RMSP.MTRFLG='M' THEN '' WHEN RMSP.MTRFLG='P' THEN RMSP.SUB9NCNO END AS BIPAME_PART,
//                        RMSP.SUBPARTTYPE AS COMP_M_GROUP,RMSP.USAGE AS COMP_USAGE  ,
//                        SUM(CASE WHEN RMSP.MTRFLG='P'AND RMSP.LOTTYPE IN('RS','RM') THEN 0 ELSE CAST(RMSP.OUTPUTQTY AS INT) END) AS OUT_QTY,
//                        SUM(CAST(RMSP.SCRAPQTY AS INT)) AS SCRAP_QTY 
//                                 FROM RMSP
//                                  LEFT JOIN SWOM WO
//                                  ON RMSP.FABID = WO.FABID
//                                     AND RMSP.WONBR = WO.WONBR" + strFilter1;

                string strSql = @"
                    SELECT DATA_YPE,RECORD_DATA,MFGDATE,WEEK_NUM,SITE,SHOP,FP_PART_NUM,TP_PART_NUM,APPLICATION,PART_SIZE,WO_NUM,
                    WORKCTR,DEFECT_CODE,WO_TYPE,LOT_TYPE,COMP_PART ,COPM_FLAG,LC_FLAG,PACK_FLAG,UPS,BIPAME_PART,COMP_M_GROUP,
                    COMP_USAGE  ,OUT_QTY,SCRAP_QTY,PRESCRAP_QTY,ISNULL(SF.SOURCEFAB,'') AS SOURCE_FAB
                    FROM
                    (
                        SELECT RMSP.FabID,CASE WHEN RMSP.SHOPCD='MODULE' THEN 'NJSR06' WHEN RMSP.SHOPCD='CELL2' THEN 'NJSR05' END AS DATA_YPE,
                        CONVERT(VARCHAR(10),RMSP.TRANSDATE,120) AS RECORD_DATA,
                        CONVERT(VARCHAR(10),RMSP.REPORTDATE,120) AS MFGDATE,
                        RMSP.REPORTWEEK AS WEEK_NUM,'WD01'AS SITE,RMSP.SHOPCD AS SHOP, RMSP.F9NCNO AS FP_PART_NUM,
                        RMSP.T9NCNO AS TP_PART_NUM, RMSP.PRODGROUP AS APPLICATION,RMSP.PRODSIZE AS PART_SIZE,
                        RMSP.WONBR AS WO_NUM,RMSP.WORKCTR,RMSP.DEFECTCODE AS DEFECT_CODE,RMSP.LOTTYPE AS WO_TYPE,
                        WO.LOTTYPE AS LOT_TYPE,
                        CASE WHEN RMSP.MTRFLG='M'THEN CASE WHEN SUBSTRING(SUBPARTID,1,1)='R' THEN SUBSTRING(SUBPARTID,2,25) ELSE SUBPARTID END 
                             WHEN RMSP.MTRFLG='P' THEN RMSP.AFT2PRODID END AS COMP_PART ,
                        CASE WHEN RMSP.MTRFLG='P'THEN 'Panel' WHEN RMSP.MTRFLG='M'THEN 'Material'END AS COPM_FLAG,
                        RMSP.LCFLAG AS LC_FLAG,RMSP.PACKFLG AS PACK_FLAG,
                        SUM(CASE WHEN RMSP.MTRFLG='M' THEN 0 WHEN RMSP.MTRFLG='P' THEN CAST(RMSP.PNLQTY AS INT) END)AS UPS,
                        CASE WHEN RMSP.MTRFLG='M' THEN '' WHEN RMSP.MTRFLG='P' THEN RMSP.SUB9NCNO END AS BIPAME_PART,
                        RMSP.SUBPARTTYPE AS COMP_M_GROUP,RMSP.USAGE AS COMP_USAGE  ,
                        SUM(CASE WHEN RMSP.MTRFLG='P'AND RMSP.LOTTYPE IN('RS','RM') THEN 0 ELSE CAST(RMSP.OUTPUTQTY AS INT) END) AS OUT_QTY,
                        SUM(CAST(RMSP.SCRAPQTY AS INT)) AS SCRAP_QTY,SUM(CAST(RMSP.SCRAPQTY*0.01*ISNULL(RA.RATE,0) AS NUMERIC(18,2))) AS PRESCRAP_QTY
                                 FROM RMSP
                                  LEFT JOIN SWOM WO
                                  ON RMSP.FABID = WO.FABID
                                  AND RMSP.WONBR = WO.WONBR 
                                  LEFT JOIN MDCRATEM RA
                                  ON RMSP.FABID=RA.FABID 
                                  AND RMSP.REPORTYEAR=RA.REPORTYEAR 
                                  AND RMSP.REPORTMONTH=RA.REPORTMONTH
                                  AND RMSP.SHOPCD=RA.SHOPCD
                                  AND RMSP.DEFECTCODE=RA.DEFECTCODE" + strFilter1;

//                strSql += @" 
//                        GROUP BY CASE WHEN RMSP.SHOPCD='MODULE' THEN 'NJSR06' WHEN RMSP.SHOPCD='CELL2' THEN 'NJSR05'END,
//                        CONVERT(VARCHAR(10),RMSP.TRANSDATE,120),CONVERT(VARCHAR(10),RMSP.REPORTDATE,120),
//                        RMSP.REPORTWEEK,RMSP.SHOPCD , RMSP.F9NCNO,RMSP.T9NCNO , RMSP.PRODGROUP ,RMSP.PRODSIZE,RMSP.WONBR ,
//						CASE WHEN RMSP.MTRFLG='M' THEN '' WHEN RMSP.MTRFLG='P' THEN RMSP.SUB9NCNO END,
//                        RMSP.LOTTYPE ,WO.LOTTYPE,CASE WHEN RMSP.MTRFLG='M'THEN RMSP.SUB9NCNO WHEN RMSP.MTRFLG='P' THEN RMSP.AFT2PRODID END,
//						CASE WHEN RMSP.MTRFLG='P'THEN 'Panel' WHEN RMSP.MTRFLG='M'THEN 'Material'END ,
//                        RMSP.LCFLAG ,RMSP.SUB9NCNO ,RMSP.PACKFLG,RMSP.SUBPARTTYPE ,RMSP.USAGE";

                strSql += @" 
                        GROUP BY RMSP.FabID,CASE WHEN RMSP.SHOPCD='MODULE' THEN 'NJSR06' WHEN RMSP.SHOPCD='CELL2' THEN 'NJSR05'END,
                        CONVERT(VARCHAR(10),RMSP.TRANSDATE,120),CONVERT(VARCHAR(10),RMSP.REPORTDATE,120),
                        RMSP.REPORTWEEK,RMSP.SHOPCD , RMSP.F9NCNO,RMSP.T9NCNO , RMSP.PRODGROUP ,RMSP.PRODSIZE,RMSP.WONBR ,RMSP.WORKCTR,RMSP.DEFECTCODE,
						CASE WHEN RMSP.MTRFLG='M' THEN '' WHEN RMSP.MTRFLG='P' THEN RMSP.SUB9NCNO END,
                        RMSP.LOTTYPE ,WO.LOTTYPE,
                        CASE WHEN RMSP.MTRFLG='M'THEN CASE WHEN SUBSTRING(SUBPARTID,1,1)='R' THEN SUBSTRING(SUBPARTID,2,25) ELSE SUBPARTID END 
                             WHEN RMSP.MTRFLG='P' THEN RMSP.AFT2PRODID END ,
						CASE WHEN RMSP.MTRFLG='P'THEN 'Panel' WHEN RMSP.MTRFLG='M'THEN 'Material'END ,
                        RMSP.LCFLAG ,RMSP.SUB9NCNO ,RMSP.PACKFLG,RMSP.SUBPARTTYPE ,RMSP.USAGE
                    ) A
                    LEFT JOIN MSFM SF
                    ON (case when A.COPM_FLAG='Panel' THEN COMP_PART ELSE FP_PART_NUM END =SF.PRODID AND A.FABID = SF.FABID)";
                strSql += @" 
                        ORDER BY DATA_YPE,RECORD_DATA,MFGDATE,WEEK_NUM,SITE,SHOP,FP_PART_NUM,TP_PART_NUM,APPLICATION,PART_SIZE,WO_NUM,
                   WORKCTR,DEFECT_CODE,WO_TYPE,LOT_TYPE,COMP_PART ,COPM_FLAG,LC_FLAG,PACK_FLAG,UPS,BIPAME_PART,COMP_M_GROUP,COMP_USAGE ";
                */
        //V1.0 Mark Begin
//                string strSql = @"
//                SELECT  'NJSR' AS DATA_YPE,CONVERT(VARCHAR(30),A.TRANSDATE,120) AS RECORD_DATA,
//		                CONVERT(VARCHAR(10),A.REPORTDATE,120) AS MFGDATE,
//		                A.REPORTWEEK AS WEEK_NUM,
//		                F.FABERPCD AS SITE,
//		                A.SHOPCD AS SHOP,
//		                A.T9NCNO AS TP_PART_NUM,
//		                A.F9NCNO AS FP_PART_NUM,
//		                A.PRODID,
//		                A.PRODGROUP AS [APPLICATION],
//		                A.PRODSIZE AS PART_SIZE,
//		                A.WONBR AS WO_NUM,
//		                A.WORKCTR AS WORKCTR,
//		                A.DEFECTCODE AS DEFECT_CODE,
//		                ISNULL(B.DEFECTDESC,'') AS DEFECTDESC,
//		                A.LOTTYPE AS WO_TYPE,
//		                D.LOTTYPE AS LOT_TYPE,
//		                CASE WHEN A.MTRFLG='P' THEN A.AFT2PRODID ELSE A.SUBPARTID END AS COMP_PART,
//		                CASE WHEN A.MTRFLG='P'THEN 'PANEL' WHEN A.MTRFLG='M'THEN 'MATERIAL'END AS COPM_FLAG,
//		                A.LCFLAG AS LC_FLAG,
//		                A.PACKFLG AS PACK_FLAG,
//                        '' AS UPS,
//		                CASE WHEN A.MTRFLG='M' THEN '' WHEN A.MTRFLG='P' THEN A.SUB9NCNO END AS BIPAME_PART,
//		                A.SUBPARTTYPE AS COMP_M_GROUP,
//		                A.USAGE AS COMP_USAGE  ,
//		                SUM(CASE WHEN A.MTRFLG='P'AND A.LOTTYPE IN('RS','RM') THEN 0 ELSE CAST(A.OUTPUTQTY AS INT) END) AS OUT_QTY,
//		                SUM(CAST(A.SCRAPQTY AS INT)) AS SCRAP_QTY,
//		                CASE WHEN SUM(CAST(A.SCRAPQTY AS INT)) >= 0 THEN 'S' ELSE 'US' END AS SCRAP_TYPE,
//		                SUM(CASE WHEN A.MTRFLG = 'P' THEN A.SCRAPQTY ELSE 0 END) AS PANEL_SCRAP_QTY,
//		                SUM(CAST(A.SCRAPQTY*0.01*ISNULL(C.RATE,0) AS NUMERIC(18,2))) AS PRESCRAP_QTY,
                //		                ISNULL(E.SOURCEFAB,'') AS SOURCE_FAB  		               
         //V1.0 Mark End
                string strSql = @"SELECT ";
                        if (ShowMfg == true)
                        {
                             strSql += @" CONVERT(VARCHAR(10),A.REPORTDATE,120)  AS MFGDATE, ";
                        }
                        else
                        {
                            strSql += @" ''  AS MFGDATE, ";                          
                        }                        
		                strSql += @" F.FABERPCD AS SITE,
		                CASE WorkCtr WHEN '8180' THEN 'CELL2' ELSE A.SHOPCD END AS SHOP,
		                A.T9NCNO AS TP_PART_NUM,
		                A.F9NCNO AS FP_PART_NUM,
		                A.PRODID,
		                MP.PRODGROUP AS [APPLICATION],
		                A.PRODSIZE AS PART_SIZE, ";//V1.1 Modify A.SHOPCD AS SHOP-->CASE WorkCtr WHEN '8180' THEN 'CELL2' ELSE A.SHOPCD END AS SHOP
                       if (ShowWoNum == true)
                       {
                           strSql += @" A.WONBR AS WO_NUM, ";
                       }
                       else
                       {
                           strSql += @" '' AS WO_NUM, ";
                       }
                       if (ShowWorkctr == true)
                       {
                           strSql += @" A.WORKCTR AS WORKCTR, ";
                       }
                       else
                       {
                           strSql += @" '' AS WORKCTR, ";
                       }
                       if (ShowDc == true)
                       {
                           strSql += @"A.DEFECTCODE AS DEFECT_CODE, ";
                       }
                       else
                       {
                           strSql += @"'' AS DEFECT_CODE, ";
                       }
                       if (ShowDd == true)
                       {
                           strSql += @" ISNULL(B.DEFECTDESC,'') AS DEFECTDESC, ";
                       }
                       else
                       {
                           strSql += @" '' AS  DEFECTDESC, ";
                       }
                       strSql += @" A.LOTTYPE AS WO_TYPE,
		                D.LOTTYPE AS LOT_TYPE,
		                CASE WHEN A.MTRFLG='P' THEN A.AFT2PRODID ELSE A.SUBPARTID END AS COMP_PART,
		                CASE WHEN A.MTRFLG='P'THEN 'PANEL' WHEN A.MTRFLG='M'THEN 'MATERIAL'END AS COPM_FLAG,
		                A.LCFLAG AS LC_FLAG, ";
                       if (ShowBip == true)
                       {
                           strSql += @" CASE WHEN A.MTRFLG='M' THEN '' WHEN A.MTRFLG='P' THEN A.SUB9NCNO END AS BIPAME_PART, ";
                       }
                       else
                       {
                           strSql += @" '' AS BIPAME_PART, ";
                       }
                       strSql += @" A.SUBPARTTYPE AS COMP_M_GROUP, ";
                       if (ShowComp == true)
                       {
                           strSql += @" A.USAGE AS COMP_USAGE  , ";
                       }
                       else
                       {
                           strSql += @" '' AS COMP_USAGE  , ";
                       }
                       strSql += @" SUM(CASE WHEN A.MTRFLG='P'AND A.LOTTYPE IN('RS','RM') THEN 0 ELSE CAST(A.OUTPUTQTY AS INT) END) AS OUT_QTY,
		                SUM(CAST(A.SCRAPQTY AS INT)) AS SCRAP_QTY,
		                SUM(CAST(A.SCRAPQTY*0.01*ISNULL(C.RATE,0) AS NUMERIC(18,2))) AS PRESCRAP_QTY,
		                ISNULL(E.SOURCEFAB,'') AS SOURCE_FAB
                FROM [RMSP] A WITH(NOLOCK) ";
                strSql += @" LEFT JOIN [MDCM] B WITH(NOLOCK) ON(A.FabID = B.FABID AND A.DefectCode = B.DEFECTCD)  LEFT JOIN [MDCRATEM] C WITH(NOLOCK) ON(A.FabID = C.FABID AND A.REPORTYEAR = C.REPORTYEAR AND A.ReportMonth = C.ReportMonth AND A.DefectCode = C.DefectCode AND A.SHOPCD = C.SHOPCD) ";
                strSql += @" LEFT JOIN [MPIM] MP WITH(NOLOCK) ON(A.FabID = MP.FABID AND A.PRODID = MP.PRODID) ";
                strSql += @"  LEFT JOIN [New_SWOM] D WITH(NOLOCK) ON(A.FabID = D.FABID AND A.Wonbr = D.WONBR)
                LEFT JOIN [MSFM] E WITH(NOLOCK) ON(A.FabID = E.FABID AND CASE WHEN A.MTRFLG='P' THEN A.AFT2PRODID ELSE A.F9NCNO END = E.PRODID)
                LEFT JOIN [MFABM] F WITH(NOLOCK) ON(A.FabID = F.FABID) ";

                strSql += @"  WHERE A.FABID = '" + this.strFabId + "' ";
                strSql += @" AND CONVERT(VARCHAR,A.REPORTDATE ,120)  BETWEEN '" + this.fromTime.ToString("yyyy-MM-dd") + " 00:00:00' AND  '" + this.toTime.ToString("yyyy-MM-dd") + " 59:59:59'";
                if (this.PartIDList != null && this.PartIDList.Count > 0)
                {
                    StringBuilder sbPartID = new StringBuilder();
                    foreach (string s in this.PartIDList)
                    {
                        sbPartID.Append("'").Append(s).Append("'");
                    }
                    strSql += @" AND A.SUBPARTID IN (" + sbPartID.ToString().Replace("''", "','") + ") ";
                }

                if (this.ProdIDList != null && this.ProdIDList.Count > 0)
                {
                    StringBuilder sbProdID = new StringBuilder();
                    foreach (string s in this.ProdIDList)
                    {
                        sbProdID.Append("'").Append(s).Append("'");
                    }
                    strSql += @" AND A.PRODID IN (" + sbProdID.ToString().Replace("''", "','") + ") ";
                }

                if (this.WonbrList != null && this.WonbrList.Count > 0)
                {
                    StringBuilder sbWonbr = new StringBuilder();
                    foreach (string s in this.WonbrList)
                    {
                        sbWonbr.Append("'").Append(s).Append("'");
                    }
                    strSql += @" AND A.WONBR IN (" + sbWonbr.ToString().Replace("''", "','") + ") ";
                }
                strSql += @" AND (A.OUTPUTQTY != 0 OR A.SCRAPQTY != 0)  GROUP BY ";
                if (ShowMfg == true)
                {
                    strSql += @" CONVERT(VARCHAR(10),A.REPORTDATE,120), ";
                }
                strSql += @" F.FABERPCD,A.SHOPCD,A.T9NCNO,A.F9NCNO,A.PRODID,MP.PRODGROUP,A.PRODSIZE,A.WONBR,";
                //if (ShowWorkctr == true) V1.1 Mark  因為上面的查詢語句用到了WORKCTR，所以必須要放在GROUP BY 裡面
                //{ V1.1 Mark
                    strSql += @"A.WORKCTR,";
                //} V1.1 Mark
                if (ShowDc == true)
                {
                    strSql += @"A.DEFECTCODE,";
                }
                if (ShowDd == true)
                {
                    strSql += @"ISNULL(B.DEFECTDESC,''),";
                }
                strSql += @"A.LOTTYPE,D.LOTTYPE,CASE WHEN A.MTRFLG='P' THEN A.AFT2PRODID ELSE A.SUBPARTID END,
                CASE WHEN A.MTRFLG='P'THEN 'PANEL' WHEN A.MTRFLG='M'THEN 'MATERIAL'END,A.LCFLAG,";
                if (ShowBip == true)
                {
                    strSql += @"CASE WHEN A.MTRFLG='M' THEN '' WHEN A.MTRFLG='P' THEN A.SUB9NCNO END,";
                }
                strSql += @"A.SUBPARTTYPE,";
                if (ShowComp == true)
                {
                    strSql += @"A.USAGE,";
                }
                strSql += @"ISNULL(E.SOURCEFAB,'')";
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
