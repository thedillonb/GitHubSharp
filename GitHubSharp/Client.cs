using System;
using System.Collections.Generic;
using System.Net;
using GitHubSharp.Utils;
using RestSharp;
using RestSharp.Deserializers;
using GitHubSharp.Controllers;
using System.Threading.Tasks;
using GitHubSharp.Models;

namespace GitHubSharp
{
    public class Client
    {
        public const string DefaultApi = "https://api.github.com";

        public const string RawUri = "https://raw.github.com";

        private const string AccessTokenUri = "https://github.com";

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

        public AuthorizationsController Authorizations
        {
            get { return new AuthorizationsController(this); }
        }
        
        /// <summary>
        /// Gets the username for this client
        /// </summary>
        public string Username { get; set; }
        
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
        /// The cache backing
        /// </summary>
        /// <value>The cache.</value>
        public ICache Cache { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        private Client()
        {
            Timeout = 30 * 1000;
        }

        /// <summary>
        /// Create a BASIC Auth Client
        /// </summary>
        public static Client Basic(string username, string password, string apiUri = DefaultApi)
        {
            if (string.IsNullOrEmpty(apiUri))
                apiUri = DefaultApi;

            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username must be valid!");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password must be valid!");

            Uri apiOut;
            if (!Uri.TryCreate(apiUri, UriKind.Absolute, out apiOut))
                throw new ArgumentException("The URL, " + apiUri + ", is not valid!");

            var c = new Client();
            c.Username = username;
            c.ApiUri = apiOut.AbsoluteUri.TrimEnd('/');
            c._client.Authenticator = new HttpBasicAuthenticator(username, password);
            return c;
        }

        /// <summary>
        /// Create a TwoFactor(BASIC + X-GitHub-OTP) Auth Client
        /// </summary>
        public static Client BasicTwoFactorAuthentication(string username, string password, string twoFactor, string apiUri = DefaultApi)
        {
            var c = Basic(username, password, apiUri);
            c._client.Authenticator = new Authenticators.TwoFactorAuthenticator(username, password, twoFactor);
            return c;
        }

        /// <summary>
        /// Create a BASIC OAuth client
        /// </summary>
        public static Client BasicOAuth(string oauth, string apiUri = DefaultApi)
        {
            if (string.IsNullOrEmpty(apiUri))
                apiUri = DefaultApi;

            Uri apiOut;
            if (!Uri.TryCreate(apiUri, UriKind.Absolute, out apiOut))
                throw new ArgumentException("The URL, " + apiUri + ", is not valid!");

            var c = new Client();
            c.ApiUri = apiOut.AbsoluteUri.TrimEnd('/');
            c._client.Authenticator = new HttpBasicAuthenticator(oauth, "x-oauth-basic");
            return c;
        }

        /// <summary>
        /// Request an access token
        /// </summary>
        public static AccessTokenModel RequestAccessToken(string clientId, string clientSecret, string code, string redirectUri, string domainUri = AccessTokenUri)
        {
            if (string.IsNullOrEmpty(domainUri))
                domainUri = AccessTokenUri;

            if (!domainUri.EndsWith("/"))
                domainUri += "/";
            domainUri += "login/oauth/access_token";

            var c = new Client();
            var request = GitHubRequest.Post<AccessTokenModel>(domainUri, new { client_id = clientId, client_secret = clientSecret, code = code, redirect_uri = redirectUri });
            var response = c.Execute(request);
            return response.Data;
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        private GitHubResponse<T> Get<T>(GitHubRequest<T> githubRequest) where T : new()
        {
            var request = new RestRequest(githubRequest.Url, Method.GET);
            if (githubRequest.Args != null)
                foreach (var arg in ObjectToDictionaryConverter.Convert(githubRequest.Args))
                    request.AddParameter(arg.Key, arg.Value);

            // If there is no cache, just directly execute and parse. Nothing more
            if (Cache == null)
                return ParseResponse<T>(ExecuteRequest(request));

            //Build the absolute URI for the cache
            var absoluteUri = _client.BuildUri(request).AbsoluteUri;

            // If the request has UseCache enabled then attempt to get it from our cache
            if (githubRequest.RequestFromCache)
            {
                var cache = Cache.Get<GitHubResponse<T>>(absoluteUri);
                if (cache != null)
                {
                    cache.WasCached = true;
                    return cache;
                }
            }
            else
            {
                var etag = githubRequest.CheckIfModified ? Cache.GetETag(absoluteUri) : null;
                if (etag != null)
                    request.AddHeader("If-None-Match", string.Format("\"{0}\"", etag));
            }

            var response = ExecuteRequest(request);
            var parsedResponse = ParseResponse<T>(response);

            // ParseResponse will throw an exception if it's not a good response.
            // So, if we get here, it means that the response is OK to cache
            if (githubRequest.CacheResponse)
                Cache.Set(absoluteUri, parsedResponse, parsedResponse.ETag);

            return parsedResponse;
        }

        private RestRequest CreatePutRequest(GitHubRequest request)
        {
            var r = new RestRequest(request.Url, Method.PUT);
            r.RequestFormat = DataFormat.Json;
            if (request.Args != null)
                r.AddBody(request.Args);
            else
                r.AddHeader("Content-Length", "0");

            return r;
        }
        
        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        private GitHubResponse<T> Put<T>(GitHubRequest<T> request) where T : new()
        {
            return ParseResponse<T>(ExecuteRequest(CreatePutRequest(request)));
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <param name="uri"></param>
        private GitHubResponse Put(GitHubRequest request)
        {
            return ParseResponse(ExecuteRequest(CreatePutRequest(request)));
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private GitHubResponse<T> Post<T>(GitHubRequest<T> githubRequest) where T : new()
        {
            var request = new RestRequest(githubRequest.Url, Method.POST);
            request.RequestFormat = DataFormat.Json;
            if (githubRequest.Args != null)
                request.AddBody(githubRequest.Args);

            return ParseResponse<T>(ExecuteRequest(request));
        }

        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
        /// <param name="uri"></param>
        private GitHubResponse Delete(GitHubRequest request)
        {
            var r = new RestRequest(request.Url, Method.DELETE);
            return ParseResponse(ExecuteRequest(r));
        }

        private RestRequest CreatePatchRequest(GitHubRequest request)
        {
            var r = new RestRequest(request.Url, Method.PATCH);
            r.RequestFormat = DataFormat.Json;
            if (request.Args != null)
                r.AddBody(request.Args);
            return r;
        }

        private GitHubResponse<T> Patch<T>(GitHubRequest<T> request) where T : new()
        {
            return ParseResponse<T>(ExecuteRequest(CreatePatchRequest(request)));
        }

        private GitHubResponse Patch(GitHubRequest request)
        {
            return ParseResponse(ExecuteRequest(CreatePatchRequest(request)));
        }

        private GitHubResponse<T> ParseResponse<T>(IRestResponse response) where T : new()
        {
            var ghr = new GitHubResponse<T>() { StatusCode = (int)response.StatusCode };
           
            foreach (var h in response.Headers)
            {
                if (h.Name.Equals("X-RateLimit-Limit"))
                    ghr.RateLimitLimit = Convert.ToInt32(h.Value);
                else if (h.Name.Equals("X-RateLimit-Remaining"))
                    ghr.RateLimitRemaining = Convert.ToInt32(h.Value);
                else if (h.Name.Equals("ETag"))
                    ghr.ETag = h.Value.ToString().Replace("\"", "").Trim();
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
                            ghr.More = GitHubRequest.Get<T>(url); 
                        }
                    }
                }
            }

            // Booleans have a special definition in the github responses.
            // They typically represent a status code Not Found = false
            // or 204 = true and 205 (reset content)
            if (typeof(T) == typeof(bool))
            {
                var b = ghr.StatusCode == 204 || ghr.StatusCode == 205;
                ghr.Data = (T)(object)(b);
            }
            else
            {
                if (response.StatusCode < (HttpStatusCode)200 || response.StatusCode >= (HttpStatusCode)300)
                    throw StatusCodeException.FactoryCreate(response);

                ghr.Data = new JsonDeserializer().Deserialize<T>(response);
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
                else if (h.Name.Equals("ETag"))
                    ghr.ETag = h.Value.ToString().Replace("\"", "");
            }

            if (response.StatusCode < (HttpStatusCode)200 || response.StatusCode >= (HttpStatusCode)300)
                throw StatusCodeException.FactoryCreate(response);

            return ghr;
        }

        /// <summary>
        /// Executes a request to the server
        /// </summary>
        internal IRestResponse ExecuteRequest(IRestRequest request)
        {
            var response = _client.Execute(request);
            if (response.ErrorException != null)
                throw response.ErrorException;
            return response;
        }

        public GitHubResponse Execute(GitHubRequest request)
        {
            switch (request.RequestMethod)
            {
                case RequestMethod.DELETE:
                    return this.Delete(request);
                case RequestMethod.PUT:
                    return this.Put(request);
                case RequestMethod.PATCH:
                    return this.Patch(request);
                default:
                    return null;
            }
        }

        public GitHubResponse<T> Execute<T>(GitHubRequest<T> request) where T : new()
        {
            switch (request.RequestMethod)
            {
                case RequestMethod.GET:
                    return this.Get<T>(request);
                case RequestMethod.POST:
                    return this.Post<T>(request);
                case RequestMethod.PUT:
                    return this.Put<T>(request);
                case RequestMethod.PATCH:
                    return this.Patch<T>(request);
                default:
                    return null;
            }
        }

        public Task<GitHubResponse> ExecuteAsync(GitHubRequest request)
        {
            return Task.Run(() => this.Execute(request));
        }

        public Task<GitHubResponse<T>> ExecuteAsync<T>(GitHubRequest<T> request) where T : new()
        {
            return Task.Run(() => this.Execute<T>(request));
        }

        public bool IsCached(GitHubRequest request)
        {
            if (Cache == null || request.RequestMethod != RequestMethod.GET)
                return false;

            var req = new RestRequest(request.Url, Method.GET);
            if (request.Args != null)
                foreach (var arg in ObjectToDictionaryConverter.Convert(request.Args))
                    req.AddParameter(arg.Key, arg.Value);

            return Cache.Exists(_client.BuildUri(req).AbsoluteUri);
        }

        public string DownloadRawResource(string rawUrl, System.IO.Stream downloadSream)
        {
            var request = new RestSharp.RestRequest(rawUrl, Method.GET);
            request.ResponseWriter = (s) => s.CopyTo(downloadSream);
            var response = ExecuteRequest(request);
            return response.ContentType;
        }
    }
}
