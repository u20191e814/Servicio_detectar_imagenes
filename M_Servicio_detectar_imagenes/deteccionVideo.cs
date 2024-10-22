using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_Servicio_detectar
{
    public  class deteccionVideo
    {
        public int id_frame { get; set; }
        public string localfile { get; set; }
        public List<deteccion> deteccions { get; set; }
    }
    public class deteccion
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int score { get; set; }
        public string clase { get; set; }
        public int seg {  get; set; }

    }

}
