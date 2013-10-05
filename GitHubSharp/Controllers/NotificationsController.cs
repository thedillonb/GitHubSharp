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

        public GitHubRequest<NotificationModel> Get()
        {
            return GitHubRequest.Get<NotificationModel>(Client, Uri);
        }

        public GitHubRequest<bool> MarkAsRead()
        {
            return GitHubRequest.Patch<bool>(Uri);
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/notifications/threads/" + Id; }
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

        public GitHubRequest<List<NotificationModel>> GetAll(int page = 1, int perPage = 100, bool? all = null, bool? participating = null)
        {
            return GitHubRequest.Get<List<NotificationModel>>(Client, Uri, new { page = page, per_page = perPage, all = all, participating = participating });
        }

        public GitHubRequest<bool> MarkAsRead(DateTime? lastReadAt)
        {
            var data = new Dictionary<string,string>();
            if (lastReadAt != null)
                data.Add("last_read_at", string.Concat(lastReadAt.Value.ToString("s"), "Z"));
            return GitHubRequest.Put<bool>(Uri, data);
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
