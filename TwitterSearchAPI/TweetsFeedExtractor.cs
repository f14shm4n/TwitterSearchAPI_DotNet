using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI
{
    public class TweetsFeedExtractor : TwitterDataExtractor<TweetsFeedResult>
    {
        public TweetsFeedExtractor(HttpClient httpClient, Func<bool> canExecute, Action<TweetsFeedResult> onResultReady) : base(httpClient, canExecute, onResultReady)
        {
        }

        protected override string GetInitialUrl(string query, long maxPosition) => TwitterUrlHelper.ConstructProfileTimelineUrl(query, maxPosition);
        protected override string GetNextPageUrl(string query, long minPosition, long maxPosition) => TwitterUrlHelper.ConstructProfileTimelineUrl(query, maxPosition);

        protected override TweetsFeedResult ResultFactory(string query, List<Tweet> tweets) => new TweetsFeedResult(tweets);
    }
}
