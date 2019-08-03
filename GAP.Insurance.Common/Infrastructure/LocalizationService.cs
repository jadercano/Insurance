using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;

namespace GAP.Insurance.Common.Infrastructure
{
    /// <summary>
    /// Utility class for handling programmatically a resource file
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager _resourceManager;

        /// <summary>
        /// Initializes a new instance of <see cref="LocalizationService"/> class
        /// </summary>
        /// <param name="resourceBaseName">root resource name</param>
        public LocalizationService(string resourceBaseName)
        {
            Assembly resourceAssembly = Assembly.GetCallingAssembly();
            _resourceManager = new ResourceManager(resourceBaseName, resourceAssembly);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LocalizationService"/> class
        /// </summary>
        /// <param name="resourceBaseName">root resource name</param>
        /// <param name="resourceAssembly">The current assembly to use for</param>
        public LocalizationService(string resourceBaseName, Assembly resourceAssembly)
        {
            _resourceManager = new ResourceManager(resourceBaseName, resourceAssembly);
        }

        /// <summary>
        /// Get message
        /// </summary>
        /// <param name="key">Message key</param>
        /// <returns>Message description</returns>
        public string GetMessage(string key)
        {
            string message = _resourceManager.GetString(key);

            if (string.IsNullOrEmpty(message))
                return key;

            return message;
        }
        /// <summary>
        /// Get formatted message
        /// </summary>
        /// <param name="key">Message key</param>
        /// <param name="args">Message parameters</param>
        /// <returns>Formatted message</returns>
        public string GetMessage(string key, params object[] args)
        {
            string message = GetMessage(key);

            if (message.ToLower().Equals(key.ToLower()))
            {
                return key;
            }

            return string.Format(message, args);
        }
    }
}
