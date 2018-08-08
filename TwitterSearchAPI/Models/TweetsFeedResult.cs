using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    public class TweetsFeedResult
    {
        public List<Tweet> Tweets { get; private set; }

        public TweetsFeedResult(List<Tweet> tweets)
        {
            if (tweets == null)
            {
                Tweets = new List<Tweet>();
            }
            else
            {
                Tweets = new List<Tweet>(tweets);
            }
        }
    }
}
