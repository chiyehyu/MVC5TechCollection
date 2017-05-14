using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class HomeController : BaseController
    {
        private FabricsEntities db = new FabricsEntities();

        public ActionResult Index()
        {
            //this.View("About").ExecuteResult(this.ControllerContext);//在這個時間點先執行特定View，例如發出動態內容的Email
            return View();
        }

        //自行定義ActionFilter
        [SharedViewBag(MyProperty = "Your application description page.")]

        //可自行定義只有本機才會進行的ActionFilter
        //[LocalOnly] 

        //可以客製特定Exception的Error Page
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error_ArgumentException")]

        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            //故意發生例外
            //throw new ArgumentException("Error not Handled!");

            return View();

            
        }

        [SharedViewBag(MyProperty = "Your application partial description page.")]
        public ActionResult PartialAbout()
        {
            //ViewBag.Message = "Your application description page.";

            if (Request.IsAjaxRequest())
            {
                return PartialView("About");
            }
            else
            {
                return View("About");
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SomeAction()
        {
            ViewBag.Message = "Your contact page.";

            return PartialView("_SuccessRedirect","/");
        }

        public ActionResult GetFile()
        {
            return File(Server.MapPath("~/Content/Discover.png"), "image/png","dl.png");//有指定第三個參數的時候，會要求瀏覽器另存新檔
        }

        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Json( db.Product.Where(p=>p.IsDel == false).Take(10) ,JsonRequestBehavior.AllowGet);
        }


        public ActionResult Test()
        {
            return View();
        }

        public ActionResult RazorTest()
        {
            int[] data = new int[] { 1,2,3,4,5 };
            return PartialView(data);
            
        }
    }
}