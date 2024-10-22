using Dapper;
using M_Servicio_detectar_imagenes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace C_Servicio_detectar_imagenes
{
    public class Consultas
    {
        private static  EventViewer evento { get; set; }
        private static string cadenaSqlConexion {  get; set; }
       
        public static List<ObjectsInImage> ObtenerRegistrosPendientes()
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                List<ObjectsInImage> lista = null;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "select top 100 PK_ObjectsInImage, image_to_detect, score from [Deteccion_Motos].[dbo].[ObjectsInImage] where image_status=0";
                    lista = cn.Query<ObjectsInImage>(squery).ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ObtenerRegistrosPendientes: " + ex.Message);
                return null;
            }
        }
        public static bool ActualizarEstado(int PK_ObjectsInImage, int image_status)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
               
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " update [Deteccion_Motos].[dbo].[ObjectsInImage] set image_status=@image_status where PK_ObjectsInImage=@PK_ObjectsInImage";
                    var param = new DynamicParameters();
                    param.Add("@PK_ObjectsInImage", PK_ObjectsInImage);
                    param.Add("@image_status", image_status);
                    cn.Execute(squery, param);
                }
                return true;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarEstado: " + ex.Message);
                return false;
            }
        }
        public static bool ActualizarImagenResultante(int PK_ObjectsInImage, string image_base64)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " update [Deteccion_Motos].[dbo].[ObjectsInImage] set image_to_result=@image_to_result where PK_ObjectsInImage=@PK_ObjectsInImage";
                    var param = new DynamicParameters();
                    param.Add("@PK_ObjectsInImage", PK_ObjectsInImage);
                    param.Add("@image_to_result", image_base64);
                    cn.Execute(squery, param);
                }
                return true;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarEstado: " + ex.Message);
                return false;
            }
        }

        public static bool InsertarObjeto(int x, int y, int w, int h, int score,string classname,string colorrgb, int FK_ObjectsInImage)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " insert into [Deteccion_Motos].[dbo].[ObjectInImage_Details] (x,y,w,h,score,classname,colorrgb, FK_ObjectsInImage) " +
                        "values (@x,@y,@w,@h,@score,@classname,@colorrgb, @FK_ObjectsInImage)";
                    var param = new DynamicParameters();
                    param.Add("@x", x);
                    param.Add("@y", y);
                    param.Add("@w", w);
                    param.Add("@h", h);
                    param.Add("@colorrgb", colorrgb);
                    param.Add("@score", score);
                    param.Add("@classname", classname);
                    param.Add("@FK_ObjectsInImage", FK_ObjectsInImage);
                    cn.Execute(squery, param);
                }
                return true;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("InsertarObjeto: " + ex.Message);
                return false;
            }
        }
    }
}
