using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Expediente
    {
        public int IDExpediente { get; set; }
        public string Peso { get; set; }
        public DateTime FechaAtencion { get; set; }
        public string ProcedimientosRealizados { get; set; }
        public string Resumen { get; set; }
        public string Vacuna { get; set; }
        public string CodigoAnimal { get; set; }
    }
}
