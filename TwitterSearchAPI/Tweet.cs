using System;

namespace TwitterSearchAPI
{
    public class Tweet
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserScreenName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Retweets { get; set; }
        public int Favourites { get; set; }

        public override string ToString() => $"Id: {Id}, UserId: {UserId}, UserName: {UserName}, UserScreenName: {UserScreenName}, Retweets: {Retweets}, Favorites: {Favourites}, CreatedAt: {CreatedAt}, Text: {Text}";
    }
}
