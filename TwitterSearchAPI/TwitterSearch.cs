using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TwitterSearchAPI
{
    public abstract class TwitterSearch
    {
        protected abstract bool CanExecute();
        protected abstract void OnTweetListReady(List<Tweet> tweets);

        public async Task SearchAsync(string query, long rateInMillis)
        {
            string payload = await ExecuteHttpRequestAsync(ConstructSearchUrl(query));
            if (payload == null)
            {
                return;
            }

            List<Tweet> tweets = TwitterParser.ParseTweets(payload);
            if (tweets.Count == 0)
            {
                return;
            }

            OnTweetListReady(tweets);

            TwitterTimelineResponse response;
            string minTweet = tweets.First().Id;
            string url = ConstructTimelineUrl(query, minTweet);

            while ((payload = await ExecuteHttpRequestAsync(url)) != null)
            {
                response = JsonConvert.DeserializeObject<TwitterTimelineResponse>(payload);
                if (response == null)
                {
                    break;
                }

                tweets = TwitterParser.ParseTweets(response.items_html);
                if (tweets.Count == 0)
                {
                    break;
                }

                OnTweetListReady(tweets);

                // TODO: I guess this can be deleted.
                if (minTweet == null)
                {
                    minTweet = tweets.First().Id;
                }

                if (!CanExecute())
                {
                    break;
                }

                string maxTweet = tweets.Last().Id;
                if (!minTweet.Equals(maxTweet))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(rateInMillis));

                    string maxPosition = "TWEET-" + maxTweet + "-" + minTweet;
                    url = ConstructTimelineUrl(query, maxPosition);
                }
                else
                {
                    break;
                }
            }
        }

        public static async Task<string> ExecuteHttpRequestAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return null;
        }

        public const string TYPE_PARAM = "f";
        public const string QUERY_PARAM = "q";
        public const string SRC_PARAM = "src";
        public const string SCROLL_CURSOR_PARAM = "max_position";
        public const string TWITTER_SEARCH_URL = "https://twitter.com/search";
        public const string TWITTER_TIMELINE_URL = "https://twitter.com/i/search/timeline";

        public static string ConstructSearchUrl(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new InvalidQueryException(query);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[QUERY_PARAM] = query;
            parameters[SRC_PARAM] = "typd";

            UriBuilder uriBuilder = new UriBuilder(TWITTER_SEARCH_URL)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }

        public static string ConstructTimelineUrl(string query, string maxPosition)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new InvalidQueryException(query);
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[QUERY_PARAM] = query;
            parameters[TYPE_PARAM] = "tweets";

            if (maxPosition != null)
            {
                parameters[SCROLL_CURSOR_PARAM] = maxPosition;
            }
            UriBuilder uriBuilder = new UriBuilder(TWITTER_TIMELINE_URL)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }
    }
}
