using System;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Tweet model.
    /// </summary>
    public class Tweet : ITwitterItem
    {
        /// <summary>
        /// Tweet id.
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// Tweet text.
        /// </summary>
        public virtual string Text { get; set; }
        /// <summary>
        /// Tweet user id.
        /// </summary>
        public virtual string UserId { get; set; }
        /// <summary>
        /// Tweet user name.
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// Tweet user screen name.
        /// </summary>
        public virtual string UserScreenName { get; set; }
        /// <summary>
        /// Tweet publish date.
        /// </summary>
        public virtual DateTime? CreatedAt { get; set; }
        /// <summary>
        /// Retweets count.
        /// </summary>
        public virtual int Retweets { get; set; }
        /// <summary>
        /// Favorites count.
        /// </summary>
        public virtual int Favourites { get; set; }
        /// <summary>
        /// Comments count.
        /// </summary>
        public virtual int Comments { get; set; }

        public override string ToString() => $"[{Id}; {UserId}; {UserName}; {UserScreenName}; {Retweets}; {Favourites}; {Comments}; {CreatedAt}; {Text}]";
    }
}
