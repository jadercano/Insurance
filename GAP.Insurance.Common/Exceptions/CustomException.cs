using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.Common.Exceptions
{
    /// <summary>
    /// Custom exception for the application
    /// </summary>
    public class CustomException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class
        /// </summary>
        /// <param name="message">A message that describes the error</param>
        public CustomException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class
        /// </summary>
        /// <param name="message">A message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public CustomException(string message, Exception innerException) : base(message, innerException) { }
    }
}
