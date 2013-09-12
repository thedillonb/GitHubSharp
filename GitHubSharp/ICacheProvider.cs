using System;

namespace GitHubSharp
{
    public interface ICacheProvider
    {
        T Get<T>(string query) where T : class;
        void Set(string query, object objectToCache);
        void Delete(string query);
        void DeleteWhereStartingWith(string query);
    }
}

