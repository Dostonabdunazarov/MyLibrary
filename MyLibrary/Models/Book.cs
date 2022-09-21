using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Не указано дата издания")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime DatePublish { get; set; }
        public Genre Genre { get; set; }
        public List<Author> Authors{ get; set; } = new List<Author>();
        public Book() { }
        public Book(string name, DateTime dt, Genre g, List<Author> a)
        {
            Title = name;
            DatePublish = dt;
            Genre = g;
            Authors = a;
        }
    }
}
