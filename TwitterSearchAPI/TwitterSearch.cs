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
        public abstract bool CanExecuteNext(List<Tweet> tweets);

        public async Task Search(string query, long rateInMillis)
        {
            TwitterResponse response;
            string url = ConstructURL(query, null);
            bool continueSearch = true;
            string minTweet = null;

            while ((response = await ExecuteSearch(url)) != null && continueSearch && response.GetTweets().Count > 0)
            {
                if (minTweet == null)
                {
                    minTweet = response.GetTweets().First().Id;
                }

                continueSearch = CanExecuteNext(response.GetTweets());

                string maxTweet = response.GetTweets().Last().Id;

                if (!minTweet.Equals(maxTweet))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(rateInMillis));

                    string maxPosition = "TWEET-" + maxTweet + "-" + minTweet;
                    url = ConstructURL(query, maxPosition);
                }
                else
                {
                    continueSearch = false;
                }
            }
        }

        public static async Task<TwitterResponse> ExecuteSearch(string url)
        {
            try
            {
                string html = null;
                using (var client = new HttpClient())
                {
                    html = await client.GetStringAsync(url);
                }

                if (!string.IsNullOrWhiteSpace(html))
                {
                    return JsonConvert.DeserializeObject<TwitterResponse>(html);
                }
            }
            catch (Exception)
            {
                try
                {
                    await Task.Delay(5000);
                    return await ExecuteSearch(url);
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2.ToString());
                }
            }

            return null;
        }

        public const string TYPE_PARAM = "f";
        public const string QUERY_PARAM = "q";
        public const string SCROLL_CURSOR_PARAM = "max_position";
        public const string TWITTER_SEARCH_URL = "https://twitter.com/i/search/timeline";

        public static string ConstructURL(string query, string maxPosition)
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
            UriBuilder uriBuilder = new UriBuilder(TWITTER_SEARCH_URL)
            {
                Query = parameters.ToString()
            };
            return uriBuilder.ToString();
        }
    }
}
