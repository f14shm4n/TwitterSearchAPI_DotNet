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
    public abstract class TwitterDataExtractor<T>
    {
        private readonly HttpClient _httpClient;
        private readonly Func<bool> _canExecute;
        private readonly Action<T> _onResultReady;

        protected TwitterDataExtractor(HttpClient httpClient, Func<bool> canExecute, Action<T> onResultReady)
        {
            _httpClient = httpClient;
            _canExecute = canExecute;
            _onResultReady = onResultReady;
        }

        #region Abstract

        protected abstract string GetInitialUrl(string query, string maxPosition);
        protected abstract string GetNextPageUrl(string query, string minPosition, string maxPosition);
        protected abstract T ResultFactory(string query, List<Tweet> tweets);

        #endregion

        #region Utils

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

        public Task ExtractAsync([NotNull] string query, int rateInMillis) => ExtractAsync(query, rateInMillis, CancellationToken.None);

        public async Task ExtractAsync([NotNull] string query, int rateInMillis, CancellationToken token)
        {
            TwitterTimelineResponse response = null;
            string minTweet = null;
            string url = GetInitialUrl(query, minTweet);
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

                _onResultReady(ResultFactory(query, tweets));

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

                    url = GetNextPageUrl(query, minTweet, maxTweet);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
