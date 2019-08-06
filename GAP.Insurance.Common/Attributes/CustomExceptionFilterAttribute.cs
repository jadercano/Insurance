using GAP.Insurance.Common.Exceptions;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.TO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GAP.Insurance.Common.Attributes
{
    /// <summary>
    /// Custom attribute that allows to intercept an exception in the API
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private ILocalizationService _localizer;
        private ILoggerService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExceptionFilterAttribute"/> type
        /// </summary>
        /// <param name="localizer">Service to use for localizing (Injected by the ASP.NET Core DI engine)</param>
        /// <param name="logger">Service to use for logging (Injected by the ASP.NET Core DI engine)</param>
        public CustomExceptionFilterAttribute(ILocalizationService localizer, ILoggerService logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

        /// <summary>
        /// Allows to handled the custom exception
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var apiError = new APIError();
            int statusCode = (int)HttpStatusCode.InternalServerError;
            CustomException exception = null;

            //Allows to write the error in the application log
            _logger.WriteError(context.Exception.Message, context.Exception);

            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            _logger.WriteLog(Helpers.LogCategory.Debug, "ERROR_UnhandledException", actionName, controllerName);

            if ((context.Exception as CustomException) == null)
            {
                exception = ProcessUnmanagedException(context);
            }

            if (context.Exception is ArgumentNullException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                exception = ProcessArgumentNullException(context);
            }

            apiError.StatusCode = statusCode;
            apiError.Error = exception.Message;
            apiError.Detail = (exception.InnerException != null) ? exception.InnerException.Message : string.Empty;

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult(apiError);
        }

        private CustomException ProcessUnmanagedException(ExceptionContext context)
        {
            string userMessage = _localizer.GetMessage("ERROR_UnhandledExceptionMessage");
            return new CustomException(userMessage, context.Exception);
        }

        private CustomException ProcessArgumentNullException(ExceptionContext context)
        {
            return new CustomException(context.Exception.Message, context.Exception);
        }
    }
}
