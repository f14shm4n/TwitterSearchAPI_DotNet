using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    /// <summary>
    /// The tweets search engine event args.
    /// </summary>
    public class TwitterSearchEventArgs : EventArgs
    {
        /// <summary>
        /// Gets query which related with current request. 
        /// </summary>
        public string Query { get; private set; }
        /// <summary>
        /// Gets tweets which was parsed.
        /// </summary>
        public List<Tweet> Tweets { get; private set; }
        /// <summary>
        /// Create new event arg instance.
        /// </summary>
        /// <param name="query">The query which related with current request.</param>
        /// <param name="tweets">Parserd tweets.</param>
        public TwitterSearchEventArgs([NotNull]string query, List<Tweet> tweets)
        {
            Query = query;
            if (tweets == null)
            {
                Tweets = new List<Tweet>();
            }
            else
            {
                Tweets = new List<Tweet>(tweets);
            }
        }
    }
}
