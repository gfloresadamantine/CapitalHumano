using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace AdministracionRH.Repositories
{
    public class DAO
    {
        private SqlDataAdapter adapter;
        private SqlCommand comando;
        private SqlConnection conn;
        private DataSet dset;

        public void CloseConnection()
        {
            try
            {
                if ((this.conn != null) && (this.conn.State == ConnectionState.Open))
                {
                    this.conn.Close();
                    this.conn = null;
                }
            }
            catch
            {
                throw;
            }
        }

        public SqlDataReader ExecuteDataReader(string strsql)
        {
            SqlDataReader reader2;
            try
            {
                this.OpenConnection();
                this.comando = new SqlCommand(strsql, this.conn);
                reader2 = this.comando.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
            return reader2;
        }


        public  SqlDataReader ExecuteReader(List<SqlParameter> parametros, string sp_name)
        {
           
                this.OpenConnection();
                this.comando = new SqlCommand();
                this.comando.Connection = this.conn;
                this.comando.CommandType = CommandType.StoredProcedure;
                this.comando.CommandText = sp_name;
                this.comando.CommandTimeout = 0;
                foreach (SqlParameter parameter in parametros)
                {
                    this.comando.Parameters.Add(parameter);
                }
                SqlDataReader reader = this.comando.ExecuteReader(CommandBehavior.CloseConnection);
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
         

                return reader;
            
        }



        public SqlDataReader ExecuteDataReader(List<SqlParameter> parametros, string sp_name)
        {
            SqlDataReader reader2;
            try
            {
                this.OpenConnection();
                this.comando = new SqlCommand();
                this.comando.Connection = this.conn;
                this.comando.CommandType = CommandType.StoredProcedure;
                this.comando.CommandText = sp_name;
                this.comando.CommandTimeout = 0;
                foreach (SqlParameter parameter in parametros)
                {
                    this.comando.Parameters.Add(parameter);
                }
                reader2 = this.comando.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
            return reader2;
        }

        public DataSet ExecuteDataSet(StringBuilder strsql)
        {
            DataSet dset;
            try
            {
                this.OpenConnection();
                this.comando = new SqlCommand(strsql.ToString(), this.conn);
                this.adapter = new SqlDataAdapter(this.comando);
                this.dset = new DataSet();
                this.adapter.Fill(this.dset);
                dset = this.dset;
            }
            catch (Exception)
            {
                throw;
            }
            return dset;
        }

        public void ExecuteNonQuery(string strsql)
        {
            try
            {
                this.OpenConnection();
                this.comando = new SqlCommand(strsql, this.conn);
                this.comando.ExecuteNonQuery();
                this.CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExecuteProcedure(List<SqlParameter> parametros, string sp_name)
        {
            try
            {
                this.OpenConnection();
                this.comando = new SqlCommand();
                this.comando.Connection = this.conn;
                this.comando.CommandType = CommandType.StoredProcedure;
                this.comando.CommandText = sp_name;
                this.comando.CommandTimeout = 0;

                foreach (SqlParameter parameter in parametros)
                {
                    this.comando.Parameters.Add(parameter);
                }
                this.comando.ExecuteNonQuery();
                this.CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ExecuteScalar(List<SqlParameter> parametros, string sp_name)
        {
            string str;
            object obj;
            try
            {
                //using (this.conn = new SqlConnection(this.GetConnectionString()))
                //{
                //    this.conn.Open();
                this.OpenConnection();
                this.comando = new SqlCommand();
                this.comando.Connection = this.conn;
                this.comando.CommandType = CommandType.StoredProcedure;
                this.comando.CommandText = sp_name;
                foreach (SqlParameter parameter in parametros)
                {
                    this.comando.Parameters.Add(parameter);
                }

                obj = this.comando.ExecuteScalar();
                //  }
            }
            catch (Exception)
            {
                throw;
            }
            return obj.ToString();
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
        }

        public void OpenConnection()
        {
            try
            {
                this.conn = new SqlConnection(this.GetConnectionString());
                this.conn.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ExecuteSqlBulkCopy(DataTable tabla, string TablaDestino, out string message)
        {
            bool done = true;
            message = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = TablaDestino;
                        foreach (DataColumn col in tabla.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName.ToUpper());
                        }
                        try
                        {
                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(tabla);
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                            done = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                done = false;
            }
            return done;
        }
    }
}