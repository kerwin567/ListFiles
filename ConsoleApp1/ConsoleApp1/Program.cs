using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch_Console
{
    class ListAllFilesDemo
    {
        public static void Main(string[] args)
        {
            Console.Write("请输入要查询的目录:   ");
            string dir = Console.ReadLine();
            try
            {
                ListFiles(new DirectoryInfo(dir));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                while (true) { };
            }
        }
        public static void ListFiles(FileSystemInfo info)
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
                    Console.WriteLine(file.FullName + "\t " + file.Length);
                //对于子目录，进行递归调用 
                else
                    ListFiles(files[i]);

            }
        }
    }
}

