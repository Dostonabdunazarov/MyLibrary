using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationContext _context;

        public AuthorsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.Include(c => c.Books).FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Books = _context.Books.ToList();
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,FullName,Books")] Author author, int[] selectedBooks)
        {
            if (ModelState.IsValid)
            {
                if (selectedBooks != null)
                {
                    foreach (var book in _context.Books.Where(s => selectedBooks.Contains(s.Id)))
                    {
                        author.Books.Add(book);
                    }
                }
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Books = _context.Books.ToList();
            var author = await _context.Authors.Include(c => c.Books).FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Books")] Author author, int[] selectedBooks)
        {
            if (id != author.Id)
            {
                return NotFound();
            }
            Author newauthor = await _context.Authors.Include(c => c.Books).FirstOrDefaultAsync(x => x.Id == id);
            newauthor.FullName = author.FullName;
            newauthor.Books.Clear();

            if (selectedBooks != null)
            {
                foreach (var book in _context.Books.Where(s => selectedBooks.Contains(id)))
                {
                    newauthor.Books.Add(book);
                }
            }
            _context.Entry(newauthor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Authors/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
