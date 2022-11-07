using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationContext _context;

        public BooksController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = _context.Books.Include(s => s.Authors).FirstOrDefault(c => c.Id == id);
            ViewBag.Auths = _context.Books.Include(s => s.Genre).FirstOrDefault(c => c.Id == id);
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DatePublish,Picture,Genre,Authors")] Book book, int? selectedGenre, int[] selectedAuthors, IFormFile userfile) {
            if (ModelState.IsValid) {
                string filename = userfile.FileName;
                filename = Path.GetFileName(filename);
                string fileupload = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", filename);
                using var stream = new FileStream(fileupload, FileMode.Create);
                _ = userfile.CopyToAsync(stream);

                if (selectedAuthors != null) {
                    foreach (var c in _context.Authors.Where(s => selectedAuthors.Contains(s.Id))) {
                        book.Authors.Add(c);
                    }
                }
                if (selectedGenre != null) {
                    book.Genre = _context.Genres.FirstOrDefault(c => c.Id == selectedGenre);
                }
                book.FilePath = filename;
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }
        public IActionResult DownloadFile(int? id) {
            if (id == null) {
                return NotFound();
            }
            var book = _context.Books.FirstOrDefault(c => c.Id == id);
            var memory = DownloadSinghFile(book.FilePath, "wwwroot\\files");
            return File(memory.ToArray(), "application/pdf", book.FilePath);
        }
        private MemoryStream DownloadSinghFile(string filename, string uploadPath) {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path)) {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            var book = await _context.Books.Include(c => c.Authors).Include(x => x.Genre).FirstOrDefaultAsync(s => s.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DatePublish")] Book book, int[] selectedAuthors, int? selectedGenre)
        {
            Book newbook = await _context.Books.Include(c => c.Authors).Include(x => x.Genre).FirstOrDefaultAsync(s => s.Id == id);
            newbook.Title = book.Title;
            newbook.DatePublish = book.DatePublish;
            newbook.Authors.Clear();
            if(selectedAuthors != null)
            {
                foreach(var c in _context.Authors.Where(s => selectedAuthors.Contains(s.Id)))
                {
                    newbook.Authors.Add(c);
                }
            }
            if(selectedGenre != null)
            {
                newbook.Genre = _context.Genres.FirstOrDefault(c => c.Id == selectedGenre);
            }
            _context.Entry(newbook).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
