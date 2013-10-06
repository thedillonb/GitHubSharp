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

        public GitHubRequest<List<GistModel>> GetGists(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<GistModel>>(Uri, new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<GistModel>> GetPublicGists(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<GistModel>>(Uri + "/public", new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<GistModel>> GetStarredGists(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<GistModel>>(Uri + "/starred", new { page = page, per_page = perPage });
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

        public GitHubRequest<List<GistModel>> GetGists(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<GistModel>>(Uri, new { page = page, per_page = perPage });
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

        public GitHubRequest<GistModel> CreateGist(GistCreateModel gist)
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

            return GitHubRequest.Post<GistModel>(Client.ApiUri + "/gists", obj);
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

        public GitHubRequest<GistModel> Get()
        {
            return GitHubRequest.Get<GistModel>(Uri);
        }

        public GitHubRequest Star()
        {
            return GitHubRequest.Put(Uri + "/star");
        }

        public GitHubRequest Unstar()
        {
            return GitHubRequest.Delete(Uri + "/star");
        }

        public GitHubRequest<List<GistCommentModel>> GetComments()
        {
            return GitHubRequest.Get<List<GistCommentModel>>(Uri + "/comments");
        }

        public GitHubRequest<GistModel> ForkGist()
        {
            return GitHubRequest.Post<GistModel>(Uri + "/forks");
        }

        public GitHubRequest Delete()
        {
            return GitHubRequest.Delete(Uri);
        }

        public GitHubRequest<bool> IsGistStarred()
        {
            return GitHubRequest.Get<bool>(Client.ApiUri + "/" + Uri + "/star");
        }

        public GitHubRequest<GistCommentModel> CreateGistComment(string body)
        {
            return GitHubRequest.Post<GistCommentModel>(Uri + "/comments", new { body = body });

        }

        public GitHubRequest<GistModel> EditGist(GistEditModel gist)
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

            return GitHubRequest.Patch<GistModel>(Uri, obj);
        }

        public override string Uri
        {
            get { return GistsController.Uri + "/" + _name; }
        }
    }
}
