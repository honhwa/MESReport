using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Yield.QualityReport
{
    public class QualityDetail2 : InnSqlAgent.IApiBase
    {
        public QualityDetail2()
        {
            //傳入參教及API說明
            //数据呈现 不再做复杂表头   横排展示数据改竖排展示   entity_no（pnl_id） 前端做合并
            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "G_FAC";
            parameters["entity_no"] = "前一界面 Panel";
            parameters["trans_date1"] = "Panel壞機 日期";
            parameters["trans_date2"] = "Panel壞機 時間";

            description = "QualityDetail2";
        }

        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Data_SH(fab);
                    return;
                default:
                    returnValue = "请填写厂别";
                    return;
            }
        }

        private void Get_Data_SH(string fab)
        {
            #region 变量声明
            string connectDB = string.Empty;
            string table_name1 = string.Empty;
            string table_name2 = string.Empty;
            string table_name3 = string.Empty;
            string table_name4 = string.Empty;
            string table_name5 = string.Empty;
            string table_name6 = string.Empty;
            string SQL01 = string.Empty;
            string SQL02 = string.Empty;
            string lstr_G_FAC = string.Empty;
            string entity_no = string.Empty;
            string trans_date1 = string.Empty;
            string trans_date2 = string.Empty;
            string trans_date = string.Empty;
            string sql_Entity = string.Empty;
            #endregion

            #region 条件凑取
            lstr_G_FAC = GetParameterValueInString("G_FAC");
            entity_no = GetParameterValueInString("entity_no");
            trans_date1 = GetParameterValueInString("trans_date1");
            trans_date2 = GetParameterValueInString("trans_date2");

            trans_date = trans_date1 + " " + trans_date2.Substring(0, 12);

            //entity_no 选择合计传递值，VB从前端串取 entity_no，FS也是采用此类方法，待与NB前端讨论，具体做法
            //sql_Entity = entity_no.Replace(",", " or entity_no= ");
            sql_Entity = entity_no;

            #region DB + table
            if (fab.Trim() == "SH" || fab.Trim() == "LCM1")
            {
                connectDB = "MESHIS";
            }
            #endregion

            #endregion

            #region SQL
            SQL01 = $@"SELECT W.TOENTITYID AS ENTITY_NO
		    ,W.TOPRODID AS PROD_NBR
		    ,W.CWORKCTR AS WORK_CTR
		    ,M.WORKNAME AS LCMPROCESS
		    ,CASE WHEN W.NSUBLINEID IS NULL THEN W.CSUBLINEID ELSE W.NSUBLINEID END AS EQUIP_NBR --Oracle空串即NULL,原判斷為 = ''
		    ,W.USERID AS OPERATOR
		    ,SUBSTR(W.TRANSID,1,10) AS TRANS_DATE1
		    ,SUBSTR(W.TRANSID,12,19) AS TRANS_DATE2
		    ,W.VERB AS TRANS_TYPE
		    ,'MES' AS COMMENT1 
    FROM (SELECT * FROM VWLTXA 
			        WHERE FABID = 'B' 
			        AND TOENTITYID = '{entity_no}'
			        AND TRANSID > TO_CHAR(TO_TIMESTAMP('{trans_date}','YYYY-MM-DD HH24:MI:SS.FF3') - INTERVAL '180' SECOND, 'YYYY-MM-DD HH24:MI:SS.FF3') 
                    AND TRANSID < TO_CHAR(TO_TIMESTAMP('{trans_date}','YYYY-MM-DD HH24:MI:SS.FF3') + INTERVAL '60' SECOND, 'YYYY-MM-DD HH24:MI:SS.FF3')
				    ) W
    LEFT JOIN MWCM M ON M.WORKCTR = W.CWORKCTR 
    ORDER BY W.TRANSID ASC";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, SQL01);
            SQL01 = SQL01.Replace("\r\n", " ");
            SQL01 = SQL01.Replace("\n", " ");
            SQL01 = SQL01.Replace("\t", " ");

            object ob = new
            {
                Data = ds,
                SQL01 = SQL01,
                FactoryType = fab,
                G_FAC = lstr_G_FAC,
            };
            returnValue = ob;
        }
    }
}
