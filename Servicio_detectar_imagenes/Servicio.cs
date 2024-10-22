using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Reg;
using System.Drawing.Imaging;
using Emgu.CV.CvEnum;
using C_Servicio_detectar_imagenes;
using M_Servicio_detectar_imagenes;

namespace Servicio_detectar_imagenes
{
    public  class Servicio
    {
        private readonly System.Timers.Timer _timer;


        private static EventViewer evento { get; set; }
        private Configuracion confi {  get; set; }

        private static bool EstadoPlay { get; set; }
       

        public Servicio()
        {
            _timer = new System.Timers.Timer(1000)
            {
                AutoReset = false
            };
            _timer.Elapsed += Tiempo_Elapsed;


        }

        private int contadorVacio = 0;
        private void Tiempo_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                List<MCvScalar> colores = new List<MCvScalar>();
                colores.Add(new MCvScalar(0, 0, 255));
                colores.Add(new MCvScalar(0, 255, 255));
                colores.Add(new MCvScalar(255, 0, 0));
                string[] classNames = File.ReadAllLines("obj.names");
                Net net = DnnInvoke.ReadNetFromDarknet("yolov4_Train_416.cfg", "yolov4_Train_final_416.weights");

                net.SetPreferableBackend(Emgu.CV.Dnn.Backend.OpenCV);
                net.SetPreferableTarget(Target.Cpu);
                while (EstadoPlay)
                {
                    bool viewftp = false;
                    try
                    {

                        List<ObjectsInImage> photos = Consultas.ObtenerRegistrosPendientes();
                        if (photos != null && photos.Count() > 0)
                        {
                            contadorVacio = 0;
                            Message.Success("Se han encontrado " + photos.Count() + " a ser eliminados");

                            foreach (var item in photos)
                            {

                                DateTime inicio = DateTime.Now;
                                
                                byte[] bit = Utils.ConvertBase64ToBitmap(item.image_to_detect);
                                if (bit==null)
                                {
                                    evento.WriteErrorLog("No se pudo recuperar Id:"+ item.PK_ObjectsInImage);
                                    continue;
                                }
                                Mat frame = new Mat();
                                CvInvoke.Imdecode(bit, ImreadModes.AnyColor,frame);
                                Consultas.ActualizarEstado(item.PK_ObjectsInImage, 2);
                                //Mat mat = BitmapConverter.ToMat(bit);
                                //string archivo = item.PK_ObjectsInImage + ".png";
                                //bit.Save(archivo);
                                //Mat frame = new Mat(archivo);
                                //Image<Bgr, byte> image = new Image<Bgr, byte>(bit);

                                Mat blob = DnnInvoke.BlobFromImage(frame, 1 / 255.0, new Size(416, 416), new MCvScalar(0, 0, 0), true, false);
                                VectorOfMat layerOutputs = new VectorOfMat();
                                string[] outNames = net.UnconnectedOutLayersNames;

                                net.SetInput(blob);
                                net.Forward(layerOutputs, outNames);
                                blob.Dispose();

                                List<Rectangle> boxes = new List<Rectangle>();
                                List<float> confidences = new List<float>();
                                List<int> classIDs = new List<int>();
                                float ConfidenceThreshold = item.score/100f;
                                int width = frame.Width;
                                int height = frame.Height;
                                for (int k = 0; k < layerOutputs.Size; k++)
                                {
                                    float[,] lo = (float[,])layerOutputs[k].GetData();
                                    int len = lo.GetLength(0);
                                    for (int i = 0; i < len; i++)
                                    {
                                        if (lo[i, 4] < ConfidenceThreshold)
                                            continue;
                                        float max = 0;
                                        int idx = 0;

                                        int len2 = lo.GetLength(1);
                                        for (int j = 5; j < len2; j++)
                                            if (lo[i, j] > max)
                                            {
                                                max = lo[i, j];
                                                idx = j - 5;
                                            }

                                        if (max > ConfidenceThreshold)
                                        {
                                            lo[i, 0] *= width;
                                            lo[i, 1] *= height;
                                            lo[i, 2] *= width;
                                            lo[i, 3] *= height;

                                            int x = (int)(lo[i, 0] - (lo[i, 2] / 2));
                                            int y = (int)(lo[i, 1] - (lo[i, 3] / 2));

                                            var rect = new Rectangle(x, y, (int)lo[i, 2], (int)lo[i, 3]);

                                            rect.X = rect.X < 0 ? 0 : rect.X;
                                            rect.X = rect.X > width ? width - 1 : rect.X;
                                            rect.Y = rect.Y < 0 ? 0 : rect.Y;
                                            rect.Y = rect.Y > height ? height - 1 : rect.Y;
                                            rect.Width = rect.X + rect.Width > width ? width - rect.X - 1 : rect.Width;
                                            rect.Height = rect.Y + rect.Height > height ? height - rect.Y - 1 : rect.Height;

                                            boxes.Add(rect);
                                            confidences.Add(max);
                                            classIDs.Add(idx);
                                        }
                                    }
                                }
                            
                                int[] bIndexes = DnnInvoke.NMSBoxes(boxes.ToArray(), confidences.ToArray(), ConfidenceThreshold, ConfidenceThreshold);

                                if (bIndexes.Length > 0)
                                {
                                    foreach (var idx in bIndexes)
                                    {
                                        var rc = boxes[idx];

                                        double ccn = Math.Round(confidences[idx], 4);
                                        int id = classIDs[idx];
                                        string clase = classNames[classIDs[idx]];
                                        var color = colores[id];
                                        CvInvoke.Rectangle(frame, boxes[idx], color, 2);
                                        int score=(int) (confidences[idx]*100);
                                        string colrs ="rgb("+ color.V2 + ","+ color.V1 +","+ color.V0+")";

                                        Consultas.InsertarObjeto(rc.X,rc.Y, rc.Width, rc.Height,score, clase,colrs ,item.PK_ObjectsInImage );
                                    }
                                }
                                layerOutputs.Dispose();
                                Bitmap bitmap = new Bitmap(frame.Cols, frame.Rows, frame.Step, PixelFormat.Format24bppRgb, frame.DataPointer);
                                string base64String = string.Empty;
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    bitmap.Save(ms, ImageFormat.Jpeg);
                                    byte[] imageBytes = ms.ToArray();
                                    base64String = Convert.ToBase64String(imageBytes);

                                }

                                //ImageConverter converter = new ImageConverter();
                                //byte[] imageData = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
                                Consultas.ActualizarImagenResultante(item.PK_ObjectsInImage, base64String);
                                Consultas.ActualizarEstado(item.PK_ObjectsInImage, 1);
                                frame.Dispose();
                              
                                DateTime fin = DateTime.Now;
                                TimeSpan diff = fin - inicio;
                                Message.warning("Duración: " + diff.ToString(@"hh\:mm\:ss\.fff"));
                                Message.Success("FIN DEL PROCESO");
                            }
                        }
                        else
                        {
                            contadorVacio++;
                            if (contadorVacio > 20)
                            {
                                contadorVacio = 0;
                                viewftp = true;
                                Console.Clear();
                            }
                            Message.warning("No se existen imagenes para procesar");
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Error("While", ex);
                    }
                    if (viewftp)
                    {
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                }
            }
            catch (Exception ex)
            {

                Message.Error("Periodo tiempo", ex);
            }

        }


        private bool IniciarAplicacion()
        {
            bool activacion = false;
            try
            {

                activacion = true;
                Message.Success("Aplicación exitosa");

            }
            catch (Exception ex)
            {
                Message.Error("Error en Iniciar aplicacion  " + ex.Message + ". (Error: 108000070)");


            }
            return activacion;
        }




        public void Start()
        {

            evento = new EventViewer();

            evento.WriteErrorLog("Inicio de aplicación ");
            evento.WriteEventViewerLog("Inicio de aplicación ", 4);
            confi = evento.config;
            bool estado = IniciarAplicacion();
            if (estado)
            {
                _timer.Start();
                EstadoPlay = true;
            }


        }
        public void Stop()
        {
            EstadoPlay = false;
            _timer.Stop();
        }
    }
}
