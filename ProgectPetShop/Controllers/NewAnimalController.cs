using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Web;

namespace ProgectPetShop.Controllers
{
    public class NewAnimalController : Controller
    {

        public IActionResult Index()
        {
            using (var db = new PetShopContext())
            {
                var ctgry = db.Categories;
                if (ctgry is null)
                    ViewBag.Categories = new List<Category>();
                else
                    ViewBag.Categories = db.Categories!.ToList();
            }
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(Animal animal, IFormFile Image)
        //{
        //    if (Image is null)
        //        return RedirectToAction("Index", "Home");

        //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Image.FileName);
        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        await Image.CopyToAsync(stream);
        //    }
        //    animal.PictureName = path;
        //    using (var db = new PetShopContext())
        //    {
        //        db.Animals?.Add(animal);
        //        await db.SaveChangesAsync();
        //    }
        //    return RedirectToAction("Index", "Admin");
        //}



        [HttpPost]
        public async Task<IActionResult> Create(Animal model, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    var uniqueFilename = $"{Guid.NewGuid()}-{Path.GetFileName(Photo.FileName)}";
                    string? baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.FullName;
                    var uploads = Path.Combine(baseDirectory, "wwwroot", "Images");
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueFilename), FileMode.Create))
                    {
                        await Photo.CopyToAsync(fileStream);
                    }
                    model.PictureName = $"/Images/{uniqueFilename}";
                }
                using (var db = new PetShopContext())
                {
                    db.Animals?.Add(model);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int ID)
        {
            using var db = new PetShopContext();
            if (db.Animals is null || db.Categories is null)
                return View("DataBaseError", "");
            var animal = db.Animals
                           .FirstOrDefault(a => a.ID == ID);
            if (animal == null)
                return View("DataBaseError", $"Animal Not Found for ID = {ID}");
            ViewBag.Categories = db.Categories!.ToList();
            ViewBag.PreFill = true;
            return View(nameof(Index), animal);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Animal model, IFormFile Photo)
        {
            using (var db = new PetShopContext())
            {
                var originalAnimal = db.Animals!.FirstOrDefault(a => a.ID == model.ID);

                if (originalAnimal == null)
                    return View("Error", "No animal found for the provided ID.");

                if (originalAnimal.Name != model.Name)
                    originalAnimal.Name = model.Name;

                if (originalAnimal.Age != model.Age)
                    originalAnimal.Age = model.Age;

                if (originalAnimal.Description != model.Description)
                    originalAnimal.Description = model.Description;

                if (originalAnimal.CategoryID != model.CategoryID)
                    originalAnimal.CategoryID = model.CategoryID;

                if (Photo != null)
                {
                    var uniqueFilename = $"{Guid.NewGuid()}-{Path.GetFileName(Photo.FileName)}";
                    string? baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.FullName;
                    var uploads = Path.Combine(baseDirectory, "wwwroot", "Images");
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueFilename), FileMode.Create))
                    {
                        await Photo.CopyToAsync(fileStream);
                    }
                    originalAnimal.PictureName = $"/Images/{uniqueFilename}";
                }
                await db.SaveChangesAsync();
            }
            TempData["SuccessMessage"] = "Animal updated successfully.";
            return RedirectToAction("Index", "Admin");
        }
    }
}
