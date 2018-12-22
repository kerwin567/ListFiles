using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ListFileS
{
    class ListAllFiles
    {

        public static void Main(string[] args)
        {
            try
            {
                //调用GetProjParantPath()函数，取当前exe文件的上层文件夹
                string directory = GetProjParantPath();
                //创建文件夹对象实例 dir
                DirectoryInfo dir = new DirectoryInfo(directory);
                //检查文件夹是否存在
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException("The directory does not exist.");
                }

                //创建动态csv文件，ListAllFiles_xing_当前时间.csv hh=12小时制，HH=24小时制
                //string csvPath = AppDomain.CurrentDomain.BaseDirectory + "\\CsvFile\\" + "ListAllFiles_xing_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".csv";

                //创建静态csv文件,如果文件夹内csv文件存在，删除文件
                string csvPath = AppDomain.CurrentDomain.BaseDirectory + "\\CsvFile\\" + "ListAllFiles_xing.csv";
                FileInfo csvInfo = new FileInfo(csvPath);
                if (csvInfo.Exists)
                {
                    csvInfo.Delete();
                }
                //将外部参数组arges[]中的第一个参数args[0]赋值给search

                string search = "";
                if (args == null || args.Length < 1)
                {
                    search = "*";
                }
                else
                {
                    search = args[0];
                }
                Console.Write("请选择功能：" + "\n" + "1.导出文件夹内所有文件的文件名至csv" + "\n" + "2.删除所有文件夹内的某个文件" + "\n");
                int chose1 = Console.ReadKey().KeyChar;
                Console.WriteLine("\n"+"请输入需要操作的文件条件： "+"\n"+"例如："+"\n"+"*.pdf"+"\n"+"*.xls"+"\n"+"*.jpg");
                search = Console.ReadLine();
                DirectoryInfo[] directoryInfos = dir.GetDirectories();
                FileInfo[] infos = dir.GetFiles(search);
                switch (chose1)
                {
                    case 49:

                        Console.WriteLine("Working...");
                        ListDirectoriesAndFiles(directoryInfos, infos, csvPath, search);
                        break;
                    case 50:
                        Console.WriteLine("Working...");
                        DeleteFiles(directoryInfos, infos, csvPath, search);
                        break;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                while (true)
                { }
            }
            finally
            {
                Console.WriteLine("Done");
                while (true)
                { }
            }
        }

        //参数：1.文件夹中的所有子文件夹 2.当前文件夹中符合条件的文件 3.创建CSV文件的目录 4.搜索文件的条件
        //使用该函数时，需要调用：          
        //DirectoryInfo[] directoryInfos = dir.GetDirectories();//读取子目录 存到directoryInfos[]数组里
        //FileInfo[] infos = dir.GetFiles(search);//返回当前目录中符合search的文件

        public static void ListDirectoriesAndFiles(DirectoryInfo[] directoryInfos, FileInfo[] infos, string CsvPath, string SearchString = "*")
        {
            //不是目录 
            if (infos == null) return;
            if (SearchString == null || SearchString.Length == 0)
            {
                SearchString = "*";
            }
            foreach (DirectoryInfo i in directoryInfos)
            {
                //过滤系统和隐藏文件
                if (((i.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((i.Attributes & FileAttributes.System) != FileAttributes.System))
                {
                    //DirectoryInfo dInfo = (DirectoryInfo)i;

                    // Iterate through all sub-directories.
                    ListDirectoriesAndFiles(i.GetDirectories(), i.GetFiles(SearchString), CsvPath, SearchString);
                }
            }
            foreach (FileInfo i in infos)
            {
                //过滤系统和隐藏文件
                if (((i.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((i.Attributes & FileAttributes.System) != FileAttributes.System))
                {
                    string data = i.Name + "," + i.FullName+","+i.LastWriteTime;
                    // Save to csv.
                    SaveCSV(CsvPath, data);
                }                
            }
        }
        public static void DeleteFiles(DirectoryInfo[] directoryInfos, FileInfo[] infos, string CsvPath, string SearchString = "*")
        {
            if (infos == null) return;
            if (SearchString == "*") return;

            foreach (DirectoryInfo i in directoryInfos)
            {
                //过滤系统和隐藏文件
                if (((i.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((i.Attributes & FileAttributes.System) != FileAttributes.System))
                {
                    DirectoryInfo dInfo = (DirectoryInfo)i;

                    // Iterate through all sub-directories.
                    DeleteFiles(dInfo.GetDirectories(), dInfo.GetFiles(SearchString), SearchString);
                }
            }
            foreach (FileInfo i in infos)
            {
                //过滤系统和隐藏文件
                if (((i.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((i.Attributes & FileAttributes.System) != FileAttributes.System))
                {
                    string data = i.Name + "," + i.FullName + "," + i.LastWriteTime;
                    // Save to csv.
                    SaveCSV(CsvPath, data);
                    FileSystem.DeleteFile(i.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            }
        }

        //循环删除文件夹内所有符合条件的文件
        public static void Deleaa(FileSystemInfo[] infos, string delefilename)
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
                    FileSystem.DeleteFile(i.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            }
        }
        //存储内容到csv文件
        public static bool SaveCSV(string fullPath, string Data)
        {
            bool re = true;
            try
            {
                FileStream fileStream = new FileStream(fullPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.Default);
                sw.WriteLine(Data);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fileStream.Close();
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
                re = false;
            }
            return re;
        }
        //获取当前应用程序的上一级路径
        public static string GetProjParantPath()
        {
            //string ParantPath = "";
            //获取当前应用程序路径
            string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            ////分解路径法获得父级路径
            //string tempPath = baseDirectoryPath.Substring(0, baseDirectoryPath.LastIndexOf("\\"));
            //string parantPath = baseDirectoryPath.Substring(0, tempPath.LastIndexOf("\\"));
            //return parantPath;
            //directoryinfo类的getparant法获得父级路径
            DirectoryInfo baseInfo = new DirectoryInfo(baseDirectoryPath);
            DirectoryInfo parantInfo = baseInfo.Parent;
            return parantInfo.FullName;
        }
    }
}


