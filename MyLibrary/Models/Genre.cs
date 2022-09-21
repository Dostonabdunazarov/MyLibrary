using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название!")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
        public Genre()
        {
            Books = new List<Book>();
        }
    }
}
