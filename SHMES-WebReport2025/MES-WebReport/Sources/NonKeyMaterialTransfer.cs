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
    public class NonKeyMaterialTransfer
    {
        #region Parameters
        private StringBuilder strSql = new StringBuilder();

        private string strFabID = string.Empty;
        private DateTime fromTime;
        private DateTime toTime;
        private string strProdID = string.Empty;
        private string strSubLineID = string.Empty;
        private string strWorkctr = string.Empty;
        private string strPartID = string.Empty;

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

        public string PartID
        {
            get
            {
                return this.strPartID;
            }
            set
            {
                this.strPartID = value;
            }
        }

        #endregion

        #region Constructor
        public NonKeyMaterialTransfer()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            if (strSql.Length > 0)
            {
                strSql.Remove(1, strSql.Length);
            }

            strSql.Append(@"SELECT USERCTR,PARTID,PARTTYPE,PRODID,PARTNAME,UNIT,");
            strSql.Append("CAST(SUM(FRMCQTY) AS NUMERIC(20,2)) AS FRMCQTY,CAST(SUM(TOMCQTY) AS NUMERIC(20,2)) AS TOMCQTY,NONKEYTYPE,");
            strSql.Append("CAST(SUM(ACQTY) AS NUMERIC(20,2)) AS ACQTY,CAST(SUM(OUTQTY) AS NUMERIC(20,2)) AS OUTQTY FROM RNKTRX ");

            try
            {
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
                if (ts.TotalSeconds > 2592000)
                {
                    throw new Exception("時間間隔超過30天！");
                }
                if (ts.Seconds < 0)
                {
                    throw new Exception("開始時間小於結束時！");
                }

                strSql.Append(" WHERE FABID = '");
                strSql.Append(this.strFabID);
                strSql.Append("'");
                strSql.Append(" AND REPORTDATE BETWEEN Convert(Varchar,'");
                strSql.Append(this.fromTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                strSql.Append("',120) AND Convert(Varchar,'");
                strSql.Append(this.toTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                strSql.Append("',120) ");

                if (this.Workctr != "" && this.SubLine != "")
                {
                    strSql.Append(" AND USERCTR IN ('");
                    strSql.Append(this.Workctr);
                    strSql.Append("','");
                    strSql.Append(this.SubLine);
                    strSql.Append("')");
                }
                else
                {
                    if (this.Workctr != "" && this.SubLine == "")
                    {
                        strSql.Append(" AND USERCTR = '");
                        strSql.Append(this.Workctr);
                        strSql.Append("' ");
                    }

                    if (this.Workctr == "" && this.SubLine != "")
                    {
                        strSql.Append(" AND USERCTR = '");
                        strSql.Append(this.SubLine);
                        strSql.Append("' ");
                    }
                }

                if (this.ProdID != "")
                {
                    strSql.Append(" AND PRODID = '");
                    strSql.Append(this.ProdID);
                    strSql.Append("' ");
                }

                if (this.PartID != "")
                {
                    strSql.Append(" AND PARTID = '");
                    strSql.Append(this.PartID);
                    strSql.Append("' ");
                }

                strSql.Append(" GROUP BY USERCTR,PARTID,PARTTYPE,PRODID,PARTNAME,UNIT,NONKEYTYPE");
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
