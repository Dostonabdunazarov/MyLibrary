using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MyLibrary.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Book b1 = new() { Title = "книга1", DatePublish = DateTime.Now };
            //Book b2 = new() { Title = "книга2", DatePublish = DateTime.Now };
            //Genre g1 = new() { Name = "Жанр1" };
            //Genre g2 = new() { Name = "Жанр2" };
            //Author author1 = new() { FullName = "author1" };
            //Author author2 = new() { FullName = "author2" };
            //Author author3 = new() { FullName = "author3" };
            //Author author4 = new() { FullName = "author4" };
            //b1.Authors.Add(author1);
            //b1.Authors.Add(author3);
            //b2.Authors.Add(author2);
            //b2.Authors.Add(author4);
            //b1.Genre = g2;
            //b2.Genre = g1;
            //Books.Add(b1);
            //Books.Add(b2);
            //Genres.Add(g1);
            //Genres.Add(g2);
            //Authors.Add(author1);
            //Authors.Add(author2);
            //Authors.Add(author3);
            //Authors.Add(author4);
            //this.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(c => c.Authors)
                .WithMany(s => s.Books)
                .UsingEntity(j => j.ToTable("BooksAuthors"));
        }
    }
}
