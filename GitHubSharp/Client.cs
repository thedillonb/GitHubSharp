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
        public static string ApiUri = "https://api.github.com";
        public static string RawUri = "https://raw.github.com";
        public static string GistUri = "https://gist.github.com";
        private readonly RestClient _client = new RestClient();

        public AuthenticatedUserController AuthenticatedUser
        {
            get { return new AuthenticatedUserController(this); }
        }

        public UsersController Users
        {
            get { return new UsersController(this); }
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
        public Client(String username, String password)
        {
            Username = username;
            Password = password;
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            _client.FollowRedirects = true;
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
        public GitHubResponse<T> Get<T>(string relativeResource, bool forceCacheInvalidation = false, Uri baseUri = null, int page = 1, int perPage = 100, object additionalArgs = null) where T : class
        {
            if (baseUri == null)
                baseUri = ApiUri;
            var uri = baseUri.AbsoluteUri + relativeResource;

            GitHubResponse<T> obj = null;

            //If there's a cache provider, check it.
            if (CacheProvider != null && !forceCacheInvalidation)
                obj = CacheProvider.Get<GitHubResponse<T>>(uri);

            //Return the cached object
            if (obj != null)
                return obj;

            var request = new RestRequest(baseUri.AbsoluteUri + relativeResource, Method.GET);
            request.AddParameter("page", page);
            request.AddParameter("per_page", perPage);
            if (additionalArgs != null)
                foreach (var arg in ObjectToDictionaryConverter.Convert(additionalArgs))
                    request.AddParameter(arg.Key, arg.Value);

            var response = ParseResponse<T>(ExecuteRequest(request));

            //If there's a cache provider, save it!
            if (CacheProvider != null)
                CacheProvider.Set(obj, uri);
            return response;
        }

        
        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public GitHubResponse<T> Put<T>(string uri, Dictionary<string, string> data = null) where T : class
        {
            return Request<T>(uri, Method.PUT, data);
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public GitHubResponse<T> Put<T>(string uri, object data = null) where T : class
        {
            return Request<T>(uri, Method.PUT, ObjectToDictionaryConverter.Convert(data));
        }
        
        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <param name="uri"></param>
        public void Put(string uri, Dictionary<string, string> data = null)
        {
            Request(uri, Method.PUT, data);
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GitHubResponse<T> Post<T>(string uri, object data) where T : class
        {
            var request = new RestRequest(ApiUri + uri, Method.POST);
            request.RequestFormat = DataFormat.Json;
            if (data != null)
                request.AddBody(data);

            return ParseResponse<T>(ExecuteRequest(request));
        }

        
        /// <summary>
        /// Post the specified uri and data.
        /// </summary>
        public GitHubResponse<T> Post<T>(string uri) where T : class
        {
            return Post<T>(uri);
        }
        
        /// <summary>
        /// Makes a 'POST' request to the server without a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void Post(string uri, Dictionary<string, string> data)
        {
            Request(uri, Method.POST, data);
        }
        
        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
        /// <param name="uri"></param>
        public void Delete(string uri)
        {
            Request(uri, Method.DELETE);
        }

        public GitHubResponse<T> Patch<T>(string uri, object data) where T : class
        {
            return Request<T>(uri, Method.PATCH, ObjectToDictionaryConverter.Convert(data));
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
        
        /// <summary>
        /// Makes a request to the server expecting a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public GitHubResponse<T> Request<T>(string uri, Method method = Method.GET, Dictionary<string, string> data = null) where T : class
        {
            return ParseResponse<T>(ExecuteRequest(ApiUri + uri, method, data));
        }


        /// <summary>
        /// Makes a request to the server expecting a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public GitHubResponse<T> RequestWithJson<T>(string uri, Method method = Method.GET, object obj = null) where T : class
        {
            return ParseResponse<T>(ExecuteRequestWithJson(ApiUri + uri, method, obj));
        }
        
        /// <summary>
        /// Makes a request to the server but does not expect a response.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        public void Request(string uri, Method method = Method.GET, Dictionary<string, string> data = null)
        {
            ExecuteRequest(ApiUri + uri, method, data);
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
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal IRestResponse ExecuteRequestWithJson(string uri, Method method, object obj)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            var request = new RestRequest(uri, method);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(obj);

            //Puts without any data must be marked as having no content!
            if (method == Method.PUT && obj == null)
                request.AddHeader("Content-Length", "0");

            return ExecuteRequest(request);
        }

        /// <summary>
        /// Executes a request to the server
        /// </summary>
        internal IRestResponse ExecuteRequest(IRestRequest request)
        {
            RestSharp.IRestResponse response = null;
            response = _client.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            //A 200 is always good.
            if (response.StatusCode >= (HttpStatusCode)200 && response.StatusCode < (HttpStatusCode)300)
                return response;

            throw StatusCodeException.FactoryCreate(response.StatusCode);
        }
    }
}
