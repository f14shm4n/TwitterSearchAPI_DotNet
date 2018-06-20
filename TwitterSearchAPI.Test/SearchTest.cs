using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TwitterSearchAPI.Test
{
    public class SearchTest
    {
        private ITestOutputHelper log;

        private string Titles = @"Twitter to Join S&P 500, Stock Hits Record Highs – Variety
WSJ investigative journalist John Carreyrou on Recode Decode: transcript
Tencent-Backed Video Streamer Kuaishou Buys Struggling ACFun – Variety";

        public SearchTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        [Fact]
        public async Task Search()
        {
            string[] titles = Titles.Split("\r\n");

            List<Tweet> tweets = new List<Tweet>();

            TwitterSearch searchEngine = new TwitterSearch(new System.Net.Http.HttpClient(), () => tweets.Count <= 20);
            searchEngine.TweetListReady += (s, e) =>
            {
                log.WriteLine("Title: {0}, Tweets: {1}", e.Query, e.Tweets.Count);

                tweets.AddRange(e.Tweets);
            };

            foreach (var t in titles)
            {
                await searchEngine.SearchAsync(t, 100);
            }

            Assert.NotEmpty(tweets);
        }
    }
}
