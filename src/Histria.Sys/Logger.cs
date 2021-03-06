﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Histria.Sys
{
   
    public static class Logger
    {
        public static string DB = "DB";
        public static string SQL = "DB.SQL";
        public static string DBMAP = "DB.MAP";
        public static string PLUGIN = "SYS.PLUGIN";
        public static string MODEL = "MODEL";
        public static string RULES = "MODEL.RULES";
        public static string PROXY_LOAD = "PROXY.LOAD";

        public static void Error(string module, string message)
        {
            WriteEntry(message, "error", module);
        }

        public static void Error(string module, Exception ex)
        {
            WriteEntry(ex.Message, "error", module);
        }

        public static void Warning(string module, string message)
        {
            WriteEntry(message, "warning", module);
        }

        public static void Info(string module, string message, double interval = 0)
        {
            WriteEntry(message, "info", module, interval);
        }

        private static void WriteEntry(string module, string message, string type, double interval = 0)
        {
            Trace.WriteLine(string.Format("{0}:{1}:{2}:{3}", type, interval.ToString("#.0"), module, message));
        }
    }
}
