using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DAL_PolyCafe
{
    public class DBUtil
    {
        private static string connString = @"Data Source=DESKTOP-NTO91QO\DUC08;Initial Catalog=Xuong_QuanLyVatTu;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public static SqlCommand GetCommand(string sql, List<Object> args, CommandType cmdType)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = cmdType;
            for (int i = 0; i < args.Count; i++)
            {
                cmd.Parameters.AddWithValue($"@{i
    }", args[i]);
            }
            return cmd;
        }

        public static void Update(string sql, List<Object> args, CommandType cmdType = CommandType.Text)
        {
            SqlCommand cmd = GetCommand(sql, args, cmdType);
            cmd.Connection.Open();
            cmd.Transaction = cmd.Connection.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (Exception )
            {
                cmd.Transaction.Rollback();
                throw;
            }
        }

        public static SqlDataReader Query(string sql, List<Object> args, CommandType cmdType = CommandType.Text)
        {
            try
            {
                SqlCommand cmd = GetCommand(sql, args, cmdType);
                cmd.Connection.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T Value<T>(string sql, List<Object> args, CommandType cmdType = CommandType.Text) where T : new() 
        {
            try
            {
                SqlCommand cmd = GetCommand(sql, args, cmdType);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    T result = new T();
                    Type type = typeof(T);
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        PropertyInfo propertyInfo = type.GetProperty(columnName);
                        if (propertyInfo != null && propertyInfo.CanWrite)
                        {
                            object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            if (value != null)
                            {
                                propertyInfo.SetValue(result, Convert.ChangeType(value, propertyInfo.PropertyType));
                            }    
                        }
                    }
                    return result;
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object ScalarQuery(string sql, List<object> args, CommandType cmdType = CommandType.Text)
        {
            SqlCommand cmd = GetCommand(sql, args, cmdType);
            cmd.Connection.Open();
            return cmd.ExecuteScalar();
        }

        public static DataTable ExecuteQuery(string sql, SqlParameter[] parameters, CommandType cmdType = CommandType.Text)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

    }
}
