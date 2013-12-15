using System;

namespace GitHubSharp.Models
{
	[Serializable]
    public class ReleaseModel
    {
		public string Url { get; set; }
		public string HtmlUrl { get; set; }
		public string AssetsUrl { get; set; }
		public string UploadUrl { get; set; }
		public ulong Id { get; set; }
		public string TagName { get; set; }
		public string TargetCommitish { get; set; }
		public string Name { get; set; }
		public string Body { get; set; }
		public bool Draft { get; set; }
		public bool Prerelease { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime PublishedAt { get; set; }
    }
}

