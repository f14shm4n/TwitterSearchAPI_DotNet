using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI.Parsers
{
    // TODO: Rework XPaths.
    /// <summary>
    /// Provides methods for tweets parsing.
    /// </summary>
    internal class TweetParser
    {
        public static List<Tweet> ParseTweets(string html)
        {
            List<Tweet> tweets = new List<Tweet>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = GetTweetsNodes(doc.DocumentNode);
            if (nodes == null || nodes.Count == 0)
            {
                return tweets;
            }

            foreach (var node in nodes)
            {
                try
                {
                    string id = GetTweetId(node);
                    string text = GetTweetText(node);
                    string userId = GetUserId(node);
                    string userScreenName = GetUserScreenName(node);
                    string userName = GetUserName(node);
                    DateTime? createdAt = GetPublishDate(node);
                    int retweets = GetRetweetsCount(node);
                    int favourites = GetFavoritesCount(node);
                    int comments = GetCommentsCount(node);

                    Tweet tweet = new Tweet
                    {
                        Id = long.Parse(id),
                        Text = text,
                        UserId = userId,
                        UserScreenName = userScreenName,
                        UserName = userName,
                        CreatedAt = createdAt,
                        Retweets = retweets,
                        Favourites = favourites,
                        Comments = comments
                    };
                    tweets.Add(tweet);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return tweets;
        }

        private static HtmlNodeCollection GetTweetsNodes(HtmlNode docNode) => docNode.SelectNodes("//li[contains(@class, 'js-stream-item')]");

        public static string GetTweetId(HtmlNode n) => n.Attributes["data-item-id"]?.Value;

        public static string GetTweetText(HtmlNode n) => n.SelectSingleNode("./descendant::p[contains(@class, 'tweet-text')]")?.InnerText;

        public static string GetUrlFromTweet(HtmlNode n) => n.SelectSingleNode("./descendant::p[contains(@class, 'tweet-text')]")?.SelectNodes("./a")?.FirstOrDefault()?.Attributes["href"].Value;

        public static string GetUserId(HtmlNode n) => n.SelectSingleNode("./descendant::div[contains(@class, 'tweet')]")?.Attributes["data-user-id"]?.Value;

        public static string GetUserScreenName(HtmlNode n) => n.SelectSingleNode("./descendant::div[contains(@class, 'tweet')]")?.Attributes["data-screen-name"]?.Value;

        public static string GetUserName(HtmlNode n) => n.SelectSingleNode("./descendant::div[contains(@class, 'tweet')]")?.Attributes["data-name"]?.Value;

        public static DateTime? GetPublishDate(HtmlNode n)
        {
            string date = n.SelectSingleNode("./descendant::span[contains(@class, '_timestamp')]")?.Attributes["data-time-ms"]?.Value;
            if (!string.IsNullOrWhiteSpace(date))
            {
                return DateTimeHelper.FromDateTimeMs(date);
            }
            return null;
        }

        public static int GetRetweetsCount(HtmlNode n)
        {
            var raw = n.SelectSingleNode("./descendant::span[contains(@class, 'ProfileTweet-action--retweet')]/span[@class='ProfileTweet-actionCount']")?.Attributes["data-tweet-stat-count"]?.Value;
            if (int.TryParse(raw, out int count))
            {
                return count;
            }
            return 0;
        }

        public static int GetFavoritesCount(HtmlNode n)
        {
            var raw = n.SelectSingleNode("./descendant::span[contains(@class, 'ProfileTweet-action--favorite')]/span[@class='ProfileTweet-actionCount']")?.Attributes["data-tweet-stat-count"]?.Value;
            if (int.TryParse(raw, out int count))
            {
                return count;
            }
            return 0;
        }

        public static int GetCommentsCount(HtmlNode n)
        {
            var raw = n.SelectSingleNode("./descendant::span[contains(@class, 'ProfileTweet-action--reply')]/span[@class='ProfileTweet-actionCount']")?.Attributes["data-tweet-stat-count"]?.Value;
            if (int.TryParse(raw, out int count))
            {
                return count;
            }
            return 0;
        }
    }
}
