using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitHubSharp.Models;

namespace GitHubSharp.Controllers
{
    public class PullRequestsController : Controller
    {
        public RepositoryController Parent { get; private set; }

        public PullRequestController this[long id]
        {
            get { return new PullRequestController(Client, this, id); }
        }

        public PullRequestsController(Client client, RepositoryController parent)
            : base(client)
        {
            Parent = parent;
        }

        public GitHubRequest<List<PullRequestModel>> GetAll(int page = 1, int perPage = 100, string state = "open")
        {
            return GitHubRequest.Get<List<PullRequestModel>>(Uri, new { page = page, per_page = perPage, state = state });
        }

        public override string Uri
        {
            get { return Parent.Uri + "/pulls"; }
        }
    }

    public class PullRequestController : Controller
    {
        public PullRequestsController Parent { get; private set; }

        public long Id { get; private set; }

        public PullRequestCommentsController Comments
        {
            get { return new PullRequestCommentsController(Client, this); }   
        }

        public PullRequestController(Client client, PullRequestsController parent, long id) 
            : base(client)
        {
            Parent = parent;
            Id = id;
        }

        public GitHubRequest<PullRequestModel> Get()
        {
            return GitHubRequest.Get<PullRequestModel>(Uri);
        }

        public GitHubRequest<List<CommitModel.CommitFileModel>> GetFiles(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<CommitModel.CommitFileModel>>(Uri + "/files", new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<CommitModel>> GetCommits(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<CommitModel>>(Uri + "/commits", new { page = page, per_page = perPage });
        }

        public GitHubRequest<PullRequestMergeModel> Merge(string commit_message = null)
        {
            return GitHubRequest.Put<PullRequestMergeModel>(Uri + "/merge", new { commit_message = commit_message });
        }

        public GitHubRequest<PullRequestModel> UpdateState(string state)
        {
            return GitHubRequest.Patch<PullRequestModel>(Uri, new { state });
        } 

        public GitHubRequest<bool> IsMerged()
        {
            return GitHubRequest.Get<bool>(Uri + "/merge");
        }

        public override string Uri
        {
            get { return Parent.Uri + "/" + Id; }
        }
    }
}

