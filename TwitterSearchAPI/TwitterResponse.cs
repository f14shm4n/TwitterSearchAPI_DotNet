using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    public class TwitterResponse
    {
        public bool has_more_items { get; set; }
        public string items_html { get; set; }
        public string min_position { get; set; }
        public string refresh_cursor { get; set; }
        public long focused_refresh_interval { get; set; }

        public List<Tweet> GetTweets()
        {
            List<Tweet> tweets = new List<Tweet>();
            var doc = new HtmlDocument();
            doc.LoadHtml(items_html);

            var elements = TwitterParser.GetTweetsNodes(doc.DocumentNode);
            if (elements == null || elements.Count == 0)
            {
                return tweets;
            }

            foreach (var el in elements)
            {
                string id = TwitterParser.GetTweetId(el);
                string text = TwitterParser.GetTweetText(el);
                string userId = TwitterParser.GetUserId(el);
                string userScreenName = TwitterParser.GetUserScreenName(el);
                string userName = TwitterParser.GetUserName(el);
                DateTime? createdAt = TwitterParser.GetPublishDate(el);
                int retweets = TwitterParser.GetRetweetsCount(el);
                int favourites = TwitterParser.GetFavoritesCount(el);

                Tweet tweet = new Tweet
                {
                    Id = id,
                    Text = text,
                    UserId = userId,
                    UserScreenName = userScreenName,
                    UserName = userName,
                    CreatedAt = createdAt,
                    Retweets = retweets,
                    Favourites = favourites
                };

                if (tweet.Id != null)
                {
                    tweets.Add(tweet);
                }
            }
            return tweets;
        }
    }
}
