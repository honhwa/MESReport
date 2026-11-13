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
    public class LineProductStatus
    {
        #region Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strFabId = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Fab ID
        /// </summary>
        public string FabId
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
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LineProductStatus()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 取得數據源
        /// </summary>
        /// <returns>數據源（DataSet）</returns>
        public DataSet GetDataSet()
        {
            try
            {
                if (this.strFabId == "")
                {
                    throw new Exception("缺失FabID！");
                }
                else
                {
                    string strSql = "SELECT MLINEM.LINEID, M1.SUBLINEID AS M1SUBLINEID, M1.PRODID AS M1PRODID, M1.SUMQTY AS M1SUMQTY, M2.SUBLINEID AS M2SUBLINEID, M2.PRODID AS M2PRODID, M2.SUMQTY AS M2SUMQTY, M3.SUBLINEID AS M3SUBLINEID, M3.PRODID AS M3PRODID, M3.SUMQTY AS M3SUMQTY FROM MLINEM LEFT JOIN (SELECT WLTM.LINEID, ISNULL(WLTM.SUBLINEID, '') AS SUBLINEID, ISNULL(WLTM.PRODID, '') AS PRODID, ISNULL(SUM(WLTM.ENTITYTQTY), 0) AS SUMQTY FROM New_WLTM WLTM WHERE WLTM.FABID = '" + this.strFabId + "' AND WLTM.CWORKCTR IN (SELECT WORKCTR AS CWORKCTR FROM MWCM WHERE MWCM.FABID = '" + this.strFabId + "' AND MWCM.WORKTYPE = 'M1' AND MWCM.SHOPCD = 'MODULE') AND WLTM.HIDEFLG = '0' AND WLTM.FABID = '" + this.strFabId + "' GROUP BY WLTM.LINEID, WLTM.SUBLINEID, WLTM.PRODID) AS M1 ON M1.LINEID = MLINEM.LINEID LEFT JOIN (SELECT WLTM.LINEID, ISNULL(WLTM.SUBLINEID, '') AS SUBLINEID, ISNULL(WLTM.PRODID, '') AS PRODID, ISNULL(SUM(WLTM.ENTITYTQTY), 0) AS SUMQTY FROM WLTM WHERE WLTM.FABID = '" + this.strFabId + "' AND WLTM.CWORKCTR IN (SELECT WORKCTR AS CWORKCTR FROM MWCM WHERE MWCM.FABID = '" + this.strFabId + "' AND MWCM.WORKTYPE = 'M2' AND MWCM.SHOPCD = 'MODULE') AND WLTM.HIDEFLG = '0' AND WLTM.FABID = '" + this.strFabId + "' GROUP BY WLTM.LINEID, WLTM.SUBLINEID, WLTM.PRODID) AS M2 ON M2.LINEID = MLINEM.LINEID LEFT JOIN (SELECT WLTM.LINEID, ISNULL(WLTM.SUBLINEID, '') AS SUBLINEID, ISNULL(WLTM.PRODID, '') AS PRODID, ISNULL(SUM(WLTM.ENTITYTQTY), 0) AS SUMQTY FROM New_WLTM WLTM WHERE WLTM.FABID = '" + this.strFabId + "' AND WLTM.CWORKCTR IN (SELECT WORKCTR AS CWORKCTR FROM MWCM WHERE MWCM.FABID = '" + this.strFabId + "' AND MWCM.WORKTYPE = 'M3' AND MWCM.SHOPCD = 'MODULE') AND WLTM.HIDEFLG = '0' AND WLTM.FABID = '" + this.strFabId + "' GROUP BY WLTM.LINEID, WLTM.SUBLINEID, WLTM.PRODID) AS M3 ON M3.LINEID = MLINEM.LINEID WHERE MLINEM.FABID = '" + this.strFabId + "'";
                    dsResult = this.dbConn.GetDataSetBySql(strSql);
                }
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetDataSet2()
        {
            string strLineSql = @"select A.LINEID AS LINEID from MSLM A ,MWCM B WHERE A.FABID='B' AND A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR
                                    AND A.SUBLINETYPE='M' AND B.WORKTYPE in ('M1','M2','M3') AND A.LINEID NOT IN ('BMXX01')
                                    GROUP BY A.LINEID ORDER BY A.LINEID";
            string strM1Sql = "select lineid,sublineid,prodid,sum(sumqty) AS SUMQTY from( SELECT MSLM.LINEID,CASE WHEN WLTM.ENTITYSTS='W' THEN MSLM.SUBLINEID ELSE WLTM.SUBLINEID END AS SUBLINEID,WLTM.PRODID,SUM(WLTM.ENTITYTQTY) AS SUMQTY FROM New_WLTM  WLTM LEFT JOIN MSLM ON WLTM.FABID = '" + this.strFabId + "' AND WLTM.LINEID = MSLM.LINEID AND WLTM.ENTITYSTS IN ('R','W') AND MSLM.WORKCTR=WLTM.CWORKCTR AND WLTM.CWORKCTR IN (SELECT WORKCTR AS CWORKCTR FROM MWCM WHERE MWCM.FABID = '" + this.strFabId + "' AND MWCM.WORKTYPE = 'M1' AND MWCM.SHOPCD = 'MODULE') AND WLTM.HIDEFLG = '0' WHERE MSLM.FABID = '" + this.strFabId + "' GROUP BY  MSLM.LINEID,WLTM.ENTITYSTS, MSLM.SUBLINEID,WLTM.SUBLINEID,WLTM.PRODID)A group by lineid,sublineid,prodid ORDER BY lineid,sublineid,prodid";
            string strM2Sql = "select lineid,sublineid,prodid,sum(sumqty) AS SUMQTY from(SELECT MSLM.LINEID,CASE WHEN WLTM.ENTITYSTS='W' THEN MSLM.SUBLINEID ELSE WLTM.SUBLINEID END AS SUBLINEID,WLTM.PRODID,SUM(WLTM.ENTITYTQTY) AS SUMQTY FROM New_WLTM WLTM LEFT JOIN MSLM ON WLTM.FABID = '" + this.strFabId + "' AND WLTM.LINEID = MSLM.LINEID AND WLTM.ENTITYSTS IN ('R','W') AND MSLM.WORKCTR=WLTM.CWORKCTR AND WLTM.CWORKCTR IN (SELECT WORKCTR AS CWORKCTR FROM MWCM WHERE MWCM.FABID = '" + this.strFabId + "' AND MWCM.WORKTYPE = 'M2' AND MWCM.SHOPCD = 'MODULE') AND WLTM.HIDEFLG = '0' WHERE MSLM.FABID = '" + this.strFabId + "' GROUP BY  MSLM.LINEID,WLTM.ENTITYSTS, MSLM.SUBLINEID,WLTM.SUBLINEID,WLTM.PRODID )A group by lineid,sublineid,prodid ORDER BY lineid,sublineid,prodid";
            string strM3Sql = "select lineid,sublineid,prodid,sum(sumqty) AS SUMQTY from(SELECT MLINEM.LINEID,CASE WHEN WLTM.ENTITYSTS='W' THEN '' ELSE WLTM.SUBLINEID END AS SUBLINEID,WLTM.PRODID,SUM(WLTM.ENTITYTQTY) AS SUMQTY FROM New_WLTM WLTM LEFT JOIN MLINEM ON WLTM.FABID = '" + this.strFabId + "' AND WLTM.LINEID = MLINEM.LINEID AND WLTM.ENTITYSTS IN ('R','W') AND  WLTM.CWORKCTR IN (SELECT WORKCTR AS CWORKCTR FROM MWCM WHERE MWCM.FABID = '" + this.strFabId + "' AND MWCM.WORKTYPE = 'M3' AND MWCM.SHOPCD = 'MODULE') AND WLTM.HIDEFLG = '0' WHERE MLINEM.FABID = '" + this.strFabId + "' GROUP BY WLTM.ENTITYSTS, MLINEM.LINEID, WLTM.SUBLINEID, WLTM.PRODID)A group by lineid,sublineid,prodid ORDER BY lineid,sublineid,prodid";

            DataTable dtLine = this.dbConn.GetDataSetBySql(strLineSql).Tables[0];
            DataTable dtM1 = this.dbConn.GetDataSetBySql(strM1Sql).Tables[0];
            DataTable dtM2 = this.dbConn.GetDataSetBySql(strM2Sql).Tables[0];
            DataTable dtM3 = this.dbConn.GetDataSetBySql(strM3Sql).Tables[0];

            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable("Source");
            dt.Columns.Add(new DataColumn("LINEID"));
            dt.Columns.Add(new DataColumn("M1SUBLINEID"));
            dt.Columns.Add(new DataColumn("M1PRODID"));
            dt.Columns.Add(new DataColumn("M1SUMQTY"));
            dt.Columns.Add(new DataColumn("M2SUBLINEID"));
            dt.Columns.Add(new DataColumn("M2PRODID"));
            dt.Columns.Add(new DataColumn("M2SUMQTY"));
            dt.Columns.Add(new DataColumn("M3SUBLINEID"));
            dt.Columns.Add(new DataColumn("M3PRODID"));
            dt.Columns.Add(new DataColumn("M3SUMQTY"));
            

            foreach (DataRow dr in dtLine.Rows)
            {
                //DataRow drNew = dt.NewRow();
                //drNew["LINEID"] = dr["LINEID"].ToString();
                string strFilter = "LINEID = '" + dr["LINEID"].ToString() + "'";
                DataRow[] drM1s = dtM1.Select(strFilter);
                DataRow[] drM2s = dtM2.Select(strFilter);
                DataRow[] drM3s = dtM3.Select(strFilter);

                int rowNum = this.GetRowCount(drM1s.Length, drM2s.Length, drM3s.Length);

                //DataRow[] newRows = new DataRow[rowNum];
                for (int i = 0; i < rowNum; i++)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["LINEID"] = dr["LINEID"].ToString();
                    if (drM1s.Length > i)
                    {
                        drNew["M1SUBLINEID"] = drM1s[i]["SUBLINEID"].ToString();
                    }
                    else
                    {
                        drNew["M1SUBLINEID"] = DBNull.Value;
                    }
                    if (drM1s.Length > i)
                    {
                        drNew["M1PRODID"] = drM1s[i]["PRODID"].ToString();
                    }
                    else
                    {
                        drNew["M1PRODID"] = DBNull.Value;
                    }
                    if (drM1s.Length > i)
                    {
                        drNew["M1SUMQTY"] = drM1s[i]["SUMQTY"].ToString();
                    }
                    else
                    {
                        drNew["M1SUMQTY"] = DBNull.Value;
                    }

                    //if (drM2s[i]["SUBLINEID"] != null)
                    if (drM2s.Length > i)
                    {
                        drNew["M2SUBLINEID"] = drM2s[i]["SUBLINEID"].ToString();
                    }
                    else
                    {
                        drNew["M2SUBLINEID"] = DBNull.Value;
                    }
                    if (drM2s.Length > i)
                    {
                        drNew["M2PRODID"] = drM2s[i]["PRODID"].ToString();
                    }
                    else
                    {
                        drNew["M2PRODID"] = DBNull.Value;
                    }
                    if (drM2s.Length > i)
                    {
                        drNew["M2SUMQTY"] = drM2s[i]["SUMQTY"].ToString();
                    }
                    else
                    {
                        drNew["M2SUMQTY"] = DBNull.Value;
                    }

                    if (drM3s.Length > i)
                    {
                        drNew["M3SUBLINEID"] = drM3s[i]["SUBLINEID"].ToString();
                    }
                    else
                    {
                        drNew["M3SUBLINEID"] = DBNull.Value;
                    }
                    if (drM3s.Length > i)
                    {
                        drNew["M3PRODID"] = drM3s[i]["PRODID"].ToString();
                    }
                    else
                    {
                        drNew["M3PRODID"] = DBNull.Value;
                    }
                    if (drM3s.Length > i)
                    {
                        drNew["M3SUMQTY"] = drM3s[i]["SUMQTY"].ToString();
                    }
                    else
                    {
                        drNew["M3SUMQTY"] = DBNull.Value;
                    }

                    dt.Rows.Add(drNew);
                }
            }

            dsResult.Tables.Add(dt);

            return dsResult;

        }

        private int GetRowCount(int p_1, int p_2, int p_3)
        {
            int result = p_1;
            if (p_2 > result)
                result = p_2;
            if (p_3 > result)
                result = p_3;

            return result;
        }
        #endregion
    }
}
