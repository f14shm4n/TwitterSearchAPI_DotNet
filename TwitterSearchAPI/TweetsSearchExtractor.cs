using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;
using TwitterSearchAPI.Parsers;

namespace TwitterSearchAPI
{
    public class TweetsSearchExtractor : TwitterDataExtractor<TweetsSearchResult>
    {
        public TweetsSearchExtractor(HttpClient httpClient, Func<bool> canExecute, Action<TweetsSearchResult> onResultReady) : base(httpClient, canExecute, onResultReady)
        {
        }

        protected override string GetInitialUrl(string query, long maxPosition) => TwitterUrlHelper.ConstructSearchTimelineUrl(query, maxPosition);

        protected override string GetNextPageUrl(string query, long minPosition, long maxPosition)
        {
            string nextPosition = "TWEET-" + maxPosition + "-" + minPosition;
            return TwitterUrlHelper.ConstructSearchTimelineUrl(query, nextPosition);
        }

        protected override TweetsSearchResult ResultFactory(string query, List<Tweet> tweets) => new TweetsSearchResult(query, tweets);
    }
}
