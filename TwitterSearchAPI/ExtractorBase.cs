using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterSearchAPI.Models;
using TwitterSearchAPI.Parsers;

namespace TwitterSearchAPI
{
    public class ExtractorBase<T> : ITwitterExtractor<T> where T : ITwitterItem
    {
        protected HttpClient Client { get; }
        protected ITwitterItemParser<T> TwitterItemParser { get; }

        protected ExtractorBase(HttpClient client, ITwitterItemParser<T> twitterItemParser)
        {
            Client = client;
            TwitterItemParser = twitterItemParser;
        }

        #region Events

        public event EventHandler<TwitterExtractorErrorEventArgs> ExtractorError;
        /// <summary>
        /// Occurre when extractor failed to deserialize a response from Twitter.
        /// </summary>
        public virtual event EventHandler<Exception> JsonDeserializationError;

        #endregion

        #region EventRaisers

        protected virtual void RaiseExtractorError(string msg, Exception ex) => ExtractorError?.Invoke(this, new TwitterExtractorErrorEventArgs(msg, ex));
        protected virtual void RaiseJsonError(Exception ex) => JsonDeserializationError?.Invoke(this, ex);

        #endregion

        /// <summary>
        /// Main extraction method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initialUrl">Initial url.</param>
        /// <param name="nextPageUrlGenerator">Next page url generator.</param>
        /// <param name="onItemsExtracted">Action which will be called each time when a new part of items are extracted.</param>
        /// <param name="canExecute">Uses to manage the execution process. Setup it as you need, to control when you want to stop extraction.</param>        
        /// <param name="executionRate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task ExtractAsync(string initialUrl, Func<NextPageMeta, string> nextPageUrlGenerator, Action<IEnumerable<T>> onItemsExtracted, Func<bool> canExecute, TimeSpan executionRate, CancellationToken token)
        {
            // Define vars
            TimelineResponse response = null;
            long minResult = 0;
            string url = initialUrl;
            string payload;
            List<T> items;
            // Start execution
            while ((payload = await ExecuteHttpRequestAsync(url)) != null)
            {
                // Check cancellation
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                // Deserialize data
                try
                {
                    response = JsonConvert.DeserializeObject<TimelineResponse>(payload);
                }
                catch (JsonReaderException ex)
                {
                    RaiseJsonError(ex);
                }
                // Check response
                if (response == null)
                {
                    break;
                }
                // Parse items
                items = TwitterItemParser.Parse(response.ItemsHtml);
                if (items.Count == 0)
                {
                    break;
                }
                // Push tweets to external code
                onItemsExtracted(items);
                // Check if we should to stop the extractor
                if (!canExecute())
                {
                    break;
                }
                // Gather data for generate next page url
                if (minResult == 0)
                {
                    minResult = items.First().Id;
                }
                long maxTweet = items.Last().Id;
                if (minResult != maxTweet)
                {
                    // Wait for some time
                    await Task.Delay(executionRate, token);

                    url = nextPageUrlGenerator(new NextPageMeta(minResult, maxTweet));
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
                return Client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                RaiseExtractorError("Http client error.", ex);
            }
            return null;
        }
    }
}
