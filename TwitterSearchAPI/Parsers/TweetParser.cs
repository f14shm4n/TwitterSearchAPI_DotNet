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
    /// <summary>
    /// Provdes universal tweets parser for type which derives from <see cref="Tweet"/>.
    /// </summary>
    /// <typeparam name="R">Type of tweets.</typeparam>
    public class TweetParser<R> : HtmlStringParser<R> where R : Tweet, new()
    {
        public override R Parse(HtmlNode node)
        {
            try
            {
                string id = XPathHelper.GetTweetId(node);
                string text = XPathHelper.GetTweetText(node);
                string userId = XPathHelper.GetUserId(node);
                string userScreenName = XPathHelper.GetUserScreenName(node);
                string userName = XPathHelper.GetUserName(node);
                DateTime? createdAt = XPathHelper.GetPublishDate(node);
                int retweets = XPathHelper.GetRetweetsCount(node);
                int favourites = XPathHelper.GetFavoritesCount(node);
                int comments = XPathHelper.GetCommentsCount(node);

                R r = new R
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
                return r;
            }
            catch (Exception ex)
            {
                RaiseParserError($"Unable to parse a single {typeof(R).FullName} item from HtmlNode. This item will be skipped.", ex);
                return null;
            }
        }
    }
}
