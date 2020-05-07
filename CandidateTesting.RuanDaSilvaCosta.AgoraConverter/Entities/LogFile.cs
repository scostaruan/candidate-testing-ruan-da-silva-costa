namespace CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Entities
{
    public class LogFile
    {
        public string CacheStatus { get; private set; }
        public string HttpMethod { get; private set; }
        public int ResponseSize { get; private set; }
        public int StatusCode { get; private set; }
        public double TimeTaken { get; private set; }
        public string UriPath { get; private set; }

        public LogFile(string cacheStatus, string httpMethod, int responseSize, int statusCode, double timeTaken, string uriPath)
        {
            CacheStatus = cacheStatus;
            HttpMethod = httpMethod;
            ResponseSize = responseSize;
            StatusCode = statusCode;
            TimeTaken = timeTaken;
            UriPath = uriPath;
        }
    }
}
