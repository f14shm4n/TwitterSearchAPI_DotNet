using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterSearchAPI.Helpers;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI.Parsers
{
    public abstract class HtmlStringParser<R> : ITwitterItemParser<R> where R : ITwitterItem
    {
        public event EventHandler<TwitterItemParserErrorEventArgs> ParserError;

        public abstract R Parse(HtmlNode node);

        public virtual List<R> Parse(string html)
        {
            // Define html doc
            HtmlDocument htmlDocument;
            // Try to parse html string
            try
            {
                htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
            }
            catch (Exception ex)
            {
                ParserError?.Invoke(this, new TwitterItemParserErrorEventArgs("Failed to create HtmlDocument from raw html string.", ex));
                return null;
            }
            // Check nodes count
            var nodes = XPathHelper.GetJsStreamItemNodes(htmlDocument.DocumentNode);
            if (nodes == null || nodes.Count == 0)
            {
                return new List<R>();
            }
            // Start parsing nodes
            List<R> results = new List<R>();
            foreach (var node in nodes)
            {
                var r = Parse(node);
                if (r != null)
                {
                    results.Add(r);
                }
            }
            return results;
        }

        #region EventHelpers

        protected virtual void RaiseParserError(string msg, Exception ex) => ParserError?.Invoke(this, new TwitterItemParserErrorEventArgs(msg, ex));

        #endregion
    }
}
