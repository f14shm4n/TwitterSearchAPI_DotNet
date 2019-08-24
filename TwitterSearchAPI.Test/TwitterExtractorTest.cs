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
        [InlineData("Telemundo Has &#8216;Game of Thrones&#8217; Hopes for &#8216;La Reina del Sur&#8217; Season 2 Premiere")]
        [InlineData("Tanya Tucker’s ‘While I’m Livin”")]
        public async Task SearchTweetsAsync(string title)
        {
            List<Tweet> tweets = new List<Tweet>();

            var extractor = new TweetExtractor(new HttpClient());            
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

            var extractor = new TweetExtractor(new HttpClient());
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

            var extractor = new TweetExtractor(new HttpClient());
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

        [Theory]
        [InlineData("https://twitter.com/cspan/lists/members-of-congress")]
        [InlineData("https://twitter.com/NYTMetro/lists/nyt-nyc-local-news")]
        public async Task ExtractTwitterListMembersAsync(string twitterListUrl)
        {
            List<UserProfile> profiles = new List<UserProfile>();

            var extractor = new UserExtractor(new HttpClient());
            await extractor.ExtractTwitterListMembersAsync(
                new TwitterListExecutionInfo
                {
                    TwitterListUrl = twitterListUrl
                },
                canExecute: () => profiles.Count <= 20,
                onUsersExtracted: results => profiles.AddRange(results));

            foreach (var t in profiles)
            {
                log.WriteLine(t.ToString());
            }

            Assert.NotEmpty(profiles);
        }

        [Theory]
        [InlineData("https://twitter.com/cspan/lists/members-of-congress")]
        [InlineData("https://twitter.com/NYTMetro/lists/nyt-nyc-local-news")]
        public async Task ExtractTwitterListSubscribersAsync(string twitterListUrl)
        {
            List<UserProfile> profiles = new List<UserProfile>();

            var extractor = new UserExtractor(new HttpClient());
            await extractor.ExtractTwitterListSubscribersAsync(
                new TwitterListExecutionInfo
                {
                    TwitterListUrl = twitterListUrl
                },
                canExecute: () => profiles.Count <= 20,
                onUsersExtracted: results => profiles.AddRange(results));

            foreach (var t in profiles)
            {
                log.WriteLine(t.ToString());
            }

            Assert.NotEmpty(profiles);
        }
    }
}
