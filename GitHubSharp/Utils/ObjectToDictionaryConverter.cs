using System.Collections.Generic;

namespace GitHubSharp.Utils
{
    public static class ObjectToDictionaryConverter
    {
        public static Dictionary<string, string> Convert(object obj)
        {
            var dictionary = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(obj, null);
                if (value != null)
                {
                    dictionary.Add(propertyInfo.Name.ToLower(), value.ToString());
                }
            }
            return dictionary;
        }
    }
}
