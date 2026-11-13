using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class LOTTYPE : CollectionStandard
{

      #region Private Parameters

        private string strFabID;
        private string strLotType = "";

        #endregion


    public LOTTYPE()
        {

        }
        #region Properties
        // Fab ID
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
        //strSubLineID
        public string LotType
        {

            get
            {
                return this.strLotType;
            }
            set
            {
                this.strLotType = value;
            }
        }
        #endregion

        #region Monthed
        public DataSet GetLotType()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = string.Empty;

                strSql = "SELECT LOTTYPE FROM MLTM WHERE FABID='" + this.strFabID + "' AND LOTTYPE IN ('PR','RW') ";
                if (this.strLotType.ToString().Trim() != "")
                {
                    strSql += " AND LOTTYPE='" + this.strLotType + "'";
                }

                ds = this.DBConn.GetDataSetBySql(strSql);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

}

