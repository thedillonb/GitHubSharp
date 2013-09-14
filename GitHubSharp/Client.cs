using System;
using System.Collections.Generic;
using System.Net;
using GitHubSharp.Utils;
using RestSharp;
using RestSharp.Deserializers;
using GitHubSharp.Controllers;

namespace GitHubSharp
{
    public class Client
    {
        private const string DefaultApi = "https://api.github.com";

        public static string RawUri = "https://raw.github.com";

        private readonly RestClient _client = new RestClient();

        public string ApiUri { get; private set; }

        public AuthenticatedUserController AuthenticatedUser
        {
            get { return new AuthenticatedUserController(this); }
        }

        public UsersController Users
        {
            get { return new UsersController(this); }
        }

        public NotificationsController Notifications
        {
            get { return new NotificationsController(this); }
        }

        public ExploreRepositoriesController Repositories
        {
            get { return new ExploreRepositoriesController(this); }
        }

        public MarkdownController Markdown
        {
            get { return new MarkdownController(this); }
        }

        public TeamsController Teams
        {
            get { return new TeamsController(this); }
        }

        public OrganizationsController Organizations
        {
            get { return new OrganizationsController(this); }
        }

        public GistsController Gists
        {
            get { return new GistsController(this); }
        }
        
        /// <summary>
        /// Gets the username for this client
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; private set; }
        
        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int Timeout 
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        /// <summary>
        /// The cache provider to use when requesting
        /// </summary>
        public ICacheProvider CacheProvider { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Client(String username, String password, String apiUri = DefaultApi)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username must be valid!");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password must be valid!");

            Uri apiOut;
            if (!Uri.TryCreate(apiUri, UriKind.Absolute, out apiOut))
                throw new ArgumentException("The URL, " + apiUri + ", is not valid!");

            Username = username;
            Password = password;
            ApiUri = apiOut.AbsoluteUri.TrimEnd('/');
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            //_client.FollowRedirects = true;
        }

        /// <summary>
        /// Invalidates a cache object starting with a specific URI
        /// </summary>
        /// <param name="startsWithUri">The starting URI to be invalidated</param>
        public void InvalidateCacheObjects(string startsWithUri)
        {
            if (CacheProvider != null)
                CacheProvider.DeleteWhereStartingWith(startsWithUri);
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        public GitHubResponse<T> Get<T>(string uri, bool forceCacheInvalidation = false, int page = 1, int perPage = 100, object additionalArgs = null) where T : class
        {
            GitHubResponse<T> response = null;

            var request = new RestRequest(uri, Method.GET);
            request.AddParameter("page", page);
            request.AddParameter("per_page", perPage);
            if (additionalArgs != null)
                foreach (var arg in ObjectToDictionaryConverter.Convert(additionalArgs))
                    request.AddParameter(arg.Key, arg.Value);

            //Build the absolute URI for the cache
            var absoluteUri = _client.BuildUri(request).AbsoluteUri;

            //If there's a cache provider, check it.
            if (CacheProvider != null && !forceCacheInvalidation)
            {
                response = CacheProvider.Get<GitHubResponse<T>>(absoluteUri);
                if (response != null)
                    return response;
            }

            response = ParseResponse<T>(ExecuteRequest(request));

            //If there's a cache provider, save it!
            if (CacheProvider != null)
                CacheProvider.Set(absoluteUri, response);
            return response;
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        public GitHubResponse Get(string uri, bool forceCacheInvalidation = false, int page = 1, int perPage = 100, object additionalArgs = null)
        {
            GitHubResponse response = null;

            var request = new RestRequest(uri, Method.GET);
            request.AddParameter("page", page);
            request.AddParameter("per_page", perPage);
            if (additionalArgs != null)
                foreach (var arg in ObjectToDictionaryConverter.Convert(additionalArgs))
                    request.AddParameter(arg.Key, arg.Value);

            //Build the absolute URI for the cache
            var absoluteUri = _client.BuildUri(request).AbsoluteUri;

            //If there's a cache provider, check it.
            if (CacheProvider != null && !forceCacheInvalidation)
            {
                response = CacheProvider.Get<GitHubResponse>(absoluteUri);
                if (response != null)
                    return response;
            }

            response = ParseResponse(ExecuteRequest(request));

            //If there's a cache provider, save it!
            if (CacheProvider != null)
                CacheProvider.Set(absoluteUri, response);
            return response;
        }

        private RestRequest CreatePutRequest(string uri, object data)
        {
            var request = new RestRequest(uri, Method.PUT);
            request.RequestFormat = DataFormat.Json;
            if (data != null)
                request.AddBody(data);

            //Puts without any data must be marked as having no content!
            if (data == null)
                request.AddHeader("Content-Length", "0");
            return request;
        }
        
        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public GitHubResponse<T> Put<T>(string uri, object data = null) where T : class
        {
            return ParseResponse<T>(ExecuteRequest(CreatePutRequest(uri, data)));
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <param name="uri"></param>
        public GitHubResponse Put(string uri, object data = null)
        {
            return ParseResponse(ExecuteRequest(CreatePutRequest(uri, data)));
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GitHubResponse<T> Post<T>(string uri, object data = null) where T : class
        {
            var request = new RestRequest(uri, Method.POST);
            request.RequestFormat = DataFormat.Json;
            if (data != null)
                request.AddBody(data);

            return ParseResponse<T>(ExecuteRequest(request));
        }

        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
        /// <param name="uri"></param>
        public GitHubResponse Delete(string uri)
        {
            var request = new RestRequest(uri, Method.DELETE);
            return ParseResponse(ExecuteRequest(request));
        }

        private RestRequest CreatePatchRequest(string uri, object data)
        {
            var request = new RestRequest(uri, Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            if (data != null)
                request.AddBody(data);
            return request;
        }

        public GitHubResponse<T> Patch<T>(string uri, object data = null) where T : class
        {
            return ParseResponse<T>(ExecuteRequest(CreatePatchRequest(uri, data)));
        }

        public GitHubResponse Patch(string uri, object data = null)
        {
            return ParseResponse(ExecuteRequest(CreatePatchRequest(uri, data)));
        }

        private GitHubResponse<T> ParseResponse<T>(IRestResponse response) where T : class
        {
            var ghr = new GitHubResponse<T>() { StatusCode = (int)response.StatusCode };
            var d = new JsonDeserializer();
            ghr.Data = d.Deserialize<T>(response);

            foreach (var h in response.Headers)
            {
                if (h.Name.Equals("X-RateLimit-Limit"))
                    ghr.RateLimitLimit = Convert.ToInt32(h.Value);
                else if (h.Name.Equals("X-RateLimit-Remaining"))
                    ghr.RateLimitRemaining = Convert.ToInt32(h.Value);
                else if (h.Name.Equals("Link"))
                {
                    var s = ((string)h.Value).Split(',');
                    foreach (var link in s)
                    {
                        var splitted = link.Split(';');
                        var url = splitted[0].Trim();
                        var what = splitted[1].Trim();
                        what = what.Substring(5);
                        what = what.Substring(0, what.Length - 1);
                        url = url.Substring(1);
                        url = url.Substring(0, url.Length - 1);

                        if (what.Equals("next"))
                        {
                            ghr.More = () => {
                                var request = new RestRequest(url, Method.GET);
                                return ParseResponse<T>(ExecuteRequest(request));
                            };
                        }
                    }
                }
            }

            return ghr;
        }

        private GitHubResponse ParseResponse(IRestResponse response)
        {
            var ghr = new GitHubResponse { StatusCode = (int)response.StatusCode };
            foreach (var h in response.Headers)
            {
                if (h.Name.Equals("X-RateLimit-Limit"))
                    ghr.RateLimitLimit = Convert.ToInt32(h.Value);
                else if (h.Name.Equals("X-RateLimit-Remaining"))
                    ghr.RateLimitRemaining = Convert.ToInt32(h.Value);
            }
            return ghr;
        }
        
        /// <summary>
        /// Executes a request to the server
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal IRestResponse ExecuteRequest(string uri, Method method, Dictionary<string, string> data)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            
            var request = new RestRequest(uri, method);
            if (data != null)
                foreach (var hd in data)
                    request.AddParameter(hd.Key, hd.Value);
            
            //Puts without any data must be marked as having no content!
            if (method == Method.PUT && data == null)
                request.AddHeader("Content-Length", "0");
            
            return ExecuteRequest(request);
        }

        /// <summary>
        /// Executes a request to the server
        /// </summary>
        internal IRestResponse ExecuteRequest(IRestRequest request)
        {
            var response = _client.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            //A 200 is always good.
            if (response.StatusCode >= (HttpStatusCode)200 && response.StatusCode < (HttpStatusCode)300)
                return response;

            throw StatusCodeException.FactoryCreate(response.StatusCode);
        }
    }
}
