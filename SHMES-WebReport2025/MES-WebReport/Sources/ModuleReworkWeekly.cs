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
    public class ModuleReworkWeekly
    {
        private string strStartTime = "";
        private string strEndTime = "";
        private string strGroup = "";
        private string strProdID = "";
        private string strFabID = "";
        private bool blCheckBox1 = false;
        private DBConn dbConn;

        #region Public Property
        /// <summary>
        /// FabID
        /// </summary>
        /// 
        public bool CheckBox1all
        {
            get
            {
                return this.blCheckBox1;
            }
            set
            {
                this.blCheckBox1 = value;
            }
        }
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

        /// <summary>
        /// 開始時間
        /// </summary>
        public string StartTime
        {
            get
            {
                return this.strStartTime;
            }
            set
            {
                this.strStartTime = value;
            }
        }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string EndTime
        {
            get
            {
                return this.strEndTime;
            }
            set
            {
                this.strEndTime = value;
            }
        }

        /// <summary>
        /// 群組
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
        /// 產品號碼
        /// </summary>
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
        #endregion

        public ModuleReworkWeekly()
        {
            this.dbConn = new DBConn();
        }

        #region Public Methods
        /// <summary>
        /// 取得綁定ModuleReworkWeekly的資料源
        /// </summary>
        /// <returns></returns>
        public DataSet Read()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                dt = this.getDate().Copy();
                if (dt == null)
                {
                    return ds;
                }

                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 取得資料
        /// </summary>
        /// <returns></returns>
        private DataTable getDate()
        {
            string strFilter = "";
            string strFilter1 = "";
            string strFilter2 = "";
            string strSql = "";
            DataTable dt = new DataTable() ;
            List<string> list = new List<string>();
            string strSql1 = "";
            DataTable dt1 = new DataTable();
            List<string> list1 = new List<string>();
            try
            {
                strFilter = strFilter + ((this.ProdID == "") ? "" : " ");
                strSql = @" SELECT distinct SUBLINEID FROM MSLM  " +
                             " WHERE FABID='" + FabID + "'  AND WORkCTR IN (SELECT WORKCTR FROM MWCM WHERE FABID='" + FabID + "' " +
                             " AND WORKTYPE='MR' AND SHOPCD='MODULE') ";
                DataTable dtTmp = this.dbConn.GetDataSetBySql(strSql, "MSLM").Tables[0];
                if (dtTmp.Rows.Count == 0)
                {
                    return dt;
                }
                list = this.getSubline(dtTmp);

                strSql1 = @" SELECT WORkCTR FROM MWCM " +" WHERE FABID='" + FabID + "' " + " AND WORKTYPE='MR' AND SHOPCD='MODULE' ";
                DataTable dtTmp1 = this.dbConn.GetDataSetBySql(strSql1, "MWCM").Tables[0];
                if (dtTmp1.Rows.Count == 0)
                {
                    return dt1;
                }
                list1 = this.getWorkctr(dtTmp1);

                //strSql = " SELECT H.MFGDATE,H.TOPRODID,H.SHIFTID,H.CSUBLIEID,H.PUTTYPE,H.TOLOTGRADE,H.DEFECTCD,SUM(H.ENTITYQTY)AS ENTITYQTY FROM ( " +
                //                    " SELECT convert(char(10), MFGDATE,20) as MFGDATE ,SHIFTID,TOPRODID,TOLOTGRADE, " +
                //                   "  NSubLineID AS CSUBLIEID ,DEFECTCD, 'INPUT' AS PUTTYPE ,SUM(NENTITYTQTY) AS ENTITYQTY " +
                //                   "  FROM WLTX WHERE FABID='" + FabID + "'  AND VERB In ('MVIN','MTAH','MTUH' ) " +
                //                   "  AND NSubLineID in (" + list[0] + ") AND CWORKCTR in (" + list[1] + ") AND NWORKCTR in (" + list[1] + ")  AND TOPRODID IN (''," + this.ProdID + ") AND MFGDATE BETWEEN '" + this.strStartTime + "' AND '" + this.strEndTime + "' " +
                //                   " GROUP BY MFGDATE,SHIFTID,TOPRODID,TOLOTGRADE,NSubLineID ,DEFECTCD " +
                //                   " UNION SELECT convert(char(10), MFGDATE,20) as MFGDATE ,SHIFTID,TOPRODID,TOLOTGRADE, " +
                //                   " CSubLineID,DEFECTCD,'OUTPUT' AS PUTTYPE,SUM(NENTITYTQTY) AS ENTITYQTY " +
                //                   "  FROM WLTX WHERE FABID='" + FabID + "'  AND VERB  IN ('MTAH','MTUH')  " +
                //                   " AND CSubLineID IN (" + list[0] + ") AND CWORKCTR IN (" + list[1] + ") AND CWORKCTR=NWORKCTR  " +
                //                   " AND CSubLineID<>NSubLineID AND TOPRODID IN (''," + this.ProdID + ") AND MFGDATE BETWEEN '" + this.StartTime + "' AND '" + this.strEndTime + "' " +
                //                   " GROUP BY MFGDATE,SHIFTID,TOPRODID,TOLOTGRADE,CSubLineID,DEFECTCD " +
                //                   " UNION  SELECT convert(char(10), MFGDATE,20) as MFGDATE ,SHIFTID,TOPRODID,TOLOTGRADE," +
                //                   " CSUBLINEID,DEFECTCD,'OUTPUT' AS PUTTYPE ,SUM(NENTITYTQTY) AS ENTITYQTY " +
                //                   " FROM WLTX WHERE FABID='" + FabID + "' AND VERB='MVOU' AND CSUBLINEID IN (" + list[0] + ") " +
                //                   " AND CWORKCTR IN (" + list[1] + ") AND TOPRODID IN (''," + this.ProdID + ") AND TOLOTGRADE='A'  " +
                //                   " AND MFGDATE BETWEEN '" + this.StartTime + "' AND '" + this.strEndTime + "' " +
                //                   " GROUP BY MFGDATE,SHIFTID,TOPRODID,TOLOTGRADE,CSUBLINEID,DEFECTCD " +
                //                   " UNION  SELECT convert(char(10), MFGDATE,20) as MFGDATE,SHIFTID,PRODID AS TOPRODID,LOTGRADE AS TOLOTGRADE,CSUBLINEID ,DEFECTCD ,'OUTPUT' as PTYTYPE ,SUM(DEFECTQTY) AS ENTITYQTY" +
                //                   " FROM WLDX WHERE FABID='" + FabID + "' AND CSUBLINEID in (" + list[0] + ") AND CWORKCTR in (" + list[1] + ") AND LOTGRADE='S'  " +
                //                   " AND CSHOPCD='MODULE' AND PRODID IN (''," + this.ProdID + ") AND MFGDATE BETWEEN '" + this.StartTime + "' AND '" + this.strEndTime + "' " +
                //                   " GROUP BY MFGDATE,SHIFTID,PRODID,CSUBLINEID,LOTGRADE,DEFECTCD  )H " +
                //                   " GROUP BY H.MFGDATE,H.TOPRODID,H.SHIFTID,H.CSUBLIEID,H.PUTTYPE,H.TOLOTGRADE,H.DEFECTCD  ";
                if (this.blCheckBox1 == false)
                {
                    strFilter1 = " AND TOPRODID IN (" + this.ProdID + ") ";
                    strFilter2 = " AND B.PRODID IN (" + this.ProdID + ") ";
                }


                strSql = " SELECT H.mfgdate,H.TOPRODID,H.SHIFTID,H.CSUBLIEID,CASE WHEN H.PUTTYPE='INPUT' THEN '投入' ELSE '產出' END  AS PUTTYPE,H.TOLOTGRADE,H.DEFECTCD,SUM(H.ENTITYQTY) AS ENTITYQTY FROM ( " +
                                    " SELECT CONVERT(VARCHAR(10),transdate,121) as MFGDATE ,SHIFTID,TOPRODID,TOLOTGRADE, " +
                                   "  NSubLineID AS CSUBLIEID ,DEFECTCD, 'INPUT' AS PUTTYPE,  " +
                                   " (sum(case when VERB in ('MVIN') then NENTITYTQTY else 0 end)) AS ENTITYQTY " +
                                   //20091207 MARK BY TOMMY.HU  " sum(case when VERB in ('MTAH') AND NSubLineID<>CSubLineID then NENTITYTQTY else 0 end)+ " +
                                   //                           " sum(case when VERB in ('MTUH') AND NSubLineID<>CSubLineID then NENTITYTQTY else 0 end)) AS ENTITYQTY " +
                                   " FROM New_WLTX WHERE FABID='" + FabID + "'  " +
       //                           " AND ( (VERB ='MVIN') OR ( VERB In ('MTAH','MTUH') AND NSubLineID<>CSubLineID ))  "+
                                  " AND VERB ='MVIN' " +
                                   " AND NSubLineID in (" + list[0] + ") AND CWORKCTR in (" + list1[0] + ") AND NWORKCTR in (" + list1[0] + ")  " + strFilter1 + " AND TRANSDATE BETWEEN '" + this.strStartTime + "' AND '" + this.strEndTime + "' " +
                                   " GROUP BY CONVERT(VARCHAR(10),transdate,121),SHIFTID,TOPRODID,TOLOTGRADE,NSubLineID ,DEFECTCD " +
                                   " UNION SELECT CONVERT(VARCHAR(10),transdate,121) as MFGDATE ,SHIFTID,TOPRODID,TOLOTGRADE, " +
                                   " CSubLineID,DEFECTCD,'OUTPUT' AS PUTTYPE,SUM(NENTITYTQTY) AS ENTITYQTY " +
                                   "  FROM New_WLTX WHERE FABID='" + FabID + "'  AND VERB  IN ('MTAH','MTUH')  " +
                                   " AND CSubLineID IN (" + list[0] + ") AND CWORKCTR IN (" + list1[0] + ") AND CWORKCTR=NWORKCTR  " +
                                   " AND CSubLineID<>NSubLineID " + strFilter1 + " AND TRANSDATE BETWEEN '" + this.StartTime + "' AND '" + this.strEndTime + "' " +
                                   " GROUP BY CONVERT(VARCHAR(10),transdate,121),SHIFTID,TOPRODID,TOLOTGRADE,CSubLineID,DEFECTCD " +
                                   " UNION  "+
                                   " SELECT CONVERT(VARCHAR(10),transdate,121) as MFGDATE ,SHIFTID,TOPRODID,TOLOTGRADE," +
                                   " CSUBLINEID,DEFECTCD,'OUTPUT' AS PUTTYPE ,SUM(NENTITYTQTY) AS ENTITYQTY " +
                                   " FROM New_WLTX WHERE FABID='" + FabID + "' AND VERB='MVOU' AND CSUBLINEID IN (" + list[0] + ") " +
                                   " AND CWORKCTR IN (" + list1[0] + ") " + strFilter1 + " AND TOLOTGRADE='A'  " +
                                   " AND TRANSDATE BETWEEN '" + this.StartTime + "' AND '" + this.strEndTime + "' " +
                                   " GROUP BY CONVERT(VARCHAR(10),transdate,121),SHIFTID,TOPRODID,TOLOTGRADE,CSUBLINEID,DEFECTCD " +
                                   " UNION "+
                                   " SELECT CONVERT(VARCHAR(10),b.transdate,121) as MFGDATE,B.SHIFTID,B.PRODID AS TOPRODID," +
                                   " B.LOTGRADE AS TOLOTGRADE,B.CSUBLINEID AS CSUBLINEID,B.DEFECTCD AS DEFECTCD,'OUTPUT' as PTYTYPE, "+
                                   " SUM(B.DEFECTQTY) AS ENTITYQTY FROM New_WLTX A,New_WLDX B  " +
                                   "  WHERE B.FABID='" + FabID + "' AND B.CSUBLINEID in (" + list[0] + ") AND B.CWORKCTR in (" + list1[0] + ") AND B.LOTGRADE='S'  " +
                                   " AND B.CSHOPCD='MODULE' " + strFilter2 + " AND B.TRANSDATE BETWEEN '" + this.StartTime + "' AND '" + this.strEndTime + "' " +
                                   " AND A.FABID=B.FABID AND A.CSUBLINEID=B.CSUBLINEID AND A.TRANSID=B.TRANSID "+
                                   " AND A.CWORKCTR=B.CWORKCTR AND A.TOENTITYID=ENTITYID AND A.VERB='MVOU' AND A.TOLOTGRADE='S' "+
                                   " AND B.DEFECTQTY>0 "+
                                   " GROUP BY CONVERT(VARCHAR(10),b.transdate,121),B.SHIFTID,B.PRODID,B.CSUBLINEID,B.LOTGRADE,B.DEFECTCD  ) H " +
                                   " GROUP BY H.mfgdate,H.TOPRODID,H.SHIFTID,H.CSUBLIEID,H.PUTTYPE,H.TOLOTGRADE,H.DEFECTCD  ";
                dt = this.dbConn.GetDataSetBySql(strSql, "WLTX").Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 由傳入的Dt,傳囘作爲條件的List<>(1.SUBLINEID;2.WORkCTR)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<string> getSubline(DataTable dt)
        {
            List<string> strList = new List<string>();
            string strSublineID = "";
            try
            {
                foreach( DataRow dr in dt.Rows )
                {
                    strSublineID = strSublineID +","+ "'" + dr["SUBLINEID"].ToString() + "'";
                }
                strSublineID = strSublineID.Substring(1);
                strList.Add(strSublineID);
                return strList;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> getWorkctr(DataTable dt1)
        {
            List<string> strList = new List<string>();

            string strWorkCtr = "";
            try
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    strWorkCtr = strWorkCtr + "," + "'" + dr["WORkCTR"].ToString() + "'";
                }
                strWorkCtr = strWorkCtr.Substring(1);
                strList.Add(strWorkCtr);
                return strList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}


