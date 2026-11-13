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

namespace Report.Sources
{
    public class OQCRecord
    {
        private string strStartTime = "";
        private string strEndTime = "";
        private string strGroup = "";
        private string strProdID = "";
        private string strFabID = "";
        private string strSubLine = "";
        private string strWeek = "";
        private DBConn dbConn;

        #region Pulbic Property

        /// <summary>
        /// Week
        /// </summary>
        public string Week
        {
            get
            {
                return this.strWeek;
            }
            set
            {
                this.strWeek = value;
            }
        }


        /// <summary>
        /// SubLine
        /// </summary>
        public string SubLine
        {
            get
            {
                return this.strSubLine;
            }
            set
            {
                this.strSubLine = value;
            }
        }

        /// <summary>
        /// FabID
        /// </summary>
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

        public OQCRecord()
        {
            dbConn = new DBConn();
        }

        #region Public Methods
        /// <summary>
        /// 讀取ROQCS
        /// </summary>
        /// <returns></returns>
        public DataSet ReadROQCS()
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = this.getROQCS().Copy();
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        /// <summary>
        /// 讀取ROQC
        /// </summary>
        /// <returns></returns>
        public DataSet ReadROQC()
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = this.getROQC().Copy();
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ROQCS資料
        /// </summary>
        /// <returns></returns>
        private DataTable getROQCS()
        {
            DataTable dt = null;
            string strSql = "";
            string strFilter = "";
            try
            {
                strFilter = strFilter + ((this.ProdID == "") ? "" : " AND ProdID = '" + this.ProdID + "'  ");
                strFilter = strFilter + ((this.Group == "") ? "" : " AND ProdGroup = '" + this.Group + "' ");
                strFilter = strFilter + ((this.Week == "") ? "" : " AND ReportWeek = '" + this.Week + "' ");

                strSql = @" SELECT ProdGroup,CONVERT(VARCHAR(10),ReportDate ,23) AS MGDATE,SHIFTID ,ProdID,AOUTQTY,QTQTY,
                                CAST(CAST(NGQTY AS numeric(17, 7))* 1000000 / CAST(QTQTY AS numeric(17,7)) AS NUMERIC)    AS DPPM, 
                                CAST((CAST(CAST(NGCNT AS  numeric(17, 7))* 100 / CAST(QTCNT AS numeric(17,7))  AS NUMERIC)  ) AS VARCHAR)  + '%' AS LRR ,
                                NGQTY,NGCNT FROM ROQCS R " +
                                " WHERE R.FabID =  '" + this.FabID + "' AND ReportDate BETWEEN '" + StartTime + "' AND '" + EndTime + "' " + strFilter;

                dt = this.dbConn.GetDataSetBySql(strSql, "ROQCS").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// ROQC資料
        /// </summary>
        /// <returns></returns>
        private DataTable getROQC()
        {
            DataTable dt = null;
            string strSql = "";
            string strFilter = "";

            try
            {
                strFilter = strFilter + ((this.ProdID == "") ? "" : " AND ProdID = '" + this.ProdID + "'  ");
                strFilter = strFilter + ((this.Group == "") ? "" : " AND ProdGroup = '" + this.Group + "' ");
                strFilter = strFilter + ((this.Week == "") ? "" : " AND ReportWeek = '" + this.Week + "' ");
                strFilter = strFilter + ((this.SubLine == "") ? "" : " AND SUBLINEID = '" + this.SubLine + "'   ");

                strSql = @" SELECT ProdGroup,CONVERT(VARCHAR(10),ReportDate ,23) AS MGDATE,SHIFTID,ProdID,SubLineID,USERID + '   ' +USERNAME AS USERID ,NGENTITYID,
                                NGCARTID,OUTQTY,OQCQTY,DEFECTCD
                                FROM ROQC WHERE FABID = '" + this.FabID + "' " + " AND ReportDate BETWEEN '" + StartTime + "' AND '" + EndTime + "' " + strFilter;
                dt = this.dbConn.GetDataSetBySql(strSql, "ROQC").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        #endregion
    }
}
