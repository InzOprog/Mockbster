using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mockbster.Data;
using Mockbster.Models;
using System.Diagnostics;

namespace Mockbster.Controllers
{
    public class HomeController : Controller
    {
        private readonly MockbsterContext _context;
        public HomeController(MockbsterContext context) { _context = context; }
        public async Task<IActionResult> Index(
            string? username,
            string? password,
            bool notUsed
            )
        {
            var defaultResponse = new LoginModel
            {
                UserData = new UserModel(),
                ErrorMessage = ""
            };
            if ((username == null || password == null) &&
                HttpContext.Request.Cookies["loggedUser"] == null)
            {
                return View(defaultResponse);
            }
            // Use LINQ to get list of genres.
            var users = from m in _context.User select m;

            // SELECT * FROM User WHERE User.Username = username
            if (!string.IsNullOrEmpty(username))
                users = users.Where(s => s.Username!.Contains(username));

            // SELECT * FROM User WHERE User.Username = username AND User.Password = password
            if (!string.IsNullOrEmpty(password))
                users = users.Where(x => x.Password!.Contains(password));

            var cookieUsername = HttpContext.Request.Cookies["loggedUser"];
            
            // User.Username is unique. Expected result for passed both if(){} is one element if user present.
            defaultResponse.ErrorMessage = "Wrong username or Password";
            if (users.Count() != 1 && cookieUsername == null) return View(defaultResponse);
            // If username is null, write cookieUsername to username
           
            var uname = username;
            uname ??= cookieUsername;
            // Logged in for max 1 hour.
            HttpContext.Response.Cookies.Append("loggedUser", uname!, 
                new CookieOptions { Expires = DateTime.Now.AddHours(1) });

            // If user is admin, sent to admin page, else to movie rental page
            return Redirect(users.FirstOrDefault()!.IsAdmin ? "/Movies" : "/MoviesUser");

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
            if (!ModelState.IsValid) return View(user);
            var users = from m in _context.User
                select m;
            if (!string.IsNullOrEmpty(user.Username))
                users = users.Where(s => s.Username!.Contains(user.Username));

            if (users.Any())
                return View();
            
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}