using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Adapter;
using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Interfaces;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;

namespace CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Test
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateLogTest()
        {
            int count = 0;

            var path = "./output/test/minhaCdn1.txt";
            var response = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2\n" +
                "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4\n" +
                "199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9\n" +
                "312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1\n";

            var messageHandler = new MockHttpMessageHandler(response, HttpStatusCode.OK);
            var httpClient = new HttpClient(messageHandler);
            var streamReader = new StreamReader(httpClient.GetStreamAsync("http://exempligratia.com/testing/arquivo.txt").Result);
            var log = Program.ReadLog(streamReader);

            Agora agora = new Agora(log, path);
            IMinhaCdn adapter = new AgoraAdapter(agora);

            Program.Log(adapter);

            using (StreamReader streamReaderLog = File.OpenText(path))
            {
                string line;
                while ((line = streamReaderLog.ReadLine()) != null)
                {
                    count++;
                }
            }

            // Minha CDN Header: 3 lines
            Assert.AreEqual((count - 3), log.Count);
        }
    }
}
