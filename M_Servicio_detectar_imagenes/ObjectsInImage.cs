using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_Servicio_detectar_imagenes
{
    public  class ObjectsInImage
    {
       
        public int PK_ObjectsInImage { get; set; }
        public string image_to_detect { get; set; }
        public int score { get; set; }
    }
}
