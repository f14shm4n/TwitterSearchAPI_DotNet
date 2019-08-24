using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Represents a twitter list execution info.
    /// </summary>
    public class TwitterListExecutionInfo : ExecutionInfo
    {
        /// <summary>
        /// Twitter list url.
        /// </summary>
        public string TwitterListUrl { get; set; }
    }
}
