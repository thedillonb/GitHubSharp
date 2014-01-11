using System;

namespace GitHubSharp.Models
{
	[Serializable]
	public class ContentUpdateModel
    {
		public SlimContentModel Content { get; set; }

		public CommitModel Commit { get; set; }
    }

	[Serializable]
	public class SlimContentModel
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string Sha { get; set; }
		public ulong? Size { get; set; }
		public string Url { get; set; }
		public string HtmlUrl { get; set; }
		public string GitUrl { get; set; }
		public string Type { get; set; }
	}
}

