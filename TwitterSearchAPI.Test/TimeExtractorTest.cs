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
    public class TimeExtractorTest
    {
        private ITestOutputHelper log;

        public TimeExtractorTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        [Theory]
        [InlineData("https://twitter.com/cspan/lists/members-of-congress")]
        public async Task ExtractTimelineFromUrl(string url)
        {
            List<Tweet> tweets = new List<Tweet>();

            TimelineExtractor searchEngine = new TimelineExtractor(new HttpClient(),
                () => tweets.Count <= 20,
                r => tweets.AddRange(r.Tweets));

            await searchEngine.ExtractAsync(url, 300);

            foreach (var t in tweets)
            {
                log.WriteLine($"Id: [{t.Id}] UserName: [{t.UserName}] UserScreenName: [{t.UserScreenName}] Text: [{t.Text}]");
            }

            Assert.NotEmpty(tweets);
        }
    }
}
