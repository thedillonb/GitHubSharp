using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class GistsController : Controller
    {
        public GistController this[string key]
        {
            get { return new GistController(Client, this, key); }
        }

        public GistsController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<List<GistModel>> GetGists(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<GistModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<GistModel>> GetPublicGists(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<GistModel>>(Uri + "/public", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<GistModel>> GetStarredGists(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<GistModel>>(Uri + "/starred", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }
        
        public string GetFile(string url)
        {
            return Client.Get<string>(url).Data;
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/gists"; }
        }
    }

    public class UserGistsController : Controller
    {
        private readonly string _user;

        public UserGistsController(Client client, string user)
            : base(client)
        {
            _user = user;
        }

        public GitHubResponse<List<GistModel>> GetGists(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<GistModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/users/" + _user + "/gists"; }
        }
    }

    public class AuthenticatedGistsController : UserGistsController
    {
        public AuthenticatedGistsController(Client client)
            : base(client, client.Username)
        {
        }

        public GitHubResponse<GistModel> CreateGist(GistCreateModel gist)
        {
            //Great... The RestSharp serializer can't handle this object...
            //Dictionary<string, obj> confuses it and converts it into {"key": "ok", "value": "dokie"}
            //instead of {"ok": "dokie"}
            var obj = new RestSharp.JsonObject();
            obj.Add(new KeyValuePair<string, object>("description", gist.Description));
            obj.Add(new KeyValuePair<string, object>("public", gist.Public));

            var files = new RestSharp.JsonObject();
            obj.Add(new KeyValuePair<string, object>("files", files));

            if (gist.Files != null)
            {
                foreach (var f in gist.Files.Keys)
                {
                    var content = new RestSharp.JsonObject();
                    files.Add(new KeyValuePair<string, object>(f, content));
                    content.Add(new KeyValuePair<string, object>("content", gist.Files[f].Content));
                }
            }

            var uri = Client.ApiUri + "/gists";
            return Client.Post<GistModel>(uri, obj);
        }
    }

    public class GistController : Controller
    {
        private readonly string _name;

        public GistsController GistsController { get; private set; }

        public GistController(Client client, GistsController gistsController, string name)
            : base(client)
        {
            _name = name;
            GistsController = gistsController;
        }

        public GitHubResponse<GistModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<GistModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public void Star()
        {
            Client.Put(Uri + "/star");
        }

        public void Unstar()
        {
            Client.Delete(Uri + "/star");
        }

        public GitHubResponse<List<GistCommentModel>> GetComments(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<GistCommentModel>>(Uri + "/comments", forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<GistModel> ForkGist()
        {
            return Client.Post<GistModel>(Uri + "/forks", null);
        }

        public void Delete()
        {
            Client.Delete(Uri);
        }

        public bool IsGistStarred()
        {
            try
            {
                var response = Client.ExecuteRequest(Client.ApiUri + "/" + Uri + "/star", RestSharp.Method.GET, null);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
            }
            catch (NotFoundException)
            {
                //This means the gist was NOT starred...
            }
            return false;
        }

        public GitHubResponse<GistCommentModel> CreateGistComment(string body)
        {
            return Client.Post<GistCommentModel>(Uri + "/comments", new { body = body });
        }

        public GitHubResponse<GistModel> EditGist(GistEditModel gist)
        {
            var obj = new RestSharp.JsonObject();
            obj.Add(new KeyValuePair<string, object>("description", gist.Description));

            var files = new RestSharp.JsonObject();
            obj.Add(new KeyValuePair<string, object>("files", files));

            if (gist.Files != null)
            {
                foreach (var f in gist.Files.Keys)
                {
                    if (gist.Files[f] == null)
                    {
                        files.Add(new KeyValuePair<string, object>(f, null));
                    }
                    else
                    {
                        var content = new RestSharp.JsonObject();
                        files.Add(new KeyValuePair<string, object>(f, content));
                        content.Add(new KeyValuePair<string, object>("content", gist.Files[f].Content));

                        if (gist.Files[f].Filename != null)
                            content.Add(new KeyValuePair<string, object>("filename", gist.Files[f].Filename));
                    }
                }
            }

            return Client.Patch<GistModel>(Uri, obj);
        }

        public override string Uri
        {
            get { return GistsController.Uri + "/" + _name; }
        }
    }
}
