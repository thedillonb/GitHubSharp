using System;

namespace GitHubSharp
{
    
    public class GitHubResponse
    {
        public int StatusCode { get; set; }

        public int RateLimitLimit { get; set; }

        public int RateLimitRemaining { get; set; }

        public string ETag { get; set; }

        public bool WasCached { get; set; }
    }

    
    public class GitHubResponse<T> : GitHubResponse where T : new()
    {
        public T Data { get; set; }

        public GitHubRequest<T> More { get; set; }
    }
}

