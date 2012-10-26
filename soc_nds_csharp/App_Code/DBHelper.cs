using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;

namespace HDIC_DB
{
    public static class DBHelper
    {
     //   private static readonly string connStringName = ConfigurationManager.AppSettings["connStringName"].ToString();
        private static readonly string connString   = ConfigurationManager.ConnectionStrings["connStringName"].ConnectionString;
        private static readonly string providerName = ConfigurationManager.ConnectionStrings["connStringName"].ProviderName;
        public static DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);

        #region Public Mothods

        public static int ExcuteNonQuery(string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteNonQuery(null, CommandType.Text, cmdText, cmdParms);
        }

        public static int ExcuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteNonQuery(null, cmdType, cmdText, cmdParms);
        }

        public static int ExcuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                using (DbCommand cmd = provider.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误");
                    }
                }
            }
        }

        public static DbDataReader ExecuteReader(string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteReader(null, CommandType.Text, cmdText, cmdParms);
        }

        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteReader(null, cmdType, cmdText, cmdParms);
        }

        public static DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            //using (DbConnection conn = provider.CreateConnection())
            //{
            //    using (DbCommand cmd = provider.CreateCommand())
            //    {
                    DbConnection conn = provider.CreateConnection();
                    DbCommand cmd = provider.CreateCommand();
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        return cmd.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误");
                    }
            //    }
            //}
        }

        public static object ExecuteScalar(string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteScalar(null, CommandType.Text, cmdText, cmdParms);
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExecuteScalar(null, cmdType, cmdText, cmdParms);
        }

        public static object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                using (DbCommand cmd = provider.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误");
                    }
                }
            }
        }

        public static DataSet ExcuteDataSet (string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteDataSet(null, CommandType.Text, cmdText, cmdParms);
        }
        public static DataSet ExcuteDataSet (CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return ExcuteDataSet(null, cmdType, cmdText, cmdParms);
        }
        public static DataSet ExcuteDataSet (DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using ( DbConnection conn = provider.CreateConnection() )
            {
                using ( DbCommand cmd = provider.CreateCommand() )
                {
                    try
                    {
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        using (DbDataAdapter da = provider.CreateDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds);
                                return ds;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        cmd.Dispose();
                        throw new Exception("数据操作错误");
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        public static DataTable GetList(string sqlString)
        {
            DataTable dt = ExcuteDataSet(sqlString, null).Tables[0];
            return dt;
        }
    }
}
