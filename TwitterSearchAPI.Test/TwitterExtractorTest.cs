using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterSearchAPI.Models;
using Xunit;
using Xunit.Abstractions;

namespace TwitterSearchAPI.Test
{
    public class TwitterExtractorTest
    {
        private ITestOutputHelper log;

        public TwitterExtractorTest(ITestOutputHelper log)
        {
            this.log = log;
        }
        
        [Theory]
        [InlineData("Twitter to Join S&P 500, Stock Hits Record Highs – Variety")]
        [InlineData("WSJ investigative journalist John Carreyrou on Recode Decode: transcript")]
        [InlineData("Tencent-Backed Video Streamer Kuaishou Buys Struggling ACFun – Variety")]
        public async Task SearchTweetsAsync(string title)
        {
            List<Tweet> tweets = new List<Tweet>();

            TwitterExtractor extractor = new TwitterExtractor(new HttpClient());
            await extractor.SearchTweetsAsync(
                new SearchExecutionInfo
                {
                    Query = title
                },
                canExecute: () => tweets.Count <= 20,
                onTweetsExtracted: results =>
                {
                    tweets.AddRange(results);
                });

            foreach (var t in tweets)
            {
                log.WriteLine(t.ToString());
            }

            Assert.NotEmpty(tweets);
        }

        [Theory]
        [InlineData("Variety")]
        [InlineData("Recode")]
        public async Task ExtractTweetsFromUserTimelineAsync(string userScreenName)
        {
            List<Tweet> tweets = new List<Tweet>();

            TwitterExtractor extractor = new TwitterExtractor(new HttpClient());
            await extractor.ExtractTweetsFromUserTimelineAsync(
                new ProfileTimelineExecutionInfo
                {
                    UserScreenName = userScreenName
                },
                canExecute: () => tweets.Count <= 20,
                onTweetsExtracted: results => tweets.AddRange(results));

            foreach (var t in tweets)
            {
                log.WriteLine(t.ToString());
            }

            Assert.NotEmpty(tweets);
        }


        [Theory]
        [InlineData("https://twitter.com/cspan/lists/members-of-congress")]
        [InlineData("https://twitter.com/NYTMetro/lists/nyt-nyc-local-news")]
        public async Task ExtractTweetsFromTimelineAsync(string timelineUrl)
        {
            List<Tweet> tweets = new List<Tweet>();

            TwitterExtractor extractor = new TwitterExtractor(new HttpClient());
            await extractor.ExtractTweetsFromTimelineAsync(
                new TimelineExecutionInfo
                {
                    TimelineUrl = timelineUrl
                },
                canExecute: () => tweets.Count <= 20,
                onTweetsExtracted: results => tweets.AddRange(results));

            foreach (var t in tweets)
            {
                log.WriteLine(t.ToString());
            }

            Assert.NotEmpty(tweets);
        }
    }
}
