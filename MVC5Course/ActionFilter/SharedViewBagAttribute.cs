using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    //可以用Action來作Logging
    class SharedViewBagAttribute : ActionFilterAttribute
    {

        public string MyProperty { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) //被過濾的對象
        {
            //filterContext.Controller.ViewBag.Message = "Your application description page.";
            filterContext.Controller.ViewBag.Message = MyProperty;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
