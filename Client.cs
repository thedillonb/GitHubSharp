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

		public static ISerializer Serializer;

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
			Timeout = new TimeSpan(0, 0, 30);
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
			var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());

            // If there is no cache, just directly execute and parse. Nothing more
            if (Cache == null)
				return await ParseResponse<T>(await ExecuteRequest(request));

            //Build the absolute URI for the cache
			var absoluteUri = url.ToString(); //_client.BuildUri(request).AbsoluteUri;

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
					request.Headers.Add("If-None-Match", string.Format("\"{0}\"", etag));
            }

			var response = await ExecuteRequest(request);
			var parsedResponse = await ParseResponse<T>(response);

            // ParseResponse will throw an exception if it's not a good response.
            // So, if we get here, it means that the response is OK to cache
            if (githubRequest.CacheResponse)
                Cache.Set(absoluteUri, parsedResponse, parsedResponse.ETag);

            return parsedResponse;
        }

		private static HttpRequestMessage CreatePutRequest(GitHubRequest request)
        {
			var r = new HttpRequestMessage(HttpMethod.Put, request.Url);
			if (request.Args != null)
			{
				r.Content = new StringContent(Serializer.Serialize(request.Args), Encoding.UTF8, "application/json");
			}
			else
			{
				r.Content = new StringContent("");
				r.Content.Headers.ContentLength = 0;
			}
            return r;
        }
        
        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
		private async Task<GitHubResponse<T>> Put<T>(GitHubRequest request) where T : new()
        {
			return await ParseResponse<T>(await ExecuteRequest(CreatePutRequest(request)));
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
		private async Task<GitHubResponse> Put(GitHubRequest request)
        {
			return await ParseResponse(await ExecuteRequest(CreatePutRequest(request)));
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
		private async Task<GitHubResponse<T>> Post<T>(GitHubRequest request) where T : new()
        {
			var r = new HttpRequestMessage(HttpMethod.Post, request.Url);
			if (request.Args != null)
				r.Content = new StringContent(Serializer.Serialize(request.Args), Encoding.UTF8, "application/json");
			return await ParseResponse<T>(await ExecuteRequest(r));
        }

        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
		private async Task<GitHubResponse> Delete(GitHubRequest request)
        {
			var r = new HttpRequestMessage(HttpMethod.Delete, request.Url);
			return await ParseResponse(await ExecuteRequest(r));
        }

		private static HttpRequestMessage CreatePatchRequest(GitHubRequest request)
        {
			var r = new HttpRequestMessage(new HttpMethod("PATCH"), request.Url);
            if (request.Args != null)
				r.Content = new StringContent(Serializer.Serialize(request.Args), Encoding.UTF8, "application/json");
            return r;
        }

		private async Task<GitHubResponse<T>> Patch<T>(GitHubRequest request) where T : new()
        {
			return await ParseResponse<T>(await ExecuteRequest(CreatePatchRequest(request)));
        }

		private async Task<GitHubResponse> Patch(GitHubRequest request)
        {
			return await ParseResponse(await ExecuteRequest(CreatePatchRequest(request)));
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
		private Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
		{
			return _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
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
			var request = new HttpRequestMessage(HttpMethod.Get, rawUrl);
			var response = await ExecuteRequest(request);
			var stream = await response.Content.ReadAsStreamAsync();
			stream.CopyTo(downloadSream);
			return "" + response.Content.Headers.ContentType;
        }

		public async Task<string> DownloadRawResource2(string rawUrl, System.IO.Stream downloadSream)
        {
			var request = new HttpRequestMessage(HttpMethod.Get, rawUrl);
			request.Headers.Accept.Clear();
			request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.raw"));
			var response = await ExecuteRequest(request);
			var stream = await response.Content.ReadAsStreamAsync();
			stream.CopyTo(downloadSream);
			return "" + response.Content.Headers.ContentType;
        }
    }
}
