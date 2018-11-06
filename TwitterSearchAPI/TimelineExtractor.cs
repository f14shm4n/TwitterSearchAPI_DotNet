using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI
{
    public class TimelineExtractor : TwitterDataExtractor<TweetsFeedResult>
    {
        public TimelineExtractor(HttpClient httpClient, Func<bool> canExecute, Action<TweetsFeedResult> onResultReady) : base(httpClient, canExecute, onResultReady)
        {
        }

        protected override string GetInitialUrl(string query, long maxPosition) => TwitterUrlHelper.ConstructTimelineUrl(query, maxPosition);

        protected override string GetNextPageUrl(string query, long minPosition, long maxPosition) => TwitterUrlHelper.ConstructTimelineUrl(query, maxPosition);

        protected override TweetsFeedResult ResultFactory(string query, List<Tweet> tweets) => new TweetsFeedResult(tweets);
    }
}
