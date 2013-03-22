using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ARR.Prototype.MVC.Models;
using ARR.Prototype.MVC.Notifications;
using Raven.Client;
using Raven.Client.Embedded;

namespace ARR.Prototype.MVC.Controllers
{
    public class RequirementController : Controller
    {
        //
        // GET: /Requirement/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Requirement/Details/5

        public ActionResult Details(int id)
        {
            RequirementModel model;
            var store = (EmbeddableDocumentStore)HttpContext.Application["raven"];
            using (var session = store.OpenSession())
            {
                model = session.Load<RequirementModel>(string.Format("requirementmodels/{0}", id));
            }

            var mailer = new NotificationSender();
            mailer.Send();

            return View(model);
        }

        //
        // GET: /Requirement/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Requirement/Create

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(RequirementModel requirement)
        {
            try
            {
                /// Saving changes using the session API
                var store = (EmbeddableDocumentStore)HttpContext.Application["raven"];
                using (IDocumentSession session = store.OpenSession())
                {
                    // Operations against session

                    // Flush those changes
                    session.Store(requirement);
                    session.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }

        //
        // GET: /Requirement/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Requirement/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Requirement/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Requirement/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
