using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmlTool
{
    class Program
    {
        private static string Path = @"C:\Users\dr186\Desktop\uml";

        private static string path2 = @"D:\WorkSpace\CrazyServer\CrazyFrameProject";
        static void Main(string[] args)
        {

            DFS(path2);

            Console.WriteLine("解析完毕");
            Console.ReadKey();

        }

        static void DFS(string path)
        {
            


            var files = Directory.GetFiles(path);
          

            foreach (var file in files)
            {
                //Console.WriteLine(file);
                if (file.Contains("cs")&&file.Last()=='s')
                {
                    var txt = File.ReadAllText(file);

                    var newFile = Path + file.Substring(path2.Length);
                    int j = 0;
                    for (int i = 0; i < newFile.Length; i++)
                    {
                        if (newFile[i] == '\\')
                        {
                            j = i;
                        }
                    }

                    Directory.CreateDirectory(newFile.Substring(0, j));
                    var fs =new FileStream(newFile,FileMode.Create,FileAccess.Write);
                    byte[] b = Encoding.GetEncoding(65001).GetBytes(txt);
                    //byte[] b = Encoding.UTF8.GetBytes(txt);
                    fs.Write(b,0,b.Length);
                    //File.WriteAllText(newFile, txt, Encoding.UTF8);
                    fs.Close();

                }
            }
            var dirs = Directory.GetDirectories(path);


            foreach (var p in dirs)
            {
                DFS(p);
            }




        }
    }
}
