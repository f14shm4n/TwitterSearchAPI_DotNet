using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    /// <summary>
    /// The twitter time line response model.
    /// </summary>
    public class TimelineResponse
    {
        [JsonProperty("has_more_items")]
        public bool HasMoreItems { get; set; }
        [JsonProperty("items_html")]
        public string ItemsHtml { get; set; }
        [JsonProperty("min_position")]
        public string MinPosition { get; set; }
        [JsonProperty("max_position")]
        public string MaxPosition { get; set; }
        [JsonProperty("refresh_cursor")]
        public string RefreshCursor { get; set; }
        [JsonProperty("focused_refresh_interval")]
        public long FocusedRefreshInterval { get; set; }
    }
}
