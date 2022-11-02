using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TalkToTreat.Models
{
    public class search
    {
        SqlConnection con;
        SqlCommand cmd;
        DataSet ds;
        SqlDataAdapter adap;
        SqlDataReader sdr;

        public search(string procedure_name)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["TalktoTreatConnectionString"].ConnectionString);
            try
            {
                cmd = new SqlCommand();
                adap = new SqlDataAdapter();
                ds = new DataSet();
                cmd.CommandText = procedure_name;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
            }
            finally
            {

            }
        }

        public string AddParameter(string parameter, string value)
        {
            try
            {
                if (value == null || value == "")
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, int value)
        {
            try
            {
                if (value == null)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, int? value)
        {
            try
            {
                if (value == null)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, decimal value)
        {
            try
            {
                if (value == null || value == 0.0M)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, DataTable value)
        {
            try
            {
                if (value == null)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, DateTime value)
        {
            try
            {
                DateTime dd = new DateTime(0001, 01, 01);
                if (value.CompareTo(dd) == 0)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, DateTime? value)
        {
            try
            {
                if (value == null)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }
        public string AddParameter(string parameter, byte[] value)
        {
            try
            {
                if (value == null)
                {
                    cmd.Parameters.AddWithValue(parameter, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameter, value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
            return "Success";
        }

        public DataSet ExecuteSearch()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Connection = con;
                adap.SelectCommand = cmd;
                adap.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
                cmd.Dispose();
            }
        }

        public DataSet ExecuteSearch1()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                adap.SelectCommand = cmd;
                adap.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
                cmd.Dispose();
            }
        }

        public List<SelectListItem> Getdropdown()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Connection = con;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    listItems.Add(new SelectListItem { Text = sdr[1].ToString(), Value = sdr[0].ToString() });
                }
                return listItems;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
                cmd.Dispose();
            }
        }
    }
}