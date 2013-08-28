using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class NotificationController : Controller
    {
        public long Id { get; private set; }

        public NotificationController(Client client, long id)
            : base(client)
        {
            Id = id;
        }

        public GitHubResponse<NotificationModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<NotificationModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public bool MarkAsRead()
        {
            return Client.Patch(Uri).StatusCode == 205;
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/notifications"; }
        }
    }

    public class NotificationsController : Controller
    {
        public NotificationController this[long id]
        {
            get { return new NotificationController(Client, id); }
        }

        public NotificationsController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<List<NotificationModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100, bool all = false, bool participating = false)
        {
            return Client.Get<List<NotificationModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage, additionalArgs: new { All = all, Participating = participating });
        }

        public bool MarkAsRead(DateTime? lastReadAt)
        {
            var data = new Dictionary<string,string>();
            if (lastReadAt != null)
                data.Add("last_read_at", string.Concat(lastReadAt.Value.ToString("s"), "Z"));
            return Client.Put(Uri, data).StatusCode == 205;
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/notifications"; }
        }
    }

    public class RepositoryNotificationsController : NotificationsController
    {
        public RepositoryController Parent { get; private set; }

        public RepositoryNotificationsController(Client client, RepositoryController parent)
            : base(client)
        {
            Parent = parent;
        }

        public override string Uri
        {
            get { return Parent.Uri + "/notifications"; }
        }
    }
}
