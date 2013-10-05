using System;
using System.Threading.Tasks;

namespace GitHubSharp
{
    public enum RequestMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH
    }

    [Serializable]
    public class GitHubRequest
    {
        public RequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public object Args { get; private set; }

        public bool RequestFromCache { get; set; }

        public bool CacheResponse { get; set; }

        public bool CheckIfModified { get; set; }

        public bool UseCache
        {
            set
            {
                    RequestFromCache = CacheResponse = CheckIfModified = value;
            }
        }

        internal GitHubRequest(string url, RequestMethod method, object args = null)
        {
            RequestMethod = method;
            Url = url;
            Args = args;
            RequestFromCache = true;
            CacheResponse = true;
            CheckIfModified = true;
        }
        
        internal static GitHubRequest<T> Get<T>(Client client, string url, object args = null) where T : new()
        {
            return new GitHubRequest<T>(url, RequestMethod.GET, args);
        }

        internal static GitHubRequest<T> Patch<T>(string url, object args = null) where T : new()
        {
            return new GitHubRequest<T>(url, RequestMethod.PATCH, args);
        }

        internal static GitHubRequest<T> Post<T>(Client client, string url, object args = null) where T : new()
        {
            return new GitHubRequest<T>(url, RequestMethod.POST, args);
        }

        internal static GitHubRequest<T> Put<T>(string url, object args = null) where T : new()
        {
            return new GitHubRequest<T>(url, RequestMethod.PUT, args);
        }

        internal static GitHubRequest Put(string url, object args = null)
        {
            return new GitHubRequest(url, RequestMethod.PUT, args);
        }

        internal static GitHubRequest Delete(string url, object args = null)
        {
            return new GitHubRequest(url, RequestMethod.DELETE, args);
        }
    }

    [Serializable]
    public class GitHubRequest<T> : GitHubRequest where T : new()
    {
        internal GitHubRequest(string url, RequestMethod method, object args = null)
            : base(url, method, args)
        {
        }
    }
}

