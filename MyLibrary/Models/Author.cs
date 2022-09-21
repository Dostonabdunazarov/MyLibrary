using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Не указано ФИО!")]
        public string FullName { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
