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

namespace TwitterSearchAPI
{
    /// <summary>
    /// The tweets search engine.
    /// </summary>
    public class TwitterSearch
    {
        private Func<bool> _canExecute;
        private Action<HttpClient> _configureHttpClient;
        private CancellationToken _cToken;

        /// <summary>
        /// Creates new instance of the twitter search.
        /// </summary>
        /// <param name="canExecute">Determines whether the twitter search can execute next iteration.</param>
        public TwitterSearch([NotNull]Func<bool> canExecute) : this(canExecute, null)
        {
            _canExecute = canExecute;
        }
        /// <summary>
        /// Creates new instance of the twitter search.
        /// </summary>
        /// <param name="canExecute">Determines whether the twitter search can execute next iteration.</param>
        /// <param name="configure">Configures the http client when it created.</param>
        public TwitterSearch([NotNull] Func<bool> canExecute, [CanBeNull] Action<HttpClient> configure)
        {
            _canExecute = canExecute;
            _configureHttpClient = configure;
        }

        /// <summary>
        /// Configures the http client when it created.
        /// </summary>
        /// <param name="configure">Configure action.</param>
        public void ConfigureHttpClient([CanBeNull] Action<HttpClient> configure) => _configureHttpClient = configure;
        /// <summary>
        /// Occurs when the next list of tweets is ready.
        /// </summary>
        public event EventHandler<TwitterSearchEventArgs> TweetListReady;
        /// <summary>
        /// Executes tweets search.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="rateInMillis">"The search iteration interval."</param>
        /// <returns>The search task.</returns>
        /// <exception cref="InvalidQueryException"></exception>
        public Task SearchAsync([NotNull] string query, int rateInMillis) => SearchAsync(query, rateInMillis, CancellationToken.None);
        /// <summary>
        /// Executes tweets search.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="rateInMillis">"The search iteration interval."</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>The search task.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="InvalidQueryException"></exception>
        public async Task SearchAsync([NotNull] string query, int rateInMillis, CancellationToken token)
        {
            TwitterTimelineResponse response = null;
            string minTweet = null;
            string url = TwitterUrlHelper.ConstructTimelineUrl(query, minTweet);
            string payload = null;
            List<Tweet> tweets;

            while ((payload = await ExecuteHttpRequestAsync(url)) != null)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                
                try
                {
                    response = JsonConvert.DeserializeObject<TwitterTimelineResponse>(payload);
                }
                catch (JsonReaderException ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                if (response == null)
                {
                    break;
                }

                tweets = TwitterParser.ParseTweets(response.items_html);
                if (tweets.Count == 0)
                {
                    break;
                }

                TweetListReady?.Invoke(this, new TwitterSearchEventArgs(query, tweets));

                if (minTweet == null)
                {
                    minTweet = tweets.First().Id;
                }

                if (!_canExecute())
                {
                    break;
                }

                string maxTweet = tweets.Last().Id;
                if (!minTweet.Equals(maxTweet))
                {
                    await Task.Delay(rateInMillis, token);

                    string maxPosition = "TWEET-" + maxTweet + "-" + minTweet;
                    url = TwitterUrlHelper.ConstructTimelineUrl(query, maxPosition);
                }
                else
                {
                    break;
                }
            }
        }

        private async Task<string> ExecuteHttpRequestAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    _configureHttpClient?.Invoke(client);
                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}
