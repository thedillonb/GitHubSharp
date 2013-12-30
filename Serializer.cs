using System;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace GitHubSharp
{
	internal static class Serializer
    {
		private static readonly Newtonsoft.Json.JsonSerializerSettings Settings = new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new JsonLowerCaseUnderscoreContractResolver() };
		public static T Deserialize<T>(string data)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data, Settings);
		}

		public static string Serialize(object data)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None, Settings);
		}

		public class JsonLowerCaseUnderscoreContractResolver : DefaultContractResolver
		{
			private readonly Regex regex = new Regex("(?!(^[A-Z]))([A-Z])");

			protected override string ResolvePropertyName(string propertyName)
			{
				return regex.Replace(propertyName, "_$2").ToLower();
			}
		}
    }
}

