﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    public class TwitterTimelineResponse
    {
        public bool has_more_items { get; set; }
        public string items_html { get; set; }
        public string min_position { get; set; }
        public string refresh_cursor { get; set; }
        public long focused_refresh_interval { get; set; }
    }
}
