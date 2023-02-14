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

        public MoviesUserController(MockbsterContext context) { _context = context; }
        public async Task<IActionResult> Index(
            string? movieGenre,
            string? movieTitle,
            decimal? maxPrice = -1
            )
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            
            var movies = from m in _context.Movie select m;

            if (!string.IsNullOrEmpty(movieTitle))
            {
                movies = movies.Where(s => s.Title!.Contains(movieTitle));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre!.Contains(movieGenre));
            }

            if (maxPrice != -1)
            {
                movies = movies.Where(x => x.Price <= maxPrice);
            }

            var queryList = new List<string>(await genreQuery.ToListAsync());
            var genres = new List<string>();
            foreach (var line in queryList)
            {
                genres.AddRange(line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }

            var movieGenreVm = new MovieUserModel
            {
                
                Genres = new SelectList(genres.Distinct()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVm);
        }

        public async Task<IActionResult> Rent(int? id)
        {
            if (id == null) { return NotFound(); }
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) { return NotFound(); }
            
            CartModel cartVm = new()
            {
                Movies = new List<Tuple<int, int, MovieModel>>()
            };
            
            if (HttpContext.Session.GetString("cartList") != null)
            {
                cartVm = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("cartList")!
                    )!;
            }
            var newItem = new Tuple<int, int, MovieModel>((int)id, 1, movie);
            if (cartVm!.Movies!.Any(item => item.Item1 == newItem.Item1))
            {
                return RedirectToAction( "Index" );
            }
            cartVm!.Movies!.Add(newItem);
            HttpContext.Session.SetString("cartList", JsonConvert.SerializeObject(cartVm));
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
