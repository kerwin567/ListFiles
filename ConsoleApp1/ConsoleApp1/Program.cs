using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FileSearch_Console
{
    class ListAllFilesDemo
    {

        public static void Main(string[] args)
        {
            try
            {
                //Console.Write("请输入要查询的目录:   ");
                //string directory = Console.ReadLine();
                //DirectoryInfo dir = new DirectoryInfo(directory);
                string directory = GetProjParantPath();
                DirectoryInfo dir = new DirectoryInfo(directory);
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException("The directory does not exist.");
                }

                
                Console.Write("请选择功能："+"\n"+"1.导出文件夹内所有文件的文件名至csv"+"\n"+"2.删除所有文件夹内的某个文件"+"\n");
                int chose1 = Console.ReadKey().KeyChar;

                switch (chose1)
                {
                    case 49:
                        Console.Write("\n"+"请输入需要导出的CSV文件地址（含文件名）：  ");
                        string csvPath = directory +"\\"+ Console.ReadLine();
                        Console.WriteLine("Enter a search string (for example *.xls): ");
                        string search = Console.ReadLine();

                        DirectoryInfo[] directoryInfos = dir.GetDirectories();
                        FileInfo[] infos = dir.GetFiles(search);
                        Console.WriteLine("Working...");
                        ListDirectoriesAndFiles(directoryInfos, infos, csvPath, search);
                        break;
                    case 50:
                        Console.Write("请输入文件名");
                        string delefilename = Console.ReadLine();
                        FileSystemInfo[] infos2 = dir.GetFileSystemInfos();
                        Deleaa(infos2, delefilename);
                        break;
                }              
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Done");

            }
        }
        public static void ListDirectoriesAndFiles(DirectoryInfo[] directoryInfos, FileInfo[] infos,string CsvPath,string SearchString="*")
        {
            //不是目录 
            if (infos == null) return;
            if (SearchString == null || SearchString.Length == 0)
            {
                SearchString = "*";
            }
            foreach (DirectoryInfo i in directoryInfos)
            {
                DirectoryInfo dInfo = (DirectoryInfo)i;

                // Iterate through all sub-directories.
                ListDirectoriesAndFiles(dInfo.GetDirectories(),dInfo.GetFiles(SearchString), CsvPath, SearchString);
            }
            foreach (FileInfo i in infos)
            {
                string data = i.Name + "," + i.FullName;
                // Save to csv.
                SaveCSV(CsvPath,data);
            }
        }
        public static void Deleaa(FileSystemInfo[] infos,string delefilename)
        {
            if (infos == null) return;
            if (delefilename == null || delefilename.Length == 0) return;
            foreach (FileSystemInfo i in infos)
            {
                // Check to see if this is a DirectoryInfo object.
                if (i is DirectoryInfo)
                {
                    // Cast the object to a DirectoryInfo object.
                    DirectoryInfo dInfo = (DirectoryInfo)i;
                    FileSystemInfo[] files = dInfo.GetFileSystemInfos();
                    // Iterate through all sub-directories.
                    Deleaa(files, delefilename);
                }
                // Check to see if this is a FileInfo object.
                else if (i is FileInfo)
                {
                    // delete file
                    i.Delete();
                }
            }
            //for (int i = 0; i < files.Length; i++)
            //{
            //    FileInfo file = files[i] as FileInfo;
            //    //是文件 
            //    if (file != null)
            //    {
            //        if(file.Name ==delefilename)
            //        {
            //            file.Delete();
            //        }
            //    }
            //    else
            //    {
            //        Deleaa(files[i], delefilename);
            //    }
            //SaveCSV(CsvPath, file.FullName);
            //Console.WriteLine(file.FullName + "\t " + file.Length);
            //对于子目录，进行递归调用 
            //else
            //ListFiles(files[i], CsvPath, SearchString);
            //}
        }
        public static bool SaveCSV(string fullPath, string Data)
        {
            bool re = true;
            try
            {
                FileStream FileStream = new FileStream(fullPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(FileStream, System.Text.Encoding.Default);
                sw.WriteLine(Data);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                FileStream.Close();
            }
            catch
            {
                re = false;
            }
            return re;
        }
        public static string GetProjParantPath()
        {
            //string ParantPath = "";
            //获取当前应用程序路径
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string TempPath = BaseDirectoryPath.Substring(0, BaseDirectoryPath.LastIndexOf("\\"));
            string ParantPath = BaseDirectoryPath.Substring(0, TempPath.LastIndexOf("\\"));
            return ParantPath;
        }

    }
}

