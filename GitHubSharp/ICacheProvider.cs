using System;

namespace GitHubSharp
{
    public interface ICacheProvider
    {
        T Get<T>(string name) where T : class;
        T Get<T>(string name, int cacheDurationInMinutes) where T : class;
        bool IsCached(string name);
        void Set<T>(T objectToCache, string name) where T : class;
        void Delete(string name);
        void DeleteWhereStartingWith(string name);
        void DeleteAll();
        int DefaultDuractionInMinutes { get; }
    }

    public class CachedObject
    {
        public object Cached { get; set; }
        public DateTime When { get; set; }
    }
}

