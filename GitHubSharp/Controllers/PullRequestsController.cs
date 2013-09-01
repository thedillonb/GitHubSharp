using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitHubSharp.Models;

namespace GitHubSharp.MonoTouch
{
    public class PullRequestsController : Controller
    {
        public RepositoryController Parent { get; private set; }

        public PullRequestsController(Client client, RepositoryController parent)
            : base(client)
        {
            Parent = parent;
        }

        public GitHubResponse<List<PullRequestModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<PullRequestModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
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

        public GitHubResponse<PullRequestModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<PullRequestModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<CommitModel.CommitFileModel>> GetFiles(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<CommitModel.CommitFileModel>>(Uri + "/files", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<PullRequestMergeModel> Merge()
        {
            return Client.Put<PullRequestMergeModel>(Uri + "/merge");
        }

        public bool IsMerged(bool forceCacheInvalidation = false)
        {
            try
            {
                if (Client.Get(Uri + "/merge", forceCacheInvalidation: forceCacheInvalidation).StatusCode == 204)
                    return true;
            }
            catch (NotFoundException)
            {
            }

            return false;
        }

        public override string Uri
        {
            get { return Parent.Uri + "/" + Id; }
        }
    }
}

