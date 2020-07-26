using DataAccess;
using DataAccess;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic
{
    public class DLExpediente
    {
        DAExpediente _daex = new DAExpediente();

        public int AgregarExpediente(Expediente _ex)
        {
            return _daex.AgregarExpediente(_ex);
        }

    }
}
