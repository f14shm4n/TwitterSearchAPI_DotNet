using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI.Parsers
{
    internal class UserListParser
    {
        public static List<UserProfile> ParseProfiles(string html)
        {
            List<UserProfile> profiles = new List<UserProfile>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//li[contains(@class, 'js-stream-item')]/div");
            if (nodes == null || nodes.Count == 0)
            {
                return profiles;
            }

            foreach (var node in nodes)
            {
                try
                {
                    long id = long.Parse(node.GetAttributeValue("data-user-id", null));
                    string userName = node.GetAttributeValue("data-name", null);
                    string userScreenName = node.GetAttributeValue("data-screen-name", null);

                    var profile = new UserProfile
                    {
                        Id = id,
                        UserName = userName,
                        UserScreenName = userScreenName
                    };

                    profiles.Add(profile);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return profiles;
        }
    }
}
