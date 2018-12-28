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
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;
using TwitterSearchAPI.Parsers;

namespace TwitterSearchAPI
{
    /// <summary>
    /// Provides API for extracting data from Twitter without TwitterAPI.
    /// </summary>
    public class TwitterExtractor
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates new instance of extractor.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        public TwitterExtractor([NotNull] HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Public Events

        /// <summary>
        /// Occurre when extractor failed to deserialize a response from Twitter.
        /// </summary>
        public virtual event Action<Exception> JsonDeserializationError;

        #endregion

        #region Public API        

        /// <summary>
        /// Executes search query to the Twitter. This method uses the Twitter endpoint: https://twitter.com/search-home.
        /// </summary>
        /// <param name="info">Execution info. Setup your data here.</param>
        /// <param name="onTweetsExtracted">Action which will be called each time when a new part of tweets are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <returns>The async task.</returns>
        public virtual Task SearchTweetsAsync([NotNull] SearchExecutionInfo info, Action<IEnumerable<Tweet>> onTweetsExtracted, Func<bool> canExecute)
        {
            var initialUrl = TwitterUrlHelper.ConstructSearchTimelineUrl(info.Query, null);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => TwitterUrlHelper.ConstructSearchTimelineUrl(info.Query, "TWEET-" + meta.MaxTweet + "-" + meta.MinTweet),
                onTweetsExtracted: onTweetsExtracted,
                canExecute: canExecute,
                info: info);
        }
        /// <summary>
        /// Extracts tweets from particular user profile timeline.
        /// </summary>
        /// <param name="info">Execution info. Setup your data here.</param>
        /// <param name="onTweetsExtracted">Action which will be called each time when a new part of tweets are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <returns>The async task.</returns>
        public virtual Task ExtractTweetsFromUserTimelineAsync([NotNull] ProfileTimelineExecutionInfo info, Action<IEnumerable<Tweet>> onTweetsExtracted, Func<bool> canExecute)
        {
            var initialUrl = TwitterUrlHelper.ConstructProfileTimelineUrl(info.UserScreenName, 0);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => TwitterUrlHelper.ConstructProfileTimelineUrl(info.UserScreenName, meta.MaxTweet),
                onTweetsExtracted: onTweetsExtracted,
                canExecute: canExecute,
                info: info);
        }
        /// <summary>
        /// Extracts tweets from particular timeline.
        /// </summary>
        /// <param name="info">Execution info. Setup your data here.</param>
        /// <param name="onTweetsExtracted">Action which will be called each time when a new part of tweets are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <returns>The async task.</returns>
        public virtual Task ExtractTweetsFromTimelineAsync([NotNull] TimelineExecutionInfo info, Action<IEnumerable<Tweet>> onTweetsExtracted, Func<bool> canExecute)
        {
            var initialUrl = TwitterUrlHelper.ConstructTimelineUrl(info.TimelineUrl, 0);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => TwitterUrlHelper.ConstructTimelineUrl(info.TimelineUrl, meta.MaxTweet),
                onTweetsExtracted: onTweetsExtracted,
                canExecute: canExecute,
                info: info);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Main extraction method.
        /// </summary>
        /// <param name="initialUrl">Initial url.</param>
        /// <param name="nextPageUrlGenerator">Next page url generator.</param>
        /// <param name="onTweetsExtracted">Action which will be called each time when a new part of tweets are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <param name="info">Base execution info.</param>
        /// <returns></returns>
        protected virtual async Task ExtractAsync(string initialUrl, Func<NextPageMeta, string> nextPageUrlGenerator, Action<IEnumerable<Tweet>> onTweetsExtracted, Func<bool> canExecute, ExecutionInfo info)
        {
            // Define vars
            TimelineResponse response = null;
            long minTweet = 0;
            string url = initialUrl;
            string payload = null;
            List<Tweet> tweets;
            // Start execution
            while ((payload = await ExecuteHttpRequestAsync(url)) != null)
            {
                // Check cancellation
                if (info.CancellationToken.IsCancellationRequested)
                {
                    info.CancellationToken.ThrowIfCancellationRequested();
                }
                // Deserialize data
                try
                {
                    response = JsonConvert.DeserializeObject<TimelineResponse>(payload);
                }
                catch (JsonReaderException ex)
                {
                    JsonDeserializationError?.Invoke(ex);
                }
                // Check response
                if (response == null)
                {
                    break;
                }
                // Parse tweets
                tweets = TwitterParser.ParseTweets(response.ItemsHtml);
                if (tweets.Count == 0)
                {
                    break;
                }
                // Push tweets to external code
                onTweetsExtracted(tweets);
                // Check if we should to stop the extractor
                if (!canExecute())
                {
                    break;
                }
                // Gather data for generate next page url
                if (minTweet == 0)
                {
                    minTweet = tweets.First().Id;
                }
                long maxTweet = tweets.Last().Id;
                if (minTweet != maxTweet)
                {
                    // Wait for some time
                    await Task.Delay(info.ExecutionRate, info.CancellationToken);

                    url = nextPageUrlGenerator(new NextPageMeta(minTweet, maxTweet));
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Preforms http request.
        /// </summary>
        /// <param name="url">Url.</param>
        /// <returns></returns>
        protected virtual Task<string> ExecuteHttpRequestAsync(string url)
        {
            try
            {
                return _httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return null;
        }

        #endregion        
    }
}
