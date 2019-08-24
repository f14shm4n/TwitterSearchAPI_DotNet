using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterSearchAPI.Models;

namespace TwitterSearchAPI
{
    public interface ITwitterExtractor<T> where T : ITwitterItem
    {
        event EventHandler<TwitterExtractorErrorEventArgs> ExtractorError;
        Task ExtractAsync(string initialUrl, Func<NextPageMeta, string> nextPageUrlGenerator, Action<IEnumerable<T>> onItemsExtracted, Func<bool> canExecute, TimeSpan executionRate, CancellationToken token);
    }
}
