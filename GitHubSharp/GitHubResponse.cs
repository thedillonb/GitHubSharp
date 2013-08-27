using System;

namespace GitHubSharp
{
    public class GitHubResponse<T> where T : class
    {
        public int StatusCode { get; set; }
        public int RateLimitLimit { get; set; }
        public int RateLimitRemaining { get; set; }
        public T Data { get; set; }
        public Func<GitHubResponse<T>> More { get; set; }
    }
}

