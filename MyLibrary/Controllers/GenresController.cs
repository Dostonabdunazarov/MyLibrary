﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class GenresController : Controller
    {
        private readonly ApplicationContext _context;

        public GenresController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Genres.ToListAsync());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genre = await _context.Genres.Include(c => c.Books).FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            ViewBag.Books = _context.Books.ToList();
            return View();
        }

        // POST: Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Books")] Genre genre, int[] selectedBooks)
        {
            if (ModelState.IsValid)
            {
                if(selectedBooks != null)
                {
                    foreach(var book in _context.Books.Where(s => selectedBooks.Contains(s.Id)))
                    {
                        genre.Books.Add(book);
                    }
                }
                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Books = _context.Books.ToList();
            var genre = await _context.Genres.Include(c=>c.Books).FirstOrDefaultAsync(s => s.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Books")] Genre genre, int[] selectedBooks)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }
            Genre newgenre = await _context.Genres.Include(c => c.Books).FirstOrDefaultAsync(x => x.Id == id);
            newgenre.Name = genre.Name;
            newgenre.Books.Clear();
            foreach (var item in _context.Books.Where(s => selectedBooks.Contains(s.Id))) 
            {
                newgenre.Books.Add(item);
            }
            _context.Entry(newgenre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
