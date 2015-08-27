using System;
using System.Threading.Tasks;

namespace GitHubSharp
{
    public interface ICache
    {
        T Get<T>(string url);

        void Set<T>(string url, T data);
    }
}

