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
    }
}
