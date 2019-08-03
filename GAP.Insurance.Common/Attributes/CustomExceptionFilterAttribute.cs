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
            var exception = context.Exception as CustomException;

            if (exception == null)
            {
                //Unhandled exception
                var controllerName = context.RouteData.Values["controller"];
                var actionName = context.RouteData.Values["action"];

                string message = _localizer.GetMessage("ERROR_UnhandledException", actionName, controllerName);
                exception = new CustomException(message, context.Exception);
            }

            //Allows to write the error in the application log
            _logger.WriteError(exception.Message, exception);

            var apiError = new APIError();
            int statusCode = (int)HttpStatusCode.InternalServerError;
            apiError.StatusCode = statusCode;
            apiError.Error = exception.Message;
            apiError.Detail = (exception.InnerException != null) ? exception.InnerException.Message : string.Empty;

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult(apiError);
        }
    }
}
