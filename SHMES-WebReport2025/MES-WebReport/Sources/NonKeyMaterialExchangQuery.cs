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
    public class NonKeyMaterialExchangQuery
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
        private List<string> verbList = new List<string>();

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

        public List<string> VerbList
        {
            get
            {
                return this.verbList;
            }
            set
            {
                this.verbList = value;
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
        public NonKeyMaterialExchangQuery()
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

            strSql.Append (@"SELECT A.TRANSDATE AS TRANSDATE,A.PARTID AS PARTID,A.PARTTYPE AS PARTTYPE,");
            strSql.Append ("B.PARTNAME AS PARTNAME,A.PRODID AS PRODID,A.CSTOCKID AS CSTOCKID,");
            strSql.Append("A.NSTOCKID AS NSTOCKID,A.PARTQTY AS PARTQTY,");
	        strSql.Append ("CASE WHEN VERB='W2MC' THEN '庫房發料' ");
			strSql.Append ("WHEN VERB='MC2S' THEN '發到產線' ");
			strSql.Append ("WHEN VERB='S2MC' THEN '產線退回' ");
			strSql.Append ("WHEN VERB='MC2Z' THEN '發到站點' ");
			strSql.Append ("WHEN VERB='Z2MC' THEN '站點退回' ");
			strSql.Append ("WHEN VERB='MC2W' THEN 'NONKEY繳庫' ");
		    strSql.Append ("ELSE '' END AS TRANSTYPE,");	
	        strSql.Append ("CASE WHEN VERB='S2MC' THEN CSUBLINEID ");
		    strSql.Append ("WHEN VERB='MC2S' THEN NSUBLINEID ");
			strSql.Append ("WHEN VERB='MC2Z' THEN NSTOCKID "); 
            strSql.Append ("WHEN VERB='Z2MC' THEN CSTOCKID ");
			strSql.Append ("ELSE '' END AS SUBLINE,");
            strSql.Append ("A.SHIFTID AS SHIFTID,A.USERID AS USERID ");
            strSql.Append ("FROM WNKPAX A LEFT JOIN MNKPAM B ON ");
            strSql.Append ("A.FABID=B.FABID AND A.PARTID=B.PARTID ");

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
                TimeSpan ts = this.toTime - this.fromTime;
                if (ts.TotalSeconds > 1728000)
                {
                    throw new Exception("時間間隔超過20天！");
                }
                if (ts.Seconds < 0)
                {
                    throw new Exception("開始時間小於結束時！");
                }
                if (this.VerbList.Count <= 0)
                {
                    throw new Exception("缺少交易類型！");
                }

                strSql.Append(" WHERE A.FABID = '");
                strSql.Append(this.strFabID);
                strSql.Append("'");
                strSql.Append(" AND A.TRANSDATE BETWEEN Convert(Varchar,'");
                strSql.Append(this.fromTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                strSql.Append("',120) AND Convert(Varchar,'");
                strSql.Append(this.toTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                strSql.Append("',120) ");
                strSql.Append(" AND A.VERB IN (");
                strSql.Append(this.AdjustString(this.VerbList));
                strSql.Append(") ");
                
                foreach (string s in VerbList)
                {
                    switch (s)
                    {
                        case "MC2S":
                            if (this.SubLine!= "")
                            {
                                strSql.Append(" OR A.NSUBLINEID = '");
                                strSql.Append(this.SubLine);
                                strSql.Append("' ");
                            }
                            break;
                        case "S2MC":
                            if (this.SubLine!= "")
                            {
                                strSql.Append(" OR A.CSUBLINEID = '");
                                strSql.Append(this.SubLine);
                                strSql.Append("' ");
                            }
                            break;
                        case "MC2Z":
                            if (this.Workctr!= "")
                            {
                                strSql.Append(" OR A.NSTOCKID = '");
                                strSql.Append(this.Workctr);
                                strSql.Append("' ");
                            }
                            break;
                        case "Z2MC":
                            if (this.Workctr!= "")
                            {
                                strSql.Append(" OR A.CSTOCKID = '");
                                strSql.Append(this.Workctr);
                                strSql.Append("' ");
                            }
                            break;
                    }
                }

                if (this.ProdID != "")
                {
                    strSql.Append(" AND A.PRODID = '");
                    strSql.Append(this.ProdID);
                    strSql.Append("' ");
                }

                if (this.PartID != "")
                {
                    strSql.Append(" AND A.PARTID = '");
                    strSql.Append(this.PartID);
                    strSql.Append("' ");
                }

                strSql.Append(" ORDER BY A.TRANSDATE,A.VERB,A.PARTID,A.NSTOCKID");
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
