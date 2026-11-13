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
    public class ModuleMaterielScrapQuery
    {
        #region Parameters
        private string strSql = string.Empty;
        private string strFilter = string.Empty;

        private string strFabID = string.Empty;
        private string fromTime=string.Empty;
        private string toTime = string.Empty;
        private string strProdID = string.Empty;
        private List<string> partIDList = new List<string>();
        private string strSubLineID = string.Empty;

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

        public string FromTime
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

        public string ToTime
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

        public List<string> PartIDList
        {
            get
            {
                return this.partIDList;
            }
            set
            {
                this.partIDList = value;
            }
        }

        public string SubLine
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

        public string Sql
        {
            get
            {
                return this.strSql;
            }
        }
        #endregion

        #region Constructor
        public ModuleMaterielScrapQuery()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            string strSqlWPAX = @"SELECT WPAX.FABID,
                                         WPAX.PARTID,
                                         WPAX.PARTTYPE,
                                         CAST(WPAX.PARTQTY AS INT) AS PARTQTY,
                                         WPAX.ReasonCD,
                                         WPAX.CSTOCKID AS FRSTOCKID,
                                         WPAX.NStockID AS TOSTOCKID
                                    FROM New_WPAX WPAX ";
            string strSqlMPAM = @"SELECT MPAM.FABID,
                                         MPAM.PARTID,
                                         MPAM.PARTNAME FROM MPAM";

            string strSql = @"SELECT WPAX.FABID,
                                         WPAX.PARTID,
                                         WPAX.PARTTYPE,
                                         MPAM.PARTNAME,                                         
                                         WPAX.ReasonCD,
                                         WPAX.CSTOCKID AS FRSTOCKID,
                                         WPAX.NStockID AS TOSTOCKID,
                                         SUM(CAST(WPAX.PARTQTY AS INT)) AS PARTQTY
                                    FROM New_WPAX WPAX 
                                   INNER JOIN MPAM ON MPAM.FABID = WPAX.FABID AND WPAX.PARTID = MPAM.PARTID";

            DataSet dsMPAM = new DataSet();
            DataSet dsWPAX = new DataSet();
            DataSet dsResult;

            try
            {
                if (this.strFabID == "")
                {
                    throw new Exception("缺失FabID！");
                }
                if (this.fromTime == null)
                {
                    throw new Exception("缺失開始時間！");
                }
                if (this.toTime == null)
                {
                    throw new Exception("缺失結束時間！");
                }
                TimeSpan ts = Convert.ToDateTime(this.toTime) - Convert.ToDateTime(this.fromTime);
                if (ts.TotalSeconds > 2592000)
                {
                    throw new Exception("時間間隔超過一個月！");
                }
                if (ts.Seconds < 0)
                {
                    throw new Exception("開始時間小於結束時！");
                }
                //if (this.partIDList.Count <= 0)
                //{
                //    throw new Exception("缺失物料料號！");
                //}

                strSql = strSql + " WHERE WPAX.FABID = '" + this.strFabID + "'";

                //strSql += " AND WPAX.TRANSCDATE BETWEEN '" + this.fromTime.ToString() + "' AND '" + this.toTime.ToString() + "' ";
                //strSql += " AND WPAX.MFGDATE BETWEEN Convert(Varchar,'" + this.fromTime.ToString() + "',120) AND Convert(Varchar,'" + this.toTime.ToString() + "',120) ";
                strSql += " AND WPAX.TRANSDATE BETWEEN '" + this.fromTime.ToString() + "' AND '" + this.toTime.ToString() + "' ";
                strSql += " AND WPAX.VERB='MTRA' ";
                //strSql += " AND WPAX.LISTID NOT IN (SELECT LISTID FROM WPAX WHERE FABID = '" + this.strFabID + "' AND MFGDATE BETWEEN Convert(Varchar,'" + this.fromTime.ToString() + "',120) AND Convert(Varchar,'" + this.toTime.ToString() + "',120)  AND VERB='MREJ' AND CPARTMODE='S' ) ";
                strSql += " AND WPAX.LISTID NOT IN (SELECT LISTID FROM New_WPAX WPAX WHERE FABID = '" + this.strFabID + "' AND TRANSDATE BETWEEN '" + this.fromTime.ToString() + "' AND '" + this.toTime.ToString() + "'  AND VERB='MREJ' AND CPARTMODE='S' ) ";
                if (this.partIDList.Count > 0)
                {
                    strSql += " AND WPAX.PARTID IN (" + this.AdjustString(this.partIDList) + ") ";
                }

                string strGroupFilter = " GROUP BY WPAX.FABID,WPAX.PARTID,WPAX.PARTTYPE,MPAM.PARTNAME,WPAX.ReasonCD,WPAX.CSTOCKID,WPAX.NStockID ";
                if (this.strSubLineID != "")
                {
                    strSql += " AND WPAX.CSTOCKID = '" + this.FetchStockID() + "' ";
                    strSql += strGroupFilter;

                    dsResult = this.dbConn.GetDataSetBySql(strSql);
                }
                else
                {
                    /*
                    //strSql += " AND WPAX.CSTOCKID IN ()";
                    string strSql1 = strSql + this.AdjustFilter1() + strGroupFilter;
                    string strSql2 = strSql + this.AdjustFilter2() + strGroupFilter;
                    string strSql3 = strSql + this.AdjustFilter3() + strGroupFilter;

                    DataSet ds1 = this.dbConn.GetDataSetBySql(strSql1);
                    DataSet ds2 = this.dbConn.GetDataSetBySql(strSql2);
                    DataSet ds3 = this.dbConn.GetDataSetBySql(strSql3);

                    //ds1.Merge(ds2);
                    //ds1.Merge(ds3);

                    DataSet dsTmp = ds1.Copy();
                    dsTmp.Tables[0].Merge(ds2.Tables[0]);
                    dsTmp.Tables[0].Merge(ds3.Tables[0]);
                    */
                    strSql += " AND ((WPAX.CSTOCKID IN (SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='LS') AND WPAX.NSTOCKID IN (SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='MC') AND WPAX.NPARTMODE = 'S' AND WPAX.CPARTMODE='G' AND WPAX.CPARTSTATUS='R' AND WPAX.NPARTSTATUS='W') ";
                    strSql += " OR (WPAX.CSTOCKID IN (SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='LS') AND WPAX.NSTOCKID IN(SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='RS') AND WPAX.NPARTMODE = 'S' AND WPAX.CPARTMODE='G' AND WPAX.CPARTSTATUS='R' AND WPAX.NPARTSTATUS='R' ) ";
                    strSql += " OR (WPAX.CSTOCKID IN (SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='RS') AND WPAX.NSTOCKID IN (SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='WH') AND WPAX.NPARTMODE = 'S' AND WPAX.CPARTMODE='S' AND WPAX.CPARTSTATUS='R' AND WPAX.NPARTSTATUS='W' )) ";

                    strSql += strGroupFilter;
                    DataSet dsTmp = this.dbConn.GetDataSetBySql(strSql);
                    dsResult = dsTmp;
                }

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// User選擇SubLine的時候讀取相應的StockID
        /// </summary>
        /// <returns>StockID</returns>
        private string FetchStockID()
        {
            string strStockID = string.Empty;
            string strSqlTmp = " SELECT STOCKID FROM MSLM WHERE FABID='" + this.strFabID + "' AND SUBLINEID='" + this.strSubLineID + "' ";
            
            DataSet dsTmp = dbConn.GetDataSetBySql(strSqlTmp);
            if (dsTmp.Tables[0].Rows.Count > 0)
            {
                strStockID = dsTmp.Tables[0].Rows[0]["STOCKID"].ToString();
            }
            else
            {
                throw new Exception("查詢不到StockID！");
            }

            return strStockID;
        }
        /*
        /// <summary>
        /// SUBLINE報廢給發料中心
        /// </summary>
        /// <returns>Filter String</returns>
        private string AdjustFilter1()
        {
            string strCStockID = string.Empty;
            string strNStockID = string.Empty;
            
            string strSql1 = "SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='LS'";
            string strSql2 = "SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='MC'";

            try
            {
                DataTable dtCStockID = this.dbConn.GetDataSetBySql(strSql1).Tables[0];
                DataTable dtNStockID = this.dbConn.GetDataSetBySql(strSql2).Tables[0];

                foreach (DataRow dr in dtCStockID.Rows)
                {
                    strCStockID += "'" + dr["STOCKID"].ToString() + "'";
                }
                strCStockID = strCStockID.Replace("''", "','");

                if (dtNStockID.Rows.Count > 1 || dtNStockID.Rows.Count < 1)
                {
                    throw new Exception("條件錯誤！");
                }
                else
                {
                    strNStockID = dtNStockID.Rows[0]["STOCKID"].ToString();
                }
                string strSql = " AND WPAX.CSTOCKID IN (" + strCStockID + ") AND WPAX.NSTOCKID = '" + strNStockID + "' AND WPAX.NPARTMODE = 'S' AND WPAX.CPARTMODE='G' AND WPAX.CPARTSTATUS='R' AND WPAX.NPARTSTATUS='W' ";
                return strSql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重工拆解物料
        /// </summary>
        /// <returns>Filter String</returns>
        private string AdjustFilter2()
        {
            string strCStockID = string.Empty;
            string strNStockID = string.Empty;

            string strSql1 = "SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='LS'";
            string strSql2 = "SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='RS'";

            try
            {
                DataTable dtCStockID = this.dbConn.GetDataSetBySql(strSql1).Tables[0];
                DataTable dtNStockID = this.dbConn.GetDataSetBySql(strSql2).Tables[0];

                foreach (DataRow dr in dtCStockID.Rows)
                {
                    strCStockID += "'" + dr["STOCKID"].ToString() + "'";
                }
                strCStockID = strCStockID.Replace("''", "','");

                if (dtNStockID.Rows.Count > 1 || dtNStockID.Rows.Count < 1)
                {
                    throw new Exception("條件錯誤！");
                }
                else
                {
                    strNStockID = dtNStockID.Rows[0]["STOCKID"].ToString();
                }

                string strSql = " AND WPAX.CSTOCKID IN (" + strCStockID + ") AND WPAX.NSTOCKID = '" + strNStockID + "' AND WPAX.NPARTMODE = 'S' AND WPAX.CPARTMODE='G' AND WPAX.CPARTSTATUS='R' AND WPAX.NPARTSTATUS='R' ";
                return strSql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重工報廢繳庫區繳給
        /// </summary>
        /// <returns>Filter String</returns>
        private string AdjustFilter3()
        {
            string strCStockID = string.Empty;
            string strNStockID = string.Empty;

            string strSql1 = "SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='RS'";
            string strSql2 = "SELECT STOCKID FROM MSTOCKM WHERE FABID ='" + this.strFabID + "' AND MSTOCKM.STOCKTYPE='WH'";

            try
            {
                DataTable dtCStockID = this.dbConn.GetDataSetBySql(strSql1).Tables[0];
                DataTable dtNStockID = this.dbConn.GetDataSetBySql(strSql2).Tables[0];

                if (dtCStockID.Rows.Count > 1 || dtCStockID.Rows.Count < 1)
                {
                    throw new Exception("條件錯誤！");
                }
                else
                {
                    strCStockID = dtCStockID.Rows[0]["STOCKID"].ToString();
                }

                if (dtNStockID.Rows.Count > 1 || dtNStockID.Rows.Count < 1)
                {
                    throw new Exception("條件錯誤！");
                }
                else
                {
                    strNStockID = dtNStockID.Rows[0]["STOCKID"].ToString();
                }

                string strSql = " AND WPAX.CSTOCKID = '" + strCStockID + "' AND WPAX.NSTOCKID = '" + strNStockID + "' AND WPAX.NPARTMODE = 'S' AND WPAX.CPARTMODE='S' AND WPAX.CPARTSTATUS='R' AND WPAX.NPARTSTATUS='W' ";
                return strSql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
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
