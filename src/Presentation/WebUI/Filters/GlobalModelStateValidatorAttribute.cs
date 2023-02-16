using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebUI.Extentions.HttpRequestExtentions;

namespace WebUI.Filters;

public class GlobalModelStateValidatorAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            Controller controller = context.Controller as Controller;
            object model = context.ActionArguments.Any()
               ? context.ActionArguments.First().Value
               : null;

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else
            {
                context.Result = (IActionResult)controller?.View(model)
                   ?? new BadRequestResult();
            }
        }

        base.OnActionExecuting(context);
    }
}

