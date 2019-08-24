using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;
using TwitterSearchAPI.Parsers;

namespace TwitterSearchAPI
{
    /// <summary>
    /// Provides tweets extractor with built-in tweet parser.
    /// </summary>
    public sealed class TweetExtractor : TweetExtractor<Tweet>
    {
        /// <summary>
        /// Creates new instance of the extractor.
        /// </summary>
        /// <param name="client">Http client.</param>
        public TweetExtractor(HttpClient client) : base(client, new TweetParser<Tweet>())
        {
        }
    }
    /// <summary>
    /// Provides tweets extractor which accepts custom tweet parser.
    /// </summary>
    /// <typeparam name="R">Type of tweet objects.</typeparam>
    public class TweetExtractor<R> : ExtractorBase<R> where R : Tweet
    {
        /// <summary>
        /// Creates new instance of the extractor with given parser and http client.
        /// </summary>
        /// <param name="client">Http client.</param>
        /// <param name="parser">Custom tweet parser.</param>
        public TweetExtractor(HttpClient client, ITwitterItemParser<R> parser) : base(client, parser)
        {
        }
        /// <summary>
        /// Executes search query to the Twitter. This method uses the Twitter endpoint: https://twitter.com/search-home.
        /// </summary>
        /// <param name="info">Execution info. Setup your data here.</param>
        /// <param name="onTweetsExtracted">Action which will be called each time when a new part of tweets are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <returns>The async task.</returns>
        public virtual Task SearchTweetsAsync([NotNull] SearchExecutionInfo info, Action<IEnumerable<Tweet>> onTweetsExtracted, Func<bool> canExecute)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            var initialUrl = info.InitialUrlGenerator != null ? info.InitialUrlGenerator(info.Query) : TwitterUrlHelper.ConstructSearchTimelineUrl(info.Query, null);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => info.NextPageUrlGenerator != null ? info.NextPageUrlGenerator(info.Query, meta) : TwitterUrlHelper.ConstructSearchTimelineUrl(info.Query, "TWEET-" + meta.MaxResult + "-" + meta.MinResult),
                onItemsExtracted: onTweetsExtracted,
                canExecute: canExecute,
                executionRate: info.ExecutionRate,
                token: info.CancellationToken);
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
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            var initialUrl = info.InitialUrlGenerator != null ? info.InitialUrlGenerator(info.UserScreenName) : TwitterUrlHelper.ConstructProfileTimelineUrl(info.UserScreenName, 0);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => info.NextPageUrlGenerator != null ? info.NextPageUrlGenerator(info.UserScreenName, meta) : TwitterUrlHelper.ConstructProfileTimelineUrl(info.UserScreenName, meta.MaxResult),
                onItemsExtracted: onTweetsExtracted,
                canExecute: canExecute,
                executionRate: info.ExecutionRate,
                token: info.CancellationToken);
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
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            var initialUrl = info.InitialUrlGenerator != null ? info.InitialUrlGenerator(info.TimelineUrl) : TwitterUrlHelper.ConstructTimelineUrl(info.TimelineUrl, 0);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => info.NextPageUrlGenerator != null ? info.NextPageUrlGenerator(info.TimelineUrl, meta) : TwitterUrlHelper.ConstructTimelineUrl(info.TimelineUrl, meta.MaxResult),
                onItemsExtracted: onTweetsExtracted,
                canExecute: canExecute,
                executionRate: info.ExecutionRate,
                token: info.CancellationToken);
        }
    }
}
