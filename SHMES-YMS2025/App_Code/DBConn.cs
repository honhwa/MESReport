using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Xml;
using System.Text;

    public class DBConn
    {
        #region parameters
        private SqlConnection sqlConnection = null;
        private SqlTransaction dbTrans = null;
        #endregion

        #region properties
        /// <summary>
        /// 數據庫聯接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (!this.sqlConnection.Equals(null))
                {
                    return this.sqlConnection.ConnectionString;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 建構式


        /// 建構成功時即打開DB連接
        /// </summary>
        public DBConn(string strFABID)
        {
            try
            {
                string strConnectionName="";
                Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                //if (strFABID == "A")
                //{
                //    strConnectionName = cfg.AppSettings.Settings["DefaultConnectionLCM1StringName"].Value;
                //}
                //if (strFABID == "B")
                //{
                //    strConnectionName = cfg.AppSettings.Settings["DefaultConnectionLCM2StringName"].Value;
                //}
                strConnectionName = cfg.AppSettings.Settings["DefaultConnection"].Value;
                string strConnectionString = cfg.ConnectionStrings.ConnectionStrings[strConnectionName].ConnectionString;

                this.sqlConnection = new SqlConnection(strConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region methods
        /// <summary>
        /// 根據傳入的SQL語句及DataSet的名稱返回DataSet
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <param name="dsName">返回DataSet的名稱</param>
        /// <returns>返回的DataSet</returns>
        public DataSet GetDataSetBySql(string sql, string dsName)
        {
            if (this.sqlConnection.Equals(null))
            {
                throw new Exception("Connection is null!");
            }
            if (sql.Equals(string.Empty))
            {
                throw new Exception("Sql is null!");
            }

            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = this.sqlConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                sqlCommand.CommandTimeout = 0;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();

                if (dsName == "")
                {
                    this.sqlConnection.Open();
                    sqlDataAdapter.Fill(ds);
                    this.sqlConnection.Close();
                }
                else
                {
                    this.sqlConnection.Open();
                    sqlDataAdapter.Fill(ds, dsName);
                    this.sqlConnection.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根據傳入的SQL語句及DataSet的名稱返回SqlDataReader
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <returns>返回的SqlDataReader</returns>
        public SqlDataReader GetDataReaderBySql(string sql)
        {
            if (this.sqlConnection.Equals(null))
            {
                throw new Exception("Connection is null!");
            }
            if (sql.Equals(string.Empty))
            {
                throw new Exception("Sql is null!");
            }

            try
            {
                SqlCommand command = new SqlCommand(sql, this.sqlConnection);
                command.CommandTimeout = 0;
                this.sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                return reader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根據傳入的SQL語句返回DataSet
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <returns>返回的DataSet</returns>
        public DataSet GetDataSetBySql(string sql)
        {
            return this.GetDataSetBySql(sql, "");
        }

        /// <summary>
        /// 根據傳入的SQL語句返回DataSet
        /// </summary>
        /// <param name="spName">存儲過程名稱</param>
        /// <param name="parameters">存儲過程參數</param>
        /// <returns>DataSet結果</returns>
        public DataSet GetDataSetBySp(string spName, SqlParameter[] parameters)
        {
            return this.GetDataSetBySp(spName, parameters, "");
        }

        /// <summary>
        /// 根據傳入的SQL語句返回DataSet
        /// </summary>
        /// <param name="spName">存儲過程名稱</param>
        /// <param name="parameters">存儲過程參數</param>
        /// <param name="dsName">DataSet名稱</param>
        /// <returns>DataSet結果</returns>
        public DataSet GetDataSetBySp(string spName, SqlParameter[] parameters, string dsName)
        {
            try
            {
                if (this.sqlConnection.Equals(null))
                {
                    throw new Exception("Connection is null");
                }

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = this.sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                sqlCommand.Parameters.AddRange(parameters);
                sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();

                if (dsName == "")
                {
                    //this.sqlConnection.Open();
                    sqlDataAdapter.Fill(ds);
                    this.sqlConnection.Close();
                }
                else
                {
                    //this.sqlConnection.Open();
                    sqlDataAdapter.Fill(ds, dsName);
                    this.sqlConnection.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根據傳入的SQL語句及DataSet的名稱返回DataSet的XML
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <param name="dsName">返回DataSet的名稱</param>
        /// <returns>返回的DataSet的XML</returns>
        public XmlDataDocument GetDataSetXmlBySql(string sql, string dsName)
        {
            if (this.sqlConnection.Equals(null))
            {
                throw new Exception("Connection is null");
            }

            try
            {
                DataSet ds = this.GetDataSetBySql(sql, dsName);
                XmlDataDocument tmpXMLDoc = new XmlDataDocument(ds);
                return tmpXMLDoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 傳入單句SQL，執行insert,update,delete操作
        /// </summary>
        /// <param name="sqlString">SQL</param>
        /// <returns></returns>
        public bool ExecuteSql(string sqlString)
        {
            System.Web.UI.Page page = HttpContext.Current.Handler as Page;
            if (this.sqlConnection.Equals(null))
            {
                page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('Connection is null');</script>");
                return false;
            }
            if (sqlString.Equals(string.Empty))
            {
                page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('Sql is null!');</script>");
                return false;
            }
            try
            {
                this.sqlConnection.Open();
                this.dbTrans = this.sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = this.sqlConnection;
                sqlCommand.Transaction = this.dbTrans;
                sqlCommand.CommandText = sqlString;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.dbTrans.Rollback();
                this.sqlConnection.Close();
                if (ex.ToString().Substring(0, 62) == "System.Data.SqlClient.SqlException: DELETE 陳述式與 REFERENCE 條件約束")
                {
                    page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('資料已被使用，無法刪除');</script>");
                }
                else
                {
                    page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + ex.ToString().Substring(0, 128) + "');</script>");
                }
                return false;
            }
            this.dbTrans.Commit();
            this.sqlConnection.Close();
            return true;
        }


        /// <summary>
        /// 傳入單句SQL，執行insert,update,delete操作帶有返回值


        /// </summary>
        /// <param name="sqlString">SQL</param>
        /// <returns></returns>
        public bool ExecuteSqlReturn(string sqlString, out string strErrDesc, out int intErrCode)
        {
            strErrDesc = "";
            intErrCode = 0;

            if (this.sqlConnection.Equals(null))
            {
                strErrDesc = "Connection is null";
                intErrCode = 66661;
                return false;
            }
            if (sqlString.Equals(string.Empty))
            {
                strErrDesc = "Sql is null";
                intErrCode = 66662;
                return false;
            }
            try
            {
                this.sqlConnection.Open();
                this.dbTrans = this.sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = this.sqlConnection;
                sqlCommand.Transaction = this.dbTrans;
                sqlCommand.CommandText = sqlString;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.dbTrans.Rollback();
                this.sqlConnection.Close();
                if (ex.ToString().Substring(0, 62) == "System.Data.SqlClient.SqlException: DELETE 陳述式與 REFERENCE 條件約束")
                {
                    strErrDesc = "資料已被使用，無法刪除";
                    intErrCode = 66663;
                }
                else if (ex.ToString().Substring(0, 62) == "System.Data.SqlClient.SqlException: 違反 PRIMARY KEY 條件約束 'PK_MF")
                {
                    strErrDesc = "資料已存在，無法新增";
                    intErrCode = 66665;
                }
                else if (ex.ToString().Substring(0, 62) == "System.Data.SqlClient.SqlException: INSERT 陳述式與 FOREIGN KEY 條件")
                {
                    strErrDesc = "外鍵約束，請確認導入的資料中，是否有其他主當未維護的資料";
                    intErrCode = 66666;
                }
                else
                {
                    strErrDesc = ex.ToString();
                    intErrCode = 66664;
                }
                return false;
            }
            this.dbTrans.Commit();
            this.sqlConnection.Close();
            return true;
        }

        #endregion
    }
