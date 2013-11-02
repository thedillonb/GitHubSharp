using System;

namespace GitHubSharp.Models
{
    [Serializable]
	public class ChangeStatusModel
	{
		public uint Deletions { get; set; }
        public uint Additions { get; set; }
        public uint Total { get; set; }
	}
}

