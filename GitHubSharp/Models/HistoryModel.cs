using System;

namespace GitHubSharp.Models
{
    
	public class HistoryModel
	{
		public string Url { get; set; }
	    public string Version { get; set; }
	    public BasicUserModel User { get; set; }
	    public ChangeStatusModel ChangeStatus { get; set; }
		public DateTimeOffset CommittedAt { get; set; }

	}
}

