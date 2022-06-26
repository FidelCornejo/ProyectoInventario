using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CapaDatos
{
    public class conexion
    {   
        //realiza la conexion de al servidor mediante la variable cadena que contiene
        //todo los datos para conectarse esta en ./web.config
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();

    }
}
