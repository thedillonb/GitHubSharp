using System.Collections.Generic;

namespace GitHubSharp
{
    class CacheObject
    {
        public int StatusCode { get; set; }
        public Dictionary<string, List<string>> Headers { get; set; }
        public string Data { get; set; }
    }
}

