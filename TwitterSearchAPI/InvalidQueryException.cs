using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    public class InvalidQueryException : Exception
    {
        public InvalidQueryException(string query) : this("Query string '" + query + "' is invalid", null)
        {

        }

        public InvalidQueryException()
        {
        }

        public InvalidQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
