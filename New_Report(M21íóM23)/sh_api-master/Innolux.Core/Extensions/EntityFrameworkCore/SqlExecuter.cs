using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
// using IBM.Data.DB2.Core;
//using IBM.EntityFrameworkCore;


namespace Innolux.Core.Extensions.EntityFrameworkCore
{
    #region SqlExecuter
    public static class SqlExecuter
    {
        public static Iconnection GetConnection(DatabaseFacade facade, DbConnection conn, DbTransaction transaction)
        {
            Iconnection Tconn = default(Iconnection);
            if (facade.IsOracle())
            {
                Tconn = new OracleDBConnection((OracleConnection)conn, transaction);
            }
            else if (facade.IsSqlServer())
            {
                Tconn = new SqlServerConnection((SqlConnection)conn, transaction);
            }


            else
            {
                throw new System.Exception("暫時不支持此數據庫");
            }
            return Tconn;
        }


        public static int executeSqlTables(this DatabaseFacade facade, List<sqlTable> sqlTables)
        {
            int count = 0;
            int suc = 0;

            var conn = facade.GetDbConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();

            }

            DbTransaction transaction = conn.BeginTransaction();
            Iconnection Tconn = GetConnection(facade, conn, transaction);
            List<string> sqlLogs = new List<string>();
            try
            {

                foreach (sqlTable sT in sqlTables)
                {
                    count = count + 1;
                    string sql = sT.ToSQLwithDataParameter();
                    var param = sT.DataParameters(Tconn);


                    string logparam = "";
                    string sqllog = sql;
                    for (int i = 0; i < param.Length; i++)
                    {
                        logparam = param[i].Value.ToString();
                        Regex r = new Regex(@"\?");
                        if (!string.IsNullOrEmpty(logparam))
                        {

                            sqllog = r.Replace(sqllog, $@"'{logparam}'", 1);
                        }
                        else
                        {

                            sqllog = r.Replace(sqllog, "''", 1);
                        }
                    }
                    sqlLogs.Add(sqllog);

                    sT.affectedRow = Tconn.executeSQL(sql, param);

                    suc = suc + sT.affectedRow;



                }
                if (suc > 0)
                {
                    transaction.Commit();
                    StringBuilder logs = new StringBuilder();
                    foreach (var log in sqlLogs)
                    {
                        logs.Append(log + "\r\n\t");
                    }
                    LogHelper.DbLoger.Info(logs.ToString());
                    return suc;
                }
                else
                {
                    transaction.Rollback();
                    StringBuilder logs = new StringBuilder();
                    foreach (var log in sqlLogs)
                    {
                        logs.Append(log + "\r\n\t");
                    }
                    LogHelper.DbLoger.Info(logs.ToString());
                    return 0;
                }
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
                conn.Close();
                StringBuilder logs = new StringBuilder();
                foreach (var log in sqlLogs)
                {
                    logs.Append(log + "\r\n\t");
                }
                LogHelper.ErrorLog.Error(logs.ToString());
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        public static int executeSQL(this DatabaseFacade facade, string sql)
        {
            try
            {
                int affectedRow = 0;
                var conn = facade.GetDbConnection();
                Iconnection Tconn = GetConnection(facade, conn, null);
                affectedRow = Tconn.executeSQL(sql);
                return affectedRow;
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public static int executeSqls(this DatabaseFacade facade, List<string> sqllist)
        {
            try
            {
                int affectedRow = 0;
                var conn = facade.GetDbConnection();
                Iconnection Tconn = GetConnection(facade, conn, null);
                affectedRow = Tconn.executeSqls(sqllist);
                return affectedRow;
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public static int executeProc(this DatabaseFacade facade, string cmdText, IDataParameter[] cmdParameters)
        {
            try
            {
                int affectedRow = 0;
                var conn = facade.GetDbConnection();
                Iconnection Tconn = GetConnection(facade, conn, null);
                affectedRow = Tconn.executeProc(cmdText, cmdParameters);
                return affectedRow;
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public static int executeProc(this DatabaseFacade facade, string cmdText)
        {
            try
            {
                int affectedRow = 0;
                var conn = facade.GetDbConnection();
                Iconnection Tconn = GetConnection(facade, conn, null);
                affectedRow = Tconn.executeProc(cmdText, null);
                return affectedRow;
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }

    #endregion


    #region sqlTable
    /// <summary>
    /// sqlColumn
    /// </summary>
    public class sqlColumn
    {
        public string name = "";
        public object value;
    }

    /// <summary>
    /// eDMLtype
    /// </summary>
    public enum eDMLtype { Insert, Update, Delete };


    /// <summary>
    /// for where statement use in the sql
    /// </summary>
    public class whereClause : List<sqlColumn>
    {
        /// <summary>
        /// Gets the <see cref="sqlColumn"/> with the specified name.
        /// </summary>
        /// <value></value>
        public sqlColumn this[string name]
        {
            get
            {
                foreach (sqlColumn sc in this)
                {
                    if (sc.name == name)
                        return sc;
                }
                return null;
            }
        }
        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public sqlColumn Add(string name, object value)
        {
            sqlColumn sc = new sqlColumn();
            sc.name = name;
            sc.value = value;
            base.Add(sc);
            return sc;
        }


    }

    /// <summary>
    /// sqlTable
    /// </summary>
    public class sqlTable : whereClause
    {
        public whereClause WhereClause = new whereClause();
        public object Tag = null;
        private eDMLtype _DMLtype;
        private string _tableName;
        private int _expectedAffectedRow;
        private int _affectedRow;
        private int _resultNum;
        private int _erroNum;

        public int ErroNum
        {
            get { return _erroNum; }
            set { _erroNum = value; }
        }

        public int ResultNum
        {
            get { return _resultNum; }
            set { _resultNum = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="sqlTable"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type">The type.</param>
        public sqlTable(string tableName, eDMLtype type)
            : this(tableName, type, 0)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="sqlTable"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type">The type.</param>
        /// <param name="expectedAffectedRow">The expected affected row.</param>
        public sqlTable(string tableName, eDMLtype type, int expectedAffectedRow)
        {
            _DMLtype = type;
            _tableName = tableName;
            _expectedAffectedRow = expectedAffectedRow;
        }
        /// <summary>
        /// Gets the DM ltype.
        /// </summary>
        /// <value>The DM ltype.</value>
        public eDMLtype DMLtype
        {
            get { return _DMLtype; }
        }
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string tableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// Gets the expected affected row.
        /// </summary>
        /// <value>The expected affected row.</value>
        public int expectedAffectedRow
        {
            get { return _expectedAffectedRow; }
        }

        /// <summary>
        /// Gets or sets the affected row.
        /// </summary>
        /// <value>The affected row.</value>
        public int affectedRow
        {
            get { return _affectedRow; }
            set
            {
                _affectedRow = value;
            }
        }

        /// <summary>
        /// to generate the SQL statement.
        /// </summary>
        /// <returns></returns>
        public string ToSQL()
        {
            if (_DMLtype == eDMLtype.Insert)
                return ToInsertSQL();
            if (_DMLtype == eDMLtype.Update)
                return ToUpdateSQL();
            if (_DMLtype == eDMLtype.Delete)
                return ToDeleteSQL();
            return "";
        }

        /// <summary>
        /// to generate the insert SQL.
        /// </summary>
        /// <returns></returns>
        private string ToInsertSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into " + _tableName + " (");
            for (int i = 0; i < base.Count; i++)
            {
                sb.Append(this[i].name);
                if (i < (base.Count - 1))
                    sb.Append(",");
            }
            sb.Append(") values (");
            for (int i = 0; i < base.Count; i++)
            {
                sb.Append("'" + this[i].value.ToString() + "'");
                if (i < (base.Count - 1))
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }
        /// <summary>
        /// to generate the update SQL.
        /// </summary>
        /// <returns></returns>
        private string ToUpdateSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update " + _tableName + " set ");
            for (int i = 0; i < base.Count; i++)
            {
                sqlColumn sc = this[i];

                if (sc.name.IndexOf("=") == -1)
                    sb.Append(sc.name + "='" + sc.value.ToString() + "' ");
                else
                    sb.Append(sc.name + " ");

                if (i < (base.Count - 1))
                    sb.Append(", ");
            }
            sb.Append("where ");
            for (int i = 0; i < WhereClause.Count; i++)
            {
                sqlColumn sc = WhereClause[i];
                sb.Append(sc.name + "='" + sc.value.ToString() + "' ");
                if (i < (WhereClause.Count - 1))
                    sb.Append("and ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// to generate the delete SQL.
        /// </summary>
        /// <returns></returns>
        private string ToDeleteSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from " + _tableName);

            sb.Append(" where ");
            for (int i = 0; i < WhereClause.Count; i++)
            {
                sqlColumn sc = WhereClause[i];
                sb.Append(sc.name + "='" + sc.value.ToString() + "' ");
                if (i < (WhereClause.Count - 1))
                    sb.Append("and ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// the SQL with data parameter.
        /// </summary>
        /// <returns></returns>
        public string ToSQLwithDataParameter()
        {
            if (_DMLtype == eDMLtype.Insert)
                return ToInsertSQL2();
            if (_DMLtype == eDMLtype.Update)
                return ToUpdateSQL2();
            if (_DMLtype == eDMLtype.Delete)
                return ToDeleteSQL2();
            return "";
        }

        /// <summary>
        /// to generate the insert SQL.
        /// </summary>
        /// <returns></returns>
        private string ToInsertSQL2()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into " + _tableName + " (");
            for (int i = 0; i < base.Count; i++)
            {
                sb.Append(this[i].name);
                if (i < (base.Count - 1))
                    sb.Append(",");
            }
            sb.Append(") values (");
            for (int i = 0; i < base.Count; i++)
            {
                sb.Append("?");
                if (i < (base.Count - 1))
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }
        /// <summary>
        /// to generate the update SQL.
        /// </summary>
        /// <returns></returns>
        private string ToUpdateSQL2()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update " + _tableName + " set ");
            for (int i = 0; i < base.Count; i++)
            {
                sqlColumn sc = this[i];

                if (sc.name.IndexOf("=") == -1)
                    sb.Append(sc.name + "=? ");
                else
                    sb.Append(sc.name + " ");

                if (i < (base.Count - 1))
                    sb.Append(", ");
            }
            sb.Append("where ");
            for (int i = 0; i < WhereClause.Count; i++)
            {
                sqlColumn sc = WhereClause[i];
                sb.Append(sc.name + "=? ");
                if (i < (WhereClause.Count - 1))
                    sb.Append("and ");
            }
            return sb.ToString();
        }
        /// <summary>
        ///to generate the delete SQL.
        /// </summary>
        /// <returns></returns>
        private string ToDeleteSQL2()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from " + _tableName);

            sb.Append(" where ");
            for (int i = 0; i < WhereClause.Count; i++)
            {
                sqlColumn sc = WhereClause[i];
                sb.Append(sc.name + "=? ");
                if (i < (WhereClause.Count - 1))
                    sb.Append("and ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Datas the parameters.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <returns></returns>
        public IDataParameter[] DataParameters(Iconnection conn)
        {
            if (_DMLtype == eDMLtype.Insert)
                return InsertDataParameter(conn);
            if (_DMLtype == eDMLtype.Update)
                return UpdateDataParameter(conn);
            if (_DMLtype == eDMLtype.Delete)
                return DeleteDataParameter(conn);
            return null;
        }

        /// <summary>
        /// Inserts the data parameter.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <returns></returns>
        private IDataParameter[] InsertDataParameter(Iconnection conn)
        {
            IDataParameter[] dbParm = conn.createDBParameter(Count);

            for (int i = 0; i < base.Count; i++)
            {
                dbParm[i].Value = this[i].value;
            }
            return dbParm;
        }
        /// <summary>
        /// Updates the data parameter.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <returns></returns>
        private IDataParameter[] UpdateDataParameter(Iconnection conn)
        {
            int count = 0;
            for (int i = 0; i < base.Count; i++)
            {
                sqlColumn sc = this[i];
                if (sc.name.IndexOf("=") == -1)
                    count++;
            }
            IDataParameter[] dbParm = conn.createDBParameter(count + WhereClause.Count);

            count = 0;
            for (int i = 0; i < base.Count; i++)
            {
                sqlColumn sc = this[i];
                if (sc.name.IndexOf("=") == -1)
                {
                    dbParm[count].Value = sc.value;
                    count++;
                }
            }

            for (int i = 0; i < WhereClause.Count; i++)
            {
                dbParm[i + count].Value = WhereClause[i].value;
            }
            return dbParm;
        }
        /// <summary>
        /// Deletes the data parameter.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <returns></returns>
        private IDataParameter[] DeleteDataParameter(Iconnection conn)
        {
            IDataParameter[] dbParm = conn.createDBParameter(WhereClause.Count);
            for (int i = 0; i < WhereClause.Count; i++)
            {
                dbParm[i].Value = WhereClause[i].value;
            }
            return dbParm;
        }

    }
    #endregion

    #region  Iconnection
    public interface Iconnection
    {
        IDataParameter[] createDBParameter(int count);
        int executeSQL(string Sql, IDataParameter[] parameter);

        int executeSQL(string Sql);

        int executeSqls(List<string> sqlStrs);

        int executeProc(string cmdText, IDataParameter[] cmdParameters);

        string getValue(string Sql);
    }

    #endregion

    #region   SqlServerConnection
    public class SqlServerConnection : Iconnection
    {
        private readonly DbConnection _conn;
        private readonly DbTransaction _trans;

        public SqlServerConnection(DbConnection conn, DbTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }


        private void wakeupConnection()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            else if (_conn.State == ConnectionState.Broken)
            {
                _conn.Close();
                _conn.Open();
            }
        }

        /// <summary>
        /// 創建PRAMETER
        /// </summary>
        /// <param name="count">個數</param>
        /// <returns></returns>
        public IDataParameter[] createDBParameter(int count)
        {
            IDataParameter[] idp = new IDataParameter[count];
            for (int i = 0; i < idp.Length; i++)
                idp[i] = _conn.CreateCommand().CreateParameter();

            return (idp);
        }

        public void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, IDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = translateParamedSQL(cmdText, cmdParms);
            cmd.CommandTimeout = 100000;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 轉換SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string translateParamedSQL(string sql, IDataParameter[] parameter)
        {
            if (parameter == null) return sql;
            StringBuilder sb = new StringBuilder(sql);
            int iParamNumber = 0;
            do
            {
                int i = sb.ToString().IndexOf('?');
                if (i == -1)
                    break;
                string paramName = "Parm" + iParamNumber.ToString();
                sb.Replace("?", "@" + paramName, i, 1);
                parameter[iParamNumber].ParameterName = paramName;
                iParamNumber++;
            } while (true);
            return sb.ToString();
        }

        public int executeSQL(string Sql, IDataParameter[] parameter)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, parameter);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Exception e)
                {
                    throw e;
                }

            }
        }

        public int executeSQL(string Sql)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, null);
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }
        }

        public int executeSqls(List<string> sqlStrs)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                int count = 0;
                DbTransaction tran = _conn.BeginTransaction();
                try
                {
                    foreach (string strsql in sqlStrs)
                    {
                        PrepareCommand(cmd, _conn, tran, strsql, null);
                        int val = cmd.ExecuteNonQuery();
                        count += val;
                    }
                    tran.Commit();
                    return count;
                }
                catch (System.Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }
        }

        public int executeProc(string cmdText, IDataParameter[] cmdParameters)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    int val;
                    cmd.Transaction = _trans;
                    cmd.Connection = _conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandTimeout = 100000;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmdParameters != null)
                    {
                        foreach (IDataParameter sp in cmdParameters)
                            cmd.Parameters.Add(sp);
                    }
                    val = cmd.ExecuteNonQuery();
                    return val;

                }
                catch (System.Exception)
                {

                    throw;
                }

            }
        }

        public string getValue(string Sql)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, null);
                    object obj = cmd.ExecuteScalar();
                    return obj != null ? obj.ToString() : string.Empty;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }

        }
    }

    #endregion

    #region  OracleConnection
    public class OracleDBConnection : Iconnection
    {
        private readonly DbConnection _conn;
        private readonly DbTransaction _trans;

        public OracleDBConnection(DbConnection conn, DbTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }

        private void wakeupConnection()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            else if (_conn.State == ConnectionState.Broken)
            {
                _conn.Close();
                _conn.Open();
            }
        }


        /// <summary>
        /// 創建PRAMETER
        /// </summary>
        /// <param name="count">個數</param>
        /// <returns></returns>
        public IDataParameter[] createDBParameter(int count)
        {
            IDataParameter[] idp = new IDataParameter[count];
            for (int i = 0; i < idp.Length; i++)
                idp[i] = _conn.CreateCommand().CreateParameter();

            return (idp);
        }

        public void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, IDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = translateParamedSQL(cmdText, cmdParms);
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 轉換SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string translateParamedSQL(string sql, IDataParameter[] parameter)
        {
            if (parameter == null) return sql;
            StringBuilder sb = new StringBuilder(sql);
            int iParamNumber = 0;
            do
            {
                int i = sb.ToString().IndexOf('?');
                if (i == -1)
                    break;
                string paramName = "Parm" + iParamNumber.ToString();
                sb.Replace("?", ":" + paramName, i, 1);
                parameter[iParamNumber].ParameterName = paramName;
                iParamNumber++;
            } while (true);
            return sb.ToString();
        }

        public int executeSQL(string Sql, IDataParameter[] parameter)
        {
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, parameter);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Exception e)
                {
                    throw e;
                }

            }
        }

        public int executeSQL(string Sql)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, null);
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }
        }

        public int executeSqls(List<string> sqlStrs)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                int count = 0;
                DbTransaction tran = _conn.BeginTransaction();
                try
                {
                    foreach (string strsql in sqlStrs)
                    {
                        PrepareCommand(cmd, _conn, tran, strsql, null);
                        int val = cmd.ExecuteNonQuery();
                        count += val;
                    }
                    tran.Commit();
                    return count;
                }
                catch (System.Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }
        }


        public int executeProc(string cmdText, IDataParameter[] cmdParameters)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    int val;
                    cmd.Transaction = _trans;
                    cmd.Connection = _conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmdParameters != null)
                    {
                        foreach (IDataParameter sp in cmdParameters)
                            cmd.Parameters.Add(sp);
                    }
                    val = cmd.ExecuteNonQuery();
                    return val;

                }
                catch (System.Exception)
                {

                    throw;
                }

            }
        }

        public string getValue(string Sql)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, null);
                    object obj = cmd.ExecuteScalar();
                    return obj != null ? obj.ToString() : string.Empty;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }

        }
    }

    #endregion

    #region DB2Connection
    public class IBMDB2Connection : Iconnection
    {
        private readonly DbConnection _conn;
        private readonly DbTransaction _trans;

        public IBMDB2Connection(DbConnection conn, DbTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }


        private void wakeupConnection()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            else if (_conn.State == ConnectionState.Broken)
            {
                _conn.Close();
                _conn.Open();
            }
        }

        /// <summary>
        /// 創建PRAMETER
        /// </summary>
        /// <param name="count">個數</param>
        /// <returns></returns>
        public IDataParameter[] createDBParameter(int count)
        {
            IDataParameter[] idp = new IDataParameter[count];
            for (int i = 0; i < idp.Length; i++)
                idp[i] = _conn.CreateCommand().CreateParameter();

            return (idp);
        }

        public void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, IDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = translateParamedSQL(cmdText, cmdParms);
            cmd.CommandTimeout = 100000;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 轉換SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string translateParamedSQL(string sql, IDataParameter[] parameter)
        {
            if (parameter == null) return sql;
            StringBuilder sb = new StringBuilder(sql);
            int iParamNumber = 0;
            do
            {
                int i = sb.ToString().IndexOf('?');
                if (i == -1)
                    break;
                string paramName = "Parm" + iParamNumber.ToString();
                sb.Replace("?", "@" + paramName, i, 1);
                parameter[iParamNumber].ParameterName = paramName;
                iParamNumber++;
            } while (true);
            return sb.ToString();
        }

        public int executeSQL(string Sql, IDataParameter[] parameter)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, parameter);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Exception e)
                {
                    throw e;
                }

            }
        }

        public int executeSQL(string Sql)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, null);
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }
        }

        public int executeSqls(List<string> sqlStrs)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                int count = 0;
                DbTransaction tran = _conn.BeginTransaction();
                try
                {
                    foreach (string strsql in sqlStrs)
                    {
                        PrepareCommand(cmd, _conn, tran, strsql, null);
                        int val = cmd.ExecuteNonQuery();
                        count += val;
                    }
                    tran.Commit();
                    return count;
                }
                catch (System.Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }
        }

        public int executeProc(string cmdText, IDataParameter[] cmdParameters)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    int val;
                    cmd.Transaction = _trans;
                    cmd.Connection = _conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandTimeout = 100000;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmdParameters != null)
                    {
                        foreach (IDataParameter sp in cmdParameters)
                            cmd.Parameters.Add(sp);
                    }
                    val = cmd.ExecuteNonQuery();
                    return val;

                }
                catch (System.Exception)
                {

                    throw;
                }

            }
        }

        public string getValue(string Sql)
        {
            wakeupConnection();
            using (DbCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, _conn, _trans, Sql, null);
                    object obj = cmd.ExecuteScalar();
                    return obj != null ? obj.ToString() : string.Empty;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    _conn.Close();
                }

            }

        }
    }
    #endregion


}
