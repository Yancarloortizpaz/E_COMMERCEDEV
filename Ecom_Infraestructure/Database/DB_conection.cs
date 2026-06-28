using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
namespace Ecom_Infraestructure.Database
{ 
        
    public class DB_conection
    {
        private readonly string _connectionString;

        public DB_conection(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
