using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GitHubSharp
{
	internal static class StringExtensions
	{
		// :trollface:
		[SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
			Justification = "Ruby don't care. Ruby don't play that.")]
		public static string ToRubyCase(this string propertyName)
		{
			return string.Join("_", propertyName.SplitUpperCase()).ToLowerInvariant();
		}

		static IEnumerable<string> SplitUpperCase(this string source)
		{
			int wordStartIndex = 0;
			var letters = source.ToCharArray();
			var previousChar = char.MinValue;

			// Skip the first letter. we don't care what case it is.
			for (int i = 1; i < letters.Length; i++)
			{
				if (char.IsUpper(letters[i]) && !char.IsWhiteSpace(previousChar))
				{
					//Grab everything before the current character.
					yield return new String(letters, wordStartIndex, i - wordStartIndex);
					wordStartIndex = i;
				}
				previousChar = letters[i];
			}

			//We need to have the last word.
			yield return new String(letters, wordStartIndex, letters.Length - wordStartIndex);
		}
	}

    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {
        protected override string ResolvePropertyName(string propertyName) {
            return propertyName.ToRubyCase();
        }
    }

	public class SimpleJsonSerializer : IJsonSerializer
	{
        private readonly Newtonsoft.Json.JsonSerializerSettings _settings;
        public SimpleJsonSerializer()
        {
            _settings = new Newtonsoft.Json.JsonSerializerSettings();
            _settings.ContractResolver = new LowercaseContractResolver();
            _settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            _settings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
        }

		public string Serialize(object item)
		{
            return Newtonsoft.Json.JsonConvert.SerializeObject(item, _settings);
		}

		public T Deserialize<T>(string json)
		{
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, _settings);
		}
	}
}
