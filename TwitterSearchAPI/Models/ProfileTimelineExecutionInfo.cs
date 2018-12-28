using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Represents a user profile timeline info.
    /// </summary>
    public class ProfileTimelineExecutionInfo : ExecutionInfo
    {
        /// <summary>
        /// User screen name which will be used to construct the url.
        /// </summary>
        public string UserScreenName { get; set; }
    }
}
