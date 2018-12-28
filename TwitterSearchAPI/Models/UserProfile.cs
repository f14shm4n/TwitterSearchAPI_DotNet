using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Represents a Twitter user profile data.
    /// </summary>
    public class UserProfile : ITwitterItem
    {
        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// User name.
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// User screen name.
        /// </summary>
        public virtual string UserScreenName { get; set; }
        /// <summary>
        /// Generates user profile url.
        /// </summary>
        /// <returns>String as Twitter profile url.</returns>
        public virtual string GetProfileUrl() => string.Format(TwitterConstants.UserProfileUrlTemplate, UserScreenName);

        public override string ToString() => $"[{Id}; {UserName}; {UserScreenName}]";
    }
}
