namespace ProgectPetShop
{
    public class ExampleDBSetup
    {
        public ExampleDBSetup()
        {
            DBReset();
            DBAddCategories();
            DBAddAnimals();
            DBAddComments();
        }
        public void DBReset()
        {
            using (var db = new PetShopContext())
            {
                db.Database.Delete();
                db.Database.CreateIfNotExists();
            }
        }

        public void DBAddCategories()
        {
            using (var db = new PetShopContext())
            {
                db.Categories!.Add(new Category { Name = "categ1" });
                db.Categories.Add(new Category { Name = "categ2" });
                db.Categories.Add(new Category { Name = "test1" });
                db.Categories.Add(new Category { Name = "test2" });
                db.SaveChanges();
            }
        }

        public void DBAddAnimals()
        {
            var extrainfo = "\nthis is an extremely pleasant animal which anyone would be incredibly fond of immediately" +
                "\ni sincerely recommend getting said animal for every home for it is one of my personal favorites.";
            using (var db = new PetShopContext())
            {
                var r = new Random();
                var categs = db.Categories!.ToList();
                var names = new string[] {  "Dog", "Cat", "Tiger", "Squirrel", "Parrot" };
                for (int i = 0; i < names.Length; i++)
                {
                    var categ = categs[r.Next(categs.Count)];
                    var newAnimal = new Animal()
                    {
                        Name = names[i],
                        Age = r.Next(1, 11),
                        Category = categ,
                        Description = $"Meet the {names[i]}. {extrainfo}",
                        PictureName = $"/{names[i]}.png",
                        Comments = null
                    };
                    db.Animals?.Add(newAnimal);
                }
                db.SaveChanges();
            }
        }
        public void DBAddComments()
        {
            using (var db = new PetShopContext())
            {
                var tiger = db.Animals?.Where(a => a.Name == "Tiger").FirstOrDefault();
                tiger?.Comments?.Add(new Comment() { Text = "This tigy is giddy", Animal = tiger });
                tiger?.Comments?.Add(new Comment() { Text = "This tigy is chad", Animal = tiger });
                tiger?.Comments?.Add(new Comment() { Text = "i wanna cuddle him so bad", Animal = tiger });
                var cat = db.Animals?.Where(a => a.Name == "Cat").FirstOrDefault();
                cat?.Comments?.Add(new Comment() { Text = "This kitty is giddy", Animal = cat });
                cat?.Comments?.Add(new Comment() { Text = "This cutie is a princess", Animal = cat });
                var dog = db.Animals?.Where(a => a.Name == "Dog").FirstOrDefault();
                dog?.Comments?.Add(new Comment() { Text = "he is so smelly", Animal = dog });
                db.SaveChanges();
            }
        }
    }
}
