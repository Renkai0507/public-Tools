using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class IOhelper
    {
        string mainpath;
        string awaypath;

        public static List<string> getallfilename(string filepath)
        {
            List<string> Allfile = new List<string>();
            DirectoryInfo Dinfo = new DirectoryInfo(filepath);
            foreach (var fi in Dinfo.GetFiles())
            {
                Allfile.Add(fi.Name);
            }
            return Allfile;
        }
        public static bool FileExists(string FilePath)
        {
            FileInfo file1 = new FileInfo(FilePath);

            if (file1.Exists == true)
            {
                return true;//表示檔案已經存在
            }
            else
            {
                return false;
            }
        }
        public static int Getfilecount(string path)
        {
            return Directory.GetFiles(path).Length;
        }

        public static void WriteContentToFileAppend(string FileContent, string FileName, Encoding encodeType)
        {
            FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);
            StreamWriter sw = new StreamWriter(fs, encodeType);
            sw.WriteLine(FileContent);
            sw.Close();
        }
        public static bool FileCreate(string FileName)
        {
            //string filePath = Path.GetTempFileName();//從暫存區隨機產生一個暫存檔

            FileInfo file1 = new FileInfo(FileName);

            if (file1.Exists == false)
            {
                var myfile= file1.Create();
                myfile.Close();
                return true;
            }
            else
            {
                return false;//表示檔案已經存在
            }

        }
        #region 檔案拷貝

        /// <summary>
        /// 檔案拷貝
        /// </summary>
        /// <param name="SourceFile">複製來源檔案</param>
        /// <param name="TargetFile">複製目的檔案</param>
        /// <returns>複製成功回傳True , 複製失敗回傳False</returns>
        public static bool FileCopy(string SourceFile, string TargetFile)
        {
            FileInfo file1 = new FileInfo(SourceFile);
            try
            {
                file1.CopyTo(TargetFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    #endregion
}
