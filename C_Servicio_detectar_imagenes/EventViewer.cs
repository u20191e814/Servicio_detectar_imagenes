using M_Servicio_detectar_imagenes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace C_Servicio_detectar_imagenes
{
    public  class EventViewer
    {
        public  Configuracion config { get; set; }
        private string nombre = "Configuracion.json";
        public EventViewer()
        {
            if (File.Exists(nombre))
            {
                string lectura = File.ReadAllText(nombre);
                config= JsonConvert.DeserializeObject<Configuracion>(lectura);
            }
        }
        public void WriteEventViewerLog(string Message, int EventType)
        {
            try
            {
                if (config.WriteLogInWindowsEvents)
                {
                    string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                    EventLog.WriteEntry(AppName, string.Format("{0} {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss K"), Message), (EventLogEntryType)EventType);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se necesita permisos para guardar los logs en windows ");
            }

        }

        public void WriteErrorLog(string Message)
        {
            try
            {
                if (config.WriteLogInDirectory)
                {
                    StreamWriter sw = null;

                    string LogDirectory = string.Empty;
                    if (string.IsNullOrEmpty(config.LogDirectory))
                        LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                    else
                        LogDirectory = Path.Combine(config.LogDirectory);

                    if (!Directory.Exists(LogDirectory))
                        Directory.CreateDirectory(LogDirectory);
                    string rutaLog = string.Format("{0}\\{1}Log {2}.txt", LogDirectory, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, DateTime.Now.ToString("dd-MM-yyyy"));
                    bool existenciaArchivo = File.Exists(rutaLog);
                    sw = new StreamWriter(rutaLog, true);
                    if (!existenciaArchivo)
                    {
                        string mensaje = "Log creado con nombre " + Path.GetFileName(rutaLog);
                        sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss K") + ": " + mensaje);
                        if (config.WriteLogInWindowsEvents)
                        {
                            string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                            EventLog.WriteEntry(AppName, string.Format("{0} {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss K"), mensaje), EventLogEntryType.Information);
                        }

                    }
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss K") + ": " + Message);
                    sw.Flush();
                    sw.Close();

                    DirectoryInfo info = new DirectoryInfo(LogDirectory);
                    List<FileInfo> files = info.GetFiles("*.txt", SearchOption.AllDirectories).OrderBy(p => p.CreationTime).ToList();
                    if (files.Count > config.LogDirectoryMaxDays)
                    {
                        foreach (FileInfo item in files)
                        {
                            if (DateTime.Now.Subtract(item.CreationTime).TotalDays > config.LogDirectoryMaxDays)
                                item.Delete();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se necesita permisos para guardar los logs en directorio ");
            }



        }
    }
    
}
