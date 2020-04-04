using System.Web.Mvc;
using GtecIt.Filters;

namespace GtecIt
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
          //  filters.Add(new CompressFilter());
            filters.Add(new SessionExpireFilterAttribute());
        }
    }
}
