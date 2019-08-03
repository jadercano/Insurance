using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.Common.Infrastructure
{
    /// <summary>
    /// Public interface that exposes helper methods for localization and globalization
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Allows to get a message from resource file
        /// </summary>
        /// <param name="key">Identifier to look for</param>
        /// <returns>The string localized</returns>
        string GetMessage(string key);
        /// <summary>
        /// Allows to get a message from resource file
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="args">Arguments to replace on the message</param>
        /// <returns>The string localized</returns>
        string GetMessage(string key, params object[] args);
    }
}
