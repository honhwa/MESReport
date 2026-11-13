using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Innolux.Core.Extensions.EntityFrameworkCore
{
    public static class SqlQueryExtensions
    {
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();

            }
            connection = conn;
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            try
            {

                var reader = command.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                command.Dispose();
                conn.Close();
                return dt;
            }
            catch (System.Exception ex)
            {
                command.Dispose();
                conn.Close();
                throw new System.Exception(ex.Message);
            }

        }

        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            try
            {
                var dt = SqlQuery(facade, sql, parameters);
                return dt.ToList<T>();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public static DataTable SqlQueryPager(this DatabaseFacade facade, int pageSize, int pageIndex, string sql, string tabel, ref int count, params object[] parameters)
        {

            DataTable dt = new DataTable();
            if (facade.IsOracle())
            {
                dt = SqlQueryPagerOralce(facade, pageSize, pageIndex, sql, tabel, ref count, parameters);
            }
            else if (facade.IsSqlServer())
            {
                dt = SqlQueryPagerSqlServer(facade, pageSize, pageIndex, sql, tabel, ref count, parameters);
            }
            //   else if (facade.IsMySql())
            //   {
            //     dt = SqlQueryPagerMySql(facade, pageSize, pageIndex, sql, tabel, ref count, parameters);
            //   }
            else
            {
                throw new System.Exception("暫時不支持此數據庫");
            }

            return dt;

        }

        public static List<T> SqlQueryPager<T>(this DatabaseFacade facade, int pageSize, int pageIndex, string sql, string tabel, ref int count, params object[] parameters) where T : class, new()
        {
            try
            {
                var dt = SqlQueryPager(facade, pageSize, pageIndex, sql, tabel, ref count, parameters);
                return dt.ToList<T>();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }



        private static DataTable SqlQueryPagerOralce(this DatabaseFacade facade, int pageSize, int pageIndex, string sql, string tabel, ref int count, params object[] parameters)
        {
            OracleCommand command = (OracleCommand)CreateCommand(facade, sql, out DbConnection conn, parameters);
            try
            {
                var ds = new DataSet();
                OracleDataAdapter oradata = new OracleDataAdapter();
                oradata.SelectCommand = command;
                oradata.Fill(ds, pageSize * (pageIndex - 1), pageSize, tabel);
                command.Parameters.Clear();
                DataTable dt = facade.SqlQuery(new StringBuilder("Select Count(1) From (" + sql + ")").ToString());
                count = Convert.ToInt32(dt.Rows[0][0]);
                command.Dispose();
                conn.Close();
                return ds.Tables[0];
            }
            catch (System.Exception ex)
            {
                command.Dispose();
                conn.Close();
                throw new System.Exception(ex.Message);
            }
        }


        private static DataTable SqlQueryPagerSqlServer(this DatabaseFacade facade, int pageSize, int pageIndex, string sql, string tabel, ref int count, params object[] parameters)
        {
            SqlCommand command = (SqlCommand)CreateCommand(facade, sql, out DbConnection conn, parameters);
            try
            {
                var ds = new DataSet();
                SqlDataAdapter data = new SqlDataAdapter();
                data.SelectCommand = command;
                data.Fill(ds, pageSize * (pageIndex - 1), pageSize, tabel);
                command.Parameters.Clear();
                DataTable dt = facade.SqlQuery(new StringBuilder("Select Count(1) From (" + sql + ") as SelectTempTable").ToString());
                count = Convert.ToInt32(dt.Rows[0][0]);
                command.Dispose();
                conn.Close();
                return ds.Tables[0];
            }
            catch (System.Exception ex)
            {
                command.Dispose();
                conn.Close();
                throw new System.Exception(ex.Message);
            }
        }

        // private static DataTable SqlQueryPagerMySql(this DatabaseFacade facade, int pageSize, int pageIndex, string sql, string tabel, ref int count, params object[] parameters)
        // {
        //   MySqlCommand command = (MySqlCommand)CreateCommand(facade, sql, out DbConnection conn, parameters);
        //   try
        //   {
        //     var ds = new DataSet();
        //     MySqlDataAdapter data = new MySqlDataAdapter();
        //     data.SelectCommand = command;
        //     data.Fill(ds, pageSize * (pageIndex - 1), pageSize, tabel);
        //     command.Parameters.Clear();
        //     DataTable dt = facade.SqlQuery(new StringBuilder("Select Count(1) From (" + sql + ")").ToString());
        //     count = Convert.ToInt32(dt.Rows[0][0]);
        //     command.Dispose();
        //     conn.Close();
        //     System.Logger.Information("InnoluxDataBaseCommand", sql);
        //     return ds.Tables[0];
        //   }
        //   catch (System.Exception ex)
        //   {
        //     command.Dispose();
        //     conn.Close();
        //     System.Logger.Error("InnoluxDataBaseCommand", sql);
        //     throw new System.Exception(ex.Message);
        //   }
        // }

        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {

            var list = ModelConvertHelper.ConvertToModel<T>(dt);
            return list;
        }
    }
}