using System;

namespace GitHubSharp
{
    public interface ICache
    {
        bool Exists(string url);

        string GetETag(string url);

        T Get<T>(string url) where T : new();

        void Set(string url, object data, string etag);
    }
}

