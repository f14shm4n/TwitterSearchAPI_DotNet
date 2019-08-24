using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Models
{
    public class NextPageMeta
    {
        public NextPageMeta(long minResult, long maxResult)
        {
            MinResult = minResult;
            MaxResult = maxResult;
        }

        public long MinResult { get; set; }
        public long MaxResult { get; set; }
    }
}
