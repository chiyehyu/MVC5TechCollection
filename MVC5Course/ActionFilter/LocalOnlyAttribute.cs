using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    //可以用Action來作Logging
    class LocalOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) //被過濾的對象
        {
            if(!filterContext.RequestContext.HttpContext.Request.IsLocal) // 判斷Request來源是否為本機電腦
            {
                filterContext.Result = new RedirectResult("/"); // 可以用來指定action回傳的結果，例如將結果改為JsonResult等
            }
            
            base.OnActionExecuting(filterContext);
        }

        
    }
}
