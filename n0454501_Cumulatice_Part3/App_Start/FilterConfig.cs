using System.Web;
using System.Web.Mvc;

namespace n0454501_Cumulatice_Part3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
