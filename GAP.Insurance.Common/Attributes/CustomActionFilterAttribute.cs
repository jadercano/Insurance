using GAP.Insurance.Common.Helpers;
using GAP.Insurance.Common.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.Common.Attributes
{
    /// <summary>
    /// Custom attribute that allows to intercep a request in the API in order to execute 
    /// pre or post actions
    /// </summary>
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        private ILoggerService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomActionFilterAttribute"/> class
        /// </summary>
        /// <param name="logger">Service to use for logging (Injected by the ASP.NET Core DI engine)</param>
        public CustomActionFilterAttribute(ILoggerService logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///  Occurs before the action method is invoked.
        /// </summary>
        /// <param name="context">The action context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var startDateTime = DateTime.Now;
            _logger.WriteLog(LogCategory.Debug, "INFO_StartActionCall", actionName, controllerName, startDateTime.ToShortDateString(), startDateTime.ToShortTimeString());
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="context">The action executed context</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var endDateTime = DateTime.Now;
            _logger.WriteLog(LogCategory.Debug, "INFO_EndActionCall", actionName, controllerName, endDateTime.ToShortDateString(), endDateTime.ToShortTimeString());
        }
    }
}
