using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using PromptCloudNotes.Interfaces;
using StructureMap;
using PromptCloudNotes.Model;

namespace Server
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            GlobalConfiguration.Configuration.Routes.MapHttpRoute("Api Default Route",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            IoC.Configure();

            GenerateDummyData();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
           
            RegisterRoutes(RouteTable.Routes);
        }

        private void GenerateDummyData()
        {
            var user = new User();
            var um = ObjectFactory.GetInstance<IUserManager>();
            user = um.CreateUser(user);

            var list = new TaskList() { Name = "Dummy list", Description = "Dummy description" };
            var tlm = ObjectFactory.GetInstance<ITaskListManager>();
            list = tlm.CreateTaskList(user.Id, list);

            var note = new Note() { Name = "note name", Description = "note description" };
            var nm = ObjectFactory.GetInstance<INoteManager>();
            note = nm.CreateNote(user.Id, list.Id, note);
        }
    }
}