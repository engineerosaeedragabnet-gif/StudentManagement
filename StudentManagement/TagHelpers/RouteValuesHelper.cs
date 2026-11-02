using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace StudentManagement.TagHelpers
{
    internal static class RouteValuesHelper
    {
        public static RouteValueDictionary FromCurrentQuery(HttpRequest request)
        {
            var values = new RouteValueDictionary();
            foreach (var pair in request.Query)
            {
                var text = pair.Value.ToString();
                if (!string.IsNullOrEmpty(text))
                    values[pair.Key] = text;
            }
            return values;
        }
    }
}