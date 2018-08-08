using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TwitterSearchAPI.Helpers
{
    /// <summary>
    /// Provides methods for building urls.
    /// </summary>
    internal static class TwitterUrlHelper
    {
        public const string TYPE_PARAM = "f";
        public const string QUERY_PARAM = "q";
        public const string SCROLL_CURSOR_PARAM = "max_position";
        public const string TWITTER_SEARCH_TIMELINE_URL = "https://twitter.com/i/search/timeline";
        public const string TWITTER_PROFILE_TIMELINE_URL = "https://twitter.com/i/profiles/show/{0}/timeline/tweets";

        public static string ConstructSearchTimelineUrl(string query, string maxPosition)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new InvalidQueryException(query);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[QUERY_PARAM] = query;
            parameters[TYPE_PARAM] = "tweets";

            if (maxPosition != null)
            {
                parameters[SCROLL_CURSOR_PARAM] = maxPosition;
            }
            UriBuilder uriBuilder = new UriBuilder(TWITTER_SEARCH_TIMELINE_URL)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }

        public static string ConstructProfileTimelineUrl(string userScreenName, string maxPosition)
        {
            if (string.IsNullOrWhiteSpace(userScreenName))
            {
                throw new InvalidQueryException(userScreenName);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            if (maxPosition != null)
            {
                parameters[SCROLL_CURSOR_PARAM] = maxPosition;
            }
            UriBuilder uriBuilder = new UriBuilder(string.Format(TWITTER_PROFILE_TIMELINE_URL, userScreenName))
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }
    }
}
