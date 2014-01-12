using System;

namespace GitHubSharp
{
    public interface ICache
    {
        bool Exists(string url);

        string GetETag(string url);

        byte[] Get(string url);

        void Set(string url, byte[] data, string etag);
    }
}

