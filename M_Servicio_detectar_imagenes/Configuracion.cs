using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_Servicio_detectar_imagenes
{
    public class Configuracion
    {
        public bool WriteLogInWindowsEvents { get; set; }
        public bool WriteLogInDirectory { get; set; }
        public string LogDirectory { get; set; }
        public int LogDirectoryMaxDays { get; set; }
        public string SqlConexion {  get; set; }
    }
}
