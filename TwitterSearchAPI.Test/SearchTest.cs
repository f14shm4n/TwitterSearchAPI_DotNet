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

        public SearchTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        [Fact]
        public async Task Search()
        {
            TwitterSearchImpl searh = new TwitterSearchImpl();
            await searh.SearchAsync("Ticketfly Website Offline After Hack – Variety", 100);

            log.WriteLine("Tweets: {0}", searh.Tweets.Count);
            foreach (var t in searh.Tweets)
            {
                log.WriteLine($"Tweet:\n{t}");
            }
            Assert.NotEmpty(searh.Tweets);
        }
    }

    public class TwitterSearchImpl : TwitterSearch
    {
        private List<Tweet> _tweets = new List<Tweet>();

        protected override bool CanExecute() => _tweets.Count >= 20;

        protected override void OnTweetListReady(List<Tweet> tweets)
        {
            _tweets.AddRange(tweets);
        }

        public List<Tweet> Tweets => _tweets;
    }
}
