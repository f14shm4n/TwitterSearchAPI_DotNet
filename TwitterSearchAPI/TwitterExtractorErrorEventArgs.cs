using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    public class TwitterExtractorErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Exception Error { get; set; }

        public TwitterExtractorErrorEventArgs(string message, Exception error)
        {
            Message = message;
            Error = error;
        }
    }
}
