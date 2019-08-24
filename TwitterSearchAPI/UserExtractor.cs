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
    /// Provides user profile extractor with built-in profile parser.
    /// </summary>
    public sealed class UserExtractor : UserExtractor<UserProfile>
    {
        /// <summary>
        /// Creates new instance of the extractor.
        /// </summary>
        /// <param name="client">Http client.</param>
        public UserExtractor(HttpClient client) : base(client, new UserParser<UserProfile>())
        {
        }
    }

    /// <summary>
    /// Provides user profile extractor which accepts custom profile parser.
    /// </summary>
    /// <typeparam name="R">Type of user profile objects.</typeparam>
    public class UserExtractor<R> : ExtractorBase<R> where R : UserProfile
    {
        /// <summary>
        /// Creates new instance of the extractor with given parser and http client.
        /// </summary>
        /// <param name="client">Http client.</param>
        /// <param name="twitterItemParser">Custom profile parser.</param>
        public UserExtractor(HttpClient client, ITwitterItemParser<R> twitterItemParser) : base(client, twitterItemParser)
        {
        }
        /// <summary>
        /// Extracts members profiles uses twitter list url.
        /// </summary>
        /// <param name="info">Execution info. Setup your data here.</param>
        /// <param name="onUsersExtracted">Action which will be called each time when a new part of profiles are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <returns>The async task.</returns>
        public virtual Task ExtractTwitterListMembersAsync([NotNull] TwitterListExecutionInfo info, Action<IEnumerable<UserProfile>> onUsersExtracted, Func<bool> canExecute)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            var initialUrl = info.InitialUrlGenerator != null ? info.InitialUrlGenerator(info.TwitterListUrl) : TwitterUrlHelper.ConstructTwitterListMembersTimelineUrl(info.TwitterListUrl, 0);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => info.NextPageUrlGenerator != null ? info.NextPageUrlGenerator(info.TwitterListUrl, meta) : TwitterUrlHelper.ConstructTwitterListMembersTimelineUrl(info.TwitterListUrl, meta.MaxResult),
                onItemsExtracted: onUsersExtracted,
                canExecute: canExecute,
                executionRate: info.ExecutionRate,
                token: info.CancellationToken);
        }
        /// <summary>
        /// Extracts subscribers profiles uses twitter list url.
        /// </summary>
        /// <param name="info">Execution info. Setup your data here.</param>
        /// <param name="onUsersExtracted">Action which will be called each time when a new part of profiles are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>
        /// <returns>The async task.</returns>
        public virtual Task ExtractTwitterListSubscribersAsync([NotNull] TwitterListExecutionInfo info, Action<IEnumerable<UserProfile>> onUsersExtracted, Func<bool> canExecute)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            var initialUrl = info.InitialUrlGenerator != null ? info.InitialUrlGenerator(info.TwitterListUrl) : TwitterUrlHelper.ConstructTwitterListSubscribersTimelineUrl(info.TwitterListUrl, 0);
            return ExtractAsync(
                initialUrl: initialUrl,
                nextPageUrlGenerator: meta => info.NextPageUrlGenerator != null ? info.NextPageUrlGenerator(info.TwitterListUrl, meta) : TwitterUrlHelper.ConstructTwitterListSubscribersTimelineUrl(info.TwitterListUrl, meta.MaxResult),
                onItemsExtracted: onUsersExtracted,
                canExecute: canExecute,
                executionRate: info.ExecutionRate,
                token: info.CancellationToken);
        }
    }
}
