using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    /// <summary>
    /// This exception is thrown when the query string has an incorrect format.
    /// </summary>
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
