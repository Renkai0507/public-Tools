using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TransExpiredEMR.Tools
{
    public class LogTool
    {
        StreamWriter writer;
        string logFilePath;
        string FileName;
        public LogTool()
        {
            logFilePath = System.Environment.CurrentDirectory + $@"\Logs";
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }
            FileName = logFilePath + "\\" + $"({DateTime.Now.ToString("yyyyMMddhhmm")})" + ".txt";
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }
            
        }

        public void SetLog(string log)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(FileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            writer = new StreamWriter(FileName, true, System.Text.Encoding.GetEncoding("UTF-8"));
            
            writer.WriteLine(log);
            writer.Dispose();
        }
    }
}
