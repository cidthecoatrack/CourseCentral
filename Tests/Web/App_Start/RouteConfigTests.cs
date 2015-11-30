using CourseCentral.Web;
using NUnit.Framework;
using System;
using System.Web.Routing;

namespace CourseCentral.Tests.Web.App_Start
{
    [TestFixture]
    public class RouteConfigTests
    {
        private RouteCollection routes;

        [SetUp]
        public void Setup()
        {
            routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
        }

        [TestCase("Default", "{controller}/{action}", "Home", "Index")]
        [TestCase("CoursesTakenViews", "CoursesTaken/{action}/{id}", "CoursesTaken", "Student")]
        [TestCase("TreesSearch", "Trees/Search/{tree}/{query}", "Trees", "Search")]
        public void RouteIsMapped(String name, String url, String controller, String action)
        {
            Assert.That(routes[name], Is.InstanceOf<Route>());

            var route = routes[name] as Route;
            Assert.That(route.Url, Is.EqualTo(url));
            Assert.That(route.Defaults["Controller"], Is.EqualTo(controller));
            Assert.That(route.Defaults["Action"], Is.EqualTo(action));
        }
    }
}
