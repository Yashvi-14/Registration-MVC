using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
namespace LoginMVC.Helpers
{
    public class DBHelper
    {
        public static DataTable GetDataTable(string sqlQuery, List<SqlParameter> mySqlParams)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionStr"].ConnectionString;
            SqlConnection conn = new SqlConnection(constr);
            try
            {
                DataTable _dataTable = new DataTable();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                if (mySqlParams != null && mySqlParams.Count() > 0)
                {
                    foreach (SqlParameter param in mySqlParams)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                SqlDataAdapter _da = new SqlDataAdapter(cmd);

                //DataSet DS = new DataSet();
                //_da.Fill(DS);
                //DataTable _dataTable = DS.Tables[0];

                _da.Fill(_dataTable);
                conn.Close();
                return _dataTable;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        public static int ExecuteQuery(string sqlQuery, List<SqlParameter> mySqlParams)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionStr"].ConnectionString;
            SqlConnection conn = new SqlConnection(constr);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                int nRowsAffected = 0;
                if (mySqlParams != null && mySqlParams.Count() > 0)
                {
                    foreach (SqlParameter param in mySqlParams)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                nRowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return nRowsAffected;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }
    }
}