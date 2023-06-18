using Microsoft.AspNetCore.Mvc;
using ProgectPetShop.Models;
using System.Data.Entity;

namespace ProgectPetShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (var db = new PetShopContext())
            {
                if (db.Animals is null || db.Comments is null)
                    return View("DataBaseError");
                var topTwoAnimals = db.Animals
                    .Include(a => a.Comments)
                    .OrderByDescending(a => a.Comments!.Count)
                    .Take(2)
                    .ToList();
                return View(topTwoAnimals);
            }
        }
    }
}
