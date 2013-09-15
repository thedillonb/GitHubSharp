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

        public GitHubResponse<List<IssueModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100, 
                                                       string milestone = null, string state = null, string assignee = null, 
                                                       string creator = null, string mentioned = null, string labels = null, 
                                                       string sort = null, string direction = null)
        {
            return Client.Get<List<IssueModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage, additionalArgs: new {
                Milestone = milestone, State = state, Assignee = assignee,
                Creator = creator, Mentioned = mentioned, Labels = labels,
                Sort = sort, Direction = direction
            });
        }
    }

    public class AuthenticatedUserIssuesController : Controller
    {
        public AuthenticatedUserIssuesController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<List<IssueModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100, 
                                                       string filter = null, string state = null, string labels = null,
                                                       string sort = null, string direction = null, string since = null)
        {
            return Client.Get<List<IssueModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage, additionalArgs: new {
                Filter = filter, State = state, Labels = labels,
                Sort = sort, Direction = direction, Since = since
            });
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

        public IssueController this[long id]
        {
            get { return new IssueController(Client, this, id); }
        }

        public RepositoryIssuesController(Client client, RepositoryController parent)
            : base(client)
        {
            Parent = parent;
        }

        public GitHubResponse<IssueModel> Create(string title, string body, string assignee, int? milestone, string[] labels)
        {
            return Client.Post<IssueModel>(Uri, new { title = title, body = body, assignee = assignee, milestone = milestone, labels = labels });
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

        public GitHubResponse<IssueCommentModel> CreateComment(string body)
        {
            return Client.Post<IssueCommentModel>(Uri + "/comments", new { body = body });
        }

        public GitHubResponse<IssueModel> Update(string title, string body, string state, string assignee, int? milestone, string[] labels)
        {
            return Client.Patch<IssueModel>(Uri, new { title = title, body = body, assignee = assignee, milestone = milestone, labels = labels, state = state });
        } 

        public override string Uri
        {
            get { return Parent.Uri + "/" + Id;  }
        }
    }
}
