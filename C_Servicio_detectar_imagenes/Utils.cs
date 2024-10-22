using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Servicio_detectar_imagenes
{
    public  class Utils
    {
        public static byte[] ConvertBase64ToBitmap(string base64String)
        {
            
            // Decode the Base64 string
            byte[] imageData = Convert.FromBase64String(base64String);
            return imageData;
            // Create a MemoryStream
            //using (MemoryStream ms = new MemoryStream(imageData))
            //{
            //    // Create a Bitmap object
            //    return new Bitmap(ms);
            //}
        }
    }
}
