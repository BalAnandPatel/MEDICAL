using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TalkToTreat.Models
{
    public class _Mysave
    {
        SqlCommand cmd;
        SqlConnection con;
        public _Mysave(string procedure_name)
        {
            cmd = new SqlCommand();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["TalktoTreatConnectionString"].ConnectionString);
            cmd.CommandText = procedure_name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
        }
        public void AddParameter(string param_name, string param_value)
        {
            if (param_value == "" || param_value == null)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public SqlParameter GetParameter(string param_name, SqlDbType param_value)
        {
            if (param_value == 0M)
            {
                cmd.Parameters.Add(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.Add(param_name, param_value);
            }

            return cmd.Parameters[param_name];
        }
        public void AddParameter(string param_name, int? param_value)
        {
            if (param_value == null)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public void AddParameter(string param_name, byte[] param_value)
        {
            if (param_value == null)
            {
                param_value = new byte[10];
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public void AddParameter(string param_name, decimal? param_value)
        {
            if (param_value == null)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public void AddParameter(string param_name, DateTime param_value)
        {
            DateTime dd = new DateTime(0001, 01, 01);
            if (param_value.CompareTo(dd) == 0)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public void AddParameter(string param_name, DateTime? param_value)
        {
            DateTime dd = new DateTime(0001, 01, 01);
            if (param_value == null)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public void AddParameter(string param_name, DataTable param_value)
        {
            if (param_value == null)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public void AddParameter(string param_name, List<int> param_value)
        {
            if (param_value == null)
            {
                cmd.Parameters.AddWithValue(param_name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(param_name, param_value);
            }
        }
        public int ExecuteSave()
        {
            int returnbyproc = 0;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            returnbyproc = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Dispose();
            cmd.Dispose();
            return returnbyproc;
        }
    }
}