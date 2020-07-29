using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion
{
    public class ConexionConfiguracion
    {
        //En startup se mapea con la clase que tenemos en el json de configuración, por eso esta propiedad debe tener el mismo nombre
        //que el atributo que tenemos en el json de configuración.
        public string DefaultConnection { get; set; }
    }
}
