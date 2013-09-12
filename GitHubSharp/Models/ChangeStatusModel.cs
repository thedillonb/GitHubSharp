using System;

namespace GitHubSharp.Models
{
    [Serializable]
	public class ChangeStatusModel
	{
		public int Deletions { get; set; }
        public int Additions { get; set; }
        public int Total { get; set; }
	}
}

