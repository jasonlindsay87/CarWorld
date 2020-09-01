using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarWorld.Controllers
{
    public class ModelController : Controller
    {
       CarWorldEntities db = new CarWorldEntities();


        public ActionResult Index()
        {
           return View(db.Models.ToList());
        }


       [HttpGet]
       public ActionResult Create()
       {
           ViewBag.manufacturers = db.Manufacturers.ToList();
           return View();

        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Create([Bind(Include = "ManufacturerID, Name, EngineSize, Doors, Details")] Model model)
       {
           try
           {
               if (ModelState.IsValid)
               {
                   db.Models.Add(model);
                   db.SaveChanges();
                   return RedirectToAction("Index");
               }
           }
           catch (DataException /* dex */)
           {
               ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
           }
           ViewBag.manufacturers = db.Manufacturers.ToList();
           return View(model);
       }


       [HttpGet]
       public ActionResult Delete(int? id, bool? saveChangesError = false)
       {
           if (id == null)
           {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           }

           if (saveChangesError.GetValueOrDefault())
           {
               ViewBag.ErrorMessage =
                   "Delete failed. Try again, and if the problem persists see your system administrator.";
           }

           Model model = db.Models.Find(id);
           if (model == null)
           {
               return HttpNotFound();
           }

           return View(model);

       }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Delete(int id)
       {
           try
           {
               Model model = db.Models.Find(id);
               db.Models.Remove(model);
               db.SaveChanges();
           }
           catch (DataException/* dex */)
           {
               //Log the error (uncomment dex variable name and add a line here to write a log.
               return RedirectToAction("Delete", new { id = id, saveChangesError = true });
           }
           return RedirectToAction("Index");
       }

    }
}