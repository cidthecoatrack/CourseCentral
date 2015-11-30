using System;
using System.Web.Mvc;
using Tree;

namespace CourseCentral.Web.Controllers
{
    public class TreesController : Controller
    {
        private BinaryTreeParser binaryTreeParser;
        private BinaryTreeSearcher binaryTreeSearcher;

        public TreesController(BinaryTreeParser binaryTreeParser, BinaryTreeSearcher binaryTreeSearcher)
        {
            this.binaryTreeParser = binaryTreeParser;
            this.binaryTreeSearcher = binaryTreeSearcher;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Search(String tree, Int32 query)
        {
            var parsedTree = binaryTreeParser.Parse(tree);
            var levels = binaryTreeSearcher.FindLevelsOf(query, parsedTree);
            return Json(new { levels = levels }, JsonRequestBehavior.AllowGet);
        }
    }
}