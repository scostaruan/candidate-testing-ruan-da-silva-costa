using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Adapter;
using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Entities;
using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace CandidateTesting.RuanDaSilvaCosta.AgoraConverter
{
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static void Main(string[] args)
        {
            if (args.Length != 3 ||
                !args[0].Equals("convert") ||
                !Uri.IsWellFormedUriString(args[1], UriKind.Absolute) ||
                string.IsNullOrEmpty(args[2]))
            {
                Console.WriteLine("Invalid arguments for file logging.");
                Console.WriteLine("Review them, please!");
                Environment.Exit(0);
            }

            List<LogFile> log = ReadLog(new StreamReader(new HttpClient().GetStreamAsync(args[1]).Result));

            #region MinhaCDN File
            //MinhaCdn minhaCdn = new MinhaCdn(log, args[2]);
            //minhaCdn.CreateLog();
            #endregion

            #region Agora File
            Agora agora = new Agora(log, args[2]);
            IMinhaCdn adapter = new AgoraAdapter(agora);

            Log(adapter);
            #endregion
        }

        public static List<LogFile> ReadLog(StreamReader streamReader)
        {
            List<LogFile> files = new List<LogFile>();
            //StreamReader streamReader = new StreamReader(client.GetStreamAsync(uri).Result);

            string line;

            while ((line = streamReader.ReadLine()) != null)
            {
                line = line.Replace("\"", string.Empty);

                string[] lineParts = line.Split('|');
                string[] lineSubparts = lineParts[3].Split(' ');

                try
                {
                    string cacheStatus = lineParts[2];
                    string httpMethod = lineSubparts[0];
                    int responseSize = Convert.ToInt32(lineParts[0]);
                    int statusCode = Convert.ToInt32(lineParts[1]);
                    double timeTaken = Convert.ToDouble(lineParts[4]);
                    string uriPath = lineSubparts[1];

                    LogFile file = new LogFile(cacheStatus, httpMethod, responseSize, statusCode, timeTaken, uriPath);

                    files.Add(file);
                }
                catch
                {
                    continue;
                }
            }

            return files;
        }

        public static void Log(IMinhaCdn minhaCdn)
        {
            minhaCdn.CreateLog();
        }
    }
}
