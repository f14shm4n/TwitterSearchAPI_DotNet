using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Represents a timeline execution info.
    /// </summary>
    public class TimelineExecutionInfo : ExecutionInfo
    {
        /// <summary>
        /// Timeline url which will be used to extract tweets.
        /// </summary>
        public string TimelineUrl { get; set; }
    }
}
