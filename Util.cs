using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static Humatrix_Transfer.GlobalConfig;

namespace Humatrix_Transfer
{
    enum LogType
    {
        ADD_TO_QUEUE,
        QUEUE_PROCESS
    }

    class Util
    {
        public Util()
        {

        }

        public static bool isDirectoryEmpty(string rootPathIn)
        {
            string[] fileExtension = allowExtension.ToLower().Split(',');
            DirectoryInfo dir = new DirectoryInfo(rootPathIn);
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => fileExtension.Contains(f.Extension.ToLower())).Count() == 0;
        }

        public static void MoveToTargetFolder(string file, string fileName, string workPath, LogType logType = LogType.ADD_TO_QUEUE)
        {
            try
            {
                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }
                File.Move(file, Path.Combine(workPath, fileName));
            }
            catch (Exception ex)
            {
                WriteToFile("ERR:" + ex.Message, logType);
            }
        }

        public static void CopyToBackUp(string file, string fileName, string rootPathBackUp, LogType logType = LogType.ADD_TO_QUEUE)
        {
            try
            {
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string path = Path.Combine(rootPathBackUp, currentYear, currentMonth);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.Copy(file, Path.Combine(path, fileName));
            }
            catch (Exception ex)
            {
                WriteToFile("ERR:" + ex.Message, logType);
            }
        }

        public static void WriteToFile(string Message, LogType logType = LogType.ADD_TO_QUEUE)
        {
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("th-TH");
            string LogPath = Path.Combine(loggingPath, (logType == LogType.ADD_TO_QUEUE ? "FileService" : "TaskQueue"));
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string filepath = LogPath + "\\ServiceLog_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(localDate.ToString(culture) + " " + Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(localDate.ToString(culture) + " " + Message);
                }
            }
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions(string _dir, string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            DirectoryInfo dir = new DirectoryInfo(_dir);
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension.ToLower()));
        }
    }
}
