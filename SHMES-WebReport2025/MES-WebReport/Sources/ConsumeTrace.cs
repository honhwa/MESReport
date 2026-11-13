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
    public class ConsumeTrace
    {
        #region Private Parameters
        private DBConn dbConn;
        private DataSet dsResult;
        private string strLotID = string.Empty;
        private string strProdID = string.Empty;
        private string strTimeBy = string.Empty;
        private string strToTimeBy = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Fab ID
        /// </summary>

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

        public string LotID
        {
            get
            {
                return this.strLotID;
            }
            set
            {
                this.strLotID = value;
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
        
        #endregion

        #region Constructor
        public ConsumeTrace()
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
                if (this.strLotID == string.Empty)
                {
                    throw new Exception("ProdID 未輸入！");
                }
                if (this.strProdID == string.Empty)
                {
                    throw new Exception("ProdID 未輸入！");
                }
                else
                {
                string strCol = string.Empty;
                string strFilter1 = string.Empty;
                string strFilter2 = string.Empty;
                string strFilter3 = string.Empty;
                strFilter1 += " WHERE PARTQTY<>0 AND PARTID<>''AND RIGHT(PARTNAME,2)=RIGHT(PARTQTYNAME,2) ";
                StringBuilder strLotID = new StringBuilder();
                StringBuilder strProdID = new StringBuilder();
                strFilter1 += " AND ENTITYID =" + " '" + this.strLotID + "' ";
                strFilter1 += " AND PRODID =" + " '" + this.strProdID + "' ";
                strFilter2 += " WHERE ENTITYID =" + " '" + this.strLotID + "' ";
                strFilter2 += " AND PRODID =" + " '" + this.strProdID + "' ";
                strFilter3 += " AND New_WLTM.ENTITYID =" + " '" + this.strLotID + "' ";
                strFilter3 += " AND New_WLTM.PRODID =" + " '" + this.strProdID + "' ";
                strFilter3 += " AND MNKMWSM.PARTQTY<>0 AND MNKMWSM.PARTID<>'' and New_WLTM.ENTITYTQTY<>0 ";

                string strSql1 = @"select PARTID,RIGHT(PARTNAME,2) AS PARTTYPE,PARTQTY,CWORKCTR AS WORKCTR,flg='直材'
				from New_WLTM
				UNPIVOT ( PARTID FOR PARTNAME in (PARTID26,PARTIDBL,PARTIDCO,PARTIDFP,PARTIDIC,PARTIDLC,PARTIDLP,PARTIDPB,PARTIDRB
				,PARTIDTC,PARTIDTP,PARTIDUC,PARTIDUP,PARTIDWB,PARTIDPL,PARTIDR1,PARTIDR2,PARTIDR3,PARTIDRF,PARTIDLG,PARTIDWC,PARTIDDF
				,PARTIDLT,PARTIDUT,PARTIDAF,PARTIDDT,PARTIDSI,PARTIDST,PARTIDUV,PARTIDYJ,PARTIDR4,PARTIDHS,PARTIDMC,PARTIDBC,PARTIDLE
                ,PARTIDBF,PARTIDSP,PARTIDUD,PARTIDLD,PARTIDPS,PARTIDLF,PARTIDM1,PARTIDM2,PARTIDM3,PARTIDM4,PARTIDM5,PARTIDM6,PARTIDM7
                ,PARTIDM8,PARTIDM9) ) AS WLTXPvtPARTID
				UNPIVOT ( PARTQTY FOR PARTQTYNAME in (PARTQTY26,PARTQTYBL,PARTQTYCO,PARTQTYFP,PARTQTYIC,PARTQTYLC,PARTQTYLP,PARTQTYPB
				,PARTQTYRB,PARTQTYTC,PARTQTYTP,PARTQTYUC,PARTQTYUP,PARTQTYWB,PARTQTYPL,PARTQTYR1,PARTQTYR2,PARTQTYR3,PARTQTYRF,PARTQTYLG
				,PARTQTYWC,PARTQTYDF,PARTQTYLT,PARTQTYUT,PARTQTYAF,PARTQTYDT,PARTQTYSI,PARTQTYST,PARTQTYUV,PARTQTYYJ,PARTQTYR4,PARTQTYHS
                ,PARTQTYMC,PARTQTYBC,PARTQTYLE,PARTQTYBF,PARTQTYSP,PARTQTYUD,PARTQTYLD,PARTQTYPS,PARTQTYLF,PARTQTYM1,PARTQTYM2,PARTQTYM3
                ,PARTQTYM4,PARTQTYM5,PARTQTYM6,PARTQTYM7,PARTQTYM8,PARTQTYM9) ) AS WLTXPvtPARTQTY";
                strFilter1 += " AND CONVERT(VARCHAR(19),TRANSDATE,120) BETWEEN '" + this.strTimeBy + "' AND '" + this.strToTimeBy + "'";
                strSql1 = strSql1 + strFilter1;

                string strSql2 = @"select distinct MNKMWSM.PARTID,MNKMWSM.partname as PARTTYPE, MNKMWSM.PARTQTY*(select entitytqty from New_WLTM " + strFilter2 + @") as partqty ,MNKMWSM.WORKCTR,flg='輔材'
                from MNKMWSM,New_WLTM WHERE New_WLTM.PRODID=MNKMWSM.PRODID AND New_WLTM.CWORKCTR=MNKMWSM.WORKCTR " + strFilter3;

                string strSql = strSql1 + " union all " + strSql2;
                strSql += "";
                this.dsResult = this.dbConn.GetDataSetBySql(strSql);
                return this.dsResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
