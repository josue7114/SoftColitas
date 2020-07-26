using DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DAExpediente
    {
        public int AgregarExpediente(Expediente ex)
        {
            IDbConnection _conn = DASoftColitas.Conexion();
            _conn.Open();
            SqlCommand _Comand = new SqlCommand("SP_REGISTRAR_PROCEDIMIENTO", _conn as SqlConnection);
            _Comand.CommandType = CommandType.StoredProcedure;
            _Comand.Parameters.Add(new SqlParameter("@PESO", ex.Peso));
            _Comand.Parameters.Add(new SqlParameter("@FECHA", ex.FechaAtencion));
            _Comand.Parameters.Add(new SqlParameter("@PROCEDIMIENTO", ex.ProcedimientosRealizados));
            _Comand.Parameters.Add(new SqlParameter("@RESUMEN", ex.Resumen));
            _Comand.Parameters.Add(new SqlParameter("@VACUNA", ex.Vacuna));
            _Comand.Parameters.Add(new SqlParameter("@COD_ANIMAL", ex.CodigoAnimal));
            int Resultado = _Comand.ExecuteNonQuery();
            _conn.Close();
            return Resultado;
        }
    }
}
