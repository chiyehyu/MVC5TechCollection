using System.Web;
using System.Web.Mvc;

namespace MVC5Course
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); //全站的ActionFilter都會套用[HandleError]
            //註冊在這裡的ActionFilter，是全站的Action都會套用
        }
    }
}
