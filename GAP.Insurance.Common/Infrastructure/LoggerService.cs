using GAP.Insurance.Common.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.Common.Infrastructure
{
    /// <summary>
    /// Service that uses Log4Net framework in order to levarage logging functionality
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private ILocalizationService _localizer;
        private ILog _logger;

        /// <summary>
        /// Initalize a new instance of the <see cref="LoggerService"/> class
        /// </summary>
        /// <param name="localizer">Service to use for localizing (Injected by the ASP.NET Core DI engine)</param>
        public LoggerService(ILocalizationService localizer)
        {
            _localizer = localizer;
            _logger = LogManager.GetLogger(typeof(LoggerService));
        }

        #region ILoggerService interface implementation
        public void WriteError(string message, Exception ex)
        {
            WriteLog(LogCategory.Error, message, false, ex);
        }

        public void WriteLog(LogCategory category, string message, params object[] args)
        {
            WriteLog(category, message, true, null, args);
        }

        public void WriteLog(LogCategory category, string message, bool translate, params object[] args)
        {
            WriteLog(category, message, translate, null, args);
        }

        public void WriteLog(LogCategory category, string message, bool translate, Exception ex, params object[] args)
        {
            string formattedMessage = (translate & _localizer != null) ? _localizer.GetMessage(message, args) : message;

            switch (category)
            {
                case LogCategory.Error:
                    if (_logger.IsErrorEnabled)
                        _logger.Error(formattedMessage, ex);

                    break;
                case LogCategory.Warning:
                    if (_logger.IsWarnEnabled)
                        _logger.Warn(formattedMessage);

                    break;
                case LogCategory.Information:
                    if (_logger.IsInfoEnabled)
                        _logger.Info(formattedMessage);

                    break;
                case LogCategory.Debug:
                    if (_logger.IsDebugEnabled)
                        _logger.Debug(formattedMessage);

                    break;
                case LogCategory.Fatal:
                    if (_logger.IsFatalEnabled)
                        _logger.Fatal(formattedMessage);

                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
