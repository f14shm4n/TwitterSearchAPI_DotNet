using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI.Parsers
{
    /// <summary>
    /// Provdes universal user profile parser for type which derives from <see cref="UserProfile"/>.
    /// </summary>
    /// <typeparam name="R">Type of user profile.</typeparam>
    public class UserParser<R> : HtmlStringParser<R> where R : UserProfile, new()
    {
        public override R Parse(HtmlNode node)
        {
            try
            {
                var accountNode = node.SelectSingleNode("./div");
                long id = long.Parse(accountNode.GetAttributeValue("data-user-id", null));
                string userName = accountNode.GetAttributeValue("data-name", null);
                string userScreenName = accountNode.GetAttributeValue("data-screen-name", null);

                R r = new R
                {
                    Id = id,
                    UserName = userName,
                    UserScreenName = userScreenName
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
