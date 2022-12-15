using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Humatrix_Transfer
{
    class GlobalConfig
    {
        public static string rootPath = ConfigurationManager.AppSettings["ROOT_PATH"];
        public static string destinationPath = ConfigurationManager.AppSettings["DESTINATION_PATH"];
        public static string backupPath = ConfigurationManager.AppSettings["BACKUP_PATH"];
        public static string allowExtension = ConfigurationManager.AppSettings["ALLOW_EXTENSION"];
        public static string loggingPath = ConfigurationManager.AppSettings["LOG_PATH"];
    }
}
