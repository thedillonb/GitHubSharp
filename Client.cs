using System;
using System.Collections.Generic;
using System.Net;
using GitHubSharp.Utils;
using GitHubSharp.Controllers;
using System.Threading.Tasks;
using GitHubSharp.Models;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace GitHubSharp
{
    public class Client
    {
        public const string DefaultApi = "https://api.github.com";

        public const string AccessTokenUri = "https://github.com";

        public static Func<HttpClient> ClientConstructor = () => new HttpClient();

        public static IJsonSerializer Serializer = new SimpleJsonSerializer();

        private readonly HttpClient _client;

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
        public TimeSpan Timeout 
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
            _client = ClientConstructor();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("GithubSharp");
            Timeout = new TimeSpan(0, 0, 20);
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

            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
            c._client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", token);
            return c;
        }

        /// <summary>
        /// Create a TwoFactor(BASIC + X-GitHub-OTP) Auth Client
        /// </summary>
        public static Client BasicTwoFactorAuthentication(string username, string password, string twoFactor, string apiUri = DefaultApi)
        {
            var c = Basic(username, password, apiUri);
            c._client.DefaultRequestHeaders.Add("X-GitHub-OTP", twoFactor);
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
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", oauth, "x-oauth-basic")));
            c._client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", token);
            return c;
        }

        /// <summary>
        /// Request an access token
        /// </summary>
        public static async Task<AccessTokenModel> RequestAccessToken(string clientId, string clientSecret, string code, string redirectUri, string domainUri = AccessTokenUri)
        {
            if (string.IsNullOrEmpty(domainUri))
                domainUri = AccessTokenUri;

            if (!domainUri.EndsWith("/", StringComparison.Ordinal))
                domainUri += "/";
            domainUri += "login/oauth/access_token";

            var c = new Client();
            var request = GitHubRequest.Post<AccessTokenModel>(domainUri, new { client_id = clientId, client_secret = clientSecret, code, redirect_uri = redirectUri });
            var response = await c.ExecuteAsync(request);
            return response.Data;
        }

        private static string ToQueryString(IEnumerable<KeyValuePair<string, string>> nvc)
        {
            var array = (from key in nvc
                select string.Format("{0}={1}", Uri.EscapeUriString(key.Key), Uri.EscapeUriString(key.Value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <returns>An object with response data</returns>
        private async Task<GitHubResponse<T>> Get<T>(GitHubRequest githubRequest) where T : new()
        {
            var url = new StringBuilder().Append(githubRequest.Url);
            if (githubRequest.Args != null)
                url.Append(ToQueryString(ObjectToDictionaryConverter.Convert(githubRequest.Args).ToArray()));

            using (var request = new HttpRequestMessage(HttpMethod.Get, url.ToString()))
            {
                // If there is no cache, just directly execute and parse. Nothing more
                if (Cache == null)
                {
                    using (var requestResponse = await ExecuteRequest(request))
                    {
                        return await ParseResponse<T>(requestResponse);
                    }
                }

                //Build the absolute URI for the cache
                var absoluteUri = url.ToString(); //_client.BuildUri(request).AbsoluteUri;

                HttpResponseMessage response = null;
                var retrievedFromCache = false;

                try
                {
                    // If the request has UseCache enabled then attempt to get it from our cache
                    if (githubRequest.RequestFromCache)
                    {
                        response = GetFromCache(absoluteUri);
                        retrievedFromCache = response != null;
                    }
                    else
                    {
                        var etag = githubRequest.CheckIfModified ? Cache.GetETag(absoluteUri) : null;
                        if (etag != null)
                            request.Headers.Add("If-None-Match", string.Format("\"{0}\"", etag));
                    }

                    if (response == null)
                        response = await ExecuteRequest(request);
                    var parsedResponse = await ParseResponse<T>(response);

                    if (retrievedFromCache)
                        parsedResponse.WasCached = true;
                    else if (githubRequest.CacheResponse)
                    {
                        // ParseResponse will throw an exception if it's not a good response.
                        // So, if we get here, it means that the response is OK to cache
                        SetCache(absoluteUri, response, parsedResponse.ETag);
                    }

                    return parsedResponse;
                }
                finally
                {
                    if (response != null)
                        response.Dispose();
                }
            }
        }

        private static HttpRequestMessage CreatePutRequest(GitHubRequest request)
        {
            var r = new HttpRequestMessage(HttpMethod.Put, request.Url);
            if (request.Args != null)
            {
                var serialized = Serializer.Serialize(request.Args);
                r.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
            }
            else
            {
                r.Content = new StringContent("");
                r.Content.Headers.ContentLength = 0;
            }
            return r;
        }

        private void SetCache(string uri, HttpResponseMessage response, string etag)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(((int)response.StatusCode).ToString());
                foreach (var header in response.Headers)
                {
                    foreach (var h in header.Value)
                        sb.Append(header.Key).Append(": ").AppendLine(h);
                }

                sb.AppendLine();
                sb.Append(response.Content.ReadAsStringAsync().Result);
                Cache.Set(uri, Encoding.UTF8.GetBytes(sb.ToString()), etag);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to set cache: " + e.Message);
            }
        }

        private HttpResponseMessage GetFromCache(string uri)
        {
            try
            {
                var data = Cache.Get(uri);
                if (data == null)
                    return null;

                using (var ms = new System.IO.MemoryStream(data))
                {
                    var newline = new System.IO.StreamReader(ms, Encoding.UTF8);

                    var statusLine = newline.ReadLine();
                    if (statusLine.Length > 32)
                        return null;

                    var response = new HttpResponseMessage((HttpStatusCode)Convert.ToInt32(statusLine));

                    while (true)
                    {
                        var line = newline.ReadLine();
                        if (line.Length == 0)
                            break;

                        try
                        {
                            var header = line.Split(new [] { ':' }, 2);
                            response.Headers.Add(header[0], header[1].Trim());
                        }
                        catch
                        {
                            // Stupid bug with Vary header... It's a good thing we don't care about it
                        }
                    }

                    response.Content = new StringContent(newline.ReadToEnd());
                    return response;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to deserialize header: " + e.Message);
                return null;
            }
        }
        
        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        private async Task<GitHubResponse<T>> Put<T>(GitHubRequest gitHubRequest) where T : new()
        {
            using (var request = CreatePutRequest(gitHubRequest))
            {
                using (var response = await ExecuteRequest(request))
                {
                    return await ParseResponse<T>(response);
                }
            }
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        private async Task<GitHubResponse> Put(GitHubRequest gitHubRequest)
        {
            using (var request = CreatePutRequest(gitHubRequest))
            {
                using (var response = await ExecuteRequest(request))
                {
                    return await ParseResponse(response);
                }
            }
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        private async Task<GitHubResponse<T>> Post<T>(GitHubRequest request) where T : new()
        {
            using (var r = new HttpRequestMessage(HttpMethod.Post, request.Url))
            {
                if (request.Args != null)
                {
                    var serialized = Serializer.Serialize(request.Args);
                    r.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
                }

                using (var response = await ExecuteRequest(r))
                {
                    return await ParseResponse<T>(response);
                }
            }
        }

        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
        private async Task<GitHubResponse> Delete(GitHubRequest request)
        {
            using (var r = new HttpRequestMessage(HttpMethod.Delete, request.Url))
            {
                using (var response = await ExecuteRequest(r))
                {
                    return await ParseResponse(response);
                }
            }
        }

        private static HttpRequestMessage CreatePatchRequest(GitHubRequest request)
        {
            using (var r = new HttpRequestMessage(new HttpMethod("PATCH"), request.Url))
            {
                if (request.Args != null)
                {
                    var serialized = Serializer.Serialize(request.Args);
                    r.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
                }
                return r;
            }
        }

        private async Task<GitHubResponse<T>> Patch<T>(GitHubRequest gitHubRequest) where T : new()
        {
            using (var request = CreatePatchRequest(gitHubRequest))
            {
                using (var response = await ExecuteRequest(request))
                {
                    return await ParseResponse<T>(response);
                }
            }
        }

        private async Task<GitHubResponse> Patch(GitHubRequest gitHubRequest)
        {
            using (var request = CreatePatchRequest(gitHubRequest))
            {
                using (var response = await ExecuteRequest(request))
                {
                    return await ParseResponse(response);
                }
            }
        }

        private static async Task<GitHubResponse<T>> ParseResponse<T>(HttpResponseMessage response) where T : new()
        {
            var ghr = new GitHubResponse<T> { StatusCode = (int)response.StatusCode };
           
            foreach (var h in response.Headers)
            {
                if (h.Key.Equals("X-RateLimit-Limit"))
                    ghr.RateLimitLimit = Convert.ToInt32(h.Value.First());
                else if (h.Key.Equals("X-RateLimit-Remaining"))
                    ghr.RateLimitRemaining = Convert.ToInt32(h.Value.First());
                else if (h.Key.Equals("ETag"))
                    ghr.ETag = h.Value.First().Replace("\"", "").Trim();
                else if (h.Key.Equals("Link"))
                {
                    var s = h.Value.First().Split(',');
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
            if (typeof(T) == typeof(bool) && (ghr.StatusCode == 204 || ghr.StatusCode == 205 || ghr.StatusCode == 404))
            {
                var b = ghr.StatusCode == 204 || ghr.StatusCode == 205;
                ghr.Data = (T)(object)(b);
                return ghr;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode < (HttpStatusCode)200 || response.StatusCode >= (HttpStatusCode)300)
                throw StatusCodeException.FactoryCreate(response, content);

            ghr.Data = Serializer.Deserialize<T>(content);

            return ghr;
        }

        private static async Task<GitHubResponse> ParseResponse(HttpResponseMessage response)
        {
            var ghr = new GitHubResponse { StatusCode = (int)response.StatusCode };
            foreach (var h in response.Headers)
            {
                if (h.Key.Equals("X-RateLimit-Limit"))
                    ghr.RateLimitLimit = Convert.ToInt32(h.Value.First());
                else if (h.Key.Equals("X-RateLimit-Remaining"))
                    ghr.RateLimitRemaining = Convert.ToInt32(h.Value.First());
                else if (h.Key.Equals("ETag"))
                    ghr.ETag = h.Value.First().Replace("\"", "");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode < (HttpStatusCode)200 || response.StatusCode >= (HttpStatusCode)300)
                throw StatusCodeException.FactoryCreate(response, content);

            return ghr;
        }

        /// <summary>
        /// Executes a request to the server
        /// </summary>
        internal async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
        {
            try
            {
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            }
            catch (TaskCanceledException e)
            {
                // Provide a better error message
                throw new TaskCanceledException("Timeout waiting for GitHub to respond. Check your connection and try again.", e);
            }
            catch(WebException e)
            {
                // Provide a better error message
                throw new WebException("Unable to communicate with GitHub. Please check your connection and try again.", e);
            }
        }

        public GitHubResponse Execute(GitHubRequest request)
        {
            var r = ExecuteAsync(request);
            return r.Result;
        }

        public GitHubResponse<T> Execute<T>(GitHubRequest<T> request) where T : new()
        {
            var r = ExecuteAsync<T>(request);
            return r.Result;
        }

        public Task<GitHubResponse> ExecuteAsync(GitHubRequest request)
        {
            switch (request.RequestMethod)
            {
                case RequestMethod.DELETE:
                    return Delete(request);
                case RequestMethod.PUT:
                    return Put(request);
                case RequestMethod.PATCH:
                    return Patch(request);
                default:
                    return null;
            }
        }

        public Task<GitHubResponse<T>> ExecuteAsync<T>(GitHubRequest<T> request) where T : new()
        {
            switch (request.RequestMethod)
            {
                case RequestMethod.GET:
                    return Get<T>(request);
                case RequestMethod.POST:
                    return Post<T>(request);
                case RequestMethod.PUT:
                    return Put<T>(request);
                case RequestMethod.PATCH:
                    return Patch<T>(request);
                default:
                    return null;
            }
        }

//        public bool IsCached(GitHubRequest request)
//        {
//            if (Cache == null || request.RequestMethod != RequestMethod.GET)
//                return false;
//
//            var req = new RestRequest(request.Url, Method.GET);
//            if (request.Args != null)
//                foreach (var arg in ObjectToDictionaryConverter.Convert(request.Args))
//                    req.AddParameter(arg.Key, arg.Value);
//
//            return Cache.Exists(_client.BuildUri(req).AbsoluteUri);
//        }

        public async Task<string> DownloadRawResource(string rawUrl, System.IO.Stream downloadSream)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, rawUrl))
            {
                using (var response = await ExecuteRequest(request))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        stream.CopyTo(downloadSream);
                        return "" + response.Content.Headers.ContentType;
                    }
                }
            }
        }

        public async Task<string> DownloadRawResource2(string rawUrl, System.IO.Stream downloadSream)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, rawUrl))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.raw"));
                using (var response = await ExecuteRequest(request))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        stream.CopyTo(downloadSream);
                        return "" + response.Content.Headers.ContentType;
                    }
                }
            }
        }
    }
}
