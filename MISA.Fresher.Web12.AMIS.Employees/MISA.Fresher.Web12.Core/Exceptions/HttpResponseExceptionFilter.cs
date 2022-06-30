using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Exceptions
{
    /// <summary>
    /// @desc: Middleware Exception handler
    /// @author: VQPhong (15/06/2022)
    /// </summary>
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MISAValidateException misaEx)
            {
                var res = new
                {
                    customStatusCode = Core.Enum.CustomizeStatusCode.BadRequest,
                    responseData = new
                    {
                        // Method 1: Responding messages respectively
                        devMsg = misaEx.Message,
                        userMsg = misaEx.Message,

                        // Method 2: Responding all at once
                        //devMsg = misaEx.Data,
                        //userMsg = misaEx.Data,
                    },
                };

                context.Result = new ObjectResult(res);

                context.ExceptionHandled = true;
            }
            else if (context.Exception is Exception ex)
            {
                var res = new
                {
                    customStatusCode = Core.Enum.CustomizeStatusCode.NormalException,
                    responseData = new
                    {
                        devMsg = ex.Message,
                        userMsg = Core.Resources.ResourceVietnam.UserMsgServerError,
                    },
                };

                context.Result = new ObjectResult(res);

                context.ExceptionHandled = true;
            }
            else if (context.Exception is HttpResponseException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
