using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterSearchAPI.Models;
using Xunit;
using Xunit.Abstractions;

namespace TwitterSearchAPI.Test
{
    public class TweetsSearchExtractorTest
    {
        private ITestOutputHelper log;

        private string Titles = @"Twitter to Join S&P 500, Stock Hits Record Highs – Variety
WSJ investigative journalist John Carreyrou on Recode Decode: transcript
Tencent-Backed Video Streamer Kuaishou Buys Struggling ACFun – Variety";

        public TweetsSearchExtractorTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        [Fact]
        public async Task Search()
        {
            string[] titles = Titles.Split("\r\n");

            List<Tweet> tweets = new List<Tweet>();

            TweetsSearchExtractor searchEngine = new TweetsSearchExtractor(new HttpClient(),
                canExecute: () => tweets.Count <= 20,
                onResultReady: r =>
                {
                    log.WriteLine("Title: {0}, Tweets: {1}", r.Query, r.Tweets.Count);
                    tweets.AddRange(r.Tweets);
                });

            foreach (var t in titles)
            {
                await searchEngine.ExtractAsync(t, 100);
            }

            Assert.NotEmpty(tweets);
        }

        [Theory]
        [InlineData("http://www.theverge.com/2018/8/7/17660564/apple-google-infowars-app-ban-downloads")]
        public async Task SearchUrl(string query)
        {
            List<Tweet> tweets = new List<Tweet>();

            TweetsSearchExtractor searchEngine = new TweetsSearchExtractor(new HttpClient(),
                () => tweets.Count <= 20,
                r =>
                {
                    log.WriteLine("Title: {0}, Tweets: {1}", r.Query, r.Tweets.Count);
                    tweets.AddRange(r.Tweets);
                });

            await searchEngine.ExtractAsync(query, 100);

            Assert.NotEmpty(tweets);
        }
    }
}
