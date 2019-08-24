using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Parsers
{
    public class TwitterItemParserErrorEventArgs : EventArgs
    {
        public TwitterItemParserErrorEventArgs(string message, Exception error)
        {
            Message = message;
            Error = error;
        }

        public string Message { get; set; }
        public Exception Error { get; set; }
    }
}
