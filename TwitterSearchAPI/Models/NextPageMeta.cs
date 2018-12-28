using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    public class NextPageMeta
    {
        public NextPageMeta(long minTweet, long maxTweet)
        {
            MinTweet = minTweet;
            MaxTweet = maxTweet;
        }

        public long MinTweet { get; set; }
        public long MaxTweet { get; set; }
    }
}
