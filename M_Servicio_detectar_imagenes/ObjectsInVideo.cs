using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_Servicio_detectar
{
    public  class ObjectsInVideo
    {
        public int PK_ObjectsInVideo { get; set; }
        public string video_to_detect { get; set; }
        public int score { get; set; }
        public int PK_ObjectInVideoFolder { get; set; }



    }
}
