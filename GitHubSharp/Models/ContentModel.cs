using System;

namespace GitHubSharp.Models
{
	public class ContentUpdateModel
    {
		public SlimContentModel Content { get; set; }

		public CommitModel Commit { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(ContentUpdateModel))
                return false;
            ContentUpdateModel other = (ContentUpdateModel)obj;
            return Content == other.Content && Commit == other.Commit;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Content != null ? Content.GetHashCode() : 0) ^ (Commit != null ? Commit.GetHashCode() : 0);
            }
        }
    }

	public class SlimContentModel
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string Sha { get; set; }
		public long? Size { get; set; }
		public string Url { get; set; }
		public string HtmlUrl { get; set; }
		public string GitUrl { get; set; }
		public string Type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(SlimContentModel))
                return false;
            SlimContentModel other = (SlimContentModel)obj;
            return Name == other.Name && Path == other.Path && Sha == other.Sha && Size == other.Size && Url == other.Url && HtmlUrl == other.HtmlUrl && GitUrl == other.GitUrl && Type == other.Type;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? Name.GetHashCode() : 0) ^ (Path != null ? Path.GetHashCode() : 0) ^ (Sha != null ? Sha.GetHashCode() : 0) ^ (Size != null ? Size.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (GitUrl != null ? GitUrl.GetHashCode() : 0) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }
        
	}
}

