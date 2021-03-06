﻿using System.Web.Mvc;
using System.Web.Routing;

namespace CourseCentral.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "CoursesTakenViews",
                url: "CoursesTaken/{action}/{id}",
                defaults: new { controller = "CoursesTaken", action = "Student" }
            );

            routes.MapRoute(
                name: "TreesSearch",
                url: "Trees/Search/{tree}/{query}",
                defaults: new { controller = "Trees", action = "Search" }
            );
        }
    }
}
