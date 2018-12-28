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

        public static string ConstructTwitterListMembersTimelineUrl(string twitterListUrl, long maxPosition) => ConstructUrlForTimeline(twitterListUrl, "/members/timeline", maxPosition);

        public static string ConstructTwitterListSubscribersTimelineUrl(string twitterListUrl, long maxPosition) => ConstructUrlForTimeline(twitterListUrl, "/subscribers/timeline", maxPosition);

        public static string ConstructTimelineUrl(string timelineUrl, long maxPosition) => ConstructUrlForTimeline(timelineUrl, "/timeline", maxPosition);

        public static string ConstructUrlForTimeline(string url, string urlPostfix, long maxPosition)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(url);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[TYPE_PARAM] = "tweets";

            if (maxPosition > 0)
            {
                parameters[SCROLL_CURSOR_PARAM] = maxPosition.ToString();
            }

            if (!url.EndsWith(urlPostfix))
            {
                url = url.TrimEnd('/') + urlPostfix;
            }

            UriBuilder uriBuilder = new UriBuilder(url)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }

        public static string ConstructSearchTimelineUrl(string query, string scrollCursorParam)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new InvalidQueryException(query);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[QUERY_PARAM] = query;
            parameters[TYPE_PARAM] = "tweets";

            if (scrollCursorParam != null)
            {
                parameters[SCROLL_CURSOR_PARAM] = scrollCursorParam;
            }
            UriBuilder uriBuilder = new UriBuilder(TWITTER_SEARCH_TIMELINE_URL)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }

        public static string ConstructProfileTimelineUrl(string userScreenName, long maxPosition)
        {
            if (string.IsNullOrWhiteSpace(userScreenName))
            {
                throw new InvalidQueryException(userScreenName);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            if (maxPosition > 0)
            {
                parameters[SCROLL_CURSOR_PARAM] = maxPosition.ToString();
            }
            UriBuilder uriBuilder = new UriBuilder(string.Format(TWITTER_PROFILE_TIMELINE_URL, userScreenName))
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }
    }
}
