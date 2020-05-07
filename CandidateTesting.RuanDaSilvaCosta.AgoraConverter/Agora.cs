using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Entities;
using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CandidateTesting.RuanDaSilvaCosta.AgoraConverter
{
    public class Agora : IAgora
    {
        private readonly string PROVIDER = "\"MINHA CDN\"";

        public List<LogFile> Log { get; private set; }
        public string LogPath { get; private set; }

        public Agora(List<LogFile> log, string logPath)
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
                    string version = "#Version: 1.0";
                    string date = string.Concat("#Date", ':', ' ', DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    string field = string.Concat("#Fields", ':', ' ', "provider", ' ', "http-method", ' ', "status-code", ' ',
                        "uri-path", ' ', "time-taken", ' ', "response-size", ' ', "cache-status");

                    byte[] header = new UTF8Encoding(true).GetBytes(string.Concat(version, '\n', date, '\n', field, '\n'));

                    fileStream.Write(header, 0, header.Length);

                    foreach (LogFile item in Log)
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(string.Concat(PROVIDER, ' ', item.HttpMethod, ' ', item.StatusCode, ' ',
                            item.UriPath, ' ', (int)item.TimeTaken, ' ', item.ResponseSize, ' ', item.CacheStatus, '\n'));

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
