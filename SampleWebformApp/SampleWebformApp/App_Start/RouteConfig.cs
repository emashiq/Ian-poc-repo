using System.Web.Routing;

namespace SampleWebformApp
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Home", "home", "~/Default.aspx");
            routes.MapPageRoute("LoginByCode", "LoginByCode", "~/Account/LoginByCode.aspx");
            routes.MapPageRoute("Logout", "Logout", "~/Account/Logout.aspx");
        }
    }
}
