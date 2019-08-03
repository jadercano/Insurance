using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.TO
{
    /// <summary>
    /// Data container that represents an API error
    /// </summary>
    public class APIError
    {
        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the status code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the error detail
        /// </summary>
        public string Detail { get; set; }
    }
}
