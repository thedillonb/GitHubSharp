using System;

namespace GitHubSharp
{
    [Serializable]
    public class GitHubResponse
    {
        public int StatusCode { get; set; }
        public int RateLimitLimit { get; set; }
        public int RateLimitRemaining { get; set; }
    }

    [Serializable]
    public class GitHubResponse<T> : GitHubResponse where T : class
    {
        [NonSerialized]
        private Func<GitHubResponse<T>> _more;

        public T Data { get; set; }

        public Func<GitHubResponse<T>> More
        {
            get { return _more; }
            set { _more = value; }
        }
    }
}

