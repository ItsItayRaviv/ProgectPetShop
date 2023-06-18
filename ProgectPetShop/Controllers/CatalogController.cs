using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace ProgectPetShop.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index(int? CategoryID)
        {
            using (var db = new PetShopContext())
            {
                if (db.Animals is null || db.Categories is null)
                    return View("DataBaseError");
                ViewBag.Categories = db.Categories.ToList();
                var animals = db.Animals.ToList();
                if (CategoryID is null)
                    return View(animals);
                animals.Where(a => a.CategoryID == CategoryID);
                return View(animals);
            }
        }

        [HttpPost]
        public IActionResult CategChanged(int id)
        {
            ViewBag.PVTMoreInfo = true;
            using (var db = new PetShopContext())
            {
                var model = db.Animals!.Where(a => a.CategoryID == id).ToList();
                return PartialView("_TablePartial", model);
            }
        }

        public IActionResult Info(int ID)
        {
            using(var db = new PetShopContext())
            {
                if (db.Animals is null || db.Comments is null)
                    return View("DataBaseError");
                var animal = db.Animals
                               .Include(a => a.Comments)
                               .Where(a => a.ID == ID)
                               .First();
                
                return View(animal);
            }
        }

        [HttpPost]
        public IActionResult AddComment(string commentInput, int ID)
        {
            using(var db = new PetShopContext())
            {
                if (db.Animals is null || db.Animals.Find(ID) is null)
                    return View("DataBaseError");
                db.Animals
                    .Find(ID).Comments?
                    .Add(new Comment
                    { Text = commentInput
                    , Animal = db.Animals.Find(ID) });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
