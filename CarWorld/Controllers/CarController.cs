using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace CarWorld.Controllers
{
    public class CarController : Controller
    {
        CarWorldEntities db = new CarWorldEntities();


        public ActionResult Index(string sortOrder, string searchColour)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Year" ? "year_desc" : "Year";
            var cars = from c in db.Cars
                select c;

            if (!String.IsNullOrWhiteSpace(searchColour))
            {
                cars = cars.Where(c => c.Colour.Contains(searchColour));
                
            }

            switch (sortOrder)
            {
                case "name_desc":
                    cars = cars.OrderByDescending(c => c.ManufacturerID);
                    break;
                case "Year":
                    cars = cars.OrderBy(c => c.ManufacturerID);
                    break;
                case "year_desc":
                    cars = cars.OrderByDescending(c => c.Year);
                    break;
                default:
                    cars = cars.OrderBy(c => c.ManufacturerID);
                    break;
            }
            return View(db.Cars.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.manufacturers = db.Manufacturers.ToList();
            ViewBag.models = db.Models.ToList();
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManufacturerID, ModelID, Year, Colour, EngineSize, Mileage, Doors, Notes")] Car car, int? ManufacturerId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Cars.Add(car);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.manufacturers = db.Manufacturers.ToList();
            ViewBag.models = db.Models.ToList();
            return View(car);
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

            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            return View(car);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Car car = db.Cars.Find(id);
                db.Cars.Remove(car);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public static List<SelectListItem> YearDropdown()
        {
            List<SelectListItem> yearList = new List<SelectListItem>();

            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                yearList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return yearList;
        }


    }
}