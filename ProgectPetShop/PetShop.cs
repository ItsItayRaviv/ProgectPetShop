using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProgectPetShop
{
        public class Animal
        {
            public int ID { get; set; }
            public int CategoryID { get; set; }
            public string? Name { get; set; }
            public int Age { get; set; }
            public string? PictureName { get; set; }
            public string? Description { get; set; }

            public virtual Category? Category { get; set; }
            public virtual ICollection<Comment>? Comments { get; set; }
        }


        public class Category
        {
            public int ID { get; set; }
            public string? Name { get; set; }

            public virtual ICollection<Animal>? Animals { get; set; }
        }

        public class Comment
        {
            public int ID { get; set; }
            public int AnimalID { get; set; }
            public string? Text { get; set; }

            public virtual Animal? Animal { get; set; }
        }
    



}
