using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Base execution info.
    /// </summary>
    public class ExecutionInfo
    {
        /// <summary>
        /// Cancellation token.
        /// </summary>
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
        /// <summary>
        /// Execution rate.
        /// </summary>
        public TimeSpan ExecutionRate { get; set; } = TimeSpan.FromMilliseconds(500);
        /// <summary>
        /// Custom generator for initial url.
        /// </summary>
        public Func<string, string> InitialUrlGenerator { get; set; }
        /// <summary>
        /// Custom generator for next page url.
        /// </summary>
        public Func<string, NextPageMeta, string> NextPageUrlGenerator { get; set; }
    }
}
