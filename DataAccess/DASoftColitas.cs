using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class DASoftColitas
    {
        private static string Conn = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=SoftColitas;Integrated Security=True";
        private static string Conn2 = @"Data Source=(local);Initial Catalog=SoftColitas;Integrated Security=True";

        public static IDbConnection Conexion()
        {
            try
            {
                return new SqlConnection(Conn);
            }
            catch
            {
                return new SqlConnection(Conn2);
            }
        }

        public static IDbCommand ObtenerComando(string pComando, IDbConnection pCon) {
            return new SqlCommand(pComando, pCon as SqlConnection);
        }


    }
}
