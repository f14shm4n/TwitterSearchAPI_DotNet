using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchAPI.Models;
using Xunit;
using Xunit.Abstractions;

namespace TwitterSearchAPI.Test
{
    public class TweetsFeedExtractorTest
    {
        private ITestOutputHelper log;

        public TweetsFeedExtractorTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        [Theory]
        [InlineData("Variety")]
        [InlineData("Recode")]
        public async Task SearchUrl(string userScreenName)
        {
            List<Tweet> tweets = new List<Tweet>();

            TweetsFeedExtractor searchEngine = new TweetsFeedExtractor(new HttpClient(),
                () => tweets.Count <= 20,
                r => tweets.AddRange(r.Tweets));

            await searchEngine.ExtractAsync(userScreenName, 100);

            foreach (var t in tweets)
            {
                log.WriteLine($"Id: [{t.Id}] UserName: [{t.UserName}] UserScreenName: [{t.UserScreenName}]");
            }

            Assert.NotEmpty(tweets);
        }
    }
}
