using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mockbster.Data;
using Mockbster.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Mockbster.Controllers
{
    public class MoviesUserController : Controller
    {
        private readonly MockbsterContext _context;

        public MoviesUserController(MockbsterContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(
            string? movieGenre,
            string? movieTitle,
            decimal? maxPrice = -1
            )
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(movieTitle))
            {
                movies = movies.Where(s => s.Title!.Contains(movieTitle));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre.Contains(movieGenre));
            }

            if (maxPrice != -1)
            {
                movies = movies.Where(x => x.Price <= maxPrice);
            }

            List<string> queryList = new List<string>(await genreQuery.ToListAsync());
            List<string> genres = new List<string> { };
            foreach (string line in queryList)
            {
                genres.AddRange(line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }

            var movieGenreVM = new MovieUserModel
            {
                
                Genres = new SelectList(genres.Distinct()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        public async Task<IActionResult> Rent(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            CartModel cartVM = new();
            cartVM.Movies = new();
            if (HttpContext.Session.GetString("carList") != null)
            {
                cartVM = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("carList")!
                    )!;
            }
            var new_item = new Tuple<int, int, MovieModel>((int)id, 1, movie);
            foreach (var item in cartVM!.Movies)
            {
                if (item.Item1 == new_item.Item1)
                {
                    return RedirectToAction( "Index" );
                }
            }
            cartVM!.Movies!.Add(new_item);
            HttpContext.Session.SetString("carList", JsonConvert.SerializeObject(cartVM));
            return RedirectToAction("Index");
        }

        public IActionResult Cart()
        {
            return RedirectToAction("Index", "Orders");
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
