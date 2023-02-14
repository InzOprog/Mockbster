using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mockbster.Data;
using Mockbster.Models;

namespace Mockbster.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MockbsterContext _context;

        public MoviesController(MockbsterContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id, int? day)
        {
            if (id == null) { return NotFound(); }

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) { return NotFound(); }

            return View(movie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImgName,Title,ReleaseDate,Genre,Price,Rating")] MovieModel movie)
        {
            if (!ModelState.IsValid) return View(movie);
            
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null) { return NotFound(); }
            
            return View(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImgName,Title,ReleaseDate,Genre,Price,Rating")] MovieModel movie)
        {
            if (id != movie.Id) { return NotFound(); }

            if (!ModelState.IsValid) return View(movie);
            
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id)) { return NotFound(); }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) { return NotFound(); }

            return View(movie);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
