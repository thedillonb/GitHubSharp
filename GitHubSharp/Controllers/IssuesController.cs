using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public abstract class IssuesController : Controller
    {
        public IssuesController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<List<IssueModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<IssueModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }
    }

    public class UserIssuesController : IssuesController
    {
        public UserIssuesController(Client client)
            : base(client)
        {
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/user/issues"; }
        }
    }

    public class OrganizationIssuesController : IssuesController
    {
        public OrganizationController Parent { get; private set; }

        public OrganizationIssuesController(Client client, OrganizationController parent)
            : base(client)
        {
            Parent = parent;
        }

        public override string Uri
        {
            get { return Parent.Uri + "/issues"; }
        }
    }

    public class RepositoryIssuesController : IssuesController
    {
        public RepositoryController Parent { get; private set; }

        public RepositoryIssuesController(Client client, RepositoryController parent)
            : base(client)
        {
            Parent = parent;
        }

        public GitHubResponse<List<IssueEventModel>> GetEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<IssueEventModel>>(Uri + "/events", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return Parent.Uri + "/issues"; }
        }
    }

    public class IssueController : Controller
    {
        public long Id { get; private set; }

        public RepositoryIssuesController Parent { get; private set; }

        public IssueController(Client client, RepositoryIssuesController parent, long id)
            : base(client)
        {
            Parent = parent;
            Id = id;
        }

        public GitHubResponse<IssueModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<IssueModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<IssueEventModel>> GetEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<IssueEventModel>>(Uri + "/events", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<IssueCommentModel>> GetComments(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<IssueCommentModel>>(Uri + "/comments", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return Parent.Uri + "/" + Id;  }
        }
    }
}
