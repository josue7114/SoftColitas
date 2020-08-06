using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;



namespace DataLogic
{
    public class DLVerifyComponents
    {
        
        public Boolean email_bien_escrito(string email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean caracteres_minimos(string componente)
        {
            int minimo = 6;
            if(componente.Length<=minimo)
            {
                return false;
            }
            return true;
        }
       
    }
}
