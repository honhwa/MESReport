using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Report.Library
{
    public class MSSQLHelper
    {
        /// <summary>
        /// 设置数据库字符串的连接
        /// </summary> 
        private static string connectionString = ConfigurationManager.ConnectionStrings["ConnectPRD"].ToString();

        #region ExecuteDataTable


        /// <summary>
        /// 执行一个查询，并返回结果集
        /// </summary>
        /// <param name="commandText">要执行的查询SQL文本命令</param>
        /// <returns>返回查询结果集</returns>
        public static DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// 执行一个查询,并返回查询结果
        /// </summary>
        /// <param name="commandText">要执行的SQL语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者SQL文本命令</param>
        /// <returns>返回查询结果集</returns>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, commandType, null);
        }

        /// <summary>
        /// 执行一个查询,并返回查询结果
        /// </summary>
        /// <param name="commandText">要执行的SQL语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者SQL文本命令</param>
        /// <param name="parameters">PL-SQL 语句或存储过程的参数数组</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            DataTable data = new DataTable();//实例化DataTable，用于装载查询结果集

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;//设置command的CommandType为指定的CommandType
                    //如果同时传入了参数，则添加这些参数
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    //通过包含查询SQL的SqlCommand实例来实例化SqlDataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(data);//填充DataTable
                }
            }
            return data;
        }
        #endregion ExecuteDataTable


        #region ExecuteReader

        /// <summary>
        /// 将 CommandText 发送到 Connection 并生成一个 SqlDataReader。
        /// </summary>
        /// <param name="commandText">要执行的查询SQL文本命令</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// 将 CommandText 发送到 Connection 并生成一个 SqlDataReader。
        /// </summary>
        /// <param name="commandText">要执行的SQL语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者SQL文本命令</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, null);
        }

        /// <summary>
        /// 将 CommandText 发送到 Connection 并生成一个 SqlDataReader。
        /// </summary>
        /// <param name="commandText">要执行的SQL语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者SQL文本命令</param>
        /// <param name="parameters">Transact-SQL 语句或存储过程的参数数组</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType, SqlCommand[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(commandText, connection);
            //如果同时传入了参数，则添加这些参数
            if (parameters != null)
            {
                foreach (SqlCommand parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            connection.Open();
            //CommandBehavior.CloseConnection参数指示关闭Reader对象时关闭与其关联的Connection对象
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        #endregion ExecuteReader



        #region ExecuteScalar

        /// <summary>
        /// 从数据库中检索单个值（例如一个聚合值）。
        /// </summary>
        /// <param name="commandText">要执行的查询PL-SQL文本命令</param>
        /// <returns></returns>
        public static Object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// 从数据库中检索单个值（例如一个聚合值）。
        /// </summary>
        /// <param name="commandText">要执行的SQL语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者PL-SQL文本命令</param>
        /// <returns></returns>
        public static Object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// 从数据库中检索单个值（例如一个聚合值）。
        /// </summary>
        /// <param name="commandText">要执行的SQL语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者SQL文本命令</param>
        /// <param name="parameters">PL-SQL 语句或存储过程的参数数组</param>
        /// <returns></returns>
        public static Object ExecuteScalar(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;//设置command的CommandType为指定的CommandType
                    //如果同时传入了参数，则添加这些参数
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    connection.Open();//打开数据库连接
                    result = command.ExecuteScalar();
                }
            }
            return result;//返回查询结果的第一行第一列，忽略其它行和列
        }

        #endregion ExecuteScalar



        #region ExecuteNonQuery

        /// <summary>
        /// 对数据库执行增删改操作
        /// </summary>
        /// <param name="commandText">要执行的查询pl-sql文本命令</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// 对数据库执行增删改操作
        /// </summary>
        /// <param name="commandText">要执行的pl-sql语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者pl-sql文本命令</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, null);
        }

        /// <summary>
        /// 对数据库执行增删改操作
        /// </summary>
        /// <param name="commandText">要执行的pl-sql语句</param>
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者pl-sql文本命令</param>
        /// <param name="parameters">pl-sql 语句或存储过程的参数数组</param>
        /// <returns>返回执行操作受影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.CommandType = commandType;//设置command的CommandType为指定的CommandType
                        //如果同时传入了参数，则添加这些参数
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        connection.Open();//打开数据库连接
                        count = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return count;//返回执行增删改操作之后，数据库中受影响的行数
        }


        /// <summary>
        /// 对数据库执行增删改操作(添加事务）
        /// </summary>
        /// <param name="oracleTxs">sql语句,参数类</param>
        /// <returns>返回执行操作受影响的行数</returns>
        public static int ExecuteNonQuery(List<SqlEntity> oracleTxs)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    SqlTransaction tx = connection.BeginTransaction();
                    command.Transaction = tx;
                    try
                    {
                        foreach (SqlEntity o in oracleTxs)
                        {
                            command.CommandText = o.commandText;
                            command.CommandType = o.commandType;
                            if (o.parameters != null && o.parameters.Length > 0)
                            {
                                //有参数
                                foreach (SqlParameter parameter in o.parameters)
                                {
                                    command.Parameters.Add(parameter);
                                }
                            }
                            count += command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        count = 0;
                        tx.Rollback();
                        return 0;
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            return count;//返回执行增删改操作之后，数据库中受影响的行数
        }

        #endregion ExecuteNonQuery

    }
    public class SqlEntity
    {
        public string commandText { get; set; }

        public CommandType commandType { get; set; }

        public SqlParameter[] parameters { get; set; }

    }
}
