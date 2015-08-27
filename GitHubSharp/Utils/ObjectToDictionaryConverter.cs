using System.Collections.Generic;
using System.Reflection;

namespace GitHubSharp.Utils
{
    public static class ObjectToDictionaryConverter
    {
        public static Dictionary<string, string> Convert(object obj)
        {
            var dictionary = new Dictionary<string, string>();
            var properties = obj.GetType().GetRuntimeProperties();
            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(obj, null);
                if (value != null)
                {
                    var valueStr = value.ToString();

                    //Booleans need lowercase!
                    if (value is bool)
                        valueStr = valueStr.ToLower();

                    dictionary.Add(propertyInfo.Name.ToLower(), valueStr);
                }
            }
            return dictionary;
        }
    }
}
