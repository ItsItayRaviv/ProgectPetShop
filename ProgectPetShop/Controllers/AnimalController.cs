using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace ProgectPetShop.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Index(int ID)
        {
            using (var db = new PetShopContext())
            {
                if (db.Animals is null || db.Comments is null)
                    return View("DataBaseError");
                var animal = db.Animals
                               .Include(a => a.Comments)
                               .FirstOrDefault(a => a.ID == ID);
                if (animal == null)
                    return View("DataBaseError", $"Animal Not Found for ID = {ID}");
                if (animal.Comments == null)
                    return View("DataBaseError", $"Comments Not Found for animal ID = {ID}");


                var animalId = animal.ID;

                var category = (from Animal in db.Animals
                             join catgry in db.Categories! on animal.CategoryID equals catgry.ID
                             where Animal.ID == animalId
                             select catgry.Name).FirstOrDefault();
                if (category == null)
                    category = "ERROR";
                ViewBag.Category = category;
                return View(animal);
            }
        }
    }
}
