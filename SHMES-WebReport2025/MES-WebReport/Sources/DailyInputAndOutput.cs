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
using System.Drawing;

namespace Report.Sources
{
    public class BaseDailyInputAndOutput
    {
        private string strFabid = "";
        private string strGroup = "";
        private string strProdid = "";
        private string strSublineid = "";
        private string strMonth = "";
        private string strYear = "";
        private DBConn dbConn;

        public BaseDailyInputAndOutput()
        {
            dbConn = new DBConn();
        }

        #region Public Preperty

        /// <summary>
        /// ProdGroup
        /// </summary>
        public string ProdGroup
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
        /// Year
        /// </summary>
        public string Year
        {
            get
            {
                return this.strYear;
            }
            set
            {
                this.strYear = value;
            }
        }

        /// <summary>
        /// FabID
        /// </summary>
        public string FabID
        {
            get
            {
                return this.strFabid;
            }
            set
            {
                this.strFabid = value;
            }
        }

        /// <summary>
        /// ProdID
        /// </summary>
        public string ProdID
        {
            get
            {
                return this.strProdid;
            }
            set
            {
                this.strProdid = value;
            }
        }

        /// <summary>
        /// SubLineID
        /// </summary>
        public string SubLineID
        {
            get
            {
                return this.strSublineid;
            }
            set
            {
                this.strSublineid = value;
            }
        }

        /// <summary>
        /// 取得月份
        /// </summary>
        public string ReportMonth
        {
            get
            {
                return this.strMonth;
            }
            set
            {
                this.strMonth = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 讀取RPIO資料
        /// </summary>
        /// <returns></returns>
        public DataSet Read()
        {
            DataSet ds = new DataSet();
            DataSet dsNew = new DataSet();
            DataTable dt = null;
            DataTable dtComplete = null;
            string strColorSn = "";
            int num = 0;
            try
            {
                ds = this.dsSplit(this.GetRPIO());
                dtComplete = this.CreateTable();
                if (ds == null)
                {
                    return dsNew;
                }

                foreach (DataTable dtTmp in ds.Tables)
                {

                    for (int i = 0; i < 7; i++)
                    {
                        DataRow drTmp = dtComplete.NewRow();
                        dtComplete.Rows.Add(drTmp);
                        num++;
                    }
                    foreach (DataRow dr in dtTmp.Rows)
                    {
                        strColorSn = this.returnSn(dr["DATE"].ToString());
                        dtComplete.Rows[num - 7]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7]["SubLineID"] = dr["SubLineID"].ToString();
                        dtComplete.Rows[num - 7][dr["DATE"].ToString()] = Int32.Parse(dr["PlanOutPut"].ToString());
                        dtComplete.Rows[num - 7]["Indexer"] = "Plan outtput(A)";
                        dtComplete.Rows[num - 7 + 1][dr["DATE"].ToString()] = Int32.Parse(dr["ACTOutPut1"].ToString());
                        dtComplete.Rows[num - 7 + 1]["Indexer"] = "Act.output (M3)";
                        dtComplete.Rows[num - 7 + 2][dr["DATE"].ToString()] = Int32.Parse(dr["ACTOutPut2"].ToString());
                        dtComplete.Rows[num - 7 + 2]["Indexer"] = "Act.output (8700)";
                        dtComplete.Rows[num - 7 + 3][dr["DATE"].ToString()] = Int32.Parse(dr["ACTOutPut1"].ToString()) - Int32.Parse(dr["PlanOutPut"].ToString());
                        if (Int32.Parse( dtComplete.Rows[num - 7 + 3][dr["DATE"].ToString()].ToString()) < 0)
                        {
                            dtComplete.Rows[num - 7 + 3][strColorSn] = "Red";
                        }
                        dtComplete.Rows[num - 7 + 3]["Indexer"] = "Diff.(M3)-(A)";
                        dtComplete.Rows[num - 7 + 4][dr["DATE"].ToString()] = Int32.Parse(dr["ACTOutPut2"].ToString()) - Int32.Parse(dr["PlanOutPut"].ToString());
                        if (Int32.Parse( dtComplete.Rows[num - 7 + 4][dr["DATE"].ToString()].ToString() )< 0)
                        {
                            dtComplete.Rows[num - 7 + 4][strColorSn] = "Red";
                        }
                        dtComplete.Rows[num - 7 + 4]["Indexer"] = "Diff.(8700)-(A)";

                        if (Int32.Parse(dr["PlanOutPut"].ToString()) == 0)
                        {
                            dtComplete.Rows[num - 7 + 5][strColorSn] = "Green";
                            dtComplete.Rows[num - 7 + 5][dr["DATE"].ToString()] = Int32.Parse(dr["ACTOutPut1"].ToString());
                            dtComplete.Rows[num - 7 + 5]["Indexer"] = "achieve rate(M3/A)";

                            dtComplete.Rows[num - 7 + 6][strColorSn] = "Green";
                            dtComplete.Rows[num - 7 + 6][dr["DATE"].ToString()] = Int32.Parse(dr["ACTOutPut1"].ToString());
                            dtComplete.Rows[num - 7 + 6]["Indexer"] = "achieve rate(8700/A)";


                       }
                        else
                        {
                            dtComplete.Rows[num - 7 + 5][dr["DATE"].ToString()] =string.Format("{0:N}",(Double.Parse(dr["ACTOutPut1"].ToString()) / Double.Parse(dr["PlanOutPut"].ToString()) * 100)) + "%";
                            //if (Int32.Parse(dtComplete.Rows[num - 7 + 5][dr["DATE"].ToString()].ToString()) < 1)
                            if ((Int32.Parse(dr["ACTOutPut1"].ToString()) / Int32.Parse(dr["PlanOutPut"].ToString())) < 1)
                            {
                                dtComplete.Rows[num - 7 + 5][strColorSn] = "Red";
                            }
                            dtComplete.Rows[num - 7 + 5]["Indexer"] = "achieve rate(M3/A)";

                            dtComplete.Rows[num - 7 + 6][dr["DATE"].ToString()] = string.Format("{0:N}", (Double.Parse(dr["ACTOutPut2"].ToString()) / Double.Parse(dr["PlanOutPut"].ToString()) * 100)) + "%";
                            //if (Int32.Parse(dtComplete.Rows[num - 7 + 6][dr["DATE"].ToString()].ToString()) < 1)
                            if ((Int32.Parse(dr["ACTOutPut2"].ToString()) / Int32.Parse(dr["PlanOutPut"].ToString())) < 1)
                            {
                                dtComplete.Rows[num - 7 + 6][strColorSn] = "Red";
                            }
                            dtComplete.Rows[num - 7 + 6]["Indexer"] = "achieve rate(8700/A)";
                        }

                        dtComplete.Rows[num - 7+1]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7+1]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7+1]["SubLineID"] = dr["SubLineID"].ToString();
                        dtComplete.Rows[num - 7 + 2]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7 + 2]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7 + 2]["SubLineID"] = dr["SubLineID"].ToString();
                        dtComplete.Rows[num - 7 + 3]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7 + 3]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7 + 3]["SubLineID"] = dr["SubLineID"].ToString();
                        dtComplete.Rows[num - 7 + 4]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7 + 4]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7 + 4]["SubLineID"] = dr["SubLineID"].ToString();
                        dtComplete.Rows[num - 7 + 5]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7 + 5]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7 + 5]["SubLineID"] = dr["SubLineID"].ToString();
                        dtComplete.Rows[num - 7 + 6]["ProdGroup"] = dr["ProdGroup"].ToString();
                        dtComplete.Rows[num - 7 + 6]["PRODID"] = dr["PRODID"].ToString();
                        dtComplete.Rows[num - 7 + 6]["SubLineID"] = dr["SubLineID"].ToString();

                        

                        //dtComplete.Rows[num - 7]["COLOR"] = ColorTranslator.FromOle(10000);
                        //dtComplete.Rows[num - 7]["COLOR"] = ColorTranslator.ToHtml(ColorTranslator.FromOle(10000));
                        //dtComplete.Rows[num - 7]["COLOR"] = "Red";
                    }
                }
                dsNew.Tables.Add(dtComplete);
                return dsNew;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 取得RPIO的資料
        /// </summary>
        /// <returns></returns>
        private DataTable GetRPIO()
        {
            string strfilter = "";
            string strSql = "";
            string strTime = "";
            DataTable dt = null;
            try
            {
                strfilter = strfilter + ((FabID == "") ? "" : " AND FABID = '" + FabID + "' ");
                strfilter = strfilter + ((this.ProdGroup == "") ? "" : " AND ProdGroup = '" + this.ProdGroup + "' ");
                strfilter = strfilter + ((ProdID == "") ? "" : " AND  Prodid = '" + ProdID + "' ");
                strfilter = strfilter + ((SubLineID == "") ? "" : " AND SubLineID = '" + SubLineID + "' ");
                strTime = Year + "-" + ReportMonth;

                strSql = @"  SELECT CASE WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "' + '-01' THEN 'ID1' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-02' THEN 'ID2' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-03' THEN 'ID3' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-04' THEN 'ID4' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-05' THEN 'ID5' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-06' THEN 'ID6' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-07' THEN 'ID7' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-08' THEN 'ID8' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-09' THEN 'ID9' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-10' THEN 'ID10' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-11' THEN 'ID11' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-12' THEN 'ID12' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-13' THEN 'ID13' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-14' THEN 'ID14' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-15' THEN 'ID15' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-16' THEN 'ID16' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-17' THEN 'ID17' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-18' THEN 'ID18' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-19' THEN 'ID19' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-20' THEN 'ID20' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-21' THEN 'ID21' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-22' THEN 'ID22' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-23' THEN 'ID23' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-24' THEN 'ID24' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-25' THEN 'ID25' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-26' THEN 'ID26' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-27' THEN 'ID27' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-28' THEN 'ID28' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-29' THEN 'ID29' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-30' THEN 'ID30' " +
         " WHEN SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,10) = '" + strTime + "'+'-31' THEN 'ID31' " +
         " END AS DATE, 	PRODID, ACTOutPut1,PlanOutPut,ProdGroup,SubLineID,ACTOutPut2 " +
         " FROM RPIO WHERE  ReportMonth = '"+ReportMonth+"'  AND SUBSTRING( CONVERT( VARCHAR(100), ReportDate,21),1,4) = '" +Year + "' " +strfilter+
         " ORDER BY ProdGroup,PRODID,SubLineID,DATE ";
                dt = this.dbConn.GetDataSetBySql(strSql, "RPIO").Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 構造dt
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("ProdGroup", typeof(string));
                dt.Columns.Add("PRODID", typeof(string));
                dt.Columns.Add("SubLineID", typeof(string));
                dt.Columns.Add("Indexer", typeof(string));
                //dt.Columns.Add("ID1", typeof(int));
                //dt.Columns.Add("ID2", typeof(int));
                //dt.Columns.Add("ID3", typeof(int));
                //dt.Columns.Add("ID4", typeof(int));
                //dt.Columns.Add("ID5", typeof(int));
                //dt.Columns.Add("ID6", typeof(int));
                //dt.Columns.Add("ID7", typeof(int));
                //dt.Columns.Add("ID8", typeof(int));
                //dt.Columns.Add("ID9", typeof(int));
                //dt.Columns.Add("ID10", typeof(int));
                //dt.Columns.Add("ID11", typeof(int));
                //dt.Columns.Add("ID12", typeof(int));
                //dt.Columns.Add("ID13", typeof(int));
                //dt.Columns.Add("ID14", typeof(int));
                //dt.Columns.Add("ID15", typeof(int));
                //dt.Columns.Add("ID16", typeof(int));
                //dt.Columns.Add("ID17", typeof(int));
                //dt.Columns.Add("ID18", typeof(int));
                //dt.Columns.Add("ID19", typeof(int));
                //dt.Columns.Add("ID20", typeof(int));
                //dt.Columns.Add("ID21", typeof(int));
                //dt.Columns.Add("ID22", typeof(int));
                //dt.Columns.Add("ID23", typeof(int));
                //dt.Columns.Add("ID24", typeof(int));
                //dt.Columns.Add("ID25", typeof(int));
                //dt.Columns.Add("ID26", typeof(int));
                //dt.Columns.Add("ID27", typeof(int));
                //dt.Columns.Add("ID28", typeof(int));
                //dt.Columns.Add("ID29", typeof(int));
                //dt.Columns.Add("ID30", typeof(int));
                //dt.Columns.Add("ID31", typeof(int));
                dt.Columns.Add("ID1", typeof(string));
                dt.Columns.Add("ID2", typeof(string));
                dt.Columns.Add("ID3", typeof(string));
                dt.Columns.Add("ID4", typeof(string));
                dt.Columns.Add("ID5", typeof(string));
                dt.Columns.Add("ID6", typeof(string));
                dt.Columns.Add("ID7", typeof(string));
                dt.Columns.Add("ID8", typeof(string));
                dt.Columns.Add("ID9", typeof(string));
                dt.Columns.Add("ID10", typeof(string));
                dt.Columns.Add("ID11", typeof(string));
                dt.Columns.Add("ID12", typeof(string));
                dt.Columns.Add("ID13", typeof(string));
                dt.Columns.Add("ID14", typeof(string));
                dt.Columns.Add("ID15", typeof(string));
                dt.Columns.Add("ID16", typeof(string));
                dt.Columns.Add("ID17", typeof(string));
                dt.Columns.Add("ID18", typeof(string));
                dt.Columns.Add("ID19", typeof(string));
                dt.Columns.Add("ID20", typeof(string));
                dt.Columns.Add("ID21", typeof(string));
                dt.Columns.Add("ID22", typeof(string));
                dt.Columns.Add("ID23", typeof(string));
                dt.Columns.Add("ID24", typeof(string));
                dt.Columns.Add("ID25", typeof(string));
                dt.Columns.Add("ID26", typeof(string));
                dt.Columns.Add("ID27", typeof(string));
                dt.Columns.Add("ID28", typeof(string));
                dt.Columns.Add("ID29", typeof(string));
                dt.Columns.Add("ID30", typeof(string));
                dt.Columns.Add("ID31", typeof(string));
                dt.Columns.Add("COLOR1", typeof(string));
                dt.Columns.Add("COLOR2", typeof(string));
                dt.Columns.Add("COLOR3", typeof(string));
                dt.Columns.Add("COLOR4", typeof(string));
                dt.Columns.Add("COLOR5", typeof(string));
                dt.Columns.Add("COLOR6", typeof(string));
                dt.Columns.Add("COLOR7", typeof(string));
                dt.Columns.Add("COLOR8", typeof(string));
                dt.Columns.Add("COLOR9", typeof(string));
                dt.Columns.Add("COLOR10", typeof(string));
                dt.Columns.Add("COLOR11", typeof(string));
                dt.Columns.Add("COLOR12", typeof(string));
                dt.Columns.Add("COLOR13", typeof(string));
                dt.Columns.Add("COLOR14", typeof(string));
                dt.Columns.Add("COLOR15", typeof(string));
                dt.Columns.Add("COLOR16", typeof(string));
                dt.Columns.Add("COLOR17", typeof(string));
                dt.Columns.Add("COLOR18", typeof(string));
                dt.Columns.Add("COLOR19", typeof(string));
                dt.Columns.Add("COLOR20", typeof(string));
                dt.Columns.Add("COLOR21", typeof(string));
                dt.Columns.Add("COLOR22", typeof(string));
                dt.Columns.Add("COLOR23", typeof(string));
                dt.Columns.Add("COLOR24", typeof(string));
                dt.Columns.Add("COLOR25", typeof(string));
                dt.Columns.Add("COLOR26", typeof(string));
                dt.Columns.Add("COLOR27", typeof(string));
                dt.Columns.Add("COLOR28", typeof(string));
                dt.Columns.Add("COLOR29", typeof(string));
                dt.Columns.Add("COLOR30", typeof(string));
                dt.Columns.Add("COLOR31", typeof(string));
                return dt;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 分割dt,將分割后的dt裝入ds
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataSet dsSplit(DataTable dt)
        {
            DataSet ds = new DataSet();
            DataTable dtNew = new DataTable();
            try
            {
                if (dt.Rows.Count == 0)
                {
                    return ds;
                }

                dtNew = dt.Clone();
                dtNew.TableName = "0";
                for (int i = 0; i < dt.Rows.Count; i++)
                {                    
                    if (i > 0)
                    {
                        if (dt.Rows[i]["ProdGroup"].ToString() != dt.Rows[i - 1]["ProdGroup"].ToString() || dt.Rows[i]["PRODID"].ToString() != dt.Rows[i - 1]["PRODID"].ToString() || dt.Rows[i]["SubLineID"].ToString() != dt.Rows[i - 1]["SubLineID"].ToString())
                        {
                            ds.Tables.Add(dtNew);
                            dtNew = dt.Clone();
                            dtNew.TableName = i.ToString();
                        }
                    }
                    dtNew.ImportRow(dt.Rows[i]);

                    if (i == dt.Rows.Count -1)
                    {
                        ds.Tables.Add(dtNew);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        /// <summary>
        /// 由傳入的ID取得當然ID 綁定的ColorID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string returnSn(string str)
        {
            return "COLOR" +  str.Substring(2, str.Length - 2);
        }


        #endregion

    }
}
