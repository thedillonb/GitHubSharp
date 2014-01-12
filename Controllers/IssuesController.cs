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

        public GitHubRequest<List<IssueModel>> GetAll(int page = 1, int perPage = 100, 
                                                       string milestone = null, string state = null, string assignee = null, 
                                                       string creator = null, string mentioned = null, string labels = null, 
                                                       string sort = null, string direction = null)
        {
            return GitHubRequest.Get<List<IssueModel>>(Uri, new {
                Milestone = milestone, State = state, Assignee = assignee,
                Creator = creator, Mentioned = mentioned, Labels = labels,
                Sort = sort, Direction = direction,
                page = page, per_page = perPage
            });
        }
    }

    public class AuthenticatedUserIssuesController : Controller
    {
        public AuthenticatedUserIssuesController(Client client)
            : base(client)
        {
        }

        public GitHubRequest<List<IssueModel>> GetAll(int page = 1, int perPage = 100, 
                                                       string filter = null, string state = null, string labels = null,
                                                       string sort = null, string direction = null, string since = null)
        {
            return GitHubRequest.Get<List<IssueModel>>(Uri, new {
                Filter = filter, State = state, Labels = labels,
                Sort = sort, Direction = direction, Since = since,
                page = page, per_page = perPage
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

        public GitHubRequest<IssueModel> Create(string title, string body, string assignee, int? milestone, string[] labels)
        {
            return GitHubRequest.Post<IssueModel>(Uri, new { title = title, body = body, assignee = assignee, milestone = milestone, labels = labels });
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

        public GitHubRequest<IssueModel> Get()
        {
            return GitHubRequest.Get<IssueModel>(Uri);
        }

        public GitHubRequest<List<IssueEventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<IssueEventModel>>(Uri + "/events", new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<IssueCommentModel>> GetComments(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<IssueCommentModel>>(Uri + "/comments", new { page = page, per_page = perPage });
        }

        public GitHubRequest<IssueCommentModel> CreateComment(string body)
        {
            return GitHubRequest.Post<IssueCommentModel>(Uri + "/comments", new { body = body });
        }

        public GitHubRequest<IssueModel> Update(string title, string body, string state, string assignee, int? milestone, string[] labels)
        {
            return GitHubRequest.Patch<IssueModel>(Uri, new { title = title, body = body, assignee = assignee, milestone = milestone, labels = labels, state = state });
        } 

		public GitHubRequest<IssueModel> UpdateAssignee(string assignee)
		{
			return GitHubRequest.Patch<IssueModel>(Uri, new { assignee });
		} 

		public GitHubRequest<IssueModel> UpdateMilestone(int? milestone)
		{
			return GitHubRequest.Patch<IssueModel>(Uri, new { milestone });
		} 

		public GitHubRequest<IssueModel> UpdateLabels(string[] labels)
		{
			return GitHubRequest.Patch<IssueModel>(Uri, new { labels });
		} 

        public override string Uri
        {
            get { return Parent.Uri + "/" + Id;  }
        }
    }
}
