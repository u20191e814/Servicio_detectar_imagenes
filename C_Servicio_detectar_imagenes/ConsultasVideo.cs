using Dapper;
using M_Servicio_detectar;
using M_Servicio_detectar_imagenes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Servicio_detectar_imagenes
{
    public class ConsultasVideo
    {
        private static EventViewer evento { get; set; }
        private static string cadenaSqlConexion { get; set; }

        public static List<ObjectsInVideo> ObtenerRegistrosPendientes()
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                List<ObjectsInVideo> lista = null;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " select top 100 PK_ObjectsInVideo, video_to_detect, score from [Deteccion_Motos].[dbo].[ObjectsInVideo] where video_status=0";
                    lista = cn.Query<ObjectsInVideo>(squery).ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ObtenerRegistrosPendientes: " + ex.Message);
                return null;
            }
        }
        public static bool ActualizarEstado(int PK_ObjectsInVideo, int video_status)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " update [Deteccion_Motos].[dbo].[ObjectsInVideo] set video_status=@video_status  where PK_ObjectsInVideo=@PK_ObjectsInVideo";
                    var param = new DynamicParameters();
                    param.Add("@PK_ObjectsInVideo", PK_ObjectsInVideo);
                    param.Add("@video_status", video_status);
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
        public static bool InsertarObjeto(int x, int y, int w, int h, int score, string classname, string colorrgb, int seg, int PK_ObjectsInVideo)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " insert into [Deteccion_Motos].[dbo].[ObjectInVideo_Details] (x,y,w,h,score,classname,colorrgb, FK_ObjectsInVideo, secs ) " +
                        "values (@x,@y,@w,@h,@score,@classname,@colorrgb, @FK_ObjectsInVideo, @secs)";
                    var param = new DynamicParameters();
                    param.Add("@x", x);
                    param.Add("@y", y);
                    param.Add("@w", w);
                    param.Add("@h", h);
                    param.Add("@colorrgb", colorrgb);
                    param.Add("@score", score);
                    param.Add("@classname", classname);
                    param.Add("@FK_ObjectsInVideo", PK_ObjectsInVideo);
                    param.Add("@secs", seg);
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

        public static bool InsertarObjetoFolder(int x, int y, int w, int h, int score, string classname, string colorrgb, int seg, int FK_ObjectInVideoFolder)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "  insert into [Deteccion_Motos].[dbo].[ObjectInVideo_Details] (x,y,w,h,score,classname,colorrgb, FK_ObjectInVideoFolder, secs ) "+
                        "           values(@x, @y, @w, @h, @score, @classname, @colorrgb, @FK_ObjectInVideoFolder, @secs)";
                    var param = new DynamicParameters();
                    param.Add("@x", x);
                    param.Add("@y", y);
                    param.Add("@w", w);
                    param.Add("@h", h);
                    param.Add("@colorrgb", colorrgb);
                    param.Add("@score", score);
                    param.Add("@classname", classname);
                    param.Add("@FK_ObjectInVideoFolder", FK_ObjectInVideoFolder);
                    param.Add("@secs", seg);
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

        public static bool ActualizarResultado(string video_to_result, int PK_ObjectsInVideo)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "update [Deteccion_Motos].[dbo].[ObjectsInVideo] set video_to_result=@video_to_result where PK_ObjectsInVideo=@PK_ObjectsInVideo";
                    var param = new DynamicParameters();
                    param.Add("@PK_ObjectsInVideo", PK_ObjectsInVideo);
                    param.Add("@video_to_result", video_to_result);
                    cn.Execute(squery, param);
                }
                return true;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarVideo: " + ex.Message);
                return false;
            }
        }
        public static bool ActualizarVideo(string duracion, int PK_ObjectsInVideo)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                bool estado = false;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " update [Deteccion_Motos].[dbo].[ObjectsInVideo] set duration=@duration where PK_ObjectsInVideo=@PK_ObjectsInVideo";
                    var param = new DynamicParameters();
                    param.Add("@duration", duracion);
                    param.Add("@PK_ObjectsInVideo", PK_ObjectsInVideo);
                    cn.Execute(squery, param); 
                    estado = true;
                }
                return estado;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarVideo: " + ex.Message);
                return false;
            }
        }

        public static List<dynamic> ValidarInfo(string clase, int segundo, int pK_ObjectsInVideo)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
              
                List<dynamic> list = new List<dynamic>();   
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "select x,y,w,h from [Deteccion_Motos].[dbo].[ObjectInVideo_Details] " +
                        "where FK_ObjectsInVideo=@FK_ObjectsInVideo and secs=@secs  and classname =@classname";
                    var param = new DynamicParameters();
                    param.Add("@FK_ObjectsInVideo", pK_ObjectsInVideo);
                    param.Add("@secs", segundo);
                    param.Add("@classname", clase);
                    list  =cn.Query<dynamic>(squery, param).ToList();
                   
                   
                }
                return list;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ValidarInfo: " + ex.Message);
                return null;
            }
        }

        public static List<ObjectsInVideo> ObtenerRegistrosPendientesFolder()
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                List<ObjectsInVideo> lista = null;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "   select top 100 PK_ObjectInVideoFolder, video_to_detect, score from [Deteccion_Motos].[dbo].[ObjectInVideoFolder] where video_status=0";
                    lista = cn.Query<ObjectsInVideo>(squery).ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ObtenerRegistrosPendientesFolder: " + ex.Message);
                return null;
            }
        }

        public static bool ActualizarVideoFolder(string duracion, int pK_ObjectInVideoFolder)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                bool estado = false;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "  update [Deteccion_Motos].[dbo].[ObjectInVideoFolder] set duration=@duration where PK_ObjectInVideoFolder=@PK_ObjectInVideoFolder";
                    var param = new DynamicParameters();
                    param.Add("@duration", duracion);
                    param.Add("@PK_ObjectInVideoFolder", pK_ObjectInVideoFolder);
                    cn.Execute(squery, param);
                    estado = true;
                }
                return estado;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarVideoFolder: " + ex.Message);
                return false;
            }
        }

        public static bool ActualizarEstadoFolder(int pK_ObjectInVideoFolder, int video_status)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "  update  [Deteccion_Motos].[dbo].[ObjectInVideoFolder] set video_status=@video_status  where PK_ObjectInVideoFolder=@PK_ObjectInVideoFolder";
                    var param = new DynamicParameters();
                    param.Add("@PK_ObjectInVideoFolder", pK_ObjectInVideoFolder);
                    param.Add("@video_status", video_status);
                    cn.Execute(squery, param);
                }
                return true;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarEstadoFolder: " + ex.Message);
                return false;
            }
        }

        public static List<dynamic> ValidarInfoFolder(string clase, int segundo, int pK_ObjectInVideoFolder)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                List < dynamic > lista = new List<dynamic>();   
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " select  x,y,w,h from [Deteccion_Motos].[dbo].[ObjectInVideo_Details]  " +
                        " where FK_ObjectInVideoFolder=@FK_ObjectInVideoFolder and secs=@secs  and classname =@classname";
                    var param = new DynamicParameters();
                    param.Add("@FK_ObjectInVideoFolder", pK_ObjectInVideoFolder);
                    param.Add("@secs", segundo);
                    param.Add("@classname", clase);
                    lista= cn.Query<dynamic>(squery, param).ToList();
                   

                }
                return lista;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ValidarInfoFolder: " + ex.Message);
                return null;
            }
        }

        public static bool ActualizarResultadoFolder(string rutasalidaMp4, int pK_ObjectInVideoFolder)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;

                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = " update [Deteccion_Motos].[dbo].[ObjectInVideoFolder] set video_to_result=@video_to_result where PK_ObjectInVideoFolder=@PK_ObjectInVideoFolder";
                    var param = new DynamicParameters();
                    param.Add("@PK_ObjectInVideoFolder", pK_ObjectInVideoFolder);
                    param.Add("@video_to_result", rutasalidaMp4);
                    cn.Execute(squery, param);
                }
                return true;
            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("ActualizarVideo: " + ex.Message);
                return false;
            }
        }

        public static void EliminarRegistros(int pK_ObjectsInVideo)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "  delete from [Deteccion_Motos].[dbo].[ObjectInVideo_Details] where FK_ObjectsInVideo=@FK_ObjectsInVideo";
                    var param = new DynamicParameters();
                    param.Add("@FK_ObjectsInVideo", pK_ObjectsInVideo);                   
                    cn.Execute(squery, param);
                }

            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("EliminarRegistros: " + ex.Message);
            }
        }

        public static void EliminarRegistrosFolder(int pK_ObjectInVideoFolder)
        {
            try
            {
                evento = new EventViewer();
                cadenaSqlConexion = evento.config.SqlConexion;
                using (SqlConnection cn = new SqlConnection(cadenaSqlConexion))
                {
                    string squery = "  delete from [Deteccion_Motos].[dbo].[ObjectInVideo_Details] where FK_ObjectInVideoFolder=@FK_ObjectInVideoFolder";
                    var param = new DynamicParameters();
                    param.Add("@FK_ObjectsInVideo", pK_ObjectInVideoFolder);
                    cn.Execute(squery, param);
                }

            }
            catch (Exception ex)
            {
                evento.WriteErrorLog("EliminarRegistrosFolder: " + ex.Message);
            }
        }
    }
}
