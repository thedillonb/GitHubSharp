using System;

namespace GitHubSharp.Models
{
    [Serializable]
	public class ForkModel
	{
		public BasicUserModel User { get; set; }
	    public string Url { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}

