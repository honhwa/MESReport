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

namespace Report.Sources
{
    public class HOLD
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private List<string> ProdIDList = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// Fab ID
        /// </summary>
        public List<string> ListProdID
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

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HOLD()
        {
            this.dbConn = new DBConn();
        }
          #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            string strFilter1 = string.Empty;
            string strGroupFilter = string.Empty;

            if (this.ProdIDList != null && this.ProdIDList.Count > 0)
            {
                StringBuilder sbProdID = new StringBuilder();
                foreach (string s in this.ProdIDList)
                {
                    sbProdID.Append("'").Append(s).Append("'");
                }

                strFilter1 += " WHERE PRODID IN (" + sbProdID.ToString().Replace("''", "','") + ") ";
            }
            else
            {

                strFilter1 = ""; 
            }


            string strSql = @" SELECT FABID,PRODID,SUM(ENTITYTQTY) AS QTY,Wonbr,Workctr,LOTGRADE,HOLDOWNER,HOLDDEFECTCD,WAITDAY,TIME_LIMIT 
                                    FROM(Select A.FABID,A.PRODID,A.ENTITYTQTY,A.Wonbr,A.Cworkctr as Workctr,A.LOTGRADE,B.HoldOwner,B.HoldDefectCD,DATEDIFF(DAY,A.TRANSDATE,GETDATE()) WAITDAY,
                                    CASE WHEN A.LOTGRADE = 'A' THEN 3 WHEN A.LOTGRADE = 'R'  THEN 7 WHEN A.LOTGRADE = 'S' THEN 3 ELSE 0 END AS TIME_LIMIT
                                    from New_WLTM A, New_WHLD B where  A.FabID= 'B' AND A.HIDEFLG = '0' AND A.EntitySts ='H' AND IsNull(A.HoldID,'')<>''  AND A.FABID = B.FABID AND A.HoldID = B.HoldID)AA
                                    " + strFilter1;
            strSql += @" GROUP BY FABID,PRODID,Wonbr,workctr,LOTGRADE,HOLDOWNER,HOLDDEFECTCD,WAITDAY,TIME_LIMIT";
           
            this.dsResult = this.dbConn.GetDataSetBySql(strSql);
            return this.dsResult;
        #endregion

        }
    }
}