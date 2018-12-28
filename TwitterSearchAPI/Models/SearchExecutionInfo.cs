using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Represents a search query info.
    /// </summary>
    public class SearchExecutionInfo : ExecutionInfo
    {
        /// <summary>
        /// Search query.
        /// </summary>
        public string Query { get; set; }
    }
}
