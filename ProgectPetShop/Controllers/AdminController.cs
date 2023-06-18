using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using ProgectPetShop.Models;

namespace ProgectPetShop.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            using (var db = new PetShopContext())
            {
                if (db.Animals is null || db.Categories is null)
                    return View("DataBaseError");
                ViewBag.Categories = db.Categories.ToList();
                var animals = db.Animals?.ToList();
                return View(animals);
            }
        }

        [HttpPost]
        public IActionResult CategChanged(int id)
        {
            using var db = new PetShopContext();
            var model = db.Animals!.Where(a => a.CategoryID == id).ToList();
            return PartialView("_TablePartial", model);
        }

        public ActionResult Delete(int ID)
        {
            using (var db = new PetShopContext())
            {
                if (db.Animals is null || db.Comments is null)
                    return View("DataBaseError");
                var animal = db.Animals.Find(ID);
                if (animal == null)
                    return View("DataBaseError", $"Animal Not Found for ID = {ID}");
                db.Animals.Remove(animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
