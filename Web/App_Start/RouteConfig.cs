using System.Web.Mvc;
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
                name: "Students",
                url: "Students/{action}",
                defaults: new { controller = "Students", action = "Index" }
            );

            routes.MapRoute(
                name: "Courses",
                url: "Courses/{action}",
                defaults: new { controller = "Courses", action = "Index" }
            );

            routes.MapRoute(
                name: "CoursesTaken",
                url: "CoursesTaken/{action}",
                defaults: new { controller = "CoursesTaken", action = "Add" }
            );

            routes.MapRoute(
                name: "CoursesTakenViews",
                url: "CoursesTaken/{action}/{id}",
                defaults: new { controller = "CoursesTaken", action = "Student" }
            );
        }
    }
}
