using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mockbster.Data;
using Mockbster.Models;
using System.Diagnostics;

namespace Mockbster.Controllers
{
    public class HomeController : Controller
    {
        private List<MovieModel> movies = new List<MovieModel>();

        private readonly MockbsterContext _context;

        public HomeController(MockbsterContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(
            string username,
            string password,
            bool notUsed
            )
        {
            if ((username == null || password == null) &&
                HttpContext.Request.Cookies["loggedUser"] == null)
            {
                return View();
            }

            if (_context.User == null)
            {
                return Problem("Entity set 'MvcMovieContext.User'  is null.");
            }

            // Use LINQ to get list of genres.
            var users = from m in _context.User
                        select m;

            if (!string.IsNullOrEmpty(username))
            {
                users = users.Where(s => s.Username!.Contains(username));
            }

            if (!string.IsNullOrEmpty(password))
            {
                users = users.Where(x => x.Password.Contains(password));
            }

            if (users.Count() == 1 || HttpContext.Request.Cookies["loggedUser"] != null)
            {
                if (username == null)
                {
                    username = HttpContext.Request.Cookies["loggedUser"];
                }
                HttpContext.Response.Cookies.Append("loggedUser", username, 
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddHours(1),
                    });
                if (username == "Admin" || HttpContext.Request.Cookies["loggedUser"] == "Admin")
                    return Redirect("/Movies");
                return Redirect("/MoviesUser");
            } else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Firstname,Lastname,Email,Username,Password")] UserModel user)
        {
            if (ModelState.IsValid)
            {

                var users = from m in _context.User
                            select m;
                if (!string.IsNullOrEmpty(user.Username))
                {
                    users = users.Where(s => s.Username!.Contains(user.Username));
                }

                if (users.Count() > 0)
                    return View();



                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}