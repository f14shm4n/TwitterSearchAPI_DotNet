using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// Represents a base for each Twitter item.
    /// </summary>
    public interface ITwitterItem
    {
        /// <summary>
        /// The item id.
        /// </summary>
        long Id { get; }
    }
}
