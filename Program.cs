using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Humatrix_Transfer.GlobalConfig;

namespace Humatrix_Transfer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Util.isDirectoryEmpty(rootPath))
            {
                Util.WriteToFile("LOG:---------- Start Process ----------");
                IEnumerable<FileInfo> files = Util.GetFilesByExtensions(rootPath, allowExtension.ToLower().Split(','));

                foreach (FileInfo file in files)
                {
                    try
                    {
                        Console.WriteLine(file.FullName);
                        Util.WriteToFile("LOG: Has file in directory");
                        Util.WriteToFile("LOG: Processing: " + file.FullName);
                        Util.WriteToFile("LOG: Moved to backup folder: " + file.FullName);
                        Util.CopyToBackUp(Path.Combine(file.Directory.ToString(), file.Name), file.Name, backupPath);
                        Util.WriteToFile("LOG: Moved to destination folder: " + file.FullName);
                        string des = Path.Combine(destinationPath, file.Extension.ToUpper().Replace(".", ""));

                        if (!Directory.Exists(des))
                        {
                            Directory.CreateDirectory(des);
                        }

                        Util.MoveToTargetFolder(file.FullName, file.Name, des);
                        
                        Util.WriteToFile("LOG: Moved Successfully: " + file.FullName);
                    }
                    catch (Exception ex)
                    {
                        Util.WriteToFile("ERR:" + ex.Message);
                    }
                }
                Util.WriteToFile("LOG:---------- End Process ----------");
            }
        }
    }
}
