using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    public abstract class BaseController : Controller //如果加上Abstract關鍵字, 則該controller只能被繼承，不能被直接存取

    {
        protected ProductRepository repo = RepositoryHelper.GetProductRepository();

        [LocalOnly]
        public ActionResult Debug()
        {
            return Content("hello");
        }

        //找不到要回到特定頁面時
        /*
        protected override void HandleUnknownAction(string actionName)
        {
            base.HandleUnknownAction(actionName);
            //this.RedirectToAction("Index", "Home").ExecuteResult(this.ControllerContext);            
        }
         * */
    }
}
