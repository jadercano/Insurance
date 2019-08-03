using GAP.Insurance.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.Common.Infrastructure
{
    /// <summary>
    /// Public interface that exposes helper methods for logging
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Allows to write an entry to the application log
        /// </summary>
        /// <param name="message">Error messsage to write</param>
        /// <param name="ex">Exception to register</param>
        void WriteError(string message, Exception ex);

        /// <summary>
        /// Allows to write an entry to the application log
        /// </summary>
        /// <param name="category">Log category to use</param>
        /// <param name="message">The key message to translate or the message</param>
        /// <param name="args">Parameters to replace in the message</param>
        void WriteLog(LogCategory category, string message, params object[] args);

        /// <summary>
        /// Allows to write an entry to the application log
        /// </summary>
        /// <param name="category">Log category to use</param>
        /// <param name="message">The key message to translate or the message</param>
        /// <param name="translate">Indicates if the message must be translated or not</param>
        /// <param name="args">Parameters to replace in the message</param>
        void WriteLog(LogCategory category, string message, bool translate, params object[] args);
    }
}
