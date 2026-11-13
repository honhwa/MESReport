using System ;       
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
/*
 * 2013-05-22  V1.00    innolux/qingqing
 * 2020-08-06  V1.01    innolux/Jeff     ADD剔除非出貨料號 8380 8700 站點產出 選項
 */
namespace Report.Sources
{
    public class InputAndOutput
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strFabId = string.Empty;
        private List<string> ProdIDList = new List<string>();
        private List<string> WorkCtrList = new List<string>();
        private List<string> ProdGroupList = new List<string>();
        private List<string> WotypeList=new List<string>() ;
        private string strTimeBy = string.Empty;
        private string strToTimeBy = string.Empty;
        private bool blDelWorkctr; //V1.01 add
        private string strType = string.Empty; //V1.00 ADD
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

        public string TimeBy
        {
            get
            {
                return this.strTimeBy;
            }
            set
            {
                this.strTimeBy = value;
            }
        }
        public string ToTimeBy
        {
            get
            {
                return this.strToTimeBy;
            }
            set
            {

                this.strToTimeBy=value;
            }
        }
        /*Begin V1.01 add*/
        public bool DelWorkctr
        {
            get
            {
                return this.blDelWorkctr;
            }
            set
            {

                this.blDelWorkctr = value;
            }
        }
        /*End V1.01 add*/
        /*v1.00 Begin*/
        public string Type
        {
            get
            {
                return this.strType;
            }
            set
            {
                this.strType = value;
            }
        }

        /*v1.00 End*/
        public List<string> WorkCTR
        {
            get
            {
                return this.WorkCtrList;
            }
            set
            {
                this.WorkCtrList = value;
            }
        }

        public List<string> ProdID
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

        public List<string> ProdGroup
        {
            get
            {
                return this.ProdGroupList;
            }
            set
            {
                this.ProdGroupList = value;
            }
        }

        public List<string> Wotype
        {
            get
            {
                return this.WotypeList ;
            }
            set
            {
                this.WotypeList = value;
            }
        }
        #endregion

        #region Constructor
        public InputAndOutput()
        {
            this.dbConn = new DBConn();
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                if (this.TimeBy == string.Empty)
                {
                    throw new Exception("沒有開始時間！");
                }
                if (this.WorkCtrList == null || this.WorkCtrList.Count <= 0)
                {
                    throw new Exception("Station未選取！");
                }
                if (this.ProdIDList == null || this.ProdIDList.Count<= 0)
                {
                    throw new Exception("ProdID未選取！");
                }
                if (this.ProdGroupList == null || this.ProdGroupList.Count <=0)
                {
                    throw new Exception("ProdGroup未選取！");
                }
                /*v1.00 Begin*/
                if (this.strType == null || this.strType.Length == 0)
                {
                    throw new Exception("查詢類型未選取！");
                }
                /*v1.00 End*/
                string strCol = string.Empty;

                string strFilter1 = string.Empty;
                strFilter1 += " WHERE RW.FABID= '" + this.strFabId + "'";
                StringBuilder strWorkCTR = new StringBuilder();
                foreach (string s in this.WorkCtrList)
                {
                    strWorkCTR.Append("'").Append(s).Append("'");
                }
                strFilter1 += " AND RW.WORKCTR IN (" + strWorkCTR.ToString().Replace("''", "','") + ") ";

                StringBuilder strProdID = new StringBuilder();
                foreach (string s in this.ProdIDList)
                {
                    strProdID.Append("'").Append(s).Append("'");
                }
                strFilter1 += " AND RW.ProdID IN (" + strProdID.ToString().Replace("''", "','") + ") ";

                StringBuilder strProdGroup =new StringBuilder ();
                foreach (string s in this.ProdGroupList)
                {
                    strProdGroup.Append("'").Append(s).Append ("'");
                }
                strFilter1 +=" AND RW.PRODGROUP IN (" + strProdGroup.ToString().Replace("''","','")+")";

                StringBuilder strWotype=new StringBuilder();
                if (this.WotypeList != null && this.WotypeList.Count > 0)
                {   
                   foreach(string s in this.WotypeList )
                   {
                        strWotype.Append("'").Append(s).Append("'");
                   }
                    //string strSqlWotype ="SELECT WONBR FROM SWOM WHERE FABID= '"+this.strFabId+"' AND WOTYPE IN ("+strWotype.ToString().Replace("''","','")+")";
                   //string strSqlWotype = "SELECT WONBR FROM SWOM WHERE FABID= '" + this.strFabId + "' AND ( WOTYPE IN (" + strWotype.ToString().Replace("''", "','") + ") OR (LOTTYPE IN (" + strWotype.ToString().Replace("''", "','").Replace("S-","") + ") AND WOTYPE = 'S')) AND CONVERT(VARCHAR(10),CREATEDATE,120) BETWEEN Dateadd(MM,-6,CONVERT(VARCHAR(10),'" + this.strTimeBy + "',120)) AND Dateadd(DD,1,CONVERT(VARCHAR(10),'" + this.strToTimeBy + "',120)) ";
                   string strSqlWotype = "SELECT WONBR FROM New_SWOM WHERE FABID= '" + this.strFabId + "'";
                   strSqlWotype = strSqlWotype + " AND ( WOTYPE IN (" + strWotype.ToString().Replace("''", "','") + ") OR (LOTTYPE IN (" + strWotype.ToString().Replace("''", "','").Replace("S-", "") + ") AND WOTYPE = 'S'))";
                    if (strWotype.ToString().IndexOf("'S-RW'") < 0)
                    {
                        strSqlWotype = strSqlWotype + " AND (WOTYPE<>'S' OR LOTTYPE <>'RW')";
                    }
                    strSqlWotype = strSqlWotype + " AND CONVERT(VARCHAR(10),CREATEDATE,120) BETWEEN Dateadd(MM,-6,CONVERT(VARCHAR(10),'" + this.strTimeBy + "',120)) AND Dateadd(DD,1,CONVERT(VARCHAR(10),'" + this.strToTimeBy + "',120)) ";
                    StringBuilder strWonbr=new StringBuilder();
                    DataTable dt = this.dbConn.GetDataSetBySql(strSqlWotype).Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        strWonbr.Append("'").Append(dr["WONBR"].ToString()).Append("'"); 
                    }
                    if (strWonbr.Length > 4)
                    {
                        strFilter1 += " AND RW.wonbr IN (" + strWonbr.ToString().Replace("''", "','") + ")";
                    }
                    else
                    {
                        strFilter1 += "AND RW.wonbr in ('xx')";
                    }
                }
                /*Begin add 1.01*/
                if (DelWorkctr == true)
                {
                    strFilter1 += "AND ((mp.DELIVERY_OUTPUT = '1' and  RW.WORKCTR not in ('8380','8700')) or mp.DELIVERY_OUTPUT = '0')";
                }
                /*End add 1.01*/
                /*V1.00 Begin*/
//                string strSql = @"
//                     select rw.workctr+ '   ' +ctr.workname as workctr,RW.SUBLINEID,NUM.T9NCNO AS T9NCNO,
//                     case when isnull(RW.SUBLINEID,'')='' then '空白' else  RW.SUBLINEID end AS LINEID,
//                     rw.shiftid,rw.prodgroup+'   '+rw.prodid as prodid,rw.wonbr,SUM(rw.inputqty) as inputqty,
//                     SUM(RW.outputaqty) as outputqty ,SUM(RW.OUTPUTRQTY+RW.OUTPUTSQTY+RW.OUTPUTAQTY) as totalqty from RWCIO rw 
//                     left join mwcm ctr on rw.workctr=ctr.workctr and rw.fabid=ctr.fabid 
//                     left JOIN MPM9M NUM ON RW.PRODID=NUM.PRODID AND RW.FABID=NUM.FABID ";
//                //string Sql=@"SELECT  MIN(OPT2) ,MAX(OPT2) FROM MPGTM where fabid='" +this.strFabId+"'" ;
//                //DataSet ds = new DataSet();
//                //ds=this.dbConn.GetDataSetBySql(Sql);
//                //string strStart = ds.Tables[0].Rows[0][0].ToString();
//                //string strEnd = ds.Tables[0].Rows[0][1].ToString();
//                //strFilter1 += " AND RW.REPORTDATE BETWEEN CONVERT(DATETIME,(CONVERT(VARCHAR(10),'" + this.strTimeBy + "',120)";
//                //strFilter1 += "+' ' +CAST('" + strStart + "'AS VARCHAR))) AND ";
//                //strFilter1 += "dateadd(hh,+24 ,CONVERT(DATETIME,(CONVERT(VARCHAR(10),'" + this.strTimeBy + "',120)";

//                //strFilter1 += " AND CONVERT(VARCHAR(10),RW.REPORTDATE,120) BETWEEN '" + this.strTimeBy + "' AND '" + this.strToTimeBy + "'";        //Mark By Li.Miao
//                strFilter1 += " AND RW.REPORTDATE BETWEEN '" + this.strTimeBy + "' AND '" + this.strToTimeBy + "'";     // Update By Li.Miao 20130115
//                strSql = strSql + strFilter1;

//                strSql += "GROUP BY rw.workctr,ctr.workname,RW.SUBLINEID ,NUM.T9NCNO,rw.prodgroup,rw.prodid,rw.shiftid,rw.wonbr";
//                this.dsResult = this.dbConn.GetDataSetBySql(strSql);
                if (this.strType == "BY ALL")
                {
                    string strSql = @"
                     select 'TOTAL'AS reportdate,rw.workctr+ '   ' +ctr.workname as workctr,RW.SUBLINEID,NUM.T9NCNO AS T9NCNO,
                     case when isnull(RW.SUBLINEID,'')='' then '空白' else  RW.SUBLINEID end AS LINEID,
                     rw.shiftid,rw.prodgroup+'   '+rw.prodid as prodid,rw.wonbr,SUM(rw.inputqty) as inputqty,
                     SUM(RW.outputaqty) as outputqty ,SUM(RW.OUTPUTRQTY+RW.OUTPUTSQTY+RW.OUTPUTAQTY) as totalqty from RWCIO rw 
                     left join mwcm ctr on rw.workctr=ctr.workctr and rw.fabid=ctr.fabid 
                     left JOIN MPM9M NUM ON RW.PRODID=NUM.PRODID AND RW.FABID=NUM.FABID 
                     left join mpim mp on mp.prodid = rw.prodid"; //ADD 1.01 left mpim mp on mp.prodid = rw.prodid
                    //string Sql=@"SELECT  MIN(OPT2) ,MAX(OPT2) FROM MPGTM where fabid='" +this.strFabId+"'" ;
                    //DataSet ds = new DataSet();
                    //ds=this.dbConn.GetDataSetBySql(Sql);
                    //string strStart = ds.Tables[0].Rows[0][0].ToString();
                    //string strEnd = ds.Tables[0].Rows[0][1].ToString();
                    //strFilter1 += " AND RW.REPORTDATE BETWEEN CONVERT(DATETIME,(CONVERT(VARCHAR(10),'" + this.strTimeBy + "',120)";
                    //strFilter1 += "+' ' +CAST('" + strStart + "'AS VARCHAR))) AND ";
                    //strFilter1 += "dateadd(hh,+24 ,CONVERT(DATETIME,(CONVERT(VARCHAR(10),'" + this.strTimeBy + "',120)";

                    //strFilter1 += " AND CONVERT(VARCHAR(10),RW.REPORTDATE,120) BETWEEN '" + this.strTimeBy + "' AND '" + this.strToTimeBy + "'";        //Mark By Li.Miao
                    strFilter1 += " AND RW.REPORTDATE BETWEEN '" + this.strTimeBy + "' AND '" + this.strToTimeBy + "'";     // Update By Li.Miao 20130115
                    strSql = strSql + strFilter1;

                    strSql += "GROUP BY  rw.workctr,ctr.workname,RW.SUBLINEID ,NUM.T9NCNO,rw.prodgroup,rw.prodid,rw.shiftid,rw.wonbr";
                    this.dsResult = this.dbConn.GetDataSetBySql(strSql);
                }

                else if (this.strType == "BY Date")
                {
                    string strSql = @"
                     select CONVERT(varchar(10) ,rw.ReportDate,120) AS reportdate,rw.workctr+ '   ' +ctr.workname as workctr,RW.SUBLINEID,NUM.T9NCNO AS T9NCNO,
                     case when isnull(RW.SUBLINEID,'')='' then '空白' else  RW.SUBLINEID end AS LINEID,
                     rw.shiftid,rw.prodgroup+'   '+rw.prodid as prodid,rw.wonbr,SUM(rw.inputqty) as inputqty,
                     SUM(RW.outputaqty) as outputqty ,SUM(RW.OUTPUTRQTY+RW.OUTPUTSQTY+RW.OUTPUTAQTY) as totalqty from RWCIO rw 
                     left join mwcm ctr on rw.workctr=ctr.workctr and rw.fabid=ctr.fabid 
                     left JOIN MPM9M NUM ON RW.PRODID=NUM.PRODID AND RW.FABID=NUM.FABID 
                     left join mpim mp on mp.prodid = rw.prodid"; //ADD 1.01 left mpim mp on mp.prodid = rw.prodid
                    strFilter1 += " AND RW.REPORTDATE BETWEEN '" + this.strTimeBy + "' AND '" + this.strToTimeBy + "'";
                    strSql = strSql + strFilter1;

                    strSql += "GROUP BY  CONVERT(varchar(10) ,rw.ReportDate,120),rw.workctr,ctr.workname,RW.SUBLINEID ,NUM.T9NCNO,rw.prodgroup,rw.prodid,rw.shiftid,rw.wonbr";
                    this.dsResult = this.dbConn.GetDataSetBySql(strSql);
                }     
                /*V1.00 End*/

                return this.dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}