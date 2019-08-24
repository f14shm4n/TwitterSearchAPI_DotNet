using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterSearchAPI.Helpers
{
    // TODO: Rework XPaths.
    internal class XPathHelper
    {
        public static HtmlNodeCollection GetJsStreamItemNodes(HtmlNode node) => node.SelectNodes("//li[contains(@class, 'js-stream-item')]");

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
