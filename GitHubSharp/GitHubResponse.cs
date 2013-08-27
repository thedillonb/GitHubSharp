using System;

namespace GitHubSharp
{
    public class GitHubResponse
    {
        public int StatusCode { get; set; }
        public int RateLimitLimit { get; set; }
        public int RateLimitRemaining { get; set; }
    }

    public class GitHubResponse<T> : GitHubResponse where T : class
    {
        public T Data { get; set; }
        public Func<GitHubResponse<T>> More { get; set; }
    }
}

