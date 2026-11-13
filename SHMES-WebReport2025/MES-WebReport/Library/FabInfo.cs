using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Report.Library
{
    public class FabInfo
    {
        #region Parameters
        private DBConn dbConn;
        #endregion

        /// <summary>
        /// 建構式
        /// </summary>
        public FabInfo()
        {
            this.dbConn = new DBConn();
        }

        /// <summary>
        /// 取得Fab信息
        /// </summary>
        /// <returns>Fab信息的DataSet</returns>
        public DataSet GetFabIdDataSet()
        {
            string strSql = "SELECT FABID FROM MFABM";
            return this.dbConn.GetDataSetBySql(strSql);
        }
    }
}
