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
using System.Text;
using System.Drawing;

namespace Report.Sources
{
    public class SubLine24HRStatus
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strFabId = string.Empty;
        private DateTime transDate;
        private List<string> workCtrList = new List<string>();
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
        /// WorkCTR List
        /// </summary>
        public List<string> WorkCTRList
        {
            get
            {
                return this.workCtrList;
            }
            set
            {
                this.workCtrList = value;
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
        public SubLine24HRStatus()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                if (this.workCtrList.Count < 1)
                {
                    throw new Exception("缺失WorkCTR！");
                }
                else
                {
                    //string strTimeSql = "SELECT TOP 1 TRANSDATE FROM RESLS5MIN ORDER BY TRANSDATE DESC";
                    //DataTable dtTime = this.dbConn.GetDataSetBySql(strTimeSql).Tables[0];
                    //if (dtTime.Rows.Count > 0)
                    //{
                    //    this.transDate = DateTime.Parse(dtTime.Rows[0]["TRANSDATE"].ToString());
                    //}
                    //else
                    //{
                    //    throw new Exception("缺失資料!");
                    //}

                    StringBuilder sb = new StringBuilder();
                    foreach (string s in this.workCtrList)
                    {
                        sb.Append("'").Append(s).Append("'");
                    }
                    string strFilter = sb.ToString().Replace("''", "','");
                    string strSql = @"SELECT RESLS5MIN.FABID,RESLS5MIN.SUBLINEID,RESLS5MIN.EQCCD,MECCU.EQCCDCOLOR,RESLS5MIN.TRANSDATE FROM RESLS5MIN INNER JOIN MECCU ON MECCU.EQCCD = RESLS5MIN.EQCCD WHERE RESLS5MIN.FABID= '" + this.strFabId + "' AND RESLS5MIN.SUBLINEID IN(SELECT SUBLINEID FROM MSLM WHERE FABID = '" + this.strFabId + "' AND WORKCTR IN (" + strFilter + ")) ORDER BY RESLS5MIN.TRANSDATE DESC";

                    DataTable dtTmp = this.dbConn.GetDataSetBySql(strSql).Tables[0];

                    if (dtTmp.Rows.Count > 0)
                    {
                        if (!DateTime.TryParse(dtTmp.Rows[0]["TRANSDATE"].ToString(),out this.transDate))
                        {
                            throw new Exception("時間轉換失敗!");
                        }
                    }

                    DataTable dtSubLine = new DataTable();
                    dtSubLine.Columns.Add("SUBLINEID", dtTmp.Columns["SUBLINEID"].DataType);

                    object lastValue = null;
                    foreach (DataRow dr in dtTmp.Select("", "SUBLINEID"))
                    {
                        if (lastValue == null || !(ColumnEqual(lastValue, dr["SUBLINEID"])))
                        {
                            lastValue = dr["SUBLINEID"];
                            dtSubLine.Rows.Add(new object[] { lastValue });
                        }
                    }

                    DataSet dsSource = new DataSet();
                    DataTable dtSource = new DataTable();
                    dtSource.Columns.Add(new DataColumn("SUBLINEID"));
                    for (int i = 1; i <= 288; i++)
                    {
                        dtSource.Columns.Add(new DataColumn("S" + i.ToString()));
                    }
                    for (int i = 1; i <= 288; i++)
                    {
                        dtSource.Columns.Add(new DataColumn("T" + i.ToString()));
                    }

                    foreach (DataRow drSubLineID in dtSubLine.Rows)
                    {
                        DataRow drNew = dtSource.NewRow();
                        drNew["SUBLINEID"] = drSubLineID["SUBLINEID"].ToString();
                        int j = 288;
                        foreach (DataRow drStatus in dtTmp.Select("SUBLINEID = '" + drSubLineID["SUBLINEID"].ToString() + "'"))
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
