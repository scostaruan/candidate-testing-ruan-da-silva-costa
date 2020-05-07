using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Entities;
using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CandidateTesting.RuanDaSilvaCosta.AgoraConverter
{
    public class MinhaCdn : IMinhaCdn
    {
        private readonly string HTTP_VERSION = "HTTP/1.1";

        public List<LogFile> Log { get; private set; }
        public string LogPath { get; private set; }

        public MinhaCdn(List<LogFile> log, string logPath)
        {
            Log = log;
            LogPath = logPath;
        }

        public void CreateLog()
        {
            try
            {
                string pathFolder = Path.GetDirectoryName(LogPath);

                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }

                using (FileStream fileStream = File.Create(LogPath))
                {
                    foreach (LogFile item in Log)
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(string.Concat(item.ResponseSize, '|', item.StatusCode, '|', item.CacheStatus, '|',
                            '\"', item.HttpMethod, ' ', item.UriPath, ' ', HTTP_VERSION, '\"', '|', item.TimeTaken, '\n'));

                        fileStream.Write(info, 0, info.Length);
                    }
                }

                using (StreamReader streamReader = File.OpenText(LogPath))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
