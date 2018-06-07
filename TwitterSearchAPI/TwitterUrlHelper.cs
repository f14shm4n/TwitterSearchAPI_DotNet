using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TwitterSearchAPI
{
    /// <summary>
    /// Provides methods for building urls.
    /// </summary>
    internal static class TwitterUrlHelper
    {
        public const string TYPE_PARAM = "f";
        public const string QUERY_PARAM = "q";
        public const string SCROLL_CURSOR_PARAM = "max_position";
        public const string TWITTER_TIMELINE_URL = "https://twitter.com/i/search/timeline";

        public static string ConstructTimelineUrl(string query, string maxPosition)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new InvalidQueryException(query);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[QUERY_PARAM] = HttpUtility.UrlEncode(query);
            parameters[TYPE_PARAM] = "tweets";

            if (maxPosition != null)
            {
                parameters[SCROLL_CURSOR_PARAM] = maxPosition;
            }
            UriBuilder uriBuilder = new UriBuilder(TWITTER_TIMELINE_URL)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }
    }
}
