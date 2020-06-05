using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalace.Helpers
{
    /* HtmlHelper that renders razor view to string*/
    public class Helper
    {
        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var stringWriter = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    stringWriter,
                    new HtmlHelperOptions()
                    );
                viewResult.View.RenderAsync(viewContext);
                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }

    /* an Attribute  that prevents access to action methods via URL or outside of this domain*/
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.GetTypedHeaders().Referer == null ||
     filterContext.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != filterContext.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
            {
                filterContext.HttpContext.Response.Redirect("/");
            }
        }
    }
}
