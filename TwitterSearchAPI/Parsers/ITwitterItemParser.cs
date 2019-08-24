using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI.Parsers
{
    public interface ITwitterItemParser<R>
    {
        event EventHandler<TwitterItemParserErrorEventArgs> ParserError;
        List<R> Parse(string html);
    }
}
