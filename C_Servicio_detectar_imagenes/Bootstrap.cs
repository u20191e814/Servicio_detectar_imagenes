using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Servicio_detectar_imagenes
{
    public enum BootstrapStyle
    {
        Default,
        Alert
    }

    public enum BootstrapType
    {
        Default,
        Success,
        Info,
        Warning,
        Danger,
        Magenta,
        Cobalt
    }

    public class Bootstrap
    {
        static Random TypeWriter = new Random();

        static void Customize(BootstrapStyle style, BootstrapType type)
        {
            switch (type)
            {
                case BootstrapType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }
                    break;
                case BootstrapType.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                    }
                    break;
                case BootstrapType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }
                    break;
                case BootstrapType.Danger:
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    break;
                case BootstrapType.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                    break;
                case BootstrapType.Cobalt:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (style == BootstrapStyle.Alert)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    break;
            }
        }

        public static void Write(string format, int min = 50, int max = 100, params object[] args)
        {
            format = String.Format(format, args);
            for (int i = 0; i < format.Length; i++)
            {
                Thread.Sleep(TypeWriter.Next(min, max));
                Console.Write(format.Substring(i, 1));
            }
        }

        public static void WriteSetCursorPosition(string format, BootstrapStyle style, BootstrapType type, params object[] args)
        {
            Customize(style, type);
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;
            Console.CursorVisible = false;
            Console.Write(String.Format(format, args));
            Console.SetCursorPosition(currentLeft, currentTop);
            Console.ResetColor();
        }

        public static void Write(string format, BootstrapStyle style, BootstrapType type, params object[] args)
        {
            Customize(style, type);
            Console.Write(String.Format(format, args));
            Console.ResetColor();
        }

        public static void Write(string format, int min, int max, BootstrapStyle style, BootstrapType type, params object[] args)
        {
            Customize(style, type);
            Write(format, min, max, args);
            Console.ResetColor();
        }

        public static void WriteLine(string format, int min = 50, int max = 100, params object[] args)
        {
            Write(format, min, max, args);
            Console.WriteLine();
        }


        public static void WriteLine(string format, BootstrapStyle style, BootstrapType type, params object[] args)
        {
            Write(format, style, type, args);
            Console.WriteLine();
        }

        public static void WriteLine(string format, int min, int max, BootstrapStyle style, BootstrapType type, params object[] args)
        {
            Write(format, min, max, style, type, args);
            Console.WriteLine();
        }

    }

}
