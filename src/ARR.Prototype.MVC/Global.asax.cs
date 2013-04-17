using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ARR.Prototype.MVC.Notifications;
using Raven.Abstractions.Data;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace ARR.Prototype.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
                UseEmbeddedHttpServer = true
            };

            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(54433);
            documentStore.Configuration.Port = 54433;

            documentStore.Initialize();

            documentStore.Changes()
                .ForAllDocuments()
                .Subscribe(new ChangeObserver());

            Application["raven"] = documentStore;
        }
    }



    public class ChangeObserver : IObserver<DocumentChangeNotification>
    {
        public void OnCompleted()
        {
            new NotificationSender().Send();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DocumentChangeNotification value)
        {
            new NotificationSender().Send();
        }
    }
}