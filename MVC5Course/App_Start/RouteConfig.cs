using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Course
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}"); // Web Form很多資料使用axd格式上傳，在Web Form和MVC的混合語法中會需要
            routes.IgnoreRoute("{foo}/{resource}.aspx/{*pathInfo}"); // 可以共用aspx

            routes.MapRoute(
                name: "Default",
                //url: "{controller}/{action}.php/{id}", //可將所有比對路徑都改成.php，然後再抓其中前段作為action
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ); // 如果路由比對成功，則會交給MVC Handler; 如果比對失敗，後續會交給IIS
        }
    }
}
