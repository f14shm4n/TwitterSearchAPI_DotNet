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
            await searh.Search("Ticketfly Website Offline After Hack – Variety", 100);

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

        public override bool CanExecuteNext(List<Tweet> tweets)
        {
            if (tweets == null || tweets.Count == 0
                || _tweets.Count >= 20)
            {
                return false;
            }

            _tweets.AddRange(tweets);

            return true;
        }

        public List<Tweet> Tweets => _tweets;
        public event Action<List<Tweet>> Done;
    }
}
