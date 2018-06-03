using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
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
    }
}
