using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Report.Library;
using System.Drawing;

namespace Report.Sources
{
    public class EQ24HRStatus
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strFabId = string.Empty;
        private DateTime transDate;
        private string strSubLine = string.Empty;
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
        /// TransDate
        /// </summary>
        public DateTime TransDate
        {
            get
            {
                return this.transDate;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EQ24HRStatus()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                if (this.strSubLine.Trim().Equals(string.Empty))
                {
                    throw new Exception("缺失SubLine！");
                }
                else
                {
                    //string strTimeSql = "SELECT TOP 1 TRANSDATE FROM REQSM5MIN ORDER BY TRANSDATE DESC";
                    //DataTable dtTime = this.dbConn.GetDataSetBySql(strTimeSql).Tables[0];
                    //if (dtTime.Rows.Count > 0)
                    //{
                    //    this.transDate = DateTime.Parse(dtTime.Rows[0]["TRANSDATE"].ToString());
                    //}
                    //else
                    //{
                    //    throw new Exception("缺失資料!");
                    //}

                    string strSql = @"SELECT REQSM5MIN.FABID,REQSM5MIN.SUBLINEID,REQSM5MIN.EQID,REQSM5MIN.EQCCD,MECCU.EQCCDCOLOR, REQSM5MIN.TRANSDATE FROM REQSM5MIN INNER JOIN MECCU ON MECCU.EQCCD = REQSM5MIN.EQCCD WHERE REQSM5MIN.FABID= '" + this.strFabId + "' AND REQSM5MIN.SUBLINEID = '" + this.strSubLine + "' ORDER BY REQSM5MIN.TRANSDATE DESC";
                    DataTable dtTmp = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                    if (dtTmp.Rows.Count > 0)
                    {
                        if (!DateTime.TryParse(dtTmp.Rows[0]["TRANSDATE"].ToString(), out this.transDate))
                        {
                            throw new Exception("時間轉換失敗!");
                        }
                    }

                    DataTable dtEQ = new DataTable();
                    dtEQ.Columns.Add("EQID", dtTmp.Columns["EQID"].DataType);

                    object lastValue = null;
                    foreach (DataRow dr in dtTmp.Select("", "EQID"))
                    {
                        if (lastValue == null || !(ColumnEqual(lastValue, dr["EQID"])))
                        {
                            lastValue = dr["EQID"];
                            dtEQ.Rows.Add(new object[] { lastValue });
                        }
                    }

                    DataSet dsSource = new DataSet();
                    DataTable dtSource = new DataTable();
                    dtSource.Columns.Add(new DataColumn("EQID"));
                    for (int i = 1; i <= 288; i++)
                    {
                        dtSource.Columns.Add(new DataColumn("S" + i.ToString()));
                    }
                    for (int i = 1; i <= 288; i++)
                    {
                        dtSource.Columns.Add(new DataColumn("T" + i.ToString()));
                    }

                    foreach (DataRow drEQID in dtEQ.Rows)
                    {
                        DataRow drNew = dtSource.NewRow();
                        drNew["EQID"] = drEQID["EQID"].ToString();
                        int j = 288;
                        foreach (DataRow drStatus in dtTmp.Select("EQID = '" + drEQID["EQID"].ToString() + "'"))
                        {
                            //drStatus["S" + j.ToString()].ToString();
                            drNew["S" + j.ToString()] = ColorTranslator.ToHtml(ColorTranslator.FromOle(int.Parse(drStatus["EQCCDCOLOR"].ToString())));
                            drNew["T" + j.ToString()] = drStatus["TRANSDATE"].ToString();
                            j--;
                        }
                        dtSource.Rows.Add(drNew);
                    }
                    this.dsResult = new DataSet();
                    this.dsResult.Tables.Add(dtSource);

                    return this.dsResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ColumnEqual(object objectA, object objectB)
        {
            if (objectA == DBNull.Value && objectB == DBNull.Value)
            {
                return true;
            }
            if (objectA == DBNull.Value || objectB == DBNull.Value)
            {
                return false;
            }
            return (objectA.Equals(objectB));
        }
        #endregion
    }
}
