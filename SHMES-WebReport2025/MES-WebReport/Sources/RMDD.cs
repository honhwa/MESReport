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
using Report.Reports;
using System.Collections.Generic;
using System.Text;

namespace Report.Sources
{
    public class BaseRMDD
    {
        #region para
        private DBConn dbcon;
        private string strFabID="";
        private string strReportDate="";
        private string strReportToDate = "";
        private string strProdID="";
        private string strShiftID="";
        private string strSubLineID="";
        private string strUserID="";
        private string strTeamID="";
        private string strLotType="";
        private string strTeamMode="";
        private string strFrMonth = ""; 
        private string strToMonth = "";
        private string strFrWeek = "";
        private string strToWeek = "";
        private string strProdGroup = "";
        private string strDataGroup = "";
        private string strDataFormat = "";
        private string strWorkctr = "";
        private List<string> ProdIDList = new List<string>();
        private List<string> SubLineIDList = new List<string>();

        #endregion

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

        public string ReportDate
        {
            get
            {
                return this.strReportDate;
            }
            set
            {
                this.strReportDate = value;
            }
        }

        public string ReportToDate
        {
            get
            {
                return this.strReportToDate;

            }
            set
            {
                this.strReportToDate = value;
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

        public string ShiftID
        {
            get
            {
                return this.strShiftID;
            }
            set
            {
                this.strShiftID = value;
            }

        }

        public string SubLineID
        {
            get
            {
                return this.strSubLineID;
            }
            set
            {
                this.strSubLineID = value;
            }
        }

        public string UserID
        {
            get
            {
                return this.strUserID;
            }
            set
            {
                this.strUserID = value;
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

        public string TeamMode
        {
            get
            {
                return this.strTeamMode;
            }
            set
            {
                this.strTeamMode = value;
            }

        }

        public string FrMonth
        {

            get
            {
                return this.strFrMonth;
            }
            set
            {
                this.strFrMonth = value;
            }
        }

        public string ToMonth
        {
            get
            {
                return this.strToMonth;
            }
            set
            {
                this.strToMonth = value;
            }
        }

        public string FrWeek
        {
            get
            {
                return this.strFrWeek;
            }
            set
            {
                this.strFrWeek = value;
            }
        }

        public string ToWeek
        {
            get
            {
                return this.strToWeek;
            }
            set
            {
                this.strToWeek = value;
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

        public string DataGroup
        {
            get
            {
                return this.strDataGroup;
            }
            set
            {
                this.strDataGroup = value;
            }
        }

        public string Workctr
        {
            get
            {

                return this.strWorkctr;
            }
            set
            {
                this.strWorkctr = value;

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

        public string DataFormat
        {
            get
            {
                return this.strDataFormat;
            }
            set
            {
                this.strDataFormat = value;
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

        public List<string> ListSubLineID
        {
            get
            {
                return this.SubLineIDList;
            }
            set
            {
                this.SubLineIDList = value;
            }
        }


        public BaseRMDD()
        {
            this.dbcon = new DBConn();
        }


        public DataSet GetRMDD()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;
                string strSqlFilter = string.Empty;
                string strFabID=string.Empty;

                //strSql = @" SELECT substring(convert(varchar,REPORTDATE,120),1,10) AS REPORTDATE,WORKCTR++'('+WORKNAME+')' as WORKCTR,CASE WHEN ISNULL(PRODGROUP,'')='' THEN '沒有機種群組' ELSE PRODGROUP END as PRODGROUP,TEAMMODE,TEAMID,SUBLINEID,PRODID, " +
                //    " LOTTYPE,ACTINPUT,AOUTPUT,CAST(WORKINGHOUR AS NUMERIC(17,2)) AS WORKINGHOUR,MANPOWER,CAST((WORKINGHOUR*MANPOWER)/10 AS NUMERIC(17,2)) AS MANPOWER1," +
                //    " STDINPUT,UPSQTY," +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(UPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4)) END AS UPSRATE,LPSQTY, " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(LPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4)) END AS LPSRATE,ICSQTY, " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(ICSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS ICSRATE,FPSQTY, " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(FPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS FPSRATE,BLSQTY, " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(BLSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS BLSRATE,WBSQTY, " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(WBSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))   AS NUMERIC(17,4)) END AS WBSRATE,TPSQTY, " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(TPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS TPSRATE  , " +
                //    " CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST((CAST(UPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(LPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(ICSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) +CAST(FPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+ CAST(BLSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(WBSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(TPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  ) AS NUMERIC(17,4))  END AS TMSRATE," +
                //    " ACTINPUT,AOUTPUT,SOUTPUT ," +
                //    " CASE WHEN ACTINPUT=0 THEN 0 ELSE CAST(CAST(SOUTPUT AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4))  AS NUMERIC(17,4))  END AS SRATE,SDEFECTCD,ROUTPUT, " +
                //    " CASE WHEN ACTINPUT=0 THEN 0 ELSE CAST(CAST(ROUTPUT AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS RRATE, " +
                //    " CASE WHEN ACTINPUT=0 THEN '0' ELSE  CAST(CAST((ACTINPUT-SOUTPUT-ROUTPUT)*100 AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4)) AS VARCHAR)+'%'   END AS YIELD, " +
                //    " CASE WHEN ACTINPUT+STDINPUT=0 THEN '' WHEN  ACTINPUT-STDINPUT>-50 THEN 'OK' ELSE CONVERT(VARCHAR,ACTINPUT-STDINPUT) END AS CAPALOSS " +
                //    " FROM RMDD WHERE FABID='" + this.strFabID.ToString().Trim() + "'  AND WORKCTR IN ('8180','8280','8380')  ";
            strSql = @"  SELECT substring(convert(varchar,REPORTDATE,120),1,10) AS REPORTDATE,
                        A.WORKCTR++'('+B.WORKNAME+')' as WORKCTR,
                        CASE WHEN ISNULL(PRODGROUP,'')='' THEN '沒有機種群組' ELSE PRODGROUP END as PRODGROUP,
                        TEAMMODE,SUBLINEID,TEAMID,PRODID,  LOTTYPE,STDINPUT,ACTINPUT,
                        CASE WHEN STDINPUT=0 THEN '0' ELSE CAST(CAST(CAST(ACTINPUT*100 AS NUMERIC)/CAST(STDINPUT AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS INPUTRATE,
                        CASE WHEN STDINPUT=0 THEN 'RED' WHEN CAST(CAST(ACTINPUT AS NUMERIC)/CAST(STDINPUT AS NUMERIC) AS NUMERIC(17,4))<1 THEN 'RED'  ELSE 'WHITE' END AS INPUTRATECOLOR, STDOUTPUT,AOUTPUT,
                        CASE WHEN STDOUTPUT=0 THEN '0' ELSE CAST(CAST(CAST(AOUTPUT*100 AS NUMERIC)/CAST(STDOUTPUT AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS OUTPUTRATE,
                        CASE WHEN STDOUTPUT=0 THEN 'RED'  
                        WHEN CAST(CAST(AOUTPUT AS NUMERIC)/CAST(STDOUTPUT AS NUMERIC) AS NUMERIC(17,4))<1 THEN 'RED'
                         ELSE 'WHITE' END AS OUTPUTRATECOLOR,CAST(WORKINGHOUR AS NUMERIC(17,2)) AS WORKINGHOUR,MANPOWER,
                        CAST((WORKINGHOUR*MANPOWER)/10 AS NUMERIC(17,2)) AS ACMANPOWER,
                        PMSYIELD,UPSQTY,CASE WHEN AOUTPUT=0 THEN '0' ELSE CAST(CAST(CAST(UPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,2)) AS VARCHAR) END AS UPSRATE,
                        CASE WHEN AOUTPUT=0 THEN 'RED' 
                        WHEN CAST(CAST(UPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>CAST(ISNULL(PlanMSYiled1,0) AS NUMERIC(17,4)) THEN 'RED' ELSE 'WHITE' END AS UPSRATECOLOR,
                         LPSQTY,CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(LPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4)) END AS LPSRATE,
                        CASE WHEN AOUTPUT=0 THEN 'RED' WHEN CAST(CAST(LPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>ISNULL(PlanMSYiled2,0) THEN 'RED' ELSE 'WHITE' END AS LPSRATECOLOR,
                         ICSQTY,CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(ICSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4)) END AS ICSRATE,
                         CASE WHEN AOUTPUT=0 THEN 'RED'  WHEN CAST(CAST(ICSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>ISNULL(PlanMSYiled1,0) THEN 'RED' ELSE 'WHITE' END AS ICSRATECOLOR,
                        FPSQTY, CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(FPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS FPSRATE,
                        CASE WHEN AOUTPUT=0 THEN 'RED'  WHEN CAST(CAST(FPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>ISNULL(PlanMSYiled2,0) THEN 'RED' 
                        ELSE 'WHITE' END AS FPSRATECOLOR, BLSQTY, CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(BLSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS BLSRATE,
                        CASE WHEN AOUTPUT=0 THEN 'RED' WHEN CAST(CAST(BLSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>ISNULL(PlanMSYiled1,0) THEN 'RED'  ELSE 'WHITE'  END AS BLSRATECOLOR,
                         WBSQTY,CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(WBSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))   AS NUMERIC(17,4)) END AS WBSRATE,
                        CASE WHEN AOUTPUT=0 THEN 'RED'  WHEN CAST(CAST(WBSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>ISNULL(PlanMSYiled2,0) THEN 'RED' ELSE 'WHITE'  END AS WBSRATECOLOR,
                         TPSQTY, CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST(CAST(TPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS TPSRATE ,
                        CASE WHEN AOUTPUT=0 THEN 'RED' 
                          WHEN CAST(CAST(TPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) AS NUMERIC(17,4))>ISNULL(PlanMSYiled3,0) THEN 'RED' ELSE 'WHITE'  END AS TPSRATECOLOR, 
                        CASE WHEN AOUTPUT=0 THEN 0 ELSE CAST((CAST(UPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(LPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(ICSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4)) +CAST(FPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+ CAST(BLSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(WBSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))+CAST(TPSQTY AS NUMERIC(17,4))/CAST(AOUTPUT AS NUMERIC(17,4))  ) AS NUMERIC(17,4))  END AS TMSRATE , SOUTPUT, 
                        CASE WHEN ACTINPUT=0 THEN 0 ELSE CAST(CAST(SOUTPUT AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4))  AS NUMERIC(17,4))  END AS SRATE,SDEFECTCD,ROUTPUT, CASE WHEN ACTINPUT=0 THEN 0 ELSE CAST(CAST(ROUTPUT AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4))  AS NUMERIC(17,4)) END AS RRATE, RDEFECTCD,  CASE WHEN ACTINPUT=0 THEN '0' ELSE  CAST(CAST(CAST((ACTINPUT-SOUTPUT-ROUTPUT)*100 AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4)) AS NUMERIC(17,2)) AS VARCHAR)+'%'   END AS YIELD,
                         CASE WHEN ACTINPUT=0 THEN 'RED' WHEN CAST(CAST((ACTINPUT-SOUTPUT-ROUTPUT)*100 AS NUMERIC(17,4))/CAST(ACTINPUT AS NUMERIC(17,4)) AS NUMERIC(17,2))<PYIELD THEN 'RED'
                         ELSE 'WHITE' END AS YIELDCOLOR,CAST(CAST(PYIELD AS NUMERIC(17,4)) AS VARCHAR)+'%' AS PYIELD, CASE WHEN ACTINPUT-STDINPUT>=0 THEN 'OK' ELSE CONVERT(VARCHAR,ACTINPUT-STDINPUT) END AS CAPALOSS,
                        CASE WHEN ACTINPUT-STDINPUT<0 THEN 'RED' ELSE  'WHITE' END AS CAPALOSSCOLOR  ";
                strSql += " FROM RMDD  A,MWCM B  WHERE A.FABID='" + this.strFabID.ToString().Trim() + "'  AND A.WORKCTR IN ('8180','8280','8380') AND A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR  "; 
                if ((strReportDate.ToString().Trim()!="") && (strReportToDate.ToString().Trim()!=""))
                {
                    strSql = strSql + " AND REPORTDATE BETWEEN '" + strReportDate.ToString().Trim() + "'  AND '" + strReportToDate.ToString().Trim() + "' ";
                }
                
                //if(strProdID.ToString().Trim()!="")
                //{
                //    strSql=strSql+ " AND PRODID='"+strProdID.ToString().Trim()+"' ";
                //}
                strSqlFilter = " AND 1=1 ";
                if (this.ProdIDList != null && this.ProdIDList.Count > 0)
                {
                    StringBuilder sbProdID = new StringBuilder();
                    foreach (string s in this.ProdIDList)
                    {
                        sbProdID.Append("'").Append(s).Append("'");
                    }
                    strSqlFilter += " AND PRODID IN (" + sbProdID.ToString().Replace("''", "','") + ") ";
                }

                if (this.strWorkctr.ToString().Trim() != "")
                {
                    strSqlFilter += " AND A.WORKCTR='" + strWorkctr.ToString().Trim() + "' ";
                }

                if ((strShiftID.ToString().Trim()!="") && (strShiftID.ToString().Trim()!= "ALL"))
                {
                    strSqlFilter = strSqlFilter + " AND SHIFTID='" + strShiftID.ToString().Trim() + "' ";
                }

                if (this.SubLineIDList != null && this.SubLineIDList.Count > 0)
                {
                    StringBuilder sbSUBLINEID = new StringBuilder();
                    foreach (string s in this.SubLineIDList)
                    {
                        sbSUBLINEID.Append("'").Append(s).Append("'");
                    }
                    strSqlFilter += " AND SUBLINEID IN (" + sbSUBLINEID.ToString().Replace("''", "','") + ") ";
                }

                //if(strSubLineID.ToString().Trim()!="")
                //{
                //    strSql=strSql+" AND SUBLINEID='"+strSubLineID.ToString().Trim()+"' ";
                //}

                //if (strUserID.ToString().Trim()!="")
                //{
                //    strSql=strSql+" AND REPORTUSERID='"+strUserID.ToString().Trim()+"' ";
                //}
                if (strTeamID.ToString().Trim()!="")
                {
                    strSqlFilter = strSqlFilter + " AND TEAMID='" + strTeamID.ToString().Trim() + "' ";
                }
                if (strLotType.ToString().Trim()!="")
                {
                    strSqlFilter = strSqlFilter + " AND LOTTYPE='" + strLotType.ToString().Trim() + "' ";
                }
                if ((strTeamMode.ToString().Trim()!="") && (strTeamMode.ToString().Trim()!="ALL") ) 
                {
                    strSqlFilter = strSqlFilter + "  AND TEAMMODE='" + strTeamMode.ToString().Trim() + "' ";  
                }
                if (strProdGroup.ToString().Trim() != "")
                {
                    strSqlFilter = strSqlFilter + "  AND PRODGROUP='" + strProdGroup.ToString().Trim() + "' ";
                }
                strSql = strSql+strSqlFilter + " ORDER BY REPORTDATE,A.WORKCTR,PRODGROUP,PRODID,TEAMMODE,TEAMID,LOTTYPE ";

                ds = this.dbcon.GetDataSetBySql(strSql);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetRMDDSMTABLE()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;
                strSql = @" SELECT substring(convert(varchar,REPORTDATE,120),1,10) AS REPORTDATE,WORKCTR+WORKNAME AS WORKCTR,TEAMMODE,TEAMID,SUM(UPSQTY) AS UPSQTY,"+
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(UPSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS UPSRATE,SUM(LPSQTY) AS LPSQTY, " +
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(LPSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS LPSRATE,SUM(ICSQTY) AS ICSQTY, " +
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(ICSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS ICSRATE,SUM(FPSQTY)  AS FPSQTY, " +
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(FPSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS FPSRATE,SUM(BLSQTY) AS BLSQTY, " +
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(BLSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS BLSRATE,SUM(WBSQTY) AS WBSQTY, " +
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(WBSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS WBSRATE,SUM(TPSQTY) AS TPSQTY, " +
                    " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(TPSQTY)*100 AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS TPSRATE " +
                    " FROM RMDD WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND WORKCTR IN ('8180','8280','8380') ";

                    if (strReportDate.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND REPORTDATE='" + strReportDate.ToString().Trim() + "' ";
                    }
                    
                    if (strProdID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND PRODID='" + strProdID.ToString().Trim() + "' ";
                    }
                    if (strShiftID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND SHIFTID='" + strShiftID.ToString().Trim() + "' ";
                    }
                    if (strSubLineID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND SUBLINEID='" + strSubLineID.ToString().Trim() + "' ";
                    }
                    if (strUserID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND REPORTUSERID='" + strUserID.ToString().Trim() + "' ";
                    }
                    if (strTeamID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND TEAMID='" + strTeamID.ToString().Trim() + "' ";
                    }
                    if (strLotType.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND LOTTYPE='" + strLotType.ToString().Trim() + "' ";
                    }
                    if ((strTeamMode.ToString().Trim() != "") && (strTeamMode.ToString().Trim() != "ALL"))
                    {
                        strSql = strSql + "  AND TEAMMODE='" + strTeamMode.ToString().Trim() + "' ";
                    }
                    if (strFrMonth.ToString().Trim() != "" && strToMonth.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND  CONVERT(VARCHAR,REPORTYEAR)+RIGHT('00'+CAST(REPORTMONTH AS VARCHAR),2) BETWEEN " + strFrMonth.ToString().Trim() + " AND " + strToMonth.ToString().Trim();
                    }

                    if (strFrWeek.ToString().Trim() != "" && strToWeek.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND  REPORTWEEK BETWEEN " + strFrWeek.ToString().Trim() + " AND " + strToWeek.ToString().Trim();
                    }
                    strSql = strSql + " GROUP BY REPORTDATE,TEAMMODE,TEAMID,WORKCTR,WORKNAME ";
                    strSql = strSql + " HAVING   SUM(AOUTPUT)>0 AND SUM(UPSQTY)+SUM(LPSQTY)+SUM(ICSQTY)+SUM(FPSQTY)+SUM(BLSQTY)+SUM(WBSQTY)+SUM(TPSQTY)>0 ";    

                ds = this.dbcon.GetDataSetBySql(strSql );
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetSql(string strSql1)
        {
            string strSql="";

                    strSql=strSql1;
                    if (strReportDate.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND REPORTDATE='" + strReportDate.ToString().Trim() + "' ";
                    }
                    
                    if (strProdID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND PRODID='" + strProdID.ToString().Trim() + "' ";
                    }
                    if (strShiftID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND SHIFTID='" + strShiftID.ToString().Trim() + "' ";
                    }
                    if (strSubLineID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND SUBLINEID='" + strSubLineID.ToString().Trim() + "' ";
                    }
                    if (strUserID.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND REPORTUSERID='" + strUserID.ToString().Trim() + "' ";
                    }
                    if ((strTeamID.ToString().Trim() != "") && (strTeamID.ToString().Trim() != "ALL"))
                    {
                        strSql = strSql + " AND TEAMID='" + strTeamID.ToString().Trim() + "' ";
                    }
                    if (strLotType.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND LOTTYPE='" + strLotType.ToString().Trim() + "' ";
                    }
                    if ((strTeamMode.ToString().Trim() != "") && (strTeamMode.ToString().Trim() != "ALL")) 
                    {
                        strSql = strSql + "  AND TEAMMODE='" + strTeamMode.ToString().Trim() + "' ";
                    }
                    if (strFrMonth.ToString().Trim() != "" && strToMonth.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND  CONVERT(VARCHAR,REPORTYEAR)+RIGHT('00'+CAST(REPORTMONTH AS VARCHAR),2) BETWEEN " + strFrMonth.ToString().Trim() + " AND " + strToMonth.ToString().Trim();
                    }

                    if (strFrWeek.ToString().Trim() != "" && strToWeek.ToString().Trim() != "")
                    {
                        strSql = strSql + " AND REPORTWEEK BETWEEN " + strFrWeek.ToString().Trim() + " AND " + strToWeek.ToString().Trim();
                    }
                    if (this.strWorkctr.ToString().Trim() != "")
                    {
                        strSql += " AND WORKCTR='" + strWorkctr.ToString().Trim() + "' ";
                    }
                    if (this.ProdGroup.ToString().Trim() != "")
                    {
                        strSql += " AND PRODGROUP='" + ProdGroup.ToString().Trim() + "' ";
                    }
                    

                    strSql = strSql + " AND WORKCTR IN ('8180','8280','8380') ";

            return strSql;

        }

        public DataSet GetRMDDSMCHART()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;
             //   strSql = @"SELECT REPORTDATE,RATE,TEAMMODE,TEAMID,PARTTYPE,PARTQTY FROM (";
             //   strSql=strSql + " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE, "+
             //           " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(UPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE , " +
             //           " TEAMMODE,TEAMID,'UP' AS PARTTYPE ,UPSQTY AS PARTQTY FROM RMDD "+
             //           " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND UPSQTY>0  AND WORKCTR IN ('8180','8280','8380') ";
             //  strSql=GetSql(strSql);
             //  strSql=strSql+" GROUP BY REPORTDATE,TEAMMODE,TEAMID,UPSQTY ";
             //  strSql = strSql + "HAVING  SUM(AOUTPUT)>0 ";
             //  strSql=strSql+" UNION ALL ";
             //  strSql = strSql + "  SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(LPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE ," +
             //               " TEAMMODE,TEAMID,'LP' AS PARTTYPE ,LPSQTY AS PARTQTY  FROM RMDD "+
             //               " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND  LPSQTY>0  AND WORKCTR IN ('8180','8280','8380') ";
             //  strSql=GetSql(strSql);
             //  strSql=strSql+" GROUP BY REPORTDATE,TEAMMODE,TEAMID,LPSQTY";
             //  strSql = strSql + " HAVING  SUM(AOUTPUT)>0 ";
             //  strSql = strSql + " UNION ALL ";
             //  strSql = strSql + " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(ICSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE , " +
             //               " TEAMMODE,TEAMID,'IC' AS PARTTYPE,ICSQTY AS PARTQTY FROM RMDD "+
             //                " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND ICSQTY>0 AND WORKCTR IN ('8180','8280','8380')  ";
             // strSql=GetSql(strSql);
             // strSql=strSql+"  GROUP BY REPORTDATE,TEAMMODE,TEAMID,ICSQTY ";
             // strSql = strSql + " HAVING  SUM(AOUTPUT)>0 ";
             // strSql=strSql+" UNION ALL "+
             //               " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(FPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE , " +
             //               " TEAMMODE,TEAMID,'FP' AS PARTTYPE,FPSQTY AS PARTQTY FROM RMDD "+
             //               " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND  FPSQTY>0 AND WORKCTR IN ('8180','8280','8380') ";
             // strSql=GetSql(strSql);
             // strSql=strSql+"  GROUP BY REPORTDATE,TEAMMODE,TEAMID,FPSQTY ";
             // strSql = strSql + " HAVING  SUM(AOUTPUT)>0 ";
             // strSql=strSql+" UNION ALL "+
             //                " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(BLSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE , " +
             //                " TEAMMODE,TEAMID,'BL' AS PARTTYPE,BLSQTY AS PARTQTY FROM RMDD "+
             //                " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND BLSQTY>0 AND WORKCTR IN ('8180','8280','8380') ";
             // strSql=GetSql(strSql);
             // strSql = strSql + "  GROUP BY REPORTDATE,TEAMMODE,TEAMID,BLSQTY  HAVING  SUM(AOUTPUT)>0" +
             //               " UNION ALL "+
             //               " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(WBSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE , " +
             //               " TEAMMODE,TEAMID,'WB' AS PARTTYPE,WBSQTY AS PARTQTY FROM RMDD "+
             //               " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND WBSQTY>0 AND WORKCTR IN ('8180','8280','8380') ";
             // strSql=GetSql(strSql);
             // strSql = strSql + "  GROUP BY REPORTDATE,TEAMMODE,TEAMID,WBSQTY  HAVING  SUM(AOUTPUT)>0" +
             //               " UNION ALL "+
             //               " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(TPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4)) AS VARCHAR) END AS RATE , " +
             //               " TEAMMODE,TEAMID,'TP' AS PARTTYPE,TPSQTY AS PARTQTY FROM RMDD "+
             //               " WHERE FABID='" + this.strFabID.ToString().Trim() + "' AND  TPSQTY>0 AND WORKCTR IN ('8180','8280','8380') ";

             //strSql=GetSql(strSql);
             //strSql = strSql + " GROUP BY REPORTDATE,TEAMMODE,TEAMID,TPSQTY  HAVING  SUM(AOUTPUT)>0";
             //strSql = strSql + " ) A ORDER BY REPORTDATE,TEAMMODE,TEAMID,PARTTYPE";   

                strSql = @"SELECT REPORTDATE,RATE,PARTTYPE FROM (";
                strSql = strSql + " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE, " +
                        " CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE  CAST(CAST(SUM(UPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4))  END AS RATE , " +
                        " 'UP' AS PARTTYPE  FROM RMDD " +
                        " WHERE FABID='" + this.strFabID.ToString().Trim() + "'   AND WORKCTR IN ('8180','8280','8380') ";
                strSql = GetSql(strSql);
                strSql = strSql + " GROUP BY REPORTDATE ";
                strSql = strSql + "HAVING  SUM(AOUTPUT)>0 ";
                strSql = strSql + " UNION ALL ";
                strSql = strSql + "  SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE  CAST(CAST(SUM(LPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4))  END AS RATE ," +
                             " 'LP' AS PARTTYPE  FROM RMDD " +
                             " WHERE FABID='" + this.strFabID.ToString().Trim() + "'   AND WORKCTR IN ('8180','8280','8380') ";
                strSql = GetSql(strSql);
                strSql = strSql + " GROUP BY REPORTDATE ";
                strSql = strSql + " HAVING  SUM(AOUTPUT)>0 ";
                strSql = strSql + " UNION ALL ";
                strSql = strSql + " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE  CAST(CAST(SUM(ICSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2))  END AS RATE , " +
                             " 'IC' AS PARTTYPE FROM RMDD " +
                              " WHERE FABID='" + this.strFabID.ToString().Trim() + "'  AND WORKCTR IN ('8180','8280','8380')  ";
                strSql = GetSql(strSql);
                strSql = strSql + "  GROUP BY REPORTDATE  ";
                strSql = strSql + " HAVING  SUM(AOUTPUT)>0 ";
                strSql = strSql + " UNION ALL " +
                              " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE  CAST(CAST(SUM(FPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2))  END AS RATE , " +
                              "  'FP' AS PARTTYPE  FROM RMDD " +
                              " WHERE FABID='" + this.strFabID.ToString().Trim() + "'   AND WORKCTR IN ('8180','8280','8380') ";
                strSql = GetSql(strSql);
                strSql = strSql + "  GROUP BY REPORTDATE  ";
                strSql = strSql + " HAVING  SUM(AOUTPUT)>0 ";
                strSql = strSql + " UNION ALL " +
                               " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE  CAST(CAST(SUM(BLSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2)) END AS RATE , " +
                               "  'BL' AS PARTTYPE  FROM RMDD " +
                               " WHERE FABID='" + this.strFabID.ToString().Trim() + "'  AND WORKCTR IN ('8180','8280','8380') ";
                strSql = GetSql(strSql);
                strSql = strSql + "  GROUP BY REPORTDATE  HAVING  SUM(AOUTPUT)>0" +
                              " UNION ALL " +
                              " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE  CAST(CAST(SUM(WBSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,2))  END AS RATE , " +
                              "  'WB' AS PARTTYPE  FROM RMDD " +
                              " WHERE FABID='" + this.strFabID.ToString().Trim() + "'   AND WORKCTR IN ('8180','8280','8380') ";
                strSql = GetSql(strSql);
                strSql = strSql + "  GROUP BY REPORTDATE   HAVING  SUM(AOUTPUT)>0" +
                              " UNION ALL " +
                              " SELECT SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE,CASE WHEN SUM(AOUTPUT)=0 THEN '0' ELSE CAST(CAST(SUM(TPSQTY) AS NUMERIC)/CAST(SUM(AOUTPUT) AS NUMERIC) AS NUMERIC(17,4))  END AS RATE , " +
                              "  'TP' AS PARTTYPE  FROM RMDD " +
                              " WHERE FABID='" + this.strFabID.ToString().Trim() + "'   AND WORKCTR IN ('8180','8280','8380') ";

                strSql = GetSql(strSql);
                strSql = strSql + " GROUP BY REPORTDATE   HAVING  SUM(AOUTPUT)>0";
                strSql = strSql + " ) A WHERE RATE>0 ORDER BY REPORTDATE,PARTTYPE";   

             ds = this.dbcon.GetDataSetBySql(strSql);
             return ds;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetMaterialChart()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;
                string strSqlHaving = string.Empty;
                strSql = " SELECT TEAMMODE, SUBSTRING(CONVERT(VARCHAR,REPORTDATE,120),1,10) AS REPORTDATE, ";
                switch(strDataGroup)
                {
                    case "0":
                        strSql=strSql+" TEAMID AS STRID, ";
                        break;
                    case "1":
                        strSql = strSql + " SUBLINEID AS STRID, ";
                        break;
                    case "2":
                        strSql = strSql + "  PRODID AS STRID,";
                        break;
                    case "3":
                        strSql = strSql + "  PRODGROUP AS STRID,";
                        break;                   
                }

                switch (strDataFormat)
                {
                    case "0":
                        strSql = strSql + "  SUM(ACTINPUT) AS QTY,CASE WHEN SUM(STDINPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(ACTINPUT)*100 AS NUMERIC)/CAST(SUM(STDINPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS RATE FROM RMDD ";
                        strSqlHaving = " HAVING SUM(ACTINPUT)>0 AND SUM(ACTINPUT)>0 ";
                        break;
                    case "1":
                        strSql = strSql + "   SUM(AOUTPUT) AS QTY,CASE WHEN SUM(STDOUTPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(AOUTPUT)*100 AS NUMERIC)/CAST(SUM(STDOUTPUT) AS NUMERIC) AS NUMERIC(17,2))  AS VARCHAR)+'%' END AS RATE FROM RMDD ";
                        strSqlHaving = " HAVING SUM(AOUTPUT)>0 AND SUM(STDOUTPUT)>0 ";
                        break;
                    case "2":  //Yield
                        strSql = strSql + "  SUM(AOUTPUT) AS QTY,CASE WHEN SUM(ACTINPUT)=0 THEN '0' ELSE  CAST(CAST(CAST((SUM(ACTINPUT)-SUM(SOUTPUT)-SUM(ROUTPUT))*100  AS NUMERIC)/CAST(SUM(ACTINPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%'  END AS RATE FROM RMDD ";
                        strSqlHaving = " HAVING SUM(AOUTPUT)>0 AND SUM(ACTINPUT)>0 ";
                        break;
                    case "3"://R
                        strSql = strSql + "  SUM(ROUTPUT) AS QTY,CASE WHEN SUM(ACTINPUT)=0 THEN '0' ELSE  CAST(CAST(CAST(SUM(ROUTPUT)*100 AS NUMERIC)/CAST(SUM(ACTINPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%'  END AS RATE FROM RMDD ";
                        strSqlHaving = " HAVING SUM(ROUTPUT)>0 AND SUM(ACTINPUT)>0 ";
                        break;
                    case "4"://S
                        strSql = strSql + "  SUM(SOUTPUT) AS QTY,CASE WHEN SUM(ACTINPUT)=0 THEN '0' ELSE CAST(CAST(CAST(SUM(SOUTPUT)*100 AS NUMERIC)/CAST(SUM(ACTINPUT) AS NUMERIC) AS NUMERIC(17,2)) AS VARCHAR)+'%' END AS RATE FROM RMDD ";
                        strSqlHaving = " HAVING SUM(SOUTPUT)>0 AND SUM(ACTINPUT)>0 ";
                        break;
                }
                strSql = strSql + " WHERE FABID='" + this.strFabID.ToString().Trim() + "' ";

                strSql = GetSql(strSql);

                switch (strDataGroup)
                {
                    case "0":
                        strSql = strSql + " GROUP BY TEAMMODE,TEAMID,REPORTDATE ";
                        strSql = strSql + strSqlHaving;
                        strSql = strSql + "  ORDER  BY REPORTDATE,TEAMMODE,TEAMID ";
                        break;
                    case "1":
                        strSql = strSql + " GROUP BY TEAMMODE,SUBLINEID,REPORTDATE ";
                        strSql = strSql + strSqlHaving;
                        strSql = strSql + "  ORDER  BY REPORTDATE,TEAMMODE,SUBLINEID ";
                        break;
                    case "2":
                        strSql = strSql + " GROUP BY TEAMMODE,PRODID,REPORTDATE ";
                        strSql = strSql + strSqlHaving;
                        strSql = strSql + "  ORDER  BY REPORTDATE,PRODID,TEAMMODE ";
                        break;
                    case "3":
                        strSql = strSql + " GROUP BY TEAMMODE,PRODGROUP,REPORTDATE ";
                        strSql = strSql + strSqlHaving;
                        strSql = strSql + "  ORDER  BY REPORTDATE,PRODGROUP,TEAMMODE ";
                        break;
                }



                ds = this.dbcon.GetDataSetBySql(strSql);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
