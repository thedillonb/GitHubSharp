using System;
using System.Collections.Generic;
using System.Net;
using GitHubSharp.Utils;
using RestSharp;
using RestSharp.Deserializers;

namespace GitHubSharp
{
    public class Client
    {
        public static string ApiUrl = "https://api.github.com";
        public static string RawUrl = "https://raw.github.com";
        public static string GistUrl = "https://gist.github.com";
        private readonly RestClient _client = new RestClient();
        
        /// <summary>
        /// Gets the username for this client
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets the AP.
        /// </summary>
        public API API { get; private set; }
        
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
        
        public uint Retries { get; set; }

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
            Retries = 3;
            Username = username;
            Password = password;
            API = new GitHubSharp.API(this);
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            _client.UserAgent = "Gistacular";
            _client.FollowRedirects = true;
        }
        
        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        public GitHubResponse<T> Get<T>(String uri) where T : class
        {
            return Request<T>(uri);
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
        public GitHubResponse<T> Post<T>(string uri, Dictionary<string, string> data) where T : class
        {
            return Request<T>(uri, Method.POST, data);
        }
        
        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GitHubResponse<T> Post<T>(string uri, T data) where T : class
        {
            return Post<T>(uri, ObjectToDictionaryConverter.Convert(data));
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
            var response = ExecuteRequest(ApiUrl + uri, method, data);
            var ghr = new GitHubResponse<T>();
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
                            ghr.Next = new GitHubResponse<T>.Pagination(0, 0);
                        }
                        else if (what.Equals("prev"))
                        {
                            ghr.Prev = new GitHubResponse<T>.Pagination(0, 0);
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
        public GitHubResponse<T> RequestWithJson<T>(string uri, Method method = Method.GET, object obj = null) where T : class
        {
            var response = ExecuteRequestWithJson(ApiUrl + uri, method, obj);
            var ghr = new GitHubResponse<T>();
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
                            ghr.Next = new GitHubResponse<T>.Pagination(0, 0);
                        }
                        else if (what.Equals("prev"))
                        {
                            ghr.Prev = new GitHubResponse<T>.Pagination(0, 0);
                        }

                    }
                }
            }

            return ghr;
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
            ExecuteRequest(ApiUrl + uri, method, data);
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
            for (var i = 0; i < Retries + 1; i++)
            {
                response = _client.Execute(request);

                //No clue what this is... Try it again...
                if (response.StatusCode == (HttpStatusCode)0)
                    continue;

                //A 200 is always good.
                if (response.StatusCode >= (HttpStatusCode)200 && response.StatusCode < (HttpStatusCode)300)
                    return response;

                throw StatusCodeException.FactoryCreate(response.StatusCode);
            }
            
            throw new InvalidOperationException("Unable to execute request. Status code 0 returned " + (Retries+1) + " times!");
        }
    }
}
