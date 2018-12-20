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
            Console.Write("请输入要查询的目录:   ");
            string dir = Console.ReadLine();
            Console.Write("请输入需要导出的CSV文件地址（含文件名）：  ");
            string CsvPath = Console.ReadLine();
            try
            {
                ListFiles(new DirectoryInfo(dir),CsvPath);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("done");

            }
        }
        public static void ListFiles(FileSystemInfo info,string csvpath)
        {
            if (!info.Exists) return;

            DirectoryInfo dir = info as DirectoryInfo;
            //不是目录 
            if (dir == null) return;


            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                    SaveCSV(csvpath, file.FullName);
                //Console.WriteLine(file.FullName + "\t " + file.Length);
                //对于子目录，进行递归调用 
                else
                    ListFiles(files[i],csvpath);
            }
        }
        public static bool SaveCSV(string fullPath, string Data)
        {
            bool re = true;
            try
            {
                FileStream FileStream = new FileStream(fullPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(FileStream, System.Text.Encoding.UTF8);
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

    }
}

