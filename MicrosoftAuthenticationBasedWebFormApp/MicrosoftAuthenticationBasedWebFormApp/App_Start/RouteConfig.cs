using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace MicrosoftAuthenticationBasedWebFormApp
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            //add route for home and about page
            routes.MapPageRoute("Home", "", "~/Default.aspx");
            routes.MapPageRoute("About", "About", "~/About.aspx");

        }
    }
}
