using System;

namespace TwitterSearchAPI
{
    /// <summary>
    /// Tweet model.
    /// </summary>
    public class Tweet
    {
        /// <summary>
        /// Tweet id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tweet text.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Tweet user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Tweet user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Tweet user screen name.
        /// </summary>
        public string UserScreenName { get; set; }
        /// <summary>
        /// Tweet publish date.
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// Retweets count.
        /// </summary>
        public int Retweets { get; set; }
        /// <summary>
        /// Favorites count.
        /// </summary>
        public int Favourites { get; set; }

        public override string ToString() => $"Id: {Id}, UserId: {UserId}, UserName: {UserName}, UserScreenName: {UserScreenName}, Retweets: {Retweets}, Favorites: {Favourites}, CreatedAt: {CreatedAt}, Text: {Text}";
    }
}
