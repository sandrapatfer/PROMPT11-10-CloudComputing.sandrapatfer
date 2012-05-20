using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;

namespace MvcWebRole1.Controllers
{
    public class HomeController : Controller
    {
        private static CloudQueue _queue;

        static HomeController()
        {
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("QueueInterRole"));
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference("myqueue");
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC in the Cloud!!!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string text)
        {
            Trace.WriteLine("Received the message: " + text, "Information");
            Trace.TraceInformation("xxxx");
            HomeController._queue.AddMessage(new CloudQueueMessage(text));
            return RedirectToAction("Index");
        }
    }
}
