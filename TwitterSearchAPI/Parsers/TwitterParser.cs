using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI.Parsers
{
    /// <summary>
    /// Provides methods for tweets parsing.
    /// </summary>
    internal class TwitterParser
    {
        public static HtmlNodeCollection GetTweetsNodes(HtmlNode docNode) => docNode.SelectNodes("//li[contains(@class, 'js-stream-item')]");

        public static string GetTweetId(HtmlNode el) => el.Attributes["data-item-id"]?.Value;

        public static string GetTweetText(HtmlNode el) => el.SelectSingleNode("./descendant::p[contains(@class, 'tweet-text')]")?.InnerText;

        public static string GetUserId(HtmlNode el) => el.SelectSingleNode("./descendant::div[contains(@class, 'tweet')]")?.Attributes["data-user-id"]?.Value;

        public static string GetUserScreenName(HtmlNode el) => el.SelectSingleNode("./descendant::div[contains(@class, 'tweet')]")?.Attributes["data-screen-name"]?.Value;

        public static string GetUserName(HtmlNode el) => el.SelectSingleNode("./descendant::div[contains(@class, 'tweet')]")?.Attributes["data-name"]?.Value;

        public static DateTime? GetPublishDate(HtmlNode el)
        {
            string date = el.SelectSingleNode("./descendant::span[contains(@class, '_timestamp')]")?.Attributes["data-time-ms"]?.Value;
            if (!string.IsNullOrWhiteSpace(date))
            {
                return DateTimeHelper.FromDateTimeMs(date);
            }
            return null;
        }

        public static int GetRetweetsCount(HtmlNode el)
        {
            var raw = el.SelectSingleNode("./descendant::span[contains(@class, 'ProfileTweet-action--retweet')]/span[@class='ProfileTweet-actionCount']")?.Attributes["data-tweet-stat-count"]?.Value;
            if (int.TryParse(raw, out int count))
            {
                return count;
            }
            return 0;
        }

        public static int GetFavoritesCount(HtmlNode el)
        {
            var raw = el.SelectSingleNode("./descendant::span[contains(@class, 'ProfileTweet-action--favorite')]/span[@class='ProfileTweet-actionCount']")?.Attributes["data-tweet-stat-count"]?.Value;
            if (int.TryParse(raw, out int count))
            {
                return count;
            }
            return 0;
        }

        public static List<Tweet> ParseTweets(string html)
        {
            List<Tweet> tweets = new List<Tweet>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var elements = GetTweetsNodes(doc.DocumentNode);
            if (elements == null || elements.Count == 0)
            {
                return tweets;
            }

            foreach (var el in elements)
            {
                string id = GetTweetId(el);
                string text = GetTweetText(el);
                string userId = GetUserId(el);
                string userScreenName = GetUserScreenName(el);
                string userName = GetUserName(el);
                DateTime? createdAt = GetPublishDate(el);
                int retweets = GetRetweetsCount(el);
                int favourites = GetFavoritesCount(el);

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
