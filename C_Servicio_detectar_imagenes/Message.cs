using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Servicio_detectar_imagenes
{
    public class Message
    {
        private static EventViewer evento = new EventViewer();
        public static void Error(string metodo, Exception ex)
        {
            evento.WriteErrorLog(ex.Message);
            evento.WriteEventViewerLog(ex.Message, 4);
            Bootstrap.WriteLine("Error en " + metodo + " : " + ex.Message, BootstrapStyle.Default, BootstrapType.Danger);

        }
        public static void Error(string mensaje)
        {
            evento.WriteErrorLog(mensaje);
            evento.WriteEventViewerLog(mensaje, 1);
            Bootstrap.WriteLine("Error en " + mensaje, BootstrapStyle.Default, BootstrapType.Danger);

        }
        public static void warning(string mensaje)
        {
            Bootstrap.WriteLine(mensaje, BootstrapStyle.Default, BootstrapType.Warning);
            evento.WriteErrorLog(mensaje);
            evento.WriteEventViewerLog(mensaje, 4);

        }
        public static void Success(string mensaje)
        {
            Bootstrap.WriteLine(mensaje, BootstrapStyle.Default, BootstrapType.Success);
            evento.WriteErrorLog(mensaje);
            evento.WriteEventViewerLog(mensaje, 4);
        }
        public static void Info(string mensaje)
        {
            Bootstrap.WriteLine(mensaje, BootstrapStyle.Default, BootstrapType.Info);
            evento.WriteErrorLog(mensaje);
            evento.WriteEventViewerLog(mensaje, 4);
        }
    }
}
