using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole1.Controllers
{
    public class HomeController : Controller
    {
        MyBlobStorage _myBlobStorage = new MyBlobStorage();

        public ActionResult Index()
        {
            //var blobContainer = _myBlobStorage.GetImagesContainer();

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index( HttpPostedFileBase fileBase)
        {
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            return View();
        }
    }
}
