using System;
using System.Collections.Generic;
using RestSharp.Serializers;

namespace GitHubSharp.Models
{
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
        public string Payload
        {
            get { return _payload; }
            set
            {
                _payload = value;
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
                switch (Type)
                {
                    case "CommitCommentEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<CommitCommentEvent>(Payload);
                        return;
                    case "CreateEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<CreateEvent>(Payload);
                        return;
                    case "DeleteEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<DeleteEvent>(Payload);
                        return;
                    case "DownloadEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<DownloadEvent>(Payload);
                        return;
                    case "FollowEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<FollowEvent>(Payload);
                        return;
                    case "ForkEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<ForkEvent>(Payload);
                        return;
                    case "ForkApplyEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<ForkApplyEvent>(Payload);
                        return;
                    case "GistEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<GistEvent>(Payload);
                        return;
                    case "GollumEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<GollumEvent>(Payload);
                        return;
                    case "IssueCommentEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<IssueCommentEvent>(Payload);
                        return;
                    case "IssuesEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<IssuesEvent>(Payload);
                        return;
                    case "MemberEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<MemberEvent>(Payload);
                        return;
                    case "PublicEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<PublicEvent>(Payload);
                        return;
                    case "PullRequestEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<PullRequestEvent>(Payload);
                        return;
                    case "PullRequestReviewCommentEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<PullRequestReviewCommentEvent>(Payload);
                        return;
                    case "PushEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<PushEvent>(Payload);
                        return;
                    case "TeamAddEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<TeamAddEvent>(Payload);
                        return;
                    case "WatchEvent":
                        PayloadObject = new RestSharp.Deserializers.JsonDeserializer().Deserialize<WatchEvent>(Payload);
                        return;
                }
            } 
            catch
            {
            }
        }

        public class RepoModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class CommitCommentEvent
        {
            public CommentModel Comment { get; set; }
        }

        public class CreateEvent
        {
            public string RefType { get; set; }
            public string Ref { get; set; }
            public string MasterBranch { get; set; }
            public string Description { get; set; }
        }

        public class DeleteEvent
        {
            public string RefType { get; set; }
            public string Ref { get; set; }
        }

        public class DownloadEvent
        {
        }

        public class FollowEvent
        {
            public BasicUserModel Target { get; set; }
        }

        public class ForkEvent
        {
            [SerializeAs(Name="forkee")]
            public RepositoryModel Forkee { get; set; }
        }

        public class ForkApplyEvent
        {
            public string Head { get; set; }
            public string Before { get; set; }
            public string After { get; set; }
        }

        public class GistEvent
        {
            public string Action { get; set; }
            public GistModel Gist { get; set; }
        }

        public class GollumEvent
        {
        }

        public class IssueCommentEvent
        {
            public string Action { get; set; }
            public IssueModel Issue { get; set; }
            public CommentModel Comment { get; set; }
        }

        public class IssuesEvent
        {
            public string Action { get; set; }
            public IssueModel Issue { get; set; }
        }

        public class MemberEvent
        {
            public BasicUserModel Member { get; set; }
            public string Action { get; set; }
        }

        public class PublicEvent
        {
        }

        public class PullRequestEvent
        {
            public string Action { get; set; }
            public int Number { get; set; }
            public PullRequestModel PullRequest { get; set; }
        }

        public class PullRequestReviewCommentEvent
        {
            public CommentModel Comment { get; set; }
        }

        public class PushEvent
        {
            public string Before { get; set; }
            public string Ref { get; set; }
            public int Size { get; set; }
            public List<CommitModel> Commits { get; set; }

            public class CommitModel
            {
                public CommitAuthorModel Author { get; set; }
                public bool Distinct { get; set; }
                public string Url { get; set; }
                public string Message { get; set; }
                public string Sha { get; set; }

                public class CommitAuthorModel
                {
                    public string Name { get; set; }
                    public string Email { get; set; }
                }
            }
        }

        public class TeamAddEvent
        {
        }

        public class WatchEvent
        {
            public string Action { get; set; }
        }
    }
}



