using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class EventModel
    {
		private string _payload;
        private string _type;

		public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                DeserializePayloadObject();
            }
        }

        public bool Public { get; set; }
		public object Payload
        {
            get { return _payload; }
            set
            {
				_payload = value.ToString();
                DeserializePayloadObject();
            }
        }

        public object PayloadObject { get; set; }
        public RepoModel Repo { get; set; }
        public BasicUserModel Actor { get; set; }
        public BasicUserModel Org { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }

        private void DeserializePayloadObject()
        {
            if (Type == null || Payload == null || PayloadObject != null)
                return;

            try
            {
                // The deserialize function on the JsonSerializer.Deserializer only takes a IResponse object.
                // So, we'll just do what we have to to create one which is just assigning it's content to our payload.
				var payout = Payload.ToString();

                switch (Type)
                {
                    case "CommitCommentEvent":
                        PayloadObject = Serializer.Deserialize<CommitCommentEvent>(payout);
                        return;
                    case "CreateEvent":
                        PayloadObject = Serializer.Deserialize<CreateEvent>(payout);
                        return;
                    case "DeleteEvent":
                        PayloadObject = Serializer.Deserialize<DeleteEvent>(payout);
                        return;
                    case "DownloadEvent":
                        PayloadObject = Serializer.Deserialize<DownloadEvent>(payout);
                        return;
                    case "FollowEvent":
                        PayloadObject = Serializer.Deserialize<FollowEvent>(payout);
                        return;
                    case "ForkEvent":
                        PayloadObject = Serializer.Deserialize<ForkEvent>(payout);
                        return;
                    case "ForkApplyEvent":
                        PayloadObject = Serializer.Deserialize<ForkApplyEvent>(payout);
                        return;
                    case "GistEvent":
                        PayloadObject = Serializer.Deserialize<GistEvent>(payout);
                        return;
                    case "GollumEvent":
                        PayloadObject = Serializer.Deserialize<GollumEvent>(payout);
                        return;
                    case "IssueCommentEvent":
                        PayloadObject = Serializer.Deserialize<IssueCommentEvent>(payout);
                        return;
                    case "IssuesEvent":
                        PayloadObject = Serializer.Deserialize<IssuesEvent>(payout);
                        return;
                    case "MemberEvent":
                        PayloadObject = Serializer.Deserialize<MemberEvent>(payout);
                        return;
                    case "PublicEvent":
                        PayloadObject = Serializer.Deserialize<PublicEvent>(payout);
                        return;
                    case "PullRequestEvent":
                        PayloadObject = Serializer.Deserialize<PullRequestEvent>(payout);
                        return;
                    case "PullRequestReviewCommentEvent":
                        PayloadObject = Serializer.Deserialize<PullRequestReviewCommentEvent>(payout);
                        return;
                    case "PushEvent":
                        PayloadObject = Serializer.Deserialize<PushEvent>(payout);
                        return;
                    case "TeamAddEvent":
                        PayloadObject = Serializer.Deserialize<TeamAddEvent>(payout);
                        return;
                    case "WatchEvent":
                        PayloadObject = Serializer.Deserialize<WatchEvent>(payout);
                        return;
					case "ReleaseEvent":
						PayloadObject = Serializer.Deserialize<ReleaseEvent>(payout);
						return;
                }
            } 
            catch
            {
            }
        }

		[Serializable]
		public class ReleaseEvent
		{
			public string Action { get; set; }
			public ReleaseModel Release { get; set; }
		}

        [Serializable]
        public class RepoModel
        {
            public ulong Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        [Serializable]
        public class CommitCommentEvent
        {
            public CommentModel Comment { get; set; }
        }

        [Serializable]
        public class CreateEvent
        {
            public string RefType { get; set; }
            public string Ref { get; set; }
            public string MasterBranch { get; set; }
            public string Description { get; set; }
        }

        [Serializable]
        public class DeleteEvent
        {
            public string RefType { get; set; }
            public string Ref { get; set; }
        }

        [Serializable]
        public class DownloadEvent
        {
        }

        [Serializable]
        public class FollowEvent
        {
            public BasicUserModel Target { get; set; }
        }

        [Serializable]
        public class ForkEvent
        {
            public RepositoryModel Forkee { get; set; }
        }

        [Serializable]
        public class ForkApplyEvent
        {
            public string Head { get; set; }
            public string Before { get; set; }
            public string After { get; set; }
        }

        [Serializable]
        public class GistEvent
        {
            public string Action { get; set; }
            public GistModel Gist { get; set; }
        }

        [Serializable]
        public class GollumEvent
        {
            public List<PageModel> Pages { get; set; }

            [Serializable]
            public class PageModel
            {
                public string PageName { get; set; }
                public string Sha { get; set; }
                public string Title { get; set; }
                public string Action { get; set; }
                public string HtmlUrl { get; set; }
            }
        }

        [Serializable]
        public class IssueCommentEvent
        {
            public string Action { get; set; }
            public IssueModel Issue { get; set; }
            public CommentModel Comment { get; set; }
        }

        [Serializable]
        public class IssuesEvent
        {
            public string Action { get; set; }
            public IssueModel Issue { get; set; }
        }

        [Serializable]
        public class MemberEvent
        {
            public BasicUserModel Member { get; set; }
            public string Action { get; set; }
        }

        [Serializable]
        public class PublicEvent
        {
        }

        [Serializable]
        public class PullRequestEvent
        {
            public string Action { get; set; }
            public ulong Number { get; set; }
            public PullRequestModel PullRequest { get; set; }
        }

        [Serializable]
        public class PullRequestReviewCommentEvent
        {
            public CommentModel Comment { get; set; }
        }

        [Serializable]
        public class PushEvent
        {
            public string Before { get; set; }
            public string Ref { get; set; }
            public ulong Size { get; set; }
            public List<CommitModel> Commits { get; set; }

            [Serializable]
            public class CommitModel
            {
                public CommitAuthorModel Author { get; set; }
                public bool Distinct { get; set; }
                public string Url { get; set; }
                public string Message { get; set; }
                public string Sha { get; set; }

                [Serializable]
                public class CommitAuthorModel
                {
                    public string Name { get; set; }
                    public string Email { get; set; }
                }
            }
        }

        [Serializable]
        public class TeamAddEvent
        {
            public TeamModel Team { get; set; }
            public BasicUserModel User { get; set; }
            public RepositoryModel Repo { get; set; }
        }

        [Serializable]
        public class WatchEvent
        {
            public string Action { get; set; }
        }
    }
}



