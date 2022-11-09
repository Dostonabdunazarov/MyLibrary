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

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {
            Database.EnsureCreated();
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
