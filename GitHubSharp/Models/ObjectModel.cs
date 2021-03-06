﻿using System.Runtime.Serialization;
using System;

namespace GitHubSharp.Models
{
    
    public class ObjectModel
    {
        public string Name { get; set; }
        public string Sha { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public ObjectItemType ObjectItemType
        {
            get { return Type == "blob" ? ObjectItemType.Blob : ObjectItemType.Tree; }
        }
    }

    
    public class BlobModel
    {
        public string Name { get; set; }
        public string Sha { get; set; }
        public string Mode { get; set; }
        public long Size { get; set; }
        public string MimeType { get; set; }
        public string Data { get; set; }
    }

    
    public enum ObjectItemType
    {
        Blob,
        Tree
    }
}